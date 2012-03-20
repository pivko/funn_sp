/*
 * Created by SharpDevelop.
 * User: rpfmorg
 * Date: 01/03/2012
 * Time: 16:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.SharePoint.Client;
using FUNN_SP_CRAWLER;

namespace FUNN_SP_PROXIES
{
	/// <summary>
	/// Description of FunnelbackList.
	/// </summary>
	public class FunnelbackList
	{
		#region Properties
		public FunnelbackConfig config { get; set; }
		public FunnelbackCrawler crawler { get; set; }
		public List ll { get; set; }
        public string LockString { get; set; }
        #endregion
		
		#region Constructors
		public FunnelbackList(List ll, FunnelbackCrawler crawler, string LockString)
		{
			this.ll = ll;
			this.crawler = crawler;
			this.config = crawler.config;
			this.LockString = LockString;
		}
		
		public FunnelbackList(List ll, string LockString)
		{
			this.ll = ll;
			this.LockString = LockString;
		}
		
		public FunnelbackList(ListItem li)
		{
			this.ll = ll;
		}
		
		public FunnelbackList()
		{
			this.ll = null;
		}
		#endregion
		
		#region Methods
		
		public void Process()
		{
			CamlQuery camlQuery = new CamlQuery();
			camlQuery.ViewXml = "<View><RowLimit>2000</RowLimit></View>";
            ListItemCollection collListItem = this.ll.GetItems(camlQuery);
            this.crawler.ctx.Load(collListItem,
                                 items => items.IncludeWithDefaultProperties(
                                            item => item.DisplayName,
                                            item => item.RoleAssignments,
                                            item => item.HasUniqueRoleAssignments
                                         ));
            this.crawler.ctx.ExecuteQuery();
            foreach (ListItem oListItem in collListItem)
           	{
            	FunnelbackItem oFI = new FunnelbackItem(oListItem, this.crawler, this.LockString);
                XmlSerializer ser = new XmlSerializer(typeof(FunnelbackItem));
                XmlWriter tx = XmlWriter.Create(@"C:\Users\rpfmorg\output\" + oFI.GetSafeFilename("xml"));
                ser.Serialize(tx, oFI);
                tx.Close();
                oFI.ProcessFile();
            }
		}
		#endregion
	}
}
