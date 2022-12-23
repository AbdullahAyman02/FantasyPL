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

            if (GlobalVar.listFixtures.Count > 0)
            {
                GlobalVar.fixtureQueried = GlobalVar.listFixtures[0];
                return;
			}
            GlobalVar.fixtureQueried = new() ;
        }
        public void OnPost()
        {
            string btnvalue = Request.Form["DeleteFixture"];
            if (btnvalue != null)
            {
                if(controller.HasStartEvent(Convert.ToInt32(Request.Form["fix"])) && !controller.HasEndEvent(Convert.ToInt32(Request.Form["fix"])))
                {
                    Message = "Fixture started but did not end yet!";
                    return;
                }
                Message = controller.DeleteFixture(Convert.ToInt32(Request.Form["fix"]));
				if (GlobalVar.listFixtures.Count > 0)
				{
					GlobalVar.fixtureQueried = GlobalVar.listFixtures[0];
					return;
				}
				GlobalVar.fixtureQueried = new();
				return;
            }
        }
    }
}
