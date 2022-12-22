using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FantasyPL.Pages
{
    public class EditFixModel : PageModel
    {
        public string Message = "";
        Controller controller = new Controller();
        public void OnGet()
        {
            controller.UpdateFixturesList();
            controller.UpdateStadiumsList();
            controller.UpdateRefereesList();
            
			if (GlobalVar.listFixtures.Count > 0)
			{
				GlobalVar.fixtureQueried = GlobalVar.listFixtures[0];
				controller.UpdateFixtureEvents(GlobalVar.fixtureQueried.ID);
			}
			else
			{
				GlobalVar.fixtureQueried = new();
				GlobalVar.fixtureEvents.Clear();
			}
		}

        public void OnPost()
        {
            string btnvalue1 = Request.Form["Refresh"];
            if (btnvalue1 != null)
            {
                int id = Convert.ToInt32(Request.Form["fix"]);
                GlobalVar.fixtureQueried = controller.SelectFixture(id);
                return;
            }
            int FID = Convert.ToInt32(Request.Form["fix"]);
            int gameweek = Convert.ToInt32(Request.Form["gameweek"]);
            string home = Request.Form["club"];
            string away = Request.Form["club1"];
            if (home == away)
            {
                Message = "Home club cannot be the same as the away club";
                return;
            }
            string Stadium = controller.SelectStadiumByAbbr(home);
            int referee = Convert.ToInt32(Request.Form["referee"]);
            string date = Request.Form["date"];
            string time = Request.Form["time"];
            if (date == "" || time == "")
            {
                Message = "You must specify date and time.";
                return;
            }
            Message = controller.UpdateFixture(FID, gameweek, date, time, home, away, Stadium, referee);
            GlobalVar.fixtureQueried = controller.SelectFixture(FID);
            controller.UpdateFixturesList();
        }
    }
}
