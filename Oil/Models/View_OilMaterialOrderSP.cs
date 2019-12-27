namespace Oil.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class View_OilMaterialOrderSP
    {
        [Key]
        [Column(Order = 0)]
        public Guid Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string No { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid ApplyPersonId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ApplyDate { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool IsDel { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        [StringLength(500)]
        public string StaffName { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(1)]
        public string NeedExec { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(1)]
        public string Discrible { get; set; }
    }
}
