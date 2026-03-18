Imports TelefoniaLib

Public Class PerfilesMovil
    Inherits PageBase

    Dim oPerf As ELL.PerfilMovil
    Dim perfComp As New BLL.PerfilMovComponent

#Region "Constantes"

    Private Const LISTADO As String = "Perfiles movil"
    Private Const NUEVO As String = "Nuevo"
    Private Const INFO As String = "Informacion"

#End Region

#Region "Page Load"

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelPerfil) : itzultzaileWeb.Itzuli(chbMostrarObsoletos) : itzultzaileWeb.Itzuli(labelPerfil2)
            itzultzaileWeb.Itzuli(btnNuevo) : itzultzaileWeb.Itzuli(btnVerDatos) : itzultzaileWeb.Itzuli(labelTope)
            itzultzaileWeb.Itzuli(chbObsoleto) : itzultzaileWeb.Itzuli(btnGuardar) : itzultzaileWeb.Itzuli(btnEliminar)
            itzultzaileWeb.Itzuli(btnVolver)
        End If
    End Sub

    ''' <summary>
    ''' Carga el listado de perfiles existentes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                titulo.Texto = LISTADO
                cargarPerfiles()
            End If
            ConfigurarEventos()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para configurar ejecucion de scripts a controles, asignacion de textos,..
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ConfigurarEventos()
        Dim sms As String = itzultzaileWeb.Itzuli("confirmarEliminar")
        cfEliminar.ConfirmText = sms

        lbLista.Attributes.Add("onchange", "javascript:seleccionarItem();")
        lbLista.Attributes.Add("onDblClick", "javascript:dobleClick();")
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Visualiza un pop-up con la informacion de un perfil, donde se puede modificar o eliminar o registrar uno nuevo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnVerDetalle(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim btn As Button = CType(sender, Button)
            Dim idAlv As Integer = Integer.MinValue

            If (btn.CommandName = "modificar") Then
                If (lbLista.SelectedIndex = -1) Then
                    Master.MensajeAdvertenciaKey = "porFavorSeleccioneUnElementoDeLaLista"
                    Exit Sub
                Else
                    idAlv = lbLista.SelectedValue
                End If
            End If

            mostrarDetalle(idAlv)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Dim batzEx As New BatzException("errVerDatos", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Carga los datos de un perfil
    ''' </summary>
    ''' <param name="idPerf">Identificador del perfil</param>
    Private Sub mostrarDetalle(ByVal idPerf As Integer)
        Try
            If (idPerf = Integer.MinValue) Then  'nuevo
                titulo.Texto = NUEVO

                txtPerfil.Text = String.Empty
                txtTope.Text = String.Empty                
                chbObsoleto.Checked = False
                pnlObsoleto.Visible = False
                chbObsoleto.Attributes.Add("initialValue", False)
            Else 'modificar
                titulo.Texto = INFO

                perfComp = New BLL.PerfilMovComponent
                oPerf = perfComp.load(idPerf)

                txtPerfil.Text = oPerf.Nombre
                txtTope.Text = oPerf.Tope                
                chbObsoleto.Checked = oPerf.Obsoleto
                pnlObsoleto.Visible = True
                chbObsoleto.Attributes.Add("initialValue", oPerf.Obsoleto)
            End If

            btnGuardar.CommandArgument = idPerf
            mvPerfiles.ActiveViewIndex = 1

        Catch ex As Exception
            Dim batzEx As New BatzException("errVerInfo", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Lista los perfiles obsoletos o no obsoletos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub chbMostrarObsoleto_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chbMostrarObsoletos.CheckedChanged
        cargarPerfiles()
    End Sub

#End Region

#Region "Cargar Datos"

    ''' <summary>
    ''' Carga todos los perfiles de la planta que se esta gestionando
    ''' </summary>
    Private Sub cargarPerfiles()
        Try
            Dim listPerf As List(Of ELL.PerfilMovil)
            oPerf = New ELL.PerfilMovil With {.IdPlanta = Master.Ticket.IdPlantaActual, .Obsoleto = chbMostrarObsoletos.Checked}            
            listPerf = perfComp.loadList(Not chbMostrarObsoletos.Checked, Master.Ticket.IdPlantaActual)

            lbLista.Items.Clear()

            Ordenar(listPerf, ELL.PerfilMovil.PropertyNames.NOMBRE, SortDirection.Ascending)

            lbLista.DataSource = listPerf
            lbLista.DataTextField = ELL.PerfilMovil.PropertyNames.NOMBRE
            lbLista.DataValueField = ELL.PerfilMovil.PropertyNames.ID
            lbLista.DataBind()

            'Lista auxiliar
            lbAuxiliar.Items.Clear()
            lbAuxiliar.DataSource = listPerf
            lbAuxiliar.DataTextField = ELL.PerfilMovil.PropertyNames.NOMBRE
            lbAuxiliar.DataValueField = ELL.PerfilMovil.PropertyNames.ID
            lbAuxiliar.DataBind()

            txtBuscar.Text = String.Empty

            mvPerfiles.ActiveViewIndex = 0
        Catch ex As Exception
            Dim batzEx As New BatzException("errMostrarDatos", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Guardar Datos"

    ''' <summary>
    ''' Guarda el perfil si es nuevo y lo modifica si ya existiera    
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGuardar.Click
        Try
            If (txtPerfil.Text = String.Empty Or txtTope.Text = String.Empty) Then
                Master.MensajeAdvertenciaKey = "debeRellenarDatos"
            Else
                oPerf = New ELL.PerfilMovil

                If (CInt(btnGuardar.CommandArgument) <> Integer.MinValue) Then oPerf.Id = CInt(btnGuardar.CommandArgument)

                oPerf.Nombre = txtPerfil.Text
                oPerf.Tope = PageBase.DecimalValue(txtTope.Text)
                oPerf.IdPlanta = Master.Ticket.IdPlantaActual
                oPerf.Obsoleto = chbObsoleto.Checked

                perfComp.Save(oPerf)
                Master.MensajeInfoKey = "datosGuardados"                
                If (oPerf.Id <> Integer.MinValue) Then
                    Dim mensa As String = "Se ha modificado el perfil de moviles - " & oPerf.Nombre & " (" & oPerf.Id & ")"
                    If (oPerf.Obsoleto And Not CType(chbObsoleto.Attributes.Item("initialValue"), Boolean)) Then
                        mensa &= " y se ha marcado como obsoleto"
                    ElseIf (Not oPerf.Obsoleto And CType(chbObsoleto.Attributes.Item("initialValue"), Boolean)) Then
                        mensa &= " y se ha desmarcado como obsoleto"                   
                    End If
                    log.Info(mensa)
                Else
                    log.Info("Se ha insertado un nuevo perfil de moviles - " & oPerf.Nombre)
                End If
                volver()
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Dim batzEx As New BatzException("errGuardar", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Ordenar"

    ''' <summary>
    ''' Ordena la lista de perfiles
    ''' </summary>
    ''' <param name="lPerfiles">Lista de perfiles</param>
    ''' <param name="sortExpr">Campo por el que hay que ordenar</param>
    ''' <param name="sortDir">Direccion de ordenacion</param>
    Private Sub Ordenar(ByRef lPerfiles As List(Of ELL.PerfilMovil), ByVal sortExpr As String, ByVal sortDir As SortDirection)
        Try
            Select Case sortExpr
                Case ELL.PerfilMovil.PropertyNames.NOMBRE
                    lPerfiles.Sort(Function(oPerf1 As ELL.PerfilMovil, oPerf2 As ELL.PerfilMovil) _
                                       If(sortDir = SortDirection.Ascending, oPerf1.Nombre < oPerf2.Nombre, oPerf1.Nombre > oPerf2.Nombre))
            End Select
        Catch
        End Try
    End Sub

#End Region

#Region "Volver"

    ''' <summary>
    ''' Vuelve a la vista del listado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnVolver_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVolver.Click
        volver()
    End Sub


    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>
    Private Sub volver()
        titulo.Texto = LISTADO
        mvPerfiles.ActiveViewIndex = 0
        cargarPerfiles()
    End Sub

#End Region

End Class