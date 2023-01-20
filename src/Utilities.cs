using System;
using System.Collections.Generic;
using System.Linq;

namespace Spacebucks
{
    public class Utilities
    {
        public static Utilities Instance;
        public readonly Random Randomize = new Random();

        public Utilities()
        {
            Instance = this;
        }

        public double GetGrossFunding()
        {
            return Reputation.Instance.reputation * SettingsClass.Instance.FundingMultiplier;
        }

        public double GetNetFunding()
        {
            return GetGrossFunding() - Costs.Instance.GetTotalCosts();
        }

        public int GetFacilityLevel(SpaceCenterFacility facility)
        {
            float level = ScenarioUpgradeableFacilities.GetFacilityLevel(facility);
            float count = ScenarioUpgradeableFacilities.GetFacilityLevelCount(facility);
            return (int)(level * count) + 1;
        }

        public void NewAlarm(string alarmName, string alarmDescription, double alarmTime)
        {
            AlarmTypeRaw alarmToSet = new AlarmTypeRaw
            {
                title = alarmName,
                description = alarmDescription,
                actions =
                {
                    warp = AlarmActions.WarpEnum.KillWarp,
                    message = AlarmActions.MessageEnum.Yes
                },
                ut = alarmTime
            };
            AlarmClockScenario.AddAlarm(alarmToSet);
        }

        public string GetARandomKerbal()
        {
            List<ProtoCrewMember> crew = HighLogic.CurrentGame.CrewRoster.Crew.ToList();
            int tries = 0;
            if (crew.Count == 0) return "Wernher Von Kerman";
            while (tries < 100)
            {
                ProtoCrewMember p = crew.ElementAt(Randomize.Next(0, crew.Count));
                if (p.rosterStatus == ProtoCrewMember.RosterStatus.Available) return p.name;
                tries++;
            }
            return "Wernher Von Kerman";
        }

        public string GetARandomBody()
        {
            return FinePrint.Utilities.CelestialUtilities.RandomBody(FlightGlobals.Bodies).displayName;
        }

        public int ConvertUtToDays(double ut)
        {
            int days = 0;
            double dayLength = FlightGlobals.GetHomeBody().solarDayLength;
            while (ut > dayLength)
            {
                days++;
                ut -= dayLength;
            }
            return days;
        }        
    }
}
