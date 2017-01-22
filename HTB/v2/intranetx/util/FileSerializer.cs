using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace HTB.v2.intranetx.util
{
    public static class FileSerializer<T> where T : class, ISerializable
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public static void Serialize(string filename, T objectToSerialize)
        {
            Stream stream = File.Open(filename, FileMode.OpenOrCreate);
            var bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, objectToSerialize);
            stream.Close();
            stream.Dispose();
        }

        public static T DeSerialize(string filename)
        {
            try
            {
                Log.Info("deserializing: " + filename);
                Stream stream = File.Open(filename, FileMode.Open);
                var bFormatter = new BinaryFormatter();
                try
                {
                    var objectToSerialize = (T) bFormatter.Deserialize(stream);
                    stream.Close();
                    stream.Dispose();
                    Log.Info("deserializing: OK");
                    return objectToSerialize;
                }
                catch (Exception e)
                {
                    Log.Error(e);
                    stream.Close();
                    stream.Dispose();
                    throw e;
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex);
                throw ex;
            }
        }
    }
}