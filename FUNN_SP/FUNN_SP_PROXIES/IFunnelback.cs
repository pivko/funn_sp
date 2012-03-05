/*
 * Created by SharpDevelop.
 * User: rpfmorg
 * Date: 05/03/2012
 * Time: 12:19
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace FUNN_SP_PROXIES
{
	/// <summary>
	/// Description of Interface1.
	/// </summary>
	public interface IFunnelback<T>
	{
		void FunnelbackAdd();
		void FunnelbackDelete();
		void FunnelbackCommit();
	}
}
