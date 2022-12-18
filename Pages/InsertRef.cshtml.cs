using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FantasyPL.Pages
{
    
    public class InsertRefModel : PageModel
    {
        public string Message = "";
        Controller controller = new();
        public void OnGet()
        {
        }

        public void OnPost() 
        {
            Referee referee= new Referee();
            referee.ID = controller.lastRefID()+1;
            referee.FName = Request.Form["Fname"];
            referee.MName = Request.Form["Mname"];
            referee.LName = Request.Form["Lname"];
            referee.Age = Convert.ToInt32(Request.Form["age"]);
            referee.Nationality = Request.Form["nationality"];
            referee.Experience = Convert.ToInt32(Request.Form["exp"]);
            Message = controller.InsertReferee(referee);
            GlobalVar.refereeQueried = referee;
        }

    }
}
