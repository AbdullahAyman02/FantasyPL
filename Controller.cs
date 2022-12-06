using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Xml.Linq;
using System.Diagnostics.Contracts;
using Microsoft.AspNetCore.Mvc;

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


        public string InsertUser(User userInfo)
        {
            try
            {
                String sql = "INSERT INTO Users " +
                             "VALUES(@Username, @Email, @Password, @FirstName, @MiddleName, @LastName, @Birthdate, @UserType, @Gender, @Country, @FantasyTeamName, @FavoriteClub, @Balance)";

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
                string query = "INSERT INTO Clubs VALUES(@name, @abbr, @year, @city, @trophies, @fname, @lname)";
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
                String query = "UPDATE Players SET FNAME = @fname, MNAME = @mname, LNAME = @lname, PRICE = @price, AGE = @age, HEIGHT = @height, WEIGHT = @weight, NATIONALITY = @nationality, DEBUT_YEAR = @year, CONTRACT_LENGTH = @contract, Points = @points WHERE CLUB_ABBREVIATION = @abbr AND PLAYER_NO = @number";
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

        public void TerminateConnection()
        {
        dBManager.CloseConnection();
        }
    }
}
