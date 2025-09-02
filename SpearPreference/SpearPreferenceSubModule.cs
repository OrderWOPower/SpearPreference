using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SpearPreference
{
    // This mod fixes the thrust weapon preference setting in RBM to only affect thrusting, non-throwing polearms.
    public class SpearPreferenceSubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad() => new Harmony("mod.bannerlord.spearpreference").PatchAll();

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            // Set the thrust weapon preference setting in RBM to the default.
            AccessTools.Field(AccessTools.TypeByName("RBMConfig.RBMConfig"), "ThrustMagnitudeModifier").SetValue(null, 0.05f);
            AccessTools.Field(AccessTools.TypeByName("RBMConfig.RBMConfig"), "OneHandedThrustDamageBonus").SetValue(null, 20);
            AccessTools.Field(AccessTools.TypeByName("RBMConfig.RBMConfig"), "TwoHandedThrustDamageBonus").SetValue(null, 20);
        }
    }
}
