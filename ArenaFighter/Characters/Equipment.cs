using System;
using System.Collections.Generic;
using System.Linq;
using ArenaFighter.Enums;

namespace ArenaFighter.Characters {
    public class Equipment {
        private readonly Dictionary<EquipmentSlot, Item> equipments;

        public Dictionary<EquipmentSlot, Item> GetEquipments => equipments;

        public bool DoesHaveEquipment {
            get { return GetEquipments.Count > 0; }
        }

        public Equipment() {
            equipments = new Dictionary<EquipmentSlot, Item>();
        }

        public Item GetEquipInSlot(EquipmentSlot equipmentSlot) {
            if (GetEquipments.ContainsKey(equipmentSlot)) {
                return GetEquipments[equipmentSlot];
            } else {
                return new Item();
            }
        }

        public void AddEquipment(EquipmentSlot equipmentSlot, Item item) {
            equipments.Add(equipmentSlot, item);
        }
    }
}
