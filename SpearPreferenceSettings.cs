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

        [SettingPropertyInteger("Non-Siege/Naval Spear Preference", 0, 100, "0", Order = 0, RequireRestart = false, HintText = "Multiplier for spear preference in non-siege/naval battles. Default is 10.")]
        [SettingPropertyGroup("Multipliers", GroupOrder = 0)]
        public int NonSiegeSpearPreferenceMultiplier { get; set; } = 10;

        [SettingPropertyInteger("Siege/Naval Spear Preference", 0, 100, "0", Order = 0, RequireRestart = false, HintText = "Multiplier for spear preference in siege/naval battles. Default is 1.")]
        [SettingPropertyGroup("Multipliers", GroupOrder = 0)]
        public int SiegeSpearPreferenceMultiplier { get; set; } = 1;
    }
}
