Imports TelefoniaLib

Public Class TarifaDatos
    Inherits PageBase

    Dim oTarifa As ELL.Telefono.TarifaDatos
    Dim tarifaComp As New BLL.TelefonoComponent

#Region "Constantes"

    Private Const LISTADO As String = "Tarifa datos"
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
            itzultzaileWeb.Itzuli(labelTarifa) : itzultzaileWeb.Itzuli(chbMostrarObsoletos) : itzultzaileWeb.Itzuli(labelTarifa2)
            itzultzaileWeb.Itzuli(btnNuevo) : itzultzaileWeb.Itzuli(btnVerDatos) : itzultzaileWeb.Itzuli(btnVolver)
            itzultzaileWeb.Itzuli(chbObsoleto) : itzultzaileWeb.Itzuli(btnGuardar) : itzultzaileWeb.Itzuli(btnEliminar)
        End If
    End Sub

    ''' <summary>
    ''' Carga el listado de tarifas existentes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                titulo.Texto = LISTADO
                cargarTarifas()
            End If
            ConfigurarEventos()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para configurar ejecucion de scripts a controles, asignacion de textos,..
    ''' </summary>
    Private Sub ConfigurarEventos()
        Dim sms As String = itzultzaileWeb.Itzuli("confirmarEliminar")
        cfEliminar.ConfirmText = sms

        lbLista.Attributes.Add("onchange", "javascript:seleccionarItem();")
        lbLista.Attributes.Add("onDblClick", "javascript:dobleClick();")
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Visualiza un pop-up con la informacion de una tarifa, donde se puede modificar o eliminar o registrar uno nuevo
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
    ''' Carga los datos de una tarifa
    ''' </summary>
    ''' <param name="idTarifa">Identificador de la tarifa</param>
    Private Sub mostrarDetalle(ByVal idTarifa As Integer)
        Try
            If (idTarifa = Integer.MinValue) Then  'nuevo
                titulo.Texto = NUEVO
                txtTarifa.Text = String.Empty                
                chbObsoleto.Checked = False
                pnlObsoleto.Visible = False
                chbObsoleto.Attributes.Add("initialValue", False)
            Else 'modificar
                titulo.Texto = INFO
                oTarifa = tarifaComp.loadTarifa(idTarifa)

                txtTarifa.Text = oTarifa.Nombre
                chbObsoleto.Checked = oTarifa.Obsoleto
                pnlObsoleto.Visible = True
                chbObsoleto.Attributes.Add("initialValue", oTarifa.Obsoleto)
            End If

            btnGuardar.CommandArgument = idTarifa
            mvTarifas.ActiveViewIndex = 1
        Catch ex As Exception
            Dim batzEx As New BatzException("errVerInfo", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Lista las tarifas obsoletos o no obsoletos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub chbMostrarObsoleto_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chbMostrarObsoletos.CheckedChanged
        cargarTarifas()
    End Sub

#End Region

#Region "Cargar Datos"

    ''' <summary>
    ''' Carga todas las tarifas de la planta que se esta gestionando
    ''' </summary>
    Private Sub cargarTarifas()
        Try
            Dim listTarifas As List(Of ELL.Telefono.TarifaDatos)
            oTarifa = New ELL.Telefono.TarifaDatos With {.IdPlanta = Master.Ticket.IdPlantaActual, .Obsoleto = chbMostrarObsoletos.Checked}
            listTarifas = tarifaComp.loadListTarifas(Not chbMostrarObsoletos.Checked, Master.Ticket.IdPlantaActual)

            lbLista.Items.Clear()

            Ordenar(listTarifas, ELL.Telefono.TarifaDatos.PropertyNames.NOMBRE, SortDirection.Ascending)

            lbLista.DataSource = listTarifas
            lbLista.DataTextField = ELL.Telefono.TarifaDatos.PropertyNames.NOMBRE
            lbLista.DataValueField = ELL.Telefono.TarifaDatos.PropertyNames.ID
            lbLista.DataBind()

            'Lista auxiliar
            lbAuxiliar.Items.Clear()
            lbAuxiliar.DataSource = listTarifas
            lbAuxiliar.DataTextField = ELL.Telefono.TarifaDatos.PropertyNames.NOMBRE
            lbAuxiliar.DataValueField = ELL.Telefono.TarifaDatos.PropertyNames.ID
            lbAuxiliar.DataBind()

            txtBuscar.Text = String.Empty

            mvTarifas.ActiveViewIndex = 0
        Catch ex As Exception
            Dim batzEx As New BatzException("errMostrarDatos", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Guardar Datos"

    ''' <summary>
    ''' Guarda la tarifa si es nueva y la modifica si ya existiera    
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGuardar.Click
        Try
            If (txtTarifa.Text = String.Empty) Then
                Master.MensajeAdvertenciaKey = "debeRellenarDatos"
            Else
                oTarifa = New ELL.Telefono.TarifaDatos

                If (CInt(btnGuardar.CommandArgument) <> Integer.MinValue) Then oTarifa.Id = CInt(btnGuardar.CommandArgument)
                oTarifa.Nombre = txtTarifa.Text
                oTarifa.IdPlanta = Master.Ticket.IdPlantaActual
                oTarifa.Obsoleto = chbObsoleto.Checked

                tarifaComp.SaveTarifa(oTarifa)
                Master.MensajeInfoKey = "datosGuardados"
                If (oTarifa.Id <> Integer.MinValue) Then
                    Dim mensa As String = "Se ha modificado la tarifa de datos - " & oTarifa.Nombre & " (" & oTarifa.Id & ")"
                    If (oTarifa.Obsoleto And Not CType(chbObsoleto.Attributes.Item("initialValue"), Boolean)) Then
                        mensa &= " y se ha marcado como obsoleto"
                    ElseIf (Not oTarifa.Obsoleto And CType(chbObsoleto.Attributes.Item("initialValue"), Boolean)) Then
                        mensa &= " y se ha desmarcado como obsoleto"
                    End If
                    log.Info(mensa)
                Else
                    log.Info("Se ha insertado una nueva tarifa de datos - " & oTarifa.Nombre)
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
    ''' Ordena la lista de tarifas
    ''' </summary>
    ''' <param name="lTarifas">Lista de tarifas</param>
    ''' <param name="sortExpr">Campo por el que hay que ordenar</param>
    ''' <param name="sortDir">Direccion de ordenacion</param>
    Private Sub Ordenar(ByRef lTarifas As List(Of ELL.Telefono.TarifaDatos), ByVal sortExpr As String, ByVal sortDir As SortDirection)
        Try
            Select Case sortExpr
                Case ELL.Telefono.TarifaDatos.PropertyNames.NOMBRE
                    lTarifas.Sort(Function(oTarif1 As ELL.Telefono.TarifaDatos, oTarif2 As ELL.Telefono.TarifaDatos) _
                                       If(sortDir = SortDirection.Ascending, oTarif1.Nombre < oTarif2.Nombre, oTarif1.Nombre > oTarif2.Nombre))
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
        mvTarifas.ActiveViewIndex = 0
        cargarTarifas()
    End Sub

#End Region

End Class