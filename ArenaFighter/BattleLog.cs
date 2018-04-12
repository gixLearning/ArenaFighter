using System;
using System.Collections.Generic;

namespace ArenaFighter {
    public class BattleLog {
        private List<string> logMessages;

        public BattleLog() {
            logMessages = new List<string>();
        }

        public void AddToLog(string message) {
            logMessages.Add(message);
        }

        public void ShowLog() {
            if(logMessages.Count == 0 ) {
                Console.WriteLine();
                Console.WriteLine("Nothing has been recorded yet.");
            } else {
                Console.WriteLine();
                foreach (string log in logMessages) {
                    Console.WriteLine(log);
                }
            }
        }
    }
}
