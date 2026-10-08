using HarmonyLib;
using System;
using TaleWorlds.MountAndBlade;

namespace SpearPreference
{
	public class SpearPreferenceMissionBehavior : MissionBehavior
	{
		private readonly Type _typeofAgentAi;

		public override MissionBehaviorType BehaviorType => MissionBehaviorType.Other;

		public SpearPreferenceMissionBehavior() => _typeofAgentAi = AccessTools.TypeByName("RBMAI.AgentAi");

		public override void OnMeleeHit(Agent attacker, Agent victim, bool isCanceled, AttackCollisionData collisionData)
		{
			if (_typeofAgentAi == null || SpearPreferenceSettings.Instance.ShouldOverrideRbmWeaponPreference)
			{
				if (attacker != null && attacker.IsHuman && attacker.HasSpearCached)
				{
					attacker.UpdateAgentStats();
				}

				if (victim != null && victim.IsHuman && victim.HasSpearCached)
				{
					victim.UpdateAgentStats();
				}
			}
		}
	}
}
