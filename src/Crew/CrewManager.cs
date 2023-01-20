using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Spacebucks
{
    public class CrewManager
    {
        public static CrewManager Instance;
        public readonly Dictionary<string, CrewMember> Kerbals = new Dictionary<string, CrewMember>();

        public CrewManager(List<ProtoCrewMember> crewMembers)
        {
            for (int i = 0; i < crewMembers.Count; i++)
            {
                ProtoCrewMember p = crewMembers.ElementAt(i);
                if (p.rosterStatus == ProtoCrewMember.RosterStatus.Dead) continue;
                Kerbals.Add(p.name, new CrewMember(p.name));
            }
            Instance = this;
            Debug.Log("[SPACEBUCKS]: Crew Manager Ready");
        }

        public void AddNewCrewMember(ProtoCrewMember crewMember)
        {
            CrewMember newCrew = new CrewMember(crewMember.name);
            Kerbals.Add(crewMember.name, newCrew);
            newCrew.Train();
            Debug.Log("[SPACEBUCKS]: New Crewmember added: " + newCrew.Name);
        }

        public void ProcessRetirees()
        {
            Debug.Log("[SPACEBUCKS]: Processing Retirees");
            for (int i = 0; i < Kerbals.Count; i++)
            {
                KeyValuePair<string, CrewMember> kvp = Kerbals.ElementAt(i);
                if (kvp.Value.CrewReference().rosterStatus != ProtoCrewMember.RosterStatus.Available) continue;
                if (kvp.Value.RetirementDate > Planetarium.GetUniversalTime()) continue;
                Debug.Log("[SPACEBUCKS]: " + kvp.Value.Name + " has retired.");
                HighLogic.CurrentGame.CrewRoster.Remove(kvp.Value.CrewReference());
                ScreenMessages.PostScreenMessage("[SPACEBUCKS]: " + kvp.Key + " has retired.");
                Kerbals.Remove(kvp.Value.Name);
            }
            Debug.Log("[SPACEBUCKS]: Retirees Processed");
        }

        public void UpdateCrewBonus(ProtoCrewMember crewMember, double launchTime)
        {
            Kerbals[crewMember.name].AddNewMissionBonus(Planetarium.GetUniversalTime() - launchTime);
        }

        public int GetCrewBonuses()
        {
            int bonuses = 0;
            for (int i = 0; i < Kerbals.Count; i++)
            {
                CrewMember c = Kerbals.ElementAt(i).Value;
                int thisBonus = c.GetBonus();
                bonuses += thisBonus;
            }
            return bonuses;
        }

        public void ResetCrewBonuses()
        {
            for (int i = 0; i < Kerbals.Count; i++)
            {
                CrewMember c = Kerbals.ElementAt(i).Value;
                c.ResetBonus();
            }
            Debug.Log("[SPACEBUCKS]: Mission Bonuses Reset");
        }

        public void ProcessDeadKerbal(ProtoCrewMember crewMember)
        {
            Kerbals.Remove(crewMember.name);
            float penalty = Reputation.Instance.reputation * (SettingsClass.Instance.DeathPenaltyPercent / 100.0f);
            Reputation.Instance.AddReputation(-penalty, TransactionReasons.VesselLoss);
            Debug.Log("[SPACEBUCKS]: Death Penalty Applied.");
        }

        public void ExtendRetirement(ProtoCrewMember crewMember, double launchTime)
        {
            double extension = (Planetarium.GetUniversalTime() - launchTime) * SettingsClass.Instance.RetirementExtensionFactor;
            Kerbals[crewMember.name].ExtendRetirementDate(extension);
        }

        public void OnSave(ConfigNode node)
        {
            Debug.Log("[SPACEBUCKS]: CrewManager OnSave");
            ConfigNode crewManagerNode = new ConfigNode("CREW_MANAGER");
            for (int i = 0; i < Kerbals.Count; i++)
            {
                KeyValuePair<string, CrewMember> crewKeys = Kerbals.ElementAt(i);
                crewKeys.Value.OnSave(crewManagerNode);
            }
            node.AddNode(crewManagerNode);
            Debug.Log("[SPACEBUCKS]: Crew Manager OnSaveComplete");
        }

        public void OnLoad(ConfigNode node)
        {
            Debug.Log("[SPACEBUCKS]: Crew Manager OnLoad");
            ConfigNode crewManagerNode = node.GetNode("CREW_MANAGER");
            if (crewManagerNode == null) return;
            ConfigNode[] crewNodes = crewManagerNode.GetNodes("CREW_MEMBER");
            for (int i = 0; i < crewNodes.Length; i++)
            {
                ConfigNode crewConfig = crewNodes.ElementAt(i);
                if (Kerbals.TryGetValue(crewConfig.GetValue("Name"), out CrewMember c)) c.OnLoad(crewConfig);
                else Debug.Log("[SPACEBUCKS]: Loaded config for " + crewConfig.GetValue("Name") + " but actual CrewMember could not be found! Skipping.");
            }
            Debug.Log("[SPACEBUCKS]: Crew Manager OnLoad Complete");
        }
    }
}
