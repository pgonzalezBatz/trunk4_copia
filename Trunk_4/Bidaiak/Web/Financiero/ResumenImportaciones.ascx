<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ResumenImportaciones.ascx.vb" Inherits="WebRaiz.ResumenImportaciones" %>
<script type="text/javascript">
    //Se genera un evento para que se lance el evento del boton lanzar
    function subirFichero(anno, mes) {
        document.getElementById('<%=hfAnoMes.ClientID %>').value = anno + '_' + mes;
        var btnLanzar = document.getElementById('<%=btnLanzar.ClientID%>');
        btnLanzar.click();
    }
</script>
<div class="row">
    <div class="col-sm-3 form-inline">
        <asp:Label runat="server" ID="labelSel" Text="Seleccione un año"></asp:Label>
        <asp:DropDownList runat="server" ID="ddlAños" AutoPostBack="true" CssClass="form-control forceInline"></asp:DropDownList>            
    </div>
    <div class="col-sm-4 col-sm-offset-1"><asp:LinkButton runat="server" ID="lnkPresupuestosFacturas" Text="Justificacion de presupuestos-facturas" CssClass="form-control"></asp:LinkButton></div>
</div>
<div runat="server" id="divPendientes" class="alert alert-warning">
    <b><asp:Label runat="server" ID="lblImportacionesPend"></asp:Label></b>
</div>
<asp:Table runat="server" ID="tImp" CssClass="table table-striped"></asp:Table>
<asp:Button runat="server" ID="btnLanzar" style="display:none;" />
<asp:HiddenField runat="server" ID="hfAnoMes" />