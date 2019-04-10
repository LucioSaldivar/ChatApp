using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChatApp.Models;
using PusherServer;

namespace ChatApp.Controllers
{
    public class AuthController : Controller
    {
        [HttpPost]
        public ActionResult Login()
        {
            string user_name = Request.Form["username"];
            if (user_name.Trim() == "")
            {
                return Redirect("/");
            }

            using (var db = new Models.ChatContext())
            {
                User user = db.Users.FirstOrDefault(u => u.name == user_name);

                if (user == null)
                {
                    user = new User { name = user_name };

                    db.Users.Add(user);
                    db.SaveChanges();
                }
                Session["user"] = user;
            }
            return Redirect("/chat");
        }
        //In the code above, we check if a user exists using the name. If it exists we retrieve the user’s details and, if it doesn’t, we create a new record first.
        //Then we assign the user’s details into a session object for use throughout the application. Lastly, we redirect the user to the chat page.

        public JsonResult AuthForChannel(string channel_name, string socket_id)
        {
            if (Session["user"] == null)
            {
                return Json(new { status = "error", message = "User is not logged in" });
            }
            var currentUser = (Models.User)Session["user"];

            var options = new PusherOptions();
            options.Cluster = "PUSHER_APP_CLUSTER";

            var pusher = new Pusher(
            "PUSHER_APP_ID",
            "PUSHER_APP_KEY",
            "PUSHER_APP_SECRET", options);

            if (channel_name.IndexOf(currentUser.id.ToString()) == -1)
            {
                return Json(
                  new { status = "error", message = "User cannot join channel" }
                );
            }

            var auth = pusher.Authenticate(channel_name, socket_id);

            return Json(auth);
        }
        // GET: Auth
        public ActionResult Index()
        {
            return View();
        }
    }
}