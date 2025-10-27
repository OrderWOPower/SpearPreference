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

        public override float GetDifficultyModifier() => _model.GetDifficultyModifier();

        public override float GetDismountResistance(Agent agent) => _model.GetDismountResistance(agent);

        public override float GetEquipmentStealthBonus(Agent agent) => _model.GetEquipmentStealthBonus(agent);

        public override float GetKnockBackResistance(Agent agent) => _model.GetKnockBackResistance(agent);

        public override float GetKnockDownResistance(Agent agent, StrikeType strikeType = StrikeType.Invalid) => _model.GetKnockDownResistance(agent, strikeType);

        public override float GetSneakAttackMultiplier(Agent agent, WeaponComponentData weapon) => _model.GetSneakAttackMultiplier(agent, weapon);

        public override float GetWeaponDamageMultiplier(Agent agent, WeaponComponentData weapon) => _model.GetWeaponDamageMultiplier(agent, weapon);

        public override void InitializeAgentStats(Agent agent, Equipment spawnEquipment, AgentDrivenProperties agentDrivenProperties, AgentBuildData agentBuildData) => _model.InitializeAgentStats(agent, spawnEquipment, agentDrivenProperties, agentBuildData);

        public override void UpdateAgentStats(Agent agent, AgentDrivenProperties agentDrivenProperties)
        {
            _model.UpdateAgentStats(agent, agentDrivenProperties);

            // Exclude agents with javelins.
            if (agent.IsHuman && !agent.Equipment.HasRangedWeapon(WeaponClass.Javelin))
            {
                MissionWeapon weapon = agent.WieldedWeapon;
                // Get the number of enemies who are closer than half the length of the agent's spear.
                int nearbyEnemyCount = !weapon.IsEmpty && weapon.CurrentUsageItem.IsPolearm ? Mission.Current.GetNearbyEnemyAgentCount(agent.Team, agent.Position.AsVec2, weapon.CurrentUsageItem.GetRealWeaponLength() / 2) : 0;

                if (nearbyEnemyCount == 0)
                {
                    // Set the agent's spear preference multiplier if there are no nearby enemies.
                    agentDrivenProperties.AiWeaponFavorMultiplierPolearm = !Mission.Current.IsSiegeBattle ? SpearPreferenceSettings.Instance.NonSiegeSpearPreferenceMultiplier : SpearPreferenceSettings.Instance.SiegeSpearPreferenceMultiplier;
                }
                else
                {
                    // Decrease the agent's spear preference multiplier if there are nearby enemies.
                    agentDrivenProperties.AiWeaponFavorMultiplierPolearm -= nearbyEnemyCount * 4;
                    agentDrivenProperties.AiWeaponFavorMultiplierPolearm = MathF.Max(agentDrivenProperties.AiWeaponFavorMultiplierPolearm, 0f);
                }
            }
        }
    }
}
