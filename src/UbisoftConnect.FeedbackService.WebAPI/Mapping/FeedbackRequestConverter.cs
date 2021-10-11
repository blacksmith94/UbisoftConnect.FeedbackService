using AutoMapper;
using System;
using UbisoftConnect.Domain.Models;
using UbisoftConnect.FeedbackService.WebAPI.DTOs.UbisoftConnect.WebAPI.DTOs;

namespace UbisoftConnect.FeedbackService.WebAPI.Mapping
{
	public class FeedbackRequestConverter : ITypeConverter<FeedbackRequest, Feedback>
	{
		public Feedback Convert(FeedbackRequest source, Feedback destination, ResolutionContext context)
		{
			return new Feedback()
			{
				UserId = source.UserId,
				SessionId = source.SessionId,
				Rating = source.FeedbackRequestContent.Rating,
				Comment = source.FeedbackRequestContent.Comment,
				Date = DateTime.Now
			};
		}
	}
}
