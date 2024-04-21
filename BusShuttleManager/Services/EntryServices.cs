using DomainModel;
namespace BusShuttleManager.Services
{
    public class EntryServices : IEntryService
    {
        private DataContext db;
        List<Entry> entries;

        public List<Entry> getAllEntries()
        {
            db = new DataContext();
            entries = db.Entry
                .Select(e => new Entry(e.TimeStamp, e.Boarded, e.LeftBehind, e.LoopId, e.DriverId, e.StopId, e.BusId)).ToList();
            return entries;
        }

        public Entry findEntryById(int id)
        {
            db = new DataContext();
            var entry = db.Entry
                .SingleOrDefault(e =>e.Id == id);

            return new Entry(entry);
        }

        public void createNewEntry(DateTime timeStamp, int boarded, int leftBehind, int busId, int driverId, int loopId, int stopId)
        {
            db = new DataContext();
            var totalEntries = db.Entry.Count();
            db.Add(new Entry{Id = totalEntries+1, TimeStamp=timeStamp, Boarded=boarded, LeftBehind=leftBehind, 
                BusId=busId, DriverId=driverId, LoopId=loopId, StopId=stopId});
            db.SaveChanges();
        }

        public List<Entry> getEntriesByDate(DateTime selectedDate)
        {
            db = new DataContext();
            return db.Entry
                     .Where(entry => entry.TimeStamp.Date == selectedDate.Date)
                     .ToList();
        }

        public List<Entry> getEntriesByLoopId(int loopId)
        {
            db = new DataContext();
            return db.Entry
                     .Where(entry => entry.LoopId == loopId)
                     .ToList();
        }

        public List<Entry> getEntriesByDateAndLoop(DateTime dateTime, int loopId)
        {
            db = new DataContext();
            return db.Entry
                .Where(entry => entry.TimeStamp.Date == dateTime.Date && entry.LoopId == loopId)
                .ToList();
        }

        public Entry getEntryForLoopBusDriver(int loopId, int busId, int driverId)
        {
            db = new DataContext();
            return db.Entry.FirstOrDefault(e => e.LoopId == loopId && e.BusId == busId && e.DriverId == driverId);
        }

    }
}