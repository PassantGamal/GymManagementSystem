using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Configurations
{
    internal class MemberSessionConfiguration : IEntityTypeConfiguration<MemberSession>
    {
        public void Configure(EntityTypeBuilder<MemberSession> builder)
        {
           builder.HasKey(X => new { X.MemberId, X.SessionId }); // Composite primary key
            builder.Ignore(X=>X.Id); // Ignore the base Id property
            builder.Property(X => X.CreatedAt)
                .HasColumnName("BookingDate")
                .HasDefaultValueSql("getdate()");

        }
    }
}
