using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FantasyPL.Pages
{
    public class CompModel : PageModel
    {
        Controller controller = new Controller();
        public void OnGet()
        {
            controller.UpdateCompetitionsByUsername(GlobalVar.LoggedInUser.Username);
            controller.GetAllCompetitions();
            if (GlobalVar.userComp.Count > 0)
            {
                GlobalVar.compQueried = GlobalVar.userComp[0];
                controller.GetParticipantsInCompetition(GlobalVar.userComp[0].Id);
            }
            else
            {
                GlobalVar.compQueried = new();
				GlobalVar.compParticipants.Clear();
            }
        }
        public void OnPost()
        {
            if (GlobalVar.LoggedInUser.UserType != 'A')
            {
                GlobalVar.compQueried.Id = Convert.ToInt32(Request.Form["comp"]);
                controller.GetParticipantsInCompetition(Convert.ToInt32(Request.Form["comp"]));
            } else
            {
				GlobalVar.compQueried.Id = Convert.ToInt32(Request.Form["comp1"]);
				controller.GetParticipantsInCompetition(Convert.ToInt32(Request.Form["comp1"]));
			}
			controller.GetAllCompetitions();
		}
        
    }

    public class Competitions
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Capacity { get; set; }
    }
}
