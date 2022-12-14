using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FantasyPL.Pages
{
    public class ProfileModel : PageModel
    {
        public string Message = "";
        Controller controller = new Controller();

        public void OnGet()
        {
            GlobalVar.isAdmin = true;
        }

        public void OnPost()
        {
            string oldPass = Request.Form["old"];
            string newPass1 = Request.Form["new1"];
            string newPass2 = Request.Form["new2"];
            if(controller.LogIn(GlobalVar.LoggedInUser.Username, oldPass) == null)
            {
                Message = "Wrong password";
                return;
            }
            if (newPass1 != newPass2)
            {
                Message = "New passwords do not match";
                return;
            }
            Message = controller.ChangePassword(GlobalVar.LoggedInUser.Username, newPass1);
        }
    }
}
