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
            string test = Request.Form["user"];
            if(test == null)
            {
                Message = "No user selected";
                return;
            }
			if (GlobalVar.LoggedInUser.Username == Request.Form["user"])
            {
				Message = controller.DeleteUser(Request.Form["user"]);
                if(Message == "User deleted successfully")
    				Response.Redirect("/Index");
                return;
			}
            Message = controller.DeleteUser(Request.Form["user"]);
            controller.GetAllUsers();
        }
    }
}
