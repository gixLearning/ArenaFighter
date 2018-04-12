using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaFighter {
    public class Dice {
        private readonly int diceSides;
        private readonly Random random;

        public Dice(int diceSides = 6) {
            this.diceSides = diceSides;
            random = new Random();
        }

        public int[] RollDice(int rolls) {
            int[] results = new int[rolls];

            for (int i = 0; i < rolls; i++) {
                results[i] = random.Next(1, diceSides + 1);
            }
            return results;
        }

        public int RollDice() {
            return random.Next(1, diceSides + 1);
        }
    }
}
