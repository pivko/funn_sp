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

using SP = Microsoft.SharePoint.Client;

namespace FUNN_SP {
	
    class Program {
       
        static void Main(string[] args) {
                
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
