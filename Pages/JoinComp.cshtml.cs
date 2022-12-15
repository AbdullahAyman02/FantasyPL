using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FantasyPL.Pages
{
    public class JoinCompModel : PageModel
    {
        Controller controller = new Controller();
        public string Message = "";
        public void OnGet()
        {
            controller.GetNewCompetitions(GlobalVar.LoggedInUser.Username);
        }
        public void OnPost()
        {
            Message = controller.JoinCompetition(Convert.ToInt32(Request.Form["comp"]), Request.Form["password"]);
            controller.GetNewCompetitions(GlobalVar.LoggedInUser.Username);
        }
    }
}
