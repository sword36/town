using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using TownInterfaces;

namespace BehaviourModel
{
    public static class Config
    {
        #region Citizens
        public static int DyingTime = 1000;
        public static int MaxCitizens = 35;
        public static int MaxHappiness = 100;
        public static int StartHappiness = 60;
        public static int StartHappinessDelta = 25;
        public static string[] ProfList = new string[] { "craftsman", "farmer", "guardian", "trader", "thief" };
        public static int MaxProfLevel = 10;
        public static int MaxEnergy = 100;
        public static int StartEnergy = 50;
        public static int StartEnergyDelta = 40;
        public static float EnergyForDrink = 0.001f;
        public static float EnergyForSleep = 0.005f;
        public static float HappyForSleep = 0.0005f;
        public static float HappyForDrink = 0.005f;
        public static float EnergyLowerBoundToUnhappy = 70;
        public static float UnhappyForWork = 0.001f;
        public static float EnergyMoveCost = 0.0005f;
        public static float EnergyPatrolCost = 0.001f;
        public static float EnergyForRest = 0.004f;
        public static float HappyForRest = 0.0005f;
        public static float UnhappyForNoFood = 10;
        public static float HomeNear = 300;
        public static float MovePrecision = 4;
        public static int NextID = 0;
        public static int TryEatInterval = 1000;
        public static float VisionRadius = 200;
        public static int HappyForSelling = 15;
        public static int SellingTime = 500;
        public static float MoneyLimitForSelling = 1000;
        public static float MoneyLimitForBuying = 500;
        public static float ThingsLimitForSelling = 2;

        public static float[] exp = { 200, 250, 350, 500, 700, 950, 1200, 1600, 2000, 2500, 3100, 3800, 4700, 5600, 6600, 7700, 8900, 10200, 11500, 1 };
        public static int MaxLevel = 20;

        #endregion

        #region Behaviours
        public static float CraftsmanWorkCost = 0.005f;
        public static float FarmerWorkCost = 0.005f;
        public static float GuardianWorkCost = 0.004f;
        public static float ThiefWorkCost = 0.002f;
        public static float TraderWorkCost = 0.0005f;

        public static float CraftsmanBagCapacity = 600;
        public static float FarmerBagCapacity = 450;
        public static float GuardianBagCapacity = 300;
        public static float ThiefBagCapacity = 750;
        public static float TraderBagCapacity = 1500;


        public static float CraftsmanSpeed = 0.1f;
        public static float FarmerSpeed = 0.1f;
        public static float GuardianSpeed = 0.125f;
        public static float ThiefSpeed = 0.125f;
        public static float TraderSpeed = 0.09f;

        public static float HappyAfterDeath = 0;
        public static float ExpForCraft = 100;
        public static float ExpForWorking = 0.5f;
        public static float ExpForPatrol = 0.4f;
        public static float LimitHappyInTavern = 95;
        public static float LimitEnergyInTavern = 20;
        public static float LowerBoundHappyToDrink = 30;
        public static int MaxProductForTrader = 2;
        #endregion

        #region Craft
        public static float ChanceToCraftFood = 1.0f / 150.0f; // 1 time in 5sec(with 30FPS)
        public static float ChanceToCraftProduct = 1.0f / 180.0f; // 1 time in 6sec
        #endregion

        #region Things
        public static float ProductCost = 400;
        public static float ProductCostDelta = 200;
        public static float ProductWeight = 50;
        public static float ProductWeightDelta = 20;

        public static float FoodCost = 300;
        public static float FoodCostDelta = 150;
        public static float FoodWeight = 30;
        public static float FoodWeightDelta = 20;

        #endregion

        #region Economic
        public static int StartMoney = 1200;
        public static int StartMoneyDelta = 500;
        public static int DrinkInTavernCost = 200;

        public static int TaxesTimerInterval = 60000;
        #endregion
    }
}
