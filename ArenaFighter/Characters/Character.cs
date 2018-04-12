using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaFighter.Characters {
    public abstract class Character {
        private readonly Attributes attributes;

        public string Name { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Strenght { get { return attributes.Strenght; } }
        public int Defense { get { return attributes.Defense; } }
        public int Initiative { get { return attributes.Initiative; } }
        public Equipment GetEquipment { get; }

        protected Character() {
            attributes = new Attributes();
            GetEquipment = new Equipment();
        }

        public void RollAttributes() {
            attributes.Roll();
        }
    }
}