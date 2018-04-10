using System;
using System.Diagnostics;
using System.Threading;
using ArenaFighter.Characters;

namespace ArenaFighter.Combat {
    internal class Round {
        private Character pc;
        private Character npc;
        private Dice dice;

        public Round(Character pc, Character npc) {
            this.pc = pc;
            this.npc = npc;
            dice = new Dice();
        }

        internal void BeginRound() {
            int opponentInitiative = 0;
            int playerInitiative = 0;


            foreach (int result in dice.RollDice(2)) {
                playerInitiative += result;
            }

            Thread.Sleep(150);

            foreach (int result in dice.RollDice(2)) {
                opponentInitiative += result;
            }

            Console.WriteLine("--------------");
            Console.WriteLine($"Initiativerolls: {pc.Name}: {playerInitiative} vs {npc.Name}: {opponentInitiative}");

            if(playerInitiative > opponentInitiative) {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{pc.Name} attacks {npc.Name}!");
                DoDamage(pc, npc);
            }
            else if(opponentInitiative > playerInitiative) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{npc.Name} attacks {pc.Name}!");
                DoDamage(npc, pc);
            }
            else {
                Console.WriteLine("Both combatants are looking for a better oppertunity to strike...");
            }

            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void DoDamage(Character attackingCharacter, Character defendingCharacter) {

            int totalDamage = attackingCharacter.Strenght - defendingCharacter.Defense;
            totalDamage = Math.Max(0, totalDamage);

            defendingCharacter.Health -= totalDamage;
            defendingCharacter.Health = Math.Max(0, defendingCharacter.Health);


            if(totalDamage == 0) {
                Console.WriteLine($"{attackingCharacter.Name} can't damage {defendingCharacter.Name}...");
                Console.WriteLine($"Maybe {attackingCharacter.Name} should just run away...");
            } else {
                Console.WriteLine($"{defendingCharacter.Name} takes " + totalDamage + $" damage! ({attackingCharacter.Strenght} - {defendingCharacter.Defense})");
                Console.WriteLine($"{defendingCharacter.Name} has {defendingCharacter.Health} health left!");
            }
        }
    }
}
