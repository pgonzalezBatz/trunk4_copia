Public Class frmLogin

    Protected WithEvents formKeyboard As KeyBoardForm3
    Private FocusOwner As String = String.Empty
    Dim mensaje As String = "Debes comprobar correspondencia entre la etiqueta de la última pieza y la etiqueta de contenedor"

#Region "Carga del formulario"

    ''' <summary>
    ''' Se muestra la pantalla de login
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub frmLogin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ConfigurarVentana()
        formKeyboard = New KeyBoardForm3
        formKeyboard.TopLevel = False
        formKeyboard.AutoSize = True
        formKeyboard.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.pnlKeyboard.Controls.Add(formKeyboard)
        formKeyboard.Show()
        GestionarTecladoVirtual(False)
        txtNumTrab.Focus()
        labelAviso.Text = ""
        Inicializar()
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
        Me.Location = New Drawing.Point(0, 0)
        pnlContenido.Left = (Me.Width - pnlContenido.Width) / 2
        pnlContenido.Top = (Me.Height - pnlContenido.Height) / 8
    End Sub

    ''' <summary>
    ''' Muestra u oculta el teclado virtual
    ''' </summary>
    ''' <param name="bMostrar">Indica si tiene que mostrar o no</param>    
    Public Sub GestionarTecladoVirtual(ByVal bMostrar As Boolean)
        pnlKeyboard.Visible = bMostrar
        txtNumTrab.Focus()
    End Sub

    ''' <summary>
    ''' Obtiene la tecla pulsada
    ''' Hay que asignarle el foco al textbox activo para que sepa donde tiene que escribir
    ''' </summary>
    ''' <param name="key"></param>    
    Protected Sub formKeyboard_TeclaContenidoPulsada(ByVal key As String) Handles formKeyboard.TeclaContenidoPulsada
        If (key.ToLower <> "cerrar") Then
            txtNumTrab.Focus()
        Else
            OcultarTecladoVirtual()
        End If
    End Sub

    ''' <summary>
    ''' Oculta el teclado virtual
    ''' </summary>
    Protected Sub OcultarTecladoVirtual()
        pnlKeyboard.Visible = False
        FocusOwner = String.Empty
    End Sub

    ''' <summary>
    ''' Solo se permiten numeros y la tecla de borrar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Overloads Sub txtNumTrab_TextChanged(ByVal sender As Object, ByVal e As Windows.Forms.KeyPressEventArgs) Handles txtNumTrab.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Back))
    End Sub


    ''' <summary>
    ''' Se inicializa el formulario
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Inicializar()
        txtNumTrab.Text = String.Empty
        btnLogin.Enabled = True
        labelMensaje.Text = mensaje
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Se comprueba si el login es correcto
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnLogin_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLogin.Click
        'Dim bEsSistemas As Boolean = ((Modulo.Negocio And AutomntoLib.ELL.Clases.Perfil.RolesUnNeg.Sistemas) = AutomntoLib.ELL.Clases.Perfil.RolesUnNeg.Sistemas)
        Dim num As String = txtNumTrab.Text.Trim
        If (num <> String.Empty) Then
            Dim result = Modulo.comprobarNumTrab(CInt(num))
            If result.StartsWith("ERR") Then
                'ERROR
                labelAviso.Text = "Error al acceder a la base de datos" & vbCrLf & result
                labelAviso.ForeColor = Color.Red
                txtNumTrab.Text = ""
            ElseIf result.Equals("0") Then
                'NO EXISTE
                labelAviso.Text = "El número de trabajador introducido (" & num & ") no está activo para esta planta"
                labelAviso.ForeColor = Color.Red
                txtNumTrab.Text = ""
            Else
                'EXISTE
                Modulo.guardaLog(CInt(num), 0)
                Me.Close()
                Application.Exit()
            End If
        Else
            labelAviso.Text = "Debes introducir un número de trabajador"
        End If
    End Sub

    ''' <summary>
    ''' Muestra u oculta el teclado virtual
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnTecladoVirt_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTecladoVirt.Click
        GestionarTecladoVirtual(Not pnlKeyboard.Visible)
    End Sub

#End Region

End Class
