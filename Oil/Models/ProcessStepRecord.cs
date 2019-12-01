namespace Oil.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProcessStepRecord")]
    public partial class ProcessStepRecord
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        [StringLength(500)]
        public string RecordRemarks { get; set; }

        public int StepOrder { get; set; }

        public Guid WaitForExecutionStaffId { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public bool WhetherToExecute { get; set; }

        [Required]
        [StringLength(50)]
        public string No { get; set; }

        public Guid RefOrderId { get; set; }

        public bool Result { get; set; }

        [StringLength(4000)]
        public string ExecuteMethod { get; set; }

        [StringLength(400)]
        public string Discrible { get; set; }
    }
}
