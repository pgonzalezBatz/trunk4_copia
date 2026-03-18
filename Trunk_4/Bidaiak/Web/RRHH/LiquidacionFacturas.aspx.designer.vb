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


Partial Public Class LiquidacionFacturas
    
    '''<summary>
    '''Control temporizador.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents temporizador As Global.System.Web.UI.Timer
    
    '''<summary>
    '''Control pnlLiquidaciones.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlLiquidaciones As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control labelTitle.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelTitle As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txtSearchHG.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtSearchHG As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control btnSearch.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnSearch As Global.System.Web.UI.HtmlControls.HtmlButton
    
    '''<summary>
    '''Control btnViewLiqPendientes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnViewLiqPendientes As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control btnViewLiqEmitidas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnViewLiqEmitidas As Global.System.Web.UI.WebControls.Button
    
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
    '''Control pnlLiqEmitidas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlLiqEmitidas As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control labelSelPlanta.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelSelPlanta As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control ddlPlantaEmpresa.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddlPlantaEmpresa As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Control labelSelFactura.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelSelFactura As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control ddlFactura.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddlFactura As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Control gvLiquidaciones.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvLiquidaciones As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Control CheckBoxIDsArray.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents CheckBoxIDsArray As Global.System.Web.UI.WebControls.Literal
    
    '''<summary>
    '''Control pnlPlantaFact.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlPlantaFact As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control labelPlantFact.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelPlantFact As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control ddlPlantFact.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddlPlantFact As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Control labelO.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelO As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelOtrasEmpresas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelOtrasEmpresas As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control ddlOtrasEmpresas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddlOtrasEmpresas As Global.System.Web.UI.WebControls.DropDownList
    
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
    '''Control btnTransferir.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnTransferir As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control btnVolver.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnVolver As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control pnlSearch.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlSearch As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control gvSearch.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvSearch As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Control pnlMensaje.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlMensaje As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control labelMensaje.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelMensaje As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control btnVolverLiq.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnVolverLiq As Global.System.Web.UI.WebControls.Button
    
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
