using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace List.Pages.Clients
{
    public class UpdateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];

            try 
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=database;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                { 
                    connection.Open();
                    String query = "SELECT * FROM clients WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader()) 
                        {
                            if (reader.Read()) 
                            {
                                clientInfo.id = "" + reader.GetInt32(0);
                                clientInfo.name = reader.GetString(1);
                                clientInfo.email = reader.GetString(2);
                                clientInfo.phone = reader.GetString(3);
                                clientInfo.address = reader.GetString(4);
                            }
                        }
                    }
                }
            }

            catch (Exception exception) 
            {
                errorMessage = exception.Message;
            }
        }

        public void OnPost() 
        {
            clientInfo.id = Request.Form["id"];
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            if (clientInfo.id.Length == 0 || clientInfo.name.Length == 0 ||
               clientInfo.email.Length == 0 || clientInfo.phone.Length == 0 ||
               clientInfo.address.Length == 0)
            {
                errorMessage = "At least one field is empty. Please fill all the fields.";
                return;
            }

            try
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=database;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String query = "UPDATE clients SET name=@name , email=@email , phone=@phone , address=@address WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);
                        command.Parameters.AddWithValue("@id", clientInfo.id);

                        command.ExecuteNonQuery();
                    }
                }
            }

            catch (Exception exception) 
            {
                errorMessage = exception.Message;
                return;
            }

            Response.Redirect("/Clients/Index");
        }
    }
}
