using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace TFGWebInterface.Pages.Proyectos
{
    public class IndexModel : PageModel
    {
        public DataTable proys = new DataTable();
        public void OnGet()
        {
            try
            {
                string connString = "Data Source=.\\SQLEXPRESS;Initial Catalog=tfgdb;Integrated Security=True";

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string sqlstr = "select ID_PROY, NOMBRE_PROY, convert(varchar, FECHA_INI, 103), CLIENTE, ESTADO from PROYECTOS";

                    SqlCommand cmd = new SqlCommand(sqlstr, conn);

                    using (SqlDataAdapter a = new SqlDataAdapter(cmd))
                    {
                        a.Fill(proys);
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
