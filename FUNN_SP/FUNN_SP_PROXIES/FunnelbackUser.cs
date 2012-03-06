/*
 * Created by VisualStudio.
 * User: mpiwoni
 * Date: 05/03/2012
 * Time: 16:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
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
           public ClientContext context { get; set; }
           
       // constructor

           public FunnelbackUser(ClientContext context, FunnelbackConfig fbx)
           {
               this.context  = context;
               this.myfbx = fbx;

           }

        //methods
     
        public void ListUsers()
        {
            if (this.context != null)
            {
               GroupCollection collGroup = this.context.Web.SiteGroups;
               this.context.Load(collGroup);
               this.context.ExecuteQuery();

               using (StreamWriter writer = new StreamWriter(this.myfbx.outputFolder + "\\users.xml"))
               {
                   writer.WriteLine("<?xml version='1.0'?>");
                   writer.WriteLine("<sharepoint>");


               
                foreach (Group group_num in collGroup)
                {

                   // Console.WriteLine(group_num.Id);
                    Group oGroup = collGroup.GetById(group_num.Id);

                    UserCollection collUser = oGroup.Users;

                    this.context.Load(collUser,
                                users => users.Include(
                                    user => user.Id,
                                    user => user.Title,
                                    user => user.LoginName,
                                    user => user.Email));

                    this.context.ExecuteQuery();

                    


                        

                    foreach (User oUser in collUser)
                    {

                  
                         writer.WriteLine("<user>");
                            
                         writer.WriteLine("<id>{0}</id>", oUser.Id);
                         writer.WriteLine("<group>{0}</group>", group_num.Id);
                         writer.WriteLine("<group_title>{0}</group_title>", group_num.Title);
                        writer.WriteLine("<title>{0}</title>", oUser.LoginName);
                        writer.WriteLine("<loginname>{0}</loginname>", oUser.LoginName);
                        writer.WriteLine("<email>{0}</email>", oUser.Email);
                     
                               

                        writer.WriteLine("</user>");

                        Console.WriteLine("User: {0} Login name: {1} Email: {2}",
                       oUser.Title, oUser.LoginName, oUser.Email);
                      
                    }

                    
                   }

                

               writer.WriteLine("</sharepoint>");
               }

                    //Iterate the owners group
                Group ownerGroup = this.context.Web.AssociatedOwnerGroup;
                this.context.Load(ownerGroup);
                this.context.Load(ownerGroup.Users);
                this.context.ExecuteQuery();


                foreach (User ownerUser in ownerGroup.Users)
                {
                //Console.WriteLine("User Name: " + ownerUser.Title + " Email: " +
               // ownerUser.Email + " Login: " + ownerUser.LoginName);
                }

                /*
                RoleAssignmentCollection roles = this.context.Web.RoleAssignments;
                this.context.Load(roles);
                this.context.ExecuteQuery();
                foreach (RoleAssignment role in roles)
                {
                    this.context.Load(role.Member);
                    this.context.ExecuteQuery();
                    Console.WriteLine(role.Member.LoginName);
                }
                */
                





            


            }
            
        }

}

}
