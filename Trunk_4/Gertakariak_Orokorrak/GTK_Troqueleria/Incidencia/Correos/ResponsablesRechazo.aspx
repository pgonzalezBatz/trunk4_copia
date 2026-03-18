<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ResponsablesRechazo.aspx.vb" Inherits="GTK_Troqueleria.ResponsablesRechazo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1b" runat="server">
         <table id="emailBody" style="width: 100%; font-family: Verdana, Geneva, Tahoma, sans-serif; font-size: x-small;">
			<caption style="font-weight: bold; background-color: #CCCCCC;">{0}</caption>
			<tr>
				<th style="background-color: #5D7B9D; text-align: right; color: White; white-space: nowrap; width: 1%;">
					<asp:Label ID="Label1b" runat="server" Text="No Conformidad:" /></th>
				<td style="white-space: nowrap;">{1}</td>
			</tr>
			<tr>
				<th style="background-color: #5D7B9D; text-align: right; color: White; white-space: nowrap; width: 1%;">
					<asp:Label ID="Label4b" runat="server" Text="Razón rechazo:" /></th>
				<td>
					<pre style="font-family: Verdana, Geneva, Tahoma, sans-serif;">{2}</pre>
				</td>
			</tr>
		</table>
    </form>
</body>
</html>
