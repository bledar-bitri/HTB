﻿
using System.Data;
using System;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace HTB.Database
{
    public class DbUtil
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static CultureInfo Provider = CultureInfo.InvariantCulture;
        
        private static string DateFormat = "dd.MM.yyyy hh:mm:ss";

        #region Database Utilities functions

        public static void NormalizeDataRow(DataRow row, DataTable dtable)
        {
            foreach (DataColumn dc in dtable.Columns)
            {
                if (row[dc.ColumnName].GetType() == typeof(DBNull))
                {
                    if (dc.DataType == Type.GetType("System.String"))
                        row[dc.ColumnName] = "";
                    else if (dc.DataType == Type.GetType("System.Decimal"))
                        row[dc.ColumnName] = 0;
                    else if (dc.DataType == Type.GetType("System.Boolean"))
                        row[dc.ColumnName] = false;
                }
            }
        }

        public static string FixString(object value)
        {
            string Result = string.Empty;

            try
            {
                if (value != null)
                {
                    Result = value.ToString().Trim();
                }
                else
                {
                    Result = string.Empty;
                }
            }
            catch
            {
                Result = string.Empty;
            }

            return Result;
        }

        public static int FixInt(object value)
        {
            int Result = 0;
            try
            {
                if (value != null)
                {
                    Result = int.Parse(value.ToString());
                }
                else
                {
                    Result = 0;
                }
            }
            catch
            {
                Result = 0;
            }
            return Result;
        }

        public static int FixBooleanToInt(object value)
        {
            int Result = 0;

            try
            {
                if (FixBoolean(value) == false)
                {
                    Result = 0;
                }
                else
                {
                    Result = 1;
                }
            }
            catch
            {
                Result = 0;
            }
            return Result;
        }

        public static bool FixBoolean(object value)
        {
            bool Result = false;

            try
            {
                if (value != null)
                {
                    string temp = value.ToString().ToUpper();
                    switch (temp)
                    {
                        case "TRUE":
                            {
                                Result = true;
                                break;
                            }
                        case "FALSE":
                            {
                                Result = false;
                                break;
                            }
                        case "0":
                            {
                                Result = false;
                                break;
                            }
                        case "1":
                            {
                                Result = true;
                                break;
                            }
                        case "T":
                            {
                                Result = true;
                                break;
                            }
                        case "F":
                            {
                                Result = false;
                                break;
                            }
                        case "YES":
                            {
                                Result = true;
                                break;
                            }
                        case "Y":
                            {
                                Result = true;
                                break;
                            }
                        case "NO":
                            {
                                Result = false;
                                break;
                            }
                        case "N":
                            {
                                Result = false;
                                break;
                            }
                        default:
                            {
                                Result = false;
                                break;
                            }
                    }
                }
                else
                {
                    Result = false;
                }
            }
            catch
            {
                Result = false;
            }
            return Result;
        }

        public static int GetBoolValueForDB(object value)
        {
            int Result = 0;

            try
            {
                if (value != null)
                {
                    string temp = value.ToString().ToUpper();
                    switch (temp)
                    {
                        case "TRUE":
                        case "1":
                        case "T":
                        case "YES":
                        case "Y":
                            Result = 1;
                            break;
                        default:
                            Result = 0;
                            break;
                    }
                }
                else
                {
                    Result = 0;
                }
            }
            catch
            {
                Result = 0;
            }
            return Result;
        }

        public static float FixFloat(object value)
        {
            float Result = 0;

            try
            {
                if (value != null)
                {
                    string str = value.ToString();
                    if (str.IndexOf(",") > 0 && str.IndexOf(",") < str.IndexOf("."))
                    {
                        str = str.Replace(",", "");
                        str = str.Replace(".", ",");
                    }
                    Result = float.Parse(str);
                }
                else
                {
                    Result = 0;
                }
            }
            catch
            {
                Result = 0;
            }
            return Result;
        }

        public static double FixDouble(object value)
        {
            double result;

            try
            {
                if (value != null)
                {
                    
                    var str = value.ToString().Replace(" ", "");
                    result = double.Parse(str, GetStringCulture(str));

                    //Log.DebugFormat("Parsed Double from: {0} to {1}", value, result);
                }
                else
                {
                    result = 0;
                }
            }
            catch
            {
                result = 0;
            }

            return result;
        }


        
        public static long FixLong(object value)
        {
            long result;
            if (value is byte[])
            {
                try
                {
                    result = BitConverter.ToInt64((byte[])value, 0);
                }
                catch
                {
                    result = 0;
                }
            }
            else
            {
                try
                {
                    result = value != null ? long.Parse(value.ToString()) : 0;
                }
                catch
                {
                    result = 0;
                }
            }
            return result;
        }
        public static DateTime FixDate(object value)
        {
            DateTime Result = new DateTime();

            try
            {
                if (value != null)
                {
                    DateTime temp = (DateTime)value;
                    if (temp.Day == 1 && temp.Month == 1 && temp.Year == 1)
                    {
                        Result = new DateTime(1900, 1, 1, 0, 0, 0, System.Globalization.Calendar.CurrentEra, System.DateTimeKind.Local);
                    }
                    else
                    {
                        Result = (DateTime)value;
                    }
                }
                else
                {
                    Result = new DateTime(1900, 1, 1, 0, 0, 0, System.Globalization.Calendar.CurrentEra, System.DateTimeKind.Local);
                }
            }
            catch
            {
                //Result = new DateTime(1900, 1, 1, 0, 0, 0, System.Globalization.Calendar.CurrentEra, System.DateTimeKind.Local);
                try
                {
                    

                    DateTime temp = DateTime.Parse(value.ToString());
                    if (temp.Day == 1 && temp.Month == 1 && temp.Year == 1)
                    {
                        Result = new DateTime(1900, 1, 1, 0, 0, 0, System.Globalization.Calendar.CurrentEra, System.DateTimeKind.Local);
                    }
                    else
                    {
                        Result = temp;
                    }
                }
                catch
                {
                    try
                    {
                        DateTime temp = DateTime.Parse(value.ToString(), new CultureInfo("DE-de"));
                        if (temp.Day == 1 && temp.Month == 1 && temp.Year == 1)
                        {
                            Result = new DateTime(1900, 1, 1, 0, 0, 0, System.Globalization.Calendar.CurrentEra,
                                System.DateTimeKind.Local);
                        }
                        else
                        {
                            Result = temp;
                        }
                    }
                    catch
                    {
                        Result = new DateTime(1900, 1, 1, 0, 0, 0, System.Globalization.Calendar.CurrentEra,
                            System.DateTimeKind.Local);
                    }
                }
            
            }

            return Result;
        }

        public static DateTime FixDateForZeroTime(object value)
        {
            DateTime Result = new DateTime();

            try
            {
                if (value != null)
                {
                    DateTime temp = (DateTime)value;
                    if (temp.Day == 1 && temp.Month == 1 && temp.Year == 1)
                    {
                        Result = new DateTime(1900, 1, 1, 0, 0, 0, System.Globalization.Calendar.CurrentEra, System.DateTimeKind.Local);
                    }
                    else
                    {
                        Result = ((DateTime)value).Date;
                    }
                }
                else
                {
                    Result = new DateTime(1900, 1, 1, 0, 0, 0, System.Globalization.Calendar.CurrentEra, System.DateTimeKind.Local);
                }
            }
            catch
            {
                Result = new DateTime(1900, 1, 1, 0, 0, 0, System.Globalization.Calendar.CurrentEra, System.DateTimeKind.Local);
            }

            return Result;
        }

        public static string DisplayDate(object value)
        {
            string temp;

            if (((DateTime)value).ToShortDateString() == "1/1/1900")
            {
                temp = string.Empty;
            }
            else
            {
                temp = ((DateTime)value).ToShortDateString();
            }

            return temp;
        }
        
        public static DateTime FixDate(string value)
        {
            DateTime Result = new DateTime();

            try
            {
                if (value != null)
                {
                    DateTime temp = DateTime.Parse(value);
                    if (temp.Day == 1 && temp.Month == 1 && temp.Year == 1)
                    {
                        Result = new DateTime(1900, 1, 1, 0, 0, 0, System.Globalization.Calendar.CurrentEra, System.DateTimeKind.Local);
                    }
                    else
                    {
                        Result = DateTime.Parse(value);
                    }
                }
                else
                {
                    Result = new DateTime(1900, 1, 1, 0, 0, 0, System.Globalization.Calendar.CurrentEra, System.DateTimeKind.Local);
                }
            }
            catch
            {
                Result = new DateTime(1900, 1, 1, 0, 0, 0, System.Globalization.Calendar.CurrentEra, System.DateTimeKind.Local);
            }

            return Result;
        }

        public static decimal FixDecimal(object value)
        {
            decimal Result = 0;

            string strValue = value.ToString().Replace("(", "-").Replace("$", "").Replace(")", "");
            if (strValue.IndexOf(",") > 0 && strValue.IndexOf(",") < strValue.IndexOf("."))
            {
                strValue = strValue.Replace(",", "");
                strValue = strValue.Replace(".", ",");
            }

            try
            {
                Result = decimal.Parse(strValue);
            }
            catch
            {
                Result = 0;
            }

            return Math.Round(Result, 4);
        }

        public static string FormatPhoneNumberForSave(string PNum)
        {
            string temp = string.Empty;
            temp = PNum;
            if (PNum.Length == 14)
            {
                temp = PNum.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
            }
            return temp;
        }

        public static string FormatPhoneNumberForDisplay(string PNum)
        {
            string temp = string.Empty;
            if (PNum.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Length == 10)
            {
                temp = "(" + PNum.Substring(0, 3) + ") ";
                temp += PNum.Substring(3, 3) + "-" + PNum.Substring(6, 4);
            }
            else
            {
                if (PNum.Trim().Length != 0)
                {
                    temp = "Invalid Phone #";
                }
            }
            return temp;
        }

        public static string PadIntWithPound(int ID, int Length)
        {
            StringBuilder sbTemp = new StringBuilder();
            sbTemp.Append(ID.ToString());

            for (int i = sbTemp.Length; i < Length; i++)
            {
                sbTemp.Insert(0, "#");
            }

            return sbTemp.ToString();
        }

        public static string PadInt(int PK_ID, int Length)
        {
            StringBuilder sbTemp = new StringBuilder();
            sbTemp.Append(PK_ID.ToString());

            for (int i = sbTemp.Length; i < Length; i++)
            {
                sbTemp.Insert(0, "0");
            }

            return sbTemp.ToString();
        }

        public static string GetDbValue(double val)
        {
            return val.ToString().Replace(",", ".");
        }


        private static CultureInfo GetStringCulture(string str)
        {
            if (str.IndexOf(",", StringComparison.Ordinal) > 0 &&
                 (str.IndexOf(".", StringComparison.Ordinal) < 0 ||
                 (str.IndexOf(".", StringComparison.Ordinal) > 0 && str.IndexOf(".", StringComparison.Ordinal) < str.IndexOf(",", StringComparison.Ordinal)))
                )
                return new CultureInfo("de-DE");
            else
                return CultureInfo.InvariantCulture;

        }
        #endregion
    }
}