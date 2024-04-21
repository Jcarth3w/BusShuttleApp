using DomainModel;
namespace BusShuttleManager.Services
{
    public class LoopServices : ILoopService
    {
        private DataContext db;
        List<Loop> loops;

        public List<Loop> getAllLoops()
        {
            db = new DataContext();
            loops = db.Loop
                .Select(l => new Loop(l.Id, l.Name)).ToList();
            return loops;
        }

        public List<Loop> getActiveLoops()
        {
            db = new DataContext();
            return db.Loop.Where(loop => loop.IsActive).ToList();
        }

        public Loop getLoopById(int id)
        {
            db = new DataContext();
            var loop = db.Loop
                .SingleOrDefault(loop => loop.Id == id);

            return new Loop(loop);
        }

        public void UpdateLoopById(int id, string name)
        {
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
            db = new DataContext();
            return db.Loop.Count();
        }

        public void CreateNewLoop(int id, string name)
        {
            db = new DataContext();
            var totalLoops = db.Loop.Count();
            db.Add(new Loop{Id=totalLoops+1, Name=name});
            db.SaveChanges();
        }
    }
}