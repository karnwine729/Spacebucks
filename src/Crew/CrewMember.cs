using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Spacebucks
{
    public class CrewMember
    {
        public double RetirementDate;
        public double AvailableDate;
        public bool InTraining;
        public bool OutSick;
        public bool OnVacation;
        public string Name { get; private set; }
        public double Wage
        {
            get
            {
                float levelModifier = 1.0f;
                if (crewRef.experienceLevel == 1.0f) levelModifier = 2.0f;
                if (crewRef.experienceLevel == 2.0f) levelModifier = 2.5f;
                if (crewRef.experienceLevel == 3.0f) levelModifier = 3.0f;
                if (crewRef.experienceLevel == 4.0f) levelModifier = 3.5f;
                if (crewRef.experienceLevel == 5.0f) levelModifier = 4.0f;
                return levelModifier * SettingsClass.Instance.BaseAstronautWage;
            }
        }
        private ProtoCrewMember crewRef;
        private int bonus;

        public CrewMember(string kerbalName)
        {
            Name = kerbalName;
            int minTerm = SettingsClass.Instance.MinimumTermYears;
            int maxTerm = SettingsClass.Instance.MaximumTermYears;
            RetirementDate = Utilities.Instance.Randomize.Next(minTerm, maxTerm) * FlightGlobals.GetHomeBody().orbit.period + Planetarium.GetUniversalTime();
            Debug.Log("[SPACEBUCKS]: New CrewMember Setup: " + kerbalName);
        }

        public ProtoCrewMember CrewReference()
        {
            List<ProtoCrewMember> crew = HighLogic.CurrentGame.CrewRoster.Crew.ToList();
            for (int i = 0; i < crew.Count; i++)
            {
                ProtoCrewMember p = crew.ElementAt(i);
                if (p.name != Name) continue;
                crewRef = p;
                break;
            }

            if (crewRef == null)
            {
                Debug.Log("[SPACEBUCKS]: Couldn't find " + Name + " in the crew list. Checking Applicants.");
                crew = HighLogic.CurrentGame.CrewRoster.Applicants.ToList();
                for (int i = 0; i < crew.Count; i++)
                {
                    ProtoCrewMember p = crew.ElementAt(i);
                    if (p.name != Name) continue;
                    crewRef = p;
                    break;
                }
            }

            if (crewRef == null) Debug.Log("[SPACEBUCKS]: Couldn't find ProtoCrewMember object for " + Name + "!");
            return crewRef;
        }

        public void AddNewMissionBonus(double timeOnMission)
        {
            int daysOnMission = Utilities.Instance.ConvertUtToDays(timeOnMission);
            double dailyWage = Wage / SettingsClass.Instance.FundingPeriodDays;
            int newBonus = (int)Math.Round(daysOnMission * dailyWage);
            bonus += newBonus;
            Debug.Log("[SPACEBUCKS]: Assigned Mission Bonus of " + newBonus + " to " + Name);
        }

        public int GetBonus()
        {
            return bonus;
        }

        public void ResetBonus()
        {
            bonus = 0;
        }

        public void ExtendRetirementDate(double extension)
        {
            Debug.Log("[SPACEBUCKS]: Extending retirement date(" + RetirementDate + ") by " + extension + " for " + Name);
            RetirementDate += extension;
        }

        public void Train()
        {
            InTraining = true;
            double trainingTime = FlightGlobals.GetHomeBody().solarDayLength * SettingsClass.Instance.TrainingLengthDays;
            AvailableDate = Planetarium.GetUniversalTime() + trainingTime;
            CrewReference().SetInactive(trainingTime);
            Utilities.Instance.NewAlarm(Name + " - Training", Name + " has completed their training.", AvailableDate);
            Debug.Log("[SPACEBUCKS]: " + Name + " entered training.");
        }

        public void OnSave(ConfigNode crewManagerNode)
        {
            ConfigNode crewNode = new ConfigNode("CREW_MEMBER");
            crewNode.SetValue("Name", Name, true);
            crewNode.SetValue("Bonus", bonus, true);
            crewNode.SetValue("RetirementDate", RetirementDate, true);
            crewNode.SetValue("AvailableDate", AvailableDate, true);
            crewNode.SetValue("InTraining", InTraining, true);
            crewNode.SetValue("OutSick", OutSick, true);
            crewNode.SetValue("OnVacation", OnVacation, true);
            crewManagerNode.AddNode(crewNode);
        }

        public void OnLoad(ConfigNode crewConfig)
        {
            Name = crewConfig.GetValue("Name");
            int.TryParse(crewConfig.GetValue("Bonus"), out bonus);
            double.TryParse(crewConfig.GetValue("RetirementDate"), out RetirementDate);
            double.TryParse(crewConfig.GetValue("AvailableDate"), out AvailableDate);
            Boolean.TryParse(crewConfig.GetValue("InTraining"), out InTraining);
            Boolean.TryParse(crewConfig.GetValue("OutSick"), out OutSick);
            Boolean.TryParse(crewConfig.GetValue("OnVacation"), out OnVacation);
        }
    }
}