using System;
using UnityEngine;

namespace GOAP.Scripts
{
    public interface ISensor
    {
        void UpdateSensor();
    }

    /// <summary>
    /// BaseSensor encapsula la lógica de actualización periódica mediante un CountdownTimer.
    /// </summary>
    public abstract class BaseSensor : ISensor
    {
        protected Transform agentTransform;
        protected float updateInterval = 1f;
        protected CountdownTimer timer;

        /// <summary>
        /// Evento que se ejecuta cada vez que se actualiza el sensor.
        /// </summary>
        protected event Action OnSensorUpdate = delegate { };

        public BaseSensor(Transform agentTransform, float updateInterval = 1f)
        {
            this.agentTransform = agentTransform;
            timer = new CountdownTimer(updateInterval);
            timer.OnTimerStop += () =>
            {
                OnSensorUpdate.Invoke();
                timer.Start();
            };
            timer.Start();
        }

        public virtual void UpdateSensor()
        {
            timer.Tick(Time.deltaTime);
        }
    }
}