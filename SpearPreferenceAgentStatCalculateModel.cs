using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SpearPreference
{
    public class SpearPreferenceAgentStatCalculateModel : AgentStatCalculateModel
    {
        private readonly AgentStatCalculateModel _model;

        public SpearPreferenceAgentStatCalculateModel(AgentStatCalculateModel model) => _model = model;

        public override bool CanAgentRideMount(Agent agent, Agent targetMount) => _model.CanAgentRideMount(agent, targetMount);

        public override float GetBreatheHoldMaxDuration(Agent agent, float baseBreatheHoldMaxDuration) => _model.GetBreatheHoldMaxDuration(agent, baseBreatheHoldMaxDuration);

        public override float GetDetachmentCostMultiplierOfAgent(Agent agent, IDetachment detachment) => _model.GetDetachmentCostMultiplierOfAgent(agent, detachment);

        public override float GetDifficultyModifier() => _model.GetDifficultyModifier();

        public override float GetDismountResistance(Agent agent) => _model.GetDismountResistance(agent);

        public override float GetEffectiveArmorEncumbrance(Agent agent, Equipment equipment) => _model.GetEffectiveArmorEncumbrance(agent, equipment);

        public override float GetEffectiveMaxHealth(Agent agent) => _model.GetEffectiveMaxHealth(agent);

        public override int GetEffectiveSkill(Agent agent, SkillObject skill) => _model.GetEffectiveSkill(agent, skill);

        public override int GetEffectiveSkillForWeapon(Agent agent, WeaponComponentData weapon) => _model.GetEffectiveSkillForWeapon(agent, weapon);

        public override float GetEnvironmentSpeedFactor(Agent agent) => _model.GetEnvironmentSpeedFactor(agent);

        public override float GetEquipmentStealthBonus(Agent agent) => _model.GetEquipmentStealthBonus(agent);

        public override float GetInteractionDistance(Agent agent) => _model.GetInteractionDistance(agent);

        public override float GetKnockBackResistance(Agent agent) => _model.GetKnockBackResistance(agent);

        public override float GetKnockDownResistance(Agent agent, StrikeType strikeType = StrikeType.Invalid) => _model.GetKnockDownResistance(agent, strikeType);

        public override float GetMaxCameraZoom(Agent agent) => _model.GetMaxCameraZoom(agent);

        public override string GetMissionDebugInfoForAgent(Agent agent) => _model.GetMissionDebugInfoForAgent(agent);

        public override float GetSneakAttackMultiplier(Agent agent, WeaponComponentData weapon) => _model.GetSneakAttackMultiplier(agent, weapon);

        public override float GetWeaponDamageMultiplier(Agent agent, WeaponComponentData weapon) => _model.GetWeaponDamageMultiplier(agent, weapon);

        public override float GetWeaponInaccuracy(Agent agent, WeaponComponentData weapon, int weaponSkill) => _model.GetWeaponInaccuracy(agent, weapon, weaponSkill);

        public override bool HasHeavyArmor(Agent agent) => _model.HasHeavyArmor(agent);

        public override void InitializeAgentStats(Agent agent, Equipment spawnEquipment, AgentDrivenProperties agentDrivenProperties, AgentBuildData agentBuildData) => _model.InitializeAgentStats(agent, spawnEquipment, agentDrivenProperties, agentBuildData);

        public override void InitializeMissionEquipment(Agent agent) => _model.InitializeMissionEquipment(agent);

        public override void UpdateAgentStats(Agent agent, AgentDrivenProperties agentDrivenProperties)
        {
            _model.UpdateAgentStats(agent, agentDrivenProperties);

            if (agent.IsHuman)
            {
                MissionWeapon spear = MissionWeapon.Invalid;
                SpearPreferenceSettings settings = SpearPreferenceSettings.Instance;

                for (EquipmentIndex index = EquipmentIndex.WeaponItemBeginSlot; index < EquipmentIndex.ExtraWeaponSlot; index++)
                {
                    MissionWeapon weapon = agent.Equipment[index];

                    if (!weapon.IsEmpty && !weapon.HasAnyUsageWithWeaponClass(WeaponClass.Javelin) && weapon.CurrentUsageItem.IsPolearm && weapon.CurrentUsageItem.SwingDamageType == DamageTypes.Invalid)
                    {
                        spear = weapon;
                    }
                }

                if (settings.ShouldOverrideRbmWeaponPreference)
                {
                    // Reset the agent's spear and sidearm preference multipliers.
                    agentDrivenProperties.AiWeaponFavorMultiplierPolearm = 1;
                    agentDrivenProperties.AiWeaponFavorMultiplierMelee = 1;
                }

                // Execute only if the agent has a spear which is not also a javelin.
                if (!spear.IsEmpty)
                {
                    try
                    {
                        Mission mission = Mission.Current;
                        // Get the number of dismounted enemies who are closer than half the length of the agent's spear by default.
                        int nearbyDismountedEnemyCount = mission.GetNearbyEnemyAgents(agent.Position.AsVec2, spear.CurrentUsageItem.GetRealWeaponLength() * settings.MaxDistanceToSwitchToSidearms, agent.Team, new MBList<Agent>()).Count(a => !a.HasMount);
                        // Get the number of mounted enemies who are closer than 50m.
                        int nearbyMountedEnemyCount = mission.GetNearbyEnemyAgents(agent.Position.AsVec2, 50, agent.Team, new MBList<Agent>()).Count(a => a.HasMount);

                        // Set the agent's spear preference multiplier.
                        agentDrivenProperties.AiWeaponFavorMultiplierPolearm = !mission.IsSiegeBattle && !mission.IsNavalBattle ? settings.NonSiegeSpearPreferenceMultiplier : settings.SiegeSpearPreferenceMultiplier;

                        if (nearbyDismountedEnemyCount > nearbyMountedEnemyCount)
                        {
                            // Set the agent's sidearm preference multiplier if there are more dismounted enemies than mounted enemies nearby.
                            agentDrivenProperties.AiWeaponFavorMultiplierMelee = (nearbyDismountedEnemyCount - nearbyMountedEnemyCount) * 10;
                        }

                        // Ensure that the agent always prefers ranged weapons first.
                        agentDrivenProperties.AiWeaponFavorMultiplierRanged = MathF.Max(agentDrivenProperties.AiWeaponFavorMultiplierPolearm, agentDrivenProperties.AiWeaponFavorMultiplierMelee);
                    }
                    catch (Exception ex)
                    {
                        InformationManager.DisplayMessage(new InformationMessage(ex.ToString()));
                    }
                }
            }
        }
    }
}
