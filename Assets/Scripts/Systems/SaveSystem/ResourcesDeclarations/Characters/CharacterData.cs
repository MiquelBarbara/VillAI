using Newtonsoft.Json;
using UnityEngine;

namespace EntitiesRelated.Core.Data
{
    /// <summary>
    /// ScriptableObject representing basic character data, including name and icon.
    /// </summary>
    [CreateAssetMenu(menuName = "Resources/Characters/CharacterData")]
    [AllowAsParameter]
    public class CharacterData : ScriptableObject
    {
        /// <summary>
        /// The name of the character.
        /// </summary>
        public string characterName;

        /// <summary>
        /// The sprite icon representing the character.
        /// </summary>
        [JsonIgnore] 
        public Sprite icon;
        
        public string GetCharacterName() => characterName;
        public Sprite GetSprite() => icon;
    }
}