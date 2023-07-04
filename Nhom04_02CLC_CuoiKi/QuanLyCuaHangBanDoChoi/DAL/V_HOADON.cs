namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class V_HOADON
    {
        [Key]
        public int maview { get; set; }
        public int SOHD { get; set; }
        [StringLength(100)]
        public string TENSP { get; set; }
        [StringLength(100)]
        public string DONGIABAN { get; set; }

        public int? SOLUONG { get; set; }
        [StringLength(100)]
        public string THANHTIEN { get; set; }

        public DateTime? NGAYLAP { get; set; }
        [StringLength(100)]
        public string TIENKHACHHANGTRA { get; set; }
        [StringLength(100)]
        public string TIENTHUA { get; set; }
        [StringLength(100)]
        public string TONGHOADON { get; set; }

        public double? KHUYENMAI { get; set; }

        [StringLength(40)]
        public string TENKH { get; set; }

        [StringLength(40)]
        public string TENNV { get; set; }
    }
}
