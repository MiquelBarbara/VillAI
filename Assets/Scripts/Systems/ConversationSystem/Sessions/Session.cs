using System;
using System.Collections.Generic;
using EntitiesRelated.Core;
using EntitiesRelated.Core.Data;
using HistoryManagment;


/// <summary>
/// Represents a participant in a session, containing character data, willingness to finish, and patience level.
/// </summary>
public class ParticipantData
{
    public Character Character { get; }
    bool _wantsToFinish;
    int _patience;
    
    public ParticipantData(Character characterData, bool wantsToFinish = false, int patience = 100)
    {
        Character = characterData ?? throw new ArgumentNullException(nameof(characterData));
        _wantsToFinish = wantsToFinish;
        _patience = patience;
    }
    
    public bool WantsToFinish => _wantsToFinish;
    public int Patience => _patience;
    
    public void SetWantsToFinish(bool wantsToFinish)
    {
        _wantsToFinish = wantsToFinish;
    }
    
    public void SetPatience(int patience)
    {
        if (patience < 0)
            throw new ArgumentOutOfRangeException(nameof(patience), "Patience cannot be negative.");
        _patience = patience;
    }
}


/// <summary>
/// Represents an abstract session between two characters with a specific purpose.
/// Tracks dialogue, turns, relationship, and session state.
/// </summary>
[AllowAsParameter]
public class Session
{
    private ParticipantData Participant1;
    private ParticipantData Participant2;
    
    public Action OnSessionEnd;
    private bool _isActive;

    List<ConversationEntry> _conversationEntries = new List<ConversationEntry>();

    public Session(Character participant1, Character participant2)
    {
        Participant1 = new ParticipantData(participant1);
        Participant2 = new ParticipantData(participant2);
        _isActive = true;
    }

    public bool ConversationEnded() => Participant1.WantsToFinish || Participant2.WantsToFinish;

    public Character GetParticipant1() => Participant1.Character;
    public Character GetParticipant2() => Participant2.Character;
    
    public void ParticipantWantsToFinish(Character participant, bool wantsToFinish)
    {
        if (Participant1.Character == participant)
        {
            Participant1.SetWantsToFinish(wantsToFinish);
        }
        else if (Participant2.Character == participant)
        {
            Participant2.SetWantsToFinish(wantsToFinish);
        }
        else
        {
            throw new ArgumentException("Participant not found in this session.");
        }
    }

    public void SetPatience(Character participant, int patienceValue)
    {
        if (Participant1.Character == participant)
        {
            Participant1.SetPatience(patienceValue);
        }
        else if (Participant2.Character == participant)
        {
            Participant2.SetPatience(patienceValue);
        }
        else
        {
            throw new ArgumentException("Participant not found in this session.");
        }
    }
    
    public void FinishSession()
    {
        if (!_isActive)
            throw new InvalidOperationException("Session is not active.");
        _isActive = false;
        OnSessionEnd?.Invoke();
    }

    /// <summary>
    /// Adds a new conversation entry to the current talk session.
    /// </summary>
    /// <param name="text">The text spoken by the active participant.</param>
    /// <param name="characterName">The name of the character</param>
    public void AddConversationEntry(string text, string characterName)
    {
        // Get the active character's CharacterData for richer conversation data.
        _conversationEntries.Add(new ConversationEntry(characterName, text));
    }
    
    public ConversationEntry? GetLastEntry(int index = -1)
    {
        if (_conversationEntries == null || _conversationEntries.Count == 0)
            return null;

        int targetIndex = _conversationEntries.Count + index;

        if (targetIndex < 0 || targetIndex >= _conversationEntries.Count)
            return null;

        return _conversationEntries[targetIndex];
    }
    
    public List<ConversationEntry> GetEntries() => _conversationEntries;
}
