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

            // Make the attacker switch to their sidearm if they keep dealing 0 damage with their spear.
            if (!weapon.IsEmpty && (weapon.CurrentUsageItem.IsPolearm && collisionData.InflictedDamage == 0 || !weapon.CurrentUsageItem.IsPolearm) && attacker.Equipment.ContainsSpear() && !attacker.HasMount)
            {
                attacker.UpdateAgentStats();
            }
        }
    }
}
