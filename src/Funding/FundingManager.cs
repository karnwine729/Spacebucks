using UnityEngine;

namespace Spacebucks
{
    public class FundingManager : Manager
    {
        public static FundingManager Instance;
        public FundingEvent NextFundingEvent;
        private Costs costs = new Costs();

        public FundingManager()
        {
            Instance = this;
            Debug.Log("[SPACEBUCKS]: Funding Manager is ready.");
        }

        public override void ManagerActionsOnEventCompleted(SpacebucksEvent eventCompleted)
        {
            Debug.Log("[SPACEBUCKS]: Funding Event completed. Setting next funding period.");
            NextFundingEvent = new FundingEvent(GetNextFundingTime(), this, true);
        }

        private double GetNextFundingTime()
        {
            double time = SettingsClass.Instance.FundingPeriodDays * FlightGlobals.GetHomeBody().solarDayLength;
            if (NextFundingEvent != null) time += NextFundingEvent.CompletionTime;
            Debug.Log("[SPACEBUCKS]: Next Funding Period at " + time);
            return time;
        }

        private bool NewAlarmIsNeeded()
        {
            double ut = Planetarium.GetUniversalTime();
            AlarmTypeBase alarmCheck = AlarmClockScenario.GetNextAlarm(ut);
            AlarmTypeBase thisAlarm = alarmCheck;
            while (true)
            {
                try
                {
                    if (thisAlarm.title.Equals("Next Funding Period"))
                    {
                        return false;
                    }
                    else
                    {
                        alarmCheck = thisAlarm;
                    }
                    thisAlarm = AlarmClockScenario.GetNextAlarm(alarmCheck.ut);
                }
                catch
                {
                    return true;
                }
            }
        }

        public void OnSave(ConfigNode node)
        {
            Debug.Log("[SPACEBUCKS]: Funding Manager: OnSave");
            ConfigNode managerNode = new ConfigNode("FUNDING_MANAGER");
            managerNode.SetValue("NextFundingEvent", NextFundingEvent.CompletionTime, true);
            node.AddNode(managerNode);
            Debug.Log("[SPACEBUCKS]: Funding Manager: OnSave Complete");
        }

        public void OnLoad(ConfigNode node)
        {
            Debug.Log("[SPACEBUCKS]: Funding Manager: OnLoad");
            ConfigNode managerNode = node.GetNode("FUNDING_MANAGER");
            double nextFundingTime = GetNextFundingTime();
            if (managerNode != null)
            {
                double.TryParse(managerNode.GetValue("NextFundingEvent"), out nextFundingTime);
            }
            NextFundingEvent = new FundingEvent(nextFundingTime, this, NewAlarmIsNeeded());
            Debug.Log("[SPACEBUCKS]: Funding Manager: OnLoad Complete");
        }
    }
}
