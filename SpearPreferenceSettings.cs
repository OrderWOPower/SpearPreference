using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;

namespace SpearPreference
{
	public class SpearPreferenceSettings : AttributeGlobalSettings<SpearPreferenceSettings>
	{
		public override string Id => "SpearPreference";

		public override string DisplayName => "Troops Prefer Spears";

		public override string FolderName => "SpearPreference";

		public override string FormatType => "json2";

		[SettingPropertyInteger("Field Battle Spear Preference", 0, 100, "0", Order = 0, RequireRestart = false, HintText = "Multiplier for spear preference in field battles, sally out battles and raids. Default is 10.")]
		[SettingPropertyGroup("Multipliers", GroupOrder = 0)]
		public int FieldBattleSpearPreferenceMultiplier { get; set; } = 10;

		[SettingPropertyInteger("Siege Battle Spear Preference", 0, 100, "0", Order = 1, RequireRestart = false, HintText = "Multiplier for spear preference in siege battles. Default is 1.")]
		[SettingPropertyGroup("Multipliers", GroupOrder = 0)]
		public int SiegeBattleSpearPreferenceMultiplier { get; set; } = 1;

		[SettingPropertyInteger("Naval Battle Spear Preference", 0, 100, "0", Order = 2, RequireRestart = false, HintText = "Multiplier for spear preference in naval battles. Default is 1.")]
		[SettingPropertyGroup("Multipliers", GroupOrder = 0)]
		public int NavalBattleSpearPreferenceMultiplier { get; set; } = 1;

		[SettingPropertyInteger("Hideout Battle Spear Preference", 0, 100, "0", Order = 3, RequireRestart = false, HintText = "Multiplier for spear preference in hideout battles. Default is 10.")]
		[SettingPropertyGroup("Multipliers", GroupOrder = 0)]
		public int HideoutBattleSpearPreferenceMultiplier { get; set; } = 10;

		[SettingPropertyInteger("Arena Battle Spear Preference", 0, 100, "0", Order = 4, RequireRestart = false, HintText = "Multiplier for spear preference in arena battles. Default is 10.")]
		[SettingPropertyGroup("Multipliers", GroupOrder = 0)]
		public int ArenaBattleSpearPreferenceMultiplier { get; set; } = 10;

		[SettingPropertyInteger("Other Battle Spear Preference", 0, 100, "0", Order = 5, RequireRestart = false, HintText = "Multiplier for spear preference in other battles. Default is 1.")]
		[SettingPropertyGroup("Multipliers", GroupOrder = 0)]
		public int OtherBattleSpearPreferenceMultiplier { get; set; } = 1;

		[SettingPropertyFloatingInteger("Maximum Distance to Switch to Sidearms", 0.0f, 10.0f, "0.0m", Order = 0, RequireRestart = false, HintText = "Maximum distance to nearby enemies for troops to switch to sidearms. Default is 2.0m.")]
		[SettingPropertyGroup("Limits", GroupOrder = 1)]
		public float MaxDistanceToSwitchToSidearms { get; set; } = 2.0f;

		[SettingPropertyBool("Override RBM Weapon Preference", Order = 0, RequireRestart = false, HintText = "Override the weapon preference model in RBM. Enabled by default.")]
		[SettingPropertyGroup("Realistic Battle Mod", GroupOrder = 2)]
		public bool ShouldOverrideRbmWeaponPreference { get; set; } = true;
	}
}
