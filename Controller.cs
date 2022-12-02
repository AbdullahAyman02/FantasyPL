using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Xml.Linq;

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
                        club.Postition = pos;
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

        public void TerminateConnection()
        {
            dBManager.CloseConnection();
        }
    }
}
