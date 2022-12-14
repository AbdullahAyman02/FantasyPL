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
                    command.Parameters.AddWithValue("@FantasyTeamName", userInfo.FantasyTeamName);
                    command.Parameters.AddWithValue("@FavoriteClub", userInfo.FavoriteClub);
                    command.Parameters.AddWithValue("@Balance", userInfo.Balance);
                    command.Parameters.AddWithValue("@Points", userInfo.Points);
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

        public Club SelectClubByName(string Name)
        {
            try {
                String query = "SELECT * FROM Clubs WHERE NAME = @name";
                using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
                {
                    command.Parameters.AddWithValue("@name", Name);
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
                String query = "UPDATE Clubs SET NAME_ABBREVIATION = @abbr, ESTABLISHMENT_YEAR = @year, CITY = @city, NUMBER_OF_TROPHIES = @trophies, OWNER_FNAME = @fname, OWNER_LNAME = @LNAME WHERE NAME = @name";
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
            try
            {
                string query = "DELETE FROM Clubs WHERE NAME = @name";
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
                return null;
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
                String query = "INSERT INTO Players VALUES(@abbr, @number, @fname, @mname, @lname, @price, @age, @height, @weight, @nationality, @year, @contract, @points, @position)";
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

        public string DeleteFTplayer(string username, string club_abbr, int player_no)
        {
            try
            {
                String query = "DELETE FROM FANTASY_TEAM WHERE USERNAME = @username AND CLUB_ABBREVIATION = @abbr AND PLAYER_NO = @number";
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
                return 0;
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
                            GlobalVar.weekFixtures.Add(fixture);
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
                return null;
            }
        }

        public void UpdateFixtureEvents(int id)
        {
            try
            {
                String query = "SELECT * FROM Match_Events WHERE FIXTURE_ID = @id ORDER BY MINUTE ASC";
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
                        int refID = 1;
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
                        if (fixture.Referee == "")
                            fixture.Referee = "-";
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
                    command.Parameters.AddWithValue("@abbr", abbr);
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

        public void UpdateStadiumsList()
        {
            String query = "SELECT * FROM Stadiums";
            using (SqlCommand command = new SqlCommand(query, dBManager.myConnection))
            {
                using (SqlDataReader reader = dBManager.ExecuteReader(command))
                {
                    GlobalVar.listStadiums.Clear();
                    while (reader.Read())
                    {
                        Stadium stadium = new Stadium();
                        stadium.name = reader.GetString(0);
                        stadium.capacity = reader.GetInt32(1);
                        stadium.city = reader.GetString(2);
                        stadium.size = reader.GetInt32(3);
                        stadium.clubAbbreviation = reader.GetString(4);
                        GlobalVar.listStadiums.Add(stadium);
                    }
                    reader.Close();
                }
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
                    Referee referee = new Referee();
                    while (reader.Read())
                    {
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
                        reader.Read();
                        string stadium = reader.GetString(0);
                        reader.Close();
                        return stadium;
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
                    u.Points= reader.GetInt32(1);
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

        public String JoinCompetition(string name, string password)
        {
            var sha = SHA256.Create();
            var byteArr = Encoding.Default.GetBytes(password);
            var hashedPasswordByte = sha.ComputeHash(byteArr);
            string hashedPassword = Convert.ToBase64String(hashedPasswordByte);
            password = hashedPassword;
            string proc = StoredProcedures.JoinCompetition;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@username", GlobalVar.LoggedInUser.Username);
            Parameters.Add("@name", name);
            Parameters.Add("@password", password);
            if (dBManager.ExecuteNonQuery(proc, Parameters) > 0)
            {
                UpdateCompetitionsByUsername(GlobalVar.LoggedInUser.Username);
                return "You joined successfully";
            }
            else
            {
                return "Invalid Competition Details";
            }
        }

        public String ExitCompetition(int id)
        {
            string proc = StoredProcedures.ExitCompetition;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@username", GlobalVar.LoggedInUser.Username);
            Parameters.Add("@id", id);
            if (dBManager.ExecuteNonQuery(proc, Parameters) > 0)
            {
                UpdateCompetitionsByUsername(GlobalVar.LoggedInUser.Username);
                return "You exited successfully";
            }
            else
            {
                return "An error has occurred";
            }
        }

        public String DeleteCompetition(string name)
        {
            string proc = StoredProcedures.DeleteCompetition;
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@name", name);
            if (dBManager.ExecuteNonQuery(proc, Parameters) > 0)
            {
                return "Competition deleted successfully";
            }
            else
            {
                return "This competition does not exit!";
            }
        }

        public void TerminateConnection()
        {
        dBManager.CloseConnection();
        }
    }
}
