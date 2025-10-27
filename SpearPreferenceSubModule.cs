using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace SpearPreference
{
    // This mod makes troops prefer spears by default.
    public class SpearPreferenceSubModule : MBSubModuleBase
    {
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject) => gameStarterObject.AddModel(new SpearPreferenceAgentStatCalculateModel((AgentStatCalculateModel)gameStarterObject.Models.Last(model => model is AgentStatCalculateModel)));

        public override void OnBeforeMissionBehaviorInitialize(Mission mission) => mission.AddMissionBehavior(new SpearPreferenceMissionBehavior());
    }
}
