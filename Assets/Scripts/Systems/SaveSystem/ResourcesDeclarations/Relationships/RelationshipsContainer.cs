using System.Collections.Generic;
using UnityEngine;
using Utilities.ScriptableObjectExtensions;

namespace Systems.SaveSystem.Memory
{
    [CreateAssetMenu(fileName = "RelationshipsContainer", menuName = "Resources/RelationshipsContainer")]
    public class RelationshipsContainer: ScriptableSave
    {
        [SerializeField] List<Relationships> relationships = new();
        
        public List<Relationships> GetAllRelationships()
        {
            return relationships;
        }
        
        public Relationships GetRelationship(string characterName)
        {
            return relationships.Find(r => r.GetCharacterName() == characterName);
        }
        public void AddRelationship(Relationships relationship)
        {
            relationships.Add(relationship);
        }
        
        public void AddRelationship(string characterName)
        {
            var newRelationship = new Relationships(characterName);
            relationships.Add(newRelationship);
        }
        
        public void AddOrUpdateRelationship(string character, RelationshipMetricData relationship)
        {
            var existing = GetRelationship(character);
            if (existing != null)
            {
                existing.UpdateMetric(relationship);
            }
            else
            {
                AddRelationship(character);
                GetRelationship(character).UpdateMetric(relationship);
            }
        }
        
        public void RemoveRelationship(Relationships relationship)
        {
            relationships.Remove(relationship);
        }
    }
}