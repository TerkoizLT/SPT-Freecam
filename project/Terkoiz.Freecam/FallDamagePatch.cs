﻿using System.Reflection;
using EFT;
using SPT.Reflection.Patching;
using EFT.HealthSystem;
using HarmonyLib;

namespace Terkoiz.Freecam
{
    public class FallDamagePatch : ModulePatch
    {
        internal static bool HasTeleported;
        
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(ActiveHealthController), nameof(ActiveHealthController.HandleFall));
        }

        [PatchPrefix]
        public static bool PatchPrefix(ActiveHealthController __instance, float height, Player ___Player)
        {
            // WARNING: The 'HandleFall' method gets called every frame for every player and AI in a raid. Be very careful with logging or expensive operations in this prefix patch!

            // Check if it's our own player
            if (___Player.IsAI)
            {
                return true; // Run original method
            }

            // Global fall damage flag overrides everything
            if (FreecamPlugin.GlobalDisableFallDamage.Value)
            {
                return false; // Prevent original method from running
            }

            // If smart fall damage flag is enabled, check if we've recently teleported and if the fall height value was positive
            if (FreecamPlugin.SmartDisableFallDamage.Value && HasTeleported && height > 0)
            {
                HasTeleported = false;
                return false; // Prevent original method from running
            }

            return true; // Run original method
        }
    }
}