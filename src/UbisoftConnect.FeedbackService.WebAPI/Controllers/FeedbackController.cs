using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UbisoftConnect.Domain.Models;
using UbisoftConnect.Domain.Services;
using UbisoftConnect.FeedbackService.WebAPI.DTOs.UbisoftConnect.WebAPI.DTOs;
using UbisoftConnect.WebAPI.DTOs;

namespace UbisoftConnect.WebAPI.Controllers
{

	[Route("api/[Controller]")]
	[ApiController]
	public class FeedbackController : Controller
	{
		private readonly ILogger<FeedbackController> logger;
		private readonly IMapper mapper;
		private readonly IFeedbackService feedbackService;

		/// <summary>
		/// Constructor of the controller
		/// </summary>
		/// <param name="logger">Injected logger used to log request results</param>
		/// <param name="mapper">Injected Automap mapper used for mapping DTOs to models and viceversa</param>
		/// <param name="feedbackService">Injected feedback service, will handle the requests once they are validated</param>
		/// <param name="feedbackRequestModelValidator">Injected feedback request validator, will validate that the request is as desired</param>
		public FeedbackController(ILogger<FeedbackController> logger, IMapper mapper, IFeedbackService feedbackService)
		{
			this.logger = logger;
			this.mapper = mapper;
			this.feedbackService = feedbackService;
		}


		/// <summary>
		/// Gets latest N feedbacks, can be filtered by rating.
		/// </summary>
		/// <param name="rating">Rating filter value, between 1 and 5.</param>
		/// <returns code="200">The list of the latest feedback</returns>
		[HttpGet()]
		[ProducesResponseType(typeof(List<FeedbackResponse>), (int)HttpStatusCode.OK)]
		public ActionResult<List<FeedbackResponse>> GetLatestFeedback(int? rating)
		{
			var model = feedbackService.GetLatest(rating);
			var response = mapper.Map<List<Feedback>, List<FeedbackResponse>>(model);
			logger.LogInformation($"Get latest feedback");

			return Ok(response);
		}

		/// <summary>
		/// Adds a new feedback.
		/// </summary>
		/// <param name="feedbackRequest">Feedback request</param>
		/// <returns code="200">A copy of the created feedback</returns>
		/// <returns code="400">Bad Request</returns>
		[HttpPost("{sessionId}")]
		[ProducesResponseType(typeof(FeedbackResponse), (int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.Conflict)]
		public async Task<ActionResult<FeedbackResponse>> PostFeedback([FromQuery] FeedbackRequest feedbackRequest)
		{
			//Map model
			var model = mapper.Map<FeedbackRequest, Feedback>(feedbackRequest);

			//Add to db
			var registryAdded = await feedbackService.AddAsync(model);
			if (!registryAdded)
				return Conflict("Can't add a feedback twice");

			//Map response
			var response = mapper.Map<Feedback, FeedbackResponse>(model);

			logger.LogInformation($"Added feedback from user '{model.UserId}' with session '{model.SessionId}'");

			return Ok(response);
		}
	}
}