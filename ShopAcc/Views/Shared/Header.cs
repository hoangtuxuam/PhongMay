using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace ShopAcc.Models
{
    public class Header
    {
        ShopAccEntities1 db = new ShopAccEntities1();
        public List<RANK> lstRANK;
        public List<SKIN> lstSKIN;
        public Header()
        {
            lstRANK = db.RANKs.ToList();
            lstSKIN = db.SKINs.ToList();
        }
    }
}