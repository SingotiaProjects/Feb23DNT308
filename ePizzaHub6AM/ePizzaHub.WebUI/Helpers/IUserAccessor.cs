using ePizzaHub.Models;

namespace ePizzaHub.WebUI.Helpers
{
    public interface IUserAccessor
    {
        UserModel GetUser();
    }
} 