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


Partial Public Class FacturacionPersona

    '''<summary>
    '''Control Titulo1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents Titulo1 As Global.Telefonia.Titulo

    '''<summary>
    '''Control pnlFiltros.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlFiltros As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control labelAño.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelAño As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control ddlAño.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddlAño As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control labelTipoLlamada.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelTipoLlamada As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control ddlTipoLlamada.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddlTipoLlamada As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control labelImportesEn.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelImportesEn As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control ddlImporte.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddlImporte As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control imgAtras.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents imgAtras As Global.System.Web.UI.WebControls.ImageButton

    '''<summary>
    '''Control rptFacPersonas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rptFacPersonas As Global.System.Web.UI.WebControls.Repeater

    '''<summary>
    '''Control pnlInfoExtensiones.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlInfoExtensiones As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control labelExtenCostes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelExtenCostes As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblExtensionesGrafico.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblExtensionesGrafico As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control GraficoFacturacion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents GraficoFacturacion As Global.Telefonia.msGrafico

    '''<summary>
    '''Control panelUpdateProgress.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents panelUpdateProgress As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control UpdateProg1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents UpdateProg1 As Global.System.Web.UI.UpdateProgress

    '''<summary>
    '''Control ModalProgress.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ModalProgress As Global.AjaxControlToolkit.ModalPopupExtender

    '''<summary>
    '''Propiedad Master.
    '''</summary>
    '''<remarks>
    '''Propiedad generada automáticamente.
    '''</remarks>
    Public Shadows ReadOnly Property Master() As Telefonia.MPTelefoniaAdm
        Get
            Return CType(MyBase.Master, Telefonia.MPTelefoniaAdm)
        End Get
    End Property
End Class
