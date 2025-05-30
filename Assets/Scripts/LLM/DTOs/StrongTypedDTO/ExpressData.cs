using System;
using NPCs;
using UnityEngine;
using Utilities.ScriptableObjectExtensions;

namespace LLM.Templates
{
    public class ExpressData : DataTransferObject
    {
        [Input]
        public String environment;
        [Input]
        public ComplexCharacterData personality;
        [Input]
        public ResourceLocator memory;
        
        
        [Output]
        public String dialogue;
    }
}