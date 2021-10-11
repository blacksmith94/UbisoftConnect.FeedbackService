using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UbisoftConnect.Configurations;
using UbisoftConnect.FeedbackService.WebAPI.DTOs.UbisoftConnect.WebAPI.DTOs;
using UbisoftConnect.WebAPI.DTOs;
using Xunit;

namespace UbisoftConnect.WebAPI.Tests.Integration
{
	[Collection("Integration")]

	public class FeedbackServiceTest
	{
		FeedbackServiceConfiguration configuration;
		Request request;
		public FeedbackServiceTest(IntegrationFixture fixture)
		{

			this.request = fixture.Request;
			this.configuration = fixture.Configuration;
		}

		[Fact]
		//The maximum rating configured should be greater or equal to the minimum rating configured
		public void Test_1_Maximum_is_Higher_Than_Minimum()
		{
			Assert.True(configuration.MaxRating >= configuration.MinRating);
		}

		[Theory]
		//Should return a 200 code, valid response
		[InlineData("TestUserId1", "860cf0b0-6a78-4dbb-93b6-5214901e9d84", 1, "Test comment")]
		[InlineData("TestUserId2", "860cf0b0-6a78-4dbb-93b6-5214901e9d84", 5, null)]
		public async Task Test_2_Should_Add_Feedback_Correctly_On_Feedback_Request(string userId, string sessionId, int rating, string comment)
		{
			var feedbackContent = new FeedbackRequestContent() { Rating = rating, Comment = comment };
			var headerDictionary = new HeaderDictionary();
			headerDictionary.Add("Ubi-UserId", userId);
			var feedbackResponse = await request.Post<FeedbackResponse>($"feedback/{sessionId}", feedbackContent, headerDictionary);

			Assert.Equal(userId, feedbackResponse.UserId);
			Assert.Equal(sessionId, feedbackResponse.SessionId.ToString());
			Assert.Equal(feedbackContent.Rating, feedbackResponse.Rating);
			Assert.Equal(feedbackContent.Comment, feedbackResponse.Comment);
		}

		[Theory]
		//No headers request, should return bad request
		[InlineData(1, "Test comment")]
		public async Task Test_3_Should_Return_BadRequest_No_Headers(int rating, string comment)
		{
			var feedbackRequest = new FeedbackRequestContent() { Rating = rating, Comment = comment };
			var feedbackResponse = await request.Post($"feedback/{Guid.NewGuid()}", feedbackRequest);
			Assert.True(feedbackResponse.StatusCode == HttpStatusCode.BadRequest);
		}

		[Theory]
		//Adding the same request twice should return a bad request on the second try
		[InlineData("testUser", "860cf0b0-6a78-4dbb-93b6-5214901e9d84", 1, "Test comment")]
		public async Task Test_4_Should_Return_BadRequest_Repeated_Rating(string userId, string sessionId, int rating, string comment)
		{
			var headerDictionary = new HeaderDictionary();
			headerDictionary.Add("Ubi-UserId", userId);
			var feedbackContent = new FeedbackRequestContent() { Rating = rating, Comment = comment };
			await request.Post($"feedback/{sessionId}", feedbackContent, headerDictionary);
			var feedbackResponse = await request.Post($"feedback/{sessionId}", feedbackContent, headerDictionary);
			Assert.True(feedbackResponse.StatusCode == HttpStatusCode.Conflict);
		}

		[Theory]
		//Should return a bad request if the sessionId is not a correct Guid
		[InlineData("user1", "5214901e9d84", 1, "Test comment")]
		[InlineData("user2", "cw23d23", 1, "Test comment")]
		public async Task Test_5_Should_Return_BadRequest_Invalid_SessionId(string userId, string sessionId, int rating, string comment)
		{

			var feedbackContent = new FeedbackRequestContent() { Rating = rating, Comment = comment };
			var headerDictionary = new HeaderDictionary();
			headerDictionary.Add("Ubi-UserId", userId);
			var feedbackResponse = await request.Post($"feedback/{sessionId}", feedbackContent, headerDictionary);

			Assert.True(feedbackResponse.StatusCode == HttpStatusCode.BadRequest);
		}

		[Theory]
		//The obtained feedbacks should match the specified rating filter
		[InlineData("test1", 1)]
		[InlineData("test2", 2)]
		[InlineData("test3", 3)]
		[InlineData("test4", 4)]
		[InlineData("test5", 5)]
		public async Task Test_6_Rating_Filter_Should_Filter_Correctly(string userId, int ratingFilter)
		{
			var headerDictionary = new HeaderDictionary();
			headerDictionary.Add("Ubi-UserId", userId);
			for (int rating = 1; rating < 5; rating++)
			{
				var feedbackRequest = new FeedbackRequestContent() { Rating = rating };
				await request.Post($"feedback/{Guid.NewGuid()}", feedbackRequest, headerDictionary);
			}

			//Request the feedback list
			var feedbacks = await request.Get<List<FeedbackResponse>>($"feedback?rating={ratingFilter}");
			foreach (var feedback in feedbacks)
			{
				Assert.True(feedback.Rating == ratingFilter);
			}
		}

		[Theory]
		//Should return a BadRequest when the rating is invalid
		[InlineData("testName1", "860cf0b0-6a78-4dbb-93b6-5214901e9d84", -1000, "Test comment")]
		[InlineData("testName2", "860cf0b0-6a78-4dbb-93b6-5214901e9d84", 1000, null)]
		public async Task Test_7_Should_Return_Bad_Request_On_Wrong_Rating(string userId, string sessionId, int rating, string comment)
		{
			if (rating < configuration.MinRating || rating > configuration.MaxRating)
			{
				var feedbackRequest = new FeedbackRequestContent() { Rating = rating, Comment = comment };
				var headerDictionary = new HeaderDictionary();
				headerDictionary.Add("Ubi-UserId", userId);
				var httpResponse = await request.Post($"feedback/{sessionId}", feedbackRequest, headerDictionary);
				Assert.True(httpResponse.StatusCode == HttpStatusCode.BadRequest);
			}
		}

		[Fact]
		//The number of obtained feedbacks should be equal or lower than the maximum configured
		public async Task Test_8_Should_Return_Max_Or_Less_Feedback()
		{
			var getResponse = await request.Get<List<FeedbackResponse>>($"feedback");
			Assert.True(getResponse.Count <= configuration.MaxLatestFeedback);
		}
	}
}
