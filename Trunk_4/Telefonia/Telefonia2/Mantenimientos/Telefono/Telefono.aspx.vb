Imports TelefoniaLib

Partial Public Class Telefono
    Inherits PageBase

    Dim oTlfno As ELL.Telefono
    Dim tlfnoComp As BLL.TelefonoComponent

#Region "Constantes"

    Private Const NUEVO_TELEFONO As String = "nuevoTelefono"
    Private Const INFO_TELEFONO As String = "infoTelefono"
    Private Const LISTADO As String = "listadoTelefonos"

#End Region

#Region "Page Load"

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelTlfno) : itzultzaileWeb.Itzuli(chbMostrarObsoletos) : itzultzaileWeb.Itzuli(btnModificar)
            itzultzaileWeb.Itzuli(btnNuevo) : itzultzaileWeb.Itzuli(labelTlfno2) : itzultzaileWeb.Itzuli(labelEmp)
            itzultzaileWeb.Itzuli(labeltlfno3) : itzultzaileWeb.Itzuli(labelTipo) : itzultzaileWeb.Itzuli(labelModelo)
            itzultzaileWeb.Itzuli(labelPin) : itzultzaileWeb.Itzuli(labelPuk) : itzultzaileWeb.Itzuli(labelDual)
            itzultzaileWeb.Itzuli(labelRoaming) : itzultzaileWeb.Itzuli(labelPerfilMov) : itzultzaileWeb.Itzuli(labelTipo2)
            itzultzaileWeb.Itzuli(labelGestor) : itzultzaileWeb.Itzuli(labelComen) : itzultzaileWeb.Itzuli(chkObsoleto)
            itzultzaileWeb.Itzuli(btnGuardar) : itzultzaileWeb.Itzuli(btnEliminar) : itzultzaileWeb.Itzuli(btnVolver)
            itzultzaileWeb.Itzuli(labelTarifa)
        End If
    End Sub

    ''' <summary>
    ''' Carga el listado de numeros directos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                cargarTelefonos()
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

#Region "Botones Ver info e nuevo"

    ''' <summary>
    ''' Visualiza la informacion de un telefono, donde se puede modificar o eliminar o registrar uno nuevo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnVerDetalle(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim btn As Button = CType(sender, Button)
            Dim idTlfno As Integer = Integer.MinValue

            If (btn.CommandName = "modificar") Then
                If (lbLista.SelectedIndex = -1) Then
                    Master.MensajeAdvertenciaKey = "seleccioneTelefono"
                    Exit Sub
                Else
                    idTlfno = lbLista.SelectedValue
                End If
            End If
            mostrarDetalle(idTlfno)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Dim batzEx As New BatzException("errVerDatos", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Carga los datos de un telefono
    ''' </summary>
    ''' <param name="idTlfno">Identificador del tlfno</param>
    Private Sub mostrarDetalle(ByVal idTlfno As Integer)
        Try
            Dim titulo As String
            inicializarControles()
            If (idTlfno = Integer.MinValue) Then  'nuevo
                titulo = NUEVO_TELEFONO
                'btnEliminar.Visible = False
                pnlObsoleto.Visible = False
                btnGuardar.CommandArgument = String.Empty
                chkObsoleto.Checked = False
                chkObsoleto.Attributes.Add("initialValue", False)
                'btnEliminar.CommandArgument = String.Empty
            Else 'modificar
                titulo = INFO_TELEFONO
                tlfnoComp = New BLL.TelefonoComponent
                oTlfno = New ELL.Telefono()
                oTlfno.Id = lbLista.SelectedValue
                oTlfno = tlfnoComp.getTelefono(oTlfno)
                lblNumero.Text = oTlfno.Numero
                ddlTelefono.SelectedIndex = ddlTelefono.Items.IndexOf(ddlTelefono.Items.FindByValue(oTlfno.FijoOMovil))
                ddlCiaTlfno.SelectedIndex = ddlCiaTlfno.Items.IndexOf(ddlCiaTlfno.Items.FindByValue(oTlfno.IdCiaTlfno))                
                txtModelo.Text = oTlfno.Modelo
                txtPin.Text = oTlfno.PIN
                txtPuk.Text = oTlfno.PUK
                chbDualizado.Checked = oTlfno.Dualizado
                chbRoaming.Checked = oTlfno.Roaming
                ddlTipo.SelectedIndex = ddlTipo.Items.IndexOf(ddlTipo.Items.FindByValue(oTlfno.VozODatos))
                ddlTipoFijo.SelectedIndex = ddlTipoFijo.Items.IndexOf(ddlTipoFijo.Items.FindByValue(oTlfno.Tipo_LineaFijo))
                ddlGestores.SelectedValue = oTlfno.IdUsuarioGestor
                txtComentarios.Text = oTlfno.Comentarios
                chkObsoleto.Checked = oTlfno.Obsoleto
                chkObsoleto.Attributes.Add("initialValue", oTlfno.Obsoleto)
                ddlPerfilMov.SelectedIndex = ddlPerfilMov.Items.IndexOf(ddlPerfilMov.Items.FindByValue(oTlfno.IdPerfilMovil))
                ddlTarifas.SelectedIndex = ddlTarifas.Items.IndexOf(ddlTarifas.Items.FindByValue(oTlfno.IdTarifaDatos))
                'btnEliminar.Visible = True
                setPaneles(False, oTlfno.EsMovil, Not oTlfno.EsMovil)
                btnGuardar.CommandArgument = idTlfno
                pnlObsoleto.Visible = True
                'btnEliminar.CommandArgument = idTlfno
            End If
            ddlTelefono.Enabled = (idTlfno = Integer.MinValue)
            titPopPup.Texto = titulo
            mvTelefonos.ActiveViewIndex = 1
        Catch ex As Exception
            Dim batzEx As New BatzException("errVerInfo", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Lista las compañias obsoletos o no obsoletos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub chbMostrarObsoleto_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chbMostrarObsoletos.CheckedChanged
        cargarTelefonos()
    End Sub

#End Region

#Region "Limpiar controles y gestion paneles"

    ''' <summary>
    ''' Limpia los controles para poder registrar un nuevo item
    ''' </summary>
    Private Sub inicializarControles()
        lblNumero.Text = String.Empty
        txtNumero.Text = String.Empty
        txtModelo.Text = String.Empty
        txtPin.Text = String.Empty
        txtPuk.Text = String.Empty
        chbDualizado.Checked = False
        chbRoaming.Checked = True   'Por defecto estara activo
        txtComentarios.Text = String.Empty
        cargarCompañias()
        cargarTiposTlfno()
        cargarTipos()
        cargarTiposLineaFijo()
        cargarGestores()
        cargarPerfilesMovil()
        cargarTarifaDatos()
        setPaneles(True, False, False)
    End Sub

    ''' <summary>
    ''' Visualiza u oculta los paneles parametrizados
    ''' </summary>
    ''' <param name="pNuevo">Visualizar u ocultar el panel de nuevo telefono</param>
    ''' <param name="pMovil">Visualizar u ocultar el panel de un movil</param>
    ''' <param name="pTlfno">Visualizar u ocultar el panel de un telefono</param>
    Private Sub setPaneles(ByVal pNuevo As Boolean, ByVal pMovil As Boolean, ByVal pTlfno As Boolean)
        pnlNuevo.Visible = pNuevo
        pnlExistente.Visible = Not pNuevo
        pnlMovil.Visible = pMovil
        pnlTlfno.Visible = pTlfno
        pnlComun.Visible = pMovil Or pTlfno
    End Sub

#End Region

#Region "Cargar Datos"

    ''' <summary>
    ''' Carga todos los telefonos de la planta que se esta gestionando
    ''' </summary>
    Private Sub cargarTelefonos()
        Try
            Dim listTlfno As List(Of ELL.Telefono)
            Dim oTlfno As New ELL.Telefono With {.IdPlanta = Master.Ticket.IdPlantaActual, .Obsoleto = chbMostrarObsoletos.Checked}
            tlfnoComp = New BLL.TelefonoComponent
            listTlfno = tlfnoComp.getTelefonos(oTlfno)

            Ordenar(listTlfno, ELL.Telefono.PropertyNames.NUMERO, SortDirection.Ascending)

            lbLista.Items.Clear()

            lbLista.DataSource = listTlfno
            lbLista.DataTextField = ELL.Telefono.PropertyNames.NUMERO
            lbLista.DataValueField = ELL.Telefono.PropertyNames.ID
            lbLista.DataBind()

            'Lista auxiliar
            lbAuxiliar.Items.Clear()
            lbAuxiliar.DataSource = listTlfno
            lbAuxiliar.DataTextField = ELL.Telefono.PropertyNames.NUMERO
            lbAuxiliar.DataValueField = ELL.Telefono.PropertyNames.ID
            lbAuxiliar.DataBind()

            txtBuscar.Text = String.Empty
            mvTelefonos.ActiveViewIndex = 0

        Catch ex As Exception
            Dim batzEx As New BatzException("errMostrarTelefonos", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Carga las compañias telefonicas
    ''' </summary>    
    Private Sub cargarCompañias()
        Try
            If (ddlCiaTlfno.Items.Count = 0) Then
                Dim listCia As List(Of ELL.CiaTlfno)
                Dim ciaComp As New BLL.CiaTlfnoComponent
                Dim oCia As New ELL.CiaTlfno                
                oCia.IdPlanta = Master.Ticket.IdPlantaActual
                listCia = ciaComp.getCompañias(oCia)

                ddlCiaTlfno.Items.Clear()

                ddlCiaTlfno.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))

                ddlCiaTlfno.DataSource = listCia
                ddlCiaTlfno.DataTextField = ELL.CiaTlfno.PropertyNames.NOMBRE
                ddlCiaTlfno.DataValueField = ELL.CiaTlfno.PropertyNames.ID
                ddlCiaTlfno.DataBind()
            End If

            ddlCiaTlfno.SelectedIndex = 0

        Catch ex As Exception
            Dim batzEx As New BatzException("errMostrarCompañias", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Carga los tipos de telefono (fijo, movil)
    ''' </summary>    
    Private Sub cargarTiposTlfno()
        Try
            If (ddlTelefono.Items.Count = 0) Then
                Dim termino, name As String

                ddlTelefono.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))

                For Each idTipo As Integer In [Enum].GetValues(GetType(ELL.Telefono.FijoMovil))
                    If (idTipo <> ELL.Telefono.FijoMovil.null) Then
                        name = [Enum].GetName(GetType(ELL.Telefono.FijoMovil), idTipo)
                        termino = itzultzaileWeb.Itzuli(name)
                        If (termino = String.Empty) Then termino = name

                        ddlTelefono.Items.Add(New ListItem(termino, idTipo))
                    End If
                Next
            End If

            ddlTelefono.SelectedIndex = 0
        Catch ex As Exception
            Dim batzEx As New BatzException("errMostrarTelefonos", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Carga los tipos que pueden ser un movil
    ''' </summary>            
    Private Sub cargarTipos()
        Try
            If (ddlTipo.Items.Count = 0) Then
                Dim termino, name As String

                ddlTipo.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))

                For Each idTipo As Integer In [Enum].GetValues(GetType(ELL.Telefono.VozDatos))
                    If (idTipo <> ELL.Telefono.VozDatos.null) Then
                        name = [Enum].GetName(GetType(ELL.Telefono.VozDatos), idTipo)
                        termino = itzultzaileWeb.Itzuli(name)
                        If (termino = String.Empty) Then termino = name

                        ddlTipo.Items.Add(New ListItem(termino, idTipo))
                    End If
                Next
            End If
            ddlTipo.SelectedIndex = 0
        Catch ex As Exception
            Throw New BatzException("errMostrarTipos", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los tipos que pueden ser un fijo
    ''' </summary>            
    Private Sub cargarTiposLineaFijo()
        Try
            If (ddlTipoFijo.Items.Count = 0) Then
                Dim termino, name As String

                ddlTipoFijo.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))

                For Each idTipo As Integer In [Enum].GetValues(GetType(ELL.Telefono.tipoLineaFijo))
                    If (idTipo <> ELL.Telefono.tipoLineaFijo.null) Then
                        name = [Enum].GetName(GetType(ELL.Telefono.tipoLineaFijo), idTipo)
                        termino = itzultzaileWeb.Itzuli(name)
                        If (termino = String.Empty) Then termino = name

                        ddlTipoFijo.Items.Add(New ListItem(termino, idTipo))
                    End If
                Next
            End If

            ddlTipoFijo.SelectedIndex = 0
        Catch ex As Exception
            Throw New BatzException("errMostrarTipos", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los gestores que podran asignarse al telefono
    ''' </summary>            
    Private Sub cargarGestores()
        Try
            Dim gestComp As New BLL.TelefonoComponent.GestorTlfnoComponent
            If (ddlGestores.Items.Count = 0) Then
                ddlGestores.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))
                ddlGestores.DataSource = gestComp.getGestores(Master.Ticket.IdPlantaActual)
                ddlGestores.DataTextField = ELL.Telefono.GestorTlfno.PropertyNames.USER
                ddlGestores.DataValueField = ELL.Telefono.GestorTlfno.PropertyNames.ID_USER
                ddlGestores.DataBind()
            End If
            ddlGestores.SelectedIndex = 0
        Catch ex As Exception
            Throw New BatzException("errMostrarGestores", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los perfiles posibles de un movil
    ''' </summary>            
    Private Sub cargarPerfilesMovil()
        Try
            If (ddlPerfilMov.Items.Count = 0) Then
                Dim perfComp As New BLL.PerfilMovComponent

                ddlPerfilMov.Items.Add(New ListItem(itzultzaileWeb.Itzuli("ninguno"), Integer.MinValue))
                Dim lPerfiles As List(Of ELL.PerfilMovil) = perfComp.loadList(True, Master.Ticket.IdPlantaActual)
                If (lPerfiles IsNot Nothing) Then lPerfiles.Sort(Function(o1 As ELL.PerfilMovil, o2 As ELL.PerfilMovil) o1.Nombre < o2.Nombre)
                ddlPerfilMov.DataSource = lPerfiles
                ddlPerfilMov.DataTextField = ELL.PerfilMovil.PropertyNames.NOMBRE
                ddlPerfilMov.DataValueField = ELL.PerfilMovil.PropertyNames.ID
                ddlPerfilMov.DataBind()
            End If

            ddlPerfilMov.SelectedIndex = 0
        Catch ex As Exception
            Throw New BatzException("Error al mostrar los perfiles", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga las tarifa de datos
    ''' </summary>            
    Private Sub cargarTarifaDatos()
        Try
            If (ddlTarifas.Items.Count = 0) Then
                Dim tarifaComp As New BLL.TelefonoComponent

                ddlTarifas.Items.Add(New ListItem(itzultzaileWeb.Itzuli("ninguno"), Integer.MinValue))
                Dim lTarifas As List(Of ELL.Telefono.TarifaDatos) = tarifaComp.loadListTarifas(True, Master.Ticket.IdPlantaActual)
                If (lTarifas IsNot Nothing) Then lTarifas.Sort(Function(o1 As ELL.Telefono.TarifaDatos, o2 As ELL.Telefono.TarifaDatos) o1.Nombre < o2.Nombre)
                ddlTarifas.DataSource = lTarifas
                ddlTarifas.DataTextField = ELL.Telefono.TarifaDatos.PropertyNames.NOMBRE
                ddlTarifas.DataValueField = ELL.Telefono.TarifaDatos.PropertyNames.ID
                ddlTarifas.DataBind()
            End If

            ddlTarifas.SelectedIndex = 0
        Catch ex As Exception
            Throw New BatzException("Error al mostrar las tarifas", ex)
        End Try
    End Sub

#End Region

#Region "Guardar Datos"

    ''' <summary>
    ''' Guarda el telefono si es nuevo y lo modifica si ya existiera
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGuardar.Click
        Try
            Dim bSave As Boolean = True
            If faltanDatos() Then
                WriteTitulo()
                Master.MensajeAdvertenciaKey = "debeRellenarDatos"
            Else
                tlfnoComp = New BLL.TelefonoComponent
                oTlfno = New ELL.Telefono

                If (btnGuardar.CommandArgument = String.Empty) Then 'nuevo
                    oTlfno.Numero = txtNumero.Text
                    oTlfno.FechaAlta = Date.Now.Date
                Else 'antiguo
                    oTlfno.Id = CInt(btnGuardar.CommandArgument)
                    oTlfno.Numero = lblNumero.Text
                End If
                oTlfno.FijoOMovil = ddlTelefono.SelectedValue
                oTlfno.IdCiaTlfno = ddlCiaTlfno.SelectedValue
                oTlfno.IdPlanta = Master.Ticket.IdPlantaActual

                If (oTlfno.FijoOMovil = ELL.Telefono.FijoMovil.movil) Then
                    oTlfno.PIN = txtPin.Text.Trim
                    oTlfno.PUK = txtPuk.Text.Trim
                    oTlfno.Modelo = txtModelo.Text.Trim
                    oTlfno.VozODatos = ddlTipo.SelectedValue
                    oTlfno.Dualizado = chbDualizado.Checked
                    oTlfno.Roaming = chbRoaming.Checked
                    If (ddlGestores.SelectedIndex <> 0) Then oTlfno.IdUsuarioGestor = ddlGestores.SelectedValue
                    If (ddlPerfilMov.SelectedIndex <> 0) Then oTlfno.IdPerfilMovil = ddlPerfilMov.SelectedValue
                    If (ddlTarifas.SelectedIndex <> 0) Then oTlfno.IdTarifaDatos = ddlTarifas.SelectedValue
                Else 'telefono                    
                    oTlfno.Tipo_LineaFijo = ddlTipoFijo.SelectedValue
                End If
                oTlfno.Comentarios = txtComentarios.Text.Trim
                oTlfno.Obsoleto = chkObsoleto.Checked

                If (oTlfno.Id <> Integer.MinValue And chkObsoleto.Checked) Then
                    'Se comprueba si esta asociado con alguna persona o extension
                    Dim tlfnoComp As New BLL.TelefonoComponent
                    Dim oTlfnoAux As New ELL.Telefono With {.Id = oTlfno.Id, .IdPlanta = oTlfno.IdPlanta}
                    oTlfnoAux = tlfnoComp.getTelefono(oTlfnoAux)
                    If (oTlfnoAux IsNot Nothing And oTlfnoAux.ListaPersonasAsig IsNot Nothing AndAlso oTlfnoAux.ListaPersonasAsig.Count > 0) Then
                        '04/07/13: Si una persona ya no tiene el telefono asignado, se podra guardar,sino no
                        Dim lListaActiv As List(Of ELL.TelefonoUsuDep) = oTlfnoAux.ListaPersonasAsig.FindAll(Function(o As ELL.TelefonoUsuDep) o.FechaHasta = Date.MinValue Or (o.FechaHasta <> Date.MinValue AndAlso o.FechaHasta > Now))
                        bSave = (lListaActiv IsNot Nothing AndAlso lListaActiv.Count = 0)
                    Else
                        Dim extComp As New BLL.ExtensionComponent
                        Dim oExt As New ELL.Extension With {.IdTelefono = oTlfno.Id}
                        bSave = (extComp.getExtensiones(oExt, Master.Ticket.IdPlantaActual, False).Count = 0)
                    End If
                End If

                If (bSave) Then
                    Dim smsSinGuardar As String = String.Empty
                    If (oTlfno.Id = Integer.MinValue) Then
                        'Para los nuevos, se comprueba que no exista o si existe que este marcado como obsoleto
                        Dim oTlfnoCons As New ELL.Telefono With {.Numero = oTlfno.Numero, .Obsoleto = True}  'Se indica obsoleto=true para que en getTelefonos, tambien traiga los telefonos
                        Dim lTlfnos As List(Of ELL.Telefono) = tlfnoComp.getTelefonos(oTlfnoCons)
                        If (lTlfnos IsNot Nothing AndAlso lTlfnos.Count > 0) Then 'Existe                            
                            For Each telefon As ELL.Telefono In lTlfnos
                                If (telefon.IdPlanta = Master.Ticket.IdPlantaActual) Then  'Si es la misma planta no se podra registrar de nuevo
                                    smsSinGuardar = itzultzaileWeb.Itzuli("telefonoRegistradoMismaPlanta")
                                    bSave = False
                                    Exit For
                                Else  'Tlfno de distinta planta: is no esta marcado como obsoleto, no se podra
                                    If (Not telefon.Obsoleto) Then
                                        smsSinGuardar = itzultzaileWeb.Itzuli("telefonoExistente") & " - "
                                        Dim plantComp As New SABLib.BLL.PlantasComponent
                                        Dim oPlant As SABLib.ELL.Planta = plantComp.GetPlanta(telefon.IdPlanta)
                                        smsSinGuardar &= oPlant.Nombre
                                        bSave = False
                                        Exit For
                                    End If
                                End If
                            Next
                        End If
                    End If
                    If (bSave) Then
                        tlfnoComp.Save(oTlfno)
                        If (oTlfno.Id <> Integer.MinValue) Then
                            Dim mensa As String = "Se ha modificado el telefono - " & oTlfno.Numero & " (" & oTlfno.Id & ")"
                            If (oTlfno.Obsoleto And Not CType(chkObsoleto.Attributes.Item("initialValue"), Boolean)) Then
                                mensa &= " y se ha marcado como obsoleto"
                            ElseIf (Not oTlfno.Obsoleto And CType(chkObsoleto.Attributes.Item("initialValue"), Boolean)) Then
                                mensa &= " y se ha desmarcado como obsoleto"
                            End If
                            log.Info(mensa)
                        Else
                            log.Info("Se ha insertado un nuevo telefono - " & oTlfno.Numero)
                        End If
                        volver()
                        Master.MensajeInfoKey = "telefonoGuardado"
                    Else
                        Master.MensajeAdvertencia = smsSinGuardar
                    End If
                Else
                    Master.MensajeAdvertenciaKey = "noSePuedeBorrarEstaRelacionadoConPersonaOExtension"
                End If
            End If
            WriteTitulo()
        Catch batzEx As BatzException
            WriteTitulo()
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            WriteTitulo()
            Dim batzEx As New BatzException("errGuardarTelefono", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Comprueba que se pueda guardar el telefono y que no falten datos
    ''' </summary>
    Private Function faltanDatos() As Boolean
        Dim faltan As Boolean = False
        If (((txtNumero.Text = String.Empty And btnGuardar.CommandArgument = String.Empty) Or (lblNumero.Text = String.Empty And btnGuardar.CommandArgument <> String.Empty)) Or ddlTelefono.SelectedIndex = 0) Then
            faltan = True
        ElseIf (ddlTelefono.SelectedValue = ELL.Telefono.FijoMovil.movil And ddlTipo.SelectedIndex = 0) Then
            faltan = True
        ElseIf (ddlTelefono.SelectedValue = ELL.Telefono.FijoMovil.fijo And ddlTipoFijo.SelectedIndex = 0) Then
            faltan = True
        End If

        Return faltan
    End Function

#End Region

#Region "Ordenar"

    ''' <summary>
    ''' Ordena la lista de telefonos
    ''' </summary>
    ''' <param name="lTelefonos">Lista de telefonos</param>
    ''' <param name="sortExpr">Campo por el que hay que ordenar</param>
    ''' <param name="sortDir">Direccion de ordenacion</param>
    Private Sub Ordenar(ByRef lTelefonos As List(Of ELL.Telefono), ByVal sortExpr As String, ByVal sortDir As SortDirection)
        Try
            Select Case sortExpr
                Case ELL.Telefono.PropertyNames.NUMERO
                    lTelefonos.Sort(Function(oTel1 As ELL.Telefono, oTel2 As ELL.Telefono) _
                                       If(sortDir = SortDirection.Ascending, oTel1.Numero < oTel2.Numero, oTel1.Numero > oTel2.Numero))
            End Select
        Catch
        End Try
    End Sub

#End Region

#Region "Write Titulo"

    ''' <summary>
    ''' Escribe el titulo de la popup
    ''' </summary>
    Private Sub WriteTitulo()
        If (lblNumero.Visible) Then
            titPopPup.Texto = INFO_TELEFONO
        Else
            titPopPup.Texto = NUEVO_TELEFONO
        End If
    End Sub

#End Region

#Region "Cambio de telefono-movil o viceversa"

    ''' <summary>
    ''' Cuando se cambia de telefono a movil o viceversa, se tendran que visualizar u ocultar algunos campos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ddlTelefono_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTelefono.SelectedIndexChanged
        Dim titulo As String
        pnlTlfno.Visible = (ddlTelefono.SelectedValue = ELL.Telefono.FijoMovil.fijo)
        pnlMovil.Visible = (ddlTelefono.SelectedValue = ELL.Telefono.FijoMovil.movil)
        pnlComun.Visible = pnlMovil.Visible Or pnlTlfno.Visible
        If (Not lblNumero.Visible) Then
            titulo = NUEVO_TELEFONO
        Else
            titulo = INFO_TELEFONO
        End If
        titPopPup.Texto = titulo
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
        Titulo.Texto = LISTADO
        mvTelefonos.ActiveViewIndex = 0
        cargarTelefonos()
    End Sub

#End Region

End Class