using Moq;
using System;
using UbisoftConnect.Configurations;
using UbisoftConnect.Domain;
using UbisoftConnect.Domain.Models;
using Xunit;

namespace UbisoftConnect.WebAPI.Tests.UnitTesting
{
	public class FeedbackServiceTest
	{
		//Tests that the feedback service is not null
		[Fact]
		public void Test_1_FeedbackService_Should_Not_Be_Null()
		{
			var feedbackRepoMock = new Mock<IRepository<Feedback>>();
			var configMock = new Mock<FeedbackServiceConfiguration>();

			var feedbackService = new Domain.Services.FeedbackService(feedbackRepoMock.Object, configMock.Object);

			Assert.NotNull(feedbackService);
		}

		//Tests that the feedback service looks for a repeated rating correctly
		[Fact]
		public void Test_2_Should_Be_Unique_Session()
		{
			var feedbackRepoMock = new Mock<IRepository<Feedback>>();
			var configMock = new Mock<FeedbackServiceConfiguration>();

			var feedbackService = new Domain.Services.FeedbackService(feedbackRepoMock.Object, configMock.Object);
			var feedback = new Feedback()
			{
				Comment = "",
				Rating = 5,
				SessionId = Guid.NewGuid(),
				UserId = "UbiUser",
				Date = DateTime.Now
			};
			Assert.True(feedbackService.IsUniqueUserAndSession(feedback));
		}
	}
}
