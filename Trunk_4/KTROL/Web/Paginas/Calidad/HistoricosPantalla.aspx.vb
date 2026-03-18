Imports System
Imports ClosedXML.Excel
Imports System.IO

Public Class HistoricosPantalla
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
                selReferencia.Referencia = String.Empty
                selReferencia.Descripcion = String.Empty
                selOperacion.Operacion = String.Empty
                selOperacion.Descripcion = String.Empty
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
        Dim listaHistoricos As List(Of ELL.Historicos)
        Dim UsuariosComponent As New Sablib.BLL.UsuariosComponent
        Dim gtkUsuario As New Sablib.ELL.Usuario
        Dim continuar As Boolean = True

        Dim listaVerificadores As New List(Of Integer)
        If Not (String.IsNullOrEmpty(hfIdUsuario1.Value)) Then
            gtkUsuario = UsuariosComponent.GetUsuario(New Sablib.ELL.Usuario With {.Id = hfIdUsuario1.Value, .IdPlanta = 1}, False)
            listaVerificadores.Add(gtkUsuario.CodPersona)
        End If
        If Not (String.IsNullOrEmpty(hfIdUsuario2.Value)) Then
            gtkUsuario = UsuariosComponent.GetUsuario(New Sablib.ELL.Usuario With {.Id = hfIdUsuario2.Value, .IdPlanta = 1}, False)
            listaVerificadores.Add(gtkUsuario.CodPersona)
        End If
        If Not (String.IsNullOrEmpty(hfIdUsuario3.Value)) Then
            gtkUsuario = UsuariosComponent.GetUsuario(New Sablib.ELL.Usuario With {.Id = hfIdUsuario3.Value, .IdPlanta = 1}, False)
            listaVerificadores.Add(gtkUsuario.CodPersona)
        End If

        listaHistoricos = oHistoricos.ObtenerControlesPantalla(selOperacion.Operacion, txtCaracteristica.Text.Trim.ToUpper, txtFechaDesde.Text.Trim, txtFechaHasta.Text.Trim, listaVerificadores, txtIdentificador.Text.Trim)
        gvControles.DataSource = listaHistoricos
        gvControles.DataBind()

        For Each col In gvControles.Columns
            If (col.HeaderText = "Cod.Op") Then
                If Not (String.IsNullOrEmpty(selOperacion.Operacion)) Then
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
    End Sub

    ''' <summary>
    ''' RowDataBound del grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvControles_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            'Sólo se tienen en cuenta las filas con datos
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                If Not (String.IsNullOrEmpty(txtCaracteristica.Text)) Then
                    Dim imagenCarac As Image = CType(e.Row.FindControl("imgCaracteristicaValor"), Image)
                    Dim valorCarac As Label = CType(e.Row.FindControl("lblCaracteristicaValor"), Label)
                    Dim okNok As Label = CType(e.Row.FindControl("lblOkNok"), Label)
                    Dim hlDescargar As HyperLink = CType(e.Row.FindControl("hlDescargar"), HyperLink)

                    If (imagenCarac IsNot Nothing AndAlso valorCarac IsNot Nothing And okNok IsNot Nothing) Then
                        Select Case okNok.Text
                            Case "0"
                                If (valorCarac.Text.Equals("0")) Then
                                    imagenCarac.Visible = True
                                    valorCarac.Visible = False
                                    imagenCarac.ImageUrl = "..\..\App_Themes\Tema1\Imagenes\caracteristica_mal.png"
                                Else
                                    imagenCarac.Visible = False
                                    valorCarac.Visible = True
                                    valorCarac.Style.Add("color", "red")
                                    valorCarac.Style.Add("font-size", "14px")
                                    valorCarac.Style.Add("font-weight", "bold")
                                End If
                            Case "1"
                                If (valorCarac.Text.Equals("0")) Then
                                    imagenCarac.Visible = True
                                    valorCarac.Visible = False
                                    imagenCarac.ImageUrl = "..\..\App_Themes\Tema1\Imagenes\caracteristica_bien.png"
                                Else
                                    imagenCarac.Visible = False
                                    valorCarac.Visible = True
                                    valorCarac.Style.Add("color", "#62CE00")
                                    valorCarac.Style.Add("font-size", "14px")
                                    valorCarac.Style.Add("font-weight", "bold")
                                End If
                            Case "2"
                                imagenCarac.Visible = False
                                valorCarac.Visible = False
                            Case Else
                                imagenCarac.Visible = False
                                valorCarac.Visible = False
                        End Select
                    End If
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
            If (errorControl.IdControlValidacion = Integer.MinValue) Then
                dvErrores.Rows(5).Visible = False
            Else
                dvErrores.Rows(5).Visible = True
            End If
            If (errorControl.Validado) Then
                dvErrores.Rows(3).Visible = True
                dvErrores.Rows(4).Visible = True
            Else
                dvErrores.Rows(3).Visible = False
                dvErrores.Rows(4).Visible = False
            End If
        Else
            dvErrores.Visible = False
            dvErrores.DataSource = Nothing
            dvErrores.DataBind()
        End If

        mpeValores.Show()

        upPopUp.Update()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnOk_Click(sender As Object, e As EventArgs)
        mpeValores.Hide()
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
                Dim okNok As Label = CType(e.Row.FindControl("lblOkNok"), Label)
                Dim hlDescargar As HyperLink = CType(e.Row.FindControl("hlDescargar"), HyperLink)

                If (imagenCarac IsNot Nothing AndAlso valorCarac IsNot Nothing) Then
                    If ((okNok.Text.Equals("1")) AndAlso (valorCarac.Text.Equals("0"))) Then
                        imagenCarac.Visible = True
                        valorCarac.Visible = False
                        imagenCarac.ImageUrl = "..\..\App_Themes\Tema1\Imagenes\caracteristica_bien.png"
                    ElseIf ((okNok.Text.Equals("0")) AndAlso (valorCarac.Text.Equals("0"))) Then
                        imagenCarac.Visible = True
                        valorCarac.Visible = False
                        imagenCarac.ImageUrl = "..\..\App_Themes\Tema1\Imagenes\caracteristica_mal.png"
                    ElseIf ((okNok.Text.Equals("1")) AndAlso (Not valorCarac.Text.Equals("0"))) Then
                        imagenCarac.Visible = False
                        valorCarac.Visible = True
                        valorCarac.Style.Add("color", "#62CE00")
                        valorCarac.Style.Add("font-size", "14px")
                        valorCarac.Style.Add("font-weight", "bold")
                    ElseIf ((okNok.Text.Equals("0")) AndAlso (Not valorCarac.Text.Equals("0"))) Then
                        imagenCarac.Visible = False
                        valorCarac.Visible = True
                        valorCarac.Style.Add("color", "red")
                        valorCarac.Style.Add("font-size", "14px")
                        valorCarac.Style.Add("font-weight", "bold")
                    Else
                        imagenCarac.Visible = False
                        valorCarac.Visible = False
                    End If
                End If

                Dim imagenVerFicheros As Image = CType(e.Row.FindControl("imgDescargarDetalle"), Image)
                Dim idRegistro As Label = CType(e.Row.FindControl("lblIdRegistro"), Label)
                Dim idControl As Label = CType(e.Row.FindControl("lblIdControl"), Label)
                'Miramos en la ruta de ktrol de Atxerre si existe algún fichero vinculado a cada registro del control
                If (Directory.Exists(ConfigurationManager.AppSettings("rutaAdjuntos") & idControl.Text & "\" & idRegistro.Text)) Then
                    imagenVerFicheros.Visible = True
                    hlDescargar.NavigateUrl = String.Format(hlDescargar.NavigateUrl, idControl.Text, idRegistro.Text)
                Else : imagenVerFicheros.Visible = False
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    ''' <summary>
    ''' Databound del DetailsView
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub dvErrores_DataBound(sender As Object, e As EventArgs) Handles dvErrores.DataBound
        If (dvErrores.DataItem IsNot Nothing) Then
            Dim errorControl As ELL.ControlesErrores = CType(dvErrores.DataItem, ELL.ControlesErrores)

            Dim lblControlValidador As Label = CType(dvErrores.FindControl("lblControlValidador"), Label)

            ' Llenamos el dropdownlist de roles
            If (lblControlValidador.Text <> Integer.MinValue) Then
                Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
                Dim gtkUsuario As New SabLib.ELL.Usuario

                lblControlValidador.Text = String.Concat("ID Control: ", lblControlValidador.Text, ". Validado por: ", UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = oControles.ObtenerControl(lblControlValidador.Text).IdUsuario}).NombreCompleto)
            Else
                lblControlValidador.Text = String.Empty
            End If
        End If
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
        txtCaracteristica.Text = String.Empty
        txtFechaDesde.Text = String.Empty
        txtFechaHasta.Text = String.Empty
        txtVerificador1.Text = String.Empty
        txtVerificador2.Text = String.Empty
        txtVerificador3.Text = String.Empty
        hfIdUsuario1.Value = String.Empty
        hfIdUsuario2.Value = String.Empty
        hfIdUsuario3.Value = String.Empty
    End Sub



#End Region

End Class