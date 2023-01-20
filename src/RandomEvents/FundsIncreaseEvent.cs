using UnityEngine;

namespace Spacebucks
{
    public class FundsIncreaseEvent : RandomEventBase
    {
        public FundsIncreaseEvent()
        {
            Name = "FundsIncreaseEvent";
            Title = "More Funding Available";
            Body = "More funding has become immediately available.";
            EventEffect = GetFundsIncrease();
            AcceptString = "+" + EventEffect + " Funds";
        }

        private int GetFundsIncrease()
        {
            float r = Reputation.Instance.reputation;
            int min = (int)(r / 10);
            int max = (int)(r / 3);
            return Utilities.Instance.Randomize.Next(min, max) * 1000;
        }

        public override bool EventCanFire()
        {
            return (Reputation.Instance.reputation > 10 && Funding.Instance.Funds < 500000);
        }

        protected override void OnEventAccepted()
        {
            Funding.Instance.AddFunds(EventEffect, TransactionReasons.None);
        }
    }
}