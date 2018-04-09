using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaFighter.Characters {
    public class Attributes {
        private int strenght;
        private int defense;

        public int Strenght {
            get { return strenght; }
            set { strenght = value; }
        }

        public int Defense {
            get { return defense; }
            set { defense = value; }
        }

        internal void Randomize() {
            Random rand = new Random();
            Strenght = rand.Next(1, 100 + 1);
            Defense = rand.Next(1, 50 + 1);
        }
    }
}
