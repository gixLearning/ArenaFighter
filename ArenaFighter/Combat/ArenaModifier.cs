namespace ArenaFighter.Combat {
    internal class ArenaModifier {
        public enum Affects {
            All,
            Attacker,
            Defender
        }

        public string AffectDescription { get; set; }
        public string AffectTitle { get; set; }
        public Affects AffectsArena { get; }
        public int Modifier { get; set; }

        public ArenaModifier(Affects affects) {
            AffectsArena = affects;
        }
    }
}