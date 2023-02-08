using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Xml.Linq;
using System.Diagnostics.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using AspNetCore.ReportingServices.ReportProcessing.ReportObjectModel;

namespace FantasyPL.Pages
{
    public class Controller
    {
        DBManager dBManager;
        public Controller()
        {
            dBManager = new DBManager();
        }
        //TODO: functions in the requirement

        public User LogIn(string username, string password)
        {
            var sha = SHA256.Create();
            var byteArr = Encoding.Default.GetBytes(password);
            var hashedPasswordByte = sha.ComputeHash(byteArr);
            string hashedPassword = Convert.ToBase64String(hashedPasswordByte);
            password = hashedPassword;
            try
            {
                DBManager dBManager = new();
                string sql = "SELECT * FROM Users WHERE USERNAME = @Username AND PASSWORD = @Password";
                using (SqlCommand command = new SqlCommand(sql, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    using (SqlDataReader reader = dBManager.ExecuteReader(command))
                    {
                        User u = new User();
                        if (!reader.HasRows)
                            return null;
                        while (reader.Read())
                        {
                            u.Username = reader.GetString(0);
                            u.UserType = reader.GetString(7)[0];
                            if (u.UserType == 'F')
                            {
                                u.Balance = reader.GetInt32(12);
                                u.Points = reader.GetInt32(13);
                            }
                        }
                        reader.Close();
                        return u;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string ChangePassword(string username, string password)
        {
            var sha = SHA256.Create();
            var byteArr = Encoding.Default.GetBytes(password);
            var hashedPasswordByte = sha.ComputeHash(byteArr);
            string hashedPassword = Convert.ToBase64String(hashedPasswordByte);
            password = hashedPassword;
            try
            {
                String query = "UPDATE Users SET PASSWORD = @pass WHERE USERNAME = @name";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@name", username);
                    command.Parameters.AddWithValue("@pass", password);

                    if (dBManager.ExecuteNonQuery(command) > 0)
                    {
                        return "Password was Updated Successfully";
                    }
                    else
                    {
                        return "An error has occurred";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public string InsertUser(User userInfo)
        {
            var sha = SHA256.Create();
            var byteArr = Encoding.Default.GetBytes(userInfo.Password);
            var hashedPasswordByte = sha.ComputeHash(byteArr);
            string hashedPassword = Convert.ToBase64String(hashedPasswordByte);
            userInfo.Password = hashedPassword;
            try
            {
                String sql = "INSERT INTO Users " +
                             "VALUES(@Username, @Email, @Password, @FirstName, @MiddleName, @LastName, @Birthdate, @UserType, @Gender, @Country, @FantasyTeamName, @FavoriteClub, @Balance, @Points)";

                using (SqlCommand command = new SqlCommand(sql, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@Username", userInfo.Username);
                    command.Parameters.AddWithValue("@Email", userInfo.Email);
                    command.Parameters.AddWithValue("@Password", userInfo.Password);
                    command.Parameters.AddWithValue("@FirstName", userInfo.FirstName);
                    command.Parameters.AddWithValue("@MiddleName", userInfo.MiddleName);
                    command.Parameters.AddWithValue("@LastName", userInfo.LastName);
                    command.Parameters.AddWithValue("@Birthdate", userInfo.Birthdate);
                    command.Parameters.AddWithValue("@UserType", userInfo.UserType);
                    command.Parameters.AddWithValue("@Gender", userInfo.Gender);
                    command.Parameters.AddWithValue("@Country", userInfo.Country);
                    if (GlobalVar.isAdmin == true) {
                        command.Parameters.AddWithValue("@FantasyTeamName", DBNull.Value);
                        command.Parameters.AddWithValue("@FavoriteClub", DBNull.Value);
                        command.Parameters.AddWithValue("@Balance", DBNull.Value);
                        command.Parameters.AddWithValue("@Points", DBNull.Value);
                    }
                    else {

                        command.Parameters.AddWithValue("@FantasyTeamName", userInfo.FantasyTeamName);
                        if(userInfo.FavoriteClub != "-")
                            command.Parameters.AddWithValue("@FavoriteClub", userInfo.FavoriteClub);
                        else
							command.Parameters.AddWithValue("@FavoriteClub", DBNull.Value);
						command.Parameters.AddWithValue("@Balance", userInfo.Balance);
                        command.Parameters.AddWithValue("@Points", userInfo.Points);
                    }

                    if (dBManager.ExecuteNonQuery(command) > 0)
                    {
                        return "User was added Successfully";
                    }
                    else
                    {
                        return "An error has occurred";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public void GetAllUsers()
        {
            string proc = StoredProcedures.GetAllUsers;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();

            using (SqlDataReader reader = dBManager.ExecuteReader(proc, Parameters))
            {
                GlobalVar.listUser.Clear();
                while (reader.Read())
                {
                    User u = new User();
                    u.Username = reader.GetString(0);
                    GlobalVar.listUser.Add(u);
                }
                reader.Close();
            }
        }

        public string DeleteUser(string username)
        {
            string proc = StoredProcedures.DeleteUser;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@username", username);
            if (dBManager.ExecuteNonQuery(proc, Parameters) > 0)
            {
                return "User deleted successfully";
            }
            else
            {
                return "An error has occurred";
            }
        }

        public void UpdateClubsList()
        {
            GlobalVar.listClubs.Clear();
            String query = "SELECT * FROM Clubs ORDER BY Points DESC, Goals_For DESC, Goals_Against ASC";
            using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
            {
                using (SqlDataReader reader = dBManager.ExecuteReader(command))
                {
                    int pos = 1;
                    while (reader.Read())
                    {
                        Club club = new Club();
                        club.Position = pos;
                        club.Name = reader.GetString(0);
                        club.Name_Abbreviation = reader.GetString(1);
                        club.Establishment_year = reader.GetInt32(2);
                        club.City = reader.GetString(3);
                        club.Number_of_Trophies = reader.GetInt32(4);
                        club.Owner_Fname = reader.GetString(5);
                        club.Owner_Lname = reader.GetString(6);
                        club.Points = reader.GetInt32(7);
                        club.Wins = reader.GetInt32(8);
                        club.Draws = reader.GetInt32(9);
                        club.Losses = reader.GetInt32(10);
                        club.Goals_For = reader.GetInt32(11);
                        club.Goals_Against = reader.GetInt32(12);
                        pos++;
                        GlobalVar.listClubs.Add(club);
                    }
                    reader.Close();
                }
            }
        }

        public string InsertClub(Club clubInfo)
        {
            try
            {
                string query = "INSERT INTO Clubs VALUES(@name, @abbr, @year, @city, @trophies, @fname, @lname, 0, 0, 0, 0, 0, 0)";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@name", clubInfo.Name);
                    command.Parameters.AddWithValue("@abbr", clubInfo.Name_Abbreviation);
                    command.Parameters.AddWithValue("@year", clubInfo.Establishment_year);
                    command.Parameters.AddWithValue("@city", clubInfo.City);
                    command.Parameters.AddWithValue("@trophies", clubInfo.Number_of_Trophies);
                    command.Parameters.AddWithValue("@fname", clubInfo.Owner_Fname);
                    command.Parameters.AddWithValue("@lname", clubInfo.Owner_Lname);
                    if (dBManager.ExecuteNonQuery(command) > 0)
                    {
                        return "Club was added Successfully";
                    }
                    else
                    {
                        return "An error has occurred";
                    }
                }
            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public Club SelectClubByName(string abbr)
        {
            try {
                String query = "SELECT * FROM Clubs WHERE Name_Abbreviation = @abbr";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@abbr", abbr);
                    using (SqlDataReader reader = dBManager.ExecuteReader(command))
                    {
                        Club club = new Club();
                        while (reader.Read())
                        {
                            club.Name = reader.GetString(0);
                            club.Name_Abbreviation = reader.GetString(1);
                            club.Establishment_year = reader.GetInt32(2);
                            club.City = reader.GetString(3);
                            club.Number_of_Trophies = reader.GetInt32(4);
                            club.Owner_Fname = reader.GetString(5);
                            club.Owner_Lname = reader.GetString(6);
                            club.Points = reader.GetInt32(7);
                            club.Wins = reader.GetInt32(8);
                            club.Draws = reader.GetInt32(9);
                            club.Losses = reader.GetInt32(10);
                            club.Goals_For = reader.GetInt32(11);
                            club.Goals_Against = reader.GetInt32(12);
                        }
                        reader.Close();
                        return club;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public Club SelectClubByAbbr(string abbr)
        {
            try
            {
                String query = "SELECT * FROM Clubs WHERE NAME_ABBREVIATION = @abbr";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@abbr", abbr);
                    using (SqlDataReader reader = dBManager.ExecuteReader(command))
                    {
                        Club club = new Club();
                        while (reader.Read())
                        {
                            club.Name = reader.GetString(0);
                            club.Name_Abbreviation = reader.GetString(1);
                            club.Establishment_year = reader.GetInt32(2);
                            club.City = reader.GetString(3);
                            club.Number_of_Trophies = reader.GetInt32(4);
                            club.Owner_Fname = reader.GetString(5);
                            club.Owner_Lname = reader.GetString(6);
                            club.Points = reader.GetInt32(7);
                            club.Wins = reader.GetInt32(8);
                            club.Draws = reader.GetInt32(9);
                            club.Losses = reader.GetInt32(10);
                            club.Goals_For = reader.GetInt32(11);
                            club.Goals_Against = reader.GetInt32(12);
                        }
                        reader.Close();
                        return club;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public string UpdateClub(Club club)
        {
            try
            {
                String query = "UPDATE Clubs SET NAME_ABBREVIATION = @abbr, ESTABLISHMENT_YEAR = @year, CITY = @city, NUMBER_OF_TROPHIES = @trophies, OWNER_FNAME = @fname, OWNER_LNAME = @LNAME WHERE NAME_ABBREVIATION = @name";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@name", club.Name);
                    command.Parameters.AddWithValue("@abbr", club.Name_Abbreviation);
                    command.Parameters.AddWithValue("@year", club.Establishment_year);
                    command.Parameters.AddWithValue("@city", club.City);
                    command.Parameters.AddWithValue("@trophies", club.Number_of_Trophies);
                    command.Parameters.AddWithValue("@fname", club.Owner_Fname);
                    command.Parameters.AddWithValue("@lname", club.Owner_Lname);
                    if (dBManager.ExecuteNonQuery(command) > 0)
                    {
                        UpdateClubsList();
                        
                        return "Club was edited Successfully";
                    }
                    else
                    {
                        return "An error has occurred";
                    }
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public string DeleteClub(string Name)
        {
            //DeletePlayers
            SelectPlayersByClubAbbr(Name);
            while (GlobalVar.clubPlayers.Count > 0)
            {
                //Update user balance here
                //Delete from Fantasy Team first
                DeleteFTplayer(Name, GlobalVar.clubPlayers[0].Player_Number);
                DeletePlayer(Name,GlobalVar.clubPlayers[0].Player_Number);
                GlobalVar.clubPlayers.RemoveAt(0);
            }
            try
            {
                string query = "DELETE FROM Clubs WHERE NAME_ABBREVIATION = @name";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@name", Name);
                    if (dBManager.ExecuteNonQuery(command) > 0)
                    {
                        UpdateClubsList();
                        return "Club was deleted Successfully";
                    }
                    else
                    {
                        return "An error has occurred";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "An error has occured";
            }
        }

        public int CheckFixtures(string club)
        {
            try
            {
                String query = "select count(*) from Fixtures where (@club = Fixtures.HOME_SIDE or @club= Fixtures.AWAY_SIDE)"; 
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@club", club);
                    using (SqlDataReader reader = dBManager.ExecuteReader(command))
                    {
                        int result = -1;
                        while (reader.Read())
                        {
                            result = reader.GetInt32(0);
                        }
                        reader.Close();
                        return result;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return -1;
            }
        }

        public void UpdatePlayersList()
        {
            GlobalVar.listPlayers.Clear();
            String query = "SELECT * FROM Players ORDER BY Points DESC";
            using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
            {
                using (SqlDataReader reader = dBManager.ExecuteReader(command))
                {
                    int pos = 1;
                    while (reader.Read())
                    {
                        player player = new player();
                        player.Club_Abbreviation = reader.GetString(0);
                        player.Player_Number = reader.GetInt32(1);
                        player.Fname = reader.GetString(2);
                        player.Mname = reader.GetString(3);
                        player.Lname = reader.GetString(4);
                        player.Price = reader.GetInt32(5);
                        player.Age = reader.GetInt32(6);
                        player.Height = reader.GetInt32(7);
                        player.Weight = reader.GetInt32(8);
                        player.Nationality = reader.GetString(9);
                        player.Debut_Year = reader.GetInt32(10);
                        player.Contract_Length = reader.GetInt32(11);
                        player.Points = reader.GetInt32(12);
                        player.Position = reader.GetString(13);
                        pos++;
                        GlobalVar.listPlayers.Add(player);
                    }
                    reader.Close();
                    foreach (var player in GlobalVar.listPlayers)
                    {
                        player.Club = SelectClubByAbbr(player.Club_Abbreviation).Name;
                    }
                }
            }
        }

        public player SelectPlayer(string club_abbr, int player_no)
        {
            try
            {
                String query = "SELECT * FROM Players WHERE CLUB_ABBREVIATION = @abbr AND PLAYER_NO = @number";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@abbr", club_abbr);
                    command.Parameters.AddWithValue("@number", player_no);
                    using (SqlDataReader reader = dBManager.ExecuteReader(command))
                    {
                        player player = new player();
                        while (reader.Read())
                        {
                            player.Club_Abbreviation = reader.GetString(0);
                            player.Player_Number = reader.GetInt32(1);
                            player.Fname = reader.GetString(2);
                            player.Mname = reader.GetString(3);
                            player.Lname = reader.GetString(4);
                            player.Price = reader.GetInt32(5);
                            player.Age = reader.GetInt32(6);
                            player.Height = reader.GetInt32(7);
                            player.Weight = reader.GetInt32(8);
                            player.Nationality = reader.GetString(9);
                            player.Debut_Year = reader.GetInt32(10);
                            player.Contract_Length = reader.GetInt32(11);
                            player.Points = reader.GetInt32(12);
                            player.Position = reader.GetString(13);
                        }
                        reader.Close();
                        return player;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public void SelectPlayersByClubAbbr(string club_abbr)
        {
            try
            {
                String query = "SELECT * FROM Players WHERE CLUB_ABBREVIATION = @club_abbr";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@club_abbr", club_abbr);
                    using (SqlDataReader reader = dBManager.ExecuteReader(command))
                    {
                        GlobalVar.clubPlayers.Clear();
                        while (reader.Read())
                        {
                            player player = new player();
                            player.Club_Abbreviation = reader.GetString(0);
                            player.Player_Number = reader.GetInt32(1);
                            player.Fname = reader.GetString(2);
                            player.Mname = reader.GetString(3);
                            player.Lname = reader.GetString(4);
                            player.Price = reader.GetInt32(5);
                            player.Age = reader.GetInt32(6);
                            player.Height = reader.GetInt32(7);
                            player.Weight = reader.GetInt32(8);
                            player.Nationality = reader.GetString(9);
                            player.Debut_Year = reader.GetInt32(10);
                            player.Contract_Length = reader.GetInt32(11);
                            player.Points = reader.GetInt32(12);
                            player.Position = reader.GetString(13);
                            GlobalVar.clubPlayers.Add(player);
                        }
                        reader.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return;
            }
        }

        public string UpdatePlayer(player p)
        {
            try
            {
                String query = "UPDATE Players SET FNAME = @fname, MNAME = @mname, LNAME = @lname, PRICE = @price, AGE = @age, HEIGHT = @height, WEIGHT = @weight, NATIONALITY = @nationality, DEBUT_YEAR = @year, CONTRACT_LENGTH = @contract, Points = @points, POSITION = @pos WHERE CLUB_ABBREVIATION = @abbr AND PLAYER_NO = @number";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@abbr", p.Club_Abbreviation);
                    command.Parameters.AddWithValue("@number", p.Player_Number);
                    command.Parameters.AddWithValue("@fname", p.Fname);
                    command.Parameters.AddWithValue("@mname", p.Mname);
                    command.Parameters.AddWithValue("@pos", p.Position);
                    command.Parameters.AddWithValue("@lname", p.Lname);
                    command.Parameters.AddWithValue("@price", p.Price);
                    command.Parameters.AddWithValue("@age", p.Age);
                    command.Parameters.AddWithValue("@height", p.Height);
                    command.Parameters.AddWithValue("@weight", p.Weight);
                    command.Parameters.AddWithValue("@nationality", p.Nationality);
                    command.Parameters.AddWithValue("@year", p.Debut_Year);
                    command.Parameters.AddWithValue("@contract", p.Contract_Length);
                    command.Parameters.AddWithValue("@points", p.Points);
                    if (dBManager.ExecuteNonQuery(command) > 0)
                    {
                        UpdatePlayersList();
                        return "Player was edited Successfully";
                    }
                    else
                    {
                        return "An error has occurred";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public string InsertPlayer(player p)
        {
            try
            {
                String query = "INSERT INTO Players VALUES(@abbr, @number, @fname, @mname, @lname, @price, @age, @height, @weight, @nationality, @year, @contract, @points, @position, @code)";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@abbr", p.Club_Abbreviation);
                    command.Parameters.AddWithValue("@number", p.Player_Number);
                    command.Parameters.AddWithValue("@fname", p.Fname);
                    command.Parameters.AddWithValue("@mname", p.Mname);
                    command.Parameters.AddWithValue("@lname", p.Lname);
                    command.Parameters.AddWithValue("@price", p.Price);
                    command.Parameters.AddWithValue("@age", p.Age);
                    command.Parameters.AddWithValue("@height", p.Height);
                    command.Parameters.AddWithValue("@weight", p.Weight);
                    command.Parameters.AddWithValue("@nationality", p.Nationality);
                    command.Parameters.AddWithValue("@year", p.Debut_Year);
                    command.Parameters.AddWithValue("@contract", p.Contract_Length);
                    command.Parameters.AddWithValue("@points", p.Points);
                    command.Parameters.AddWithValue("@position", p.Position);
                    command.Parameters.AddWithValue("@code", p.FPLcode);
                    if (dBManager.ExecuteNonQuery(command) > 0)
                    {
                        UpdatePlayersList();
                        return "Player was added Successfully";
                    }
                    else
                    {
                        return "An error has occurred";
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public string DeletePlayer(string abbr, int number)
        {
            try
            {
                String query = "DELETE FROM Players WHERE CLUB_ABBREVIATION = @abbr AND PLAYER_NO = @number";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@abbr", abbr);
                    command.Parameters.AddWithValue("@number", number);
                    if (dBManager.ExecuteNonQuery(command) > 0)
                    {
                        UpdatePlayersList();
                        return "Player was Deleted Successfully";
                    }
                    else
                    {
                        return "An error has occurred";
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public string InsertFTplayer(string username, string club_abbr, int player_no)
        {
            try
            {
                String query = "INSERT INTO FANTASY_TEAM values(@username, @abbr, @number)";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@abbr", club_abbr);
                    command.Parameters.AddWithValue("@number", player_no);
                    if (dBManager.ExecuteNonQuery(command) > 0)
                    {
                        SelectPlayersByUsername(username);
                        return "Player was added Successfully";
                    }
                    else
                    {
                        return "An error has occurred";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public string DeleteFTplayer(string club_abbr, int player_no, string username = "")
        {
            try
            {
                String query = "";
                if (username != "")
                {
                    query = "DELETE FROM FANTASY_TEAM WHERE USERNAME = @username AND CLUB_ABBREVIATION = @abbr AND PLAYER_NO = @number";

                } else
                {
                    query = "DELETE FROM FANTASY_TEAM WHERE CLUB_ABBREVIATION = @abbr AND PLAYER_NO = @number";
                }
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@abbr", club_abbr);
                    command.Parameters.AddWithValue("@number", player_no);
                    if (dBManager.ExecuteNonQuery(command) > 0)
                    {
                        SelectPlayersByUsername(username);
                        return "Player was deleted Successfully";
                    }
                    else
                    {
                        return "An error has occurred";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public void SelectPlayersByUsername(string Username)
        {
            try
            {
                String query = "SELECT P.* FROM Players P, FANTASY_TEAM F WHERE F.Username = @username AND F.CLUB_ABBREVIATION = P.CLUB_ABBREVIATION AND F.PLAYER_NO = P.PLAYER_NO";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@username", Username);
                    using (SqlDataReader reader = dBManager.ExecuteReader(command))
                    {
                        GlobalVar.userPlayers.Clear();
                        int i = 1;
                        while (reader.Read())
                        {
                            player player = new player();
                            player.Club_Abbreviation = reader.GetString(0);
                            player.Player_Number = reader.GetInt32(1);
                            player.Fname = reader.GetString(2);
                            player.Mname = reader.GetString(3);
                            player.Lname = reader.GetString(4);
                            player.Price = reader.GetInt32(5);
                            player.Age = reader.GetInt32(6);
                            player.Height = reader.GetInt32(7);
                            player.Weight = reader.GetInt32(8);
                            player.Nationality = reader.GetString(9);
                            player.Debut_Year = reader.GetInt32(10);
                            player.Contract_Length = reader.GetInt32(11);
                            player.Points = reader.GetInt32(12);
                            player.Position = reader.GetString(13);
                            player.Count = i++;
                            player.FPLcode = reader.GetInt32(14);
                            GlobalVar.userPlayers.Add(player);
                        }
                        reader.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return;
            }
        }

        public int CountPositionforUsername(string Username, string position)
        {
            try
            {
                String query = "SELECT Count(*) FROM Players P, FANTASY_TEAM F WHERE F.Username = @username AND F.CLUB_ABBREVIATION = P.CLUB_ABBREVIATION AND F.PLAYER_NO = P.PLAYER_NO AND P.Position = @pos";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@username", Username);
                    command.Parameters.AddWithValue("@pos", position);
                    return (int)command.ExecuteScalar();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return -1;
            }
        }

        public void UpdateFixturesByWeek(int gameweek)
        {
            try
            {
                String query = "SELECT * FROM Fixtures WHERE GAMEWEEK = @week";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@week", gameweek);
                    using (SqlDataReader reader = dBManager.ExecuteReader(command))
                    {
                        GlobalVar.weekFixtures.Clear();
                        int refID = 0;
                        while (reader.Read())
                        {
                            Fixture fixture = new Fixture();
                            fixture.ID = reader.GetInt32(0);
                            fixture.Gameweek = reader.GetInt32(1);
                            fixture.Date = reader.GetDateTime(2).ToShortDateString();
                            fixture.StartTime = reader.GetTimeSpan(3).ToString();
                            if (reader["HOME_SCORE"] != DBNull.Value)
                                fixture.HomeScore = reader.GetInt32(4).ToString();
                            else
                                fixture.HomeScore = "-";
                            if (reader["AWAY_SCORE"] != DBNull.Value)
                                fixture.AwayScore = reader.GetInt32(5).ToString();
                            else
                                fixture.AwayScore = "-";
                            fixture.HomeSide = reader.GetString(6);
                            fixture.AwaySide = reader.GetString(7);
                            if (reader["STADIUM"] != DBNull.Value)
                                fixture.Stadium = reader.GetString(8);
                            else
                                fixture.Stadium = "-";
                            if (reader["REFEREE_ID"] != DBNull.Value)
                                fixture.refID = reader.GetInt32(9);
                            else
                                fixture.refID = 0;
                            GlobalVar.weekFixtures.Add(fixture);
                        }
                        reader.Close();
                        foreach(var fix in GlobalVar.weekFixtures) {
                            fix.Referee = GetRefereeName(fix.refID);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return;
            }
        }

        public string GetRefereeName(int id)
        {
            try
            {
                String query = "SELECT FNAME FROM Referee WHERE ID = @id";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = dBManager.ExecuteReader(command))
                    {
                        string name = "";
                        while (reader.Read())
                        {
                            name = reader.GetString(0);
                        }
                        reader.Close();
                        return name;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return "";
            }
        }

        public void UpdateFixtureEvents(int id, int option = 1)
        {
            try
            {
                String query = "";
                if (option == 1)
                {
                    query = "SELECT * FROM Match_Events WHERE FIXTURE_ID = @id ORDER BY MINUTE ASC";
                } else
                {
					query = "SELECT * FROM Match_Events WHERE FIXTURE_ID = @id AND EVENT_TYPE NOT IN ('END', 'START') ORDER BY MINUTE ASC";
				}
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = dBManager.ExecuteReader(command))
                    {
                        GlobalVar.fixtureEvents.Clear();
                        while (reader.Read())
                        {
                            MEvent e = new MEvent();
                            e.ID = reader.GetInt32(0);
                            e.FixtureID = reader.GetInt32(1);
                            e.EventType = reader.GetString(2);
                            e.Minute = reader.GetInt32(3);
                            if (reader["CLUB_ABBREVIATION"] != DBNull.Value)
                            {
                                e.ClubAbbreviation = reader.GetString(4);
                            } else
                            {
                                e.ClubAbbreviation = "-";
                            }
                            if (reader["PLAYER_NO"] != DBNull.Value)
                            {
                                e.Player = reader.GetInt32(5).ToString();
                            } else
                            {
                                e.Player = "-";
                            }
                            GlobalVar.fixtureEvents.Add(e);
                        }
                        reader.Close();
                        foreach(var e in GlobalVar.fixtureEvents)
                        {
                            if (e.Player != "-")
                                e.Player = SelectPlayer(e.ClubAbbreviation, Convert.ToInt32(e.Player)).Fname;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return;
            }
        }

        public Fixture SelectFixture(int id)
        {
            try
            {
                String query = "SELECT * FROM Fixtures WHERE FIXTURE_ID = @id";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = dBManager.ExecuteReader(command))
                    {
                        Fixture fixture = new Fixture();
                        int refID = 0;
                        while (reader.Read())
                        {
                            fixture.ID = reader.GetInt32(0);
                            fixture.Gameweek = reader.GetInt32(1);
                            fixture.Date = reader.GetDateTime(2).ToShortDateString();
                            fixture.StartTime = reader.GetTimeSpan(3).ToString();
                            if (reader["HOME_SCORE"] != DBNull.Value)
                                fixture.HomeScore = reader.GetInt32(4).ToString();
                            else
                                fixture.HomeScore = "-";
                            if (reader["AWAY_SCORE"] != DBNull.Value)
                                fixture.AwayScore = reader.GetInt32(5).ToString();
                            else
                                fixture.AwayScore = "-";
                            fixture.HomeSide = reader.GetString(6);
                            fixture.AwaySide = reader.GetString(7);
                            if (reader["STADIUM"] != DBNull.Value)
                                fixture.Stadium = reader.GetString(8);
                            else
                                fixture.Stadium = "-";
                            if (reader["REFEREE_ID"] != DBNull.Value)
                                refID = reader.GetInt32(9);
                            else
                                refID = 0;
                        }
                        reader.Close();
                        fixture.Referee = GetRefereeName(refID);
                        return fixture;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        public string InsertEvent(int FID, int EID, string type, int min, string abbr, int pno)
        {
            try
            {
                String sql = "INSERT INTO Match_Events " +
                             "VALUES(@EID, @FID, @type, @min, @abbr, @pno)";

                using (SqlCommand command = new SqlCommand(sql, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@FID", FID);
                    command.Parameters.AddWithValue("@EID", EID);
                    command.Parameters.AddWithValue("@type", type);
                    command.Parameters.AddWithValue("@min", min);
                    if (abbr == "-")
                    {
                        command.Parameters.AddWithValue("@abbr", DBNull.Value);
                    } else
                    {
						command.Parameters.AddWithValue("@abbr", abbr);
					}
                    if(pno == -1)
                        command.Parameters.AddWithValue("@pno", DBNull.Value);
                    else
						command.Parameters.AddWithValue("@pno", pno);
					if (dBManager.ExecuteNonQuery(command) > 0)
                    {
                        return "Event was added Successfully";
                    }
                    else
                    {
                        return "An error has occurred";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public int lastEventID(int FID)
        {
            try
            {
                String query = "SELECT TOP 1 EVENT_ID FROM Match_Events WHERE FIXTURE_ID = @FID ORDER BY EVENT_ID DESC";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@FID", FID);
                    using (SqlDataReader reader = dBManager.ExecuteReader(command))
                    {
                        if (reader.HasRows)
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);
                            reader.Close();
                            return Convert.ToInt32(dt.Rows[0][0]);
                        } else
                        {
                            return 0;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return 0;
            }
        }

        public int lastEventMin(int FID)
        {
            try
            {
                String query = "SELECT TOP 1 MINUTE FROM Match_Events WHERE FIXTURE_ID = @FID ORDER BY MINUTE DESC";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@FID", FID);
                    using (SqlDataReader reader = dBManager.ExecuteReader(command))
                    {
                        if (reader.HasRows)
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);
                            reader.Close();
                            return Convert.ToInt32(dt.Rows[0][0]);
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return 0;
            }
        }

        public int lastFixtureID()
        {
            try
            {
                String query = "SELECT TOP 1 FIXTURE_ID FROM Fixtures ORDER BY FIXTURE_ID DESC";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    using (SqlDataReader reader = dBManager.ExecuteReader(command))
                    {
                        if (reader.HasRows)
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);
                            reader.Close();
                            return Convert.ToInt32(dt.Rows[0][0]);
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return 0;
            }
        }

        public string InsertFixture(int gw, string date, string time, string home, string away, string stadium, string referee)
        {
            try
            {
                String sql = "INSERT INTO Fixtures(FIXTURE_ID, GAMEWEEK, DATE, START_TIME, HOME_SIDE, AWAY_SIDE, STADIUM, REFEREE_ID) " +
                             "VALUES(@ID, @gw, @date, @time, @home, @away, @stadium, @referee)";

                using (SqlCommand command = new SqlCommand(sql, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@ID", lastFixtureID()+1);
                    command.Parameters.AddWithValue("@gw", gw);
                    command.Parameters.AddWithValue("@date", date);
                    command.Parameters.AddWithValue("@time", time);
                    command.Parameters.AddWithValue("@home", home);
                    command.Parameters.AddWithValue("@away", away);
                    command.Parameters.AddWithValue("@stadium", stadium);
                    command.Parameters.AddWithValue("@referee", referee);
                    if (dBManager.ExecuteNonQuery(command) > 0)
                    {
                        return "Fixture was added Successfully";
                    }
                    else
                    {
                        return "An error has occurred";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public void UpdateRefereesList()
        {
            String query = "SELECT * FROM Referee";
            using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
            {
                using (SqlDataReader reader = dBManager.ExecuteReader(command))
                {
                    GlobalVar.listReferees.Clear();
                    while (reader.Read())
                    {
						Referee referee = new Referee();
						referee.ID = reader.GetInt32(0);
                        referee.FName = reader.GetString(1);
                        referee.MName = reader.GetString(2);
                        referee.LName = reader.GetString(3);
                        referee.Age = reader.GetInt32(4);
                        referee.Nationality = reader.GetString(5);
                        referee.Experience = reader.GetInt32(6);
                        GlobalVar.listReferees.Add(referee);
                    }
                    reader.Close();
                }
            }
        }

        public void UpdateFixturesList()
        {
            String query = "SELECT * FROM Fixtures";
            using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
            {
                using (SqlDataReader reader = dBManager.ExecuteReader(command))
                {
                    GlobalVar.listFixtures.Clear();
                    while (reader.Read())
                    {
                        Fixture fixture = new Fixture();
                        fixture.ID = reader.GetInt32(0);
                        fixture.Gameweek = reader.GetInt32(1);
                        fixture.Date = reader.GetDateTime(2).ToShortDateString();
                        fixture.StartTime = reader.GetTimeSpan(3).ToString();
                        if (reader["HOME_SCORE"] != DBNull.Value)
                            fixture.HomeScore = reader.GetInt32(4).ToString();
                        else
                            fixture.HomeScore = "-";
                        if (reader["AWAY_SCORE"] != DBNull.Value)
                            fixture.AwayScore = reader.GetInt32(5).ToString();
                        else
                            fixture.AwayScore = "-";
                        fixture.HomeSide = reader.GetString(6);
                        fixture.AwaySide = reader.GetString(7);
                        if (reader["STADIUM"] != DBNull.Value)
                            fixture.Stadium = reader.GetString(8);
                        else
                            fixture.Stadium = "-";
                        if (reader["REFEREE_ID"] != DBNull.Value)
                            fixture.Referee = reader.GetInt32(9).ToString();
                        else
                            fixture.Referee = "-";
                        GlobalVar.listFixtures.Add(fixture);
                    }
                    reader.Close();
                }
            }
        }

        public string UpdateFixture(int id, int gw, string date, string time, string home, string away, string stadium, int referee)
        {
            try
            {
                String query = "UPDATE Fixtures SET GAMEWEEK = @gw, DATE = @date, START_TIME = @time, HOME_SIDE = @home, AWAY_SIDE = @away, STADIUM = @stadium, REFEREE_ID = @referee WHERE FIXTURE_ID = @id";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@gw", gw);
                    command.Parameters.AddWithValue("@date", date);
                    command.Parameters.AddWithValue("@time", time);
                    command.Parameters.AddWithValue("@home", home);
                    command.Parameters.AddWithValue("@away", away);
                    command.Parameters.AddWithValue("@stadium", stadium);
                    command.Parameters.AddWithValue("@referee", referee);
                    if (dBManager.ExecuteNonQuery(command) > 0)
                    {
                        UpdatePlayersList();
                        return "Fixture was edited Successfully";
                    }
                    else
                    {
                        return "An error has occurred";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public string SelectStadiumByAbbr(string abbr)
        {
            try
            {
                String query = "SELECT NAME FROM Stadiums WHERE CLUB_ABBREVIATION = @abbr";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@abbr", abbr);
                    using (SqlDataReader reader = dBManager.ExecuteReader(command))
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            string stadium = reader.GetString(0);
                            reader.Close();
                            return stadium;
                        }
                        else
                            return "-";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public string SelectManagerByAbbr(string abbr)
        {
            try
            {
                String query = "SELECT FNAME FROM Manager WHERE CLUB_MANAGED = @abbr";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@abbr", abbr);
                    using (SqlDataReader reader = dBManager.ExecuteReader(command))
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            string stadium = reader.GetString(0);
                            reader.Close();
                            return stadium;
                        }
                        else
                            return "-";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public string DeleteFixture(int Fixture_ID)
        {
            try
            {
                UpdateFixtureEvents(Fixture_ID);
                while(GlobalVar.fixtureEvents.Count > 0)
                {
                    DeleteEventofFixture(Fixture_ID, GlobalVar.fixtureEvents[0].ID);
                }
                string query = "DELETE FROM Fixtures WHERE FIXTURE_ID = @id";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@id", Fixture_ID);
                    if (dBManager.ExecuteNonQuery(command) > 0)
                    {
                        UpdateFixturesList();
                        return "Fixture was deleted successfully";
                    }
                    else
                    {
                        return "An error has occurred";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public string DeleteEventofFixture(int Fixture_ID,int Event_ID)
        {
            try
            {
                string query = "DELETE FROM Match_Events WHERE FIXTURE_ID = @F_id and Event_ID = @E_id";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@F_id", Fixture_ID);
                    command.Parameters.AddWithValue("@E_id", Event_ID);
                    if (dBManager.ExecuteNonQuery(command) > 0)
                    {
                        UpdateFixtureEvents(Fixture_ID);
                        return "Event was deleted successfully";
                    }
                    else
                    {
                        return "An error has occurred";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public void UpdateCompetitionsByUsername(string Username)
        {
                
            String proc = StoredProcedures.GetCompetitionsForUser;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@username", Username);
            using (SqlDataReader reader = dBManager.ExecuteReader(proc, Parameters))
            {
                    GlobalVar.userComp.Clear();
                    while (reader.Read())
                    {
                        Competitions comp = new Competitions();
                        comp.Id = reader.GetInt32(0);
                        comp.Name = reader.GetString(1);
                        GlobalVar.userComp.Add(comp);
                    }
                    reader.Close();
            }
        }

        public void GetParticipantsInCompetition(int id)
        {
            string proc = StoredProcedures.GetParticipants;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@CompID", id);

            using (SqlDataReader reader = dBManager.ExecuteReader(proc, Parameters))
            {
                GlobalVar.compParticipants.Clear();
                while (reader.Read())
                {
                    User u = new User();
                    u.Username = reader.GetString(0);
                    u.FantasyTeamName = reader.GetString(1);
                    u.Points= reader.GetInt32(2);
                    GlobalVar.compParticipants.Add(u);
                }
                reader.Close();
            }
        }

        public int lastCompID()
        {
            try
            {
                String query = "SELECT TOP 1 ID FROM Competitions ORDER BY ID DESC";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    using (SqlDataReader reader = dBManager.ExecuteReader(command))
                    {
                        if (reader.HasRows)
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);
                            reader.Close();
                            return Convert.ToInt32(dt.Rows[0][0]);
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return 0;
            }
        }

        public string CreateCompetition(Competitions comp)
        {
            var sha = SHA256.Create();
            var byteArr = Encoding.Default.GetBytes(comp.Password);
            var hashedPasswordByte = sha.ComputeHash(byteArr);
            string hashedPassword = Convert.ToBase64String(hashedPasswordByte);
            comp.Password = hashedPassword;
            string proc = StoredProcedures.CreateCompetition;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@id", comp.Id);
            Parameters.Add("@name", comp.Name);
            Parameters.Add("@password", comp.Password);
            Parameters.Add("@capacity", comp.Capacity);
            
            if (dBManager.ExecuteNonQuery(proc, Parameters) > 0)
            {
                UpdateCompetitionsByUsername(GlobalVar.LoggedInUser.Username);
                return "Competition created successfully";
            }
            else
            {
                return "An error has occured";
            }
            
        }

        public String JoinCompetition(int id, string password)
        {
            var sha = SHA256.Create();
            var byteArr = Encoding.Default.GetBytes(password);
            var hashedPasswordByte = sha.ComputeHash(byteArr);
            string hashedPassword = Convert.ToBase64String(hashedPasswordByte);
            password = hashedPassword;
            string proc = StoredProcedures.JoinCompetition;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@username", GlobalVar.LoggedInUser.Username);
            Parameters.Add("@id", id);
            Parameters.Add("@password", password);
            if (dBManager.ExecuteNonQuery(proc, Parameters) > 0)
            {
                UpdateCompetitionsByUsername(GlobalVar.LoggedInUser.Username);
                return "You joined successfully";
            }
            else
            {
                return "Invalid Competition Details or the Competition is full";
            }
        }

        public String ExitCompetition(string username, int id)
        {
            string proc = StoredProcedures.ExitCompetition;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@username", username);
            Parameters.Add("@id", id);
            if (dBManager.ExecuteNonQuery(proc, Parameters) > 0)
            {
                UpdateCompetitionsByUsername(GlobalVar.LoggedInUser.Username);
                return "User exited successfully";
            }
            else
            {
                return "An error has occurred";
            }
        }

        public String DeleteCompetition(int id)
        {
            string proc = StoredProcedures.DeleteCompetition;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@id", id);
            if (dBManager.ExecuteNonQuery(proc, Parameters) > 0)
            {
                return "Competition deleted successfully";
            }
            else
            {
                return "An error has occurred";
            }
        }

        public void GetAllCompetitions()
        {
            string proc = StoredProcedures.GetAllCompetitions;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();

            using (SqlDataReader reader = dBManager.ExecuteReader(proc, Parameters))
            {
                GlobalVar.listComp.Clear();
                while (reader.Read())
                {
                    Competitions c = new();
                    c.Id = reader.GetInt32(0);
                    c.Name = reader.GetString(1);
                    c.Password = reader.GetString(2);
                    c.Capacity = reader.GetInt32(3);
                    GlobalVar.listComp.Add(c);
                }
                reader.Close();
            }
        }

        public void GetNewCompetitions(string username)
        {
            string proc = StoredProcedures.GetNewCompetitions;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@username", username);
            using (SqlDataReader reader = dBManager.ExecuteReader(proc, Parameters))
            {
                GlobalVar.newComp.Clear();
                while (reader.Read())
                {
                    Competitions c = new();
                    c.Id = reader.GetInt32(0);
                    c.Name = reader.GetString(1);
                    c.Password = reader.GetString(2);
                    c.Capacity = reader.GetInt32(3);
                    GlobalVar.newComp.Add(c);
                }
                reader.Close();
            }
        }

        public void UpdateStadiumsList()
        {
            string proc = StoredProcedures.GetNewStadiums;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            using (SqlDataReader reader = dBManager.ExecuteReader(proc, Parameters))
            {
                GlobalVar.listStadiums.Clear();
                while (reader.Read())
                {
                    Stadium stadium = new Stadium();
                    stadium.name = reader.GetString(0);
                    stadium.capacity = reader.GetInt32(1);
                    stadium.city = reader.GetString(2);
                    stadium.size = reader.GetInt32(3);
                    GlobalVar.listStadiums.Add(stadium);
                }
                reader.Close();
            }
        }

		public void UpdateAllStadiums()
		{
			string proc = StoredProcedures.GetStadiums;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			using (SqlDataReader reader = dBManager.ExecuteReader(proc, Parameters))
			{
				GlobalVar.allStadiums.Clear();
				while (reader.Read())
				{
					Stadium stadium = new Stadium();
					stadium.name = reader.GetString(0);
					stadium.capacity = reader.GetInt32(1);
					stadium.city = reader.GetString(2);
					stadium.size = reader.GetInt32(3);
					GlobalVar.allStadiums.Add(stadium);
				}
				reader.Close();
			}
		}

		public String DeleteStadium(string name)
		{
			string proc = StoredProcedures.DeleteStadium;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@name", name);
			if (dBManager.ExecuteNonQuery(proc, Parameters) > 0)
			{
				return "Stadium deleted successfully";
			}
			else
			{
				return "An error has occurred";
			}
		}

		public void UpdateManagersList()
		{
			string proc = StoredProcedures.GetNewManager;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			using (SqlDataReader reader = dBManager.ExecuteReader(proc, Parameters))
			{
				GlobalVar.listManagers.Clear();
				while (reader.Read())
				{
					Manager man = new();
					man.ID = reader.GetInt32(0);
					man.FName = reader.GetString(1);
					man.MName = reader.GetString(2);
					man.LName = reader.GetString(3);
					GlobalVar.listManagers.Add(man);
				}
				reader.Close();
			}
		}

		public void UpdateAllManagers()
		{
			string proc = StoredProcedures.GetManagers;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			using (SqlDataReader reader = dBManager.ExecuteReader(proc, Parameters))
			{
				GlobalVar.allManagers.Clear();
				while (reader.Read())
				{
					Manager man = new();
					man.ID = reader.GetInt32(0);
					man.FName = reader.GetString(1);
					man.MName = reader.GetString(2);
					man.LName = reader.GetString(3);
					GlobalVar.allManagers.Add(man);
				}
				reader.Close();
			}
		}

		public String DeleteManager(int id)
		{
			string proc = StoredProcedures.DeleteManager;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@id", id);
			if (dBManager.ExecuteNonQuery(proc, Parameters) > 0)
			{
				return "Manager deleted successfully";
			}
			else
			{
				return "An error has occurred";
			}
		}
		public int SetStadium(string club_abbr, string name)
        {
            string proc = StoredProcedures.SetStadium;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@abbr", club_abbr);
            Parameters.Add("@name", name);
            return dBManager.ExecuteNonQuery(proc, Parameters);
        }

		public int SetManager(string club_abbr, int id)
		{
			string proc = StoredProcedures.SetManager;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@abbr", club_abbr);
			Parameters.Add("@id", id);
			return dBManager.ExecuteNonQuery(proc, Parameters);
		}

		public int lastManID()
        {
            try
            {
                String query = "SELECT TOP 1 ID FROM Manager ORDER BY ID DESC";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    using (SqlDataReader reader = dBManager.ExecuteReader(command))
                    {
                        if (reader.HasRows)
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);
                            reader.Close();
                            return Convert.ToInt32(dt.Rows[0][0]);
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return 0;
            }
        }

		public string InsertStadium(string name, int capacity, string city, int size)
		{
			string proc = StoredProcedures.InsertStadium;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@name", name);
			Parameters.Add("@capacity", capacity);
			Parameters.Add("@city", city);
			Parameters.Add("@size", size);

			if (dBManager.ExecuteNonQuery(proc, Parameters) > 0)
			{
				return "Stadium added successfully";
			}
			else
			{
				return "An error has occured";
			}
		}

		public string InsertManager(Manager m)
		{
			string proc = StoredProcedures.InsertManager;
			Dictionary<string, object> Parameters = new Dictionary<string, object>();
			Parameters.Add("@id", m.ID);
			Parameters.Add("@fname", m.FName);
			Parameters.Add("@mname", m.MName);
			Parameters.Add("@lname", m.LName);
			Parameters.Add("@age", m.age);
			Parameters.Add("@nationality", m.nationality);
			Parameters.Add("@competitionswon", m.competitions_won);
			Parameters.Add("@exp", m.experience_in_years);

			if (dBManager.ExecuteNonQuery(proc, Parameters) > 0)
			{
				return "Manager added successfully";
			}
			else
			{
				return "An error has occured";
			}

		}

        public int GetBalanceOfUser(string username)
        {
            try
            {
                String sql = "select balance from users where username = @user";

                using (SqlCommand command = new SqlCommand(sql, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@user", username);
                    using (SqlDataReader reader = dBManager.ExecuteReader(command))
                    {
                        reader.Read();
                        int balance = reader.GetInt32(0);
                        reader.Close();
                        return balance;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        public int GetConfiguration()
        {
            try
            {
                String sql = "select * from AllowEditConfig";

                using (SqlCommand command = new SqlCommand(sql, dBManager.myConnection))
                {
                    
                    using (SqlDataReader reader = dBManager.ExecuteReader(command))
                    {
                        reader.Read();
                        int config = reader.GetInt32(0);
                        reader.Close();
                        return config;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        public bool GetFT()
        {
            try
            {
                String sql = "select * from AllowEditConfig";

                using (SqlCommand command = new SqlCommand(sql, dBManager.myConnection))
                {

                    using (SqlDataReader reader = dBManager.ExecuteReader(command))
                    {
                        reader.Read();
                        bool config = reader.GetBoolean(0);
                        reader.Close();
                        return config;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public void ToggleFT()
        {
            try
            {
                String sql = "update AllowEditConfig set Config =  case when Config = 1 then 0 else 1 end";

                using (SqlCommand command = new SqlCommand(sql, dBManager.myConnection))
                {
                    dBManager.ExecuteNonQuery(command);
                   
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }





        public int GetPriceOfPlayer(string club_abbr,int player_no)
        {
            try
            {
                String query = "SELECT PRICE FROM PLAYERS WHERE CLUB_ABBREVIATION = @club_abbr AND PLAYER_NO = @player_no";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@club_abbr", club_abbr);
                    command.Parameters.AddWithValue("@player_no", player_no);
                    using (SqlDataReader reader = dBManager.ExecuteReader(command))
                    {
                        int price = 0;
                        while (reader.Read())
                        {
                            price = reader.GetInt32(0);
                        }
                        reader.Close();
                        return price;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return -1;
            }
        }

        public DataTable GetStadiumInfo()
        {
            string query = "Select * From Stadiums";
            using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
            {
                using (SqlDataReader reader = dBManager.ExecuteReader(command))
                {
                    DataTable dt = new DataTable();
                    if (reader.HasRows)
                    {
                        dt.Load(reader);
                        reader.Close();
                        return dt;
                    }
                    else
                    {
                        reader.Close();
                        return dt;
                    }
                }
            }
        }

		public DataTable FavClub()
		{
			string query = "Select * From Users Where User_type != 'A'";
			using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
			{
				using (SqlDataReader reader = dBManager.ExecuteReader(command))
				{
					DataTable dt = new DataTable();
					if (reader.HasRows)
					{
						dt.Load(reader);
						reader.Close();
						return dt;
					}
					else
					{
						reader.Close();
						return dt;
					}
				}
			}
		}

		public int lastRefID()
        {
            try
            {
                String query = "SELECT TOP 1 ID FROM Referee ORDER BY ID DESC";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    using (SqlDataReader reader = dBManager.ExecuteReader(command))
                    {
                        if (reader.HasRows)
                        {
                            DataTable dt = new DataTable();
                            dt.Load(reader);
                            reader.Close();
                            return Convert.ToInt32(dt.Rows[0][0]);
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return 0;
            }
        }

        public string InsertReferee(Referee referee)
        {
            try
            {
                String sql = "INSERT INTO Referee " +
                             "VALUES(@ID, @fname, @mname, @lname, @age, @nationality, @exp)";

                using (SqlCommand command = new SqlCommand(sql, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@ID", referee.ID);
                    command.Parameters.AddWithValue("@fname", referee.FName);
                    command.Parameters.AddWithValue("@mname", referee.MName);
                    command.Parameters.AddWithValue("@lname", referee.LName);
                    command.Parameters.AddWithValue("@age", referee.Age);
                    command.Parameters.AddWithValue("@nationality", referee.Nationality);
                    command.Parameters.AddWithValue("@exp", referee.Experience);
                    if (dBManager.ExecuteNonQuery(command) > 0)
                    {
                        return "Referee was added Successfully";
                    }
                    else
                    {
                        return "An error has occurred";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public String DeleteReferee(int id)
        {
            string query = "DELETE FROM Referee WHERE ID = @id";
            using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
            {
                command.Parameters.AddWithValue("@id", id);
                if (dBManager.ExecuteNonQuery(command) > 0)
                {
                    UpdateFixturesList();
                    return "Referee deleted successfully";
                }
                else
                {
                    return "An error has occurred";
                }
            }
        }


        public string GetFavClub(string username)
        {
            try
            {
                String query = "SELECT CLUB_SUPPORTED FROM USERS WHERE USERNAME = @name";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@name", username);

                    using (SqlDataReader reader = dBManager.ExecuteReader(command))
                    {
                        if (reader.HasRows)
                        {
                            string club = "";
                            reader.Read();
                            if (reader["CLUB_SUPPORTED"] != DBNull.Value)
                                club = reader.GetString(0);
                            else
                                club = "-";
  
                            reader.Close();
                            return club;
                        }
                        else
                        {
                            return "-";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public bool HasStartEvent(int fixId)
        {
            try
            {
                String query = "SELECT Event_id from match_events where fixture_id = @fixId and event_type = 'Start' ";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("fixId", fixId);
                    using (SqlDataReader reader = dBManager.ExecuteReader(command))
                    {
                        return reader.HasRows;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }


        public bool HasEndEvent(int fixId)
        {
            try
            {
                String query = "SELECT Event_id from match_events where fixture_id = @fixId and event_type = 'End' ";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("fixId", fixId);
                    using (SqlDataReader reader = dBManager.ExecuteReader(command))
                    {
                        return reader.HasRows;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public string UpdateFavClub(string username, string abbr)
        {
            try
            {
                String query = "UPDATE Users SET CLUB_SUPPORTED = @abbr WHERE USERNAME = @name";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@name", username);
                    if(abbr != "-")
                        command.Parameters.AddWithValue("@abbr", abbr);
                    else
                        command.Parameters.AddWithValue("@abbr", DBNull.Value);

                    if (dBManager.ExecuteNonQuery(command) > 0)
                    {
                        return "Favorite Club was Updated Successfully";
                    }
                    else
                    {
                        return "An error has occurred";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public DataTable MostGCByClub(string abbr)
        {
            string proc = StoredProcedures.MostGC;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@abbr", abbr);
            using (SqlDataReader reader = dBManager.ExecuteReader(proc, Parameters))
            {
                DataTable dt = new DataTable();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                    reader.Close();
                    return dt;
                }
                else
                {
                    reader.Close();
                    return dt;
                }
            }
        }

        public DataTable CompUser()
        {
            string query = "SELECT u.username, email, fname, mname, lname, birthdate, gender, country, fantasy_team_name, club_supported, competition_name \r\nFROM Users u left outer join Participates_In p on u.USERNAME=p.USERNAME left outer join Competitions c on c.ID = p.COMPETITION_ID\r\nwhere u.USER_TYPE = 'F'";
            using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
            {
                using (SqlDataReader reader = dBManager.ExecuteReader(command))
                {
                    DataTable dt = new DataTable();
                    if (reader.HasRows)
                    {
                        dt.Load(reader);
                        reader.Close();
                        return dt;
                    }
                    else
                    {
                        reader.Close();
                        return dt;
                    }
                }
            }
        }

		public int GetPoints(string username)
        {
			try
			{
				String query = "SELECT Points from Users where USERNAME = @username";
				using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
				{
					command.Parameters.AddWithValue("@username", username);
					using (SqlDataReader reader = dBManager.ExecuteReader(command))
					{
                        int p = -1;

						while (reader.Read())
                        {
                            p = reader.GetInt32(0);
                            
                        }
                        reader.Close();

                        return p;
                    }
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return -1;
			}
		}

        public DataTable WeeklyPP(int fix)
        {
            string proc = StoredProcedures.WeeklyPP;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@fix", fix);
            using (SqlDataReader reader = dBManager.ExecuteReader(proc, Parameters))
            {
                DataTable dt = new DataTable();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                    reader.Close();
                    return dt;
                }
                else
                {
                    reader.Close();
                    return dt;
                }
            }
        }

        public DataTable PercentComp()
        {
            string proc = StoredProcedures.PercentComp;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            using (SqlDataReader reader = dBManager.ExecuteReader(proc, Parameters))
            {
                DataTable dt = new DataTable();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                    reader.Close();
                    return dt;
                }
                else
                {
                    reader.Close();
                    return dt;
                }
            }
        }

        public DataTable SelectedTimes()
        {
            string proc = StoredProcedures.SelectedTimes;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            using (SqlDataReader reader = dBManager.ExecuteReader(proc, Parameters))
            {
                DataTable dt = new DataTable();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                    reader.Close();
                    return dt;
                }
                else
                {
                    reader.Close();
                    return dt;
                }
            }
        }

        public DataTable PlayersStats()
        {
            string proc = StoredProcedures.PlayersStats;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            using (SqlDataReader reader = dBManager.ExecuteReader(proc, Parameters))
            {
                DataTable dt = new DataTable();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                    reader.Close();
                    return dt;
                }
                else
                {
                    reader.Close();
                    return dt;
                }
            }
        }

        public void TerminateConnection()
        {
        dBManager.CloseConnection();
        }
        
    }
}
