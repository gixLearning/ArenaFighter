using ArenaFighter.Characters;
using NameGenerator;
using System;

namespace ArenaFighter.Combat {
    public class Battle {
        private Player player;
        private Dice dice;
        private Generator nameGenerator;
        private BattleLog battleLog;
        private bool opponentIsAlive;

        public bool BattleEnded { get; set; }

        public Battle(Player player, BattleLog battleLog) {
            this.player = player;
            this.battleLog = battleLog;
            dice = new Dice();
            nameGenerator = new Generator();
        }

        private Character CreateOpponent() {
            Character character = new NonPlayerCharacter {
                Name = nameGenerator.RandomName(NamingStyle.Hyborian),
                Health = 100
            };
            character.MaxHealth = character.Health;

            character.RollAttributes();
            return character;
        }

        /// <summary>
        /// Main loop of game.
        /// </summary>
        internal void Play() {
            bool quitPlay = false;
            bool showMenu = true;
            ConsoleKey key;

            do {
                if (showMenu) {
                    DisplayBattleMenu();
                    showMenu = false;
                }

                key = Console.ReadKey(false).Key;

                switch (key) {
                    case (ConsoleKey.Q): {
                        quitPlay = true;
                        break;
                    }
                    case (ConsoleKey.F): {
                        Console.WriteLine("combat");
                        DoCombat();
                        showMenu = true;
                        break;
                    }
                    case (ConsoleKey.C): {
                        DisplayCharacterInfo(player.PlayerCharacter);
                        showMenu = true;
                        break;
                    }
                    case (ConsoleKey.R): {
                        Console.WriteLine($"\n{player.PlayerCharacter.Name} has choosen to quit fighting and retire to live a peaceful life...");
                        player.PlayerCharacter.IsDefeated = true;
                        Console.ReadLine();
                        break;
                    }
                }

                if (key != ConsoleKey.Enter) {
                    Console.WriteLine();
                }

            } while (!quitPlay && !player.PlayerCharacter.IsDefeated);

            BattleEnded = true;
        }

        private void DisplayCharacterInfo(PlayerCharacter playerCharacter) {
            Console.Clear();
            ConsoleColor warning;
            ConsoleColor originalColor = Console.ForegroundColor;

            if (playerCharacter.Health < (playerCharacter.MaxHealth / 2)) {
                warning = ConsoleColor.Red;
            }
            else {
                warning = originalColor;
            }


            Console.WriteLine("### Character stats ###");
            Console.WriteLine($"Name: {playerCharacter.Name}\n" +
                $"Strenght: {playerCharacter.Strenght}\n" +
                $"Defense: {playerCharacter.Defense}");

            Console.ForegroundColor = warning;
            Console.WriteLine($"Current Health: {playerCharacter.Health}");
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine($"Victories: {playerCharacter.Victories}");
            Console.WriteLine($"Earned coins (for player): {player.Coins}");
            Console.WriteLine();
        }

        internal void DoCombat() {
            Console.Clear();
            Character opponent;
            Character pc = player.PlayerCharacter;
            bool characterIsAlive = true;
            bool doCombat = true;
            Round round;

            while (characterIsAlive && doCombat) {
                opponent = CreateOpponent();

                Console.WriteLine($"Your opponent is {opponent.Name}");
                Console.WriteLine("Stats: \n" +
                    $"Strenght: {opponent.Strenght}\n" +
                    $"Defence: {opponent.Defense}");
                opponentIsAlive = true;
                Console.WriteLine("COMMENCE BATTLE!");
                Console.ReadLine();

                do {
                    round = new Round(pc, opponent);
                    round.BeginRound();

                    if (opponent.Health <= 0) {
                        opponentIsAlive = false;
                    }

                    if (pc.Health <= 0) {
                        characterIsAlive = false;
                    }

                    Console.ReadLine();
                } while (opponentIsAlive && characterIsAlive);

                if (!opponentIsAlive) {
                    Console.WriteLine("VICTORIUS!\n");
                    player.AddDefeatedOpponent((NonPlayerCharacter)opponent);
                    player.PlayerCharacter.Victories++;
                    Random r = new Random();

                    int coins = r.Next(1, 5);
                    Console.WriteLine($"You've earned {coins} coins for {pc.Name} victory over {opponent.Name}!");
                    player.Coins += coins;

                    doCombat = false;
                }
                if (!characterIsAlive) {
                    Console.WriteLine("DEFEAT!\n");
                    player.PlayerCharacter.IsDefeated = true;
                }
            }
        }

        private void DisplayBattleMenu() {
            Console.WriteLine("--------------");
            Console.WriteLine("[F] Find new opponent\n" +
                "[C] Check character stats\n" +
                "[R] Retire fighter");
        }
    }
}