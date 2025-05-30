using UnityEngine;

namespace Blackboard_Architecture
{
    /// <summary>
    /// Manages the Blackboard instance and initializes it using BlackboardData.
    /// </summary>
    public class BlackboardController : MonoBehaviour
    {
        [SerializeField] private BlackboardData blackboardData;
        private readonly Blackboard _blackboard = new();

        /// <summary>
        /// Initializes the Blackboard by setting values from BlackboardData and logging the entries.
        /// </summary>
        private void Awake()
        {
            blackboardData.SetValuesOnBlackboard(_blackboard);
            _blackboard.Debug();
        }
        
        /// <summary>
        /// Retrieves the current Blackboard instance.
        /// </summary>
        /// <returns>The <see cref="Blackboard"/> instance.</returns>
        public Blackboard GetBlackboard()
        {
            return _blackboard;
        }
    }
}