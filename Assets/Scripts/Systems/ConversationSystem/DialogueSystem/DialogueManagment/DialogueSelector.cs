using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Selects a random dialogue line for a given NPC from pre-defined ScriptableObjects.
/// </summary>
public class DialogueSelector : MonoBehaviour
{
    private const string DialogueFolderPath = "ScriptableObjects/DialogueLines";

    public List<string> GetRandomDialogue(string npcName)
    {
        // Load all DialogueContainer assets in the specified folder
        var dialogueContainers = Resources.LoadAll<DialogueData>(DialogueFolderPath + "/" + npcName);

        var matchingDialogues = new List<DialogueData>();

        foreach (var dialogue in dialogueContainers)
            if (Equals(npcName, dialogue.GetNpcName()))
                matchingDialogues.Add(dialogue);

        if (matchingDialogues.Count == 0) return null;

        // Choose a random DialogueContainer from the matching dialogues
        var randomIndex = Random.Range(0, matchingDialogues.Count);
        var selectedDialogue = matchingDialogues[randomIndex];

        return selectedDialogue.line;
    }
}