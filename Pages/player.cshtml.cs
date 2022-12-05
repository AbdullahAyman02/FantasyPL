using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FantasyPL.Pages
{
    public class playerModel : PageModel
    {
        Controller controller = new Controller();
        public void OnGet()
        {
            controller.UpdatePlayersList();
            GlobalVar.playerQueried = GlobalVar.listPlayers[0];
        }
        public void OnPost()
        {
            string playerName = Request.Form["player"];
            GlobalVar.playerQueried = controller.SelectPlayerByName(playerName);
        }

    }
    public class player
    {
        public string Fname { get; set; }
        public string Mname { get; set; }
        public string Lname { get; set; }
        public int Price { get; set; }
        public int Age { get; set; }
        public int Player_Number { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public string Nationality { get; set; }
        public int Debut_Year { get; set; }
        public int Contract_Length { get; set; }
        public string Club_Abbreviation { get; set; }
        public int Points { get; set; }
    }
}
