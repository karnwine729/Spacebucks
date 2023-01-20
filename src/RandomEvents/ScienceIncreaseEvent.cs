using System;
using UnityEngine;

namespace Spacebucks
{
    public class ScienceIncreaseEvent : RandomEventBase
    {
        public ScienceIncreaseEvent()
        {
            Name = "ScienceIncreaseEvent";
            Title = "Science Gain";
            Body = "A recent scientific discovery has been shared with the space program.";
            EventEffect = GetScienceIncrease();
            AcceptString = "+" + EventEffect + " Science";
        }

        private int GetScienceIncrease()
        {
            float r = Reputation.Instance.reputation;
            if (r >= 500) return Utilities.Instance.Randomize.Next(5, 26);
            int min = Math.Max((int)(r / 100), 1);
            int max = (int)(r / 20);
            return Utilities.Instance.Randomize.Next(min, max);            
        }

        public override bool EventCanFire()
        {
            return (Reputation.Instance.reputation > 60);
        }

        protected override void OnEventAccepted()
        {
            ResearchAndDevelopment.Instance.AddScience(EventEffect, TransactionReasons.None);
        }
    }
}