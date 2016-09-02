using Lia.Blog.Utils.Data;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lia.Blog.Domain.Entity
{
    public class User : IdentityUser,IEntity<string>//, IAggregateRoot<string>
    {
        //public new int Id { get { var intId = 0; int.TryParse(base.Id, out intId); return intId; } set { } }

        public User()
        {
            //Roles = new HashSet<Role>();
            BlogInfos = new HashSet<BlogInfo>();
            Comments = new HashSet<Comment>();
            Id = CombHelper.NewComb().ToString();
            LastModifiedTime = DateTime.Now;
        }

        public void SetCountryByCity(City city)
        {
            switch (city)
            {
                case City.London:
                    Country = Country.UK;
                    break;
                case City.Paris:
                    Country = Country.France;
                    break;
                case City.Chicago:
                    Country = Country.USA;
                    break;
                default:
                    Country = Country.None;
                    break;
            }
        }

        public Country Country { get; set; }

        public City City { get; set; }


        [StringLength(50)]
        public string NickName { get; set; }

        //public bool EmailConfirmed { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime LastModifiedTime { get; set; }

        //public virtual ICollection<Role> Roless { get; set; }

        public virtual ICollection<BlogInfo> BlogInfos { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }

    public enum City
    {
        London,
        Paris,
        Chicago
    }

    public enum Country
    {
        None,UK,France,USA
    }
}
