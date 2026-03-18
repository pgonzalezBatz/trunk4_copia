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


Partial Public Class GestionAnticipos
    
    '''<summary>
    '''Control labelTitle.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelTitle As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txtIdViajeF.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtIdViajeF As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control ftbIdViajeF.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ftbIdViajeF As Global.AjaxControlToolkit.FilteredTextBoxExtender
    
    '''<summary>
    '''Control searchUserF.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents searchUserF As Global.WebRaiz.BusquedaUsuarios
    
    '''<summary>
    '''Control btnSearchF.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnSearchF As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control btnResetF.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnResetF As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control pnlAnticiposCancelados.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlAnticiposCancelados As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control labelAnticCancel.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelAnticCancel As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control rptAnticCancel.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rptAnticCancel As Global.System.Web.UI.WebControls.Repeater
    
    '''<summary>
    '''Control tabPaneles.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents tabPaneles As Global.AjaxControlToolkit.TabContainer
    
    '''<summary>
    '''Control tabP1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents tabP1 As Global.AjaxControlToolkit.TabPanel
    
    '''<summary>
    '''Control gvaSolicitados.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvaSolicitados As Global.WebRaiz.GridViewAnticipo
    
    '''<summary>
    '''Control tabP2.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents tabP2 As Global.AjaxControlToolkit.TabPanel
    
    '''<summary>
    '''Control gvaPreparados.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvaPreparados As Global.WebRaiz.GridViewAnticipo
    
    '''<summary>
    '''Control tabP3.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents tabP3 As Global.AjaxControlToolkit.TabPanel
    
    '''<summary>
    '''Control gvaEntregados.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvaEntregados As Global.WebRaiz.GridViewAnticipo
    
    '''<summary>
    '''Control tabP4.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents tabP4 As Global.AjaxControlToolkit.TabPanel
    
    '''<summary>
    '''Control labelResulCerradoSinFilter.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelResulCerradoSinFilter As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control gvaCerrados.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvaCerrados As Global.WebRaiz.GridViewAnticipo
    
    '''<summary>
    '''Propiedad Master.
    '''</summary>
    '''<remarks>
    '''Propiedad generada automáticamente.
    '''</remarks>
    Public Shadows ReadOnly Property Master() As WebRaiz.Master
        Get
            Return CType(MyBase.Master,WebRaiz.Master)
        End Get
    End Property
End Class
