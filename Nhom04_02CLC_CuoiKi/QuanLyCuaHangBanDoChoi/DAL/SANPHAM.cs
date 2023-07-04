namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SANPHAM")]
    public partial class SANPHAM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SANPHAM()
        {
            CTHDs = new HashSet<CTHD>();
        }

        [Key]
        public int MASP { get; set; }

        [StringLength(100)]
        public string TENSP { get; set; }

        [StringLength(128)]
        public string MALOAISP { get; set; }

        public int? SOLUONG { get; set; }

        public double? KHUYENMAI { get; set; }

        public double? DONGIANHAP { get; set; }

        public double? LOINHUAN { get; set; }

        public double? DONGIABAN { get; set; }

        [Column(TypeName = "image")]
        public byte[] HINHANH { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CTHD> CTHDs { get; set; }

        public virtual LOAISANPHAM LOAISANPHAM { get; set; }
    }
}
