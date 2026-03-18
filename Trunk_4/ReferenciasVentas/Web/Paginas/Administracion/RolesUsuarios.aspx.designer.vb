'------------------------------------------------------------------------------
' <generado automáticamente>
'     Este código fue generado por una herramienta.
'
'     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
'     se vuelve a generar el código. 
' </generado automáticamente>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class RolesUsuarios
    
    '''<summary>
    '''Control titProducto.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents titProducto As Global.ReferenciasVentas.Titulo
    
    '''<summary>
    '''Control upUsersRoles.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents upUsersRoles As Global.System.Web.UI.UpdatePanel
    
    '''<summary>
    '''Control lblAdvertencia.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblAdvertencia As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblRol.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblRol As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control ddlRol.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddlRol As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Control gvRolUsuario.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvRolUsuario As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Control gvRolUsuarioExcepcion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvRolUsuarioExcepcion As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Propiedad Master.
    '''</summary>
    '''<remarks>
    '''Propiedad generada automáticamente.
    '''</remarks>
    Public Shadows ReadOnly Property Master() As ReferenciasVentas.Site
        Get
            Return CType(MyBase.Master,ReferenciasVentas.Site)
        End Get
    End Property
End Class
