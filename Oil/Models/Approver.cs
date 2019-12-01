namespace Oil.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Approver")]
    public partial class Approver
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string JobCode { get; set; }

        public int AreaLeve { get; set; }

        [StringLength(100)]
        public string Discrible { get; set; }

        public int Order { get; set; }

        public Guid? ProcessItemId { get; set; }

        [StringLength(100)]
        public string ExecuteMethod { get; set; }
    }
}
