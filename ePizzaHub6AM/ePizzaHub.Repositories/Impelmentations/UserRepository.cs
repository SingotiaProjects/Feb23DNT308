using ePizzaHub.Entities;
using ePizzaHub.Models;
using ePizzaHub.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;

namespace ePizzaHub.Repositories.Impelmentations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private AppDbContext context
        {
            get
            {
                return _db as AppDbContext;
            }
        }
        public UserRepository(AppDbContext db) : base(db)
        {

        }
        public bool CreateUser(User user, string Role)
        {
            try
            {
                user.Password = BC.HashPassword(user.Password);

                Role role = context.Roles.Where(r => r.Name == Role).First();
                user.Roles.Add(role); //adding into UserRoles
                context.Users.Add(user);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public UserModel ValidateUser(string Email, string Password)
        {
            var user = context.Users.Include(u => u.Roles).Where(u => u.Email == Email).FirstOrDefault();
            if (user != null)
            {
                var isVerified = BC.Verify(Password, user.Password);
                if (isVerified)
                {
                    UserModel model = new UserModel();

                    model.Id = user.Id;
                    model.Name = user.Name;
                    model.Email = user.Email;
                    model.PhoneNumber = user.PhoneNumber;
                    model.Roles = user.Roles.Select(r => r.Name).ToArray();
                    return model;
                }
            }
            return null;
        }
    }
}
