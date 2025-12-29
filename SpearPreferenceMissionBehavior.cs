using TaleWorlds.MountAndBlade;

namespace SpearPreference
{
    public class SpearPreferenceMissionBehavior : MissionBehavior
    {
        public override MissionBehaviorType BehaviorType => MissionBehaviorType.Other;

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
