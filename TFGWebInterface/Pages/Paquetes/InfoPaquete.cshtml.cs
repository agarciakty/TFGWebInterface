using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static TFGWebInterface.Pages.Stock.IndexModel;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace TFGWebInterface.Pages.Paquetes
{
    public class InfoPaqueteModel : PageModel
    {
        public string paquete = string.Empty;
        public DataTable datosParcelas = new DataTable();
        public DataTable contenido = new DataTable();
        public void OnGet()
        {
            paquete = Request.Query["paquete"];
            

            try
            {
                string connString = "Data Source=.\\SQLEXPRESS;Initial Catalog=tfgdb;Integrated Security=True";

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string sqlstr = "select parcela, convert(varchar, FECHA_RECP, 113) AS FECHA, case when CONFIRMADO is null then 'EN PROCESO' ELSE CONFIRMADO END AS ESTADO, NOMBRE AS ALMACEN from PAQUETES_ENTRADA_ALMACEN LEFT JOIN ALMACENES ON ALMACENES.COD_ALMACEN = PAQUETES_ENTRADA_ALMACEN.COD_ALMACEN where N_PAQUETE = " + paquete + " order by FECHA_RECP desc";
                    using (SqlCommand cmd = new SqlCommand(sqlstr, conn))
                    {
                        using (SqlDataAdapter a = new SqlDataAdapter(cmd))
                        {
                            a.Fill(datosParcelas);
                        }
                    }

                    sqlstr = "select ARTICULO, CANTIDAD, CANTIDAD_REAL from PAQUETES_MP where PAQUETE = " + paquete;
                    using (SqlCommand cmd = new SqlCommand(sqlstr, conn))
                    {
                        using (SqlDataAdapter a = new SqlDataAdapter(cmd))
                        {
                            a.Fill(contenido);
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
