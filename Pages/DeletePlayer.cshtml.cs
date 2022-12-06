using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FantasyPL.Pages
{
    public class DeletePlayerModel : PageModel
    {
        Controller controller = new Controller();
        public string Message = "";
        public void OnGet()
        {
            controller.UpdateClubsList();
            controller.UpdatePlayersList();
            GlobalVar.playerQueried = GlobalVar.listPlayers[0];
            controller.SelectPlayersByClubAbbr(GlobalVar.playerQueried.Club_Abbreviation);
        }

        public void OnPost()
        {
            Message = "";
            string btn_value = Request.Form["Refresh"];
            if (btn_value != null)
            {
                controller.SelectPlayersByClubAbbr(Request.Form["club3"]);
                if (GlobalVar.clubPlayers.Count > 0)
                    GlobalVar.playerQueried = GlobalVar.clubPlayers[0];
                return;
            }

            string club_abbr = Request.Form["club3"];
            int player_no = Convert.ToInt32(Request.Form["player"]);
            Message = controller.DeletePlayer(club_abbr, player_no);
            if (Message == "Player was Deleted Successfully")
            {
                controller.UpdatePlayersList();
                GlobalVar.playerQueried = GlobalVar.listPlayers[0];
                controller.SelectPlayersByClubAbbr(GlobalVar.playerQueried.Club_Abbreviation);
            }
        }
    }
}
