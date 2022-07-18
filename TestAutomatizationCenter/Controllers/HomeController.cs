using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestAutomatizationCenter.Models;
using System.Security.Cryptography;
using System.Text;

namespace TestAutomatizationCenter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.Users.ToList());
        }

        public IActionResult Chat()
        {
            var chatContent = new ChatContent();

            foreach(var user in _db.Users.ToArray())
            {
                chatContent.Users.Add(user.Login);
            }

            chatContent.Messages.AddRange(_db.Messages.ToArray());

            return View(chatContent);
        }

        private static string GetHashSha256(string text)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += string.Format("{0:x2}", x);
            }
            return hashString;
        }

        public async Task<IActionResult> AddUser(User user)
        {
            user.Password = GetHashSha256(user.Password);
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SendMessage(string text, string login)
        {
            var user = _db.Users.FirstOrDefault(x => x.Login == login);
            
            if (user == null)
            {
                return BadRequest();
            }

            var message = new Message()
            {
                Text = text,
                TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                User = user
            };

            await _db.Messages.AddAsync(message);
            await _db.SaveChangesAsync();

            return Ok(message);
        }

        public IActionResult GetMessages()
        {
            return Ok(_db.Messages.ToArray());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
