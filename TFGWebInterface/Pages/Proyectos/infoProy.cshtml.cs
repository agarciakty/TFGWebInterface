using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;

namespace TFGWebInterface.Pages.Proyectos
{
    public class infoProyModel : PageModel
    {
        public DataTable piezas = new DataTable();
        public DataTable datosproy = new DataTable();
        public string idProy = string.Empty;
        public void OnGet()
        {
            try
            {
                string connString = "Data Source=.\\SQLEXPRESS;Initial Catalog=tfgdb;Integrated Security=True";

                idProy = Request.Query["id"];

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string sqlstr = "select * from PROYECTOS where ID_PROY = " + idProy;

                    SqlCommand cmd = new SqlCommand(sqlstr, conn);

                    using (SqlDataAdapter a = new SqlDataAdapter(cmd))
                    {
                        a.Fill(datosproy);
                    }

                    sqlstr = "select * from PIEZAS where ID_PROYECTO = " + idProy + " and codigo_padre = '0' order by CODIGO_PADRE";

                    cmd = new SqlCommand(sqlstr, conn);

                    using (SqlDataAdapter a = new SqlDataAdapter(cmd))
                    {
                        a.Fill(piezas);
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
