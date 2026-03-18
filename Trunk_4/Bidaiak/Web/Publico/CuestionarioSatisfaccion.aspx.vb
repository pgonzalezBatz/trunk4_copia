Public Class CuestionarioSatisfaccion
    Inherits PageBase

#Region "Carga de la pagina"

    ''' <summary>
    ''' Carga de la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.NotShowHeader()
                Master.SetTitle = "Cuestionario de satisfaccion"
                mv.ActiveViewIndex = 0
                Dim viajesBLL As New BLL.ViajesBLL
                Dim idViaje As Integer = CInt(Request.QueryString("idViaje"))
                log.Info("Acceso al cuestionario del viaje " & idViaje)
                Dim oViaje As ELL.Viaje = viajesBLL.loadInfo(idViaje, bSoloCabecera:=True)
                If (oViaje.IdUserSolicitador <> Master.Ticket.IdUser) Then
                    log.Warn("El usuario - " & Master.Ticket.NombreCompleto & "(" & Master.Ticket.IdUser & ") ha intentado acceder al cuestionario del viaje " & idViaje & " cuando el planificador del mismo es " & oViaje.IdUserSolicitador)
                    Response.Redirect("~/PermisoDenegado.aspx?mensa=3", False)
                Else
                    labelInfo.Text = itzultzaileWeb.Itzuli("Por favor, valora la gestion del viaje [NUM_VIAJE] por parte de la agencia contestando a las siguientes preguntas").ToString.Replace("[NUM_VIAJE]", "V" & idViaje)
                    labelQuestion1.Text = itzultzaileWeb.Itzuli("1. ¿Que te ha parecido la [gestion] global realizada por parte de la agencia de viajes?").ToString.Replace("[", "<b><u>").Replace("]", "</u></b>")
                    labelQuestion2.Text = itzultzaileWeb.Itzuli("2. ¿Como ha sido la [atencion] recibida por parte del personal de la agencia de viajes?").ToString.Replace("[", "<b><u>").Replace("]", "</u></b>")
                    'Comprobamos que no se haya rellenado ya el cuestionario
                    Dim bidaiakBLL As New BLL.BidaiakBLL
                    If (bidaiakBLL.ExistCuestionario(idViaje)) Then
                        mv.ActiveViewIndex = 1
                        divResul.Attributes.Add("class", "alert alert-info")
                        log.Warn("El cuestionario ya ha sido rellenado anteriormente")
                        labelMensaje.Text = itzultzaileWeb.Itzuli("El cuestionario ya ha sido rellenado anteriormente")
                    Else
                        loadAnswers()
                    End If
                End If
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Traducciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CuestionarioSafisfaccion_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelComenQuestion1) : itzultzaileWeb.Itzuli(labelQuestion3) : itzultzaileWeb.Itzuli(btnGuardar)
        End If
    End Sub

#End Region

#Region "Cuestionario"

    ''' <summary>
    ''' Carga la respuestas
    ''' </summary>
    Private Sub loadAnswers()
        Dim lAnswers As New List(Of ListItem)
        lAnswers.Add(New ListItem(itzultzaileWeb.Itzuli("Nada Satisfactoria"), 1))
        lAnswers.Add(New ListItem(itzultzaileWeb.Itzuli("Poco Satisfactoria"), 2))
        lAnswers.Add(New ListItem(itzultzaileWeb.Itzuli("Normal"), 3))
        lAnswers.Add(New ListItem(itzultzaileWeb.Itzuli("Satisfactoria"), 4))
        lAnswers.Add(New ListItem(itzultzaileWeb.Itzuli("Muy satisfactoria"), 5))
        lAnswers.Add(New ListItem(itzultzaileWeb.Itzuli("No sabe/No contesta"), 6))
        rptAnswer1.DataSource = lAnswers : rptAnswer1.DataBind()
        rptAnswer2.DataSource = lAnswers : rptAnswer2.DataBind()
        rptAnswer3.DataSource = lAnswers : rptAnswer3.DataBind()
    End Sub

    ''' <summary>
    ''' Se enlazan las respuestas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub rptAnswer_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptAnswer1.ItemDataBound, rptAnswer2.ItemDataBound, rptAnswer3.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim item As ListItem = e.Item.DataItem
            Dim radio As RadioButton = CType(e.Item.FindControl("rbAnswer"), RadioButton)
            radio.Text = item.Text
            radio.Attributes.Add("value", item.Value)
        End If
    End Sub

    ''' <summary>
    ''' Se guarda el cuestionario
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            Dim cuest As New With {.IdViaje = CInt(Request.QueryString("IdViaje")), .Answer1 = 0, .TextAnswer1 = "", .Answer2 = 0, .Answer3 = 0}
            Dim radio As RadioButton
            For Each rptItem As RepeaterItem In rptAnswer1.Items
                radio = CType(rptItem.FindControl("rbAnswer"), RadioButton)
                If (radio.Checked) Then
                    cuest.Answer1 = CInt(radio.Attributes.Item("value"))
                    cuest.TextAnswer1 = txtComenQuestion1.Text
                    Exit For
                End If
            Next
            For Each rptItem As RepeaterItem In rptAnswer2.Items
                radio = CType(rptItem.FindControl("rbAnswer"), RadioButton)
                If (radio.Checked) Then
                    cuest.Answer2 = CInt(radio.Attributes.Item("value"))
                    Exit For
                End If
            Next
            For Each rptItem As RepeaterItem In rptAnswer3.Items
                radio = CType(rptItem.FindControl("rbAnswer"), RadioButton)
                If (radio.Checked) Then
                    cuest.Answer3 = CInt(radio.Attributes.Item("value"))
                    Exit For
                End If
            Next
            If (cuest.Answer1 = 0 OrElse cuest.Answer2 = 0 OrElse cuest.Answer3 = 0) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Tiene que rellenar todos los datos")
            ElseIf (Cuest.TextAnswer1.Length > 500) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Se han excedido los [SIZE] caracteres que puede tener la respuesta").ToString.Replace("[SIZE]", "500")
            Else
                Dim bidaiakBLL As New BLL.BidaiakBLL
                bidaiakBLL.SaveCuestionario(cuest)
                log.Info("Cuestionario " & cuest.IdViaje & " guardado")
                mv.ActiveViewIndex = 1
                divResul.Attributes.Add("class", "alert alert-success")
                labelMensaje.Text = itzultzaileWeb.Itzuli("Los datos del cuestionario han sido guardados con exito. Muchas gracias por su colaboracion")
            End If
        Catch ex As Exception
            mv.ActiveViewIndex = 1
            divResul.Attributes.Add("class", "alert alert-danger")
            labelMensaje.Text = itzultzaileWeb.Itzuli("No se han podido guardar los datos del cuestionario. Por favor, cree un helpdesk para intentar solucionarlo")
        End Try
    End Sub

#End Region

End Class