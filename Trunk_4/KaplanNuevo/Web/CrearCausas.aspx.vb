
Imports System.Data


Public Class CrearCausas
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
        BindDataView2()
        BindDataView3()
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
            'Dim i As Integer
            Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            Dim listaType As List(Of KaplanLib.ELL.Asociacion)
            'poner los valores de efectos
            listaType = oDocBLL.CargarListaAsociaciones6(CInt(Request.QueryString("idRef")), CInt(Request.QueryString("idCond")))
            If listaType.Count > 0 Then
                NomEmpf.Text = listaType(0).Condicion3
                NomEmpf2.Text = listaType(0).Condicion4
                NomEmpf3.Text = listaType(0).Condicion5
                DdlPrimero.SelectedValue = listaType(0).TipoC
                DdlSegundo.SelectedValue = listaType(0).ClaseC
                DdlTercero.SelectedValue = listaType(0).cont
            End If
            'Dim listaType2 As List(Of KaplanLib.ELL.Kaplan)
            'If hfEmpresa.Value <> "" Then
            '    listaType = oDocBLL.CargarListaEmpresastexto(PageBase.plantaAdmin, hfEmpresa.Value)
            'Else
            '    listaType = oDocBLL.CargarListaEmpresas(PageBase.plantaAdmin)
            'End If
            'For i = 0 To listaType.Count - 1
            '    If listaType(i).Work > 0 Then
            '        listaType2 = oDocBLL.loadListEmpresas4(listaType(i).Work)
            '        listaType(i).textolibre2 = listaType2(0).Nombre
            '    End If
            'Next


            'If (listaType.Count > 0) Then
            '    gvType.DataSource = listaType
            '    gvType.DataBind()
            '    gvType.Caption = String.Empty
            'Else
            '    gvType.DataSource = Nothing
            '    gvType.DataBind()
            '    gvType.Caption = "No hay registros"
            'End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error", ex)
        End Try
    End Sub



    Protected Sub BindDataView3()
        Try
            'Calcular()
            Dim listaType2 As List(Of KaplanLib.ELL.Kaplan)
            Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            Dim listaType As List(Of KaplanLib.ELL.Asociacion)
            listaType = oDocBLL.CargarListaAsociaciones6Detec(CInt(Request.QueryString("idRef")), CInt(Request.QueryString("idCond")))

            For i = 0 To listaType.Count - 1
                'busca listaType(i).causa el texto o modo_fallo
                listaType2 = oDocBLL.loadListCausas(listaType(i).TipoC)

                If listaType(i).TipoC = 0 Then
                    listaType(i).Condicion2 = "Modo_Fallo"
                Else
                    listaType(i).Condicion2 = "Causa: " & listaType2(0).Nombre
                End If
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



    Protected Sub BindDataView2()
        Try

            'grabar lo que hay en Car1 en tabla causas con idasociacion y idcondicion
            Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            Dim identificador As Integer = 0
            Dim listaType3 As List(Of KaplanLib.ELL.Asociacion)
            listaType3 = oDocBLL.CargarListaAsociaciones7(CInt(Request.QueryString("idRef")), CInt(Request.QueryString("idCond")))
            If listaType3.Count > 0 Then
                identificador = listaType3(0).Id
            End If
            Dim listaType As List(Of KaplanLib.ELL.Asociacion)
            Dim listaType2 As List(Of KaplanLib.ELL.Asociacion)
            listaType2 = oDocBLL.CargarListaAsociaciones6(CInt(Request.QueryString("idRef")), CInt(Request.QueryString("idCond")))
            Dim ATipoC2 As List(Of KaplanLib.ELL.Kaplan)
            ATipoC2 = oDocBLL.CargarListaCausas777("0", "0", "0", identificador)

            ddlCausas2.Items.Clear()
            Dim liblanco As New ListItem("", 0)
            ddlCausas2.Items.Add(liblanco)
            For i = 0 To ATipoC2.Count - 1
                Dim liresponsable As New ListItem(ATipoC2(i).Descripcion, ATipoC2(i).Id)
                ddlCausas2.Items.Add(liresponsable)
            Next

            listaType = oDocBLL.CargarListaAsociaciones6(CInt(Request.QueryString("idRef")), CInt(Request.QueryString("idCond")))
            ddlCausas.Items.Clear()
            ddlCausas.Items.Add(liblanco)
            For i = 0 To listaType.Count - 1
                Dim liresponsable As New ListItem(listaType(i).Condicion1, listaType(i).Id)
                ddlCausas.Items.Add(liresponsable)

                'hay que calculare por cada causa
                Calcular(listaType(i).Id)
            Next


            'Dim listaType As New List(Of KaplanLib.ELL.Asociacion)
            listaType = oDocBLL.CargarListaAsociaciones6(CInt(Request.QueryString("idRef")), CInt(Request.QueryString("idCond")))


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
            Throw New SabLib.BatzException("Error", ex)
        End Try
    End Sub


    Protected Sub Calcular(ByVal id As Integer)
        Try
            'leo causas y deteccion con 2 ids y saco su valor maximo de Detectabilidad y Ocurrencias y leo 
            'DdlPrimero DdlSegundo DdlTercero su mayor valor y con eso voy a calcular
            Dim valor1 As Integer = 0
            Dim valor2 As Integer = 0
            Dim valor2tmp As Integer = 0
            Dim valor3 As Integer = 0
            Dim Resultado As String = ""
            Dim Resultado2 As String = "L"
            If CInt(DdlPrimero.SelectedValue) > CInt(DdlSegundo.SelectedValue) Then
                If CInt(DdlPrimero.SelectedValue) > CInt(DdlTercero.SelectedValue) Then
                    valor1 = CInt(DdlPrimero.SelectedValue)
                Else
                    valor1 = CInt(DdlTercero.SelectedValue)
                End If
            Else
                If CInt(DdlSegundo.SelectedValue) > CInt(DdlTercero.SelectedValue) Then
                    valor1 = CInt(DdlSegundo.SelectedValue)
                Else
                    valor1 = CInt(DdlTercero.SelectedValue)
                End If
            End If

            Dim listaType2 As List(Of KaplanLib.ELL.Asociacion)
            Dim listaType3 As List(Of KaplanLib.ELL.Asociacion)
            Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            Dim listaType As List(Of KaplanLib.ELL.Asociacion)
            Dim listaTypetmp As List(Of KaplanLib.ELL.Asociacion)

            listaTypetmp = oDocBLL.CargarListaAsociaciones6(CInt(Request.QueryString("idRef")), CInt(Request.QueryString("idCond")))
            listaType = oDocBLL.CargarListaAsociaciones6(CInt(Request.QueryString("idRef")), CInt(Request.QueryString("idCond")))

            For i = 0 To listaType.Count - 1
                If listaType(i).Id = id Then 'solo si es la causa en concreto

                    valor2 = listaType(i).Work

                    listaType2 = oDocBLL.CargarListaAsociaciones6Detec2(listaType(i).Id)
                    If listaType2.Count > 0 Then
                        'valor2 = listaType2(0).Work
                        'si hay menos MF lo cambio
                        listaType3 = oDocBLL.CargarListaAsociaciones6Detec3(CInt(Request.QueryString("idRef")), CInt(Request.QueryString("idCond")))
                        If listaType3.Count > 0 Then
                            'If listaType3(0).Work < valor2 Then
                            'valor2 = listaType3(0).Work
                            valor3 = listaType3(0).Work
                            'End If

                        Else
                            valor3 = 10
                        End If
                    Else
                        'MF
                        listaType3 = oDocBLL.CargarListaAsociaciones6Detec3(CInt(Request.QueryString("idRef")), CInt(Request.QueryString("idCond")))
                        If listaType3.Count > 0 Then
                            valor3 = listaType3(0).Work
                        Else
                            valor3 = 10 'no tiene control deteccion ni en una causa cualquiera ni en MF 
                        End If
                    End If

                    'actualizar tabla causas cada linea (texto causa viene en listaType(i).Condicion1
                    Resultado = CalcularLetra(valor1, valor2, valor3)
                    Dim tipo2 As New KaplanLib.ELL.Asociacion With {
                    .Process = CInt(Request.QueryString("idCond")),
                    .Textolibre = listaType(i).Condicion1,
                     .Textolibre2 = Resultado
                        }
                    oDocumentosBLL.ModificarEmpTotal2Efectos2(tipo2, CInt(Request.QueryString("idRef")))

                    'fin 'actualizar tabla causas cada linea

                    If i > 0 Then
                        If valor1 < listaTypetmp(i - 1).Work Then
                            valor1 = listaTypetmp(i - 1).Work
                        End If
                        If valor2 < listaTypetmp(i - 1).ClaseC Then
                            valor2 = listaTypetmp(i - 1).ClaseC
                        End If
                        If valor3 < listaTypetmp(i - 1).cont Then
                            valor3 = listaTypetmp(i - 1).cont
                        End If
                    End If
                    listaTypetmp(i).Work = valor1
                    listaTypetmp(i).ClaseC = valor2
                    listaTypetmp(i).cont = valor3



                End If
            Next
            'en la tabla causas estan todas las lineas si son h m  h, mirarlo en Textolibre2
            For i = 0 To listaType.Count - 1
                Resultado = listaType(i).Textolibre2
                If Resultado = "H" Then
                    Resultado2 = "H"
                End If
                If Resultado = "M" And Resultado2 <> "H" Then
                    Resultado2 = "M"
                End If
            Next


            Resultado = Resultado2 ' 'CalcularLetra(valor1, valor2, valor3)
            If Resultado = "H" Then
                imgEstado1.Visible = False
                imgEstado2.Visible = False
                imgEstado3.Visible = True
            End If
            If Resultado = "M" Then
                imgEstado1.Visible = False
                imgEstado2.Visible = True
                imgEstado3.Visible = False
            End If
            If Resultado = "L" Then
                imgEstado1.Visible = True
                imgEstado2.Visible = False
                imgEstado3.Visible = False
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error", ex)
        End Try
    End Sub


    Private Function CalcularLetra(ByVal valor1 As Integer, ByVal valor2 As Integer, ByVal valor3 As Integer) As String
        Dim tmp1 As Integer = Integer.MinValue
        Dim resul As String = "L"
        If valor1 > 8 Then
            If valor2 > 3 Then
                resul = "H"
                If valor3 = 1 Then 'excepcion
                    resul = "M"
                End If
            End If
        End If

        If valor1 > 8 Then
            If valor2 < 4 Then
                resul = "L"
                If valor3 > 4 Then
                    resul = "M"
                End If
                If valor3 > 6 Then
                    resul = "H"
                End If
            End If
        Else 'no > 8

            If valor1 > 6 Then
                If valor2 > 5 Then
                    resul = "H"
                    If valor3 = 1 Then
                        resul = "M"
                    End If
                Else
                    If valor2 > 3 Then
                        resul = "M"
                        If valor3 > 6 Then
                            resul = "H"
                        End If
                    Else
                        resul = "L"
                        If valor3 > 4 Then
                            resul = "M"
                        End If
                    End If
                End If
            Else 'no > 6
                If valor2 > 7 Then
                    resul = "M"
                    If valor3 > 4 Then
                        resul = "H"
                    End If
                Else
                    If valor2 > 5 Then
                        resul = "M"
                        If valor3 = 1 Then
                            resul = "L"
                        End If
                    Else
                        resul = "L"
                        If valor2 > 3 And valor3 > 6 Then
                            resul = "M"
                        End If
                    End If
                End If


                If valor1 > 3 Then
                    If valor2 > 7 Then
                        resul = "M"
                        If valor3 > 4 Then
                            resul = "H"
                        End If
                    Else
                        If valor2 > 5 Then
                            resul = "M"
                            If valor3 = 1 Then
                                resul = "L"
                            End If
                        Else
                            If valor2 > 3 Then
                                resul = "L"
                                If valor3 > 6 Then
                                    resul = "M"
                                End If
                            Else
                                resul = "L"
                            End If
                        End If
                    End If


                Else ' la ultima hoja
                    resul = "L"
                    If valor2 > 7 Then
                        resul = "M"
                        If valor3 < 5 Then
                            resul = "L"
                        End If
                    End If
                End If
            End If


        End If









        Return resul
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
            ComprobarAcceso()
            'BindDataView2()
            'hfwork.Value = CifEmp.Text
            'hfstep.Value = DescWork.Text

            'sacar los datos del registro


            Dim listaAsociacionFallo As New List(Of KaplanLib.ELL.Asociacion)
            listaAsociacionFallo = oDocBLL.CargarListaAsociaciones5(CInt(Request.QueryString("idRef")))
            If Request.QueryString("idCond") = "1" Then
                NomEmp.Text = listaAsociacionFallo(0).Condicion1
            End If
            If Request.QueryString("idCond") = "2" Then
                NomEmp.Text = listaAsociacionFallo(0).Condicion2
            End If
            If Request.QueryString("idCond") = "3" Then
                NomEmp.Text = listaAsociacionFallo(0).Condicion3
            End If
            If Request.QueryString("idCond") = "4" Then
                NomEmp.Text = listaAsociacionFallo(0).Condicion4
            End If
            If Request.QueryString("idCond") = "5" Then
                NomEmp.Text = listaAsociacionFallo(0).Condicion5
            End If
            If Request.QueryString("idCond") = "6" Then
                NomEmp.Text = listaAsociacionFallo(0).Condicion6
            End If
            hfcomponente.Value = listaAsociacionFallo(0).componente
            hfReferencia.Value = listaAsociacionFallo(0).referencia
            'fin sacar los datos del registro

            If Not (Page.IsPostBack) Then


                'rellenar modal
                Dim liPreventivavacio As New ListItem("", 0)

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
                ATipoC2 = oDocBLL.CargarListaCausas777(listaAsociacionFallo(0).referencia, listaAsociacionFallo(0).componente, listaAsociacionFallo(0).Work, listaAsociacionFallo(0).Steps)

                If ATipoC.Count > 0 Then


                    For Each Acaracteristic2 In ATipoC2
                        'Acaracteristic2.Nombre = "Causa:" & Acaracteristic2.Nombre & ",  Valor: " & Acaracteristic2.textolibre & " -------------------  Control P.= " & Acaracteristic2.Descripcion & ",  Valor: " & Acaracteristic2.textolibre2 & "  "
                        Dim Aliresponsable22 As New ListItem(Acaracteristic2.Nombre, Acaracteristic2.Id)
                        ClaseC.Items.Add(Aliresponsable22)

                    Next

                End If

                'fin modal




                Dim s As String
                s = Request.QueryString("id")
                If s = "0" Then
                    mView.ActiveViewIndex = 0
                    txtEmpresa.Focus()

                End If
                If s = "1" Then
                    mView.ActiveViewIndex = 2

                    ''''''''''''         DdlSubcontrata 
                    Dim liresponsablevacio As New ListItem("No es subcontrata", 0)

                    '''''''''''''         DdlSubcontrata2.Items.Add(liresponsablevacio)

                    Dim listaEmpr As List(Of KaplanLib.ELL.Preventiva)
                    listaEmpr = oDocBLL.CargarListaPre(PageBase.plantaAdmin)     'procesos

                    If listaEmpr.Count > 0 Then
                        DdlPreventiva2.Items.Clear()
                        For Each responsable In listaEmpr
                            Dim liresponsable As New ListItem(responsable.Nombre, responsable.Id)

                            DdlPreventiva2.Items.Add(liresponsable)

                        Next
                    End If


                End If
                If s = "2" Then
                    mView.ActiveViewIndex = 3
                End If


                Initialize()
                CargarDocumentos()
            End If
            mView.ActiveViewIndex = 1




        Catch ex As Exception
            Throw New SabLib.BatzException("Error ", ex)
        End Try
    End Sub





    Private Sub CargarDocumentos()
        Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
        Dim lista As List(Of KaplanLib.ELL.Kaplan)

        lista = oDocBLL.CargarListaEmpresas(PageBase.plantaAdmin)
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

                Dim Iddoc As Int32 = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).id

                BindDataView()   'para limpiar el grid
                CargarDetalle(Iddoc)

            End If


            If e.CommandName = "Desactivar" Then
                Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
                Dim Iddoc As Int32 = CInt(strDoc)

                Dim TipoTra As New KaplanLib.ELL.Kaplan With {.Obsoleto = 1, .Id = Iddoc}
                If (oDocumentosBLL.ModificarEmp(TipoTra)) Then

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("STEP disabled").ToUpper
                    BindDataView()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se desactivaba la empresa").ToUpper
                End If

            End If
            If e.CommandName = "Activar" Then
                Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
                Dim Iddoc As Int32 = CInt(strDoc)

                Dim TipoTra As New KaplanLib.ELL.Kaplan With {.Obsoleto = 0, .Id = Iddoc}
                If (oDocumentosBLL.ModificarEmp(TipoTra)) Then

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("STEP activated").ToUpper
                    BindDataView()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Error").ToUpper
                End If

            End If


        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar un documento", ex)
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
    ''' <param name="idStep"></param>
    ''' <remarks></remarks>
    Private Sub ConfiguracionProduct(ByVal idStep As Integer, Optional ByVal idType As Integer = 0, Optional ByVal idTransmissionMode As Integer = 0)
        Try
            Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            Dim listaDOC As List(Of KaplanLib.ELL.Documentos)
            Dim lista As List(Of KaplanLib.ELL.Kaplan)
            Dim userBLL As New SabLib.BLL.UsuariosComponent

            'si es nuevo elemento
            If idStep = 0 Then
                'lblNuevaSolicitud.Text = "Creación de una nueva empresa"
                flag_Modificar.Value = "0"
                'listaDOC = oDocBLL.CargarLista(PageBase.plantaAdmin)

                LimpiarCampos()
            Else


                lista = oDocBLL.CargarTiposEmpresa(idStep, PageBase.plantaAdmin)
                'flag_Modificar.Value = idStep

                HdnNombre.Value = lista(0).Nombre
                HdnCIF.Value = lista(0).Descripcion


                NomEmp.Text = lista(0).Nombre
                CifEmp.Text = lista(0).Descripcion
                DescWork.Text = lista(0).textolibre

                Dim listaPRE As List(Of KaplanLib.ELL.Preventiva)
                listaPRE = oDocBLL.CargarListaPre(PageBase.plantaAdmin)

                If listaPRE.Count > 0 Then

                    DdlWork.Items.Clear()
                    For Each responsable In listaPRE
                        Dim liresponsable As New ListItem(responsable.Nombre, responsable.Id)
                        DdlWork.Items.Add(liresponsable)

                    Next

                End If
                If lista(0).Proceso > 0 Then
                    DdlWork.SelectedValue = lista(0).Proceso
                Else
                    DdlWork.SelectedIndex = 0
                End If





                DdlSubcontrata.Items.Clear()
                Dim liPreventivavacio2 As New ListItem("", 0)

                Dim listaPRE2 As List(Of KaplanLib.ELL.Kaplan)
                listaPRE2 = oDocBLL.CargarListaEmpresasWork(PageBase.plantaAdmin)

                If listaPRE2.Count > 0 Then


                    For Each responsable In listaPRE2
                        Dim liresponsable2 As New ListItem(responsable.Nombre, responsable.Id)
                        DdlSubcontrata.Items.Add(liresponsable2)

                    Next

                End If




                DdlSteps.Items.Clear()
                Dim listaPRE3 As List(Of KaplanLib.ELL.Kaplan)
                listaPRE3 = oDocBLL.CargarListaEmpresas(PageBase.plantaAdmin)

                If listaPRE3.Count > 0 Then


                    For Each responsable In listaPRE3 'steps
                        Dim liresponsable As New ListItem(responsable.Nombre, responsable.Id)
                        DdlSteps.Items.Add(liresponsable)

                    Next

                End If
                'If lista(0).Steps > 0 Then
                DdlSteps.SelectedValue = idStep
                If lista(0).Work > 0 Then
                    DdlSubcontrata.SelectedValue = lista(0).Work
                Else
                    DdlSubcontrata.SelectedIndex = 0
                End If
                'DdlSubcontrata.SelectedValue = lista(0).Work   'work

                BindDataView2()
                BindDataView3()

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
            Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL

            Dim tipo2 As New KaplanLib.ELL.Asociacion With {
                .Process = CInt(Request.QueryString("idCond")),
                .Textolibre = NomEmpf.Text,
                 .Textolibre2 = NomEmpf2.Text,
                    .Parametro = NomEmpf3.Text,
                    .cont = DdlPrimero.SelectedValue,
                    .ClaseC = DdlSegundo.SelectedValue,
                    .TipoC = DdlTercero.SelectedValue,
                     .Caracteristica = ClaseC.SelectedItem.[Text],  'car11.Text,
                         .Textolibre3 = ClaseC.SelectedValue ' Car1.Text
                    }
            If (oDocumentosBLL.ModificarEmpTotal2Efectos(tipo2, CInt(Request.QueryString("idRef")))) Then

                Master.MensajeInfo = ItzultzaileWeb.Itzuli("Saved")

            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("Error")
            End If




        Catch ex As Exception

            Throw New SabLib.BatzException("Error", ex)
        End Try

    End Sub


    ''' <summary>
    ''' Guardar un documento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnGrav_Click(sender As Object, e As EventArgs) Handles btnGrav.Click
        Try


            Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            '    ' no usamos valor de  DdlSubcontrata2 mas que para no añadir docs     Dim tipo As New  kaplanlib.ELL.Empresas With {.Planta = PageBase.plantaAdmin, .Nif = txtCIF.Text, .Nombre = txtNombre.Text, .subcontrata = DdlSubcontrata2.SelectedValue}
            Dim tipo As New KaplanLib.ELL.Empresas With {.Planta = PageBase.plantaAdmin, .Nif = txtCIF.Text, .Nombre = txtNombre.Text, .subcontrata = DdlPreventiva2.SelectedValue, .FecEnv = Date.MinValue, .FecRec = Date.MaxValue}
            Dim tipo2 As New KaplanLib.ELL.Kaplan With {
                    .Proceso = DdlPreventiva2.SelectedValue, 'proceso
                    .Obsoleto = 0, 'obsoleto
                    .Descripcion = txtCIF.Text,
             .Nombre = txtNombre.Text
                    }

            'comprobar si existe nif
            'lista = oDocBLL.CargarTiposEmpresaCIF(txtCIF.Text, PageBase.plantaAdmin)
            'If lista.Count > 0 Then



            '    Master.MensajeError = ItzultzaileWeb.Itzuli("Ya existe una empresa con ese Nombre")
            '    '  Master.MensajeInfo = "Ya existe una empresa con ese CIF "

            'Else


            If (oDocumentosBLL.GuardarEmp(tipo2)) Then 'sin commit no actualiza emp para insert emd

                Master.MensajeInfo = ItzultzaileWeb.Itzuli("Step") & " " & txtNombre.Text & " " & ItzultzaileWeb.Itzuli("Saved")

            Else

                Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba la empresa") & " " & txtNombre.Text
            End If

            mView.ActiveViewIndex = 0

            LimpiarCampos()
            Initialize()
            CargarDocumentos()



            'End If

        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba la empresa") & " " & txtNombre.Text
        End Try

    End Sub

    ''' <summary>
    ''' Guardar un documento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnGuardarNuevaReferencia2_Click(sender As Object, e As EventArgs) Handles GrabarVista2.Click
        Try


            Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            '    ' no usamos valor de  DdlSubcontrata2 mas que para no añadir docs     Dim tipo As New  kaplanlib.ELL.Empresas With {.Planta = PageBase.plantaAdmin, .Nif = txtCIF.Text, .Nombre = txtNombre.Text, .subcontrata = DdlSubcontrata2.SelectedValue}
            Dim tipo As New KaplanLib.ELL.Empresas With {.Planta = PageBase.plantaAdmin, .Nif = txtCIF.Text, .Nombre = txtNombre.Text, .subcontrata = DdlPreventiva2.SelectedValue, .FecEnv = Date.MinValue, .FecRec = Date.MaxValue}
            Dim tipo2 As New KaplanLib.ELL.Kaplan With {
                    .Proceso = DdlPreventiva2.SelectedValue, 'proceso
                    .Obsoleto = 0, 'obsoleto
                    .Descripcion = txtCIF.Text,
             .Nombre = txtNombre.Text
                    }

            'comprobar si existe nif
            'lista = oDocBLL.CargarTiposEmpresaCIF(txtCIF.Text, PageBase.plantaAdmin)
            'If lista.Count > 0 Then



            '    Master.MensajeError = ItzultzaileWeb.Itzuli("Ya existe una empresa con ese Nombre")
            '    '  Master.MensajeInfo = "Ya existe una empresa con ese CIF "

            'Else


            If (oDocumentosBLL.GuardarEmp(tipo2)) Then 'sin commit no actualiza emp para insert emd

                Master.MensajeInfo = ItzultzaileWeb.Itzuli("Step") & " " & txtNombre.Text & " " & ItzultzaileWeb.Itzuli("Saved")

            Else

                Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba la empresa") & " " & txtNombre.Text
            End If

            mView.ActiveViewIndex = 0

            LimpiarCampos()
            Initialize()
            CargarDocumentos()



            'End If

        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba la empresa") & " " & txtNombre.Text
        End Try

    End Sub

    Protected Sub btnLimpiarCampos_Click(sender As Object, e As EventArgs) Handles btnLimpiarCampos.Click
        LimpiarCampos()
    End Sub
    ''' <summary>
    ''' Limpia los campos
    ''' </summary>
    Private Sub LimpiarCampos()

    End Sub
    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        LimpiarCampos()
        mView.ActiveViewIndex = 1
        Response.Redirect("~/Calculo.aspx?idRef=" & hfReferencia.Value & "&idComp=" & hfcomponente.Value, False)
    End Sub

    'Protected Sub btnControlP_Click(sender As Object, e As EventArgs) Handles btnControlP.Click
    '    'LimpiarCampos()
    '    mView.ActiveViewIndex = 1
    '    Response.Redirect("~/ControlP.aspx?idCond=" & Request.QueryString("idCond") & "idRef=" & Request.QueryString("idRef"), False)
    'End Sub

    Protected Sub btnCancelar2_Click(sender As Object, e As EventArgs) Handles CancelVista2.Click
        LimpiarCampos()
        mView.ActiveViewIndex = 1
        Response.Redirect("~/Calculo.aspx?idRef=" & hfReferencia.Value & "&idComp=" & hfcomponente.Value, False)
    End Sub
    Protected Sub btnCancelar3_Click(sender As Object, e As EventArgs) Handles CancelVista3.Click
        LimpiarCampos()
        mView.ActiveViewIndex = 1
        Response.Redirect("~/Calculo.aspx?idRef=" & hfReferencia.Value & "&idComp=" & hfcomponente.Value, False)
    End Sub




    Protected Sub btnQuitarSeleccionados_Click(sender As Object, e As EventArgs) Handles btnQuitarSeleccionados.Click

        Try
            Dim prueba As String
            prueba = ddlCausas2.SelectedItem.Text ' Car1.Text  ' es la causa, necesito la asociacion e id condicion ?idCond=1&idRef=3844
            'If ClaseC.SelectedValue = 0 Then
            '    Master.MensajeError = "Selecciona Causa"
            '    Exit Sub
            'End If

            'Dim tipo2 As New KaplanLib.ELL.Asociacion With {
            '    .Process = CInt(Request.QueryString("idCond")),
            '    .Textolibre = NomEmpf.Text,
            '     .Textolibre2 = NomEmpf2.Text,
            '        .Parametro = NomEmpf3.Text,
            '        .cont = DdlPrimero.SelectedValue,
            '        .ClaseC = DdlSegundo.SelectedValue,
            '        .TipoC = DdlTercero.SelectedValue,
            '         .Caracteristica = ClaseC.SelectedItem.[Text],  'car11.Text,
            '             .Textolibre3 = ClaseC.SelectedValue ' Car1.Text
            '        }
            Dim tipo2 As New KaplanLib.ELL.Asociacion With {
                .Process = CInt(Request.QueryString("idCond")),
                .Textolibre = NomEmpf.Text,
                 .Textolibre2 = NomEmpf2.Text,
                    .Parametro = NomEmpf3.Text,
                    .cont = DdlPrimero.SelectedValue,
                    .ClaseC = DdlSegundo.SelectedValue,
                    .TipoC = DdlTercero.SelectedValue,
                     .Caracteristica = prueba,  'car11.Text,
               .Condicion1 = txtCP.Text,
                      .Work = ddlOcu.SelectedValue,
                         .Textolibre3 = ClaseC.SelectedValue ' Car1.Text
                    }
            If (oDocumentosBLL.ModificarEmpTotal2(tipo2, CInt(Request.QueryString("idRef")))) Then

                Master.MensajeInfo = ItzultzaileWeb.Itzuli("Saved")

            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("Error")
            End If






            BindDataView2()


        Catch ex As Exception

            Master.MensajeError = "error"

        End Try
    End Sub


    Protected Sub btnQuitarSeleccionadosx_Click(sender As Object, e As EventArgs) Handles btnQuitarSeleccionadosx.Click

        Try
            Dim tipo2 As New KaplanLib.ELL.Asociacion With {
                .Process = CInt(Request.QueryString("idCond")),
                .Textolibre = TrabajoSTD.Text,
                    .cont = ddlcontrol.SelectedValue,
                      .Work = ddlCausas.SelectedValue
                    }
            If (oDocumentosBLL.ModificarEmpTotal2Metodo(tipo2, CInt(Request.QueryString("idRef")))) Then

                Master.MensajeInfo = ItzultzaileWeb.Itzuli("Saved")

            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("Error")
            End If



            BindDataView3()

            Master.MensajeInfo = ("correcto").ToUpper

        Catch ex As Exception

            Master.MensajeError = "Un error ha ocurrido cuando se asignaban los documentos a la empresa" & " " & txtNombre.Text

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

            If e.CommandName = "Desactivar" Then
                Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
                Dim Iddoc As Int32 = CInt(strDoc)

                Dim TipoTra As New KaplanLib.ELL.Kaplan With {.Obsoleto = 1, .Id = Iddoc}
                If (oDocumentosBLL.UpdateSteps(TipoTra)) Then

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("Characteristic disabled").ToUpper
                    BindDataView3()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se desactivaba").ToUpper
                End If

            End If
            If e.CommandName = "Activar" Then
                Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
                Dim Iddoc As Int32 = CInt(strDoc)

                Dim TipoTra As New KaplanLib.ELL.Kaplan With {.Obsoleto = 1, .Id = Iddoc}
                If (oDocumentosBLL.UpdateDetec(TipoTra)) Then

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("Borrado").ToUpper
                    BindDataView3()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Error").ToUpper
                End If

            End If


        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar un documento", ex)
        End Try

    End Sub


    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' <para>Redirecciona al detalle del item seleccionado</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvType2_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvType2.RowCommand
        Try

            If e.CommandName = "Desactivar" Then
                Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
                Dim Iddoc As Int32 = CInt(strDoc)

                Dim TipoTra As New KaplanLib.ELL.Kaplan With {.Obsoleto = 1, .Id = Iddoc}
                ''''If (oDocumentosBLL.UpdateSteps(TipoTra)) Then

                ''''    Master.MensajeInfo = ItzultzaileWeb.Itzuli("Characteristic disabled").ToUpper
                ''''    BindDataView2()
                ''''Else
                ''''    Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se desactivaba").ToUpper
                ''''End If

            End If
            If e.CommandName = "Activar" Then
                Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
                Dim Iddoc As Int32 = CInt(strDoc)

                Dim TipoTra As New KaplanLib.ELL.Kaplan With {.Obsoleto = 1, .Id = Iddoc}
                If (oDocumentosBLL.UpdateCausa(TipoTra)) Then

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("Causa borrada").ToUpper
                    BindDataView2()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Error").ToUpper
                End If

            End If


        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar un documento", ex)
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
    ''' <summary>
    ''' RowCommand del gridview
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvType3_OnRowCommand(sender As Object, e As GridViewCommandEventArgs)

    End Sub

End Class