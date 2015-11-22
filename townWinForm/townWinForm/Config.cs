using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace townWinForm
{
    public static class Config
    {
        
        #region Common

        public static int GameSpeed = 1; //control: Track bar

        #endregion

        #region Colors

        public static Color MarketColor = Color.Tomato;
        public static Color HouseColor = Color.DimGray;
        public static Color FarmColor = Color.DarkGoldenrod;
        public static Color GuildColor = Color.Indigo;
        public static Color BarracksColor = Color.Maroon;
        public static Color TavernColor = Color.RosyBrown;
        public static Color FactoryColor = Color.DarkGreen;
        public static Color StreetColor = Color.Gainsboro;

        #endregion

        #region Citizens
        public static int DyingTime = 500;
        public static int MaxCitizens = 50;
        public static int MaxHappiness = 100;
        public static int StartHappiness = 75;
        public static int StartHappinessDelta = 25;
        public static string[] ProfList = new string[] { "craftsman", "farmer", "guardian", "trader", "thief" };
        public static int MaxProfLevel = 10;
        public static int MaxEnergy = 100;
        public static float EnergyForDrink = 0.001f;
        public static float EnergyForSleep = 0.003f;
        public static float HappyForSleep = 0.005f;
        public static float HappyForDrink = 0.007f;
        public static float EnergyLowerBoundToUnhappy = 50;
        public static float UnhappyForWork = 1;
        public static float EnergyMoveCost = 0.001f;
        public static float EnergyForRest = 0.02f;
        public static float HappyForRest = 0.001f;
        public static float UnhappyForNoFood = 10;
        public static float HomeNear = 200;
        public static float MovePrecision = 3;
        public static int NextID = 0;
        public static int TryEatInterval = 1000;

        public static float[] exp = { 200, 500, 900, 1400, 2000, 2700, 3500, 4400, 5400 };

        #endregion

        #region Behaviours
        public static float CraftsmanWorkCost = 0.005f;
        public static float FarmerWorkCost = 0.005f;
        public static float GuardianWorkCost = 0.004f;
        public static float ThiefWorkCost = 0.002f;
        public static float TraderWorkCost = 0.001f;

        public static float CraftsmanBagCapacity = 200;
        public static float FarmerBagCapacity = 150;
        public static float GuardianBagCapacity = 100;
        public static float ThiefBagCapacity = 250;
        public static float TraderBagCapacity = 300;


        private static float MoveK = 1f;
        public static float CraftsmanSpeed = 0.1f / MoveK;
        public static float FarmerSpeed = 0.1f / MoveK;
        public static float GuardianSpeed = 0.125f / MoveK;
        public static float ThiefSpeed = 0.15f / MoveK;
        public static float TraderSpeed = 0.1f / MoveK;
        #endregion

        #region Craft
        public static float ChanceToCraftFood = 1.0f / 150.0f; // 1 time in 5sec(with 30FPS)
        public static float ChanceToCraftProduct = 1.0f / 180.0f; // 1 time in 6sec
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
            get { return tileSize; }
        }
        public static int StreetHeight
        {
            get { return minBuildingSize + maxBuildingSize; }
        }
        public static int TownHeight
        {
            get { return StreetHeight * Blocks; }
        }

        public static int Houses
        {
            get { return MaxCitizens / MaxResidents + 7; }
        }

        public static int Productions
        {
            get { return MaxCitizens / MaxWorkers + 4; }
        }

        public static string[] BuildingTypes = new string[] { "house", "tavern", "barracks", "market", "guild", "farm", "factory" };

        public static int Taverns = 2;
        public static int MaxResidents = 2;
        public static int MaxWorkers = 4;
        public static int ThiefGuildsAmount = 2;
        public static int BarracksAmount = 2;
        public static int Markets = MaxCitizens / 50 + 2;
        private static float tileSize = 28;
        public static int minBuildingSize = 6;
        public static int maxBuildingSize = 8;
        public static int TownWidth = 0;
        public static int Blocks = 0;
        
        public static int BuildingBagCapacity = 1000;
        
        #endregion

        #region Display
        public static float dx = 0;
        public static float dy = 24;
        public static int FPS = 30;
        public static float ScrollSpeed = 1000;

        #endregion

        #region Economic
        public static int StartMoney = 1000;
        public static int StartMoneyDelta = 500;

        #endregion

        #region Log
        public static int MaxMessageLength = 100;

        #endregion
    }
}
