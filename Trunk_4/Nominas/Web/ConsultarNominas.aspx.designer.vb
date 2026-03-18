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


Partial Public Class ConsultarNominas
    
    '''<summary>
    '''Control labelMesAno.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelMesAno As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelMes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelMes As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control ddlMes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddlMes As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Control labelAno.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelAno As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control ddlAño.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddlAño As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Control btnBuscar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnBuscar As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control pnlResultados.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlResultados As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control labelTitleNominas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelTitleNominas As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control rptNominas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rptNominas As Global.System.Web.UI.WebControls.Repeater
    
    '''<summary>
    '''Control pnlSinResultados.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlSinResultados As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control pnlOharrakIgorre.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlOharrakIgorre As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control labelTitleOharrak.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelTitleOharrak As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control pnlOtrosDocs.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlOtrosDocs As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control labelTitleDoc.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelTitleDoc As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control rptDocumentos.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rptDocumentos As Global.System.Web.UI.WebControls.Repeater
    
    '''<summary>
    '''Control hfHost.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfHost As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control lblHidden.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblHidden As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control mpePopUp.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents mpePopUp As Global.AjaxControlToolkit.ModalPopupExtender
    
    '''<summary>
    '''Control pnlPassword.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlPassword As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control pnlDNI.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlDNI As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control lblError.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblError As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelDNI.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelDNI As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txtDNI.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtDNI As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control rfvDNI.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfvDNI As Global.System.Web.UI.WebControls.RequiredFieldValidator
    
    '''<summary>
    '''Control vceDNI.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents vceDNI As Global.AjaxControlToolkit.ValidatorCalloutExtender
    
    '''<summary>
    '''Control pnlImpreso.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlImpreso As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control lblImpreso.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblImpreso As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control imgCerrar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents imgCerrar As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''Control imgImprimir.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents imgImprimir As Global.System.Web.UI.WebControls.ImageButton

    '''<summary>
    '''Propiedad Master.
    '''</summary>
    '''<remarks>
    '''Propiedad generada automáticamente.
    '''</remarks>
    Public Shadows ReadOnly Property Master() As Nominas.MPWeb
        Get
            Return CType(MyBase.Master, Nominas.MPWeb)
        End Get
    End Property
End Class
