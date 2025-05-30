using EntitiesRelated.Core;
using GameplayFocused;
using NPCs;

namespace Systems.DialogueSystem
{
    public interface ITradeSessionService
    {
        Session StartTradeSession(Character character, Npc npc);
    }

    public class TradeSessionService : ITradeSessionService
    {
        public Session StartTradeSession(Character character, Npc npc)
        {
            var session = new Session(character, npc);
            GameManager.Instance.ConversationSystem.StartSession(session, ConversationType.Trade);

            return session;
        }
    }

}