using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace TFGWebInterface.Pages.Usuarios
{
    public class NuevoModel : PageModel
    {

        public Usuario datosUs = new Usuario();
        public DataTable apps = new DataTable();
        public bool error = false;
        public bool creado = false;
        public string passGenerada = string.Empty;
        public void OnGet()
        {

            
        }
        public static string HashString(string passwordString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(passwordString))
                sb.Append(b.ToString("X3"));
            return sb.ToString();
        }

        public static byte[] GetHash(string passwordString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(passwordString));
        }

        public void OnPost()
        {
            datosUs.usuario = Request.Form["usuario"];
            datosUs.email = Request.Form["email"];

            string[] permisos = Request.Form["permisosCheck"];

            if (datosUs.usuario != null && datosUs.email != null && datosUs.usuario.Length != 0 && datosUs.email.Length != 0 )
            {
                StringBuilder builder = new StringBuilder();
                Random random = new Random();
                char ch;
                for (int i = 0; i < 10; i++)
                {
                    ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                    builder.Append(ch);
                }

                passGenerada = builder.ToString();

                string passSHA = HashString(passGenerada);


                try
                {
                    string connString = "Data Source=.\\SQLEXPRESS;Initial Catalog=tfgdb;Integrated Security=True";

                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();

                        string strsql = "insert into USUARIOS_PRIV (usuario, email, pass, priv) values ('" + datosUs.usuario + "', '" + datosUs.email + "', '" + passSHA + "', '-')";

                        SqlCommand cmd = new SqlCommand(strsql, conn);

                        cmd.ExecuteNonQuery();

                        foreach (string s in permisos)
                        {
                            string strsql2 = "insert into PERMISOS_APPS (USUARIO, APP) VALUES ('" + datosUs.usuario + "', '" + s + "')";

                            SqlCommand cmd2 = new SqlCommand(strsql2, conn);

                            cmd2.ExecuteNonQuery();
                        }
                        

                        conn.Close();
                    }
                }
                catch (Exception ex)
                {

                }

                creado = true;
            }
            else
            {
                error = true;
            }
        }
    }
}
