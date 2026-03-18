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


Partial Public Class PresupServiciosNew
    
    '''<summary>
    '''Control temporizador.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents temporizador As Global.System.Web.UI.Timer
    
    '''<summary>
    '''Control labelDivInfo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelDivInfo As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelViaje.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelViaje As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblIdViaje.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblIdViaje As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelFechas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelFechas As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblFIda.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblFIda As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblFVuelta.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblFVuelta As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelSolicitante.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelSolicitante As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblSolicitante.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblSolicitante As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelRespVal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelRespVal As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblRespVal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblRespVal As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control hfRespVal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfRespVal As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control labelNivel.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelNivel As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblNivel.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblNivel As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control gvIntegrantes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvIntegrantes As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Control hfNumInt.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfNumInt As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control labelEstado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelEstado As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblEstado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblEstado As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control hfEstado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfEstado As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control labelRespondidoPor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelRespondidoPor As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblUserRespuesta.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblUserRespuesta As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelDivObserv.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelDivObserv As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txtObservaciones.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtObservaciones As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control btnCrearPresupTop.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnCrearPresupTop As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control pnlBotonesTop.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlBotonesTop As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control btnGuardarTop.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnGuardarTop As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control btnEnviarTop.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnEnviarTop As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control btnPrevisualizarTop.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnPrevisualizarTop As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control btnVolverTop.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnVolverTop As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control labelObjetivoTotal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelObjetivoTotal As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblObjTotal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblObjTotal As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control hfObjetivo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfObjetivo As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control labelPresupTotal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelPresupTotal As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblTotal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblTotal As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control hfReal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfReal As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control divAvion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents divAvion As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Control labelDivServAereo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelDivServAereo As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblAOrigenTarifa.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblAOrigenTarifa As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelATarifaObjCab.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelATarifaObjCab As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelATarifaRealCab.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelATarifaRealCab As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelATarifaPerso.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelATarifaPerso As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblATarifaObjPerso.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblATarifaObjPerso As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txtATarifaRealPerso.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtATarifaRealPerso As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control rfvATarifaRealPerso.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfvATarifaRealPerso As Global.System.Web.UI.WebControls.RequiredFieldValidator
    
    '''<summary>
    '''Control ftbATarifaRealPerso.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ftbATarifaRealPerso As Global.AjaxControlToolkit.FilteredTextBoxExtender
    
    '''<summary>
    '''Control labelATarifaTotal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelATarifaTotal As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblATarifaObjTotal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblATarifaObjTotal As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblATarifaRealTotal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblATarifaRealTotal As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control divHotel.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents divHotel As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Control labelDivHotel.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelDivHotel As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelHDias.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelHDias As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txtHNumDias.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtHNumDias As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control rfvHNumDias.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfvHNumDias As Global.System.Web.UI.WebControls.RequiredFieldValidator
    
    '''<summary>
    '''Control ftbeHNumDias.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ftbeHNumDias As Global.AjaxControlToolkit.FilteredTextBoxExtender
    
    '''<summary>
    '''Control lblHOrigenTarifa.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblHOrigenTarifa As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelHTarifaObjCab.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelHTarifaObjCab As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelHTarifaRealCab.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelHTarifaRealCab As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelHTarifaDiaPerso.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelHTarifaDiaPerso As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblHTarifaObjDiaPerso.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblHTarifaObjDiaPerso As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txtHTarifaRealDiaPerso.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtHTarifaRealDiaPerso As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control ftbHTarifaRealDiaPerso.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ftbHTarifaRealDiaPerso As Global.AjaxControlToolkit.FilteredTextBoxExtender
    
    '''<summary>
    '''Control labelHTarifaTotal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelHTarifaTotal As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblHTarifaObjTotal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblHTarifaObjTotal As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblHTarifaRealTotal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblHTarifaRealTotal As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control divCocheAlq.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents divCocheAlq As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    
    '''<summary>
    '''Control labelDivCoche.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelDivCoche As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelCNumDias.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelCNumDias As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txtCNumDias.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtCNumDias As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control rfvCNumDias.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rfvCNumDias As Global.System.Web.UI.WebControls.RequiredFieldValidator
    
    '''<summary>
    '''Control ftbeCNumDias.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ftbeCNumDias As Global.AjaxControlToolkit.FilteredTextBoxExtender
    
    '''<summary>
    '''Control lblCOrigenTarifa.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblCOrigenTarifa As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelCTarifaObjCab.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelCTarifaObjCab As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelCTarifaRealCab.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelCTarifaRealCab As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelCTarifaObjDia.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelCTarifaObjDia As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblCTarifaObjDia.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblCTarifaObjDia As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control txtCTarifaRealDia.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtCTarifaRealDia As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control ftbeCTarifaRealDia.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ftbeCTarifaRealDia As Global.AjaxControlToolkit.FilteredTextBoxExtender
    
    '''<summary>
    '''Control labelCTarifaTotal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelCTarifaTotal As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblCTarifaObjTotal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblCTarifaObjTotal As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblCTarifaRealTotal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblCTarifaRealTotal As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control btnCrearPresupBottom.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnCrearPresupBottom As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control pnlBotonesBottom.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlBotonesBottom As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control btnGuardarBottom.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnGuardarBottom As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control btnEnviarBottom.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnEnviarBottom As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control btnPrevisualizarBottom.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnPrevisualizarBottom As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control btnVolverBottom.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnVolverBottom As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control labelTitleModalDelete.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelTitleModalDelete As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control hfModalAction.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfModalAction As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control labelConfirmMessageModal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelConfirmMessageModal As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control hfModalParam.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfModalParam As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control btnAceptarModalDel.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnAceptarModalDel As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control labelCancelarModalDel.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelCancelarModalDel As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelTitleConfirmEnvio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelTitleConfirmEnvio As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control labelModalEnvioMessage.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelModalEnvioMessage As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control btnModalEnviarPresup.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnModalEnviarPresup As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control labelModalCancelEnviarPresup.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents labelModalCancelEnviarPresup As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control PanelCargandoDatos.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents PanelCargandoDatos As Global.WebRaiz.PanelCargandoDatos
    
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
