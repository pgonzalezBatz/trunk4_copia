Imports System
Imports ClosedXML.Excel
Imports System.IO

Public Class Historicos
    Inherits PageBase

    Dim oControles As New BLL.ControlesBLL

#Region "Page Load"

    ''' <summary>
    ''' Muestra las colas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Try
                ComprobarAcceso()
            Catch ex As Exception
                Response.Redirect(PageBase.PAG_PERMISODENEGADO)
            End Try

            AgregarScriptBusquedaUsuarios()

            If Not Page.IsPostBack Then
                'SelOp.Operacion = "8101977"
                'txtSelectorOperacion.Text = "8101977"
                'txtFecha.Text = "06/05/2013"
                'txtFechaFin.Text = "06/06/2013"
                Initialize()
            End If

            Page.MaintainScrollPositionOnPostBack = True
        Catch ex As Exception
           Global_asax.log.Error("Error al cargar la página", ex)
        End Try
    End Sub

#End Region

#Region "Métodos"

    ''' <summary>
    ''' Comprueba que el perfil tenga acceso a la página
    ''' </summary>
    Private Sub ComprobarAcceso()
        If (Session("ticket") Is Nothing AndAlso Session("PerfilUsuario") Is Nothing) Then
            Response.Redirect(PageBase.PAG_PERMISODENEGADO)
        End If
    End Sub

    ''' <summary>
    ''' Inicializa las variables y vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Initialize()
        ClearDataViews()
        BindDataViews()
    End Sub

    ''' <summary>
    ''' Limpia los datos vinculados
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ClearDataViews()
    End Sub

    ''' <summary>
    ''' Vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BindDataViews()
    End Sub

    ''' <summary>
    ''' Agrega un script de búsqueda de usuario
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AgregarScriptBusquedaUsuarios()
        'Indicamos la cultura
        Dim cultura As String = CultureUser()

        'Verificador 1
        If (Not hfIdUsuario1 Is Nothing AndAlso Not String.IsNullOrEmpty(hfIdUsuario1.Value)) Then
            imgSeleccion1.Attributes("class") = "imagen-seleccionado"
        Else
            imgSeleccion1.Attributes("class") = "imagen-no-seleccionado"
        End If

        Dim script As String = "initBusquedaUsuarios('" & txtVerificador1.ClientID & "', '" & hfIdUsuario1.ClientID & "', '" & helper1.ClientID & "','../BuscarUsuarios.aspx',false,'','" & imgSeleccion1.ClientID & "', '" & cultura & "');"
        ScriptManager.RegisterStartupScript(Page, Me.GetType(), "Verificador1", script, True)

        'Verificador 2
        If (Not hfIdUsuario2 Is Nothing AndAlso Not String.IsNullOrEmpty(hfIdUsuario2.Value)) Then
            imgSeleccion2.Attributes("class") = "imagen-seleccionado"
        Else
            imgSeleccion2.Attributes("class") = "imagen-no-seleccionado"
        End If

        Dim script2 As String = "initBusquedaUsuarios('" & txtVerificador2.ClientID & "', '" & hfIdUsuario2.ClientID & "', '" & helper2.ClientID & "','../BuscarUsuarios.aspx',false,'','" & imgSeleccion2.ClientID & "', '" & cultura & "');"
        ScriptManager.RegisterStartupScript(Page, Me.GetType(), "Verificador2", script2, True)

        'Verificador 3
        If (Not hfIdUsuario3 Is Nothing AndAlso Not String.IsNullOrEmpty(hfIdUsuario3.Value)) Then
            imgSeleccion3.Attributes("class") = "imagen-seleccionado"
        Else
            imgSeleccion3.Attributes("class") = "imagen-no-seleccionado"
        End If

        'Dim cultura As String = CultureUser()

        Dim script3 As String = "initBusquedaUsuarios('" & txtVerificador3.ClientID & "', '" & hfIdUsuario3.ClientID & "', '" & helper3.ClientID & "','../BuscarUsuarios.aspx',false,'','" & imgSeleccion3.ClientID & "', '" & cultura & "');"
        ScriptManager.RegisterStartupScript(Page, Me.GetType(), "Verificador3", script3, True)
    End Sub

    ''' <summary>
    ''' Obtiene la cultura del usuario logueado
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CultureUser() As String
        Dim persona As New Sablib.ELL.Ticket
        Dim culture As String = "es-ES"

        If (Session("Ticket") IsNot Nothing) Then
            persona = CType(Session("Ticket"), Sablib.ELL.Ticket)
            culture = persona.Culture
        End If

        Return culture
    End Function

    ''' <summary>
    ''' Leemos de la sesión el identificador del usuario logueado
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetIdUsuario() As Integer
        If (Session("Ticket") IsNot Nothing) Then
            Dim ticketGene As Sablib.ELL.Ticket = Nothing
            Return ticketGene.IdUser
        Else : Return Integer.MinValue
        End If
        'Return 60210
    End Function

#End Region

#Region "Handlers"

    ''' <summary>
    ''' Buscar datos históricos tras el filtrado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnFiltrar_Click(sender As Object, e As EventArgs)
        Dim oHistoricos As New BLL.HistoricosBLL
        Dim listaHistoricos As List(Of ELL.Historicos)
        Dim UsuariosComponent As New Sablib.BLL.UsuariosComponent
        Dim gtkUsuario As New Sablib.ELL.Usuario
        Dim codigoOperacion As Integer
        Dim continuar As Boolean = True

        lblProblemaFechas.Visible = False

        If Not (String.IsNullOrEmpty(txtSelectorOperacion.Text.Trim())) Then
            codigoOperacion = CInt(txtSelectorOperacion.Text.Trim())
        Else
            codigoOperacion = Integer.MinValue
        End If

        Dim caracteristica As String = txtCaracteristica.Text.Trim()

        Dim fechaDesdeString As String = txtFecha.Text.Trim()
        Dim fechaHastaString As String = txtFechaFin.Text.Trim()
        If (String.IsNullOrEmpty(fechaDesdeString) AndAlso String.IsNullOrEmpty(fechaHastaString)) Then
            fechaHastaString = System.DateTime.Now.ToShortDateString()
            fechaDesdeString = System.DateTime.Now.AddDays(-5).ToShortDateString()
            'ElseIf (String.IsNullOrEmpty(fechaDesdeString)) Then
        Else
            'Comprobamos que la diferencia entre las dos fecha no supera 1 semana
            Dim fechaDesde As DateTime = DateTime.Parse(fechaDesdeString)
            Dim fechaHasta As DateTime = DateTime.Parse(fechaHastaString)
            Dim difFechas As TimeSpan = fechaHasta.Subtract(fechaDesde)
            If (difFechas.Days > 7) Then
                lblProblemaFechas.Visible = True
                continuar = False
            End If
        End If

        Dim listaVerificadores As New List(Of Integer)
        If Not (String.IsNullOrEmpty(hfIdUsuario1.Value)) Then
            gtkUsuario = UsuariosComponent.GetUsuario(New Sablib.ELL.Usuario With {.Id = hfIdUsuario1.Value, .IdPlanta = 1}, False)
            listaVerificadores.Add(gtkUsuario.CodPersona)
            'listaVerificadores.Add(hfIdUsuario1.Value)
        End If
        If Not (String.IsNullOrEmpty(hfIdUsuario2.Value)) Then
            gtkUsuario = UsuariosComponent.GetUsuario(New Sablib.ELL.Usuario With {.Id = hfIdUsuario2.Value, .IdPlanta = 1}, False)
            listaVerificadores.Add(gtkUsuario.CodPersona)
            'listaVerificadores.Add(hfIdUsuario2.Value)
        End If
        If Not (String.IsNullOrEmpty(hfIdUsuario3.Value)) Then
            gtkUsuario = UsuariosComponent.GetUsuario(New Sablib.ELL.Usuario With {.Id = hfIdUsuario3.Value, .IdPlanta = 1}, False)
            listaVerificadores.Add(gtkUsuario.CodPersona)
            'listaVerificadores.Add(hfIdUsuario3.Value)
        End If

        If (continuar) Then
            listaHistoricos = oHistoricos.ObtenerControles(codigoOperacion, caracteristica, fechaDesdeString, fechaHastaString, listaVerificadores)
            gvControles.DataSource = listaHistoricos
            gvControles.DataBind()

            For Each col In gvControles.Columns
                If (col.HeaderText = "Cod.Op") Then
                    If Not (String.IsNullOrEmpty(txtSelectorOperacion.Text)) Then
                        col.Visible = False
                    Else
                        col.visible = True
                    End If
                ElseIf (col.HeaderText = "Reparación") Then
                    If (Not String.IsNullOrEmpty(txtVerificador1.Text) OrElse Not String.IsNullOrEmpty(txtVerificador2.Text) OrElse Not String.IsNullOrEmpty(txtVerificador3.Text)) Then
                        col.Visible = False
                    Else
                        col.visible = True
                    End If
                ElseIf (col.HeaderText = "Cambio Ref") Then
                    If (Not String.IsNullOrEmpty(txtVerificador1.Text) OrElse Not String.IsNullOrEmpty(txtVerificador2.Text) OrElse Not String.IsNullOrEmpty(txtVerificador3.Text)) Then
                        col.Visible = False
                    Else
                        col.visible = True
                    End If
                ElseIf (col.HeaderText = "Valor") Then
                    If (String.IsNullOrEmpty(txtCaracteristica.Text)) Then
                        col.Visible = False
                    Else
                        col.visible = True
                    End If
                ElseIf (col.HeaderText = "Fichero") Then
                    If (String.IsNullOrEmpty(txtCaracteristica.Text)) Then
                        col.Visible = False
                    Else
                        col.visible = True
                    End If
                End If
            Next

            upControles.Update()

        End If
    End Sub

    Protected Sub gvControles_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            'Sólo se tienen en cuenta las filas con datos
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                If Not (String.IsNullOrEmpty(txtCaracteristica.Text)) Then
                    Dim imagenCarac As Image = CType(e.Row.FindControl("imgCaracteristicaValor"), Image)
                    Dim valorCarac As Label = CType(e.Row.FindControl("lblCaracteristicaValor"), Label)
                    Dim hlDescargar As HyperLink = CType(e.Row.FindControl("hlDescargar"), HyperLink)

                    If (imagenCarac IsNot Nothing AndAlso valorCarac IsNot Nothing) Then
                        'If Not (String.IsNullOrEmpty(tipo.Text)) Then
                        If (valorCarac.Text.Equals("OK") OrElse valorCarac.Text.Equals("NOK")) Then
                            imagenCarac.Visible = True
                            valorCarac.Visible = False
                            If (valorCarac.Text.Equals("OK")) Then
                                imagenCarac.ImageUrl = "..\App_Themes\Tema1\Imagenes\caracteristica_bien.png"
                            ElseIf (valorCarac.Text.Equals("NOK")) Then
                                imagenCarac.ImageUrl = "..\App_Themes\Tema1\Imagenes\caracteristica_mal.png"
                            End If
                        ElseIf Not (String.IsNullOrEmpty(valorCarac.Text)) Then
                            If (valorCarac.Text.Contains("*")) Then
                                valorCarac.Style.Add("color", "red")
                                valorCarac.Style.Add("font-size", "14px")
                                valorCarac.Style.Add("font-weight", "bold")
                            Else
                                valorCarac.Style.Add("color", "#62CE00")
                                valorCarac.Style.Add("font-size", "14px")
                                valorCarac.Style.Add("font-weight", "bold")
                            End If
                            imagenCarac.Visible = False
                            valorCarac.Visible = True
                        Else
                            imagenCarac.Visible = False
                            valorCarac.Visible = False
                        End If
                    End If

                    Dim lblIdControl As Label = CType(e.Row.FindControl("lblIdControl"), Label)
                    Dim lblIdRegistro As Label = CType(e.Row.FindControl("lblIdRegistro"), Label)
                    Dim imagenFichero As Image = CType(e.Row.FindControl("imgDescargar"), Image)
                    'If (oFicheros.ExisteFichero(lblIdControl.Text, lblIdRegistro.Text)) Then
                    '    imagenFichero.Visible = True
                    '    hlDescargar.NavigateUrl = String.Format(hlDescargar.NavigateUrl, lblIdControl.Text, lblIdRegistro.Text)
                    'Else
                    '    imagenFichero.Visible = False
                    'End If
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    ''' <summary>
    ''' Link para ver los datos de un control
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnVerDatos_Click(sender As Object, e As EventArgs)
        'Retrieve Customer ID  
        Dim lnkVerDatos As LinkButton = TryCast(sender, LinkButton)
        Dim idControl As String = lnkVerDatos.CommandArgument

        Dim listaControlesValores As List(Of ELL.ControlesValoresResumen)
        listaControlesValores = oControles.ObtenerControlValores(idControl)
        gvControlesValores.DataSource = listaControlesValores
        gvControlesValores.DataBind()

        Dim errorControl As ELL.ControlesErrores
        Dim listaErroresControl As New List(Of ELL.ControlesErrores)

        errorControl = oControles.ObtenerControlErrores(idControl)
        listaErroresControl.Add(errorControl)

        If (errorControl IsNot Nothing) Then
            dvErrores.Visible = True
            dvErrores.DataSource = listaErroresControl
            dvErrores.DataBind()
        Else
            dvErrores.Visible = False
            dvErrores.DataSource = Nothing
            dvErrores.DataBind()
        End If

        modalPopUpExtender1.Show()

        upPopUp.Update()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnOk_Click(sender As Object, e As EventArgs)
        modalPopUpExtender1.Hide()
    End Sub

    ''' <summary>
    ''' RwDataBound del grid anidado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvControlesValores_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            'Sólo se tienen en cuenta las filas con datos
            If (e.Row.RowType = DataControlRowType.DataRow) Then

                Dim imagenCarac As Image = CType(e.Row.FindControl("imgCaracteristicaValor"), Image)
                Dim valorCarac As Label = CType(e.Row.FindControl("lblCaracteristicaValor"), Label)
                Dim hlDescargar As HyperLink = CType(e.Row.FindControl("hlDescargar"), HyperLink)

                If (imagenCarac IsNot Nothing AndAlso valorCarac IsNot Nothing) Then
                    'If Not (String.IsNullOrEmpty(tipo.Text)) Then
                    If (valorCarac.Text.Equals("OK") OrElse valorCarac.Text.Equals("NOK")) Then
                        imagenCarac.Visible = True
                        valorCarac.Visible = False
                        If (valorCarac.Text.Equals("OK")) Then
                            imagenCarac.ImageUrl = "..\App_Themes\Tema1\Imagenes\caracteristica_bien.png"
                        ElseIf (valorCarac.Text.Equals("NOK")) Then
                            imagenCarac.ImageUrl = "..\App_Themes\Tema1\Imagenes\caracteristica_mal.png"
                        End If
                    ElseIf Not (String.IsNullOrEmpty(valorCarac.Text)) Then
                        If (valorCarac.Text.Contains("*")) Then
                            valorCarac.Style.Add("color", "red")
                            valorCarac.Style.Add("font-size", "14px")
                            valorCarac.Style.Add("font-weight", "bold")
                        Else
                            valorCarac.Style.Add("color", "#62CE00")
                            valorCarac.Style.Add("font-size", "14px")
                            valorCarac.Style.Add("font-weight", "bold")
                        End If
                        imagenCarac.Visible = False
                        valorCarac.Visible = True
                    Else
                        imagenCarac.Visible = False
                        valorCarac.Visible = False
                    End If
                End If

                Dim imagenVerFicheros As Image = CType(e.Row.FindControl("imgDescargarDetalle"), Image)
                Dim idRegistro As Label = CType(e.Row.FindControl("lblIdRegistro"), Label)
                Dim idControl As Label = CType(e.Row.FindControl("lblIdControl"), Label)
                'If (oFicheros.ExisteFichero(idControl.Text, idRegistro.Text)) Then
                '    imagenVerFicheros.Visible = True
                '    hlDescargar.NavigateUrl = String.Format(hlDescargar.NavigateUrl, idControl.Text, idRegistro.Text)
                'Else
                '    imagenVerFicheros.Visible = False
                'End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    ' ''' <summary>
    ' ''' Exportar los datos del filtrado de búsqueda a documento Excel
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Protected Sub btnExcel_Click(sender As Object, e As EventArgs)
    ''Creamos un nuevo Workbook
    'Dim wb As New XLWorkbook()
    'Dim ms As New MemoryStream()

    ''Creamos un worksheet
    'Dim ws = wb.Worksheets.Add("KTROL")

    'Dim filaN1 = ws.Row(1)
    'filaN1.Height = 50

    'ws.Cell("A1").Value = "BATZ"
    'Dim rngBatz = ws.Range("A1:C2").Merge()
    'rngBatz.Style.Border.RightBorder = XLBorderStyleValues.None
    'rngBatz.Style.Font.SetFontSize("20")
    'rngBatz.Style.Font.Bold = True
    'rngBatz.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
    'rngBatz.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left

    'ws.Cell("D1").Value = "HOJA DE REGISTROS"
    'Dim rngHojaRegistros = ws.Range("D1:G2").Merge()
    'rngHojaRegistros.Style.Font.SetFontSize("20")
    'rngHojaRegistros.Style.Font.Bold = True
    'rngHojaRegistros.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
    'rngHojaRegistros.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center

    'Dim filaN3 = ws.Row(3)
    'filaN3.Height = 30

    'ws.Cell("A3").Value = "CLIENTE: " + "AUDI"
    ''ws.Cell("A3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
    'Dim rngCliente = ws.Range("A3:E3").Merge()
    'rngCliente.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
    'rngCliente.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left

    'ws.Cell("F3").Value = "REFERENCIA DEL CLIENTE: " + "8K0 011 031 K"
    ''ws.Cell("D3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
    'Dim rngReferenciaCliente = ws.Range("F3:K3").Merge()
    'rngReferenciaCliente.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
    'rngReferenciaCliente.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left

    'ws.Cell("L3").Value = "SECCION: " + "ASSEMBLY - MONTAJE"
    ''ws.Cell("H3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
    'Dim rngSeccion = ws.Range("L3:P3").Merge()
    'rngSeccion.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
    'rngSeccion.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left

    'Dim filaN4 = ws.Row(4)
    'filaN4.Height = 30

    'ws.Cell("A4").Value = "DENOMINACION: " + "ELEVADOR B8"
    'Dim rngDenominacion = ws.Range("A4:E4").Merge()
    'rngDenominacion.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
    'rngDenominacion.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left

    'ws.Cell("F4").Value = "CODIGO OPERACION BATZ: " + "8101977"
    ''ws.Cell("D3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
    'Dim rngCodigoOperacion = ws.Range("D4:H4").Merge()
    'rngCodigoOperacion.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
    'rngCodigoOperacion.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left

    'ws.Cell("I4").Value = "OPERACION: " + "ASSEMBLY - MONTAJE  Elevador B8"
    ''ws.Cell("H3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
    'Dim rngOperacion = ws.Range("I4:M4").Merge()
    'rngOperacion.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center
    'rngOperacion.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left

    'wb.SaveAs(ms)

    'Try
    '    Dim nombreFichero As String = "HelloWorld.xlsx"
    '    'Dim httpResponse As HttpResponse = Response
    '    'Response.ClearContent()
    '    '
    '    Response.ClearContent()
    '    Response.ClearHeaders()
    '    Response.ContentType = "application/vnd.ms-excel"
    '    Response.AddHeader("Content-Disposition", "attachment;filename=" + nombreFichero)
    '    Response.BinaryWrite(ms.ToArray())
    'Catch ex As Exception
    'Finally
    '    Response.End()
    'End Try        
    'End Sub

#End Region

End Class