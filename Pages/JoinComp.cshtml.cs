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
        }
        public void OnPost()
        {
            Message = controller.JoinCompetition(Request.Form["name"], Request.Form["password"]);
        }
    }
}
