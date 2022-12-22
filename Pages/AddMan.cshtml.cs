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
            bool result = ManInfo.FName.All(Char.IsLetter);
            ManInfo.MName = Request.Form["mname"];
            bool result1 = ManInfo.MName.All(Char.IsLetter);
            ManInfo.LName = Request.Form["lname"];
            bool result2 = ManInfo.LName.All(Char.IsLetter);
            if (!result || !result1 || !result2)
            {
                Message = "Name must contain letters only";
                return;
            }
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
