using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using TFGWebInterface.Pages.Usuarios;

namespace TFGWebInterface.Pages.Paquetes
{
    public class NuevoAlmacenModel : PageModel
    {
        public bool error = false;
        public bool creado = false;
        public void OnGet()
        {
        }
        

        public void OnPost()
        {
            string id = Request.Form["id"];
            string nombre = Request.Form["nombre"];


            if (id != null && nombre != null && id.Length != 0 && nombre.Length != 0)
            {
                try
                {
                    string connString = "Data Source=.\\SQLEXPRESS;Initial Catalog=tfgdb;Integrated Security=True";

                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();

                        string strsql = "insert into ALMACENES (cod_almacen, nombre) values ('" + id + "', '" + nombre + "')";

                        SqlCommand cmd = new SqlCommand(strsql, conn);

                        cmd.ExecuteNonQuery();


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
