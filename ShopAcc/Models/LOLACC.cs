//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShopAcc.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class LOLACC
    {
        public LOLACC()
        {
            this.ACC_SKIN = new HashSet<ACC_SKIN>();
            this.ANHs = new HashSet<ANH>();
        }
    
        public string ID { get; set; }
        public string TEN { get; set; }
        public int GIA { get; set; }
        public string MOTA { get; set; }
        public string RANKID { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public string THANHVIENID { get; set; }
    
        public virtual ICollection<ACC_SKIN> ACC_SKIN { get; set; }
        public virtual RANK RANK { get; set; }
        public virtual ICollection<ANH> ANHs { get; set; }
        public virtual THANHVIEN THANHVIEN { get; set; }
    }
}
