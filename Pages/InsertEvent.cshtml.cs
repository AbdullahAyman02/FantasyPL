using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ServiceModel.Channels;

namespace FantasyPL.Pages
{
    public class InsertEventModel : PageModel
    {
		public string Message = "";
		Controller controller = new();
		public bool hasStart = false;
		public bool hasEnd = false;
		public MEvent EventInfo = new MEvent();

		public void OnGet()
		{
			controller.UpdateFixturesList();
			if (GlobalVar.listFixtures.Count > 0 && GlobalVar.fixture_in_insert_event == null)
			{
				GlobalVar.fixture_in_insert_event = GlobalVar.listFixtures[0];
				controller.SelectPlayersByClubAbbr(GlobalVar.fixture_in_insert_event.HomeSide);
			}
			else
			{
				GlobalVar.clubPlayers.Clear();
			}
			GlobalVar.HA = true;
			hasStart = controller.HasStartEvent(GlobalVar.fixture_in_insert_event.ID);
			hasEnd = controller.HasEndEvent(GlobalVar.fixture_in_insert_event.ID);
		}
        public void OnPost()
        {
			Message = "";
			string btnvalue2 = Request.Form["Refresh2"];
			hasStart = controller.HasStartEvent(Convert.ToInt32(Request.Form["fixture"]));
			hasEnd = controller.HasEndEvent(Convert.ToInt32(Request.Form["fixture"]));
			if (btnvalue2 != null)
			{
				//GlobalVar.fixtureQueried = controller.SelectFixture(Convert.ToInt32(Request.Form["fixture"]));
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
				string minu = Request.Form["minute"];
				if (minu == "")
				{
					Message = "Please Specify a Minute.";
					return;
				}
				int min = Convert.ToInt32(Request.Form["minute"]);
				int EID = controller.lastEventID(FID) + 1;
				Message = controller.InsertEvent(FID, EID, type, min, clubAbbr, playerNo);
				return;
			}
			string btnvalue3 = Request.Form["end"];
			if (btnvalue3 != null)
			{
				int FID = Convert.ToInt32(Request.Form["fixture"]);
				string minu = Request.Form["minute"];
				if (minu == "")
				{
					Message = "Please Specify a Minute.";
					return;
				}
				int min = Convert.ToInt32(Request.Form["minute"]);
				int EID = controller.lastEventID(FID) + 1;
				Message = controller.InsertEvent(FID, EID, "End", min, "-", -1);
				hasEnd = controller.HasEndEvent(GlobalVar.fixture_in_insert_event.ID);
				return;
			}
			string btnvalue4 = Request.Form["start"];
			if (btnvalue4 != null)
			{
				int FID = Convert.ToInt32(Request.Form["fixture"]);
				int EID = controller.lastEventID(FID) + 1;
				Message = controller.InsertEvent(FID, EID, "Start", 0, "-", -1);
				hasStart = controller.HasStartEvent(Convert.ToInt32(Request.Form["fixture"]));
				return;
			}
		}
    }
}
