using RegionServer.Model.Effects;
using RegionServer.Model.Effects.Definitions;

namespace RegionServer.Calculators
{
    public class SpellEnvironment : Environment
    {
        public int CharacterSpell { get; set; }
        public int TargetSpell { get; set; }
    }
}
