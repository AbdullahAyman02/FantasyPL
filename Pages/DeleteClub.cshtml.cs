using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FantasyPL.Pages
{
    public class DeleteClubModel : PageModel
    {
        public string Message = "";
        Controller controller = new Controller();
        public void OnGet()
        {
        }
        
        public void OnPost()
        {
            Message = controller.DeleteClub(Request.Form["club"]);
        }
    }
}
