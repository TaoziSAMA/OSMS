namespace Oil.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StaffRole")]
    public partial class StaffRole
    {
        public Guid Id { get; set; }

        public Guid StaffId { get; set; }

        public Guid RoleId { get; set; }
    }
}
