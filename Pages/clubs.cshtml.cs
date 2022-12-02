using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
namespace FantasyPL.Pages
{
    public class clubsModel : PageModel
    {
        Controller controller = new Controller();
        public void OnGet()
        {
            controller.UpdateClubsList();
            GlobalVar.clubQueried = GlobalVar.listClubs[0];
        }
        public void OnPost()
        {
            string clubname = Request.Form["club"];
            GlobalVar.clubQueried = controller.SelectClubByName(clubname);
        }
    }

    public class Club
    {
        public int Postition { get; set; }
        public string Name { get; set; }
        public string Name_Abbreviation { get; set; }
        public int Establishment_year { get; set; }
        public string City { get; set; }
        public int? Number_of_Trophies { get; set; }
        public string Owner_Fname { get; set; }
        public string Owner_Lname { get; set; }
        public int Points { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int Goals_For { get; set; }
        public int Goals_Against { get; set; }

    }
}
