using System.Collections.Generic;
using EntitiesRelated.Core.Data;
using LLM.Templates.Stories;
using UnityEngine;

/// <summary>
/// Enumerates various personality traits that a character can possess.
/// </summary>
public enum PersonalityTrait
{
    Kind,
    Sarcastic,
    Aggressive,
    Shy,
    Loyal,
    Brave,
    Curious,
    Optimistic,
    Pessimistic,
    Charismatic,
    Stubborn,
    Intelligent,
    Cunning,
    Selfish,
    Generous,
    Ambitious,
    Humble,
    Arrogant,
    Empathic,
    Reckless,
    Introvert,
    Extrovert
}

/// <summary>
/// Enumerates the possible speech styles a character can use.
/// </summary>
public enum SpeechStyle
{
    Formal,
    Casual,
    Slang,
    Humorous
}

/// <summary>
/// Enumerates gender options for a character.
/// </summary>
public enum Gender
{
    Male,
    Female,
    Other
}

namespace NPCs
{
    /// <summary>
    /// Represents complex character data for an NPC, including personality, appearance, background, and speech style.
    /// </summary>
    [Story("[characterName] is a [age] year-old [gender] who looks like [appearance]. " +
           "That usually speaks in a [speechStyle] manner and has this distinctive personality traits [personalityTraits] " +
           "Has the following motivations: [motivations]. " +
           "The backstory is: [background]." +
           "Other info: [otherInfo]")]
    [CreateAssetMenu(menuName = "Resources/Characters/ComplexCharacterData")]
    [AllowAsParameter]
    public class ComplexCharacterData : CharacterData
    {
        
        /// <summary>
        /// The age of the character.
        /// </summary>
        public int age;
        
        /// <summary>
        /// The gender of the character.
        /// </summary>
        public Gender gender;
        
        /// <summary>
        /// The birthdate of the character.
        /// </summary>
        public string birthdate;

        /// <summary>
        /// A description of the character's appearance.
        /// </summary>
        public string appearance;
        
        /// <summary>
        /// A list of personality traits defining the character.
        /// </summary>
        public List<PersonalityTrait> personalityTraits;
        
        /// <summary>
        /// The motivations of the character.
        /// </summary>
        [TextArea(0, 100)]
        public string motivations;
        
        /// <summary>
        /// The background or backstory of the character.
        /// </summary>
        [TextArea(0, 100)]
        public string background;

        /// <summary>
        /// The speech style the character uses.
        /// </summary>
        public SpeechStyle speechStyle;

        /// <summary>
        /// Additional information about the character.
        /// </summary>
        [TextArea(0, 100)]
        public string otherInfo;

        /// <summary>
        /// Gets the current mood of the character.
        /// </summary>
        public Mood CurrentMood { get; private set; }

        /// <summary>
        /// Sets a new mood for the character.
        /// </summary>
        /// <param name="newMood">The new mood to assign.</param>
        public void SetMood(Mood newMood)
        {
            CurrentMood = newMood;
        }
    }
}
