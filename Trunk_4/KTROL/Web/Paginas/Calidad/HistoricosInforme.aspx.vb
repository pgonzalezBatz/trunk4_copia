Imports System
Imports ClosedXML.Excel
Imports System.IO

Public Class HistoricosInforme
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

            If Not (Page.IsPostBack) Then
                EstablecerFechas()
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

    ''' <summary>
    ''' Devuelve true si la relación entre la referencia y la operación es correcta
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function VerificarReferenciaOperacion(ByVal referencia As String, ByVal operacion As String) As Boolean
        Dim BBDD As New KaPlanLib.DAL.ELL
        Dim listaRelaciones As List(Of KaPlanLib.Registro.OPERACIONES_DE_UN_ARTICULO)
        listaRelaciones = (From operacionesArticulo In BBDD.OPERACIONES_DE_UN_ARTICULO _
          Join maestroArticulos In BBDD.MAESTRO_ARTICULOS On operacionesArticulo.CODIGO Equals maestroArticulos.CODIGO _
          Where maestroArticulos.CODIGO = referencia And operacionesArticulo.COD_OPERACION = operacion
          Select operacionesArticulo).ToList()
        If (listaRelaciones.Count = 1) Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Ponemos fecha límite a las fechas y forzamos a que hagan click en la imagen del calendario
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EstablecerFechas()
        txtFechaDesde.Attributes.Add("readonly", "readonly")
        txtFechaHasta.Attributes.Add("readonly", "readonly")
        imgCalendarioDesde_CalendarExtender.EndDate = DateTime.Today
        imgCalendarioHasta_CalendarExtender.EndDate = DateTime.Today
    End Sub

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
        Dim UsuariosComponent As New Sablib.BLL.UsuariosComponent
        Dim gtkUsuario As New Sablib.ELL.Usuario
        Dim referencia As Integer
        Dim codigoOperacion As Integer
        Dim continuar As Boolean = True

        Dim fechaDesdeString As String = txtFechaDesde.Text.Trim()
        Dim fechaHastaString As String = txtFechaHasta.Text.Trim()
        If (String.IsNullOrEmpty(fechaDesdeString) AndAlso String.IsNullOrEmpty(fechaHastaString)) Then
            fechaHastaString = System.DateTime.Now.ToShortDateString()
            fechaDesdeString = System.DateTime.Now.AddDays(-5).ToShortDateString()
        Else
            Dim fechaDesde As DateTime = DateTime.Parse(fechaDesdeString)
            Dim fechaHasta As DateTime = DateTime.Parse(fechaHastaString)
        End If

        If Not (String.IsNullOrEmpty(selOperacion.Operacion.Trim())) Then
            codigoOperacion = CInt(selOperacion.Operacion.Trim())
        Else
            continuar = False
            Master.MensajeError = "Debes seleccionar un código de operación"
        End If

        If Not (String.IsNullOrEmpty(selReferencia.Referencia.Trim())) Then
            referencia = CInt(selReferencia.Referencia.Trim())
        Else
            continuar = False
            Master.MensajeError = "Debes seleccionar una referencia"
        End If

        If (continuar) Then
            If Not (VerificarReferenciaOperacion(selReferencia.Referencia.Trim(), selOperacion.Operacion.Trim())) Then
                continuar = False                
                Master.MensajeError = String.Format("El código de operación {0} no está relacionado con la referencia {1}", selOperacion.Operacion.Trim(), selReferencia.Referencia.Trim())
            End If
        End If

        If (continuar) Then
            Master.LimpiarMensajes()
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvControles_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            'Sólo se tienen en cuenta las filas con datos
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                Dim imagenCarac As Image = CType(e.Row.FindControl("imgCaracteristicaValor"), Image)
                Dim valorCarac As Label = CType(e.Row.FindControl("lblCaracteristicaValor"), Label)
                Dim hlDescargar As HyperLink = CType(e.Row.FindControl("hlDescargar"), HyperLink)

                If (imagenCarac IsNot Nothing AndAlso valorCarac IsNot Nothing) Then
                    If (valorCarac.Text.Equals("OK") OrElse valorCarac.Text.Equals("NOK")) Then
                        imagenCarac.Visible = True
                        valorCarac.Visible = False
                        If (valorCarac.Text.Equals("OK")) Then
                            imagenCarac.ImageUrl = "..\..\App_Themes\Tema1\Imagenes\caracteristica_bien.png"
                        ElseIf (valorCarac.Text.Equals("NOK")) Then
                            imagenCarac.ImageUrl = "..\..\App_Themes\Tema1\Imagenes\caracteristica_mal.png"
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
            End If
        Catch ex As Exception
        End Try
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
                    If (valorCarac.Text.Equals("OK") OrElse valorCarac.Text.Equals("NOK")) Then
                        imagenCarac.Visible = True
                        valorCarac.Visible = False
                        If (valorCarac.Text.Equals("OK")) Then
                            imagenCarac.ImageUrl = "..\..\App_Themes\Tema1\Imagenes\caracteristica_bien.png"
                        ElseIf (valorCarac.Text.Equals("NOK")) Then
                            imagenCarac.ImageUrl = "..\..\App_Themes\Tema1\Imagenes\caracteristica_mal.png"
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

    ''' <summary>
    ''' Generamos la hoja de registros
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnGenerarInforme_Click(sender As Object, e As EventArgs)
        Dim pageBa As New PageBase
        Dim url As String = String.Empty

        Try
            If (Not String.IsNullOrEmpty(selReferencia.Referencia) AndAlso Not String.IsNullOrEmpty(selOperacion.Operacion) AndAlso Not String.IsNullOrEmpty(txtFechaDesde.Text.Trim()) AndAlso Not String.IsNullOrEmpty(txtFechaHasta.Text.Trim())) Then
                Dim xmlDoc As New System.Xml.XmlDocument
                xmlDoc.Load(Server.MapPath("~") & "\App_Data\InformesCognos.xml")
                Dim nav As System.Xml.XPath.XPathNavigator = xmlDoc.CreateNavigator
                Dim iterator As System.Xml.XPath.XPathNodeIterator = nav.Select("/Informes/Informe[@name='KTROL_HOJA_REGISTROS']")
                If (iterator.MoveNext) Then url = iterator.Current.Value
                Dim IPLocal As Boolean = (Request.ServerVariables("REMOTE_ADDR") = "::1" OrElse (New List(Of String) From {"192", "172", "10"}).Contains(Request.ServerVariables("REMOTE_ADDR").Split(".")(0)))
                'url = String.Format("{0}/" & url, If(IPLocal = True, "http://usotegieta2.batz.es", "https://kuboak.batz.com"))
                url = String.Format("{0}/" & url, "https://cognos.batz.es")
                url = url.Replace(PageBase.COGNOS_CULTURA, Master.Cultura.Name)
                url = url.Replace(PageBase.COGNOS_REFERENCIA, selReferencia.Referencia.Trim())
                url = url.Replace(PageBase.COGNOS_CODOPERACION, selOperacion.Operacion.Trim())
                url = url.Replace(PageBase.COGNOS_FECHADESDE, DateTime.Parse(txtFechaDesde.Text, System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + " 00:00:00")
                url = url.Replace(PageBase.COGNOS_FECHAHASTA, DateTime.Parse(txtFechaHasta.Text, System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + " 23:59:59")
                'url = url.Replace(PageBase.COGNOS_FECHAHASTA, DateTime.ParseExact(txtFechaHasta.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + " 23:59:59")
                url = url.Replace(PageBase.COGNOS_IDCONTROL, txtIdentificador.Text.ToUpper().Trim())
                windowOpen(url)
            Else
                Master.MensajeError = "Se deben seleccionar una referencia, un código de operación y un rango de fechas"
            End If
        Catch ex As ApplicationException
            Master.MensajeAdvertencia = ex.Message
        Catch ex As Exception
           Global_asax.log.Error(ex)
            Master.MensajeError = ex.Message
        End Try
    End Sub

    ''' <summary>
    ''' Generar informe de estadísticas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnGenerarEstadisticas_Click(sender As Object, e As EventArgs)
        Dim pageBa As New PageBase
        Dim url As String = String.Empty

        Try
            If (Not String.IsNullOrEmpty(selReferencia.Referencia) AndAlso Not String.IsNullOrEmpty(selOperacion.Operacion) AndAlso Not String.IsNullOrEmpty(txtFechaDesde.Text.Trim()) AndAlso Not String.IsNullOrEmpty(txtFechaHasta.Text.Trim())) Then
                Dim xmlDoc As New System.Xml.XmlDocument
                xmlDoc.Load(Server.MapPath("~") & "\App_Data\InformesCognos.xml")
                Dim nav As System.Xml.XPath.XPathNavigator = xmlDoc.CreateNavigator
                Dim iterator As System.Xml.XPath.XPathNodeIterator = nav.Select("/Informes/Informe[@name='KTROL_ESTADISTICAS']")
                If (iterator.MoveNext) Then url = iterator.Current.Value
                Dim IPLocal As Boolean = (Request.ServerVariables("REMOTE_ADDR") = "::1" OrElse (New List(Of String) From {"192", "172", "10"}).Contains(Request.ServerVariables("REMOTE_ADDR").Split(".")(0)))
                'url = String.Format("{0}/" & url, If(IPLocal = True, "http://usotegieta2.batz.es", "https://kuboak.batz.com"))
                url = String.Format("{0}/" & url, "https://cognos.batz.es")
                url = url.Replace(PageBase.COGNOS_CULTURA, Master.Cultura.Name)
                url = url.Replace(PageBase.COGNOS_REFERENCIA, selReferencia.Referencia.Trim())
                url = url.Replace(PageBase.COGNOS_CODOPERACION, selOperacion.Operacion.Trim())
                url = url.Replace(PageBase.COGNOS_FECHADESDE, DateTime.Parse(txtFechaDesde.Text, System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + " 00:00:00")
                url = url.Replace(PageBase.COGNOS_FECHAHASTA, DateTime.Parse(txtFechaHasta.Text, System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd") + " 23:59:59")
                url = url.Replace(PageBase.COGNOS_IDCONTROL, txtIdentificador.Text.ToUpper().Trim())
                windowOpen(url)
            Else
                Master.MensajeError = "Se deben seleccionar una referencia, un código de operación y un rango de fechas"
            End If            
        Catch ex As ApplicationException
            Master.MensajeAdvertencia = ex.Message
        Catch ex As Exception
           Global_asax.log.Error(ex)
            Master.MensajeError = ex.Message
        End Try
    End Sub

    ''' <summary>
    ''' Limpiar los campos de filtrado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnLimpiar_Click(sender As Object, e As EventArgs)
        selReferencia.Referencia = String.Empty
        selReferencia.Descripcion = String.Empty        
        selOperacion.Operacion = String.Empty
        selOperacion.Descripcion = String.Empty
        txtIdentificador.Text = String.Empty
        txtFechaDesde.Text = String.Empty
        txtFechaHasta.Text = String.Empty
    End Sub

#End Region

End Class