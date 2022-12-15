using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FantasyPL.Pages
{
    public class RemoveFromCompModel : PageModel
    {
        public string Message = "";
        Controller controller = new();
        public void OnGet()
        {
            controller.GetAllCompetitions();
            GlobalVar.compQueried.Id = 0;
            if (GlobalVar.listComp.Count > 0)
                controller.GetParticipantsInCompetition(GlobalVar.listComp[0].Id);
            else
                GlobalVar.compParticipants.Clear();
        }
        public void OnPost()
        {
            string btnValue = Request.Form["Refresh"];
            if(btnValue != null)
            {
                GlobalVar.compQueried.Id = Convert.ToInt32(Request.Form["comp"]);
                controller.GetParticipantsInCompetition(Convert.ToInt32(Request.Form["comp"]));
                return;
            }
            Message = controller.ExitCompetition(Request.Form["participant"], Convert.ToInt32(Request.Form["comp"]));
            controller.GetParticipantsInCompetition(Convert.ToInt32(Request.Form["comp"]));
        }
    }
}
