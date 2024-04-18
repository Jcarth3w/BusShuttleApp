using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using BusShuttleManager.Models;
using DomainModel;
using BusShuttleManager.Services;


namespace BusShuttleManager.Services;

public class HomeController : Controller 
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    IDriverService driverService;

    IBusService busService;

    IRouteService routeService;

    IStopService stopService;

    IEntryService entryService;

    ILoopService loopService;



    public HomeController(ILogger<HomeController> logger, IDriverService dService, IBusService bService, 
        IRouteService rService, IStopService sService, IEntryService eService, ILoopService lService, UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
        this.driverService = dService;
        this.busService = bService;
        this.routeService = rService;
        this.stopService = sService;
        this.entryService = eService;
        this.loopService = lService;
    }

    [Authorize(Policy = "ManagerOnly")]
    public IActionResult ManagerPage()
    {
        return View();
    }

    [Authorize(Policy = "ActivatedDriver")]
    public IActionResult DriverPage()
    {
        var loops = loopService.getAllLoops(); 
        var busses = busService.getAllBusses();
        var drivers = driverService.getAllDrivers();

        var viewModel = DriverPageViewModel.FromData(loops, busses, drivers);
        return View(viewModel);
    }

    /*DRIVERS*/

    [Authorize(Policy = "ManagerOnly")]
    public IActionResult Drivers()
    {
        var userClaims = User.Claims.ToList();

        // Log the claims
        foreach (var claim in userClaims)
        {
            _logger.LogInformation($"Claim Type: {claim.Type}, Value: {claim.Value}");
        }

        // Check if the user has the "Manager" claim
        var hasManagerClaim = userClaims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Manager");
        _logger.LogInformation($"User has Manager claim: {hasManagerClaim}");

        return View(driverService.getAllDrivers().Select(d => DriverViewModel.FromDriver(d)));
    }

    [Authorize(Policy = "ManagerOnly")]
    public IActionResult UpdateDriver([FromRoute] int id)
    {
        var driverEditModel = new EditDriverModel();  
        var driver = driverService.findDriverById(id);
 
        driverEditModel = EditDriverModel.FromDriver(driver);
        return View(driverEditModel);
    }


    [Authorize(Policy = "ManagerOnly")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateDriver(int id, [Bind("FirstName,LastName")] EditDriverModel driver)
    {
        driverService.UpdateDriverById(id, driver.FirstName, driver.LastName);
        return RedirectToAction("Drivers");
    }


    [Authorize(Policy = "ManagerOnly")]
    public IActionResult CreateDriver()
    {
        var driverCreateModel = CreateDriverModel.NewDriver(driverService.GetAmountOfDrivers());
        return View(driverCreateModel);
    }


    [Authorize(Policy = "ManagerOnly")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateDriver(int id, [Bind("FirstName,LastName,Email")] CreateDriverModel driver)
    {
        _logger.LogInformation("Email: " + driver.Email);

        _logger.LogInformation("Found email: " + driverService.FindDriverByEmail(driver.Email));
        if(driverService.FindDriverByEmail(driver.Email) == "")
        {
           
            ModelState.AddModelError("Email", "Email not found");

            var driverCreateModel = CreateDriverModel.NewDriver(id); 
            return View(driverCreateModel);
        }

        driverService.CreateNewDriver(id, driver.FirstName, driver.LastName, driver.Email);

        var user = await _userManager.FindByEmailAsync(driver.Email);
    
        if (user != null)
        {
            await _userManager.AddClaimAsync(user, new Claim("IsActivated", "true"));
        }
        return RedirectToAction("Drivers");
    }


    [Authorize(Policy = "ManagerOnly")]
    [HttpDelete]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteDriver(int id)
    {
        driverService.DeleteDriver(id);
        return RedirectToAction("Drivers");
    }
  

    /*BUSSES*/
    [Authorize(Policy = "ManagerOnly")]
    public IActionResult Busses()
    {
        return View(busService.getAllBusses().Select(b => BusViewModel.FromBus(b)));
    }


    [Authorize(Policy = "ManagerOnly")]
    public IActionResult UpdateBus([FromRoute] int id)
    {
        var busEditModel =  new EditBusModel();
        var bus = busService.findBusById(id);

        busEditModel = EditBusModel.FromBus(bus);
        return View(busEditModel);
    }


    [Authorize(Policy = "ManagerOnly")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateBus(int id, [Bind("BusName")] EditBusModel bus)
    {
        busService.UpdateBusById(id, bus.BusName);
        return RedirectToAction("Busses");
    }
    

    [Authorize(Policy = "ManagerOnly")]
    public IActionResult CreateBus()
    {
        var busCreateModel = BusCreateModel.NewBus(busService.GetAmountOfBusses());
        return View(busCreateModel);
    }


    [Authorize(Policy = "ManagerOnly")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateBus(int id, [Bind("BusName")] BusCreateModel bus)
    {
        busService.CreateNewBus(id, bus.BusName);
        return RedirectToAction("Busses");
    }


    /*ROUTES*/
    [Authorize(Policy = "ManagerOnly")]
    public IActionResult Routes()
    {
        var viewModel = new RoutesViewModel
        {
            Loops = loopService.getAllLoops(),
            Routes = new List<Routes>()
        };
        return View(viewModel);
    }

    [Authorize(Policy = "ManagerOnly")]
    [HttpPost]
    public IActionResult Routes(int selectedLoopId)
    {
        var loops = loopService.getAllLoops(); 
        var stops = stopService.getAllStops();
        var selectedLoop = loopService.getLoopById(selectedLoopId); 
        var routes = routeService.getRoutesByLoopId(selectedLoopId); 
        var viewModel = RoutesViewModel.FromLoopID(routes, loops, selectedLoop, stops);
        return View(viewModel);
    }

    [Authorize(Policy = "ManagerOnly")]
    [HttpPost]
    public IActionResult AddRoute(int selectedLoopId, int selectStopId)
    {
        var loops = loopService.getAllLoops(); 
        var stops = stopService.getAllStops();
        var selectedLoop = loopService.getLoopById(selectedLoopId); 
        var routes = routeService.getRoutesByLoopId(selectedLoopId); 
        var newOrder = routes.Count + 1;
        routeService.CreateNewRoute(newOrder, selectedLoopId, selectStopId);
        routes = routeService.getRoutesByLoopId(selectedLoopId); 
        var viewModel = RoutesViewModel.FromLoopID(routes, loops, selectedLoop, stops);
        return View(viewModel);
    }

    
    [Authorize(Policy = "ManagerOnly")]
    [HttpPost]
    public IActionResult MoveRouteUp(int selectedLoopId, int routeId)
    {
        var loops = loopService.getAllLoops(); 
        var stops = stopService.getAllStops();
        var selectedLoop = loopService.getLoopById(selectedLoopId); 
        var routes = routeService.getRoutesByLoopId(selectedLoopId); 
        routeService.IncreaseRouteOrder(routeId);
        _logger.LogInformation("Here");
        var viewModel = RoutesViewModel.FromLoopID(routes, loops, selectedLoop, stops);
        return View(viewModel);
    }


    [Authorize(Policy = "ManagerOnly")]
    [HttpPost]
    public IActionResult MoveRouteDown(int selectedLoopId, int routeId)
    {
        var loops = loopService.getAllLoops(); 
        var stops = stopService.getAllStops();
        var selectedLoop = loopService.getLoopById(selectedLoopId); 
        var routes = routeService.getRoutesByLoopId(selectedLoopId); 
        routeService.DecreaseRouteOrder(routeId);
        _logger.LogInformation("routeId: " + routeId);
        var viewModel = RoutesViewModel.FromLoopID(routes, loops, selectedLoop, stops);
        return View(viewModel);
    }


    /*STOPS*/
    [Authorize(Policy = "ManagerOnly")]
    public IActionResult Stops()
    {
        return View(stopService.getAllStops().Select(s=>StopViewModel.FromStop(s)));
    }


    [Authorize(Policy = "ManagerOnly")]
    public IActionResult UpdateStop([FromRoute] int id)
    {
        var stopEditModel =  new StopEditModel();
        var stop = stopService.findStopById(id);

        stopEditModel = StopEditModel.FromStop(stop);
        return View(stopEditModel);
    }


    [Authorize(Policy = "ManagerOnly")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateStop(int id, [Bind("Name")] StopEditModel stop)
    {
        stopService.UpdateStopById(id, stop.Name);
        return RedirectToAction("Stops");
    }

    [Authorize(Policy = "ManagerOnly")]
    public IActionResult CreateStop()
    {
        var stopCreateModel = StopCreateModel.NewStop(stopService.GetAmountOfStops());
        return View(stopCreateModel);
    }


    [Authorize(Policy = "ManagerOnly")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateStop(int id, [Bind("Name,Latitude,Longitude")] StopCreateModel stop)
    {
        stopService.CreateNewStop(id, stop.Name, stop.Latitude, stop.Longitude);
        return RedirectToAction("Stops");
    }



    /*LOOPS*/

    [Authorize(Policy = "ManagerOnly")]
    public IActionResult Loops()
    {
        return View(loopService.getAllLoops().Select(l => LoopViewModel.FromLoop(l)));
    }

    [Authorize(Policy = "ManagerOnly")]
    public IActionResult UpdateLoop([FromRoute] int id)
    {
        var loopEditModel =  new LoopEditModel();
        var loop = loopService.getLoopById(id);

        loopEditModel = LoopEditModel.FromLoop(loop);
        return View(loopEditModel);
    }


    [Authorize(Policy = "ManagerOnly")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateLoop(int id, [Bind("Name")] LoopEditModel loop)
    {
        loopService.UpdateLoopById(id, loop.Name);
        return RedirectToAction("Loops");
    }

    [Authorize(Policy = "ManagerOnly")]
    public IActionResult CreateLoop()
    {
        var loopCreateModel = LoopCreateModel.NewLoop(loopService.GetAmountOfLoops());
        return View(loopCreateModel);
    }


    [Authorize(Policy = "ManagerOnly")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateLoop(int id, [Bind("Name")] LoopCreateModel loop)
    {
        loopService.CreateNewLoop(id, loop.Name);
        return RedirectToAction("Loops");
    }


    /*ENTRIES*/
    [Authorize(Policy = "ManagerOnly")]
    [HttpGet]
    public IActionResult Entries()
    {
        var entries = entryService.getAllEntries().ToList();
        var viewModels = entries.Select(entry =>
        {
            var loop = loopService.getLoopById(entry.LoopId);
            var driver = driverService.findDriverById(entry.DriverId);
            var stop = stopService.findStopById(entry.StopId);
            var bus = busService.findBusById(entry.BusId);

            return EntryViewModel.FromEntry(entry, loop, driver, stop, bus);
        }).ToList();

        return View(viewModels);
    }


    [Authorize(Policy = "ManagerOnly")]
    [HttpGet]
    public IActionResult SearchEntries([FromQuery]DateTime dateTime)
    {
        var entries = entryService.getEntriesByDate(dateTime).ToList();
        _logger.LogInformation("Entered time:" + dateTime);
        var viewModels = entries.Select(entry =>
        {
            var loop = loopService.getLoopById(entry.LoopId);
            var driver = driverService.findDriverById(entry.DriverId);
            var stop = stopService.findStopById(entry.StopId);
            var bus = busService.findBusById(entry.BusId);

            return EntryViewModel.FromEntry(entry, loop, driver, stop, bus);
        }).ToList();

        return View(viewModels);
    }
}