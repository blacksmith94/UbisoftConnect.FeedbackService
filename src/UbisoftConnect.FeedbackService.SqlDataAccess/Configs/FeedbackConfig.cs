using UbisoftConnect.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UbisoftConnect.SqlDataAccess.Configs
{
	/// <summary>
	/// This class lets Entity Framework map the table design to the Feedback model, all Feedback table schema specifications should be defined here.
	/// </summary>
	public class FeedbackConfig : IEntityTypeConfiguration<Feedback>
	{
		public void Configure(EntityTypeBuilder<Feedback> builder)
		{
			builder.ToTable("feedback");

			builder.Property(f => f.UserId).HasColumnName("userId");
			builder.Property(f => f.SessionId).HasColumnName("sessionId");
			builder.Property(f => f.Rating).HasColumnName("rating");
			builder.Property(f => f.Comment).HasColumnName("comment");
			builder.Property(f => f.Date).HasColumnName("date");

			builder.HasKey(f => new { f.SessionId, f.UserId });

			builder.HasIndex(f => f.Rating);
		}
	}
}
