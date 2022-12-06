using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FantasyPL.Pages
{
    public class EditPlayerModel : PageModel
    {
        player playerinfo = new player();
        public string Message = "";
        Controller controller = new Controller();
        public void OnGet()
        {
            controller.UpdateClubsList();
        }

        public void OnPost()
        {
            string btnvalue = Request.Form["Refresh"];
            if (btnvalue != null)
            {
                controller.SelectPlayersByClubAbbr(Request.Form["club3"]);
                if (GlobalVar.clubPlayers.Count > 0)
                {
                    GlobalVar.playerQueried = GlobalVar.clubPlayers[0];
                } else
                {
                    player p = new player();
                    p.Fname = "NA";
                    p.Mname = "NA";
                    p.Lname = "NA";
                    p.Club_Abbreviation = Request.Form["club3"];
                    GlobalVar.playerQueried = p;
                }
                return;
            }
            string clubAbbr = Request.Form["club3"];
            int playerNo = Convert.ToInt16(Request.Form["player2"]);
            string btnvalue1 = Request.Form["Refresh2"];
            if (btnvalue1 != null)
            {
                player player = controller.SelectPlayer(clubAbbr, playerNo);
                GlobalVar.playerQueried = player;
                return;
            }
            playerinfo.Club_Abbreviation = clubAbbr;
            playerinfo.Player_Number = playerNo;
            playerinfo.Fname = Request.Form["Fname"];
            playerinfo.Mname = Request.Form["Mname"];
            playerinfo.Lname = Request.Form["Lname"];
            playerinfo.Price = Convert.ToInt32(Request.Form["price"]);
            playerinfo.Age = Convert.ToInt16(Request.Form["age"]);
            playerinfo.Height = Convert.ToInt16(Request.Form["height"]);
            playerinfo.Weight = Convert.ToInt16(Request.Form["weight"]);
            playerinfo.Nationality = Request.Form["nationality"];
            playerinfo.Debut_Year = Convert.ToInt16(Request.Form["debut"]);
            playerinfo.Contract_Length = Convert.ToInt16(Request.Form["contract"]);
            playerinfo.Points = Convert.ToInt16(Request.Form["points"]);
            Message = controller.UpdatePlayer(playerinfo);
            if (Message == "Player was added Successfully")
            {
                GlobalVar.playerQueried = playerinfo;
                controller.SelectPlayersByClubAbbr(playerinfo.Club_Abbreviation);
            }
        }
    }
}
