Imports TelefoniaLib

Partial Public Class AsigNumSinExt
    Inherits PageBase

#Region "Page Load"

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelTlfno) : itzultzaileWeb.Itzuli(btnVerDatos) : itzultzaileWeb.Itzuli(labelTlfno2)
            itzultzaileWeb.Itzuli(labelCia) : itzultzaileWeb.Itzuli(labelAsigA) : itzultzaileWeb.Itzuli(labelAsigAPerso)
            itzultzaileWeb.Itzuli(btnAsignarPerso) : itzultzaileWeb.Itzuli(labelHistorico) : itzultzaileWeb.Itzuli(btnVolver)            
        End If
    End Sub

    ''' <summary>
    ''' Carga el listado de telefonos sin extensiones asignadas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Asignacion de telefonos sin extension"
                cargarTelefonosSinExtension()

                'Cargamos la primera vez para que luego el acceso sea mas rapido
                'cargarDepartamentos()
                cargarPersonas()
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
        lbLista.Attributes.Add("onDblClick", "javascript:dobleClickTlfno();")
        listaPerso.Attributes.Add("onchange", "javascript:seleccionarPerso();")
        listaPerso.Attributes.Add("onDblClick", "javascript:dobleClickPerso();")
        'listaDep.Attributes.Add("onchange", "javascript:seleccionarDep();")
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Visualiza la informacion de una extension para poder asignarle personas 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnVerDetalle(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim idTlfno As Integer = Integer.MinValue

            If (lbLista.SelectedIndex = -1) Then
                Master.MensajeAdvertencia = "seleccioneTelefono"
                Exit Sub
            Else
                idTlfno = lbLista.SelectedValue
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
    ''' Muestra la informacion de la
    ''' </summary>
    ''' <param name="idTlfno">Identificador del telefono</param>
    Private Sub mostrarDetalle(ByVal idTlfno As Integer)
        Try
            Dim oTlfno As New ELL.Telefono
            Dim cultura As String = Master.Ticket.Culture
            Dim tlfnoComp As New BLL.TelefonoComponent
            Dim ciaComp As New BLL.CiaTlfnoComponent
            Dim tipo As String = String.Empty
            oTlfno.Id = idTlfno
            oTlfno = tlfnoComp.getTelefono(oTlfno)

            lblTlfno.Text = oTlfno.Numero

            'pnlDepartamento.Visible = Not (oExt.IdTipoAsignacion = ELL.Extension.AsociarA.personal)

            If (oTlfno.IdCiaTlfno <> Integer.MinValue) Then
                lblCia.Text = ciaComp.getCompañia(oTlfno.IdCiaTlfno).Nombre
            End If            

            Dim row As DataRow
            Dim dt As New DataTable
            dt.Columns.Add("Id")
            dt.Columns.Add("Nombre")
            dt.Columns.Add("Tipo")
            If (oTlfno.ListaPersonasAsig IsNot Nothing) Then
                For Each oPerso As ELL.TelefonoUsuDep In oTlfno.ListaPersonasAsig
                    If (oPerso.FechaHasta = Date.MinValue Or (oPerso.FechaHasta <> Date.MinValue And oPerso.FechaHasta > Date.Now)) Then
                        row = dt.NewRow
                        row("Id") = oPerso.IdUsuario
                        row("Nombre") = oPerso.NombreUsuario
                        row("Tipo") = "P"
                        dt.Rows.Add(row)
                    End If
                Next
            End If

            'If (oExt.ListaDepartamentosAsig IsNot Nothing) Then   'oExt.Personal And 
            '    For Each oDept As ELL.ExtensionUsuDep In oExt.ListaDepartamentosAsig
            '        row = dt.NewRow
            '        row("Id") = oDept.IdDepartamento
            '        row("Nombre") = oDept.NombreDepartamento
            '        row("Tipo") = "D"
            '        dt.Rows.Add(row)
            '    Next
            'End If

            rptUsu.DataSource = dt
            rptUsu.DataBind()

            gvHistorico.DataSource = oTlfno.ListaPersonasAsig
            gvHistorico.DataBind()

            btnAsignarPerso.CommandArgument = idTlfno
            'btnAsignarDep.CommandArgument = idExt
            mvTelefonos.ActiveViewIndex = 1
        Catch ex As Exception
            Dim batzEx As New BatzException("errVerInfo", ex)
            Throw batzEx
        End Try
    End Sub

#End Region

#Region "Repeater"

    ''' <summary>
    ''' Cuando se enlaza el repeater, rellena algunos campos de la fila, que son calculados
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub rptUsuDep_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptUsu.ItemDataBound
        If (e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item) Then
            Dim img As Image = CType(e.Item.FindControl("imgUsu"), Image)
            Dim texto As Label = CType(e.Item.FindControl("lblUsu"), Label)
            Dim link As LinkButton = CType(e.Item.FindControl("lnkDesasignar"), LinkButton)
            Dim row As DataRowView = CType(e.Item.DataItem, DataRowView)

            If (row("Tipo") = "P") Then
                img.ImageUrl = "~\App_Themes\Tema1\Images\persona.gif"
                ' Else
                '   img.ImageUrl = "~\App_Themes\Tema1\Images\departamento.gif"
            End If
            texto.Text = row("Nombre")
            link.CommandArgument = row("Id")
            link.CommandName = row("Tipo")
            itzultzaileWeb.Itzuli(link)
        End If
    End Sub

    ''' <summary>
    ''' Desasigna una persona al telefono
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub lnkDesasignar_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim link As LinkButton = CType(sender, LinkButton)
            Dim id As Integer = CInt(link.CommandArgument)
            Dim oTlfno As New ELL.Telefono
            Dim tlfnoComp As New BLL.TelefonoComponent

            oTlfno.Id = CInt(btnAsignarPerso.CommandArgument)

            Dim lUser As New List(Of ELL.TelefonoUsuDep)
            Dim oUser As New ELL.TelefonoUsuDep
            oUser.IdUsuario = id
            oUser.IdTelefono = oTlfno.Id
            lUser.Add(oUser)
            oTlfno.ListaPersonasAsig = lUser
            'ElseIf (oExt.IdTipoAsignacion = ELL.Extension.AsociarA.departamental) Then
            '    Dim lDep As New List(Of ELL.ExtensionUsuDep)
            '    Dim oDep As New ELL.ExtensionUsuDep
            '    oDep.IdDepartamento = id
            '    oDep.IdExtension = oExt.Id
            '    lDep.Add(oDep)
            '    oExt.ListaDepartamentosAsig = lDep
            'Else  'Otros
            '    Dim lOtros As New List(Of ELL.ExtensionUsuDep)
            '    Dim oOtro As New ELL.ExtensionUsuDep
            '    oOtro.IdOtros = id
            '    oOtro.IdExtension = oExt.Id
            '    lOtros.Add(oOtro)
            '    oExt.ListaDepartamentosAsig = lOtros
            tlfnoComp.modificarAsignacion(oTlfno, BLL.TelefonoComponent.Asignacion.desasignar)
            log.Info("Se ha desasignado el numero sin extension-> Id tlfno:" & oTlfno.Id & " del Id Usuario:" & id)
            Master.MensajeInfo = "desasignacionCorrecta"

            mostrarDetalle(oTlfno.Id)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Funciones"

    ''' <summary>
    ''' Formatea la fecha
    ''' </summary>
    ''' <param name="oFecha">Fecha</param>
    ''' <returns></returns>
    Protected Function FormatFecha(ByVal oFecha As Date) As String
        If (oFecha = Date.MinValue) Then
            Return String.Empty
        Else
            Return oFecha.ToShortDateString
        End If
    End Function

#End Region

#Region "Cargar Datos"

    ''' <summary>
    ''' Se enlaza el historico
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvHistorico_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvHistorico.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        End If
    End Sub

    ''' <summary>
    ''' Carga todas las personas
    ''' </summary>            
    Private Sub cargarPersonas()
        Try
            If (listaPerso.Items.Count = 0) Then
                Dim userComp As New BLL.UsuariosComponent
                Dim listUsu As List(Of Sablib.ELL.Usuario) = userComp.getUsuarios(Master.Ticket.IdPlantaActual)

                OrdenarUsers(listUsu, Sablib.ELL.Usuario.ColumnNames.NOMBRECOMPLETO, SortDirection.Ascending)
                listaPerso.Items.Clear()

                listaPerso.DataSource = listUsu
                listaPerso.DataTextField = Sablib.ELL.Usuario.ColumnNames.NOMBRECOMPLETO
                listaPerso.DataValueField = Sablib.ELL.Usuario.ColumnNames.ID
                listaPerso.DataBind()

                'Lista auxiliar
                lbAuxiliar2.Items.Clear()
                lbAuxiliar2.DataSource = listUsu
                lbAuxiliar2.DataTextField = Sablib.ELL.Usuario.ColumnNames.NOMBRECOMPLETO
                lbAuxiliar2.DataValueField = Sablib.ELL.Usuario.ColumnNames.ID
                lbAuxiliar2.DataBind()

                txtPersona.Text = String.Empty
            End If

        Catch ex As Exception
            Throw New BatzException("errMostrandoPersonas", ex)
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
    '            Dim listDep As List(Of ELL.Departamento) = genComp.getDepartamentos(BLL.DepartamentosComponent.EDepartamentos.Todos)

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
    ''' Carga los telefonos sin extension
    ''' </summary>    
    Private Sub cargarTelefonosSinExtension()
        Try
            Dim listTlfno As List(Of ELL.Telefono)
            Dim tlfnoComp As New BLL.TelefonoComponent
            Dim oTlfno As New ELL.Telefono
            oTlfno.IdPlanta = Master.Ticket.IdPlantaActual
            oTlfno.FijoOMovil = ELL.Telefono.FijoMovil.movil
            listTlfno = tlfnoComp.getTelefonosLibres(oTlfno)

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

        Catch ex As Exception
            Throw New BatzException("errMostrarExtensiones", ex)
        End Try
    End Sub

#End Region

#Region "Asignar personas"

    ''' <summary>
    ''' Añade el telefono un usuario
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub btnAsignar(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim btn As Button = CType(sender, Button)
            Dim oTlfno As New ELL.Telefono

            Dim tlfnoComp As New BLL.TelefonoComponent
            oTlfno.Id = CInt(btnAsignarPerso.CommandArgument)
            'If (btn.CommandName = "P") Then
            '    oExt.IdTipoAsignacion = ELL.Extension.AsociarA.personal
            'ElseIf (btn.CommandName = "D") Then
            '    oExt.IdTipoAsignacion = ELL.Extension.AsociarA.departamental
            'Else
            '    oExt.IdTipoAsignacion = ELL.Extension.AsociarA.otros
            'End If
            'If (oExt.IdTipoAsignacion = ELL.Extension.AsociarA.personal) Then
            Dim lUser As New List(Of ELL.TelefonoUsuDep)
            Dim oUser As ELL.TelefonoUsuDep = Nothing
            Dim idUsers As String = String.Empty
            For Each item As ListItem In listaPerso.Items
                If (item.Selected) Then
                    oUser = New ELL.TelefonoUsuDep
                    oUser.IdUsuario = item.Value
                    oUser.IdTelefono = oTlfno.Id
                    idUsers &= If(idUsers <> String.Empty, ",", "") & item.Value
                    lUser.Add(oUser)
                End If
            Next
            oTlfno.ListaPersonasAsig = lUser
            'ElseIf (oExt.IdTipoAsignacion = ELL.Extension.AsociarA.departamental) Then
            '    Dim lDep As New List(Of ELL.ExtensionUsuDep)
            '    Dim oDep As ELL.ExtensionUsuDep = Nothing
            '    For Each item As ListItem In listaDep.Items
            '        If (item.Selected) Then
            '            oDep = New ELL.ExtensionUsuDep
            '            oDep.IdDepartamento = item.Value
            '            oDep.IdExtension = oExt.Id
            '            lDep.Add(oDep)
            '        End If
            '    Next
            '    oExt.ListaDepartamentosAsig = lDep
            'Else  'Otros
            '    Dim lOtros As New List(Of ELL.ExtensionUsuDep)
            '    Dim oOtro As ELL.ExtensionUsuDep = Nothing
            '    For Each item As ListItem In listaDep.Items
            '        If (item.Selected) Then
            '            oOtro = New ELL.ExtensionUsuDep
            '            oOtro.IdOtros = item.Value
            '            oOtro.IdExtension = oExt.Id
            '            lOtros.Add(oOtro)
            '        End If
            '    Next
            '    oExt.ListaOtrosAsig = lOtros
            'End If
            tlfnoComp.modificarAsignacion(oTlfno, BLL.TelefonoComponent.Asignacion.asignar)
            log.Info("Se ha asignado el numero sin extension-> Id tlfno:" & oTlfno.Id & " a los usuarios:" & idUsers)
            txtPersona.Text = String.Empty
            'txtDepartamento.Text = String.Empty

            Master.MensajeInfo = "asignaciónCorrecta"
            mostrarDetalle(oTlfno.Id)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

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
                    lTelefonos.Sort(Function(oTlfno1 As ELL.Telefono, oTlfno2 As ELL.Telefono) _
                                       If(sortDir = SortDirection.Ascending, oTlfno1.Numero < oTlfno2.Numero, oTlfno1.Numero > oTlfno2.Numero))
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

#End Region

#Region "Volver"

    ''' <summary>
    ''' Vuelve a la primera vista para seleccionar nuevas extensiones internas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnVolver_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVolver.Click
        mvTelefonos.ActiveViewIndex = 0
        txtBuscar.Text = String.Empty
    End Sub

#End Region

End Class
