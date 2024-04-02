using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BusShuttleManager.Models;
using DomainModel;
using BusShuttleManager.Services;

namespace BusShuttleManager.Services;

public class HomeController : Controller 
{
    private readonly ILogger<HomeController> _logger;
    IDriverService driverService;

    IBusService busService;

    IRouteService routeService;

    IStopService stopService;

    IEntryService entryService;

    ILoopService loopService;


    public HomeController(ILogger<HomeController> logger, IDriverService dService, IBusService bService, 
        IRouteService rService, IStopService sService, IEntryService eService, ILoopService lService)
    {
        _logger = logger;
        this.driverService = dService;
        this.busService = bService;
        this.routeService = rService;
        this.stopService = sService;
        this.entryService = eService;
        this.loopService = lService;
    }

    /*DRIVERS*/
    public IActionResult Drivers()
    {
        return View(driverService.getAllDrivers().Select(d => DriverViewModel.FromDriver(d)));
    }

    public IActionResult UpdateDriver([FromRoute] int id)
    {
        var driverEditModel = new EditDriverModel();  
        var driver = driverService.findDriverById(id);
 
        driverEditModel = EditDriverModel.FromDriver(driver);
        return View(driverEditModel);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateDriver(int id, [Bind("FirstName,LastName")] EditDriverModel driver)
    {
        driverService.UpdateDriverById(id, driver.FirstName, driver.LastName);
        return RedirectToAction("Drivers");
    }


    public IActionResult CreateDriver()
    {
        var driverCreateModel = CreateDriverModel.NewDriver(driverService.GetAmountOfDrivers());
        return View(driverCreateModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateDriver(int id, [Bind("FirstName,LastName")] CreateDriverModel driver)
    {
   
        driverService.CreateNewDriver(id, driver.FirstName, driver.LastName);
        return Redirect("Drivers");

    }


    [HttpDelete]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteDriver(int id)
    {
        driverService.DeleteDriver(id);
        return RedirectToAction("Drivers");
    }
  

    /*BUSSES*/
    public IActionResult Busses()
    {
        return View(busService.getAllBusses().Select(b => BusViewModel.FromBus(b)));
    }


    public IActionResult UpdateBus([FromRoute] int id)
    {
        var busEditModel =  new EditBusModel();
        var bus = busService.findBusById(id);

        busEditModel = EditBusModel.FromBus(bus);
        return View(busEditModel);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateBus(int id, [Bind("BusName")] EditBusModel bus)
    {
        busService.UpdateBusById(id, bus.BusName);
        return RedirectToAction("Busses");
    }

    public IActionResult CreateBus()
    {
        var busCreateModel = BusCreateModel.NewBus(busService.GetAmountOfBusses());
        return View(busCreateModel);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateBus(int id, [Bind("BusName")] BusCreateModel bus)
    {
        busService.CreateNewBus(id, bus.BusName);
        return RedirectToAction("Busses");
    }


    /*ROUTES*/

    public IActionResult Routes()
    {
        return View(routeService.getAllRoutes().Select(r => RoutesViewModel.FromRoutes(r)));
    }


    /*STOPS*/
    public IActionResult Stops()
    {
        return View(stopService.getAllStops().Select(s=>StopViewModel.FromStop(s)));
    }

    public IActionResult UpdateStop([FromRoute] int id)
    {
        var stopEditModel =  new StopEditModel();
        var stop = stopService.findStopById(id);

        stopEditModel = StopEditModel.FromStop(stop);
        return View(stopEditModel);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateStop(int id, [Bind("Name")] StopEditModel stop)
    {
        stopService.UpdateStopById(id, stop.Name);
        return RedirectToAction("Stops");
    }


    public IActionResult CreateStop()
    {
        var stopCreateModel = StopCreateModel.NewStop(stopService.GetAmountOfStops());
        return View(stopCreateModel);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateStop(int id, [Bind("Name,Latitude,Longitude")] StopCreateModel stop)
    {
        stopService.CreateNewStop(id, stop.Name, stop.Latitude, stop.Longitude);
        return RedirectToAction("Stops");
    }



    /*LOOPS*/

    public IActionResult Loops()
    {
        return View(loopService.getAllLoops().Select(l => LoopViewModel.FromLoop(l)));
    }

    public IActionResult UpdateLoop([FromRoute] int id)
    {
        var loopEditModel =  new LoopEditModel();
        var loop = loopService.findLoopById(id);

        loopEditModel = LoopEditModel.FromLoop(loop);
        return View(loopEditModel);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateLoop(int id, [Bind("Name")] LoopEditModel loop)
    {
        loopService.UpdateLoopById(id, loop.Name);
        return RedirectToAction("Loops");
    }

    public IActionResult CreateLoop()
    {
        var loopCreateModel = LoopCreateModel.NewLoop(loopService.GetAmountOfLoops());
        return View(loopCreateModel);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateLoop(int id, [Bind("Name")] LoopCreateModel loop)
    {
        loopService.CreateNewLoop(id, loop.Name);
        return RedirectToAction("Loops");
    }


    public IActionResult Entries()
    {
        return View(entryService.getAllEntries().Select(e => EntryViewModel.FromEntry(e)));
    }
}