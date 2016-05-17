using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Pattern.Ef6;
using AspNet.Identity.MySQL;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
namespace Template.Data.Models
{
    public partial class UserProfile : Entity
    {
        public UserProfile()
        {
        }
        public int UserProfileID { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public DateTime BirthDate { get; set; }
        public Sex Gender { get; set; }
    }

}
public enum CivilStatusType
{
    Single,
    Married,
    Divorced
}
public enum Sex
{
    [Description("Male")]
    M,
    [Description("Female")]
    F
    //,[Description("T")]
    //T
}