Public Class ExportExcelMovil
    Inherits Page

    Private itzultzaileWeb As itzultzaile

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelExportando)
        End If
    End Sub

    ''' <summary>
    ''' Exporta a excel
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub ExportExcelMovil_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Export(Request.QueryString("mov"), Request.QueryString("est"))
        End If
    End Sub

    ''' <summary>
    ''' Exporta a excel. Funciona solo si esta instalado el 2007 porque no tengo la libreria del 2003
    ''' </summary>    
    Public Sub Export(ByVal movil As String, ByVal estado As String)
        ' Clear the response  
        Response.Clear()
        ' Set the type and filename  
        Response.AddHeader("content-disposition", "attachment;filename=MovilesLibres.xls")
        Response.ContentType = "application/vnd-excel"
        ' Add the HTML from the GridView to a StringWriter so we can write it out later  
        Dim gv As New GridView
        gv.DataSource = loadMovilesExtensiones(movil, estado)
        gv.DataBind()
        Dim sw As IO.StringWriter = New IO.StringWriter
        Dim hw As HtmlTextWriter = New HtmlTextWriter(sw)
        gv.RenderControl(hw)
        ' Write out the data  
        Response.Write(sw.ToString)
        Response.End()
    End Sub

    ''' <summary>
    ''' Carga las extensiones moviles
    ''' </summary>
    ''' <param name="movil">Movile</param>
    ''' <param name="estado">Estado</param>
    ''' <returns></returns>    
    Private Function loadMovilesExtensiones(ByVal movil As String, ByVal estado As String) As DataTable
        Dim ticket As ELL.TicketTlfno = CType(Session("Ticket"), ELL.TicketTlfno)
        Dim cultura As String = ticket.Culture
        Dim tlfnoComp As New BLL.TelefonoComponent
        Dim oTlfno As New ELL.Telefono With {.IdPlanta = ticket.IdPlantaActual, .Numero = movil}
        Dim direccion As String = "ASC"
        'Si es administrador de planta vera la de todos
        If Not (ticket.EsAdministradorPlanta) Then
            oTlfno.IdUsuarioGestor = ticket.IdUser
        End If
        Dim dt As DataTable = tlfnoComp.getTelefonosGestor2(oTlfno, ticket.IdPlantaActual, estado)
        If (dt IsNot Nothing) Then
            For index As Integer = dt.Rows.Count - 1 To 0 Step -1
                If (dt.Rows(index)("NOMBRE") = String.Empty) Then
                    dt.Rows(index)("FECHA_DESDE") = String.Empty
                Else
                    dt.Rows(index)("FECHA_DESDE") = CDate(dt.Rows(index)("FECHA_DESDE")).ToShortDateString
                End If
            Next
        End If
        Return dt
    End Function

End Class