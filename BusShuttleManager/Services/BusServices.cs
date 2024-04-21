using DomainModel;
namespace BusShuttleManager.Services
{
    public class BusServices : IBusService
    {
        private DataContext db;
        List<Bus> busses;

        public BusServices()
        { 

        }

        public List<Bus> getAllBusses()
        {
             db = new DataContext();
            busses = db.Bus
                .Select(b => new Bus(b.Id, b.BusName)).ToList();
            return busses;
        }

        public List<Bus> getActiveBusses()
        {
            db = new DataContext();
            return db.Bus.Where(bus => bus.IsActive).ToList();
        }

        public Bus findBusById(int id)
        {
            db = new DataContext();
            var bus = db.Bus
                .SingleOrDefault(bus =>bus.Id == id);

            return new Bus(bus);
        }

        public void UpdateBusById(int id, string name)
        {
            db = new DataContext();
            var existingBus = db.Bus.SingleOrDefault(bus => bus.Id == id);
            existingBus.Update(name);

            var bus = db.Bus
                .SingleOrDefault(bus => bus.Id == existingBus.Id);
            
            if(bus != null)
            {
                bus.BusName = name;
                db.SaveChanges();
            }
        }

        public void deactivateBus(int id)
        {
            db = new DataContext();
            var existingBus = db.Bus.FirstOrDefault(b => b.Id == id);
        
            if (existingBus != null)
            {
                existingBus.IsActive = false;
                db.SaveChanges();
            }
        }

        public int GetAmountOfBusses()
        {
            db = new DataContext();
            return db.Bus.Count();
        }

        public void CreateNewBus(int id, string name)
        {
            db = new DataContext();
            db.Add(new Bus{Id=id, BusName=name});
            db.SaveChanges();
        }
    }
}