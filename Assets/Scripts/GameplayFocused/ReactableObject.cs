using System.Collections;
using EntitiesRelated.Core;
using GOAP.Scripts.Configuration.Binding;
using UnityEngine;

namespace UI
{

    public class ReactableObject : MonoBehaviour
    {
        [TextArea(0, 300)]  [SerializeField] private string Description;
        [SerializeField] private CapabilitySO withCapability;

        public string GetDescription() => Description;
        public CapabilitySO GetCapability() => withCapability;

        public void Start()
        {
            ActiveObjectRegistry<ReactableObject>.Register(this);
        }
    }
}