using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FantasyPL.Pages
{
    public class DeleteFEModel : PageModel
    {
        public String Message = "";
        Controller controller = new Controller();
        public void OnGet()
        {
            controller.UpdateFixturesList();
            controller.UpdateFixtureEvents(GlobalVar.listFixtures[0].ID);
            if (GlobalVar.listFixtures.Count > 0)
                GlobalVar.fixtureQueried = GlobalVar.listFixtures[0];
        }
        public void OnPost()
        {
            string btnvalue1 = Request.Form["Refresh"];
            if (btnvalue1 != null)
            {
                GlobalVar.fixtureQueried = controller.SelectFixture(Convert.ToInt32(Request.Form["fix"]));
                controller.UpdateFixtureEvents(Convert.ToInt32(Request.Form["fix"]));
                return;
            }
            string btnvalue = Request.Form["DeleteFixture"];
            if (btnvalue != null)
            {
                Message = controller.DeleteFixture(Convert.ToInt32(Request.Form["fix"]));
                controller.UpdateFixtureEvents(Convert.ToInt32(GlobalVar.listFixtures[0].ID));
                if(GlobalVar.listFixtures.Count > 0)
                    GlobalVar.fixtureQueried = GlobalVar.listFixtures[0];
                return;
            }
            Message = controller.DeleteEventofFixture(Convert.ToInt32(Request.Form["fix"]), Convert.ToInt32(Request.Form["evt"]));
        }
    }
}
