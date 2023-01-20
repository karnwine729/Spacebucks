using Contracts;
using FlightTracker;
using UnityEngine;

namespace Spacebucks
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    internal class RegisterGameEvents : MonoBehaviour
    {
        private void Awake()
        {
            Debug.Log("[SPACEBUCKS]: RegisterGameEvents Awake");
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            Debug.Log("[SPACEBUCKS]: Registering GameEvents");
            GameEvents.OnVesselRollout.Add(AddLaunch);
            GameEvents.Contract.onOffered.Add(OnContractOffered);
            GameEvents.OnCrewmemberHired.Add(CrewManagerAddCrew);            
            GameEvents.onKerbalStatusChanged.Add(PotentialKerbalDeath);
            GameEvents.onGUIAstronautComplexSpawn.Add(AstronautComplexSpawned);
            GameEvents.onGUIAstronautComplexDespawn.Add(AstronautComplexDespawned);
            Debug.Log("[SPACEBUCKS]: Stock Events Registered");
            FlightTrackerApi.OnFlightTrackerUpdated.Add(HandleRecovery);
            Debug.Log("[SPACEBUCKS]: FlightTracker Events Registered");
            Debug.Log("[SPACEBUCKS]: All Events Successfully Registered");
        }

        private void OnDisable()
        {
            Debug.Log("[SPACEBUCKS]: Unregistering GameEvents");
            GameEvents.OnVesselRollout.Remove(AddLaunch);
            GameEvents.Contract.onOffered.Remove(OnContractOffered);
            GameEvents.OnCrewmemberHired.Remove(CrewManagerAddCrew);            
            GameEvents.onKerbalStatusChanged.Remove(PotentialKerbalDeath);
            GameEvents.onGUIAstronautComplexSpawn.Remove(AstronautComplexSpawned);
            GameEvents.onGUIAstronautComplexDespawn.Remove(AstronautComplexDespawned);
            Debug.Log("[SPACEBUCKS] Unregistered Stock Events");
            FlightTrackerApi.OnFlightTrackerUpdated.Remove(HandleRecovery);
            Debug.Log("[SPACEBUCKS] Unregistered Flight Tracker Event");
            Debug.Log("[SPACEBUCKS]: All Events Successfully Unregistered");
        }

        private void AddLaunch(ShipConstruct ship)
        {
            if (HighLogic.CurrentGame.Mode != Game.Modes.CAREER) return;
            LaunchCosts.Instance.OnVesselRollout(ship);
        }

        private void OnContractOffered(Contract contract)
        {
            if (HighLogic.CurrentGame.Mode != Game.Modes.CAREER) return;
            ContractInterceptor.Instance.OnContractOffered(contract);
        }        

        private void CrewManagerAddCrew(ProtoCrewMember crewMember, int newActiveCrewCount)
        {
            if (HighLogic.CurrentGame.Mode != Game.Modes.CAREER) return;
            CrewManager.Instance.AddNewCrewMember(crewMember);
        }

        private void PotentialKerbalDeath(ProtoCrewMember crewMember, ProtoCrewMember.RosterStatus statusFrom, ProtoCrewMember.RosterStatus statusTo)
        {
            if (HighLogic.CurrentGame.Mode != Game.Modes.CAREER) return;
            if (statusTo == ProtoCrewMember.RosterStatus.Dead)
            {
                CrewManager.Instance.ProcessDeadKerbal(crewMember);
            }
        }

        private void AstronautComplexSpawned()
        {
            if (HighLogic.CurrentGame.Mode != Game.Modes.CAREER) return;
            AstronautComplexOverride.Instance.UpdateCount = 4;
            AstronautComplexOverride.Instance.AstronautComplexSpawned = true;
        }

        private void AstronautComplexDespawned()
        {
            AstronautComplexOverride.Instance.AstronautComplexSpawned = false;
        }

        private void HandleRecovery(ProtoCrewMember crewMember)
        {
            if (HighLogic.CurrentGame.Mode != Game.Modes.CAREER) return;
            CrewManager.Instance.UpdateCrewBonus(crewMember, FlightTrackerApi.Instance.GetLaunchTime(crewMember.name));
            CrewManager.Instance.ExtendRetirement(crewMember, FlightTrackerApi.Instance.GetLaunchTime(crewMember.name));
            CrewManager.Instance.ProcessRetirees();
        }
    }
}
