using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace TFGWebInterface.Pages.Paquetes
{
    public class DatosAlmacenModel : PageModel
    {
        public DataTable parcelas = new DataTable();
        public DataTable paquetes = new DataTable();
        public string idAlmacen = string.Empty;
        public string nombre = string.Empty;
        public void OnGet()
        {
            idAlmacen = Request.Query["almacen"];
            nombre = Request.Query["nombre"];


            try
            {
                string connString = "Data Source=.\\SQLEXPRESS;Initial Catalog=tfgdb;Integrated Security=True";

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string sqlstr = "select NAVE, FILA, COLUMNA , (select distinct count(n_paquete) from PAQUETES_ENTRADA_ALMACEN pea where PARCELA like (concat(pa.NAVE, pa.FILA, pa.COLUMNA) ) and pa.COD_ALMACEN = pea.COD_ALMACEN ) as [MOVIMIENTOS TOTALES] from PARCELAS_ALMACEN pa where COD_ALMACEN = " + idAlmacen;
                    using (SqlCommand cmd = new SqlCommand(sqlstr, conn))
                    {
                        using (SqlDataAdapter a = new SqlDataAdapter(cmd))
                        {
                            a.Fill(parcelas);
                        }
                    }

                    sqlstr = "select * from (select distinct n_paquete,(select top(1) PARCELA from PAQUETES_ENTRADA_ALMACEN pa2 where pa2.N_PAQUETE = pa1.N_PAQUETE order by FECHA_RECP desc) as parcela,(select top(1) COD_ALMACEN from PAQUETES_ENTRADA_ALMACEN pa3 where pa3.N_PAQUETE = pa1.N_PAQUETE order by FECHA_RECP desc) as almacen from PAQUETES_ENTRADA_ALMACEN pa1) todo where almacen = 1 order by N_PAQUETE";
                    using (SqlCommand cmd = new SqlCommand(sqlstr, conn))
                    {
                        using (SqlDataAdapter a = new SqlDataAdapter(cmd))
                        {
                            a.Fill(paquetes);
                        }
                    }

                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void OnPost()
        {

            idAlmacen = Request.Form["id"];
            nombre = Request.Form["nombre"];
            try
            {
                string connString = "Data Source=.\\SQLEXPRESS;Initial Catalog=tfgdb;Integrated Security=True";

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string strsql = "insert into PARCELAS_ALMACEN (COD_ALMACEN, NAVE, FILA, COLUMNA) values ('" + idAlmacen + "', '" + Request.Form["nave"] + "', '" + Request.Form["fila"] + "', '" + Request.Form["columna"] + "')";

                    SqlCommand cmd = new SqlCommand(strsql, conn);

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Response.Redirect("/Paquetes/DatosAlmacen?almacen=" + idAlmacen + "&nombre=" + nombre);
        }
    }
}
