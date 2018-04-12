using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaFighter.Characters {
    public class Attributes {
        private int strenght;
        private int defense;
        private int initiative;

        public int Initiative {
            get { return initiative; }
            set { initiative = value; }
        }

        public int Strenght {
            get { return strenght; }
            set { strenght = value; }
        }

        public int Defense {
            get { return defense; }
            set { defense = value; }
        }

        internal void Roll() {
            Dice dice = new Dice();
            Strenght = dice.RollDice();
            Defense = dice.RollDice();
            Initiative = dice.RollDice();
        }
    }
}
