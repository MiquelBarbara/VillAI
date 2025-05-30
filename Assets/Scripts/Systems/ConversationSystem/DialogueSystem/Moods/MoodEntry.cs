using System;

[Serializable]
public class MoodEntry
{
    public Mood Mood;
    public string Context;

    public MoodEntry(Mood mood, string context)
    {
        Mood = mood;
        Context = context;
    }
}