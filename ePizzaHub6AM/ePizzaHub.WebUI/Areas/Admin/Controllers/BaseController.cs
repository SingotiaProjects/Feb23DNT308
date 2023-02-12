using Microsoft.AspNetCore.Mvc;
using ePizzaHub.WebUI.Helpers;

namespace ePizzaHub.WebUI.Areas.Admin.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    [Area("Admin")]
    public class BaseController : Controller
    {
        
    }
}
