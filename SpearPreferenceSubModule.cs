using System.Linq;
using System.Reflection;
using HarmonyLib;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SpearPreference
{
	// This mod makes troops prefer spears by default.
	public class SpearPreferenceSubModule : MBSubModuleBase
	{
		private Harmony _harmony;
		private MethodBase _tickWeaponPreferenceMethod, _applyWeaponPreferenceMethod;

		protected override void OnSubModuleLoad() => _harmony = new Harmony("mod.bannerlord.spearpreference");

		protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
		{
			gameStarterObject.AddModel(new SpearPreferenceAgentStatCalculateModel((AgentStatCalculateModel)gameStarterObject.Models.Last(model => model is AgentStatCalculateModel)));

			_tickWeaponPreferenceMethod = AccessTools.Method(AccessTools.Inner(AccessTools.TypeByName("RBMAI.AgentAi"), "WeaponPreference"), "TickWeaponPreference");
			_applyWeaponPreferenceMethod = AccessTools.Method(AccessTools.Inner(AccessTools.TypeByName("RBMAI.AgentAi"), "WeaponPreference"), "ApplyWeaponPreference");

			// Check whether RBM is loaded.
			if (_tickWeaponPreferenceMethod != null && _applyWeaponPreferenceMethod != null)
			{
				_harmony.Patch(_tickWeaponPreferenceMethod, prefix: new HarmonyMethod(AccessTools.Method(typeof(SpearPreferenceRbmInteraction), "Prefix")));
				_harmony.Patch(_applyWeaponPreferenceMethod, prefix: new HarmonyMethod(AccessTools.Method(typeof(SpearPreferenceRbmInteraction), "Prefix")));
			}
		}

		public override void OnBeforeMissionBehaviorInitialize(Mission mission) => mission.AddMissionBehavior(new SpearPreferenceMissionBehavior());

		public override void OnGameEnd(Game game)
		{
			if (_tickWeaponPreferenceMethod != null && _applyWeaponPreferenceMethod != null)
			{
				_harmony.Unpatch(_tickWeaponPreferenceMethod, AccessTools.Method(typeof(SpearPreferenceRbmInteraction), "Prefix"));
				_harmony.Unpatch(_applyWeaponPreferenceMethod, AccessTools.Method(typeof(SpearPreferenceRbmInteraction), "Prefix"));
			}
		}
	}
}
