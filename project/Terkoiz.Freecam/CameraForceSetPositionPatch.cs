using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;

namespace Terkoiz.Freecam
{
    public class CameraForceSetPositionPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(CameraClass), nameof(CameraClass.ForceSetPosition));
        }

        [PatchPrefix]
        public static bool PatchPrefix()
        {
            // When FreeCam is active, prevent EFT from snapping the camera back to the player's head
            if (FreecamController.IsFreeCamScriptActive)
            {
                return false;
            }

            return true;
        }
    }
}
