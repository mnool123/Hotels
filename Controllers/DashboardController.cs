using Hotels.Data;
using Hotels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;


namespace Hotels.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> SendEmail()
        {
            var Message = new MimeMessage();
            Message.From.Add(new MailboxAddress("Test Message", "mnalsalh8090@gmail.com"));
            Message.To.Add(MailboxAddress.Parse("fajarsaleh70@gmail.com"));
            Message.Subject = "Test EMail From My Project in Asp.net Core MVC";
            Message.Body = new TextPart("Plain")
            {
                Text = "Welcome In My App"
            };
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect("smtp.gmail.com", 587);
                    client.Authenticate("mnalsalh8090@gmail.com", "woevtbmmxxppfuxe");
                    await client.SendAsync(Message);
                    client.Disconnect(true);
                }
                catch (Exception e)
                {
                    return e.Message.ToString();
                }
            }
            return "Ok";
        }

        public IActionResult Delete(int id)
        {
            var hotelDel = _context.hotel.SingleOrDefault(x => x.Id == id);
            if (hotelDel != null)

            {
                _context.hotel.Remove(hotelDel);
                _context.SaveChanges();
                TempData["Del"] = "Ok";
            }
            return RedirectToAction("Index");
        }
		public IActionResult CreateNewRooms(Rooms rooms)
		{
			_context.rooms.Add(rooms);
			_context.SaveChanges();
			return RedirectToAction("Rooms");
		}
		public IActionResult CreateNewRoomDetails(RoomDetails roomDetails)
		{
			_context.roomDetails.Add(roomDetails);
			_context.SaveChanges();
			return RedirectToAction("RoomDetails");
		}
		[HttpPost]
        public async Task<IActionResult> Index(string city)
        {
            var hotel = _context.hotel.Where(x => x.City.Equals(city));
            return View(hotel);
        }
		public async Task<IActionResult> RoomDetails()
		{
			var hotel = _context.hotel.ToList();
			ViewBag.hotel = hotel;

			var rooom = _context.rooms.ToList();
			ViewBag.rooms = rooom;

			var roomdd = _context.roomDetails.ToList();
			return View(roomdd);

		}
		public IActionResult Rooms()
		{
			var hotel = _context.hotel.ToList();
			ViewBag.hotel = hotel;
            //ViewBag.currentuser = Request.Cookies["UserName"];
            ViewBag.currentuser = HttpContext.Session.GetString("UserName");
            var rooms = _context.rooms.ToList();

            return View(rooms);

		}
        public IActionResult Edit(int id)
        {
            var hoteledit = _context.hotel.SingleOrDefault(x => x.Id == id);

            return View(hoteledit);
        }
        public IActionResult Update(Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                _context.hotel.Update(hotel);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Edit");
        }
        //woevtbmmxxppfuxe
        [Authorize]
		public IActionResult Index()
        {
            var currentuser = HttpContext.User.Identity.Name;
            ViewBag.currentuser = currentuser;
            //CookieOptions option = new CookieOptions();
            //option.Expires = DateTime.Now.AddMinutes(20);
            //Response.Cookies.Append("UserName", currentuser, option);
            HttpContext.Session.SetString("UserName", currentuser);
            var hotel = _context.hotel.ToList();
            return View(hotel);
        }
        public IActionResult CreateNewHotel(Hotel hotels)
        {
            if (ModelState.IsValid)
            {
                _context.hotel.Add(hotels);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            var hotel = _context.hotel.ToList();
            return View("Index", hotel);
        }
    }
}
