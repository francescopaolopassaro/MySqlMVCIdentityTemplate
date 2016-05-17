using Template.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Data.Mapping
{
    public class UserProfileMap : EntityTypeConfiguration<UserProfile>
    {
        public UserProfileMap()
        {
            // Primary Key
            this.HasKey(t => t.UserProfileID);

            this.Property(t => t.Surname)
              .HasMaxLength(70); //Global len.standards

            this.Property(t => t.Name)
            .HasMaxLength(70); //Global len.standards

            this.Property(t => t.UserId)
              .IsRequired()
              .HasMaxLength(128);
         
          
        }
    }
}
