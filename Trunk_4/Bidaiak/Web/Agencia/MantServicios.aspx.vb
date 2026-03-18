Public Class MantServicios
    Inherits PageBase

#Region "Page Load"

    ''' <summary>
    ''' Inicializa el mantenimiento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Servicios de agencia"
                cargarListado()
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
    Private Sub BusquedaPersonas_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelListadoServ) : itzultzaileWeb.Itzuli(lnkNuevo) : itzultzaileWeb.Itzuli(labelNombre)
            itzultzaileWeb.Itzuli(rfvNombre) : itzultzaileWeb.Itzuli(labelDescr) : itzultzaileWeb.Itzuli(chbReqUsuario)
            itzultzaileWeb.Itzuli(chbObsoleto) : itzultzaileWeb.Itzuli(btnGuardar) : itzultzaileWeb.Itzuli(btnVolver)
            itzultzaileWeb.Itzuli(revDescripcion)
        End If
    End Sub

#End Region

#Region "Vista Listado"

    ''' <summary>
    ''' Carga el listado con los servicios de agencia existentes
    ''' </summary>
    Private Sub cargarListado()
        mvServicios.ActiveViewIndex = 0
        Dim servBLL As New BLL.ServicioDeAgenciaBLL
        Dim lServicios As List(Of ELL.ServicioDeAgencia) = servBLL.loadList(Master.IdPlantaGestion)
        If (lServicios IsNot Nothing) Then lServicios = lServicios.OrderBy(Of String)(Function(o) o.Nombre).ToList
        gvServicios.DataSource = lServicios
        gvServicios.DataBind()
    End Sub

    ''' <summary>
    ''' Se redirige al detalle de un servicio
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvServicios_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvServicios.RowCommand
        Try
            mostrarDetalle(e.CommandArgument)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se enlazan los datos con la tabla
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvServicios_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvServicios.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oServ As ELL.ServicioDeAgencia = e.Row.DataItem
            If (oServ.Obsoleto) Then
                e.Row.CssClass = "danger"
                e.Row.ToolTip = itzultzaileWeb.Itzuli("obsoleto")
            End If
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvServicios, "Select$" + CStr(oServ.Id))
        End If
    End Sub

    ''' <summary>
    ''' Paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvServicios_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvServicios.PageIndexChanging
        Try
            gvServicios.PageIndex = e.NewPageIndex
            cargarListado()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Abre el detalle para registrar un nuevo servicio
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub lnkNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkNuevo.Click
        Try
            mostrarDetalle(Integer.MinValue)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Detalle"

    ''' <summary>
    ''' Muestra el detalle del objeto
    ''' </summary>    
    ''' <param name="id">Id del servicio</param>
    Private Sub mostrarDetalle(ByVal id As Integer)
        inicializarDetalle()
        If (id <> Integer.MinValue) Then
            Dim servBLL As New BLL.ServicioDeAgenciaBLL
            Dim oServAg As ELL.ServicioDeAgencia = servBLL.loadInfo(id)
            txtNombre.Text = oServAg.Nombre
            txtDescripcion.Text = oServAg.Descripcion
            chbObsoleto.Checked = oServAg.Obsoleto
            chbObsoleto.Visible = True
            chbReqUsuario.Checked = oServAg.RequiereUsuario
            btnGuardar.CommandArgument = oServAg.Id
        End If
    End Sub

    ''' <summary>
    ''' Inicializa el formulario de detalle
    ''' </summary>    
    Private Sub inicializarDetalle()
        txtNombre.Text = String.Empty : txtDescripcion.Text = String.Empty
        chbObsoleto.Checked = False : chbObsoleto.Visible = False
        chbReqUsuario.Checked = False
        btnGuardar.CommandArgument = String.Empty
        mvServicios.ActiveViewIndex = 1
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Guarda la informacion del servicio de agencia
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            If (txtNombre.Text = String.Empty) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe seleccionar todos los datos")
            Else
                Dim servBLL As New BLL.ServicioDeAgenciaBLL
                Dim oServAgen As New ELL.ServicioDeAgencia With {.Nombre = txtNombre.Text.Trim, .Descripcion = txtDescripcion.Text.Trim, .Obsoleto = chbObsoleto.Checked, .IdPlanta = Master.IdPlantaGestion, .RequiereUsuario = chbReqUsuario.Checked}
                If (btnGuardar.CommandArgument <> String.Empty) Then oServAgen.Id = CInt(btnGuardar.CommandArgument)
                servBLL.Save(oServAgen)
                If (btnGuardar.CommandArgument = String.Empty) Then
                    log.Info("MANT_SERVICIOS: Se ha registrado un nuevo servicio de agencia (" & txtNombre.Text & ")")
                Else
                    Dim mensa As String = "MANT_SERVICIOS: Se han modificado los datos del servicio de agencia (" & txtNombre.Text & ")"
                    mensa &= If(chbObsoleto.Checked, " o se ha marcado como obsoleta", " o se ha desmarcado de obsoleta")
                    log.Info(mensa)
                End If
                Master.MensajeInfo = itzultzaileWeb.Itzuli("datosGuardados")
                Try
                    cargarListado()
                Catch ex As Exception
                    Dim sms As String = itzultzaileWeb.Itzuli("datosGuardados") & ". " & itzultzaileWeb.Itzuli("Ha ocurrido un error al listar los servicios")
                    Master.MensajeError = sms
                End Try
            End If
        Catch batzEx As BatzException
            Master.MensajeError = "errGuardar"
        End Try
    End Sub

    ''' <summary>
    ''' Cancela y vuelve al listado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolver.Click
        Volver()
    End Sub

    ''' <summary>
    ''' Vuelve a cargar el listado
    ''' </summary>    
    Private Sub Volver()
        Try
            cargarListado()
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

End Class