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


Partial Public Class Liquidacion
    
    '''<summary>
    '''Control temporizador.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents temporizador As Global.System.Web.UI.Timer
    
    '''<summary>
    '''Control pnlInfo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlInfo As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control lblTextoLiquidacion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblTextoLiquidacion As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lnkTipoLiq.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lnkTipoLiq As Global.System.Web.UI.WebControls.LinkButton
    
    '''<summary>
    '''Control pnlFiltro.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlFiltro As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control labelFiltrar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelFiltrar As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txtFechaFiltro.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtFechaFiltro As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control imgCalFFiltro.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents imgCalFFiltro As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''Control ceFF.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ceFF As Global.AjaxControlToolkit.CalendarExtender
    
    '''<summary>
    '''Control chbHGEntregada.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents chbHGEntregada As Global.System.Web.UI.WebControls.CheckBox
    
    '''<summary>
    '''Control btnFiltrar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnFiltrar As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control labelProcesando.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelProcesando As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control btnFiltrarHidden.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnFiltrarHidden As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control pnlInfoIntegracion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlInfoIntegracion As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control labelCompruebe.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelCompruebe As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelFEmision.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelFEmision As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txtFechaEmision.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtFechaEmision As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control imgCalendarioFE.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents imgCalendarioFE As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''Control ceFE.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ceFE As Global.AjaxControlToolkit.CalendarExtender
    
    '''<summary>
    '''Control gvLiquidaciones.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvLiquidaciones As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Control pnlMarca.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlMarca As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control labelInfo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelInfo As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control CheckBoxIDsArray.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents CheckBoxIDsArray As Global.System.Web.UI.WebControls.Literal
    
    '''<summary>
    '''Control pnlBotones.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlBotones As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control btnContinuar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnContinuar As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control btnIntegrar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnIntegrar As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control btnVolver.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnVolver As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control btnIntegrarHidden.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnIntegrarHidden As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control hfHojas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfHojas As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control hfStatePag.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfStatePag As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control hfConnectionId.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfConnectionId As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Propiedad Master.
    '''</summary>
    '''<remarks>
    '''Propiedad generada automáticamente.
    '''</remarks>
    Public Shadows ReadOnly Property Master() As WebRaiz.MPWeb
        Get
            Return CType(MyBase.Master,WebRaiz.MPWeb)
        End Get
    End Property
End Class
