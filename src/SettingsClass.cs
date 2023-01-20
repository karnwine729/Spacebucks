namespace Spacebucks
{
    public class SettingsClass
    {
        public static SettingsClass Instance;

        public float FundingPeriodDays = 73.0f;
        public int FundingMultiplier = 5419;
        public float StartingRepDecay = 0.20f;
        public int CostAdmin = 4000;				// 5%
        public int CostAstronautComplex = 4800;		// 6%
        public int CostMissionControl = 6400;		// 8%
        public int CostRnd = 24000;                 // 30%
        public int CostSph = 14400;					// 18%
        public int CostTrackingStation = 8000;		// 10%
        public int CostVab = 18400;					// 23%
        public int CostVabLaunch = 10;
        public int CostSphLaunch = 2;
        public int BaseAstronautWage = 500;
        public int MinimumTermYears = 20;
        public int MaximumTermYears = 40;
        public double RetirementExtensionFactor = 0.33f;
        public int TrainingLengthDays = 180;
        public int DeathPenaltyPercent = 30;
        public float RandomEventChance = 0.15f;
        public int RandomEventCooldownDays = 30;

        public SettingsClass()
        {
            Instance = this;
        }
    }
}