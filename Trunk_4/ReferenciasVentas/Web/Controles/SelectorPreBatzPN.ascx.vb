
Partial Public Class SelectorPreBatzPN
    Inherits System.Web.UI.UserControl

#Region "Propiedades"

    ''' <summary>
    ''' Referencia en activo. Guardada en session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdReferencia() As String
        Get
            If (ViewState("IdReferencia") Is Nothing) Then
                Return String.Empty
            Else
                Return ViewState("IdReferencia").ToString
            End If
        End Get
        Set(ByVal value As String)
            ViewState("IdReferencia") = value.Trim
        End Set
    End Property

    ''' <summary>
    ''' Indica si la referencia activa es valida. Guardada en session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property EsValida() As Boolean
        Get
            If (ViewState("EsValidaRef") Is Nothing) Then
                Return False
            Else
                Return CType(ViewState("EsValidaRef"), Boolean)
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("EsValidaRef") = value
        End Set
    End Property

    ''' <summary>
    ''' Para acceder al identificador del Batz Part Number elegido
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdPrevBatzPN() As String
        Get
            Return hfPrevBatzPN.Value
        End Get
        Set(ByVal value As String)
            hfPrevBatzPN.Value = value
        End Set
    End Property

    ''' <summary>
    ''' Para acceder al cuadro de texto del proyecto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PrevBatzPN() As String
        Get
            Return txtPrevBatzPN.Text
        End Get
        Set(ByVal value As String)
            txtPrevBatzPN.Text = value.Trim
        End Set
    End Property

    ''' <summary>
    ''' Descripcion de la referencia en activo. Guardada en session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RFV_PrevBatzPN() As Boolean
        Get
            Return rfvPrevBatzPN.Enabled
        End Get
        Set(ByVal value As Boolean)
            rfvPrevBatzPN.Enabled = value
        End Set
    End Property

    ''' <summary>
    ''' Se produce al comprobar que la referencia no existe
    ''' </summary>
    ''' <remarks></remarks>
    <Obsolete> _
    Public Event PrevBatzPnNoExistente()

    ''' <summary>
    ''' Evento que se produce al seleccionar una referencia (Al dar al boton aceptar o al salir)
    ''' </summary>
    ''' <remarks></remarks>
    Public Event PrevBatzPnSeleccionado()

    ''' <summary>
    ''' Se produce cuando el componente quiere mandar un mensaje personalizado al usuario.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event ApplicationException(ByVal ex As ApplicationException)

    Public Event Exception(ByVal ex As Exception)
#End Region

#Region "Eventos de Pagina"

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
    End Sub

    ''' <summary>
    ''' Registra un script para que al salir de la caja de texto se compruebe la referencia y al final, se recargue la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        acePrevBatzPN.OnClientItemSelected = "PrevBatzPNElegido_" + Me.ID
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "PrevBatzPNElegido_" & Me.ID, "function PrevBatzPNElegido_" & Me.ID & "(source, eventArgs){ var hdnValueId = document.getElementById('" & hfPrevBatzPN.ClientID & "'); hdnValueId.value = eventArgs.get_value();}", True)

        txtPrevBatzPN.Attributes.Add("onkeypress", "comprobarEnter_" & Me.ID & "();")
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "comprobar_" & Me.ID, "function comprobarReferencia_" & Me.ID & "(){" & Page.ClientScript.GetPostBackEventReference(btnComprobar, Nothing) & "}", True)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "comprobarEnter_" & Me.ID, "function comprobarEnter_" & Me.ID & "() {if (event.keyCode == 13) {comprobarReferencia_" & Me.ID & "();}}", True)
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Metodo de Refresco del control.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Recargar()
        ComprobarReferencia(btnComprobar, Nothing)
    End Sub

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
                If (ViewState("IdProSel") IsNot Nothing AndAlso ViewState("IdProSel") <> String.Empty) Then
                    PrevBatzPN = ViewState("DescProSel")
                    EsValida = True

                    txtPrevBatzPN.Text = PrevBatzPN
                    RaiseEvent PrevBatzPnSeleccionado()
                End If
            Else  ' Se ha lanzado este evento, al perder el foco del cuadro de texto de la referencia
                PrevBatzPN = txtPrevBatzPN.Text
                If (txtPrevBatzPN.Text <> String.Empty) Then
                    Dim oBrainBLL As New BLL.BrainBLL
                    Dim pro As ELL.BrainBase

                    pro = oBrainBLL.CargarProyectoBrain(PrevBatzPN.ToLower)

                    If (pro IsNot Nothing) Then
                        EsValida = True
                        RaiseEvent PrevBatzPnSeleccionado()
                    Else
                        PrevBatzPN = String.Empty
                        EsValida = False
                    End If
                Else  'Se deja en blanco pero hay que lanzar un evento de referencia no existente
                    PrevBatzPN = String.Empty
                    EsValida = False
                End If
            End If

            'Se limpian las variables del viewstate de Seleccion de nuevas referencias para que entre distintas iteraciones, no de problemas
            ViewState("IdProSel") = Nothing
            ViewState("DescProSel") = Nothing

        Catch ex As ApplicationException
            PrevBatzPN = String.Empty
            EsValida = False
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