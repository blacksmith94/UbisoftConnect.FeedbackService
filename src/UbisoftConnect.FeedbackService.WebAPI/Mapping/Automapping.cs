using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using UbisoftConnect.Domain.Models;
using UbisoftConnect.FeedbackService.WebAPI.DTOs.UbisoftConnect.WebAPI.DTOs;
using UbisoftConnect.FeedbackService.WebAPI.Mapping;
using UbisoftConnect.WebAPI.DTOs;
using UbisoftConnect.WebAPI.Validation;

namespace UbisoftConnect.WebAPI.Mapping
{
	/// <summary>
	/// This class defines the mapping between DTOs and models.
	/// <para/>
	/// Custom mapping can also be added by using .ConvertUsing<CustomConverter>() where CustomConverter would be a class that implements ITypeConverter;
	/// </summary>
	public class Automapping : Profile
	{
		public Automapping()
		{
			CreateMap<FeedbackRequest, Feedback>()
				.ConvertUsing<FeedbackRequestConverter>();

			CreateMap<Feedback, FeedbackResponse>();

		}
	}
}
