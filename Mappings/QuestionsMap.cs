using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizApp.Models.Questions;

namespace QuizApp.Mappings;

public class QuestionsMap : IEntityTypeConfiguration<QuestionsModel>
{
    public void Configure(EntityTypeBuilder<QuestionsModel> builder)
    {
        builder.ToTable("Questions");
        builder.HasKey(q => q.Id);
        builder.Property(q => q.Text)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasOne(q => q.Quiz)
            .WithMany(q => q.Questions)
            .HasForeignKey(q => q.QuizId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
