using System;
using ComplexServerCommon.Enums;

namespace RegionServer.Model.CharacterDatas
{
    public class EloKeeper : ICharacterData
    {
        private static readonly double MAX_ELO_GAIN = 30;
        private static readonly double ELO_COEFFICIENT = 500;

        public CCharacter Owner { get; set; }
        private double elo = 1200; //starting elo
        private double prevElo;

        public void UpdateElo(double newElo)
        {
            if (newElo <= 0) newElo = elo;
            prevElo = elo;
            elo = newElo;
        }

        public double GetElo()
        {
            return elo;
        }

        public double GetPreviousElo()
        {
            return prevElo;
        }

        public void postFightUpdate(FightWinLossTie result, double myElo, double enemyElo)
        {
            switch (result)
            {
                case (FightWinLossTie.Win):
                    UpdateElo(CalculateElo(myElo, enemyElo, 1, 0));
                    break;
                case (FightWinLossTie.Loss):
                    UpdateElo(CalculateElo(myElo, enemyElo, 1, 1));
                    break;
                case (FightWinLossTie.Tie):
                    UpdateElo(CalculateElo(myElo, enemyElo, 0, 1));
                    break;
            }
        }

        public static double CalculateElo(double currentRating1, double currentRating2, double score1, double score2)
        {
            double E = 0;
            double finalResult1, finalResult2;
            double point1, point2;

            if (score1 != score2)
            {
                if (score1 > score2)
                {
                    E = MAX_ELO_GAIN - Math.Round(1 / (1 + Math.Pow(10, ((currentRating2 - currentRating1) / ELO_COEFFICIENT))) * MAX_ELO_GAIN);
                    finalResult1 = currentRating1 + E;
                    finalResult2 = currentRating2 - E;
                }
                else
                {
                    E = MAX_ELO_GAIN - Math.Round(1 / (1 + Math.Pow(10, ((currentRating1 - currentRating2) / ELO_COEFFICIENT))) * MAX_ELO_GAIN);
                    finalResult1 = currentRating1 - E;
                    finalResult2 = currentRating2 + E;
                }
            }
            else
            {
                if (currentRating1 == currentRating2)
                {
                    finalResult1 = currentRating1;
                    finalResult2 = currentRating2;
                }
                else
                {
                    if (currentRating1 > currentRating2)
                    {
                        E = (MAX_ELO_GAIN - Math.Round(1 / (1 + Math.Pow(10, ((currentRating1 - currentRating2) / ELO_COEFFICIENT))) * MAX_ELO_GAIN)) - (MAX_ELO_GAIN - Math.Round(1 / (1 + Math.Pow(10, ((currentRating2 - currentRating1) / ELO_COEFFICIENT))) * MAX_ELO_GAIN));
                        finalResult1 = currentRating1 - E;
                        finalResult2 = currentRating2 + E;
                    }
                    else
                    {
                        E = (MAX_ELO_GAIN - Math.Round(1 / (1 + Math.Pow(10, ((currentRating2 - currentRating1) / ELO_COEFFICIENT))) * MAX_ELO_GAIN)) - (MAX_ELO_GAIN - Math.Round(1 / (1 + Math.Pow(10, ((currentRating1 - currentRating2) / ELO_COEFFICIENT))) * MAX_ELO_GAIN));
                        finalResult1 = currentRating1 + E;
                        finalResult2 = currentRating2 - E;
                    }
                }
            }
            point1 = finalResult1 - currentRating1;
            point2 = finalResult2 - currentRating2;

            return finalResult1;
        }
    }
}
