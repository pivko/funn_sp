/*
 * Created by VisualStudio.
 * User: mpiwoni
 * Date: 05/03/2012
 * Time: 16:27
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
	/// Description of FunnelbackUser.
	/// </summary>
	class FunnelbackUser
   {
     
           public FunnelbackConfig myfbx { get; set; }
           public Web ww { get; set; }
           public ClientContext context { get; set; }
     /// <summary>
     /// constructor
     /// </summary>
           public FunnelbackUser(ClientContext context)
           {
               this.context  = context;

           }


     
        public void ListUsers()
        {
            if (this.context != null)
            {
                GroupCollection collGroup = this.context.Web.SiteGroups;
                this.context.Load(collGroup);
                this.context.ExecuteQuery();
               
                 //Console.WriteLine(this.context.Web.SiteUserInfoList.);
                // Console.ReadLine();
                //Console.WriteLine(collGroup.Count);
               // Console.ReadKey();
                foreach (Group group_num in collGroup)
                {

                    Console.WriteLine(group_num.Id);
                    Group oGroup = collGroup.GetById(group_num.Id);

                    UserCollection collUser = oGroup.Users;

                    this.context.Load(collUser,
                                users => users.Include(
                                    user => user.Title,
                                    user => user.LoginName,
                                    user => user.Email));

                    this.context.ExecuteQuery();

                    foreach (User oUser in collUser)
                    {
                        Console.WriteLine("User: {0} Login name: {1} Email: {2}",
                        oUser.Title, oUser.LoginName, oUser.Email);
                    }







                }
            


            }
            
        }

}

}
