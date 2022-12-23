using AspNetCore.Reporting;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using FantasyPL.Pages;
//using System.Windows.Forms;

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
            parameters.Add("prm1", DateTime.Now.ToString("dd-MMM-yyyy"));//

            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("FavClub", dt);//

            var res = localReport.Execute(RenderType.Pdf, extension, parameters, mimeType);
            return File(res.MainStream, "application/pdf");
            //button href
        }

        public IActionResult MostGCByClub()
        {
            var dt = new DataTable();
            dt = controller.MostGCByClub(GlobalVar.clubQueried.Name_Abbreviation);

            string mimeType = "";
            int extension = 1;
            var path = $"{_webHostEnv.WebRootPath}\\Reports\\MostGC.rdlc";//

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("prm1", GlobalVar.clubQueried.Name);//
            parameters.Add("prm2", DateTime.Now.ToString("dd-MMM-yyyy"));

            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("MostGC", dt);//

            var res = localReport.Execute(RenderType.Pdf, extension, parameters, mimeType);
            return File(res.MainStream, "application/pdf");
        }

        public IActionResult WeeklyPP()
        {
            var dt = new DataTable();
            dt = controller.WeeklyPP(GlobalVar.week);

            string mimeType = "";
            int extension = 1;
            var path = $"{_webHostEnv.WebRootPath}\\Reports\\WeeklyPP.rdlc";//

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("prm1", GlobalVar.week.ToString());//
            parameters.Add("prm2", DateTime.Now.ToString("dd-MMM-yyyy"));

            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("WeeklyPP", dt);//

            var res = localReport.Execute(RenderType.Pdf, extension, parameters, mimeType);
            return File(res.MainStream, "application/pdf");
        }

        public IActionResult CompUser()
        {
            var dt = new DataTable();
            dt = controller.CompUser();
            
            string mimeType = "";
            int extension = 1;
            var path = $"{_webHostEnv.WebRootPath}\\Reports\\CompUser.rdlc";//

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("prm1", DateTime.Now.ToString("dd-MMM-yyyy"));

            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("CompUser", dt);//
            

            var res = localReport.Execute(RenderType.Pdf, extension, parameters, mimeType);
            return File(res.MainStream, "application/pdf");
        }

        public IActionResult PercentComp()
        {
            var dt = new DataTable();
            dt = controller.PercentComp();

            string mimeType = "";
            int extension = 1;
            var path = $"{_webHostEnv.WebRootPath}\\Reports\\Percent.rdlc";//

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("prm1", DateTime.Now.ToString("dd-MMM-yyyy"));

            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("Percent", dt);//

            var res = localReport.Execute(RenderType.Pdf, extension, parameters, mimeType);
            return File(res.MainStream, "application/pdf");
        }

        public IActionResult SelectedTimes()
        {
            var dt = new DataTable();
            dt = controller.SelectedTimes();

            string mimeType = "";
            int extension = 1;
            var path = $"{_webHostEnv.WebRootPath}\\Reports\\SelectedTimes.rdlc";//

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("prm1", DateTime.Now.ToString("dd-MMM-yyyy"));

            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("SelectedTimes", dt);//

            var res = localReport.Execute(RenderType.Pdf, extension, parameters, mimeType);
            return File(res.MainStream, "application/pdf");
        }

        public IActionResult PlayersStats()
        {
            var dt = new DataTable();
            dt = controller.PlayersStats();

            string mimeType = "";
            int extension = 1;
            var path = $"{_webHostEnv.WebRootPath}\\Reports\\Stats.rdlc";//

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("prm1", DateTime.Now.ToString("dd-MMM-yyyy"));

            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("Stats", dt);//

            var res = localReport.Execute(RenderType.Pdf, extension, parameters, mimeType);
            return File(res.MainStream, "application/pdf");
        }
    }
}
