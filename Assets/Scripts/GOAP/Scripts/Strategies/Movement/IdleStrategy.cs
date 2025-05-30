namespace GOAP.Scripts
{
    public class IdleStrategy : IActionStrategy
    {
        private readonly InGameCountdownTimer timer;
        

        public IdleStrategy(float duration)
        {
            timer = new InGameCountdownTimer(duration);
            timer.OnTimerStart += () => Complete = false;
            timer.OnTimerStop += () => Complete = true;
        }
        
        public bool CanPerform => true;
        public bool Complete { get; private set; }

        public void Start()
        {
            timer.Start();
        }

        public void Update(float deltaTime)
        {
            timer.Tick(deltaTime);
        }
    }
}