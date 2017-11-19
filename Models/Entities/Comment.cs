using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace Models.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public DateTimeOffset Time { get; set; }
        public int  OrderId { get; set; }
        public bool Readed { get; set; }


        public User User { get; set; }
        public Order Order { get; set; }
    }
}
