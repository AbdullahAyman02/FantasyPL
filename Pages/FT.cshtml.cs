using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

namespace FantasyPL.Pages
{
    public class FTModel : PageModel
    {
        public string Message = "";
        Controller controller = new Controller();
        public void OnGet()
        {
            controller.UpdatePlayersList();
            controller.SelectPlayersByUsername(GlobalVar.LoggedInUser.Username);
        }

        public void OnPost()
        {
            string btnvalue = Request.Form["Delete Player"];
            if (btnvalue != null)
            {
                string[] value1 = Request.Form["player2"].ToString().Split(" ");
                string abbr1 = value1[0];
                int no1 = Convert.ToInt32(value1[1]);
                Message = controller.DeleteFTplayer(GlobalVar.LoggedInUser.Username, abbr1, no1);
                GlobalVar.LoggedInUser.Balance = controller.GetBalanceOfUser(GlobalVar.LoggedInUser.Username);
                return;
            }
            string[] value = Request.Form["player"].ToString().Split(" ");
            string abbr = value[0];
            int no = Convert.ToInt32(value[1]);
            player p = controller.SelectPlayer(abbr, no);
            int count = controller.CountPositionforUsername(GlobalVar.LoggedInUser.Username, p.Position);
            if ((p.Position == "GoalKeeper" && (count >= 1)) || (p.Position != "GoalKeeper" && (count >= 4)) || GlobalVar.userPlayers.Count >= 11)
            {
                Message = "Cannot Add Player either because your team is full or you have selected max no. of players for this position already";
                return;
            }
            if(controller.GetPriceOfPlayer(abbr, no) > controller.GetBalanceOfUser(GlobalVar.LoggedInUser.Username))
            {
                Message = "You do not have enough balance.";
                return;
            }
            Message = controller.InsertFTplayer(GlobalVar.LoggedInUser.Username, abbr, no);
            GlobalVar.LoggedInUser.Balance = controller.GetBalanceOfUser(GlobalVar.LoggedInUser.Username);
        }
    }
}
