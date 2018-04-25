﻿using ArenaFighter.Characters;
using ArenaFighter.Combat;
using System;
using System.Collections.Generic;

namespace ArenaFighter {

    internal class AdventureMap {
        private Player player;
        private Battle battle;
        private char[,] charMap;
        private char originChar;
        private char destinationChar;
        private char ch = '♦';
        private char shop = '\u2302';
        private char evilChar = '\u25BC';
        private List<string> menuStrings = new List<string>();

        //private string ch = "♥";
        private bool exit;

        public AdventureMap(Player player, Battle battle) {
            this.player = player;
            this.battle = battle;
            charMap = LoadMap();

            menuStrings.Add("╔══════════════════════════════════╗");
            menuStrings.Add("║ [C] Check character stats        ║");
            menuStrings.Add("║ [E] Check character equipment    ║");
            menuStrings.Add("║ [H] Heal (4 coins)               ║");
            menuStrings.Add("║ [L] Show the battlelog           ║");
            menuStrings.Add("║ [R] Retire fighter               ║");
            menuStrings.Add("║ [X] Exit menus/game              ║");
            menuStrings.Add("╚══════════════════════════════════╝");
        }

        private char[,] LoadMap() {
            char[,] charMap = new char[Console.WindowWidth, Console.WindowHeight];

            for (int i = 0; i < Console.WindowWidth; i++) {
                for (int j = 0; j < Console.WindowHeight; j++) {
                    char currentChar = ' ';

                    if ((i == 0) || (i == Console.WindowWidth - 1)) {
                        currentChar = '║';
                    }
                    else {
                        if ((j == 0) || (j == Console.WindowHeight - 1)) {
                            currentChar = '═';
                        }
                    }

                    charMap[i, j] = currentChar;
                }
            }

            charMap[0, 0] = '╔';
            charMap[Console.WindowWidth - 1, 0] = '╗';
            charMap[0, Console.WindowHeight - 1] = '╚';
            charMap[Console.WindowWidth - 1, Console.WindowHeight - 1] = '╝';

            charMap[10, 10] = shop;

            return charMap;
        }

        internal void Play() {
            Coord coord = new Coord {
                X = 1,
                Y = 1
            };

            ConsoleKeyInfo keyInfo;

            DrawMap(charMap);
            DrawEvilThings(charMap);

            Console.SetCursorPosition(coord.X, coord.Y);
            DrawCoord(coord, charMap);

            do {
                keyInfo = Console.ReadKey(true);

                //Draw the char, reset the coord if it fails/is out of bounds
                switch (keyInfo.Key) {
                    case ConsoleKey.DownArrow: {
                        coord.Y += 1;
                        if (!DrawCoord(coord, charMap)) {
                            coord.Y -= 1;
                        }
                        break;
                    }
                    case ConsoleKey.UpArrow: {
                        coord.Y -= 1;
                        if (!DrawCoord(coord, charMap)) {
                            coord.Y += 1;
                        }
                        break;
                    }
                    case ConsoleKey.RightArrow: {
                        coord.X += 1;
                        if (!DrawCoord(coord, charMap)) {
                            coord.X -= 1;
                        }
                        break;
                    }
                    case ConsoleKey.LeftArrow: {
                        coord.X -= 1;
                        if (!DrawCoord(coord, charMap)) {
                            coord.X += 1;
                        }
                        break;
                    }
                    case ConsoleKey.C: {
                        battle.DisplayCharacterInfo(player.PlayerCharacter);
                        DrawMap(charMap);
                        DrawCoord(coord, charMap);
                        break;
                    }
                    case ConsoleKey.E: {
                        battle.DisplayCharacterEquipment(player.PlayerCharacter);
                        DrawMap(charMap);
                        DrawCoord(coord, charMap);
                        break;
                    }

                    case ConsoleKey.H: {
                        DisplayHelpMenu();
                        keyInfo = Console.ReadKey(true);
                        DrawMap(charMap);
                        DrawCoord(coord, charMap);

                        //Avoid getting exit game when pressing x in helpmenu
                        continue;
                    }
                }

                CheckForEvilThings(coord, charMap, player);
                CheckForNiceThings(coord, charMap, player);

                exit = keyInfo.Key == ConsoleKey.X;
                if (exit) {
                    Console.WriteLine("Exit game? (Press X again to quit)");
                    keyInfo = Console.ReadKey(true);
                    exit = keyInfo.Key == ConsoleKey.X;
                    if (!exit) {
                        DrawMap(charMap);
                        DrawCoord(coord, charMap);
                    }
                }
            } while (!exit);
            battle.BattleEnded = true;
        }
        private void DisplayHelpMenu() {
            int currentX = Console.CursorLeft;
            int currentY = Console.CursorTop;

            for (int i = 0; i < menuStrings.Count; i++) {
                Console.WriteLine(menuStrings[i]);
                Console.SetCursorPosition(currentX, currentY + i + 1);
            }
        }

        private void CheckForEvilThings(Coord coord, char[,] charMap, Player player) {
            int oldPosX = Console.CursorLeft;
            int oldPosY = Console.CursorTop;

            if (charMap[coord.X, coord.Y] == evilChar) {
                battle.DoCombat();

                charMap[coord.X, coord.Y] = ' ';
                DrawMap(charMap);

                Console.SetCursorPosition(oldPosX, oldPosY);
                Console.Write(ch);
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            }
        }

        private void CheckForNiceThings(Coord coord, char[,] charMap, Player player) {
            int oldPosX = Console.CursorLeft;
            int oldPosY = Console.CursorTop;

            if (charMap[coord.X, coord.Y] == shop) {
                battle.DisplayItemShopMenu();
                DrawMap(charMap);

                Console.SetCursorPosition(oldPosX, oldPosY);
                Console.Write(ch);
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            }
        }

        /// <summary>
        /// Draw the char by the coord and check if the coord is not out of bounds.
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="charmap"></param>
        /// <returns>True if it's inside of bounds, false otherwise</returns>
        private bool DrawCoord(Coord coord, char[,] charmap) {
            if (coord.X - 1 < 0 || coord.Y - 1 < 0 || coord.X >= Console.WindowWidth - 1 || coord.Y >= Console.WindowHeight - 1) {
                return false;
            }

            originChar = charmap[Console.CursorLeft, Console.CursorTop];
            destinationChar = charmap[coord.X, coord.Y];

            Console.Write(originChar);
            Console.SetCursorPosition(coord.X, coord.Y);
            Console.Write(ch);

            originChar = destinationChar;

            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            return true;
        }

        private void DrawMap(char[,] map) {
            int maxX = map.GetLength(0);
            int maxY = map.GetLength(1);

            for (int y = 0; y < maxY; y++) {
                string line = string.Empty;
                for (int x = 0; x < maxX; x++) {
                    line += map[x, y];
                }
                Console.SetCursorPosition(0, y);
                Console.Write(line);
            }
            Console.SetCursorPosition(0, 0);
        }

        private void DrawEvilThings(char[,] charMap) {
            Random random = new Random();

            char evilChar = '\u25BC';

            int xMax = charMap.GetLength(0);
            int yMax = charMap.GetLength(1);

            for (int i = 0; i < 3; i++) {
                int x = random.Next(0, xMax);
                int y = random.Next(0, yMax);

                charMap[x, y] = evilChar;
                Console.SetCursorPosition(x, y);
                Console.WriteLine(evilChar);
            }
        }
    }

    public class Coord {
        public int X { get; set; }
        public int Y { get; set; }
    }
}