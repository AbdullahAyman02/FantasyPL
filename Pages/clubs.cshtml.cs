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
            if (GlobalVar.listClubs.Count > 0) {
                GlobalVar.clubQueried = GlobalVar.listClubs[0];
                GlobalVar.clubQueried.Stadium = controller.SelectStadiumByAbbr(GlobalVar.listClubs[0].Name_Abbreviation);
                GlobalVar.clubQueried.Manager = controller.SelectManagerByAbbr(GlobalVar.listClubs[0].Name_Abbreviation); 
            } else
            {
                GlobalVar.clubQueried = new();
            }
		}
        public void OnPost()
        {
            string clubname = Request.Form["club"];
            GlobalVar.clubQueried = controller.SelectClubByName(clubname);
            GlobalVar.clubQueried.Stadium = controller.SelectStadiumByAbbr(clubname);
            GlobalVar.clubQueried.Manager = controller.SelectManagerByAbbr(clubname);
        }
    }

    public class Club
    {
        public int Position { get; set; }
        public string Name { get; set; }
        public string Name_Abbreviation { get; set; }

        public string Stadium { get; set; }
        public string Manager { get; set; }
            
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

    public class Manager
    {
        public int ID { get; set; }
        public string FName { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }
        public int age { get; set; }
        public string nationality { get; set; }
        public int competitions_won { get; set; }
        public int experience_in_years { get; set; }
        public string club_managed { get; set; }
    }
}
