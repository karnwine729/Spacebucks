using KSP.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Spacebucks
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class AstronautComplexOverride : MonoBehaviour
    {
        public static AstronautComplexOverride Instance;
        public bool AstronautComplexSpawned;
        public int UpdateCount = 4;

        private void Awake()
        {
            Instance = this;
        }

        private void LateUpdate()
        {
            if (!AstronautComplexSpawned || UpdateCount <= 0) return;
            List<CrewListItem> crewItems = FindObjectsOfType<CrewListItem>().ToList();
            UpdateCount--;
            for (int i = 0; i < crewItems.Count; i++)
            {
                CrewListItem c = crewItems.ElementAt(i);
                if (c.GetCrewRef().type != ProtoCrewMember.KerbalType.Crew) continue;
                c.SetLabel(GenerateAstronautString(c.GetCrewRef().name));
            }
        }

        private string GenerateAstronautString(string kerbalName)
        {
            CrewMember c = CrewManager.Instance.Kerbals[kerbalName];
            if (c == null) return "Available For Next Mission";
            StringBuilder sb = new StringBuilder();
            if (!c.CrewReference().inactive) sb.AppendLine("Available | " + "Wage: " + c.Wage);
            else
            {
                if (!c.OutSick && !c.OnVacation && c.InTraining) sb.AppendLine("In Training | " + "Wage: " + c.Wage);
                if (c.OutSick && !c.OnVacation && !c.InTraining) sb.AppendLine("Out Sick | " + "Wage: " + c.Wage);
                if (!c.OutSick && c.OnVacation && !c.InTraining) sb.AppendLine("On Vacation | " + "Wage: " + c.Wage);

                int availableDate = Utilities.Instance.ConvertUtToDays(c.AvailableDate - Planetarium.GetUniversalTime()) + 1;
                if (availableDate == 1) sb.AppendLine("Available in 1 day.");
                else sb.AppendLine("Available in " + availableDate + " days.");


            }
            return sb.ToString();
        }
    }
}