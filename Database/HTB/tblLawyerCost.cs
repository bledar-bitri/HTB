/*
 * Author:			Generated Code
 * Date Created:	06.07.2014
 * Description:		Represents a row in the tblLawyerCost table
*/
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTB.Database
{
    public class tblLawyerCost
    {

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
        public int LawyerCostId { get; set; }
        public int LawyerCostLawyerId { get; set; }
        public double LawyerCostFrom { get; set; }
        public double LawyerCostTo { get; set; }
        public double LawyerCostPercent { get; set; }
        public double LawyerCostAmount { get; set; }
    }
}
