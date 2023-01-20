using UnityEngine;

namespace Spacebucks
{
    public class ReputationIncreaseEvent : RandomEventBase
    {
        public ReputationIncreaseEvent()
        {
            Name = "ReputationIncreaseEvent";
            Title = "Funding Increase";
            Body = "Recent events have resulted in increased funding available for the space program.";
            EventEffect = GetReputationIncrease();
            AcceptString = "+" + EventEffect + "% Future Funding";
        }

        private int GetReputationIncrease()
        {
            return Utilities.Instance.Randomize.Next(5, 19);
        }

        public override bool EventCanFire()
        {
            return (Reputation.Instance.reputation > 0 && Reputation.Instance.reputation < 500);
        }

        protected override void OnEventAccepted()
        {            
            Reputation.Instance.AddReputation(Reputation.Instance.reputation * EventEffect / 100, TransactionReasons.None);            
        }
    }
}