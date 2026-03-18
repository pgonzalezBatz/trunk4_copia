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


Partial Public Class ProcesarFacturas

    '''<summary>
    '''Control wFacturas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents wFacturas As Global.System.Web.UI.WebControls.Wizard

    '''<summary>
    '''Control wStep0.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents wStep0 As Global.System.Web.UI.WebControls.WizardStep

    '''<summary>
    '''Control ucChequearEjecucion_Step0.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ucChequearEjecucion_Step0 As Global.WebRaiz.ChequearEjecucion_Fact

    '''<summary>
    '''Control wStep1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents wStep1 As Global.System.Web.UI.WebControls.WizardStep

    '''<summary>
    '''Control ucSubirFichero_Step1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ucSubirFichero_Step1 As Global.WebRaiz.SubirFichero_Fact

    '''<summary>
    '''Control wStep2.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents wStep2 As Global.System.Web.UI.WebControls.WizardStep

    '''<summary>
    '''Control ucImportarAlbaranesTemp_Step2.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ucImportarAlbaranesTemp_Step2 As Global.WebRaiz.ImportarAlbaranesTemp_Fact

    '''<summary>
    '''Control wStep3.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents wStep3 As Global.System.Web.UI.WebControls.WizardStep

    '''<summary>
    '''Control ucResumenImportacionTemp_Step3.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ucResumenImportacionTemp_Step3 As Global.WebRaiz.ResumenImportacionTemp_Fact

    '''<summary>
    '''Control wStep4.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents wStep4 As Global.System.Web.UI.WebControls.WizardStep

    '''<summary>
    '''Control ucVisualizarAsientosContables_Step4.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ucVisualizarAsientosContables_Step4 As Global.WebRaiz.VisualizarAsientosContables_Fact

    '''<summary>
    '''Control wStep5.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents wStep5 As Global.System.Web.UI.WebControls.WizardStep

    '''<summary>
    '''Control ucFinalizarImportacion_Step5.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ucFinalizarImportacion_Step5 As Global.WebRaiz.FinalizarImportacion_Fact

    '''<summary>
    '''Control wStep6.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents wStep6 As Global.System.Web.UI.WebControls.WizardStep

    '''<summary>
    '''Control ucProcesando_Step6.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ucProcesando_Step6 As Global.WebRaiz.Procesando_Fact

    '''<summary>
    '''Control wStep7.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents wStep7 As Global.System.Web.UI.WebControls.WizardStep

    '''<summary>
    '''Control ucResultado_Step7.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ucResultado_Step7 As Global.WebRaiz.Resultado_Fact

    '''<summary>
    '''Propiedad Master.
    '''</summary>
    '''<remarks>
    '''Propiedad generada automáticamente.
    '''</remarks>
    Public Shadows ReadOnly Property Master() As WebRaiz.Master
        Get
            Return CType(MyBase.Master, WebRaiz.Master)
        End Get
    End Property
End Class
