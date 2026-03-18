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


Partial Public Class TramitarSolicitudes
    
    '''<summary>
    '''Control upSolicitudes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents upSolicitudes As Global.System.Web.UI.UpdatePanel
    
    '''<summary>
    '''Control pnlTramitarSolicitudes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlTramitarSolicitudes As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control titTramitarSolicitudes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents titTramitarSolicitudes As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblTipoSolicitud.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblTipoSolicitud As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control ddlTipoSolicitud.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddlTipoSolicitud As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Control lblSolicitud.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblSolicitud As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control ddlSolicitud.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddlSolicitud As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Control divSolicitudAprobada.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents divSolicitudAprobada As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Control lblIntegraciónRealizada.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblIntegraciónRealizada As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control btnAprobarSolicitud.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnAprobarSolicitud As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control cbeValidar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cbeValidar As Global.AjaxControlToolkit.ConfirmButtonExtender
    
    '''<summary>
    '''Control mpeValidar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents mpeValidar As Global.AjaxControlToolkit.ModalPopupExtender
    
    '''<summary>
    '''Control pnlConfirmarAprobacion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlConfirmarAprobacion As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control lblConfirmacion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblConfirmacion As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblConfirmar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblConfirmar As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control btnSi.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnSi As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control btnNo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnNo As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control lblApplicant.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblApplicant As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txtApplicant.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtApplicant As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblAppDate.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblAppDate As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txtApplDate.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtApplDate As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblValidator.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblValidator As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txtValidator.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtValidator As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control dlReferencia.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents dlReferencia As Global.System.Web.UI.WebControls.DataList
    
    '''<summary>
    '''Control btnRechazarSolicitud.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnRechazarSolicitud As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control pnlSinSolicitudes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlSinSolicitudes As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control lblNoRecord.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblNoRecord As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control btnMP_Open.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnMP_Open As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control mpe_RechazarSolicitud.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents mpe_RechazarSolicitud As Global.AjaxControlToolkit.ModalPopupExtender
    
    '''<summary>
    '''Control hfIdSolicitud.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfIdSolicitud As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control pnlRechazarIncidencia.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlRechazarIncidencia As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control imgCerrar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents imgCerrar As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''Control lblMotivoRechazo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblMotivoRechazo As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txtMotivoRechazo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtMotivoRechazo As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control rfvMotivoRechazo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfvMotivoRechazo As Global.System.Web.UI.WebControls.RequiredFieldValidator
    
    '''<summary>
    '''Control vceMotivoRechazo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents vceMotivoRechazo As Global.AjaxControlToolkit.ValidatorCalloutExtender
    
    '''<summary>
    '''Control btnConfirmarRechazo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnConfirmarRechazo As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control PanelCargandoDatos1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents PanelCargandoDatos1 As Global.ReferenciasVentas.PanelCargandoDatos
    
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
