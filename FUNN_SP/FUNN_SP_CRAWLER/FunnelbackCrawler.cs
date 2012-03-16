/*
 * Created by SharpDevelop.
 * User: rpfmorg
 * Date: 16/03/2012
 * Time: 12:58
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.SharePoint.Client;
using FUNN_SP_PROXIES;
using FUNN_SP_AUTH;

namespace FUNN_SP_CRAWLER
{
	/// <summary>
	/// Description of FunnelbackCrawler.
	/// </summary>
	public class FunnelbackCrawler
	{
		#region Properties
		public ClientContext ctx { get; set; }
		public FunnelbackConfig config { get; set; }
		#endregion
		
		#region Constructors
		public FunnelbackCrawler(ClientContext ctx, FunnelbackConfig config)
		{
			this.ctx = ctx;
			this.config = config;
		}
		
		public FunnelbackCrawler(String configpath)
		{
			this.config = new FunnelbackConfig(configpath);
			this.ctx = GenerateClientContext();
		}

		public FunnelbackCrawler()
		{
			this.config = new FunnelbackConfig(@"C:\Users\rpfmorg\funnelback.cfg");
			this.ctx = GenerateClientContext();
		}
		
		public FunnelbackCrawler(FunnelbackConfig config)
		{
			this.config = config;
			this.ctx = GenerateClientContext();
		}
		#endregion
		
		#region Methods
		public ClientContext GenerateClientContext()
		{
			MsOnlineClaimsHelper claimsHelper = new MsOnlineClaimsHelper(config.targetSite,config.username,config.password);
			ClientContext ctx = new ClientContext(config.targetSite);
			ctx.ExecutingWebRequest += claimsHelper.clientContext_ExecutingWebRequest;
			return ctx;
		}
		
		public void Crawl()
		{
            if (this.ctx != null)
            {
                Site oSite = this.ctx.Site;
                WebCollection oWebs = oSite.RootWeb.Webs;
                FunnelbackSite fbxs = new FunnelbackSite();
                fbxs.ww = oSite.RootWeb;
                fbxs.config = this.config;
                fbxs.Process();
                this.ctx.Load(oWebs);
                this.ctx.ExecuteQuery();
                fbxs.FunnelbackCommit();
            }
		}
		#endregion
	}
}
