using UbisoftConnect.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UbisoftConnect.Domain.Services
{
	/// <summary>
	/// Feedback interface, defines the functions that the service will implement.
	/// </summary>
	public interface IFeedbackService
	{
		/// <summary>
		/// Adds a new feedback to the database;
		/// <param name="feedbackToAdd"> Feedback model to be added </param>
		/// </summary>
		Task<bool> AddAsync(Feedback feedbackToAdd);

		/// <summary>
		/// Gets the latest N feedbacks from the table.
		/// </summary>
		/// <param name="filterRating">Filter the feedbacks by rating, can be null</param>
		/// <returns> The list of the latest feedback </returns>
		List<Feedback> GetLatest(int? filterRating);

		/// <summary>
		/// Checks if the feedback already exists
		/// </summary>
		/// <param name="feedbackItem"> Feedback to check </param>
		/// <returns>True or False</returns>
		bool IsUniqueUserAndSession(Feedback feedbackItem);
	}
}
