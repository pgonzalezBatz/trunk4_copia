
Imports System.Data


Public Class OperacionesMod
    Inherits PageBase
    'Inherits System.Web.UI.Page

    Dim oDocumentosBLL As New KaplanLib.BLL.DocumentosBLL




#Region "Propiedades"

    Protected Overrides ReadOnly Property RolesAcceso As System.Collections.Generic.List(Of KaplanLib.ELL.Roles.RolUsuario)
        Get
            Dim roles As New List(Of KaplanLib.ELL.Roles.RolUsuario)
            roles.Add(KaplanLib.ELL.Roles.RolUsuario.Administrador)
            roles.Add(KaplanLib.ELL.Roles.RolUsuario.Administrador2)
            'roles.Add(Kaplanlib.ELL.Roles.RolUsuario.Recepcion)
            Return roles
        End Get
    End Property

#End Region


#Region "METODOS"


    ''' <summary>
    ''' Comprueba que el perfil tenga acceso a la página
    ''' </summary>
    Private Sub ComprobarAcceso()
        ' Hay que comprobar que alguno de los roles del usuario está contenido en la lista de roles de acceso de la pagina
        If (PerfilUsuario) Is Nothing Then  'es un usuario no identificado en web.config. solo ira a aquello que no lo necesite. extranet que no pondre esto.  comprobare el id de depto
            '    If (RolesAcceso Is Nothing ) Then
            Dim segmentos As String() = Page.Request.Url.Segments
            Dim pag As String = segmentos(Page.Request.Url.Segments.GetUpperBound(0))
            'WriteLog("Se ha intentado acceder a una ruta que su perfil no lo permite. Pag(" & pag & ")", TipoLog.Warn, Nothing)
            Response.Redirect("~/PermisoDenegado.aspx", True)
            '    End If

        Else


            Dim tieneAcceso As Boolean = ExisteRolEnPagina(PerfilUsuario.IdRol)

            ' El administrador puede entrar a todas la páginas aunque no se haya definido su rol explicitamente en cada página
            If ((RolesAcceso Is Nothing OrElse Not tieneAcceso) AndAlso Not (PerfilUsuario.IdRol = KaplanLib.ELL.Roles.RolUsuario.Administrador)) Then
                Dim segmentos As String() = Page.Request.Url.Segments
                Dim pag As String = segmentos(Page.Request.Url.Segments.GetUpperBound(0))
                'WriteLog("Se ha intentado acceder a una ruta que su perfil no lo permite. Pag(" & pag & ")", TipoLog.Warn, Nothing)
                Response.Redirect("~/PermisoDenegado.aspx", True)
            End If



        End If
    End Sub

    ''' <summary>
    ''' Comprueba que el rol del usuario está contenido en la lista de roles de acceso de la pagina
    ''' </summary>
    ''' <param name="rolUsuario"></param>
    ''' <returns>True si existe alguno. False en caso contrario</returns>
    ''' <remarks></remarks>
    Private Function ExisteRolEnPagina(ByVal rolUsuario As Integer) As Boolean
        Dim idRol As Integer = Integer.MinValue
        Dim existe As Boolean = False
        If (RolesAcceso IsNot Nothing) Then
            existe = RolesAcceso.Exists(Function(f) f = [Enum].Parse(GetType(KaplanLib.ELL.Roles.RolUsuario), rolUsuario.ToString()))
            If (existe) Then
                Return existe
            End If
        End If

        Return existe
    End Function
#End Region


#Region "METODOS"

    ''' <summary>
    ''' Inicializa las variables y vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Initialize()
        ClearGridView()

        BindDataView()
    End Sub

    ''' <summary>
    ''' Limpia los datos vinculados
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ClearGridView()
        gvType.DataSource = Nothing
        gvType.DataBind()
    End Sub
    ''' <summary>
    ''' Vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BindDataView()
        Try
            Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            Dim listaType As New List(Of KaplanLib.ELL.Xpert)




            Dim strSql As String = "select * from cubos.DESPIECES where t2tenr='" & hfReferencia.Value & "' " ' and T2KOMP ='" & hfcomponente.Value & "'"
            Dim cn As New OleDb.OleDbConnection()
            Dim dr As OleDb.OleDbDataReader
            cn.ConnectionString = ConfigurationManager.ConnectionStrings("AS400").ConnectionString
            Dim cm As New OleDb.OleDbCommand(strSql, cn)
            cm.CommandTimeout = 30
            cn.Open()
            dr = cm.ExecuteReader()
            Dim plantas As String = ""
            'Dim aNHI As New List(Of String)

            Dim cont As Integer = -1
            listaType.Clear()
            Dim referencia As String = ""
            Dim desc_ref As String = ""
            Dim padre As String = ""
            Dim desc_comp As String = ""
            Dim componente As String = ""
            Dim GM As String = ""
            Dim Nivel As Integer = 0
            Dim Cantidad As Integer = 0
            Dim Tipo As Integer = 0

            While dr.Read
                Nivel = If(dr("T2NIVL") Is Nothing OrElse dr("T2NIVL") Is DBNull.Value, Nivel, dr("T2NIVL"))
                desc_ref = If(dr("T2BEZ1") Is Nothing OrElse dr("T2BEZ1") Is DBNull.Value, desc_ref, dr("T2BEZ1"))
                'If Nivel = 1 Then
                '    txtDescref.Text = hfcomponente.Value
                '    txtDescref.Visible = True
                'End If
                desc_comp = If(dr("T2DENO") Is Nothing OrElse dr("T2DENO") Is DBNull.Value, desc_comp, dr("T2DENO"))
                componente = If(dr("T2KOMP") Is Nothing OrElse dr("T2KOMP") Is DBNull.Value, componente, dr("T2KOMP"))
                If componente = hfcomponente.Value Then
                    txtDescref.Text = desc_comp
                End If
                If hfreferencia.Value = hfcomponente.Value Then
                    txtDescref.Text = desc_ref ' es la ref principal
                End If
                padre = If(dr("T2BGNR") Is Nothing OrElse dr("T2BGNR") Is DBNull.Value, "sin padre", dr("T2BGNR"))
                If padre = hfcomponente.Value Then

                    Dim item As New KaplanLib.ELL.Xpert
                    cont = cont + 1



                    referencia = If(dr("t2tenr") Is Nothing OrElse dr("t2tenr") Is DBNull.Value, referencia, dr("t2tenr"))

                    desc_comp = If(dr("T2DENO") Is Nothing OrElse dr("T2DENO") Is DBNull.Value, desc_comp, dr("T2DENO"))
                    componente = If(dr("T2KOMP") Is Nothing OrElse dr("T2KOMP") Is DBNull.Value, componente, dr("T2KOMP"))
                    Cantidad = If(dr("T2NPZA") Is Nothing OrElse dr("T2NPZA") Is DBNull.Value, Cantidad, dr("T2NPZA"))
                    GM = If(dr("T2MAGR") Is Nothing OrElse dr("T2MAGR") Is DBNull.Value, GM, dr("T2MAGR"))
                    Tipo = If(dr("T2TAR2") Is Nothing OrElse dr("T2TAR2") Is DBNull.Value, Tipo, dr("T2TAR2"))


                    'If Tipo = 1 Or Tipo = 2 Then
                    '    item.Check1 = "True"
                    'Else
                    '    item.Check1 = "False"
                    'End If

                    item.referencia = referencia
                    'item.Id = componente

                    item.desc_ref = desc_ref
                    item.desc_comp = desc_comp
                    item.GM = GM
                    item.componente = componente
                    item.padre = padre
                    item.cantidad = Cantidad
                    listaType.Add(item)

                    txtDescref.Visible = True

                End If

            End While

            'fin prueba



            Dim listaType2 As List(Of KaplanLib.ELL.Asociacion)
            If hfwork.Value <> "" And hfstep.Value <> "" Then
                'los hijos vienen en el 666
                listaType2 = oDocBLL.CargarListaAsociaciones(CInt(hfcont.Value), hfReferencia.Value, hfcomponente.Value, CInt(hfwork.Value), CInt(hfstep.Value))
                'listaType2 = oDocBLL.CargarListaAsociaciones(hfReferencia.Value, hfcomponente.Value, hfwork.Value, hfstep.Value) 'work, step

                Dim arrHijos As String() = Nothing
                'arrHijos = Split(listaType2(0).Hijos, ";")
                arrHijos = Split(hfhijos.Value, ";")
                For i = 0 To listaType.Count - 1

                    listaType(i).Check1 = False
                    If arrHijos.Length > 0 Then

                        For j = 0 To arrHijos.Length - 1
                            If arrHijos(j) = listaType(i).componente Then
                                listaType(i).Check1 = "True"


                            End If
                        Next
                    End If


                Next
            Else

                For i = 0 To listaType.Count - 1
                    listaType(i).Check1 = False
                Next
            End If



            dr.Close()

            cn.Close()
            cn.Dispose()


            If (listaType.Count > 0) Then

                gvType.DataSource = listaType
                gvType.DataBind()
                gvType.Caption = String.Empty
            Else

                gvType.DataSource = Nothing
                gvType.DataBind()
                gvType.Caption = "No hay registros"
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error", ex)
        End Try
    End Sub



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

    ''' <summary>
    ''' Cancelación de la edición del grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvType_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)

    End Sub

    ''' <summary>
    ''' Habilitar la edición de un registro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvType_RowEditing(sender As Object, e As GridViewEditEventArgs)

    End Sub

    ''' <summary>
    ''' Edición de un registro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvType_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)

    End Sub



#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            hfReferencia.Value = Request.QueryString("idRef")
            hfcomponenteID.Value = Request.QueryString("idComp") 'linea 666 de asociaciones, hfcomponenteID es el id que viene de calculo
            Dim linea666 As List(Of KaplanLib.ELL.Asociacion)
            linea666 = oDocBLL.CargarListaAsociaciones4(hfReferencia.Value, hfcomponenteID.Value)
            hfcomponente.Value = linea666(0).componente
            hfwork.Value = linea666(0).Work
            hfstep.Value = linea666(0).Steps
            hfcont.Value = linea666(0).cont
            hfhijos.Value = linea666(0).Hijos

            'tenemos cont, ref, comp, procework, step, hijos en 666
            ' leere con esos 5 valores en asociaciones <> 666 y estaran las lineas, loadListAsociaciones3
            Dim listaTypeStep As New List(Of KaplanLib.ELL.Asociacion)
            listaTypeStep = oDocBLL.CargarListaAsociaciones4(linea666(0).cont, linea666(0).referencia, linea666(0).componente, linea666(0).Work, linea666(0).Steps)
            For k = 0 To listaTypeStep.Count - 1
                listaTypeStep(k).Max = listaTypeStep(k).Max / 1000
                listaTypeStep(k).Min = listaTypeStep(k).Min / 1000
            Next






            If 0 > 0 Then
                Dim Idsteps As Int32
                'Dim Idst As List(Of KaplanLib.ELL.Kaplan)
                'Idst = oDocBLL.CargarListaEmpresastexto(PageBase.plantaAdmin, DdlSteps.SelectedValue)
                'Idsteps = Idst(0).Id
                Idsteps = CInt(hfstep.Value)
                CargarDetalle(Idsteps)
            End If


            'rellenar modal
            Dim liPreventivavacio As New ListItem("", 0)
            'sacar descripciones
            Dim des1 As List(Of KaplanLib.ELL.Kaplan)
            Dim des2 As List(Of KaplanLib.ELL.Kaplan)
            des2 = oDocBLL.CargarTiposEmpresaProcess(linea666(0).Work, 0)
            des1 = oDocBLL.loadListEmpresas3(linea666(0).Steps)
            Dim liPreventivavacio3 As New ListItem(des1(0).Nombre, 0)
            Dim liPreventivavacio2 As New ListItem(des2(0).Nombre, 0)

            TipoC.Items.Clear()
            TipoC.Items.Add(liPreventivavacio)

            Dim ATipoC As List(Of KaplanLib.ELL.Kaplan)
            ATipoC = oDocBLL.CargarTiposChar()

            If ATipoC.Count > 0 Then


                For Each Acaracteristic In ATipoC
                    Dim Aliresponsable2 As New ListItem(Acaracteristic.textolibre & " " & Acaracteristic.Nombre, Acaracteristic.Id)
                    TipoC.Items.Add(Aliresponsable2)

                Next

            End If


            ClaseC.Items.Clear()
            ClaseC.Items.Add(liPreventivavacio)

            Dim ATipoC2 As List(Of KaplanLib.ELL.Kaplan)
            ATipoC2 = oDocBLL.CargarListaEmpresasComponent(0)

            If ATipoC.Count > 0 Then


                For Each Acaracteristic2 In ATipoC2
                    Dim Aliresponsable22 As New ListItem(Acaracteristic2.Nombre, Acaracteristic2.Id)
                    ClaseC.Items.Add(Aliresponsable22)

                Next

            End If

            'fin modal

            'Dim liPreventivavacio As New ListItem("", 0)


            mView.ActiveViewIndex = 1
            'ComprobarAcceso()

            If 0 > 0 Then

            Else


                DdlSteps.Items.Clear()
                DdlSteps.Items.Add(liPreventivavacio3)

                Dim listaSteps As List(Of KaplanLib.ELL.Kaplan)
                If DdlWork.SelectedValue <> "" Then
                    listaSteps = oDocBLL.CargarListaEmpresas2(DdlWork.SelectedValue)
                Else
                    listaSteps = oDocBLL.CargarListaEmpresas2(0)
                End If


                If listaSteps.Count > 0 Then


                    For Each steps In listaSteps 'steps
                        Dim listeps As New ListItem(steps.Nombre, steps.Id)
                        DdlSteps.Items.Add(listeps)

                    Next

                End If

            End If

            If 0 > 0 Then
            Else


                DdlWork.Items.Clear()
                DdlWork.Items.Add(liPreventivavacio2)

                Dim listaWork As List(Of KaplanLib.ELL.Kaplan)
                listaWork = oDocBLL.CargarListaEmpresasProcess(PageBase.plantaAdmin)

                If listaWork.Count > 0 Then


                    For Each caracteristic In listaWork
                        Dim liresponsable2 As New ListItem(caracteristic.Nombre, caracteristic.Id)
                        'DdlWork.Items.Add(liresponsable2)

                    Next

                End If
            End If



            DdlWork.SelectedIndex = 0
            DdlSteps.SelectedIndex = 0


            'DdlWork.SelectedValue = linea666(0).Work
            'DdlSteps.SelectedValue = linea666(0).Steps


            DdlCaracterist.Items.Clear()

            DdlCaracterist.Items.Add(liPreventivavacio)

            Dim listaCar As List(Of KaplanLib.ELL.Kaplan)
            listaCar = oDocBLL.CargarTiposChar()

            If listaCar.Count > 0 Then


                For Each caracteristic In listaCar
                    Dim liresponsable2 As New ListItem(caracteristic.Nombre, caracteristic.Id)
                    DdlCaracterist.Items.Add(liresponsable2)

                Next

            End If
            If Not (Page.IsPostBack) Then
                TxtCaracteristicas3.Text = linea666(0).Textolibre3

                mView.ActiveViewIndex = 1



                Initialize()
                BindDataView2()
                BindDataView3()
            End If




        Catch ex As Exception
            Throw New SabLib.BatzException("Error", ex)
        End Try
    End Sub





    Private Sub CargarDocumentos()

    End Sub

    Protected Sub gvType2_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)
        gvType2.EditIndex = -1
        BindDataView2() '2(0)
        btnQuitarSeleccionados.Focus()
    End Sub
    Protected Sub gvType3_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)
        gvType3.EditIndex = -1
        BindDataView3() '2(0)
        btnQuitarSeleccionadosx.Focus()
    End Sub
    Protected Sub gvType2_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)
        gvType2.EditIndex = -1
        BindDataView2() '2(0)

    End Sub
    Protected Sub gvType3_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)
        gvType3.EditIndex = -1
        BindDataView3() '2(0)

    End Sub


    Protected Sub ddlTheDropDownList_Init(sender As Object, e As EventArgs)
        'ESTE ES CLASE, EL PRIMERO CON DIBUJOS
        Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL

        Dim listaEmpr As List(Of KaplanLib.ELL.Kaplan)
        listaEmpr = oDocBLL.CargarTiposChar()

        For i = 0 To listaEmpr.Count - 1
            'listaEmpr2.Add(listaEmpr(i).textolibre & " " & listaEmpr(i).Nombre)
            listaEmpr(i).Nombre = listaEmpr(i).textolibre & " " & listaEmpr(i).Nombre
        Next


        Dim ddl As DropDownList
        ddl = sender
        'ddl.DataSource = listaEmpr2
        'ddl.DataBind()
        Dim liPreventivavacio As New ListItem("", 0)


        ddl.Items.Add(liPreventivavacio)

        If listaEmpr.Count > 0 Then
            ddl.Items.Clear()
            ddl.Items.Add(liPreventivavacio)
            For Each responsable In listaEmpr
                Dim liresponsable As New ListItem(responsable.Nombre, responsable.Id)

                ddl.Items.Add(liresponsable)

            Next
        End If

        'tenemos el simbolo como hfClaseC2.Value, buscamos el id para ponerlo en selected value
        Dim listaSim As List(Of KaplanLib.ELL.Kaplan)
        listaSim = oDocBLL.CargarTiposCharSimb(Left(hfClaseC2.Value, 1))

        'listaSim = oDocBLL.CargarTiposCharSimb2(hfTipoC2.Value)

        If listaSim.Count > 0 Then
            ddl.SelectedValue = listaSim(0).Id.ToString
            'Else
            '    ddl.SelectedValue = "0"
        End If

        'ddl.SelectedItem.Text = hfClaseC.Value
    End Sub

    Protected Sub ddlTheDropDownList2_Init(sender As Object, e As EventArgs)
        'ESTE ES TIPO, EL SEGUNDO SIN DIBUJOS
        Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL

        Dim listaEmpr As List(Of KaplanLib.ELL.Kaplan)
        'listaEmpr = oDocBLL.CargarTiposChar()

        'Dim ATipoC2 As List(Of KaplanLib.ELL.Kaplan)
        listaEmpr = oDocBLL.CargarListaEmpresasComponent(0)

        For i = 0 To listaEmpr.Count - 1
            'listaEmpr2.Add(listaEmpr(i).textolibre & " " & listaEmpr(i).Nombre)
            listaEmpr(i).Nombre = listaEmpr(i).textolibre & " " & listaEmpr(i).Nombre
        Next


        Dim ddl As DropDownList
        ddl = sender
        'ddl.DataSource = listaEmpr2
        'ddl.DataBind()

        If listaEmpr.Count > 0 Then
            ddl.Items.Clear()
            For Each responsable In listaEmpr
                Dim liresponsable As New ListItem(responsable.Nombre, responsable.Id)

                ddl.Items.Add(liresponsable)

            Next
        End If

        'tenemos el texto como hfTipoC2.Value, buscamos el id para ponerlo en selected value

        Dim listaSim As List(Of KaplanLib.ELL.Kaplan)
        listaSim = oDocBLL.CargarTiposCharSimb2(hfTipoC2.Value)


        If listaSim.Count > 0 Then
            ddl.SelectedValue = listaSim(0).Id.ToString
            'Else
            '    ddl.SelectedValue = "0"
        End If

        'ddl.SelectedItem.Text = hfClaseC.Value
    End Sub

    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' <para>Redirecciona al detalle del item seleccionado</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvType2_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvType2.RowCommand
        Try
            If e.CommandName = "Edit" Then
                Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
                Dim Iddoc As Int32 = CInt(strDoc)
                'mirar cual era el 

                Dim campo3 As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(4)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
                Dim campo4 As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(5)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
                hfClaseC2.Value = campo3
                hfTipoC2.Value = campo4
            End If

            If e.CommandName = "Activar" Then

                Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
                Dim Iddoc As Int32 = CInt(strDoc)

                'Dim campo1 As TextBox = DirectCast(gvType2.Rows(0).FindControl("txtModTarjetaxx"), TextBox)
                ' es el valor modificado      campo1.text
                Dim campo1 As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(2)).Controls(1)), System.Web.UI.WebControls.TextBox).[Text]
                Dim campo2 As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(3)).Controls(1)), System.Web.UI.WebControls.TextBox).[Text]

                Dim campo3 As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(4)).Controls(1)), System.Web.UI.WebControls.DropDownList).SelectedItem.Text
                Dim campo32 As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(4)).Controls(1)), System.Web.UI.WebControls.DropDownList).SelectedValue

                'Dim campo3old As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(4)).Controls(1)), System.Web.UI.WebControls.TextBox).[Text]
                Dim campo4 As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(5)).Controls(1)), System.Web.UI.WebControls.DropDownList).SelectedItem.Text
                Dim campo5 As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(6)).Controls(1)), System.Web.UI.WebControls.TextBox).[Text]
                Dim campo6 As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(7)).Controls(1)), System.Web.UI.WebControls.TextBox).[Text]
                If IsNumeric(campo5) Then
                    campo5 = campo5
                Else
                    campo5 = "0"
                End If
                If IsNumeric(campo6) Then
                    campo6 = campo6
                Else
                    campo6 = "0"
                End If

                Dim TipoTra As New KaplanLib.ELL.Asociacion With {.Id = Iddoc, .Caracteristica = campo1, .Caracteristica2 = campo2, .tClaseC = campo3, .tTipoC = campo4, .Max = campo5, .Min = campo6}


                'debo coger los datos y meterlos en el pop, para luego guradarlos

                If (oDocumentosBLL.ModificarEmp2CharVV(TipoTra)) Then
                    gvType2.EditIndex = -1
                    BindDataView2()
                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("Modificado").ToUpper
                    BindDataView2()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Error").ToUpper
                End If
                btnQuitarSeleccionados.Focus()
            End If



            If e.CommandName = "Activar2" Then 'esto va a ser condiciones
                Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
                Dim Iddoc As Int32 = CInt(strDoc) 'es el id de stepst



                Dim campo1 As TextBox = DirectCast(gvType2.Rows(Iddoc).FindControl("TrabajoSTDa"), TextBox)
                Dim campo2 As TextBox = DirectCast(gvType2.Rows(Iddoc).FindControl("Parametroa"), TextBox)
                Dim campo3 As TextBox = DirectCast(gvType2.Rows(Iddoc).FindControl("txtEspecificaciona"), TextBox)
                Dim campo4 As TextBox = DirectCast(gvType2.Rows(Iddoc).FindControl("txtClasea"), TextBox)
                Dim campo5 As TextBox = DirectCast(gvType2.Rows(Iddoc).FindControl("txtMaximo2a"), TextBox)
                Dim campo6 As TextBox = DirectCast(gvType2.Rows(Iddoc).FindControl("txtMinimo2a"), TextBox)
                Dim TipoTra As New KaplanLib.ELL.Asociacion With {.Id = Iddoc, .Caracteristica = campo1.Text, .Caracteristica2 = campo2.Text, .tClaseC = campo3.Text} ' , .tTipoC = campo4, .Max = campo5, .Min = campo6
                'debo coger los datos y meterlos en el pop, para luego guradarlos

                If (oDocumentosBLL.ModificarEmp3Char(TipoTra)) Then

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("Modificado").ToUpper
                    'BindDataView()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Error").ToUpper
                End If
                btnQuitarSeleccionados.Focus()

            End If


            If e.CommandName = "Desactivar" Then
                Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
                Dim Iddoc As Int32 = CInt(strDoc)

                ''Dim campo1 As TextBox = DirectCast(gvType2.Rows(0).FindControl("txtModTarjetaxx"), TextBox)
                '' es el valor modificado      campo1.text
                'Dim campo1 As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(2)).Controls(1)), System.Web.UI.WebControls.TextBox).[Text]
                'Dim campo2 As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(3)).Controls(1)), System.Web.UI.WebControls.TextBox).[Text]
                'Dim campo3 As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(4)).Controls(1)), System.Web.UI.WebControls.TextBox).[Text]
                'Dim campo4 As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(5)).Controls(1)), System.Web.UI.WebControls.TextBox).[Text]
                'Dim campo5 As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(6)).Controls(1)), System.Web.UI.WebControls.TextBox).[Text]
                'Dim campo6 As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(7)).Controls(1)), System.Web.UI.WebControls.TextBox).[Text]
                'If IsNumeric(campo5) Then
                'Else
                '    campo5 = "0"
                'End If
                'If IsNumeric(campo6) Then
                'Else
                '    campo6 = "0"
                'End If

                Dim TipoTra As New KaplanLib.ELL.Asociacion With {.Id = Iddoc}
                'debo coger los datos y meterlos en el pop, para luego guradarlos

                If (oDocumentosBLL.GuardarOpe3BorrarAsociacion(TipoTra)) Then

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("Borrado").ToUpper
                    BindDataView2()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Error").ToUpper
                End If
                btnQuitarSeleccionados.Focus()
            End If
            'btnQuitarSeleccionados.Focus()
            'TxtCaracteristicas3.Focus()

        Catch ex As Exception
            Throw New SabLib.BatzException("Error", ex)
        End Try

    End Sub

    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' <para>Redirecciona al detalle del item seleccionado</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvType3_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvType3.RowCommand
        Try

            If e.CommandName = "Activar" Then
                Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
                Dim Iddoc As Int32 = CInt(strDoc)


                ' es el valor modificado      campo1.text
                Dim campo1 As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(2)).Controls(1)), System.Web.UI.WebControls.TextBox).[Text]
                Dim campo2 As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(3)).Controls(1)), System.Web.UI.WebControls.TextBox).[Text]
                Dim campo3 As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(4)).Controls(1)), System.Web.UI.WebControls.TextBox).[Text]
                Dim campo4 As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(5)).Controls(1)), System.Web.UI.WebControls.TextBox).[Text]
                Dim campo5 As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(6)).Controls(1)), System.Web.UI.WebControls.TextBox).[Text]
                Dim campo6 As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(7)).Controls(1)), System.Web.UI.WebControls.TextBox).[Text]
                If IsNumeric(campo5) Then
                    campo5 = campo5
                Else
                    campo5 = "0"
                End If
                If IsNumeric(campo6) Then
                    campo6 = campo6
                Else
                    campo6 = "0"
                End If

                Dim TipoTra As New KaplanLib.ELL.Asociacion With {.Id = Iddoc, .Caracteristica = campo1, .Caracteristica2 = campo2, .tClaseC = campo3, .tTipoC = campo4, .Max = campo5, .Min = campo6}
                'debo coger los datos y meterlos en el pop, para luego guradarlos

                If (oDocumentosBLL.ModificarEmp2CharVVV(TipoTra)) Then

                    gvType3.EditIndex = -1
                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("Modificado").ToUpper
                    BindDataView3()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Error").ToUpper
                End If
                btnQuitarSeleccionadosx.Focus()
            End If


            If e.CommandName = "Desactivar" Then
                Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
                Dim Iddoc As Int32 = CInt(strDoc)

                ''Dim campo1 As TextBox = DirectCast(gvType2.Rows(0).FindControl("txtModTarjetaxx"), TextBox)
                '' es el valor modificado      campo1.text
                'Dim campo1 As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(2)).Controls(1)), System.Web.UI.WebControls.TextBox).[Text]
                'Dim campo2 As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(3)).Controls(1)), System.Web.UI.WebControls.TextBox).[Text]
                'Dim campo3 As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(4)).Controls(1)), System.Web.UI.WebControls.TextBox).[Text]
                'Dim campo4 As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(5)).Controls(1)), System.Web.UI.WebControls.TextBox).[Text]
                'Dim campo5 As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(6)).Controls(1)), System.Web.UI.WebControls.TextBox).[Text]
                'Dim campo6 As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(7)).Controls(1)), System.Web.UI.WebControls.TextBox).[Text]
                'If IsNumeric(campo5) Then
                'Else
                '    campo5 = "0"
                'End If
                'If IsNumeric(campo6) Then
                'Else
                '    campo6 = "0"
                'End If

                Dim TipoTra As New KaplanLib.ELL.Asociacion With {.Id = Iddoc}
                'debo coger los datos y meterlos en el pop, para luego guradarlos

                If (oDocumentosBLL.GuardarOpe3BorrarAsociacion(TipoTra)) Then

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("Borrado").ToUpper
                    BindDataView3()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Error").ToUpper
                End If
                btnQuitarSeleccionadosx.Focus()
            End If
            'btnQuitarSeleccionados.Focus()
            'TxtCaracteristicas3.Focus()

        Catch ex As Exception
            Throw New SabLib.BatzException("Error", ex)
        End Try

    End Sub

    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' <para>Redirecciona al detalle del item seleccionado</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvType_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvType.RowCommand
        Try
            If e.CommandName = "Edit" Then

                Dim Iddoc As Int32 = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).componente

                BindDataView()   'para limpiar el grid
                CargarDetalle(Iddoc)

            End If


            If e.CommandName = "Desactivar" Then
                Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
                Dim Iddoc As Int32 = CInt(strDoc)

                Dim TipoTra As New KaplanLib.ELL.Kaplan With {.Obsoleto = 1, .Id = Iddoc}
                If (oDocumentosBLL.ModificarEmp2(TipoTra)) Then

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("Activated").ToUpper
                    BindDataView()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Error").ToUpper
                End If

            End If
            If e.CommandName = "Activar" Then
                Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
                Dim Iddoc As Int32 = CInt(strDoc)

                Dim TipoTra As New KaplanLib.ELL.Kaplan With {.Obsoleto = 0, .Id = Iddoc}
                If (oDocumentosBLL.ModificarEmp2(TipoTra)) Then

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("Activated").ToUpper
                    BindDataView()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Error").ToUpper
                End If

            End If


        Catch ex As Exception
            Throw New SabLib.BatzException("Error", ex)
        End Try

    End Sub
    Protected Sub gvType_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvType.PageIndexChanging
        gvType.PageIndex = e.NewPageIndex
        BindDataView()
    End Sub
    Protected Sub PageDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        ' Recupera la fila.
        Dim pagerRow As GridViewRow = gvType.BottomPagerRow
        ' Recupera el control DropDownList...
        Dim pageList As DropDownList = CType(pagerRow.Cells(0).FindControl("PageDropDownList"), DropDownList)
        ' Se Establece la propiedad PageIndex para visualizar la página seleccionada...
        gvType.PageIndex = pageList.SelectedIndex
        'Quita el mensaje de información si lo hubiera...
        '   lblInfo.Text = ""
    End Sub
    Protected Sub gvType_DataBound(ByVal sender As Object, ByVal e As EventArgs)
        ' Recupera la el PagerRow...
        Dim pagerRow As GridViewRow = gvType.BottomPagerRow
        If pagerRow IsNot Nothing Then



            ' Recupera los controles DropDownList y label...
            Dim pageList As DropDownList = CType(pagerRow.Cells(0).FindControl("PageDropDownList"), DropDownList)
            Dim pageLabel As Label = CType(pagerRow.Cells(0).FindControl("CurrentPageLabel"), Label)
            If Not pageList Is Nothing Then

                If pageList.SelectedIndex > -1 Then

                    gvType.PageIndex = pageList.SelectedIndex
                    BindDataView()

                End If

                ' Se crean los valores del DropDownList tomando el número total de páginas... 
                Dim i As Integer
                For i = 0 To gvType.PageCount - 1  'PageCount  
                    ' Se crea un objeto ListItem para representar la �gina...
                    Dim pageNumber As Integer = i + 1
                    Dim item As ListItem = New ListItem(pageNumber.ToString())

                    If pageList.SelectedIndex > 1 Then
                        If i = pageList.SelectedIndex Then ' gvType.PageIndex Then
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
                pageLabel.Text = ItzultzaileWeb.Itzuli("Página") & " " & currentPage.ToString() & " " & ItzultzaileWeb.Itzuli("de") & " " & gvType.PageCount.ToString()
                pageList.SelectedIndex = gvType.PageIndex
            End If
        End If
    End Sub


    ''' <summary>
    ''' Cargar los tipos y Habilitar/Deshabilitar dependiendo del producto seleccionado
    ''' </summary>
    ''' <param name="idDocumento"></param>
    ''' <remarks></remarks>
    Private Sub ConfiguracionProduct(ByVal idDocumento As Integer, Optional ByVal idType As Integer = 0, Optional ByVal idTransmissionMode As Integer = 0)
        Try
            Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            Dim listaDOC As List(Of KaplanLib.ELL.Documentos)
            Dim lista As List(Of KaplanLib.ELL.Asociacion)
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim liPreventivavacio As New ListItem("", 0)

            'si es nuevo elemento
            If idDocumento = 0 Then
                'lblNuevaSolicitud.Text = "Creación de una nueva empresa"
                flag_Modificar.Value = "0"
                'listaDOC = oDocBLL.CargarLista(PageBase.plantaAdmin)


            Else
                lista = oDocBLL.loadListStepsChar2t(CInt(hfwork.Value), idDocumento, 0)
                For i = 0 To lista.Count - 1
                    lista(i).Max = lista(i).Max / 1000
                    lista(i).Min = lista(i).Min / 1000
                Next
                If lista.Count > 0 Then


                    flag_Modificar.Value = idDocumento
                    HdnNombre.Value = "mirar" ' lista(0).Id.ToString
                    HdnCIF.Value = "mirar" 'lista(0).Id.ToString




                End If





            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error", ex)
        End Try

    End Sub




    ''' <summary>
    ''' Cargar el detalle de un portador de coste
    ''' </summary>
    Private Sub CargarDetalle(ByVal idDocumento As Integer)
        mView.ActiveViewIndex = 1
        ConfiguracionProduct(idDocumento)

    End Sub



    ''' <summary>
    ''' Guardar un documento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnGuardarNuevaReferencia_Click(sender As Object, e As EventArgs) Handles btnGuardarNuevaSolicitud.Click
        Try

            Dim i As Int16
            Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            Dim listaType As New List(Of KaplanLib.ELL.Xpert)
            Dim listaTypeCond As New List(Of KaplanLib.ELL.Asociacion)

            Dim strSql As String = "select * from cubos.DESPIECES where t2tenr='" & hfReferencia.Value & "' " ' and T2KOMP ='" & hfcomponente.Value & "'"
            Dim cn As New OleDb.OleDbConnection()
            Dim dr As OleDb.OleDbDataReader
            cn.ConnectionString = ConfigurationManager.ConnectionStrings("AS400").ConnectionString
            Dim cm As New OleDb.OleDbCommand(strSql, cn)
            cm.CommandTimeout = 30
            cn.Open()
            dr = cm.ExecuteReader()
            Dim plantas As String = ""

            Dim cont As Integer = -1
            listaType.Clear()
            listaTypeCond.Clear()
            Dim referencia As String = ""
            Dim desc_ref As String = ""
            Dim padre As String = ""
            Dim desc_comp As String = ""
            Dim componente As String = ""
            Dim GM As String = ""
            Dim Nivel As Integer = 0
            Dim Cantidad As Integer = 0
            Dim Tipo As Integer = 0

            While dr.Read
                Nivel = If(dr("T2NIVL") Is Nothing OrElse dr("T2NIVL") Is DBNull.Value, Nivel, dr("T2NIVL"))
                desc_ref = If(dr("T2BEZ1") Is Nothing OrElse dr("T2BEZ1") Is DBNull.Value, desc_ref, dr("T2BEZ1"))

                desc_comp = If(dr("T2DENO") Is Nothing OrElse dr("T2DENO") Is DBNull.Value, desc_comp, dr("T2DENO"))
                componente = If(dr("T2KOMP") Is Nothing OrElse dr("T2KOMP") Is DBNull.Value, componente, dr("T2KOMP"))
                If componente = hfcomponente.Value Then
                    txtDescref.Text = desc_comp
                End If
                padre = If(dr("T2BGNR") Is Nothing OrElse dr("T2BGNR") Is DBNull.Value, "sin padre", dr("T2BGNR"))
                If padre = hfcomponente.Value Then

                    Dim item As New KaplanLib.ELL.Xpert
                    cont = cont + 1



                    referencia = If(dr("t2tenr") Is Nothing OrElse dr("t2tenr") Is DBNull.Value, referencia, dr("t2tenr"))

                    desc_comp = If(dr("T2DENO") Is Nothing OrElse dr("T2DENO") Is DBNull.Value, desc_comp, dr("T2DENO"))
                    componente = If(dr("T2KOMP") Is Nothing OrElse dr("T2KOMP") Is DBNull.Value, componente, dr("T2KOMP"))
                    Cantidad = If(dr("T2NPZA") Is Nothing OrElse dr("T2NPZA") Is DBNull.Value, Cantidad, dr("T2NPZA"))
                    GM = If(dr("T2MAGR") Is Nothing OrElse dr("T2MAGR") Is DBNull.Value, GM, dr("T2MAGR"))
                    Tipo = If(dr("T2TAR2") Is Nothing OrElse dr("T2TAR2") Is DBNull.Value, Tipo, dr("T2TAR2"))


                    item.referencia = referencia
                    'item.Id = componente

                    item.desc_ref = desc_ref
                    item.desc_comp = desc_comp
                    item.GM = GM
                    item.componente = componente
                    item.padre = padre
                    item.cantidad = Cantidad
                    listaType.Add(item)

                End If

            End While
            dr.Close()
            cn.Close()
            cn.Dispose()

            'referencia, componente, work, step, caracacteristicas, y hijos
            'hfReferencia.Value, hfcomponente.Value, 6666,  DdlSteps.selectedvalue, .. + TxtCaracteristicas.text, hijos
            Dim hijos As String = ""
            For i = 0 To gvType.Rows.Count - 1

                Dim CheckBoxElim As CheckBox = CType(gvType.Rows(i).FindControl("chkMarcado"), CheckBox)

                If CheckBoxElim.Checked = 0 Then
                Else

                    hijos = hijos & listaType(i).componente & ";"
                End If

            Next





            Dim tipo2 As New KaplanLib.ELL.Asociacion With {
                    .Process = CInt(hfwork.Value),
                    .Steps = CInt(hfstep.Value),
                    .referencia = hfReferencia.Value,
                    .componente = hfcomponente.Value,
                    .cont = CInt(hfcont.Value),
                    .Textolibre = TxtCaracteristicas3.Text,
                    .TrabajoSTD = "",
                    .Parametro = "",
                    .tTipoC = "",
                    .tClaseC = "",
                     .Textolibre2 = "",
                      .Textolibre3 = "",
                       .origen = PageBase.Origen,
            .Hijos = hijos
                    }

            'cont, refer, compon, proces, step
            'tipo2.cont = PageBase.plantaAdmin  CONT ES EL QUE VIENE, LE DARE EL MISMO VALOR
            'AQUI DEBO BORRAR LOS REGISTROS DE ES CONT REF ........
            oDocumentosBLL.GuardarOpeBorrar(tipo2)
            For i = 0 To gvType2.Rows.Count - 1
                'las condiciones las tengo que sacar de sus campos
                Dim campo1 As TextBox = DirectCast(gvType2.Rows(i).FindControl("TrabajoSTDa"), TextBox)
                Dim campo2 As TextBox = DirectCast(gvType2.Rows(i).FindControl("Parametroa"), TextBox)
                Dim campo3 As TextBox = DirectCast(gvType2.Rows(i).FindControl("txtEspecificaciona"), TextBox)
                Dim campo4 As TextBox = DirectCast(gvType2.Rows(i).FindControl("txtClasea"), TextBox)
                Dim campo5 As TextBox = DirectCast(gvType2.Rows(i).FindControl("txtMaximo2a"), TextBox)
                Dim campo6 As TextBox = DirectCast(gvType2.Rows(i).FindControl("txtMinimo2a"), TextBox)

                Dim item2 As New KaplanLib.ELL.Asociacion
                item2.Textolibre2 = campo1.Text
                item2.Textolibre3 = campo2.Text
                item2.Condicion3 = campo3.Text
                item2.Condicion4 = campo4.Text
                item2.Condicion5 = campo5.Text
                item2.Condicion6 = campo6.Text
                listaTypeCond.Add(item2)


                'oDocumentosBLL.GuardarOpe(tipo2)
            Next


            'leer todas las caracteristicas
            'gvType2.EditIndex = -1
            'BindDataView2() '2(0)
            'gvType3.EditIndex = -1
            'BindDataView3() '2(0)
            For i = 0 To gvType2.Rows.Count - 1

                Dim label1 As Label = CType(gvType2.Rows(i).FindControl("lblCaracte"), Label)
                tipo2.Caracteristica = label1.Text
                Dim label2 As Label = CType(gvType2.Rows(i).FindControl("lblDescripcion"), Label)
                tipo2.Caracteristica2 = label2.Text
                Dim label3 As Label = CType(gvType2.Rows(i).FindControl("lblClase"), Label)
                'sacar el valor de  label3.Text o escribir su valor?
                tipo2.tTipoC = label3.Text
                Dim label4 As Label = CType(gvType2.Rows(i).FindControl("lblTipo"), Label)
                tipo2.tClaseC = label4.Text
                Dim label5 As Label = CType(gvType2.Rows(i).FindControl("lblMax"), Label)
                Dim label6 As Label = CType(gvType2.Rows(i).FindControl("lblMin"), Label)
                If IsNumeric(label5.Text) Then
                Else
                    label5.Text = "0"
                End If
                If IsNumeric(label6.Text) Then
                Else
                    label6.Text = "0"
                End If
                tipo2.Max = CDbl(label5.Text)

                tipo2.Min = CDbl(label6.Text)


                tipo2.Condicion1 = listaTypeCond(i).Textolibre2
                tipo2.Condicion2 = listaTypeCond(i).Textolibre3
                tipo2.Condicion3 = listaTypeCond(i).Condicion3
                tipo2.Condicion4 = listaTypeCond(i).Condicion4
                tipo2.Condicion5 = listaTypeCond(i).Condicion5
                tipo2.Condicion6 = listaTypeCond(i).Condicion6

                oDocumentosBLL.GuardarOpe(tipo2)
            Next

            'leer todas las caracteristicas




            For i = 0 To gvType3.Rows.Count - 1

                Dim label1 As Label = CType(gvType3.Rows(i).FindControl("lblDescripcionp"), Label)
                tipo2.Caracteristica = label1.Text
                Dim label2 As Label = CType(gvType3.Rows(i).FindControl("lblClasep"), Label)
                tipo2.Caracteristica2 = label2.Text
                Dim label3 As Label = CType(gvType3.Rows(i).FindControl("lblCaractere"), Label)
                tipo2.TrabajoSTD = label3.Text
                Dim label4 As Label = CType(gvType3.Rows(i).FindControl("lblTipop"), Label)
                tipo2.Parametro = label4.Text
                Dim label5 As Label = CType(gvType3.Rows(i).FindControl("lblMaxp"), Label)
                Dim label6 As Label = CType(gvType3.Rows(i).FindControl("lblMinp"), Label)
                If IsNumeric(label5.Text) Then
                Else
                    label5.Text = "0"
                End If
                If IsNumeric(label6.Text) Then
                Else
                    label6.Text = "0"
                End If
                tipo2.Max = CDbl(label5.Text)

                tipo2.Min = CDbl(label6.Text)
                tipo2.TipoC = 999


                oDocumentosBLL.GuardarOpe(tipo2)
            Next





            'grabar de 1 a n refern, compo, proces, step, hijos,     tipoC = 999 si no es carac

            'If (oDocumentosBLL.GuardarOpe(tipo2)) Then

            Master.MensajeInfo = ItzultzaileWeb.Itzuli("Saved")
            PageBase.plantaAdmin = PageBase.plantaAdmin + 1


            Response.Redirect("~/Calculo.aspx?idRef=" & hfReferencia.Value & "&idComp=" & hfcomponente.Value, False)


        Catch ex As Exception
            Master.MensajeError = "error"

        End Try

    End Sub


    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Try

            Response.Redirect("~/Referencia.aspx?idEmp=" & hfReferencia.Value, False)

        Catch ex As Exception
            Master.MensajeError = ItzultzaileWeb.Itzuli("Error")
        End Try


    End Sub



    Protected Sub calculo_Click(sender As Object, e As EventArgs) Handles btnCalculo.Click
        Try 'voy a avisar que grabe, no hago poner el codigo de btnGuardarNuevaSolicitud y ademas redirect


            Response.Redirect("~/Calculo.aspx?idRef=" & hfReferencia.Value & "&idComp=" & hfcomponente.Value, False)
        Catch ex As Exception
            Master.MensajeError = ItzultzaileWeb.Itzuli("Error")
        End Try
    End Sub

    ''' <summary>
    ''' RowCommand del gridview
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvType2_OnRowCommand(sender As Object, e As GridViewCommandEventArgs)

    End Sub
    Protected Sub gvType3_OnRowCommand(sender As Object, e As GridViewCommandEventArgs)

    End Sub
    Protected Sub gvType3_RowEditing(sender As Object, e As GridViewEditEventArgs)
        gvType3.EditIndex = e.NewEditIndex
        BindDataView3()
        btnQuitarSeleccionadosx.Focus()

    End Sub

    Protected Sub gvType2_RowEditing(sender As Object, e As GridViewEditEventArgs)
        gvType2.EditIndex = e.NewEditIndex
        BindDataView2()
        btnQuitarSeleccionados.Focus()

    End Sub

    'Protected Sub btnEliminar2a_Click(sender As Object, e As EventArgs) Handles btnEliminar2a.Click
    '    Try

    '        Master.MensajeInfo = ("Se ha añadido correctamente").ToUpper

    '    Catch ex As Exception

    '        Master.MensajeError = "error"

    '    End Try
    'End Sub
    Protected Sub btnQuitarSeleccionadosx_Click(sender As Object, e As EventArgs) Handles btnQuitarSeleccionadosx.Click

        Try

            Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL

            Dim lista2 As List(Of KaplanLib.ELL.Kaplan)
            lista2 = oDocBLL.CargarTiposEmpresa(CInt(hfstep.Value), 0)

            Dim lista As List(Of KaplanLib.ELL.Kaplan)
            lista = oDocBLL.CargarTiposEmpresaStep(CInt(hfstep.Value), lista2(0).Proceso)

            If IsNumeric(txtMaximo2.Text) Then
                txtMaximo2.Text = txtMaximo2.Text * 1000
            Else
                txtMaximo2.Text = "0"
            End If
            If IsNumeric(txtMinimo2.Text) Then
                txtMinimo2.Text = txtMinimo2.Text * 1000
            Else
                txtMinimo2.Text = "0"
            End If
            Dim tipo2 As New KaplanLib.ELL.Asociacion With {
               .Process = CInt(hfwork.Value),
                    .Steps = CInt(hfstep.Value),
                    .referencia = hfReferencia.Value,
                    .componente = hfcomponente.Value,
                    .cont = CInt(hfcont.Value),
                        .TipoC = 999,
                         .ClaseC = 0,
                   .Caracteristica2 = txtClase.Text,
                   .Max = CDbl(txtMaximo2.Text),
                       .Min = CDbl(txtMinimo2.Text),
                           .TrabajoSTD = TrabajoSTD.Text,
                               .Parametro = Parametro.Text,
                               .Textolibre = TxtCaracteristicas3.Text,
                               .origen = PageBase.Origen,
                .Caracteristica = txtEspecificacion.Text
                        }

            oDocumentosBLL.GuardarOpe(tipo2)



            BindDataView3()
            btnQuitarSeleccionadosx.Focus()
            Master.MensajeInfo = ("Se ha añadido correctamente").ToUpper

        Catch ex As Exception

            Master.MensajeError = "error"

        End Try
    End Sub

    Protected Sub btnQuitarSeleccionados_Click(sender As Object, e As EventArgs) Handles btnQuitarSeleccionados.Click

        Try

            Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL

            Dim lista2 As List(Of KaplanLib.ELL.Kaplan)
            lista2 = oDocBLL.CargarTiposEmpresa(CInt(hfstep.Value), 0)

            Dim lista As List(Of KaplanLib.ELL.Kaplan)
            lista = oDocBLL.CargarTiposEmpresaStep(CInt(hfstep.Value), lista2(0).Proceso)

            If IsNumeric(txtMax.Text) Then
                txtMax.Text = txtMax.Text * 1000
            Else
                txtMax.Text = "0"
            End If
            If IsNumeric(txtMin.Text) Then
                txtMin.Text = txtMin.Text * 1000
            Else
                txtMin.Text = "0"
            End If
            If IsNumeric(hfTipoC.Value) Then
                hfTipoC.Value = hfTipoC.Value
            Else
                hfTipoC.Value = "0"
            End If
            If IsNumeric(hfClaseC.Value) Then
                hfClaseC.Value = hfClaseC.Value
            Else
                hfClaseC.Value = "0"
            End If
            Dim borrar As Integer = 0
            borrar = hfClaseC.Value
            Dim tipo2 As New KaplanLib.ELL.Asociacion With {
               .Process = CInt(hfwork.Value),
                    .Steps = CInt(hfstep.Value),
                    .referencia = hfReferencia.Value,
                    .componente = hfcomponente.Value,
                    .cont = CInt(hfcont.Value),
                        .TipoC = hfTipoC.Value,
                         .ClaseC = hfClaseC.Value,
                   .Caracteristica2 = car11.Text,
                   .Max = CDbl(txtMax.Text),
                       .Min = CDbl(txtMin.Text),
                           .TrabajoSTD = "",
                               .Parametro = "",
                               .Textolibre = TxtCaracteristicas3.Text,
                               .origen = PageBase.Origen,
                .Caracteristica = Car1.Text
                        }


            oDocumentosBLL.GuardarOpe(tipo2)  'mirarlo





            BindDataView2()
            btnQuitarSeleccionados.Focus()
            Master.MensajeInfo = ("Se ha añadido correctamente").ToUpper

            '    BindDataView2()
        Catch ex As Exception

            Master.MensajeError = "error"

        End Try
    End Sub

    Sub Guardar()


        Dim i As Int16
        Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
        Dim listaType As New List(Of KaplanLib.ELL.Xpert)

        Dim strSql As String = "select * from cubos.DESPIECES where t2tenr='" & hfReferencia.Value & "' " ' and T2KOMP ='" & hfcomponente.Value & "'"
            Dim cn As New OleDb.OleDbConnection()
        Dim dr As OleDb.OleDbDataReader
        cn.ConnectionString = ConfigurationManager.ConnectionStrings("AS400").ConnectionString
        Dim cm As New OleDb.OleDbCommand(strSql, cn)
        cm.CommandTimeout = 30
        cn.Open()
        dr = cm.ExecuteReader()
        Dim plantas As String = ""

        Dim cont As Integer = -1
        listaType.Clear()
        Dim referencia As String = ""
        Dim desc_ref As String = ""
        Dim padre As String = ""
        Dim desc_comp As String = ""
        Dim componente As String = ""
        Dim GM As String = ""
        Dim Nivel As Integer = 0
        Dim Cantidad As Integer = 0
        Dim Tipo As Integer = 0

        While dr.Read
            Nivel = If(dr("T2NIVL") Is Nothing OrElse dr("T2NIVL") Is DBNull.Value, Nivel, dr("T2NIVL"))
            desc_ref = If(dr("T2BEZ1") Is Nothing OrElse dr("T2BEZ1") Is DBNull.Value, desc_ref, dr("T2BEZ1"))

            desc_comp = If(dr("T2DENO") Is Nothing OrElse dr("T2DENO") Is DBNull.Value, desc_comp, dr("T2DENO"))
            componente = If(dr("T2KOMP") Is Nothing OrElse dr("T2KOMP") Is DBNull.Value, componente, dr("T2KOMP"))
            If componente = hfcomponente.Value Then
                txtDescref.Text = desc_comp
            End If
            padre = If(dr("T2BGNR") Is Nothing OrElse dr("T2BGNR") Is DBNull.Value, "sin padre", dr("T2BGNR"))
            If padre = hfcomponente.Value Then

                Dim item As New KaplanLib.ELL.Xpert
                cont = cont + 1



                referencia = If(dr("t2tenr") Is Nothing OrElse dr("t2tenr") Is DBNull.Value, referencia, dr("t2tenr"))

                desc_comp = If(dr("T2DENO") Is Nothing OrElse dr("T2DENO") Is DBNull.Value, desc_comp, dr("T2DENO"))
                componente = If(dr("T2KOMP") Is Nothing OrElse dr("T2KOMP") Is DBNull.Value, componente, dr("T2KOMP"))
                Cantidad = If(dr("T2NPZA") Is Nothing OrElse dr("T2NPZA") Is DBNull.Value, Cantidad, dr("T2NPZA"))
                GM = If(dr("T2MAGR") Is Nothing OrElse dr("T2MAGR") Is DBNull.Value, GM, dr("T2MAGR"))
                Tipo = If(dr("T2TAR2") Is Nothing OrElse dr("T2TAR2") Is DBNull.Value, Tipo, dr("T2TAR2"))


                item.referencia = referencia
                'item.Id = componente

                item.desc_ref = desc_ref
                item.desc_comp = desc_comp
                item.GM = GM
                item.componente = componente
                item.padre = padre
                item.cantidad = Cantidad
                listaType.Add(item)


            End If

        End While


        'referencia, componente, work, step, caracacteristicas, y hijos
        'hfReferencia.Value, hfcomponente.Value, 6666,  DdlSteps.selectedvalue, .. + TxtCaracteristicas.text, hijos
        Dim hijos As String = ""
        For i = 0 To gvType.Rows.Count - 1

            Dim CheckBoxElim As CheckBox = CType(gvType.Rows(i).FindControl("chkMarcado"), CheckBox)

            If CheckBoxElim.Checked = 0 Then
            Else

                hijos = hijos & listaType(i).componente & ";"
            End If

        Next





        Dim tipo2 As New KaplanLib.ELL.Asociacion With {
                    .Work = CInt(hfwork.Value),
                    .Steps = CInt(hfstep.Value),
                    .referencia = hfReferencia.Value,
                    .componente = hfcomponente.Value,
                           .Caracteristica = DdlCaracterist.SelectedValue,
            .Caracteristica2 = TxtCaracteristicas.Text,
            .origen = PageBase.Origen,
            .Hijos = hijos
                    }
        oDocumentosBLL.GuardarOpe(tipo2)







    End Sub





    Protected Sub BindDataView2()
        Try

            Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            Dim listaType As List(Of KaplanLib.ELL.Asociacion)
            Dim lista As List(Of KaplanLib.ELL.Kaplan)
            Dim listaType3 As List(Of KaplanLib.ELL.Kaplan)

            listaType = oDocBLL.loadListStepsChartVV(CInt(hfcont.Value), hfReferencia.Value, hfcomponente.Value, CInt(hfwork.Value), CInt(hfstep.Value))

            For i = 0 To listaType.Count - 1
                listaType(i).Max = listaType(i).Max / 1000
                listaType(i).Min = listaType(i).Min / 1000


                If listaType(i).tClaseC <> "" Then
                    listaType(i).componente = listaType(i).tClaseC
                End If
                If listaType(i).tTipoC <> "" Then
                    listaType(i).referencia = listaType(i).tTipoC
                End If


                'Dim campo1 As TextBox = DirectCast(gvType2.Rows(0).FindControl("TrabajoSTDa"), TextBox)
                If listaType(i).Max = 0 Then
                    listaType(i).Textolibre = ""
                Else
                    listaType(i).Textolibre = "Se ha superado el valor máximo de " & listaType(i).Max
                End If
                If listaType(i).Min = 0 Then
                    listaType(i).Textolibre2 = ""
                Else
                    listaType(i).Textolibre2 = "No llega al valor mínimo de " & listaType(i).Min
                End If

            Next
            'si existen datos en tTipoC o tclaseC se cambian , los he modificado 
            'For i = 0 To listaType.Count - 1
            '    If listaType(i).tClaseC <> "" Then
            '        listaType(i).componente = listaType(i).tClaseC
            '    End If
            '    If listaType(i).tTipoC <> "" Then
            '        listaType(i).referencia = listaType(i).tTipoC
            '    End If

            'Next

            If (listaType.Count > 0) Then
                gvType2.DataSource = listaType
                gvType2.DataBind()
                gvType2.Caption = String.Empty
            Else
                gvType2.DataSource = Nothing
                gvType2.DataBind()
                gvType2.Caption = "No hay registros"
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar un documento", ex)
        End Try
    End Sub




    Protected Sub BindDataView3()
        Try

            Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            Dim listaType As List(Of KaplanLib.ELL.Asociacion)
            listaType = oDocBLL.loadListStepsChar2tVV(CInt(hfcont.Value), hfReferencia.Value, hfcomponente.Value, CInt(hfwork.Value), CInt(hfstep.Value))

            For i = 0 To listaType.Count - 1
                listaType(i).Max = listaType(i).Max / 1000
                listaType(i).Min = listaType(i).Min / 1000
            Next


            If (listaType.Count > 0) Then
                gvType3.DataSource = listaType
                gvType3.DataBind()
                gvType3.Caption = String.Empty
            Else
                gvType3.DataSource = Nothing
                gvType3.DataBind()
                gvType3.Caption = "No hay registros"
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error", ex)
        End Try
    End Sub

End Class