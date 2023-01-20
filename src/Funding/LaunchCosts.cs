using UnityEngine;

namespace Spacebucks
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class LaunchCosts : MonoBehaviour
    {
        public static LaunchCosts Instance;
        public double LaunchCostsAccumulator;

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        public void OnSave(ConfigNode node)
        {
            node.SetValue("LaunchCosts", LaunchCostsAccumulator, true);
        }

        public void OnLoad(ConfigNode node)
        {
            node.TryGetValue("LaunchCosts", ref LaunchCostsAccumulator);
        }

        public void OnVesselRollout(ShipConstruct ship)
        {
            if (SettingsClass.Instance.CostVabLaunch < 0 || SettingsClass.Instance.CostSphLaunch < 0) return;
            if (SettingsClass.Instance.CostVabLaunch + SettingsClass.Instance.CostSphLaunch == 0) return;

            ship.GetShipMass(out float dryMass, out float fuelMass);
            float mass = dryMass + fuelMass;
            SpaceCenterFacility facility = (ship.shipFacility == EditorFacility.VAB) ? SpaceCenterFacility.LaunchPad : SpaceCenterFacility.Runway;

            LaunchCostsAccumulator += CalculateLaunchCost(facility, mass);

            Debug.Log("[SPACEBUCKS]: Launch Registered");
        }

        private double CalculateLaunchCost(SpaceCenterFacility facility, float shipMass)
        {
            int baseCost = (facility == SpaceCenterFacility.LaunchPad) ? SettingsClass.Instance.CostVabLaunch : SettingsClass.Instance.CostSphLaunch;
            int facilityLevel = Utilities.Instance.GetFacilityLevel(facility);
            switch (facilityLevel)
            {
                default: return baseCost * shipMass * 1.0f;
                case 2: return baseCost * shipMass * 1.1f;
                case 3: return baseCost * shipMass * 1.2f;
                case 4: return baseCost * shipMass * 1.3f;
                case 5: return baseCost * shipMass * 1.5f;                
            }
        }

        public double GetLaunchCosts()
        {
            return LaunchCostsAccumulator;
        }

        public void ResetLaunchCosts()
        {
            LaunchCostsAccumulator = 0;
            Debug.Log("[SPACEBUCKS]: Launch Costs Reset");
        }
    }
}