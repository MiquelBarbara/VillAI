using UnityEngine;

namespace Systems.ReactSystem
{
    [CreateAssetMenu(menuName = "Data/React/ReactableObject")]
    public class ReactableObject : ScriptableObject
    {
        [SerializeField] private string defaultReaction;
        [SerializeField] private string context;

        public string GetDefaultReaction()
        {
            return defaultReaction;
        }

        public string GetContext()
        {
            return context;
        }
    }
}