using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace townWinForm
{
    public static class Config
    {
        #region Common
        public static int GameSpeed = 1; //control: Track bar
        public static float Zoom = 1; //control: Track bar

        #endregion

        #region Citizens
        public static int MaxCitizens = 50;
        public static int MaxHappiness = 100;
        public static int StartHappiness = 75;
        public static int StartHappinessDelta = 25;
        public static string[] ProfList = new string[] {"craftsman", "farmer", "guardian", "trader", "thief"};
        public static int MaxProfLevel = 10;
        public static int MaxEnergy = 100;
        public static float EnergyForSleep = 1;
        public static float HappyForSleep = 0.5f;
        public static float EnergyLowerBoundToUnhappy = 50;
        public static float UnhappyForWork = 1;
        public static float EnergyMoveCost = 0.2f;
        public static float EnergyForRest = 2;
        public static float HappyForRest = 1;

        #endregion

        #region Behaviours
        public static float CraftsmanWorkCost = 3;
        public static float FarmerWorkCost = 5;
        public static float GuardianWorkCost = 4;
        public static float ThiefWorkCost = 2;
        public static float TraderWorkCost = 1;

        public static float CraftsmanBagCapacity = 200;
        public static float FarmerBagCapacity = 150;
        public static float GuardianBagCapacity = 100;
        public static float ThiefBagCapacity = 250;
        public static float TraderBagCapacity = 300;

        #endregion

        #region Things
        public static float ProductCost = 300;
        public static float ProductCostDelta = 200;
        public static float ProductWeight = 50;
        public static float ProductWeightDelta = 20;

        public static float FoodCost = 200;
        public static float FoodCostDelta = 150;
        public static float FoodWeight = 30;
        public static float FoodWeightDelta = 20;

        #endregion

        #region Town
        public static float TileSize
        {
            get { return tileSize * Zoom; }
        }

        public static int StreetHeight
        {
            get { return minBuildingSize + maxBuildingSize; }
        }

        private static float tileSize = 15;
        public static int minBuildingSize = 6;
        public static int maxBuildingSize = 8;
        public static int TownWidth = 60;
        public static int TownHeight = StreetHeight * 4;

        #endregion

        #region Display
        public static float dx = 0;
        public static float dy = 0;
        public static int FPS = 30;
        public static float ScrollSpeed = 1000;

        #endregion

        #region Economic
        public static int StartMoney = 1000;
        public static int StartMoneyDelta = 500;

        #endregion
    }
}
