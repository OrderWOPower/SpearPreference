using HarmonyLib;
using TaleWorlds.Core;

namespace SpearPreference
{
    [HarmonyPatch(typeof(WeaponComponentData), "Init")]
    public class SpearPreferenceComponentData
    {
        public static void Prefix(string weaponUsageName, ref int thrustDamage)
        {
            if (weaponUsageName.Contains("Polearm") && !weaponUsageName.Contains("Javelin"))
            {
                // Multiply the thrust damage of spears.
                thrustDamage *= SpearPreferenceSettings.Instance.SpearPreferenceMultiplier;
            }
        }
    }
}
