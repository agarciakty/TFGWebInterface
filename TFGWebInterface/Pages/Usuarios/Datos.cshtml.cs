using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using System.Text;

namespace TFGWebInterface.Pages.Usuarios
{
    public class DatosModel : PageModel
    {

        public Usuario datosUs = new Usuario();
        public DataTable apps = new DataTable();
        public DataTable permisos = new DataTable();

        public bool error = false;
        public bool actualizado = false;
        public string passGenerada = string.Empty;
        public void OnGet()
        {
            string usuario = Request.Query["usuario"];

            try
            {
                string connString = "Data Source=.\\SQLEXPRESS;Initial Catalog=tfgdb;Integrated Security=True";

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    string sqlstr = "SELECT USUARIO, EMAIL FROM USUARIOS_PRIV WHERE USUARIO = '" + usuario + "'";
                    using (SqlCommand cmd = new SqlCommand(sqlstr, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                datosUs.usuario = reader.GetString(0);
                                datosUs.email = reader.GetString(1);

                            }
                        }
                    }

                   /* string strsql2 = "select ID_APP AS ID, NOMBRE_APP AS APLICACIÓN, dpt.nombre as DEPARTAMENTO from APPS app left join rrhh_db.dbo.DEPARTAMENTOS dpt on dpt.id = app.DEPTO";

                    SqlCommand cmd2 = new SqlCommand(strsql2, conn);

                    using (SqlDataAdapter a = new SqlDataAdapter(cmd2))
                    {
                        a.Fill(apps);
                    }

                    /*string strsql3 = "select app from PERMISOS_APPS where USUARIO = '" + datosUs.usuario + "'";

                    SqlCommand cmd3 = new SqlCommand(strsql3, conn);

                    using (SqlDataAdapter a = new SqlDataAdapter(cmd3))
                    {
                        a.Fill(permisos);
                    }*/

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                
            }
        }


        public void OnPost()
        {
            datosUs.usuario = Request.Form["usuario"];
            datosUs.email = Request.Form["email"];

            string[] permisos = Request.Form["permisosCheck"];

            if (datosUs.usuario != null && datosUs.email != null && datosUs.usuario.Length != 0 && datosUs.email.Length != 0)
            {
                try
                {
                    string connString = "Data Source=.\\SQLEXPRESS;Initial Catalog=tfgdb;Integrated Security=True";

                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();

                        string strsql = "DELETE FROM PERMISOS_APPS WHERE usuario = '" + datosUs.usuario + "'";

                        SqlCommand cmd = new SqlCommand(strsql, conn);

                        cmd.ExecuteNonQuery();

                        strsql = "UPDATE USUARIOS_PRIV SET email = '" + datosUs.email + "' where usuario = '" + datosUs.usuario + "'";

                        cmd = new SqlCommand(strsql, conn);

                        cmd.ExecuteNonQuery();

                        foreach (string s in permisos)
                        {
                            string strsql2 = "insert into PERMISOS_APPS (USUARIO, APP) VALUES ('" + datosUs.usuario + "', '" + s + "')";

                            SqlCommand cmd2 = new SqlCommand(strsql2, conn);

                            cmd2.ExecuteNonQuery();
                        }


                        conn.Close();
                    }
                }
                catch (Exception ex)
                {

                }

                actualizado = true;
            }
            else
            {
                error = true;
            }
        }
    }
}
