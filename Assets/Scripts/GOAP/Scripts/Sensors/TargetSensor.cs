using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.ALPHA;

namespace GOAP.Scripts
{
    /// <summary>
    ///     TargetSensor detecta objetos del tipo T consultando el ActiveObjectRegistry.
    ///     Permite inyectar la estrategia de selección de target (comando), el agente y la distancia de detección en el constructor.
    /// </summary>
    public class TargetSensor<T> : BaseSensor where T : Component
    {

        private readonly float detectionRadius;

        public event Action<T> OnTargetChanged = delegate { };

        private T currentTarget;
        private Vector3 lastKnownPosition = Vector3.zero;

        public List<T> detectedObjects = new List<T>();
        public T CurrentTarget => currentTarget;
        public Vector3 LastKnownPosition => lastKnownPosition;


        /// <summary>
        ///     Crea un TargetSensor inyectando explícitamente el Transform del agente, el radio de detección y la estrategia de selección.
        /// </summary>
        public TargetSensor(Transform agentTransform, float interval, float detectionRadius): base(agentTransform, interval)
        {
            this.agentTransform = agentTransform ?? throw new ArgumentNullException(nameof(agentTransform));
            this.detectionRadius = detectionRadius;
            OnSensorUpdate += CheckForTarget;
        }
        
        private void CheckForTarget()
        {
            IReadOnlyList<T> objects = ActiveObjectRegistry<T>.ActiveObjects;
            detectedObjects = (from o in objects where o != null let dist = Vector3.Distance(agentTransform.position, o.transform.position) where !(dist > detectionRadius) where o.gameObject != agentTransform.gameObject select o).ToList();
            
            if(currentTarget != null && detectedObjects.Contains(currentTarget))
            {
                lastKnownPosition = currentTarget.transform.position;
            }
        }

        public void SetTarget(T target)
        {
            var newPos = target.transform.position;
            if (currentTarget != null && newPos == lastKnownPosition) return;

            currentTarget = target;
            lastKnownPosition = newPos;
            OnTargetChanged.Invoke(currentTarget);
        }

        public Vector3 TargetPosition => currentTarget ? currentTarget.transform.position : Vector3.zero;
        public bool IsTargetInRange => currentTarget != null;
    }
}
