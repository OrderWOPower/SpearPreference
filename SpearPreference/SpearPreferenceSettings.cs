using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;

namespace SpearPreference
{
    public class SpearPreferenceSettings : AttributeGlobalSettings<SpearPreferenceSettings>
    {
        public override string Id => "SpearPreference";

        public override string DisplayName => "RBM Spear Preference Fix";

        public override string FolderName => "SpearPreference";

        public override string FormatType => "json2";

        [SettingPropertyInteger("Spear Preference", 1, 100, "0", Order = 0, RequireRestart = false, HintText = "Multiplier for spear preference. Default is 3.")]
        [SettingPropertyGroup("Multipliers", GroupOrder = 0)]
        public int SpearPreferenceMultiplier { get; set; } = 3;
    }
}
