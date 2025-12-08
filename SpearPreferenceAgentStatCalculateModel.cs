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
                MissionWeapon weapon = agent.WieldedWeapon;

                if (!weapon.IsEmpty)
                {
                    Mission mission = Mission.Current;

                    if (!mission.IsSiegeBattle && !mission.IsNavalBattle)
                    {
                        // Get the number of dismounted enemies who are closer than half the length of the agent's spear.
                        int nearbyDismountedEnemyCount = mission.GetNearbyEnemyAgents(agent.Position.AsVec2, weapon.CurrentUsageItem.GetRealWeaponLength() / 2, agent.Team, new MBList<Agent>()).Count(a => !a.HasMount);
                        // Get the number of mounted enemies who are closer than 50m.
                        int nearbyMountedEnemyCount = mission.GetNearbyEnemyAgents(agent.Position.AsVec2, 50, agent.Team, new MBList<Agent>()).Count(a => a.HasMount);

                        if (!weapon.CurrentUsageItem.IsPolearm || nearbyDismountedEnemyCount <= nearbyMountedEnemyCount)
                        {
                            // Set the agent's spear preference multiplier.
                            agentDrivenProperties.AiWeaponFavorMultiplierPolearm = SpearPreferenceSettings.Instance.NonSiegeSpearPreferenceMultiplier;
                        }
                        else if (weapon.CurrentUsageItem.IsPolearm && nearbyDismountedEnemyCount > nearbyMountedEnemyCount)
                        {
                            // Decrease the agent's spear preference multiplier if there are more dismounted enemies than mounted enemies nearby.
                            agentDrivenProperties.AiWeaponFavorMultiplierPolearm -= (nearbyDismountedEnemyCount - nearbyMountedEnemyCount) * 5;
                            agentDrivenProperties.AiWeaponFavorMultiplierPolearm = MathF.Max(agentDrivenProperties.AiWeaponFavorMultiplierPolearm, 0f);
                        }
                    }
                    else
                    {
                        agentDrivenProperties.AiWeaponFavorMultiplierPolearm = SpearPreferenceSettings.Instance.SiegeSpearPreferenceMultiplier;
                    }
                }

                if (agentDrivenProperties.AiWeaponFavorMultiplierPolearm > 1)
                {
                    // Ensure that the agent always prefers ranged weapons first over spears.
                    agentDrivenProperties.AiWeaponFavorMultiplierRanged = agentDrivenProperties.AiWeaponFavorMultiplierPolearm + 1;
                }
            }
        }
    }
}
