using System;
using EntitiesRelated.Core.Data;
using GameplayFocused.TimeManagment;
using NPCs;
using UnityEngine;

namespace HistoryManagment
{
    /// <summary>
    /// Represents a single entry in a conversation session.
    /// Uses CharacterData for the speaker instead of a string.
    /// </summary>
    [Serializable]
    public struct ConversationEntry
    {
        public string Speaker;
        public string Text;
        public float Timestamp;

        public ConversationEntry(string speaker, string text)
        {
            Speaker = speaker;
            Text = text;
            Timestamp = DayNightController.Instance.GetTime();
        }
        
        public string GetText()
        {
            return Text;
        }
        
        public string GetSpeaker()
        {
            return Speaker;
        }
        
        public void GetLastEntry(out string speaker, out string text)
        {
            speaker = Speaker;
            text = Text;
        }
    }
}