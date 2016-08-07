namespace RegionServer.Model.Constants
{
    public class ExperienceConstants
    {
        public const int LEVEL_0 = 0;
        public const int LEVEL_1 = 200;
        public const int LEVEL_2 = 800;
        public const int LEVEL_3 = 1700;
        public const int LEVEL_4 = 4200;
        public const int LEVEL_5 = 9100;
        public const int LEVEL_6 = 19000;
        public const int LEVEL_7 = 39200;
        public const int LEVEL_8 = 88000;
        public const int LEVEL_9 = 180000;
        public const int LEVEL_10 = 410000;
        public const int LEVEL_11 = 915000;
        public const int LEVEL_12 = 2100000;
        public const int LEVEL_13 = 4500000;
        public const int LEVEL_14 = 10450000;
        public const int LEVEL_15 = 24100000;
        public const int LEVEL_16 = 55000000;

        public const int MAX_LEVEL = 16;

        //TODO: this is utter garbage.
        public static int getExpForLevel(int level)
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
