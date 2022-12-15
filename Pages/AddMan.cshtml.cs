using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FantasyPL.Pages
{
    public class AddManModel : PageModel
    {
        public string Message = "";
        Controller controller = new Controller();
        public Manager ManInfo = new Manager();
        public void OnGet()
        {
        }

        public void OnPost() 
        {
            ManInfo.ID = controller.lastManID() + 1;
            ManInfo.FName = Request.Form["fname"];
            ManInfo.MName = Request.Form["mname"];
            ManInfo.LName = Request.Form["lname"];
            ManInfo.age = Convert.ToInt32(Request.Form["age"]);
            ManInfo.nationality = Request.Form["nationality"];
            ManInfo.competitions_won = Convert.ToInt32(Request.Form["competitions_won"]);
            ManInfo.experience_in_years = Convert.ToInt32(Request.Form["experience"]);
            if(ManInfo.experience_in_years > ManInfo.age)
            {
                Message = "Experience cannot be more than age!";
                return;
            }
            Message = controller.InsertManager(ManInfo);
            controller.UpdateManagersList();
        }
    }
}
