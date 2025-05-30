using System;

namespace GameplayFocused
{
    public struct Quest
    {
        private string description;
        private int reward;
        private Func<bool> completionCondition;
    }
}