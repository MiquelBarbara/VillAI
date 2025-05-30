using System;
using System.Collections.Generic;
using Blackboard_Architecture;
using Game.ALPHA;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GOAP.Scripts
{
    
    /// <summary>
    ///     The `Sensor` class handles the detection of nearby objects within a specified radius
    ///     and tracks the position of a target (e.g., the player). It uses a `SphereCollider`
    ///     as a trigger to detect objects, and it notifies listeners when the target changes.
    /// </summary>
    public class Sensor<T>  : ISensor where T : Component 
    {
        private List<T> detectedObjects;
        private readonly float detectionRadius = 5f; // The radius of the detection sphere.
        private Vector3 lastKnownPosition; // The last recorded position of the target.

        private GameObject target; // The currently detected target.
        private ITargetSelectionCommand targetSelectionCommand;
        private CountdownTimer timer; // Timer to periodically check the target's position.
        private readonly float timerInterval = 1f; // Interval for periodic target updates.
        
        private Transform transform;
        private BlackboardKey BlackboardKey;
        private Blackboard blackboard;
        
        public T Detectable => target ? target.GetComponent<T>() : null;

        /// <summary>
        ///     Returns the list of detected objects with the specified component type.
        /// </summary>
        public List<T> DetectedObjects => new(detectedObjects);

        /// <summary>
        ///     Gets the position of the current target. Returns `Vector3.zero` if no target is detected.
        /// </summary>
        public Vector3 TargetPosition => target ? target.transform.position : Vector3.zero;

        /// <summary>
        ///     Indicates whether a target is currently in range.
        /// </summary>
        public bool IsTargetInRange => TargetPosition != Vector3.zero;

        /// <summary>
        ///     Event triggered whenever the detected objects change.
        /// </summary>
        public event Action OnDetectedObjectsChanged = delegate { };

        /// <summary>
        ///     Event triggered whenever the detected target changes position or is lost.
        /// </summary>
        public event Action OnTargetChanged = delegate { };


        /// <summary>
        ///     Starts the timer for periodic updates to the target's position.
        /// </summary>
        public void Init(Transform agentTransform)
        {
            this.transform = agentTransform;
            detectedObjects = new List<T>(Object.FindObjectsOfType<T>());
            timer = new CountdownTimer(timerInterval);
            timer.OnTimerStop += () =>
            {
                var detectable = SelectTarget(transform.position);
                UpdateTargetPosition(detectable?.gameObject);

                timer.Start();
            };
            timer.Start();
        }
        
        /// <summary>
        ///     Updates the timer on each frame.
        /// </summary>
        public void UpdateSensor()
        {
            timer.Tick(Time.deltaTime);
        }

        public void SetTargetSelectionCommand(ITargetSelectionCommand command)
        {
            targetSelectionCommand = command;
        }
        public void WithBlackboard(Blackboard blackboard) => this.blackboard = blackboard;
        public void WithBlackboardKey(BlackboardKey key) => BlackboardKey = key;

        /// <summary>
        ///     Updates the current target and notifies listeners if the target's position changes.
        /// </summary>
        /// <param name="target">The new target to track, or `null` if no target is present.</param>
        private void UpdateTargetPosition(GameObject target = null)
        {
            this.target = target;
            if (IsTargetInRange && (lastKnownPosition != TargetPosition || lastKnownPosition != Vector3.zero))
            {
                lastKnownPosition = TargetPosition;
                OnTargetChanged.Invoke();
                blackboard.SetValue(BlackboardKey, TargetPosition);
            }
        }

        private T SelectTarget(Vector3 agentPosition)
        {
            return targetSelectionCommand?.Execute<T>(detectedObjects, agentPosition);
        }

        /// <summary>
        ///     Draws a gizmo in the editor to visualize the detection radius.
        ///     The gizmo changes color based on whether a target is in range.
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = IsTargetInRange ? Color.red : Color.green;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}