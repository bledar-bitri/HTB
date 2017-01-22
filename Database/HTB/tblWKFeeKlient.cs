/*
 * Author:			Generated Code
 * Date Created:	14.03.2011
 * Description:		Represents a row in the tblWKFeeKlient table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblWKFeeKlient : Record
	{
		#region Property Declaration
		private int _wKFeeKlient;
		private int _wKFeeKlientReference;
		private double _wKFeeKlientContacterFee;
		private double _wKFeeKlientSalesPromoterFee;
        public int WKFeeKlient
		{
			get { return _wKFeeKlient; }
			set { _wKFeeKlient = value; }
		}
		public int WKFeeKlientReference
		{
			get { return _wKFeeKlientReference; }
			set { _wKFeeKlientReference = value; }
		}
		public double WKFeeKlientContacterFee
		{
			get { return _wKFeeKlientContacterFee; }
			set { _wKFeeKlientContacterFee = value; }
		}
		public double WKFeeKlientSalesPromoterFee
		{
			get { return _wKFeeKlientSalesPromoterFee; }
			set { _wKFeeKlientSalesPromoterFee = value; }
		}
		#endregion
	}
}
