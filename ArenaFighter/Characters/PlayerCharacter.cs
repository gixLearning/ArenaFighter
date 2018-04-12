using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaFighter.Characters {
    public class PlayerCharacter : Character {
        public bool IsDefeated { get; set; }
        public int Victories { get; set; }
    }
}
