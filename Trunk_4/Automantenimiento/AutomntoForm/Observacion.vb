Public Class frmObservacion

    Protected WithEvents formKeyboard As ReducedKeyBoardForm
    Private FocusOwner As String = String.Empty
    Private callForm As Form = Nothing
    Private varResulName As String = String.Empty
    Private eventControlName As String = String.Empty

#Region "Carga del formulario"

    ''' <summary>
    ''' Constructor por defecto
    ''' </summary>    
    Public Sub New()
        InitializeComponent()
    End Sub

    ''' <summary>
    ''' Longitud maxima del cuadro de texto
    ''' </summary>
    ''' <param name="pMaxLength">Longitud del cuadro de texto</param>
    Public Sub New(ByVal pForm As Form, ByVal pVarResulName As String, ByVal pEventControlName As String, ByVal pMaxLength As Integer, ByVal Titulo As String, ByVal texto As String)
        InitializeComponent()
        callForm = pForm
        varResulName = pVarResulName
        eventControlName = pEventControlName
        lblPunto.Text = Titulo.ToUpper
        txtObservacion.MaxLength = pMaxLength
        txtObservacion.Text = texto
        MostrarTecladoVirtual()
        formKeyboard = New ReducedKeyBoardForm
        formKeyboard.TopLevel = False
        formKeyboard.AutoSize = True
        formKeyboard.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.pnlKeyboard.Controls.Add(formKeyboard)
        formKeyboard.Show()
        FocusOwner = String.Empty
        pnlKeyboard.Visible = False
    End Sub

    ''' <summary>
    ''' Al cargar, se evita que se seleccione el texto
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub frmObservacion_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        ConfigurarVentana()
        txtObservacion.SelectionStart = txtObservacion.Text.Length
        txtObservacion.DeselectAll()
    End Sub

    ''' <summary>
    ''' Configura los parametros de la ventana
    ''' </summary>    
    Private Sub ConfigurarVentana()
        Me.TopMost = True  'Para que sea modal
        Me.TopLevel = True
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.Width = My.Computer.Screen.Bounds.Width
        Me.Height = My.Computer.Screen.Bounds.Height
        'Situa la ventana en el punto 0,0
        Me.Location = New Drawing.Point(0, 0)
        'Se centra el contenido del formulario
        pnlContenido.Left = (Me.Width - pnlContenido.Width) / 2
        pnlContenido.Top = (Me.Height - pnlContenido.Height) / 6
    End Sub

    ''' <summary>
    ''' Muestra el teclado virtual
    ''' </summary>
    Protected Sub MostrarTecladoVirtual()
        pnlKeyboard.Visible = True
        FocusOwner = txtObservacion.Name
    End Sub

    ''' <summary>
    ''' Oculta el teclado virtual
    ''' </summary>
    Protected Sub OcultarTecladoVirtual()
        pnlKeyboard.Visible = False
        FocusOwner = String.Empty
    End Sub

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Obtiene la tecla pulsada
    ''' Hay que asignarle el foco al textbox activo para que sepa donde tiene que escribir
    ''' </summary>
    ''' <param name="key"></param>    
    Protected Sub formKeyboard_TeclaContenidoPulsada(ByVal key As String) Handles formKeyboard.TeclaContenidoPulsada
        If (key.ToLower <> "cerrar") Then
            Dim ctrl As Control = CType(Me.Controls.Find(FocusOwner, True).FirstOrDefault, Control) 'CType(pnlContenido.Controls.Find(FocusOwner, True).FirstOrDefault, Control)
            If (ctrl IsNot Nothing) Then
                Dim txt As TextBox = CType(ctrl, TextBox)
                If (txt IsNot Nothing) Then txt.Focus()
            End If
        Else
            OcultarTecladoVirtual()
        End If
    End Sub

    ''' <summary>
    ''' Se guarda el texto
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        CType(callForm.Controls.Find(varResulName, True).FirstOrDefault, Label).Text = txtObservacion.Text
        If (eventControlName <> String.Empty) Then
            Dim btn As Button = CType(callForm.Controls.Find(eventControlName, True).FirstOrDefault, Button)
            btn.PerformClick()
        End If
        Cerrar()
    End Sub

    ''' <summary>
    ''' Se cierra el formulario
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnCerrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCerrar.Click
        Cerrar()
    End Sub

    ''' <summary>
    ''' Muestra el teclado virtual
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub txtObservacion_GotFocus(ByVal sender As Object, ByVal e As EventArgs) Handles txtObservacion.GotFocus
        MostrarTecladoVirtual()
    End Sub

    ''' <summary>
    ''' Oculta el teclado virtual
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub txtObservacion_LostFocus(ByVal sender As Object, ByVal e As EventArgs) Handles txtObservacion.LostFocus
        'OcultarTecladoVirtual()
    End Sub

    ''' <summary>
    ''' Se elimina de memoria el formulario
    ''' </summary>    
    Private Sub Cerrar()
        Me.Close()
        Me.Dispose()
    End Sub

#End Region

End Class