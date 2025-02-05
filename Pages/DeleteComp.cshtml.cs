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
            controller.GetAllCompetitions();
        }
        public void OnPost()
        {
            Message = controller.DeleteCompetition(Convert.ToInt32(Request.Form["comp"]));
            controller.GetAllCompetitions();
        }
    }
}
