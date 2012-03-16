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
		public FunnelbackCrawler()
		{
		}
		
		public void Crawl()
		{
			FunnelbackConfig fbx = new FunnelbackConfig(@"C:\Users\rpfmorg\funnelback.cfg");

            //get all we need for claims authentication

            MsOnlineClaimsHelper claimsHelper = new MsOnlineClaimsHelper(fbx.targetSite,fbx.username,fbx.password);
             
            //from now on we can use sharepoint being authenticated 
            using (ClientContext ctx = new ClientContext(fbx.targetSite))
            {
                ctx.ExecutingWebRequest += claimsHelper.clientContext_ExecutingWebRequest;

                ctx.Load(ctx.Web);
                ctx.ExecuteQuery();
                if (ctx != null)
                {
                    Site oSite = ctx.Site;
                    WebCollection oWebs = oSite.RootWeb.Webs;
                    FunnelbackSite fbxs = new FunnelbackSite();
                    fbxs.ww = oSite.RootWeb;
                    fbxs.config = fbx;
                    fbxs.Process();
                    ctx.Load(oWebs);
                    ctx.ExecuteQuery();
                    fbxs.FunnelbackCommit();
                }
                

            }

		}
	}
}
