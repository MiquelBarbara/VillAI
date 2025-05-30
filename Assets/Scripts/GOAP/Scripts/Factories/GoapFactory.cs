using Game.Utilities.DependencyInjection;
using UnityEngine;

namespace GOAP.Scripts
{
    public class GoapFactory : MonoBehaviour, IDependencyProvider
    {
        [Provide]
        public GoapFactory ProvideFactory()
        {
            return this;
        }

        public IGoapPlanner CreatePlanner(bool poweredByAI = false)
        {
            return new GoapPlanner();
        }
    }
}