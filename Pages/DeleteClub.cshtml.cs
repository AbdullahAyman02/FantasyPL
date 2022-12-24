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
            string club = Request.Form["club"];
			if (club == null)
            {
                Message = "No club selected";
                return;
            }
            int result = controller.CheckFixtures(Request.Form["club"]);
            if (result == -1)
                Message = "An error has occured";
            else if(result == 0)
                Message = controller.DeleteClub(Request.Form["club"]);
            else
                Message = "Cannot delete a club because it has fixtures";
        }
    }
}
