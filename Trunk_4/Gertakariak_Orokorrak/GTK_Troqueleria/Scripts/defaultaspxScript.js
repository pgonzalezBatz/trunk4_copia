function pageLoad(sender, args) {
    $(document).ready(function () { $get("<%=pnlFiltro.ClientID%>").parentElement.style.height = "auto"; });
    $find('<%=pce_pnl_Usuarios_Filtro.ClientID%>').add_shown(function () { var obj = $get('<%= txt_Usuarios_Filtro.ClientID%>'); obj.focus(); obj.select(); });
    $find('<%=pce_pnl_Proveedores_Filtro.ClientID%>').add_shown(function () { var obj = $get('<%= txt_Proveedor_Filtro.ClientID%>'); obj.focus(); obj.select(); });
}

/**************************************************************************************************************/
/* Seleccion automatica de subnodos                                                                           */
/**************************************************************************************************************/
function OnTreeClick(evt) {
    var src = window.event != window.undefined ? window.event.srcElement : evt.target;
    var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
    if (isChkBoxClick) {
        var parentTable = GetParentByTagName("table", src);
        var nxtSibling = parentTable.nextSibling;
        if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node
        {
            if (nxtSibling.tagName.toLowerCase() == "div") //if node has children
            {
                //check or uncheck children at all levels
                CheckUncheckChildren(parentTable.nextSibling, src.checked);
            }
        }
        //check or uncheck parents at all levels
        //CheckUncheckParents(src, src.checked);
    }
}
function CheckUncheckChildren(childContainer, check) {
    var childChkBoxes = childContainer.getElementsByTagName("input");
    var childChkBoxCount = childChkBoxes.length;
    for (var i = 0; i < childChkBoxCount; i++) {
        childChkBoxes[i].checked = check;
    }
}
//utility function to get the container of an element by tagname
function GetParentByTagName(parentTagName, childElementObj) {
    var parent = childElementObj.parentNode;
    while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
        parent = parent.parentNode;
    }
    return parent;
}
/**************************************************************************************************************/

/* Responsable NC (Perseguidor) ************************************************************************************/
function Set_Usuarios_Filtro(source, eventArgs) {
    $find('<%=pce_pnl_Usuarios_Filtro.ClientID%>').hidePopup();
    $("#lvUsuarios_Filtro_UL").append('<li><input name="hd_IdUsuarios_Filtro" type="hidden" value="' + eventArgs.get_value() + '">' + $get('<%= txt_Usuarios_Filtro.ClientID%>').value + ' <a href="#hd_IdUsuarios_Filtro" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;"><asp:Image ID="imgBorrar_Usuario_Filtro" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" /></a></li>');
    $get('<%= txt_Usuarios_Filtro.ClientID%>').value = "";
}
/* Proveedores NC **************************************************************************************************/
function Set_Proveedores_Filtro(source, eventArgs) {
    $find('<%=pce_pnl_Proveedores_Filtro.ClientID%>').hidePopup();
    $("#lvProveedores_Filtro_UL").append('<li><input name="hd_IdProveedores_Filtro" type="hidden" value="' + eventArgs.get_value() + '">' + $get('<%= txt_Proveedor_Filtro.ClientID%>').value + ' <a href="#hd_IdProveedores_Filtro" onclick="Borrar_Item(this)" style="display: inline; vertical-align: middle;"><asp:Image ID="imgBorrar_Proveedor_Filtro" runat="server" ImageUrl="~/App_Themes/Batz/IconosAcciones/Eliminar16.png" /></a></li>');
    $get('<%= txt_Proveedor_Filtro.ClientID%>').value = "";
}
/*******************************************************************************************************************/
function Borrar_Item(e) {
    e.parentNode.parentNode.removeChild(e.parentNode);
}
        /*******************************************************************************************************************/