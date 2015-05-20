<%@ Page Language="C#" Inherits="WebExample.Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">

<!--

//
// SecSign ID Api ASP.NET / C#
//
// (c) 2014, 2015 SecSign Technologies Inc.
//

-->

<html>
<head runat="server">
	<title>SecSignIDApi ASP.NET example</title>
</head>
<body>
	<h1>SecSignIDApi ASP.NET example</h1>
	<br /><br /><br />
		
	<form runat='server' id='LoginForm'>
		<asp:RegularExpressionValidator runat='server'
			Display='dynamic'
            ControlToValidate="secsignid" 
            ErrorMessage="SecSign ID contains illegal characters."
            ValidationExpression="^[\w\-_@.]+$">&nbsp; <!-- do not display anything in front of label secsign id -->
		</asp:RegularExpressionValidator>
		<asp:ValidationSummary runat='server' HeaderText="An error occured: " />
		<asp:RequiredFieldValidator runat='server'
			Display='dynamic'
            ControlToValidate='secsignid'
            ErrorMessage="SecSign ID is required.">&nbsp; <!-- do not display anything in front of label secsign id -->
		</asp:RequiredFieldValidator>
			
		<br />
			
		SecSign ID: <input id='secsignid' name='secsignid' type='text' size='30' maxlength='30' runat='server' />
		<asp:button name='login' id='login' type='submit' value='1' runat='server' Text='Login'  PostBackUrl="~/SecSignID.aspx"/> <br />
	</form>		
	
</body>
</html>
