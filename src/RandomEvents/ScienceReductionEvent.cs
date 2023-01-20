using System;
using UnityEngine;

namespace Spacebucks
{
    public class ScienceReductionEvent : RandomEventBase
    {
        public ScienceReductionEvent()
        {
            Name = "ScienceReductionEvent";
            Title = "Incident Report";
            Body = "An incident in the R&D Complex resulted in a loss of science!";
            EventEffect = GetScienceReduction() * -1;
            AcceptString = EventEffect + " Science";
        }

        private int GetScienceReduction()
        {
            float s = ResearchAndDevelopment.Instance.Science;
            int max = (int)(s / 8);
            return Math.Min(Utilities.Instance.Randomize.Next(max) + 1, 100);
        }

        public override bool EventCanFire()
        {
            return (ResearchAndDevelopment.Instance.Science >= 8);
        }

        protected override void OnEventAccepted()
        {
            ResearchAndDevelopment.Instance.AddScience(EventEffect, TransactionReasons.None);            
        }
    }
}
