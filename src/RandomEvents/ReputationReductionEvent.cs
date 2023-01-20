using UnityEngine;

namespace Spacebucks
{
    public class ReputationReductionEvent : RandomEventBase
    {
        public ReputationReductionEvent()
        {
            Name = "ReputationReductionEvent";
            Title = "Funding Decrease";
            Body = "Recent events have resulted in decreased funding available for the space program.";
            EventEffect = GetReputationIncrease() * -1;
            AcceptString = EventEffect + "% Future Funding";
        }

        private int GetReputationIncrease()
        {
            return Utilities.Instance.Randomize.Next(5, 19);
        }

        public override bool EventCanFire()
        {
            return (Reputation.Instance.reputation >= 20);
        }

        protected override void OnEventAccepted()
        {            
            Reputation.Instance.AddReputation(Reputation.Instance.reputation * EventEffect / 100, TransactionReasons.None);            
        }
    }
}