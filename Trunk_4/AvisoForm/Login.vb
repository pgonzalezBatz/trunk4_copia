Public Class frmLogin

    ''''Protected WithEvents formKeyboard As ReducedKeyBoardForm
    Protected WithEvents formKeyboard As KeyBoardForm3
    Private FocusOwner As String = String.Empty
    Dim mensaje1 As String = "Ya hay suficientes piezas, debes hacer TRES LECTURAS CORRECTAS con la pistola. Después, introduce tu número de trabajador y clicka en 'Continuar'"
    Dim mensaje2 As String = "Quedan menos de 10 piezas para terminar el lote... Debes hacer TRES LECTURAS CORRECTAS con la pistola"
    'Dim mensaje1 As String = "Ya "
    'Dim mensaje2 As String = "Que"

#Region "Carga del formulario"

    ''' <summary>
    ''' Se muestra la pantalla de login
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub frmLogin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ConfigurarVentana()
        'pnlKeyboard.Left = (Me.Width - pnlKeyboard.Width) / 2
        'pnlKeyboard.Width = Me.Width
        'pnlKeyboard.Height = Me.Height - pnlContenido.Height
        'pnlKeyboard.Top = pnlContenido.Bottom + 30
        'Traduccion()
        ''''formKeyboard = New ReducedKeyBoardForm
        formKeyboard = New KeyBoardForm3
        formKeyboard.TopLevel = False
        formKeyboard.AutoSize = True
        formKeyboard.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.pnlKeyboard.Controls.Add(formKeyboard)
        formKeyboard.Show()
        GestionarTecladoVirtual(False)
        txtNumTrab.Focus()
        labelAviso.Text = Modulo.Orden & " - " & Modulo.Fase & " - " & Modulo.Piezas & " - " & Modulo.Cantidad & " - " & Modulo.Pendientes & " - " & Modulo.Flag
        Inicializar()
        'labelTitulo.Text = itzultzaileWeb.Itzuli("Automantenimiento de la maquina").ToUpper        
    End Sub

    ''' <summary>
    ''' Configura los parametros de la ventana
    ''' </summary>    
    Private Sub ConfigurarVentana()
        ''''Me.TopMost = True  'Para que sea modal
        ''''Me.TopLevel = True
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None  'Para que no tenga botones de minimizar y cerrar        
        'Establece como dimensiones de la ventana las de la pantalla
        Me.Width = My.Computer.Screen.Bounds.Width
        Me.Height = My.Computer.Screen.Bounds.Height
        'Situa la ventana en el punto 0,0
        Me.Location = New Drawing.Point(0, 0)
        'Se centra el contenido del formulario
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

    '''' <summary>
    '''' Traduce los labels del formulario
    '''' </summary>    
    'Private Sub Traduccion()
    '    labelTitulo.Text = itzultzaileWeb.Itzuli("Automantenimiento de la maquina").ToUpper
    '    lblMaquina.Text = itzultzaileWeb.Itzuli(Modulo.DescMaquina)
    '    labelNumTrab.Text = itzultzaileWeb.Itzuli("IdTrabajador")
    '    labelPassword.Text = itzultzaileWeb.Itzuli("Contraseña")
    '    btnLogin.Text = itzultzaileWeb.Itzuli("Entrar")
    '    gbDatos.Text = itzultzaileWeb.Itzuli("Datos")
    '    ToolTip.SetToolTip(txtNumTrab, itzultzaileWeb.Itzuli("CodigoDeTrabajador"))
    '    ToolTip.SetToolTip(txtPassword, itzultzaileWeb.Itzuli("Contraseña del portal del empleado"))
    'End Sub

    '''' <summary>
    '''' No se permite cerrar el formulario
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>    
    'Private Sub frmLogin_FormClosing(ByVal sender As Object, ByVal e As Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    '    If (Not Modulo.CanClose) Then e.Cancel = True
    'End Sub

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
        'Traduccion()

        labelMensaje.Text = If(Modulo.Flag = 1, mensaje1, mensaje2)
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
            'btnLogin.Enabled = False ': btnLogin.Text = itzultzaileWeb.Itzuli("Logeando") & "..." : btnLogin.Refresh()

            Dim result = Modulo.comprobarNumTrab(CInt(num))
            If result < 0 Then
                'ERROR
                labelAviso.Text = "Error al acceder a la base de datos"
                labelAviso.ForeColor = Color.Red
            ElseIf result = 0 Then
                'NO EXISTE
                labelAviso.Text = "El número de trabajador introducido no está activo para esta planta"
                labelAviso.ForeColor = Color.Red
            Else
                'EXISTE
                Modulo.guardaLog(CInt(num))
                Me.Close()
                Application.Exit()
            End If
            'Habria que meter lo del login del usuario pasandole el idPlanta pero haria falta actualizar las librerias de SAB y entonces cascarias en las maquinas por el cliente de oracle instalado
            'If (bEsSistemas) Then
            '    Dim userBLL As New SabLib.BLL.UsuariosComponent
            '    Dim oUser As SabLib.ELL.Usuario = Modulo.GetUsuario(New SabLib.ELL.Usuario With {.CodPersona = CInt(txtNumTrab.Text), .IdPlanta = Modulo.IdPlanta}, True)
            '    If (oUser IsNot Nothing) Then
            '        Modulo.Ticket = Modulo.Login(oUser.CodPersona, oUser.PWD, Modulo.IdPlanta)
            '    Else
            '        Modulo.Ticket = Nothing
            '    End If
            'Else
            '    Modulo.Ticket = Modulo.Login(CInt(txtNumTrab.Text), SabLib.BLL.Utils.EncriptarPassword(txtPassword.Text), Modulo.IdPlanta)
            'End If        
        Else
            'MostrarMensaje("debeRellenarDatos")
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

    Private Sub Label2_Click(sender As Object, e As EventArgs)

    End Sub

#End Region

End Class
