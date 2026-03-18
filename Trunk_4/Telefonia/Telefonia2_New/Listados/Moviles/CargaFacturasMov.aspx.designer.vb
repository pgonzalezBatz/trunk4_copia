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


Partial Public Class CargaFacturasMov

    '''<summary>
    '''Control mvCarga.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents mvCarga As Global.System.Web.UI.WebControls.MultiView

    '''<summary>
    '''Control vSubirFich.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents vSubirFich As Global.System.Web.UI.WebControls.View

    '''<summary>
    '''Control labelSelFichero.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelSelFichero As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control fuFichero.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fuFichero As Global.System.Web.UI.WebControls.FileUpload

    '''<summary>
    '''Control labelTamano.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelTamano As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control btnSubir.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnSubir As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Control lblNoSubir.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblNoSubir As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control btnQuitarBloqueo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnQuitarBloqueo As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Control vImportar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents vImportar As Global.System.Web.UI.WebControls.View

    '''<summary>
    '''Control labelFich.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelFich As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblFichero.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblFichero As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control labelSubidoOK.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelSubidoOK As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control labelResumen.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelResumen As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control gvFacturas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvFacturas As Global.System.Web.UI.WebControls.GridView

    '''<summary>
    '''Control pnlImportar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlImportar As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control btnImportar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnImportar As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Control lblMensa.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblMensa As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control pnlNoImportar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlNoImportar As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control labelAsociarCIF.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelAsociarCIF As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control mvResumen.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents mvResumen As Global.System.Web.UI.WebControls.View

    '''<summary>
    '''Control labelFFactura.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelFFactura As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblFechaFactura.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblFechaFactura As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control labelTotalFactura.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelTotalFactura As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblTotalFactura.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblTotalFactura As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control labelTotalFacturado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelTotalFacturado As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblTotalFacturado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblTotalFacturado As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control labelRegInsert.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelRegInsert As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblRegistrosInsertados.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblRegistrosInsertados As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control labelInfo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelInfo As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control btnRefrescar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnRefrescar As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Control lblUltimaEjecucion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblUltimaEjecucion As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control pnlResulRefresco.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlResulRefresco As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control lblResulRefresco.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblResulRefresco As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control pnlError.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlError As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Propiedad Master.
    '''</summary>
    '''<remarks>
    '''Propiedad generada automáticamente.
    '''</remarks>
    Public Shadows ReadOnly Property Master() As Telefonia.MPTelefonia
        Get
            Return CType(MyBase.Master, Telefonia.MPTelefonia)
        End Get
    End Property
End Class
