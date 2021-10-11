using FluentValidation;
using UbisoftConnect.Configurations;
using UbisoftConnect.FeedbackService.WebAPI.DTOs.UbisoftConnect.WebAPI.DTOs;

namespace UbisoftConnect.WebAPI.Validation
{
	/// <summary>
	/// This class will validate a Feedback Request payload using Fluent Validator, 
	/// </summary>
	public class FeedbackRequestValidator : AbstractValidator<FeedbackRequest>
	{
		private readonly FeedbackServiceConfiguration configuration;
		/// <summary>
		/// Constructor
		/// <param name="configuration"> Injected the service configuration </param>
		/// </summary>
		public FeedbackRequestValidator(FeedbackServiceConfiguration configuration)
		{
			this.configuration = configuration;
			RuleFor(feedbackRequest => feedbackRequest.UserId).NotNull().NotEmpty().WithMessage("User id can't be empty").WithErrorCode("1");
			RuleFor(feedbackRequest => feedbackRequest.FeedbackRequestContent).NotNull().WithMessage("Feedback content can't be null").WithErrorCode("2");
			When(feedbackRequest => feedbackRequest.FeedbackRequestContent != null, () =>
			{
				RuleFor(feedbackRequest => feedbackRequest.FeedbackRequestContent).Must((feedbackRequestContent) => ValidRating(feedbackRequestContent.Rating)).WithMessage($"Rating must be an integer from {configuration.MinRating} to {configuration.MaxRating}").WithErrorCode("3");
			});
		}

		/// <summary>
		/// Validates that rating is below or equal to MinRating and above or equal to MaxRating.
		/// <param name="rating"> Rating to validate </param>
		/// </summary>
		private bool ValidRating(int rating)
		{
			return (rating >= configuration.MinRating && rating <= configuration.MaxRating);
		}
	}
}
