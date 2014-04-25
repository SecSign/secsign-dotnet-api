<%@ Page Language="C#" Inherits="WebExample.SecSignID" %>
<%@ PreviousPageType VirtualPath="~/Default.aspx" %> 

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head runat="server">
	<title>SecSignIDApi ASP.NET example</title>
</head>
<body>
	<h1>SecSignIDApi ASP.NET example</h1>
	<br /><br /><br />
		
	<form id='CheckAuthSessionForm' runat='server'>
		<asp:Label id='lblMessage' runat='server'/>
			
		<asp:HiddenField id='requestid' value='' runat='server'/> <br />
		<asp:HiddenField id='secsignid' value='' runat='server'/> <br />
		<asp:HiddenField id='authsessionid' value='' runat='server'/> <br />
		<asp:HiddenField id='servicename' value='' runat='server'/> <br />
		<asp:HiddenField id='serviceaddress' value='' runat='server'/> <br />
		<asp:HiddenField id='authsessionicondata' value='' runat='server'/> <br />

		<table>
			<tr>
				<td colspan='2'>
					Please verify the access pass using your smartphone: <br /><br/>
				</td>
			</tr>
			<tr>
				<td colspan='2'>
					<img id='authSessionIconDisplay' src="" runat='server' />
					<br /><br />
				</td>
			</tr>
			<tr>
				<td align='left'>
					<asp:button type ='submit' name='cancel' id='cancel' value='1' style='width:100px' runat='server' Text='Cancel' />
				</td>
				<td align='right'>
					<asp:button type ='submit' name='check' id='check' value='1' style='width:100px' runat='server' Text='OK' />
				</td>
			</tr>
		</table>
	</form>
</body>
</html>