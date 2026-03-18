<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PreAsientoCont.ascx.vb" Inherits="WebRaiz.PreAsientoCont" %>
<script src="../../../js/Utiles.js" type="text/javascript"></script>
<script src="../../../js/jQuery/jquery.js" type="text/javascript"></script>
<script src="../../../App_Themes/Tema1/ClueTip/jquery.cluetip.js" type="text/javascript"></script>
<link href="../../../App_Themes/Tema1/ClueTip/jquery.cluetip.css" type="text/css" rel="stylesheet" media="screen" /> 
<script type="text/javascript">
    // el código javascript    
    Sys.Application.add_load(init);
    function init() {
        $('a.tips').cluetip({
            activation: 'onclick',
            width: '950px',
            sticky: true,
            closePosition: 'title',
            arrows: true,
            closeText: '<img src="../../App_Themes/Tema1/ClueTip/Images/cross.png" alt="Cerrar/Close" style="border:none;" />',
            cluetipClass: 'jtip',
            dropShadow: false,
            clickThrough: false,
            fx: { open: 'fadeIn', openSpeed: '400' }
        });
    };
</script>
    <fieldset style="width:900px">
        <br /><asp:Label ID="labelInfo" runat="server" Text="Se va a visualizar el asiento contable generado para la factura de Eroski" CssClass="labelDetalle"></asp:Label><br /><br />          
        <asp:Table runat="server" ID="tAsientos" CellPadding="0" CellSpacing="0" CssClass="GridView2" Width="900" />        
        <div id="botones"><asp:Button runat="server" ID="btnGenerar" Text="Continuar" /></div>
    </fieldset>          