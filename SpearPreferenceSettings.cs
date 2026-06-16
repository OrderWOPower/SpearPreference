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

		[SettingPropertyInteger("Non-Siege/Non-Naval Spear Preference", 0, 100, "0", Order = 0, RequireRestart = false, HintText = "Multiplier for spear preference in non-siege/non-naval battles. Default is 10.")]
		[SettingPropertyGroup("Multipliers", GroupOrder = 0)]
		public int NonSiegeSpearPreferenceMultiplier { get; set; } = 10;

		[SettingPropertyInteger("Siege/Naval Spear Preference", 0, 100, "0", Order = 0, RequireRestart = false, HintText = "Multiplier for spear preference in siege/naval battles. Default is 1.")]
		[SettingPropertyGroup("Multipliers", GroupOrder = 0)]
		public int SiegeSpearPreferenceMultiplier { get; set; } = 1;

		[SettingPropertyFloatingInteger("Maximum Distance to Switch to Sidearms", 0.0f, 10.0f, "0.0m", Order = 0, RequireRestart = false, HintText = "Maximum distance to nearby enemies for troops to switch to sidearms. Default is 2.0m.")]
		[SettingPropertyGroup("Limits", GroupOrder = 1)]
		public float MaxDistanceToSwitchToSidearms { get; set; } = 2.0f;

		[SettingPropertyBool("Override RBM Weapon Preference", Order = 0, RequireRestart = false, HintText = "Override the weapon preference model in RBM. Enabled by default.")]
		[SettingPropertyGroup("Realistic Battle Mod", GroupOrder = 2)]
		public bool ShouldOverrideRbmWeaponPreference { get; set; } = true;
	}
}
