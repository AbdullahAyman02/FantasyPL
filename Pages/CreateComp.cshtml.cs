using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FantasyPL.Pages
{
    public class CreateCompModel : PageModel
    {
        Controller controller = new Controller();
        public string Message = "";
        public void OnGet()
        {
        }

        public void OnPost() {
            Competitions comp= new Competitions();
            comp.Name = Request.Form["name"];
            comp.Id = controller.lastCompID()+1;
            comp.Password= Request.Form["password"];
            comp.Capacity = Convert.ToInt32(Request.Form["Capacity"]);
            Message = controller.CreateCompetition(comp);
        }
    }
}
