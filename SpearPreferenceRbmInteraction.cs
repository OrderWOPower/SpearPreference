namespace SpearPreference
{
	public class SpearPreferenceRbmInteraction
	{
		// Override RBM's weapon preference logic.
		public static bool Prefix() => !SpearPreferenceSettings.Instance.ShouldOverrideRbmWeaponPreference;
	}
}
