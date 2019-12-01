namespace Oil.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RoleResourceModule")]
    public partial class RoleResourceModule
    {
        public Guid Id { get; set; }

        public Guid RoleId { get; set; }

        public Guid ResourceModuleId { get; set; }
    }
}
