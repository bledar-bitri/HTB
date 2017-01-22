using System;
using System.Data;
using System.IO;
using HTB.Database;
using HTB.v2.intranetx.util;
using HTBExtras.XML;

namespace HTB.v2.intranetx.aktenint.tablet
{
    public partial class ProcessMessageTablet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string messageType = GlobalUtilArea.GetEmptyIfNull(Request.Params[GlobalHtmlParams.MESSAGE_TYPE]).Trim().Replace("\r\n", "");
            try
            {
                if (messageType == GlobalHtmlParams.MESSAGE_ADD_GEGNER_PHONE_NUMBER)
                {
                    if(AddGegnerPhone(GetXmlGegnerPhone(GlobalUtilArea.GetXmlData(Request))))
                        Response.Write("OK");
                    else
                        Response.Write("ERROR: Telefonnummer nicht gespeichert!");
                }
                else if (messageType == GlobalHtmlParams.MESSAGE_DELETE_GEGNER_PHONE_NUMBER)
                {
                    DeleteGegnerPhone(GlobalUtilArea.GetZeroIfConvertToIntError(Request[GlobalHtmlParams.GEGNER_PHONE_ID]));
                }
            }
            catch (Exception ex)
            {
                Response.Write("ERROR: " + ex.Message);
                Response.Write(ex.StackTrace);
            }
        }

        #region GegnerPhone
        private void DeleteGegnerPhone(int gegnerPhoneId)
        {
            var set = new RecordSet();
            set.ExecuteNonQuery("DELETE tblGegnerPhone  WHERE GPhoneID = " + gegnerPhoneId);
        }
        private bool AddGegnerPhone(XmlGegnerPhone gphone)
        {
            var phone = (tblGegnerPhone) HTBUtilities.HTBUtils.GetSqlSingleRecord("SELECT * FROM tblGegnerPhone WHERE GPhoneCountry = '" + gphone.GPhoneCountry +
                                                                                  "' and GPhoneCity = '" + gphone.GPhoneCity +
                                                                                  "' and GPhone = '" + gphone.GPhone + "'"
                                                                                  , typeof (tblGegnerPhone));
            var set = new RecordSet();
            if (phone == null)
            {
                phone = new tblGegnerPhone
                            {
                                GPhoneType = gphone.GPhoneType,
                                GPhoneGegnerID = gphone.GPhoneGegnerID,
                                GPhoneDescription = gphone.GPhoneDescription,
                                GPhoneDate = DateTime.Now,
                                GPhoneCountry = gphone.GPhoneCountry,
                                GPhoneCity = gphone.GPhoneCity,
                                GPhone = gphone.GPhone
                            };
                if (gphone.IsMainPhoneNumber)
                {
                    set.ExecuteNonQuery("UPDATE tblGegner SET GegnerPhoneCountry = '" + phone.GPhoneCountry + "', GegnerPhoneCity = '" + phone.GPhoneCity + "', GegnerPhone = '" + phone.GPhone + "' WHERE GegnerID = " + gphone.GPhoneGegnerID);
                }
                return RecordSet.Insert(phone);
            }
            return true;
        }

        private XmlGegnerPhone GetXmlGegnerPhone(string xmlData)
        {
            var ds = new DataSet();
            ds.ReadXml(new StringReader(xmlData));
            var rec = new XmlGegnerPhone();
            foreach (DataTable tbl in ds.Tables)
            {
                if (tbl.TableName.ToUpper().Trim() == rec.GetType().Name.ToUpper())
                {
                    foreach (DataRow dr in tbl.Rows)
                    {
                        rec.LoadFromDataRow(dr);
                    }
                }
            }
            return rec;
        }
        #endregion
    }
}