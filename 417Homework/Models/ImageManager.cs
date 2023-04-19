using System.Data.SqlClient;
using System.Net.NetworkInformation;
using static System.Net.Mime.MediaTypeNames;

namespace _417Homework.Models
{
    public class ImageManager
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=RandomDatabase; Integrated Security=true;";

        public int AddImage(Image image)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Images VALUES(@fileName, @password, @views) SELECT SCOPE_IDENTITY()";

            command.Parameters.AddWithValue("@fileName", image.FileName);
            command.Parameters.AddWithValue("@password", image.Password);
            command.Parameters.AddWithValue("@views", image.Views);
            connection.Open();
            return (int)(decimal)command.ExecuteScalar();

        }

        public List<Image> GetAll()
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Images ";
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            List<Image> images = new List<Image>();
            while (reader.Read())
            {
                images.Add(new Image
                {
                    Id = (int)reader["Id"],
                    FileName = (string)reader["FileName"],
                    Password = (string)reader["Password"],
                    Views = (int)reader["Views"]
                });
            }
            return images;
        }

        public Image GetImageForId(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Images " + 
                "WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            Image image = new Image();
            while (reader.Read())
            {

                image.Id = (int)reader["Id"];
                image.FileName = (string)reader["FileName"];
                image.Password = (string)reader["Password"];
                image.Views = (int)reader["Views"];
                return image;

            }
            return null;
        }

        public void IncreaseViews(int id, int views)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = "UPDATE Images " + 
                "SET Views = @views " + 
                "WHERE Id = @id";

            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@views", views);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }

}
