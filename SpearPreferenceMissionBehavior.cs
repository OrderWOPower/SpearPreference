using System;
using System.Linq;
using System.Reflection;
using TaleWorlds.MountAndBlade;

namespace SpearPreference
{
	public class SpearPreferenceMissionBehavior : MissionBehavior
	{
		private readonly Type _typeofStanceLogic;

		public override MissionBehaviorType BehaviorType => MissionBehaviorType.Other;

		public SpearPreferenceMissionBehavior()
		{
			_typeofStanceLogic = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).FirstOrDefault(type => type.FullName == "RBMAI.StanceLogic");

			if (_typeofStanceLogic != null && SpearPreferenceSettings.Instance.ShouldOverrideRbmWeaponPreference)
			{
				// Override the interval for RBM's UpdateAgentStats call to an infinite amount of time.
				_typeofStanceLogic.GetField("timeToUpdateAgents", BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, float.PositiveInfinity);
			}
		}

		public override void OnMeleeHit(Agent attacker, Agent victim, bool isCanceled, AttackCollisionData collisionData)
		{
			if (_typeofStanceLogic == null || SpearPreferenceSettings.Instance.ShouldOverrideRbmWeaponPreference)
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
