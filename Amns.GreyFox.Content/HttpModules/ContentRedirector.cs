using System;
using System.Configuration;

namespace Amns.GreyFox.Content.HttpModules
{
	public class ContentRedirector : System.Web.IHttpModule 
	{
		/// <summary>
		/// Init is required from the IHttpModule interface
		/// </summary>
		/// <param name="Appl"></param>
		public void Init(System.Web.HttpApplication Appl) 
		{
			//make sure to wire up to BeginRequest
			Appl.BeginRequest+=new System.EventHandler(Rewrite_BeginRequest);
		}
 
		/// <summary>
		/// Dispose is required from the IHttpModule interface
		/// </summary>
		public void Dispose() 
		{
			//make sure you clean up after yourself
		}

		/// <summary>
		/// To handle the starting of the incoming request
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void Rewrite_BeginRequest(object sender, System.EventArgs args) 
		{		
			System.Web.HttpApplication Appl = (System.Web.HttpApplication)sender;
			string path = Appl.Request.Path;

			if(path.StartsWith("/Content/"))
			{
				string clip = GetClip(path);
				sendToNewUrl(url, Appl);
			}			
		}

		private void getClip(string path)
		{

		}

		private void sendToNewUrl(string url, System.Web.HttpApplication Appl) 
		{
			Appl.Context.RewritePath(url);
		}
	}
}