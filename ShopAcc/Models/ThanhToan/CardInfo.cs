using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CardInfo
/// </summary>
public class CardInfo
{

    public string merchant_id { get; set; }
    public string api_user { get; set; }
    public string api_password { get; set; }
    public string pin { get; set; }
    public string seri { get; set; }
    public CardType cardtype { get; set; }
    public int code { get; set; }
    public string msg { get; set; }
    public string info_card { get; set; }
    public string transaction_id { get; set; }
    public string note { get; set; }


    public enum CardType : int
    {
        Viettel = 1,
        Mobiphone = 2,
        Vinaphone = 3,
        Gate = 4,
		VTCVcoin = 5,
		Vietnammoble = 10,
        Zing = 11,
        OnCash = 14
    }

}
