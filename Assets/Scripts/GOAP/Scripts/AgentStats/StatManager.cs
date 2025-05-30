using EntitiesRelated.Core;

namespace GOAP.Scripts.AgentStats
{
    /// <summary>
    /// Defines an interface for updating stats between characters and NPCs.
    /// </summary>
    public interface IStatManager
    {
        /// <summary>
        /// Updates the stat identified by the given key for both a character and an NPC.
        /// </summary>
        /// <param name="character">The character whose stat will be updated.</param>
        /// <param name="npc">The NPC whose stat will be updated.</param>
        /// <param name="key">The key identifying the stat.</param>
        /// <param name="amount">The amount to increase the stat by.</param>
        void UpdateStats(Character character, Npc npc, string key, float amount);
    }

    /// <summary>
    /// Implements stat updating by increasing the stat value on both a character and an NPC.
    /// </summary>
    public class StatManager : IStatManager
    {
        /// <summary>
        /// Updates the stat for the specified character and NPC.
        /// </summary>
        /// <param name="character">The character whose stat is updated.</param>
        /// <param name="npc">The NPC whose stat is updated.</param>
        /// <param name="key">The key of the stat.</param>
        /// <param name="amount">The amount to increase the stat.</param>
        public void UpdateStats(Character character, Npc npc, string key, float amount)
        {
            // Update the character's stat.
            var characterStatSystem = character.gameObject.GetComponent<StatSystem>();
            characterStatSystem?.IncreaseStat(key, amount);
            
            // Update the NPC's stat.
            var npcStatSystem = npc.gameObject.GetComponent<StatSystem>();
            npcStatSystem?.IncreaseStat(key, amount);
        }
    }
}