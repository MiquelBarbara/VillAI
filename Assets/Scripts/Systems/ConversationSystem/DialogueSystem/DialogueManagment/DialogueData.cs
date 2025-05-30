using System.Collections.Generic;
using EntitiesRelated.Core.Data;
using NPCs;
using UnityEngine;

/// <summary>
/// ScriptableObject holding NPC dialogue lines and character data.
/// </summary>
[CreateAssetMenu(menuName = "Data/Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    public List<string> line;
    public CharacterData characterData;

    public string GetNpcName()
    {
        return characterData.characterName;
    }

    public Sprite GetNpcSprite()
    {
        if (characterData == null || characterData.icon == null) return null;

        return characterData.icon;
    }
}