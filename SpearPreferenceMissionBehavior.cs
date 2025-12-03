using TaleWorlds.MountAndBlade;

namespace SpearPreference
{
    public class SpearPreferenceMissionBehavior : MissionBehavior
    {
        public override MissionBehaviorType BehaviorType => MissionBehaviorType.Other;

        public override void OnMeleeHit(Agent attacker, Agent victim, bool isCanceled, AttackCollisionData collisionData)
        {
            int affectorWeaponSlotOrMissileIndex = collisionData.AffectorWeaponSlotOrMissileIndex;
            MissionWeapon weapon = affectorWeaponSlotOrMissileIndex >= 0 ? attacker.Equipment[affectorWeaponSlotOrMissileIndex] : MissionWeapon.Invalid;

            // Make the attacker switch to their sidearm if the victim is closer than half the length of the attacker's spear.
            if (!weapon.IsEmpty && ((weapon.CurrentUsageItem.IsPolearm && victim != null && attacker.GetDistanceTo(victim) <= weapon.CurrentUsageItem.GetRealWeaponLength() / 2) || !weapon.CurrentUsageItem.IsPolearm) && attacker.Equipment.ContainsSpear())
            {
                attacker.UpdateAgentStats();
            }
        }
    }
}
