using System;
using System.Collections.Generic;
using System.IO;

namespace NameGenerator {
    public class Generator {
        private List<string> nameList;
        private Random random;

        public Generator() {
            nameList = new List<string>();
            random = new Random();
            LoadListOfNames();
        }

        private void LoadListOfNames() {
            using(StreamReader reader = File.OpenText("Resources/namelist.txt")) {
                string line;
                while((line = reader.ReadLine()) != null) {
                    nameList.Add(line);
                }
            }
        }

        public string RandomName(NamingStyle style) {
            //Not many styles here so just return from The One List (to rule them all)
            return nameList[random.Next(0, nameList.Count - 1)];
        }
    }
}
