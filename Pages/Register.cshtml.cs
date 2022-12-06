using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace FantasyPL.Pages
{
    public class RegisterModel : PageModel
    {
        Controller controller = new Controller();
        public User userInfo = new User();
        public string Message = "";

        public void OnGet()
        {
            controller.UpdateClubsList();
        }

        public void OnPost()
        {
            userInfo.Birthdate = null;
            userInfo.FirstName = Request.Form["Fname"];
            userInfo.MiddleName = Request.Form["Mname"];
            userInfo.LastName = Request.Form["Lname"];
            userInfo.FantasyTeamName = Request.Form["FTname"];
            userInfo.Country = Request.Form["country"];
            userInfo.FavoriteClub = Request.Form["favorite_club"];
            userInfo.Email = Request.Form["Email"];
            userInfo.Username = Request.Form["username"];
            string password = Request.Form["password"];
            var sha = SHA256.Create();
            var byteArr = Encoding.Default.GetBytes(password);
            var hashedPasswordByte = sha.ComputeHash(byteArr);
            string hashedPassword = Convert.ToBase64String(hashedPasswordByte);
            userInfo.Password = hashedPassword;
            userInfo.Gender = Request.Form["gender"];
            userInfo.Birthdate = Request.Form["birthday"];
            userInfo.UserType = 'F';
            userInfo.Balance = 100;

            //save data
            Message = controller.InsertUser(userInfo);

            if(userInfo.FirstName.Length == 0)
                return;

            //successMessage = "You have been Registered Successfully!!!!!!!";

            //userInfo.FirstName = "";
            //userInfo.MiddleName = "";
            //userInfo.LastName = "";
            //userInfo.FantasyTeamName = "";
            //userInfo.FavoriteClub = "";
            //userInfo.Country = "";
            //userInfo.Email = "";
            //userInfo.Username = "";
            //userInfo.Password = "";
            //userInfo.Gender = "";
            //userInfo.Birthdate = "";

            // Response.Redirect("/Index");
        }
    }

    public class User
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FantasyTeamName { get; set; }
        public string Country { get; set; }
        public string FavoriteClub { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string? Birthdate { get; set; }
        public char UserType { get; set;}

        public int Balance { get; set; }
    }
}
