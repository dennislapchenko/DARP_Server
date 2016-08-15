using RegionServer.Model;
using RegionServer.Model.Effects;
using RegionServer.Model.Effects.Definitions;

namespace RegionServer.Calculators
{
    public class SpellEnvironment : Environment
    {
        public byte CharacterSpell { get; set; }
        public byte TargetSpell { get; set; }

        public SpellEnvironment(CCharacter player, byte playerSpell, CCharacter target, byte targetSpell)
        {
            Character = player;
            CharacterSpell = playerSpell;
            Target = target;
            TargetSpell = targetSpell;
        }
    }
}
