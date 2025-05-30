using System;
using System.Collections.Generic;
using System.Linq;
using Game.ALPHA;
using UnityEngine;

namespace GOAP.Scripts
{
    public class TargetSensorWithStrategy<T> : BaseSensor where T : Component
    {
        private readonly ITargetSelectionCommand targetSelectionCommand;
        private readonly float detectionRadius;

        public event Action<T> OnTargetChanged = delegate { };

        private T currentTarget;
        private Vector3 lastKnownPosition = Vector3.zero;

        public T CurrentTarget => currentTarget;
        public Vector3 LastKnownPosition => lastKnownPosition;
        public float DetectionRadius => detectionRadius;

        /// <summary>
        ///     Crea un TargetSensor inyectando explícitamente el Transform del agente, el radio de detección y la estrategia de selección.
        /// </summary>
        public TargetSensorWithStrategy(Transform agentTransform, float interval, float detectionRadius, ITargetSelectionCommand targetSelectionCommand): base(agentTransform, interval)
        {
            this.agentTransform = agentTransform ?? throw new ArgumentNullException(nameof(agentTransform));
            this.detectionRadius = detectionRadius;
            this.targetSelectionCommand = targetSelectionCommand ?? throw new ArgumentNullException(nameof(targetSelectionCommand));
            OnSensorUpdate += CheckForTarget;
        }
        
        private void CheckForTarget()
        {
            IReadOnlyList<T> objects = ActiveObjectRegistry<T>.ActiveObjects;
            var withinRadius = (from o in objects where o != null let dist = Vector3.Distance(agentTransform.position, o.transform.position) where !(dist > detectionRadius) where o.gameObject != agentTransform.gameObject select o).ToList();

            T newTarget = targetSelectionCommand.Execute(withinRadius, agentTransform.position);

            if (newTarget != null)
            {
                var newPos = newTarget.transform.position;
                if (currentTarget != null && newPos == lastKnownPosition) return;
                currentTarget = newTarget;
                lastKnownPosition = newPos;
                OnTargetChanged.Invoke(currentTarget);
            }
            else if (currentTarget != null)
            {
                currentTarget = null;
                lastKnownPosition = Vector3.zero;
                OnTargetChanged.Invoke(null);
            }
        }

        public Vector3 TargetPosition => currentTarget ? currentTarget.transform.position : Vector3.zero;
        public bool IsTargetInRange => currentTarget != null;
    }
}