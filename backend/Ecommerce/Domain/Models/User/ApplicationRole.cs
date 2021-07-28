using AspNetCore.Identity.Mongo.Model;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.User
{
    public class ApplicationRole : MongoRole<ObjectId>
    {
        public override ObjectId Id { get; set; }

        public ApplicationRole()
        {
        }

        public ApplicationRole(string roleName)
            : this()
        {
            Name = roleName;
        }
    }
}
