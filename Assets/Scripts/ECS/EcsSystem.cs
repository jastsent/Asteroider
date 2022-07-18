namespace ECS
{
    public abstract class EcsSystem
    {
        public EcsWorld EcsWorld { get; private set; }

        public void SetWorld(EcsWorld world)
        {
            EcsWorld = world;
        }
        
        public virtual void Init()
        {
            
        }

        public virtual void Update()
        {
            
        }

        public virtual void Destroy()
        {
            
        }
    }
}
