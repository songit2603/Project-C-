namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NHANVIEN")]
    public partial class NHANVIEN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NHANVIEN()
        {
            HOADONs = new HashSet<HOADON>();
        }

        [Key]
        public int MANV { get; set; }

        [StringLength(40)]
        public string HOTEN { get; set; }

        [StringLength(30)]
        public string SDT { get; set; }

        public bool? GIOITINH { get; set; }

        [StringLength(30)]
        public string NGAYSINH { get; set; }

        [Column(TypeName = "image")]
        public byte[] HINHANH { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HOADON> HOADONs { get; set; }

        public virtual TAIKHOAN TAIKHOAN { get; set; }
    }
}
