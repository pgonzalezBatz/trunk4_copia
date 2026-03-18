Imports log4net
Imports System.Text
Imports System.Security.Cryptography
Imports System.Data
Imports System.Web.UI.WebControls

Public Class ConsultaSolicitudesComite
    Inherits PageBase


    Private itzultzaileWeb As New LocalizationLib.Itzultzaile

    Dim oDocumentosBLL As New CEticoLib.BLL.cEtico


#Region "METODOS"


    ''' <summary>
    ''' Inicializa las variables y vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Initialize()
        ClearGridView()
        BindDataView2(0)
    End Sub





    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then   ' itzultzaileWeb.Itzuli(Label3) :
            itzultzaileWeb.Itzuli(Label1) : itzultzaileWeb.Itzuli(Label11) : itzultzaileWeb.Itzuli(Label20)
            itzultzaileWeb.Itzuli(lblAsignacionC) : itzultzaileWeb.Itzuli(CheckBox2) '    : itzultzaileWeb.Itzuli(ddlUni)
            '''''itzultzaileWeb.Itzuli(txtComentario) : itzultzaileWeb.Itzuli(btnCancelar) : itzultzaileWeb.Itzuli(btnGuardarNuevaSolicitud)
            '''''itzultzaileWeb.Itzuli(Label2)
        End If
    End Sub







    ''' <summary>
    ''' Limpia los datos vinculados
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ClearGridView()
        'gvType.DataSource = Nothing
        'gvType.DataBind()
        gvType2.DataSource = Nothing
        gvType2.DataBind()
    End Sub


    Protected Sub BindDataView2(ByVal chequeado As Integer)
        Try
            Dim oDocBLL As New CEticoLib.BLL.cEtico
            Dim listaType2 As List(Of CEticoLib.ELL.CEtico)


            Dim oDocumentosBLL As New CEticoLib.BLL.cEtico
            '     Dim Solicitud As New RRHHLib.ELL.CEtico With {.Id = 22}

            'If chequeado = 1 Then
            '    If plantaAdmin = 99 Then
            '        listaType2 = oDocBLL.CargarListaSolicitudesTodos()
            '    Else
            '        listaType2 = oDocBLL.CargarListaSolicitudesTodosPlanta(plantaAdmin)
            '    End If

            'Else
            listaType2 = oDocBLL.CargarListaSolicitudesComite()
            'End If



            'para desencriptar
            For i = 0 To listaType2.Count - 1

                'lo primero, si esta cerrado cambie texto traducción por motivo cerrado


                listaType2(i).comentariocorto = Left(listaType2(i).comentario, 110)
                If listaType2(i).traduccion <> "" Then

                Else
                    listaType2(i).traduccion = ""
                End If

                If listaType2(i).Accion = "" And listaType2(i).cierre <> "" Then 'cerrado no es QKceY3P/2wc=
                    listaType2(i).traduccion = (listaType2(i).cierre)
                End If



                listaType2(i).traduccioncorto = Left(listaType2(i).traduccion, 30)



                If listaType2(i).Accion <> "" And listaType2(i).Accion <> "" Then
                    listaType2(i).Accion = (listaType2(i).Accion)
                Else
                    listaType2(i).Accion = "0"
                End If

            Next


            If (listaType2.Count > 0) Then

                gvType2.DataSource = listaType2
                gvType2.DataBind()
                gvType2.Caption = String.Empty
            Else
                gvType2.DataSource = Nothing
                gvType2.DataBind()
                gvType2.Caption = "No hay registros"
            End If

        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar la solicitud", ex)
        End Try
    End Sub

    Public Function Encriptar(ByVal Input As String) As String



        Return Input

    End Function


    Public Function Desencriptar(ByVal Input As String) As String


        Return Input

    End Function


#End Region

#Region "HANDLERS"


    ''' <summary>
    ''' RowCommand del gridview
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvType_OnRowCommand(sender As Object, e As GridViewCommandEventArgs)

    End Sub
    Protected Sub gvType2_OnRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim txtModTarjeta As TextBox = DirectCast(gvType2.Controls(0).Controls(0).FindControl("txtModTarjeta"), TextBox)
        Dim prueba As CheckBox = DirectCast(gvType2.Controls(0).Controls(0).FindControl("chkTarjeta"), CheckBox)
    End Sub


    Protected Sub gvType2_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)
        gvType2.EditIndex = -1
        'BindDataView() '2(0)
        flag_Actualizar.Value = "0"
        '  txtEmpresa.ReadOnly = False
        '   txtTra.ReadOnly = False
    End Sub


    Protected Sub gvType2_RowEditing(sender As Object, e As GridViewEditEventArgs)
        gvType2.EditIndex = e.NewEditIndex
        mView.ActiveViewIndex = 0
        'BindDataView()
        flag_Actualizar.Value = "1"


    End Sub

    Protected Sub gvType2_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)
        gvType2.EditIndex = -1
        'BindDataView() '2(0)
        flag_Actualizar.Value = "0"
        ''  txtEmpresa.ReadOnly = False
        '  txtTra.ReadOnly = False
    End Sub


#End Region




    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If Not (Page.IsPostBack) Then
                Initialize()
            End If

            ''Dim oDocBLL As New CEticoLib.BLL.cEtico
            ''Dim lista As List(Of CEticoLib.ELL.Rol)
            ''lista = oDocBLL.CargarRolCualquierPlantaMayor2(Session("Ticket").IdUser) 'tambien 2 sacara
            ''If lista.Count > 0 Then
            ''    CheckBox2.Visible = False
            ''    PageBase.rolUsuario = lista(0).Id
            ''    If lista(0).Id > 2 Then
            ''        plantaAdmin = 99
            ''    End If
            ''Else
            ''    CheckBox2.Visible = False
            ''End If


            'PageBase.rolUsuario = 2
            CheckBox2.Visible = True
            'plantaAdmin = 99

        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar una solicitud", ex)
        End Try
    End Sub




    Protected Sub gvType_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvType.PageIndexChanging
        gvType.PageIndex = e.NewPageIndex
        BindDataView2(HdnPedido.Value)
    End Sub

    Protected Sub gvType2_DataBound(ByVal sender As Object, ByVal e As EventArgs)
        ' Recupera la el PagerRow...
        Dim pagerRow As GridViewRow = gvType2.BottomPagerRow
        If pagerRow IsNot Nothing Then

            gvType2.EditIndex = -1

            ' Recupera los controles DropDownList y label...
            Dim pageList As DropDownList = CType(pagerRow.Cells(0).FindControl("PageDropDownList2"), DropDownList)
            Dim pageLabel As Label = CType(pagerRow.Cells(0).FindControl("CurrentPageLabel"), Label)
            If Not pageList Is Nothing Then

                If pageList.SelectedIndex > -1 Then

                    gvType2.PageIndex = pageList.SelectedIndex
                    'BindDataView()

                End If

                ' Se crean los valores del DropDownList tomando el número total de páginas... 
                Dim i As Integer
                For i = 0 To gvType2.PageCount - 1  'PageCount  
                    ' Se crea un objeto ListItem para representar la �gina...
                    Dim pageNumber As Integer = i + 1
                    Dim item As ListItem = New ListItem(pageNumber.ToString())

                    If pageList.SelectedIndex > 1 Then
                        If i = pageList.SelectedIndex Then ' gvType2.PageIndex Then
                            item.Selected = True
                        End If
                    End If

                    ' Se añade el ListItem a la colección de Items del DropDownList...
                    pageList.Items.Add(item)
                Next i
            End If
            If Not pageLabel Is Nothing Then
                ' Calcula el nº de �gina actual...
                Dim currentPage As Integer = gvType2.PageIndex + 1
                ' Actualiza el Label control con la �gina actual.
                '      pageLabel.Text = ItzultzaileWeb.Itzuli("Página") & " " & currentPage.ToString() & " " & ItzultzaileWeb.Itzuli("de") & " " & gvType2.PageCount.ToString()
                pageList.SelectedIndex = gvType2.PageIndex
            End If
        End If
    End Sub

    Protected Sub gvType2_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvType2.RowCommand
        Try
            Dim mailTo As String

            Dim paramBLL As New SabLib.BLL.ParametrosBLL
            Dim oParams As SabLib.ELL.Parametros = paramBLL.consultar()

            Dim oDocBLL As New CEticoLib.BLL.cEtico
            Dim lista As List(Of CEticoLib.ELL.CEtico)
            Dim lista2 As List(Of CEticoLib.ELL.CEtico)

            If e.CommandName = "Edit" Then
                Dim id As Integer 'es el numero de fila
                id = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex

                Dim valorSeccionado As Object
                Dim valorSeccionado2 As Object
                Dim Claveseleccionada As Object
                'Dim codigoTra As String
                'Dim categoria As String
                'Dim comentario As String
                Dim email As String
                valorSeccionado = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).BindingContainer.NamingContainer, System.Web.UI.WebControls.GridView).Rows '(DirectCast(e.CommandSource, System.Web.UI.Control).BindingContainer.Controls)
                valorSeccionado2 = DirectCast(((DirectCast((valorSeccionado(id)), System.Web.UI.Control).Controls(8)).Controls(1)), System.Web.UI.WebControls.ListControl).SelectedItem.Value ' DirectCast((valorSeccionado(6)), System.Web.UI.Control).Controls(1)
                Claveseleccionada = (DirectCast((valorSeccionado(id)), System.Web.UI.Control).Controls(1)).Controls(0)
                Dim clave As Integer
                clave = CInt(DirectCast(Claveseleccionada, System.Web.UI.WebControls.Label).[Text])

                'para el correo. es el codpersona y saco email, paso como parametro
                codigoTra = DirectCast(((DirectCast((valorSeccionado(id)), System.Web.UI.Control).Controls(2)).Controls(0)), System.Web.UI.WebControls.Label).[Text]
                categoria = DirectCast(((DirectCast((valorSeccionado(id)), System.Web.UI.Control).Controls(4)).Controls(0)), System.Web.UI.WebControls.Label).[Text]
                comentario = DirectCast(((DirectCast((valorSeccionado(id)), System.Web.UI.Control).Controls(5)).Controls(1)), System.Web.UI.WebControls.WebControl).ToolTip
                'descomprimir comentario
                '         comentario = Desencriptar(comentario)



                'hay que update el valor encriptar(valorSeccionado2) de la clave clave como sol000 


                Dim textoEncriptado As String
                textoEncriptado = (valorSeccionado2)

                Dim Solicitud As New CEticoLib.ELL.CEtico With {.Id = clave, .codCategoria = textoEncriptado}

                hfclave.Value = clave


                email = oDocumentosBLL.CargarEmail(CInt(codigoTra))(0).email  'no el de webconfig
                If valorSeccionado2 = "1" Then
                    ModalPopupExtender4.Show() 'motivo de rechazo
                End If
                If valorSeccionado2 = "3" Or CInt(valorSeccionado2) = 0 Then
                    If (oDocumentosBLL.ModificarSolicitud(Solicitud)) Then

                        Master.MensajeInfo = itzultzaileWeb.Itzuli("La solicitud se ha guardado correctamente")
                    Else
                        Master.MensajeError = itzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba la solicitud")

                    End If



                End If
                If valorSeccionado2 = "2" Then
                    ModalPopupExtender5.Show() ' Cerrada > Comentario de cierre (hay quehacerlo en nuevo campo)
                End If
                'resto no se hace nada (presidencia, etc)
                If CInt(valorSeccionado2) = 4 Then

                    'si dejamos lo anterior esto ira en botones de abajo
                    ModalPopupExtender1.Show() ' RRHH > traducción



                End If
                If CInt(valorSeccionado2) = 5 Then

                    ModalPopupExtender2.Show() ' RRHH > traducción



                End If

            End If



        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar la solicitud", ex)
        End Try

    End Sub
    Protected Sub gvType2_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvType2.PageIndexChanging
        gvType2.PageIndex = e.NewPageIndex
        BindDataView2(0)
    End Sub
    ''' <summary>
    ''' Cargar los tipos y Habilitar/Deshabilitar Transmission Mode dependiendo del producto seleccionado
    ''' </summary>
    ''' <param name="idTrabajador"></param>
    ''' <remarks></remarks>
    Private Sub ConfiguracionProduct(ByVal idTrabajador As Integer, Optional ByVal idType As Integer = 0, Optional ByVal idTransmissionMode As Integer = 0)
        Try
            Dim oDocBLL As New CEticoLib.BLL.cEtico
            '    Dim userBLL As New CEticoLib.BLL.UsuariosComponent

            'si es nuevo elemento
            If idTrabajador = 0 Then

                flag_Modificar.Value = "0"

                TxtFechaFin.Text = "30/12/" & Now.Year.ToString
                TxtFechaIni.Text = Now.ToShortDateString

            Else
                flag_Modificar.Value = idTrabajador

            End If


        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar la empresa", ex)
        End Try

    End Sub




    ''' <summary>
    ''' Cargar el detalle 
    ''' </summary>
    Private Sub CargarDetalle(ByVal idDocumento As Integer)
        mView.ActiveViewIndex = 0

        ConfiguracionProduct(idDocumento)


    End Sub

    Private Sub CargarDetalle2(ByVal idDocumento As Integer)
        mView.ActiveViewIndex = 1

        ConfiguracionProduct(idDocumento)
        ' BindDataView()

    End Sub
    Private Sub CargarDetallePedido(ByVal idTrabajador As Integer)


    End Sub




    Protected Sub gvType_DataBound(ByVal sender As Object, ByVal e As EventArgs)
        ' Recupera la el PagerRow...
        Dim pagerRow As GridViewRow = gvType.BottomPagerRow
        If pagerRow IsNot Nothing Then

            gvType.EditIndex = -1

            ' Recupera los controles DropDownList y label...
            Dim pageList As DropDownList = CType(pagerRow.Cells(0).FindControl("PageDropDownList"), DropDownList)
            Dim pageLabel As Label = CType(pagerRow.Cells(0).FindControl("CurrentPageLabel"), Label)
            If Not pageList Is Nothing Then

                If pageList.SelectedIndex > -1 Then

                    gvType.PageIndex = pageList.SelectedIndex
                    BindDataView2(0)

                End If

                ' Se crean los valores del DropDownList tomando el número total de páginas... 
                Dim i As Integer
                For i = 0 To gvType.PageCount - 1  'PageCount  
                    ' Se crea un objeto ListItem para representar la �gina...
                    Dim pageNumber As Integer = i + 1
                    Dim item As ListItem = New ListItem(pageNumber.ToString())

                    If pageList.SelectedIndex > 1 Then
                        If i = pageList.SelectedIndex Then ' gvType2.PageIndex Then
                            item.Selected = True
                        End If
                    End If

                    ' Se añade el ListItem a la colección de Items del DropDownList...
                    pageList.Items.Add(item)
                Next i
            End If
            If Not pageLabel Is Nothing Then
                ' Calcula el nº de �gina actual...
                Dim currentPage As Integer = gvType.PageIndex + 1
                ' Actualiza el Label control con la �gina actual.
                '   pageLabel.Text = ItzultzaileWeb.Itzuli("Página") & " " & currentPage.ToString() & " " & ItzultzaileWeb.Itzuli("de") & " " & gvType.PageCount.ToString()
                pageList.SelectedIndex = gvType.PageIndex
            End If
        End If
    End Sub

    Protected Sub gvType_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)
        gvType.EditIndex = -1
        BindDataView2(0)
        flag_Actualizar.Value = "0"

    End Sub
    Protected Sub gvType_RowEditing(sender As Object, e As GridViewEditEventArgs)
        gvType.EditIndex = e.NewEditIndex
        mView.ActiveViewIndex = 0
        BindDataView2(0)
        flag_Actualizar.Value = "1"

    End Sub
    Protected Sub gvType_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)
        gvType.EditIndex = -1
        BindDataView2(0)
        flag_Actualizar.Value = "0"

    End Sub
    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        '   LimpiarCampos()
        mView.ActiveViewIndex = 0
    End Sub

    Protected Sub btnCancelar2_Click(sender As Object, e As EventArgs) Handles btnCancelar2.Click

        mView.ActiveViewIndex = 2
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Checked = True Then
            BindDataView2(1)
        Else
            BindDataView2(0)
        End If
    End Sub




    Protected Sub btnGrabar1_Click(sender As Object, e As EventArgs)   'rechazo
        Try
            'pasa el valor del texto a alguna variable
            Dim text As TextBox = pnlPopup4.FindControl("TextBox1")
            Dim texto As String = text.Text

            'Dim text2 As TextBox = pnlPopup1.FindControl("TextBox2")
            'Dim texto2 As String = text2.Text


            Dim textoescrito As String = text.Text
            text.Text = ""
            Dim textoEncriptado As String
            textoEncriptado = (textoescrito)

            Dim valorSeccionado2 As String = "1"


            Dim Solicitud As New CEticoLib.ELL.CEtico With {.Id = hfclave.Value, .codCategoria = (valorSeccionado2), .traduccion = textoEncriptado}
            'si hemos dejado lo anterior va aqui
            If (oDocumentosBLL.ModificarSolicitud(Solicitud)) Then
                'habra que mandar mail al individuo

                Dim paramBLL As New SabLib.BLL.ParametrosBLL
                Dim oParams As SabLib.ELL.Parametros = paramBLL.consultar()

                Dim mailTo As String = oDocumentosBLL.CargarEmail(CInt(codigoTra))(0).email

                SabLib.BLL.Utils.EnviarEmail("SolicitudCR@batz.es", mailTo, itzultzaileWeb.Itzuli("Se ha rechazado la solicitud en Canal Ético") & " ", itzultzaileWeb.Itzuli("Categoría") & ": " & itzultzaileWeb.Itzuli(categoria) & "<br><br>" & itzultzaileWeb.Itzuli("Comentario al rechazo") & ":<br>" & textoescrito & "<br><br>", oParams.ServidorEmail)




                If CheckBox2.Checked = True Then
                    BindDataView2(1)
                Else
                    BindDataView2(0)
                End If
                Master.MensajeInfo = itzultzaileWeb.Itzuli("La solicitud se ha guardado correctamente")


            Else
                Master.MensajeError = itzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba la solicitud")

            End If




        Catch ex As Exception

            Throw New SabLib.BatzException("Error ", ex)
        End Try

    End Sub

    Protected Sub btnGrabar2_Click(sender As Object, e As EventArgs) 'direccion
        Try
            Dim mailTo As String
            Dim lista As List(Of CEticoLib.ELL.CEtico)
            Dim lista2 As List(Of CEticoLib.ELL.CEtico)

            Dim text2 As TextBox = pnlPopup1.FindControl("TextBox2")
            Dim texto2 As String = text2.Text
            'mandar mail a jgalan@batz.es
            Dim paramBLL As New SabLib.BLL.ParametrosBLL
            Dim oParams As SabLib.ELL.Parametros = paramBLL.consultar()


            Dim textoescrito As String = TextBox2.Text
            TextBox2.Text = ""
            Dim textoEncriptado As String
            textoEncriptado = (textoescrito)

            Dim valorSeccionado2 As String = "4"

            Dim Solicitud As New CEticoLib.ELL.CEtico With {.Id = hfclave.Value, .codCategoria = (valorSeccionado2), .traduccion = textoEncriptado}
            'si hemos dejado lo anterior va aqui
            If (oDocumentosBLL.ModificarSolicitud(Solicitud)) Then

                Master.MensajeInfo = itzultzaileWeb.Itzuli("La solicitud se ha guardado correctamente")





                mailTo = ConfigurationManager.AppSettings("emailpresidencia")
                Dim bodymensaje As String = ""

                lista = oDocumentosBLL.GetSolicitud(CInt(hfclave.Value))
                lista2 = oDocumentosBLL.GetUsuario(CInt(codigoTra), lista(0).planta)
                Dim usuario As String
                If lista2.Count > 0 Then
                    usuario = lista2(0).Idtra.ToUpper()
                Else
                    usuario = ""
                End If

                'Claveseleccionada = 624 el campo sol000, puedo sacar todo y descomprimir
                'sacar el nombre de trabajador de SAB.USUARIOS es el id y sacare noimbre, apellido1 y 2
                'bodymensaje = itzultzaileWeb.Itzuli("Solicitante") & ":  " & codigoTra & "<br>"
                'bodymensaje = bodymensaje & itzultzaileWeb.Itzuli("Planta") & ":  " & lista(0).plantaDesc & "<br>"
                'bodymensaje = bodymensaje & itzultzaileWeb.Itzuli("Fecha de Solicitud") & ":  " & lista(0).Fecha & "<br>"
                'bodymensaje = bodymensaje & itzultzaileWeb.Itzuli("Categoría") & ":  " & categoria & "<br>"
                'bodymensaje = bodymensaje & itzultzaileWeb.Itzuli("Comentario") & ":  " & comentario & "<br>"
                'bodymensaje = bodymensaje & itzultzaileWeb.Itzuli("Traducción") & ":  " & Desencriptar(lista(0).traduccion) & "<br>"


                bodymensaje = "<table cellpadding='4' cellspacing='0' style='border: 1px solid black;'>"
                bodymensaje = bodymensaje & "<tr>"
                bodymensaje = bodymensaje & "<td style='background-color:#E9ECF1'>"
                bodymensaje = bodymensaje & itzultzaileWeb.Itzuli("Trabajador")
                bodymensaje = bodymensaje & "</td>"
                bodymensaje = bodymensaje & "<td>"
                bodymensaje = bodymensaje & usuario
                bodymensaje = bodymensaje & "</td>"
                bodymensaje = bodymensaje & "</tr>"
                bodymensaje = bodymensaje & "<tr>"
                bodymensaje = bodymensaje & "<td style='background-color:#E9ECF1'>"
                bodymensaje = bodymensaje & itzultzaileWeb.Itzuli("Planta")
                bodymensaje = bodymensaje & "</td>"
                bodymensaje = bodymensaje & "<td>"
                bodymensaje = bodymensaje & lista(0).plantaDesc
                bodymensaje = bodymensaje & "</td>"
                bodymensaje = bodymensaje & "</tr>"
                bodymensaje = bodymensaje & "<tr>"
                bodymensaje = bodymensaje & "<td style='background-color:#E9ECF1'>"
                bodymensaje = bodymensaje & itzultzaileWeb.Itzuli("Fecha")
                bodymensaje = bodymensaje & "</td>"
                bodymensaje = bodymensaje & "<td>"
                bodymensaje = bodymensaje & lista(0).Fecha
                bodymensaje = bodymensaje & "</td>"
                bodymensaje = bodymensaje & "</tr>"

                bodymensaje = bodymensaje & "<tr>"
                bodymensaje = bodymensaje & "<td style='background-color:#E9ECF1'>"
                bodymensaje = bodymensaje & itzultzaileWeb.Itzuli("Categoría")
                bodymensaje = bodymensaje & "</td>"
                bodymensaje = bodymensaje & "<td>"
                bodymensaje = bodymensaje & itzultzaileWeb.Itzuli(categoria)
                bodymensaje = bodymensaje & "</td>"
                bodymensaje = bodymensaje & "</tr>"
                bodymensaje = bodymensaje & "<tr>"
                bodymensaje = bodymensaje & "<td style='background-color:#E9ECF1'>"
                bodymensaje = bodymensaje & itzultzaileWeb.Itzuli("Comentario")
                bodymensaje = bodymensaje & "</td>"
                bodymensaje = bodymensaje & "<td>"
                bodymensaje = bodymensaje & comentario
                bodymensaje = bodymensaje & "</td>"
                bodymensaje = bodymensaje & "</tr>"
                bodymensaje = bodymensaje & "<tr>"
                bodymensaje = bodymensaje & "<td style='background-color:#E9ECF1'>"
                bodymensaje = bodymensaje & itzultzaileWeb.Itzuli("Traduccion")
                bodymensaje = bodymensaje & "</td>"
                bodymensaje = bodymensaje & "<td>"
                bodymensaje = bodymensaje & (lista(0).traduccion)
                bodymensaje = bodymensaje & "</td>"
                bodymensaje = bodymensaje & "</tr>"


                bodymensaje = bodymensaje & "</table>"
                SabLib.BLL.Utils.EnviarEmail("SolicitudCR@batz.es", mailTo, itzultzaileWeb.Itzuli("Se ha asignado a Presidencia una entrada en Canal Ético") & " ", bodymensaje, oParams.ServidorEmail)


            Else
                Master.MensajeError = itzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba la solicitud")

            End If


            'reiniciar el grid
            If CheckBox2.Checked = True Then
                BindDataView2(1)
            Else
                BindDataView2(0)
            End If




        Catch ex As Exception

            Throw New SabLib.BatzException("Error ", ex)
        End Try

    End Sub


    Protected Sub btnGrabarw2_Click(sender As Object, e As EventArgs) 'comite
        Try
            Dim mailTo As String
            Dim lista As List(Of CEticoLib.ELL.CEtico)
            Dim lista2 As List(Of CEticoLib.ELL.CEtico)

            Dim text2 As TextBox = pnlPopupw1.FindControl("TextBox4")
            Dim texto2 As String = text2.Text


            Dim textoescrito As String = text2.Text
            TextBox2.Text = ""
            Dim textoEncriptado As String
            textoEncriptado = (textoescrito)

            Dim valorSeccionado2 As String = "5"

            Dim Solicitud As New CEticoLib.ELL.CEtico With {.Id = hfclave.Value, .codCategoria = (valorSeccionado2), .traduccion = textoEncriptado}
            'si hemos dejado lo anterior va aqui
            If (oDocumentosBLL.ModificarSolicitud(Solicitud)) Then

                Master.MensajeInfo = itzultzaileWeb.Itzuli("La solicitud se ha guardado correctamente")


                'mandar mail a jgalan@batz.es
                Dim paramBLL As New SabLib.BLL.ParametrosBLL
                Dim oParams As SabLib.ELL.Parametros = paramBLL.consultar()





                mailTo = ConfigurationManager.AppSettings("emailcomite")
                Dim bodymensaje As String = ""



                lista = oDocumentosBLL.GetSolicitud(CInt(hfclave.Value))
                lista2 = oDocumentosBLL.GetUsuario(CInt(codigoTra), lista(0).planta)
                Dim usuario As String
                If lista2.Count > 0 Then
                    usuario = lista2(0).Idtra.ToUpper()
                Else
                    usuario = ""
                End If


                bodymensaje = "<table cellpadding='4' cellspacing='0' style='border: 1px solid black;'>"
                bodymensaje = bodymensaje & "<tr>"
                bodymensaje = bodymensaje & "<td style='background-color:#E9ECF1'>"
                bodymensaje = bodymensaje & itzultzaileWeb.Itzuli("Trabajador")
                bodymensaje = bodymensaje & "</td>"
                bodymensaje = bodymensaje & "<td>"
                bodymensaje = bodymensaje & usuario
                bodymensaje = bodymensaje & "</td>"
                bodymensaje = bodymensaje & "</tr>"
                bodymensaje = bodymensaje & "<tr>"
                bodymensaje = bodymensaje & "<td style='background-color:#E9ECF1'>"
                bodymensaje = bodymensaje & itzultzaileWeb.Itzuli("Planta")
                bodymensaje = bodymensaje & "</td>"
                bodymensaje = bodymensaje & "<td>"
                bodymensaje = bodymensaje & lista(0).plantaDesc
                bodymensaje = bodymensaje & "</td>"
                bodymensaje = bodymensaje & "</tr>"
                bodymensaje = bodymensaje & "<tr>"
                bodymensaje = bodymensaje & "<td style='background-color:#E9ECF1'>"
                bodymensaje = bodymensaje & itzultzaileWeb.Itzuli("Fecha")
                bodymensaje = bodymensaje & "</td>"
                bodymensaje = bodymensaje & "<td>"
                bodymensaje = bodymensaje & lista(0).Fecha
                bodymensaje = bodymensaje & "</td>"
                bodymensaje = bodymensaje & "</tr>"

                bodymensaje = bodymensaje & "<tr>"
                bodymensaje = bodymensaje & "<td style='background-color:#E9ECF1'>"
                bodymensaje = bodymensaje & itzultzaileWeb.Itzuli("Categoría")
                bodymensaje = bodymensaje & "</td>"
                bodymensaje = bodymensaje & "<td>"
                bodymensaje = bodymensaje & itzultzaileWeb.Itzuli(categoria)
                bodymensaje = bodymensaje & "</td>"
                bodymensaje = bodymensaje & "</tr>"
                bodymensaje = bodymensaje & "<tr>"
                bodymensaje = bodymensaje & "<td style='background-color:#E9ECF1'>"
                bodymensaje = bodymensaje & itzultzaileWeb.Itzuli("Comentario")
                bodymensaje = bodymensaje & "</td>"
                bodymensaje = bodymensaje & "<td>"
                bodymensaje = bodymensaje & comentario
                bodymensaje = bodymensaje & "</td>"
                bodymensaje = bodymensaje & "</tr>"
                bodymensaje = bodymensaje & "<tr>"
                bodymensaje = bodymensaje & "<td style='background-color:#E9ECF1'>"
                bodymensaje = bodymensaje & itzultzaileWeb.Itzuli("Traduccion")
                bodymensaje = bodymensaje & "</td>"
                bodymensaje = bodymensaje & "<td>"
                bodymensaje = bodymensaje & (lista(0).traduccion)
                bodymensaje = bodymensaje & "</td>"
                bodymensaje = bodymensaje & "</tr>"


                bodymensaje = bodymensaje & "</table>"
                SabLib.BLL.Utils.EnviarEmail("SolicitudCR@batz.es", mailTo, itzultzaileWeb.Itzuli("Se ha asignado a Comité una entrada en Canal Ético") & " ", bodymensaje, oParams.ServidorEmail)







            Else
                Master.MensajeError = itzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba la solicitud")

            End If


            'reiniciar el grid
            If CheckBox2.Checked = True Then
                BindDataView2(1)
            Else
                BindDataView2(0)
            End If




        Catch ex As Exception

            Throw New SabLib.BatzException("Error ", ex)
        End Try

    End Sub



    Protected Sub btnGrabar3_Click(sender As Object, e As EventArgs)  'Cerrada
        Try
            'pasa el valor del texto a alguna variable
            'Dim text As TextBox = pnlPopup4.FindControl("TextBox1")
            'Dim texto As String = text.Text

            Dim text2 As TextBox = pnlPopup1.FindControl("TextBox3")
            Dim texto2 As String = text2.Text


            Dim textoescrito As String = text2.Text
            text2.Text = ""
            Dim textoEncriptado As String
            textoEncriptado = (textoescrito)

            Dim valorSeccionado2 As String = "2"


            Dim Solicitud As New CEticoLib.ELL.CEtico With {.Id = hfclave.Value, .codCategoria = (valorSeccionado2), .traduccion = textoEncriptado} 'traduccion es motivo cerrado
            'si hemos dejado lo anterior va aqui
            If (oDocumentosBLL.ModificarSolicitudcerrada(Solicitud)) Then

                Master.MensajeInfo = itzultzaileWeb.Itzuli("La solicitud se ha cerrado correctamente")

                'mandar mail 
                Dim paramBLL As New SabLib.BLL.ParametrosBLL
                Dim oParams As SabLib.ELL.Parametros = paramBLL.consultar()

                '   Dim mailTo As String = ConfigurationManager.AppSettings("emailAdministrador")

                Dim mailTo As String = oDocumentosBLL.CargarEmail(CInt(codigoTra))(0).email 'no el de webconfig ConfigurationManager.AppSettings("emailAdministrador")


                SabLib.BLL.Utils.EnviarEmail("SolicitudCR@batz.es", mailTo, itzultzaileWeb.Itzuli("Se ha cerrado la solicitud") & " ", itzultzaileWeb.Itzuli("Categoría") & ": " & itzultzaileWeb.Itzuli(categoria) & "<br><br>" & itzultzaileWeb.Itzuli("Comentario de cierre") & ":<br>" & textoescrito & "<br><br>", oParams.ServidorEmail)


                Master.MensajeInfo = itzultzaileWeb.Itzuli("La solicitud se ha cerrado correctamente")

            Else
                Master.MensajeError = itzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba la solicitud")

            End If



            'reiniciar el grid
            If CheckBox2.Checked = True Then
                BindDataView2(1)
            Else
                BindDataView2(0)
            End If


        Catch ex As Exception

            Throw New SabLib.BatzException("Error ", ex)
        End Try

    End Sub
    Protected Sub btnCancelar4_Click(sender As Object, e As EventArgs) ' Handles btnCancelar_Click.Click
        Try


            Dim text As TextBox = pnlPopup4.FindControl("TextBox1")
            Dim texto As String = text.Text

            'reiniciar el grid
            If CheckBox2.Checked = True Then
                BindDataView2(1)
            Else
                BindDataView2(0)
            End If
        Catch ex As Exception

            Throw New SabLib.BatzException("Error ", ex)
        End Try

    End Sub
End Class