using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FantasyPL.Pages
{
    public class InsertFEModel : PageModel
    {
        public string Message = "";
        Controller controller = new Controller();
        public Fixture fixtureInfo = new Fixture();
        public MEvent eventInfo = new MEvent();
        public void OnGet()
        {
            controller.UpdateClubsList();
            controller.UpdateStadiumsList();
            controller.UpdateRefereesList();
            controller.UpdateFixturesList();
            GlobalVar.fixture_in_insert_event = GlobalVar.listFixtures[0];
            GlobalVar.HA = true;
            controller.SelectPlayersByClubAbbr(GlobalVar.fixture_in_insert_event.HomeSide);
        }
        public void OnPost()
        {
            Message = "";
            string btnvalue2 = Request.Form["Refresh2"];
            if (btnvalue2 != null)
            {
                GlobalVar.fixtureQueried = controller.SelectFixture(Convert.ToInt32(Request.Form["fixture"]));
                GlobalVar.fixture_in_insert_event = controller.SelectFixture(Convert.ToInt32(Request.Form["fixture"]));
                controller.SelectPlayersByClubAbbr(GlobalVar.fixture_in_insert_event.HomeSide);
                return;
            }
            string btnvalue = Request.Form["Refresh"];
            if (btnvalue != null)
            {
                controller.SelectPlayersByClubAbbr(Request.Form["club_abbr"]);
                GlobalVar.HA = GlobalVar.fixtureQueried.HomeSide == Request.Form["club_abbr"];
                return;
            }
            string btnvalue1 = Request.Form["Insert"];
            if (btnvalue1 != null)
            {
                string clubAbbr = Request.Form["club_abbr"];
                int playerNo = Convert.ToInt16(Request.Form["event_player"]);
                string type = Request.Form["event"];
                int FID = Convert.ToInt32(Request.Form["fixture"]);
                int min = Convert.ToInt32(Request.Form["minute"]);
                int EID = controller.lastEventID(FID)+1;
                Message = controller.InsertEvent(FID, EID, type, min, clubAbbr, playerNo);
                return;
            }
            int GW = Convert.ToInt32(Request.Form["gameweek"]);
            string date = Request.Form["date"];
            string time = Request.Form["time"];
            if(date == "" || time == "")
            {
                Message = "You must specify date and time.";
                return;
            }
            string home = Request.Form["club"];
            string away = Request.Form["club1"];
            if(home == away)
            {
                Message = "Home club cannot be the same as the away club";
                return;
            }
            string stadium = controller.SelectStadiumByAbbr(home);
            string referee = Request.Form["referee"];
            Message = controller.InsertFixture(GW, date, time, home, away, stadium, referee);
        }
    }

    public class Stadium { 
        public string name { get; set; }
        public int capacity { get; set; }
        public string city { get; set; }
        public int size { get; set; }
        public string clubAbbreviation { get; set; }
    }

}
