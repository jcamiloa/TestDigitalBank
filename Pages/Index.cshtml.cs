using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using TestDigitalBank.Data;
using TestDigitalBank.Models;
using TabUsuEntity = TestDigitalBank.Models.TabUsu;

namespace TestDigitalBank.Pages
{
    public class IndexModel : PageModel
    {
		private readonly MVCDbContext _context;

		public List<TabUsu> Usuario { get; set; } = new List<TabUsu>();

        public IndexModel(MVCDbContext context)
		{
			_context = context;
		}

		public async void OnGet()
        {
            Usuario = _context.TabUsu.ToList();
        }
    }
}