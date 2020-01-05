namespace Oil.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LeaveOffice")]
    public partial class LeaveOffice
    {
        public Guid Id { get; set; }

        [StringLength(100)]
        public string No { get; set; }

        [StringLength(100)]
        public string StaffName { get; set; }

        public Guid JobId { get; set; }

        [StringLength(1)]
        public string LeaveType { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ApplyDate { get; set; }

        [StringLength(500)]
        public string Reason { get; set; }

        [StringLength(500)]
        public string ExplanationHandover { get; set; }

        public Guid? HandoverSatffId { get; set; }

        public Guid ApplyPersonId { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public bool? IsDel { get; set; }
    }
}
