using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace FantasyPL.Pages
{
    public class InsertClubModel : PageModel
    {
        public Club clubInfo = new Club();
        public string Message = "";
        Controller controller = new Controller();

        public void OnGet()
        {
            controller.UpdateStadiumsList();
            controller.UpdateManagersList();
        }
        public void OnPost()
        {
            clubInfo.Name = Request.Form["name"];
            clubInfo.Name_Abbreviation = Request.Form["name_abbreviation"];
            var match1 = clubInfo.Name_Abbreviation.All(Char.IsLetter);
			clubInfo.Establishment_year = Convert.ToInt32(Request.Form["establishment_year"]);
            clubInfo.City = Request.Form["city"];
            clubInfo.Number_of_Trophies = Convert.ToInt32(Request.Form["number_of_trophies"]);
            clubInfo.Owner_Fname = Request.Form["owner_fname"];
            var match2 = clubInfo.Owner_Fname.All(Char.IsLetter);
			clubInfo.Owner_Lname = Request.Form["owner_lname"];
            var match3 = clubInfo.Owner_Lname.All(Char.IsLetter);
			clubInfo.Stadium = Request.Form["stadium"];
            clubInfo.Manager = Request.Form["manager"];
            if(!match1 || !match2 || !match3)
            {
				Message = "Name must contain letters only";
				return;
			}

            if(clubInfo.Manager == null || clubInfo.Stadium == null)
            {
                Message = "Please select a stadium and a manager. If either list is empty, insert new ones first";
                return;
            }
			Message = controller.InsertClub(clubInfo);
            if(Message == "Club was added Successfully")
            {
                controller.SetStadium(clubInfo.Name_Abbreviation, Request.Form["stadium"]);
                controller.SetManager(clubInfo.Name_Abbreviation, Convert.ToInt32(Request.Form["manager"]));
                controller.UpdateStadiumsList();
                controller.UpdateManagersList();
            }
            controller.UpdateClubsList();
        }
    }
}
