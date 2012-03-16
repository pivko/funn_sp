/*
 * Created by SharpDevelop.
 * User: rpfmorg
 * Date: 01/03/2012
 * Time: 15:48
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.SharePoint.Client;

namespace FUNN_SP_PROXIES
{
	/// <summary>
	/// Funnelback proxy for a Sharepoint Site
	/// </summary>
	[XmlRoot("fbSite")]
	public class FunnelbackSite : IFunnelback<FunnelbackSite>
	{
		#region Properties
		public FunnelbackConfig config { get; set; }
        public Web ww { get; set; }
        #endregion

        #region Methods
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
                    FunnelbackSite fbxs = new FunnelbackSite();
                    fbxs.config = this.config;
                    fbxs.ww = sww;
                    fbxs.Process();
                    
                    ListCollection collList = sww.Lists;
                    sww.Context.Load(collList); // Query for Web
                    sww.Context.ExecuteQuery(); // Execute

                    foreach (List oList in collList)
                    {
                    	List oListy = collList.GetByTitle(oList.Title);
                        FunnelbackList oFbl = new FunnelbackList(oListy, fbxs.GetLockString());
                        oFbl.config = fbxs.config;
                        oFbl.Process();
                    }
                }
            }
        }
        
        public string GetLockString()
        {
        	string lockstring = "";
        	if (this.ww != null)
        	{
        		RoleAssignmentCollection oRac = this.ww.RoleAssignments;
        		this.ww.Context.Load(oRac);
        		this.ww.Context.ExecuteQuery();
        		foreach (RoleAssignment oRa in oRac)
        		{
        			Principal oMem = oRa.Member;
        			this.ww.Context.Load(oMem);
        			this.ww.Context.ExecuteQuery();
        			lockstring += oMem.Id.ToString();
        			lockstring += "LL";
        			RoleDefinitionBindingCollection oRDBC = oRa.RoleDefinitionBindings;
        			this.ww.Context.Load(oRDBC);
        			this.ww.Context.ExecuteQuery();
        			foreach (RoleDefinition oRd in oRDBC)
        			{
        				lockstring += oRd.Name;
        				lockstring += "|";
        			}
        		}
        	}
        	return lockstring;
        }

        #endregion
        
        #region Funnelback
        
        public void FunnelbackAdd()
        {
        	
        }
        
        public void FunnelbackDelete()
        {
        	
        }
        
        public void FunnelbackCommit()
        {
        	FunnelbackWinSCP fbconnect = new FunnelbackWinSCP(this.config);
            fbconnect.Synchronize();
        }
        
        #endregion
	}
}
