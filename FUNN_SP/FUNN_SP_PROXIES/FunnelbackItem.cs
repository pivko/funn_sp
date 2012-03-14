/*
 * Created by SharpDevelop.
 * User: rpfmorg
 * Date: 01/03/2012
 * Time: 16:24
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Microsoft.SharePoint.Client;

namespace FUNN_SP_PROXIES
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public class FunnelbackItem : IXmlSerializable, IFunnelback<FunnelbackItem>
	{
		
		#region Properties
		
		public ListItem li { get; set; }
		public StreamWriter writer { get; set; }
		public FunnelbackConfig config { get; set; }
		public string LockString { get; set; }
		
		#endregion

		#region Constructors
		
		public FunnelbackItem(ListItem li, string LockString)
		{
			this.li = li;
			this.LockString = LockString;
		}
		
		public FunnelbackItem(ListItem li)
		{
			this.li = li;
		}
		
		public FunnelbackItem()
		{
			this.li = null;
		}
		
		#endregion
		
		#region Methods
		
		public string GetSafeFilename(string extension)
		{
			//TODO: This isn't actually very safe at the moment
			return this.li.FieldValues["UniqueId"] + "." + extension;
		}
		
		#endregion
		
		#region Xml Serialization
		
		public void WriteXml(XmlWriter xwriter)
		{
			if (this.li != null)
			{
				xwriter.WriteStartElement("fbitem");
				foreach(string fieldkey in this.config.WantedFields)
				{
					xwriter.WriteElementString(fieldkey, SafeFieldValue(fieldkey));
				}
				xwriter.WriteElementString(@"url", this.GetSafeUrl("FileRef"));
				xwriter.WriteElementString(@"LockString", this.LockString);
				xwriter.WriteEndElement();
			}
		}
		
		public void ReadXml(XmlReader xreader)
		{
			
		}
		
		public XmlSchema GetSchema()
		{
			return (null);
		}
				
		#endregion
		
		#region Funnelback Push
		
		public void FunnelbackAdd()
		{
			
		}
		
		public void FunnelbackDelete()
		{
			
		}
		
		public void FunnelbackCommit()
		{
			
		}
		
		#endregion
		
		#region Utilities
		
		public string GetSafeUrl(string key)
		{
			string oSafeUrl = "None";
			if (this.li.FieldValues.Keys.Contains(key))
			{
				oSafeUrl = this.config.urlstub + this.li.FieldValues[key];
			}
			return oSafeUrl;
		}
		
		public string SafeFieldValue(string key)
		{
			string oSafeValueString = "None";
			if (this.li.FieldValues.Keys.Contains(key))
			{
				if (this.config.WantedFields.Contains(key))
				{
					oSafeValueString = this.li.FieldValues[key].ToString();
				}
				if (this.config.CDataFields.Contains(key))
				{
					oSafeValueString = @"<![CDATA[" + oSafeValueString + @"]]>";
				}
				if (this.config.LookupFields.Contains(key))
				{
					FieldUserValue oFLV = (FieldUserValue)this.li.FieldValues[key];
					oSafeValueString = oFLV.LookupValue;
				}
			}
			return oSafeValueString;		
		}

		#endregion		
			
	}
}
