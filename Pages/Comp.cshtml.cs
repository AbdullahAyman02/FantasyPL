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
            if (GlobalVar.userComp.Count > 0)
            {
                GlobalVar.compQueried = GlobalVar.userComp[0];
                controller.GetParticipantsInCompetition(GlobalVar.userComp[0].Id);
            }
            else
                GlobalVar.compParticipants.Clear();
        }
        public void OnPost()
        {
            GlobalVar.compQueried.Id = Convert.ToInt32(Request.Form["comp"]);
            controller.GetParticipantsInCompetition(Convert.ToInt32(Request.Form["comp"]));
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
