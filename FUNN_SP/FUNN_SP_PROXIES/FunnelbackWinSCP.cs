/*
 * Created by SharpDevelop.
 * User: rpfmorg
 * Date: 12/03/2012
 * Time: 16:20
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using WinSCP;

namespace FUNN_SP_PROXIES
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public class FunnelbackWinSCP
	{
		#region Properties
		private Session oSession {get; set;}
		private SessionOptions oSessionOptions {get; set;}
		public FunnelbackConfig config {get; set;}
		#endregion

		#region Constructor
		public FunnelbackWinSCP(FunnelbackConfig config)
		{
			this.config = config;
			this.oSessionOptions = new SessionOptions {
				Protocol = Protocol.Scp,
				HostName = this.config.fbdomain,
				UserName = this.config.fbuser,
				Password = this.config.fbpassword,
				SshHostKey = "ssh-rsa 2048 e6:98:fa:82:4a:76:fe:78:46:18:66:bf:a6:9d:6b:a8"
			};
		}
		#endregion
		
		#region Methods
		public void Synchronize()
		{
			using (Session session = new Session())
			{
				session.Open(this.oSessionOptions);
				SynchronizationResult oSR;
				oSR = session.SynchronizeDirectories(
					SynchronizationMode.Remote,
					@"C:\Users\rpfmorg\output",
					"/opt/funnelback/custom_data/sharepoint-output", false);
				oSR.Check();
			}
		}
		
		#endregion

	}
}
