using EntitiesRelated.Core;
using GameplayFocused;
using NPCs;
using Systems.ReactSystem;
using UnityEngine;

public class ReactInteract : MonoBehaviour, IReactable
{
    [SerializeField] private ReactableObject _reactableObject;

    public void React(Character character)
    {
        if (character is Npc npc)
        {
            var npcPosition = npc.transform;
            GameManager.Instance.ReactSystem.ProcessReaction(npcPosition, _reactableObject);
        }
    }
}