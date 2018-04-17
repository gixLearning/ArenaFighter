using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaFighter.Characters {
    public class Player {
        private List<Character> defeatedOpponents;
        private List<Character> defeatedCharacters;

        public PlayerCharacter PlayerCharacter { get; set; }
        public int Coins { get; set; }
        public int DefeatedOpponents { get { return defeatedOpponents.Count; } }

        public Player() {
            defeatedOpponents = new List<Character>();
            defeatedCharacters = new List<Character>();
        }

        public void AddDefeatedOpponent(NonPlayerCharacter character) {
            defeatedOpponents.Add(character);
        }

        public void AddDefeatedCharacters(PlayerCharacter character) {
            defeatedCharacters.Add(character);
        }
    }
}
