using UbisoftConnect.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UbisoftConnect.Configurations;
using System;

namespace UbisoftConnect.Domain.Services
{
	/// <summary>
	/// This class will be registered as a service and will handle the domain's Feedback logic
	/// </summary>
	public class FeedbackService : IFeedbackService
	{
		private readonly IRepository<Feedback> repository;
		private readonly FeedbackServiceConfiguration configuration;

		/// <summary>
		/// FeedbackService constructor.
		/// </summary>
		/// <param name="repository">Injected the repository for the Feedback table </param>
		/// <param name="configuration">Injected service configuration registered at startup</param>
		public FeedbackService(IRepository<Feedback> repository,
							   FeedbackServiceConfiguration configuration)
		{
			this.repository = repository;
			this.configuration = configuration;
		}

		/// <summary>
		/// Task that stores feedback in DB.
		/// </summary>
		/// <param name="feedbackToAdd">Feedback model to add.</param>
		/// <returns>A Task with with a bool indicating if it was added or not</returns>

		public async Task<bool> AddAsync(Feedback feedbackToAdd)
		{
			bool result = false;

			if (IsUniqueUserAndSession(feedbackToAdd))
			{
				await repository.Add(feedbackToAdd);
				await repository.Save();
				result = true;
			}
			return await Task.FromResult(result);
		}

		/// <summary>
		/// Gets the latest N feedbacks from the table.
		/// </summary>
		/// <param name="filterRating">Filter for the rating, can be null.</param>
		/// <returns>A Task with list of feedback models</returns>
		public List<Feedback> GetLatest(int? filterRating)
		{
			var query = this.repository.Query;
			if (filterRating != null)
			{
				query = query.Where(f => f.Rating == filterRating);
			}
			return query.Skip(Math.Max(0, query.Count() - configuration.MaxLatestFeedback)).ToList();
		}

		/// <summary>
		/// Checks if the feedback already exists
		/// </summary>
		/// <param name="feedbackItem"> Feedback to check </param>
		/// <returns>True or False</returns>
		public bool IsUniqueUserAndSession(Feedback feedbackItem)
		{
			var query = this.repository.Query;

			var ratingCount = query.Where(feedback => feedback.SessionId == feedbackItem.SessionId)
			.Where(feedback => feedback.UserId == feedbackItem.UserId)
			.Count();
			if (ratingCount > 0)
			{
				return false;
			}

			return true;
		}
	}
}