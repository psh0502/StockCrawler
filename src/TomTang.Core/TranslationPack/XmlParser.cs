using System;
using System.IO;
using System.Xml;

namespace TomTang.TranslationPack
{
	/// <summary>
	/// LangXmlParser ªººK­n´y­z¡C
	/// </summary>
	public class XmlParser
	{
		// Fields
		private XmlDocument moXD = new XmlDocument();
		public XmlParser(FileInfo languagePackFile)
		{
			StreamReader oReader = languagePackFile.OpenText();
			try
			{
				moXD.LoadXml(oReader.ReadToEnd());
			}
			catch
			{
				throw;
			}
			finally
			{
				oReader.Close();
			}
		}
		public XmlParser(string Xml)
		{
			moXD.LoadXml(Xml);
		}

		public string AttributeText(string strXPath, string sAttributeName)
		{
			try
			{
				return moXD.DocumentElement.SelectSingleNode(strXPath).Attributes[sAttributeName].Value;
			}
			catch
			{
				return null;
			}
		}

		public XmlAttributeCollection GetAttributes(string strXPath)
		{
			try
			{
				return moXD.DocumentElement.SelectSingleNode(strXPath).Attributes;
			}
			catch
			{
				return null;
			}
		}

		public string uiText(string strXPath)
		{
			try
			{
				return moXD.DocumentElement.SelectSingleNode(strXPath).InnerText;
			}
			catch
			{
				return null;
			}
		}
	}
}
