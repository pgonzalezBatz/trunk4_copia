'Imports Excel = Microsoft.Office.Interop.Excel
Imports TelefoniaLib

Public Class ExportExcelMovil
    Inherits System.Web.UI.Page

    Private itzultzaileWeb As TraduccionesLib.itzultzaile

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
            'Export("\\supelegor\Documentos\Logs\Telefonia\MovilesLibres_" & Session.SessionID & ".xls", Request.QueryString("mov"), Request.QueryString("est"))
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
        Dim sw As System.IO.StringWriter = New System.IO.StringWriter
        Dim hw As System.Web.UI.HtmlTextWriter = New HtmlTextWriter(sw)
        gv.RenderControl(hw)

        ' Write out the data  
        Response.Write(sw.ToString)
        Response.End()
        '******************************
        '    Dim oXL As Excel.Application
        '    Dim oWB As Excel.Workbook
        '    Dim oSheet As Excel.Worksheet
        '    Dim oRange As Excel.Range

        '    ' Start Excel and get Application object. 
        '    oXL = New Excel.Application

        '    ' Set some properties 
        '    oXL.Visible = False
        '    oXL.DisplayAlerts = False

        '    ' Get a new workbook. 
        '    oWB = oXL.Workbooks.Add

        '    ' Get the active sheet 
        '    oSheet = DirectCast(oWB.ActiveSheet, Excel.Worksheet)
        '    oSheet.Name = "Moviles"

        '    ' Process the DataTable 
        '    Dim dt As Data.DataTable = loadMovilesExtensiones(movil, estado)

        '    ' Create the data rows 
        '    Dim rowCount As Integer = 1
        '    For Each dr As DataRow In dt.Rows
        '        rowCount += 1
        '        For i As Integer = 1 To dt.Columns.Count
        '            ' Add the header the first time through 
        '            If rowCount = 2 Then
        '                oSheet.Cells(1, i) = dt.Columns(i - 1).ColumnName
        '            End If
        '            oSheet.Cells(rowCount, i) = dr.Item(i - 1).ToString
        '        Next
        '    Next

        '    ' Resize the columns 
        '    oRange = oSheet.Range(oSheet.Cells(1, 1), _
        '              oSheet.Cells(rowCount, dt.Columns.Count))
        '    oRange.EntireColumn.AutoFit()

        '    ' Save the sheet and close 
        '    oSheet = Nothing
        '    oRange = Nothing
        '    oWB.SaveAs(fileName)
        '    oWB.Close()
        '    oWB = Nothing
        '    oXL.Quit()

        '    ' Clean up 
        '    ' NOTE: When in release mode, this does the trick 
        '    GC.WaitForPendingFinalizers()
        '    GC.Collect()
        '    GC.WaitForPendingFinalizers()
        '    GC.Collect()

        '    Dim excel As Byte() = IO.File.ReadAllBytes(fileName)
        '    Response.Clear()
        '    Response.AddHeader("content-disposition", "attachment;filename=MovilesLibres.xls")
        '    Response.ContentType = "application/ms-excel"
        '    Response.OutputStream.Write(excel, 0, excel.Length)
        '    Response.OutputStream.Flush()
        '    Response.OutputStream.Close()
        '    Response.End()
        'End Sub

        'Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
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
        Dim oTlfno As New ELL.Telefono
        Dim direccion As String = "ASC"

        oTlfno.IdPlanta = ticket.IdPlantaActual
        oTlfno.Numero = movil
        'Si es administrador de planta o administrador general, vera la de todos
        If Not (ticket.EsAdministrador Or ticket.EsAdministradorPlanta) Then
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