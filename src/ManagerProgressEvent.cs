namespace Spacebucks
{
    public class ManagerProgressEvent : SpacebucksEvent
    {
        public ManagerProgressEvent()
        {
            CompletionTime = Planetarium.GetUniversalTime() + FlightGlobals.GetHomeBody().solarDayLength;
            AddTimer();
        }

        public override void OnEventCompleted()
        {
            Spacebucks.Instance.ProgressEvent = new ManagerProgressEvent();
            Spacebucks.Instance.LastProgressUpdate = Planetarium.GetUniversalTime();
        }
    }
}
