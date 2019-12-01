namespace Oil.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SystemResourceModule")]
    public partial class SystemResourceModule
    {
        public Guid Id { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Code { get; set; }

        [StringLength(1000)]
        public string Url { get; set; }

        public int Type { get; set; }

        public Guid? ParentId { get; set; }

        public int? OrderNo { get; set; }
    }
}
