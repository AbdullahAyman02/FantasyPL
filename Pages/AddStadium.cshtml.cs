using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FantasyPL.Pages
{
    public class AddStadiumModel : PageModel
    {
        public String Message = "";
        Controller controller = new();
        public Stadium stadiumInfo = new Stadium();
        public void OnGet()
        {
        }

        public void OnPost() 
        {
            stadiumInfo.name = Request.Form["name"];
            var match = stadiumInfo.name.IndexOfAny("0123456789".ToCharArray()) != -1;
            if(match)
            {
				Message = "Name must not contain numbers";
                return;
			}
            stadiumInfo.capacity = Convert.ToInt32(Request.Form["capacity"]);
            stadiumInfo.city = Request.Form["city"];
            stadiumInfo.size = Convert.ToInt32(Request.Form["size"]);
            Message = controller.InsertStadium(stadiumInfo.name, stadiumInfo.capacity, stadiumInfo.city, stadiumInfo.size);
            controller.UpdateStadiumsList();
        }
    }
}
