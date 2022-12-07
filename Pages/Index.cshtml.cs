using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace FantasyPL.Pages
{
    public static class GlobalVar
    {
        public static List<Club> listClubs = new List<Club>();
        public static List<player> listPlayers = new List<player>();
        public static List<player> userPlayers = new List<player>();
        public static Club clubQueried = new Club();
        public static User LoggedInUser = new User();
        public static player playerQueried = new player();
        public static List<player> clubPlayers = new List<player>();
        public static List<string> Positions = new List<string> { "Attacker", "Midfielder", "Defender", "GoalKeeper" };
        public static List<string> Cities = new List<string> { "London", "Birmingham", "Bournemouth", "Brighton & Hove", "Burnley", "Liverpool", "Leicester", "Manchester", "Newcastle", "Norwich", "Sheffield", "Southampton", "London", "Watford", "London", "Wolverhampton"};
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

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public void OnPost()
        {
            userInfo.Username = Request.Form["Username"];
            string password = Request.Form["Password"];
            var sha = SHA256.Create();
            var byteArr = Encoding.Default.GetBytes(password);
            var hashedPasswordByte = sha.ComputeHash(byteArr);
            string hashedPassword = Convert.ToBase64String(hashedPasswordByte);
            userInfo.Password = hashedPassword;
            try
            {
                DBManager dBManager = new();
                string sql = "SELECT * FROM Users WHERE USERNAME = @Username AND PASSWORD = @Password";
                using (SqlCommand command = new SqlCommand(sql, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@Username", userInfo.Username);
                    command.Parameters.AddWithValue("@Password", userInfo.Password);
                    SqlDataReader reader = dBManager.ExecuteReader(command);
                    if(!reader.HasRows)
                        errorMessage = "User not registered";
                    while (reader.Read()) {
                        GlobalVar.LoggedInUser.Username = reader.GetString(0);
                        GlobalVar.LoggedInUser.UserType = reader.GetString(7)[0];
                        successMessage = "Logged in successfully";
                        Response.Redirect("/clubs");
                    }
                    reader.Close();
                }
            } catch (Exception ex)
            {
                errorMessage = ex.Message;
                successMessage = "";
            }
        }
    }
}