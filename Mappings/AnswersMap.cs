using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizApp.Models.Answers;

namespace QuizApp.Mappings;

public class AnswersMap : IEntityTypeConfiguration<AnswersModel>
{
    public void Configure(EntityTypeBuilder<AnswersModel> builder)
    {
        builder.ToTable("Answers");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Text)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(a => a.IsCorrect)
            .IsRequired();

        builder.HasOne(a => a.Questions)
            .WithMany(q => q.Answers)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
