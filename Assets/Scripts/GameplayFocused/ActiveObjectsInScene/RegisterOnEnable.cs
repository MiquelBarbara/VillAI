namespace GameplayFocused.ActiveObjectsInScene
{
    using UnityEngine;

    /// <summary>
    /// Base class that automatically registers/unregisters the component instance
    /// in the ActiveObjectRegistry for its own type T.
    /// </summary>
    public abstract class RegisterOnEnable<T> : MonoBehaviour where T : Component
    {
        private T _self;

        protected virtual void Awake()
        {
            _self = this as T;
        }
        
        public abstract void Register<T>() where T : Component;
        public abstract void Unregister<T>() where T : Component;

        protected virtual void OnEnable()
        {
            Register<T>();
        }

        protected virtual void OnDisable()
        {
            Unregister<T>();
        }
        
    }
}