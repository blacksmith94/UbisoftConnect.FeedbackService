using System;

namespace UbisoftConnect.Domain.Models
{    /// <summary>
     /// Feedback model object that will be tracked by Entity Framework
     /// </summary>

    public class Feedback
    {
        /// <summary>
        /// User id
        /// </summary>
        /// <example>"UbisoftUser"</example>
        public string UserId { get; set; }

        /// <summary>
        /// Session id
        /// </summary>
        /// <example>"fa1afb52-b989-457c-a46a-0c532282924b"</example>
        public Guid SessionId { get; set; }

        /// <summary>
        /// Session rating, must be a value from 1 to 5;
        /// </summary>
        /// <example>1</example>
        public int Rating { get; set; }

        /// <summary>
        /// Feedback comment, can be null
        /// </summary>
        /// <example>"I had so much fun!"</example>
        public string? Comment { get; set; }

        /// <summary>
        /// Feedback date
        /// </summary>
        /// <example>2021-08-05T12:21:41.9536967+02:00</example>
        public DateTime Date { get; set; }
    }
}