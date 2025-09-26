using HarmonyLib;
using TaleWorlds.MountAndBlade;

namespace SpearPreference
{
    // This mod makes troops prefer spears.
    public class SpearPreferenceSubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad() => new Harmony("mod.bannerlord.spearpreference").PatchAll();
    }
}
