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


Partial Public Class DiferenciaInventario
    
    '''<summary>
    '''Control btnConsultar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnConsultar As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control btnInventarioManual.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnInventarioManual As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control btnCargarDatosArchivo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnCargarDatosArchivo As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control FileUpload1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents FileUpload1 As Global.System.Web.UI.WebControls.FileUpload
    
    '''<summary>
    '''Control importarABD.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents importarABD As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control filtroTabla.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents filtroTabla As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control fechaFiltro.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fechaFiltro As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control fechaFiltroData.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fechaFiltroData As Global.System.Web.UI.HtmlControls.HtmlInputText
    
    '''<summary>
    '''Control btnFiltro.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnFiltro As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control btnFiltroOff.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnFiltroOff As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control pnlDiferenciaInventario.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlDiferenciaInventario As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control titleBD.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents titleBD As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control titleFile.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents titleFile As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control grdDiferenciaInventario.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents grdDiferenciaInventario As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Control grdImportarListado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents grdImportarListado As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Propiedad Master.
    '''</summary>
    '''<remarks>
    '''Propiedad generada automáticamente.
    '''</remarks>
    Public Shadows ReadOnly Property Master() As CostesReales.MPCR
        Get
            Return CType(MyBase.Master,CostesReales.MPCR)
        End Get
    End Property
End Class
