Imports AutomntoLib

Public Class frmAvisoAuditoria

#Region "Inicializacion"

    ''' <summary>
    ''' Se carga el formulario de automantenimiento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub frmAvisoAuditoria_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Try
            ConfigurarVentana()
            CargarDatos()
            btnAceptar.Focus()  'Se asigna el foco al boton
            Traduccion()
        Catch ex As Exception
            log.Error("Ha ocurrido un error al cargar el formulario de aviso de auditoria", ex)
            MessageBox.Show(itzultzaileWeb.Itzuli("Error al cargar el formulario").ToUpper, "Carga", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Configura los parametros de la ventana
    ''' </summary>    
    Private Sub ConfigurarVentana()
        Me.TopMost = True  'Para que sea modal
        Me.TopLevel = True
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
    Private Sub frmAvisoAuditoria_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        If (Not Modulo.CanClose) Then e.Cancel = True
    End Sub

    ''' <summary>
    ''' Traduce los labels del formulario
    ''' </summary>    
    Private Sub Traduccion()
        labelTitulo.Text = itzultzaileWeb.Itzuli("Aviso de auditoria").ToUpper
        labelInfo.Text = itzultzaileWeb.Itzuli("Tiene que auditar los siguientes grupos") & ":"
        btnAceptar.Text = itzultzaileWeb.Itzuli("Aceptar")
    End Sub

#End Region

#Region "Cargar datos"

    ''' <summary>
    ''' Carga los grupos pendientes
    ''' </summary>    
    Private Sub CargarDatos()        
        Dim lbl As New Label        
        Dim pnlContenido As New Panel
        pnlContenido.Size = New Size(Me.Width - 50, 50)
        pnlContenido.Left = labelInfo.Left - 20 : pnlContenido.Top = labelInfo.Top - 15
        pnlContenido.AutoSize = True
        Me.Controls.Add(pnlContenido)
        Dim xGrupo As Integer = pnlContenido.Location.X
        Dim yGrupo As Integer = pnlContenido.Location.Y
        Dim maqBLL As New BLL.MaquinaBLL
        Dim grupBLL As New BLL.GrupoBLL
        Dim oMaq As ELL.Clases.Maquina
        Dim oGrup As ELL.Clases.Grupo
        For Each iGrup As Integer In Modulo.IdGrupos
            lbl = New Label()
            lbl.Name = "lblGrupo" & iGrup
            lbl.Font = New Font("Arial", 18, FontStyle.Bold)
            lbl.Location = New Drawing.Point(xGrupo, yGrupo)            
            lbl.AutoSize = True
            oGrup = grupBLL.consultar(iGrup)
            lbl.Text = " - " & oGrup.Nombre
            If (oGrup.IdMaquina > 0) Then
                oMaq = maqBLL.consultar(New ELL.Clases.Maquina With {.Id = oGrup.IdMaquina})
                lbl.Text &= " (" & oMaq.DescripcionCompleta & ")"
            End If
            lbl.Text &= vbCrLf
            pnlContenido.Controls.Add(lbl)
            yGrupo += lbl.Height
        Next
        btnAceptar.Top = Me.Height - 150
        btnAceptar.Left = 50
        btnAceptar.Width = Me.Width - 100
        btnAceptar.Height = 100
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Se cierra el formulario
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click        
        log.Info("AVISO_AUDITORIA: Se da por enterado de que tiene auditorias pendientes por realizar")
        Try
            Dim automntoBLL As New BLL.AutomntoBLL
            Dim paramBLL As New BLL.ParametroBLL
            Dim servidor As String = String.Empty
            Dim oParam As ELL.Clases.Parametros = paramBLL.consultar(Modulo.IdPlanta)
            If (oParam IsNot Nothing) Then servidor = oParam.Servidor
            automntoBLL.UpdateComunicacion(New ELL.Clases.ComunicacionEquipo With {.Equipo = Equipo, .Aplicacion = ELL.Clases.ComunicacionEquipo.Aplication.Formulario, .IdPlanta = Modulo.IdPlanta, .FechaFin = Now, .Servidor = servidor})
        Catch batzEx As Sablib.BatzException
            log.Error("Ha fallado la comunicacion al salir del aviso de auditoria del formulario")
        End Try
        Environment.Exit(0)  'Con el Application.Exit no funcionaba
    End Sub

#End Region

End Class