using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArenaFighter.Characters;
using ArenaFighter.Combat;

namespace ArenaFighter {
    class Program {

        private static bool charSelectConfirmed = false;
        private static bool gameIsRunning = false;

        static void Main(string[] args) {
            string key;
            Player player = new Player();
            PlayerCharacter pc;

            do {
                Console.WriteLine("Please set a name for your character:");
                key = Console.ReadLine();
                pc = new PlayerCharacter {
                    Name = key
                };

                Console.WriteLine("Figher name:" + pc.Name);
                Console.WriteLine("Stats: \n"
                    + "Strenght:" + pc.Strenght + "\n"
                    + "Defense: " + pc.Defense);

                ConsoleKey response;
                do {
                    Console.Write("Use this character? (y/n)");
                    response = Console.ReadKey(false).Key;
                    if(response != ConsoleKey.Enter) {
                        Console.WriteLine();
                    }
                } while (response != ConsoleKey.Y && response != ConsoleKey.N);
                
                charSelectConfirmed = response == ConsoleKey.Y;
            } while (!charSelectConfirmed);

            Console.ReadLine();
            Console.Clear();
            DoIntro();            
            player.PlayerCharacter = pc;

            Battle battle = new Battle(player);
            Console.ReadLine();
        }

        private static void DoIntro() {
            Blinker("ADVENTURETIME!", 3000, true);
            Debug.WriteLine("Control yeilded");
        }

        /// <summary>
        /// Blink some text on the screen!
        /// </summary>
        /// <param name="text">The text to colorise</param>
        /// <param name="duration">Duration in milliseconds</param>
        /// <param name="clearScreen">Clear the screen between each blink</param>
        private static void Blinker(string text, int duration, bool clearScreen = false) {
            Stopwatch stopwatch = new Stopwatch();
            Random rand = new Random();

            stopwatch.Start();
            while (stopwatch.ElapsedMilliseconds < duration) {
                
                Console.ForegroundColor = (ConsoleColor)(rand.Next(10, 16));
                Console.Write(text);
                Thread.Sleep(200);

                if (clearScreen) {
                    Console.Clear();
                }
            }
            stopwatch.Stop();
        }
    }
}
