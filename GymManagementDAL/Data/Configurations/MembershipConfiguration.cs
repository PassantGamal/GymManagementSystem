using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.SqlServer.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Configurations
{
    internal class MembershipConfiguration : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> builder)
        {
            builder.Property(X => X.CreatedAt)
                .HasColumnName("StartDate")
                .HasDefaultValueSql("GETDATE()");
            builder.HasKey(X => new { X.MemberId, X.PlanId }); // Composite primary key
            builder.Ignore(X => X.Id); // Ignore the Id property since we are using a composite key

            builder.HasOne(X => X.Member)
                .WithMany(X => X.Memberships)
                .HasForeignKey(X => X.MemberId);
            builder.HasOne(X => X.Plan)
                .WithMany(X => X.PlanMembers)
                .HasForeignKey(X => X.PlanId);

        }
    }
}
