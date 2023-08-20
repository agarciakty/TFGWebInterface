using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using TFGWebInterface.Pages.Usuarios;

namespace TFGWebInterface.Pages.Stock
{
    public class IndexModel : PageModel
    {
        public List<ArticuloMP> articulos = new List<ArticuloMP>();
        public void OnGet()
        {
            try
            {
                string connString = "Data Source=.\\SQLEXPRESS;Initial Catalog=tfgdb;Integrated Security=True";

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string sqlstr = "select a.ID, CODIGO, DENOMINACION, c.CATEGORIA, p.NOMBRE as PROVEEDOR, STOCK from tfgdb.dbo.ARTICULOS_MP a left join tfgdb.dbo.CATEGORIAS_MP c on a.CATEGORIA = c.ID_CATEGORIA left join tfgdb.dbo.PROVEEDORES p on p.ID = a.PROVEEDOR";
                    using (SqlCommand cmd = new SqlCommand(sqlstr, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ArticuloMP art = new ArticuloMP();


                                art.ID = reader.GetInt32(0).ToString();
                                art.codigo = reader.GetString(1);
                                art.denomi = reader.GetString(2);
                                art.catego = reader.GetString(3);
                                art.proveedor = reader.GetString(4);
                                art.stock = reader.GetString(5);
                                articulos.Add(art);


                            }
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

        public class ArticuloMP
        {
            public string ID;
            public string codigo;
            public string denomi;
            public string catego;
            public string proveedor;
            public string stock;
        }
    }
}
