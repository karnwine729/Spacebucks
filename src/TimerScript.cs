using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Spacebucks
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class TimerScriptFlight : TimerScript { }

    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class TimerScriptSpaceCentre : TimerScript { }

    [KSPAddon(KSPAddon.Startup.TrackingStation, false)]
    public class TimerScriptTrackingStation : TimerScript { }

    public class TimerScript : MonoBehaviour
    {
        public static TimerScript Instance;
        private readonly Dictionary<SpacebucksEvent, double> fundingEvents = new Dictionary<SpacebucksEvent, double>();
        private List<KeyValuePair<SpacebucksEvent, double>> fundingEventCache;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            InvokeRepeating(nameof(CheckTimers), 0.1f, 0.1f);
        }

        public void AddTimer(SpacebucksEvent eventToAdd)
        {            
            fundingEvents.Add(eventToAdd, eventToAdd.CompletionTime);
        }

        public void RemoveTimer(SpacebucksEvent eventToRemove)
        {
            fundingEvents.Remove(eventToRemove);
        }

        private void CheckTimers()
        {
            
            double time = Planetarium.GetUniversalTime();
            fundingEventCache = fundingEvents.ToList();            
            foreach (KeyValuePair<SpacebucksEvent, double> e in fundingEventCache)
            {
                if (e.Value > time) continue;
                e.Key.OnEventCompleted();
                fundingEvents.Remove(e.Key);
                if (e.Key.StopTimewarpOnCompletion) TimeWarp.SetRate(0, true);
            }
        }
    }
}
