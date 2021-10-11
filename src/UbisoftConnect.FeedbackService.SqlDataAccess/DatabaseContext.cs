using UbisoftConnect.SqlDataAccess.Configs;
using Microsoft.EntityFrameworkCore;

namespace UbisoftConnect.SqlDataAccess
{
	/// <summary>
	/// This class extends the EF DbContext so we can futher configurate the bindings between the models and the DB tables.
	/// All the domain model repositories should be registered here.
	/// </summary>
	public class DatabaseContext : DbContext
	{
		public DatabaseContext() : base() { }

		/// <summary>
		/// Overloaded constructor passing the configuration given by the Startup class.
		/// </summary>
		public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
		{
		}

		/// <summary>
		/// All the domain EF configurations should be applied here.
		/// </summary>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfiguration(new FeedbackConfig());
		}
	}
}
