using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FantasyPL.Pages
{
    public class playerModel : PageModel
    {
        Controller controller = new Controller();
        public string Message = "";
        public void OnGet()
        {
            controller.UpdateClubsList();
            controller.UpdatePlayersList();
            if (GlobalVar.listPlayers.Count > 0)
            {
                GlobalVar.playerQueried = GlobalVar.listPlayers[0];
                controller.SelectPlayersByClubAbbr(GlobalVar.playerQueried.Club_Abbreviation);
            }
            else
                GlobalVar.playerQueried = new();
			
        }
        public void OnPost()
        {
            Message = "";
            string btnvalue = Request.Form["Refresh"];
            if (btnvalue != null)
            {
                controller.SelectPlayersByClubAbbr(Request.Form["club2"]);
                if (GlobalVar.clubPlayers.Count > 0)
                {
                    GlobalVar.playerQueried = GlobalVar.clubPlayers[0];
                }
                else
                {
                    player p = new player();
                    p.Club_Abbreviation = Request.Form["club2"];
                    GlobalVar.playerQueried = p;
                }
                return;
            }
            string clubAbbr = Request.Form["club2"];
            int playerNo = Convert.ToInt16(Request.Form["player"]);
            player player = controller.SelectPlayer(clubAbbr, playerNo);
            if(player.Club_Abbreviation != null)
                GlobalVar.playerQueried = controller.SelectPlayer(clubAbbr, playerNo);
            else
            {
                Message = "No Player with this data";
            }
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
        public string Club { get; set; }
        public int Points { get; set; }
        public string Position { get; set; }
        public int Count { get; set; }  //player number in fantasy team (to show in list of FT players instead of counting one by one)
    }
}
