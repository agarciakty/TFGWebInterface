using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using System.Text;
using static TFGWebInterface.Pages.Stock.IndexModel;

namespace TFGWebInterface.Pages.Stock
{
    public class DatosModel : PageModel
    {
        public ArticuloMP datosArt = new ArticuloMP();
        public DataTable datosparcelas = new DataTable();

        public void OnGet()
        {
            string id = Request.Query["id"];

            try
            {
                string connString = "Data Source=.\\SQLEXPRESS;Initial Catalog=tfgdb;Integrated Security=True";

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string sqlstr = "select a.ID, CODIGO, DENOMINACION, c.CATEGORIA, p.NOMBRE as PROVEEDOR, STOCK from tfgdb.dbo.ARTICULOS_MP a left join tfgdb.dbo.CATEGORIAS_MP c on a.CATEGORIA = c.ID_CATEGORIA left join tfgdb.dbo.PROVEEDORES p on p.ID = a.PROVEEDOR where a.ID = " + id;
                    using (SqlCommand cmd = new SqlCommand(sqlstr, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ArticuloMP art = new ArticuloMP();


                                datosArt.ID = reader.GetInt32(0).ToString();
                                datosArt.codigo = reader.GetString(1);
                                datosArt.denomi = reader.GetString(2);
                                datosArt.catego = reader.GetString(3);
                                datosArt.proveedor = reader.GetString(4);
                                datosArt.stock = reader.GetString(5);


                            }
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
