using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Utilities.ScriptableObjectExtensions;

namespace Systems.SaveSystem.Memory
{
    
    public interface ISummarizable
    {
        public void ResourceLocatorSubscribe(ResourceLocator resourceLocator);
    }
    public abstract class Summarizable<T> : ScriptableSave, ISummarizable
    {
        [SerializeField] [JsonIgnore] int maxEntries = 10;
        [SerializeField] public string summary = "";
        [SerializeField] public List<T> entries = new();
        public event Action<Summarizable<T>> OnNeedSummary;
        public void RequestSummary()
        {
            OnNeedSummary?.Invoke(this);
        }
        
        public void ResourceLocatorSubscribe(ResourceLocator resourceLocator)
        {
            OnNeedSummary += resourceLocator.HandleResourceSummary;
        }

        public Type GetDataType() 
        {
            return typeof(T);
        }

        public virtual void AddEntry(T entry)
        {
            if (entry == null) return;
            entries.Add(entry);
            if (NeedsSummaryUpdate())
            {
                RequestSummary();
            }
        }
        
        public T GetEntry(int index)
        {
            if (index < 0 || index >= entries.Count)
            {
                throw new System.IndexOutOfRangeException("Index out of range");
            }
            return entries[index];
        }
        
        public string GetSummary()
        {
            return summary;
        }

        public void SetSummary(string summary)
        {
            this.summary = summary;
            
            int entriesToRemove = entries.Count / 2;
            if (entriesToRemove > 0)
            {
                entries.RemoveRange(0, entriesToRemove);
            }
        }
        
        public bool NeedsSummaryUpdate()
        {
            return maxEntries > 0 && entries.Count > maxEntries;
        }
        
        public T GetLastEntry()
        {
            if (entries.Count == 0)
            {
                throw new InvalidOperationException("No entries available.");
            }
            return entries[^1];
        }
    }
}