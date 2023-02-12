﻿using ePizzaHub.Entities;
using ePizzaHub.Models;
using ePizzaHub.Services.Interfaces;
using ePizzaHub.WebUI.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ePizzaHub.WebUI.Controllers
{
    public class CartController : Controller
    {
        ICartService _cartService;
        IUserAccessor _userAccessor;
        public CartController(ICartService cartService, IUserAccessor userAccessor)
        {
            _cartService = cartService;
            _userAccessor = userAccessor;
        }

        Guid CartId
        {
            get
            {
                Guid Id;
                string CId = Request.Cookies["CId"];
                if (string.IsNullOrEmpty(CId))
                {
                    Id = Guid.NewGuid();
                    //newly added
                    Response.Cookies.Append("CId", Id.ToString(), new CookieOptions { Expires = DateTime.Now.AddDays(1) });
                }
                else
                {
                    Id = Guid.Parse(CId);
                }
                return Id;
            }
        }
        UserModel CurrentUser
        {
            get
            {
                return _userAccessor.GetUser();
            }
        }

        public IActionResult Index()
        {
            CartModel cart = _cartService.GetCartDetails(CartId);
            if (CurrentUser != null && cart != null)
            {
                _cartService.UpdateCart(cart.Id, CurrentUser.Id);
                TempData.Set("Cart", cart);
            }
            return View(cart);
        }

        [Route("Cart/AddToCart/{ItemId}/{UnitPrice}/{Quantity}")]
        public IActionResult AddToCart(int ItemId, decimal UnitPrice, int Quantity)
        {
            int UserId = CurrentUser != null ? CurrentUser.Id : 0;

            if (ItemId > 0 && Quantity > 0)
            {
                Cart cart = _cartService.AddItem(UserId, CartId, ItemId, UnitPrice, Quantity);
                var data = JsonSerializer.Serialize(cart);
                return Json(data);
            }
            else
            {
                return Json("");
            }
        }

        [Route("Cart/UpdateQuantity/{Id}/{Quantity}")]
        public IActionResult UpdateQuantity(int Id, int Quantity)
        {
            int count = _cartService.UpdateQuantity(CartId, Id, Quantity);
            return Json(count);
        }

        public IActionResult DeleteItem(int Id)
        {
            int count = _cartService.DeleteItem(CartId, Id);
            return Json(count);
        }
        public IActionResult GetCartCount()
        {
            int count = _cartService.GetCartCount(CartId);
            return Json(count);
        }

        [HttpGet]
        public IActionResult CheckOut()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CheckOut(AddressModel address)
        {
            TempData.Set("Address", address);
            return RedirectToAction("Index","Payment");
        }
    }
}
