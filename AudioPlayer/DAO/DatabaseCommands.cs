using AudioPlayer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayer.DAO
{
    public class DatabaseCommands : IDAO
    {

        private SqlConnection connection;

        public void UpdateSettingStyle(string style)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE [Setting]" +
                                    "SET Style = @style";

                cmd.Parameters.AddWithValue("@style", style);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {


            }
            finally
            {
                connection.Close();
            }
        }

        public string SelectStyle()
        {
            try
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT Style FROM [Setting]";

                return Convert.ToString(cmd.ExecuteScalar());
                
            }
            catch (Exception e)
            {
                return "dark";
            }
            finally
            {
                connection.Close();
            }
        }

        public void UpdateAudio(Audio audio)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE [Audio]" +
                                    "SET Title = @title, Author = @author, Genre = @genre," +
                                                               "Year = @year, Mark = @mark " +
                                         "Where Id = @id";

                cmd.Parameters.AddWithValue("@title", audio.Name);
                cmd.Parameters.AddWithValue("@author", audio.Author);
                cmd.Parameters.AddWithValue("@genre", audio.Genre);
                cmd.Parameters.AddWithValue("@year", audio.Year);
                cmd.Parameters.AddWithValue("@mark", audio.Mark);
                cmd.Parameters.AddWithValue("@id", audio.Id);

                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {

             
            }
            finally
            {
                connection.Close();
            }
        }

        public void DeleteFromPlayList(Audio audio, PlayList playList)
        {
            try
            {
                connection.Open();

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "DELETE FROM [Audio_PlayList]" +
                                    "WHERE Audio = @audioId AND PlayList = @playListId";

                cmd.Parameters.AddWithValue("@audioId", audio.Id);
                cmd.Parameters.AddWithValue("@playListId", playList.Id);
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        public void DeleteAudio(Audio audio)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "DELETE FROM [Audio]" +
                                    "WHERE Id = @id";

                cmd.Parameters.AddWithValue("@id", audio.Id);
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        private void DeleteAudio(int id)
        {
            try
            {
                //connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "DELETE FROM [Audio]" +
                                    "WHERE Id = @id";

                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {

            }
            finally
            {
                //connection.Close();
            }
        }

        public void DeletePlaylist(PlayList playList)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "DELETE FROM [PlayList]" +
                                    "WHERE Id = @id";

                cmd.Parameters.AddWithValue("@id", playList.Id);
                cmd.ExecuteNonQuery();

            }
            catch(Exception e)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        public int InsertAudio(Audio audio)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO [Audio] (Source, Title, Author, Genre, Year, Mark)" +
                                             "VALUES(@src, @title, @author, @genre, @year, @mark);" +
                                  "SET @id = SCOPE_IDENTITY()";

                SqlParameter idParam = new SqlParameter
                {
                    ParameterName = "@id",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(idParam);

                cmd.Parameters.AddWithValue("@src", audio.Path);
                cmd.Parameters.AddWithValue("@title", audio.Name);
                cmd.Parameters.AddWithValue("@author", audio.Author);
                cmd.Parameters.AddWithValue("@genre", audio.Genre);
                cmd.Parameters.AddWithValue("@year", audio.Year);
                cmd.Parameters.AddWithValue("@mark", audio.Mark);

                cmd.ExecuteNonQuery();
                return (int)idParam.Value;
            }
            catch (Exception e)
            {
                return -1;
            }
            finally
            {
                connection.Close();
            }
            
            
        }

        public void InsertAudioToPlayList(Audio audio, PlayList playList)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO [Audio_PlayList] (Audio, PlayList)" +
                                                            "VALUES(@audio, @playList)";

                cmd.Parameters.AddWithValue("@audio", audio.Id);
                cmd.Parameters.AddWithValue("@playList", playList.Id);

                cmd.ExecuteNonQuery();

            }
            catch(Exception e)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        public int InsertPlayList(PlayList playList)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO [PlayList] (Name)" +
                                             "VALUES(@name)" +
                                  "SET @id = SCOPE_IDENTITY()";

                cmd.Parameters.AddWithValue("@name", playList.Name);

                SqlParameter idParam = new SqlParameter
                {
                    ParameterName = "@id",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(idParam);


                cmd.ExecuteNonQuery();
                return (int)idParam.Value;
            }
            catch(Exception e)
            {
                return -1;
            }
            finally
            {
                connection.Close();
            }
        }

        public ObservableCollection<PlayList> SelectAll()
        {
            ObservableCollection<PlayList> playLists = new ObservableCollection<PlayList>();
            playLists.Add(new PlayList("Default Play List"));
            List<int> idForDelete = new List<int>();

            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            try
            {

                cmd.CommandText = "SELECT * FROM [Audio]";
                SqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {

                    if (File.Exists(Convert.ToString(reader["Source"])))
                    {
                        playLists[0].Audios.Add(new Audio(Convert.ToString(reader["Author"]),
                                                      Convert.ToString(reader["Title"]),
                                                      Convert.ToString(reader["Genre"]),
                                                      Convert.ToString(reader["Source"]),
                                                      Convert.ToString(reader["Year"]),
                                                      Convert.ToInt32(reader["Mark"]))
                                                    { Id = Convert.ToInt32(reader["Id"]) });
                    }
                    else
                    {
                        idForDelete.Add(Convert.ToInt32(reader["Id"]));
                    }

                }
                reader.Close();

                foreach(var id in idForDelete)
                {
                    DeleteAudio(id);
                }


                cmd.CommandText = "SELECT * FROM [PlayList]";
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    playLists.Add(new PlayList(Convert.ToString(reader["Name"])) { Id = Convert.ToInt32(reader["Id"]) });
                }
                reader.Close();

                cmd.CommandText = "SELECT * FROM [Audio_PlayList]";
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var audioId = SearchAudioByID(Convert.ToInt32(reader["Audio"]), playLists[0].Audios);
                    var playListId = SearchPlayListByID(Convert.ToInt32(reader["PlayList"]), playLists);
                    if (audioId != -1 && playListId != -1)
                        playLists[playListId].Audios.Add(playLists[0].Audios[audioId]);
                }
                reader.Close();
                return playLists;
            }
            catch(Exception e)
            {
                return playLists;
            }
            finally
            {
                connection.Close();
            }
        }

        public DatabaseCommands()
        {
            connection = DatabaseConnection.GetConnection();
        }

        private int SearchAudioByID(int id, ObservableCollection<Audio> audios)
        {
            foreach(var item in audios)
            {
                if (item.Id.Equals(id))
                    return audios.IndexOf(item);
            }
            return -1;
        }

        private int SearchPlayListByID(int id, ObservableCollection<PlayList> playLists)
        {
            foreach (var item in playLists)
            {
                if (item.Id.Equals(id))
                    return playLists.IndexOf(item);
            }
            return -1;
        }

    }
}
