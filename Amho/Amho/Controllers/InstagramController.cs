using InstagramApiSharp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.Logger;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using InstagramApiSharp;
using Microsoft.Ajax.Utilities;
using DataLayer;
using DataLayer.Models;

namespace Amho.Controllers
{
    [Authorize]
    public class InstagramController : Controller
    {
        // GET: Instagram
        private static UserSessionData user;
        public static string currentUser;

        public ActionResult Index()
        {
            if (user != null)
            {
                var user = ctx.api.GetLoggedUser().LoggedInUser;
                ViewBag.User = user;


                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> DoLogin(LoginInstagramViewMoedel login)
        {
            user = new UserSessionData();
            //It must change

            user.UserName = login.UserName;
            user.Password = login.Password;
            //user.UserName = "mr_biime";
            //user.UserName = "pezeshkafzar";
            //user.Password = "810995107";
            //It must change

            ctx.api = InstaApiBuilder.CreateBuilder()
                .SetUser(user)
                .UseLogger(new DebugLogger(LogLevel.All))
                .SetRequestDelay(RequestDelay.FromSeconds(0, 1))
                .Build();

            var loginRequest = await ctx.api.LoginAsync();
            if (loginRequest.Succeeded)
            {
                HttpCookie logedinUser = new HttpCookie("LogedInUser", login.UserName) {
                    Expires = DateTime.Now.AddHours(1),
                    HttpOnly = true,
                    Secure = true
                };
                
                Response.Cookies.Add(logedinUser);
                currentUser = login.UserName;
                return RedirectToAction("index");
                //var res = await ctx.api.UserProcessor.GetCurrentUserFollowersAsync(PaginationParameters.MaxPagesToLoad(6));
            }
            else
            {
                return View("Login");
            }
        }

        public ActionResult DoLogin()
        {
            return RedirectToAction("index");
        }

        public async Task<ActionResult> GetFollowers(string username)
        {
            var followers = await ctx.api.UserProcessor.GetCurrentUserFollowersAsync(PaginationParameters.MaxPagesToLoad(6));
            ViewBag.Followers = followers;
            return PartialView();
        }

        public async Task<ActionResult> ShowUser(string username)
        {
            var user = await ctx.api.UserProcessor.GetUserAsync(username);
            ViewBag.User = user;
            return PartialView();
        }

        public ActionResult FollowUserFollowerModal(bool isFollow)
        {
            ViewBag.IsFollow = isFollow;
            return PartialView();
        }

        public static int MakeRandomDelay(int a, int b)
        {
            Random rnd = new Random();
            int delay = rnd.Next(a, b);
            return delay;
        }

        [HttpPost]
        public async Task<ActionResult> FollowUserFollower(string username, int count)
        {
            int skip = 0;
            int followed = 0;
            var myFollowers = await ctx.api.UserProcessor.GetCurrentUserFollowersAsync(PaginationParameters.MaxPagesToLoad(8));
            var userFollowers = await ctx.api.UserProcessor.GetUserFollowersAsync(username, PaginationParameters.MaxPagesToLoad(8));
            if (myFollowers.Succeeded && userFollowers.Succeeded)
            {
                for (int i = 0; i < count + skip; i++)
                {
                    if (!myFollowers.Value.Any(f => f.Pk == userFollowers.Value[i].Pk))
                    {
                        var follow = await ctx.api.UserProcessor.FollowUserAsync(userFollowers.Value[i].Pk);
                        int delay = MakeRandomDelay(0, 4);
                        await Task.Delay(delay);
                        followed++;
                    }
                    else
                    {
                        skip++;
                    }
                }
                ViewBag.IsFollow = true;
                ViewBag.Count = followed;
                return PartialView();
            }
            else
            {
                return RedirectToAction("index");
            }
        }

        [HttpPost]
        public async Task<ActionResult> UnfollowMyFollowings(int count)
        {
            var myFollowings = await ctx.api.UserProcessor.GetUserFollowingAsync(currentUser, PaginationParameters.MaxPagesToLoad(8));
            if (myFollowings.Succeeded)
            {

                for (int i = count - 1; i >= 0; i--)
                {
                    //if (!db.Followings.Any(f=>f.PK==myFollowings.Value[i].Pk))
                    //{
                    var unfollow = await ctx.api.UserProcessor.UnFollowUserAsync(myFollowings.Value[i].Pk);
                    int delay = MakeRandomDelay(0, 4);
                    await Task.Delay(delay);
                    //}

                }
                ViewBag.IsFollow = false;
                ViewBag.Count = count;
                return PartialView("FollowUserFollower");
            }
            return RedirectToAction("index");
        }

        public async Task<ActionResult> ShowFollowingWhichNotFollowers()
        {
            var user = ctx.api.UserProcessor;
            var myFollowers = await user.GetCurrentUserFollowersAsync(PaginationParameters.MaxPagesToLoad(10));
            var myFollowings = await user.GetUserFollowingAsync(currentUser, PaginationParameters.MaxPagesToLoad(10));
            List<dynamic> list = new List<dynamic>();

            foreach (var item in myFollowings.Value)
            {
                if (!myFollowers.Value.Any(u => u.Pk == item.Pk))
                {
                    list.Add(item);
                }
            }
            ViewBag.List = list;
            var count = list.Count();
            ViewBag.Count = count;
            return PartialView();
        }

        public async Task<ActionResult> ShowUserPosts(string username)
        {
            var userMedia = await ctx.api.UserProcessor.GetUserMediaAsync(username, PaginationParameters.MaxPagesToLoad(8));
            ViewBag.List = userMedia;

            return PartialView();
        }

        public async Task<bool> FollowUser(string username)
        {
            bool result = false;
            var user = ctx.api.UserProcessor.GetUserAsync(username);
            var follow = await ctx.api.UserProcessor.FollowUserAsync(user.Result.Value.Pk);
            result = follow.Succeeded;
            return result;
        }

        public async Task<bool> UnFollowUser(string username)
        {
            bool result = false;
            var user = await ctx.api.UserProcessor.GetUserAsync(username);
            var unfollow = await ctx.api.UserProcessor.UnFollowUserAsync(user.Value.Pk);
            result = unfollow.Succeeded;
            return result;
        }

    }
}