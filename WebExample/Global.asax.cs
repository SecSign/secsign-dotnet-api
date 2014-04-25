using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;

namespace WebExample
{
	public class Global : System.Web.HttpApplication
	{
		
		protected virtual void Application_Start (Object sender, EventArgs e)
		{
		}
		
		protected virtual void Session_Start (Object sender, EventArgs e)
		{
		}
		
		protected virtual void Application_BeginRequest (Object sender, EventArgs e)
		{
		}
		
		protected virtual void Application_EndRequest (Object sender, EventArgs e)
		{
		}
		
		protected virtual void Application_AuthenticateRequest (Object sender, EventArgs e)
		{
		}
		
		protected virtual void Application_Error (Object sender, EventArgs e)
		{
			if(Context != null)
			{
				Exception ex = Server.GetLastError();
				Response.Clear();
				
				if(ex.InnerException != null){
					Response.Write("<h1>" + ex.InnerException.Message + "</h1>");
				} else {
					Response.Write("<h1>" + ex.Message + "</h1>");
				}
				Response.Write("<pre>" + ex.ToString() + "</pre>");
						
				Server.ClearError();
			}
			
		}
		
		protected virtual void Session_End (Object sender, EventArgs e)
		{
		}
		
		protected virtual void Application_End (Object sender, EventArgs e)
		{
		}
	}
}

