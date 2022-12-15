using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FantasyPL.Pages
{
    public class DeleteStadiumModel : PageModel
    {
        public string Message = "";
        Controller controller = new();
        public void OnGet()
        {
            controller.UpdateAllStadiums();
        }

        public void OnPost()
        {
            Message = controller.DeleteStadium(Request.Form["stadium"]);
            controller.UpdateAllStadiums();
            controller.UpdateStadiumsList();
        }
    }
}
