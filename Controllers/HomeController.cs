using AspNetCore.Reporting;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using FantasyPL.Pages;

namespace FantasyPL.Controllers
{ 
    public class HomeController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnv;
        FantasyPL.Pages.Controller controller = new();

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnv)
        {
            _logger = logger;
            _webHostEnv = webHostEnv;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult StadiumInfo()
        {
            var dt = new DataTable();
            dt = controller.GetStadiumInfo();//

            string mimeType = "";
            int extension = 1;
            var path = $"{_webHostEnv.WebRootPath}\\Reports\\Report1.rdlc";//

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("prm1", DateTime.Now.ToString("dd-MMM-yyyy"));//

            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("DataSet1", dt);//

            var res = localReport.Execute(RenderType.Pdf, extension, parameters, mimeType);
            return File(res.MainStream, "application/pdf");
            //button href
        }

        public IActionResult FavClub()
        {
            var dt = new DataTable();
            dt = controller.FavClub();//

            string mimeType = "";
            int extension = 1;
            var path = $"{_webHostEnv.WebRootPath}\\Reports\\FavClub.rdlc";//

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            //parameters.Add("prm1", DateTime.Now.ToString("dd-MMM-yyyy"));//

            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("FavClub", dt);//

            var res = localReport.Execute(RenderType.Pdf, extension, parameters, mimeType);
            return File(res.MainStream, "application/pdf");
            //button href
        }
    }
}
