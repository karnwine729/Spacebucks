using System;
using UnityEngine;

namespace Spacebucks
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class RandomEventLoader : MonoBehaviour
    {
        public static RandomEventLoader Instance;
        private double cooldownTimer;

        private void Awake()
        {
            Instance = this;
        }

        private void RollEvent()
        {
            if (HighLogic.CurrentGame.Mode != Game.Modes.CAREER) return;
            if (cooldownTimer > Planetarium.GetUniversalTime()) return;
            if (Utilities.Instance.Randomize.NextDouble() > SettingsClass.Instance.RandomEventChance) return;
            RandomEventBase re = GetRandomEvent();
            Debug.Log("[SPACEBUCKS]: Attempting to Fire Event: " + re.Name);
            if (!re.EventCanFire())
            {
                Debug.Log("[SPACEBUCKS]: Event Can't Fire");
                return;
            }
            Debug.Log("[SPACEBUCKS]: Event Can Fire");
            re.OnEventFire();
            cooldownTimer = Planetarium.GetUniversalTime() + FlightGlobals.GetHomeBody().solarDayLength * SettingsClass.Instance.RandomEventCooldownDays;
        }

        private RandomEventBase GetRandomEvent()
        {
            int rng = Utilities.Instance.Randomize.Next(100);
            if (rng < 14) return new ReputationReductionEvent();
            if (rng < 28) return new ReputationIncreaseEvent();
            if (rng < 42) return new FundsReductionEvent();
            if (rng < 56) return new FundsIncreaseEvent();
            if (rng < 70) return new ScienceReductionEvent();
            if (rng < 84) return new ScienceIncreaseEvent();
            if (rng < 88) return new SickEvent();
            if (rng < 96) return new VacationEvent();
            return new TrainingEvent();
        }

        public void OnSave(ConfigNode node)
        {
            ConfigNode eventNode = new ConfigNode("EVENTS");
            eventNode.SetValue("cooldown", cooldownTimer, true);
            node.AddNode(eventNode);
        }

        public void OnLoad(ConfigNode node)
        {
            ConfigNode eventNode = node.GetNode("EVENTS");
            if (eventNode == null) return;
            double.TryParse(eventNode.GetValue("cooldown"), out cooldownTimer);
            RollEvent();
        }
    }
}