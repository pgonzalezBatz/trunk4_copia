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


Partial Public Class ProcesarLiquidaciones
    
    '''<summary>
    '''Control WLiquidaciones.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents WLiquidaciones As Global.System.Web.UI.WebControls.Wizard
    
    '''<summary>
    '''Control wStep1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents wStep1 As Global.System.Web.UI.WebControls.WizardStep
    
    '''<summary>
    '''Control ucListado_Step1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ucListado_Step1 As Global.WebRaiz.ListadoLiq
    
    '''<summary>
    '''Control wStep2.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents wStep2 As Global.System.Web.UI.WebControls.WizardStep
    
    '''<summary>
    '''Control ucResumen_Step2.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ucResumen_Step2 As Global.WebRaiz.ResumenLiq
    
    '''<summary>
    '''Control wStep3.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents wStep3 As Global.System.Web.UI.WebControls.WizardStep
    
    '''<summary>
    '''Control ucProcesando_Step3.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ucProcesando_Step3 As Global.WebRaiz.ProcesandoLiq
    
    '''<summary>
    '''Control wStep4.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents wStep4 As Global.System.Web.UI.WebControls.WizardStep
    
    '''<summary>
    '''Control ucResul_Step4.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ucResul_Step4 As Global.WebRaiz.ResultadoLiq
    
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
