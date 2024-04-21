using DomainModel;
namespace BusShuttleManager.Services
{
    public class StopServices : IStopService
    {
        private DataContext db;
        List<Stop> stops;

        public List<Stop> getAllStops()
        {
            db = new DataContext();
            stops = db.Stop
                .Select(s => new Stop(s.Id, s.Name, s.Latitude, s.Longitude)).ToList();
            return stops;
        }

        public List<Stop> getActiveStops()
        {
            db = new DataContext();
            return db.Stop.Where(stop => stop.IsActive).ToList();
        }


        public Stop findStopById(int id)
        {
            db = new DataContext();
            var stop = db.Stop
                .SingleOrDefault(s =>s.Id == id);

            return new Stop(stop);
        }

        public void UpdateStopById(int id, string name)
        {
            db = new DataContext();
            var existingStop = db.Stop.SingleOrDefault(stop => stop.Id == id);
            existingStop.Update(name);

            var stop = db.Stop
                .SingleOrDefault(stop => stop.Id == existingStop.Id);
            
            if(stop != null)
            {
                stop.Name = name;
                db.SaveChanges();
            }
        }

        public int GetAmountOfStops()
        {
            db = new DataContext();
            return db.Stop.Count();
        }

        public void CreateNewStop(int id, string name, double lat, double lon)
        {
            db = new DataContext();
            db.Add(new Stop{Id = id, Name=name, Latitude=lat, Longitude=lon});
            db.SaveChanges();
        }

        public void deactivateStop(int id)
        {
            db = new DataContext();
            var existingStop = db.Stop.FirstOrDefault(s => s.Id == id);
        
            if (existingStop != null)
            {
                existingStop.IsActive = false;
                db.SaveChanges();
            }
        }
    }
}