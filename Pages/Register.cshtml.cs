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
        public User userInfo = new User();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
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
            userInfo.UserType = "F";

            //save data
            try
            {
                DBManager dBManager = new DBManager();
                String sql = "INSERT INTO Users " +
                             "VALUES(@Username, @Email, @Password, @FirstName, @MiddleName, @LastName, @Birthdate, @UserType, @Gender, @Country, @FantasyTeamName, @FavoriteClub)";

                using (SqlCommand command = new SqlCommand(sql, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@Username", userInfo.Username);
                    command.Parameters.AddWithValue("@Email", userInfo.Email);
                    command.Parameters.AddWithValue("@Password", userInfo.Password);
                    command.Parameters.AddWithValue("@FirstName", userInfo.FirstName);
                    command.Parameters.AddWithValue("@MiddleName", userInfo.MiddleName);
                    command.Parameters.AddWithValue("@LastName", userInfo.LastName);
                    command.Parameters.AddWithValue("@Birthdate", userInfo.Birthdate);
                    command.Parameters.AddWithValue("@UserType", userInfo.UserType);
                    command.Parameters.AddWithValue("@Gender", userInfo.Gender);
                    command.Parameters.AddWithValue("@Country", userInfo.Country);
                    command.Parameters.AddWithValue("@FantasyTeamName", userInfo.FantasyTeamName);
                    command.Parameters.AddWithValue("@FavoriteClub", userInfo.FavoriteClub);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        errorMessage = ex.Message;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                successMessage = "";
                return;
            }

            if(userInfo.FirstName.Length == 0)
                return;

            successMessage = "You have been Registered Successfully!!!!!!!";

            userInfo.FirstName = "";
            userInfo.MiddleName = "";
            userInfo.LastName = "";
            userInfo.FantasyTeamName = "";
            userInfo.FavoriteClub = "";
            userInfo.Country = "";
            userInfo.Email = "";
            userInfo.Username = "";
            userInfo.Password = "";
            userInfo.Gender = "";
            userInfo.Birthdate = "";
            

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
        public string UserType { get; set;}
    }
}
