using Microsoft.AspNetCore.Mvc;
using System;

namespace UbisoftConnect.FeedbackService.WebAPI.DTOs
{
	namespace UbisoftConnect.WebAPI.DTOs
	{
		/// <summary>
		/// Feedback DTO that arrives to the controller when adding a new feedback, will be mapped to a Feedback model.
		/// FeedbackRequest --> Feedback
		/// </summary>
		public class FeedbackRequest
		{
			[FromRoute(Name = "sessionId")]
			public Guid SessionId { get; set; }

			[FromHeader(Name = "Ubi-UserId")]
			public string UserId { get; set; }

			[FromBody]
			public FeedbackRequestContent FeedbackRequestContent { get; set; }
		}
		public class FeedbackRequestContent
		{
			public int Rating { get; set; }

			public string? Comment { get; set; }
		}
	}
}
