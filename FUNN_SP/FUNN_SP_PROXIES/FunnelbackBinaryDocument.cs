/*
 * Created by SharpDevelop.
 * User: rpfmorg
 * Date: 21/03/2012
 * Time: 12:55
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using Microsoft.SharePoint.Client;
using FUNN_SP_AUTH;
using FUNN_SP_CRAWLER;

namespace FUNN_SP_PROXIES
{
	/// <summary>
	/// Description of FunnelbackBinaryDocument.
	/// </summary>
	public class FunnelbackBinaryDocument
	{
		#region Properties
		public Microsoft.SharePoint.Client.File fi { get; set; }
		public FunnelbackCrawler crawler { get; set; }
		public FunnelbackConfig config { get; set; }
		#endregion
		
		#region Constructors
		public FunnelbackBinaryDocument(Microsoft.SharePoint.Client.File fi, FunnelbackCrawler crawler)
		{
			this.fi = fi;
			this.crawler = crawler;
			this.config = crawler.config;
		}
		#endregion
		
		#region Methods
		public void ProcessFile()
		{
			Console.WriteLine("WORD exists called: {0} at {1} or {2}",
				    	            this.fi.Name,
									this.fi.Path,
								    this.fi.ServerRelativeUrl);
			ClaimsWebClient cwc = new ClaimsWebClient(new Uri(this.config.targetSite),this.config.username,this.config.password);
					
			Stream fs = ((ClaimsWebClient)cwc).OpenRead(
				string.Format("{0}{1}", this.config.urlstub, this.fi.ServerRelativeUrl));
						
			FileStream ofs = System.IO.File.Create(this.config.outputFolder + @"\" + this.fi.Name);
			fs.CopyTo(ofs);
			ofs.Close();
			Console.WriteLine("Check " + this.config.outputFolder + @"\" + this.fi.Name);
			Console.ReadLine();				
		}
		#endregion

	}
}
