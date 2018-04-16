using System;
using System.Diagnostics;
using System.Threading;
using ArenaFighter.Characters;
using ArenaFighter.Enums;

namespace ArenaFighter.Combat {
    internal class Round {
        private BattleLog battleLog;
        private Dice dice;
        private Character npc;
        private Character pc;
        private ArenaModifier arenaModifier;

        public Round(Character pc, Character npc, ArenaModifier arenaModifier, BattleLog battleLog) {
            this.pc = pc;
            this.npc = npc;
            this.arenaModifier = arenaModifier;
            this.battleLog = battleLog;
            dice = new Dice();
        }

        internal void BeginRound() {
            int opponentInitiative = npc.Initiative;
            int playerInitiative = pc.Initiative;
            int playerInitiativeRoll = 0;
            int opponentInitiativeRoll = 0;

            foreach (int result in dice.RollDice(1)) {
                playerInitiativeRoll += result;
            }

            Thread.Sleep(150);

            foreach (int result in dice.RollDice(1)) {
                opponentInitiativeRoll += result;
            }

            playerInitiative += AddModifier(Modifiers.Initiative, pc) + playerInitiativeRoll + arenaModifier.Modifier;
            opponentInitiative += AddModifier(Modifiers.Initiative, npc) + opponentInitiativeRoll + arenaModifier.Modifier;


            Console.WriteLine("--------------");
            Console.WriteLine($"Initiativerolls: {pc.Name}: {playerInitiative} ({pc.Initiative} + {playerInitiativeRoll}) vs {npc.Name}: {opponentInitiative} ({npc.Initiative} + {opponentInitiativeRoll})");

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

        private int AddModifier(Modifiers modifierType, Character character) {
            int modifier = 0;

            EquipmentSlot equipmentSlot = EquipmentSlot.Empty;

            switch (modifierType) {
                case Modifiers.Initiative: {
                    equipmentSlot = EquipmentSlot.Accessory;
                    break;
                }
                case Modifiers.Damage: {
                    equipmentSlot = EquipmentSlot.Hands;
                    break;
                }
                case Modifiers.Defense:
                equipmentSlot = EquipmentSlot.Torso;
                break;
            }

            if (character.GetEquipment.DoesHaveEquipment && equipmentSlot != EquipmentSlot.Empty) {
                modifier += (int)character.GetEquipment.GetEquipInSlot(equipmentSlot)?.Modifier;
            }

            return modifier;
        }

        private void DoDamage(Character attackingCharacter, Character defendingCharacter) {
            int totalDamage = attackingCharacter.Strenght - (defendingCharacter.Defense + AddModifier(Modifiers.Defense, defendingCharacter));
            int modifierDamage = 0;

            modifierDamage += AddModifier(Modifiers.Damage, attackingCharacter);

            totalDamage += modifierDamage;
            totalDamage = Math.Max(0, totalDamage);

            defendingCharacter.Health -= totalDamage;
            defendingCharacter.Health = Math.Max(0, defendingCharacter.Health);

            if(totalDamage == 0) {
                Console.WriteLine($"{attackingCharacter.Name} can't damage {defendingCharacter.Name}...");
                Console.WriteLine($"Maybe {attackingCharacter.Name} should just run away...");
            } else {
                Console.WriteLine($"{defendingCharacter.Name} takes " + totalDamage + $" damage! ({attackingCharacter.Strenght}" + (modifierDamage > 0 ? " + " + modifierDamage : "") + $" - {defendingCharacter.Defense})");
                Console.WriteLine($"{defendingCharacter.Name} has {defendingCharacter.Health} health left!");

                battleLog.AddToLog($"{attackingCharacter.Name} did damage to {defendingCharacter.Name}!");
            }
        }
    }
}
