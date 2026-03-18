<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Responsables.aspx.vb" Inherits="GTK_Troqueleria.Responsables" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
         <table id="emailBody" style="width: 100%; font-family: Verdana, Geneva, Tahoma, sans-serif; font-size: x-small;">
			<caption style="font-weight: bold; background-color: #CCCCCC;">{0}</caption>
			<tr>
				<th style="background-color: #5D7B9D; text-align: right; color: White; white-space: nowrap; width: 1%;">
					<asp:Label ID="Label1" runat="server" Text="No Conformidad:" /></th>
				<td style="white-space: nowrap;">{1}</td>
			</tr>
			<tr>
				<th style="background-color: #5D7B9D; text-align: right; color: White; white-space: nowrap; width: 1%;">
					<asp:Label ID="Label4" runat="server" Text="Descripción:" /></th>
				<td>
					<pre style="font-family: Verdana, Geneva, Tahoma, sans-serif;">{2}</pre>
				</td>
			</tr>
			<tr>
				<td colspan="2" style="background-color: #5d9d95; font-weight: bold; color: #FFFFFF; text-align: center;">{3}</td>
			</tr>
		</table>
    </form>
</body>
</html>
