using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FantasyPL.Pages
{
    public class DeleteCompModel : PageModel
    {
        Controller controller = new Controller();
        public string Message = "";
        public void OnGet()
        {
        }
        public void OnPost()
        {
            Message = controller.DeleteCompetition(Request.Form["name"]);
        }
    }
}
