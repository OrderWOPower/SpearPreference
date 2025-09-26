using HarmonyLib;
using SandBox.GameComponents;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.MountAndBlade;

namespace SpearPreference
{
    [HarmonyPatch]
    public class SpearPreferenceAgentStatCalculateModel
    {
        private static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(CustomBattleAgentStatCalculateModel), "UpdateHumanStats");
            yield return AccessTools.Method(typeof(SandboxAgentStatCalculateModel), "UpdateHumanStats");
        }

        [HarmonyAfter(new string[] { "com.rbmcombat" })]
        private static void Postfix(AgentDrivenProperties agentDrivenProperties) => agentDrivenProperties.AiWeaponFavorMultiplierPolearm = SpearPreferenceSettings.Instance.SpearPreferenceMultiplier;
    }
}
