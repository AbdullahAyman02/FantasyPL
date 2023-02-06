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
				controller.UpdateFixtureEvents(GlobalVar.listFixtures[0].ID, 2);
			}
			else
			{
				GlobalVar.fixture_in_insert_event = new();
				GlobalVar.clubPlayers.Clear();
				GlobalVar.fixtureEvents.Clear();
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
				controller.UpdateFixtureEvents(Convert.ToInt32(Request.Form["fixture"]), 2);
				return;
			}
			string btnvalue = Request.Form["Refresh"];
			if (btnvalue != null)
			{
				controller.SelectPlayersByClubAbbr(Request.Form["club_abbr"]);
				GlobalVar.HA = GlobalVar.fixture_in_insert_event.HomeSide == Request.Form["club_abbr"];
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
                int lastmin = controller.lastEventMin(FID);
                if (minu == "")
				{
					Message = "Please Specify a Minute.";
					return;
				}
				else if (Convert.ToInt32(minu) < lastmin)
				{
					Message = "Cannot insert event before other previous events. ";
					return;
				}
				bool thereIsGoal = false;
				if (type != "Assist")
					thereIsGoal= true;
				player p = controller.SelectPlayer(clubAbbr,playerNo);
				foreach(MEvent e in GlobalVar.fixtureEvents)
				{
					if (e.EventType == "Red Card" && e.Player == p.Fname && e.ClubAbbreviation == clubAbbr && e.Minute <= Convert.ToInt32(minu))
					{
						Message = "The player has a red card and is already out of play. ";
						return;
					}
					if (type == "Assist" && e.EventType == "Goal" && e.Minute == Convert.ToInt32(minu) && e.ClubAbbreviation == clubAbbr && e.Player != p.Fname)
						thereIsGoal= true;
				}
				if(!thereIsGoal)
				{
					Message = "Please Enter the Goal before the Assist. ";
					return;
				}

				int min = Convert.ToInt32(Request.Form["minute"]);
				int EID = controller.lastEventID(FID) + 1;
				Message = controller.InsertEvent(FID, EID, type, min, clubAbbr, playerNo);
				controller.UpdateFixtureEvents(FID,2);
				return;
			}
			string btnvalue3 = Request.Form["end"];
			if (btnvalue3 != null)
			{
				int FID = Convert.ToInt32(Request.Form["fixture"]);
				string minu = Request.Form["minute"];
				if (minu == "" || Convert.ToInt32(minu) < controller.lastEventMin(FID))
				{
					Message = "Please Specify a Minute that is after all previous events.";
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
				if(controller.SelectFixture(FID).Referee == "")
				{
					Message = "Cannot Start a match without a referee.";
					return;
				}
				int EID = controller.lastEventID(FID) + 1;
				Message = controller.InsertEvent(FID, EID, "Start", 0, "-", -1);
				hasStart = controller.HasStartEvent(Convert.ToInt32(Request.Form["fixture"]));
				if (controller.GetFT() == true)
					controller.ToggleFT();
                return;
			}
			string btnvalue5 = Request.Form["Refresh1"];
			if (btnvalue5 != null)
			{
				GlobalVar.fixtureQueried = controller.SelectFixture(Convert.ToInt32(Request.Form["fixture"]));
				controller.UpdateFixtureEvents(Convert.ToInt32(Request.Form["fixture"]),2);
				return;
			}
			
			string btnvalue6 = Request.Form["DeleteEvent"];
			if (btnvalue6 != null)
			{
				Message = controller.DeleteEventofFixture(Convert.ToInt32(Request.Form["fixture"]), Convert.ToInt32(Request.Form["evt"]));
				controller.UpdateFixtureEvents(Convert.ToInt32(Request.Form["fixture"]),2);
				return;
			}
		}
    }
}
