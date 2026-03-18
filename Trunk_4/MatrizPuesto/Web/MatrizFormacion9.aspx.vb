
Imports System.Data


Public Class MatrizFormacion9
    Inherits PageBase

    'Inherits System.Web.UI.Page

    Dim oDocumentosBLL As New BLL.DocumentosBLL
    Dim idTrabajadorParam As Int32 = 0
    Dim cab0 As String = ""
    Dim cab1 As String = ""
    Dim cab2 As String = ""
    Dim cab3 As String = ""
    Dim cab4 As String = ""
    Dim cab5 As String = ""
    Dim cab6 As String = ""
    Dim cab7 As String = ""
    Dim cab8 As String = ""
    Dim cab9 As String = ""



#Region "Propiedades"

    Protected Overrides ReadOnly Property RolesAcceso As System.Collections.Generic.List(Of ELL.Roles.RolUsuario)
        Get
            Dim roles As New List(Of ELL.Roles.RolUsuario)
            roles.Add(ELL.Roles.RolUsuario.Administrador)
            roles.Add(ELL.Roles.RolUsuario.Administrador2)
            roles.Add(ELL.Roles.RolUsuario.Prevencion)
            roles.Add(ELL.Roles.RolUsuario.RRHH)
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
            If ((RolesAcceso Is Nothing OrElse Not tieneAcceso) AndAlso Not (PerfilUsuario.IdRol = ELL.Roles.RolUsuario.Administrador) AndAlso Not (PerfilUsuario.IdRol = ELL.Roles.RolUsuario.Supervisores)) Then
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
        BindDataView2()
    End Sub

    ''' <summary>
    ''' Limpia los datos vinculados
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ClearGridView()
        gvType2.DataSource = Nothing
        gvType2.DataBind()

    End Sub


    Protected Sub BindDataView2()
        Try


            Dim oDocBLL As New BLL.DocumentosBLL
            Dim i As Int16

            Dim listaDocumentos As List(Of ELL.Documentos)
            listaDocumentos = oDocBLL.CargarListaMatrizDoc(PageBase.plantaAdmin)
            Dim listaTrabajadores As List(Of ELL.TrabajadoresDocMatriz)
            Dim listaType3 As List(Of ELL.Empresas)
            Dim codpuesto As Integer = 0
            If hfEmpresa.Value <> "" Then

                listaType3 = oDocBLL.CargarListaEmpresastexto(PageBase.plantaAdmin, hfEmpresa.Value)
                If (listaType3.Count > 0) Then
                    codpuesto = listaType3(0).Id
                    listaTrabajadores = oDocBLL.CargarListaMatrizTraEmp(PageBase.plantaAdmin, codpuesto)
                Else
                    listaTrabajadores = oDocBLL.CargarListaMatrizTra(PageBase.plantaAdmin)
                End If

            Else

                listaTrabajadores = oDocBLL.CargarListaMatrizTra(PageBase.plantaAdmin)
            End If


            Dim listaType2 As List(Of ELL.TrabajadoresDoc)
            Dim listaType4 As List(Of ELL.TrabajadoresDoc)
            Dim listaType5 As List(Of ELL.TrabajadoresDoc)
            'los documentos para ese puesto son:
            listaType4 = oDocBLL.CargarListaMatriz(codpuesto, plantaAdmin)

            'por ejem el puesto seleccionado 193, tiene 2 tra (listaTrabajadores(xx).codtra 2804  ,listaType2(i).coddoc 394

            For j = 0 To listaTrabajadores.Count - 1


                'meter comentario 
                '          listaType5 = oDocBLL.CargarListaMatrizCadaTra(codpuesto, )





                ''''EMPIEZA EL PRIMERO
                'para el primer doc, asi para los n
                If listaType4.Count > 0 Then
                    i = 0 ' por ser el primer doc

                    'la cabecera debo poner listaType2(0).nombre
                    'hacer calculos con listaType2(0).coddoc para ver si correcto
                    Dim no_correcto As Int16 = 0
                    listaTrabajadores(j).Comentario = listaType4(i).coddoc.ToString & ";" & listaTrabajadores(j).codtra.ToString 'el codtra ; y coddoc 
                    'estado
                    'hay que cargar los datos de este doc da adok_trd
                    listaType2 = oDocBLL.CargarListaTraDocAsignadosMatrizTodos(listaType4(i).coddoc, listaTrabajadores(j).codtra, PageBase.plantaAdmin)
                    If listaType2.Count > 0 Then
                        cab0 = listaType4(i).Nombre

                        i = 0 'siempre 0

                        If listaType2(i).FecRec = Date.MinValue Then
                            no_correcto = 1
                            'If listaType2(i).obligatorio = False Then
                            '    listaType2(i).estado = "Documento no obligatorio"
                            'Else
                            listaType2(i).estado = ItzultzaileWeb.Itzuli("Dcto. no entregado")
                            'End If
                        Else
                            If listaType2(i).FecRec = Date.MinValue Then
                                no_correcto = 1
                                'If listaType2(i).obligatorio = False Then
                                '    listaType2(i).estado = "Documento no obligatorio"
                                'Else
                                listaType2(i).estado = ItzultzaileWeb.Itzuli("Caducidad no asignada")
                                'End If
                            Else

                                If listaType2(i).FecCad < Now And listaType2(i).FecCad <> Date.MinValue Then '"01/01/1900"  CDate(Now.ToShortDateString)
                                    no_correcto = 1
                                    'If listaType2(i).obligatorio = False Then
                                    '    listaType2(i).estado = "Documento caducado no obligatorio"
                                    'Else
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

                                    'End If
                                Else
                                    'si esta puesto incorrecto en la mod del doc se pone asi
                                    If listaType2(i).correcto = 1 Then
                                        no_correcto = 1
                                        listaType2(i).estado = ItzultzaileWeb.Itzuli("Marcado Incorrecto")
                                    Else
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



                                            End If

                                        End If

                                    End If

                                End If

                            End If
                        End If
                        'mostramos un icono u otro en función de si es ok o no, o no es obligatorio
                        If no_correcto = 1 Then
                            listaTrabajadores(j).doc1 = no_correcto

                            listaType2(i).txtcorrecto = "Errores"

                        Else
                            If no_correcto = 2 Then

                                listaTrabajadores(j).doc1 = 2
                                listaType2(i).txtcorrecto = "Avisos"
                            Else

                                listaTrabajadores(j).doc1 = 0

                                listaType2(i).txtcorrecto = "Correcto"
                            End If

                        End If


                        'no aplica
                        If listaType2(i).correcto = 3 Then
                            listaTrabajadores(j).doc1 = 2
                            listaType2(i).txtcorrecto = "Avisos"

                            no_correcto = 2
                            listaType2(i).estado = "No aplica"
                            listaType2(i).txtcorrecto = "No aplica"
                        End If



                        If listaType2(0).necesario = 0 Then
                            listaTrabajadores(j).doc1 = 3

                            'listaType2(i).txtcorrecto = "No asignado"
                        End If
                    Else
                        listaTrabajadores(j).doc1 = 3
                    End If
                End If
                ''''FIN EL PRIMERO

                'para el segundo doc, poner todo lo de arriba
                If listaType4.Count > 1 And listaTrabajadores(j).doc1 >= 0 Then
                    i = 1 ' por ser el primer doc
                    cab1 = listaType4(i).Nombre
                    listaTrabajadores(j).Comentario = listaType4(i).coddoc.ToString & ";" & listaTrabajadores(j).Comentario  ' & ";" & listaTrabajadores(j).codtra.ToString 'el codtra ; y coddoc 
                    Dim no_correcto As Int16 = 0
                    'estado
                    'hay que cargar los datos de este doc da adok_trd
                    listaType2 = oDocBLL.CargarListaTraDocAsignadosMatrizTodos(listaType4(i).coddoc, listaTrabajadores(j).codtra, PageBase.plantaAdmin)
                    i = 0 'siempre 0
                    If listaType2.Count > 0 Then
                        If listaType2(i).FecRec = Date.MinValue Then
                            no_correcto = 1
                            'If listaType2(i).obligatorio = False Then
                            '    listaType2(i).estado = "Documento no obligatorio"
                            'Else
                            listaType2(i).estado = ItzultzaileWeb.Itzuli("Dcto. no entregado")
                            'End If
                        Else
                            If listaType2(i).FecRec = Date.MinValue Then
                                no_correcto = 1
                                'If listaType2(i).obligatorio = False Then
                                '    listaType2(i).estado = "Documento no obligatorio"
                                'Else
                                listaType2(i).estado = ItzultzaileWeb.Itzuli("Caducidad no asignada")
                                'End If
                            Else

                                If listaType2(i).FecCad < Now And listaType2(i).FecCad <> Date.MinValue Then '"01/01/1900"  CDate(Now.ToShortDateString)
                                    no_correcto = 1
                                    'If listaType2(i).obligatorio = False Then
                                    '    listaType2(i).estado = "Documento caducado no obligatorio"
                                    'Else
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

                                    'End If
                                Else
                                    'si esta puesto incorrecto en la mod del doc se pone asi
                                    If listaType2(i).correcto = 1 Then
                                        no_correcto = 1
                                        listaType2(i).estado = ItzultzaileWeb.Itzuli("Marcado Incorrecto")
                                    Else
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



                                            End If

                                        End If

                                    End If

                                End If

                            End If
                        End If
                        'mostramos un icono u otro en función de si es ok o no, o no es obligatorio
                        If no_correcto = 1 Then
                            listaTrabajadores(j).doc2 = no_correcto

                            listaType2(i).txtcorrecto = "Errores"

                        Else
                            If no_correcto = 2 Then

                                listaTrabajadores(j).doc2 = 2
                                listaType2(i).txtcorrecto = "Avisos"
                            Else

                                listaTrabajadores(j).doc2 = 0

                                listaType2(i).txtcorrecto = "Correcto"
                            End If

                        End If

                        If listaType2(0).necesario = 0 Then
                            listaTrabajadores(j).doc2 = 3

                            ' listaType2(i).txtcorrecto = "No asignado"
                        End If
                    Else

                        listaTrabajadores(j).doc2 = 3
                    End If
                End If

                If listaType4.Count > 2 And listaTrabajadores(j).doc2 >= 0 Then
                    i = 2 ' por ser el primer doc
                    cab2 = listaType4(2).Nombre



                    listaTrabajadores(j).Comentario = listaType4(i).coddoc.ToString & ";" & listaTrabajadores(j).Comentario
                    Dim no_correcto As Int16 = 0
                    'estado
                    'hay que cargar los datos de este doc da adok_trd
                    listaType2 = oDocBLL.CargarListaTraDocAsignadosMatrizTodos(listaType4(i).coddoc, listaTrabajadores(j).codtra, PageBase.plantaAdmin)
                    If listaType2.Count > 0 Then


                        i = 0 'siempre 0

                        If listaType2(i).FecRec = Date.MinValue Then
                            no_correcto = 1
                            'If listaType2(i).obligatorio = False Then
                            '    listaType2(i).estado = "Documento no obligatorio"
                            'Else
                            listaType2(i).estado = ItzultzaileWeb.Itzuli("Dcto. no entregado")
                            'End If
                        Else
                            If listaType2(i).FecRec = Date.MinValue Then
                                no_correcto = 1
                                'If listaType2(i).obligatorio = False Then
                                '    listaType2(i).estado = "Documento no obligatorio"
                                'Else
                                listaType2(i).estado = ItzultzaileWeb.Itzuli("Caducidad no asignada")
                                'End If
                            Else

                                If listaType2(i).FecCad < Now And listaType2(i).FecCad <> Date.MinValue Then '"01/01/1900"  CDate(Now.ToShortDateString)
                                    no_correcto = 1
                                    'If listaType2(i).obligatorio = False Then
                                    '    listaType2(i).estado = "Documento caducado no obligatorio"
                                    'Else
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

                                    'End If
                                Else
                                    'si esta puesto incorrecto en la mod del doc se pone asi
                                    If listaType2(i).correcto = 1 Then
                                        no_correcto = 1
                                        listaType2(i).estado = ItzultzaileWeb.Itzuli("Marcado Incorrecto")
                                    Else
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



                                            End If

                                        End If

                                    End If

                                End If

                            End If

                        End If
                        'mostramos un icono u otro en función de si es ok o no, o no es obligatorio
                        If no_correcto = 1 Then
                            listaTrabajadores(j).doc3 = no_correcto

                            listaType2(i).txtcorrecto = "Errores"

                        Else
                            If no_correcto = 2 Then

                                listaTrabajadores(j).doc3 = 2
                                listaType2(i).txtcorrecto = "Avisos"
                            Else

                                listaTrabajadores(j).doc3 = 0

                                listaType2(i).txtcorrecto = "Correcto"
                            End If

                        End If

                        If listaType2(0).necesario = 0 Then
                            listaTrabajadores(j).doc3 = 3

                            ' listaType2(i).txtcorrecto = "No asignado"
                        End If
                    Else
                        listaTrabajadores(j).doc3 = 3
                    End If
                End If

                If listaType4.Count > 3 And listaTrabajadores(j).doc3 >= 0 Then
                    i = 3 ' por ser el primer doc
                    cab3 = listaType4(3).Nombre


                    listaTrabajadores(j).Comentario = listaType4(i).coddoc.ToString & ";" & listaTrabajadores(j).Comentario
                    Dim no_correcto As Int16 = 0
                    'estado
                    'hay que cargar los datos de este doc da adok_trd
                    listaType2 = oDocBLL.CargarListaTraDocAsignadosMatrizTodos(listaType4(i).coddoc, listaTrabajadores(j).codtra, PageBase.plantaAdmin)
                    If listaType2.Count > 0 Then


                        i = 0 'siempre 0

                        If listaType2(i).FecRec = Date.MinValue Then
                            no_correcto = 1
                            'If listaType2(i).obligatorio = False Then
                            '    listaType2(i).estado = "Documento no obligatorio"
                            'Else
                            listaType2(i).estado = ItzultzaileWeb.Itzuli("Dcto. no entregado")
                            'End If
                        Else
                            If listaType2(i).FecRec = Date.MinValue Then
                                no_correcto = 1
                                'If listaType2(i).obligatorio = False Then
                                '    listaType2(i).estado = "Documento no obligatorio"
                                'Else
                                listaType2(i).estado = ItzultzaileWeb.Itzuli("Caducidad no asignada")
                                'End If
                            Else

                                If listaType2(i).FecCad < Now And listaType2(i).FecCad <> Date.MinValue Then '"01/01/1900"  CDate(Now.ToShortDateString)
                                    no_correcto = 1
                                    'If listaType2(i).obligatorio = False Then
                                    '    listaType2(i).estado = "Documento caducado no obligatorio"
                                    'Else
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

                                    'End If
                                Else
                                    'si esta puesto incorrecto en la mod del doc se pone asi
                                    If listaType2(i).correcto = 1 Then
                                        no_correcto = 1
                                        listaType2(i).estado = ItzultzaileWeb.Itzuli("Marcado Incorrecto")
                                    Else
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



                                            End If

                                        End If

                                    End If

                                End If

                            End If
                        End If
                        'mostramos un icono u otro en función de si es ok o no, o no es obligatorio
                        If no_correcto = 1 Then
                            listaTrabajadores(j).doc4 = no_correcto

                            listaType2(i).txtcorrecto = "Errores"

                        Else
                            If no_correcto = 2 Then

                                listaTrabajadores(j).doc4 = 2
                                listaType2(i).txtcorrecto = "Avisos"
                            Else

                                listaTrabajadores(j).doc4 = 0

                                listaType2(i).txtcorrecto = "Correcto"
                            End If

                        End If

                        If listaType2(0).necesario = 0 Then
                            listaTrabajadores(j).doc4 = 3

                            ' listaType2(i).txtcorrecto = "No asignado"
                        End If
                    Else
                        listaTrabajadores(j).doc4 = 3
                    End If

                End If



                If listaType4.Count > 4 And listaTrabajadores(j).doc4 >= 0 Then
                    i = 4 ' por ser el primer doc
                    cab4 = listaType4(4).Nombre


                    listaTrabajadores(j).Comentario = listaType4(i).coddoc.ToString & ";" & listaTrabajadores(j).Comentario
                    Dim no_correcto As Int16 = 0
                    'estado
                    'hay que cargar los datos de este doc da adok_trd
                    listaType2 = oDocBLL.CargarListaTraDocAsignadosMatrizTodos(listaType4(i).coddoc, listaTrabajadores(j).codtra, PageBase.plantaAdmin)
                    i = 0 'siempre 0
                    If listaType2.Count > 0 Then


                        If listaType2(i).FecRec = Date.MinValue Then
                            no_correcto = 1
                            'If listaType2(i).obligatorio = False Then
                            '    listaType2(i).estado = "Documento no obligatorio"
                            'Else
                            listaType2(i).estado = ItzultzaileWeb.Itzuli("Dcto. no entregado")
                            'End If
                        Else
                            If listaType2(i).FecRec = Date.MinValue Then
                                no_correcto = 1
                                'If listaType2(i).obligatorio = False Then
                                '    listaType2(i).estado = "Documento no obligatorio"
                                'Else
                                listaType2(i).estado = ItzultzaileWeb.Itzuli("Caducidad no asignada")
                                'End If
                            Else

                                If listaType2(i).FecCad < Now And listaType2(i).FecCad <> Date.MinValue Then '"01/01/1900"  CDate(Now.ToShortDateString)
                                    no_correcto = 1
                                    'If listaType2(i).obligatorio = False Then
                                    '    listaType2(i).estado = "Documento caducado no obligatorio"
                                    'Else
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

                                    'End If
                                Else
                                    'si esta puesto incorrecto en la mod del doc se pone asi
                                    If listaType2(i).correcto = 1 Then
                                        no_correcto = 1
                                        listaType2(i).estado = ItzultzaileWeb.Itzuli("Marcado Incorrecto")
                                    Else
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



                                            End If

                                        End If

                                    End If

                                End If

                            End If
                        End If
                        'mostramos un icono u otro en función de si es ok o no, o no es obligatorio
                        If no_correcto = 1 Then
                            listaTrabajadores(j).doc5 = no_correcto

                            listaType2(i).txtcorrecto = "Errores"

                        Else
                            If no_correcto = 2 Then

                                listaTrabajadores(j).doc5 = 2
                                listaType2(i).txtcorrecto = "Avisos"
                            Else

                                listaTrabajadores(j).doc5 = 0

                                listaType2(i).txtcorrecto = "Correcto"
                            End If

                        End If

                        If listaType2(0).necesario = 0 Then
                            listaTrabajadores(j).doc5 = 3

                            ' listaType2(i).txtcorrecto = "No asignado"
                        End If
                    Else
                        listaTrabajadores(j).doc5 = 3
                    End If
                End If


                If listaType4.Count > 5 And listaTrabajadores(j).doc5 >= 0 Then
                    i = 5 ' por ser el primer doc
                    cab5 = listaType4(5).Nombre


                    listaTrabajadores(j).Comentario = listaType4(i).coddoc.ToString & ";" & listaTrabajadores(j).Comentario
                    Dim no_correcto As Int16 = 0
                    'estado
                    'hay que cargar los datos de este doc da adok_trd
                    listaType2 = oDocBLL.CargarListaTraDocAsignadosMatrizTodos(listaType4(i).coddoc, listaTrabajadores(j).codtra, PageBase.plantaAdmin)
                    If listaType2.Count > 0 Then


                        i = 0 'siempre 0

                        If listaType2(i).FecRec = Date.MinValue Then
                            no_correcto = 1
                            'If listaType2(i).obligatorio = False Then
                            '    listaType2(i).estado = "Documento no obligatorio"
                            'Else
                            listaType2(i).estado = ItzultzaileWeb.Itzuli("Dcto. no entregado")
                            'End If
                        Else
                            If listaType2(i).FecRec = Date.MinValue Then
                                no_correcto = 1
                                'If listaType2(i).obligatorio = False Then
                                '    listaType2(i).estado = "Documento no obligatorio"
                                'Else
                                listaType2(i).estado = ItzultzaileWeb.Itzuli("Caducidad no asignada")
                                'End If
                            Else

                                If listaType2(i).FecCad < Now And listaType2(i).FecCad <> Date.MinValue Then '"01/01/1900"  CDate(Now.ToShortDateString)
                                    no_correcto = 1
                                    'If listaType2(i).obligatorio = False Then
                                    '    listaType2(i).estado = "Documento caducado no obligatorio"
                                    'Else
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

                                    'End If
                                Else
                                    'si esta puesto incorrecto en la mod del doc se pone asi
                                    If listaType2(i).correcto = 1 Then
                                        no_correcto = 1
                                        listaType2(i).estado = ItzultzaileWeb.Itzuli("Marcado Incorrecto")
                                    Else
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



                                            End If

                                        End If

                                    End If

                                End If

                            End If
                        End If
                        'mostramos un icono u otro en función de si es ok o no, o no es obligatorio
                        If no_correcto = 1 Then
                            listaTrabajadores(j).doc6 = no_correcto

                            listaType2(i).txtcorrecto = "Errores"

                        Else
                            If no_correcto = 2 Then

                                listaTrabajadores(j).doc6 = 2
                                listaType2(i).txtcorrecto = "Avisos"
                            Else

                                listaTrabajadores(j).doc6 = 0

                                listaType2(i).txtcorrecto = "Correcto"
                            End If

                        End If

                        If listaType2(0).necesario = 0 Then
                            listaTrabajadores(j).doc6 = 3

                            ' listaType2(i).txtcorrecto = "No asignado"
                        End If
                    Else
                        listaTrabajadores(j).doc6 = 3
                    End If
                End If


                If listaType4.Count > 6 And listaTrabajadores(j).doc6 >= 0 Then
                    i = 6 ' por ser el primer doc
                    cab6 = listaType4(6).Nombre


                    listaTrabajadores(j).Comentario = listaType4(i).coddoc.ToString & ";" & listaTrabajadores(j).Comentario
                    Dim no_correcto As Int16 = 0
                    'estado
                    'hay que cargar los datos de este doc da adok_trd
                    listaType2 = oDocBLL.CargarListaTraDocAsignadosMatrizTodos(listaType4(i).coddoc, listaTrabajadores(j).codtra, PageBase.plantaAdmin)
                    If listaType2.Count > 0 Then


                        i = 0 'siempre 0

                        If listaType2(i).FecRec = Date.MinValue Then
                            no_correcto = 1
                            'If listaType2(i).obligatorio = False Then
                            '    listaType2(i).estado = "Documento no obligatorio"
                            'Else
                            listaType2(i).estado = ItzultzaileWeb.Itzuli("Dcto. no entregado")
                            'End If
                        Else
                            If listaType2(i).FecRec = Date.MinValue Then
                                no_correcto = 1
                                'If listaType2(i).obligatorio = False Then
                                '    listaType2(i).estado = "Documento no obligatorio"
                                'Else
                                listaType2(i).estado = ItzultzaileWeb.Itzuli("Caducidad no asignada")
                                'End If
                            Else

                                If listaType2(i).FecCad < Now And listaType2(i).FecCad <> Date.MinValue Then '"01/01/1900"  CDate(Now.ToShortDateString)
                                    no_correcto = 1
                                    'If listaType2(i).obligatorio = False Then
                                    '    listaType2(i).estado = "Documento caducado no obligatorio"
                                    'Else
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

                                    'End If
                                Else
                                    'si esta puesto incorrecto en la mod del doc se pone asi
                                    If listaType2(i).correcto = 1 Then
                                        no_correcto = 1
                                        listaType2(i).estado = ItzultzaileWeb.Itzuli("Marcado Incorrecto")
                                    Else
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



                                            End If

                                        End If

                                    End If

                                End If

                            End If
                        End If
                        'mostramos un icono u otro en función de si es ok o no, o no es obligatorio
                        If no_correcto = 1 Then
                            listaTrabajadores(j).doc7 = no_correcto

                            listaType2(i).txtcorrecto = "Errores"

                        Else
                            If no_correcto = 2 Then

                                listaTrabajadores(j).doc7 = 2
                                listaType2(i).txtcorrecto = "Avisos"
                            Else

                                listaTrabajadores(j).doc7 = 0

                                listaType2(i).txtcorrecto = "Correcto"
                            End If
                        End If


                        If listaType2(0).necesario = 0 Then
                            listaTrabajadores(j).doc7 = 3

                            ' listaType2(i).txtcorrecto = "No asignado"
                        End If
                    Else
                        listaTrabajadores(j).doc7 = 3
                    End If
                End If


                If listaType4.Count > 7 And listaTrabajadores(j).doc7 >= 0 Then
                    i = 7 ' por ser el primer doc
                    cab7 = listaType4(7).Nombre


                    listaTrabajadores(j).Comentario = listaType4(i).coddoc.ToString & ";" & listaTrabajadores(j).Comentario
                    Dim no_correcto As Int16 = 0
                    'estado
                    'hay que cargar los datos de este doc da adok_trd
                    listaType2 = oDocBLL.CargarListaTraDocAsignadosMatrizTodos(listaType4(i).coddoc, listaTrabajadores(j).codtra, PageBase.plantaAdmin)
                    If listaType2.Count > 0 Then


                        i = 0 'siempre 0

                        If listaType2(i).FecRec = Date.MinValue Then
                            no_correcto = 1
                            'If listaType2(i).obligatorio = False Then
                            '    listaType2(i).estado = "Documento no obligatorio"
                            'Else
                            listaType2(i).estado = ItzultzaileWeb.Itzuli("Dcto. no entregado")
                            'End If
                        Else
                            If listaType2(i).FecRec = Date.MinValue Then
                                no_correcto = 1
                                'If listaType2(i).obligatorio = False Then
                                '    listaType2(i).estado = "Documento no obligatorio"
                                'Else
                                listaType2(i).estado = ItzultzaileWeb.Itzuli("Caducidad no asignada")
                                'End If
                            Else

                                If listaType2(i).FecCad < Now And listaType2(i).FecCad <> Date.MinValue Then '"01/01/1900"  CDate(Now.ToShortDateString)
                                    no_correcto = 1
                                    'If listaType2(i).obligatorio = False Then
                                    '    listaType2(i).estado = "Documento caducado no obligatorio"
                                    'Else
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

                                    'End If
                                Else
                                    'si esta puesto incorrecto en la mod del doc se pone asi
                                    If listaType2(i).correcto = 1 Then
                                        no_correcto = 1
                                        listaType2(i).estado = ItzultzaileWeb.Itzuli("Marcado Incorrecto")
                                    Else
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



                                            End If

                                        End If

                                    End If

                                End If

                            End If
                        End If
                        'mostramos un icono u otro en función de si es ok o no, o no es obligatorio
                        If no_correcto = 1 Then
                            listaTrabajadores(j).doc8 = no_correcto

                            listaType2(i).txtcorrecto = "Errores"

                        Else
                            If no_correcto = 2 Then

                                listaTrabajadores(j).doc8 = 2
                                listaType2(i).txtcorrecto = "Avisos"
                            Else

                                listaTrabajadores(j).doc8 = 0

                                listaType2(i).txtcorrecto = "Correcto"
                            End If

                        End If

                        If listaType2(0).necesario = 0 Then
                            listaTrabajadores(j).doc8 = 3

                            ' listaType2(i).txtcorrecto = "No asignado"
                        End If
                    Else
                        listaTrabajadores(j).doc8 = 3
                    End If
                End If


                If listaType4.Count > 8 And listaTrabajadores(j).doc8 >= 0 Then
                    i = 8 ' por ser el primer doc
                    cab8 = listaType4(8).Nombre


                    listaTrabajadores(j).Comentario = listaType4(i).coddoc.ToString & ";" & listaTrabajadores(j).Comentario
                    Dim no_correcto As Int16 = 0
                    'estado
                    'hay que cargar los datos de este doc da adok_trd
                    listaType2 = oDocBLL.CargarListaTraDocAsignadosMatrizTodos(listaType4(i).coddoc, listaTrabajadores(j).codtra, PageBase.plantaAdmin)
                    If listaType2.Count > 0 Then


                        i = 0 'siempre 0

                        If listaType2(i).FecRec = Date.MinValue Then
                            no_correcto = 1
                            'If listaType2(i).obligatorio = False Then
                            '    listaType2(i).estado = "Documento no obligatorio"
                            'Else
                            listaType2(i).estado = ItzultzaileWeb.Itzuli("Dcto. no entregado")
                            'End If
                        Else
                            If listaType2(i).FecRec = Date.MinValue Then
                                no_correcto = 1
                                'If listaType2(i).obligatorio = False Then
                                '    listaType2(i).estado = "Documento no obligatorio"
                                'Else
                                listaType2(i).estado = ItzultzaileWeb.Itzuli("Caducidad no asignada")
                                'End If
                            Else

                                If listaType2(i).FecCad < Now And listaType2(i).FecCad <> Date.MinValue Then '"01/01/1900"  CDate(Now.ToShortDateString)
                                    no_correcto = 1
                                    'If listaType2(i).obligatorio = False Then
                                    '    listaType2(i).estado = "Documento caducado no obligatorio"
                                    'Else
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

                                    'End If
                                Else
                                    'si esta puesto incorrecto en la mod del doc se pone asi
                                    If listaType2(i).correcto = 1 Then
                                        no_correcto = 1
                                        listaType2(i).estado = ItzultzaileWeb.Itzuli("Marcado Incorrecto")
                                    Else
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



                                            End If

                                        End If

                                    End If

                                End If

                            End If
                        End If
                        'mostramos un icono u otro en función de si es ok o no, o no es obligatorio
                        If no_correcto = 1 Then
                            listaTrabajadores(j).doc9 = no_correcto

                            listaType2(i).txtcorrecto = "Errores"

                        Else
                            If no_correcto = 2 Then

                                listaTrabajadores(j).doc9 = 2
                                listaType2(i).txtcorrecto = "Avisos"
                            Else

                                listaTrabajadores(j).doc9 = 0

                                listaType2(i).txtcorrecto = "Correcto"
                            End If
                        End If


                        If listaType2(0).necesario = 0 Then
                            listaTrabajadores(j).doc9 = 3

                            ' listaType2(i).txtcorrecto = "No asignado"
                        End If
                    Else
                        listaTrabajadores(j).doc9 = 3
                    End If
                End If


                If listaType4.Count > 9 And listaTrabajadores(j).doc9 >= 0 Then
                    i = 9 ' por ser el primer doc
                    cab9 = listaType4(9).Nombre


                    listaTrabajadores(j).Comentario = listaType4(i).coddoc.ToString & ";" & listaTrabajadores(j).Comentario
                    Dim no_correcto As Int16 = 0
                    'estado
                    'hay que cargar los datos de este doc da adok_trd
                    listaType2 = oDocBLL.CargarListaTraDocAsignadosMatrizTodos(listaType4(i).coddoc, listaTrabajadores(j).codtra, PageBase.plantaAdmin)
                    If listaType2.Count > 0 Then


                        i = 0 'siempre 0

                        If listaType2(i).FecRec = Date.MinValue Then
                            no_correcto = 1
                            'If listaType2(i).obligatorio = False Then
                            '    listaType2(i).estado = "Documento no obligatorio"
                            'Else
                            listaType2(i).estado = ItzultzaileWeb.Itzuli("Dcto. no entregado")
                            'End If
                        Else
                            If listaType2(i).FecRec = Date.MinValue Then
                                no_correcto = 1
                                'If listaType2(i).obligatorio = False Then
                                '    listaType2(i).estado = "Documento no obligatorio"
                                'Else
                                listaType2(i).estado = ItzultzaileWeb.Itzuli("Caducidad no asignada")
                                'End If
                            Else

                                If listaType2(i).FecCad < Now And listaType2(i).FecCad <> Date.MinValue Then '"01/01/1900"  CDate(Now.ToShortDateString)
                                    no_correcto = 1
                                    'If listaType2(i).obligatorio = False Then
                                    '    listaType2(i).estado = "Documento caducado no obligatorio"
                                    'Else
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

                                    'End If
                                Else
                                    'si esta puesto incorrecto en la mod del doc se pone asi
                                    If listaType2(i).correcto = 1 Then
                                        no_correcto = 1
                                        listaType2(i).estado = ItzultzaileWeb.Itzuli("Marcado Incorrecto")
                                    Else
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



                                            End If

                                        End If

                                    End If

                                End If

                            End If
                        End If
                        'mostramos un icono u otro en función de si es ok o no, o no es obligatorio
                        If no_correcto = 1 Then
                            listaTrabajadores(j).doc10 = no_correcto

                            listaType2(i).txtcorrecto = "Errores"

                        Else
                            If no_correcto = 2 Then

                                listaTrabajadores(j).doc10 = 2
                                listaType2(i).txtcorrecto = "Avisos"
                            Else

                                listaTrabajadores(j).doc10 = 0

                                listaType2(i).txtcorrecto = "Correcto"
                            End If
                        End If


                        If listaType2(0).necesario = 0 Then
                            listaTrabajadores(j).doc10 = 3

                            ' listaType2(i).txtcorrecto = "No asignado"
                        End If
                    Else
                        listaTrabajadores(j).doc10 = 3
                    End If
                End If

            Next


            Registros.Text = listaTrabajadores.Count.ToString

            If (listaType4.Count > 0) Then

                gvType2.DataSource = listaTrabajadores
                gvType2.DataBind()
                gvType2.Caption = String.Empty
            Else
                Registros.Text = "0"
                '     Registros2.Text = "0"
                gvType2.DataSource = Nothing
                gvType2.DataBind()
                gvType2.Caption = "No hay trabajadores asignados  o documentos para ese puesto"
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
            PageBase.plantaAdmin = 9
            Dim persona As New SabLib.ELL.Ticket

            If (Session("Ticket") IsNot Nothing) Then
                persona = CType(Session("Ticket"), SabLib.ELL.Ticket)
            End If

            Dim plantas As Int32 = persona.Plantas.Count

            If plantas = 0 Then
                MsgBox("El usuario no está asignado a ninguna planta")
            Else




                'fin de actualizar trd

                ComprobarAcceso()
                txtComentario.Attributes.Add("placeholder", ItzultzaileWeb.Itzuli("Comentario"))
                TxtFechaValidez.Attributes.Add("placeholder", ItzultzaileWeb.Itzuli("Fecha de entrada en vigor"))
                mView.ActiveViewIndex = 1

                Initialize()

            End If

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

            Dim IdEmpre As Int32 = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).id
            Dim DesEmpre As String = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).nombre
            idTrabajador.Value = IdEmpre
            DescEmpresa.Value = DesEmpre
            BindDataView2()  'para limpiar el grid
            ConfiguracionProduct(IdEmpre)

        End If


    End Sub


    Public Sub ibHistorico_Click(sender As Object, e As EventArgs)
        Try
            Dim fila As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            Dim idDocumento As String = gvType2.DataKeys(fila.RowIndex)("clave").ToString()

            'sacar el nombre del tipo de doc

            Dim listaDoc As List(Of ELL.Documentos)

            listaDoc = oDocumentosBLL.CargarDocumentos(156, PageBase.plantaAdmin)
            If listaDoc.Count > 0 Then
                txtDocuEmp.Text = listaDoc(0).Nombre
            End If


            Dim listaEmpDoc As List(Of ELL.TrabajadoresDoc)
            'buscar en adok_emd
            Dim tipo As New ELL.TrabajadoresDoc With {.Planta = PageBase.plantaAdmin, .codtra = CInt(idDocumento), .coddoc = 156}

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



                    'listaEmpDoc(i).ubicacion = Mid(listaEmpDoc(i).ubicacion, 32)

                Next

                txtCIFHis.Text = txtCIF.Text
                txtNombreHis.Text = txtNombre.Text
                mView.ActiveViewIndex = 3

                gvTypeHis.DataSource = listaEmpDoc
                gvTypeHis.DataBind()
                gvTypeHis.Caption = String.Empty

            Else
                Master.MensajeError = "No se ha encontrado el documento".ToUpper 'error 
            End If
        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("Error en el histórico de documentos")
        End Try

    End Sub

    Public Sub ibModificar_Click(sender As Object, e As EventArgs)
        Try

            Dim fila As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            Dim idDocumento As String = gvType2.DataKeys(fila.RowIndex)("Comentario").ToString()
            Dim idDocumento1 As String

            idDocumento1 = Split(idDocumento, ";")(0)

            idTrabajador.Value = idDocumento1 'aqui es el trabajador, el doc es siempre 156

            idDoc.Value = Split(idDocumento, ";")(1)
            idDocumento = idDoc.Value
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim oDocBLL As New BLL.DocumentosBLL


            Dim lista As List(Of ELL.Trabajadores)
            lista = oDocBLL.CargarTiposTrabajadorActivos(PageBase.plantaAdmin, CInt(idTrabajador.Value))

            txtNombre2.Text = lista(0).Nombre & "        DNI = " & lista(0).tDNI

            ddlCaducidad.Items.Clear()
            Dim licaducidadVacio As New ListItem("", 0)
            ddlCaducidad.Items.Add(licaducidadVacio)
            Dim listaCad As List(Of ELL.Caducidades)
            listaCad = oDocBLL.CargarListaCad(PageBase.plantaAdmin)

            For Each caducidad In listaCad
                Dim licaducidad As New ListItem(caducidad.nombre, caducidad.id)
                ddlCaducidad.Items.Add(licaducidad)

            Next






            'buscar en adok_trd
            Dim listaDoc2 As List(Of ELL.TrabajadoresDoc)
            listaDoc2 = oDocumentosBLL.CargarListaTraDoc(PageBase.plantaAdmin, CInt(idTrabajador.Value), CInt(idDocumento))
            If listaDoc2.Count > 0 Then

                If listaDoc2(0).tiposCarne <> "" Then
                    Dim roles As String() = Nothing
                    roles = listaDoc2(0).tiposCarne.Split(";")
                    If roles.Count > 1 Then


                        For Each rol As String In roles
                            'leer todos las lineas de carne
                            For i = 0 To ListBox22.Items.Count - 1


                                If (ListBox22.Items(i).Text = rol) Then
                                    ListBox22.Items(i).Selected = True
                                    'Else
                                    '    ListBox22.Items(i).Selected = False
                                End If
                            Next
                        Next

                    End If

                End If


            End If









            Dim listaEmpDoc As List(Of ELL.TrabajadoresDoc)
            'buscar en adok_emd
            Dim tipo As New ELL.TrabajadoresDoc With {.Planta = PageBase.plantaAdmin, .codtra = idTrabajador.Value, .coddoc = idDoc.Value}


            listaEmpDoc = oDocumentosBLL.LeerTraDoc(tipo)
            If listaEmpDoc.Count > 0 Then
                If listaEmpDoc(0).FecRec > Date.MinValue Then
                    TxtFechaRec.Text = listaEmpDoc(0).FecRec.ToShortDateString
                Else
                    TxtFechaRec.Text = ""
                End If
                Dim listaDoc As List(Of ELL.Documentos)
                listaDoc = oDocumentosBLL.CargarDocumentos(CInt(idDocumento), PageBase.plantaAdmin)
                txtNombreDoc.Text = listaDoc(0).Nombre

                txtComentario.Text = listaEmpDoc(0).Comentario
                TxtFechaVal.Enabled = True
                TxtFechaVal.ReadOnly = False
                imgCalendarioFechaVal.Visible = True
                If listaEmpDoc(0).FecIni > Date.MinValue Then
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
                    TxtFechaVal.Text = ItzultzaileWeb.Itzuli("Documento de plantilla")
                    TxtFechaVal2.Text = ItzultzaileWeb.Itzuli("Sin caducidad")
                Else
                    TxtFechaVal2.Text = listaEmpDoc(0).FecCad.ToShortDateString

                End If

                txtUbicacion.Text = listaEmpDoc(0).ubicacionfisica
                rblCorrecto.SelectedValue = listaEmpDoc(0).correcto
                FechaValG2.Value = listaEmpDoc(0).FecCad.ToShortDateString

                'falta la parte opcional, si el doc tiene correos en DOC013 miramos impuestos en emd009 listaEmpDoc(0).impuestos
                If listaDocu(0).listacorreos <> "" Then 'tiene correos a notificar, es una excepcion
                    If listaEmpDoc(0).Aptitud < 1 Then
                        listaEmpDoc(0).Aptitud = 5 'son valores de sin deuda
                        rblImpuestos.SelectedValue = "5"
                    Else
                        rblImpuestos.SelectedValue = listaEmpDoc(0).Aptitud
                    End If
                    PanelX.Visible = True
                Else
                    'no debe salir esa linea
                    PanelX.Visible = False
                End If

                mView.ActiveViewIndex = 2

            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento ").ToUpper 'error  
            End If
        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al modificar el documento")
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

        mView.ActiveViewIndex = 2
    End Sub

    Protected Sub Volver(sender As Object, e As EventArgs) Handles botonVolver.Click
        Try
            If idTrabajadorParam > 0 Then


                Response.Redirect("~/EstadoEmpresaDocumento.aspx?empresa=" & idTrabajadorParam, False)


            End If

        Catch ex As Exception
            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al ver los trabajadores")
        End Try



    End Sub


    Public Sub ibSubir_Click(sender As Object, e As EventArgs)
        Try

            Dim fila As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            Dim idDocumento As String = gvType2.DataKeys(fila.RowIndex)("clave").ToString()
            txtEmpDoc.Text = DescEmpresa.Value
            idDoc.Value = idDocumento
            'buscar en pla la plantilla de ese doc es pla005
            Dim liAsignacion As List(Of ELL.Plantillas)
            liAsignacion = oDocumentosBLL.CargarListaPla(PageBase.plantaAdmin, idDocumento)

            If liAsignacion.Count > 0 Then
                idPlantilla0.Value = liAsignacion(0).nombre
                '             btnDescargarPlantilla.Visible = True
            Else
                idPlantilla0.Value = "0"
                '           btnDescargarPlantilla.Visible = False
            End If


            'buscar en adok_doc
            Dim listaDoc As List(Of ELL.Documentos)
            'Dim tipo As New ELL.Documentos With {.Planta = PageBase.plantaAdmin, .Id = CInt(idDocumento)}
            listaDoc = oDocumentosBLL.CargarDocumentos(CInt(idDocumento), PageBase.plantaAdmin)
            If listaDoc.Count > 0 Then
                txtDocEmp.Text = listaDoc(0).Nombre
            End If
            'buscar en adok_tra
            Dim listaDoc2 As List(Of ELL.TrabajadoresDoc)

            listaDoc2 = oDocumentosBLL.CargarListaTraDoc(PageBase.plantaAdmin, CInt(idTrabajador.Value), CInt(idDocumento))
            If listaDoc2.Count > 0 Then
                If listaDoc2(0).ubicacion <> "" Then

                    If listaDoc2(0).FecIni = Date.MinValue Then
                        TxtFechaValidez.Text = "" ' Now.ToShortDateString
                    Else
                        TxtFechaValidez.Text = (listaDoc2(0).FecIni).ToShortDateString
                    End If
                    If (listaDoc2(0)).periodicidad = 13 Or (listaDoc2(0)).periodicidad = 998 Then 'no tiene caducidad
                        TxtFechaValidez.ReadOnly = True
                        imgCalendarioFechaValidez.Visible = False
                        TxtFechaValidez.Text = "Documento sin caducidad"
                    End If

                    FecUltDoc.Text = Mid(listaDoc2(0).ubicacion, 24, 2) & "/" & Mid(listaDoc2(0).ubicacion, 22, 2) & "/" & Mid(listaDoc2(0).ubicacion, 18, 4)
                Else
                    TxtFechaValidez.Text = "" 'Now.ToShortDateString
                    FecUltDoc.Text = ItzultzaileWeb.Itzuli("No hay ningún documento subido")
                End If


                If listaDoc(0).Plantilla = 1 Then 'los de plantilla no tienen caducidad

                    imgCalendarioFechaValidez.Visible = False
                    TxtFechaValidez.ReadOnly = True
                    TxtFechaValidez.Text = "Documento sin caducidad"
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
            liPlantilla = oDocumentosBLL.CargarListaPla(PageBase.plantaAdmin, "156")

            If liPlantilla.Count > 0 Then

                If liPlantilla(0).nombre <> "" Then
                    idDocumento = DirFicherosBajar & "Ficheros_Matriz_Puestos/" & liPlantilla(0).nombre

                    Dim script As String = "<script type=""text/javascript"">window.open('" & idDocumento & "');</script>"
                    ClientScript.RegisterStartupScript(Me.GetType, "openWindow", script)
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                End If

            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("Plantilla no subida").ToUpper 'error 
            End If


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

            'sacar el nombre del tipo de doc


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



                    'listaEmpDoc(i).ubicacion = Mid(listaEmpDoc(i).ubicacion, 32)

                Next

                txtCIFHis.Text = txtCIF.Text
                txtNombreHis.Text = txtNombre2.Text
                mView.ActiveViewIndex = 3

                gvTypeHis.DataSource = listaEmpDoc
                gvTypeHis.DataBind()
                gvTypeHis.Caption = String.Empty

            Else
                Master.MensajeError = "No se ha encontrado el documento".ToUpper 'error 
            End If
        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("Error en el histórico de documentos")
        End Try



    End Sub
    Public Sub ibVer_Click(sender As Object, e As EventArgs)
        Try

            Dim oDocBLL As New BLL.DocumentosBLL
            Dim fila As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            Dim idDocumento As String = gvType2.DataKeys(fila.RowIndex)("Comentario").ToString() 'es el trabajador
            Dim idDocumento1 As String

            Dim arrayDoc As String() = Nothing
            arrayDoc = Split(idDocumento, ";")

            idDocumento1 = Split(idDocumento, ";")(0)
            Dim listaEmpDoc As List(Of ELL.TrabajadoresDoc)


            Dim listaDocumentos As List(Of ELL.Documentos)
            listaDocumentos = oDocBLL.CargarListaMatrizDoc(PageBase.plantaAdmin)
            Dim idDoc1 = listaDocumentos(0).Id

            'buscar en adok_emd
            ' Dim tipo As New ELL.TrabajadoresDoc With {.Planta = PageBase.plantaAdmin, .codtra = idDocumento1, .coddoc = idDoc1}
            Dim tipo As New ELL.TrabajadoresDoc With {.Planta = PageBase.plantaAdmin, .codtra = arrayDoc(arrayDoc.Length - 1), .coddoc = arrayDoc(arrayDoc.Length - 2)}
            listaEmpDoc = oDocumentosBLL.LeerTraDoc(tipo)
            If listaEmpDoc.Count > 0 Then

                If listaEmpDoc(0).ubicacion <> "" Then
                    hfurl.Value = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & (oDocumentosBLL.LeerTraDoc(tipo))(0).ubicacion
                    'se puede hacer asi tambien aparte de por javascript
                    If listaEmpDoc(0).ubicacion <> "" Then
                        idDocumento = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & listaEmpDoc(0).ubicacion

                        Dim script As String = "<script type=""text/javascript"">window.open('" & idDocumento & "');</script>"
                        ClientScript.RegisterStartupScript(Me.GetType, "openWindow", script)
                    Else
                        Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                    End If


                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                End If
            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
            End If
        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al ver el documento")
        End Try

    End Sub
    Public Sub ibVer_Click2(sender As Object, e As EventArgs)
        Try

            Dim oDocBLL As New BLL.DocumentosBLL
            Dim fila As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            Dim idDocumento As String = gvType2.DataKeys(fila.RowIndex)("Comentario").ToString() 'es el trabajador
            Dim idDocumento1 As String
            Dim arrayDoc As String() = Nothing
            arrayDoc = Split(idDocumento, ";")

            idDocumento1 = Split(idDocumento, ";")(0)
            Dim listaEmpDoc As List(Of ELL.TrabajadoresDoc)


            Dim listaDocumentos As List(Of ELL.Documentos)
            listaDocumentos = oDocBLL.CargarListaMatrizDoc(PageBase.plantaAdmin)
            Dim idDoc1 = listaDocumentos(1).Id

            'buscar en adok_emd
            Dim tipo As New ELL.TrabajadoresDoc With {.Planta = PageBase.plantaAdmin, .codtra = arrayDoc(arrayDoc.Length - 1), .coddoc = arrayDoc(arrayDoc.Length - 3)}
            listaEmpDoc = oDocumentosBLL.LeerTraDoc(tipo)
            If listaEmpDoc.Count > 0 Then

                If listaEmpDoc(0).ubicacion <> "" Then
                    hfurl.Value = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & (oDocumentosBLL.LeerTraDoc(tipo))(0).ubicacion
                    'se puede hacer asi tambien aparte de por javascript
                    If listaEmpDoc(0).ubicacion <> "" Then
                        idDocumento = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & listaEmpDoc(0).ubicacion

                        Dim script As String = "<script type=""text/javascript"">window.open('" & idDocumento & "');</script>"
                        ClientScript.RegisterStartupScript(Me.GetType, "openWindow", script)
                    Else
                        Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                    End If


                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                End If
            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
            End If
        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al ver el documento")
        End Try

    End Sub
    Public Sub ibVer_Click3(sender As Object, e As EventArgs)
        Try

            Dim oDocBLL As New BLL.DocumentosBLL
            Dim fila As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            Dim idDocumento As String = gvType2.DataKeys(fila.RowIndex)("Comentario").ToString() 'es el trabajador
            Dim idDocumento1 As String
            Dim arrayDoc As String() = Nothing
            arrayDoc = Split(idDocumento, ";")
            idDocumento1 = Split(idDocumento, ";")(0)
            Dim listaEmpDoc As List(Of ELL.TrabajadoresDoc)


            Dim listaDocumentos As List(Of ELL.Documentos)
            listaDocumentos = oDocBLL.CargarListaMatrizDoc(PageBase.plantaAdmin)
            Dim idDoc1 = listaDocumentos(2).Id

            'buscar en adok_emd
            Dim tipo As New ELL.TrabajadoresDoc With {.Planta = PageBase.plantaAdmin, .codtra = arrayDoc(arrayDoc.Length - 1), .coddoc = arrayDoc(arrayDoc.Length - 4)}
            listaEmpDoc = oDocumentosBLL.LeerTraDoc(tipo)
            If listaEmpDoc.Count > 0 Then

                If listaEmpDoc(0).ubicacion <> "" Then
                    hfurl.Value = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & (oDocumentosBLL.LeerTraDoc(tipo))(0).ubicacion
                    'se puede hacer asi tambien aparte de por javascript
                    If listaEmpDoc(0).ubicacion <> "" Then
                        idDocumento = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & listaEmpDoc(0).ubicacion

                        Dim script As String = "<script type=""text/javascript"">window.open('" & idDocumento & "');</script>"
                        ClientScript.RegisterStartupScript(Me.GetType, "openWindow", script)
                    Else
                        Master.MensajeInfo = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                    End If


                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                End If
            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
            End If
        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al ver el documento")
        End Try

    End Sub

    Public Sub ibVer_Click4(sender As Object, e As EventArgs)
        Try

            Dim oDocBLL As New BLL.DocumentosBLL
            Dim fila As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            Dim idDocumento As String = gvType2.DataKeys(fila.RowIndex)("Comentario").ToString() 'es el trabajador
            Dim idDocumento1 As String
            Dim arrayDoc As String() = Nothing
            arrayDoc = Split(idDocumento, ";")
            idDocumento1 = Split(idDocumento, ";")(0)
            Dim listaEmpDoc As List(Of ELL.TrabajadoresDoc)


            Dim listaDocumentos As List(Of ELL.Documentos)
            listaDocumentos = oDocBLL.CargarListaMatrizDoc(PageBase.plantaAdmin)
            Dim idDoc1 = listaDocumentos(3).Id

            'buscar en adok_emd
            '     Dim tipo As New ELL.TrabajadoresDoc With {.Planta = PageBase.plantaAdmin, .codtra = idDocumento1, .coddoc = idDoc1}
            Dim tipo As New ELL.TrabajadoresDoc With {.Planta = PageBase.plantaAdmin, .codtra = arrayDoc(arrayDoc.Length - 1), .coddoc = arrayDoc(arrayDoc.Length - 5)}
            listaEmpDoc = oDocumentosBLL.LeerTraDoc(tipo)
            If listaEmpDoc.Count > 0 Then

                If listaEmpDoc(0).ubicacion <> "" Then
                    hfurl.Value = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & (oDocumentosBLL.LeerTraDoc(tipo))(0).ubicacion
                    'se puede hacer asi tambien aparte de por javascript
                    If listaEmpDoc(0).ubicacion <> "" Then
                        idDocumento = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & listaEmpDoc(0).ubicacion

                        Dim script As String = "<script type=""text/javascript"">window.open('" & idDocumento & "');</script>"
                        ClientScript.RegisterStartupScript(Me.GetType, "openWindow", script)
                    Else
                        Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                    End If


                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                End If
            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
            End If
        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al ver el documento")
        End Try

    End Sub

    Public Sub ibVer_Click5(sender As Object, e As EventArgs)
        Try

            Dim oDocBLL As New BLL.DocumentosBLL
            Dim fila As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            Dim idDocumento As String = gvType2.DataKeys(fila.RowIndex)("Comentario").ToString() 'es el trabajador
            Dim idDocumento1 As String
            Dim arrayDoc As String() = Nothing
            arrayDoc = Split(idDocumento, ";")
            idDocumento1 = Split(idDocumento, ";")(0)
            Dim listaEmpDoc As List(Of ELL.TrabajadoresDoc)


            Dim listaDocumentos As List(Of ELL.Documentos)
            listaDocumentos = oDocBLL.CargarListaMatrizDoc(PageBase.plantaAdmin)
            Dim idDoc1 = listaDocumentos(4).Id

            'buscar en adok_emd
            Dim tipo As New ELL.TrabajadoresDoc With {.Planta = PageBase.plantaAdmin, .codtra = arrayDoc(arrayDoc.Length - 1), .coddoc = arrayDoc(arrayDoc.Length - 6)}
            listaEmpDoc = oDocumentosBLL.LeerTraDoc(tipo)
            If listaEmpDoc.Count > 0 Then

                If listaEmpDoc(0).ubicacion <> "" Then
                    hfurl.Value = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & (oDocumentosBLL.LeerTraDoc(tipo))(0).ubicacion
                    'se puede hacer asi tambien aparte de por javascript
                    If listaEmpDoc(0).ubicacion <> "" Then
                        idDocumento = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & listaEmpDoc(0).ubicacion

                        Dim script As String = "<script type=""text/javascript"">window.open('" & idDocumento & "');</script>"
                        ClientScript.RegisterStartupScript(Me.GetType, "openWindow", script)
                    Else
                        Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                    End If


                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                End If
            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
            End If
        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al ver el documento")
        End Try

    End Sub
    Public Sub ibVer_Click6(sender As Object, e As EventArgs)
        Try

            Dim oDocBLL As New BLL.DocumentosBLL
            Dim fila As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            Dim idDocumento As String = gvType2.DataKeys(fila.RowIndex)("Comentario").ToString() 'es el trabajador
            Dim idDocumento1 As String
            Dim arrayDoc As String() = Nothing
            arrayDoc = Split(idDocumento, ";")
            idDocumento1 = Split(idDocumento, ";")(0)
            Dim listaEmpDoc As List(Of ELL.TrabajadoresDoc)


            Dim listaDocumentos As List(Of ELL.Documentos)
            listaDocumentos = oDocBLL.CargarListaMatrizDoc(PageBase.plantaAdmin)
            Dim idDoc1 = listaDocumentos(5).Id

            'buscar en adok_emd
            Dim tipo As New ELL.TrabajadoresDoc With {.Planta = PageBase.plantaAdmin, .codtra = arrayDoc(arrayDoc.Length - 1), .coddoc = arrayDoc(arrayDoc.Length - 7)}
            listaEmpDoc = oDocumentosBLL.LeerTraDoc(tipo)
            If listaEmpDoc.Count > 0 Then

                If listaEmpDoc(0).ubicacion <> "" Then
                    hfurl.Value = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & (oDocumentosBLL.LeerTraDoc(tipo))(0).ubicacion
                    'se puede hacer asi tambien aparte de por javascript
                    If listaEmpDoc(0).ubicacion <> "" Then
                        idDocumento = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & listaEmpDoc(0).ubicacion

                        Dim script As String = "<script type=""text/javascript"">window.open('" & idDocumento & "');</script>"
                        ClientScript.RegisterStartupScript(Me.GetType, "openWindow", script)
                    Else
                        Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                    End If


                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                End If
            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
            End If
        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al ver el documento")
        End Try

    End Sub

    Public Sub ibVer_Click7(sender As Object, e As EventArgs)
        Try

            Dim oDocBLL As New BLL.DocumentosBLL
            Dim fila As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            Dim idDocumento As String = gvType2.DataKeys(fila.RowIndex)("Comentario").ToString() 'es el trabajador
            Dim idDocumento1 As String
            Dim arrayDoc As String() = Nothing
            arrayDoc = Split(idDocumento, ";")
            idDocumento1 = Split(idDocumento, ";")(0)
            Dim listaEmpDoc As List(Of ELL.TrabajadoresDoc)


            Dim listaDocumentos As List(Of ELL.Documentos)
            listaDocumentos = oDocBLL.CargarListaMatrizDoc(PageBase.plantaAdmin)
            Dim idDoc1 = listaDocumentos(6).Id

            'buscar en adok_emd
            Dim tipo As New ELL.TrabajadoresDoc With {.Planta = PageBase.plantaAdmin, .codtra = arrayDoc(arrayDoc.Length - 1), .coddoc = arrayDoc(arrayDoc.Length - 8)}
            listaEmpDoc = oDocumentosBLL.LeerTraDoc(tipo)
            If listaEmpDoc.Count > 0 Then

                If listaEmpDoc(0).ubicacion <> "" Then
                    hfurl.Value = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & (oDocumentosBLL.LeerTraDoc(tipo))(0).ubicacion
                    'se puede hacer asi tambien aparte de por javascript
                    If listaEmpDoc(0).ubicacion <> "" Then
                        idDocumento = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & listaEmpDoc(0).ubicacion

                        Dim script As String = "<script type=""text/javascript"">window.open('" & idDocumento & "');</script>"
                        ClientScript.RegisterStartupScript(Me.GetType, "openWindow", script)
                    Else
                        Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                    End If


                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                End If
            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
            End If
        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al ver el documento")
        End Try

    End Sub

    Public Sub ibVer_Click8(sender As Object, e As EventArgs)
        Try

            Dim oDocBLL As New BLL.DocumentosBLL
            Dim fila As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            Dim idDocumento As String = gvType2.DataKeys(fila.RowIndex)("Comentario").ToString() 'es el trabajador
            Dim idDocumento1 As String
            Dim arrayDoc As String() = Nothing
            arrayDoc = Split(idDocumento, ";")
            idDocumento1 = Split(idDocumento, ";")(0)
            Dim listaEmpDoc As List(Of ELL.TrabajadoresDoc)


            Dim listaDocumentos As List(Of ELL.Documentos)
            listaDocumentos = oDocBLL.CargarListaMatrizDoc(PageBase.plantaAdmin)
            Dim idDoc1 = listaDocumentos(7).Id

            'buscar en adok_emd
            Dim tipo As New ELL.TrabajadoresDoc With {.Planta = PageBase.plantaAdmin, .codtra = arrayDoc(arrayDoc.Length - 1), .coddoc = arrayDoc(arrayDoc.Length - 9)}
            listaEmpDoc = oDocumentosBLL.LeerTraDoc(tipo)
            If listaEmpDoc.Count > 0 Then

                If listaEmpDoc(0).ubicacion <> "" Then
                    hfurl.Value = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & (oDocumentosBLL.LeerTraDoc(tipo))(0).ubicacion
                    'se puede hacer asi tambien aparte de por javascript
                    If listaEmpDoc(0).ubicacion <> "" Then
                        idDocumento = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & listaEmpDoc(0).ubicacion

                        Dim script As String = "<script type=""text/javascript"">window.open('" & idDocumento & "');</script>"
                        ClientScript.RegisterStartupScript(Me.GetType, "openWindow", script)
                    Else
                        Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                    End If


                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                End If
            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
            End If
        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al ver el documento")
        End Try

    End Sub


    Public Sub ibVer_Click9(sender As Object, e As EventArgs)
        Try

            Dim oDocBLL As New BLL.DocumentosBLL
            Dim fila As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            Dim idDocumento As String = gvType2.DataKeys(fila.RowIndex)("Comentario").ToString() 'es el trabajador
            Dim idDocumento1 As String
            Dim arrayDoc As String() = Nothing
            arrayDoc = Split(idDocumento, ";")
            idDocumento1 = Split(idDocumento, ";")(0)
            Dim listaEmpDoc As List(Of ELL.TrabajadoresDoc)


            Dim listaDocumentos As List(Of ELL.Documentos)
            listaDocumentos = oDocBLL.CargarListaMatrizDoc(PageBase.plantaAdmin)
            Dim idDoc1 = listaDocumentos(8).Id

            'buscar en adok_emd
            Dim tipo As New ELL.TrabajadoresDoc With {.Planta = PageBase.plantaAdmin, .codtra = arrayDoc(arrayDoc.Length - 1), .coddoc = arrayDoc(arrayDoc.Length - 10)}
            listaEmpDoc = oDocumentosBLL.LeerTraDoc(tipo)
            If listaEmpDoc.Count > 0 Then

                If listaEmpDoc(0).ubicacion <> "" Then
                    hfurl.Value = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & (oDocumentosBLL.LeerTraDoc(tipo))(0).ubicacion
                    'se puede hacer asi tambien aparte de por javascript
                    If listaEmpDoc(0).ubicacion <> "" Then
                        idDocumento = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & listaEmpDoc(0).ubicacion

                        Dim script As String = "<script type=""text/javascript"">window.open('" & idDocumento & "');</script>"
                        ClientScript.RegisterStartupScript(Me.GetType, "openWindow", script)
                    Else
                        Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                    End If


                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                End If
            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
            End If
        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al ver el documento")
        End Try

    End Sub



    Public Sub ibVer_Click10(sender As Object, e As EventArgs)
        Try

            Dim oDocBLL As New BLL.DocumentosBLL
            Dim fila As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
            Dim idDocumento As String = gvType2.DataKeys(fila.RowIndex)("Comentario").ToString() 'es el trabajador
            Dim idDocumento1 As String
            Dim arrayDoc As String() = Nothing
            arrayDoc = Split(idDocumento, ";")
            idDocumento1 = Split(idDocumento, ";")(0)
            Dim listaEmpDoc As List(Of ELL.TrabajadoresDoc)


            Dim listaDocumentos As List(Of ELL.Documentos)
            listaDocumentos = oDocBLL.CargarListaMatrizDoc(PageBase.plantaAdmin)
            Dim idDoc1 = listaDocumentos(9).Id

            'buscar en adok_emd
            Dim tipo As New ELL.TrabajadoresDoc With {.Planta = PageBase.plantaAdmin, .codtra = arrayDoc(arrayDoc.Length - 1), .coddoc = arrayDoc(arrayDoc.Length - 11)}
            listaEmpDoc = oDocumentosBLL.LeerTraDoc(tipo)
            If listaEmpDoc.Count > 0 Then

                If listaEmpDoc(0).ubicacion <> "" Then
                    hfurl.Value = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & (oDocumentosBLL.LeerTraDoc(tipo))(0).ubicacion
                    'se puede hacer asi tambien aparte de por javascript
                    If listaEmpDoc(0).ubicacion <> "" Then
                        idDocumento = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & listaEmpDoc(0).ubicacion

                        Dim script As String = "<script type=""text/javascript"">window.open('" & idDocumento & "');</script>"
                        ClientScript.RegisterStartupScript(Me.GetType, "openWindow", script)
                    Else
                        Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                    End If


                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                End If
            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
            End If
        Catch ex As Exception

            Master.MensajeError = ItzultzaileWeb.Itzuli("Error al ver el documento")
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
                    Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
                End If
            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
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

            'Dim listaDoc As List(Of ELL.Documentos)
            'Dim tipo As New ELL.Documentos With {.Planta = PageBase.plantaAdmin, .Id = CInt(idDocumento)}
            'listaDoc = oDocumentosBLL.CargarDocumentos(CInt(idDoc.Value), PageBase.plantaAdmin)
            'periodo = listaDoc(0).Periodo

            'el periodo se coge de emd007 no de de doc
            Dim listaDoc As List(Of ELL.TrabajadoresDoc)
            listaDoc = oDocumentosBLL.CargarListaTraDoc(PageBase.plantaAdmin, CInt(idTrabajador.Value), CInt(idDoc.Value))
            '  periodo = listaDoc(0).periodicidad
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


            Dim tipo As New ELL.TrabajadoresDoc With {.Comentario = txtComentario.Text, .Planta = PageBase.plantaAdmin, .FecCad = fechaCad, .FecIni = fechaValidez, .codtra = idTrabajador.Value, .coddoc = idDoc.Value, .FecRec = fechaR, .ubicacion = txtUbicacion.Text, .estado = rblCorrecto.SelectedValue, .Aptitud = rblImpuestos.SelectedValue, .periodicidad = periodo} ', .periodicidad = ddlCaducidad.SelectedValue

            If (oDocumentosBLL.ModificarTraDoc(tipo, 3, Session("Ticket").nombreusuario)) Then

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



    Protected Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        mView.ActiveViewIndex = 1
    End Sub

    Private Sub gvType2_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvType2.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then



            Dim imgDoc1 As LinkButton = CType(e.Row.FindControl("imgDoc1"), LinkButton)
            If imgDoc1.ToolTip = "" Then
                imgDoc1.Attributes.Add("onClick", "return false;")
            End If
            Dim imgDoc2 As LinkButton = CType(e.Row.FindControl("imgDoc2"), LinkButton)
            If imgDoc2.ToolTip = "" Then
                imgDoc2.Attributes.Add("onClick", "return false;")
            End If
            Dim imgDoc3 As LinkButton = CType(e.Row.FindControl("imgDoc3"), LinkButton)
            If imgDoc3.ToolTip = "" Then
                imgDoc3.Attributes.Add("onClick", "return false;")
            End If
            Dim imgDoc4 As LinkButton = CType(e.Row.FindControl("imgDoc4"), LinkButton)
            If imgDoc4.ToolTip = "" Then
                imgDoc4.Attributes.Add("onClick", "return false;")
            End If
            Dim imgDoc5 As LinkButton = CType(e.Row.FindControl("imgDoc5"), LinkButton)
            If imgDoc5.ToolTip = "" Then
                imgDoc5.Attributes.Add("onClick", "return false;")
            End If
            Dim imgDoc6 As LinkButton = CType(e.Row.FindControl("imgDoc6"), LinkButton)
            If imgDoc6.ToolTip = "" Then
                imgDoc6.Attributes.Add("onClick", "return false;")
            End If
            Dim imgDoc7 As LinkButton = CType(e.Row.FindControl("imgDoc7"), LinkButton)
            If imgDoc7.ToolTip = "" Then
                imgDoc7.Attributes.Add("onClick", "return false;")
            End If
            Dim imgDoc8 As LinkButton = CType(e.Row.FindControl("imgDoc8"), LinkButton)
            If imgDoc8.ToolTip = "" Then
                imgDoc8.Attributes.Add("onClick", "return false;")
            End If
            Dim imgDoc9 As LinkButton = CType(e.Row.FindControl("imgDoc9"), LinkButton)
            If imgDoc9.ToolTip = "" Then
                imgDoc9.Attributes.Add("onClick", "return false;")
            End If
        End If
    End Sub

    Private Sub rblCorrecto_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblCorrecto.SelectedIndexChanged
        mView.ActiveViewIndex = 2
    End Sub

    Private Sub TxtFechaVal_TextChanged(sender As Object, e As EventArgs) Handles TxtFechaVal.TextChanged
        Dim tmp1 As Date
        Dim numdias As Double
        Dim tmp2 As Date
        Dim tmp3 As Date
        If IsDate(TxtFechaVal.Text) Then

            mView.ActiveViewIndex = 2
            tmp1 = CDate(FechaValG.Value)
            tmp3 = CDate(TxtFechaVal.Text)
            numdias = DateDiff(DateInterval.Day, tmp1, tmp3)

            tmp2 = CDate(FechaValG2.Value)

            tmp2 = DateAdd(DateInterval.Day, numdias, tmp2)
            TxtFechaVal2.Text = tmp2.ToShortDateString
        End If

    End Sub

    Private Sub gvType2_PreRender(sender As Object, e As EventArgs) Handles gvType2.PreRender
        gvType2.Columns(4).HeaderText = cab0
        gvType2.Columns(5).HeaderText = cab1
        gvType2.Columns(6).HeaderText = cab2
        gvType2.Columns(7).HeaderText = cab3
        gvType2.Columns(8).HeaderText = cab4
        gvType2.Columns(9).HeaderText = cab5
        gvType2.Columns(10).HeaderText = cab6
        gvType2.Columns(11).HeaderText = cab7
        gvType2.Columns(12).HeaderText = cab8
        gvType2.Columns(13).HeaderText = cab9
        gvType2.DataBind()
    End Sub
End Class