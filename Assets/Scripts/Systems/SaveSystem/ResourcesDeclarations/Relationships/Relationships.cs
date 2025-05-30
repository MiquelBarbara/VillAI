using System;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.SaveSystem.Memory
{
    [AllowAsParameter]
    public enum RelationshipMetric
    {
        Trust,
        Respect,
        Affinity,
        Compatibility,
        ConflictLevel,
        Quality
    }
    
    [AllowAsParameter] [Serializable]
    public class RelationshipMetricData
    {
        [SerializeField] RelationshipMetric Metric;
        [SerializeField] float Value;
        [SerializeField] string Reason;
        
        public RelationshipMetricData(RelationshipMetric metric, float value, string reason)
        {
            Metric = metric;
            Value = value;
            Reason = reason;
        }
        
        public RelationshipMetric GetMetric() => Metric;
        public float GetValue() => Value;
        public string GetReason() => Reason;
        
        public void SetMetric(RelationshipMetric metric) => Metric = metric;
        public void SetValue(float value) => Value = value;
        public void AddValue(float value) => Value += value;
        public void SubtractValue(float value) => Value -= value;
        public void SetReason(string reason) => Reason = reason;
    }

    [Serializable]
    public class Relationships
    {
        [SerializeField] string characterName;
        [SerializeField] private List<RelationshipMetricData> relationshipMetrics = new();
        [SerializeField]  string RelationshipContext;
        [SerializeField] string PerceptionOfOther;
        
        public Relationships(string characterName, string relationshipContext = "", string perceptionOfOther = "")
        {
            this.characterName = characterName;
            RelationshipContext = relationshipContext;
            PerceptionOfOther = perceptionOfOther;
        }
        
        public string GetCharacterName() => characterName;
        
        public void UpdateMetric(RelationshipMetric metric, float change, string reason)
        {
            var existingMetric = relationshipMetrics.Find(m => m.GetMetric() == metric);
            if(existingMetric != null)
            {
                existingMetric.AddValue(change);
                existingMetric.SetReason(reason);
            }
            else
            {
                relationshipMetrics.Add(new RelationshipMetricData(metric, change, reason));
            }
        }
        
        public void UpdateMetric(RelationshipMetricData metricData)
        {
            UpdateMetric(metricData.GetMetric(), metricData.GetValue(), metricData.GetReason());
        }
    }
}