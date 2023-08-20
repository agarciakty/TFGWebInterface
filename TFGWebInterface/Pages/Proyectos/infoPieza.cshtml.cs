using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;

namespace TFGWebInterface.Pages.Proyectos
{
    public class infoPiezaModel : PageModel
    {
        public DataTable piezas = new DataTable();
        public DataTable oper = new DataTable();
        public DataTable datospieza = new DataTable();
        public string idProy = string.Empty;
        public string idPieza = string.Empty;
        public void OnGet()
        {
            try
            {
                string connString = "Data Source=.\\SQLEXPRESS;Initial Catalog=tfgdb;Integrated Security=True";

                idProy = Request.Query["proyecto"];
                idPieza = Request.Query["pieza"];

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string sqlstr = "select * from PIEZAS where ID_PROYECTO = " + idProy + " AND CODIGO LIKE '" + idPieza + "'";

                    SqlCommand cmd = new SqlCommand(sqlstr, conn);

                    using (SqlDataAdapter a = new SqlDataAdapter(cmd))
                    {
                        a.Fill(datospieza);
                    }

                    sqlstr = "select * from PIEZAS where ID_PROYECTO = " + idProy + " and codigo_padre = '" + idPieza + "' order by CODIGO_PADRE";

                    cmd = new SqlCommand(sqlstr, conn);

                    using (SqlDataAdapter a = new SqlDataAdapter(cmd))
                    {
                        a.Fill(piezas);
                    }

                    if (piezas.Rows.Count == 0)
                    {
                        sqlstr = "select NOMBRE_OPERACION, MP_BASE, CONCAT(NOMBRE, ' ', apellido1, ' ', apellido2), ESTADO, CASE WHEN FECHA_INI IS NULL THEN '-' ELSE convert(varchar, FECHA_INI, 113) END AS INICIO, CASE WHEN FECHA_FAB IS NULL THEN '-' ELSE convert(varchar, FECHA_FAB, 113) END AS FIN, ID from OPERACIONES op left join TIPOS_OPER tope on tope.ID_OPERACION = op.OPERACION left join rrhh_db.dbo.PERSONAL per on per.dni = op.DNI_EMP_ASIGNADO left join ARTICULOS_MP amp on amp.CODIGO = MP_BASE where PIEZA = '" + idPieza + "' AND PROYECTO = '" + idProy + "'";

                        cmd = new SqlCommand(sqlstr, conn);

                        using (SqlDataAdapter a = new SqlDataAdapter(cmd))
                        {
                            a.Fill(oper);
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
}
