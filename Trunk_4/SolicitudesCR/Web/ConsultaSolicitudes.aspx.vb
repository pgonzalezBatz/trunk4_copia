Imports log4net
Imports System.Text
Imports System.Security.Cryptography
Imports System.Data
Imports System.Web.UI.WebControls

Public Class ConsultaSolicitudes
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
            itzultzaileWeb.Itzuli(Label11) : itzultzaileWeb.Itzuli(Label20)  '  itzultzaileWeb.Itzuli(Label1) :
            itzultzaileWeb.Itzuli(lblAsignacionC) : itzultzaileWeb.Itzuli(CheckBox2) '    : itzultzaileWeb.Itzuli(ddlUni)
            '''''itzultzaileWeb.Itzuli(txtComentario) : itzultzaileWeb.Itzuli(btnCancelar) : itzultzaileWeb.Itzuli(btnGuardarNuevaSolicitud)
            'Dim texto As Object = itzultzaileWeb.Itzuli(Label18)
            'texto = texto & "hola"
            'Label18.Text = texto
            itzultzaileWeb.Itzuli(Label18) : itzultzaileWeb.Itzuli(Label19)
        End If
        If varAceptado = "2" Then
            Label18.Visible = False
            Label17.Visible = True
        Else
            Label17.Visible = False
            Label18.Visible = True
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

            If CheckBox2.Checked = True Then
                If System.Configuration.ConfigurationManager.AppSettings("presidencia").ToString.Contains(PageBase.rolUsuario.ToString) Or System.Configuration.ConfigurationManager.AppSettings("RRHH").ToString.Contains(PageBase.rolUsuario.ToString) Then
                    listaType2 = oDocBLL.CargarListaSolicitudesTodos()
                Else
                    listaType2 = oDocBLL.CargarListaSolicitudesTodosPlanta(DirectCast(Session("Ticket"), SabLib.ELL.Ticket).IdTrabajador)
                End If

            Else
                listaType2 = oDocBLL.CargarListaSolicitudes(PageBase.rolUsuario, DirectCast(Session("Ticket"), SabLib.ELL.Ticket).IdTrabajador)
            End If



            'para desencriptar
            For i = 0 To listaType2.Count - 1

                'lo primero, si esta cerrado cambie texto traducción por motivo cerrado




                'listaType2(i).Idtra = Desencriptar(listaType2(i).Idtra)
                'listaType2(i).codCategoria = Desencriptar(listaType2(i).codCategoria)
                'listaType2(i).comentario = Desencriptar(listaType2(i).comentario)
                listaType2(i).comentariocorto = Left(listaType2(i).comentario, 110)
                If listaType2(i).comentariocorto <> listaType2(i).comentario Then
                    listaType2(i).comentariocorto = listaType2(i).comentariocorto & " ..."
                End If
                If listaType2(i).traduccion <> "" Then
                    'listaType2(i).traduccion = Desencriptar(listaType2(i).traduccion)
                Else
                    listaType2(i).traduccion = ""
                End If

                'If listaType2(i).Accion = "" And listaType2(i).cierre <> "" Then 'cerrado no es QKceY3P/2wc=
      '''''''''''''''''''''''''''''''''          listaType2(i).traduccion = (listaType2(i).cierre)
                'End If



                listaType2(i).traduccioncorto = Left(listaType2(i).traduccion, 30)
                listaType2(i).Fecha = listaType2(i).Fecha.ToShortDateString
                listaType2(i).FechaMod =listaType2(i).FechaMod.ToShortDateString

                If listaType2(i).codCategoria <> "" Then
                    listaType2(i).codCategoria = "true"
                Else
                    listaType2(i).codCategoria = "false"
                End If



                If System.Configuration.ConfigurationManager.AppSettings("presidencia").ToString.Contains(PageBase.rolUsuario.ToString) Or System.Configuration.ConfigurationManager.AppSettings("RRHH").ToString.Contains(PageBase.rolUsuario.ToString) Then
                    'email es si es visible para no rol
                    listaType2(i).sel1 = "true"
                    listaType2(i).email = "false"

                Else
                    listaType2(i).sel1 = "false"
                    listaType2(i).email = "true"

                    listaType2(i).campo2 = " " 'responsable

                    If listaType2(i).Accion = "1" Then
                        listaType2(i).campo2 = "RRHH" 'responsable
                    End If
                    If listaType2(i).Accion = "2" Then
                        listaType2(i).campo2 = "Presidencia" 'responsable
                    End If


                    listaType2(i).campo1 = "" 'accion

                    If listaType2(i).cierre = "0" Then
                        listaType2(i).campo1 = "En tramite" 'accion
                    End If
                    If listaType2(i).cierre = "1" Then
                        listaType2(i).campo1 = "Aceptada" 'accion
                    End If
                    If listaType2(i).cierre = "2" Then
                        listaType2(i).campo1 = "Denegada" 'accion
                    End If



                    'poner aqui lo de el tercer check
                    listaType2(i).campo3 = ""
                    If listaType2(i).bajas = "0" Then
                        listaType2(i).campo3 = "Bajas Societarias y Devoluciones" 'accion
                    End If
                    If listaType2(i).bajas = "1" Then
                        listaType2(i).campo3 = "Reduc. de jornada y Horarios espec." 'accion
                    End If
                    If listaType2(i).bajas = "2" Then
                        listaType2(i).campo3 = "Excedencias" 'accion
                    End If
                    If listaType2(i).bajas = "3" Then
                        listaType2(i).campo3 = "Varios" 'accion
                    End If
                    If listaType2(i).bajas = "4" Then
                        listaType2(i).campo3 = "No procede"
                    End If
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
            Throw New Sablib.BatzException("Error al mostrar la solicitud", ex)
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
    Public Sub ibHistorico_Click(sender As Object, e As EventArgs)
        Try

            Dim fila As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            Dim idDocumento As String = gvType2.DataKeys(fila.RowIndex)("Id").ToString()

            Dim oDocumentosBLL As New CEticoLib.BLL.cEtico
            Dim listaEmpDoc As List(Of CEticoLib.ELL.CEtico)

            'atencion 'habra que leer el hist


            Dim tipo As New CEticoLib.ELL.CEtico With {.Id = CInt(idDocumento), .cierre = 9999}


            listaEmpDoc = oDocumentosBLL.LeerEmpDocHist(idDocumento)

            If listaEmpDoc.Count > 0 Then
                txtTrabajad.Text = listaEmpDoc(0).plantaDesc

                If listaEmpDoc(0).comentario.Length > 49 Then
                    txtDescri.Text = Left(listaEmpDoc(0).comentario, 50) & " ..."
                Else
                    txtDescri.Text = Left(listaEmpDoc(0).comentario, 50)
                End If

                For i = 0 To listaEmpDoc.Count - 1



                    'leo todos los datos del reg
                    'Dim oDocumentosBLL As New CEticoLib.BLL.cEtico
                    '     Dim listaEmpDoc2 As List(Of CEticoLib.ELL.CEtico)

                    'atencion 'habra que leer el hist

                    '''''''listaEmpDoc2 = oDocumentosBLL.LeerEmpDoc(listaEmpDoc(i).Id)

                    '''''''listaEmpDoc(i).Fecha = listaEmpDoc2(i).Fecha
                    '''''''listaEmpDoc(i).codCategoria = listaEmpDoc2(i).codCategoria
                    '''''''listaEmpDoc(i).Idtra = listaEmpDoc2(i).Idtra
                    '''''''listaEmpDoc(i).comentario = listaEmpDoc2(i).comentario

                    'fin hist




                    'lo primero, si esta cerrado cambie texto traducción por motivo cerrado




                    'listaType2(i).Idtra = Desencriptar(listaType2(i).Idtra)
                    'listaType2(i).codCategoria = Desencriptar(listaType2(i).codCategoria)
                    'listaType2(i).comentario = Desencriptar(listaType2(i).comentario)
                    listaEmpDoc(i).comentariocorto = Left(listaEmpDoc(i).comentario, 110) & " ..."
                    If listaEmpDoc(i).traduccion <> "" Then
                        'listaType2(i).traduccion = Desencriptar(listaType2(i).traduccion)
                    Else
                        listaEmpDoc(i).traduccion = ""
                    End If
                    'If i > 0 Then
                    '    listaEmpDoc(i).comentariocorto = ""
                    '    listaEmpDoc(i).traduccion = ""
                    'End If

                    'If listaType2(i).Accion = "" And listaType2(i).cierre <> "" Then 'cerrado no es QKceY3P/2wc=
                    '''''''''listaEmpDoc(i).traduccion = (listaEmpDoc(i).cierre)
                    'End If



                    listaEmpDoc(i).traduccioncorto = Left(listaEmpDoc(i).traduccion, 30)



                    If listaEmpDoc(i).codCategoria <> "" Then
                        listaEmpDoc(i).codCategoria = "true"
                    Else
                        listaEmpDoc(i).codCategoria = "false"
                    End If

                Next

                mView.ActiveViewIndex = 4

                gvTypeHist.DataSource = listaEmpDoc
                gvTypeHist.DataBind()
                gvTypeHist.Caption = String.Empty

            Else
                Master.MensajeInfo = itzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
            End If
        Catch ex As Exception

            Master.MensajeError = itzultzaileWeb.Itzuli("Error en el histórico de documentos")
        End Try

    End Sub

    Public Sub ibVer_Click(sender As Object, e As EventArgs)
        Try


            Dim fila As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            Dim idDocumento As String = gvType2.DataKeys(fila.RowIndex)("Id").ToString()
            Dim oDocBLL As New CEticoLib.BLL.cEtico



            Dim oDocumentosBLL As New CEticoLib.BLL.cEtico
            Dim listaEmpDoc As List(Of CEticoLib.ELL.CEtico)

            listaEmpDoc = oDocBLL.LeerEmpDoc(idDocumento)


            If listaEmpDoc.Count > 0 Then


                If listaEmpDoc(0).codCategoria <> "" Then
                    'idDocumento = Server.MapPath("~/") & "Ficheros/Documentos/" & listaEmpDoc(0).ubicacion
                    idDocumento = System.Configuration.ConfigurationManager.AppSettings("PathFicherosBajar").ToString() & listaEmpDoc(0).codCategoria

                    'ScriptManager.RegisterClientScriptBlock(, , "OpenWindow", pClientScript, True)
                    'ScriptManager.RegisterStartupScript(Me, GetType(Page), "anioactual", Script, True)
                    'ScriptManager.RegisterStartupScript(Page, tipo, Clave, "Sys.WebForms.PageRequestManager.getInstance().add_endRequest(" & FuncionJS & ");", True)

                    'Dim URL As String = "ReferenciaVentaBrain.aspx?IdRef=" & CommandArgument
                    Dim script As String = "<script type=""text/javascript"">window.open('" & idDocumento & "');</script>"
                    ClientScript.RegisterStartupScript(Me.GetType, "openWindow", script)
                    'Process.Start(idDocumento)
                    BindDataView2(0)

                Else
                    Master.MensajeInfo = itzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                End If

            Else
                Master.MensajeInfo = itzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
            End If
        Catch ex As Exception

            Master.MensajeError = itzultzaileWeb.Itzuli("Error al abrir el documento")
        End Try

    End Sub
    Public Sub ibVerHist_Click(sender As Object, e As EventArgs)
        Try


            Dim fila As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            Dim idDocumento As String = gvTypeHist.DataKeys(fila.RowIndex)("Id").ToString()
            Dim oDocBLL As New CEticoLib.BLL.cEtico



            Dim oDocumentosBLL As New CEticoLib.BLL.cEtico
            Dim listaEmpDoc As List(Of CEticoLib.ELL.CEtico)

            listaEmpDoc = oDocBLL.LeerEmpDoc(idDocumento)


            If listaEmpDoc.Count > 0 Then


                If listaEmpDoc(0).codCategoria <> "" Then
                    'idDocumento = Server.MapPath("~/") & "Ficheros/Documentos/" & listaEmpDoc(0).ubicacion
                    idDocumento = System.Configuration.ConfigurationManager.AppSettings("PathFicherosBajar").ToString() & listaEmpDoc(0).codCategoria

                    'ScriptManager.RegisterClientScriptBlock(, , "OpenWindow", pClientScript, True)
                    'ScriptManager.RegisterStartupScript(Me, GetType(Page), "anioactual", Script, True)
                    'ScriptManager.RegisterStartupScript(Page, tipo, Clave, "Sys.WebForms.PageRequestManager.getInstance().add_endRequest(" & FuncionJS & ");", True)

                    'Dim URL As String = "ReferenciaVentaBrain.aspx?IdRef=" & CommandArgument
                    Dim script As String = "<script type=""text/javascript"">window.open('" & idDocumento & "');</script>"
                    ClientScript.RegisterStartupScript(Me.GetType, "openWindow", script)
                    'Process.Start(idDocumento)

                Else
                    Master.MensajeInfo = itzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                End If

            Else
                Master.MensajeInfo = itzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
            End If
        Catch ex As Exception

            Master.MensajeError = itzultzaileWeb.Itzuli("Error al abrir el documento")
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'If System.Configuration.ConfigurationManager.AppSettings("presidencia").ToString = PageBase.rolUsuario.ToString Or System.Configuration.ConfigurationManager.AppSettings("RRHH").ToString = PageBase.rolUsuario.ToString Then
            '    pnl1.Visible = True

            'Else
            '    pnl1.Visible = False

            'End If

            If Not (Page.IsPostBack) Then
                   Initialize()
            End If

            'PageBase.rolUsuario = 2
            'plantaAdmin = 99

        Catch ex As Exception
            Throw New Sablib.BatzException("Error al mostrar una solicitud", ex)
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
            'para historico, cuando se da a guardar, se graba en tabla ese movimiento con su fecha, luego saco grid por fecha


            Dim mailTo As String

            Dim paramBLL As New SabLib.BLL.ParametrosBLL
            Dim oParams As SabLib.ELL.Parametros = paramBLL.consultar()

            Dim oDocBLL As New CEticoLib.BLL.cEtico
            Dim txtResponsable As String
            Dim txtAccion As String

            If e.CommandName = "Edit" Then
                Dim id As Integer 'es el numero de fila
                id = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex

                Dim valorSeccionado As Object
                Dim valorSeccionado2 As Object
                Dim Claveseleccionada As Object
                'Dim codigoTra As String
                'Dim categoria As String
                'Dim comentario As String
                Dim fech As Object
                Dim fecha As String = ""
                Dim email As String
                valorSeccionado = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).BindingContainer.NamingContainer, System.Web.UI.WebControls.GridView).Rows '(DirectCast(e.CommandSource, System.Web.UI.Control).BindingContainer.Controls)
                valorSeccionado2 = DirectCast(((DirectCast((valorSeccionado(id)), System.Web.UI.Control).Controls(7)).Controls(1)), System.Web.UI.WebControls.ListControl).SelectedItem.Value ' DirectCast((valorSeccionado(6)), System.Web.UI.Control).Controls(1)

                categoria = DirectCast(((DirectCast((valorSeccionado(id)), System.Web.UI.Control).Controls(6)).Controls(1)), System.Web.UI.WebControls.ListControl).SelectedItem.Value
                bajas = DirectCast(((DirectCast((valorSeccionado(id)), System.Web.UI.Control).Controls(12)).Controls(1)), System.Web.UI.WebControls.ListControl).SelectedItem.Value

                If bajas = 99 Then
                    Master.MensajeError = itzultzaileWeb.Itzuli("Debe seleccionar un tipo de Solicitud")
                     BindDataView2(0)
                    Exit Sub
                End If

                Claveseleccionada = DirectCast(valorSeccionado(id), System.Web.UI.Control).Controls(1).Controls(1)   '(DirectCast((valorSeccionado(id)), System.Web.UI.Control).Controls(1)).Controls(0)
                fech = DirectCast(valorSeccionado(id), System.Web.UI.Control).Controls(4).Controls(1)
                fecha = DirectCast(fech, System.Web.UI.WebControls.Label).[Text]
                hfFecha.Value = fecha
                Dim clave As Integer
                clave = CInt(DirectCast(Claveseleccionada, System.Web.UI.WebControls.Label).[Text])

                'para el correo. es el codpersona y saco email, paso como parametro
                codigoTra = DirectCast(Session("Ticket"), SabLib.ELL.Ticket).IdTrabajador   ' DirectCast(((DirectCast((valorSeccionado(id)), System.Web.UI.Control).Controls(2)).Controls(0)), System.Web.UI.WebControls.Label).[Text]

                '      comentario = DirectCast(((DirectCast((valorSeccionado(id)), System.Web.UI.Control).Controls(3)).Controls(1)), System.Web.UI.WebControls.WebControl).ToolTip
                'descomprimir comentario
                '         comentario = Desencriptar(comentario)



                'hay que update el valor encriptar(valorSeccionado2) de la clave clave como sol000 


                Dim Solicitud As New CEticoLib.ELL.CEtico With {.Id = clave, .Accion = CInt(valorSeccionado2), .cierre = CInt(categoria), .bajas = CInt(bajas)}

                hfclave.Value = clave


                email = oDocumentosBLL.CargarEmail(CInt(codigoTra))(0).email  'no el de webconfig
                If valorSeccionado2 = "0" Then
                    'habra que mandar mail al individuo
                    'sabemos que clave es el sol000, y sol001 sera el codpersona
                    'sabemos que el codpersona del que lo solicito es codigoTra, en sab saco su email
                    Dim listatrab As List(Of CEticoLib.ELL.CEtico)

                    listatrab = oDocBLL.LeerEmpDoc(clave)


                    mailTo = oDocumentosBLL.CargarEmail(CInt(listatrab(0).Idtra))(0).email
                    '    SabLib.BLL.Utils.EnviarEmail("solicitudescr@batz.es", mailTo, itzultzaileWeb.Itzuli("Su solicitud al Consejo Rector se está tramitando"), itzultzaileWeb.Itzuli("Su solicitud al Consejo Rector se está tramitando") & "<br><br>", oParams.ServidorEmail)

                End If
                If valorSeccionado2 = "1" Or valorSeccionado2 = "2" Then
                    hfTipo.Value = valorSeccionado2
                    varAceptado = valorSeccionado2
                    ModalPopupExtender4.Show() 'motivo de rechazo o cierre
                Else
                    'If valorSeccionado2 = "3" Or CInt(valorSeccionado2) = 0 Then


                    'habra que mandar mail al individuo

                    'Dim paramBLL As New SabLib.BLL.ParametrosBLL
                    'Dim oParams As SabLib.ELL.Parametros = paramBLL.consultar()

                    mailTo = email

                    Dim mensajeto As String = ""

                    'quito el mail por cambio de responsable el 20/06/2018
                    ''''''''''''If categoria = "1" Then
                    ''''''''''''    txtResponsable = "RRHH"
                    ''''''''''''    mensajeto = System.Configuration.ConfigurationManager.AppSettings("emailRRHH").ToString
                    ''''''''''''    SabLib.BLL.Utils.EnviarEmail("solicitudescr@batz.es", mailTo, itzultzaileWeb.Itzuli("Información sobre su solicitud al Consejo Rector"), itzultzaileWeb.Itzuli("Después de analizar la solicitud al Consejo Rector que realizó el día") & " " & fecha & " " & itzultzaileWeb.Itzuli("se ha considerado gestionarla a través de RRHH") & " <br><br>", oParams.ServidorEmail)
                    ''''''''''''Else
                    ''''''''''''    txtResponsable = "Presidencia"
                    ''''''''''''    mensajeto = System.Configuration.ConfigurationManager.AppSettings("emailpresidencia").ToString
                    ''''''''''''    SabLib.BLL.Utils.EnviarEmail("solicitudescr@batz.es", mailTo, itzultzaileWeb.Itzuli("Información sobre su solicitud al Consejo Rector"), itzultzaileWeb.Itzuli("Después de analizar la solicitud al Consejo Rector que realizó el día") & " " & fecha & " " & itzultzaileWeb.Itzuli("se ha considerado tratarla en el Consejo Rector") & " <br><br>", oParams.ServidorEmail)
                    ''''''''''''End If
                    If valorSeccionado2 = "99" Then
                        txtAccion = ""
                    Else


                        If valorSeccionado2 = "0" Then
                            txtAccion = "En tramite"
                        Else
                            If valorSeccionado2 = "1" Then
                                txtAccion = "Aceptada"
                            Else
                                txtAccion = "Denegada"
                            End If
                        End If
                    End If
                    'mail al creador de la solicitud

                End If


                If (oDocumentosBLL.ModificarSolicitud(Solicitud)) Then
                    Dim myTickethist As SabLib.ELL.Ticket
                    Dim lg As New SabLib.BLL.LoginComponent
                    myTickethist = lg.Login(User.Identity.Name.ToLower)


                    'grabar tambien el historico
                    'leo todos los datos del reg
                    Dim oDocumentosBLL As New CEticoLib.BLL.cEtico
                    Dim listaEmpDoc As List(Of CEticoLib.ELL.CEtico)

                    'atencion 'habra que leer el hist

                    listaEmpDoc = oDocumentosBLL.LeerEmpDoc(Solicitud.Id)

                    Solicitud.Fecha = listaEmpDoc(0).Fecha
                    Solicitud.codCategoria = listaEmpDoc(0).codCategoria
                    Solicitud.Idtra = listaEmpDoc(0).Idtra
                    Solicitud.comentario = listaEmpDoc(0).comentario
                    Solicitud.planta = listaEmpDoc(0).planta
                    Solicitud.plantaDesc = myTickethist.NombreCompleto   'listaEmpDoc(0).plantaDesc
                    'Solicitud.campo3 = myTickethist.NombreCompleto
                    ''Solicitud.cierre = listaEmpDoc(0).cierre

                    oDocumentosBLL.ModificarSolicitudHist(Solicitud)


                    'fin hist

                    Master.MensajeInfo = itzultzaileWeb.Itzuli("La solicitud se ha guardado correctamente")
                Else
                    Master.MensajeError = itzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba la solicitud")

                    End If



                'End If
                ''''''''''''''If valorSeccionado2 = "2" Then
                ''''''''''''''    ModalPopupExtender5.Show() ' Cerrada > Comentario de cierre (hay quehacerlo en nuevo campo)
                ''''''''''''''End If
                '''''''''''''''resto no se hace nada (presidencia, etc)
                ''''''''''''''If CInt(valorSeccionado2) = 4 Then

                ''''''''''''''    'si dejamos lo anterior esto ira en botones de abajo
                ''''''''''''''    ModalPopupExtender1.Show() ' RRHH > traducción


                ''''''''''''''    'habra que mandar mail al individuo

                ''''''''''''''    'Dim paramBLL As New SabLib.BLL.ParametrosBLL
                ''''''''''''''    'Dim oParams As SabLib.ELL.Parametros = paramBLL.consultar()

                ''''''''''''''    mailTo = email
                ''''''''''''''    SabLib.BLL.Utils.EnviarEmail("CEtico@batz.es", mailTo, itzultzaileWeb.Itzuli("Su solicitud en Canal Ético se ha pasado a Presidencia") & " ", itzultzaileWeb.Itzuli("Su solicitud en Canal Ético se ha pasado a Presidencia <br><br> Categoría") & ": " & itzultzaileWeb.Itzuli(categoria) & "<br><br>", oParams.ServidorEmail)



                ''''''''''''''End If
                ''''''''''''''If CInt(valorSeccionado2) = 5 Then

                ''''''''''''''    ModalPopupExtender2.Show() ' RRHH > traducción

                ''''''''''''''    'habra que mandar mail al individuo

                ''''''''''''''    'Dim paramBLL As New SabLib.BLL.ParametrosBLL
                ''''''''''''''    'Dim oParams As SabLib.ELL.Parametros = paramBLL.consultar()

                ''''''''''''''    mailTo = email

                ''''''''''''''    SabLib.BLL.Utils.EnviarEmail("CEtico@batz.es", mailTo, itzultzaileWeb.Itzuli("Su solicitud en Canal Ético se ha pasado a Comite de Cumplimiento") & " ", itzultzaileWeb.Itzuli("Su solicitud en Canal Ético se ha pasado a Comite de Cumplimiento <br><br> Categoría") & ": " & itzultzaileWeb.Itzuli(categoria) & "<br><br>", oParams.ServidorEmail)



                ''''''''''''''End If

            End If
            BindDataView2(0)


        Catch ex As Exception
            Throw New Sablib.BatzException("Error al mostrar la solicitud", ex)
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
            Throw New Sablib.BatzException("Error al mostrar la empresa", ex)
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




    Protected Sub gvTypeHist_DataBound(ByVal sender As Object, ByVal e As EventArgs)
        ' Recupera la el PagerRow...
        Dim pagerRow As GridViewRow = gvTypeHist.BottomPagerRow
        If pagerRow IsNot Nothing Then

            gvTypeHist.EditIndex = -1

            ' Recupera los controles DropDownList y label...
            Dim pageList As DropDownList = CType(pagerRow.Cells(0).FindControl("PageDropDownList"), DropDownList)
            Dim pageLabel As Label = CType(pagerRow.Cells(0).FindControl("CurrentPageLabel"), Label)
            If Not pageList Is Nothing Then

                If pageList.SelectedIndex > -1 Then

                    gvTypeHist.PageIndex = pageList.SelectedIndex
                    BindDataView2(0)

                End If

                ' Se crean los valores del DropDownList tomando el número total de páginas... 
                Dim i As Integer
                For i = 0 To gvTypeHist.PageCount - 1  'PageCount  
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
                Dim currentPage As Integer = gvTypeHist.PageIndex + 1
                ' Actualiza el Label control con la �gina actual.
                '   pageLabel.Text = ItzultzaileWeb.Itzuli("Página") & " " & currentPage.ToString() & " " & ItzultzaileWeb.Itzuli("de") & " " & gvType.PageCount.ToString()
                pageList.SelectedIndex = gvTypeHist.PageIndex
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
            '     Dim textoEncriptado As String
            '       textoEncriptado = Encriptar(textoescrito)

            Dim valorSeccionado2 As String = "1"


            Dim Solicitud As New CEticoLib.ELL.CEtico With {.Id = hfclave.Value, .codCategoria = Encriptar(valorSeccionado2), .traduccion = textoescrito}
            'si hemos dejado lo anterior va aqui
            If (oDocumentosBLL.ModificarSolicitudComment(Solicitud)) Then
                'habra que mandar mail al individuo

                Dim paramBLL As New SabLib.BLL.ParametrosBLL
                Dim oParams As SabLib.ELL.Parametros = paramBLL.consultar()

                'con el id de la sol, saco sol001 como codpersona
                Dim lista As List(Of CEticoLib.ELL.CEtico)
                lista = oDocumentosBLL.GetSolicitud(Solicitud.Id)



                Dim mailTo As String = oDocumentosBLL.CargarEmail(lista(0).Idtra)(0).email
                'itzultzaileWeb.Itzuli("Categoría") & ": " & itzultzaileWeb.Itzuli(categoria)
                'SabLib.BLL.Utils.EnviarEmail("SolicitudCR@batz.es", mailTo, itzultzaileWeb.Itzuli("Se ha rechazado la solicitud a Consejo Rector") & " ", "<br><br>" & itzultzaileWeb.Itzuli("Comentario al rechazo") & ":<br>" & textoescrito & "<br><br>", oParams.ServidorEmail)

                'buscar en sab idculturas segun email de usuarios
                Dim tmpCultura As String = ""
                Dim mailcultura As String = ""

                mailcultura = mailTo


                Dim listaType2 As List(Of CEticoLib.ELL.CEtico)
                listaType2 = oDocumentosBLL.CargarCultura(mailcultura)
                If listaType2.Count > 0 Then
                    tmpCultura = listaType2(0).campo1
                End If

                Dim cultureInfotmp As Globalization.CultureInfo
                Dim cultureInfo2 As Globalization.CultureInfo

                cultureInfotmp = Threading.Thread.CurrentThread.CurrentCulture

                If tmpCultura <> "" Then
                    cultureInfo2 = Globalization.CultureInfo.CreateSpecificCulture(tmpCultura)
                    Threading.Thread.CurrentThread.CurrentCulture = cultureInfo2
                End If
                If hfTipo.Value = 1 Then
                    SabLib.BLL.Utils.EnviarEmail("solicitudescr@batz.es", mailTo, itzultzaileWeb.Itzuli("Resolución sobre su solicitud al Consejo Rector"), itzultzaileWeb.Itzuli("Después de analizar la solicitud al Consejo Rector que realizó el día") & " " & hfFecha.Value & " " & "se ha aceptado" & ". <br><br>" & textoescrito, oParams.ServidorEmail)
                Else
                    SabLib.BLL.Utils.EnviarEmail("solicitudescr@batz.es", mailTo, itzultzaileWeb.Itzuli("Resolución sobre su solicitud al Consejo Rector"), itzultzaileWeb.Itzuli("Después de analizar la solicitud al Consejo Rector que realizó el día") & " " & hfFecha.Value & " " & "se ha rechazado" & ". <br><br>" & textoescrito, oParams.ServidorEmail)
                End If


                Threading.Thread.CurrentThread.CurrentCulture = cultureInfotmp



                If CheckBox2.Checked = True Then
                    BindDataView2(1)
                Else
                    BindDataView2(0)
                End If
                Master.MensajeInfo = itzultzaileWeb.Itzuli("La solicitud se ha guardado correctamente")


            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba la solicitud")

            End If




        Catch ex As Exception

            Throw New Sablib.BatzException("Error ", ex)
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
            textoEncriptado = Encriptar(textoescrito)

            Dim valorSeccionado2 As String = "4"

            Dim Solicitud As New CEticoLib.ELL.CEtico With {.Id = hfclave.Value, .codCategoria = Encriptar(valorSeccionado2), .traduccion = textoEncriptado}
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

                'buscar en sab idculturas segun email de usuarios
                Dim tmpCultura As String = ""
                Dim mailcultura As String = ""
                Dim mailcultura2 As String = ""
                mailcultura = mailTo


                Dim listaType2 As List(Of CEticoLib.ELL.CEtico)
                listaType2 = oDocumentosBLL.CargarCultura(mailcultura)
                If listaType2.Count > 0 Then
                    tmpCultura = listaType2(0).campo1
                End If

                Dim cultureInfotmp As Globalization.CultureInfo
                Dim cultureInfo2 As Globalization.CultureInfo

                cultureInfotmp = Threading.Thread.CurrentThread.CurrentCulture

                If tmpCultura <> "" Then
                    cultureInfo2 = Globalization.CultureInfo.CreateSpecificCulture(tmpCultura)
                    Threading.Thread.CurrentThread.CurrentCulture = cultureInfo2
                End If
                SabLib.BLL.Utils.EnviarEmail("SolicitudesCR@batz.es", mailTo, itzultzaileWeb.Itzuli("Se ha asignado a Presidencia una solicitud a Consejo Rector") & " ", bodymensaje, oParams.ServidorEmail)
                Threading.Thread.CurrentThread.CurrentCulture = cultureInfotmp

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


    Protected Sub btnGrabarw2_Click(sender As Object, e As EventArgs) 'RRHH
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





                mailTo = ConfigurationManager.AppSettings("emailRRHH")
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

                'buscar en sab idculturas segun email de usuarios
                Dim tmpCultura As String = ""
                Dim mailcultura As String = ""
                Dim mailcultura2 As String = ""
                mailcultura = mailTo


                Dim listaType2 As List(Of CEticoLib.ELL.CEtico)
                listaType2 = oDocumentosBLL.CargarCultura(mailcultura)
                If listaType2.Count > 0 Then
                    tmpCultura = listaType2(0).campo1
                End If

                Dim cultureInfotmp As Globalization.CultureInfo
                Dim cultureInfo2 As Globalization.CultureInfo

                cultureInfotmp = Threading.Thread.CurrentThread.CurrentCulture

                If tmpCultura <> "" Then
                    cultureInfo2 = Globalization.CultureInfo.CreateSpecificCulture(tmpCultura)
                    Threading.Thread.CurrentThread.CurrentCulture = cultureInfo2
                End If
                SabLib.BLL.Utils.EnviarEmail("SolicitudesCR@batz.es", mailTo, itzultzaileWeb.Itzuli("Se ha asignado a Comité una entrada") & " ", bodymensaje, oParams.ServidorEmail)
                Threading.Thread.CurrentThread.CurrentCulture = cultureInfotmp






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

                Master.MensajeInfo = ItzultzaileWeb.Itzuli("La solicitud se ha cerrado correctamente")

                'mandar mail 
                Dim paramBLL As New Sablib.BLL.ParametrosBLL
                Dim oParams As Sablib.ELL.Parametros = paramBLL.consultar()

                '   Dim mailTo As String = ConfigurationManager.AppSettings("emailRRHH")

                Dim mailTo As String = oDocumentosBLL.CargarEmail(CInt(codigoTra))(0).email 'no el de webconfig ConfigurationManager.AppSettings("emailAdministrador")

                'buscar en sab idculturas segun email de usuarios
                Dim tmpCultura As String = ""
                Dim mailcultura As String = ""
                Dim mailcultura2 As String = ""
                mailcultura = mailTo


                Dim listaType2 As List(Of CEticoLib.ELL.CEtico)
                listaType2 = oDocumentosBLL.CargarCultura(mailcultura)
                If listaType2.Count > 0 Then
                    tmpCultura = listaType2(0).campo1
                End If

                Dim cultureInfotmp As Globalization.CultureInfo
                Dim cultureInfo2 As Globalization.CultureInfo

                cultureInfotmp = Threading.Thread.CurrentThread.CurrentCulture

                If tmpCultura <> "" Then
                    cultureInfo2 = Globalization.CultureInfo.CreateSpecificCulture(tmpCultura)
                    Threading.Thread.CurrentThread.CurrentCulture = cultureInfo2
                End If
                SabLib.BLL.Utils.EnviarEmail("SolicitudCR@batz.es", mailTo, itzultzaileWeb.Itzuli("Se ha cerrado la solicitud") & " ", itzultzaileWeb.Itzuli("Categoría") & ": " & itzultzaileWeb.Itzuli(categoria) & "<br><br>" & itzultzaileWeb.Itzuli("Comentario de cierre") & ":<br>" & textoescrito & "<br><br>", oParams.ServidorEmail)
                Threading.Thread.CurrentThread.CurrentCulture = cultureInfotmp

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

            Throw New Sablib.BatzException("Error ", ex)
        End Try

    End Sub


    Private Sub gvTypeHist_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvTypeHist.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            If System.Configuration.ConfigurationManager.AppSettings("presidencia").ToString.Contains(PageBase.rolUsuario.ToString) Or System.Configuration.ConfigurationManager.AppSettings("RRHH").ToString.Contains(PageBase.rolUsuario.ToString) Then
            Else

                Dim imgDoc1 As DropDownList = CType(e.Row.FindControl("ddlUni2"), DropDownList)
                imgDoc1.Visible = False
                Dim imgDoc2 As DropDownList = CType(e.Row.FindControl("ddlUni3"), DropDownList)
                imgDoc2.Visible = False
                Dim imgDoc3 As Label = CType(e.Row.FindControl("lblFechaModhist"), Label)
                imgDoc3.Visible = False
            End If





        End If
    End Sub

    Private Sub gvType2_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvType2.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            If System.Configuration.ConfigurationManager.AppSettings("presidencia").ToString.Contains(PageBase.rolUsuario.ToString) Or System.Configuration.ConfigurationManager.AppSettings("RRHH").ToString.Contains(PageBase.rolUsuario.ToString) Then
            Else

                Dim imgDoc1 As DropDownList = CType(e.Row.FindControl("ddlUni2"), DropDownList)
                imgDoc1.Visible = False
                Dim imgDoc2 As DropDownList = CType(e.Row.FindControl("ddlUni3"), DropDownList)
                imgDoc2.Visible = False
            End If

        End If
    End Sub
    Private Sub gvTypeHist_PreRender(sender As Object, e As EventArgs) Handles gvTypeHist.PreRender

        If System.Configuration.ConfigurationManager.AppSettings("presidencia").ToString.Contains(PageBase.rolUsuario.ToString) Or System.Configuration.ConfigurationManager.AppSettings("RRHH").ToString.Contains(PageBase.rolUsuario.ToString) Then
            gvTypeHist.Columns(5).HeaderText = "Responsable"
            gvTypeHist.Columns(9).HeaderText = "Tipo solicitud"
            gvTypeHist.Columns(10).HeaderText = "Autor"


            gvTypeHist.DataBind()

        End If



    End Sub

    Private Sub gvType2_PreRender(sender As Object, e As EventArgs) Handles gvType2.PreRender

        If System.Configuration.ConfigurationManager.AppSettings("presidencia").ToString.Contains(PageBase.rolUsuario.ToString) Or System.Configuration.ConfigurationManager.AppSettings("RRHH").ToString.Contains(PageBase.rolUsuario.ToString) Then
            gvType2.Columns(6).HeaderText = "Responsable"
            gvType2.Columns(12).HeaderText = "Tipo solicitud"


            gvType2.DataBind()

        End If



    End Sub

End Class