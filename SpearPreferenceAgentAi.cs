namespace SpearPreference
{
	public class SpearPreferenceAgentAi
	{
		// Override RBM's weapon preference logic.
		public static bool Prefix() => !SpearPreferenceSettings.Instance.ShouldOverrideRbmWeaponPreference;
	}
}
