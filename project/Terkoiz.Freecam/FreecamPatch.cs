using System.Reflection;
using SPT.Reflection.Patching;
using Comfort.Common;
using EFT;
using HarmonyLib;

namespace Terkoiz.Freecam
{
    public class FreecamPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(GameWorld), nameof(GameWorld.OnGameStarted));
        }

        [PatchPostfix]
        public static void PatchPostFix()
        {
            var gameWorld = Singleton<GameWorld>.Instance;
            
            if (gameWorld == null)
                return;

            // Add FreecamController to GameWorld GameObject
            gameWorld.gameObject.AddComponent<FreecamController>();
        }
    }
}