using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

public class VippayCard
{
    private CardInfo info = new CardInfo();
    public VippayCard()
    {

    }
    public VippayCard(string merchant_id, string api_user, string api_password)
    {
        this.info.merchant_id = merchant_id;
        this.info.api_user = api_user;
        this.info.api_password = api_password;
    }
    public CardInfo cardCharging(string pin, string seri, CardInfo.CardType cardType, string note)
    {
        this.info.pin = pin;
        this.info.seri = seri;
        this.info.cardtype = cardType;
        this.info.note = note;
        if (this.info.merchant_id == "" || this.info.api_user == "" || this.info.api_password == "")
        {
            this.info.code = 1;
            this.info.msg = "Chưa nhập thông tin cần thiết!";
        }
        System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        ASCIIEncoding encoding = new ASCIIEncoding();
        string postData = "";
        postData += "merchant_id=" + this.info.merchant_id;
        postData += "&pin=" + this.info.pin;
        postData += "&seri=" + this.info.seri;
        postData += "&note=" + this.info.note;
        postData += "&card_type=" + (int)this.info.cardtype;
        byte[] data = encoding.GetBytes(postData);
        HttpWebRequest myRequest =
          (HttpWebRequest)WebRequest.Create("https://vippay.vn/api/api/card");
        myRequest.Credentials = new NetworkCredential(this.info.api_user, this.info.api_password);
        myRequest.Method = "POST";
        myRequest.ContentType = "application/x-www-form-urlencoded";
        myRequest.ContentLength = data.Length;
        Stream newStream = myRequest.GetRequestStream();

        newStream.Write(data, 0, data.Length);
        newStream.Close();

        HttpWebResponse response = (HttpWebResponse)myRequest.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string output = reader.ReadToEnd();
        JavaScriptSerializer js = new JavaScriptSerializer();
        CardInfo infoResult = js.Deserialize<CardInfo>(output);
        response.Close();
        this.info.code = infoResult.code;
        this.info.info_card = infoResult.info_card;
        this.info.msg = infoResult.msg;
        this.info.transaction_id = infoResult.transaction_id;
        this.info.note = infoResult.note;
        return this.info;
    }
}