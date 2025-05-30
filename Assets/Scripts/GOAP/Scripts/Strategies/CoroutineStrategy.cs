using System.Collections;
using UnityEngine;

namespace GOAP.Scripts
{
    public abstract class CoroutineStrategy: IActionStrategy
    {
        protected MonoBehaviour provider;
        protected bool _coroutineDone = false;

        public CoroutineStrategy(MonoBehaviour provider)
        {
            this.provider = provider;
        }
        
        public CoroutineStrategy(GameObject provider)
        {
            this.provider = provider.GetComponent<GoapAgent>();
        }
        
        public virtual void Start()
        {
            Debug.Log($"Starting coroutine for {provider.name} with strategy {GetType().Name}");
            provider.StartCoroutine(Predict());
        }
        protected abstract IEnumerator Predict();
    
        public virtual bool CanPerform => !Complete;
        public virtual bool Complete => _coroutineDone;
    }
}