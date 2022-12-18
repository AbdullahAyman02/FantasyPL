using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FantasyPL.Pages
{
    public class DeleteRefModel : PageModel
    {
        Controller controller = new();
        public string Message = "";


        public void OnGet()
        {
            controller.UpdateRefereesList();
        }

        public void OnPost()
        {
            Message = controller.DeleteReferee(Convert.ToInt32(Request.Form["referee"]));
            controller.UpdateRefereesList();

        }
    }
}
