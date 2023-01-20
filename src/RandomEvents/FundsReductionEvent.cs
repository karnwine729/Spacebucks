using UnityEngine;
using System;

namespace Spacebucks
{
    public class FundsReductionEvent : RandomEventBase
    {
        private int rng;

        public FundsReductionEvent()
        {
            Name = "FundsReductionEvent";
            Title = "Incident Report";
            rng = Utilities.Instance.Randomize.Next(7);
            Body = GenerateMessageBody();
            EventEffect = (int)GetFundsReduction() * -1;
            AcceptString = EventEffect + " Funds";
        }

        private string GenerateMessageBody()
        {
            return "An incident in the " + GetFacilityName() + " resulted in some maintenance repairs and equipment costs.";
        }

        private string GetFacilityName()
        {
            switch (rng)
            {
                case 0: return "Administration Building";
                case 1: return "Astronaut Complex";
                case 2: return "Mission Control Building";
                case 3: return "R&D Complex";
                case 4: return "Spaceplane Hangar";
                case 5: return "Tracking Station";
                case 6: return "Vehicle Assembly Building";
            }
            return "<error>";
        }

        private SpaceCenterFacility GetFacility()
        {
            switch (rng)
            {
                case 0: return SpaceCenterFacility.Administration;
                case 1: return SpaceCenterFacility.AstronautComplex;
                case 2: return SpaceCenterFacility.MissionControl;
                case 3: return SpaceCenterFacility.ResearchAndDevelopment;
                case 4: return SpaceCenterFacility.SpaceplaneHangar;
                case 5: return SpaceCenterFacility.TrackingStation;
                case 6: return SpaceCenterFacility.VehicleAssemblyBuilding;
            }
            return SpaceCenterFacility.LaunchPad;
        }

        private double GetFundsReduction()
        {
            SpaceCenterFacility facility = GetFacility();
            if (facility == SpaceCenterFacility.LaunchPad) return -1;

            int level = Utilities.Instance.GetFacilityLevel(facility);
            if (rng == 0) return Utilities.Instance.Randomize.Next(4000, 6000) * Math.Pow(level, 1.2);
            if (rng == 1) return Utilities.Instance.Randomize.Next(4800, 7200) * Math.Pow(level, 1.2);
            if (rng == 2) return Utilities.Instance.Randomize.Next(6400, 9600) * Math.Pow(level, 1.2);
            if (rng == 3) return Utilities.Instance.Randomize.Next(24000, 36000) * Math.Pow(level, 1.2);
            if (rng == 4) return Utilities.Instance.Randomize.Next(14400, 21600) * Math.Pow(level, 1.2);
            if (rng == 5) return Utilities.Instance.Randomize.Next(8000, 12000) * Math.Pow(level, 1.2);
            return Utilities.Instance.Randomize.Next(18400, 27600) * Math.Pow(level, 1.2);
        }

        public override bool EventCanFire()
        {
            return (Math.Abs(EventEffect) <= Funding.Instance.Funds);
        }

        protected override void OnEventAccepted()
        {
            Funding.Instance.AddFunds(EventEffect, TransactionReasons.None);
        }
    }
}