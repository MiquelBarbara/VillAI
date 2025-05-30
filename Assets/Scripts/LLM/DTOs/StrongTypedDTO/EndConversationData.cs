using System;
using System.Collections.Generic;
using EntitiesRelated.Core.Data;
using HistoryManagment;
using UnityEngine;

namespace LLM.Templates
{

    public class EndConversationData : DataTransferObject
    {
        [Input]
        public CharacterData character;
        [Input]
        public String input;
        [Input]
        public List<ConversationEntry> history;
        
        
        [Output]
        public bool endConversation;
        [Output]
        public int newPatience;
    }
}