
Imports System.Data


Public Class EstadoTrabajadorDocumentoRRHH
    Inherits PageBase

    'Inherits System.Web.UI.Page

    Dim oDocumentosBLL As New BLL.DocumentosBLL
    Dim idTrabajadorParam As Int32 = 0


#Region "Propiedades"

    Protected Overrides ReadOnly Property RolesAcceso As System.Collections.Generic.List(Of ELL.Roles.RolUsuario)
        Get
            Dim roles As New List(Of ELL.Roles.RolUsuario)
            roles.Add(ELL.Roles.RolUsuario.Administrador)
            roles.Add(ELL.Roles.RolUsuario.Administrador2)
            roles.Add(ELL.Roles.RolUsuario.Supervisores)
            roles.Add(ELL.Roles.RolUsuario.Financiero)
            roles.Add(ELL.Roles.RolUsuario.RRHH)
            Return roles
        End Get
    End Property

#End Region


#Region "METODOSrol"


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
            If ((RolesAcceso Is Nothing OrElse Not tieneAcceso) AndAlso Not (PerfilUsuario.IdRol = ELL.Roles.RolUsuario.Administrador) AndAlso Not (PerfilUsuario.IdRol = ELL.Roles.RolUsuario.Financiero)) Then
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

    Protected Sub BindDataViewlimpia()

        Try
            ClearGridView()

            Dim oDocBLL As New BLL.DocumentosBLL
            Dim trabajadores As List(Of ELL.Trabajadores)
            If hfEmpresa.Value <> "" Then
                trabajadores = oDocBLL.CargarListaTrabajadorestexto(PageBase.plantaAdmin, Split(hfEmpresa.Value, ",")(1), Split(hfEmpresa.Value, ",")(0))
            Else

                If Not idTrabajadorParam = 0 Then
                    trabajadores = oDocBLL.CargarListaTrabajadoresClaveEmpEstado(PageBase.plantaAdmin, idTrabajadorParam)

                Else
                    trabajadores = oDocBLL.CargarListaTraActivos(PageBase.plantaAdmin)
                End If


            End If
            '   listaType = oDocBLL.CargarListaTrabajadoresClave(PageBase.plantaAdmin, idTrabajadorParam) 'de momento, quitarlo
            If (trabajadores.Count > 0) Then


                gvType.DataSource = trabajadores
                gvType.DataBind()
                gvType.Caption = String.Empty
            Else

                gvType.DataSource = Nothing
                gvType.DataBind()
                gvType.Caption = ""
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
            Dim trabajadores As List(Of ELL.Trabajadores)

            If hfEmpresa.Value <> "" Then

                trabajadores = oDocBLL.CargarListaTrabajadorestexto(PageBase.plantaAdmin, Split(hfEmpresa.Value, ",")(1), Split(hfEmpresa.Value, ",")(0))

            Else

                If Not idTrabajadorParam = 0 Then
                    'aqui añadir empresa + trabajador
                    If hfTra.Value <> "" Then
                        'Dim trabajadorTxt As String
                        'trabajadorTxt = hfTra.Value
                        trabajadores = oDocBLL.CargarListaTrabajadorestexto(PageBase.plantaAdmin, Split(hfTra.Value, ",")(1), Split(hfTra.Value, ",")(0))

                    Else
                        trabajadores = oDocBLL.CargarListaTrabajadoresClaveEmpEstado(PageBase.plantaAdmin, idTrabajadorParam)
                    End If

                Else
                    trabajadores = oDocBLL.CargarListaTraActivosNoCad(666)
                End If



            End If
            '   listaType = oDocBLL.CargarListaTrabajadoresClave(PageBase.plantaAdmin, idTrabajadorParam) 'de momento, quitarlo
            If (trabajadores.Count > 0) Then
                'desde aqui para errores 
                Dim j As Int32
                Dim i As Int32
                Dim listaType2 As List(Of ELL.TrabajadoresDoc)

                Dim paginado As Integer
                If gvType.PageIndex > 0 Then
                    paginado = trabajadores.Count - 1
                Else
                    If trabajadores.Count > 15 Then
                        paginado = 15
                    Else
                        paginado = trabajadores.Count - 1
                    End If
                End If
                For j = 0 To paginado '  trabajadores.Count - 1 (quito paginacion para mas rapido, solo calcula 10)

                    trabajadores(j).responsable = 0 'de momento responsable como error activo como aviso
                    trabajadores(j).activo = 0

                    'sacar desc empresa
                    Dim listaEmp As List(Of ELL.Empresas)
                    listaEmp = oDocBLL.CargarTiposEmpresa(trabajadores(j).Empresa, PageBase.plantaAdmin)
                    If listaEmp.Count > 0 Then
                        trabajadores(j).DescEmpresa = listaEmp(0).Nombre
                    End If



                    listaType2 = oDocBLL.CargarListaTraDocAsignados(PageBase.plantaAdmin, trabajadores(j).Id)
                    If (listaType2.Count > 0) Then
                        Dim lista9 As List(Of ELL.Trabajadores)
                        Dim userBLL As New SabLib.BLL.UsuariosComponent
                        lista9 = oDocBLL.CargarTiposTrabajadorActivos(PageBase.plantaAdmin, trabajadores(j).Id)

                        For i = 0 To listaType2.Count - 1
                            Dim no_correcto As Int16 = 0
                            'mirar si para esa empresa es obligatorio un doc segun  su PageBase.plantaAdmin, listaType2(i).clave (es doc) y empresa=lista9(0).Empresa(lo saco con codtra)  y ponerlo


                            Dim listaType9 As List(Of ELL.EmpresasDoc)
                            listaType9 = oDocBLL.CargarListaTraDocObligatorio(PageBase.plantaAdmin, listaType2(i).clave, lista9(0).Empresa)
                            If listaType9.Count > 0 Then
                                listaType2(i).obligatorio = True
                            End If

                            'estado
                            If listaType2(i).FecRec = Date.MinValue Then
                                no_correcto = 1

                                listaType2(i).estado = "Dcto. no entregado"

                            Else
                                If listaType2(i).FecRec = Date.MinValue Then
                                    no_correcto = 1

                                    listaType2(i).estado = "Caducidad no asignada"

                                Else

                                    If listaType2(i).FecCad < Now And listaType2(i).FecCad <> Date.MinValue Then '"01/01/1900"  CDate(Now.ToShortDateString)
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
                                                listaType2(i).estado = "Validez caducada"
                                            Else
                                                listaType2(i).estado = "Validez caducada, pero dentro de margen"
                                                no_correcto = 2
                                            End If
                                        End If

                                    Else
                                        'si esta puesto incorrecto en la mod del doc se pone asi
                                        If listaType2(i).correcto = 1 Then
                                            no_correcto = 1
                                            listaType2(i).estado = "Marcado No Apto"
                                        Else
                                            If listaType2(i).correcto = 2 Then
                                                no_correcto = 1
                                                listaType2(i).estado = "No validado"
                                            Else

                                                'fecha de validez si no es de plantilla
                                                If listaType2(i).periodicidad <> 13 And listaType2(i).FecIni = Date.MinValue And listaType2(i).plantilla = 0 Then
                                                    no_correcto = 1
                                                    listaType2(i).estado = "Falta fecha inicio de Validez"
                                                Else
                                                    no_correcto = 0
                                                    listaType2(i).estado = "Documento correcto"
                                                    If listaType2(i).Listacorreos <> "" Then

                                                        Select Case listaType2(i).Aptitud
                                                            Case 2
                                                                no_correcto = 2
                                                                listaType2(i).estado = "Apto con limitaciones."
                                                            Case 6
                                                                no_correcto = 2
                                                                listaType2(i).estado = "Especialmente Sensible"
                                                            Case 7
                                                                no_correcto = 2
                                                                listaType2(i).estado = ItzultzaileWeb.Itzuli("Cita para reconocimiento.")
                                                            Case 8
                                                                no_correcto = 2
                                                                listaType2(i).estado = ItzultzaileWeb.Itzuli("Apto provisional.")
                                                            Case 3
                                                                no_correcto = 1
                                                                listaType2(i).estado = "No apto"
                                                            Case 4
                                                                no_correcto = 1
                                                                listaType2(i).estado = ItzultzaileWeb.Itzuli("Rechaza reconocimiento")
                                                            Case 5
                                                                no_correcto = 1
                                                                listaType2(i).estado = "No validada aptitud"
                                                        End Select
                                                    End If

                                                End If

                                            End If

                                        End If

                                    End If

                                End If
                            End If
                            'mostramos un icono u otro en función de si es ok o no, o no es obligatorio
                            If no_correcto = 1 Then

                                listaType2(i).txtcorrecto = "No Apto"
                                trabajadores(j).responsable = trabajadores(j).responsable + 1

                            Else
                                If no_correcto = 2 Then
                                    listaType2(i).txtcorrecto = "Avisos"
                                    trabajadores(j).activo = trabajadores(j).activo + 1
                                Else
                                    listaType2(i).txtcorrecto = "Apto"
                                End If

                            End If


                            'leer si existe el registro en adok_emd cogido en campo ubicacion
                            If listaType2(i).ubicacion = "" Then
                                listaType2(i).ubicacion = "false" 'para quitar el boton
                            Else
                                listaType2(i).ubicacion = "true"
                            End If


                            'no aplica
                            If listaType2(i).correcto = 3 Then
                                no_correcto = 3
                                listaType2(i).FecCad = CDate("31/12/2099")
                                listaType2(i).estado = "No caduca"
                                listaType2(i).txtcorrecto = "No caduca"
                            End If

                        Next

                    End If

                    If trabajadores(j).responsable > 0 Then

                        If trabajadores(j).responsable > 1 Then
                            trabajadores(j).puesto = trabajadores(j).responsable.ToString & " " & ItzultzaileWeb.Itzuli("Errores")
                        Else
                            trabajadores(j).puesto = trabajadores(j).responsable.ToString & " " & ItzultzaileWeb.Itzuli("Error")
                        End If
                    Else

                        If trabajadores(j).activo > 0 Then

                            If trabajadores(j).activo > 1 Then
                                trabajadores(j).puesto = trabajadores(j).activo.ToString & " " & ItzultzaileWeb.Itzuli("Avisos")
                            Else
                                trabajadores(j).puesto = trabajadores(j).activo.ToString & " " & ItzultzaileWeb.Itzuli("Aviso")
                            End If
                        Else
                            trabajadores(j).puesto = "Apto"
                        End If
                    End If

                    If trabajadores(j).FecFin = Date.MinValue Then
                        trabajadores(j).FechaFin = ""
                    Else
                        trabajadores(j).FechaFin = trabajadores(j).FecFin.ToShortDateString
                    End If


                Next  'hasta aqui			

                gvType.DataSource = trabajadores
                gvType.DataBind()
                gvType.Caption = String.Empty
            Else
                gvType.DataSource = Nothing
                gvType.DataBind()
                gvType.Caption = ""
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar un documento", ex)
        End Try
    End Sub

    Protected Sub BindDataView2()
        Try
            Dim oDocBLL As New BLL.DocumentosBLL
            Dim i As Int16

            Dim codtra As Int32 = CInt(idTrabajador.Value)
            If codtra > 0 Then

                Dim lista9 As List(Of ELL.Trabajadores)
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                lista9 = oDocBLL.CargarTiposTrabajadorActivos(PageBase.plantaAdmin, codtra)






                Dim listaType2 As List(Of ELL.TrabajadoresDoc)

                listaType2 = oDocBLL.CargarListaTraDocAsignados(PageBase.plantaAdmin, codtra)
                If (listaType2.Count > 0) Then


                    For i = 0 To listaType2.Count - 1
                        Dim no_correcto As Int16 = 0

                        'mirar si para esa empresa es obligatorio un doc segun  su PageBase.plantaAdmin, listaType2(i).clave (es doc) y empresa=lista9(0).Empresa(lo saco con codtra)  y ponerlo


                        Dim listaType9 As List(Of ELL.EmpresasDoc)
                        listaType9 = oDocBLL.CargarListaTraDocObligatorio(PageBase.plantaAdmin, listaType2(i).clave, lista9(0).Empresa)
                        If listaType9.Count > 0 Then
                            listaType2(i).obligatorio = True
                        End If



                        'estado
                        If listaType2(i).FecRec = Date.MinValue Then
                            no_correcto = 1

                            listaType2(i).estado = "Dcto. no entregado"

                        Else
                            If listaType2(i).FecRec = Date.MinValue Then
                                no_correcto = 1

                                listaType2(i).estado = "Caducidad no asignada"

                            Else

                                If listaType2(i).FecCad < Now And listaType2(i).FecCad <> Date.MinValue Then '"01/01/1900"  CDate(Now.ToShortDateString)
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
                                            listaType2(i).estado = "Validez caducada"
                                        Else
                                            listaType2(i).estado = "Validez caducada, pero dentro de margen"
                                            no_correcto = 2
                                        End If
                                    End If


                                Else
                                    'si esta puesto incorrecto en la mod del doc se pone asi
                                    If listaType2(i).correcto = 1 Then
                                        no_correcto = 1
                                        listaType2(i).estado = "Marcado No Apto"
                                    Else

                                        If listaType2(i).correcto = 2 Then
                                            no_correcto = 1
                                            listaType2(i).estado = "No validado"
                                        Else

                                            'fecha de validez si no es de plantilla
                                            If listaType2(i).periodicidad <> 13 And listaType2(i).FecIni = Date.MinValue And listaType2(i).plantilla = 0 Then
                                                no_correcto = 1
                                                listaType2(i).estado = "Falta fecha inicio de Validez"
                                            Else
                                                no_correcto = 0
                                                listaType2(i).estado = "Apto"
                                                If listaType2(i).Listacorreos <> "" Then

                                                    Select Case listaType2(i).Aptitud
                                                        Case 2
                                                            no_correcto = 2
                                                            listaType2(i).estado = "Apto con limitaciones."
                                                        Case 6
                                                            no_correcto = 2
                                                            listaType2(i).estado = "Especialmente Sensible"
                                                        Case 7
                                                            no_correcto = 2
                                                            listaType2(i).estado = ItzultzaileWeb.Itzuli("Cita para reconocimiento.")
                                                        Case 8
                                                            no_correcto = 2
                                                            listaType2(i).estado = ItzultzaileWeb.Itzuli("Apto provisional.")
                                                        Case 3
                                                            no_correcto = 1
                                                            listaType2(i).estado = "No apto"
                                                        Case 4
                                                            no_correcto = 1
                                                            listaType2(i).estado = ItzultzaileWeb.Itzuli("Rechaza reconocimiento")
                                                        Case 5
                                                            no_correcto = 1
                                                            listaType2(i).estado = "No validada aptitud"
                                                    End Select
                                                End If

                                            End If
                                        End If
                                    End If

                                End If


                            End If
                        End If
                        'mostramos un icono u otro en función de si es ok o no, o no es obligatorio
                        If no_correcto = 1 Then

                            listaType2(i).txtcorrecto = "No Apto"
                        Else
                            If no_correcto = 2 Then
                                listaType2(i).txtcorrecto = "Avisos"
                            Else
                                listaType2(i).txtcorrecto = "Apto"
                            End If

                        End If


                        'leer si existe el registro en adok_emd cogido en campo ubicacion
                        If listaType2(i).ubicacion = "" Then
                            listaType2(i).ubicacion = "false" 'para quitar el boton
                        Else
                            listaType2(i).ubicacion = "true"
                        End If

                        listaType2(i).ubicacionHist = listaType2(i).ubicacion
                        If listaType2(i).clave = "123" Then
                            listaType2(i).ubicacionHist = "true" ' "hist del 123" 'para quitar el boton                            
                        End If

                        'leer si existe plantilla
                        If listaType2(i).Planta = 0 Then 'temporalmente he usado planta para meter valor de si es de empresa
                            listaType2(i).plantilla = 0

                        End If




                        If listaType2(i).EsDocumento = 6 Then   'tipo de documento doc012= 6 es carne 
                            listaType2(i).Nombre = listaType2(i).Comentario.Replace(";", "<br>")
                            'listaType2(i).Nombre = listaType2(i).Nombre & "<br>" & 
                        End If




                        'no aplica
                        If listaType2(i).correcto = 3 Then
                            no_correcto = 3
                            listaType2(i).FecCad = CDate("31/12/2099")
                            listaType2(i).estado = "No caduca"
                            listaType2(i).txtcorrecto = "No caduca"
                        End If

                    Next


                    Registros.Text = listaType2.Count.ToString
                    '   Registros2.Text = listaType2.Count.ToString
                    gvType2.DataSource = listaType2
                    gvType2.DataBind()
                    gvType2.Caption = String.Empty
                Else
                    Registros.Text = "0"
                    '     Registros2.Text = "0"
                    gvType2.DataSource = Nothing
                    gvType2.DataBind()
                    gvType2.Caption = "No hay documentos asignados"
                End If


            Else
                Dim listaType2 As List(Of ELL.Documentos)
                listaType2 = oDocBLL.CargarListaTraDocTot(PageBase.plantaAdmin)
                If (listaType2.Count > 0) Then
                    Registros.Text = listaType2.Count.ToString
                    '      Registros2.Text = listaType2.Count.ToString
                    gvType2.DataSource = listaType2
                    gvType2.DataBind()
                    gvType2.Caption = String.Empty

                Else
                    Registros.Text = "0"
                    '     Registros2.Text = "0"
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


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            'comprobar los documentos obligatorios de cada puesto, y para las personas de ese puesto se les pone, si se ha seleccionado puesto y si no
            Dim oDocBLL As New BLL.DocumentosBLL
            Dim lista As List(Of ELL.Empresas)
            Dim lista4 As List(Of ELL.Empresas)
            If Request.QueryString("empresa") <> "" Then
                lista = oDocBLL.CargarTiposEmpresa(CInt(Request.QueryString("empresa")), PageBase.plantaAdmin)
                If (lista.Count > 0) Then



                    Dim listaType2 As List(Of ELL.EmpresasDoc)
                    listaType2 = oDocBLL.CargarListaEmpDocAsignados(PageBase.plantaAdmin, CInt(Request.QueryString("empresa")))
                    If (listaType2.Count > 0) Then 'son los docs de un puesto
                        For i = 0 To listaType2.Count - 1
                            'actualizar a los trabajadores de este puesto con sus docs, en blanco si no estan


                            Dim listaType2w As List(Of ELL.Empresas)
                            listaType2w = oDocBLL.CargarTiposTrabajador(CInt(Request.QueryString("empresa")), PageBase.plantaAdmin)
                            If (listaType2w.Count > 0) Then 'son los trabajadores de un puesto
                                Dim bucle As Integer
                                If listaType2w.Count > 19 Then
                                    bucle = 20
                                Else
                                    bucle = listaType2w.Count
                                End If

                                For j = 0 To bucle - 1
                                    'actualizar a cada trabajador con este doc en blanco si no lo tiene
                                    'y si tiene trd009 a 1 ponerlo a 0
                                    'doc listaType2(i).clave
                                    'tra  listaType2w(j).id
                                    lista4 = oDocBLL.CargarTiposEmpresa2(listaType2(i).clave, listaType2w(j).Id)
                                    If (lista4.Count = 0) Then 'se inserta en blanco


                                        Dim tipo2 As New ELL.TrabajadoresDoc With {.Planta = PageBase.plantaAdmin, .codtra = listaType2w(j).Id, .coddoc = listaType2(i).clave, .periodicidad = listaType2(i).periodicidad}
                                        oDocumentosBLL.ModificarTraDoc(tipo2, 0, Session("Ticket").nombreusuario)   'UpdateEmpDoc

                                    Else 'puede ser que lo tengas de antes al cambiar de puesto, con trd009 = 1, ponerlo a 0
                                        Dim tipo2 As New ELL.TrabajadoresDoc With {.Planta = PageBase.plantaAdmin, .codtra = listaType2w(j).Id, .coddoc = listaType2(i).clave, .periodicidad = listaType2(i).periodicidad}
                                        oDocumentosBLL.ModificarTraDoc2(tipo2, 0, Session("Ticket").nombreusuario)
                                    End If

                                Next

                            End If




                        Next

                    End If
                End If

            Else

                'lista = oDocBLL.CargarEmpresas()
                'For k = 0 To lista.Count - 1

                '    Dim listaType2 As List(Of ELL.EmpresasDoc)
                '    listaType2 = oDocBLL.CargarListaEmpDocAsignados(PageBase.plantaAdmin, lista(k).Id)
                '    If (listaType2.Count > 0) Then 'son los docs de un puesto
                '        For i = 0 To listaType2.Count - 1
                '            'actualizar a los trabajadores de este puesto con sus docs, en blanco si no estan
                '            'hacer un bucle con todos los trabajadores de esa empresa lista(k).Id


                '            Dim listaType3 As List(Of ELL.Trabajadores)
                '            listaType3 = oDocBLL.loadTrabajadores(lista(k).Id)
                '            If (listaType3.Count > 0) Then 'son los docs de un puesto
                '                For m = 0 To listaType3.Count - 1




                '                    Dim listaType2w As List(Of ELL.Empresas)
                '                    listaType2w = oDocBLL.CargarTiposTrabajador(lista(k).Id, PageBase.plantaAdmin)

                '                    If (listaType2w.Count > 0) Then 'son los trabajadores de un puesto
                '                        For j = 0 To listaType2w.Count - 1
                '                            'actualizar a cada trabajador con este doc en blanco si no lo tiene
                '                            'doc listaType2(i).clave
                '                            'tra  listaType2w(j).id
                '                            lista4 = oDocBLL.CargarTiposEmpresa2(listaType2(i).clave, listaType2w(j).Id)
                '                            If (lista4.Count = 0) Then 'se inserta en blanco


                '                                Dim tipo2 As New ELL.TrabajadoresDoc With {.Planta = PageBase.plantaAdmin, .codtra = listaType2w(j).Id, .coddoc = listaType2(i).clave, .periodicidad = listaType2(i).periodicidad}
                '                                oDocumentosBLL.ModificarTraDoc(tipo2, 0, Session("Ticket").nombreusuario)   'UpdateEmpDoc

                '                            Else 'puede ser que lo tengas de antes al cambiar de puesto, con trd009 = 1, ponerlo a 0
                '                                Dim tipo2 As New ELL.TrabajadoresDoc With {.Planta = PageBase.plantaAdmin, .codtra = listaType2w(j).Id, .coddoc = listaType2(i).clave, .periodicidad = listaType2(i).periodicidad}
                '                                oDocumentosBLL.ModificarTraDoc2(tipo2, 0, Session("Ticket").nombreusuario)


                '                            End If

                '                        Next

                '                    End If


                '                Next
                '            End If

                '        Next

                '    End If
                'Next

            End If







            txtTra.Visible = False
            lblTra.Visible = False

            ComprobarAcceso()
            TxtFechaValidez.ReadOnly = False

            txtComentario.Attributes.Add("placeholder", ItzultzaileWeb.Itzuli("Comentario"))
            TxtFechaValidez.Attributes.Add("placeholder", ItzultzaileWeb.Itzuli("Fecha de entrada en vigor"))

            botonVolver.Visible = False
            btoVolver.Visible = False


            If Request.Params.AllKeys(0) = "empresa" Then
                idTrabajadorParam = CInt(Request.QueryString("empresa"))
                PageBase.empresaBusqueda = idTrabajadorParam
                txtTra.Visible = True
                lblTra.Visible = True

                btoVolver.Visible = True

                'Dim oDocBLL As New BLL.DocumentosBLL
                'Dim lista As List(Of ELL.Empresas)
                lista = oDocBLL.CargarTiposEmpresa(idTrabajadorParam, PageBase.plantaAdmin)

                txtEmpresa.Text = lista(0).Nombre
                txtEmpresa.ReadOnly = "true"
                ex177.Text = "Puesto"
                botonVolver.Visible = True

            Else
                If idTrabajador.Value <> "" Then
                    idTrabajadorParam = CInt(idTrabajador.Value)
                Else
                    idTrabajadorParam = 0
                End If

            End If




            '           If Not (Page.IsPostBack) Then
            txtEmpresa.Focus()
            mView.ActiveViewIndex = 0
            If Not (Page.IsPostBack) Then
                If Request.QueryString("pagina") <> "" Then
                    gvType.PageIndex = CInt(Request.QueryString("pagina"))
                End If

            End If
            Initialize()

        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar el estado de documentos de un trabajador", ex)
        End Try
    End Sub



    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' <para>Redirecciona al detalle del item seleccionado</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvType_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvType.RowCommand

        If e.CommandName = "Edit" Then
            '    BindDataView()
            Dim IdEmpre As Int32 = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).id
            Dim DesEmpre As String = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).nombre
            idTrabajador.Value = IdEmpre
            DescEmpresa.Value = DesEmpre
            BindDataView2()  'para limpiar el grid
            ConfiguracionProduct(IdEmpre)

        End If


    End Sub
    Protected Sub gvTypeHis_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvTypeHis.RowCommand
        If e.CommandName = "Desactivar" Then
            Dim strDoc As Object = DirectCast(((DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.Controls(1)).Controls(1)), System.Web.UI.WebControls.Label).[Text]
            Dim Iddoc2 As Int32 = CInt(strDoc)

            'con trd008 buscar trd001 como coddoc
            Dim listaEmpDoc2 As List(Of ELL.TrabajadoresDoc)
            Dim tipo2 As New ELL.TrabajadoresDoc With {.clave = Iddoc2}
            listaEmpDoc2 = oDocumentosBLL.LeerTraDocHisClave(tipo2)


            Dim TipoTra As New ELL.Trabajadores With {.Planta = PageBase.plantaAdmin, .activo = 1, .Id = Iddoc2}
            If (oDocumentosBLL.ModificarDOSTra(TipoTra)) Then

                Master.MensajeInfo = ItzultzaileWeb.Itzuli("El documento se ha borrado correctamente").ToUpper

                Dim listaEmpDoc As List(Of ELL.TrabajadoresDoc)
                'buscar en adok_emd
                'Dim tipo As New ELL.EmpresasDoc With {.Planta = PageBase.plantaAdmin, .codemp = CInt(idDoc.Value)}
                Dim tipo As New ELL.TrabajadoresDoc With {.Planta = PageBase.plantaAdmin, .codtra = CInt(idTrabajador.Value), .coddoc = listaEmpDoc2(0).coddoc}


                listaEmpDoc = oDocumentosBLL.LeerTraDocHis(tipo)
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

            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se borraba el documento").ToUpper
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

            'sacar el nombre del tipo de doc

            Dim listaDoc As List(Of ELL.Documentos)

            listaDoc = oDocumentosBLL.CargarDocumentos(CInt(idDocumento), PageBase.plantaAdmin)
            If listaDoc.Count > 0 Then
                txtDocuEmp.Text = listaDoc(0).Nombre
            End If


            Dim listaEmpDoc As List(Of ELL.TrabajadoresDoc)
            'buscar en adok_emd
            Dim tipo As New ELL.TrabajadoresDoc With {.Planta = PageBase.plantaAdmin, .codtra = CInt(idTrabajador.Value), .coddoc = CInt(idDocumento)}

            listaEmpDoc = oDocumentosBLL.LeerTraDocHis(tipo)
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


            txtNombre2.Text = txtNombre2.Text & "   DNI= " & txtCIF.Text
            'cargar caducidad
            ddlCaducidad2.Items.Clear()

            Dim listaCad As List(Of ELL.Caducidades)
            listaCad = oDocumentosBLL.CargarListaCad(PageBase.plantaAdmin)

            For Each caducidad In listaCad
                Dim licaducidad As New ListItem(caducidad.nombre, caducidad.id)
                ddlCaducidad2.Items.Add(licaducidad)

            Next


            'buscar en adok_trd
            Dim listaDoc2 As List(Of ELL.TrabajadoresDoc)
            listaDoc2 = oDocumentosBLL.CargarListaTraDoc(PageBase.plantaAdmin, CInt(idTrabajador.Value), CInt(idDocumento))
            If listaDoc2.Count > 0 Then
                ddlCaducidad2.SelectedValue = listaDoc2(0).periodicidad









                If listaDoc2(0).tiposCarne <> "" Then
                    Dim roles As String() = Nothing
                    roles = listaDoc2(0).tiposCarne.Split(";")
                    If roles.Count > 1 Then



                    End If

                End If














            End If


            Dim listaEmpDoc As List(Of ELL.TrabajadoresDoc)
            'buscar en adok_emd
            Dim tipo As New ELL.TrabajadoresDoc With {.Planta = PageBase.plantaAdmin, .codtra = idTrabajador.Value, .coddoc = idDoc.Value}


            listaEmpDoc = oDocumentosBLL.LeerTraDoc(tipo)
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

                'TxtFechaVal.Enabled = True
                'TxtFechaVal.ReadOnly = False
                'imgCalendarioFechaVal.Visible = True
                'If listaEmpDoc(0).FecIni > Date.MinValue Then
                '    TxtFechaVal.Text = listaEmpDoc(0).FecIni.ToShortDateString
                '    FechaValG.Value = listaEmpDoc(0).FecIni.ToShortDateString
                'Else

                '    TxtFechaVal.Text = ""
                'End If


                'If listaEmpDoc(0).periodicidad > 0 Then
                '    ddlCaducidad.SelectedValue = listaEmpDoc(0).periodicidad
                'Else
                '    ddlCaducidad.SelectedValue = 13
                'End If


                If listaEmpDoc(0).periodicidad = 13 Or listaEmpDoc(0).periodicidad = 998 Then 'no tiene caducidad
                    listaEmpDoc(0).periodicidad = 13
                    'TxtFechaVal.ReadOnly = True
                    'imgCalendarioFechaVal.Visible = False
                    'TxtFechaVal.Text = "Documento sin caducidad"
                End If

                Dim listaDocu As List(Of ELL.Documentos)
                listaDocu = oDocumentosBLL.CargarDocumentos(CInt(idDoc.Value), PageBase.plantaAdmin)
                If listaDocu(0).Plantilla = 1 Then 'no tiene caducidad
                    'TxtFechaVal.ReadOnly = True
                    'imgCalendarioFechaVal.Visible = False
                    'TxtFechaVal.Text = ItzultzaileWeb.Itzuli("Documento de plantilla")
                    TxtFechaVal2.Text = ItzultzaileWeb.Itzuli("Sin caducidad")
                Else
                    TxtFechaVal2.Text = listaEmpDoc(0).FecCad.ToShortDateString

                End If


                txtUbicacion.Text = listaEmpDoc(0).ubicacionfisica
                rblCorrecto.SelectedValue = listaEmpDoc(0).correcto

                FechaValG2.Value = listaEmpDoc(0).FecCad.ToShortDateString

                'rblCorrecto.Items.Clear()
                'Dim strCorrecto As String = ItzultzaileWeb.Itzuli("Correcto")
                'Dim liCorrecto0 As New ListItem(" -" & strCorrecto & " ", 0)
                'rblCorrecto.Items.Add(liCorrecto0)
                'Dim liCorrecto1 As New ListItem(Chr(12) & Chr(12) & ItzultzaileWeb.Itzuli("Apto.") & Chr(12) & Chr(12) & Chr(12) & Chr(12), 1)
                'rblCorrecto.Items.Add(liCorrecto1)
                'Dim liCorrecto2 As New ListItem(Chr(12) & Chr(12) & ItzultzaileWeb.Itzuli("Incorrecto.") & Chr(12) & Chr(12), 2)
                'rblCorrecto.Items.Add(liCorrecto2)
                'rblCorrecto.SelectedValue = listaEmpDoc(0).correcto

                'falta la parte opcional, si el doc tiene correos en DOC013 miramos impuestos en emd009 listaEmpDoc(0).impuestos


                mView.ActiveViewIndex = 2

            Else
                Master.MensajeInfo = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error  
            End If


        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al modificar el documento")
        End Try
    End Sub



    Protected Sub Volver(sender As Object, e As EventArgs) Handles botonVolver.Click
        Try
            If ex177.Text = "Empresa" Then

                Response.Redirect("~/EstadoEmpresaDocumento.aspx?hfEmpresa=" & txtEmpresa.Text, False)
            Else

                If idTrabajadorParam > 0 Then


                    Response.Redirect("~/EstadoEmpresaDocumento.aspx?empresa=" & idTrabajadorParam, False)


                End If
            End If
        Catch ex As Exception
            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al ver los trabajadores")
        End Try



    End Sub

    Protected Sub btoVolver_click(sender As Object, e As EventArgs) Handles btoVolver.Click
        Try
            If idTrabajadorParam > 0 Then
                Response.Redirect("~/EstadoTrabajadorDocumento.aspx?empresa=" & idTrabajadorParam & "&pagina=" & gvType.PageIndex, False)
            End If

        Catch ex As Exception
            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al ver los trabajadores")
        End Try



    End Sub

    Protected Sub btoVolverEmpresa_click(sender As Object, e As EventArgs) Handles btnVolverEmpresa.Click
        Try
            If Request.QueryString("empresa") <> "" Then
                If idTrabajadorParam > 0 Then
                    Response.Redirect("~/EstadoTrabajadorDocumentoRRHH.aspx?empresa=" & idTrabajadorParam & "&pagina=" & gvType.PageIndex, False)
                Else
                    Response.Redirect("~/EstadoTrabajadorDocumentoRRHH.aspx", False)
                End If
            Else
                Response.Redirect("~/EstadoTrabajadorDocumentoRRHH.aspx", False)
            End If

        Catch ex As Exception
            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al ver los trabajadores")
        End Try



    End Sub

    Private Sub btnSubir2_Click(sender As Object, e As EventArgs) Handles btnSubir2.Click

        'INSERTAR en emd. en realidad es update con la direccion del fichero y subir fichero
        Dim fechaValidez As Date
        Dim fechaCad As Date
        Dim cantidad As Int32
        Dim intervalo As String
        Dim periodo As Int32
        Try 'ListBox22

            If FuDoc.PostedFile.ContentLength > 5242880 Then
                Master.MensajeError = ItzultzaileWeb.Itzuli("Ha superado el límite máximo de 5 Mb. Intente comprimir el archivo (*.zip) o pónganse en contacto con adok@batz.es")
                mView.ActiveViewIndex = 4
                Exit Sub

            End If
            'If ListBox22.Items(6).Selected = "" Then
            '    Master.MensajeError = ItzultzaileWeb.Itzuli("Selecciona al menos un carné")
            '    mView.ActiveViewIndex = 4
            '    Exit Sub
            'End If

            If Not IsDate(TxtFechaValidez.Text) And TxtFechaValidez.Text <> ItzultzaileWeb.Itzuli("Documento sin caducidad") Then
                Master.MensajeError = ItzultzaileWeb.Itzuli("Fecha de inicio de validez obligatoria")
                mView.ActiveViewIndex = 4
                Exit Sub
            End If


            If FuDoc.HasFile Then

                Dim fmt2 As String = "00"
                Dim fmt3 As String = "000"
                Dim fmt4 As String = "0000"
                Dim fmt6 As String = "000000"
                Dim fmt8 As String = "00000000"
                Dim codigo As Int32 = idDoc.Value  'txtDocEmp.Text
                Dim codtrar As Int32 = idTrabajador.Value
                'buscar desc con cod emp y cod doc idTrabajador.Value txtDocEmp.Text

                Dim oDocBLL As New BLL.DocumentosBLL

                Dim fechatext As String = Now.Year.ToString(fmt4) & Now.Month.ToString(fmt2) & Now.Day.ToString(fmt2) & Now.Hour.ToString(fmt2) & Now.Minute.ToString(fmt2) & Now.Second.ToString(fmt2)
                Dim prefijo As String = PageBase.plantaAdmin.ToString(fmt3) & codtrar.ToString(fmt6) & codigo.ToString(fmt8) & fechatext
                FuDoc.SaveAs(DirFicherosSubir & "Ficheros_Matriz_Puestos/Documentos/" & prefijo & Right(FuDoc.FileName, 5))
                'poner un texto                    lblPlantillaSubida.Text = "Documento subido:  " & fuDoc.PostedFile.FileName
                'subir a oracle

                'codigo 13 = no tiene intervalo
                'calcular caducidad emd004
                'leer de adok_doc con  idDoc.Value los valores de intervalo cantidad y si tiene caducidad. el doc003 -> per000 se saca per003 como cantidad y per002 ->int000 que saca int001=intervalo


                'el periodo se coge de trd007 no de de doc
                Dim listaDoc As List(Of ELL.TrabajadoresDoc)
                listaDoc = oDocumentosBLL.CargarListaTraDoc(PageBase.plantaAdmin, CInt(idTrabajador.Value), CInt(idDoc.Value))
                periodo = listaDoc(0).periodicidad


                'nuevo, lo cojo de lo seleccionado en intranet, asi permito cambiar. en extranet del documento
                Dim listaDocu As List(Of ELL.Documentos)
                listaDocu = oDocumentosBLL.CargarDocumentos(CInt(idDoc.Value), PageBase.plantaAdmin)

                periodo = ddlCaducidad.SelectedValue



                Dim listaCad As List(Of ELL.Caducidades)
                listaCad = oDocBLL.CargarCad(PageBase.plantaAdmin, periodo)

                cantidad = listaCad(0).cantidad

                'leer int001
                listaCad = oDocBLL.CargarInt(PageBase.plantaAdmin, listaCad(0).intervalo)
                intervalo = Trim(listaCad(0).nombre)


                'intervalo = "d"
                'cantidad = 1
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

                If listaDocu(0).Plantilla = 1 Then
                    'fechaValidez = Now
                    fechaCad = Date.MinValue
                End If

                Dim intestado As Integer
                If fechaCad > Now Then 'no caducado ya mismo
                    intestado = 2

                    ''''''''si el documento tiene lista de correo a avisar de su subida, se le manda mensaje
                    '''''''If listaDocu(0).listacorreos <> "" Then
                    '''''''    'prueba correo
                    '''''''    Dim paramBLL As New SabLib.BLL.ParametrosBLL
                    '''''''    Dim oParams As SabLib.ELL.Parametros = paramBLL.consultar()
                    '''''''    Dim mailFrom As String = ConfigurationManager.AppSettings("MailFrom")

                    '''''''    SabLib.BLL.Utils.EnviarEmail("adoknet" & "@batz.es", listaDocu(0).listacorreos, ItzultzaileWeb.Itzuli("Nuevo documento a revisar"), ItzultzaileWeb.Itzuli("Se ha subido un nuevo documento en adok que requiere su revisión.") & "<br><br>" & ItzultzaileWeb.Itzuli("Puede acceder en el siguiente link:") & "  " & System.Configuration.ConfigurationManager.AppSettings("sitioAvisos").ToString & "/ValidacionTrabajadores.aspx", oParams.ServidorEmail)

                    '''''''End If

                Else 'caducado o sin caducidad
                    'poner  emd003 = 1 incorrecto
                    If fechaCad = Date.MinValue Then
                        intestado = 2
                    Else
                        intestado = 1
                    End If
                End If





                Dim tiposCarne As String = ""


                If tiposCarne = "" And idDoc.Value = "450" Then
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Selecciona un tipo de carné")
                    mView.ActiveViewIndex = 4
                    Exit Sub
                End If


                Dim tipo As New ELL.TrabajadoresDoc With {.tiposCarne = tiposCarne, .Planta = PageBase.plantaAdmin, .periodicidad = periodo, .estado = intestado, .codtra = codtrar, .coddoc = codigo, .FecCad = fechaCad, .FecIni = fechaValidez, .ubicacion = prefijo & Right(FuDoc.FileName, 5)}

                If (oDocumentosBLL.ModificarTraDoc(tipo, 2, Session("Ticket").nombreusuario)) Then  'actualizamos emd005 y emd004 y emd006
                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("Se ha añadido el documento") & " " & codigo & " " & ItzultzaileWeb.Itzuli("al trabajador") & " " & idTrabajador.Value & " " & ItzultzaileWeb.Itzuli("correctamente").ToUpper
                    'falta refrescar el doc
                    BindDataView2()
                    FecUltDoc.Text = Now.ToShortDateString
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se insertaba el documento") & " " & codigo & " " & ItzultzaileWeb.Itzuli("al trabajador") & " " & idTrabajador.Value
                End If


            Else
                '    lblPlantillaSubida.Text = "Fichero no seleccionado."

            End If
            mView.ActiveViewIndex = 1

        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al ver el documento")
        End Try
    End Sub


    Public Sub ibSubir_Click(sender As Object, e As EventArgs)
        Try
            Dim oDocBLL As New BLL.DocumentosBLL

            Dim fila As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            Dim idDocumento As String = gvType2.DataKeys(fila.RowIndex)("clave").ToString()
            txtEmpDoc.Text = DescEmpresa.Value
            idDoc.Value = idDocumento
            'buscar en pla la plantilla de ese doc es pla005
            'Dim liAsignacion As List(Of ELL.Plantillas)
            'liAsignacion = oDocumentosBLL.CargarListaPla(PageBase.plantaAdmin, idDocumento)

            'If liAsignacion.Count > 0 Then
            '    idPlantilla0.Value = liAsignacion(0).nombre
            '    '               btnDescargarPlantilla.Visible = True
            'Else
            '    idPlantilla0.Value = "0"
            '    '               btnDescargarPlantilla.Visible = False
            'End If


            'cargar caducidad
            ddlCaducidad.Items.Clear()

            Dim listaCad As List(Of ELL.Caducidades)
            listaCad = oDocBLL.CargarListaCad(PageBase.plantaAdmin)

            For Each caducidad In listaCad
                Dim licaducidad As New ListItem(caducidad.nombre, caducidad.id)
                ddlCaducidad.Items.Add(licaducidad)

            Next

            'buscar en adok_doc
            Dim listaDoc As List(Of ELL.Documentos)
            'Dim tipo As New ELL.Documentos With {.Planta = PageBase.plantaAdmin, .Id = CInt(idDocumento)}
            listaDoc = oDocumentosBLL.CargarDocumentos(CInt(idDocumento), PageBase.plantaAdmin)
            If listaDoc.Count > 0 Then
                txtDocEmp.Text = listaDoc(0).Nombre
                'lo cojo del doc y si hay ya elegido deo de trd lo que haya grabado ddlCaducidad.SelectedValue = listaDoc(0).Periodo
                ddlCaducidad.SelectedValue = listaDoc(0).Periodo

            End If
            'buscar en adok_tra
            Dim listaDoc2 As List(Of ELL.TrabajadoresDoc)

            listaDoc2 = oDocumentosBLL.CargarListaTraDoc(PageBase.plantaAdmin, CInt(idTrabajador.Value), CInt(idDocumento))
            If listaDoc2.Count > 0 Then
                If listaDoc2(0).ubicacion <> "" Then

                    '''''''''''''' no se cambia la que tiene por defecto  ddlCaducidad.SelectedValue = listaDoc2(0).periodicidad

                    If listaDoc2(0).FecIni = Date.MinValue Then
                        TxtFechaValidez.Text = "" ' Now.ToShortDateString
                    Else
                        TxtFechaValidez.Text = (listaDoc2(0).FecIni).ToShortDateString
                    End If
                    If (listaDoc2(0)).periodicidad = 13 Or (listaDoc2(0)).periodicidad = 998 Then 'no tiene caducidad
                        TxtFechaValidez.ReadOnly = True
                        imgCalendarioFechaValidez.Visible = False
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
                    'TxtFechaVal.Text = ItzultzaileWeb.Itzuli("Documento sin caducidad")
                End If




                ''''''If IsDate(TxtFechaVal.Text) Then
                ''''''    fechaValidez = CDate(TxtFechaVal.Text)
                ''''''Else
                ''''''    fechaValidez = Date.MinValue
                ''''''End If




            Else
                FecUltDoc.Text = ItzultzaileWeb.Itzuli("No hay ningún documento subido")
            End If

            '          ddlCaducidad.Enabled = False

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
            liPlantilla = oDocumentosBLL.CargarListaPla(PageBase.plantaAdmin, idDocumento)

            If liPlantilla.Count > 0 Then
                'idPlantilla0.Value = liPlantilla(0).nombre

                If liPlantilla(0).nombre <> "" Then


                    'tratar el idioma
                    If Right(Split(liPlantilla(0).nombre.ToUpper, ".")(0), 4) = "-EUS" Or Right(Split(liPlantilla(0).nombre.ToUpper, ".")(0), 4) = "-ENG" Or Right(Split(liPlantilla(0).nombre.ToUpper, ".")(0), 4) = "X-ESP" Then
                        'BUSCAR EL CORRESPONDIENTE
                        'Dim cultura As String
                        'cultura = System.Globalization.CultureInfo.CurrentCulture.Name
                        Dim docnombre As String = ""
                        Select Case Mid(System.Globalization.CultureInfo.CurrentCulture.Name, 1, 3)
                            Case "es-"
                                docnombre = Mid(Split(liPlantilla(0).nombre, ".")(0), 1, Split(liPlantilla(0).nombre, ".")(0).Length - 4) & "-ESP." & Split(liPlantilla(0).nombre, ".")(1)

                            Case "eu-"
                                docnombre = Mid(Split(liPlantilla(0).nombre, ".")(0), 1, Split(liPlantilla(0).nombre, ".")(0).Length - 4) & "-EUS." & Split(liPlantilla(0).nombre, ".")(1)
                            Case "en-"
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
                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                End If

                '          btnDescargarPlantilla.Visible = True
            Else
                Master.MensajeInfo = ItzultzaileWeb.Itzuli("Plantilla no subida").ToUpper 'error 
            End If
            mView.ActiveViewIndex = 1

        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado la plantilla")
        End Try

    End Sub
    Public Sub ibVerHis_Click(sender As Object, e As EventArgs)
        Try
            Dim idDocumento As String = idDoc.Value
            Dim codtrab As Integer = CInt(idTrabajador.Value)


            'sacar el nombre del tipo de doc

            Dim listaDoc As List(Of ELL.Documentos)

            listaDoc = oDocumentosBLL.CargarDocumentos(CInt(idDocumento), PageBase.plantaAdmin)
            If listaDoc.Count > 0 Then
                txtDocuEmp.Text = listaDoc(0).Nombre
            End If


            Dim listaEmpDoc As List(Of ELL.TrabajadoresDoc)
            'buscar en adok_emd
            Dim tipo As New ELL.TrabajadoresDoc With {.Planta = PageBase.plantaAdmin, .codtra = CInt(idTrabajador.Value), .coddoc = CInt(idDocumento)}

            listaEmpDoc = oDocumentosBLL.LeerTraDocHis(tipo)
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
    Public Sub ibVer_Click2(sender As Object, e As EventArgs)
        Try



            Dim idDocumento As String = idDoc.Value
            Dim listaEmpDoc As List(Of ELL.TrabajadoresDoc)

            'buscar en adok_emd
            Dim tipo As New ELL.TrabajadoresDoc With {.Planta = PageBase.plantaAdmin, .codtra = idTrabajador.Value, .coddoc = idDocumento}
            listaEmpDoc = oDocumentosBLL.LeerTraDoc(tipo)
            If listaEmpDoc.Count > 0 Then

                If listaEmpDoc(0).ubicacion <> "" Then
                    hfurl.Value = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & (oDocumentosBLL.LeerTraDoc(tipo))(0).ubicacion
                    'se puede hacer asi tambien aparte de por javascript
                    If listaEmpDoc(0).ubicacion <> "" Then
                        idDocumento = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & listaEmpDoc(0).ubicacion
                        'Process.Start(idDocumento)
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

            mView.ActiveViewIndex = 2

        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al abrir el documento")
        End Try

    End Sub
    Public Sub ibVer_Click(sender As Object, e As EventArgs)
        Try


            Dim fila As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            Dim idDocumento As String = gvType2.DataKeys(fila.RowIndex)("clave").ToString()
            Dim listaEmpDoc As List(Of ELL.TrabajadoresDoc)

            'buscar en adok_emd
            Dim tipo As New ELL.TrabajadoresDoc With {.Planta = PageBase.plantaAdmin, .codtra = idTrabajador.Value, .coddoc = idDocumento}
            listaEmpDoc = oDocumentosBLL.LeerTraDoc(tipo)
            If listaEmpDoc.Count > 0 Then

                If listaEmpDoc(0).ubicacion <> "" Then
                    hfurl.Value = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & (oDocumentosBLL.LeerTraDoc(tipo))(0).ubicacion
                    'se puede hacer asi tambien aparte de por javascript
                    If listaEmpDoc(0).ubicacion <> "" Then
                        idDocumento = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & listaEmpDoc(0).ubicacion
                        'Process.Start(idDocumento)
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




            mView.ActiveViewIndex = 1

        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al abrir el documento")
        End Try

    End Sub
    Public Sub ibVer_HIST_Click(sender As Object, e As EventArgs)
        Try


            Dim fila As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            Dim idDocumento As String = gvTypeHis.DataKeys(fila.RowIndex)("clave").ToString()
            Dim listaEmpDoc As List(Of ELL.TrabajadoresDoc)

            'buscar en adok_emd
            Dim tipo As New ELL.TrabajadoresDoc With {.clave = idDocumento}
            listaEmpDoc = oDocumentosBLL.LeerTraDocHisClave(tipo)
            If listaEmpDoc.Count > 0 Then

                If listaEmpDoc(0).ubicacion <> "" Then

                    idDocumento = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & listaEmpDoc(0).ubicacion
                    ' Response.Redirect(idDocumento, True)
                    'Process.Start(idDocumento)
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

            mView.ActiveViewIndex = 1

        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al mostrar el documento")
        End Try

    End Sub






    ''' <summary>
    ''' Cargar los tipos y Habilitar/Deshabilitar Transmission Mode dependiendo del producto seleccionado
    ''' </summary>
    ''' <param name="idtrabajador"></param>
    ''' <remarks></remarks>
    Private Sub ConfiguracionProduct(ByVal idtrabajador As Integer)
        'aqui poner los datos del trabajador
        Try
            Dim oDocBLL As New BLL.DocumentosBLL
            Dim lista As List(Of ELL.Trabajadores)
            Dim userBLL As New SabLib.BLL.UsuariosComponent

            lista = oDocBLL.CargarTiposTrabajadorActivos(PageBase.plantaAdmin, idtrabajador)

            txtCIF.Text = lista(0).tDNI
            txtNombre.Text = lista(0).Nombre
            txtNombre2.Text = lista(0).Nombre
            '      lblNuevaSolicitud.Text = "Documentos de trabajador" ' de " & lista(0).Nombre
            mView.ActiveViewIndex = 1
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar el trabajador", ex)
        End Try

    End Sub

    Protected Sub PageDropDownList_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        ' Recupera la fila.
        Dim pagerRow As GridViewRow = gvType2.BottomPagerRow
        ' Recupera el control DropDownList...
        Dim pageList As DropDownList = CType(pagerRow.Cells(0).FindControl("PageDropDownList"), DropDownList)
        ' Se Establece la propiedad PageIndex para visualizar la página seleccionada...
        gvType2.PageIndex = pageList.SelectedIndex
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
                    '           BindDataView()

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


            'codigo 13 = no tiene intervalo
            'calcular caducidad emd004
            'leer de adok_doc con  idDoc.Value los valores de intervalo cantidad y si tiene caducidad. el doc003 -> per000 se saca per003 como cantidad y per002 ->int000 que saca int001=intervalo


            'el periodo se coge de emd007 no de de doc
            Dim listaDoc As List(Of ELL.TrabajadoresDoc)
            listaDoc = oDocumentosBLL.CargarListaTraDoc(PageBase.plantaAdmin, CInt(idTrabajador.Value), CInt(idDoc.Value))
            If ddlCaducidad2.SelectedValue > 0 Then
                periodo = ddlCaducidad2.SelectedValue
            Else
                periodo = listaDoc(0).periodicidad

            End If


            Dim listaCad As List(Of ELL.Caducidades)
            listaCad = oDocBLL.CargarCad(PageBase.plantaAdmin, periodo)

            cantidad = listaCad(0).cantidad

            'leer int001
            listaCad = oDocBLL.CargarInt(PageBase.plantaAdmin, listaCad(0).intervalo)

            intervalo = Trim(listaCad(0).nombre)
            'If IsDate(TxtFechaVal.Text) Then
            '    fechaValidez = CDate(TxtFechaVal.Text)
            'Else
            '    fechaValidez = Date.MinValue
            'End If
            'If intervalo <> "nnnn" And Trim(TxtFechaVal.Text) <> "" And IsDate(TxtFechaVal.Text) Then

            '    If cantidad = 0 Then 'sin periodicidad
            '        fechaCad = Date.MaxValue
            '    Else
            '        fechaCad = DateAdd(intervalo, cantidad, fechaValidez)
            '    End If

            'Else

            '    fechaCad = Date.MaxValue
            'End If



            Dim tiposCarne As String = ""




            Dim tipo As New ELL.TrabajadoresDoc With {.tiposCarne = tiposCarne, .Comentario = txtComentario.Text, .Planta = PageBase.plantaAdmin, .FecCad = fechaCad, .FecIni = fechaValidez, .codtra = idTrabajador.Value, .coddoc = idDoc.Value, .FecRec = fechaR, .ubicacion = txtUbicacion.Text, .estado = rblCorrecto.SelectedValue, .Aptitud = 0, .periodicidad = periodo} ', .periodicidad = ddlCaducidad.SelectedValue

            If (oDocumentosBLL.ModificarTraDoc(tipo, 4, Session("Ticket").nombreusuario)) Then

                Master.MensajeInfo = ItzultzaileWeb.Itzuli("El documento se ha guardado correctamente")

                Dim listaDocu As List(Of ELL.Documentos)
                listaDocu = oDocumentosBLL.CargarDocumentos(CInt(idDoc.Value), PageBase.plantaAdmin)

                'jon nuevo
                If rblCorrecto.SelectedValue = 3 Then
                    Dim listaType3 As List(Of ELL.Trabajadores)
                    Dim nomtrabajador As String = ""
                    listaType3 = oDocBLL.CargarTiposTrabajadorActivosNew(plantaAdmin, idTrabajador.Value)
                    If (listaType3.Count > 0) Then 'son los docs de un puesto
                        nomtrabajador = listaType3(0).Nombre
                    End If


                    'de aqui saco la empresa, y de ahi el responsable
                    Dim mailResponsable As String = ""
                    Dim mailTra As String = ""
                    Dim Rlista2 As List(Of ELL.Responsables)
                    Dim RoDocBLL As New BLL.DocumentosBLL
                    Rlista2 = RoDocBLL.CargarResponsablesMail(listaType3(0).Empresa, PageBase.plantaAdmin)
                    If Rlista2.Count > 0 Then

                        For i = 0 To Rlista2.Count - 1
                            mailResponsable = mailResponsable & Rlista2(i).Mail & ";"

                        Next
                        mailResponsable = mailResponsable.Substring(0, mailResponsable.Length - 1)

                    End If

                    'correo del trabajador
                    Rlista2 = RoDocBLL.CargarResponsablesNombreMail(idTrabajador.Value, PageBase.plantaAdmin)
                    mailTra = Rlista2(0).Mail

                    'Dim Rlista2 As List(Of ELL.Responsables)

                    'Rlista2 = oDocumentosBLL.CargarResponsablesNombreMail(CInt(tipo.responsable), PageBase.plantaAdmin)
                    'If Rlista2.Count > 0 Then
                    '    Dim nombre As String = Rlista2(0).Nombre
                    '    mailResponsable = Rlista2(0).Mail.ToLower & ";"
                    'End If

                    'de moimento
                    'mailResponsable = ""
                    'mailTra = ""

                    Dim paramBLL As New SabLib.BLL.ParametrosBLL
                    Dim oParams As SabLib.ELL.Parametros = paramBLL.consultar()
                    'SabLib.BLL.Utils.EnviarEmail("matrizpuesto" & "@batz.es", "jzarraga@batz.es", ItzultzaileWeb.Itzuli("Se ha marcado como NO CADUCA al trabajador: ") & nomtrabajador, "<br><br>", oParams.ServidorEmail)
                    'SabLib.BLL.Utils.EnviarEmail("matrizpuesto" & "@batz.es", mailTra, ItzultzaileWeb.Itzuli("Se ha marcado como NO CADUCA al trabajador: ") & nomtrabajador, "<br><br>", oParams.ServidorEmail)
                    'SabLib.BLL.Utils.EnviarEmail("matrizpuesto" & "@batz.es", mailResponsable, ItzultzaileWeb.Itzuli("Se ha marcado como NO CADUCA al trabajador: ") & nomtrabajador, "<br><br>", oParams.ServidorEmail)
                End If
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



    Protected Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        mView.ActiveViewIndex = 1
    End Sub

    Private Sub gvType_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvType.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim imgEstado As LinkButton = CType(e.Row.FindControl("imgEstado"), LinkButton)
            imgEstado.Attributes.Add("onClick", "return false;")

        End If
    End Sub

    Private Sub gvType2_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvType2.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim imgEstado As LinkButton = CType(e.Row.FindControl("imgEstado"), LinkButton)
            imgEstado.Attributes.Add("onClick", "return false;")

            Dim imgEstado2 As LinkButton = CType(e.Row.FindControl("imgEstado2"), LinkButton)
            imgEstado2.Attributes.Add("onClick", "return false;")



        End If
    End Sub

    'Private Sub TxtFechaVal_TextChanged(sender As Object, e As EventArgs) Handles TxtFechaVal.TextChanged
    '    Dim tmp1 As Date
    '    Dim numdias As Double
    '    Dim tmp2 As Date
    '    Dim tmp3 As Date
    '    'If IsDate(TxtFechaVal.Text) And IsDate(FechaValG.Value) And IsDate(FechaValG2.Value) Then

    '    '    tmp1 = CDate(FechaValG.Value)
    '    '    tmp3 = CDate(TxtFechaVal.Text)
    '    '    numdias = DateDiff(DateInterval.Day, tmp1, tmp3)

    '    '    tmp2 = CDate(FechaValG2.Value)

    '    '    tmp2 = DateAdd(DateInterval.Day, numdias, tmp2)
    '    '    TxtFechaVal2.Text = tmp2.ToShortDateString
    '    'End If
    '    mView.ActiveViewIndex = 2
    'End Sub

    Private Sub ddlCaducidad2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCaducidad2.SelectedIndexChanged
        Dim fechaValidez As Date
        Dim fechaCad As Date
        Dim cantidad As Int32
        Dim intervalo As String
        Dim periodo As Int32
        Dim oDocBLL As New BLL.DocumentosBLL

        periodo = ddlCaducidad2.SelectedValue

        Dim listaCad As List(Of ELL.Caducidades)
        listaCad = oDocBLL.CargarCad(PageBase.plantaAdmin, periodo)

        cantidad = listaCad(0).cantidad

        'leer int001
        listaCad = oDocBLL.CargarInt(PageBase.plantaAdmin, listaCad(0).intervalo)
        intervalo = Trim(listaCad(0).nombre)


        'If intervalo <> "nnnn" And Trim(TxtFechaVal.Text) <> "" And IsDate(TxtFechaVal.Text) Then
        '    fechaValidez = CDate(TxtFechaVal.Text)
        '    If cantidad = 0 Then 'sin periodicidad
        '        fechaCad = Date.MaxValue
        '    Else
        '        fechaCad = DateAdd(intervalo, cantidad, fechaValidez)
        '    End If

        'Else
        '    fechaValidez = Date.MinValue
        '    fechaCad = Date.MaxValue
        'End If
        TxtFechaVal2.Text = fechaCad.ToShortDateString

        mView.ActiveViewIndex = 2
    End Sub

End Class