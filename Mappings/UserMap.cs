using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuizApp.Models.User;

namespace QuizApp.Mappings
{
    public class UserMap : IEntityTypeConfiguration<UserModel>
    {

        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(u => u.HashPassword)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(u => u.DateCreated)
                   .IsRequired()
                   .HasDefaultValueSql("GETDATE()"); 

            builder.Property(u => u.SexTypes)
                   .IsRequired()
                   .HasConversion<string>();
        }
    }
}
