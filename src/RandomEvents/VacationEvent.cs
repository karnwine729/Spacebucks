using UnityEngine;

namespace Spacebucks
{
    public class VacationEvent : RandomEventBase
    {        
        public VacationEvent()
        {
            KerbalName = Utilities.Instance.GetARandomKerbal();
            Name = "VacationEvent";
            Title = KerbalName + " On Vacation";
            Body = KerbalName + " has decided to go on vacation.";
            EventEffect = GetVacationDays();
            AcceptString = KerbalName + " On Vacation for " + EventEffect + " Days";
        }

        private int GetVacationDays()
        {
            return Utilities.Instance.Randomize.Next(1, 7) * 5;
        }        

        public override bool EventCanFire()
        {
            return CrewManager.Instance.Kerbals[KerbalName].CrewReference() != null && !CrewManager.Instance.Kerbals[KerbalName].CrewReference().inactive;
        }

        protected override void OnEventAccepted()
        {
            CrewManager.Instance.Kerbals[KerbalName].InTraining = false;
            CrewManager.Instance.Kerbals[KerbalName].OutSick = false;
            CrewManager.Instance.Kerbals[KerbalName].OnVacation = true;
            double timeInactive = FlightGlobals.GetHomeBody().solarDayLength * EventEffect;
            CrewManager.Instance.Kerbals[KerbalName].AvailableDate = Planetarium.GetUniversalTime() + timeInactive;
            CrewManager.Instance.Kerbals[KerbalName].CrewReference().SetInactive(timeInactive);
        }
    }
}