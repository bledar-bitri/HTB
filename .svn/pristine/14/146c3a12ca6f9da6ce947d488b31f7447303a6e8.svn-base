using System.Collections;
using HTB.Database;

namespace HTBExtras.XML
{
    public class XmlLoginRecord : Record
    {
        public string UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public ArrayList Roads { get; set; }

        public XmlLoginRecord()
        {
            Roads = new ArrayList();
        }
        public XmlLoginRecord(tblUser user)
        {
            Roads = new ArrayList();
            Assign(user);
        }
        public void Assign(tblUser user)
        {
            UserId = user.UserID.ToString();
            UserFirstName = user.UserVorname;
            UserLastName = user.UserNachname;
        }
    }
}
