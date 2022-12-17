using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace FantasyPL.Pages
{
    public class StoredProcedures
    {
        public static string GetParticipants = "GetParticipants";
        public static string GetCompetitionsForUser = "GetCompetitionsForUser";
        public static string CreateCompetition = "CreateCompetition";
        public static string JoinCompetition = "JoinCompetition";
        public static string ExitCompetition = "ExitCompetition";
        public static string DeleteCompetition = "DeleteCompetition";
        public static string GetAllCompetitions = "GetAllCompetitions";
        public static string GetNewCompetitions = "GetNewCompetitions";
        public static string DeleteUser = "DeleteUser";
        public static string GetAllUsers = "GetAllUsers";
        public static string SetStadium = "SetStadium";
        public static string SetManager = "SetManager";
        public static string GetNewStadiums = "GetFreeStadium";
        public static string GetStadiums = "GetStadiums";
        public static string DeleteStadium = "DeleteStadium";
        public static string GetNewManager = "GetFreeManager";
        public static string GetManagers = "GetManagers";
        public static string DeleteManager = "DeleteManager";
        public static string InsertStadium = "InsertStadium";
        public static string InsertManager = "InsertManager";

    }
    public static class GlobalVar
    {
        public static List<Club> listClubs = new List<Club>();
        public static List<player> listPlayers = new List<player>();
        public static List<player> userPlayers = new List<player>();
        public static Club clubQueried = new Club();
        public static User LoggedInUser = new User();
        public static player playerQueried = new player();
        public static List<player> clubPlayers = new List<player>();
        public static List<Fixture> weekFixtures = new List<Fixture>();
        public static List<Fixture> listFixtures = new List<Fixture>();
        public static Fixture fixtureQueried = new Fixture();
        public static List<Stadium> listStadiums = new List<Stadium>();
        public static List<Stadium> allStadiums = new List<Stadium>();
        public static List<Manager> listManagers = new List<Manager>();
        public static List<Manager> allManagers = new List<Manager>();
        public static List<Referee> listReferees = new List<Referee>();
        public static Fixture fixture_in_insert_event = new Fixture();
        public static bool HA = false;      //true for home side, false for away side
        public static bool isAdmin = false; //registering admin user or fan user
        public static int week = 1;
        public static Competitions compQueried = new Competitions();
        public static List<User> compParticipants = new List<User>();
        public static List<Competitions> userComp = new List<Competitions>();
        public static List<Competitions> listComp = new List<Competitions>();
        public static List<Competitions> newComp = new List<Competitions>();
        public static List<User> listUser = new List<User>();
        public static List<string> EventTypes = new List<string> { "Goal", "Assist", "Save", "Red Card", "Yellow Card", "Tackle", "End", "Start"};
        public static List<MEvent> fixtureEvents = new List<MEvent>();
        public static List<string> Positions = new List<string> { "Attacker", "Midfielder", "Defender", "GoalKeeper" };
        public static List<string> Cities = new List<string> { "London", "Birmingham", "Bournemouth", "Brighton & Hove", "Burnley", "Liverpool", "Leicester", "Manchester", "Newcastle", "Norwich", "Sheffield", "Southampton", "Watford", "Wolverhampton"};
        public static List<string> Countries = new List<string> { "Afghanistan", "Åland Islands","Albania" ,"Algeria","American Samoa" ,"Andorra","Angola" ,"Anguilla","Antarctica","Antigua and Barbuda"
,"Argentina"
,"Armenia"
,"Aruba"
,"Australia"
,"Austria"
,"Azerbaijan"
,"Bahamas"
,"Bahrain"
,"Bangladesh"
,"Barbados"
,"Belarus"
,"Belgium"
,"Belize"
,"Benin"
,"Bermuda"
,"Bhutan"
,"Bolivia"
,"Bosnia and Herzegovina"
,"Botswana"
,"Bouvet Island"
,"Brazil"
,"British Indian Ocean Territory"
,"Brunei Darussalam"
,"Bulgaria"
,"Burkina Faso"
,"Burundi"
,"Cambodia"
,"Cameroon"
,"Canada"
,"Cape Verde"
,"Cayman Islands"
,"Central African Republic"
,"Chad"
,"Chile"
,"China"
,"Christmas Island"
,"Cocos (Keeling) Islands"
,"Colombia"
,"Comoros"
,"Congo"
,"Congo, The Democratic Republic of The"
,"Cook Islands"
,"Costa Rica"
,"Cote D'ivoire"
,"Croatia"
,"Cuba"
,"Cyprus"
,"Czech Republic"
,"Denmark"
,"Djibouti"
,"Dominica"
,"Dominican Republic"
,"Ecuador"
,"Egypt"
,"El Salvador"
,"Equatorial Guinea"
,"Eritrea"
,"Estonia"
,"Ethiopia"
,"Falkland Islands (Malvinas)"
,"Faroe Islands"
,"Fiji"
,"Finland"
,"France"
,"French Guiana"
,"French Polynesia"
,"French Southern Territories"
,"Gabon"
,"Gambia"
,"Georgia"
,"Germany"
,"Ghana"
,"Gibraltar"
,"Greece"
,"Greenland"
,"Grenada"
,"Guadeloupe"
,"Guam"
,"Guatemala"
,"Guernsey"
,"Guinea"
,"Guinea-bissau"
,"Guyana"
,"Haiti"
,"Heard Island and Mcdonald Islands"
,"Holy See (Vatican City State)"
,"Honduras"
,"Hong Kong"
,"Hungary"
,"Iceland"
,"India"
,"Indonesia"
,"Iran, Islamic Republic of"
,"Iraq"
,"Ireland"
,"Isle of Man"
,"Italy"
,"Jamaica"
,"Japan"
,"Jersey"
,"Jordan"
,"Kazakhstan"
,"Kenya"
,"Kiribati"
,"Korea, Democratic People's Republic of"
,"Korea, Republic of"
,"Kuwait"
,"Kyrgyzstan"
,"Lao People's Democratic Republic"
,"Latvia"
,"Lebanon"
,"Lesotho"
,"Liberia"
,"Libyan Arab Jamahiriya"
,"Liechtenstein"
,"Lithuania"
,"Luxembourg"
,"Macao"
,"Macedonia, The Former Yugoslav Republic of"
,"Madagascar"
,"Malawi"
,"Malaysia"
,"Maldives"
,"Mali"
,"Malta"
,"Marshall Islands"
,"Martinique"
,"Mauritania"
,"Mauritius"
,"Mayotte"
,"Mexico"
,"Micronesia, Federated States of"
,"Moldova, Republic of"
,"Monaco"
,"Mongolia"
,"Montenegro"
,"Montserrat"
,"Morocco"
,"Mozambique"
,"Myanmar"
,"Namibia"
,"Nauru"
,"Nepal"
,"Netherlands"
,"Netherlands Antilles"
,"New Caledonia"
,"New Zealand"
,"Nicaragua"
,"Niger"
,"Nigeria"
,"Niue"
,"Norfolk Island"
,"Northern Mariana Islands"
,"Norway"
,"Oman"
,"Pakistan"
,"Palau"
,"Palestine"
,"Panama"
,"Papua New Guinea"
,"Paraguay"
,"Peru"
,"Philippines"
,"Pitcairn"
,"Poland"
,"Portugal"
,"Puerto Rico"
,"Qatar"
,"Reunion"
,"Romania"
,"Russian Federation"
,"Rwanda"
,"Saint Helena"
,"Saint Kitts and Nevis"
,"Saint Lucia"
,"Saint Pierre and Miquelon"
,"Saint Vincent and The Grenadines"
,"Samoa"
,"San Marino"
,"Sao Tome and Principe"
,"Saudi Arabia"
,"Senegal"
,"Serbia"
,"Seychelles"
,"Sierra Leone"
,"Singapore"
,"Slovakia"
,"Slovenia"
,"Solomon Islands"
,"Somalia"
,"South Africa"
,"South Georgia and The South Sandwich Islands"
,"Spain"
,"Sri Lanka"
,"Sudan"
,"Suriname"
,"Svalbard and Jan Mayen"
,"Swaziland"
,"Sweden"
,"Switzerland"
,"Syrian Arab Republic"
,"Taiwan"
,"Tajikistan"
,"Tanzania, United Republic of"
,"Thailand"
,"Timor-leste"
,"Togo"
,"Tokelau"
,"Tonga"
,"Trinidad and Tobago"
,"Tunisia"
,"Turkey"
,"Turkmenistan"
,"Turks and Caicos Islands"
,"Tuvalu"
,"Uganda"
,"Ukraine"
,"United Arab Emirates"
,"United Kingdom"
,"United States"
,"United States Minor Outlying Islands"
,"Uruguay"
,"Uzbekistan"
,"Vanuatu"
,"Venezuela"
,"Viet Nam"
,"Virgin Islands, British"
,"Virgin Islands, U.S."
,"Wallis and Futuna"
,"Western Sahara"
,"Yemen"
,"Zambia"
,"Zimbabwe"};
    }
    public class IndexModel : PageModel
    {
        public User userInfo = new();
        public string successMessage = "";
        public string errorMessage = "";
        private readonly ILogger<IndexModel> _logger;
        Controller controller = new Controller();

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            GlobalVar.isAdmin = false;
            GlobalVar.LoggedInUser = null;
        }

        public void OnPost()
        {
            userInfo.Username = Request.Form["Username"];
            userInfo.Password = Request.Form["Password"];
            GlobalVar.LoggedInUser = controller.LogIn(userInfo.Username, userInfo.Password);
            if(GlobalVar.LoggedInUser != null)
            {
                successMessage = "Logged in successfully";
                Response.Redirect("/clubs");
            } else
            {
                errorMessage = "User not registered";
            }
        }
    }
}