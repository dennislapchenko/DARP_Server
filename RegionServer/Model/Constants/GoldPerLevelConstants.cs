namespace RegionServer.Model.Constants
{
    public class GoldPerLevelConstants
    {
        public const int LEVEL_0 = 0;
        public const int LEVEL_1 = 15;
        public const int LEVEL_2 = 50;
        public const int LEVEL_3 = 100;
        public const int LEVEL_4 = 200;
        public const int LEVEL_5 = 400;
        public const int LEVEL_6 = 800;
        public const int LEVEL_7 = 1600;
        public const int LEVEL_8 = 3200;
        public const int LEVEL_9 = 6400;
        public const int LEVEL_10 = 9900;
        public const int LEVEL_11 = 9900;
        public const int LEVEL_12 = 9900;
        public const int LEVEL_13 = 9900;
        public const int LEVEL_14 = 9900;
        public const int LEVEL_15 = 9900;
        public const int LEVEL_16 = 9900;

        //TODO: this is utter garbage.
        public static int getGoldForLevel(int level)
        {
            switch (level)
            {
                case (1):
                    return LEVEL_1;
                case (2):
                    return LEVEL_2;
                case (3):
                    return LEVEL_3;
                case (4):
                    return LEVEL_4;
                case (5):
                    return LEVEL_5;
                case (6):
                    return LEVEL_6;
                case (7):
                    return LEVEL_7;
                case (8):
                    return LEVEL_8;
                case (9):
                    return LEVEL_9;
                case (10):
                    return LEVEL_10;
                case (11):
                    return LEVEL_11;
                case (12):
                    return LEVEL_12;
                case (13):
                    return LEVEL_13;
                case (14):
                    return LEVEL_14;
                case (15):
                    return LEVEL_15;
                case (16):
                    return LEVEL_16;
                default:
                    return -3000;
            }
        }
    }
}
