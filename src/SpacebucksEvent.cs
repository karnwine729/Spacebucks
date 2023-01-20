namespace Spacebucks
{
    public class SpacebucksEvent
    {
        public double CompletionTime;
        public bool StopTimewarpOnCompletion = false;
        protected Manager ParentManager;

        protected void AddTimer()
        {
            TimerScript.Instance.AddTimer(this);
        }

        public virtual void OnEventCompleted() { }

        protected void InformParent()
        {
            ParentManager.ManagerActionsOnEventCompleted(this);
        }
    }
}
