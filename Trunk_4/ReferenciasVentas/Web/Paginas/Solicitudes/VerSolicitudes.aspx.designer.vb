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


Partial Public Class VerSolicitudes

    '''<summary>
    '''Control lblEstadoSolicitud.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblEstadoSolicitud As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control ddlEstadoSolicitud.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddlEstadoSolicitud As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control pnlSolicitudesPendientes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlSolicitudesPendientes As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control dlSolicitudes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents dlSolicitudes As Global.System.Web.UI.WebControls.DataList

    '''<summary>
    '''Control pnlSolicitudasTramitadas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlSolicitudasTramitadas As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control Label1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents Label1 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control pnlCabeceraFiltrado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlCabeceraFiltrado As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control imgCollapseFiltrado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents imgCollapseFiltrado As Global.System.Web.UI.WebControls.Image

    '''<summary>
    '''Control lblFiltradoSolicitudes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblFiltradoSolicitudes As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control cpeDatosSolicitudes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cpeDatosSolicitudes As Global.AjaxControlToolkit.CollapsiblePanelExtender

    '''<summary>
    '''Control pnlFiltrado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlFiltrado As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control lblIdentificador.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblIdentificador As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtIdFiltrado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtIdFiltrado As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control lblAprobado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblAprobado As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control rblAprobadoFiltrado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rblAprobadoFiltrado As Global.System.Web.UI.WebControls.RadioButtonList

    '''<summary>
    '''Control lblUsuarioFiltrado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblUsuarioFiltrado As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtUsuarioFiltrado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtUsuarioFiltrado As Global.ReferenciasVentas.SelectorUsuario

    '''<summary>
    '''Control lblFechaCreacion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblFechaCreacion As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblFechaCreacionDesde.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblFechaCreacionDesde As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtFechaCreacionDesde.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtFechaCreacionDesde As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control imgCalendarioCreacionDesde.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents imgCalendarioCreacionDesde As Global.System.Web.UI.WebControls.ImageButton

    '''<summary>
    '''Control imgCalendarioCreacionDesde_CalendarExtender.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents imgCalendarioCreacionDesde_CalendarExtender As Global.AjaxControlToolkit.CalendarExtender

    '''<summary>
    '''Control lblFechaResolucion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblFechaResolucion As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblFechaResolucionDesde.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblFechaResolucionDesde As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtFechaResolucionDesde.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtFechaResolucionDesde As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control imgCalendarioResolucionDesde.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents imgCalendarioResolucionDesde As Global.System.Web.UI.WebControls.ImageButton

    '''<summary>
    '''Control imgCalendarioResolucionDesde_CalendarExtender.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents imgCalendarioResolucionDesde_CalendarExtender As Global.AjaxControlToolkit.CalendarExtender

    '''<summary>
    '''Control lblFechaCreacionHasta.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblFechaCreacionHasta As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtFechaCreacionHasta.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtFechaCreacionHasta As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control imgCalendarioCreacionHasta.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents imgCalendarioCreacionHasta As Global.System.Web.UI.WebControls.ImageButton

    '''<summary>
    '''Control imgCalendarioCreacionHasta_CalendarExtender.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents imgCalendarioCreacionHasta_CalendarExtender As Global.AjaxControlToolkit.CalendarExtender

    '''<summary>
    '''Control lblFechaResolucionHasta.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblFechaResolucionHasta As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtFechaResolucionHasta.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtFechaResolucionHasta As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control imgCalendarioResolucionHasta.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents imgCalendarioResolucionHasta As Global.System.Web.UI.WebControls.ImageButton

    '''<summary>
    '''Control imgCalendarioResolucionHasta_CalendarExtender.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents imgCalendarioResolucionHasta_CalendarExtender As Global.AjaxControlToolkit.CalendarExtender

    '''<summary>
    '''Control lnkbLimpiarCampos.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lnkbLimpiarCampos As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Control lnkbFiltrado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lnkbFiltrado As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Control gvSolicitudesTramitadas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvSolicitudesTramitadas As Global.System.Web.UI.WebControls.GridView

    '''<summary>
    '''Propiedad Master.
    '''</summary>
    '''<remarks>
    '''Propiedad generada automáticamente.
    '''</remarks>
    Public Shadows ReadOnly Property Master() As ReferenciasVentas.Site
        Get
            Return CType(MyBase.Master, ReferenciasVentas.Site)
        End Get
    End Property
End Class
