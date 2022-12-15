using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FantasyPL.Pages
{
    public class DeleteManModel : PageModel
    {
		public string Message = "";
		Controller controller = new();
		public void OnGet()
		{
			controller.UpdateAllManagers();
		}

		public void OnPost()
		{
			Message = controller.DeleteManager(Convert.ToInt32(Request.Form["manager"]));
			controller.UpdateAllManagers();
			controller.UpdateManagersList();
		}
	}
}
