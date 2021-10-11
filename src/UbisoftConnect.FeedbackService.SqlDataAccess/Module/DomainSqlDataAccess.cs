using Autofac;
using UbisoftConnect.Domain;
using UbisoftConnect.Domain.Models;

namespace UbisoftConnect.SqlDataAccess.Module
{
	/// <summary>
	/// This class adds the contents of the SqlDataAccess module into the autofac IoC container.
	/// It registers the EF Repositories.
	/// </summary>
	public class DomainSqlDataAccess : Autofac.Module
	{

		/// <summary>
		/// Override this function to add registrations to the container.
		/// All the domain model repositories should be registered here.
		/// </summary>
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<EFRepository<Feedback>>().As<IRepository<Feedback>>();
		}
	}
}
