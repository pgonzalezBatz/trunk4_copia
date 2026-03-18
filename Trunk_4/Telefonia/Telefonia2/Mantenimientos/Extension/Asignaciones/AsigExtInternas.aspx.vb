Imports TelefoniaLib

Partial Public Class AsigExtInternas
    Inherits PageBase

#Region "Page Load"

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelExt) : itzultzaileWeb.Itzuli(btnVerDatos) : itzultzaileWeb.Itzuli(labelExt2)
            itzultzaileWeb.Itzuli(labelNombre) : itzultzaileWeb.Itzuli(labelTipo) : itzultzaileWeb.Itzuli(labelAsigA)
            itzultzaileWeb.Itzuli(labelAsigAPerso) : itzultzaileWeb.Itzuli(btnAsignarPerso) : itzultzaileWeb.Itzuli(labelAsigADepart)
            itzultzaileWeb.Itzuli(btnAsignarDep) : itzultzaileWeb.Itzuli(btnVolver) : itzultzaileWeb.Itzuli(labelHistorico)
        End If
    End Sub

    ''' <summary>
    ''' Carga el listado de tipos de liena existentes
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                cargarExtensionesInternas()

                'Cargamos la primera vez para que luego el acceso sea mas rapido
                cargarDepartamentos(Master.Ticket.IdPlantaActual)
                'Esto se cargara cuando se sepa si es personal u otros
                'cargarPersonasYOtros(Master.Ticket.IdPlantaActual)
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
        lbLista.Attributes.Add("onchange", "javascript:seleccionarItem();")
        lbLista.Attributes.Add("onDblClick", "javascript:dobleClickExt();")
        listaPerso.Attributes.Add("onchange", "javascript:seleccionarPerso();")
        listaPerso.Attributes.Add("onDblClick", "javascript:dobleClickPerso();")
        listaDep.Attributes.Add("onchange", "javascript:seleccionarDep();")
        listaDep.Attributes.Add("onDblClick", "javascript:dobleClickDep();")        
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Visualiza la informacion de una extension para poder asignarle personas o departamentos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnVerDetalle(ByVal sender As Object, ByVal e As System.EventArgs)
        Try            
            Dim idExt As Integer = Integer.MinValue
            If (lbLista.SelectedIndex = -1) Then
                Master.MensajeAdvertenciaKey = "seleccioneExtension"
                Exit Sub
            Else
                idExt = lbLista.SelectedValue
            End If
            mostrarDetalle(idExt)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Dim batzEx As New BatzException("errVerDatos", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Muestra la informacion de la
    ''' </summary>
    ''' <param name="idExt">Identificador de la extension</param>
    Private Sub mostrarDetalle(ByVal idExt As Integer)
        Try
            Dim oExt As New ELL.Extension
            Dim cultura As String = Master.Ticket.Culture
            Dim extComp As New BLL.ExtensionComponent
            Dim tipo As String = String.Empty
            mvExtensiones.ActiveViewIndex = 1
            oExt.Id = idExt
            oExt = extComp.getExtension(oExt, Master.Ticket.IdPlantaActual, False)

            lblExten.Text = oExt.Extension
            lblNombreExt.Text = oExt.Nombre

            'Si no tiene telefono, el idTlfno sera 0 indicando que no se ha asignado a ningun telefono
            If (oExt.IdTelefono = Integer.MinValue) Then
                hfIdTlfno.Value = 0
            Else
                hfIdTlfno.Value = oExt.IdTelefono
            End If

			pnlDepartamento.Visible = oExt.IdTipoAsignacion = ELL.Extension.AsociarA.departamental 'Not (oExt.IdTipoAsignacion = ELL.Extension.AsociarA.personal)

			listaPerso.Items.Clear() 'Se borra porque a veces se pintaran las personas y otras los otros

            If (oExt.IdTipoAsignacion = ELL.Extension.AsociarA.personal) Then
                tipo = itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Extension.AsociarA), ELL.Extension.AsociarA.personal))
				btnAsignarPerso.CommandName = "P"
				cargarPersonas(Master.Ticket.IdPlantaActual)
            ElseIf (oExt.IdTipoAsignacion = ELL.Extension.AsociarA.departamental) Then                
                tipo = itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Extension.AsociarA), ELL.Extension.AsociarA.departamental))
				btnAsignarPerso.CommandName = "P" 'Cuando se muestren los departamentos, solo se mostraran las personas, no los otros
				cargarPersonas(Master.Ticket.IdPlantaActual)  'Cuando es departamental, tambien cargamos las personas
            Else
                tipo = itzultzaileWeb.Itzuli([Enum].GetName(GetType(ELL.Extension.AsociarA), ELL.Extension.AsociarA.otros))
				btnAsignarPerso.CommandName = "O"
				cargarOtros(Master.Ticket.IdPlantaActual)
            End If

            If (oExt.Visible) Then
                tipo &= "," & itzultzaileWeb.Itzuli("visible")
            Else
                tipo &= "," & itzultzaileWeb.Itzuli("noVisible")
            End If
            lblTipo.Text = tipo

            Dim row As DataRow
            Dim dt As New DataTable
            dt.Columns.Add("Id")
            dt.Columns.Add("Nombre")
            dt.Columns.Add("Tipo")
            dt.Columns.Add("Fecha")

            If (oExt.ListaPersonasAsig IsNot Nothing) Then
                For Each oPerso As ELL.ExtensionUsuDep In oExt.ListaPersonasAsig
                    If (oPerso.FechaHasta = Date.MinValue Or oPerso.FechaHasta > Now) Then
                        row = dt.NewRow
                        row("Id") = oPerso.IdUsuario
                        row("Nombre") = oPerso.NombreUsuario
                        row("Tipo") = "P"
                        row("Fecha") = oPerso.FechaDesde.ToShortDateString
                        dt.Rows.Add(row)
                    End If
                Next
            End If

            If (oExt.ListaDepartamentosAsig IsNot Nothing) Then
                For Each oDept As ELL.ExtensionUsuDep In oExt.ListaDepartamentosAsig
                    If (oDept.FechaHasta = Date.MinValue Or oDept.FechaHasta > Now) Then
                        row = dt.NewRow
                        row("Id") = oDept.IdDepartamento
                        row("Nombre") = oDept.NombreDepartamento
                        row("Tipo") = "D"
                        row("Fecha") = oDept.FechaDesde.ToShortDateString
                        dt.Rows.Add(row)
                    End If
                Next
			End If

			If (oExt.ListaOtrosAsig IsNot Nothing) Then
                For Each oOtros As ELL.ExtensionUsuDep In oExt.ListaOtrosAsig
                    If (oOtros.FechaHasta = Date.MinValue Or oOtros.FechaHasta > Now) Then
                        row = dt.NewRow
                        row("Id") = oOtros.IdOtros
                        row("Nombre") = oOtros.NombreOtros
                        row("Tipo") = "O"
                        row("Fecha") = oOtros.FechaDesde.ToShortDateString
                        dt.Rows.Add(row)
                    End If
                Next
			End If

			rptUsuDep.DataSource = dt
            rptUsuDep.DataBind()

            Dim listaAll As New List(Of ELL.ExtensionUsuDep)
            listaAll.AddRange(oExt.ListaPersonasAsig)
            listaAll.AddRange(oExt.ListaOtrosAsig)
            listaAll.AddRange(oExt.ListaDepartamentosAsig)            

            If (listaAll.Count > 0) Then
                listaAll.Sort(Function(o1 As ELL.ExtensionUsuDep, o2 As ELL.ExtensionUsuDep) o1.FechaDesde < o2.FechaDesde)
                pnlHistorico.Visible = True
                gvHistorico.DataSource = listaAll
                gvHistorico.DataBind()
            Else
                pnlHistorico.Visible = False
            End If

            btnAsignarPerso.CommandArgument = idExt
            btnAsignarDep.CommandArgument = idExt            
        Catch ex As Exception
            Dim batzEx As New BatzException("errVerInfo", ex)
            Throw batzEx
        End Try
    End Sub

    ''' <summary>
    ''' Se enlazan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvHistorico_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvHistorico.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim item As ELL.ExtensionUsuDep = e.Row.DataItem
            Dim lblAsignado As Label = CType(e.Row.FindControl("lblAsignado"), Label)
            Dim lblFechaDesde As Label = CType(e.Row.FindControl("lblFechaDesde"), Label)
            Dim lblFechaHasta As Label = CType(e.Row.FindControl("lblFechaHasta"), Label)

            If (item.NombreUsuario <> String.Empty) Then
                lblAsignado.Text = item.NombreUsuario
            ElseIf (item.NombreOtros <> String.Empty) Then
                lblAsignado.Text = item.NombreOtros
            ElseIf (item.NombreDepartamento <> String.Empty) Then
                lblAsignado.Text = item.NombreDepartamento
            End If
            lblFechaDesde.Text = item.FechaDesde.ToShortDateString
            If (item.FechaHasta <> Date.MinValue) Then lblFechaHasta.Text = item.FechaHasta.ToShortDateString
        End If
    End Sub

#End Region

#Region "Repeater"

    ''' <summary>
    ''' Cuando se enlaza el repeater, rellena algunos campos de la fila, que son calculados
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub rptUsuDep_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptUsuDep.ItemDataBound
        If (e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item) Then
            Dim img As Image = CType(e.Item.FindControl("imgUsuDep"), Image)
            Dim texto As Label = CType(e.Item.FindControl("lblUsuDep"), Label)
            Dim fecha As Label = CType(e.Item.FindControl("lblFAsig"), Label)
            Dim link As LinkButton = CType(e.Item.FindControl("lnkDesasignar"), LinkButton)
            Dim row As DataRowView = CType(e.Item.DataItem, DataRowView)

			If (row("Tipo") = "P" Or row("Tipo") = "O") Then
				img.ImageUrl = "~\App_Themes\Tema1\Images\persona.gif"
			Else
				img.ImageUrl = "~\App_Themes\Tema1\Images\departamento.gif"
			End If
            texto.Text = row("Nombre")
            link.CommandArgument = row("Id")
            link.CommandName = row("Tipo")
            fecha.Text = row("Fecha")
            fecha.ToolTip = itzultzaileWeb.Itzuli("Fecha de asignacion")
            itzultzaileWeb.Itzuli(link)
        End If
    End Sub

    ''' <summary>
    ''' Desasigna una persona o departamento de la extension
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub lnkDesasignar_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim link As LinkButton = CType(sender, LinkButton)
            Dim id As String = link.CommandArgument
            Dim oExt As New ELL.Extension
            Dim extComp As New BLL.ExtensionComponent

            oExt.Id = CInt(btnAsignarPerso.CommandArgument)
            If (link.CommandName = "P") Then
                oExt.IdTipoAsignacion = ELL.Extension.AsociarA.personal
            ElseIf (link.CommandName = "D") Then
                oExt.IdTipoAsignacion = ELL.Extension.AsociarA.departamental
            Else
                oExt.IdTipoAsignacion = ELL.Extension.AsociarA.otros
            End If

            oExt.IdTelefono = CInt(hfIdTlfno.Value)

            If (oExt.IdTipoAsignacion = ELL.Extension.AsociarA.personal) Then
                Dim lUser As New List(Of ELL.ExtensionUsuDep)
                Dim oUser As New ELL.ExtensionUsuDep
                oUser.IdUsuario = CInt(id)
                oUser.IdExtension = oExt.Id
                'oUser.IdTelefono = oExt.IdTelefono
                lUser.Add(oUser)
                oExt.ListaPersonasAsig = lUser
            ElseIf (oExt.IdTipoAsignacion = ELL.Extension.AsociarA.departamental) Then
                Dim lDep As New List(Of ELL.ExtensionUsuDep)
                Dim oDep As New ELL.ExtensionUsuDep
                oDep.IdDepartamento = id
                oDep.IdExtension = oExt.Id
                'oDep.IdTelefono = oExt.IdTelefono
                lDep.Add(oDep)
                oExt.ListaDepartamentosAsig = lDep
            Else  'Otros
                Dim lOtros As New List(Of ELL.ExtensionUsuDep)
                Dim oOtro As New ELL.ExtensionUsuDep
                oOtro.IdOtros = id
                oOtro.IdExtension = oExt.Id
                'oOtro.IdTelefono = oExt.IdTelefono
                lOtros.Add(oOtro)
				oExt.ListaOtrosAsig = lOtros
            End If
            extComp.modificarAsignacion(oExt, BLL.ExtensionComponent.Asignacion.desasignar)
            Dim mensa As String = "Se ha devuelto la extension " & lblExten.Text & " (" & oExt.Id & ")"
            If (oExt.IdTipoAsignacion = ELL.Extension.AsociarA.personal) Then
                mensa &= " de la persona (" & oExt.ListaPersonasAsig.First.IdUsuario & ")"
            ElseIf (oExt.IdTipoAsignacion = ELL.Extension.AsociarA.departamental) Then
                mensa &= " del departamento (" & oExt.ListaDepartamentosAsig.First.IdDepartamento & ")"
            ElseIf (oExt.IdTipoAsignacion = ELL.Extension.AsociarA.otros) Then
                mensa &= " del item otros (" & oExt.ListaOtrosAsig.First.IdOtros & ")"
            End If
            log.Info(mensa)
            Master.MensajeInfoKey = "desasignacionCorrecta"

            mostrarDetalle(oExt.Id)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Cargar Datos"

    ''' <summary>
    ''' Carga todas las personas
    ''' </summary>            
    Private Sub cargarPersonas(ByVal idPlanta As Integer)
        Try
            If (listaPerso.Items.Count = 0) Then
                Dim userComp As New BLL.UsuariosComponent
                Dim listUsu As List(Of SABLib.ELL.Usuario) = userComp.getUsuarios(idPlanta)

                OrdenarUsers(listUsu, SABLib.ELL.Usuario.ColumnNames.NOMBRECOMPLETO, SortDirection.Ascending)
                listaPerso.Items.Clear()

                listaPerso.DataSource = listUsu
                listaPerso.DataTextField = SABLib.ELL.Usuario.ColumnNames.NOMBRECOMPLETO
                listaPerso.DataValueField = SABLib.ELL.Usuario.ColumnNames.ID
                listaPerso.DataBind()

                'Lista auxiliar
                lbAuxiliar2.Items.Clear()
                lbAuxiliar2.DataSource = listUsu
                lbAuxiliar2.DataTextField = SABLib.ELL.Usuario.ColumnNames.NOMBRECOMPLETO
                lbAuxiliar2.DataValueField = SABLib.ELL.Usuario.ColumnNames.ID
                lbAuxiliar2.DataBind()

                txtPersona.Text = String.Empty
            End If

        Catch ex As Exception
            Throw New BatzException("errMostrandoPersonas", ex)
        End Try
	End Sub

	''' <summary>
	''' Carga los otros items de una planta
	''' </summary>            
	''' <param name="idPlanta">Planta de la que hay que cargar los otros</param>
	Private Sub cargarOtros(ByVal idPlanta As Integer)
		Try
			If (listaPerso.Items.Count = 0) Then
				Dim otrosComp As New BLL.OtrosComponent				

				Dim oOtro As New ELL.Otros With {.IdPlanta = idPlanta}
				Dim listOtros As List(Of ELL.Otros) = otrosComp.getOtros(oOtro)
				OrdenarOtros(listOtros, ELL.Otros.PropertyNames.NOMBRE, SortDirection.Ascending)

				listaPerso.Items.Clear()
				lbAuxiliar2.Items.Clear()

				listaPerso.DataSource = listOtros
				listaPerso.DataTextField = ELL.Otros.PropertyNames.NOMBRE
				listaPerso.DataValueField = ELL.Otros.PropertyNames.ID
				listaPerso.DataBind()

				'Lista auxiliar
				lbAuxiliar2.Items.Clear()
				lbAuxiliar2.DataSource = listOtros
				lbAuxiliar2.DataTextField = ELL.Otros.PropertyNames.NOMBRE
				lbAuxiliar2.DataValueField = ELL.Otros.PropertyNames.ID
				lbAuxiliar2.DataBind()

				txtPersona.Text = String.Empty
			End If
		Catch ex As Exception
			Throw New BatzException("errMostrandoPersonas", ex)
		End Try
	End Sub

	'''' <summary>
	'''' Carga todas las personas de una planta y los otros items en la misma lista
	'''' </summary>            
	'''' <param name="idPlanta">Planta de la que hay que cargar las personas</param>
	'Private Sub cargarPersonasYOtros(ByVal idPlanta As Integer)
	'	Try
	'		If (listaPerso.Items.Count = 0) Then
	'			Dim userComp As New BLL.UsuariosComponent
	'			Dim otrosComp As New BLL.OtrosComponent
	'			Dim listUsu As List(Of SABLib.ELL.Usuario) = userComp.getUsuarios(idPlanta)
	'			OrdenarUsers(listUsu, SABLib.ELL.Usuario.ColumnNames.NOMBRECOMPLETO, SortDirection.Ascending)

	'			Dim oOtro As New ELL.Otros With {.IdPlanta = idPlanta}
	'			Dim listOtros As List(Of ELL.Otros) = otrosComp.getOtros(oOtro)
	'			OrdenarOtros(listOtros, ELL.Otros.PropertyNames.NOMBRE, SortDirection.Ascending)

	'			listaPerso.Items.Clear()
	'			lbAuxiliar2.Items.Clear()

	'			For Each oUser As SABLib.ELL.Usuario In listUsu
	'				listaPerso.Items.Add(New ListItem(oUser.NombreCompleto, "P" & oUser.Id))
	'				lbAuxiliar2.Items.Add(New ListItem(oUser.NombreCompleto, "P" & oUser.Id))
	'			Next

	'			For Each oOtro In listOtros
	'				listaPerso.Items.Add(New ListItem(oOtro.Nombre, "O" & oOtro.Id))
	'				lbAuxiliar2.Items.Add(New ListItem(oOtro.Nombre, "O" & oOtro.Id))
	'			Next

	'			txtPersona.Text = String.Empty
	'		End If
	'	Catch ex As Exception
	'		Throw New BatzException("errMostrandoPersonas", ex)
	'	End Try
	'End Sub

    ''' <summary>
    ''' Carga todos los departamentos    
    ''' </summary>            
    ''' <param name="idPlanta">Planta de la que hay que cargar los departamentos</param>
    Private Sub cargarDepartamentos(ByVal idPlanta As Integer)
        Try
            Dim depComp As New BLL.DepartamentosComponent
            Dim listDep As List(Of Sablib.ELL.Departamento) = depComp.getDepartamentos(BLL.DepartamentosComponent.EDepartamentos.Activos, idPlanta)            
            listDep.Sort(Function(o1 As Sablib.ELL.Departamento, o2 As Sablib.ELL.Departamento) o1.Nombre < o2.Nombre)
            listaDep.Items.Clear()
            listaDep.DataSource = listDep
            listaDep.DataTextField = "Nombre"
            listaDep.DataValueField = "Id"
            listaDep.DataBind()
            'Lista auxiliar
            lbAuxiliar3.Items.Clear()
            lbAuxiliar3.DataSource = listDep
            lbAuxiliar3.DataTextField = "Nombre"
            lbAuxiliar3.DataValueField = "Id"
            lbAuxiliar3.DataBind()
            txtDepartamento.Text = String.Empty
        Catch ex As Exception
            Throw New BatzException("errMostrandoDepartamentos", ex)
        End Try
    End Sub

    '''' <summary>
    '''' Carga todos los departamentos
    '''' </summary>            
    'Private Sub cargarDepartamentos()
    '    Try
    '        If (listaDep.Items.Count = 0) Then                
    '            Dim genComp As New BLL.DepartamentosComponent
    '            Dim oDep As New ELL.Departamento
    '            Dim listDep As List(Of ELL.Departamento) = genComp.getDepartamentos(BLL.DepartamentosComponent.EDepartamentos.Todos, Master.Ticket.IdPlantaActual)

    '            listaDep.Items.Clear()

    '            listaDep.DataSource = listDep
    '            listaDep.DataTextField = "Nombre"
    '            listaDep.DataValueField = "Id"
    '            listaDep.DataBind()

    '            'Lista auxiliar
    '            lbAuxiliar3.Items.Clear()
    '            lbAuxiliar3.DataSource = listDep
    '            lbAuxiliar3.DataTextField = "Nombre"
    '            lbAuxiliar3.DataValueField = "Id"
    '            lbAuxiliar3.DataBind()

    '            txtDepartamento.Text = String.Empty
    '        End If

    '    Catch ex As Exception
    '        Throw New BatzException("errMostrandoDepartamentos", ex)
    '    End Try
    'End Sub

    ''' <summary>
    ''' Carga las extensiones existentes
    ''' </summary>    
    Private Sub cargarExtensionesInternas()
        Try            
            Dim listExt As List(Of ELL.Extension)
            Dim extComp As New BLL.ExtensionComponent
            Dim oExt As New ELL.Extension
            oExt.IdPlanta = Master.Ticket.IdPlantaActual
            oExt.IdTipoExtension = ELL.Extension.TipoExt.interna            
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

#End Region

#Region "Asignar personas y departamentos"

    ''' <summary>
    ''' Añade a la extension un usuario o departamento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnAsignar(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim btn As Button = CType(sender, Button)
            Dim oExt As New ELL.Extension            

            Dim extComp As New BLL.ExtensionComponent
            oExt.Id = CInt(btnAsignarPerso.CommandArgument)
            If (btn.CommandName = "P") Then
                oExt.IdTipoAsignacion = ELL.Extension.AsociarA.personal
            ElseIf (btn.CommandName = "D") Then
                oExt.IdTipoAsignacion = ELL.Extension.AsociarA.departamental
            Else
                oExt.IdTipoAsignacion = ELL.Extension.AsociarA.otros
            End If
            If (oExt.IdTipoAsignacion = ELL.Extension.AsociarA.personal) Then
                Dim lUser As New List(Of ELL.ExtensionUsuDep)
                Dim oUser As ELL.ExtensionUsuDep = Nothing
                For Each item As ListItem In listaPerso.Items
                    If (item.Selected) Then
                        oUser = New ELL.ExtensionUsuDep
                        oUser.IdUsuario = item.Value
                        oUser.IdExtension = oExt.Id
                        oUser.IdTelefono = CInt(hfIdTlfno.Value)
                        lUser.Add(oUser)
                    End If
                Next
                oExt.ListaPersonasAsig = lUser
            ElseIf (oExt.IdTipoAsignacion = ELL.Extension.AsociarA.departamental) Then
                Dim lDep As New List(Of ELL.ExtensionUsuDep)
                Dim oDep As ELL.ExtensionUsuDep = Nothing
                For Each item As ListItem In listaDep.Items
                    If (item.Selected) Then
                        oDep = New ELL.ExtensionUsuDep
                        oDep.IdDepartamento = item.Value
                        oDep.IdExtension = oExt.Id
                        oDep.IdTelefono = CInt(hfIdTlfno.Value)
                        lDep.Add(oDep)
                    End If
                Next
                oExt.ListaDepartamentosAsig = lDep
            Else  'Otros
                Dim lOtros As New List(Of ELL.ExtensionUsuDep)
                Dim oOtro As ELL.ExtensionUsuDep = Nothing
				For Each item As ListItem In listaPerso.Items
					If (item.Selected) Then
						oOtro = New ELL.ExtensionUsuDep
						oOtro.IdOtros = item.Value
						oOtro.IdExtension = oExt.Id
						oOtro.IdTelefono = CInt(hfIdTlfno.Value)
						lOtros.Add(oOtro)
					End If
				Next
                oExt.ListaOtrosAsig = lOtros
            End If
            extComp.modificarAsignacion(oExt, BLL.ExtensionComponent.Asignacion.asignar)

            txtPersona.Text = String.Empty
            txtDepartamento.Text = String.Empty
            Dim mensa As String = "Se ha asignado la extension " & lblExten.Text & " (" & oExt.Id & ")"
            If (oExt.IdTipoAsignacion = ELL.Extension.AsociarA.personal) Then
                mensa &= " a la persona (" & oExt.ListaPersonasAsig.First.IdUsuario & ")"
            ElseIf (oExt.IdTipoAsignacion = ELL.Extension.AsociarA.departamental) Then
                mensa &= " al departamento (" & oExt.ListaDepartamentosAsig.First.IdDepartamento & ")"
            ElseIf (oExt.IdTipoAsignacion = ELL.Extension.AsociarA.otros) Then
                mensa &= " al item otros (" & oExt.ListaOtrosAsig.First.IdOtros & ")"
            End If
            log.Info(mensa)
            Master.MensajeInfoKey = "asignaciónCorrecta"
            mostrarDetalle(oExt.Id)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
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

    ''' <summary>
    ''' Ordena la lista de usuario
    ''' </summary>
    ''' <param name="lUsuarios">Lista de usuarios</param>
    ''' <param name="sortExpr">Campo por el que hay que ordenar</param>
    ''' <param name="sortDir">Direccion de ordenacion</param>
    Private Sub OrdenarUsers(ByRef lUsuarios As List(Of SABLib.ELL.Usuario), ByVal sortExpr As String, ByVal sortDir As SortDirection)
        Try
            Select Case sortExpr
                Case SABLib.ELL.Usuario.ColumnNames.NOMBRECOMPLETO
                    lUsuarios.Sort(Function(oUser1 As SABLib.ELL.Usuario, oUser2 As SABLib.ELL.Usuario) _
                                       If(sortDir = SortDirection.Ascending, oUser1.NombreCompleto < oUser2.NombreCompleto, oUser1.NombreCompleto > oUser2.NombreCompleto))
            End Select
        Catch
        End Try
	End Sub

	''' <summary>
	''' Ordena la lista de otros
	''' </summary>
	''' <param name="lOtros">Lista de otros</param>
	''' <param name="sortExpr">Campo por el que hay que ordenar</param>
	''' <param name="sortDir">Direccion de ordenacion</param>    
	Private Sub OrdenarOtros(ByRef lOtros As List(Of ELL.Otros), ByVal sortExpr As String, ByVal sortDir As SortDirection)
		Try
			Select Case sortExpr
				Case ELL.Otros.PropertyNames.NOMBRE
					lOtros.Sort(Function(oOtro1 As ELL.Otros, oOtro2 As ELL.Otros) _
									   If(sortDir = SortDirection.Ascending, oOtro1.Nombre < oOtro2.Nombre, oOtro1.Nombre > oOtro2.Nombre))
			End Select
		Catch
		End Try
	End Sub

#End Region

#Region "Volver"

    ''' <summary>
    ''' Vuelve a la primera vista para seleccionar nuevas extensiones internas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnVolver_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVolver.Click
        mvExtensiones.ActiveViewIndex = 0
        txtBuscar.Text = String.Empty
    End Sub

#End Region

End Class
