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

        public static Color MarketColor = Color.FromArgb(205, 85, 85);
        public static Color HouseColor = Color.DimGray;
        public static Color FarmColor = Color.FromArgb(139, 129, 76);
        public static Color GuildColor = Color.FromArgb(75, 0, 129); //4b0082 75 00 129
        public static Color BarracksColor = Color.FromArgb(139, 26, 26);
        public static Color TavernColor = Color.FromArgb(205, 104, 57);
        public static Color FactoryColor = Color.FromArgb(110, 139, 61);
        public static Color StreetColor = Color.Gainsboro;

        #endregion

        #region Citizens
        public static int DyingTime = 1000;
        public static int MaxCitizens = 10;
        public static int MaxHappiness = 100;
        public static int StartHappiness = 80;
        public static int StartHappinessDelta = 15;
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
        public static float UnhappyForWork = 0.002f;
        public static float EnergyMoveCost = 0.0005f;
        public static float EnergyPatrolCost = 0.001f;
        public static float EnergyForRest = 0.004f;
        public static float HappyForRest = 0.0005f;
        public static float UnhappyForNoFood = 10;
        public static float HomeNear = 300;
        public static float MovePrecision = 3;
        public static int NextID = 0;
        public static int TryEatInterval = 1000;

        public static float[] exp = { 200, 250, 350, 500, 700, 950, 1200, 1600, 2000, 2500, 3100, 3800, 4700, 5600, 6600, 7700, 8900, 10200, 11500, 1 };
        public static int MaxLevel = 20;

        #endregion

        #region Behaviours
        public static float CraftsmanWorkCost = 0.005f;
        public static float FarmerWorkCost = 0.005f;
        public static float GuardianWorkCost = 0.004f;
        public static float ThiefWorkCost = 0.002f;
        public static float TraderWorkCost = 0.0015f;

        public static float CraftsmanBagCapacity = 200;
        public static float FarmerBagCapacity = 150;
        public static float GuardianBagCapacity = 100;
        public static float ThiefBagCapacity = 250;
        public static float TraderBagCapacity = 300;


        public static float CraftsmanSpeed = 0.1f;
        public static float FarmerSpeed = 0.1f;
        public static float GuardianSpeed = 0.125f;
        public static float ThiefSpeed = 0.15f;
        public static float TraderSpeed = 0.09f;

        public static float HappyAfterDeath = 0;
        public static float ExpForCraft = 100;
        public static float ExpForWorking = 0.5f;
        public static float ExpForParol = 0.4f;
        public static float LimitHappyInTavern = 95;
        public static float LimitEnergyInTavern = 20;
        public static float LowerBoundHappyToDrink = 30;
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
        private static float tileSize = 32;
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
        public static Dictionary<string, Color> ProfColors = new Dictionary<string, Color>()
        {
            { "trader", Color.FromArgb(245, 125, 125) },
            { "thief", Color.FromArgb(115, 40, 169) },
            { "guardian", Color.FromArgb(179, 66, 66) },
            { "farmer", Color.FromArgb(179, 169, 116) },
            { "craftsman", Color.FromArgb(150, 179, 101) }
        };

        #endregion

        #region Economic
        public static int StartMoney = 1000;
        public static int StartMoneyDelta = 500;
        public static int DrinkInTavernCost = 200;

        #endregion

        #region Log
        public static int MaxMessageLength = 100;

        #endregion
    }
}
