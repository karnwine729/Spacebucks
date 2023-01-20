namespace Spacebucks
{
    [KSPScenario(ScenarioCreationOptions.AddToExistingCareerGames | ScenarioCreationOptions.AddToNewCareerGames, GameScenes.FLIGHT, GameScenes.TRACKSTATION, GameScenes.SPACECENTER)]
    public class SpacebucksScenario : ScenarioModule
    {
        public override void OnSave(ConfigNode node)
        {
            Spacebucks.Instance.OnSave(node);
        }

        public override void OnLoad(ConfigNode node)
        {
            Spacebucks.Instance.OnLoad(node);
        }
    }
}