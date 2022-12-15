using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FantasyPL.Pages
{
    public class ExitCompModel : PageModel
    {
        Controller controller = new Controller();
        public string Message = "";
        public void OnGet()
        {
            GlobalVar.compQueried = null;
            controller.UpdateCompetitionsByUsername(GlobalVar.LoggedInUser.Username);
        }
        public void OnPost()
        {
            Message = controller.ExitCompetition( GlobalVar.LoggedInUser.Username, Convert.ToInt32(Request.Form["comp"]));
        }
    }
}
