namespace UbisoftConnect.Configurations
{
	/// <summary>
	/// Ubisoft FeedbackService configuration
	/// </summary>
	public class FeedbackServiceConfiguration
	{
		/// <summary>
		/// Database connection string
		/// </summary>
		/// <example>"Data Source=ubisoft_connect.db"</example>
		public string DatabaseConnection { get; set; }

		/// <summary>
		/// The maximum number of feedbacks to get when requesting the latest feedbacks
		/// </summary>
		/// <example>15</example>
		public int MaxLatestFeedback { get; set; }

		/// <summary>
		/// The minimum rating of a feedback
		/// </summary>
		/// <example>1</example>
		public int MinRating { get; set; }

		/// <summary>
		/// The maximum rating of a feedback
		/// </summary>
		/// <example>5</example>
		public int MaxRating { get; set; }
	}
}
