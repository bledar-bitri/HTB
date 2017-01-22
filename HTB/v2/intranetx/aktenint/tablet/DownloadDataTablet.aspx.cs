﻿using System;
using System.Collections;
using HTB.Database;
using HTB.v2.intranetx.util;
using HTBExtras;
using HTBExtras.XML;
using HTBUtilities;

namespace HTB.v2.intranetx.aktenint.tablet
{
    public partial class DownloadDataTablet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string downloadType = GlobalUtilArea.GetEmptyIfNull(Request.Params[GlobalHtmlParams.DOWNLOAD_TYPE]);
            try
            {
                if (downloadType == GlobalHtmlParams.DOWNLOAD_PHONE_TYPES)
                {
                    SendPhoneTypes();
                }
                else if (downloadType == GlobalHtmlParams.DOWNLOAD_AKT_TYPES)
                {
                    SendAktTypes();
                }
                else if (downloadType == GlobalHtmlParams.DOWNLOAD_ACTION_TYPES)
                {
                    SendActionTypes();
                }
                else if (downloadType == GlobalHtmlParams.DOWNLOAD_FIELD_PERSONS)
                {
                    SendFieldPersons();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                Response.Write(ex.StackTrace);
            }
        }

        private void SendPhoneTypes()
        {
            ArrayList list = HTBUtils.GetSqlRecords("SELECT * FROM tblPhoneType Order By PhoneTypeSequence", typeof(XmlPhoneType));
            foreach (XmlPhoneType rec in list)
            {
//                Response.Write(rec.ToXmlString(counter++ == 0));
                Response.Write(rec.ToXmlString());
            }
        }

        private void SendAktTypes()
        {
//            ArrayList list = HTBUtils.GetSqlRecords("SELECT AktTypeIntId, AktTypeIntCaption FROM tblAktTypeInt WHERE AktTypeINTCaption NOT LIKE '%Auto%' Order By AktTypeIntCaption ", typeof(XmlAktTypeIntRecord));
            ArrayList list = HTBUtils.GetSqlRecords("SELECT AktTypeIntId, AktTypeIntCaption FROM tblAktTypeInt Order By AktTypeIntCaption ", typeof(XmlAktTypeIntRecord));
            list.Insert(0, new XmlAktTypeIntRecord
                               {
                                   AktTypeIntID = 0,
                                   AktTypeIntCaption = "Alle"
                               });
            foreach (XmlAktTypeIntRecord rec in list)
            {
//                Response.Write(rec.ToXmlString(counter++ == 0));
                Response.Write(rec.ToXmlString());
            }
        }

        private void SendActionTypes()
        {
            ArrayList list = HTBUtils.GetSqlRecords("SELECT * FROM tblAktenIntActionType ", typeof(tblAktenIntActionType));
            foreach (tblAktenIntActionType rec in list)
            {
                Response.Write(rec.ToXmlString());
            }
        }

        private void SendFieldPersons()
        {
            ArrayList list = HTBUtils.GetSqlRecords("SELECT UserID, UserVorname, UserNachname FROM tbluser WHERE UserStatus = 1", typeof(XmlFieldPerson));
            foreach (XmlFieldPerson rec in list)
            {
                Response.Write(rec.ToXmlString());
            }
        }

    }
}