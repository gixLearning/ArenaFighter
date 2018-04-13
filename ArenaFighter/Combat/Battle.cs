using ArenaFighter.Characters;
using ArenaFighter.Enums;
using NameGenerator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArenaFighter.Combat {
    public class Battle {
        private Player player;
        private Dice dice;
        private Generator nameGenerator;
        private BattleLog battleLog;
        private List<Item> aviableItems;

        private bool opponentIsAlive;
        private bool showHelpFirstTime = true;
        private bool exitShop;
        private readonly ConsoleColor originalColor = Console.ForegroundColor;

        public bool BattleEnded { get; set; }

        public Battle(Player player, BattleLog battleLog) {
            this.player = player;
            player.Coins = 100;

            this.battleLog = battleLog;
            dice = new Dice();
            nameGenerator = new Generator();

            aviableItems = new List<Item> {
                new Item(EquipmentSlot.Hands, "Awesome sword", 5, 6),
                new Item(EquipmentSlot.Accessory, "Ring of Speed", 1, 12),
                new Item(EquipmentSlot.Torso, "Red shirt", -5, 5)
            };
        }

        private Character CreateOpponent() {
            Character character = new NonPlayerCharacter {
                Name = nameGenerator.RandomName(NamingStyle.Hyborian),
                Health = new Dice().RollDice()
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
                    case ConsoleKey.E: {
                        DisplayCharacterEquipment(player.PlayerCharacter);
                        showMenu = true;
                        break;
                    }
                    case ConsoleKey.Q: {
                        quitPlay = true;
                        break;
                    }
                    case ConsoleKey.H: {
                        DoHeal(player.PlayerCharacter);
                        showMenu = true;
                        break;
                    }
                    case ConsoleKey.F: {
                        DoCombat();
                        showMenu = true;
                        break;
                    }
                    case ConsoleKey.C: {
                        DisplayCharacterInfo(player.PlayerCharacter);
                        showMenu = true;
                        break;
                    }
                    case ConsoleKey.S: {
                        DisplayItemShopMenu();
                        break;
                    }
                    case ConsoleKey.L: {
                        ShowBattleLog();
                        showMenu = true;
                        break;
                    }
                    case ConsoleKey.R: {
                        Console.WriteLine($"\n{player.PlayerCharacter.Name} has choosen to quit fighting and retire to live a peaceful life...");
                        player.PlayerCharacter.IsDefeated = true;
                        battleLog.AddToLog($"{player.PlayerCharacter.Name} retired from fighting.");
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

        private void DisplayCharacterEquipment(PlayerCharacter playerCharacter) {
            Console.Clear();
            Console.WriteLine("### Character equipment ###");

            Equipment equipment = playerCharacter.GetEquipment;

            if(!equipment.DoesHaveEquipment) {
                Console.WriteLine($"{playerCharacter.Name} doesn't have any equipment yet...");
                return;
            }

            foreach (KeyValuePair<EquipmentSlot, Item> item in equipment.GetEquipments) {
                Console.WriteLine(item.Key.ToString() + ": " + item.Value.Name);
            }
        }

        private void ShowBattleLog() {
            battleLog.ShowLog();
        }

        private void DoHeal(PlayerCharacter playerCharacter) {
            Console.WriteLine();
            if(player.Coins >= 4) {
                Console.WriteLine($"{playerCharacter.Name} was healed for 4 coins.");

                playerCharacter.Health = playerCharacter.MaxHealth;
                player.Coins -= 4;
            } else {
                Console.WriteLine("You do not have enough coins to pay for healing.");
            }
        }

        private void DisplayCharacterInfo(PlayerCharacter playerCharacter) {
            Console.Clear();
            ConsoleColor textColor;

            if (playerCharacter.Health <= (playerCharacter.MaxHealth / 2)) {
                textColor = ConsoleColor.Red;
            }
            else {
                textColor = originalColor;
            }


            Console.WriteLine("### Character stats ###");
            Console.WriteLine($"Name: {playerCharacter.Name}\n" +
                $"Strenght: {playerCharacter.Strenght}\n" +
                $"Defense: {playerCharacter.Defense}");

            Console.ForegroundColor = textColor;
            Console.WriteLine($"Current Health: {playerCharacter.Health}");
            Console.ForegroundColor = originalColor;

            Console.WriteLine($"Victories: {playerCharacter.Victories}");
            Console.WriteLine($"Coins: {player.Coins}");
            Console.WriteLine();
            Console.ResetColor();
        }

        internal void DoCombat() {
            Console.Clear();
            Character opponent;
            Character pc = player.PlayerCharacter;
            bool characterIsAlive = true;
            bool doCombat = true;
            bool fleeCombat = false;
            Round round;
            ConsoleKey key;

            while (characterIsAlive && doCombat) {
                opponent = CreateOpponent();

                Console.WriteLine($"Your opponent is {opponent.Name}");
                Console.WriteLine("Stats: \n" +
                    $"Health: {opponent.Health}\n" +
                    $"Strenght: {opponent.Strenght}\n" +
                    $"Defence: {opponent.Defense}\n" +
                    $"Initiative: {opponent.Initiative}");
                opponentIsAlive = true;
                Console.WriteLine("COMMENCE BATTLE!");

                if (showHelpFirstTime) {
                    Console.WriteLine("(You can flee a battle by pressing [F])");
                    showHelpFirstTime = false;
                }

                Console.ReadLine();

                battleLog.AddToLog($"A battle begins between {pc.Name} and {opponent.Name}!");

                while (opponentIsAlive && characterIsAlive && !fleeCombat) {
                    round = new Round(pc, opponent, battleLog);
                    round.BeginRound();

                    if (opponent.Health <= 0) {
                        opponentIsAlive = false;
                    }

                    if (pc.Health <= 0) {
                        characterIsAlive = false;
                    }

                    key = Console.ReadKey(true).Key;
                    fleeCombat = key == ConsoleKey.F && opponentIsAlive && characterIsAlive;
                }

                if (fleeCombat) {
                    Console.WriteLine($"{pc.Name} shamefully fled the combat...");
                    battleLog.AddToLog($"{pc.Name} ran away from {opponent.Name}...");
                    doCombat = false;
                }

                if (!opponentIsAlive) {
                    Console.WriteLine("VICTORIUS!\n");
                    player.AddDefeatedOpponent((NonPlayerCharacter)opponent);
                    player.PlayerCharacter.Victories++;
                    Random r = new Random();

                    int coins = r.Next(1, 5);
                    Console.WriteLine($"You've earned {coins} coins for {pc.Name} victory over {opponent.Name}!");
                    player.Coins += coins;

                    battleLog.AddToLog($"{pc.Name} was victorius in battle against {opponent.Name}!");

                    doCombat = false;
                }

                if (!characterIsAlive) {
                    Console.WriteLine("DEFEAT!\n");
                    battleLog.AddToLog($"{pc.Name} was defeated by {opponent.Name} and only withered bones remains now...");

                    player.PlayerCharacter.IsDefeated = true;
                }
            }
        }

        private void DisplayItemShopMenu() {
            Console.Clear();
            Console.WriteLine("### Item Shop ###");

            ListItemShopItems(aviableItems);

            Console.WriteLine();
            Console.WriteLine("[X] Exit shop");

            //ConsoleKey key;
            ConsoleKeyInfo key;
            int itemToBuy;
            Item item;

            do {
                key = Console.ReadKey(true);
                exitShop = key.Key == ConsoleKey.X;
                

                if(Int32.TryParse(key.KeyChar.ToString(), out itemToBuy)) {
                    itemToBuy = itemToBuy - 1;
                    if (aviableItems.Count > itemToBuy && itemToBuy >= 0 && aviableItems[itemToBuy] != null) {
                        item = aviableItems[itemToBuy];

                        if(BuyItem(item, player.PlayerCharacter)) {
                            aviableItems.Remove(item);
                        }
                        ListItemShopItems(aviableItems);
                    }
                }
            } while (!exitShop);

            Console.Clear();
            DisplayBattleMenu();
        }

        private void ListItemShopItems(List<Item> aviableItems) {

            if (aviableItems.Count <= 0) {
                Console.WriteLine("--- Currently no items aviable ---");
            }
            else {
                Console.WriteLine("Aviable Items:");
                for (int i = 0; i < aviableItems.Count; i++) {
                    int x = i + 1;
                    Console.Write($"[{x}] {aviableItems[i].Name} ");
                }
                Console.WriteLine();
            }
        }

        private bool BuyItem(Item item, PlayerCharacter playerCharacter) {
            if(player.Coins >= item.Cost) {
                Console.WriteLine($"{playerCharacter.Name} buys " + item.Name);
                playerCharacter.GetEquipment.Equip(item.OccupiesSlot, item);
                return true;
            } else {
                Console.WriteLine("You can't afford that item");
                return false;
            }
        }

        private void DisplayBattleMenu() {
            Console.WriteLine("--------------");
            Console.WriteLine("[F] Find new opponent\n" +
                "[C] Check character stats\n" +
                "[E] Check character equipment\n" +
                "[H] Heal (4 coins)\n" +
                "[S] Buy equipment\n" +
                "[L] Show the battlelog\n" +
                "[R] Retire fighter");
        }
    }
}