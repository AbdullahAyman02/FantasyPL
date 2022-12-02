using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace FantasyPL.Pages
{
    public class EditClubModel : PageModel
    {
        public Club clubInfo = new Club();
        public string Message = "";
        Controller controller = new Controller();

        public void OnGet()
        {
        }
        public void OnPost()
        {
            clubInfo.Name = Request.Form["club"];
            clubInfo.Name_Abbreviation = Request.Form["name_abbreviation"];
            clubInfo.Establishment_year = Convert.ToInt32(Request.Form["establishment_year"]);
            clubInfo.City = Request.Form["city"];
            clubInfo.Number_of_Trophies = Convert.ToInt32(Request.Form["number_of_trophies"]);
            clubInfo.Owner_Fname = Request.Form["owner_fname"];
            clubInfo.Owner_Lname = Request.Form["owner_lname"];
            Message = controller.UpdateClub(clubInfo);
        }
    }
}
