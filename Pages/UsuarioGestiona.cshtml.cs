using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestDigitalBank.Data;
using TestDigitalBank.Models;
using TabUsuEntity = TestDigitalBank.Models.TabUsu;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Data;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using System.Globalization;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Data.Entity.Core.Mapping;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TestDigitalBank.Pages
{
    public class UsuarioGestionaModel : PageModel
	{
		private string connectionString;
		private readonly MVCDbContext _context;
		private string Parametro = "0";
		private int Id;

        public UsuarioGestionaModel(IConfiguration configuration, MVCDbContext context)
		{
			connectionString = configuration.GetConnectionString("MVCDbContextConnectionString");
            _context = context;
        }
		public void OnGet(string? codigo)
        {
            if(codigo != null){
                
			    string[] subs = codigo.Split('_');
                string SelecSex = "Masculino";

				Parametro = subs[0];
			    Id = Convert.ToInt32(subs[1]);

                if(Parametro == "1")
                {
                    ViewData["breadcrumb"] = "Usuario Gestiona - Adicionar";
				}else if(Parametro == "2")
                {
					ViewData["breadcrumb"] = "Usuario Gestiona - Editar";
				}
				else if (Parametro == "3")
				{
					ViewData["breadcrumb"] = "Usuario Gestiona - Eliminar";
				}

				ViewData["Procedimiento"] = Parametro;

				List<String> ListEstado = new List<String>();
				ListEstado.Add("Masculino");
				ListEstado.Add("Femenino");

				if (Parametro != "1" ) {

                    var Usuario = _context.TabUsu.Where(m => m.Id == Id).ToList();

				    foreach(var a in Usuario)
				    {

                        DateTime dt = DateTime.ParseExact(a.UsuFecNac.ToString().Substring(0, 10), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        string s = dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    
                        ViewData["Id"] = a.Id;
                        ViewData["UsuNom"] = a.UsuNom;
                        ViewData["UsuFecNac"] = s;

                        if(a.UsuSex == "F"){SelecSex = "Femenino";}

					}

			    }

				ViewData["UsuSex"] = new SelectList(ListEstado, null, null, SelecSex);
			}
            else { Redirect("/Index");}

		}

        [HttpPost]
        public IActionResult OnPost()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                using (SqlCommand cmd = new("ProcCrudUsu", conn))
                {
                    int IdUsu = 0;
					int TipoPro = 0;
					int numericValue;

					bool isNumber = int.TryParse(Request.Form["Id"], out numericValue);					
					if (isNumber == true)                    {
						IdUsu = Convert.ToInt32(Request.Form["Id"]);
					}

					isNumber = int.TryParse(Request.Form["TipoPro"], out numericValue);
					if (isNumber == true)
					{
						TipoPro = Convert.ToInt32(Request.Form["TipoPro"]);
					}

					cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = IdUsu;
                    cmd.Parameters.Add("@UsuNom", System.Data.SqlDbType.VarChar).Value = Request.Form["UsuNom"].ToString();
					cmd.Parameters.Add("@UsuFecNac", System.Data.SqlDbType.VarChar).Value = Request.Form["UsuFecNac"].ToString();
                    cmd.Parameters.Add("@UsuSex", System.Data.SqlDbType.VarChar).Value = Request.Form["UsuSex"].ToString().Substring(0, 1); 
                    cmd.Parameters.Add("@TipPro", System.Data.SqlDbType.Int).Value = TipoPro; 
                    cmd.Parameters.Add("@ResMens", System.Data.SqlDbType.VarChar).Value = "";
                    cmd.Parameters.Add("@ResEst", System.Data.SqlDbType.VarChar).Value = "";
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    return Redirect("/Index");

                }
            }
        }

    }
}
