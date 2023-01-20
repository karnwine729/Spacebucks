using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Spacebucks
{
    public class Costs
    {
        public static Costs Instance;
        private bool costsAreCached = false;
        private double totalCostsCache;
        private readonly SpaceCenterFacility[] facilities = (SpaceCenterFacility[])Enum.GetValues(typeof(SpaceCenterFacility));


        public Costs()
        {
            Instance = this;
        }

        public double GetTotalCosts()
        {
            if (costsAreCached)
            {
                return totalCostsCache;
            }
            Debug.Log("[SPACEBUCKS]: Calculating Costs...");
            double costs = 0;
            costs += GetFacilityCosts();
            costs += LaunchCosts.Instance.GetLaunchCosts();
            costs += GetWageCosts();
            costs += CrewManager.Instance.GetCrewBonuses();
            totalCostsCache = costs;
            costsAreCached = true;
            Debug.Log("[SPACEBUCKS]: Cached costs (" + Math.Round(costs) + "). Uncaching costs in 5 seconds...");
            Spacebucks.Instance.Invoke(nameof(Spacebucks.Instance.UncacheCosts), 5.0f);
            return costs;
        }

        public void UncacheCosts()
        {
            costsAreCached = false;
            Debug.Log("[SPACEBUCKS]: Costs are now uncached.");
        }

        public double GetFacilityCosts()
        {
            double costs = 0;
            for (int i = 0; i < facilities.Length; i++)
            {
                SpaceCenterFacility facility = facilities.ElementAt(i);
                if (facility == SpaceCenterFacility.LaunchPad || facility == SpaceCenterFacility.Runway) continue;
                float levelCoefficient = GetLevelCoefficient(Utilities.Instance.GetFacilityLevel(facility));
                int baseCost = GetBaseFacilityCost(facility);
                costs += baseCost * levelCoefficient;
            }
            return costs;
        }

        private float GetLevelCoefficient(int level)
        {
            if (level == 2) return 2.6f;
            if (level == 3) return 4.7f;
            if (level == 4) return 7.0f;
            if (level == 5) return 9.5f;
            return 1.0f;
        }

        private int GetBaseFacilityCost(SpaceCenterFacility facility)
        {
            switch (facility)
            {
                case SpaceCenterFacility.Administration: return SettingsClass.Instance.CostAdmin;
                case SpaceCenterFacility.AstronautComplex: return SettingsClass.Instance.CostAstronautComplex;
                case SpaceCenterFacility.MissionControl: return SettingsClass.Instance.CostMissionControl;
                case SpaceCenterFacility.ResearchAndDevelopment: return SettingsClass.Instance.CostRnd;
                case SpaceCenterFacility.SpaceplaneHangar: return SettingsClass.Instance.CostSph;
                case SpaceCenterFacility.TrackingStation: return SettingsClass.Instance.CostTrackingStation;
                case SpaceCenterFacility.VehicleAssemblyBuilding: return SettingsClass.Instance.CostVab;
                default: return 0;
            }
        }

        public double GetWageCosts()
        {
            List<CrewMember> crew = CrewManager.Instance.Kerbals.Values.ToList();
            double wages = 0;
            for (int i = 0; i < crew.Count; i++)
            {
                CrewMember c = crew.ElementAt(i);
                if (c.CrewReference().rosterStatus == ProtoCrewMember.RosterStatus.Dead || c.CrewReference().rosterStatus == ProtoCrewMember.RosterStatus.Missing) continue;
                wages += c.Wage;
            }
            return wages;
        }
    }
}