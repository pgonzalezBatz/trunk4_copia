Imports TelefoniaLib

Partial Public Class AsigMoviles
    Inherits PageBase

#Region "Page Load"

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelMovil) : itzultzaileWeb.Itzuli(btnBuscar) : itzultzaileWeb.Itzuli(btnExportarExcel)
            itzultzaileWeb.Itzuli(labelMovil2) : itzultzaileWeb.Itzuli(labelExt) : itzultzaileWeb.Itzuli(labelAsigA)
            itzultzaileWeb.Itzuli(labelFechaFin) : itzultzaileWeb.Itzuli(btnDevolver) : itzultzaileWeb.Itzuli(labelAsigAPerso)
            itzultzaileWeb.Itzuli(labelFInicio) : itzultzaileWeb.Itzuli(btnAsignar) : itzultzaileWeb.Itzuli(labelHistorico)
            itzultzaileWeb.Itzuli(btnVolver)
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
                'Cargamos la primera vez para que luego el acceso sea mas rapido                
                cargarPersonasYOtros()
                cargarEstados()
                cargarMovilesGestor(String.Empty, ddlEstados.SelectedValue)                
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
        lbLista.Attributes.Add("onchange", "javascript:seleccionar();")
        btnExportarExcel.OnClientClick = "exportarExcel();"
    End Sub

#End Region

#Region "Buscar y Detalle"

    ''' <summary>
    ''' Se recargan los moviles con las opcions elegidas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Try
            cargarMovilesGestor(txtMovil.Text.Trim, ddlEstados.SelectedValue)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Muestra la informacion del movil
    ''' </summary>
    ''' <param name="idTlfno">Identificador del telefono movil</param>
    Private Sub mostrarDetalle(ByVal idTlfno As Integer)
        Try
            Dim oTlfno As New ELL.Telefono
            Dim oExt As New ELL.Extension
            Dim cultura As String = Master.Ticket.Culture
            Dim tlfnoComp As New BLL.TelefonoComponent
            Dim extComp As New BLL.ExtensionComponent
            Dim bLibre As Boolean = True
            Dim listaAll As List(Of ELL.ExtensionUsuDep)
            mvMoviles.ActiveViewIndex = 1
            'Se recupera el telefono y la extension
            oTlfno.Id = idTlfno
            hfIdTelefono.Value = idTlfno
            btnVolver.CommandArgument = idTlfno
            oTlfno = tlfnoComp.getTelefono(oTlfno)
            oExt.IdTelefono = oTlfno.Id
            oExt.IdTipoExtension = ELL.Extension.TipoExt.movil
            oExt = extComp.getExtension(oExt, Master.Ticket.IdPlantaActual, False)

            lblMovil.Text = oTlfno.Numero
            lblExtension.Text = oExt.Extension
            btnAsignar.CommandArgument = oExt.Id

            For Each extusu As ELL.ExtensionUsuDep In oExt.ListaPersonasAsig
                If (extusu.FechaHasta = Date.MinValue) Then
                    bLibre = False
                    btnDevolver.CommandArgument = extusu.IdUsuario
                    btnDevolver.CommandName = "P"
                    lblUsuarioAsignado.Text = extusu.NombreUsuario
                    Exit For
                End If
            Next

            If (bLibre) Then  'si no ha encontrado en personas, se busca en otros
                For Each extotro As ELL.ExtensionUsuDep In oExt.ListaOtrosAsig
                    If (extotro.FechaHasta = Date.MinValue) Then
                        bLibre = False
                        btnDevolver.CommandArgument = extotro.IdOtros
                        btnDevolver.CommandName = "O"
                        lblUsuarioAsignado.Text = extotro.NombreOtros
                        Exit For
                    End If
                Next
            End If

            listaAll = oExt.ListaPersonasAsig
            listaAll.AddRange(oExt.ListaOtrosAsig)

            If (listaAll.Count > 0) Then
                listaAll.Sort(Function(o1 As ELL.ExtensionUsuDep, o2 As ELL.ExtensionUsuDep) o1.FechaDesde < o2.FechaDesde)

                pnlHistorico.Visible = True
                gvHistorico.DataSource = listaAll
                gvHistorico.DataBind()
            Else
                pnlHistorico.Visible = False
            End If
            'If (oExt.ListaPersonasAsig IsNot Nothing AndAlso oExt.ListaPersonasAsig.Count > 0) Then

            '    For Each extusu As ELL.ExtensionUsuDep In oExt.ListaPersonasAsig
            '        If (extusu.FechaHasta = Date.MinValue) Then
            '            bLibre = False
            '            btnDevolver.CommandArgument = extusu.IdUsuario
            '            lblUsuarioAsignado.Text = extusu.NombreUsuario
            '            Exit For
            '        End If
            '    Next
            '    'Historico

            '    oExt.ListaPersonasAsig.Sort(Function(o1 As ELL.ExtensionUsuDep, o2 As ELL.ExtensionUsuDep) o1.FechaDesde < o2.FechaDesde)

            '    pnlHistorico.Visible = True
            '    gvHistorico.DataSource = oExt.ListaPersonasAsig
            '    gvHistorico.DataBind()
            'Else
            '    pnlHistorico.Visible = False
            'End If

            If (Not bLibre) Then
                pnlAsignar.Visible = False
                pnlDevolver.Visible = True
            Else
                pnlAsignar.Visible = True
                pnlDevolver.Visible = False
                lblUsuarioAsignado.Text = itzultzaileWeb.Itzuli("sinAsignar")
            End If

            txtFechaInicio.Text = Now.Date.ToShortDateString
            txtFechaFin.Text = Now.Date.ToShortDateString
        Catch ex As Exception
            Dim batzEx As New BatzException("errVerInfo", ex)
            Throw batzEx
        End Try
    End Sub

#End Region

#Region "Gridview Listado"

#Region "Funciones"

    ''' <summary>
    ''' Formatea la fecha del repeater
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

    ''' <summary>
    ''' Devuelve el string que no este vacio
    ''' </summary>
    ''' <param name="nombreUser">String 1</param>
    ''' <param name="nombreOtro">String 2</param>
    ''' <returns></returns>
    Protected Function ObtenerAsignadoA(ByVal nombreUser As String, ByVal nombreOtro As String) As String
        If (nombreUser <> String.Empty) Then
            Return nombreUser
        Else
            Return nombreOtro
        End If
    End Function

#End Region

#Region "RowDataBound"

    ''' <summary>
    ''' Evento que surge cuando se enlazan los moviles
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvMoviles_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMoviles.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf e.Row.RowType = DataControlRowType.DataRow Then
            Dim row As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim lblNum As Label = CType(e.Row.FindControl("lblNumero"), Label)
            Dim lblExt As Label = CType(e.Row.FindControl("lblExtension"), Label)
            Dim lblNombre As Label = CType(e.Row.FindControl("lblNombre"), Label)
            Dim lblDepartamento As Label = CType(e.Row.FindControl("lblDepartamento"), Label)
            Dim lblFechaDesde As Label = CType(e.Row.FindControl("lblFechaDesde"), Label)

            lblNum.Text = row.Item(DAL.TELEFONO.ColumnNames.NUMERO)
            lblExt.Text = row.Item(DAL.EXTENSION.ColumnNames.EXTENSION)
            lblNombre.Text = row.Item("NOMBRE")
            If (CType(row.Item("FECHA_DESDE"), Date) <> Date.MinValue) Then
                lblFechaDesde.Text = CType(row.Item("FECHA_DESDE"), Date).ToShortDateString
            End If

            If (lblNombre.Text.Trim = String.Empty) Then
                lblNombre.Text = itzultzaileWeb.Itzuli("libre")
            End If

            lblDepartamento.Text = row.Item("DEPARTAMENTO")

            'Estilo para que al posicionarse sobre la fila, se pinte de un color
            e.Row.Attributes.Add("onmouseover", "SartuN(this);")
            e.Row.Attributes.Add("onmouseout", "IrtenN(this);")
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvMoviles, "Select$" + CStr(row.Item(DAL.TELEFONO.ColumnNames.ID)))
        End If
    End Sub

#End Region

#Region "RowCommand"

    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' <para>Redirecciona al detalle del movil seleccionado</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvMoviles_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMoviles.RowCommand
        Try
            If (e.CommandName = "Select") Then
                mostrarDetalle(CInt(e.CommandArgument))
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Orden"

    ''' <summary>
    ''' Evento para ordenar las columnas del resultado de la busqueda
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvMoviles_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvMoviles.Sorting
        Try
            If (GridViewSortDirection = SortDirection.Ascending) Then
                GridViewSortDirection = SortDirection.Descending
            Else
                GridViewSortDirection = SortDirection.Ascending
            End If

            If (GridViewSortExpresion Is Nothing) Then
                GridViewSortExpresion = String.Empty
            Else
                GridViewSortExpresion = e.SortExpression
            End If

            cargarMovilesGestor(txtMovil.Text, ddlEstados.SelectedValue)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Indica la direccion en la que hay que ordenar
    ''' </summary>
    ''' <value></value>
    ''' <returns>Orden</returns>
    Public Property GridViewSortDirection() As SortDirection
        Get
            If (ViewState("sortDirection") Is Nothing) Then
                ViewState("sortDirection") = SortDirection.Ascending
            End If
            Return CType(ViewState("sortDirection"), SortDirection)
        End Get
        Set(ByVal value As SortDirection)
            ViewState("sortDirection") = value
        End Set
    End Property

    ''' <summary>
    ''' Indica la expresion de ordenacion
    ''' </summary>
    ''' <value></value>
    ''' <returns>Expresion</returns>
    Public Property GridViewSortExpresion() As String
        Get
            If (ViewState("sortExpresion") Is Nothing) Then
                ViewState("sortExpresion") = ELL.Extension.PropertyNames.EXTENSION
            End If
            Return ViewState("sortExpresion").ToString()
        End Get
        Set(ByVal value As String)
            ViewState("sortExpresion") = value
        End Set
    End Property

#End Region

#Region "Paginación"

    ''' <summary>
    ''' Se realiza la paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvMoviles_Paging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvMoviles.PageIndexChanging
        Try
            gvMoviles.PageIndex = e.NewPageIndex
            cargarMovilesGestor(txtMovil.Text, ddlEstados.SelectedValue)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#End Region

#Region "Cargar Datos"

    ''' <summary>
    ''' Carga todas las personas
    ''' </summary>            
    Private Sub cargarPersonasYOtros()
        Try
            If (lbLista.Items.Count = 0) Then
                Dim cultura As String = Master.Ticket.Culture
                Dim userComp As New BLL.UsuariosComponent
                Dim otrosComp As New BLL.OtrosComponent
                Dim listUsu As List(Of Sablib.ELL.Usuario) = userComp.getUsuarios(Master.Ticket.IdPlantaActual)

                Dim oOtro As New ELL.Otros With {.IdPlanta = Master.Ticket.IdPlantaActual}
                Dim listOtros As List(Of ELL.Otros) = otrosComp.getOtros(oOtro)

                OrdenarUsers(listUsu, Sablib.ELL.Usuario.ColumnNames.NOMBRECOMPLETO, SortDirection.Ascending)
                OrdenarOtros(listOtros, ELL.Otros.PropertyNames.NOMBRE, SortDirection.Ascending)

                lbLista.Items.Clear()
                For Each oUser As Sablib.ELL.Usuario In listUsu
                    lbLista.Items.Add(New ListItem(oUser.NombreCompleto, "P" & oUser.Id))
                Next

                For Each oOtro In listOtros
                    lbLista.Items.Add(New ListItem(oOtro.Nombre, "O" & oOtro.Id))
                Next

                'Se carga la lista auxiliar
                lbAuxiliar.Items.Clear()
                For Each oUser As Sablib.ELL.Usuario In listUsu
                    lbAuxiliar.Items.Add(New ListItem(oUser.NombreCompleto, "P" & oUser.Id))
                Next

                For Each oOtro In listOtros
                    lbAuxiliar.Items.Add(New ListItem(oOtro.Nombre, "O" & oOtro.Id))
                Next

                'Antes
                'lbLista.Items.Clear()

                'lbLista.DataSource = listUsu
                'lbLista.DataTextField = SABLib.ELL.Usuario.ColumnNames.NOMBRECOMPLETO
                'lbLista.DataValueField = SABLib.ELL.Usuario.ColumnNames.ID
                'lbLista.DataBind()

                ''Lista auxiliar
                'lbAuxiliar.Items.Clear()
                'lbAuxiliar.DataSource = listUsu
                'lbAuxiliar.DataTextField = SABLib.ELL.Usuario.ColumnNames.NOMBRECOMPLETO
                'lbAuxiliar.DataValueField = SABLib.ELL.Usuario.ColumnNames.ID
                'lbAuxiliar.DataBind()

                txtBuscar.Text = String.Empty
            End If

        Catch ex As Exception
            Throw New BatzException("errMostrandoPersonas", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los estados (todos,libre,ocupado) en los que podra estar un tlfno
    ''' </summary>            
    Private Sub cargarEstados()
        Try
            If (ddlEstados.Items.Count = 0) Then
                ddlEstados.Items.Add(New ListItem(itzultzaileWeb.Itzuli("todos"), BLL.TelefonoComponent.Estado.todos))
                ddlEstados.Items.Add(New ListItem(itzultzaileWeb.Itzuli("libres"), BLL.TelefonoComponent.Estado.libre))
                ddlEstados.Items.Add(New ListItem(itzultzaileWeb.Itzuli("ocupados"), BLL.TelefonoComponent.Estado.ocupado))
            End If

            ddlEstados.SelectedIndex = 0
        Catch ex As Exception
            Throw New BatzException("errMostrarEstado", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los numeros de telefonos moviles que:
    ''' -Que esten gestionados por el gestor
    ''' -Que tengan asociada una extension
    ''' -Que su extension movil, coincida con un grupo de extension libre
    ''' </summary>            
    ''' <param name="state">Estado a mostrar</param>
    Private Function cargarMovilesGestor(ByVal movil As String, ByVal state As BLL.TelefonoComponent.Estado) As Data.DataView
        Try
            Dim cultura As String = Master.Ticket.Culture
            Dim tlfnoComp As New BLL.TelefonoComponent
            Dim oTlfno As New ELL.Telefono
            Dim direccion As String = "ASC"

            oTlfno.IdPlanta = Master.Ticket.IdPlantaActual
            oTlfno.Numero = movil
            'Si es administrador de planta o administrador general, vera la de todos
            If Not (Master.Ticket.EsAdministrador Or Master.Ticket.EsAdministradorPlanta) Then
                oTlfno.IdUsuarioGestor = Master.Ticket.IdUser
            End If

            Dim dtTlfnos As DataTable = tlfnoComp.getTelefonosGestor2(oTlfno, Master.Ticket.IdPlantaActual, state)

            If (GridViewSortExpresion <> String.Empty) Then
                If (GridViewSortDirection = SortDirection.Descending) Then direccion = "DESC"
                dtTlfnos.DefaultView.Sort = GridViewSortExpresion & " " & direccion
            End If

            Dim dv As Data.DataView = dtTlfnos.DefaultView
            gvMoviles.DataSource = dv
            gvMoviles.DataBind()

            Return dv
        Catch ex As Exception
            Throw New BatzException("errMostrarTelefonos", ex)
        End Try
    End Function

#End Region

#Region "Asignacion y devolucion"

    ''' <summary>
    ''' Asigna el telefono movil a un usuario
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnAsignar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAsignar.Click
        Try
            Dim btn As Button = CType(sender, Button)
            Dim extComp As New BLL.ExtensionComponent
            Dim oExtUsu As New ELL.ExtensionUsuDep
            Dim tipoAsig As ELL.Extension.AsociarA

            If (lbLista.SelectedValue.StartsWith("P")) Then
                tipoAsig = ELL.Extension.AsociarA.personal
            ElseIf (lbLista.SelectedValue.StartsWith("O")) Then
                tipoAsig = ELL.Extension.AsociarA.otros
            End If

            oExtUsu.IdExtension = CInt(btnAsignar.CommandArgument)
            oExtUsu.IdTelefono = CInt(hfIdTelefono.Value)

            If (tipoAsig = ELL.Extension.AsociarA.personal) Then
                oExtUsu.IdUsuario = CInt(lbLista.SelectedValue.Substring(1))
            ElseIf (tipoAsig = ELL.Extension.AsociarA.otros) Then
                oExtUsu.IdOtros = CInt(lbLista.SelectedValue.Substring(1))
            End If

            oExtUsu.FechaDesde = txtFechaInicio.Text.Trim

            If (extComp.AsigDevolucionTlfno(oExtUsu, BLL.ExtensionComponent.AsignacionDev.asignacion, tipoAsig)) Then
                Dim mensa As String = "Se ha asignado el movil (" & oExtUsu.IdTelefono & ") de la extension (" & oExtUsu.IdExtension & ")"
                If (tipoAsig = ELL.Extension.AsociarA.personal) Then
                    mensa &= " a la persona (" & oExtUsu.IdUsuario & ")"
                ElseIf (tipoAsig = ELL.Extension.AsociarA.otros) Then
                    mensa &= " al item de otros (" & oExtUsu.IdOtros & ")"
                End If
                log.Info(mensa)
                cargarMovilesGestor(txtMovil.Text.Trim, ddlEstados.SelectedValue)
                Master.MensajeInfoKey = "asignaciónCorrecta"
            Else
                Master.MensajeAdvertenciaKey = "errAsignacion"
            End If

            mostrarDetalle(CInt(btnVolver.CommandArgument))
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Realiza una devolucion de la persona del telefono movil
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnDevolucion_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDevolver.Click
        Try
            Dim btn As Button = CType(sender, Button)
            Dim extComp As New BLL.ExtensionComponent
            Dim oExtUsu As New ELL.ExtensionUsuDep
            Dim tipoAsig As ELL.Extension.AsociarA

            If (btnDevolver.CommandName = "P") Then
                oExtUsu.IdUsuario = CInt(btn.CommandArgument)
                tipoAsig = ELL.Extension.AsociarA.personal
            Else
                oExtUsu.IdOtros = CInt(btn.CommandArgument)
                tipoAsig = ELL.Extension.AsociarA.otros
            End If
            oExtUsu.IdExtension = CInt(btnAsignar.CommandArgument)
            oExtUsu.IdTelefono = CInt(hfIdTelefono.Value)
            oExtUsu.FechaDesde = ObtenerFechaDesdeActual()
            oExtUsu.FechaHasta = txtFechaFin.Text.Trim

            If (oExtUsu.FechaDesde = Date.MinValue) Then
                Master.MensajeAdvertenciaKey = "No se ha recogido bien la fecha desde"
            Else
                If (extComp.AsigDevolucionTlfno(oExtUsu, BLL.ExtensionComponent.AsignacionDev.devolucion, tipoAsig)) Then
                    log.Info("Se ha devuelto el movil (" & oExtUsu.IdTelefono & ") de la extension (" & oExtUsu.IdExtension & ")")
                    cargarMovilesGestor(txtMovil.Text.Trim, ddlEstados.SelectedValue)
                    Master.MensajeInfoKey = "devolucionCorrecta"
                Else
                    Master.MensajeAdvertenciaKey = "devolucionNoRealizada"
                End If
            End If

            mostrarDetalle(CInt(btnVolver.CommandArgument))
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene la fecha desde, en la que tiene el movil
    ''' </summary>
    ''' <returns>Fecha</returns>
    Private Function ObtenerFechaDesdeActual() As Date
        Try
            Dim fecha As Date = Date.MinValue

            'For i = 0 To gvHistorico.Rows.Count - 1
            '    If (CType(gvHistorico.DataKeys(i).Values.Item(1), Date) = Date.MinValue) Then
            '        fecha = CType(gvHistorico.DataKeys(i).Values.Item(0), Date)
            '        Exit For
            '    End If
            'Next
            For i = 0 To gvHistorico.Rows.Count - 1
                If (CType(gvHistorico.Rows.Item(i).Cells(2).Controls(1), Label).Text() = String.Empty) Then
                    fecha = CType(CType(gvHistorico.Rows.Item(i).Cells(1).Controls(1), Label).Text(), Date)
                    Exit For
                End If
            Next

            Return fecha
        Catch ex As Exception
            Throw New BatzException("errRealizarAccion", ex)
        End Try
    End Function

    ''' <summary>
    ''' Se enlazan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvHistorico_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvHistorico.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        End If
    End Sub

#End Region

#Region "Ordenar"

    ''' <summary>
    ''' Ordena la lista de usuario
    ''' </summary>
    ''' <param name="lUsuarios">Lista de usuarios</param>
    ''' <param name="sortExpr">Campo por el que hay que ordenar</param>
    ''' <param name="sortDir">Direccion de ordenacion</param>
    Private Sub OrdenarUsers(ByRef lUsuarios As List(Of Sablib.ELL.Usuario), ByVal sortExpr As String, ByVal sortDir As SortDirection)
        Try
            Select Case sortExpr
                Case Sablib.ELL.Usuario.ColumnNames.NOMBRECOMPLETO
                    lUsuarios.Sort(Function(oUser1 As Sablib.ELL.Usuario, oUser2 As Sablib.ELL.Usuario) _
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

#Region "Exportar"

    ' ''' <summary>
    ' ''' Importa a excel el resultado del grid
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>    
    'Private Sub btnExportarExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportarExcel.Click        
    '    ScriptManager.RegisterStartupScript(Page, GetType(Page), "export", "window.open('ExportExcelMovil.aspx?mov=" + txtMovil.Text + "&est=" + ddlEstados.SelectedValue + ");", True)
    'End Sub

#End Region

#Region "Volver"

    ''' <summary>
    ''' Vuelve a la primera vista para seleccionar nuevos moviles libres
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnVolver_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVolver.Click
        Try
            mvMoviles.ActiveViewIndex = 0
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

End Class

