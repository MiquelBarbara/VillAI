using System;
using EntitiesRelated.Core.Data;
using JetBrains.Annotations;
using NPCs;
using Systems.SaveSystem.Memory.Conversations;
using UnityEngine;
using UnityEngine.Serialization;
using UnityServiceLocator;
using Utilities.ScriptableObjectExtensions;

namespace EntitiesRelated.Core
{
    /// <summary>
    /// Represents a character in the game with animation, data, inventory, currency, and mood memory functionalities.
    /// </summary>
    public class Character : MonoBehaviour
    {
        private ResourceLocator memoryComponent;

        public virtual void Start()
        {
            memoryComponent = GetComponent<ResourceLocator>();
        }
        
        [CanBeNull]
        public TMemory GetMemory<TMemory>() where TMemory : ScriptableObject
        {
            return (TMemory) memoryComponent?.Get<TMemory>();
        }
        
        public ComplexCharacterData GetCharacterData()
        {
            return memoryComponent?.Get<ComplexCharacterData>();
        }
        
        public string GetName()
        {
            return memoryComponent?.Get<ComplexCharacterData>().characterName;
        }
        
        public ItemContainer GetInventory()
        {
            return memoryComponent?.Get<ItemContainer>();
        }
        
        public int GetCurrency()
        {
            return memoryComponent.Get<ItemContainer>().GetCurrency();
        }
        
        public void AddItem(Item item, int amount = 1)
        {
            GetInventory().Add(item, amount);
        }
        
        public void RemoveItem(Item item, int amount = 1)
        {
            GetInventory().Remove(item, amount);
        }
        
        public void EarnMoney(int amount)
        {
            GetInventory().AddCurrency(amount);
        }
        
        public int SpendMoney(int amount)
        {
            GetInventory().RemoveCurrency(amount);
            return amount;
        }
    }
}
