Imports TelefoniaLib

Partial Public Class Extension
    Inherits PageBase

    Dim oExt As ELL.Extension
    Dim extComp As BLL.ExtensionComponent

#Region "Page Load"

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelExt) : itzultzaileWeb.Itzuli(chbMostrarObsoletos) : itzultzaileWeb.Itzuli(btnModificar)
            itzultzaileWeb.Itzuli(btnNuevo) : itzultzaileWeb.Itzuli(labelNoExt) : itzultzaileWeb.Itzuli(labelTipo)
            itzultzaileWeb.Itzuli(labelExt2) : itzultzaileWeb.Itzuli(labelFactA) : itzultzaileWeb.Itzuli(labelNombre)
            itzultzaileWeb.Itzuli(labelVisible) : itzultzaileWeb.Itzuli(labelNumDirecto) : itzultzaileWeb.Itzuli(labelTipoLinea)
            itzultzaileWeb.Itzuli(labelAlveolo) : itzultzaileWeb.Itzuli(labelVisible2) : itzultzaileWeb.Itzuli(labelNoMovil)
            itzultzaileWeb.Itzuli(labelAsigA) : itzultzaileWeb.Itzuli(labelPerso) : itzultzaileWeb.Itzuli(labelExt3)
            itzultzaileWeb.Itzuli(labelOtros) : itzultzaileWeb.Itzuli(chkObsoleto) : itzultzaileWeb.Itzuli(btnGuardar)
            itzultzaileWeb.Itzuli(btnEliminar) : itzultzaileWeb.Itzuli(btnVolver) : itzultzaileWeb.Itzuli(labelPrestamo)
        End If
    End Sub

    ''' <summary>
    ''' Carga el listado de extensiones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Listado de extensiones"
                cargarExtensiones()
                setPaneles(False, False)
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
    ''' Visualiza un pop-up con la informacion de una extension, donde se puede modificar o eliminar o registrar uno nuevo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnVerDetalle(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim btn As Button = CType(sender, Button)
            Dim idExt As Integer = Integer.MinValue

            If (btn.CommandName = "modificar") Then
                If (lbLista.SelectedIndex = -1) Then
                    Master.MensajeAdvertencia = "seleccioneExtension"
                    Exit Sub
                Else
                    idExt = lbLista.SelectedValue
                End If
            End If

            btnEliminar.CommandArgument = idExt
            btnGuardar.CommandArgument = idExt

            mostrarDetalle(idExt)

        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Dim batzEx As New BatzException("errVerDatos", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Carga los datos de una extension
    ''' </summary>
    ''' <param name="idExt">Identificador de la extension</param>
    Private Sub mostrarDetalle(ByVal idExt As Integer)
        Try
            Dim titulo As String = String.Empty
            inicializarControles(idExt)
            inicializarViewstate()
            If (idExt = Integer.MinValue) Then  'nuevo
                Master.SetTitle = "Nueva extension"
                'ddlExtension.Enabled = True
                'btnEliminar.Visible = False
                chkObsoleto.Checked = False
                chkObsoleto.Attributes.Add("initialValue", False)
                pnlObsoleto.Visible = False
                ddlTipo.Enabled = True
                ViewState("IdExtInterna") = Integer.MinValue
                ViewState("Obsoleta") = False
                setPaneles(False, False)
            Else 'modificar
                Dim tipoInterna As Boolean = False
                Master.SetTitle = "Detalle"

                extComp = New BLL.ExtensionComponent
                oExt = New ELL.Extension
                oExt.Id = lbLista.SelectedValue
                oExt = extComp.getExtension(oExt, Master.Ticket.IdPlantaActual)

                lblNumero.Text = oExt.Extension
                ddlTipo.SelectedValue = oExt.IdTipoExtension
                ddlTipo.Enabled = False
                chkObsoleto.Checked = oExt.Obsoleto
                chkObsoleto.Attributes.Add("initialValue", oExt.Obsoleto)
                ViewState("Telefono") = oExt.IdTelefono
                ViewState("Obsoleta") = oExt.Obsoleto

                'Se guarda si se podra guardar como obsoleto o no
                Dim bSave As Boolean = True
                If (oExt.ListaPersonasAsig IsNot Nothing AndAlso oExt.ListaPersonasAsig.Count > 0) Then bSave = False
                If (oExt.ListaDepartamentosAsig IsNot Nothing AndAlso oExt.ListaDepartamentosAsig.Count > 0) Then bSave = False
                If (oExt.ListaOtrosAsig IsNot Nothing AndAlso oExt.ListaOtrosAsig.Count > 0) Then bSave = False
                ViewState("Save") = bSave

                If (oExt.IdTipoExtension = ELL.Extension.TipoExt.interna) Then
                    tipoInterna = True

                    cargarAsociarA()
                    Dim idAsoc As Integer = oExt.IdTipoAsignacion
                    ViewState("IdTipoAsignacionInt") = oExt.IdTipoAsignacion

                    ddlExtension.SelectedIndex = ddlExtension.Items.IndexOf(ddlExtension.Items.FindByValue(idAsoc))
                    cargarFacturadoA()
                    ddlFacturado.SelectedIndex = ddlFacturado.Items.IndexOf(ddlFacturado.Items.FindByValue(oExt.IdDepartamentoFac))

                    txtNombre.Text = oExt.Nombre
                    chbVisibleInt.Checked = oExt.Visible
                    chkNumDirecto.Checked = (oExt.IdTelefono <> Integer.MinValue)

                    pnlNumero.Visible = chkNumDirecto.Checked
                    cargarNumerosTlfnoLibres(ELL.Telefono.FijoMovil.fijo, oExt.IdTelefono)
                    ddlNumero.SelectedValue = oExt.IdTelefono

                    cargarTiposLinea()
                    ddlTipoLinea.SelectedValue = oExt.IdTipoLinea
                    If (oExt.IdAlveolo <> Integer.MinValue) Then
                        pnlAlveolo.Visible = True
						cargarAlveolos()
						'Cargar alveolos, solo carga los libres asi que le añadimos el que tenga
						Dim alvComp As New BLL.AlveoloComponent
						Dim oAlv As New ELL.Alveolo With {.Id = oExt.IdAlveolo}
						oAlv = alvComp.getAlveolo(oAlv)
						ddlAlveolo.Items.Add(New ListItem(oAlv.Ruta, oExt.IdAlveolo))
						ddlAlveolo.SelectedValue = oExt.IdAlveolo
						'ddlAlveolo.SelectedIndex = ddlAlveolo.Items.IndexOf(ddlAlveolo.Items.FindByValue(oExt.IdAlveolo))
                    End If
                Else
                    lblNumero.Text = oExt.Extension
                    chbVisibleMov.Checked = oExt.Visible
                    chbPrestamo.Checked = oExt.Prestamo
                    cargarNumerosTlfnoLibres(ELL.Telefono.FijoMovil.movil, oExt.IdTelefono)
                    ddlMovil.SelectedIndex = ddlMovil.Items.IndexOf(ddlMovil.Items.FindByValue(oExt.IdTelefono))
                    cargarAsignarA()
                    pnlAsignarV.Visible = True

                    'Por defecto
                    pnlAsignarVPersona.Visible = False
                    pnlAsignarVExtension.Visible = False
                    pnlAsignarVOtros.Visible = False
                    ddlAsignarV.Visible = False

                    ddlAsignarA.SelectedValue = ELL.Extension.AsignarA.sinAsignar
                    ViewState("TipoAsignacion") = ELL.Extension.AsignarA.sinAsignar
                    oExt.TipoAsignacionMovil = ELL.Extension.AsignarA.sinAsignar

                    If (oExt.ListaPersonasAsig.Count > 0) Then
                        cargarPersonas()
						'ddlAsignarV.SelectedValue = oExt.ListaPersonasAsig.Item(0).IdUsuario  'Si se ha dado de baja, da error
						ddlAsignarV.SelectedIndex = ddlAsignarV.Items.IndexOf(ddlAsignarV.Items.FindByValue(oExt.ListaPersonasAsig.Item(0).IdUsuario))
                        ddlAsignarA.SelectedValue = ELL.Extension.AsignarA.persona
                        ViewState("AsignadoA") = oExt.ListaPersonasAsig.Item(0).IdUsuario
                        ViewState("TipoAsignacion") = ELL.Extension.AsignarA.persona
                        oExt.TipoAsignacionMovil = ELL.Extension.AsignarA.persona
                        pnlAsignarVPersona.Visible = True
                        ddlAsignarV.Visible = True
                    ElseIf (oExt.IdExtensionInterna <> Integer.MinValue) Then
                        cargarExtensionesInternasLibres(oExt.IdExtensionInterna)
                        ddlAsignarV.SelectedIndex = ddlAsignarV.Items.IndexOf(ddlAsignarV.Items.FindByValue(oExt.IdExtensionInterna))
                        ddlAsignarA.SelectedValue = ELL.Extension.AsignarA.extensionInterna
                        ViewState("AsignadoA") = oExt.IdExtensionInterna
                        ViewState("TipoAsignacion") = ELL.Extension.AsignarA.extensionInterna
                        oExt.TipoAsignacionMovil = ELL.Extension.AsignarA.extensionInterna
                        pnlAsignarVExtension.Visible = True
                        ddlAsignarV.Visible = True
                    ElseIf (oExt.ListaOtrosAsig.Count > 0) Then
                        cargarOtros()                        
                        ddlAsignarV.SelectedIndex = ddlAsignarV.Items.IndexOf(ddlAsignarV.Items.FindByValue(oExt.ListaOtrosAsig.Item(0).IdOtros))
                        ddlAsignarA.SelectedValue = ELL.Extension.AsignarA.otros
                        ViewState("AsignadoA") = oExt.ListaOtrosAsig.Item(0).IdOtros
                        ViewState("TipoAsignacion") = ELL.Extension.AsignarA.otros
                        oExt.TipoAsignacionMovil = ELL.Extension.AsignarA.otros
                        pnlAsignarVOtros.Visible = True
                        ddlAsignarV.Visible = True
                    End If

                    ViewState("IdExtInterna") = oExt.IdExtensionInterna
                End If

                'btnEliminar.Visible = True
                pnlObsoleto.Visible = True
                setPaneles(tipoInterna, Not tipoInterna)
            End If

            mvExtensiones.ActiveViewIndex = 1
            Master.SetTitle = titulo

        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
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
        cargarExtensiones()
    End Sub

#End Region

#Region "Limpiar controles y gestion paneles"

    ''' <summary>
    ''' Limpia los controles para poder registrar un nuevo item
    ''' </summary>
    ''' <param name="idExt">Identificador de la extension</param>
    Private Sub inicializarControles(ByVal idExt As Integer)
        'Elementos comunes
        lblNumero.Text = String.Empty
        txtNumero.Text = String.Empty
        pnlNuevo.Visible = (idExt = Integer.MinValue)
        pnlExistente.Visible = Not pnlNuevo.Visible
        pnlAlveolo.Visible = False
        pnlNumero.Visible = False
        chbPrestamo.Checked = False
        cargarTipoExtensiones()
    End Sub

    ''' <summary>
    ''' Inicializa las variables del viewstate que se utilizan para saber si ha realizado algun cambio que implique reasignaciones
    ''' </summary>
    Private Sub inicializarViewstate()
        ViewState("Telefono") = Nothing        
        ViewState("TipoAsignacion") = Nothing
        ViewState("IdTipoExtension") = Nothing
        ViewState("AsignadoA") = Nothing
        ViewState("Obsoleta") = Nothing
    End Sub

    ''' <summary>
    ''' Indica que panel se visualizara y cual no 
    ''' </summary>
    ''' <param name="pInterna">True si se visualizara el panel interna</param>
    ''' <param name="pMovil">True si se visualizara el panel movil</param>    
    Private Sub setPaneles(ByVal pInterna As Boolean, ByVal pMovil As Boolean)
        pnlInterna.Visible = pInterna
        pnlMovil.Visible = pMovil
    End Sub

#End Region

#Region "Cargar Datos"

    ''' <summary>
    ''' Carga las extensiones existentes
    ''' </summary>
    Private Sub cargarExtensiones()
        Try
            Dim listExt As List(Of ELL.Extension)
            Dim extComp As New BLL.ExtensionComponent
            Dim oExt As New ELL.Extension With {.IdPlanta = Master.Ticket.IdPlantaActual, .Obsoleto = chbMostrarObsoletos.Checked}
            'para listar todas, el idTipoExt a integer.minvalue
            listExt = extComp.getExtensiones(oExt, Master.Ticket.IdPlantaActual, False)

            Ordenar(listExt, ELL.Extension.PropertyNames.EXTENSION, SortDirection.Ascending)

            lbLista.Items.Clear()

            lbLista.DataSource = listExt
            lbLista.DataTextField = ELL.Extension.PropertyNames.EXTENSION
            lbLista.DataValueField = ELL.Extension.PropertyNames.ID
            lbLista.DataBind()

            'Lista auxiliar
            lbAuxiliar.Items.Clear()
            lbAuxiliar.DataSource = listExt
            lbAuxiliar.DataTextField = ELL.Extension.PropertyNames.EXTENSION
            lbAuxiliar.DataValueField = ELL.Extension.PropertyNames.ID
            lbAuxiliar.DataBind()

            txtBuscar.Text = String.Empty
        Catch ex As Exception
            Throw New BatzException("errMostrarExtensiones", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga las opciones de si una extension es interna o movil tipos que pueden ser un movil
    ''' </summary>            
    Private Sub cargarTipoExtensiones()
        Try
            If (ddlTipo.Items.Count = 0) Then
                Dim termino, name As String
                ddlTipo.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))

                For Each idTipo As Integer In [Enum].GetValues(GetType(ELL.Extension.TipoExt))
                    name = [Enum].GetName(GetType(ELL.Extension.TipoExt), idTipo)
                    termino = itzultzaileWeb.Itzuli(name)
                    If (termino = String.Empty) Then termino = name

                    ddlTipo.Items.Add(New ListItem(termino, idTipo))
                Next
            End If
            ddlTipo.SelectedIndex = 0
        Catch ex As Exception
            Throw New BatzException("errMostrarTipos", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga a quien estara asociada
    ''' </summary>            
    Private Sub cargarAsociarA()
        Try
            If (ddlExtension.Items.Count = 0) Then
                Dim termino, name As String            

                ddlExtension.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))

                For Each idAsoc As Integer In [Enum].GetValues(GetType(ELL.Extension.AsociarA))
                    name = [Enum].GetName(GetType(ELL.Extension.AsociarA), idAsoc)
                    termino = itzultzaileWeb.Itzuli(name)
                    If (termino = String.Empty) Then termino = name

                    ddlExtension.Items.Add(New ListItem(termino, idAsoc))
                Next
            End If

            ddlExtension.SelectedIndex = 0
        Catch ex As Exception
            Throw New BatzException("errMostrarTipos", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga del desplegable donde se elegira, si elegir extension interna o persona
    ''' </summary>            
    Private Sub cargarAsignarA()
        Try
            If (ddlAsignarA.Items.Count = 0) Then
                Dim termino, name As String

                ddlAsignarA.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))

                For Each idAsig As Integer In [Enum].GetValues(GetType(ELL.Extension.AsignarA))
                    name = [Enum].GetName(GetType(ELL.Extension.AsignarA), idAsig)
                    termino = itzultzaileWeb.Itzuli(name)
                    If (termino = String.Empty) Then termino = name

                    ddlAsignarA.Items.Add(New ListItem(termino, idAsig))
                Next
            End If

            ddlAsignarA.SelectedIndex = 0
        Catch ex As Exception
            Throw New BatzException("errMostrarTipos", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los tipos de linea de una extension interna
    ''' </summary>            
    Private Sub cargarTiposLinea()
        Try
            If (ddlTipoLinea.Items.Count = 0) Then
                Dim cultura As String = Master.Ticket.Culture
                Dim tipoLineaComp = New BLL.TipoLineaComponent
                Dim oTipoL As New ELL.TipoLinea
                oTipoL.IdTipoExtension = ELL.Extension.TipoExt.interna
                oTipoL.Cultura = cultura
                oTipoL.IdTipoExtension = ddlTipo.SelectedValue
                Dim listTipos As List(Of ELL.TipoLinea) = tipoLineaComp.getTiposLinea(oTipoL)
                ddlTipoLinea.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))

                ddlTipoLinea.DataSource = listTipos
                ddlTipoLinea.DataTextField = ELL.TipoLinea.PropertyNames.NOMBRE
                ddlTipoLinea.DataValueField = ELL.TipoLinea.PropertyNames.ID
                ddlTipoLinea.DataBind()
            End If

            ddlTipoLinea.SelectedIndex = 0
        Catch ex As Exception
            Throw New BatzException("errMostrarTiposLinea", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga todos los alveolos
    ''' </summary>            
    Private Sub cargarAlveolos()
        Try
            If (ddlAlveolo.Items.Count = 0) Then
                Dim cultura As String = Master.Ticket.Culture
                Dim alvComp = New BLL.AlveoloComponent
                'Dim oAlv As New ELL.Alveolo With {.IdPlanta = Master.Ticket.IdPlantaActual}
                Dim listAlv As List(Of ELL.Alveolo) = alvComp.getAlveolosLibres(Master.Ticket.IdPlantaActual)
                ddlAlveolo.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))

                ddlAlveolo.DataSource = listAlv
                ddlAlveolo.DataTextField = ELL.Alveolo.PropertyNames.RUTA
                ddlAlveolo.DataValueField = ELL.Alveolo.PropertyNames.ID
                ddlAlveolo.DataBind()
            End If
            ddlAlveolo.SelectedIndex = 0
        Catch ex As Exception
            Throw New BatzException("errMostrandoAlveolos", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga todas las personas
    ''' </summary>            
    Private Sub cargarPersonas()
        Try
            'If (ddlAsignarA.Items.Count = 0) Then
            Dim cultura As String = Master.Ticket.Culture
            Dim userComp As New SABLib.BLL.UsuariosComponent
            Dim oUser As New SABLib.ELL.Usuario
            Dim lPlantas As New List(Of SABLib.ELL.Planta)
            Dim oPlanta As New SABLib.ELL.Planta
            oPlanta.Id = Master.Ticket.IdPlantaActual
            lPlantas.Add(oPlanta)
            oUser.Plantas = lPlantas
            oUser.IdPlanta = oPlanta.Id
			Dim listUsu As List(Of SABLib.ELL.Usuario) = userComp.GetUsuariosPlanta(oUser)
            ddlAsignarV.Items.Clear()
            ddlAsignarV.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))

            listUsu.Sort(Function(o1 As SABLib.ELL.Usuario, o2 As SABLib.ELL.Usuario) o1.NombreCompleto < o2.NombreCompleto)

            ddlAsignarV.DataSource = listUsu
            ddlAsignarV.DataTextField = SABLib.ELL.Usuario.ColumnNames.NOMBRECOMPLETO
            ddlAsignarV.DataValueField = SABLib.ELL.Usuario.ColumnNames.ID
            ddlAsignarV.DataBind()

            'End If
            ddlAsignarV.SelectedIndex = 0
        Catch ex As Exception
            Throw New BatzException("errMostrandoPersonas", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los departamentos a los que se puede facturar
    ''' </summary>            
    Private Sub cargarFacturadoA()
        Try
            If (ddlFacturado.Items.Count = 0) Then
                Dim cultura As String = Master.Ticket.Culture
                Dim depComp As New BLL.DepartamentosComponent                
                Dim listDep As List(Of Sablib.ELL.Departamento) = depComp.getDepartamentos(BLL.DepartamentosComponent.EDepartamentos.Activos, Master.Ticket.IdPlantaActual)
                listDep.Sort(Function(o1 As Sablib.ELL.Departamento, o2 As Sablib.ELL.Departamento) o1.Nombre < o2.Nombre)
                ddlFacturado.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))
                ddlFacturado.DataSource = listDep
                ddlFacturado.DataTextField = "NOMBRE"
                ddlFacturado.DataValueField = "ID"
                ddlFacturado.DataBind()
                ddlFacturado.SelectedIndex = 0
            End If
        Catch ex As Exception
            Throw New BatzException("errMostrandoDepartamentos", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los numeros de telefonos que esten libres
    ''' </summary>            
    ''' <param name="fijoMovil">Indicara si ha de cargar los moviles o fijos</param>
    ''' <param name="idTlfno">Si viene informado, tambien se mostrar este numero</param>
    Private Sub cargarNumerosTlfnoLibres(ByVal fijoMovil As ELL.Telefono.FijoMovil, Optional ByVal idTlfno As Integer = Integer.MinValue)
        Try
            Dim drop As DropDownList = Nothing
            If (fijoMovil = ELL.Telefono.FijoMovil.fijo) Then
                drop = ddlNumero
            ElseIf (fijoMovil = ELL.Telefono.FijoMovil.movil) Then
                drop = ddlMovil
            End If
            Dim cultura As String = Master.Ticket.Culture
            Dim tlfnoComp As New BLL.TelefonoComponent
            Dim oTlfno As New ELL.Telefono
            oTlfno.IdPlanta = Master.Ticket.IdPlantaActual
            oTlfno.FijoOMovil = fijoMovil
            oTlfno.Id = idTlfno
            Dim listTlfno As List(Of ELL.Telefono) = tlfnoComp.getTelefonosLibres(oTlfno)

            drop.Items.Clear()
            drop.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))

            drop.DataSource = listTlfno
            drop.DataTextField = ELL.Telefono.PropertyNames.NUMERO
            drop.DataValueField = ELL.Telefono.PropertyNames.ID
            drop.DataBind()

            drop.SelectedIndex = 0
        Catch ex As Exception
            Throw New BatzException("errMostrarTelefonos", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga las extensiones existentes
    ''' </summary>    
    ''' <param name="idExt">Si idExtension no es integer.minValue, tambien se mostrara</param>
    ''' <remarks></remarks>
    Private Sub cargarExtensionesInternasLibres(ByVal idExt As Integer)
        Try
            Dim cultura As String = Master.Ticket.Culture
            Dim listExt As List(Of ELL.Extension)
            Dim extComp As New BLL.ExtensionComponent
            Dim oExt As New ELL.Extension
            oExt.IdTipoExtension = ELL.Extension.TipoExt.interna
            oExt.IdPlanta = Master.Ticket.IdPlantaActual
            oExt.IdExtensionInterna = idExt
            listExt = extComp.getExtensionesLibres(oExt)

            ddlAsignarV.Items.Clear()
            ddlAsignarV.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))

            ddlAsignarV.DataSource = listExt
            ddlAsignarV.DataTextField = ELL.Extension.PropertyNames.EXTENSION
            ddlAsignarV.DataValueField = ELL.Extension.PropertyNames.ID
            ddlAsignarV.DataBind()

            ddlAsignarV.SelectedIndex = 0
        Catch ex As Exception
            Throw New BatzException("errMostrarExtensiones", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga todas los items de otros
    ''' </summary>            
    Private Sub cargarOtros()
        Try
            Dim cultura As String = Master.Ticket.Culture
            Dim otrosComp As New BLL.OtrosComponent
            Dim oOtro As New ELL.Otros With {.IdPlanta = Master.Ticket.IdPlantaActual}
            Dim listOtros As List(Of ELL.Otros) = otrosComp.getOtros(oOtro)
            ddlAsignarV.Items.Clear()
            ddlAsignarV.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))

            ddlAsignarV.DataSource = listOtros
            ddlAsignarV.DataTextField = ELL.Otros.PropertyNames.NOMBRE
            ddlAsignarV.DataValueField = ELL.Otros.PropertyNames.ID
            ddlAsignarV.DataBind()

            'End If
            ddlAsignarV.SelectedIndex = 0
        Catch ex As Exception
            Throw New BatzException("errMostrandoPersonas", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los desplegables del detalle, dependiendo si es interna o no
    ''' </summary>
    ''' <param name="bInterna"></param>
    Private Sub cargarPanel(ByVal bInterna As Boolean)
        If (bInterna) Then
            cargarAsociarA()
            cargarFacturadoA()
            txtNombre.Text = String.Empty
            chbVisibleInt.Checked = False
            chkNumDirecto.Checked = False
            cargarNumerosTlfnoLibres(ELL.Telefono.FijoMovil.fijo)
            pnlNumero.Visible = False
            cargarTiposLinea()
            cargarAlveolos()
            pnlAlveolo.Visible = False
        Else
            chbVisibleInt.Checked = False
            cargarNumerosTlfnoLibres(ELL.Telefono.FijoMovil.movil)
            cargarAsignarA()
            pnlAsignarV.Visible = False
        End If
    End Sub

#End Region

#Region "Ordenar"

    ''' <summary>
    ''' Ordena la lista de extensiones
    ''' </summary>
    ''' <param name="lExtensiones">Lista de extensiones</param>
    ''' <param name="sortExpr">Campo por el que hay que ordenar</param>
    ''' <param name="sortDir">Direccion de ordenacion</param>
    Private Sub Ordenar(ByRef lExtensiones As List(Of ELL.Extension), ByVal sortExpr As String, ByVal sortDir As SortDirection)
        Try
            Select Case sortExpr
                Case ELL.Extension.PropertyNames.EXTENSION
                    lExtensiones.Sort(Function(oExt1 As ELL.Extension, oExt2 As ELL.Extension) _
                                       If(sortDir = SortDirection.Ascending, oExt1.Extension < oExt2.Extension, oExt1.Extension > oExt2.Extension))
            End Select
        Catch
        End Try
    End Sub

#End Region

#Region "Guardar Datos"

    ''' <summary>
    ''' Guarda el alveolo si es nuevo y lo modifica si ya existiera
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGuardar.Click
        Try
            Dim bSave As Boolean = True
            If (ddlTipo.SelectedIndex = 0) Then
                Master.MensajeAdvertencia = "debeRellenarDatos"
                WriteTitulo()
                Exit Sub
            ElseIf (ddlTipo.SelectedValue = ELL.Extension.TipoExt.interna) Then
                If (ddlExtension.SelectedIndex = 0 Or ddlFacturado.SelectedIndex = 0 Or txtNombre.Text = String.Empty Or _
                   (chkNumDirecto.Checked And ddlNumero.SelectedIndex = 0) Or ddlTipoLinea.SelectedIndex = 0 Or _
                   (pnlAlveolo.Visible And ddlAlveolo.SelectedIndex = 0)) Then
                    WriteTitulo()
                    Master.MensajeAdvertencia = "debeRellenarDatos"
                    Exit Sub
                End If
            ElseIf (ddlTipo.SelectedValue = ELL.Extension.TipoExt.movil) Then
                If (ddlMovil.SelectedIndex = 0 Or ddlAsignarA.SelectedIndex = 0 Or (ddlAsignarA.SelectedIndex > 0 And pnlAsignarV.Visible And ddlAsignarV.SelectedIndex = 0)) Then
                    WriteTitulo()
                    Master.MensajeAdvertencia = "debeRellenarDatos"
                    Exit Sub
                End If
            End If
            Dim oExtOld As ELL.Extension = Nothing
            extComp = New BLL.ExtensionComponent
            oExt = New ELL.Extension

            oExt.Id = CInt(btnGuardar.CommandArgument)
            If (oExt.Id = Integer.MinValue) Then
                oExt.Extension = CInt(txtNumero.Text.Trim)
            Else
                oExt.Extension = CInt(lblNumero.Text)
            End If

            oExt.IdPlanta = Master.Ticket.IdPlantaActual
            oExt.IdTipoExtension = ddlTipo.SelectedValue
            If (ddlTipo.SelectedValue = ELL.Extension.TipoExt.interna) Then
                oExt.IdTipoAsignacion = ddlExtension.SelectedValue
                oExt.IdDepartamentoFac = ddlFacturado.SelectedValue
                oExt.Nombre = txtNombre.Text.Trim
                oExt.Visible = chbVisibleInt.Checked
                If (chkNumDirecto.Checked) Then
                    oExt.IdTelefono = ddlNumero.SelectedValue
                End If
                oExt.IdTipoLinea = ddlTipoLinea.SelectedValue
                If (pnlAlveolo.Visible) Then
                    oExt.IdAlveolo = ddlAlveolo.SelectedValue
                End If

            Else  'movil
                oExt.Visible = chbVisibleMov.Checked
                oExt.Prestamo = chbPrestamo.Checked
                oExt.IdTelefono = ddlMovil.SelectedValue
                If (ddlAsignarA.SelectedValue = ELL.Extension.AsignarA.extensionInterna) Then
                    oExt.IdExtensionInterna = ddlAsignarV.SelectedValue
                    'no se le asigna nada en el idTipoAsignacion
                ElseIf (ddlAsignarA.SelectedValue = ELL.Extension.AsignarA.persona) Then
                    Dim listPer As New List(Of ELL.ExtensionUsuDep)
                    Dim oUser As New ELL.ExtensionUsuDep
                    oUser.IdUsuario = ddlAsignarV.SelectedValue
                    oUser.IdExtension = oExt.Id
                    oUser.IdTelefono = oExt.IdTelefono
                    listPer.Add(oUser)
                    oExt.ListaPersonasAsig = listPer
                    oExt.IdTipoAsignacion = ELL.Extension.AsociarA.personal
                ElseIf (ddlAsignarA.SelectedValue = ELL.Extension.AsignarA.otros) Then
                    Dim listOtros As New List(Of ELL.ExtensionUsuDep)
                    Dim oOtro As New ELL.ExtensionUsuDep
                    oOtro.IdOtros = ddlAsignarV.SelectedValue
                    oOtro.IdExtension = oExt.Id
                    oOtro.IdTelefono = oExt.IdTelefono
                    listOtros.Add(oOtro)
                    oExt.ListaOtrosAsig = listOtros
                    oExt.IdTipoAsignacion = ELL.Extension.AsociarA.otros
                ElseIf (ddlAsignarA.SelectedValue = ELL.Extension.AsignarA.sinAsignar) Then
                    oExt.IdTipoAsignacion = Integer.MinValue
                    oExt.TipoAsignacionMovil = ELL.Extension.AsignarA.sinAsignar
                End If
                'Añadido el 17/10/2011. Cuando una extension movil es obsoleta, se desliga
                If (chkObsoleto.Checked) Then
                    oExt.TipoAsignacionMovil = ELL.Extension.AsignarA.sinAsignar
                    oExt.IdExtensionInterna = Integer.MinValue
                End If                
            End If
            If (CType(ViewState("Obsoleta"), Boolean) And chkObsoleto.Checked) Then
                Master.MensajeAdvertencia = "No se pueden realizar cambios estando la extension obsoleta. Marquela como no obsoleta para que los cambios se realicen"
                Exit Sub
            End If
            oExt.Obsoleto = chkObsoleto.Checked

            'Si se va a poner como activa, se comprueba que el alveolo no exista ya
            If (Not oExt.Obsoleto AndAlso CType(ViewState("Obsoleta"), Boolean) AndAlso oExt.IdAlveolo <> Integer.MinValue) Then
                'Al cambiar de obsoleto a no obsoleto se comprueba si existe alguna extension con el idAlveolo
                Dim lExt As List(Of ELL.Extension) = extComp.getExtensiones(New ELL.Extension With {.IdAlveolo = oExt.IdAlveolo, .IdPlanta = Master.Ticket.IdPlantaActual}, Master.Ticket.IdPlantaActual, False)
                If (lExt IsNot Nothing AndAlso lExt.Count > 0) Then
                    Dim extension As Integer = 0
                    For Each mExt As ELL.Extension In lExt
                        If (mExt.Id <> oExt.Id) Then
                            extension = mExt.Extension
                        End If
                    Next
                    If (extension > 0) Then
                        Master.MensajeAdvertencia = "No se puede marcar como activa porque el alveolo ya existe para la extension " & extension
                        Exit Sub
                    End If
                End If
            End If

            If (oExt.Id <> Integer.MinValue And chkObsoleto.Checked) Then
                oExt.IdTelefono = Integer.MinValue
                oExt.IdTipoAsignacion = Integer.MinValue
                oExt.TipoAsignacionMovil = ELL.Extension.AsignarA.sinAsignar
                ViewState("TipoAsignacion") = ELL.Extension.AsignarA.sinAsignar                
            End If

            If (bSave) Then
                If (oExt.Id = Integer.MinValue) Then
                    'Para los nuevos, se comprueba que no exista
                    Dim oExtCons As New ELL.Extension With {.Extension = oExt.Extension}
                    If (extComp.getExtension(oExtCons, Master.Ticket.IdPlantaActual) IsNot Nothing) Then
                        'ya existe
                        bSave = False
                    End If
                End If

                If (bSave) Then
                    'Solo se comprobaran los cambios si no es una nueva extension
                    If (oExt.Id <> Integer.MinValue) Then 'And ddlTipo.SelectedValue <> ELL.Extension.TipoExt.interna) Then
                        oExtOld = ComprobarCambios(oExt)
                    End If

                    extComp.Save(oExt, oExtOld)
                    If (oExt.Id <> Integer.MinValue) Then
                        Dim mensa As String = "Se ha modificado la extension - " & oExt.Extension & " (" & oExt.Id & ")"
                        If (oExt.Obsoleto And Not CType(chkObsoleto.Attributes.Item("initialValue"), Boolean)) Then
                            mensa &= " y se ha marcado como obsoleto"
                        ElseIf (Not oExt.Obsoleto And CType(chkObsoleto.Attributes.Item("initialValue"), Boolean)) Then
                            mensa &= " y se ha desmarcado como obsoleto"
                        End If
                        log.Info(mensa)
                    Else
                        log.Info("Se ha insertado una nueva extension - " & oExt.Extension)
                    End If
                    volver()
                    Master.MensajeInfo = "extensionGuardada"
                Else
                    Master.MensajeAdvertencia = "extensionExistente"
                End If
            Else
                Master.MensajeAdvertencia = "noSePuedeBorrarPorEstarRelacionadoConTelefonosOPersonas"
            End If
            WriteTitulo()
        Catch batzEx As BatzException
            WriteTitulo()
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            WriteTitulo()
            Dim batzEx As New BatzException("errGuardarExtension", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Comprueba si ha habido cambios que impliquen reasignacion
    ''' Si no existen, devolvera nothing
    ''' </summary>
    ''' <param name="oExtActual">Objeto extension actual</param>
    ''' <returns></returns>
    Private Function ComprobarCambios(ByVal oExtActual As ELL.Extension) As ELL.Extension
        Dim oExtOld As ELL.Extension = Nothing
        Dim bCambios As Boolean = False
        Dim bAntesExtInt As Boolean = False
        Dim bQuitarListas As Boolean = False   'Cuando sea true, se quitaran las listas de la extension consultada ya que no haran falta porque se habra cambiado el telefono o la extension interna
        If (oExtActual.IdTipoExtension = ELL.Extension.TipoExt.interna) Then
            If ((ViewState("IdTipoAsignacionInt") IsNot Nothing AndAlso ViewState("IdTipoAsignacionInt") <> oExtActual.IdTipoAsignacion)) Then
                bCambios = True
                'No se le quita porque luego se le desasignara todo
            ElseIf (((ViewState("Telefono") Is Nothing Or (ViewState("Telefono") IsNot Nothing And CInt(ViewState("Telefono")) = Integer.MinValue)) And oExtActual.IdTelefono = Integer.MinValue) OrElse _
                (ViewState("Telefono") IsNot Nothing AndAlso ViewState("Telefono") <> oExtActual.IdTelefono)) Then
                bCambios = True
                bQuitarListas = True
            End If

        Else 'movil
            If ((ViewState("Telefono") Is Nothing And oExtActual.IdTelefono = Integer.MinValue) OrElse _
                (ViewState("Telefono") IsNot Nothing AndAlso ViewState("Telefono") <> oExtActual.IdTelefono)) Then
                bCambios = True
                bQuitarListas = (ViewState("TipoAsignacion") IsNot Nothing AndAlso ViewState("TipoAsignacion") = ELL.Extension.AsignarA.sinAsignar)
            ElseIf (ViewState("TipoAsignacion") IsNot Nothing) Then
                If (oExtActual.IdExtensionInterna = Integer.MinValue AndAlso ViewState("TipoAsignacion") = ELL.Extension.AsignarA.extensionInterna) Then  'Antes era una extension interna y ahora no
                    bCambios = True
                    bAntesExtInt = True
                ElseIf (oExtActual.ListaPersonasAsig IsNot Nothing AndAlso oExtActual.ListaPersonasAsig.Count > 0 AndAlso ViewState("TipoAsignacion") = ELL.Extension.AsignarA.persona AndAlso ViewState("AsignadoA") IsNot Nothing) Then
                    bCambios = (oExtActual.ListaPersonasAsig.Item(0).IdUsuario <> ViewState("AsignadoA"))
                ElseIf (oExtActual.ListaPersonasAsig IsNot Nothing AndAlso oExtActual.ListaPersonasAsig.Count > 0 AndAlso ViewState("TipoAsignacion") <> ELL.Extension.AsignarA.persona) Then
                    bCambios = True
                ElseIf (oExtActual.ListaOtrosAsig IsNot Nothing AndAlso oExtActual.ListaOtrosAsig.Count > 0 AndAlso ViewState("TipoAsignacion") = ELL.Extension.AsignarA.otros AndAlso ViewState("AsignadoA") IsNot Nothing) Then
                    bCambios = (oExtActual.ListaOtrosAsig.Item(0).IdOtros <> ViewState("AsignadoA"))
                ElseIf (oExtActual.ListaOtrosAsig IsNot Nothing AndAlso oExtActual.ListaOtrosAsig.Count > 0 AndAlso ViewState("TipoAsignacion") <> ELL.Extension.AsignarA.otros) Then
                    bCambios = True
                ElseIf (oExtActual.IdExtensionInterna <> Integer.MinValue AndAlso ViewState("TipoAsignacion") = ELL.Extension.AsignarA.extensionInterna AndAlso ViewState("AsignadoA") IsNot Nothing) Then
                    bCambios = (oExtActual.IdExtensionInterna <> ViewState("AsignadoA"))
                    bQuitarListas = True
                ElseIf (oExtActual.IdExtensionInterna <> Integer.MinValue AndAlso ViewState("TipoAsignacion") <> ELL.Extension.AsignarA.extensionInterna) Then
                    bCambios = True
                    bQuitarListas = True
                ElseIf (ViewState("TipoAsignacion") <> ELL.Extension.AsignarA.sinAsignar) Then
                    bCambios = True
                    bQuitarListas = True
                End If
            End If
        End If

        If (bCambios) Then
            Dim oExtAux As New ELL.Extension
            oExtAux.Id = oExt.Id
            oExtOld = extComp.getExtension(oExtAux, Master.Ticket.IdPlantaActual)            
            If (bQuitarListas) Then
                oExtOld.ListaPersonasAsig = Nothing
                oExtOld.ListaDepartamentosAsig = Nothing
                oExtOld.ListaOtrosAsig = Nothing
            End If
            If (bAntesExtInt) Then  'Como antes era una extension interna, hay que cargar en ListaPersonasAsig una fila con el usuario, id_ext_interna y el id_telefono de la extension interna
                oExtOld.ListaPersonasAsig = Nothing
                oExtOld.ListaDepartamentosAsig = Nothing
                oExtOld.ListaOtrosAsig = Nothing

                Dim oExtUsu As New ELL.ExtensionUsuDep
                oExtAux = New ELL.Extension With {.Id = CInt(ViewState("IdExtInterna"))}
                oExtAux = extComp.getExtension(oExtAux, Master.Ticket.IdPlantaActual)
                If (oExtAux.ListaPersonasAsig IsNot Nothing AndAlso oExtAux.ListaPersonasAsig.Count > 0) Then
                    oExtAux.ListaPersonasAsig.Sort(Function(o1 As ELL.ExtensionUsuDep, o2 As ELL.ExtensionUsuDep) o1.FechaDesde > o2.FechaDesde)  'Ordenamos para quedarnos con la fecha desde mayor el primer registro
                    If (oExtAux.ListaPersonasAsig.Item(0).FechaHasta = DateTime.MinValue) Then
                        oExtUsu.IdExtension = oExt.Id   'aqui pondremos la extension movil
                        oExtUsu.IdTelefono = oExtAux.ListaPersonasAsig.Item(0).IdTelefono
                        oExtUsu.FechaDesde = oExtAux.ListaPersonasAsig.Item(0).FechaDesde
                        oExtUsu.FechaHasta = Date.Now
                        oExtUsu.IdUsuario = oExtAux.ListaPersonasAsig.Item(0).IdUsuario
                        oExtOld.ListaPersonasAsig = New List(Of ELL.ExtensionUsuDep)
                        oExtOld.ListaPersonasAsig.Add(oExtUsu)
                    End If
                End If
            End If
        End If

        Return oExtOld
    End Function

#End Region

#Region "Cambios y visualizacion de paneles respectivos"

    ''' <summary>
    ''' Visualiza u oculta, los numeros de telefonos libres que puede seleccionar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub chkNumDirecto_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkNumDirecto.CheckedChanged
        pnlNumero.Visible = chkNumDirecto.Checked
        WriteTitulo()
    End Sub

    ''' <summary>
    ''' Al seleccionarse un tipo de linea, se tiene que comprobar si requiere alveolo para mostrarlo o no
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ddlTipoLinea_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTipoLinea.SelectedIndexChanged
        Try
            Dim visualizar As Boolean = False
            Dim tipoLinComp As New BLL.TipoLineaComponent
            Dim oTipo As ELL.TipoLinea
            If (ddlTipoLinea.SelectedIndex > 0) Then
                oTipo = tipoLinComp.getTipoLinea(ddlTipoLinea.SelectedValue)
                visualizar = oTipo.RequiereAlveolo
            End If
            pnlAlveolo.Visible = visualizar
            If (visualizar) Then cargarAlveolos() 'Si se tiene que visualizar los alveolos, se cargan por si no estuvieran cargados
            WriteTitulo()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se selecciona, de que tipo va a ser la extension, si interna o movil
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ddlTipo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTipo.SelectedIndexChanged
        If (ddlTipo.SelectedIndex > 0) Then
            Dim bInterna As Boolean = (ddlTipo.SelectedValue = ELL.Extension.TipoExt.interna)
            cargarPanel(bInterna)
            setPaneles(bInterna, Not bInterna)
        Else
            setPaneles(False, False)
        End If
        pnlAsignarVOtros.Visible = False
        pnlAsignarVPersona.Visible = False
        pnlAsignarVExtension.Visible = False
        WriteTitulo()
    End Sub

    ''' <summary>
    ''' Si se selecciona extension interna, mostrar estas, sino las personas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ddlAsignarA_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAsignarA.SelectedIndexChanged
        Dim visible As Boolean = False
        If (ddlAsignarA.SelectedIndex = 1) Then  'Extension interna
            visible = True
            pnlAsignarVPersona.Visible = False
            pnlAsignarVExtension.Visible = True
            pnlAsignarVOtros.Visible = False
            cargarExtensionesInternasLibres(CInt(ViewState("IdExtInterna")))
        ElseIf (ddlAsignarA.SelectedIndex = 2) Then 'personas
            visible = True
            pnlAsignarVPersona.Visible = True
            pnlAsignarVExtension.Visible = False
            pnlAsignarVOtros.Visible = False
            cargarPersonas()
        ElseIf (ddlAsignarA.SelectedIndex = 3) Then 'otros
            visible = True
            pnlAsignarVPersona.Visible = False
            pnlAsignarVExtension.Visible = False
            pnlAsignarVOtros.Visible = True
            cargarOtros()
        ElseIf (ddlAsignarA.SelectedIndex = 4) Then 'sin asignar
            visible = False
            pnlAsignarVPersona.Visible = False
            pnlAsignarVExtension.Visible = False
            pnlAsignarVOtros.Visible = False
        End If

        pnlAsignarV.Visible = visible
        ddlAsignarV.Visible = visible
        WriteTitulo()
    End Sub

#End Region

#Region "Write Titulo"

    ''' <summary>
    ''' Escribe el titulo de la popup
    ''' </summary>
    Private Sub WriteTitulo()
        If (lblNumero.Visible) Then
            Master.SetTitle = "Detalle"
        Else
            Master.SetTitle = "Nueva extension"
        End If
    End Sub

#End Region

#Region "Volver"

    ''' <summary>
    ''' Vuelve a la vista del listado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolver.Click
        volver()
    End Sub

    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>
    Private Sub volver()
        Master.SetTitle = "Listado de extensiones"
        mvExtensiones.ActiveViewIndex = 0
    End Sub

#End Region

End Class
