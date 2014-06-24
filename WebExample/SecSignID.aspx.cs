

//
// SecSign ID Api ASP.NET / C#
//
// (c) 2014 SecSign Technologies Inc.
//


using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;

using SecSignID;

namespace WebExample
{
	public partial class SecSignID : System.Web.UI.Page
	{
		/// <summary>
		/// Is called whenever the page is loaded.
		/// </summary>
		protected void Page_Load(object sender, EventArgs e)
		{
			if(PreviousPage == null && !Page.IsPostBack)
			{
				// user is not allowed to call this page directly
				// input field for secsign id is in class Default.aspx as qell as validation controls
				Response.Redirect("Default.aspx");
				return;
			}
			
			if(PreviousPage != null && PreviousPage.IsCrossPagePostBack)
			{
				// browser did send post request to this page
				Default sourcePage = (Default) PreviousPage;
				string secsignString = sourcePage.SecSignID;
			
				// get auth session
				this.requestAuthSessionForLogin(secsignString);
			}
			else
			{
				if(Page.IsPostBack)
				{
					// form send form back to server
					// get post data and decide whether go back when cancel has been clicked or to check ticket
					NameValueCollection form = Request.Form;
					if (!string.IsNullOrEmpty(form["cancel"]))
					{
						//string cancelValue = form["cancel"];
						//if(cancelValue.Equals("1")) // <- asp sets its own values which are equal to Text-attribute
						{
							this.cancelLoginProcess();
							return;
						}
					}
					else if (!string.IsNullOrEmpty(form["check"]))
					{
						//string checkValue = form["check"];
						//if(checkValue.Equals("1"))
						{
							this.checkAuthSessionState();
							return;
						}
					}
				}
			}
 		}
		
		/// <summary>
		/// Requests the authentication session for login.
		/// </summary>
		/// <param name='secsignidString'>the secsign id the authentication session is for</param>
		private void requestAuthSessionForLogin(string secsignidString)
		{
			// request authentication session
			SecSignIDApi secSignIDApi = null;
			AuthSession authSession = null;
			try
			{
				secSignIDApi = new SecSignIDApi();
				authSession = secSignIDApi.RequestAuthSession(secsignidString, "ASP.NET example how to use SecSignIDApi", "localhost");

				// set all values
				this.secsignid.Value 		= authSession.GetSecSignID();
				this.authsessionid.Value	= authSession.GetAuthSessionID();
				this.requestid.Value   		= authSession.GetRequestID();
				this.servicename.Value   	= authSession.GetRequestingService();
				this.serviceaddress.Value   = authSession.GetRequestingServiceAddress();
				
				this.authSessionIconDisplay.Src = "data:image/png;base64," + authSession.GetIconData();
				this.authSessionIconDisplay.Alt = "SecSign ID Access Pass Icon";
			}
			catch(System.Net.WebException ex)
			{
				handleError(ex, false);
			}
			catch(System.Exception ex)
			{
				if(secSignIDApi != null && authSession != null)
				{
					// we could get an auth session which has to be canceled now
					try
					{
						secSignIDApi.CancelAuthSession(authSession);
					}
					catch{}
				}
				handleError(ex, false);
			}
		}
		
		/// <summary>
		/// Checks the authentication session status.
		/// </summary>
		private void checkAuthSessionState()
		{
			int authSessionState = AuthSession.NOSTATE;
			try
			{
				NameValueCollection form = Request.Form; // get form which has send the request
				
				AuthSession authSession = new AuthSession(form["secsignid"], 
									form["authsessionid"],
									form["requestid"], null,null, null); 
				
				SecSignIDApi secSignIDApi = new SecSignIDApi();
				
				// check ticket state
				authSessionState = secSignIDApi.GetAuthSessionState(authSession);
				
				if(authSessionState == AuthSession.ACCEPTED){
					Response.Redirect("Intern.aspx?secsignid=" + authSession.GetSecSignID());
				}
				else if(authSessionState == AuthSession.PENDING || authSessionState == AuthSession.FETCHED){
					this.lblMessage.Text = "the auth session is still pending... it has neither be accepted nor denied.";
				}
				else
				{
					if(authSessionState == AuthSession.DENIED){
						Response.Write("You have denied the auth session...");
					}

					// render previous page
					if(PreviousPage != null){
						Response.Redirect(PreviousPage.AppRelativeVirtualPath);
					} else {
						Response.Redirect("Default.aspx");
					}
				}
			}
			catch(System.Exception ex)
			{
				handleError(ex, false);
			}
		}
		
		/// <summary>
		/// Cancels the login process.
		/// </summary>
		private void cancelLoginProcess()
		{
			try
			{
				NameValueCollection form = Request.Form; // get form which has send the request
				
				AuthSession authSession = new AuthSession(form["secsignid"], form["authsessionid"], form["requestid"], null, null, null);
				SecSignIDApi secSignIDApi = new SecSignIDApi();
				
				// cancel auth session
				secSignIDApi.CancelAuthSession(authSession);
				
				// render previous page
				if(PreviousPage != null){
					Response.Redirect(PreviousPage.AppRelativeVirtualPath);
				} else {
					Response.Redirect("Default.aspx");
				}
			}
			catch(System.Exception ex)
			{
				handleError(ex, false);
			}
		}
		
		/// <summary>
		/// Handles the error.
		/// </summary>
		/// <param name='ex'>the error</param>
		/// <param name='redirect'>shall be redirected or use of Server.Transfer</param>
		private void handleError(Exception ex, bool redirect)
		{
			// print error
			Response.Write("<h1>" + ex.Message + "</h1>");
			Response.Write("<pre>" + ex.ToString() + "</pre>");
				
			// render previous page
			string path = (PreviousPage != null ? PreviousPage.AppRelativeVirtualPath : "Default.aspx");
			if(redirect){
					Response.Redirect(path);
			} else {
					Server.Transfer(path);
			}
		}
	}
}

