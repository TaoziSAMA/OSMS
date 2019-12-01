namespace Oil.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LoginInfo")]
    public partial class LoginInfo
    {
        public Guid Id { get; set; }

        public Guid StaffId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? LoginTime { get; set; }

        public virtual Staff Staff { get; set; }
    }
}
