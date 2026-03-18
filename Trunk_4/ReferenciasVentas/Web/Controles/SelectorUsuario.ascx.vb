
Partial Public Class SelectorUsuario
    Inherits System.Web.UI.UserControl

#Region "Propiedades"

    ''' <summary>
    ''' Obtiene la cadena de conexión
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>        
    Protected ReadOnly Property CadenaConexion As String
        Get
            Dim status As String = "BONOSISTEST"
            Return System.Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
        End Get
    End Property

    ''' <summary>
    ''' Para acceder al cuadro de texto del proyecto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Usuario() As String
        Get
            Return txtUsuario.Text
        End Get
        Set(ByVal value As String)
            txtUsuario.Text = value.Trim
        End Set
    End Property

    ''' <summary>
    ''' Descripcion de la referencia en activo. Guardada en session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdUsuario() As String
        Get
            If Not (String.IsNullOrEmpty(hfUsuario.Value)) Then
                Return hfUsuario.Value
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            hfUsuario.Value = value
        End Set
    End Property

    ''' <summary>
    ''' Evento que se produce al seleccionar una referencia (Al dar al boton aceptar o al salir)
    ''' </summary>
    ''' <remarks></remarks>
    Public Event UsuarioSeleccionado()

    ''' <summary>
    ''' Se produce cuando el componente quiere mandar un mensaje personalizado al usuario.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event ApplicationException(ByVal ex As ApplicationException)

    Public Event Exception(ByVal ex As Exception)

#End Region

#Region "Eventos de Pagina"

    ''' <summary>
    ''' Registra un script para que al salir de la caja de texto se compruebe la referencia y al final, se recargue la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtUsuario.Attributes.Add("onkeypress", "comprobarEnter_" & Me.ID & "();")
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "comprobar_" & Me.ID, "function comprobarReferencia_" & Me.ID & "(){" & Page.ClientScript.GetPostBackEventReference(btnComprobar, Nothing) & "}", True)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "comprobarEnter_" & Me.ID, "function comprobarEnter_" & Me.ID & "() {if (event.keyCode == 13) {comprobarReferencia_" & Me.ID & "();}}", True)
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Boton oculto que comprobará que la referencia sea correcta en dos situaciones:
    '''    -Al perder el foco el texto de la referencia 
    '''    -Al clickear el boton aceptar despues de haber seleccionado una referencia
    ''' </summary>    
    ''' <remarks></remarks>
    Protected Sub ComprobarReferencia(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim myButton As Button = CType(sender, Button)
            If (myButton.CommandName = "A") Then  'Se ha pulsado el boton aceptar
                If (ViewState("IdUsuario") IsNot Nothing AndAlso ViewState("IdUsuario") <> String.Empty) Then
                    IdUsuario = ViewState("IdUsuario")
                    Usuario = ViewState("DescProSel")

                    txtUsuario.Text = Usuario

                    RaiseEvent UsuarioSeleccionado()
                End If
            Else  ' Se ha lanzado este evento, al perder el foco del cuadro de texto de la referencia
                'Usuario = txtUsuario.Text
                'If (txtUsuario.Text <> String.Empty) Then
                '    Dim oUsuariosBLL As New BLL.UsuariosBLL
                '    Dim datosUsuario As ELL.Usuarios

                '    datosUsuario = oUsuariosBLL.c(Usuario.ToLower)

                '    If (datosUsuario IsNot Nothing) Then
                '        IdUsuario = pro.Id
                '        EsValida = True

                '        RaiseEvent UsuarioSeleccionado()
                '    Else
                '        Usuario = String.Empty
                '    End If
                'Else  'Se deja en blanco pero hay que lanzar un evento de referencia no existente
                '    Usuario = String.Empty
                'End If
            End If

            'Se limpian las variables del viewstate de Seleccion de nuevas referencias para que entre distintas iteraciones, no de problemas
            ViewState("IdUsuario") = Nothing

        Catch ex As ApplicationException
            Usuario = String.Empty
            RaiseEvent ApplicationException(ex)
        Catch ex As Exception
            'Para indicar que no existe            
            RaiseEvent Exception(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Al realizarse el onblur, llama a comprobar referencia
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnComprobar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnComprobar.Click
        ComprobarReferencia(sender, e)
    End Sub

#End Region

End Class