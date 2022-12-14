using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FantasyPL.Pages
{
    public class fixtureModel : PageModel
    {
        public string Message = "";
        Controller controller = new Controller();
        public void OnGet()
        { 
            GlobalVar.week = 1;
            controller.UpdateFixturesByWeek(GlobalVar.week);
            GlobalVar.fixtureQueried = GlobalVar.weekFixtures[0];
            GlobalVar.fixtureEvents.Clear();
        }

        public void OnPost()
        {
            string btnvalue = Request.Form["Refresh"];
            if (btnvalue != null)
            {
                controller.UpdateFixturesByWeek(Convert.ToInt32(Request.Form["club2"]));
                GlobalVar.week = Convert.ToInt32(Request.Form["club2"]);
                return;
            }
            GlobalVar.fixtureQueried = controller.SelectFixture(Convert.ToInt32(Request.Form["fixture"]));
            controller.UpdateFixtureEvents(Convert.ToInt32(Request.Form["fixture"]));
        }
    }

    public class Fixture
    {
        public int ID { get; set; }
        public int Gameweek { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string HomeScore { get; set; }
        public string AwayScore { get; set; }
        public string HomeSide { get; set; }
        public string AwaySide { get; set; }
        public string Stadium { get; set; }
        public string Referee { get; set; }
    }

    public class MEvent
    {
        public int ID { get; set; }
        public int FixtureID { get; set; }
        public string EventType { get; set; }
        public int Minute { get; set; }
        public string ClubAbbreviation { get; set; }
        public string Player { get; set; }
    }

    public class Referee
    {
        public int ID { get; set; }
        public string FName { get; set; }
        public string MName { get; set; }
        public string LName { get; set; }
        public int Age { get; set; }
        public string Nationality { get; set; }
        public int Experience { get; set; }
    }
}
