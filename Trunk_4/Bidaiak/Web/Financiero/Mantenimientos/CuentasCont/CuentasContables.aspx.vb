Public Class CuentasContables
    Inherits PageBase

#Region "Petaña Listado"

#Region "Page Load"

    ''' <summary>
    ''' Carga el listado de cuentas contables
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Cuentas contables"
                tabContainer.ActiveTabIndex = 0
                txtFilter.Attributes.Add("placeholder", "Codigo departamento,departamento o cuentas contables")
                inicializarSincronizacion()
                cargarCuentas()
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CuentasContables_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelInfo) : itzultzaileWeb.Itzuli(txtFilter) : itzultzaileWeb.Itzuli(pnlSincronizar)
            itzultzaileWeb.Itzuli(labelInfo2) : itzultzaileWeb.Itzuli(labelInfo3) : itzultzaileWeb.Itzuli(btnSincronizar)
            itzultzaileWeb.Itzuli(pnlSinCambios) : itzultzaileWeb.Itzuli(labelCambioDpto) : itzultzaileWeb.Itzuli(pnlEliminados)
            itzultzaileWeb.Itzuli(pnlEliminados) : itzultzaileWeb.Itzuli(labelInfo4) : itzultzaileWeb.Itzuli(btnEliminar)
            itzultzaileWeb.Itzuli(pnlAñadidos) : itzultzaileWeb.Itzuli(labelInfo5) : itzultzaileWeb.Itzuli(btnAñadir2)
            itzultzaileWeb.Itzuli(btnAñadir) : itzultzaileWeb.Itzuli(tabListado) : itzultzaileWeb.Itzuli(tabSincronizar)
            itzultzaileWeb.Itzuli(labelDepto) : itzultzaileWeb.Itzuli(btnCerrarM) : itzultzaileWeb.Itzuli(labelCtaNormal)
            itzultzaileWeb.Itzuli(btnGuardarM) : itzultzaileWeb.Itzuli(labelCtaReducida) : itzultzaileWeb.Itzuli(labelCtaExenta)
            itzultzaileWeb.Itzuli(btnEliminar2)
        End If
    End Sub

    ''' <summary>
    ''' Carga las cuentas contables no obsoletas
    ''' </summary>    
    Private Sub cargarCuentas()
        Try
            Dim deptBLL As New BLL.DepartamentosBLL
            Dim oDept As New ELL.Departamento
            Dim texto As String = txtFilter.Text.Trim
            If (IsNumeric(texto)) Then
                Dim iTexto As Integer = CInt(texto)
                oDept.CodigoDepartamento = texto
                oDept.Cuenta0 = iTexto : oDept.Cuenta8 = iTexto : oDept.Cuenta18 = iTexto
                oDept.OFImproductiva = iTexto
            Else
                oDept.Departamento = texto
            End If
            Dim lCuentas As List(Of ELL.Departamento) = deptBLL.loadList(oDept, Master.IdPlantaGestion, False)
            If (gvCuentas.Attributes("CurrentSortField") Is Nothing) Then
                gvCuentas.Attributes("CurrentSortField") = "CodigoDepartamento"
                gvCuentas.Attributes("CurrentSortDirection") = SortDirection.Ascending
            End If
            If (lCuentas IsNot Nothing) Then OrdenarLista(lCuentas, gvCuentas.Attributes("CurrentSortField"), gvCuentas.Attributes("CurrentSortDirection"))
            gvCuentas.DataSource = lCuentas
            gvCuentas.DataBind()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar las cuentas contables")
        End Try
    End Sub

    ''' <summary>
    ''' Muestra el panel modal para introducir la cuenta
    ''' </summary>
    ''' <param name="action">True lo muestra y false lo oculta</param>
    Private Sub ShowModalBox(ByVal action As Boolean)
        If (action) Then
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divModal').modal('show');", True)
        Else
            ScriptManager.RegisterStartupScript(Page, GetType(Page), "modal", "$('#divModal').modal('hide');$('body').removeClass('modal-open');$('.modal-backdrop').hide();", True) '2º intruccion necesaria por estar dentro de un updatePanel
        End If
    End Sub

#End Region

#Region "Eventos gridView"

    ''' <summary>
    ''' Se realiza la paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvCuentas_Paging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvCuentas.PageIndexChanging
        Try
            gvCuentas.PageIndex = e.NewPageIndex
            cargarCuentas()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se traducen las cabeceras
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvCuentas_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvCuentas.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oDepart As ELL.Departamento = e.Row.DataItem
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvCuentas, "Select$" + oDepart.CodigoDepartamento.ToString)
        End If
    End Sub

    ''' <summary>
    ''' Evento que surge al hacer click en la fila
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvCuentas_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvCuentas.RowCommand
        Try
            If (e.CommandName.ToLower = "select") Then
                Dim departBLL As New BLL.DepartamentosBLL
                Dim oDepart As ELL.Departamento = departBLL.loadInfo(CInt(e.CommandArgument), Master.IdPlantaGestion)
                lblDpto.Text = oDepart.CodigoDepartamento
                txtCuenta18.Text = oDepart.Cuenta18
                txtCuenta8.Text = oDepart.Cuenta8
                txtCuenta0.Text = oDepart.Cuenta0
                btnGuardarM.CommandArgument = e.CommandArgument
                ShowModalBox(True)
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se traducen las cabeceras
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvAñadidos_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvAñadidos.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        End If
    End Sub

    ''' <summary>
    ''' Se traducen las cabeceras
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvEliminados_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvEliminados.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        End If
    End Sub

    ''' <summary>
    ''' Se realiza el orden
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvCuentas_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs) Handles gvCuentas.Sorting
        Try
            gvCuentas.Attributes("CurrentSortField") = e.SortExpression
            If (gvCuentas.Attributes("CurrentSortDirection") Is Nothing) Then
                gvCuentas.Attributes("CurrentSortDirection") = SortDirection.Ascending
            Else
                gvCuentas.Attributes("CurrentSortDirection") = If(gvCuentas.Attributes("CurrentSortDirection") = SortDirection.Ascending, SortDirection.Descending, SortDirection.Ascending)
            End If
            cargarCuentas()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Ordena una lista sin conocer el tipo que es
    ''' </summary>
    ''' <param name="lista"></param>    
    ''' <param name="CampoOrden"></param>
    ''' <param name="DireccionCampo"></param>        
    Private Sub OrdenarLista(ByRef lista As List(Of ELL.Departamento), Optional ByVal CampoOrden As String = Nothing, Optional ByVal DireccionCampo As SortDirection = SortDirection.Ascending)
        Select Case CampoOrden
            Case "CodigoDepartamento"
                If (DireccionCampo = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of Integer)(Function(o) o.CodigoDepartamento).ToList
                Else
                    lista = lista.OrderByDescending(Of Integer)(Function(o) o.CodigoDepartamento).ToList
                End If
            Case "Departamento"
                If (DireccionCampo = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of String)(Function(o) o.Departamento).ToList
                Else
                    lista = lista.OrderByDescending(Of String)(Function(o) o.Departamento).ToList
                End If
            Case "Cuenta18"
                If (DireccionCampo = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of Integer)(Function(o) o.Cuenta18).ToList
                Else
                    lista = lista.OrderByDescending(Of Integer)(Function(o) o.Cuenta18).ToList
                End If
            Case "Cuenta8"
                If (DireccionCampo = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of Integer)(Function(o) o.Cuenta8).ToList
                Else
                    lista = lista.OrderByDescending(Of Integer)(Function(o) o.Cuenta8).ToList
                End If
            Case "Cuenta0"
                If (DireccionCampo = SortDirection.Ascending) Then
                    lista = lista.OrderBy(Of Integer)(Function(o) o.Cuenta0).ToList
                Else
                    lista = lista.OrderByDescending(Of Integer)(Function(o) o.Cuenta0).ToList
                End If
        End Select
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Busca los departamentos especificados en el cuadro de texto
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.ServerClick
        Try
            cargarCuentas()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Guarda las cuentas del departamento seleccionado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardarM.Click
        Try
            If (txtCuenta18.Text = String.Empty OrElse txtCuenta8.Text = String.Empty OrElse txtCuenta0.Text = String.Empty) Then
                ShowModalBox(True)
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Introduzca todos los datos")
            Else
                Dim navBLL As New BLL.NavisionBLL
                Dim mensaCuentas, mensa As String
                mensaCuentas = String.Empty
                Dim lCuentasCont As New List(Of Integer)
                lCuentasCont.Add(CInt(txtCuenta18.Text))
                If (Not lCuentasCont.Exists(Function(o) o = CInt(txtCuenta8.Text))) Then lCuentasCont.Add(CInt(txtCuenta8.Text))
                If (Not lCuentasCont.Exists(Function(o) o = CInt(txtCuenta0.Text))) Then lCuentasCont.Add(CInt(txtCuenta0.Text))
                If (Not navBLL.existenCuentasContable(Master.IdPlantaGestion, lCuentasCont, mensaCuentas)) Then
                    mensa = itzultzaileWeb.Itzuli("Las siguientes cuentas contables no existen en Navision") & ":" & mensaCuentas
                    log.Warn("CTAS_CONTABLES:" & mensa)
                    Master.MensajeAdvertencia = mensa
                    ShowModalBox(True)
                Else
                    Dim cuentasBLL As New BLL.DepartamentosBLL
                    Dim oDepart As New ELL.Departamento With {.CodigoDepartamento = btnGuardarM.CommandArgument, .Cuenta18 = CInt(txtCuenta18.Text), .Cuenta8 = CInt(txtCuenta8.Text), .Cuenta0 = CInt(txtCuenta0.Text)}
                    cuentasBLL.Update(oDepart)
                    ShowModalBox(False)
                    log.Info("CTAS_CONTABLES:Se ha actualizado la cuenta del departamento (" & btnGuardarM.CommandArgument & ") con 18% - " & txtCuenta18.Text & " | 8% - " & txtCuenta8.Text & " | 0% - " & txtCuenta0.Text)
                    Master.MensajeInfo = itzultzaileWeb.Itzuli("Cuenta actualizada")
                    cargarCuentas()
                End If
            End If
        Catch batzEx As BatzException
            ShowModalBox(True)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#End Region

#Region "Pestaña Sincronizar"

    ''' <summary>
    ''' Realiza la sincronizacion con una tabla donde estan los departamentos actualizados
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnSincronizar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSincronizar.Click
        Try
            log.Info("SINCRONIZACION:Se va a realizar una sincronizacion de departamentos")
            Sincronizar(True)
            log.Info("SINCRONIZACION:Sincronizacion realizada con exito")
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se sincronizan los departamentos
    ''' </summary>
    ''' <param name="bInfoLog">Informar el log</param>
    Private Sub Sincronizar(ByVal bInfoLog As Boolean)
        Try
            Dim deptBLL As New BLL.DepartamentosBLL
            Dim lEliminados, lAñadidos As List(Of ELL.Departamento)
            lEliminados = Nothing : lAñadidos = Nothing
            deptBLL.Sincronizar(Master.IdPlantaGestion, lAñadidos, lEliminados)
            If (lEliminados Is Nothing And lAñadidos Is Nothing) Then
                pnlSinCambios.Visible = True
                If (bInfoLog) Then log.Info("SINCRONIZACION:No hay cambios")
            Else
                pnlSinCambios.Visible = False
                If (lAñadidos IsNot Nothing) Then
                    pnlAñadidos.Visible = True
                    gvAñadidos.DataSource = lAñadidos
                    gvAñadidos.DataBind()
                    If (bInfoLog) Then log.Info("SINCRONIZACION:Existen " & lAñadidos.Count & " departamentos no registrados en Bidaiak")
                End If
                If (lEliminados IsNot Nothing) Then
                    pnlEliminados.Visible = True
                    gvEliminados.DataSource = lEliminados
                    gvEliminados.DataBind()
                    If (bInfoLog) Then log.Info("SINCRONIZACION:Se han eliminado " & lEliminados.Count & " departamentos")
                End If
            End If
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al sincronizar los departamentos", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Inicializa los controles antes de sincronizar
    ''' </summary>    
    Private Sub inicializarSincronizacion()
        pnlSinCambios.Visible = False : pnlAñadidos.Visible = False : pnlEliminados.Visible = False
        gvAñadidos.DataSource = Nothing : gvEliminados.DataSource = Nothing
        btnAñadir.OnClientClick = "return ChequearCuentas();" : btnAñadir2.OnClientClick = "return ChequearCuentas();"
        gvAñadidos.DataBind() : gvEliminados.DataBind()
    End Sub

    ''' <summary>
    ''' Añade los departamentos informados a la tabla
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Añadir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAñadir.Click, btnAñadir2.Click
        Try
            Dim lCuentas As New List(Of ELL.Departamento)
            Dim deptBLL As New BLL.DepartamentosBLL
            Dim oDept As ELL.Departamento
            Dim myTextBox As TextBox
            Dim iAñadir As Integer
            Dim bTodosAñadidos As Boolean = True
            Dim depAñadir As String = String.Empty
            For Each row As GridViewRow In gvAñadidos.Rows
                iAñadir = 0
                oDept = New ELL.Departamento With {.CodigoDepartamento = row.Cells(0).Text, .Departamento = row.Cells(1).Text, .IdPlanta = Master.IdPlantaGestion}
                myTextBox = CType(row.Cells(2).Controls(1), TextBox)
                If (myTextBox.Text <> String.Empty) Then
                    oDept.Cuenta18 = CInt(myTextBox.Text)
                    iAñadir += 1
                End If
                myTextBox = CType(row.Cells(3).Controls(1), TextBox)
                If (myTextBox.Text <> String.Empty) Then
                    oDept.Cuenta8 = CInt(myTextBox.Text)
                    iAñadir += 1
                End If
                myTextBox = CType(row.Cells(4).Controls(1), TextBox)
                If (myTextBox.Text <> String.Empty) Then
                    oDept.Cuenta0 = CInt(myTextBox.Text)
                    iAñadir += 1
                End If
                If (iAñadir = 3) Then
                    If (depAñadir <> String.Empty) Then depAñadir &= ","
                    depAñadir &= oDept.Departamento
                    lCuentas.Add(oDept)
                ElseIf (iAñadir > 0 And iAñadir < 3) Then
                    bTodosAñadidos = False
                End If
            Next
            deptBLL.Add(lCuentas)
            gvAñadidos.DataSource = Nothing
            gvAñadidos.DataBind()
            pnlAñadidos.Visible = False
            log.Info("CTAS_CONTABLES: Se han añadido los departamentos (" & depAñadir & ")")
            If (lCuentas.Count > 0 And bTodosAñadidos) Then
                Master.MensajeInfo = itzultzaileWeb.Itzuli("Departamentos añadidos")
            ElseIf (lCuentas.Count > 0 And Not bTodosAñadidos) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Se han añadido los departamentos pero existe alguno del cual no se han informado las 3 cuentas y no se ha añadido")
            End If
            Sincronizar(False)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al añadir los departamentos")
        End Try
    End Sub

    ''' <summary>
    ''' Marca como obsoletos, los departamentos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnEliminar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEliminar.Click, btnEliminar2.Click
        Try
            Dim deptBLL As New BLL.DepartamentosBLL
            Dim lCuentas As New List(Of Integer)
            Dim depQuitar As String = String.Empty
            For Each row As GridViewRow In gvEliminados.Rows
                If (depQuitar <> String.Empty) Then depQuitar &= ","
                depQuitar &= row.Cells(1).Text
                lCuentas.Add(CInt(row.Cells(0).Text))
            Next
            deptBLL.Delete(lCuentas)
            gvEliminados.DataSource = Nothing : gvEliminados.DataBind()
            pnlEliminados.Visible = False
            log.Info("los departamentos (" & depQuitar & ") se han marcado como obsoletos")
            Master.MensajeInfo = itzultzaileWeb.Itzuli("Departamentos eliminados")
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al marcar como obsoletos los departamentos")
        End Try
    End Sub

#End Region

End Class