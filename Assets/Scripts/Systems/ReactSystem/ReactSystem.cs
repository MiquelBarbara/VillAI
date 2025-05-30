using System.Collections;
using System.Collections.Generic;
using LLM;
using UnityEngine;

namespace Systems.ReactSystem
{
    public class ReactSystem : MonoBehaviour
    {
        //[SerializeField] private FloatingTextSystem writer;
        private PythonClient _pythonClient;

        private void Start()
        {
            _pythonClient = PythonClient.Instance;
        }

        public void ProcessReaction(Transform npcPosition, ReactableObject reactableObject)
        {
            var newList = new List<string>();

            var reactionParameters = new Dictionary<string, object>
            {
                { "description", reactableObject.GetContext() }
            };

            
            //var bubble = new TextBubble(npcPosition, newList);

            //writer.Initialize(bubble);
        }
    }
}