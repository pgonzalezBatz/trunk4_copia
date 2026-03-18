Imports iTextSharp.text.pdf
Imports System.Data
Imports System.Net

Imports System.Web.Script.Serialization

Public Class EstadoEmpresaDocumento9
    Inherits PageBase
    'Inherits System.Web.UI.Page
    Dim oDocumentosBLL As New BLL.DocumentosBLL



#Region "METODOS"


    ''' <summary>
    ''' Inicializa las variables y vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Initialize()
        ClearGridView()
        BindDataView()
        '     BindDataView2()
    End Sub

    ''' <summary>
    ''' Limpia los datos vinculados
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ClearGridView()
        gvType.DataSource = Nothing
        gvType.DataBind()
    End Sub

    Protected Sub BindDataViewlimpia()

        Try
            ClearGridView()

            Dim oDocBLL As New BLL.DocumentosBLL
            Dim listaType As List(Of ELL.Empresas)
            If hfEmpresa.Value <> "" Then
                listaType = oDocBLL.CargarListaEmpresastexto(PageBase.plantaAdmin, hfEmpresa.Value)
            Else
                listaType = oDocBLL.CargarListaEmpresasActivas(PageBase.plantaAdmin)
            End If

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
            Throw New SabLib.BatzException("Error al mostrar un documento", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BindDataView()
        Try
            Dim oDocBLL As New BLL.DocumentosBLL
            Dim listaType As List(Of ELL.Empresas)
            Dim contaTrabajadores As Int16 = 0

            If Not (Page.IsPostBack) Then
                If Request.QueryString("hfEmpresa") <> "" Then  'es para el volver
                    hfEmpresa.Value = Request.QueryString("hfEmpresa")

                End If
                If Request.QueryString("pagina") <> "" Then
                    gvType.PageIndex = CInt(Request.QueryString("pagina"))
                End If
            End If
            If hfEmpresa.Value <> "" Then
                listaType = oDocBLL.CargarListaEmpresastexto(PageBase.plantaAdmin, hfEmpresa.Value)
                pnlEmpresasInfo.Visible = False
                Label7.Text = ItzultzaileWeb.Itzuli("Total puestos")
                'Label11.Text = ItzultzaileWeb.Itzuli("Total trabajadores")
            Else
                contaTrabajadores = oDocBLL.loadContaTrabajadores(PageBase.plantaAdmin)(0).Id
                If PerfilUsuario.IdRol = ELL.Roles.RolUsuario.Supervisores Then
                    '  Label11.Visible = False
                    'For i = 0 To 200
                    '    If intDependientes(i) = 0 Then
                    '        Label12.Text = i.ToString
                    '        contaTrabajadores = i
                    '        Exit For
                    '    End If
                    'Next

                    listaType = oDocBLL.CargarListaEmpresasActivasConTraResponsable(PageBase.plantaAdmin, intPuestos) 'intDependientes
                Else

                    listaType = oDocBLL.CargarListaEmpresasActivasConTra(PageBase.plantaAdmin)
                End If

                pnlEmpresasInfo.Visible = True
            End If


            ''''''If hfTra.Value <> "" Then
            ''''''    'jon devuelve trabajador            listaType = oDocBLL.CargarListaTrabajadorestexto(PageBase.plantaAdmin, Split(hfTra.Value, ",")(1), Split(hfTra.Value, ",")(0))
            ''''''    pnlEmpresasInfo.Visible = False
            ''''''    Label7.Text = ItzultzaileWeb.Itzuli("Total puestos")
            ''''''End If

            If (listaType.Count > 0) Then
                Dim listaType2 As List(Of ELL.EmpresasDoc)

                Dim contaerrores2 As Int16
                Dim contaavisos2 As Int16
                Dim contaerrores As Int16
                Dim contaavisos As Int16

                Dim paginado As Integer
                If gvType.PageIndex > 0 Then
                    paginado = listaType.Count - 1
                Else
                    If listaType.Count > 10 Then
                        paginado = 10
                    Else
                        paginado = listaType.Count - 1
                    End If
                End If
                For j = 0 To paginado ' listaType.Count - 1 (quito paginacion para mas rapido, solo calcula 10)
                    listaType(j).recibi = 0 'de momento recibi como error activo como aviso
                    listaType(j).activo = 0

                    contaerrores2 = 0
                    contaavisos2 = 0
                    contaerrores = 0
                    contaavisos = 0

                    listaType2 = oDocBLL.CargarListaEmpDocAsignados(PageBase.plantaAdmin, listaType(j).Id)
                    If (listaType2.Count > 0) Then

                        For i = 0 To listaType2.Count - 1
                            Dim no_correcto As Int16 = 0
                            'estado
                            If listaType2(i).FecRec = Date.MinValue Then
                                no_correcto = 1
                                listaType2(i).estado = ItzultzaileWeb.Itzuli("Dcto. no entregado")

                            Else
                                If listaType2(i).FecRec = Date.MinValue Then
                                    no_correcto = 1
                                    listaType2(i).estado = ItzultzaileWeb.Itzuli("Caducidad no asignada")
                                Else
                                    'si esta puesto incorrecto en la mod del doc se pone asi

                                    If listaType2(i).correcto = 2 Then
                                        no_correcto = 1
                                        listaType2(i).estado = ItzultzaileWeb.Itzuli("No validado")
                                    Else

                                        'fecha de validez si no es de plantilla
                                        If listaType2(i).periodicidad <> 13 And listaType2(i).FecIni = Date.MinValue And listaType2(i).plantilla = 0 Then
                                            no_correcto = 1
                                            listaType2(i).estado = ItzultzaileWeb.Itzuli("Falta fecha inicio de Validez")
                                        Else
                                            no_correcto = 0
                                            listaType2(i).estado = ItzultzaileWeb.Itzuli("Documento correcto")

                                            If no_correcto = 0 And listaType2(i).FecCad < Now And listaType2(i).FecCad <> Date.MinValue Then '"01/01/1900"  CDate(Now.ToShortDateString)
                                                no_correcto = 1
                                                'si es de plantilla
                                                If listaType2(i).plantilla = 1 And listaType2(i).FecCad <> Date.MaxValue Then
                                                    '    no_correcto = 1
                                                    listaType2(i).estado = "Validez caducada por existir nueva plantilla"
                                                Else
                                                    'margen se saca de DOC008
                                                    Dim margen As Integer = 0
                                                    If listaType2(i).Margen > 0 Then
                                                        margen = listaType2(i).Margen
                                                    End If
                                                    If listaType2(i).FecCad < CDate(Now.AddDays(-margen).ToShortDateString) And listaType2(i).FecCad <> Date.MinValue Then
                                                        listaType2(i).estado = ItzultzaileWeb.Itzuli("Validez caducada")
                                                    Else
                                                        listaType2(i).estado = ItzultzaileWeb.Itzuli("Validez caducada, pero dentro de margen")
                                                        no_correcto = 2
                                                    End If
                                                End If


                                            End If


                                        End If

                                    End If

                                    If listaType2(i).correcto = 1 Then
                                        no_correcto = 1
                                        listaType2(i).estado = ItzultzaileWeb.Itzuli("Marcado Incorrecto")

                                    End If

                                End If
                            End If



                            'mostramos un icono u otro en función de si es ok o no, o no es obligatorio
                            If no_correcto = 1 Then

                                listaType2(i).txtcorrecto = "Errores" ' "'btn alert-danger' ><span class='glyphicon glyphicon-check'></span" '"~/App_Themes/Batz/IconosAcciones/error1.gif"
                                listaType(j).recibi = listaType(j).recibi + 1

                            Else
                                If no_correcto = 2 Then
                                    listaType2(i).txtcorrecto = "Avisos" ' "btn  alert-warning' ><span class='glyphicon glyphicon-check'></span" '"~/App_Themes/Batz/IconosAcciones/atencion1.gif"
                                    listaType(j).activo = listaType(j).activo + 1
                                Else
                                    listaType2(i).txtcorrecto = "Correcto" '"'btn  alert-success' ><span class='glyphicon glyphicon-check'></span" '"~/App_Themes/Batz/IconosAcciones/ok1.gif"
                                End If

                            End If


                            'leer si existe el registro en adok_emd cogido en campo ubicacion
                            If listaType2(i).ubicacion = "" Then
                                listaType2(i).ubicacion = "false" 'para quitar el boton
                            Else
                                listaType2(i).ubicacion = "true"
                            End If


                            listaType2(i).nomemp = "true"


                        Next


                        'por cada trabajador
                        'hasta aqui para doc de empresa
                        'para cada trabajador seria:

                        Dim trabajadores As List(Of ELL.Trabajadores)
                        Dim ninguntrabajador As Integer = 0
                        trabajadores = oDocBLL.CargarListaTrabajadoresClaveEmpEstado(PageBase.plantaAdmin, listaType(j).Id)

                        If (trabajadores.Count > 0) Then
                            'desde aqui para errores
                            Dim j2 As Int32
                            Dim i2 As Int32
                            Dim listaType22 As List(Of ELL.TrabajadoresDoc)

                            For j2 = 0 To trabajadores.Count - 1
                                trabajadores(j2).responsable = 0 'de momento responsable como error activo como aviso
                                trabajadores(j2).activo = 0


                                listaType22 = oDocBLL.CargarListaTraDocAsignados(PageBase.plantaAdmin, trabajadores(j2).Id)
                                If (listaType22.Count > 0) Then


                                    For i2 = 0 To listaType22.Count - 1
                                        Dim no_correcto2 As Int16 = 0
                                        'estado
                                        If listaType22(i2).FecRec = Date.MinValue Then
                                            no_correcto2 = 1

                                            listaType22(i2).estado = ItzultzaileWeb.Itzuli("Dcto. no entregado")

                                        Else
                                            If listaType22(i2).FecRec = Date.MinValue Then
                                                no_correcto2 = 1

                                                listaType22(i2).estado = ItzultzaileWeb.Itzuli("Caducidad no asignada")

                                            Else

                                                If listaType22(i2).FecCad < Now And listaType22(i2).FecCad <> Date.MinValue Then '"01/01/1900"  CDate(Now.ToShortDateString)
                                                    no_correcto2 = 1

                                                    'si es de plantilla
                                                    If listaType22(i2).plantilla = 1 And listaType22(i2).FecCad <> Date.MaxValue Then
                                                        '    no_correcto = 1
                                                        listaType22(i2).estado = ItzultzaileWeb.Itzuli("Validez caducada por existir nueva plantilla")
                                                    Else
                                                        'margen se saca de DOC008
                                                        If listaType22(i2).FecCad < CDate(Now.AddDays(-listaType22(i2).Margen).ToShortDateString) And listaType22(i2).FecCad <> Date.MinValue Then
                                                            listaType22(i2).estado = ItzultzaileWeb.Itzuli("Validez caducada")
                                                        Else
                                                            listaType22(i2).estado = ItzultzaileWeb.Itzuli("Validez caducada, pero dentro de margen")
                                                            no_correcto2 = 2
                                                        End If
                                                    End If

                                                Else
                                                    'si esta puesto incorrecto en la mod del doc se pone asi
                                                    If listaType22(i2).correcto = 1 Then
                                                        no_correcto2 = 1
                                                        listaType22(i2).estado = ItzultzaileWeb.Itzuli("Marcado Incorrecto")
                                                    Else
                                                        If listaType22(i2).correcto = 2 Then
                                                            no_correcto2 = 1
                                                            listaType22(i2).estado = ItzultzaileWeb.Itzuli("No validado")
                                                        Else

                                                            'fecha de validez si no es de plantilla
                                                            If listaType22(i2).periodicidad <> 13 And listaType22(i2).FecIni = Date.MinValue And listaType22(i2).plantilla = 0 Then
                                                                no_correcto2 = 1
                                                                listaType22(i2).estado = ItzultzaileWeb.Itzuli("Falta fecha inicio de Validez")
                                                            Else
                                                                no_correcto2 = 0
                                                                listaType22(i2).estado = ItzultzaileWeb.Itzuli("Documento correcto")


                                                            End If

                                                        End If

                                                    End If

                                                End If

                                            End If
                                        End If
                                        'mostramos un icono u otro en función de si es ok o no, o no es obligatorio
                                        If no_correcto2 = 1 Then

                                            listaType22(i2).txtcorrecto = "Errores"
                                            contaerrores = contaerrores + 1

                                        Else
                                            If no_correcto2 = 2 Then
                                                listaType22(i2).txtcorrecto = "Avisos"
                                                contaavisos = contaavisos + 1
                                            Else
                                                listaType22(i2).txtcorrecto = "Correcto"
                                            End If

                                        End If


                                        'leer si existe el registro en adok_emd cogido en campo ubicacion
                                        If listaType22(i2).ubicacion = "" Then
                                            listaType22(i2).ubicacion = "false" 'para quitar el boton
                                        Else
                                            listaType22(i2).ubicacion = "true"
                                        End If

                                    Next

                                End If

                                If contaerrores > 0 Then
                                    contaerrores2 = contaerrores2 + 1
                                ElseIf contaavisos > 0 Then
                                    contaavisos2 = contaavisos2 + 1
                                End If
                                contaerrores = 0
                                contaavisos = 0

                            Next
                        Else

                            ninguntrabajador = 1
                        End If

                        'fin para cada trabajador

                        If contaerrores2 > 0 Then
                            listaType(j).Autonomo = 2
                            If contaerrores2 = 1 Then
                                listaType(j).medio2 = contaerrores2.ToString & " " & ItzultzaileWeb.Itzuli("Trabajador")
                            Else
                                listaType(j).medio2 = contaerrores2.ToString & " " & ItzultzaileWeb.Itzuli("Trabajadores")
                            End If

                        Else

                            If contaavisos2 > 0 Then
                                listaType(j).Autonomo = 1
                                If contaavisos2 = 1 Then
                                    listaType(j).medio2 = contaavisos2.ToString & " " & ItzultzaileWeb.Itzuli("Con Aviso")
                                Else
                                    listaType(j).medio2 = contaavisos2.ToString & " " & ItzultzaileWeb.Itzuli("Con Avisos")
                                End If

                            Else
                                If ninguntrabajador = 0 Then
                                    listaType(j).medio2 = "Correcto"
                                    listaType(j).Autonomo = 0
                                Else
                                    listaType(j).medio2 = "Sin Trabajadores"
                                    listaType(j).Autonomo = 1
                                End If



                            End If
                        End If
                        listaType(j).medio2 = listaType(j).medio2 & " de " & trabajadores.Count

                        '''''''''''''''''''''      contaTrabajadores = contaTrabajadores + trabajadores.Count


                        If listaType(j).recibi > 0 Then
                            If listaType(j).recibi = 1 Then
                                listaType(j).medio = listaType(j).recibi.ToString & " " & ItzultzaileWeb.Itzuli("Error")
                            Else
                                listaType(j).medio = listaType(j).recibi.ToString & " " & ItzultzaileWeb.Itzuli("Errores")
                            End If

                        Else

                            If listaType(j).activo > 0 Then
                                If listaType(j).activo = 1 Then
                                    listaType(j).medio = listaType(j).activo.ToString & " " & ItzultzaileWeb.Itzuli("Aviso")
                                Else
                                    listaType(j).medio = listaType(j).activo.ToString & " " & ItzultzaileWeb.Itzuli("Avisos")
                                End If

                            Else
                                listaType(j).medio = "Correcto"
                            End If
                        End If





                    End If
                Next  'fin por cada empresa

                Label8.Text = listaType.Count.ToString
                'Label12.Text = contaTrabajadores.ToString

                gvType.DataSource = listaType
                gvType.DataBind()
                gvType.Caption = String.Empty
            Else

                Label8.Text = "0"
                'Label12.Text = "0"
                gvType.DataSource = Nothing
                gvType.DataBind()
                gvType.Caption = "No hay registros"
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar un documento", ex)
        End Try
    End Sub

    Protected Sub BindDataView2()
        Try
            Dim oDocBLL As New BLL.DocumentosBLL
            Dim i As Int16
            Dim listaType As List(Of ELL.Solicitudes)
            Dim contavacios As Integer = 0
            Dim codemp As Int32 = CInt(idEmpresa.Value)
            If codemp > 0 Then
                'poner valores de empresa sabiendo planta y emp
                Dim lista As List(Of ELL.Empresas)




                lista = oDocBLL.CargarTiposEmpresa(codemp, PageBase.plantaAdmin)
                If (lista.Count > 0) Then  'la empresa tiene registro en adok

                    Dim listaType2 As List(Of ELL.EmpresasDoc)
                    listaType2 = oDocBLL.CargarListaEmpDocAsignados(PageBase.plantaAdmin, codemp)
                    If (listaType2.Count > 0) Then


                        For i = 0 To listaType2.Count - 1
                            Dim no_correcto As Int16 = 0
                            'estado
                            If listaType2(i).FecRec = Date.MinValue Then
                                no_correcto = 1
                                listaType2(i).estado = ItzultzaileWeb.Itzuli("Dcto. no entregado")

                            Else
                                If listaType2(i).FecRec = Date.MinValue Then
                                    no_correcto = 1

                                    listaType2(i).estado = ItzultzaileWeb.Itzuli("Caducidad no asignada")

                                Else


                                    'si esta puesto incorrecto en la mod del doc se pone asi

                                    If listaType2(i).correcto = 2 Then
                                        no_correcto = 1
                                        listaType2(i).estado = ItzultzaileWeb.Itzuli("No validado")
                                    Else

                                        'fecha de validez si no es de plantilla
                                        If listaType2(i).periodicidad <> 13 And listaType2(i).FecIni = Date.MinValue And listaType2(i).plantilla = 0 Then
                                            no_correcto = 1
                                            listaType2(i).estado = ItzultzaileWeb.Itzuli("Falta fecha inicio de Validez")
                                        Else
                                            no_correcto = 0
                                            listaType2(i).estado = ItzultzaileWeb.Itzuli("Documento correcto")

                                            If no_correcto = 0 And listaType2(i).FecCad < Now And listaType2(i).FecCad <> Date.MinValue Then '"01/01/1900"  CDate(Now.ToShortDateString)
                                                no_correcto = 1

                                                'si es de plantilla
                                                If listaType2(i).plantilla = 1 And listaType2(i).FecCad <> Date.MaxValue Then
                                                    '    no_correcto = 1
                                                    listaType2(i).estado = ItzultzaileWeb.Itzuli("Validez caducada por existir nueva plantilla")
                                                Else
                                                    'margen se saca de DOC008
                                                    Dim margen As Integer = 0
                                                    If listaType2(i).Margen > 0 Then
                                                        margen = listaType2(i).Margen
                                                    End If
                                                    If listaType2(i).FecCad < CDate(Now.AddDays(-margen).ToShortDateString) And listaType2(i).FecCad <> Date.MinValue Then
                                                        listaType2(i).estado = ItzultzaileWeb.Itzuli("Validez caducada")
                                                    Else
                                                        listaType2(i).estado = ItzultzaileWeb.Itzuli("Validez caducada, pero dentro de margen")
                                                        no_correcto = 2
                                                    End If
                                                End If

                                            Else


                                            End If


                                        End If


                                    End If

                                    If listaType2(i).correcto = 1 Then

                                        no_correcto = 1
                                        listaType2(i).estado = ItzultzaileWeb.Itzuli("Marcado Incorrecto")
                                    End If


                                End If
                            End If

                            If listaType2(i).tipodoc = 1 Then 'carne
                                If listaType2(i).estado = ItzultzaileWeb.Itzuli("Dcto. no entregado") Then
                                    listaType2(i).estado = ""
                                End If
                            End If


                            'mostramos un icono u otro en función de si es ok o no, o no es obligatorio
                            If no_correcto = 1 Then

                                listaType2(i).txtcorrecto = "Errores" ' "'btn alert-danger' ><span class='glyphicon glyphicon-check'></span" '"~/App_Themes/Batz/IconosAcciones/error1.gif"

                            Else
                                If no_correcto = 2 Then
                                    listaType2(i).txtcorrecto = "Avisos" ' "btn  alert-warning' ><span class='glyphicon glyphicon-check'></span" '"~/App_Themes/Batz/IconosAcciones/atencion1.gif"
                                Else
                                    listaType2(i).txtcorrecto = "Correcto" '"'btn  alert-success' ><span class='glyphicon glyphicon-check'></span" '"~/App_Themes/Batz/IconosAcciones/ok1.gif"
                                End If

                            End If


                            'leer si existe el registro en adok_emd cogido en campo ubicacion
                            If listaType2(i).ubicacion = "" Then
                                listaType2(i).ubicacion = "false" 'para quitar el boton
                            Else
                                listaType2(i).ubicacion = "true"
                            End If


                            listaType2(i).nomemp = "true"


                        Next


                        Dim contaborrados As Integer = 0

                        Registros.Text = (listaType2.Count - contavacios).ToString
                        gvType2.DataSource = listaType2
                        gvType2.DataBind()
                        gvType2.Caption = String.Empty
                    Else
                        Registros.Text = "0"
                        gvType2.DataSource = Nothing
                        gvType2.DataBind()
                        gvType2.Caption = "No hay registros"
                    End If
                Else
                    idEmpresa.Value = "0"
                End If
            Else
                Dim listaType2 As List(Of ELL.Documentos)
                listaType2 = oDocBLL.CargarListaEmpDocTot(PageBase.plantaAdmin)
                If (listaType2.Count > 0) Then

                    gvType2.DataSource = listaType2
                    gvType2.DataBind()
                    gvType2.Caption = String.Empty

                Else

                    gvType2.DataSource = Nothing
                    gvType2.DataBind()
                    gvType2.Caption = "No hay registros"
                End If

            End If


        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar un documento", ex)
        End Try
    End Sub

    Protected Sub gvType_RowEditing(sender As Object, e As GridViewEditEventArgs)

    End Sub

#End Region




#Region "Propiedades"

    Protected Overrides ReadOnly Property RolesAcceso As System.Collections.Generic.List(Of ELL.Roles.RolUsuario)
        Get
            Dim roles As New List(Of ELL.Roles.RolUsuario)
            roles.Add(ELL.Roles.RolUsuario.Administrador)
            roles.Add(ELL.Roles.RolUsuario.Administrador2)
            roles.Add(ELL.Roles.RolUsuario.Extranet)

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
            If ((RolesAcceso Is Nothing OrElse Not tieneAcceso) AndAlso Not (PerfilUsuario.IdRol = ELL.Roles.RolUsuario.Administrador)) Then
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
            existe = RolesAcceso.Exists(Function(f) f = [Enum].Parse(GetType(ELL.Roles.RolUsuario), rolUsuario.ToString()))
            If (existe) Then
                Return existe
            End If
        End If

        Return existe
    End Function
#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            PageBase.plantaAdmin = 9
            If Not (Page.IsPostBack) Then



                Dim persona As New SabLib.ELL.Ticket ' lo pongo aqui por seguridad

                If (Session("Ticket") IsNot Nothing) Then
                    persona = CType(Session("Ticket"), SabLib.ELL.Ticket)
                End If

                Dim empresa As New SabLib.ELL.Empresa ' lo pongo aqui por seguridad '''      persona.IdEmpresa = 10345   para puebas extranet

                empresa.Id = persona.IdEmpresa
                Dim accessDB As New SabLib.DAL.EmpresasDAL

                Dim regempresa As String() = accessDB.consultar(empresa)   '.5435
                Dim strempre As String = regempresa(3) 'es el cif



                Initialize()
            Else

                Initialize()

            End If


        Catch ex As Exception

            Master.MensajeError = "Error load de Estadoempresadocumento " & ex.ToString
        End Try
    End Sub


    Protected Sub gvTypeHis_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvTypeHis.RowCommand
        If e.CommandName = "Desactivar" Then
            Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
            Dim Iddoc2 As Int32 = CInt(strDoc)

            Dim TipoTra As New ELL.Trabajadores With {.Planta = PageBase.plantaAdmin, .activo = 1, .Id = Iddoc2}
            If (oDocumentosBLL.ModificarDOS(TipoTra)) Then

                Master.MensajeInfo = ItzultzaileWeb.Itzuli("El documento se ha borrado correctamente").ToUpper

                Dim listaEmpDoc As List(Of ELL.EmpresasDoc)
                'buscar en adok_emd
                '  Dim tipo As New ELL.EmpresasDoc With {.Planta = PageBase.plantaAdmin, .codemp = CInt(idDoc.Value)}
                Dim tipo As New ELL.EmpresasDoc With {.Planta = PageBase.plantaAdmin, .codemp = CInt(idEmpresa.Value), .coddoc = CInt(idDoc.Value)}


                listaEmpDoc = oDocumentosBLL.LeerEmpDocHis(tipo)
                If listaEmpDoc.Count > 0 Then
                    Dim fecha, fecha1, fecha2, fecha3 As String
                    For i = 0 To listaEmpDoc.Count - 1
                        fecha1 = Mid(listaEmpDoc(i).ubicacion, 18, 4)
                        fecha2 = Mid(listaEmpDoc(i).ubicacion, 22, 2)
                        fecha3 = Mid(listaEmpDoc(i).ubicacion, 24, 2)
                        fecha = fecha3 & "/" & fecha2 & "/" & fecha1
                        If IsDate(fecha) Then
                            listaEmpDoc(i).FecRec = CDate(fecha)
                            listaEmpDoc(i).Abrev = listaEmpDoc(i).FecRec.ToShortDateString
                        Else
                            listaEmpDoc(i).FecRec = Date.MinValue
                            listaEmpDoc(i).Abrev = ""
                        End If

                        '    listaEmpDoc(i).ubicacion = Mid(listaEmpDoc(i).ubicacion, 32)

                    Next

                    txtCIFHis.Text = txtCIF.Text
                    txtNombreHis.Text = txtNombre.Text
                    mView.ActiveViewIndex = 3

                    gvTypeHis.DataSource = listaEmpDoc
                    gvTypeHis.DataBind()
                    gvTypeHis.Caption = String.Empty

                Else
                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                End If

            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("Un Error ha ocurrido cuando se borraba el documento").ToUpper
            End If

        End If

    End Sub
    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' <para>Redirecciona al detalle del item seleccionado</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvType_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvType.RowCommand

        If e.CommandName = "Edit" Then

            Dim IdEmpre As Int32 = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).id
            Dim DesEmpre As String = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).nombre
            idEmpresa.Value = IdEmpre
            DescEmpresa.Value = DesEmpre
            BindDataView2()
            ConfiguracionProduct(IdEmpre)

        End If

        If e.CommandName = "Trabajadores" Then
            Dim IdEmpre As Int32 = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).id
            If PageBase.intRol = 0 Then
                Response.Redirect("EstadoTrabajadorDocumento9.aspx?empresa=" & IdEmpre.ToString, True)
            Else
                Response.Redirect("EstadoTrabajadorDocumentoResp9.aspx?empresa=" & IdEmpre.ToString, True)
            End If

        End If


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



    Protected Sub gvType_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvType.PageIndexChanging
        gvType.PageIndex = e.NewPageIndex
        BindDataView()
    End Sub
    Public Sub ibHistorico_Click(sender As Object, e As EventArgs)
        Try
            Dim fila As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            Dim idDocumento As String = gvType2.DataKeys(fila.RowIndex)("clave").ToString()
            idDoc.Value = idDocumento
            'sacar el nombre del tipo de doc

            Dim listaDoc As List(Of ELL.Documentos)
            'Dim tipo As New ELL.Documentos With {.Planta = PageBase.plantaAdmin, .Id = CInt(idDocumento)}
            listaDoc = oDocumentosBLL.CargarDocumentos(CInt(idDocumento), PageBase.plantaAdmin)
            If listaDoc.Count > 0 Then
                txtDocuEmp.Text = listaDoc(0).Nombre
            End If


            Dim listaEmpDoc As List(Of ELL.EmpresasDoc)
            'buscar en adok_emd
            Dim tipo As New ELL.EmpresasDoc With {.Planta = PageBase.plantaAdmin, .codemp = CInt(idEmpresa.Value), .coddoc = CInt(idDocumento)}


            listaEmpDoc = oDocumentosBLL.LeerEmpDocHis(tipo)
            If listaEmpDoc.Count > 0 Then
                Dim fecha, fecha1, fecha2, fecha3 As String
                For i = 0 To listaEmpDoc.Count - 1
                    fecha1 = Mid(listaEmpDoc(i).ubicacion, 18, 4)
                    fecha2 = Mid(listaEmpDoc(i).ubicacion, 22, 2)
                    fecha3 = Mid(listaEmpDoc(i).ubicacion, 24, 2)
                    fecha = fecha3 & "/" & fecha2 & "/" & fecha1
                    If IsDate(fecha) Then
                        listaEmpDoc(i).FecRec = CDate(fecha)
                        listaEmpDoc(i).Abrev = listaEmpDoc(i).FecRec.ToShortDateString
                    Else
                        listaEmpDoc(i).FecRec = Date.MinValue
                        listaEmpDoc(i).Abrev = ""
                    End If

                Next

                txtCIFHis.Text = txtCIF.Text
                txtNombreHis.Text = txtNombre.Text
                mView.ActiveViewIndex = 3

                gvTypeHis.DataSource = listaEmpDoc
                gvTypeHis.DataBind()
                gvTypeHis.Caption = String.Empty

            Else
                Master.MensajeInfo = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
            End If
        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("Error en el histórico de documentos")
        End Try

    End Sub
    Public Sub ibModificar_Click(sender As Object, e As EventArgs)
        Try

            Dim fila As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            Dim idDocumento As String = gvType2.DataKeys(fila.RowIndex)("clave").ToString()
            idDoc.Value = idDocumento
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim oDocBLL As New BLL.DocumentosBLL

            ddlCaducidad.Items.Clear()
            Dim licaducidadVacio As New ListItem("", 0)
            ddlCaducidad.Items.Add(licaducidadVacio)
            Dim listaCad As List(Of ELL.Caducidades)
            listaCad = oDocBLL.CargarListaCad(PageBase.plantaAdmin)

            For Each caducidad In listaCad
                Dim licaducidad As New ListItem(caducidad.nombre, caducidad.id)
                ddlCaducidad.Items.Add(licaducidad)

            Next

            Dim listaEmpDoc As List(Of ELL.EmpresasDoc)
            'buscar en adok_emd
            Dim tipo As New ELL.EmpresasDoc With {.Planta = PageBase.plantaAdmin, .codemp = idEmpresa.Value, .coddoc = idDoc.Value}


            listaEmpDoc = oDocumentosBLL.LeerEmpDoc(tipo)

            If listaEmpDoc.Count > 0 Then
                Dim listaDoc As List(Of ELL.Documentos)
                listaDoc = oDocumentosBLL.CargarDocumentos(CInt(idDocumento), PageBase.plantaAdmin)
                txtNombreDoc.Text = listaDoc(0).Nombre
                txtComentario.Text = listaEmpDoc(0).Comentario

                If listaEmpDoc(0).FecRec > Date.MinValue Then
                    TxtFechaRec.Text = listaEmpDoc(0).FecRec.ToShortDateString
                Else
                    TxtFechaRec.Text = ""
                End If


                imgCalendarioFechaVal.Visible = True
                If listaEmpDoc(0).FecIni > Date.MinValue Then
                    TxtFechaVal.ReadOnly = False
                    TxtFechaVal.Text = listaEmpDoc(0).FecIni.ToShortDateString
                    FechaValG.Value = listaEmpDoc(0).FecIni.ToShortDateString
                Else
                    TxtFechaVal.Text = ""
                End If


                If listaEmpDoc(0).periodicidad > 0 Then
                    ddlCaducidad.SelectedValue = listaEmpDoc(0).periodicidad
                Else
                    ddlCaducidad.SelectedValue = 13
                End If

                If ddlCaducidad.SelectedValue = 13 Then 'no tiene caducidad
                    '          TxtFechaVal.ReadOnly = True

                    TxtFechaVal.ReadOnly = True
                    imgCalendarioFechaVal.Visible = False
                    TxtFechaVal.Text = ItzultzaileWeb.Itzuli("Documento sin caducidad")
                Else
                    TxtFechaVal.ReadOnly = False
                End If

                Dim listaDocu As List(Of ELL.Documentos)
                listaDocu = oDocumentosBLL.CargarDocumentos(CInt(idDoc.Value), PageBase.plantaAdmin)
                If listaDocu(0).Plantilla = 1 Then 'no tiene caducidad
                    TxtFechaVal.ReadOnly = True
                    imgCalendarioFechaVal.Visible = False
                    ddlCaducidad.SelectedValue = 13
                    TxtFechaVal.Text = ItzultzaileWeb.Itzuli("Documento de plantilla")
                    TxtFechaVal2.Text = ItzultzaileWeb.Itzuli("Sin caducidad")
                Else
                    TxtFechaVal2.Text = listaEmpDoc(0).FecCad.ToShortDateString
                End If

                txtUbicacion.Text = listaEmpDoc(0).ubicacionfisica
                rblCorrecto.SelectedValue = listaEmpDoc(0).correcto

                FechaValG2.Value = listaEmpDoc(0).FecCad.ToShortDateString
                '         TxtFechaRec.Text = listaEmpDoc(0).FecIni

                'falta la parte opcional, si el doc tiene correos en DOC013 miramos impuestos en emd009 listaEmpDoc(0).impuestos
                If listaDocu(0).listacorreos <> "" Then 'tiene correos a notificar, es una excepcion
                    If listaEmpDoc(0).Impuestos < 1 Then
                        listaEmpDoc(0).Impuestos = 4 'son valores de sin deuda
                    End If
                    rblImpuestos.SelectedValue = listaEmpDoc(0).Impuestos
                    PanelX.Visible = True
                Else
                    'no debe salir esa linea
                    PanelX.Visible = False
                End If

                mView.ActiveViewIndex = 2

            Else
                Master.MensajeInfo = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error  
            End If




        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al modificar el documento")
        End Try
    End Sub

    Protected Sub PageDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvType.PageIndexChanging
        gvType.PageIndex = e.NewPageIndex
    End Sub

    Private Sub btnSubir2_Click(sender As Object, e As EventArgs) Handles btnSubir2.Click

        'INSERTAR en emd. en realidad es update con la direccion del fichero y subir fichero
        Dim fechaValidez As Date
        Dim fechaCad As Date
        Dim cantidad As Int32
        Dim intervalo As String
        Dim periodo As Int32
        Try
            If FuDoc.PostedFile.ContentLength > 5242880 Then
                Master.MensajeError = ItzultzaileWeb.Itzuli("Ha superado el límite máximo de 5 Mb. Intente comprimir el archivo (*.zip) o pónganse en contacto con adok@batz.es")
                mView.ActiveViewIndex = 4
                Exit Sub

            End If
            'poner fechaCad para TC1 y TC2
            If ddlMes.SelectedValue > 0 Then
                If ddlMes.SelectedValue = 12 Then 'diciembre
                    TxtFechaValidez.Text = CDate("01/" & 1 & "/" & Now.Year)
                Else
                    If ddlMes.SelectedValue = 11 Or ddlMes.SelectedValue = 10 Then 'Noviembre
                        TxtFechaValidez.Text = CDate("01/" & ddlMes.SelectedValue + 1 & "/" & Now.Year - 1)
                    Else
                        TxtFechaValidez.Text = CDate("01/" & ddlMes.SelectedValue + 1 & "/" & Now.Year)
                    End If

                End If


            Else
                If ddlMes.SelectedValue = 0 And (idDoc.Value = 158 Or idDoc.Value = 162) Then
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Fecha de inicio de validez obligatoria")
                    mView.ActiveViewIndex = 4
                    Exit Sub
                End If

            End If

            If Not IsDate(TxtFechaValidez.Text) And TxtFechaValidez.Text <> ItzultzaileWeb.Itzuli("Documento sin caducidad") Then
                Master.MensajeError = ItzultzaileWeb.Itzuli("Fecha de inicio de validez obligatoria")
                mView.ActiveViewIndex = 4
                Exit Sub
            End If


            If FuDoc.HasFile Then  'fuDoc  view4.Controls(13).HasFile  DirectCast((view4.Controls(13)), FileUpload)

                Dim fmt2 As String = "00"
                Dim fmt3 As String = "000"
                Dim fmt4 As String = "0000"
                Dim fmt6 As String = "000000"
                Dim fmt8 As String = "00000000"
                Dim codigo As Int32 = idDoc.Value  'txtDocEmp.Text
                Dim codempr As Int32 = idEmpresa.Value
                'buscar desc con cod emp y cod doc idEmpresa.Value txtDocEmp.Text

                Dim oDocBLL As New BLL.DocumentosBLL

                Dim fechatext As String = Now.Year.ToString(fmt4) & Now.Month.ToString(fmt2) & Now.Day.ToString(fmt2) & Now.Hour.ToString(fmt2) & Now.Minute.ToString(fmt2) & Now.Second.ToString(fmt2)
                Dim prefijo As String = PageBase.plantaAdmin.ToString(fmt3) & codempr.ToString(fmt6) & codigo.ToString(fmt8) & fechatext
                FuDoc.SaveAs(DirFicherosSubir & "Ficheros_Matriz_Puestos/Documentos/" & prefijo & Right(FuDoc.FileName, 5))
                'poner un texto                    lblPlantillaSubida.Text = "Documento subido: " & fuDoc.PostedFile.FileName
                'subir a oracle

                'codigo 13 = no tiene intervalo
                'calcular caducidad emd004
                'leer de adok_doc con  idDoc.Value los valores de intervalo cantidad y si tiene caducidad. el doc003 -> per000 se saca per003 como cantidad y per002 ->int000 que saca int001=intervalo


                'el periodo NO se coge de emd007 sino de doc
                Dim listaDoc As List(Of ELL.EmpresasDoc)
                listaDoc = oDocumentosBLL.CargarListaEmpDoc(PageBase.plantaAdmin, CInt(idEmpresa.Value), CInt(idDoc.Value))

                If ddlCaducidad2.SelectedValue = 998 Then 'no se ha seleccionado, se coge el del emd007
                    ddlCaducidad2.SelectedValue = listaDoc(0).periodicidad

                End If

                periodo = ddlCaducidad2.SelectedValue

                Dim listaCad As List(Of ELL.Caducidades)
                listaCad = oDocBLL.CargarCad(PageBase.plantaAdmin, periodo)

                cantidad = listaCad(0).cantidad

                'leer int001
                listaCad = oDocBLL.CargarInt(PageBase.plantaAdmin, listaCad(0).intervalo)
                intervalo = Trim(listaCad(0).nombre)

                If intervalo <> "nnnn" And Trim(TxtFechaValidez.Text) <> "" And IsDate(TxtFechaValidez.Text) Then
                    fechaValidez = CDate(TxtFechaValidez.Text)
                    If cantidad = 0 Then 'sin periodicidad
                        fechaCad = Date.MaxValue
                    Else
                        fechaCad = DateAdd(intervalo, cantidad, fechaValidez)
                    End If


                Else
                    fechaValidez = Date.MinValue
                    fechaCad = Date.MaxValue
                End If



                'si es documento de plantilla quitamos caducidad
                Dim listaDocu As List(Of ELL.Documentos)
                listaDocu = oDocumentosBLL.CargarDocumentos(CInt(idDoc.Value), PageBase.plantaAdmin)
                If listaDocu(0).Plantilla = 1 Then '   And CInt(idDoc.Value) <> 123  el 123 es el de manual que tiene plantilla y caducidad
                    'fechaValidez = Now
                    fechaCad = Date.MinValue
                End If

                Dim intestado As Integer
                If fechaCad > Now Then 'no caducado ya mismo
                    intestado = 2
                    'si el documento tiene lista de correo a avisar de su subida, se le manda mensaje
                    If listaDocu(0).listacorreos <> "" Then
                        'prueba correo
                        Dim paramBLL As New SabLib.BLL.ParametrosBLL
                        Dim oParams As SabLib.ELL.Parametros = paramBLL.consultar()
                        Dim mailFrom As String = ConfigurationManager.AppSettings("MailFrom")

                    End If

                Else 'caducado o sin caducidad

                    If fechaCad = Date.MinValue Then
                        intestado = 2
                    Else
                        intestado = 1
                    End If

                End If


                Dim tipo As New ELL.EmpresasDoc With {.periodicidad = periodo, .Planta = PageBase.plantaAdmin, .estado = intestado, .codemp = codempr, .coddoc = codigo, .FecCad = fechaCad, .FecIni = fechaValidez, .ubicacion = prefijo & Right(FuDoc.FileName, 5)}



                If (oDocumentosBLL.ModificarEmpDoc(tipo, 2, Session("Ticket").nombreusuario)) Then  'actualizamos emd005 y emd004 y emd006
                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("Se ha insertado el documento") & " " & codigo & " " & ItzultzaileWeb.Itzuli("al Puesto") & " " & idEmpresa.Value & " " & ItzultzaileWeb.Itzuli("correctamente").ToUpper
                    'falta refrescar el doc
                    BindDataView2()
                    FecUltDoc.Text = Now.ToShortDateString
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se insertaba el documento") & " " & codigo & " " & ItzultzaileWeb.Itzuli("al Puesto") & " " & idEmpresa.Value
                End If



            End If
            mView.ActiveViewIndex = 1

        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al subir el documento")
        End Try
    End Sub


    Public Sub ibSubir_Click(sender As Object, e As EventArgs)
        Try

            Dim fila As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            Dim idDocumento As String = gvType2.DataKeys(fila.RowIndex)("clave").ToString()
            txtEmpDoc.Text = DescEmpresa.Value ' idEmpresa.Value
            idDoc.Value = idDocumento
            'buscar en pla la plantilla de ese doc es pla005
            Dim liAsignacion As List(Of ELL.Plantillas)
            liAsignacion = oDocumentosBLL.CargarListaPla(PageBase.plantaAdmin, idDocumento)

            If liAsignacion.Count > 0 Then
                idPlantilla0.Value = liAsignacion(0).nombre
                '            btnDescargarPlantilla.Visible = True
            Else
                idPlantilla0.Value = "0"
                '           btnDescargarPlantilla.Visible = False
            End If

            'cargar caducidad
            ddlCaducidad.Items.Clear()

            Dim listaCad As List(Of ELL.Caducidades)
            listaCad = oDocumentosBLL.CargarListaCad(PageBase.plantaAdmin)

            For Each caducidad In listaCad
                Dim licaducidad As New ListItem(caducidad.nombre, caducidad.id)
                ddlCaducidad2.Items.Add(licaducidad)

            Next


            'buscar en adok_doc
            Dim listaDoc As List(Of ELL.Documentos)
            'Dim tipo As New ELL.Documentos With {.Planta = PageBase.plantaAdmin, .Id = CInt(idDocumento)}
            listaDoc = oDocumentosBLL.CargarDocumentos(CInt(idDocumento), PageBase.plantaAdmin)
            If listaDoc.Count > 0 Then
                txtDocEmp.Text = listaDoc(0).Nombre
                ddlCaducidad2.SelectedValue = listaDoc(0).Periodo
            End If
            'buscar en adok_emd
            Dim listaDoc2 As List(Of ELL.EmpresasDoc)
            ' Dim tipo As New ELL.EmpresasDoc With {.Planta = PageBase.plantaAdmin, Idempresa = CInt(idDocumento)}
            listaDoc2 = oDocumentosBLL.CargarListaEmpDoc(PageBase.plantaAdmin, CInt(idEmpresa.Value), CInt(idDocumento))
            If listaDoc2.Count > 0 Then
                TxtFechaValidez.ReadOnly = False
                imgCalendarioFechaValidez.Visible = True


                If listaDoc2(0).ubicacion <> "" Then
                    '''''''''''''' no se cambia la que tiene por defecto    ddlCaducidad2.SelectedValue = listaDoc2(0).periodicidad

                    If listaDoc2(0).FecIni = Date.MinValue Then
                        TxtFechaValidez.Text = "" ' Now.ToShortDateString
                    Else
                        TxtFechaValidez.Text = (listaDoc2(0).FecIni).ToShortDateString
                    End If
                    If (listaDoc2(0)).periodicidad = 13 Or (listaDoc2(0)).periodicidad = 998 Then 'no tiene caducidad

                        imgCalendarioFechaValidez.Visible = False
                        TxtFechaValidez.ReadOnly = True
                        TxtFechaValidez.Text = ItzultzaileWeb.Itzuli("Documento sin caducidad")

                    End If

                    FecUltDoc.Text = Mid(listaDoc2(0).ubicacion, 24, 2) & "/" & Mid(listaDoc2(0).ubicacion, 22, 2) & "/" & Mid(listaDoc2(0).ubicacion, 18, 4)
                Else
                    TxtFechaValidez.Text = "" ' Now.ToShortDateString
                    FecUltDoc.Text = ItzultzaileWeb.Itzuli("No hay ningún documento subido")
                End If


                If listaDoc(0).Plantilla = 1 Then 'los de plantilla no tienen caducidad

                    imgCalendarioFechaValidez.Visible = False
                    TxtFechaValidez.ReadOnly = True
                    TxtFechaValidez.Text = ItzultzaileWeb.Itzuli("Documento sin caducidad")
                End If

            Else
                FecUltDoc.Text = ItzultzaileWeb.Itzuli("No hay ningún documento subido")
            End If

            mView.ActiveViewIndex = 4

        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al subir el documento")
        End Try

    End Sub

    Public Sub ibPlantilla_Click(sender As Object, e As EventArgs)
        Try


            Dim fila As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            Dim idDocumento As String = gvType2.DataKeys(fila.RowIndex)("clave").ToString()

            'buscar en pla la plantilla de ese doc es pla005
            Dim liPlantilla As List(Of ELL.Plantillas)
            liPlantilla = oDocumentosBLL.CargarListaPla(PageBase.plantaAdmin, CInt(idDocumento))

            If liPlantilla.Count > 0 Then
                'idPlantilla0.Value = liPlantilla(0).nombre
                If liPlantilla(0).nombre <> "" Then

                    'tratar el idioma
                    If Right(Split(liPlantilla(0).nombre.ToUpper, ".")(0), 4) = "-EUS" Or Right(Split(liPlantilla(0).nombre.ToUpper, ".")(0), 4) = "-ENG" Or Right(Split(liPlantilla(0).nombre.ToUpper, ".")(0), 4) = "X-ESP" Then
                        'BUSCAR EL CORRESPONDIENTE
                        Dim cultura As String
                        cultura = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName

                        Dim docnombre As String = ""
                        Select Case cultura
                            Case "es"
                                docnombre = Mid(Split(liPlantilla(0).nombre, ".")(0), 1, Split(liPlantilla(0).nombre, ".")(0).Length - 4) & "-ESP." & Split(liPlantilla(0).nombre, ".")(1)

                            Case "eu"
                                docnombre = Mid(Split(liPlantilla(0).nombre, ".")(0), 1, Split(liPlantilla(0).nombre, ".")(0).Length - 4) & "-EUS." & Split(liPlantilla(0).nombre, ".")(1)
                            Case "en"
                                docnombre = Mid(Split(liPlantilla(0).nombre, ".")(0), 1, Split(liPlantilla(0).nombre, ".")(0).Length - 4) & "-ENG." & Split(liPlantilla(0).nombre, ".")(1)
                            Case Else
                                docnombre = Mid(Split(liPlantilla(0).nombre, ".")(0), 1, Split(liPlantilla(0).nombre, ".")(0).Length - 4) & "-ENG." & Split(liPlantilla(0).nombre, ".")(1)
                        End Select
                        'HAY QUE VER QUE EXISTE, SI NO BAJO EL ULTIMO
                        Dim liPlantilla2 As List(Of ELL.Plantillas)
                        liPlantilla2 = oDocumentosBLL.CargarListaPlaDoc(PageBase.plantaAdmin, docnombre)
                        If liPlantilla2.Count > 0 Then
                            idDocumento = DirFicherosBajar & "Ficheros_Matriz_Puestos/" & docnombre
                        Else
                            idDocumento = DirFicherosBajar & "Ficheros_Matriz_Puestos/" & liPlantilla(0).nombre
                        End If

                    Else
                        idDocumento = DirFicherosBajar & "Ficheros_Matriz_Puestos/" & liPlantilla(0).nombre
                        'Process.Start(idDocumento)
                        Dim script As String = "<script type=""text/javascript"">window.open('" & idDocumento & "');</script>"
                        ClientScript.RegisterStartupScript(Me.GetType, "openWindow", script)
                    End If



                Else
                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("No se ha encontrado el documento ").ToUpper 'error 
                End If

            Else
                Master.MensajeInfo = ItzultzaileWeb.Itzuli("Plantilla no subida ").ToUpper 'error 
            End If


        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado la plantilla")
        End Try

    End Sub



    Public Sub ibPlantilla2_Click(sender As Object, e As EventArgs)
        Try


            Dim fila As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            Dim idDocumento As String = gvType2.DataKeys(fila.RowIndex)("clave").ToString()

            'buscar en pla la plantilla de ese doc es pla005
            Dim liPlantilla As List(Of ELL.Plantillas)
            liPlantilla = oDocumentosBLL.CargarListaPla2(PageBase.plantaAdmin, idDocumento)

            If liPlantilla.Count > 0 Then

                If liPlantilla(0).nombre <> "" Then
                    idDocumento = DirFicherosBajar & "Ficheros_Matriz_Puestos/" & liPlantilla(0).nombre
                    'Process.Start(idDocumento)
                    Dim script As String = "<script type=""text/javascript"">window.open('" & idDocumento & "');</script>"
                    ClientScript.RegisterStartupScript(Me.GetType, "openWindow", script)
                Else
                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("No se ha encontrado el documento ").ToUpper 'error 
                End If

            Else
                Master.MensajeInfo = ItzultzaileWeb.Itzuli("Plantilla no subida ").ToUpper 'error 
            End If


        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado la plantilla")
        End Try

    End Sub



    Public Sub ibVer_Click2(sender As Object, e As EventArgs)
        Try



            Dim idDocumento As String = idDoc.Value
            Dim listaEmpDoc As List(Of ELL.EmpresasDoc)

            'buscar en adok_emd
            Dim tipo As New ELL.EmpresasDoc With {.Planta = PageBase.plantaAdmin, .codemp = idEmpresa.Value, .coddoc = idDocumento}
            listaEmpDoc = oDocumentosBLL.LeerEmpDoc(tipo)
            If listaEmpDoc.Count > 0 Then

                If listaEmpDoc(0).ubicacion <> "" Then
                    hfurl.Value = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & (oDocumentosBLL.LeerEmpDoc(tipo))(0).ubicacion
                    'se puede hacer asi tambien aparte de por javascript
                    If listaEmpDoc(0).ubicacion <> "" Then
                        'idDocumento = Server.MapPath("~/") & "Ficheros_Matriz_Puestos/Documentos/" & listaEmpDoc(0).ubicacion
                        idDocumento = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & listaEmpDoc(0).ubicacion

                        Dim script As String = "<script type=""text/javascript"">window.open('" & idDocumento & "');</script>"
                        ClientScript.RegisterStartupScript(Me.GetType, "openWindow", script)


                    Else
                        Master.MensajeInfo = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                    End If

                Else
                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                End If
            Else
                Master.MensajeInfo = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
            End If
        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al abrir el documento")
        End Try

    End Sub

    Public Sub ibVer_Click(sender As Object, e As EventArgs)
        Try


            Dim fila As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            Dim idDocumento As String = gvType2.DataKeys(fila.RowIndex)("clave").ToString()
            Dim listaEmpDoc As List(Of ELL.EmpresasDoc)

            'buscar en adok_emd
            Dim tipo As New ELL.EmpresasDoc With {.Planta = PageBase.plantaAdmin, .codemp = idEmpresa.Value, .coddoc = idDocumento}
            listaEmpDoc = oDocumentosBLL.LeerEmpDoc(tipo)
            If listaEmpDoc.Count > 0 Then

                If listaEmpDoc(0).ubicacion <> "" Then
                    hfurl.Value = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & (oDocumentosBLL.LeerEmpDoc(tipo))(0).ubicacion
                    'se puede hacer asi tambien aparte de por javascript
                    If listaEmpDoc(0).ubicacion <> "" Then
                        'idDocumento = Server.MapPath("~/") & "Ficheros_Matriz_Puestos/Documentos/" & listaEmpDoc(0).ubicacion
                        idDocumento = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & listaEmpDoc(0).ubicacion

                        'Dim URL As String = "ReferenciaVentaBrain.aspx?IdRef=" & CommandArgument
                        Dim script As String = "<script type=""text/javascript"">window.open('" & idDocumento & "');</script>"
                        ClientScript.RegisterStartupScript(Me.GetType, "openWindow", script)
                        'Process.Start(idDocumento)

                    Else
                        Master.MensajeInfo = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                    End If

                Else
                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                End If
            Else
                Master.MensajeInfo = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
            End If
        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al abrir el documento")
        End Try

    End Sub
    Public Sub ibVer_HIST_Click(sender As Object, e As EventArgs)
        Try

            Dim fila As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            Dim idDocumento As String = gvTypeHis.DataKeys(fila.RowIndex)("clave").ToString()
            Dim listaEmpDoc As List(Of ELL.EmpresasDoc)

            'buscar en adok_emd
            Dim tipo As New ELL.EmpresasDoc With {.clave = idDocumento}
            listaEmpDoc = oDocumentosBLL.LeerEmpDocHisClave(tipo)
            If listaEmpDoc.Count > 0 Then

                If listaEmpDoc(0).ubicacion <> "" Then
                    '  hfurl.Value = Server.MapPath("~/") & "Ficheros_Matriz_Puestos/" & (oDocumentosBLL.LeerEmpDoc(tipo))(0).ubicacion
                    idDocumento = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & listaEmpDoc(0).ubicacion
                    ' Response.Redirect(idDocumento, True)
                    ' Process.Start(idDocumento)
                    Dim script As String = "<script type=""text/javascript"">window.open('" & idDocumento & "');</script>"
                    ClientScript.RegisterStartupScript(Me.GetType, "openWindow", script)
                    'Dim imb As ImageButton = CType(sender, ImageButton)
                    'ScriptManager.RegisterStartupScript(Page, GetType(Page), "abrirAdjunto", "window.open('PopUpDoc.aspx?idDocs=" & idDocumento & "&type=doc');", True) 'CInt(imb.CommandArgument)

                Else
                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                End If
            Else
                Master.MensajeInfo = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
            End If
        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al ver el documento")
        End Try

    End Sub



    ''' <summary>
    ''' Cargar los tipos y Habilitar/Deshabilitar Transmission Mode dependiendo del producto seleccionado
    ''' </summary>
    ''' <param name="idEmpresa"></param>
    ''' <remarks></remarks>
    Private Sub ConfiguracionProduct(ByVal idEmpresa As Integer)
        'aqui poner los datos de la empresa
        Try
            Dim oDocBLL As New BLL.DocumentosBLL
            Dim lista As List(Of ELL.Empresas)
            Dim userBLL As New SabLib.BLL.UsuariosComponent

            lista = oDocBLL.CargarTiposEmpresa(idEmpresa, PageBase.plantaAdmin)
            If lista.Count > 0 Then
                txtCIF.Text = lista(0).Nif
                txtNombre.Text = lista(0).Nombre
                txtNombre3.Text = lista(0).Nombre
                '       lblNuevaSolicitud.Text = "Documentos de  de " & lista(0).Nombre
                mView.ActiveViewIndex = 1
            Else
                Master.MensajeInfo = ItzultzaileWeb.Itzuli("No tienes configurada ningun Puesto a administrar").ToUpper 'error 
            End If

        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar el Puesto" & idEmpresa, ex)
        End Try

    End Sub


    Protected Sub GrabarMod_Click(sender As Object, e As EventArgs) Handles GrabarMod.Click
        Try
            Dim fechaValidez As Date
            Dim fechaCad As Date
            Dim cantidad As Int32
            Dim intervalo As String
            Dim periodo As Int32
            Dim oDocBLL As New BLL.DocumentosBLL

            Dim fechaR As Date
            If (TxtFechaRec.Text = "") Then
                fechaR = Date.MinValue
            Else
                fechaR = CDate(TxtFechaRec.Text)
            End If



            'el periodo se coge de emd007 no de de doc
            Dim listaDoc As List(Of ELL.EmpresasDoc)
            listaDoc = oDocumentosBLL.CargarListaEmpDoc(PageBase.plantaAdmin, CInt(idEmpresa.Value), CInt(idDoc.Value))
            'periodo = listaDoc(0).periodicidad
            periodo = ddlCaducidad.SelectedValue

            Dim listaCad As List(Of ELL.Caducidades)
            listaCad = oDocBLL.CargarCad(PageBase.plantaAdmin, periodo)

            cantidad = listaCad(0).cantidad

            'leer int001
            listaCad = oDocBLL.CargarInt(PageBase.plantaAdmin, listaCad(0).intervalo)
            intervalo = Trim(listaCad(0).nombre)


            If intervalo <> "nnnn" And Trim(TxtFechaVal.Text) <> "" And IsDate(TxtFechaVal.Text) Then
                fechaValidez = CDate(TxtFechaVal.Text)
                If cantidad = 0 Then 'sin periodicidad
                    fechaCad = Date.MaxValue
                Else
                    fechaCad = DateAdd(intervalo, cantidad, fechaValidez)
                End If

            Else
                fechaValidez = Date.MinValue
                fechaCad = Date.MaxValue
            End If
            Dim tipo As New ELL.EmpresasDoc With {.Comentario = txtComentario.Text, .Planta = PageBase.plantaAdmin, .FecCad = fechaCad, .FecIni = fechaValidez, .codemp = CInt(idEmpresa.Value), .coddoc = CInt(idDoc.Value), .FecRec = fechaR, .ubicacion = txtUbicacion.Text, .estado = rblCorrecto.SelectedValue, .Impuestos = rblImpuestos.SelectedValue, .periodicidad = ddlCaducidad.SelectedValue}

            If (oDocumentosBLL.ModificarEmpDoc(tipo, 3, Session("Ticket").nombreusuario)) Then


                If fechaCad > Now Then 'no caducado ya mismo

                    'si el documento tiene lista de correo a avisar de su subida, se le manda mensaje
                    If listaDoc(0).Listacorreos <> "" Then
                        'prueba correo
                        Dim paramBLL As New SabLib.BLL.ParametrosBLL
                        Dim oParams As SabLib.ELL.Parametros = paramBLL.consultar()
                        Dim mailFrom As String = ConfigurationManager.AppSettings("MailFrom")

                        'se manda cuando se pone deuda o deuda aplazada
                        If CInt(idDoc.Value) = 161 And (rblImpuestos.SelectedValue = 2 Or rblImpuestos.SelectedValue = 3) Then   'Session("Ticket").nombreusuario
                            SabLib.BLL.Utils.EnviarEmail("MatrizPuesto" & "@batz.es", listaDoc(0).Listacorreos, ItzultzaileWeb.Itzuli("Nuevo documento a revisar"), ItzultzaileWeb.Itzuli("Se ha subido un nuevo documento en adok que requiere su revisión.") & "<br><br>" & ItzultzaileWeb.Itzuli("Puede acceder en el siguiente link:") & "  " & System.Configuration.ConfigurationManager.AppSettings("sitioAvisos").ToString & "/ValidacionSS.aspx", oParams.ServidorEmail)
                        End If
                        If CInt(idDoc.Value) = 163 And (rblImpuestos.SelectedValue = 2 Or rblImpuestos.SelectedValue = 3) Then
                            SabLib.BLL.Utils.EnviarEmail("MatrizPuesto" & "@batz.es", listaDoc(0).Listacorreos, ItzultzaileWeb.Itzuli("Nuevo documento a revisar"), ItzultzaileWeb.Itzuli("Se ha subido un nuevo documento en adok que requiere su revisión.") & "<br><br>" & ItzultzaileWeb.Itzuli("Puede acceder en el siguiente link:") & "  " & System.Configuration.ConfigurationManager.AppSettings("sitioAvisos").ToString & "/ValidacionImp.aspx", oParams.ServidorEmail)
                        End If


                    End If

                End If 'caducado o sin caducidad



                Master.MensajeInfo = ItzultzaileWeb.Itzuli("El documento se ha guardado correctamente")

            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba el documento")
            End If

            mView.ActiveViewIndex = 1
            Initialize()
            BindDataView2()
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al grabar el documento", ex)
        End Try
    End Sub
    Protected Sub btnCancelar3_Click(sender As Object, e As EventArgs) Handles CancelVista3.Click
        BindDataView2()
        mView.ActiveViewIndex = 1
    End Sub

    Protected Sub CancelVista_Click(sender As Object, e As EventArgs) Handles CancelVista.Click
        mView.ActiveViewIndex = 1
    End Sub

    Protected Sub btoVolver_click(sender As Object, e As EventArgs) Handles btoVolver.Click
        Try
            Response.Redirect("~/EstadoEmpresaDocumento9.aspx?pagina=" & gvType.PageIndex, False)

        Catch ex As Exception
            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al ver los trabajadores")
        End Try


    End Sub

    Protected Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        mView.ActiveViewIndex = 1
    End Sub

    Protected Sub trabajadores_Click(sender As Object, e As EventArgs) Handles btnTrabajadores.Click
        Try

            If PageBase.intRol = 0 Then
                Response.Redirect("EstadoTrabajadorDocumento9.aspx?empresa=" & idEmpresa.ToString, True)
            Else
                Response.Redirect("EstadoTrabajadorDocumentoResp9.aspx?empresa=" & idEmpresa.ToString, True)
            End If
        Catch ex As Exception
            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al ver los trabajadores")
        End Try
    End Sub

    Private Sub gvType2_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvType2.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim imgEstado As LinkButton = CType(e.Row.FindControl("imgEstado"), LinkButton)
            imgEstado.Attributes.Add("onClick", "return false;")

            Dim imgEstado2 As LinkButton = CType(e.Row.FindControl("imgEstado2"), LinkButton)
            imgEstado2.Attributes.Add("onClick", "return false;")


        End If
    End Sub

    Private Sub gvType_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvType.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim imgEstado As LinkButton = CType(e.Row.FindControl("imgEstado"), LinkButton)
            imgEstado.Attributes.Add("onClick", "return false;")

        End If
    End Sub
    Private Sub TxtFechaVal_TextChanged(sender As Object, e As EventArgs) Handles TxtFechaVal.TextChanged
        Dim tmp1 As Date
        Dim numdias As Double
        Dim tmp2 As Date
        Dim tmp3 As Date
        If IsDate(TxtFechaVal.Text) Then


            tmp1 = CDate(FechaValG.Value)
            tmp3 = CDate(TxtFechaVal.Text)
            numdias = DateDiff(DateInterval.Day, tmp1, tmp3)

            tmp2 = CDate(FechaValG2.Value)

            tmp2 = DateAdd(DateInterval.Day, numdias, tmp2)
            TxtFechaVal2.Text = tmp2.ToShortDateString
        End If

    End Sub

    Protected Sub btnBatz_Click(sender As Object, e As EventArgs) 'Handles btnBatz.Click
        Try

            PageBase.plantaAdmin = 1
            Initialize()
            BindDataView2()
        Catch ex As Exception

            Throw New SabLib.BatzException("Error al descargar el certificado", ex)
        End Try

    End Sub

    Protected Sub btnGuardarNuevaReferencia_Click(sender As Object, e As EventArgs) 'Handles btnGuardarNuevaSolicitud.Click
        Try
            '   btnCancelar.Attributes.Add("onclick", "window.close();")
            'grabar en ADOK_CER Planta, coddoc, codemp, aceptado= 1, systame
            Dim codigo As Int32 = idDoc.Value
            Dim codemp As Int32 = idEmpresa.Value
            'comprobar si es repetido el click
            Dim listaType3 As List(Of ELL.EmpresasDoc)
            listaType3 = oDocumentosBLL.CargarListaEmpDocAsignadosTraCer3(PageBase.plantaAdmin, codigo, codemp, Now.AddMinutes(-9))

            If (listaType3.Count = 0) Then 'NO ha dicho que si tiene el certificado. 
                oDocumentosBLL.GuardarCer(PageBase.plantaAdmin, codigo, codemp, 0, Session("Ticket").nombreusuario)
            End If

            'volver a cargar la pagina para que saque un segundo popup
            'Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)


        Catch ex As Exception

            Throw New SabLib.BatzException("Error al descargar el certificado", ex)
        End Try

    End Sub
    Protected Sub btnArraluce_Click(sender As Object, e As EventArgs) ' Handles btnArraluce_Click.Click
        Try
            PageBase.plantaAdmin = 230
            Initialize()
            BindDataView2()
        Catch ex As Exception

            Throw New SabLib.BatzException("Error al descargar el certificado", ex)
        End Try

    End Sub
    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) ' Handles btnCancelar_Click.Click
        Try
            '    btnCancelar.Attributes.Add("onclick", "window.close();")
            'grabar en ADOK_CER Planta, coddoc, codemp, aceptado= 1, systame
            Dim codigo As Int32 = idDoc.Value
            Dim codemp As Int32 = idEmpresa.Value

            'comprobar si es repetido el click
            Dim listaType3 As List(Of ELL.EmpresasDoc)
            listaType3 = oDocumentosBLL.CargarListaEmpDocAsignadosTraCer3(PageBase.plantaAdmin, codigo, codemp, Now.AddMinutes(-9))

            If (listaType3.Count = 0) Then 'NO ha dicho que si tiene el certificado. 
                oDocumentosBLL.GuardarCer(PageBase.plantaAdmin, codigo, codemp, 1, Session("Ticket").nombreusuario)
            End If


        Catch ex As Exception

            Throw New SabLib.BatzException("Error al descargar el certificado", ex)
        End Try

    End Sub

    Protected Sub btnCancelar4_Click(sender As Object, e As EventArgs) ' Handles btnCancelar_Click.Click
        Try
            Response.Redirect("~/CrearTrabajadorSolicitud.aspx", False)
        Catch ex As Exception

            Throw New SabLib.BatzException("Error ", ex)
        End Try

    End Sub

    Protected Sub Descargar2_Click(sender As Object, e As EventArgs)
        Try
            'Plantilla a leer
            'buscar en pla la plantilla de ese doc es pla005
            Dim idDocumento As String = idDoc.Value
            Dim liPlantilla As List(Of ELL.Plantillas)
            liPlantilla = oDocumentosBLL.CargarListaPla(PageBase.plantaAdmin, CInt(idDocumento))

            If liPlantilla.Count > 0 Then
                'idPlantilla0.Value = liPlantilla(0).nombre

                If liPlantilla(0).nombre <> "" Then
                    idDocumento = DirFicherosBajar & "Ficheros_Matriz_Puestos/" & liPlantilla(0).nombre


                    Dim script As String = "<script type=""text/javascript"">window.open('" & idDocumento & "');</script>"
                    ClientScript.RegisterStartupScript(Me.GetType, "openWindow", script)


                    Dim codigo As Int32 = idDoc.Value
                    Dim codemp As Int32 = idEmpresa.Value
                    'comprobar si es repetido el click
                    Dim listaType3 As List(Of ELL.EmpresasDoc)
                    listaType3 = oDocumentosBLL.CargarListaEmpDocAsignadosTraCer3(PageBase.plantaAdmin, codigo, codemp, Now.AddMinutes(-9))

                    If (listaType3.Count = 0) Then 'NO ha dicho que si tiene el certificado. 
                        oDocumentosBLL.GuardarCer(PageBase.plantaAdmin, codigo, codemp, 0, Session("Ticket").nombreusuario)
                    End If

                Else
                    '      Master.MensajeInfo = ItzultzaileWeb.Itzuli("No se ha encontrado el documento ").ToUpper 'error 
                End If

            Else
                '   Master.MensajeInfo = ItzultzaileWeb.Itzuli("Plantilla no subida ").ToUpper 'error 
            End If


        Catch ex As Exception

            Throw New SabLib.BatzException("Error al descargar la plantilla", ex)
        End Try

    End Sub


    Protected Sub btnGuardarNuevaReferencia3_Click(sender As Object, e As EventArgs) 'Handles btnGuardarNuevaSolicitud.Click

        Try

            Dim idDocumento As String = idDoc.Value
            Dim liPlantilla As List(Of ELL.Plantillas)
            liPlantilla = oDocumentosBLL.CargarListaPla(PageBase.plantaAdmin, CInt(idDocumento))

            'Plantilla a leer
            'buscar en pla la plantilla de ese doc es pla005


            If liPlantilla.Count > 0 Then

                If liPlantilla(0).nombre <> "" Then
                    idDocumento = DirFicherosBajar & "Ficheros_Matriz_Puestos/" & liPlantilla(0).nombre
                    'Process.Start(idDocumento)
                    Dim script As String = "<script type=""text/javascript"">window.open('" & idDocumento & "');</script>"
                    ClientScript.RegisterStartupScript(Me.GetType, "openWindow", script)

                    Dim codigo As Int32 = idDoc.Value
                    Dim codemp As Int32 = idEmpresa.Value
                    'comprobar si es repetido el click
                    Dim listaType3 As List(Of ELL.EmpresasDoc)
                    listaType3 = oDocumentosBLL.CargarListaEmpDocAsignadosTraCer3(PageBase.plantaAdmin, codigo, codemp, Now.AddMinutes(-9))

                    If (listaType3.Count = 0) Then 'NO ha dicho que si tiene el certificado. 
                        oDocumentosBLL.GuardarCer(PageBase.plantaAdmin, codigo, codemp, 0, Session("Ticket").nombreusuario)
                    End If
                Else
                    '      Master.MensajeInfo = ItzultzaileWeb.Itzuli("No se ha encontrado el documento ").ToUpper 'error 
                End If

            Else
                '   Master.MensajeInfo = ItzultzaileWeb.Itzuli("Plantilla no subida ").ToUpper 'error 
            End If
        Catch ex As Exception

            Throw New SabLib.BatzException("Error al descargar la plantilla", ex)
        End Try

    End Sub

    Private Sub ddlCaducidad_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCaducidad.SelectedIndexChanged
        Dim fechaValidez As Date
        Dim fechaCad As Date
        Dim cantidad As Int32
        Dim intervalo As String
        Dim periodo As Int32
        Dim oDocBLL As New BLL.DocumentosBLL

        periodo = ddlCaducidad.SelectedValue

        Dim listaCad As List(Of ELL.Caducidades)
        listaCad = oDocBLL.CargarCad(PageBase.plantaAdmin, periodo)

        cantidad = listaCad(0).cantidad

        'leer int001
        listaCad = oDocBLL.CargarInt(PageBase.plantaAdmin, listaCad(0).intervalo)
        intervalo = Trim(listaCad(0).nombre)


        If intervalo <> "nnnn" And Trim(TxtFechaVal.Text) <> "" And IsDate(TxtFechaVal.Text) Then
            fechaValidez = CDate(TxtFechaVal.Text)
            If cantidad = 0 Then 'sin periodicidad
                fechaCad = Date.MaxValue
            Else
                fechaCad = DateAdd(intervalo, cantidad, fechaValidez)
            End If

        Else
            fechaValidez = Date.MinValue
            fechaCad = Date.MaxValue
        End If


        TxtFechaVal2.Text = fechaCad.ToShortDateString
    End Sub


End Class