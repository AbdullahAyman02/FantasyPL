using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FantasyPL.Pages
{
    public class IndexModel : PageModel
    {
        public User userInfo = new();
        public string successMessage = "";
        public string errorMessage = "";
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public void OnPost()
        {
            userInfo.Username = Request.Form["Username"];
            userInfo.Password = Request.Form["Password"];
            try
            {
                DBManager dBManager = new();
                string sql = "SELECT * FROM Users WHERE USERNAME = @Username AND PASSWORD = @Password";
                using (SqlCommand command = new SqlCommand(sql, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@Username", userInfo.Username);
                    command.Parameters.AddWithValue("@Password", userInfo.Password);
                    SqlDataReader reader = dBManager.ExecuteReader(command);
                    if (reader.HasRows)
                        successMessage = "Logged in successfully";
                    else
                        errorMessage = "User not registered";
                }
            } catch (Exception ex)
            {
                errorMessage = ex.Message;
                successMessage = "";
            }
        }
    }
}