using ePizzaHub.Entities;
using ePizzaHub.Models;
using ePizzaHub.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ePizzaHub.Repositories.Impelmentations
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private AppDbContext context
        {
            get
            {
                return _db as AppDbContext;
            }
        }

        public CartRepository(AppDbContext db):base(db)
        {

        }
        public Cart GetCart(Guid CartId)
        {
            return context.Carts.Include("Items").Where(p => p.Id == CartId && p.IsActive == true).FirstOrDefault();
        }

        public int DeleteItem(Guid cartId, int itemId)
        {
            var item = context.CartItems.Where(ci => ci.CartId == cartId && ci.Id == itemId).FirstOrDefault();
            if (item != null)
            {
                context.CartItems.Remove(item);
                return context.SaveChanges();
            }
            else
            {
                return 0;
            }
        }
        public CartModel GetCartDetails(Guid CartId)
        {
            var model = (from cart in context.Carts
                         where cart.Id == CartId && cart.IsActive == true
                         select new CartModel
                         {
                             Id = cart.Id,
                             UserId = cart.UserId,
                             CreatedDate = cart.CreatedDate,
                             Items = (from cartItem in context.CartItems
                                      join item in context.Items
                                      on cartItem.ItemId equals item.Id
                                      where cartItem.CartId == CartId
                                      select new ItemModel
                                      {
                                          Id = cartItem.Id,
                                          Name = item.Name,
                                          Description = item.Description,
                                          ImageUrl = item.ImageUrl,
                                          Quantity = cartItem.Quantity,
                                          ItemId = item.Id,
                                          UnitPrice = cartItem.UnitPrice
                                      }).ToList()
                         }).FirstOrDefault();
            return model;
        }

        public int UpdateCart(Guid cartId, int userId)
        {
            Cart cart = GetCart(cartId);
            cart.UserId = userId;
            return context.SaveChanges();
        }

        public int UpdateQuantity(Guid cartId, int itemId, int Quantity)
        {
            bool flag = false;
            var cart = GetCart(cartId);
            if (cart != null)
            {
                for (int i = 0; i < cart.Items.Count; i++)
                {
                    if (cart.Items[i].Id == itemId)
                    {
                        flag = true;
                        cart.Items[i].Quantity += (Quantity);
                        break;
                    }
                }
                if (flag)
                    return context.SaveChanges();
            }
            return 0;
        }
    }
}
