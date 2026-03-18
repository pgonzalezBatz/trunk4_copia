Imports AutomntoLib

Public Class frmResul

#Region "Constructor"

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="Texto">Texto a mostrar</param>
    ''' <param name="bIconOk">True si se muestra el icono de ok y false en caso contrario</param>    
    Public Sub New(ByVal Texto As String, ByVal bIconOk As Boolean)
        InitializeComponent()
        lblTexto.Text = Texto
        If (bIconOk) Then
            pcbImagen.Image = Image.FromFile(Modulo.ImagenesConfig & "Ok_Big.png")
        Else
            pcbImagen.Image = Image.FromFile(Modulo.ImagenesConfig & "Error_Big.png")
        End If
    End Sub

#End Region

#Region "Carga del formulario"

    ''' <summary>
    ''' Carga el formulario de resultado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Resultado_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Try
            ConfigurarVentana()
            'Se centra el contenido del formulario
            pcbImagen.Left = (Me.Width - pcbImagen.Width) / 2
            pcbImagen.Top = 100
            btnSalir.Top = Me.Height - 150
            btnSalir.Left = 50
            btnSalir.Width = Me.Width - 75
            btnSalir.Height = 100
            If (Modulo.IdGrupos.Count > 1) Then
                btnSalir.Text = itzultzaileWeb.Itzuli("Siguiente grupo")
            Else
                btnSalir.Text = itzultzaileWeb.Itzuli("Salir")
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
        Me.TopMost = True  'Para que sea modal
        Me.TopLevel = True
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None  'Para que no tenga botones de minimizar y cerrar
        'Establece como dimensiones de la ventana las de la pantalla
        Me.Width = My.Computer.Screen.Bounds.Width
        Me.Height = My.Computer.Screen.Bounds.Height
        'Situa la ventana en el punto 0,0
        Me.Location = New Drawing.Point(0, 0)
        Modulo.CanClose = True  'Para que se pueda cerrar
        'Se centra el contenido del formulario
        pnlContenido.Left = (Me.Width - pnlContenido.Width) / 2
        pnlContenido.Top = (Me.Height - pnlContenido.Height) / 2
    End Sub

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Se finaliza la aplicacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>   
    Private Sub btnSalir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSalir.Click
        Try
            Dim automntoBLL As New AutomntoLib.BLL.AutomntoBLL
            Dim paramBLL As New AutomntoLib.BLL.ParametroBLL
            Dim servidor As String = String.Empty
            Dim oParam As AutomntoLib.ELL.Clases.Parametros = paramBLL.consultar(Modulo.IdPlanta)
            If (oParam IsNot Nothing) Then servidor = oParam.Servidor
            automntoBLL.UpdateComunicacion(New AutomntoLib.ELL.Clases.ComunicacionEquipo With {.Equipo = Equipo, .Aplicacion = AutomntoLib.ELL.Clases.ComunicacionEquipo.Aplication.Formulario, .IdPlanta = Modulo.IdPlanta, .FechaFin = Now, .Servidor = servidor})
        Catch batzEx As SabLib.BatzException
            log.Error("Ha fallado la comunicacion al salir correctamente del formulario")
        End Try
        SiguienteEjecucion()
    End Sub

    ''' <summary>
    ''' Comprueba si tiene una siguiente ejecucion
    ''' </summary>
    Private Sub SiguienteEjecucion()
        Try
            'Se quita el grupo ejecutado
            Modulo.IdGrupos.Remove(Modulo.IdGrupoEjecucion)
            If (Modulo.IdGrupos.Count > 0) Then
                log.Info("Como tiene mas grupos, se ejecuta el siguiente")
                Modulo.IdGrupoEjecucion = Modulo.IdGrupos.First
                Dim grupBLL As New BLL.GrupoBLL
                Dim maqBLL As New BLL.MaquinaBLL
                Dim oGrup As ELL.Clases.Grupo = grupBLL.consultar(Modulo.IdGrupoEjecucion)
                'Se buscan todas las que tengan el nombre del equipo, porque pueden existir mas de una para un mismo equipo
                Dim lMaq As List(Of ELL.Clases.Maquina) = maqBLL.consultarListado(New ELL.Clases.Maquina With {.PC = Modulo.Equipo, .IdPlanta = Modulo.IdPlanta})
                If (lMaq Is Nothing Or (lMaq IsNot Nothing AndAlso lMaq.Count = 0)) Then
                    log.Warn("No se ha encontrado ninguna maquina asociada al equipo " & Modulo.Equipo)
                    Application.Exit()
                    Exit Sub
                Else
                    Dim oMaq As ELL.Clases.Maquina = lMaq.Find(Function(o As ELL.Clases.Maquina) o.Id = oGrup.IdMaquina)
                    If (oMaq Is Nothing) Then
                        log.Warn("No se ha encontrado la maquina asociada al grupo " & oGrup.Id)
                        Application.Exit()
                        Exit Sub
                    Else
                        log.Info("Se va a ejecutar el siguiente grupo " & Modulo.IdGrupoEjecucion)
                        Dim automntoBLL As New BLL.AutomntoBLL
                        Dim paramBLL As New BLL.ParametroBLL
                        Dim oParam As ELL.Clases.Parametros = paramBLL.consultar(Modulo.IdPlanta)
                        Dim servidor As String = If(oParam IsNot Nothing, oParam.Servidor, String.Empty)
                        Modulo.IdMaquina = oMaq.Id
                        Modulo.DescMaquina = oMaq.DescripcionCompleta
                        Modulo.Negocio = oMaq.Maq_UnidadNegocio
                        automntoBLL.UpdateComunicacion(New ELL.Clases.ComunicacionEquipo With {.Equipo = Equipo, .Aplicacion = ELL.Clases.ComunicacionEquipo.Aplication.Formulario, .IdPlanta = IdPlanta, .FechaInicio = Now, .Servidor = servidor})
                        Me.Hide()
                        Dim formHoja As New frmHoja(New frmLogin)
                        formHoja.ShowDialog()
                        Me.Close()
                    End If
                End If
            End If
        Catch ex As Exception
            log.Error("Ha ocurrido un error comprobando si tiene mas ejecuciones", ex)
        End Try
        Application.Exit()
    End Sub

#End Region

End Class