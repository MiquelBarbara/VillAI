using EntitiesRelated.Core;
using Unity.VisualScripting;
using UnityEngine;

namespace UI
{
    public class SignWithBubble: Sign
    {
        public override void Interact(Character character)
        {
            StartCoroutine(UIManager.Instance.ShowBubble( this.transform, dialogueData.line,5f));
        }
    }
}