using System;
using System.IO;
using System.Xml;
using System.Data;

namespace TomTang.DbAccess {
	/// <summary>
	/// Assist to handle the transform between database object and DOM object
	/// </summary>
	public class XmlDbHelper {
		private XmlDbHelper(){}
		/// <summary>
		/// Get a xml string from an existed dataset object.
		/// </summary>
		/// <param name="ds">dataset object</param>
		/// <returns>xml string</returns>
		public static string DataSet2Xml(DataSet ds) {
			StringWriter oWriter = new StringWriter();
			ds.WriteXml(oWriter);
			return oWriter.ToString();
		}
		/// <summary>
		/// Get a general dataset object from xml string
		/// </summary>
		/// <param name="Xml">Xml string</param>
		/// <returns>General dataset object</returns>
		public static DataSet Xml2DataSet(string Xml) {
			DataSet oDs = new DataSet();
			StringReader oReader = new StringReader(Xml);
			oDs.ReadXml(oReader);
			return oDs;
		}
		/// <summary>
		/// You may pass your customerized strong-type dataset as reference to restore by xml string
		/// </summary>
		/// <param name="Xml">Xml string</param>
		/// <param name="ds">Customerized strong-type dataset</param>
		public static void Xml2DataSet(string Xml, DataSet ds) {
			if (null == ds) throw new NullReferenceException("You can't pass null as customerized strong-type dataset object, there must be something!");
			StringReader oReader = new StringReader(Xml);
			ds.ReadXml(oReader);
		}
	}
}
