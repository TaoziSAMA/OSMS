namespace Oil.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OilMaterialOrderDetail")]
    public partial class OilMaterialOrderDetail
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        [StringLength(100)]
        public string OilSpec { get; set; }

        public decimal? Volume { get; set; }

        public decimal? Surplus { get; set; }

        public decimal? DayAvg { get; set; }

        public decimal? NeedAmount { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public bool? IsDel { get; set; }
    }
}
