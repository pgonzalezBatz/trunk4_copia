<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PermisoDenegado2.aspx.vb" Inherits="KaplanNew.PermisoDenegado2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="position:absolute; width:auto; top:30%; left:40%;">
                <tr>
                    <td style="width: 10%">
                        <asp:Image ID="Image1" runat="server" ImageAlign="Middle" ToolTip="Error" ImageUrl="~/App_Themes/Batz/Imagenes/error_big.gif" /></td>
                    <td>
                        <asp:Label runat="server" ID="lblMensaje" Style="font-weight: bold; font-size: 28px;" Text="Permiso denegado"></asp:Label>
                    </td>
                </tr>
                <tr>                                
                    <td style="text-align:left" colspan="2">
                        &nbsp;</td>                          
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
