using System;
using UnityEngine;

namespace Spacebucks
{
    public class FundingEvent : SpacebucksEvent
    {
        public readonly float FundingPeriod;

        public FundingEvent(double fundingTime, FundingManager manager, bool newAlarmNeeded)
        {
            FundingPeriod = SettingsClass.Instance.FundingPeriodDays;
            CompletionTime = fundingTime;
            ParentManager = manager;
            if (newAlarmNeeded) Utilities.Instance.NewAlarm("Next Funding Period", "Next Funding Period", CompletionTime);
            StopTimewarpOnCompletion = true;
            AddTimer();
        }

        public override void OnEventCompleted()
        {
            CrewManager.Instance.ProcessRetirees();
            int netFunding = (int)Math.Round(Utilities.Instance.GetNetFunding());
            Debug.Log("[SPACEBUCKS]: OnFundingAwarded. Awarding " + netFunding + " funds.");
            Funding.Instance.AddFunds(netFunding, TransactionReasons.Contracts);
            LaunchCosts.Instance.ResetLaunchCosts();
            CrewManager.Instance.ResetCrewBonuses();
            ApplyRepDecay();
            InformParent();
        }

        public void ApplyRepDecay()
        {
            Debug.Log("[SPACEBUCKS]: Applying Rep Decay");
            float r = Reputation.Instance.reputation;
            float decayCoeff = (float)Math.Exp(r * -0.0023);
            float repDecay = SettingsClass.Instance.StartingRepDecay * decayCoeff * r;
            Debug.Log("[SPACEBUCKS]: Rep Decay: -" + repDecay);
            Reputation.Instance.AddReputation(-repDecay, TransactionReasons.Contracts);
        }
    }
}
