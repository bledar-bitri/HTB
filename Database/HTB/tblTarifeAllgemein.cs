/*
 * Author:			Generated Code
 * Date Created:	07.03.2011
 * Description:		Represents a row in the tblTarifeAllgemein table
*/
using System.Data.SqlClient;
using System.Data;
using System;
namespace HTB.Database
{
	public class tblTarifeAllgemein : Record
	{
		#region Property Declaration
		private int _tarifeAllgemeinID;
		private int _tarifeAllgemeinKlient;
		private double _tarifeAllgemeinPorto;
		private double _tarifeAllgemeinBoni;
		private double _tarifeAllgemeinMEANinner;
		private double _tarifeAllgemeinMEANaus;
		private double _tarifeAllgemeinMEANausl;
		private double _tarifeAllgemeinRatenErin;

        [MappingAttribute(FieldType = MappingAttribute.FIELD_TYPE_ID, FieldAutoNumber = true)]
		public int TarifeAllgemeinID
		{
			get { return _tarifeAllgemeinID; }
			set { _tarifeAllgemeinID = value; }
		}
		public int TarifeAllgemeinKlient
		{
			get { return _tarifeAllgemeinKlient; }
			set { _tarifeAllgemeinKlient = value; }
		}
		public double TarifeAllgemeinPorto
		{
			get { return _tarifeAllgemeinPorto; }
			set { _tarifeAllgemeinPorto = value; }
		}
		public double TarifeAllgemeinBoni
		{
			get { return _tarifeAllgemeinBoni; }
			set { _tarifeAllgemeinBoni = value; }
		}
		public double TarifeAllgemeinMEANinner
		{
			get { return _tarifeAllgemeinMEANinner; }
			set { _tarifeAllgemeinMEANinner = value; }
		}
		public double TarifeAllgemeinMEANaus
		{
			get { return _tarifeAllgemeinMEANaus; }
			set { _tarifeAllgemeinMEANaus = value; }
		}
		public double TarifeAllgemeinMEANausl
		{
			get { return _tarifeAllgemeinMEANausl; }
			set { _tarifeAllgemeinMEANausl = value; }
		}
		public double TarifeAllgemeinRatenErin
		{
			get { return _tarifeAllgemeinRatenErin; }
			set { _tarifeAllgemeinRatenErin = value; }
		}
		#endregion
	}
}
