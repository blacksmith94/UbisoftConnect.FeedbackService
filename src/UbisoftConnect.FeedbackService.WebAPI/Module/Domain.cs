using Autofac;
using FluentValidation;
using UbisoftConnect.Domain.Services;
using UbisoftConnect.FeedbackService.WebAPI.DTOs.UbisoftConnect.WebAPI.DTOs;
using UbisoftConnect.WebAPI.Validation;

namespace UbisoftConnect.WebAPI.Module
{
	/// <summary>
	/// This class adds the contents of the Domain module into the autofac IoC container.
	/// It registers the domain services and validators.
	/// </summary>
	public class Domain : Autofac.Module
	{
		/// <summary>
		/// Register the domain services and validators
		/// </summary>
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<UbisoftConnect.Domain.Services.FeedbackService>().As<IFeedbackService>();
			builder.RegisterType<FeedbackRequestValidator>().As<IValidator<FeedbackRequest>>();
		}
	}
}
