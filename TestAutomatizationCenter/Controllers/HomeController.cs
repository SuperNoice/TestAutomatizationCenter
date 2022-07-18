using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestAutomatizationCenter.Models;
using TestAutomatizationCenter.Utils;

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
            try
            {
                return View(_db.Users.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Index: ");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public IActionResult Chat()
        {
            try
            {
                var chatContent = new ChatContent();

                foreach (var user in _db.Users.ToArray())
                {
                    chatContent.Users.Add(user.Login);
                }

                chatContent.Messages.AddRange(_db.Messages.ToArray());

                return View(chatContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Chat: ");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<IActionResult> AddUser(User user)
        {
            try
            {
                user.Name = user.Name.Trim();
                user.Login = user.Login.Trim();
                user.Password = user.Password.Trim();

                if (user.Name == "" || user.Login == "" || user.Password == "")
                {
                    return BadRequest();
                }

                user.Password = Cryptography.GetHashSha256(user.Password);
                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddUser: ");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<IActionResult> SendMessage(string text, string login)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "SendMessage: ");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public IActionResult GetMessages()
        {
            try
            {
                return Ok(_db.Messages.ToArray());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetMessages: ");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
