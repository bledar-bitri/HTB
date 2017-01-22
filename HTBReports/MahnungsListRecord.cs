using System.Collections;
using HTB.Database;

namespace HTBReports
{
    public class MahnungsListRecord : Record
    {
        private ArrayList _mahynungsList = new ArrayList();
        public ArrayList MahnungsList
        {
            get { return _mahynungsList; }
            set { _mahynungsList = value; }
        }
    }
}
