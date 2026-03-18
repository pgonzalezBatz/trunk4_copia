<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PlantillaAviso.aspx.vb" Inherits="GTK_Troqueleria.PlantillaAviso" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <table id="emailBody" style="width: 100%; font-family: Verdana, Geneva, Tahoma, sans-serif; font-size: x-small; border: 1px solid #5D7B9D;">
            <caption style="font-weight: bold; background-color: #CCCCCC;">{0}</caption>
            <tr>
                <th style="background-color: #5D7B9D; text-align: right; color: White; white-space: nowrap; width: 1%; border: 1px solid #5D7B9D;">
                    <asp:Label ID="Label1" runat="server" Text="Numero Trabajador" />
                </th>
                <td style="white-space: nowrap; border: 1px solid #5D7B9D;">{1}</td>
            </tr>
            <tr>
                <th style="background-color: #5D7B9D; text-align: right; color: White; white-space: nowrap; width: 1%; border: 1px solid #5D7B9D;">
                    <asp:Label ID="Label2" runat="server" Text="OF-OP-(Marca)" />
                </th>
                <td style="white-space: nowrap; border: 1px solid #5D7B9D;">{2}</td>
            </tr>
            <%--<tr>
                <th style="background-color: #5D7B9D; text-align: right; color: White; white-space: nowrap; width: 1%; border: 1px solid #5D7B9D;">
                    <asp:Label ID="Label3" runat="server" Text="Funcion" />
                </th>
                <td style="white-space: nowrap; border: 1px solid #5D7B9D;">{3}</td>
            </tr>--%>
            <tr>
                <th style="background-color: #5D7B9D; text-align: right; color: White; white-space: nowrap; width: 1%; border: 1px solid #5D7B9D;">
                    <asp:Label ID="Label4" runat="server" Text="SubProceso" />
                </th>
                <td style="white-space: nowrap; border: 1px solid #5D7B9D;">{3}</td>
            </tr>
            <tr>
                <th style="background-color: #5D7B9D; text-align: right; color: White; white-space: nowrap; width: 1%; border: 1px solid #5D7B9D;">
                    <asp:Label ID="Label3" runat="server" Text="Producto" />
                </th>
                <td style="white-space: nowrap; border: 1px solid #5D7B9D;">{4}</td>
            </tr>
            <tr>
                <th style="background-color: #5D7B9D; text-align: right; color: White; white-space: nowrap; width: 1%; border: 1px solid #5D7B9D;">
                    <asp:Label ID="Label8" runat="server" Text="Característica" />
                </th>
                <td style="white-space: nowrap; border: 1px solid #5D7B9D;">{5}</td>
            </tr>
            <tr>
                <th style="background-color: #5D7B9D; text-align: right; color: White; white-space: nowrap; width: 1%; border: 1px solid #5D7B9D;">
                    <asp:Label ID="Label5" runat="server" Text="Descripcion" />
                </th>
                <td>
                    <pre style="font-family: Verdana, Geneva, Tahoma, sans-serif; border: 1px solid #5D7B9D;">{6}</pre>
                </td>
            </tr>
            <tr>
                <th style="background-color: #5D7B9D; text-align: right; color: White; white-space: nowrap; width: 1%; border: 1px solid #5D7B9D;">
                    <asp:Label ID="Label6" runat="server" Text="Accion" />
                </th>
                <td>
                    <pre style="font-family: Verdana, Geneva, Tahoma, sans-serif; border: 1px solid #5D7B9D;">{7}</pre>
                </td>
            </tr>
            <tr>
                <th style="background-color: #5D7B9D; text-align: right; color: White; white-space: nowrap; width: 1%; border: 1px solid #5D7B9D;">
                    <asp:Label ID="Label7" runat="server" Text="ID Intranet" />
                </th>
                <td>
					<a href="{8}" target="_blank" type="text/html">{9}</a>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="background-color: #5d9d95; font-weight: bold; color: #FFFFFF; text-align: center;">{10}</td>
            </tr>            
        </table>

    </form>
</body>
</html>
