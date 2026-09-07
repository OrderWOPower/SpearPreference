using System;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SpearPreference
{
	public class SpearPreferenceAgentStatCalculateModel : AgentStatCalculateModel
	{
		private readonly AgentStatCalculateModel _model;
		private readonly Type _typeofAgentAi;

		public SpearPreferenceAgentStatCalculateModel(AgentStatCalculateModel model)
		{
			_model = model;
			_typeofAgentAi = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).FirstOrDefault(type => type.FullName == "RBMAI.AgentAi");
		}

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
			SpearPreferenceSettings settings = SpearPreferenceSettings.Instance;

			_model.UpdateAgentStats(agent, agentDrivenProperties);

			if (agent.IsHuman && (_typeofAgentAi == null || settings.ShouldOverrideRbmWeaponPreference))
			{
				// Reset the agent's spear and sidearm preference multipliers.
				agentDrivenProperties.AiWeaponFavorMultiplierPolearm = 1;
				agentDrivenProperties.AiWeaponFavorMultiplierMelee = 1;

				// Exclude mounted agents.
				if (!agent.HasMount)
				{
					for (EquipmentIndex index = EquipmentIndex.WeaponItemBeginSlot; index < EquipmentIndex.ExtraWeaponSlot; index++)
					{
						MissionWeapon weapon = agent.Equipment[index];

						// Execute only if the agent has a spear which is not also a javelin.
						if (!weapon.IsEmpty && !weapon.HasAnyUsageWithWeaponClass(WeaponClass.Javelin) && weapon.CurrentUsageItem.IsPolearm && weapon.CurrentUsageItem.SwingDamageType == DamageTypes.Invalid)
						{
							try
							{
								Mission mission = Mission.Current;
								// Get the number of dismounted enemies who are closer than 2m by default.
								int nearbyDismountedEnemyCount = mission.GetNearbyEnemyAgents(agent.Position.AsVec2, settings.MaxDistanceToSwitchToSidearms, agent.Team, new MBList<Agent>()).Count(a => !a.HasMount);
								// Get the number of mounted enemies who are closer than 50m.
								int nearbyMountedEnemyCount = mission.GetNearbyEnemyAgents(agent.Position.AsVec2, 50, agent.Team, new MBList<Agent>()).Count(a => a.HasMount);

								// Set the agent's spear preference multiplier.
								if (mission.IsFieldBattle || mission.IsSallyOutBattle || mission.IsNavalRaidBattle)
								{
									agentDrivenProperties.AiWeaponFavorMultiplierPolearm = settings.FieldBattleSpearPreferenceMultiplier;
								}
								else if (mission.IsSiegeBattle)
								{
									agentDrivenProperties.AiWeaponFavorMultiplierPolearm = settings.SiegeBattleSpearPreferenceMultiplier;
								}
								else if (mission.IsNavalBattle)
								{
									agentDrivenProperties.AiWeaponFavorMultiplierPolearm = settings.NavalBattleSpearPreferenceMultiplier;
								}
								else if (MapEvent.PlayerMapEvent != null && MapEvent.PlayerMapEvent.IsHideoutBattle)
								{
									agentDrivenProperties.AiWeaponFavorMultiplierPolearm = settings.HideoutBattleSpearPreferenceMultiplier;
								}
								else if (CampaignMission.Current?.Location?.StringId == "arena")
								{
									agentDrivenProperties.AiWeaponFavorMultiplierPolearm = settings.ArenaBattleSpearPreferenceMultiplier;
								}
								else
								{
									agentDrivenProperties.AiWeaponFavorMultiplierPolearm = settings.OtherBattleSpearPreferenceMultiplier;
								}

								// Execute only if the agent is wielding a polearm.
								if (nearbyDismountedEnemyCount > nearbyMountedEnemyCount && !agent.WieldedWeapon.IsEmpty && agent.WieldedWeapon.CurrentUsageItem.IsPolearm)
								{
									// Set the agent's sidearm preference multiplier if there are more dismounted enemies than mounted enemies nearby.
									agentDrivenProperties.AiWeaponFavorMultiplierMelee = (nearbyDismountedEnemyCount - nearbyMountedEnemyCount) * 20;
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
	}
}
