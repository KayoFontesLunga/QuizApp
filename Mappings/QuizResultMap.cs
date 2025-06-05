using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizApp.Models.Quiz;


namespace QuizApp.Mappings
{
    public class QuizResultMap : IEntityTypeConfiguration<QuizResultModel>
    {
        public void Configure(EntityTypeBuilder<QuizResultModel> builder)
        {
            builder.ToTable("QuizResults");

            builder.HasKey(qr => qr.Id);

            builder.Property(qr => qr.Score)
                .IsRequired();

            builder.Property(qr => qr.SubmittedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(qr => qr.User)
                .WithMany(u => u.QuizResults)
                .HasForeignKey(qr => qr.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(qr => qr.Quiz)
                .WithMany(q => q.QuizResults)
                .HasForeignKey(qr => qr.QuizId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
