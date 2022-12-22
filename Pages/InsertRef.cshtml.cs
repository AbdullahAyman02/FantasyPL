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
            var result = referee.FName.All(Char.IsLetter);
            referee.MName = Request.Form["Mname"];
            var result1 = referee.MName.All(Char.IsLetter);
            referee.LName = Request.Form["Lname"];
            var result2 = referee.LName.All(Char.IsLetter);
            if (!result || !result1 || !result2)
            {
                Message = "Name must contain letters only";
                return;
            }
            referee.Age = Convert.ToInt32(Request.Form["age"]);
            referee.Nationality = Request.Form["nationality"];
            referee.Experience = Convert.ToInt32(Request.Form["exp"]);
            Message = controller.InsertReferee(referee);
            GlobalVar.refereeQueried = referee;
        }

    }
}
