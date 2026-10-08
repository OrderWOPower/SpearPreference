using HarmonyLib;
using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SpearPreference
{
	// This mod makes troops prefer spears by default.
	public class SpearPreferenceSubModule : MBSubModuleBase
	{
		private Harmony _harmony;
		private Type _typeofAgentAi;

		protected override void OnSubModuleLoad() => _harmony = new Harmony("mod.bannerlord.spearpreference");

		protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
		{
			gameStarterObject.AddModel(new SpearPreferenceAgentStatCalculateModel((AgentStatCalculateModel)gameStarterObject.Models.Last(model => model is AgentStatCalculateModel)));

			_typeofAgentAi = AccessTools.TypeByName("RBMAI.AgentAi");

			// Check whether RBM is loaded.
			if (_typeofAgentAi != null)
			{
				_harmony.Patch(AccessTools.Method(AccessTools.Inner(_typeofAgentAi, "WeaponPreference"), "TickWeaponPreference"), prefix: new HarmonyMethod(AccessTools.Method(typeof(SpearPreferenceAgentAi), "Prefix")));
				_harmony.Patch(AccessTools.Method(AccessTools.Inner(_typeofAgentAi, "WeaponPreference"), "ApplyWeaponPreference"), prefix: new HarmonyMethod(AccessTools.Method(typeof(SpearPreferenceAgentAi), "Prefix")));
			}
		}

		public override void OnBeforeMissionBehaviorInitialize(Mission mission) => mission.AddMissionBehavior(new SpearPreferenceMissionBehavior());

		public override void OnGameEnd(Game game)
		{
			if (_typeofAgentAi != null)
			{
				_harmony.Unpatch(AccessTools.Method(AccessTools.Inner(_typeofAgentAi, "WeaponPreference"), "TickWeaponPreference"), AccessTools.Method(typeof(SpearPreferenceAgentAi), "Prefix"));
				_harmony.Unpatch(AccessTools.Method(AccessTools.Inner(_typeofAgentAi, "WeaponPreference"), "ApplyWeaponPreference"), AccessTools.Method(typeof(SpearPreferenceAgentAi), "Prefix"));
			}
		}
	}
}
