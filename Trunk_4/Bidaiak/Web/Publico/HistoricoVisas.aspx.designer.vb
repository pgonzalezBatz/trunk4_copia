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


Partial Public Class HistoricoVisas
    
    '''<summary>
    '''Control temporizador.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents temporizador As Global.System.Web.UI.Timer
    
    '''<summary>
    '''Control labelTitle.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelTitle As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control rbtPorFechas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rbtPorFechas As Global.System.Web.UI.WebControls.RadioButton
    
    '''<summary>
    '''Control ddlMes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddlMes As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Control ddlAño.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddlAño As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Control rbtVerTodos.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rbtVerTodos As Global.System.Web.UI.WebControls.RadioButton
    
    '''<summary>
    '''Control rbtSinJustificar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rbtSinJustificar As Global.System.Web.UI.WebControls.RadioButton
    
    '''<summary>
    '''Control btnSearchF.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnSearchF As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control pnlJustificar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlJustificar As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control btnJustificar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnJustificar As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control labelInfo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelInfo As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control pnlJust1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlJust1 As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control btnGuardar1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnGuardar1 As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control btnCancelar1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnCancelar1 As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control gvVisa.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvVisa As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Control pnlAvisarJustif.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlAvisarJustif As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control btnAvisarJustif.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnAvisarJustif As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control hfOpcionSel.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfOpcionSel As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control pnlJust2.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlJust2 As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control btnGuardar2.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnGuardar2 As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control btnCancelar2.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnCancelar2 As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control labelTitleModal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelTitleModal As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelTarjetaM.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelTarjetaM As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblTarjetaM.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblTarjetaM As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelFechaM.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelFechaM As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblFechaM.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblFechaM As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelViajeHojaM.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelViajeHojaM As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblViajeHojaM.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblViajeHojaM As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelLocalidad.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelLocalidad As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblLocalidadM.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblLocalidadM As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelSectorM.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelSectorM As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblSectorM.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblSectorM As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelEstablecimientoM.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelEstablecimientoM As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblEstablecimientoM.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblEstablecimientoM As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelImporteMonGastoM.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelImporteMonGastoM As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblImporteMonGastoM.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblImporteMonGastoM As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelImporteEurM.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelImporteEurM As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblImporteEurM.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblImporteEurM As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelEstadoM.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelEstadoM As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblEstadoM.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblEstadoM As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelObservacionesM.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelObservacionesM As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txtObservacionesM.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtObservacionesM As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control btnJustificarModal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnJustificarModal As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control labelCancelarModal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelCancelarModal As Global.System.Web.UI.WebControls.Label
    
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
