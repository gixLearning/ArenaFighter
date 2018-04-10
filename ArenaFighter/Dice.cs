using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaFighter {
    public class Dice {
        private const int diceSides = 6;
        private Random random;

        public int[] RollDice(int rolls) {
            random = new Random();

            int[] results = new int[rolls];

            for (int i = 0; i < rolls; i++) {
                results[i] = random.Next(1, diceSides + 1);
            }
            return results;
        }

        public int RollDice() {
            random = new Random();

            return random.Next(1, diceSides + 1);
        }
    }
}
