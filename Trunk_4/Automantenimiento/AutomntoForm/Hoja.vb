Imports AutomntoLib

Public Class frmHoja

    Protected WithEvents formKeyboard As ReducedKeyBoardForm
    Private FocusOwner As String = String.Empty
    Private bTieneAyudaMaquinas As Boolean = False
    Private callForm As frmLogin

#Region "Constructores"

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(ByVal pForm As Form)
        InitializeComponent()
        callForm = pForm
    End Sub

#End Region

#Region "Carga del formulario"

    ''' <summary>
    ''' Se carga el formulario de automantenimiento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub frmHoja_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Try
            ConfigurarVentana()
            Dim bNecProd As Boolean = False
            CargarDatos(bNecProd)
            lblNombrePersona.Focus()  'Se asigna el foco a un label porque sino se asigna a un cuadro de texto y se visualiza el keyForm por defecto
            formKeyboard = New ReducedKeyBoardForm With {.TopLevel = False, .AutoSize = True, .FormBorderStyle = FormBorderStyle.None}
            Me.pnlKeyboard.Controls.Add(formKeyboard)
            formKeyboard.Show()
            FocusOwner = String.Empty
            pnlKeyboard.Visible = False
            pnlKeyboard.Left = (Me.Width - pnlKeyboard.Width) / 2
            pnlKeyboard.Width = Me.Width
            pnlKeyboard.Top = Me.Height - pnlKeyboard.Height - 50
            btnSalir.Left = Me.Width - 100
            btnSalir.Visible = (Modulo.Negocio And ELL.Clases.Perfil.RolesUnNeg.Sistemas) 'El boton de salir solo es para sistemas, ya que en Troqueleria meten trabajador y contraseña
            Traduccion()
            If (bNecProd) Then
                If (MessageBox.Show(itzultzaileWeb.Itzuli("¿Va a existir produccion en la maquina?"), itzultzaileWeb.Itzuli("Automnto"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No) Then
                    log.Info("Se ha marcado que no existe produccion en la maquina")
                    GuardarSinProduccion()
                Else
                    log.Info("Se ha marcado que si existe produccion en la maquina")
                End If
            End If
        Catch ex As Exception
            log.Error("Ha ocurrido un error al cargar el formulario", ex)
            MessageBox.Show(itzultzaileWeb.Itzuli("Error al cargar el formulario").ToUpper, "Carga", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Configura los parametros de la ventana
    ''' </summary>    
    Private Sub ConfigurarVentana()
        Me.TopLevel = True  'Para que sea modal
        Me.FormBorderStyle = FormBorderStyle.None  'Para que no tenga botones de minimizar y cerrar
        'Establece como dimensiones de la ventana las de la pantalla
        Me.Width = My.Computer.Screen.Bounds.Width
        Me.Height = My.Computer.Screen.Bounds.Height
        'Situa la ventana en el punto 0,0
        Me.Location = New Point(0, 0)
        Me.AutoScroll = True    'Para que si no entran los puntos, tenga una barra de scroll        
    End Sub

    ''' <summary>
    ''' No se permite cerrar el formulario
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub frmLogin_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        If (Not Modulo.CanClose) Then e.Cancel = True
    End Sub

    ''' <summary>
    ''' Traduce los labels del formulario
    ''' </summary>    
    Private Sub Traduccion()
        labelPersona.Text = itzultzaileWeb.Itzuli("Persona")
        labelMaquina.Text = itzultzaileWeb.Itzuli("Maquina")
        myTooltip.SetToolTip(btnSalir, itzultzaileWeb.Itzuli("Cerrar sesion para entrar con otro trabajador diferente"))
    End Sub

#End Region

#Region "Cargar datos"

    ''' <summary>
    ''' Muestra la imagen de la ayuda mas grande
    ''' </summary>
    Protected Sub MostrarImagen(ByVal sender As Object, ByVal e As EventArgs)
        Dim pcb As PictureBox = CType(sender, PictureBox)
        Dim idAyuda As Integer = CInt(pcb.Name)
        Dim frmAyuda As New frmAyuda(idAyuda, "M")
        frmAyuda.Show()
    End Sub

    ''' <summary>
    ''' Muestra la ayuda del punto
    ''' </summary>
    Protected Sub MostrarAyuda(ByVal sender As Object, ByVal e As EventArgs)
        Dim pcb As PictureBox = CType(sender, PictureBox)
        Dim idPunto As Integer = CInt(pcb.Name)
        Dim frmAyuda As New frmAyuda(idPunto, "P")
        frmAyuda.Show()
    End Sub

    ''' <summary>
    ''' Muestra la ayuda del punto
    ''' </summary>
    Protected Sub MostrarTexto(ByVal sender As Object, ByVal e As EventArgs)
        Dim myBtn As Button = CType(sender, Button)
        Dim idPunto As Integer = CInt(myBtn.Name.Split("_")(1))
        Dim text As String = CType(myBtn.Parent.Controls.Find("lblDescrip_" & idPunto, True).FirstOrDefault, Label).Text
        Dim textoPunto As String = CType(myBtn.Parent.Controls.Find("lblIdPunt_" & idPunto, True).FirstOrDefault, Label).Text
        Dim frmObservacion As New frmObservacion(Me, "lblDescrip_" & idPunto, "btnChangeState_" & idPunto, 500, textoPunto.ToUpper, text)
        frmObservacion.Show()
    End Sub

    ''' <summary>
    ''' Cambia el icono de la linea de texto, dependiendo si el texto es blanco o distinto
    ''' </summary>
    Protected Sub ChangeState(ByVal sender As Object, ByVal e As EventArgs)
        Dim myBtn As Button = CType(sender, Button)
        Dim idPunto As Integer = CInt(myBtn.Name.Split("_")(1))
        Dim btnEditar As Button = CType(myBtn.Parent.Controls.Find("btnShowDescripPanel_" & idPunto, True).FirstOrDefault, Button)
        Dim lblDescrip As String = CType(myBtn.Parent.Controls.Find("lblDescrip_" & idPunto, True).FirstOrDefault, Label).Text           
        If (lblDescrip = String.Empty) Then
            btnEditar.Image = Image.FromFile(Modulo.ImagenesConfig & "EditWithOutText.png")
            myTooltip.SetToolTip(btnEditar, itzultzaileWeb.Itzuli("Introduzca una observacion"))
        Else
            btnEditar.Image = Image.FromFile(Modulo.ImagenesConfig & "EditWithText.png")
            myTooltip.SetToolTip(btnEditar, itzultzaileWeb.Itzuli("Editar la observacion"))
        End If
    End Sub

    ''' <summary>
    ''' Muestra el teclado virtual
    ''' </summary>
    Protected Sub MostrarTecladoVirtual(ByVal sender As Object, ByVal e As EventArgs)
        Dim txt As TextBox = CType(sender, TextBox)
        pnlKeyboard.Visible = True
        FocusOwner = txt.Name
    End Sub

    ''' <summary>
    ''' Oculta el teclado virtual
    ''' </summary>
    Protected Sub OcultarTecladoVirtual()
        pnlKeyboard.Visible = False
        FocusOwner = String.Empty
    End Sub

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
    ''' Carga los datos del automantenimiento de la maquina
    ''' </summary>    
    ''' <param name="bNecesidadProd">Se dejara en esta variable si hay que preguntarle si hay produccion</param>
    Private Sub CargarDatos(ByRef bNecesidadProd As Boolean)
        lblNombrePersona.Text = Ticket.NombreCompleto
        lblMaquina.Text = Modulo.DescMaquina

        Dim puntBLL As New BLL.PuntoBLL
        Dim maqBLL As New BLL.MaquinaBLL
        Dim lbl As Label : Dim pImage As PictureBox : Dim rdb As RadioButton : Dim btn As Button : Dim pnl As Panel : Dim gb As GroupBox : Dim txt As TextBox
        Dim y, saltoY, left, top, pnlWidth, pnlHeight, xPunto, xRadio1, xRadio2, xImg, xEdit As Integer
        Dim colorPar As Color = Color.FromName("LightSteelBlue")
        Dim colorImpar As Color = Color.FromName("GhostWhite")
        Dim mstreamImage As IO.MemoryStream
        Dim lPuntos As List(Of ELL.Clases.Punto) = Nothing
        lPuntos = puntBLL.consultarListadoPuntosMaquina(Modulo.IdMaquina, Modulo.IdGrupoEjecucion)
        If (lPuntos IsNot Nothing) Then lPuntos.Sort(Function(o1 As ELL.Clases.Punto, o2 As ELL.Clases.Punto) o1.RangoKultura(Ticket.Culture) < o2.RangoKultura(Ticket.Culture))
        Dim lAyuda As List(Of ELL.Clases.Maquina.Ayuda) = maqBLL.consultarListadoAyudas(Modulo.IdMaquina, True)
        pnlWidth = 850 : pnlHeight = 55 'pnlHeight = 50
        left = (Me.Width - pnlWidth) / 2 : top = 100 : y = 5 : saltoY = 40 'saltoY = 35
        xPunto = 40 : xRadio1 = 700 : xRadio2 = 750 : xEdit = 800 : xImg = 10
        Dim lLista As New List(Of String())
        'IMAGENES DE AYUDA
        '---------------
        If (lAyuda IsNot Nothing AndAlso lAyuda.Count > 0) Then
            bTieneAyudaMaquinas = True
            gb = New GroupBox With {.Text = itzultzaileWeb.Itzuli("Imagenes de ayuda"), .Font = New Font("Arial", "15"), .Size = New Size(200, pnlHeight), .AutoSize = True, .Left = 15, .Top = top + y}
            Dim bDosColumnas As Boolean = (lAyuda.Count > 5)  'Si tiene mas de 5 imagenes, se mostraran en dos columnas            
            Dim xAyuda, yAyuda, numColumna As Integer
            numColumna = 0 : xAyuda = 35 : yAyuda = top - 60 'top + y
            For Each ayuda As ELL.Clases.Maquina.Ayuda In lAyuda
                pImage = New PictureBox With {.Name = ayuda.Id, .Location = New Point(xAyuda, yAyuda), .SizeMode = PictureBoxSizeMode.StretchImage, .Width = 100, .Height = 100, .Cursor = Cursors.Hand}
                mstreamImage = New IO.MemoryStream(ayuda.Imagen)
                pImage.Image = Image.FromStream(mstreamImage)
                AddHandler pImage.Click, AddressOf MostrarImagen
                myTooltip.SetToolTip(pImage, itzultzaileWeb.Itzuli("Click para ver mas grande"))
                gb.Controls.Add(pImage)
                mstreamImage.Close()
                If (bDosColumnas) Then
                    If (numColumna = 0) Then
                        numColumna = 1
                        xAyuda = 180
                    Else
                        numColumna = 0
                        xAyuda = 35
                        yAyuda += pImage.Height + 10
                    End If
                Else
                    yAyuda += pImage.Height + 10
                End If
            Next
            Me.Controls.Add(gb)
            left = gb.Width + 25
        End If
        'LEYENDA
        '---------------
        pnl = New Panel With {.Size = New Size(pnlWidth, pnlHeight), .Left = left, .Top = top + y}
        pImage = New PictureBox With {.Location = New Point(xImg, 15), .Width = 24, .Height = 24}
        pImage.Image = Image.FromFile(Modulo.ImagenesConfig & "operario.png")
        pnl.Controls.Add(pImage)
        lbl = New Label With {.Text = itzultzaileWeb.Itzuli("Responsabilidad de operario"), .Font = New Font("Arial", "15"), .Location = New Point(xPunto, 15), .AutoSize = True}
        pnl.Controls.Add(lbl)
        Me.Controls.Add(pnl)
        y += pnl.Height
        'CABECERA DE LA TABLA
        '---------------
        pnl = New Panel With {.Size = New Size(pnlWidth, pnlHeight), .Left = left, .Top = top + y, .BorderStyle = BorderStyle.FixedSingle, .BackColor = Color.FromName("Gainsboro")}
        lbl = New Label With {.Text = itzultzaileWeb.Itzuli("Punto").ToUpper, .Font = New Font("Arial", "18"), .Location = New Point(xPunto, 5), .AutoSize = True}
        pnl.Controls.Add(lbl)
        pImage = New PictureBox With {.Location = New Point(xRadio1, 5), .Width = 24, .Height = 24}
        pImage.Image = Image.FromFile(Modulo.ImagenesConfig & "ok.png")
        pnl.Controls.Add(pImage)
        pImage = New PictureBox With {.Location = New Point(xRadio2, 5), .Width = 24, .Height = 24}
        pImage.Image = Image.FromFile(Modulo.ImagenesConfig & "nook.png")
        pnl.Controls.Add(pImage)
        Me.Controls.Add(pnl)
        pImage = New PictureBox With {.Location = New Point(xEdit, 5), .Width = 24, .Height = 24}
        pImage.Image = Image.FromFile(Modulo.ImagenesConfig & "Edit.png")
        pnl.Controls.Add(pImage)
        Me.Controls.Add(pnl)
        y += saltoY
        'PUNTOS
        '---------------
        Dim oMaq As ELL.Clases.Maquina = maqBLL.consultar(New ELL.Clases.Maquina With {.Id = IdMaquina})
        bNecesidadProd = (oMaq.NecesidadProduccion)
        Dim index As Integer = 0
        Dim xPuntoOrig As Integer = xPunto
        Dim bModificable As Boolean
        For Each myPunto As ELL.Clases.Punto In lPuntos
            bModificable = True
            xPunto = xPuntoOrig
            pnl = New Panel With {.Size = New Size(pnlWidth, pnlHeight), .Left = left, .Top = top + y, .BorderStyle = BorderStyle.FixedSingle, .BackColor = If(index Mod 2 = 0, colorPar, colorImpar)}
            If (myPunto.Responsable = ELL.Clases.Punto.PtoResponsable.Operario) Then
                pImage = New PictureBox With {.Location = New Point(xImg, 15), .Width = 24, .Height = 24}
                pImage.Image = Image.FromFile(Modulo.ImagenesConfig & "operario.png")
                pnl.Controls.Add(pImage)
            End If
            If (myPunto.NombreImgAyuda <> String.Empty OrElse myPunto.RangoAyudaKultura(Ticket.Culture) <> String.Empty) Then  'Si el punto tiene una ayuda, se muestra el icono
                pImage = New PictureBox With {.Location = New Point(xPunto, 15), .Width = 24, .Height = 24, .Name = myPunto.Id, .Cursor = Cursors.Hand}
                pImage.Image = Image.FromFile(Modulo.ImagenesConfig & "Help.png")
                myTooltip.SetToolTip(pImage, itzultzaileWeb.Itzuli("Click para ver la ayuda del punto"))
                AddHandler pImage.Click, AddressOf MostrarAyuda
                pnl.Controls.Add(pImage)
                xPunto += 30
            End If
            lbl = New Label With {.Name = "lblIdPunt_" & myPunto.Id, .Text = myPunto.RangoKultura(Ticket.Culture), .Location = New Point(xPunto, 15), .AutoSize = True}
            'De momento, solo vamos a poner la fuente en la sisdleX12C
            If (oMaq.Id = 241 Or oMaq.Id = 242 Or oMaq.Id = 243 Or oMaq.Id = 244) Then  'Las lineas de MONTAJE PALANCA X12C
                lbl.Font = New Font("Batz Font", "13")
            Else
                lbl.Font = New Font("Arial", "13")
            End If
            'Si el texto es muy grande, no cabe
            If (lbl.PreferredWidth > pnlWidth - 175) Then lbl.Font = New Font("Arial", "11")
            pnl.Controls.Add(lbl)
            xPunto += lbl.Width
            If (myPunto.RequiereValor) Then
                txt = New TextBox With {.Name = "txt_" & myPunto.Id, .Font = New Font("Arial", "13"), .Location = New Point(xPunto + 5, 15), .MaxLength = 10, .Width = 50}
                AddHandler txt.GotFocus, AddressOf MostrarTecladoVirtual
                pnl.Controls.Add(txt)
            End If
            rdb = New RadioButton With {.Name = "rdb_ok_" & myPunto.Id, .Checked = False, .Location = New Point(xRadio1, 20), .AutoSize = True, .Size = New Size(20, 20), .Appearance = Appearance.Button, .FlatStyle = FlatStyle.Popup, .ImageAlign = ContentAlignment.MiddleCenter, .Top = 14}
            rdb.Image = Image.FromFile(Modulo.ImagenesConfig & "Radiobutton.png")
            '141020 Gaizka me dice que por defecto estarán chequeados
            If (Modulo.Negocio And ELL.Clases.Perfil.RolesUnNeg.Sistemas) Then rdb.Checked = True
            AddHandler rdb.Click, AddressOf ChangeRadioButton
            pnl.Controls.Add(rdb)
            rdb = New RadioButton With {.Name = "rdb_no_" & myPunto.Id, .Checked = False, .Location = New Point(xRadio2, 20), .AutoSize = True, .Size = New Size(20, 20), .Appearance = Appearance.Button, .FlatStyle = FlatStyle.Popup, .ImageAlign = ContentAlignment.MiddleCenter, .Top = 14}
            rdb.Image = Image.FromFile(Modulo.ImagenesConfig & "Radiobutton.png")
            AddHandler rdb.Click, AddressOf ChangeRadioButton
            pnl.Controls.Add(rdb)
            'Si la maquina es de troqueleria, habra que mirar si el punto todavia esta sin solucionar para no permitir tocarlo            
            Dim bVisualizarText As Boolean = False
            If ((oMaq.Maq_UnidadNegocio And ELL.Clases.Perfil.RolesUnNeg.Troqueleria) = ELL.Clases.Perfil.RolesUnNeg.Troqueleria) Then
                Dim hojasLinBLL As New BLL.HojasLineasBLL
                Dim rdbNoOk, rdbOk As RadioButton
                Dim textTooltip As String = itzultzaileWeb.Itzuli("Este punto esta pendiente de solucionarse por parte del personal de mantenimiento")
                If (Not hojasLinBLL.isClosed(IdMaquina, myPunto.Id)) Then
                    'Si el punto es independiente, se podra asignar siempre una descripcion a este punto
                    rdbOk = CType(pnl.Controls.Find("rdb_ok_" & myPunto.Id, False).FirstOrDefault, RadioButton)
                    rdbNoOk = CType(pnl.Controls.Find("rdb_no_" & myPunto.Id, False).FirstOrDefault, RadioButton)
                    If (myPunto.DependienteAnterior = ELL.Clases.Punto.Dependencia.Independiente) Then
                        rdbOk.Checked = False : rdbNoOk.Checked = False
                    Else
                        rdbNoOk.Image = Image.FromFile(Modulo.ImagenesConfig & "Radiobutton_NoOk.png")
                        rdbOk.Checked = False : rdbNoOk.Checked = True : bVisualizarText = True
                        myTooltip.SetToolTip(pnl, textTooltip)
                        myTooltip.SetToolTip(rdbNoOk, textTooltip)
                        myTooltip.SetToolTip(CType(pnl.Controls.Find("lblIdPunt_" & myPunto.Id, False).FirstOrDefault, Label), textTooltip)
                        pnl.BackColor = ColorTranslator.FromHtml("#FFD8B0")
                    End If
                End If
            End If
            'Panel con un cuadro de texto que se activara cuando se marque como no ok                
            btn = New Button With {.Name = "btnShowDescripPanel_" & myPunto.Id, .AutoSize = True, .Size = New Size(20, 20), .Location = New Point(xEdit, 20), .Top = 14, .Font = New Font("Arial", "16")}
            btn.Image = Image.FromFile(Modulo.ImagenesConfig & "EditWithOutText.png")
            myTooltip.SetToolTip(btn, itzultzaileWeb.Itzuli("Introduzca una observacion"))
            AddHandler btn.Click, AddressOf MostrarTexto
            btn.Visible = bVisualizarText
            pnl.Controls.Add(btn)
            'Label donde se guardara las observaciones introducidas
            lbl = New Label With {.Name = "lblDescrip_" & myPunto.Id, .Visible = False}
            pnl.Controls.Add(lbl)
            'Boton para cambiar el icono de las observaciones
            btn = New Button With {.Name = "btnChangeState_" & myPunto.Id, .Visible = True, .Size = New Size(0, 0), .Location = New Point(xEdit + 50, 20)} 'Tiene que ser visible porque sino, al ser llamado desde otro formulario, no se genera el evento click            
            AddHandler btn.Click, AddressOf ChangeState
            pnl.Controls.Add(btn)
            Me.Controls.Add(pnl)
            y += saltoY
            index += 1
            'Hay que ponerlo aquí una vez añadido el panel ya que si no, cuando iba a ChangeRadioButton no encontraba el parent del radio
            If (Modulo.Negocio And ELL.Clases.Perfil.RolesUnNeg.Sistemas) Then
                Dim rdbOk As RadioButton
                rdbOk = CType(pnl.Controls.Find("rdb_ok_" & myPunto.Id, False).FirstOrDefault, RadioButton)
                ChangeRadioButton(rdbOk, Nothing)
            End If
        Next
        'BOTON GUARDAR
        '---------------
        btn = New Button With {.Name = "btnGuardar", .AutoSize = True, .Location = New Point(left, top + y + 40), .Font = New Font("Arial", "18"), .Text = itzultzaileWeb.Itzuli("Guardar"), .Width = pnlWidth, .Height = 75}
        AddHandler btn.Click, AddressOf Guardar
        Me.Controls.Add(btn)
    End Sub

    ''' <summary>
    ''' Evento al hacer click en un radiobutton
    ''' Se le cambiara la imagen de fondo y la  imagen
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ChangeRadioButton(ByVal sender As Object, ByVal e As EventArgs)
        Dim rdb As RadioButton = CType(sender, RadioButton)
        Dim otherRadio As RadioButton
        Dim idPunto As Integer = CInt(rdb.Name.Substring(7))
        Dim nameRadio As String = rdb.Name.Substring(0, 6) 'rdb_ok or rdb_no
        Dim colorOk As Color = Color.FromArgb(183, 250, 173)
        Dim colorNo As Color = Color.FromArgb(254, 209, 209)
        Dim colorUnSelected As Color = Color.FromArgb(248, 250, 248)
        If (CType(rdb.Parent, Panel).BackColor <> ColorTranslator.FromHtml("#FFD8B0")) Then
            If (nameRadio = "rdb_ok") Then
                otherRadio = CType(rdb.Parent.Controls.Find("rdb_no_" & idPunto, True).FirstOrDefault, RadioButton)
                CType(rdb.Parent.Controls.Find("btnShowDescripPanel_" & idPunto, True).FirstOrDefault, Button).Visible = False
                If (rdb.Checked) Then
                    rdb.Image = Image.FromFile(Modulo.ImagenesConfig & "Radiobutton_Ok.png")
                    otherRadio.Image = Image.FromFile(Modulo.ImagenesConfig & "Radiobutton.png")
                    rdb.BackColor = colorOk
                    otherRadio.BackColor = colorUnSelected
                Else
                    rdb.Image = Image.FromFile(Modulo.ImagenesConfig & "Radiobutton.png")
                    otherRadio.Image = Image.FromFile(Modulo.ImagenesConfig & "Radiobutton_NoOk.png")
                    rdb.BackColor = colorUnSelected
                    otherRadio.BackColor = colorNo
                End If
            Else
                otherRadio = CType(rdb.Parent.Controls.Find("rdb_ok_" & idPunto, True).FirstOrDefault, RadioButton)
                CType(rdb.Parent.Controls.Find("btnShowDescripPanel_" & idPunto, True).FirstOrDefault, Button).Visible = True
                If (rdb.Checked) Then
                    rdb.Image = Image.FromFile(Modulo.ImagenesConfig & "Radiobutton_NoOk.png")
                    otherRadio.Image = Image.FromFile(Modulo.ImagenesConfig & "Radiobutton.png")
                    rdb.BackColor = colorNo
                    otherRadio.BackColor = colorUnSelected
                Else
                    rdb.Image = Image.FromFile(Modulo.ImagenesConfig & "Radiobutton.png")
                    otherRadio.Image = Image.FromFile(Modulo.ImagenesConfig & "Radiobutton_Ok.png")
                    rdb.BackColor = colorUnSelected
                    otherRadio.BackColor = colorOk
                End If
            End If
        Else 'No se puede modificar porque esta marcado para solucionarse
            CType(rdb.Parent.Controls.Find("rdb_ok_" & idPunto, True).FirstOrDefault, RadioButton).Checked = False
            CType(rdb.Parent.Controls.Find("rdb_no_" & idPunto, True).FirstOrDefault, RadioButton).Checked = True
            MessageBox.Show(itzultzaileWeb.Itzuli("Este punto esta pendiente de solucionarse por parte del personal de mantenimiento").ToUpper, itzultzaileWeb.Itzuli("Informacion"), MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    ''' <summary>
    ''' Evento para guardar el formulario
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Guardar(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Modulo.ConfigureLog4net()  'Por si ha estado mas de un dia en ejecucion, que escriba el log en el dia correspondiente
            Dim button As Button = CType(sender, Button)
            button.Text = itzultzaileWeb.Itzuli("Guardando") & "..."
            button.Enabled = False : button.Refresh()
            Dim oHoja As New ELL.Clases.HojaCab With {.Fecha = Now, .IdMaquina = Modulo.IdMaquina, .IdUsuario = Modulo.Ticket.IdUser, .Tipo = Modulo.Tipo}
            Dim lineas As New List(Of ELL.Clases.HojaLineas)
            Dim lineasNoEnabled As List(Of Integer) = Nothing
            Dim radio As RadioButton
            Dim txtValor As TextBox
            Dim lblObservacion As Label
            Dim bCabecera, bAlertaOperario As Boolean
            Dim bLineasNoOk As Boolean = False
            Dim idPunto, status As Integer
            Dim valor, observacion As String
            bAlertaOperario = False
            For Each myControl As Control In Me.Controls
                If (myControl.GetType() Is GetType(Panel)) Then
                    status = -1 : idPunto = -1 : bCabecera = True : valor = String.Empty : observacion = String.Empty
                    For Each myControl2 As Control In myControl.Controls
                        If (myControl2.GetType() Is GetType(RadioButton)) Then
                            bCabecera = False
                            radio = CType(myControl2, RadioButton)
                            If (radio.Name.Split("_")(1) = "ok" And radio.Checked) Then
                                idPunto = CInt(radio.Name.Split("_")(2))
                                status = 1
                            ElseIf (radio.Name.Split("_")(1) = "no" And radio.Checked) Then
                                'Si la fila esta inhabilitada, no se avisara por email ya que esta pendiente de solucionarse                                
                                If (CType(radio.Parent, Panel).BackColor = ColorTranslator.FromHtml("#FFD8B0")) Then
                                    status = 0
                                    If (lineasNoEnabled Is Nothing) Then lineasNoEnabled = New List(Of Integer)
                                    Dim myIdPuntNoEnabled As Integer = CInt(radio.Name.Split("_")(2))
                                    lineasNoEnabled.Add(myIdPuntNoEnabled)
                                    log.Warn("El punto " & myIdPuntNoEnabled & " de la maquina " & IdMaquina & " esta pendiente de solucionarse")
                                End If
                                idPunto = CInt(radio.Name.Split("_")(2))
                                bLineasNoOk = True
                                status = 0
                                'Se comprueba si es responsable el operario. Si el primer control es un picturebox, sera porque es operario
                                If (myControl.Parent.Controls(0).GetType() Is GetType(PictureBox)) Then bAlertaOperario = True
                            End If
                        ElseIf (myControl2.GetType() Is GetType(TextBox)) Then
                            bCabecera = False
                            txtValor = CType(myControl2, TextBox)
                            valor = txtValor.Text
                            'Si es de sistemas, los campos de texto son obligatorios
                            If (((Modulo.Negocio And ELL.Clases.Perfil.RolesUnNeg.Sistemas) = ELL.Clases.Perfil.RolesUnNeg.Sistemas) And valor = String.Empty) Then
                                status = -1
                                Exit For
                            End If
                        ElseIf (myControl2.GetType() Is GetType(Label) And myControl2.Name.StartsWith("lblDescrip_")) Then
                            bCabecera = False
                            lblObservacion = CType(myControl2, Label)
                            observacion = lblObservacion.Text
                        End If
                    Next
                    If (Not bCabecera) Then
                        If (idPunto = -1 Or status = -1) Then
                            OcultarTecladoVirtual()  'No se porque, se visualiza en este caso y al final
                            MessageBox.Show(itzultzaileWeb.Itzuli("debeRellenarDatos").ToUpper, itzultzaileWeb.Itzuli("Guardar"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            button.Text = itzultzaileWeb.Itzuli("Guardar")
                            button.Enabled = True : button.Refresh()
                            OcultarTecladoVirtual()
                            Exit Sub
                        Else
                            lineas.Add(New ELL.Clases.HojaLineas With {.idPunto = idPunto, .Estado = status, .Valor = valor, .Observaciones = observacion})
                        End If
                    End If
                End If
            Next
            If (lineas.Count = 0) Then
                OcultarTecladoVirtual()
                MessageBox.Show(itzultzaileWeb.Itzuli("debeRellenarDatos").ToUpper, itzultzaileWeb.Itzuli("Guardar"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                button.Text = itzultzaileWeb.Itzuli("Guardar")
                button.Enabled = True : button.Refresh()
                OcultarTecladoVirtual()
            Else 'Se guardan los puntos
                'If (bAlertaOperario) Then
                '    MessageBox.Show(itzultzaileWeb.Itzuli("Usted es responsable de algunos puntos que ha marcado como no ok.").ToUpper, itzultzaileWeb.Itzuli("Guardar"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                'End If
                'HABRA QUE GENERAR LAS SOLICITUDES DE TRABAJO
                '-------------------------------------------------------------------
                If (bLineasNoOk) Then '270217: Ahora va a servir para los dos AndAlso ((Modulo.Negocio And ELL.Clases.Perfil.RolesUnNeg.Sistemas) = ELL.Clases.Perfil.RolesUnNeg.Sistemas)
                    Dim lineasNoOk As List(Of ELL.Clases.HojaLineas) = lineas.FindAll(Function(o As ELL.Clases.HojaLineas) o.Estado = 0)
                    Dim mapeoBLL As New BLL.MapeoOlanetBLL
                    Dim olanetBLL As New BLL.OlanetBLL
                    Dim prismaBLL As New BLL.PrismaBLL
                    Dim maqBLL As New BLL.MaquinaBLL
                    Dim puntBLL As New BLL.PuntoBLL
                    Dim userBLL As New SabLib.BLL.UsuariosComponent
                    Dim webServiceResul As Boolean
                    Dim asset As String = String.Empty
                    Dim xmlResponse, xmlRequest, urlWebService, userPrisma, companyPrisma, requestType, codTraAutomnto, codTraPrisma, requestName, observaciones As String
                    Dim tipoContrato As Integer
                    Dim oMaq As ELL.Clases.Maquina = maqBLL.consultar(New ELL.Clases.Maquina With {.Id = Modulo.IdMaquina})
                    If (oMaq.Asset.Trim <> String.Empty) Then
                        If ((Modulo.Negocio And ELL.Clases.Perfil.RolesUnNeg.Sistemas) = ELL.Clases.Perfil.RolesUnNeg.Sistemas) Then
                            Dim oMapeo As ELL.Clases.MapeoOlanet = mapeoBLL.consultar(New ELL.Clases.MapeoOlanet With {.IdPlanta = Modulo.IdPlanta, .PC = Modulo.Equipo})
                            Dim machineStatus As ELL.Clases.EstadosMaquinaOlanet = ELL.Clases.EstadosMaquinaOlanet.Produccion
                            'Se obtiene el estado de la maquina. Si no existiese el mapeo, se marcara como en produccion
                            If (oMapeo IsNot Nothing) Then
                                machineStatus = olanetBLL.GetEstadoMaquina(oMapeo.IdOlanet)
                                log.Info("El estado de la linea de olanet " & oMapeo.IdOlanet & " asociada al equipo " & Modulo.Equipo & " es " & [Enum].GetName(GetType(ELL.Clases.EstadosMaquinaOlanet), machineStatus))
                            Else
                                log.Warn("No se ha conseguido obtener la idLineaOlanet asociada al equipo " & Modulo.Equipo & ". Por tanto, vamos a suponer que el estado de la maquina es 'En produccion'")
                            End If
                            Select Case machineStatus
                                Case ELL.Clases.EstadosMaquinaOlanet.Paro
                                    requestType = "01"  'Averia con paro
                                Case Else
                                    requestType = "02"  'Averia sin paro
                            End Select
                        Else
                            requestType = "10" 'Averia sin paro                               
                        End If
                        asset = oMaq.Asset
                        companyPrisma = [Enum].Parse(GetType(ELL.Clases.Maquina.Planta), oMaq.Company).ToString
                        'El codigo de trabajador lo obtendremos de hacer la consulta con la tabla requester de prisma.Habra que añadir el 90 si es trabajador eventual
                        codTraAutomnto = Modulo.Ticket.IdTrabajador
                        tipoContrato = 1 'TODO:Hay que revisar esto userBLL.TipoContrato(codTraAutomnto)
                        If (tipoContrato = 2) Then codTraAutomnto += 900000 'Si es eventual, se le suma 900.000            
                        codTraPrisma = prismaBLL.GetNumTrabajador(codTraAutomnto, companyPrisma)
                        If (String.IsNullOrEmpty(codTraPrisma)) Then
                            log.Warn("No se ha encontrado el codigo de trabajador de prisma de " & Modulo.Ticket.IdTrabajador & ". Se le va a asignar el codigo de trabajador por defecto")
                            codTraPrisma = Configuration.ConfigurationManager.AppSettings("numTrabAutomntoPrisma")
                        End If
                        urlWebService = Configuration.ConfigurationManager.AppSettings("urlWebServicePrisma")
                        userPrisma = Configuration.ConfigurationManager.AppSettings("userPrisma")
                        For Each myLinea As ELL.Clases.HojaLineas In lineasNoOk
                            Try
                                If (lineasNoEnabled IsNot Nothing AndAlso lineasNoEnabled.Count > 0) Then
                                    If (lineasNoEnabled.Exists(Function(o As Integer) o = myLinea.idPunto)) Then
                                        log.Info("No se va a registar la solicitud prisma ya que el punto no está habilitado")
                                        Continue For
                                    End If
                                End If
                                xmlRequest = String.Empty : xmlResponse = String.Empty
                                observaciones = puntBLL.consultar(myLinea.idPunto).RangoKultura(Ticket.Culture)
                                If (myLinea.Observaciones <> String.Empty) Then observaciones &= " (" & myLinea.Observaciones & ")"
                                If ((Modulo.Negocio And ELL.Clases.Perfil.RolesUnNeg.Troqueleria) = ELL.Clases.Perfil.RolesUnNeg.Troqueleria) Then 'En troqueleria, el request name sera el texto del punto
                                    requestName = observaciones
                                Else 'En sistemas, pondra Automnto
                                    requestName = [Enum].GetName(GetType(ELL.Clases.HojaCab.TipoHoja), Modulo.Tipo).Replace("_", " ")
                                End If
                                webServiceResul = False
                                myLinea.NumSolicitudPrisma = prismaBLL.GenerarSolicitudTrabajo(urlWebService, userPrisma, companyPrisma, requestName, requestType, asset, codTraPrisma, observaciones, xmlRequest, xmlResponse, webServiceResul)
                                If (Not webServiceResul) Then
                                    log.Error("Error al generar la solicitud de Prisma " & myLinea.NumSolicitudPrisma & ". Peticion =>" & xmlRequest & "|| Respuesta =>" & xmlResponse, Nothing)
                                Else
                                    log.Info("Se ha generado la solicitud de trabajo de la company " & companyPrisma & " - " & myLinea.NumSolicitudPrisma & vbCrLf &
                                        "Peticion =>" & xmlRequest & vbCrLf &
                                        "Respuesta =>" & xmlResponse)
                                End If
                            Catch batzEx As SabLib.BatzException
                                log.Error("Ha ocurrido un problema al intentar generar la solicitud de trabajo de la linea con el punto " & myLinea.idPunto, batzEx)
                            End Try
                        Next
                    Else
                        log.Warn("No se puede generar una solicitud prisma porque la maquina no tiene un Asset informado")
                    End If
                Else
                    log.Warn("Todas las lineas ok")
                End If
                '-------------------------------------------------------------------
                Dim hojaBLL As New BLL.HojaCabBLL
                oHoja.Lineas = lineas
                Dim idCab As Integer = hojaBLL.Save(oHoja)
                log.Info("El trabajador ha guardado los datos del formulario de la maquina " & Modulo.IdMaquina)
                Modulo.CanClose = True
                Try
                    Dim grupoBLL As New BLL.GrupoBLL
                    Dim oGrupo As ELL.Clases.Grupo = grupoBLL.consultar(Modulo.IdGrupoEjecucion)
                    Dim fechaPlanifEjec As Date = grupoBLL.ObtenerDiaPrevistoEjecucionGrupo(oGrupo, Now)
                    'De la fecha que nos devuelve, no nos vale la hora. La hora sera la del grupo
                    If (fechaPlanifEjec <> Date.MinValue) Then fechaPlanifEjec = New Date(fechaPlanifEjec.Year, fechaPlanifEjec.Month, fechaPlanifEjec.Day, oGrupo.Hora.Hour, oGrupo.Hora.Minute, 0)
                    grupoBLL.SaveEjecucion(oGrupo, Modulo.Tipo, idCab, fechaPlanifEjec)
                Catch batzEx As Sablib.BatzException
                    log.Error("Ha ocurrido un error al guardar la ejecucion." & batzEx.Termino, batzEx.Excepcion)
                End Try
                If (bLineasNoOk) Then AvisarPorEmail(idCab, lineasNoEnabled)
                Dim frmResul As New frmResul(itzultzaileWeb.Itzuli("datosGuardados"), True)
                frmResul.Show()
            End If
        Catch ex As Exception
            log.Error("Ha ocurrido un error al guardar los datos del formulario", ex)
            Modulo.CanClose = True
            Dim frmResul As New frmResul(itzultzaileWeb.Itzuli("errGuardar") & vbCrLf & itzultzaileWeb.Itzuli("Contacte con el administrador"), False)
            frmResul.Show()
        End Try
    End Sub

    ''' <summary>
    ''' Guarda los datos cuando no haya que realizar el automnto. No hay necesidad de produccion
    ''' </summary>
    Private Sub GuardarSinProduccion()
        Try           
            Dim oHoja As New ELL.Clases.HojaCab With {.Fecha = Now, .IdMaquina = Modulo.IdMaquina, .IdUsuario = Modulo.Ticket.IdUser, .Tipo = Modulo.Tipo, .TipoInc = ELL.Clases.HojaCab.TipoIncidencia.Sin_Produccion}
            Dim lineas As New List(Of ELL.Clases.HojaLineas)
            Dim radio As RadioButton
            Dim idPunto As Integer
            For Each myControl As Control In Me.Controls
                If (myControl.GetType() Is GetType(Panel)) Then
                    For Each myControl2 As Control In myControl.Controls
                        If (myControl2.GetType() Is GetType(RadioButton)) Then
                            radio = CType(myControl2, RadioButton)
                            idPunto = CInt(radio.Name.Split("_")(2))
                            If Not (lineas.Exists(Function(o As ELL.Clases.HojaLineas) o.idPunto = idPunto)) Then lineas.Add(New ELL.Clases.HojaLineas With {.idPunto = idPunto, .Estado = 1})
                        End If
                    Next
                End If
            Next
            If (lineas.Count = 0) Then
                OcultarTecladoVirtual()
                MessageBox.Show(itzultzaileWeb.Itzuli("debeRellenarDatos").ToUpper, itzultzaileWeb.Itzuli("Guardar"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation)               
                OcultarTecladoVirtual()
            Else 'Se guardan los puntos                
                Dim hojaBLL As New BLL.HojaCabBLL
                oHoja.Lineas = lineas
                Dim idCab As Integer = hojaBLL.Save(oHoja)
                log.Info("El trabajador ha guardado los datos del formulario de la maquina " & Modulo.IdMaquina)
                Modulo.CanClose = True
                Try
                    Dim grupoBLL As New BLL.GrupoBLL
                    Dim oGrupo As ELL.Clases.Grupo = grupoBLL.consultar(Modulo.IdGrupoEjecucion)
                    Dim fechaPlanifEjec As Date = grupoBLL.ObtenerDiaPrevistoEjecucionGrupo(oGrupo, Now)
                    'De la fecha que nos devuelve, no nos vale la hora. La hora sera la del grupo
                    If (fechaPlanifEjec <> Date.MinValue) Then fechaPlanifEjec = New Date(fechaPlanifEjec.Year, fechaPlanifEjec.Month, fechaPlanifEjec.Day, oGrupo.Hora.Hour, oGrupo.Hora.Minute, 0)
                    grupoBLL.SaveEjecucion(oGrupo, Modulo.Tipo, idCab, fechaPlanifEjec)
                Catch batzEx As Sablib.BatzException
                    log.Error("Ha ocurrido un error al guardar la ejecucion." & batzEx.Termino, batzEx.Excepcion)
                End Try
                Dim frmResul As New frmResul(itzultzaileWeb.Itzuli("datosGuardados"), True)
                frmResul.Show()
            End If
        Catch ex As Exception
            log.Error("Ha ocurrido un error al guardar los datos del formulario", ex)
            Modulo.CanClose = True
            Dim frmResul As New frmResul(itzultzaileWeb.Itzuli("errGuardar") & vbCrLf & itzultzaileWeb.Itzuli("Contacte con el administrador"), False)
            frmResul.Show()
        End Try
    End Sub

    ''' <summary>
    ''' Avisa por email de las lineas marcadas como no ok
    ''' </summary>
    ''' <param name="idCab">Id de la cabecera</param>
    ''' <param name="lineasNoEnabled">Lista de los puntos que estan pendientes de solucionarse y no se debe avisar en el email</param>    
    Private Sub AvisarPorEmail(ByVal idCab As Integer, ByVal lineasNoEnabled As List(Of Integer))
        Try
            log.Info("Se va a proceder a avisar por email de las incidencias marcadas como no ok")
            Dim hojaCabBLL As New BLL.HojaCabBLL
            Dim hojaLinBLL As New BLL.HojasLineasBLL
            Dim maqBLL As New BLL.MaquinaBLL
            Dim userBLL As New Sablib.BLL.UsuariosComponent
            Dim puntBLL As New BLL.PuntoBLL
            Dim punt As ELL.Clases.Punto
            Dim cabecera As ELL.Clases.HojaCab = hojaCabBLL.consultar(idCab, True)
            Dim maq As ELL.Clases.Maquina = maqBLL.consultar(New ELL.Clases.Maquina With {.Id = cabecera.IdMaquina})
            Dim oUser As Sablib.ELL.Usuario = userBLL.GetUsuario(New Sablib.ELL.Usuario With {.Id = cabecera.IdUsuario}, False)
            Dim numIncidencias As Integer = 0
            Dim subject, body, link, emailTo_Directo, emailTo_Portal As String
            If (lineasNoEnabled IsNot Nothing) Then
                'Se quitan las lineas de los puntos no habilitados que no se avisaran
                Dim idPuntLinea As Integer
                For index As Integer = cabecera.Lineas.Count - 1 To 0 Step -1
                    idPuntLinea = cabecera.Lineas.Item(index).idPunto
                    If (lineasNoEnabled.Exists(Function(o As Integer) o = idPuntLinea)) Then
                        cabecera.Lineas.RemoveAt(index)
                    End If
                Next
            End If
            If (cabecera.Lineas.Count > 0) Then
                subject = "Automantenimiento: Incidencias no ok"
                body = "<b>Fecha:</b> " & cabecera.Fecha.ToShortDateString & " " & cabecera.Fecha.ToShortTimeString
                body &= "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Maquina:</b> " & maq.Descripcion
                If (Not String.IsNullOrEmpty(maq.PC)) Then body &= " (" & maq.PC & ")"
                body &= "<br /><br /><b>Usuario:</b> " & oUser.NombreCompleto & "<br /><br />"
                body &= "Los siguientes puntos han sido marcados como no ok:<br /><br />"
                body &= "<table border='1'>"
                body &= "<tr><th style='background-color:#F3F3F3;'>Punto</th></tr>"
                For Each lin As ELL.Clases.HojaLineas In cabecera.Lineas
                    If (lin.Estado = 0) Then 'no ok
                        body &= "<tr>"
                        punt = puntBLL.consultar(lin.idPunto)
                        link = "<a href='https://intranet2.batz.es/Automantenimiento/Index_Directo.aspx?idLin=" & lin.Id & "'>" & punt.RangoKultura(Modulo.Ticket.Culture) & "</a>"
                        body &= "<td>" & link & "</td>"
                        body &= "</tr>"
                        numIncidencias += 1
                    End If
                Next
                If (numIncidencias > 0) Then
                    emailTo_Directo = String.Empty : emailTo_Portal = String.Empty
                    Dim oUserMant As Sablib.ELL.Usuario
                    Dim perfBLL As New BLL.PerfilesBLL
                    Dim avisosBLL As New BLL.AvisosBLL
                    Dim perfil As ELL.Clases.Perfil
                    Dim lAvisos As List(Of ELL.Clases.Avisos) = avisosBLL.consultarListado(New ELL.Clases.Avisos With {.IdPlanta = Modulo.IdPlanta, .NegocioUser = Modulo.Negocio})
                    If (lAvisos IsNot Nothing AndAlso lAvisos.Count > 0) Then
                        Dim paramBLL As New SabLib.BLL.ParametrosBLL
                        Dim oParam As SabLib.ELL.Parametros = paramBLL.consultar
                        For Each oAviso As ELL.Clases.Avisos In lAvisos
                            oUserMant = oAviso.GetUsuario
                            If (oUserMant IsNot Nothing) Then
                                perfil = perfBLL.consultar(oUserMant.Id, Modulo.IdPlanta)
                                If (perfil.AccesoDirecto) Then  'Acceso directo
                                    emailTo_Directo &= If(emailTo_Directo <> String.Empty, ";", "") & oUserMant.Email
                                Else 'Acesso portal
                                    emailTo_Portal &= If(emailTo_Portal <> String.Empty, ";", "") & oUserMant.Email
                                End If
                            Else
                                log.Warn("Aviso de incidencias: No se ha podido encontrar el usuario de mantenimiento para el usuario " & oAviso.IdUser)
                            End If
                        Next
                        If (emailTo_Directo <> String.Empty Or emailTo_Portal <> String.Empty) Then
                            If (emailTo_Directo <> String.Empty) Then
                                SabLib.BLL.Utils.EnviarEmail(Configuration.ConfigurationManager.AppSettings("emailFrom"), emailTo_Directo, subject, body, oParam.ServidorEmail)
                                log.Info("Aviso de incidencias: Se ha avisado por email (Directo) de " & numIncidencias & " incidencias marcada como no ok a: " & emailTo_Directo)
                            End If
                            If (emailTo_Portal <> String.Empty) Then
                                body = body.Replace("Index_Directo.aspx", "Index.aspx")  'Reemplazamos las rutas para que acceda a la pagina del portal y no a la del directo si no le pertenece
                                SabLib.BLL.Utils.EnviarEmail(Configuration.ConfigurationManager.AppSettings("emailFrom"), emailTo_Portal, subject, body, oParam.ServidorEmail)
                                log.Info("Aviso de incidencias: Se ha avisado por email (Portal) de " & numIncidencias & " incidencias marcada como no ok a: " & emailTo_Portal)
                            End If
                        Else
                            log.Warn("Aviso de incidencias: No se ha avisado a ningun usuario de las incidencias")
                        End If
                    Else
                        log.Warn("Aviso de incidencias: No existe ningun usuario para avisar de las incidencias")
                    End If
                Else
                    log.Warn("Aviso de incidencias: No se ha avisado por email porque todos los puntos marcados como no ok, estaban inhabilitados")
                End If
            Else
                log.Warn("Aviso de incidencias: No se ha avisado por email porque todos los puntos marcados como no ok, estaban inhabilitados")
            End If            
        Catch ex As Exception
            log.Error("Ha ocurrido un error al intentar avisar por email de incidencias marcadas como no ok", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve a la pagina de Login por si se ha equivocado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        log.Warn("Se sale de la session para iniciar con otro numero de trabajador")
        Modulo.Ticket = Nothing
        Me.Hide()
        callForm.Inicializar()
        callForm.Show()
    End Sub

#End Region

End Class