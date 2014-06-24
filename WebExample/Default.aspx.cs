
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
	public partial class Default : System.Web.UI.Page
	{
		/// <summary>
		/// Getter for the secsign id. The getter is used in SecSignID.aspx where PreviousPage is set due to Cross Page Post Backs
		/// </summary>
		public String SecSignID
		{
   		 	get
    		{
        		return secsignid.Value;
    		}
		}
	}
}

