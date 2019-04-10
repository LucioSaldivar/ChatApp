using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


// We are implementing the Entity Framework ORM using the Code First method. This method involves writing the code defining our models (tables) without any existing database or tables.
//With this method, the database and tables will be created when our application code is executed.
namespace ChatApp.Models
{
    public class ChatContext: DbContext
    {
        public ChatContext() : base("Infinri")
        {

        }
        public static ChatContext Create()
        {
            return new ChatContext();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
    }
}