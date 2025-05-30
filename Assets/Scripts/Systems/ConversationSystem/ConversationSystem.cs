using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EntitiesRelated.Core;
using LLM.Services;
using LLM.Templates;
using NPCs;
using Systems.DialogueSystem.Moods;
using Systems.SaveSystem.Memory;
using UI;
using UnityEngine;
using UnityServiceLocator;


public enum ConversationType
{
    Conversation,
    Trade
}
namespace Systems
{
    /// <summary>
    /// Base class for managing conversation sessions in the game.
    /// Handles session lifecycle (start, transition, end) and is extended by specific conversation systems.
    /// </summary>
    public class ConversationSystem : MonoBehaviour
    {
        [SerializeField] public DialogueSelector DialogueSelector;

        UIManager _uiManager;
        
        [SerializeField] PromptDefinition _npcConversationPrediction;
        
        [SerializeField] PromptDefinition _endConversationPrediction;
        
        [SerializeField] PromptDefinition _moodPrediction;
        
        [SerializeField] PromptDefinition _tradingPrediction;
        
        [SerializeField] ItemDatabase _itemDatabase;
        
        public Dictionary<Session, Coroutine> _activeSessions = new Dictionary<Session, Coroutine>();

        public void Start()
        {
            _uiManager = UIManager.Instance;
        }

        /// <summary>
        /// Inicia una nueva sesión de conversación utilizando el SessionManager.
        /// </summary>
        /// <param name="session">La sesión a iniciar.</param>
        public void StartSession(Session session, ConversationType type = ConversationType.Conversation)
        {
           if(_activeSessions.ContainsKey(session))
               return;
           
           if(ConversationType.Trade == type)
           {
               // Handle trade session logic here
               ProcessTradeSession(session);
               return;
           }
           else
           {
               ProcessConversationSession(session);
           }
        }

        public void ProcessConversationSession(Session session)
        {
            bool isParticipant1ANpc = session.GetParticipant1() is Npc;
            bool isParticipant2ANpc = session.GetParticipant2() is Npc;
            Debug.Log($"Starting session between {session.GetParticipant1().GetName()} and {session.GetParticipant2().GetName()}");
           
            if (!isParticipant1ANpc)
            {
                StartCoroutine(PlayerToNpcSession(session));
                return;
            }

            if (isParticipant2ANpc)
            {
                StartCoroutine(NpcToNpcSession(session));
            }
        }
        
        public void ProcessTradeSession(Session session)
        {
            bool isParticipant1ANpc = session.GetParticipant1() is Npc;
            bool isParticipant2ANpc = session.GetParticipant2() is Npc;
            Debug.Log($"Starting session between {session.GetParticipant1().GetName()} and {session.GetParticipant2().GetName()}");
           
            if (!isParticipant1ANpc)
            {
                StartCoroutine(PlayerToNpcTradeSession(session));
                return;
            }

            if (isParticipant2ANpc)
            {
                StartCoroutine(NpcToNpcTradeSession(session));
            }
        }
        
        public IEnumerator PlayerToNpcTradeSession(Session session)
        {
            UIManager.Instance.HideToolbar();
            UIManager.Instance.ShowTwoInventoryPanels(session.GetParticipant1().GetInventory(), session.GetParticipant2().GetInventory());
            while (!session.ConversationEnded()) {
                yield return GetInputFromPlayer(session);
                yield return ShopResponse(session, session.GetParticipant2(), session.GetParticipant1());
                yield return CheckTransaction(session);
                yield return CheckEnd(session, session.GetParticipant1());
                yield return CheckEnd(session, session.GetParticipant2());
            }
            UIManager.Instance.HideTwoInventoryPanels();
            UIManager.Instance.ShowToolbar();
            session.GetParticipant2().GetMemory<ConversationContainer>()?.InsertIntoGroup(session.GetEntries(), session.GetParticipant1().GetName());
            EndSession(session);
        }
        
        public IEnumerator NpcToNpcTradeSession(Session session)
        {
            
            while (!session.ConversationEnded()) {
                yield return NpcResponse(session, session.GetParticipant1(), session.GetParticipant2());
                yield return ShopResponse(session, session.GetParticipant2(), session.GetParticipant1());
                yield return CheckTransaction(session);
                yield return CheckEnd(session, session.GetParticipant1());
                yield return CheckEnd(session, session.GetParticipant2());
            }

            session.GetParticipant1().GetMemory<ConversationContainer>()?.InsertIntoGroup(session.GetEntries(), session.GetParticipant2().GetName());
            session.GetParticipant2().GetMemory<ConversationContainer>()?.InsertIntoGroup(session.GetEntries(), session.GetParticipant1().GetName());
            EndSession(session);
        }

        
        public IEnumerator PlayerToNpcSession(Session session)
        {
            UIManager.Instance.HideToolbar();
            while (!session.ConversationEnded()) {
                yield return GetInputFromPlayer(session);
                yield return MoodChange(session, session.GetParticipant1(), session.GetParticipant2());
                yield return NpcResponse(session, session.GetParticipant2(), session.GetParticipant1());
                yield return CheckEnd(session, session.GetParticipant1());
                yield return CheckEnd(session, session.GetParticipant2());
            }
            UIManager.Instance.ShowToolbar();
            session.GetParticipant2().GetMemory<ConversationContainer>()?.InsertIntoGroup(session.GetEntries(), session.GetParticipant1().GetName());
            EndSession(session);
        }
        
        public IEnumerator NpcToNpcSession(Session session)
        {
            while (!session.ConversationEnded()) {
                yield return NpcResponse(session, session.GetParticipant1(), session.GetParticipant2());
                yield return MoodChange(session, session.GetParticipant1(), session.GetParticipant2());
                yield return NpcResponse(session, session.GetParticipant2(), session.GetParticipant1());
                yield return MoodChange(session, session.GetParticipant2(), session.GetParticipant1());
                yield return CheckEnd(session, session.GetParticipant1());
                yield return CheckEnd(session, session.GetParticipant2());
            }
            session.GetParticipant1().GetMemory<ConversationContainer>()?.InsertIntoGroup(session.GetEntries(), session.GetParticipant2().GetName());
            session.GetParticipant2().GetMemory<ConversationContainer>()?.InsertIntoGroup(session.GetEntries(), session.GetParticipant1().GetName());
            EndSession(session);
        }
        
        public void PlayerToDumbSession(Session session)
        {
            RandomDialogue(session);
        }
        
        
        /// <summary>
        /// Get Input from the player and add it to the session.
        /// /// </summary>
        /// <param name="session">La sesión a iniciar.</param>
        public IEnumerator GetInputFromPlayer(Session session)
        {
            string input = "";
            yield return StartCoroutine(_uiManager.GetPlayerInput(userInput => input = userInput));
            session.AddConversationEntry(input, session.GetParticipant1().GetName());
        }
        
        public IEnumerator NpcResponse(Session session, Character generator, Character receiver)
        {
            // Instantiate the data container for the AI
            NpcConversationTemplateData conversationData = new NpcConversationTemplateData
            {
                PassiveCharacter = receiver.GetMemory<ComplexCharacterData>()!,
                message = session.GetLastEntry()?.GetText(),
                ActiveCharacter = generator.GetMemory<ComplexCharacterData>(),
                emotional_state = generator.GetMemory<MoodContainer>()!.actualMood,
                relationship = generator.GetMemory<RelationshipsContainer>()?.GetRelationship(receiver.GetName()),
                otherConversations = generator.GetMemory<ConversationContainer>()?.GetAllSummaries(receiver.GetName()) ?? new List<string>(),
            };
            
            // Retrieve the configured service from the repository
            ServiceLocator.Global.Get<IPredictionService>(out var service);
            yield return service.Predict(_npcConversationPrediction, conversationData);

            if(conversationData.npc_response == null || conversationData.npc_response.Trim().Length == 0)
            {
                Debug.LogWarning("NPC response is empty or null. Skipping response.");
                yield break;
            }
            
            // Save the resulting text to the session as an "NPC spoke"
            session.AddConversationEntry(conversationData.npc_response, generator.GetName());

            // Store internal thoughts/knowledge in the active participant's memory
            generator.GetMemory<Thoughts>()?.AddEntry(conversationData.internal_thoughts);
            generator.GetMemory<KnowledgeContainer>()?.AddEntry(conversationData.knowledge);

            if (receiver is MainCharacter)
            {
                yield return UIManager.Instance.ShowPanel(StringExtensions.SplitStringIntoSentences(conversationData.npc_response), generator.GetCharacterData());
            }
            else
            {
                yield return UIManager.Instance.ShowBubble(generator.gameObject.transform, StringExtensions.SplitStringIntoSentences(conversationData.npc_response));
            }
        }
        
        
        public IEnumerator CheckEnd(Session session, Character evaluatedCharacter)
        {
            ServiceLocator.Global.Get<IPredictionService>(out var service);
        
            var template = new EndConversationData()
            {
                character = evaluatedCharacter.GetCharacterData(),
                input = session.GetLastEntry(-2)?.GetText(),
                history = session.GetEntries(),
            };
            yield return service.Predict(_endConversationPrediction, template);
            bool response2 = template.endConversation;
            
            session.ParticipantWantsToFinish(evaluatedCharacter, response2);
            session.SetPatience(evaluatedCharacter, template.newPatience);
        }
        
        public void RandomDialogue(Session session)
        {
            // Fetch a random line of dialogue for the NPC
            var dialogue = DialogueSelector.GetRandomDialogue(session.GetParticipant2().GetName());

            // Write it to the UI
            //yield return WriterManager.WriteContent(session.Encapsulate(dialogue));
        }
        
        public IEnumerator MoodChange(Session session, Character generator, Character receiver)
        {
            var templateData = new MoodTemplateData()
            {
                available_moods = Enum.GetNames(typeof(Mood)).ToList(),
                ActiveCharacter = generator.GetCharacterData(),
                message = session.GetLastEntry()?.GetText(),
                PassiveCharacter = receiver.GetCharacterData(),
                emotional_state = receiver.GetMemory<MoodContainer>()!.actualMood,
                relationship = receiver.GetMemory<RelationshipsContainer>()?.GetRelationship(generator.GetName()),
            };
            
            // Request a PredictionService from the repository
            ServiceLocator.Global.Get<IPredictionService>(out var service);
            yield return service.Predict(_moodPrediction, templateData);
            
            // Store the resulting mood in the active character's memory
            receiver.GetMemory<MoodContainer>()?.AddEntry(new MoodEntry(templateData.mood, templateData.feelings));
        }

        public IEnumerator RelationshipChange(Session session, Character generator, Character receiver)
        {
            var templateData = new UpdateRelationshipTemplateData()
            {
                character1 = generator.GetCharacterData(),
                character2 = receiver.GetCharacterData(),
                relationship = receiver.GetMemory<RelationshipsContainer>()?.GetRelationship(generator.GetName()),
                entries = session.GetEntries(),
                metrics = Enum.GetNames(typeof(RelationshipMetric)).ToList()
            };
            
            // Request a PredictionService from the repository
            ServiceLocator.Global.Get<IPredictionService>(out var service);
            yield return service.Predict(_tradingPrediction, templateData);
            
            // Store the resulting relationship metrics in the active character's memory
            var relationshipContainer = receiver.GetMemory<RelationshipsContainer>();
            foreach (var metric in templateData.listUpdatedMetrics)
            {
                relationshipContainer?.AddOrUpdateRelationship(templateData.character1.characterName, metric);
            }
        }
        
        public IEnumerator CheckTransaction(Session session)
        {
            
            /*string actionText = actionType == TradeActionType.Buy ? "buy" : "sell";
            string message = $"Are you sure you want to {actionText} {tradePurpose.GetAmount()} {tradePurpose.GetItemName()}?";

            bool decisionMade = false;
            AcceptRejectPanel.gameObject.SetActive(true);
            AcceptRejectPanel.Show(
                message,
                () =>
                {
                    tradePurpose.ExecuteTrade(session.GetActive(), session.GetPassive() as Npc);
                    decisionMade = true;
                },
                () =>
                {
                    tradePurpose.RemoveAction();
                    decisionMade = true;
                }
            );
                
            yield return new WaitUntil(() => decisionMade);*/
            yield break;
        }
        
        public IEnumerator ShopResponse(Session session, Character generator, Character receiver)
        {
            // Create an instance of the auto‐generated template data for Trading
            var templateData = new TradingTemplateData()
            {
                client_input = session.GetLastEntry()?.GetText(),
                client_gold = receiver.GetCurrency(),
                client_inventory = receiver.GetInventory(),
                store_gold = generator.GetCurrency(),
                store_inventory = generator.GetInventory(),
                history = session.GetEntries()
            };

            // Call the appropriate AI service
            ServiceLocator.Global.Get<IPredictionService>(out var service);
            yield return service.Predict(_tradingPrediction,templateData);

            // AI-supplied dialogue response
            session.AddConversationEntry(templateData.shopper_response, generator.GetName());
            if (receiver is MainCharacter)
            {
                yield return UIManager.Instance.ShowPanel(StringExtensions.SplitStringIntoSentences(templateData.shopper_response), generator.GetCharacterData());
            }
            else
            {
                yield return UIManager.Instance.ShowBubble(generator.gameObject.transform, StringExtensions.SplitStringIntoSentences(templateData.shopper_response));
            }
            
            var item = _itemDatabase.GetByName(templateData.item_name);
            
            string message = "";
            if (templateData.item_name == null || templateData.amount <= 0)
            {
                Debug.LogWarning("No item selected for trade or amount is zero.");
                yield break;
            }
            if(CanBuy(item, templateData.amount, receiver.GetCurrency()) || CanSell(item, templateData.amount, generator.GetCurrency()))
            {
                if (templateData.wantToBuy)
                {
                    message = $"Do you want to buy {templateData.item_name} for {templateData.amount} gold?";
                } else if (templateData.wantToSell)
                {
                    message = $"Do you want to sell {templateData.item_name} for {templateData.amount} gold?";
                }
                else
                {
                    yield break;
                }
            }
            else
            {
                yield break;
            }
            
            bool decisionMade = false;
            UIManager.Instance.ShowAcceptRejectPanel(message ,
                () =>
                {
                    if (templateData.wantToBuy)
                    {
                        generator.AddItem(_itemDatabase.GetByName(templateData.item_name), templateData.amount);
                        generator.SpendMoney(templateData.amount);
                    }
                    else if (templateData.wantToSell)
                    {
                        receiver.RemoveItem(_itemDatabase.GetByName(templateData.item_name), templateData.amount);
                        receiver.EarnMoney(templateData.amount);
                    }
                    decisionMade = true;
                },
                () =>
                {
                    decisionMade = true;
                });
            yield return new WaitUntil(() => decisionMade);
        }

        public bool CanBuy(Item item, int amount, int currency)
        {
            if (item.storePrice * amount <= currency) return true;
            Debug.LogWarning($"Not enough currency to buy {amount} of {item.name}. Required: {item.storePrice * amount}, Available: {currency}");
            return false;
        }
        
        public bool CanSell(Item item, int amount, int currency)
        {
            if (item.resellPrice * amount > currency )
            {
                return true;
            }
            Debug.LogWarning($"Cannot sell {amount} of {item.name}. Not enough items in inventory.");
            return false;
        }

        
        /// <summary>
        /// Finaliza la sesión de conversación actual.
        /// </summary>
        /// <param name="session">La sesión a finalizar.</param>
        public void EndSession(Session session)
        {
            Debug.Log($"Ending session between {session.GetParticipant1().GetName()} and {session.GetParticipant2().GetName()}");
            session.FinishSession();
        }
    }
}
