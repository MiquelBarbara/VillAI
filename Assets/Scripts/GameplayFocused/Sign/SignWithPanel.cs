using EntitiesRelated.Core;

namespace UI
{
    public class SignWithPanel: Sign
    {
        public override void Interact(Character character)
        {
            StartCoroutine(UIManager.Instance.ShowPanel(dialogueData.line));
        }
    }
}