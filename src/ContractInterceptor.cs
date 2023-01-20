using Contracts;
using System.Linq;
using UnityEngine;

namespace Spacebucks
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class ContractInterceptor : MonoBehaviour
    {
        public static ContractInterceptor Instance;

        protected void Awake()
        {
            if (HighLogic.CurrentGame.Mode != Game.Modes.CAREER) Destroy(this);
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
        }

        public void OnContractOffered(Contract contract)
        {
            if (contract.FundsCompletion <= 0) return;

            float advanceRep = (float)contract.FundsAdvance / 10000;
            float failureRep = (float)contract.FundsFailure / 10000;
            float completionRep = (float)contract.FundsCompletion / 10000;
            for (int i = 0; i < contract.AllParameters.Count(); i++)
            {
                ContractParameter p = contract.AllParameters.ElementAt(i);
                completionRep += (float)p.FundsCompletion / 10000;
                p.FundsCompletion = 0;
            }

            contract.ReputationFailure += failureRep;
            contract.ReputationFailure += advanceRep * -1;
            contract.ReputationCompletion += completionRep;
            contract.ReputationCompletion += advanceRep;
            if (contract.ReputationCompletion < 1) contract.ReputationCompletion = 1;

            contract.FundsAdvance = 0;
            contract.FundsCompletion = 0;
            contract.FundsFailure = 0;
        }

        public void OnDisable()
        {
            GameEvents.Contract.onOffered.Remove(OnContractOffered);
        }
    }
}
