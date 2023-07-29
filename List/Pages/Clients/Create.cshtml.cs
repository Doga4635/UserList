using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace List.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost() 
        {
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];


            if (clientInfo.name.Length == 0 || clientInfo.email.Length == 0 || clientInfo.phone.Length == 0 || clientInfo.address.Length == 0)
            {
                errorMessage = "At least one field is empty. Please fill all the fields.";
                return;
            }


            try 
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=database;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection( connectionString )) 
                {
                   connection.Open();
                    String query = "INSERT INTO clients " +
                         "(name, email, phone, address) VALUES " +
                         "(@name, @email, @phone, @address);";

                    using (SqlCommand command = new SqlCommand(query,connection)) 
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);

                        command.ExecuteNonQuery();
                    }
                }

            }
            catch ( Exception exception ) 
            {
                errorMessage = exception.Message;
                return;
            }

            clientInfo.name = ""; clientInfo.email = ""; clientInfo.phone = ""; clientInfo.address = "";
            successMessage = "New client added successfully";

            Response.Redirect("/Clients/Index");

        }
    }
}
