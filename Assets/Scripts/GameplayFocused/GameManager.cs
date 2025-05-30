using Game.ALPHA;
using GameplayFocused.TimeManagment;
using HistoryManagment;
using Systems;
using Systems.DialogueSystem;
using Systems.ReactSystem;
using Systems.TradingSystem;
using UnityEngine;
using UnityServiceLocator;

namespace GameplayFocused
{
    /// <summary>
    /// The central game manager that initializes various systems and holds global references.
    /// Implements a singleton pattern to allow easy access from other components.
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        /// <summary>
        /// Reference to the player GameObject.
        /// </summary>
        public GameObject player;

        /// <summary>
        /// The dialogue system for managing conversations.
        /// </summary>
        public ConversationSystem ConversationSystem;
        
        /// <summary>
        /// The reaction system for processing in-game reactions.
        /// </summary>
        public ReactSystem ReactSystem;
    
        /// <summary>
        /// The day/night controller for managing in-game time.
        /// </summary>
        public DayNightController TimeController;
    
        /// <summary>
        /// The player's inventory container.
        /// </summary>
        public ItemContainer inventory;

        /// <summary>
        /// The drag-and-drop controller for managing item interactions.
        /// </summary>
        public ItemDragAndDropController dragAndDropController;

        [SerializeField] private ItemDatabase itemDatabase;
        
        protected override void Awake()
        {
            base.Awake();
            // Register services
            ServiceLocator.Global.Register<IResourceService>(new ResourceService());
        }
        
        private void Start()
        {
            Application.targetFrameRate = 60;
            // Additional initialization (e.g., Python script execution) can be added here.
        }
    
        /// <summary>
        /// Retrieves the item database.
        /// </summary>
        /// <returns>The ItemDatabase instance.</returns>
        public ItemDatabase GetItemDatabase()
        {
            return itemDatabase;
        }
        
    }
}
