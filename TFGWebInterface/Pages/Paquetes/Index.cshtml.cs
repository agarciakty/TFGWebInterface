using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace TFGWebInterface.Pages.Paquetes
{
    public class IndexModel : PageModel
    {
        public DataTable datospaqs = new DataTable();
        public void OnGet()
        {


            try
            {
                string connString = "Data Source=.\\SQLEXPRESS;Initial Catalog=tfgdb;Integrated Security=True";

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string sqlstr = "select distinct paquete, count (ARTICULO) as ARTICULOS, (select top (1) parcela from PAQUETES_ENTRADA_ALMACEN where N_PAQUETE = PAQUETE order by FECHA_RECP desc) from PAQUETES_MP group by PAQUETE ORDER BY PAQUETE";
                    using (SqlCommand cmd = new SqlCommand(sqlstr, conn))
                    {
                        using (SqlDataAdapter a = new SqlDataAdapter(cmd))
                        {
                            a.Fill(datospaqs);
                        }
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
