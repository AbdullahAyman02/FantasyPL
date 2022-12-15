using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FantasyPL.Pages
{
    public class DeleteUserModel : PageModel
    {
        Controller controller = new Controller();
        public string Message = "";
        public void OnGet()
        {
            controller.GetAllUsers();
        }
        public void OnPost()
        {
            Message = controller.DeleteUser(Request.Form["user"]);
            controller.GetAllUsers();
        }
    }
}
