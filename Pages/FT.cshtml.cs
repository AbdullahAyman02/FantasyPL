using FplClient.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System.Net;

namespace FantasyPL.Pages
{
    public class FTModel : PageModel
    {
        public string Message = "";
        Controller controller = new Controller();
        public async void OnGet()
        {
            controller.UpdatePlayersList();
            controller.SelectPlayersByUsername(GlobalVar.LoggedInUser.Username);
            GlobalVar.statusFT = controller.GetFT();
            GlobalVar.LoggedInUser.Points = controller.GetPoints(GlobalVar.LoggedInUser.Username);
            //var jObj = JObject.Parse("fpl.json");
            //var playername = jObj["elements"]["first_name"];
            //var result = players.OfType<JProperty>().Where((a, b) => {
            //    return true;
            //}).Select<JProperty, playerFT>((jp, i) => {
            //    return new playerFT
            //    {
            //        Id = jp.Value["id"].ToObject<string>(),
            //        Desc = jp.Value["description"].ToObject<string>()
            //    };
            //}).ToArray();
            //var client = new FplEntryClient(new HttpClient());
            //var playerData = await client.GetTeam(teamID: 12345, gameweek: 1);
        }

        public void OnPost()
        {
            string btnvalue = Request.Form["Delete Player"];
            if (btnvalue != null)
            {
                string test2 = Request.Form["player2"];
                if (test2 == null)
                {
                    Message = "No Player selected";
                    return;
                }
                string[] value1 = Request.Form["player2"].ToString().Split(" ");
                string abbr1 = value1[0];
                int no1 = Convert.ToInt32(value1[1]);
                Message = controller.DeleteFTplayer(abbr1, no1, GlobalVar.LoggedInUser.Username);
                GlobalVar.LoggedInUser.Balance = controller.GetBalanceOfUser(GlobalVar.LoggedInUser.Username);
                return;
            }
            string test = Request.Form["player"];
            if (test == null)
            {
                Message = "No Player selected";
                return;
            }
            string[] value = Request.Form["player"].ToString().Split(" ");
            string abbr = value[0];
            int no = Convert.ToInt32(value[1]);
            player p = controller.SelectPlayer(abbr, no);
            if (controller.GetPriceOfPlayer(abbr, no) > controller.GetBalanceOfUser(GlobalVar.LoggedInUser.Username))
            {
                Message = "You do not have enough balance.";
                return;
            }
            int count = controller.CountPositionforUsername(GlobalVar.LoggedInUser.Username, "GoalKeeper");
            if ((p.Position != "GoalKeeper" && !(count >= 1)) && GlobalVar.userPlayers.Count >= 10)
            {
                Message = "You need to have 1 GoalKeeper in your team";
                return;
            }
            count = controller.CountPositionforUsername(GlobalVar.LoggedInUser.Username, p.Position);
            if ((p.Position == "GoalKeeper" && (count >= 1)) || (p.Position != "GoalKeeper" && (count >= 4)) || GlobalVar.userPlayers.Count >= 11)
            {
                Message = "Cannot Add Player either because your team is full or you have selected max no. of players for this position already";
                return;
            }
            Message = controller.InsertFTplayer(GlobalVar.LoggedInUser.Username, abbr, no);
            GlobalVar.LoggedInUser.Balance = controller.GetBalanceOfUser(GlobalVar.LoggedInUser.Username);
            GlobalVar.LoggedInUser.Points = controller.GetPoints(GlobalVar.LoggedInUser.Username);
        }
    }

    class playerFT{
        public string FName { get; set; }
        public string SName { get; set; }
        public string Club { get; set; }
        public string Code { get; set; }

        }
}
