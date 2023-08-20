using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using TFGWebInterface.Pages.Usuarios;

namespace TFGWebInterface.Pages.Paquetes
{
    public class AlmacenesModel : PageModel
    {
        public DataTable listaAlmacenes = new DataTable();
        public void OnGet()
        {
            try
            {
                string connString = "Data Source=.\\SQLEXPRESS;Initial Catalog=tfgdb;Integrated Security=True";

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string sqlstr = "SELECT DISTINCT * FROM ALMACENES";

                    SqlCommand cmd = new SqlCommand(sqlstr, conn);

                    using (SqlDataAdapter a = new SqlDataAdapter(cmd))
                    {
                        a.Fill(listaAlmacenes);
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
}
