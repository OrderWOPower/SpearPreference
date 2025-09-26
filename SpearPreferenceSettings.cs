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

        [SettingPropertyInteger("Spear Preference", 1, 100, "0", Order = 0, RequireRestart = false, HintText = "Multiplier for spear preference. Default is 10.")]
        [SettingPropertyGroup("Multipliers", GroupOrder = 0)]
        public float SpearPreferenceMultiplier { get; set; } = 10;
    }
}
