using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArenaFighter.Enums;

namespace ArenaFighter.Characters {
    public class Item {

        public string Name { get; }
        public int Modifier { get; }
        public EquipmentSlot OccupiesSlot { get; }

        public Item() {

        }

        public Item(EquipmentSlot equipmentSlot, string name, int modifierValue) {
            OccupiesSlot = equipmentSlot;
            Modifier = modifierValue;
            Name = name;
        }
    }
}
