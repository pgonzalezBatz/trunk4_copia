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


Partial Public Class Departamentos
    
    '''<summary>
    '''Control ValidationSummary.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ValidationSummary As Global.System.Web.UI.WebControls.ValidationSummary
    
    '''<summary>
    '''Control mvDeptos.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents mvDeptos As Global.System.Web.UI.WebControls.MultiView
    
    '''<summary>
    '''Control vwListado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents vwListado As Global.System.Web.UI.WebControls.View
    
    '''<summary>
    '''Control pnlDeptoProgNominas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlDeptoProgNominas As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control lblMensaProgNominas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblMensaProgNominas As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control pnlDeptoKEM.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlDeptoKEM As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control labelTitulo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelTitulo As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lnkCrearDepto.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lnkCrearDepto As Global.System.Web.UI.WebControls.LinkButton
    
    '''<summary>
    '''Control gvDeptos.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvDeptos As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Control vwDetalle.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents vwDetalle As Global.System.Web.UI.WebControls.View
    
    '''<summary>
    '''Control lblCabecera.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblCabecera As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control pnlExistente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlExistente As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control labelId.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelId As Global.System.Web.UI.WebControls.Literal
    
    '''<summary>
    '''Control lblIdDepto.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblIdDepto As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelNombre.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelNombre As Global.System.Web.UI.WebControls.Literal
    
    '''<summary>
    '''Control txtNombre.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtNombre As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control btnSave.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnSave As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control btnCancel.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnCancel As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control rfvNombre.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfvNombre As Global.System.Web.UI.WebControls.RequiredFieldValidator
    
    '''<summary>
    '''Propiedad Master.
    '''</summary>
    '''<remarks>
    '''Propiedad generada automáticamente.
    '''</remarks>
    Public Shadows ReadOnly Property Master() As KEM.MPKEM
        Get
            Return CType(MyBase.Master,KEM.MPKEM)
        End Get
    End Property
End Class
