using Core.Items;
using System;
using UnityEngine;

namespace Core.Character
{
    [Serializable]
    public class ItemSlot
    {
        [field: SerializeField] public UsebleReference Reference { get; private set; }
        [field: SerializeField] public ItemType AcceptebleItemType { get; private set; }

        public Useble Item 
        {
            get 
            {
                return Reference.Value;
            }
            set 
            {
                if (value.ItemType != AcceptebleItemType) 
                {
                    throw new Exception("Wrong item type");
                }

                Reference.Value = value;
            }
        }
    }
}
