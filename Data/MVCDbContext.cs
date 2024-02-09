using Microsoft.EntityFrameworkCore;
using TestDigitalBank.Models;

namespace TestDigitalBank.Data
{
	public class MVCDbContext: DbContext
	{
		public MVCDbContext(DbContextOptions<MVCDbContext> options) : base(options)
		{
		}
		public DbSet<TabUsu> TabUsu { get; set; }
		
	}
}
