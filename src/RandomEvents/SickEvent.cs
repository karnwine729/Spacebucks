using UnityEngine;

namespace Spacebucks
{
    public class SickEvent : RandomEventBase
    {        
        public SickEvent()
        {
            KerbalName = Utilities.Instance.GetARandomKerbal();
            Name = "SickEvent";
            Title = KerbalName + "Out Sick";
            Body = KerbalName + " has come down with an illness and will be unavailable for a few days.";
            EventEffect = GetSickDays();
            AcceptString = KerbalName + " Out Sick for " + EventEffect + " Days";
        }

        private int GetSickDays()
        {
            return Utilities.Instance.Randomize.Next(4, 16);
        }        

        public override bool EventCanFire()
        {
            return CrewManager.Instance.Kerbals[KerbalName].CrewReference() != null && !CrewManager.Instance.Kerbals[KerbalName].CrewReference().inactive;
        }

        protected override void OnEventAccepted()
        {
            CrewManager.Instance.Kerbals[KerbalName].InTraining = false;
            CrewManager.Instance.Kerbals[KerbalName].OutSick = true;
            CrewManager.Instance.Kerbals[KerbalName].OnVacation = false;
            double timeInactive = FlightGlobals.GetHomeBody().solarDayLength * EventEffect;
            CrewManager.Instance.Kerbals[KerbalName].AvailableDate = Planetarium.GetUniversalTime() + timeInactive;
            CrewManager.Instance.Kerbals[KerbalName].CrewReference().SetInactive(timeInactive);
        }
    }
}