using System;
using System.Linq;
using System.Reflection;
using TaleWorlds.MountAndBlade;

namespace SpearPreference
{
    public class SpearPreferenceMissionBehavior : MissionBehavior
    {
        public override MissionBehaviorType BehaviorType => MissionBehaviorType.Other;

        public override void AfterStart()
        {
            if (SpearPreferenceSettings.Instance.ShouldOverrideRbmWeaponPreference)
            {
                AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).FirstOrDefault(type => type.FullName == "RBMAI.PostureLogic")?.GetField("timeToUpdateAgents", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, float.PositiveInfinity);
            }
        }

        public override void OnMeleeHit(Agent attacker, Agent victim, bool isCanceled, AttackCollisionData collisionData)
        {
            if (attacker != null && attacker.IsHuman && attacker.Equipment.ContainsSpear())
            {
                attacker.UpdateAgentStats();
            }

            if (victim != null && victim.IsHuman && victim.Equipment.ContainsSpear())
            {
                victim.UpdateAgentStats();
            }
        }
    }
}
