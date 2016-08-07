using ComplexServerCommon;
using ComplexServerCommon.Enums;
using ComplexServerCommon.MessageObjects;

namespace RegionServer.Model.CharacterDatas
{
    public class GeneralStats : ICharacterData
    {
        public CCharacter Owner { get; set; }
        public int Experience { get; set; }
        public int NextLevelExperience { get; set; }

        public int Battles { get; set; }
        public int Win { get; set; }
        public int Loss { get; set; }
        public int Tie { get; set; }

        public int Gold { get; set; }
        public int Skulls { get; set; }

        public int InventorySlots { get; set; }

        public int TotalAllocatedStats { get; set; }

        public GeneralStats()
        {
        }

        public static implicit operator GenStatData(GeneralStats gStats)
        {
            return new GenStatData(gStats.Owner.Name, gStats.Experience, gStats.NextLevelExperience, gStats.Battles,
                gStats.Win, gStats.Loss, gStats.Tie, gStats.Gold, gStats.Skulls, gStats.InventorySlots);
        }

        public void postFightUpdate(FightWinLossTie result, Rewards reward)
        {
            AddExperience(reward.Experience);
            Gold += reward.Gold;
            Battles++;
            switch (result)
            {
                case (FightWinLossTie.Win):
                    Win++;
                    break;
                case (FightWinLossTie.Loss):
                    Loss++;
                    break;
                case (FightWinLossTie.Tie):
                    Tie++;
                    break;
            }
        }

        public string SerializeStats()
        {
            var owner = Owner;
            Owner = null;
            //trying to serialize with OWner object causes self referencing errors
            var result = SerializeUtil.Serialize(this);
            Owner = owner;
            return result;
        }

        public void DeserializeStats(string genStats)
        {
            var gStats = SerializeUtil.Deserialize<GeneralStats>(genStats);
            this.Experience = gStats.Experience;
            this.Battles = gStats.Battles;
            this.Win = gStats.Win;
            this.Loss = gStats.Loss;
            this.Tie = gStats.Tie;
            this.Gold = gStats.Gold;
            this.Skulls = gStats.Skulls;
        }

        public void AddExperience(int value)
        {
            Experience += value;
            if (Experience >= NextLevelExperience)
            {
                Owner.LevelUp();
            }
        }
    }
}