using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities.ScriptableObjectExtensions;

[Serializable]
public class DialogueBagData
{
    [SerializeField] private List<string> dialogueList;
    [SerializeField] private Mood mood;
}

namespace Systems.SaveSystem.Memory
{
    [CreateAssetMenu(fileName = "DialogueBag", menuName = "Resources/DialogueBag")]
    public class DialogueBag: ScriptableSave
    {
        [SerializeField] List<DialogueBagData> dialogueList = new();
        public DialogueBagData GetRandomDialogue()
        {
            if (dialogueList.Count == 0) return null;
            int randomIndex = UnityEngine.Random.Range(0, dialogueList.Count);
            return dialogueList[randomIndex];
        }
    }
}