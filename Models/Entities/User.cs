using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Models.Entities;

namespace API.Models
{
    public class User
    {

        [Key]
        public int Id { get; set; }

        [StringLength(maximumLength:255, MinimumLength = 1)]
        [Index(IsUnique = true)]
        public string Email { get; set; }

        [Index]
        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string AuthToken { get; set; }

        [Index]
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string Name { get; set; }

        [Index]
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string Phone { get; set; }

        [ForeignKey("AvatarFile")]
        public int? AvatarFileId { get; set; }
        public virtual File AvatarFile { get; set; }

        public Role Role { get; set; }

        [Required]
        public DateTimeOffset DateRegistered { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        [InverseProperty("OwnerUser")]
        public virtual ICollection<Order> OrdersAsOwner { get; set; }

        [InverseProperty("CustomerUser")]
        public virtual ICollection<Order> OrdersAsCustomer { get; set; }

        
    }

    public enum Role
    {
        PortalAdmin = 0,
        PortalManager = 1,
        RegisteredUser = 2
    }

}