using System.Linq;
using UnityEngine;

namespace Spacebucks
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class SpacebucksSpaceCentre : Spacebucks { }

    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class SpacebucksFlight : Spacebucks { }

    [KSPAddon(KSPAddon.Startup.TrackingStation, false)]
    public class SpacebucksTrackingStation : Spacebucks { }

    public class Spacebucks : MonoBehaviour
    {
        public static Spacebucks Instance;
        public SettingsClass Settings;
        public FundingManager InitializedFundingManager;
        public CrewManager InitializedCrewManager;
        public ManagerProgressEvent ProgressEvent;
        public double LastProgressUpdate = 0;
        private Utilities utilitiesReference = new Utilities();

        private void Awake()
        {
            Instance = this;
            Settings = new SettingsClass();
            InitializedFundingManager = new FundingManager();
            InitializedCrewManager = new CrewManager(HighLogic.CurrentGame.CrewRoster.Crew.ToList());
            LastProgressUpdate = Planetarium.GetUniversalTime();
            Debug.Log("[SPACEBUCKS]: Awake");
        }

        private void Start()
        {
            Costs.Instance.GetTotalCosts();
        }

        public void UncacheCosts()
        {
            Costs.Instance.UncacheCosts();
        }

        public void OnSave(ConfigNode node)
        {
            Debug.Log("[SPACEBUCKS]: OnSave");
            FundingManager.Instance.OnSave(node);
            CrewManager.Instance.OnSave(node);
            LaunchCosts.Instance.OnSave(node);
            RandomEventLoader.Instance.OnSave(node);
            node.SetValue("LastProgressUpdate", LastProgressUpdate, true);
            Debug.Log("[SPACEBUCKS]: OnSave Complete");
        }

        public void OnLoad(ConfigNode node)
        {
            Debug.Log("[SPACEBUCKS]: OnLoad");
            FundingManager.Instance.OnLoad(node);
            CrewManager.Instance.OnLoad(node);
            LaunchCosts.Instance.OnLoad(node);
            RandomEventLoader.Instance.OnLoad(node);
            node.TryGetValue("LastProgressUpdate", ref LastProgressUpdate);
            if (ProgressEvent == null) ProgressEvent = new ManagerProgressEvent();
            Debug.Log("[SPACEBUCKS]: OnLoad Complete");
        }
    }
}