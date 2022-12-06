using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FantasyPL.Pages
{
    public class ViewPlayerLBModel : PageModel
    {
        Controller controller = new Controller();
        public void OnGet()
        {
            controller.UpdatePlayersList();
        }
    }
}
