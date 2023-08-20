using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace TFGWebInterface.Pages.Usuarios
{
    public class IndexModel : PageModel
    {

        public List<Usuario> Usuarios = new List<Usuario>();    
        public void OnGet()
        {
            try
            {
                string connString = "Data Source=.\\SQLEXPRESS;Initial Catalog=tfgdb;Integrated Security=True";

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string sqlstr = "SELECT DISTINCT USUARIO, EMAIL FROM USUARIOS_PRIV";
                    using (SqlCommand cmd = new SqlCommand(sqlstr, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Usuario usr = new Usuario();

                                usr.usuario = reader.GetString(0);
                                usr.email = reader.GetString(1);

                                Usuarios.Add(usr);

                            }
                        }
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            { 
                Console.WriteLine(ex.ToString());
            }

        }
    }

    public class Usuario
    {
        public string usuario;
        public string email;
        public string passCifrada;

    }
}
