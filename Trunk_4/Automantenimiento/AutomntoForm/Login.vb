Public Class frmLogin

    Protected WithEvents formKeyboard As ReducedKeyBoardForm
    Private FocusOwner As String = String.Empty

#Region "Carga del formulario"

    ''' <summary>
    ''' Se muestra la pantalla de login
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub frmLogin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ConfigurarVentana()
        pnlKeyboard.Left = (Me.Width - pnlKeyboard.Width) / 2
        pnlKeyboard.Width = Me.Width
        pnlKeyboard.Height = Me.Height - pnlContenido.Height
        pnlKeyboard.Top = pnlContenido.Bottom + 30
        lblInfo.Visible = False
        Traduccion()
        'En sistemas, no habra que mostrar la password
        pnlPassword.Visible = ((Modulo.Negocio And AutomntoLib.ELL.Clases.Perfil.RolesUnNeg.Troqueleria) = AutomntoLib.ELL.Clases.Perfil.RolesUnNeg.Troqueleria)
        formKeyboard = New ReducedKeyBoardForm
        formKeyboard.TopLevel = False
        formKeyboard.AutoSize = True
        formKeyboard.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.pnlKeyboard.Controls.Add(formKeyboard)
        formKeyboard.Show()
        FocusOwner = "NT"
        GestionarTecladoVirtual(False)
        txtNumTrab.Focus()
        If (Modulo.Tipo = AutomntoLib.ELL.Clases.HojaCab.TipoHoja.Automantenimiento) Then
            labelTitulo.Text = itzultzaileWeb.Itzuli("Automantenimiento de la maquina").ToUpper
        ElseIf (Modulo.Tipo = AutomntoLib.ELL.Clases.HojaCab.TipoHoja.Inicio_produccion_automatico) Then
            labelTitulo.Text = itzultzaileWeb.Itzuli("Inicio de produccion de la maquina").ToUpper
        End If
    End Sub

    ''' <summary>
    ''' Configura los parametros de la ventana
    ''' </summary>    
    Private Sub ConfigurarVentana()
        Me.TopMost = True  'Para que sea modal
        Me.TopLevel = True
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None  'Para que no tenga botones de minimizar y cerrar
        Modulo.CanClose = True  'Especifica que se puede cerrar el formulario
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
            If (FocusOwner = "NT") Then
                txtNumTrab.Focus()
            ElseIf (FocusOwner = "P") Then
                txtPassword.Focus()
            End If
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
    ''' Traduce los labels del formulario
    ''' </summary>    
    Private Sub Traduccion()
        labelTitulo.Text = itzultzaileWeb.Itzuli("Automantenimiento de la maquina").ToUpper
        lblMaquina.Text = itzultzaileWeb.Itzuli(Modulo.DescMaquina)
        labelNumTrab.Text = itzultzaileWeb.Itzuli("IdTrabajador")
        labelPassword.Text = itzultzaileWeb.Itzuli("Contraseña")
        btnLogin.Text = itzultzaileWeb.Itzuli("Entrar")
        gbDatos.Text = itzultzaileWeb.Itzuli("Datos")
        ToolTip.SetToolTip(txtNumTrab, itzultzaileWeb.Itzuli("CodigoDeTrabajador"))
        ToolTip.SetToolTip(txtPassword, itzultzaileWeb.Itzuli("Contraseña del portal del empleado"))
    End Sub

    ''' <summary>
    ''' No se permite cerrar el formulario
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub frmLogin_FormClosing(ByVal sender As Object, ByVal e As Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If (Not Modulo.CanClose) Then e.Cancel = True
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
    ''' Marca que el control de numero de trabajador tiene el foco
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub txtNumTrab_GotFocus(ByVal sender As Object, ByVal e As EventArgs) Handles txtNumTrab.GotFocus
        FocusOwner = "NT"
    End Sub

    ''' <summary>
    ''' Marca que el control de password tiene el foco
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub txtPassword_GotFocus(ByVal sender As Object, ByVal e As EventArgs) Handles txtPassword.GotFocus
        FocusOwner = "P"
    End Sub

    ''' <summary>
    ''' Se inicializa el formulario
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Inicializar()
        txtNumTrab.Text = String.Empty : txtPassword.Text = String.Empty
        btnLogin.Enabled = True
        Traduccion()
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Se comprueba si el login es correcto
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnLogin_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLogin.Click
        Modulo.ConfigureLog4net()  'Por si ha estado mas de un dia en ejecucion, que escriba el log en el dia correspondiente
        Dim bEsSistemas As Boolean = ((Modulo.Negocio And AutomntoLib.ELL.Clases.Perfil.RolesUnNeg.Sistemas) = AutomntoLib.ELL.Clases.Perfil.RolesUnNeg.Sistemas)
        If (txtNumTrab.Text.Trim <> String.Empty AndAlso
            (bEsSistemas Or (Not bEsSistemas AndAlso txtPassword.Text.Trim <> String.Empty))) Then
            btnLogin.Enabled = False : btnLogin.Text = itzultzaileWeb.Itzuli("Logeando") & "..." : btnLogin.Refresh()
            If (txtNumTrab.Text = "000000") Then 'Puerta trasera para poder cerrar el formulario
                log.Info("El administrador cierra el formulario")
                Me.Close()
                Application.Exit()
            Else
                'Habria que meter lo del login del usuario pasandole el idPlanta pero haria falta actualizar las librerias de SAB y entonces cascarias en las maquinas por el cliente de oracle instalado
                If (bEsSistemas) Then
                    Dim userBLL As New SabLib.BLL.UsuariosComponent
                    Dim oUser As SabLib.ELL.Usuario = Modulo.GetUsuario(New SabLib.ELL.Usuario With {.CodPersona = CInt(txtNumTrab.Text), .IdPlanta = Modulo.IdPlanta}, True)
                    If (oUser IsNot Nothing) Then
                        Modulo.Ticket = Modulo.Login(oUser.CodPersona, oUser.PWD, Modulo.IdPlanta)
                    Else
                        Modulo.Ticket = Nothing
                    End If
                Else
                    Modulo.Ticket = Modulo.Login(CInt(txtNumTrab.Text), SabLib.BLL.Utils.EncriptarPassword(txtPassword.Text), Modulo.IdPlanta)
                End If
                If (Modulo.Ticket Is Nothing) Then
                    log.Warn("Login incorrecto del usuario " & txtNumTrab.Text & "(" & [Enum].GetName(GetType(AutomntoLib.ELL.Clases.Perfil.RolesUnNeg), Modulo.Negocio) & ")")
                    MostrarMensaje("Login incorrecto")
                    txtPassword.Text = String.Empty
                    btnLogin.Enabled = True : btnLogin.Text = itzultzaileWeb.Itzuli("Entrar") : btnLogin.Refresh()
                Else
                    log.Info("Se ha conectado el trabajador " & Modulo.Ticket.NombreCompleto & "(" & Modulo.Ticket.IdTrabajador & ")")
                    Application.CurrentCulture = New System.Globalization.CultureInfo(Modulo.Ticket.Culture)  'Se asigna la cultura del ticket a la aplicacion
                    Me.Hide()
                    Dim formHoja As New frmHoja(Me)
                    formHoja.ShowDialog()
                    Me.Close()
                End If
            End If
        Else
            MostrarMensaje("debeRellenarDatos")
        End If
    End Sub

    ''' <summary>
    ''' Muestra un mensaje
    ''' </summary>
    ''' <param name="mensa">Mensaje</param>    
    Private Sub MostrarMensaje(ByVal mensa As String)
        lblInfo.Visible = True
        lblInfo.Text = itzultzaileWeb.Itzuli(mensa)
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
