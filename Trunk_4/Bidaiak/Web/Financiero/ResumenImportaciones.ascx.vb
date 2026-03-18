Public Class ResumenImportaciones
    Inherits UserControl

#Region "Variables"

    Public Event SubirFichero(ByVal año As Integer, ByVal mes As Integer)
    Public Event ErrorGenerado(ByVal mensaje As String)
    Private itzultzaileWeb As New Itzultzaile

    ''' <summary>
    ''' Tipos de importaciones posibles
    ''' </summary>    
    Public Enum TipoImp As Integer
        Visa = 1
        Eroski = 2
    End Enum

    ''' <summary>
    ''' Indicara el tipo de importacion
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public Property TipoImportacion As TipoImp
        Get
            Return CType(ViewState("tipo"), TipoImp)
        End Get
        Set(value As TipoImp)
            ViewState("tipo") = value
        End Set
    End Property

#End Region

#Region "Page Load"

    ''' <summary>
    ''' inicializa los controles
    ''' </summary>
    Public Sub iniciar()
        cargarAños()
        pintarImportaciones()
    End Sub

    ''' <summary>
    ''' Se traduce el control
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelSel) : itzultzaileWeb.Itzuli(lnkPresupuestosFacturas)
        End If
    End Sub

#End Region

#Region "Carga de datos"

    ''' <summary>
    ''' Carga los años seleccionables
    ''' El año actual y dos anteriores
    ''' </summary>    
    Private Sub cargarAños()
        If (ddlAños.Items.Count = 0) Then
            ddlAños.Items.Add(Now.Year)
            If (Now.Year - 1 >= 2013) Then ddlAños.Items.Add(Now.Year - 1)
            If (Now.Year - 2 >= 2013) Then ddlAños.Items.Add(Now.Year - 2)
        End If
        ddlAños.SelectedValue = Now.Year
    End Sub

    ''' <summary>
    ''' Pinta una tabla con la informacion de las importaciones
    ''' Indica si se han realizado o no
    ''' </summary>    
    Private Sub pintarImportaciones()
        Try
            Dim importBLL As New BLL.BidaiakBLL
            Dim info As BLL.BidaiakBLL.Importacion
            Dim row As TableRow
            Dim cell As TableCell
            Dim img As ImageButton
            Dim importacionPend As Integer = 0
            Dim idPlanta As Integer = CInt(Session("IdPlanta"))
            'Cabecera
            row = New TableRow
            row.Cells.Add(New TableHeaderCell With {.Text = itzultzaileWeb.Itzuli("Año")})
            row.Cells.Add(New TableHeaderCell With {.Text = itzultzaileWeb.Itzuli("ene")})
            row.Cells.Add(New TableHeaderCell With {.Text = itzultzaileWeb.Itzuli("feb")})
            row.Cells.Add(New TableHeaderCell With {.Text = itzultzaileWeb.Itzuli("mar")})
            row.Cells.Add(New TableHeaderCell With {.Text = itzultzaileWeb.Itzuli("abr")})
            row.Cells.Add(New TableHeaderCell With {.Text = itzultzaileWeb.Itzuli("may")})
            row.Cells.Add(New TableHeaderCell With {.Text = itzultzaileWeb.Itzuli("jun")})
            row.Cells.Add(New TableHeaderCell With {.Text = itzultzaileWeb.Itzuli("jul")})
            row.Cells.Add(New TableHeaderCell With {.Text = itzultzaileWeb.Itzuli("ago")})
            row.Cells.Add(New TableHeaderCell With {.Text = itzultzaileWeb.Itzuli("sep")})
            row.Cells.Add(New TableHeaderCell With {.Text = itzultzaileWeb.Itzuli("oct")})
            row.Cells.Add(New TableHeaderCell With {.Text = itzultzaileWeb.Itzuli("nov")})
            row.Cells.Add(New TableHeaderCell With {.Text = itzultzaileWeb.Itzuli("dic")})
            tImp.Rows.Add(row)
            'Se pinta la informacion de cada linea
            row = New TableRow
            cell = New TableCell : cell.Text = ddlAños.SelectedValue : row.Cells.Add(cell)
            For mes As Integer = 1 To 12
                cell = New TableCell
                'Si es la ultima semana, tambien se podra subir el fichero del mes siguiente
                If ((Now.Year > CInt(ddlAños.SelectedValue)) OrElse _
                    (Now.Year = CInt(ddlAños.SelectedValue) AndAlso (Now.Month > mes OrElse (Now.Month = mes AndAlso Now.Day > 24)))) Then
                    info = importBLL.loadImportacionDoc(TipoImportacion, CInt(ddlAños.SelectedValue), mes, idPlanta)
                    'NAV:Cuando se quiera probar a volver a procesar un fichero ya procesado
                    'If (TipoImportacion = TipoImp.Visa AndAlso (mes = 3 OrElse mes = 4)) Then
                    'info = Nothing
                    'If (TipoImportacion = TipoImp.Eroski AndAlso (mes = 1 OrElse mes = 4)) Then
                    'info = Nothing
                    'End If
                    If (info IsNot Nothing) Then 'Importacion ya realiza
                            img = New ImageButton
                            img.ImageUrl = "~/App_Themes/Tema1/Images/Aceptada.png"
                            img.ToolTip = itzultzaileWeb.Itzuli("Fichero subido. Haga click para ver el resumen")
                            Dim paginaResumen As String = If(TipoImportacion = TipoImp.Visa, "ViewImportVisa.aspx", "ViewImportEroski.aspx")
                            img.OnClientClick = "window.location.href='" & paginaResumen & "?id=" & info.Id & "';return false;"
                            cell.Controls.Add(img)
                        Else  'Mostrar para subir
                            img = New ImageButton
                            img.ImageUrl = "~/App_Themes/Tema1/Images/EstadosHojas/Upload.png"
                            img.ToolTip = If(TipoImportacion = TipoImp.Visa, itzultzaileWeb.Itzuli("Subir fichero de visas"), itzultzaileWeb.Itzuli("Subir factura de Eroski"))
                            img.OnClientClick = "subirFichero(" & ddlAños.SelectedValue & "," & mes & ");return false;"
                            cell.Controls.Add(img)
                            importacionPend += 1
                        End If
                    Else
                        img = New ImageButton
                    img.ImageUrl = "~/App_Themes/Tema1/Images/EstadosHojas/SinVisa.png"
                    img.ToolTip = itzultzaileWeb.Itzuli("Todavia no es posible subir el fichero")
                    img.Style.Add("cursor", "default")
                    img.OnClientClick = "return false;"
                    cell.Controls.Add(img)
                End If
                row.Cells.Add(cell)
            Next
            tImp.Rows.Add(row)
            If (importacionPend > 0) Then
                divPendientes.Visible = True
                lblImportacionesPend.Text = itzultzaileWeb.Itzuli("Quedan [X] importaciones pendientes de realizar").Replace("[X]", importacionPend)
            Else
                divPendientes.Visible = False
            End If
            lnkPresupuestosFacturas.Visible = (TipoImportacion = TipoImp.Eroski)
        Catch ex As Exception
            PageBase.log.Error("Error al pintar la informacion del resumen de importaciones de tipo (" & [Enum].GetName(GetType(TipoImp), TipoImportacion) & ")", ex)
            RaiseEvent ErrorGenerado(itzultzaileWeb.Itzuli("Error al mostrar la informacion"))
        End Try
    End Sub

    ''' <summary>
    ''' Muestra la informacion de las importaciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ddlAños_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAños.SelectedIndexChanged
        pintarImportaciones()
    End Sub

    ''' <summary>
    ''' Se manda subir un fichero
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnLanzar_Click(sender As Object, e As EventArgs) Handles btnLanzar.Click
        Dim param As String() = hfAnoMes.Value.Split("_")
        RaiseEvent SubirFichero(CInt(param(0)), CInt(param(1)))
    End Sub

    ''' <summary>
    ''' Se accede a la pagina de justificacion de facturas y presupuestos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub lnkPresupuestosFacturas_Click(sender As Object, e As EventArgs) Handles lnkPresupuestosFacturas.Click
        Response.Redirect("Presupuestos/PresupuestosFacturas.aspx", False)
    End Sub

#End Region

End Class