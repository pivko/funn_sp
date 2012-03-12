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

    public class FunnelbackXmlConfig
    {
        public string outputFolder { get; set; }
        public string targetSite { get; set; }
        public string[] WantedFields { get; set; }
        public string[] CDataFields { get; set; }
        public string[] LookupFields { get; set; }
        public string[] UserFields { get; set; }
    }

    public class FunnelbackXmlSite
    {
        public FunnelbackXmlConfig myfbx { get; set; }
        public Web ww { get; set; }

        public void Process()
        {
            if (this.ww != null)
            {
                WebCollection oWebs = this.ww.Webs;
                this.ww.Context.Load(oWebs);
                this.ww.Context.ExecuteQuery();
                foreach (Web sww in oWebs)
                {
                    Console.WriteLine("Site: {0}", sww.Title);
                    Console.ReadLine();
                    FunnelbackXmlSite fbxs = new FunnelbackXmlSite();
                    fbxs.myfbx = this.myfbx;
                    fbxs.ww = sww;
                    fbxs.Process();
                }
            }
        }

        public void FunnelbackWriteXml()
        {

        }
    }




 public class FunnelbackXmlRecord
	{
		public FunnelbackXmlConfig myfbx { get; set; }
		public ListItem li { get; set; }
				
		public string SafeFieldValue(string key)
		{
			string oSafeValueString = "None";
			if (this.li.FieldValues.Keys.Contains(key))
			{
				if (this.myfbx.WantedFields.Contains(key))
				{
					oSafeValueString = this.li.FieldValues[key].ToString();
				}
				if (this.myfbx.CDataFields.Contains(key))
				{
					oSafeValueString = @"<![CDATA[" + oSafeValueString + @"]]>";
				}
				if (this.myfbx.LookupFields.Contains(key))
				{
					FieldUserValue oFLV = (FieldUserValue)this.li.FieldValues[key];
					oSafeValueString = oFLV.LookupValue;
				}
			}
			return oSafeValueString;		
		}
		
		public void FunnelbackWriteXml()
		{
			if (this.li != null)
			{
				using (StreamWriter writer = new StreamWriter(this.myfbx.outputFolder + @"\" + this.li["UniqueId"].ToString() + ".xml"))
				{
					writer.WriteLine(@"<?xml version='1.0'?>");
					writer.WriteLine(@"<FBSPRecord>");
					writer.WriteLine("<id>{0}</id><title>{1}</title><hura>{2}</hura><type>{3}</type><null>{4}</null>",
                    	                  this.li.Id,
                    	                  this.li.DisplayName,
                    	                  this.li.HasUniqueRoleAssignments,
                    	                  this.li.FileSystemObjectType,
                    	                  this.li.ServerObjectIsNull
                    	                 );                 			
					List<string> klist = new List<string>(this.li.FieldValues.Keys);
					foreach (String klistkey in klist)
					{
						string oSFV = SafeFieldValue(klistkey);
						if (oSFV != "None")
						{
							writer.WriteLine("<{0}>{1}</{0}>", klistkey.Replace(" ", "_"), oSFV);							
						}
					}
					writer.WriteLine(@"</FBSPRecord>");
				}				
			}
		}
	}
    
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
                    using (StreamWriter writer = new StreamWriter(fbx.outputFolder + "\\first.xml"))
                    {
                        Site oSite = ctx.Site;
                        WebCollection oWebs = oSite.RootWeb.Webs;
                        FunnelbackSite fbxs = new FunnelbackSite();
                        fbxs.ww = oSite.RootWeb;
                        fbxs.myfbx = fbx;
                        fbxs.Process();
                        ctx.Load(oWebs);
                        ctx.ExecuteQuery();
                        writer.WriteLine(@"<?xml version='1.0'?>");
                        writer.WriteLine(@"<sharepoint>");
                        foreach (Web oWebsite in oWebs)
                        {

                            ListCollection collList = oWebsite.Lists;
                            ctx.Load(collList); // Query for Web
                            ctx.ExecuteQuery(); // Execute

                            writer.WriteLine(@"<site>");
                            writer.WriteLine("<title>{0}</title>", oWebsite.Title);
                            foreach (List oList in collList)
                            {
                                writer.WriteLine("<list>{0}</list>", oList.Title);
                                List oListy = collList.GetByTitle(oList.Title);
                                CamlQuery camlQuery = new CamlQuery();
                                camlQuery.ViewXml = "<View><RowLimit>100</RowLimit></View>";
                                ListItemCollection collListItem = oListy.GetItems(camlQuery);
                                ctx.Load(collListItem,
                                         items => items.IncludeWithDefaultProperties(
                                            item => item.DisplayName,
                                            item => item.HasUniqueRoleAssignments
                                         ));
                                ctx.ExecuteQuery();
                                foreach (ListItem oListItem in collListItem)
                                {
                                    FunnelbackItem oFI = new FunnelbackItem(oListItem);
                                    oFI.config = new FunnelbackConfig("funnelback.cfg");
                                    
                                    XmlSerializer ser = new XmlSerializer(typeof(FunnelbackItem));
                                    XmlWriter tx = XmlWriter.Create(@"C:\Users\rpfmorg\output\" + oFI.GetSafeFilename("xml"));
                                    ser.Serialize(tx, oFI);
                                    tx.Close();
                                }
                            }
                            writer.WriteLine(@"</site>");
                        }
                        writer.WriteLine(@"</sharepoint>");

                    }
                }
                

            }
        }
    }
}
