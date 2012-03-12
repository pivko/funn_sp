/*
 * Created by SharpDevelop.
 * User: rpfmorg
 * Date: 05/03/2012
 * Time: 12:37
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Net;
using System.Text;

namespace FUNN_SP_PROXIES
{
	/// <summary>
	/// Description of FunnelbackWebRequest.
	/// </summary>
	public class FunnelbackWebRequest
	{
		#region Properties
		private WebRequest oWR {get; set;}
		public FunnelbackConfig config {get; set;}
		#endregion
		
		#region Constructor
		public FunnelbackWebRequest(FunnelbackConfig config)
		{
			this.config = config;
		}
		#endregion
		
		#region Methods
		public void Post()
		{
			string url = config.url;
			string postData = "test";
			byte[] byteArray = Encoding.UTF8.GetBytes(postData);
			this.oWR = WebRequest.Create(config.url);
			this.oWR.Method = "POST";
			this.oWR.ContentType = "application/x-www-form-urlencoded";
			this.oWR.ContentLength = byteArray.Length;
			Stream dataStream = this.oWR.GetRequestStream();
			dataStream.Write(byteArray, 0, byteArray.Length);
			dataStream.Close();
			
			WebResponse oWResp = this.oWR.GetResponse();
			
		}
		#endregion
	}
}
