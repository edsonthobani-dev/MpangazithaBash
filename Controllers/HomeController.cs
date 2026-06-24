using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MpangazithaBash.DAL;
using MpangazithaBash.Models.Home;

namespace MpangazithaBash.Controllers
{
    public class HomeController : Controller
    {
        dbMyOnlineShoppingEntities context = new dbMyOnlineShoppingEntities();
        public ActionResult Index(string search, int page = 1)
        {
            ViewBag.Search = search;
            HomeIndexViewModel model = new HomeIndexViewModel();
            return View(model.CreateModel(search, page));
        }
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Location()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddToCart(int productId)
        {
            List<item> cart;

            if (Session["cart"] == null)
            {
                cart = new List<item>();
            }
            else
            {
                cart = (List<item>)Session["cart"];
            }

            var product = context.Tbl_Product.Find(productId);

            // check if product already in cart
            item existingItem = cart.FirstOrDefault(i => i.tbl_product.ProductId == productId);

            if (existingItem != null)
            {
                // ✅ already in cart - just increase quantity
                existingItem.Quantity++;
            }
            else
            {
                // ✅ not in cart - add new
                cart.Add(new item()
                {
                    tbl_product = product,
                    Quantity = 1
                });
            }

            Session["cart"] = cart;
            return RedirectToAction("Index");
        }

        public ActionResult Cart()
        {
            if (Session["cart"] == null)
            {
                return View(new List<item>());
            }
            else
            {
                List<item> cart = (List<item>)Session["cart"];
                return View(cart);
            }
        }
        public ActionResult RemoveFromCart(int productId)
        {
            if (Session["cart"] != null)
            {
                List<item> cart = (List<item>)Session["cart"];
                item itemToRemove = cart.FirstOrDefault(i => i.tbl_product.ProductId == productId);

                if (itemToRemove != null)
                {
                    if (itemToRemove.Quantity > 1)
                    {
                        // ✅ reduce quantity by 1 instead of removing completely
                        itemToRemove.Quantity--;
                    }
                    else
                    {
                        // ✅ remove completely if quantity is 1
                        cart.Remove(itemToRemove);
                    }
                    Session["cart"] = cart;
                }
            }
            return RedirectToAction("Cart");
        }

        public ActionResult Checkout()
        {
            if (Session["cart"] == null || (Session["cart"] as List<item>).Count == 0)
            {
                return RedirectToAction("Index");
            }

            // ✅ pass empty model to view
            return View(new CheckoutDetail());
        }

        [HttpPost]
        public ActionResult CheckoutDetails(CheckoutDetail cd)
        {
            // ✅ if logged in skip model validation
            if (Session["member"] != null)
            {
                Tbl_Member loggedIn = (Tbl_Member)Session["member"];

                List<item> cart = (List<item>)Session["cart"];
                foreach (var cartItem in cart)
                {
                    Tbl_Cart tbl_cart = new Tbl_Cart()
                    {
                        ProductId = cartItem.tbl_product.ProductId,
                        MemberId = loggedIn.MemberId,
                        CartStatusId = 1
                    };
                    context.Tbl_Cart.Add(tbl_cart);
                }
                context.SaveChanges();
                Session["cart"] = null;
                return RedirectToAction("CheckoutSuccess");
            }

            // ✅ not logged in - validate form
            if (ModelState.IsValid)
            {
                Tbl_Member member = new Tbl_Member()
                {
                    FirstName = cd.FirstName,
                    LastName = cd.LastName,
                    Email = cd.Email,
                    Password = cd.Password,
                    IsActive = true,
                    IsDelete = false,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                };
                context.Tbl_Member.Add(member);
                context.SaveChanges();
                Session["member"] = member;

                List<item> cart = (List<item>)Session["cart"];
                foreach (var cartItem in cart)
                {
                    Tbl_Cart tbl_cart = new Tbl_Cart()
                    {
                        ProductId = cartItem.tbl_product.ProductId,
                        MemberId = member.MemberId,
                        CartStatusId = 1
                    };
                    context.Tbl_Cart.Add(tbl_cart);
                }
                context.SaveChanges();
                Session["cart"] = null;
                return RedirectToAction("CheckoutSuccess");
            }

            return View("Checkout", cd);
        }

        public ActionResult CheckoutSuccess()
        {
            return View();
        }
        public ActionResult IncreaseQuantity(int productId)
        {
            if (Session["cart"] != null)
            {
                List<item> cart = (List<item>)Session["cart"];
                item existingItem = cart.FirstOrDefault(i => i.tbl_product.ProductId == productId);
                if (existingItem != null)
                {
                    existingItem.Quantity++;
                }
                Session["cart"] = cart;
            }
            return RedirectToAction("Cart");
        }
        public ActionResult Payment()
        {
            return View();
        }

        public ActionResult ProductDetail(int? productId)
        {
            if (productId == null)
                return RedirectToAction("Index");

            var product = context.Tbl_Product.Find(productId);

            if (product == null)
                return HttpNotFound();

            return View(product);
        }
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Tbl_Member member)
        {
            // check if email already exists
            var existing = context.Tbl_Member
                .FirstOrDefault(m => m.Email == member.Email);

            if (existing != null)
            {
                ModelState.AddModelError("Email", "Email already exists!");
                return View(member);
            }

            member.IsActive = true;
            member.IsDelete = false;
            member.CreatedOn = DateTime.Now;
            member.ModifiedOn = DateTime.Now;

            context.Tbl_Member.Add(member);
            context.SaveChanges();

            // auto login after register
            Session["member"] = member;
            return RedirectToAction("Index");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Email, string Password)
        {
            var member = context.Tbl_Member
                .FirstOrDefault(m => m.Email == Email
                    && m.Password == Password
                    && m.IsDelete == false);

            if (member != null)
            {
                Session["member"] = member;
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Invalid email or password!");
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session["member"] = null;
            return RedirectToAction("Index");
        }

        public ActionResult OrderHistory()
        {
            if (Session["member"] == null)
            {
                return RedirectToAction("Login");
            }

            Tbl_Member member = (Tbl_Member)Session["member"];

            var orders = context.Tbl_Cart
                .Where(c => c.MemberId == member.MemberId)
                .ToList();

            return View(orders);
        }
    }
}