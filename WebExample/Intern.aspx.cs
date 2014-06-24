
//
// SecSign ID Api ASP.NET / C#
//
// (c) 2014 SecSign Technologies Inc.
//

using System;
using System.Web;
using System.Web.UI;

namespace WebExample
{
	public partial class Intern : System.Web.UI.Page
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
			
			// somebody logged in...
			this.lblSecSignID.Text = Request.QueryString["secsignid"];
		}
	}
}

