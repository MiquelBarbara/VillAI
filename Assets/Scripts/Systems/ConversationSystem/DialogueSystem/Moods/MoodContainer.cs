using Systems.SaveSystem.Memory;
using UnityEngine;

namespace Systems.DialogueSystem.Moods
{
    [CreateAssetMenu(fileName = "MoodContainer", menuName = "Resources/MoodContainer")]
    public class MoodContainer : Summarizable<MoodEntry>
    {
        public Mood actualMood;

        public override void AddEntry(MoodEntry entry)
        {
            base.AddEntry(entry);
            actualMood = entry.Mood;
        }
    }
}