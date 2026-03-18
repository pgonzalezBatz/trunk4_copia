Imports System.Globalization

Public Class _Error1
    Inherits System.Web.UI.Page

    Private itzultzaileWeb As New LocalizationLib.Itzultzaile
    'Private log As log4net.ILog = log4net.LogManager.GetLogger("root.Ktrol")

#Region "Métodos"

    ''' <summary>
    ''' Cultura del navegador
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function Cultura() As CultureInfo
        Dim languages As String() = HttpContext.Current.Request.UserLanguages
        If languages Is Nothing OrElse languages.Length = 0 Then
            Return CultureInfo.CreateSpecificCulture("en-GB")
        End If
        Try
            Dim language As String = languages(0).ToLowerInvariant().Trim()
            Return CultureInfo.CreateSpecificCulture(language)
        Catch generatedExceptionName As ArgumentException
            Return CultureInfo.CreateSpecificCulture("en-GB")
        End Try
    End Function

    ''' <summary>
    ''' Mostrar el mensaje en pantalla dependiendo del error
    ''' </summary>
    ''' <param name="tipoError"></param>
    ''' <param name="usu"></param>
    ''' <remarks></remarks>
    Private Sub TratarError(ByVal tipoError As String, Optional ByVal usu As String = "")        
        Select Case tipoError
            Case "1"
                'Es un error de caducidad de sesión
                lblMensaje.Text = itzultzaileWeb.Itzuli("Ha caducado la sesión. Vuelva a loguearse en la aplicación")
				'Global_asax.log.Debug(vbCrLf & lblMensaje.Text & vbCrLf & String.Format("tipoError: {0} / usu: {1}", tipoError, usu))
			Case "2"
				'Alguna de las variables de Kaplan es nulo y no se puede hacer el control
				If (Not String.IsNullOrEmpty(usu) And Session("CodOperacion") IsNot Nothing) Then
                    Dim caracteristicas As List(Of KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN_FABRICACION)
                    caracteristicas = cargarCodigosOperacion(Session("CodOperacion").ToString(), usu)
                    For Each carac In caracteristicas
                        If (String.IsNullOrEmpty(carac.METODO_CONTROL)) Then
                            lblMensaje.Text = itzultzaileWeb.Itzuli(String.Format("La característica con posición {0} de la referencia {1}, con nombre {2}, no tiene método de control asignado y es obligatorio. Contactad con calidad pra que arreglen el problema.", carac.POSICION, Session("CodOperacion").ToString(), carac.CARAC_PARAM))
                        ElseIf (Not carac.METODO_CONTROL.ToLower().Contains("atributos") AndAlso Not carac.METODO_CONTROL.ToLower().Contains("atr") AndAlso Not carac.METODO_CONTROL.ToLower().Contains("variables") AndAlso Not carac.METODO_CONTROL.ToLower().Contains("var")) Then
                            lblMensaje.Text = itzultzaileWeb.Itzuli(String.Format("La característica con posición {0} de la referencia {1}, con nombre {2}, tiene un método de control diferente a tipo atributo o variable. Contactad con calidad pra que arreglen el problema.", carac.POSICION, Session("CodOperacion").ToString(), carac.CARAC_PARAM))
                        End If
                    Next
                End If
				If (String.IsNullOrEmpty(lblMensaje.Text)) Then
					MensajeDefecto()
				End If
		End Select
    End Sub

	''' <summary>
	''' Mostrar el mensaje de error por defecto
	''' </summary>
	''' <remarks></remarks>
	Private Sub MensajeDefecto()
		lblMensaje.Text = itzultzaileWeb.Itzuli("Ha ocurrido un error que impide continuar con el registro del control.")
		Global_asax.log.Error(vbCrLf & lblMensaje.Text & vbCrLf & String.Format("TipoError(t): {0} / usu(u): {1}", Request.QueryString("t").ToString(), Request.QueryString("u").ToString()))
	End Sub

	''' <summary>
	''' Carga las lineas de las caracteristicas del plan
	''' </summary>
	''' <remarks></remarks>
	Private Function cargarCodigosOperacion(ByVal codigoOperacion As String, ByVal usuario As String) As List(Of KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN_FABRICACION)
        Dim lista As New List(Of KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN_FABRICACION)
        Try
            Dim conexion As New KaPlanLib.DAL.ELL

            If (usuario.Equals("op")) Then
                lista = (From reg As KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN_FABRICACION In conexion.CARACTERISTICAS_DEL_PLAN_FABRICACION
                         Where reg.CODIGO = codigoOperacion And reg.Responsable_Reg_Ope = True And (reg.Responsable_Reg_Gestor Is Nothing OrElse reg.Responsable_Reg_Gestor = False) Order By reg.ORDEN_CARAC Ascending Select reg).ToList()
            ElseIf (usuario.Equals("cal")) Then
                lista = (From reg As KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN_FABRICACION In conexion.CARACTERISTICAS_DEL_PLAN_FABRICACION _
                             Where reg.CODIGO = codigoOperacion And reg.Responsable_Reg_Cal = True Order By reg.ORDEN_CARAC Ascending Select reg).ToList()
            End If
            
        Catch ex As Exception
			Global_asax.log.Error("Error al cargar las caracteristicas en la página de error", ex)
			Throw New BatzException("Error al cargar las caracteristicas", ex)
        End Try
        Return lista
    End Function

#End Region

#Region "Handlers"

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Try
			If Not Page.IsPostBack Then
				If (Session("Ticket") Is Nothing) Then
					'Si no tiene Ticket,se coge del navegador
					System.Threading.Thread.CurrentThread.CurrentCulture = Cultura()
				End If
				If (Request.QueryString("t") IsNot Nothing) Then
					If (Request.QueryString("u") IsNot Nothing) Then
						TratarError(Request.QueryString("t").ToString(), Request.QueryString("u").ToString())
					Else
						TratarError(Request.QueryString("t").ToString())
					End If
				Else
					Global_asax.log.Debug("Falta el tipo de Error (t)")
					Throw New ApplicationException("Ha ocurrido un error que impide continuar con el registro del control.")
				End If
			End If
		Catch ex As ApplicationException
			Master.ascx_Mensajes.MensajeError(ex)
			lblMensaje.Text = ex.Message
		Catch ex As Exception
			Global_asax.log.Error(ex)
			Master.ascx_Mensajes.MensajeError(ex)
			lblMensaje.Text = ex.Message
		End Try
	End Sub

	''' <summary>
	''' Traduccion
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>    
	Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(lnkKtrol) : itzultzaileWeb.Itzuli(imgKtrol)
        End If
    End Sub

    ''' <summary>
    ''' Se redirige a Helpdesk
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub imgCrear_Click(sender As Object, e As ImageClickEventArgs) Handles imgKtrol.Click
        RedirigirKtrol()
    End Sub

    ''' <summary>
    ''' Se redirige a Helpdesk
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub lnkCrear_Click(sender As Object, e As EventArgs) Handles lnkKtrol.Click
        RedirigirKtrol()
    End Sub

    ''' <summary>
    ''' Redirige a Helpdesk
    ''' </summary>    
    Private Sub RedirigirKtrol()
        'Dim url As String = Request.Url.Scheme & "://"
        'url &= If(ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live", "intranet2", "intranet-test")
        'url &= ".batz.es/ktrol"
        Session("PerfilUsuario") = Nothing
        'Response.Redirect(url, False)
        Response.Redirect("~", False)
    End Sub

#End Region

End Class