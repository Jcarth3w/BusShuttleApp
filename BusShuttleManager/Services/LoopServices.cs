using DomainModel;
namespace BusShuttleManager.Services
{
    public class LoopServices : ILoopService
    {
        private readonly ILogger<LoopServices> logger;
        private DataContext db;
        List<Loop> loops;

        public LoopServices(ILogger<LoopServices> logger)
        {
            this.logger = logger;
        }

        public List<Loop> getAllLoops()
        {
            logger.LogInformation("Getting all loops...");
            db = new DataContext();
            loops = db.Loop
                .Select(l => new Loop(l.Id, l.Name)).ToList();
            return loops;
        }

        public List<Loop> getActiveLoops()
        {
            logger.LogInformation("Getting all active loops...")
            db = new DataContext();
            return db.Loop.Where(loop => loop.IsActive).ToList();
        }

        public Loop getLoopById(int id)
        {
            logger.LogInformation("Finding loop by ID: {Id}", id);
            db = new DataContext();
            var loop = db.Loop
                .SingleOrDefault(loop => loop.Id == id);

            return new Loop(loop);
        }

        public void UpdateLoopById(int id, string name)
        {
            logger.LogInformation("Updating loop with ID: {Id}", id);
            db = new DataContext();
            var existingLoop = db.Loop.SingleOrDefault(loop => loop.Id == id);
            existingLoop.Update(name);

            var loop = db.Loop
                .SingleOrDefault(loop => loop.Id == existingLoop.Id);
            
            if(loop != null)
            {
                loop.Name = name;
                db.SaveChanges();
            }
        }

        public void deactivateLoop(Loop loop)
        {
            logger.LogInformation("Deactivating loop with ID: {Id}", id);
            db = new DataContext();
            var existingLoop = db.Loop.FirstOrDefault(l => l.Id == loop.Id);
        
            if (existingLoop != null)
            {
                existingLoop.IsActive = false;
                db.SaveChanges();
            }

        }

        public int GetAmountOfLoops()
        {
            logger.LogInformation("Getting total amount of loops...");
            db = new DataContext();
            return db.Loop.Count();
        }

        public void CreateNewLoop(int id, string name)
        {
            logger.LogInformation("Creating new loop with ID: {Id}", id);
            db = new DataContext();
            var totalLoops = db.Loop.Count();
            db.Add(new Loop{Id=totalLoops+1, Name=name});
            db.SaveChanges();
        }
    }
}