using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FantasyPL.Pages
{
    public class ProfileModel : PageModel
    {
        public string Message = "";
        Controller controller = new Controller();
        public string FavClub = "";

        public void OnGet()
        {
            GlobalVar.isAdmin = true;
            FavClub = controller.GetFavClub(GlobalVar.LoggedInUser.Username);
            controller.UpdateClubsList();
            GlobalVar.statusFT = controller.GetFT();
        }

        public void OnPost()
        {
            string btnvalue = Request.Form["club"];
            if (btnvalue != null)
            {
                Message = controller.UpdateFavClub(GlobalVar.LoggedInUser.Username, Request.Form["favorite_club"]);
                FavClub = Request.Form["favorite_club"];
                return;
            }
            string btnvalue1 = Request.Form["toggle"];
            if (btnvalue1 != null)
            {
                controller.ToggleFT();
                GlobalVar.statusFT = controller.GetFT();
                return;
            }
            string oldPass = Request.Form["old"];
            string newPass1 = Request.Form["new1"];
            string newPass2 = Request.Form["new2"];
            if(oldPass == null || newPass1 == null || newPass2== null) {
                Message = "Please fill the empty fields";
                return;
            }
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
