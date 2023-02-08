using AspNetCore.ReportingServices.RdlExpressions.ExpressionHostObjectModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FantasyPL.Pages
{
    public class InsertPlayerModel : PageModel
    {
        player playerinfo = new player();
        public string Message = "";
        Controller controller = new Controller();

        public void OnGet()
        {
            GlobalVar.playerQueried = playerinfo;
        }

        public void OnPost()
        { 
            string clubAbbr = Request.Form["club3"];
            if(clubAbbr == null)
            {
                Message = "Please Specify a Club.";
                return;
            }
            int playerNo = Convert.ToInt16(Request.Form["number"]);
            playerinfo.Club_Abbreviation = clubAbbr;
            playerinfo.Player_Number = playerNo;
            playerinfo.Fname = Request.Form["Fname"];
            var result = playerinfo.Fname.All(Char.IsLetter);
            playerinfo.Mname = Request.Form["Mname"];
            var result1 = playerinfo.Mname.All(Char.IsLetter);
            playerinfo.Lname = Request.Form["Lname"];
            var result2 = playerinfo.Lname.All(Char.IsLetter);
            if(!result || !result1 || !result2)
            {
                Message = "Name must contain letters only";
                return;
            }
            playerinfo.Price = Convert.ToInt32(Request.Form["price"]);
            playerinfo.Age = Convert.ToInt16(Request.Form["age"]);
            playerinfo.Height = Convert.ToInt16(Request.Form["height"]);
            playerinfo.Weight = Convert.ToInt16(Request.Form["weight"]);
            playerinfo.Nationality = Request.Form["nationality"];
            playerinfo.Debut_Year = Convert.ToInt16(Request.Form["debut"]);
            playerinfo.Contract_Length = Convert.ToInt16(Request.Form["contract"]);
            playerinfo.Points = Convert.ToInt16(Request.Form["points"]);
            playerinfo.Position = Request.Form["position"];
            playerinfo.FPLcode = Convert.ToInt32(Request.Form["fplcode"]);
            Message = controller.InsertPlayer(playerinfo);
            controller.UpdatePlayersList();
        }
    }
}
