using EntitiesRelated.Core;
using GOAP.Scripts.AgentStats;
using NPCs;
using UnityEngine.InputSystem;

namespace GOAP.Scripts
{
    public interface IAgentManager
    {
        void StopAgents(Character character, Npc npc);
        void ResumeAgents(Character character, Npc npc);
    }
    
    public class AgentManager : IAgentManager
    {
        public void StopAgents(Character character, Npc npc)
        {
            var characterGoapAgent = character.gameObject.GetComponent<GoapAgent>();
            var playerinput = character.gameObject.GetComponent<PlayerInput>();
            var npcGoapAgent = npc.gameObject.GetComponent<GoapAgent>();

            playerinput?.DeactivateInput();
            characterGoapAgent?.Stop();
            npcGoapAgent?.Stop();
            
            characterGoapAgent?.GetComponent<StatSystem>().IncreaseStat("Socialization", 100);
            npcGoapAgent?.GetComponent<StatSystem>().IncreaseStat("Socialization", 100);
        }

        public void ResumeAgents(Character character, Npc npc)
        {
            var characterGoapAgent = character.gameObject.GetComponent<GoapAgent>();
            var playerinput = character.gameObject.GetComponent<PlayerInput>();
            var npcGoapAgent = npc.gameObject.GetComponent<GoapAgent>();

            playerinput?.ActivateInput();
            characterGoapAgent?.Resume();
            npcGoapAgent?.Resume();
        }
    }

}