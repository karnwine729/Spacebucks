using UnityEngine;

namespace Spacebucks
{
    public class TrainingEvent : RandomEventBase
    {        
        public TrainingEvent()
        {
            KerbalName = Utilities.Instance.GetARandomKerbal();
            Name = "TrainingEvent";
            Title = KerbalName + " in Training";
            Body = KerbalName + " has re-entered training for a few days.";
            EventEffect = GetTrainingDays();
            AcceptString = KerbalName + " in Training for " + EventEffect + " Days";
        }

        private int GetTrainingDays()
        {
            return Utilities.Instance.Randomize.Next(1, 7) * 5;
        }        

        public override bool EventCanFire()
        {
            return CrewManager.Instance.Kerbals[KerbalName].CrewReference() != null && !CrewManager.Instance.Kerbals[KerbalName].CrewReference().inactive;
        }

        protected override void OnEventAccepted()
        {
            CrewManager.Instance.Kerbals[KerbalName].InTraining = true;
            CrewManager.Instance.Kerbals[KerbalName].OutSick = false;
            CrewManager.Instance.Kerbals[KerbalName].OnVacation = false;
            double timeInactive = FlightGlobals.GetHomeBody().solarDayLength * EventEffect;
            CrewManager.Instance.Kerbals[KerbalName].AvailableDate = Planetarium.GetUniversalTime() + timeInactive;
            CrewManager.Instance.Kerbals[KerbalName].CrewReference().SetInactive(timeInactive);
        }
    }
}