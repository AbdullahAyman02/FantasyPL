using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

namespace FantasyPL.Pages
{
    public class FTModel : PageModel
    {
        public string Message = "";
        Controller controller = new Controller();
        public void OnGet()
        {
            controller.UpdatePlayersList();
            controller.SelectPlayersByUsername(GlobalVar.LoggedInUser.Username);
        }

        public void OnPost()
        {
            string btnvalue = Request.Form["Delete Player"];
            if (btnvalue != null)
            {
                string[] value1 = Request.Form["player2"].ToString().Split(" ");
                string abbr1 = value1[0];
                int no1 = Convert.ToInt32(value1[1]);
                Message = controller.DeleteFTplayer(GlobalVar.LoggedInUser.Username, abbr1, no1);
                return;
            }
            string[] value = Request.Form["player"].ToString().Split(" ");
            string abbr = value[0];
            int no = Convert.ToInt32(value[1]);
            Message = controller.InsertFTplayer(GlobalVar.LoggedInUser.Username, abbr, no);
        }
    }
}
