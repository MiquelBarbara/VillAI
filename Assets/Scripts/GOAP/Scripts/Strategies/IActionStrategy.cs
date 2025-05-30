namespace GOAP.Scripts
{
    /// <summary>
    /// Represents a strategy for executing an action in the GOAP system.
    /// </summary>
    public interface IActionStrategy
    {
        // Boolean indicating if the action can be performed based on current conditions.
        bool CanPerform { get; }
        // Boolean indicating if the action is complete.
        bool Complete { get; }

        // Starts the action, initializing any necessary state.
        void Start()
        {
        }

        // Updates the action's state based on the elapsed time since the last update.
        void Update(float deltaTime)
        {
        }

        // Stops the action, cleaning up any resources or state.
        void Stop()
        {
        }
    }
}