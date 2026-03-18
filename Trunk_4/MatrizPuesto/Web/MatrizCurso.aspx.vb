
Imports System.Data


Public Class MatrizCurso
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
    Dim cab10 As String = ""
    Dim cab11 As String = ""



#Region "Propiedades"

    Protected Overrides ReadOnly Property RolesAcceso As System.Collections.Generic.List(Of ELL.Roles.RolUsuario)
        Get
            Dim roles As New List(Of ELL.Roles.RolUsuario)
            roles.Add(ELL.Roles.RolUsuario.Administrador)
            roles.Add(ELL.Roles.RolUsuario.Administrador2)
            roles.Add(ELL.Roles.RolUsuario.Prevencion)
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
        '  gvType2.DataSource = Nothing
        '      gvType2.DataBind()

    End Sub

    Protected Sub BindDataView2()
        Try


            Dim oDocBLL As New BLL.DocumentosBLL
            Dim i As Int16

            Dim listaDocumentos As List(Of ELL.Documentos)
            listaDocumentos = oDocBLL.CargarListaMatrizDoc(PageBase.plantaAdmin)
            Dim docsHistorico As List(Of ELL.TrabajadoresDocMatriz)

            Dim final As List(Of ELL.TrabajadoresDocMatriz)

            Dim codpuesto As Integer = 0
            Dim codtrabajadores As Integer = 0
            docsHistorico = oDocBLL.CargarListaMatrizTra(0)

            Dim listaTrabajador As List(Of ELL.TrabajadoresDoc)

            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim listadocs As List(Of ELL.Documentos)
            listadocs = oDocBLL.CargarDocumentosCer(PageBase.plantaAdmin, "")

            If hfEmpresa.Value <> "" Then

                listadocs = oDocBLL.CargarDocumentosCer(PageBase.plantaAdmin, hfEmpresa.Value)

                Dim tipo2 As New ELL.Trabajadores With {.Planta = PageBase.plantaAdmin, .Id = listadocs(0).Id}

                If (listadocs.Count > 0) Then
                    'ESTOS SON TODOS LOS DOCS DE TRD_HIST de ese tipo doc
                    docsHistorico = oDocumentosBLL.LeerTraDocHisCurso(tipo2, ddlTodos.SelectedValue)

                End If

            Else

                docsHistorico = oDocBLL.CargarListaMatrizTraEmp(0, 0)
                If hfProfesion.Value <> "" Or hfResponsable.Value <> "" Then
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Se debe seleccionar un Curso").ToUpper
                End If

            End If

            final = oDocBLL.CargarListaMatrizTra(PageBase.plantaAdmin)

            Dim RegHist As List(Of ELL.TrabajadoresDoc)
            Dim listayears As List(Of ELL.TrabajadoresDoc)
            Dim RegMatriz As List(Of ELL.TrabajadoresDoc)
            Dim RegistroMatriz As List(Of ELL.TrabajadoresDoc)
            oDocBLL.DeleteMatriz()
            RegMatriz = oDocBLL.CargarListaMat()
            'los documentos para ese puesto son:
            If docsHistorico.Count > 0 Then

                'POR CADA TRABAJADOR HARE ESTE BUCLE
                'MIRO POR ESE CURSO, que son esos docs de  CUANTOS TRABAJADORES HAY
                'debo coger los trabajadores de esos docs, el trd000
                Dim inTrabajadores As String = ""
                For t = 0 To docsHistorico.Count - 1
                    inTrabajadores = inTrabajadores & docsHistorico(t).clave.ToString & ","
                Next
                inTrabajadores = inTrabajadores.Substring(0, inTrabajadores.Length - 1)
                Dim profesion As Integer = 0
                Dim responsable As Integer = 0
                'filtrar estos trabajadores segun tr004 y demas
                If txtProfesion.Text <> "" Then
                    'buscar en bbdd su cod
                    Dim lista1 As List(Of ELL.TrabajadoresDoc)
                    lista1 = oDocBLL.CargarProfesion(txtProfesion.Text)
                    profesion = lista1(0).clave

                End If
                If txtResponsable.Text <> "" Then
                    'buscar en bbdd su cod
                    Dim lista1 As List(Of ELL.TrabajadoresDoc)
                    lista1 = oDocBLL.CargarResponsable(txtResponsable.Text)
                    responsable = lista1(0).clave

                End If

                listaTrabajador = oDocBLL.CargarListaMatrizTrabajadores(inTrabajadores, profesion, responsable, ddlTodos.SelectedValue)


                Dim incodtrabajadores As String = ""
                For n = 0 To listaTrabajador.Count - 1 'bucle por cada trabajador
                    incodtrabajadores = incodtrabajadores & listaTrabajador(n).coddoc.ToString & ","
                Next
                If incodtrabajadores.Length > 0 Then
                    incodtrabajadores = incodtrabajadores.Substring(0, incodtrabajadores.Length - 1)
                Else
                    incodtrabajadores = "0"
                End If


                '        incodtrabajadores = "57084,61530"
                'los años para todos esos trabajadores y curso son:
                listayears = oDocBLL.CargarListaMatriz3(incodtrabajadores, listadocs(0).Id, ddlTodos.SelectedValue, PageBase.plantaAdmin)
                For k = 0 To listayears.Count - 1
                    If k = 0 Then
                        cab0 = listayears(0).coddoc.ToString
                    End If
                    If k = 1 Then
                        cab1 = listayears(1).coddoc.ToString
                    End If
                    If k = 2 Then
                        cab2 = listayears(2).coddoc.ToString
                    End If
                    If k = 3 Then
                        cab3 = listayears(3).coddoc.ToString
                    End If
                    If k = 4 Then
                        cab4 = listayears(4).coddoc.ToString
                    End If
                    If k = 5 Then
                        cab5 = listayears(5).coddoc.ToString
                    End If
                    If k = 6 Then
                        cab6 = listayears(6).coddoc.ToString
                    End If
                    If k = 7 Then
                        cab7 = listayears(7).coddoc.ToString
                    End If
                    If k = 8 Then
                        cab8 = listayears(8).coddoc.ToString
                    End If
                    If k = 9 Then
                        cab9 = listayears(9).coddoc.ToString
                    End If
                    If k = 10 Then
                        cab10 = listayears(10).coddoc.ToString
                    End If
                    If k = 11 Then
                        cab11 = listayears(11).coddoc.ToString
                    End If
                Next
                'por ejem el puesto seleccionado 193, tiene 2 tra (docsHistorico(xx).codtra 2804  ,RegHist(i).coddoc 394
                Dim contador1 As Integer = 0
                Dim contador0 As Integer = 0
                Dim contador2 As Integer = 0
                Dim contador3 As Integer = 0
                Dim contador4 As Integer = 0
                Dim contador5 As Integer = 0
                Dim contador6 As Integer = 0
                Dim contador7 As Integer = 0
                Dim contador8 As Integer = 0
                Dim contador9 As Integer = 0
                Dim contador10 As Integer = 0
                Dim contador11 As Integer = 0


                For n = 0 To listaTrabajador.Count - 1 'bucle por cada trabajador
                    'quiero todos los docs de este trabajador listaTrabajador(n).coddoc y doc = listadocs(0).Id

                    Dim tipo5 As New ELL.TrabajadoresDoc With {.Planta = PageBase.plantaAdmin, .codtra = listaTrabajador(n).coddoc, .coddoc = listadocs(0).Id}

                    'ESTOS SON TODOS LOS DOCS DE TRD_HIST de ese tipo doc
                    Dim docsHistoricox As List(Of ELL.TrabajadoresDoc)
                    docsHistoricox = oDocumentosBLL.LeerTraDocHisTodos(tipo5, ddlTodos.SelectedValue)
                    For j = 0 To docsHistoricox.Count - 1
                        'docsHistoricox me sirve para identificar el indice de docsHistorico
                        Dim indice As Integer = 0
                        For f = 0 To docsHistorico.Count - 1
                            If docsHistorico(f).clave = docsHistoricox(j).clave Then
                                indice = f
                            End If
                        Next

                        ''''EMPIEZA EL PRIMERO
                        'para el primer doc, asi para los n
                        If listayears.Count > 0 Then
                            'meter el año de cada registro segun su clave trd008
                            RegHist = oDocBLL.CargarListaHist(docsHistoricox(j).clave)
                            If RegHist.Count = 0 Then
                                Continue For
                            End If
                            docsHistorico(indice).doc1 = RegHist(0).clave

                            Dim intTmp As Integer
                            If RegHist(0).nomtra <> "" Then
                                If docsHistorico(indice).nomtra = "" Then
                                    intTmp = CInt(RegHist(0).nomtra)
                                Else
                                    intTmp = CInt(RegHist(0).nomtra)
                                End If

                                docsHistorico(indice).nomtra = intTmp.ToString
                                docsHistorico(indice).NIF = RegHist(0).NIF
                                docsHistorico(indice).codtra = RegHist(0).codtra
                                docsHistorico(indice).txtcorrecto = RegHist(0).txtcorrecto

                                If docsHistorico(indice).doc1 = CInt(cab0) Then
                                    docsHistorico(indice).xdoc2 = docsHistorico(indice).nomtra.ToString
                                    contador1 += CInt(docsHistorico(indice).nomtra)
                                End If
                            End If
                        End If
                        ''''FIN EL PRIMERO

                        'para el segundo doc, poner todo lo de arriba
                        If listayears.Count > 1 Then
                            RegHist = oDocBLL.CargarListaHist(docsHistoricox(j).clave)
                            If RegHist.Count = 0 Then
                                Continue For
                            End If
                            docsHistorico(indice).doc1 = RegHist(0).clave

                            Dim intTmp As Integer
                            If RegHist(0).nomtra <> "" Then
                                If docsHistorico(indice).nomtra = "" Then
                                    intTmp = CInt(RegHist(0).nomtra)
                                Else
                                    intTmp = CInt(RegHist(0).nomtra)
                                End If

                                docsHistorico(indice).nomtra = intTmp.ToString
                                docsHistorico(indice).NIF = RegHist(0).NIF
                                docsHistorico(indice).codtra = RegHist(0).codtra
                                docsHistorico(indice).txtcorrecto = RegHist(0).txtcorrecto


                                If docsHistorico(indice).doc1 = CInt(cab1) Then
                                    docsHistorico(indice).xdoc3 = docsHistorico(indice).nomtra.ToString
                                    contador2 += CInt(docsHistorico(indice).nomtra)
                                End If
                            End If
                        End If

                        If listayears.Count > 2 Then
                            RegHist = oDocBLL.CargarListaHist(docsHistoricox(j).clave)
                            If RegHist.Count = 0 Then
                                Continue For
                            End If
                            docsHistorico(indice).doc1 = RegHist(0).clave

                            Dim intTmp As Integer
                            If RegHist(0).nomtra <> "" Then
                                If docsHistorico(indice).nomtra = "" Then
                                    intTmp = CInt(RegHist(0).nomtra)
                                Else
                                    intTmp = CInt(RegHist(0).nomtra)
                                End If

                                docsHistorico(indice).nomtra = intTmp.ToString
                                docsHistorico(indice).NIF = RegHist(0).NIF
                                docsHistorico(indice).codtra = RegHist(0).codtra
                                docsHistorico(indice).txtcorrecto = RegHist(0).txtcorrecto


                                If docsHistorico(indice).doc1 = CInt(cab2) Then
                                    docsHistorico(indice).xdoc4 = docsHistorico(indice).nomtra.ToString
                                    contador3 += CInt(docsHistorico(indice).nomtra)
                                End If
                            End If
                        End If

                        If listayears.Count > 3 Then

                            RegHist = oDocBLL.CargarListaHist(docsHistoricox(j).clave)
                            If RegHist.Count = 0 Then
                                Continue For
                            End If
                            docsHistorico(indice).doc1 = RegHist(0).clave

                            Dim intTmp As Integer
                            If RegHist(0).nomtra <> "" Then
                                If docsHistorico(indice).nomtra = "" Then
                                    intTmp = CInt(RegHist(0).nomtra)
                                Else
                                    intTmp = CInt(RegHist(0).nomtra)
                                End If

                                docsHistorico(indice).nomtra = intTmp.ToString
                                docsHistorico(indice).NIF = RegHist(0).NIF
                                docsHistorico(indice).codtra = RegHist(0).codtra
                                docsHistorico(indice).txtcorrecto = RegHist(0).txtcorrecto

                                If docsHistorico(indice).doc1 = CInt(cab3) Then
                                    docsHistorico(indice).xdoc5 = docsHistorico(indice).nomtra.ToString
                                    contador4 += CInt(docsHistorico(indice).nomtra)
                                End If
                            End If

                        End If
                        If listayears.Count > 4 Then
                            RegHist = oDocBLL.CargarListaHist(docsHistoricox(j).clave)
                            If RegHist.Count = 0 Then
                                Continue For
                            End If
                            docsHistorico(indice).doc1 = RegHist(0).clave

                            Dim intTmp As Integer
                            If RegHist(0).nomtra <> "" Then
                                If docsHistorico(indice).nomtra = "" Then
                                    intTmp = CInt(RegHist(0).nomtra)
                                Else
                                    intTmp = CInt(RegHist(0).nomtra)
                                End If

                                docsHistorico(indice).nomtra = intTmp.ToString
                                docsHistorico(indice).NIF = RegHist(0).NIF
                                docsHistorico(indice).codtra = RegHist(0).codtra
                                docsHistorico(indice).txtcorrecto = RegHist(0).txtcorrecto


                                If docsHistorico(indice).doc1 = CInt(cab4) Then
                                    docsHistorico(indice).xdoc6 = docsHistorico(indice).nomtra.ToString
                                    contador5 += CInt(docsHistorico(indice).nomtra)
                                End If
                            End If
                        End If
                        If listayears.Count > 5 Then

                            RegHist = oDocBLL.CargarListaHist(docsHistoricox(j).clave)
                            If RegHist.Count = 0 Then
                                Continue For
                            End If
                            docsHistorico(indice).doc1 = RegHist(0).clave

                            Dim intTmp As Integer
                            If RegHist(0).nomtra <> "" Then
                                If docsHistorico(indice).nomtra = "" Then
                                    intTmp = CInt(RegHist(0).nomtra)
                                Else
                                    intTmp = CInt(RegHist(0).nomtra)
                                End If

                                docsHistorico(indice).nomtra = intTmp.ToString
                                docsHistorico(indice).NIF = RegHist(0).NIF
                                docsHistorico(indice).codtra = RegHist(0).codtra
                                docsHistorico(indice).txtcorrecto = RegHist(0).txtcorrecto

                                If docsHistorico(indice).doc1 = CInt(cab5) Then
                                    docsHistorico(indice).xdoc7 = docsHistorico(indice).nomtra.ToString
                                    contador6 += CInt(docsHistorico(indice).nomtra)
                                End If
                            End If
                        End If
                        If listayears.Count > 6 Then

                            RegHist = oDocBLL.CargarListaHist(docsHistoricox(j).clave)
                            If RegHist.Count = 0 Then
                                Continue For
                            End If
                            docsHistorico(indice).doc1 = RegHist(0).clave

                            Dim intTmp As Integer
                            If RegHist(0).nomtra <> "" Then
                                If docsHistorico(indice).nomtra = "" Then
                                    intTmp = CInt(RegHist(0).nomtra)
                                Else
                                    intTmp = CInt(RegHist(0).nomtra)
                                End If

                                docsHistorico(indice).nomtra = intTmp.ToString
                                docsHistorico(indice).NIF = RegHist(0).NIF
                                docsHistorico(indice).codtra = RegHist(0).codtra
                                docsHistorico(indice).txtcorrecto = RegHist(0).txtcorrecto

                                If docsHistorico(indice).doc1 = CInt(cab6) Then
                                    docsHistorico(indice).xdoc8 = docsHistorico(indice).nomtra.ToString
                                    contador7 += CInt(docsHistorico(indice).nomtra)
                                End If
                            End If
                        End If
                        If listayears.Count > 7 Then

                            RegHist = oDocBLL.CargarListaHist(docsHistoricox(j).clave)
                            If RegHist.Count = 0 Then
                                Continue For
                            End If
                            docsHistorico(indice).doc1 = RegHist(0).clave

                            Dim intTmp As Integer
                            If RegHist(0).nomtra <> "" Then
                                If docsHistorico(indice).nomtra = "" Then
                                    intTmp = CInt(RegHist(0).nomtra)
                                Else
                                    intTmp = CInt(RegHist(0).nomtra)
                                End If

                                docsHistorico(indice).nomtra = intTmp.ToString
                                docsHistorico(indice).NIF = RegHist(0).NIF
                                docsHistorico(indice).codtra = RegHist(0).codtra
                                docsHistorico(indice).txtcorrecto = RegHist(0).txtcorrecto

                                If docsHistorico(indice).doc1 = CInt(cab7) Then
                                    docsHistorico(indice).xdoc9 = docsHistorico(indice).nomtra.ToString
                                    contador8 += CInt(docsHistorico(indice).nomtra)
                                End If
                            End If
                        End If
                        If listayears.Count > 8 Then

                            RegHist = oDocBLL.CargarListaHist(docsHistoricox(j).clave)
                            If RegHist.Count = 0 Then
                                Continue For
                            End If
                            docsHistorico(indice).doc1 = RegHist(0).clave

                            Dim intTmp As Integer
                            If RegHist(0).nomtra <> "" Then
                                If docsHistorico(indice).nomtra = "" Then
                                    intTmp = CInt(RegHist(0).nomtra)
                                Else
                                    intTmp = CInt(RegHist(0).nomtra)
                                End If

                                docsHistorico(indice).nomtra = intTmp.ToString
                                docsHistorico(indice).NIF = RegHist(0).NIF
                                docsHistorico(indice).codtra = RegHist(0).codtra
                                docsHistorico(indice).txtcorrecto = RegHist(0).txtcorrecto

                                If docsHistorico(indice).doc1 = CInt(cab8) Then
                                    docsHistorico(indice).xdoc10 = docsHistorico(indice).nomtra.ToString
                                    contador9 += CInt(docsHistorico(indice).nomtra)
                                End If
                            End If
                        End If
                        'If listayears.Count > 9 Then
                        '    If docsHistorico(indice).doc1 = CInt(cab9) Then
                        '        docsHistorico(indice).xdoc11 = docsHistorico(indice).nomtra.ToString
                        '        contador10 += CInt(docsHistorico(indice).nomtra)
                        '    End If

                        'End If
                    Next


                Next
                'FIN POR CADA TRABAJADOR HARE ESTE BUCLE



                'habria que reordenar docsHistorico y hacer select y asignarselo a   
                Dim valores As String
                Dim valoresdoc As String = ""
                oDocBLL.DeleteMatriz()
                For k = 0 To docsHistorico.Count - 1
                    If docsHistorico(k).xdoc2 <> "" Then
                        valoresdoc = "xdoc2" & "," & docsHistorico(k).xdoc2
                    End If
                    If docsHistorico(k).xdoc3 <> "" Then
                        valoresdoc = "xdoc3" & "," & docsHistorico(k).xdoc3
                    End If
                    If docsHistorico(k).xdoc4 <> "" Then
                        valoresdoc = "xdoc4" & "," & docsHistorico(k).xdoc4
                    End If
                    If docsHistorico(k).xdoc5 <> "" Then
                        valoresdoc = "xdoc5" & "," & docsHistorico(k).xdoc5
                    End If
                    If docsHistorico(k).xdoc6 <> "" Then
                        valoresdoc = "xdoc6" & "," & docsHistorico(k).xdoc6
                    End If
                    If docsHistorico(k).xdoc7 <> "" Then
                        valoresdoc = "xdoc7" & "," & docsHistorico(k).xdoc7
                    End If
                    If docsHistorico(k).xdoc8 <> "" Then
                        valoresdoc = "xdoc8" & "," & docsHistorico(k).xdoc8
                    End If
                    If docsHistorico(k).xdoc9 <> "" Then
                        valoresdoc = "xdoc9" & "," & docsHistorico(k).xdoc9
                    End If
                    If docsHistorico(k).xdoc10 <> "" Then
                        valoresdoc = "xdoc10" & "," & docsHistorico(k).xdoc10
                    End If
                    If docsHistorico(k).xdoc11 <> "" Then
                        valoresdoc = "xdoc11" & "," & docsHistorico(k).xdoc11
                    End If
                    If docsHistorico(k).xdoc12 <> "" Then
                        valoresdoc = "xdoc12" & "," & docsHistorico(k).xdoc12
                    End If
                    'miramos si esta

                    RegistroMatriz = oDocBLL.CargarListaMatClave(docsHistorico(k).codtra.ToString)
                    If RegistroMatriz.Count > 0 Then 'modificar
                        'si hubiera 2 el mismo año habria que mirarlo aqui
                        ' cod sin hacer del mismo año
                        valores = RegistroMatriz(0).Abrev & docsHistorico(k).doc1.ToString & "," & valoresdoc & "," & docsHistorico(k).ubicacion & ";"
                        oDocBLL.SaveMatriz(docsHistorico(k).codtra.ToString, valores)


                    Else 'insertar
                        If valoresdoc <> "" Then


                            valores = docsHistorico(k).doc1.ToString & "," & valoresdoc & "," & docsHistorico(k).ubicacion & ";"

                            If docsHistorico(k).codtra <> Integer.MinValue Then
                                oDocBLL.SaveMatrizInsert(docsHistorico(k).codtra.ToString, valores)
                            End If

                        End If
                    End If


                Next


                Dim contalinea As Integer = 0
                Dim array1 As String() = Nothing
                Dim array2 As String() = Nothing
                RegMatriz = oDocBLL.CargarListaMat()
                For i = 0 To RegMatriz.Count - 1
                    docsHistorico(i).xdoc2 = ""
                    docsHistorico(i).xdoc3 = ""
                    docsHistorico(i).xdoc4 = ""
                    docsHistorico(i).xdoc5 = ""
                    docsHistorico(i).xdoc6 = ""
                    docsHistorico(i).xdoc7 = ""
                    docsHistorico(i).xdoc8 = ""
                    docsHistorico(i).xdoc9 = ""
                    docsHistorico(i).xdoc10 = ""
                    docsHistorico(i).xdoc11 = ""
                    docsHistorico(i).xdoc12 = ""

                    array1 = Split(RegMatriz(i).Abrev, ";")
                    For j = 0 To array1.Count - 2


                        array2 = Split(array1(j), ",")

                        docsHistorico(i).doc1 = array2(0)
                        If array2(1) = "xdoc2" Then
                            docsHistorico(i).xdoc2 = array2(2)
                            docsHistorico(i).fdoc2 = array2(3)
                            contalinea += CInt(array2(2))
                        End If
                        If array2(1) = "xdoc3" Then
                            docsHistorico(i).xdoc3 = array2(2)
                            docsHistorico(i).fdoc3 = array2(3)
                            contalinea += CInt(array2(2))
                        End If
                        If array2(1) = "xdoc4" Then
                            docsHistorico(i).xdoc4 = array2(2)
                            docsHistorico(i).fdoc4 = array2(3)
                            contalinea += CInt(array2(2))
                        End If
                        If array2(1) = "xdoc5" Then
                            docsHistorico(i).xdoc5 = array2(2)
                            docsHistorico(i).fdoc5 = array2(3)
                            contalinea += CInt(array2(2))
                        End If
                        If array2(1) = "xdoc6" Then
                            docsHistorico(i).xdoc6 = array2(2)
                            docsHistorico(i).fdoc6 = array2(3)
                            contalinea += CInt(array2(2))
                        End If
                        If array2(1) = "xdoc7" Then
                            docsHistorico(i).xdoc7 = array2(2)
                            docsHistorico(i).fdoc7 = array2(3)
                            contalinea += CInt(array2(2))
                        End If
                        If array2(1) = "xdoc8" Then
                            docsHistorico(i).xdoc8 = array2(2)
                            docsHistorico(i).fdoc8 = array2(3)
                            contalinea += CInt(array2(2))
                        End If
                        If array2(1) = "xdoc9" Then
                            docsHistorico(i).xdoc9 = array2(2)
                            docsHistorico(i).fdoc9 = array2(3)
                            contalinea += CInt(array2(2))
                        End If
                        If array2(1) = "xdoc10" Then
                            docsHistorico(i).xdoc10 = array2(2)
                            docsHistorico(i).fdoc10 = array2(3)
                            contalinea += CInt(array2(2))
                        End If
                        If array2(1) = "xdoc11" Then
                            docsHistorico(i).xdoc11 = array2(2)
                            docsHistorico(i).fdoc11 = array2(3)
                            contalinea += CInt(array2(2))
                        End If
                        If array2(1) = "xdoc12" Then
                            docsHistorico(i).xdoc12 = array2(2)
                            docsHistorico(i).fdoc12 = array2(3)
                            contalinea += CInt(array2(2))
                        End If
                        'poner nombre tra y su desc curso
                        docsHistorico(i).txtcorrecto = RegMatriz(i).NIF
                        'buscar su nombre
                        Dim listaTypeTra As List(Of ELL.Trabajadores)
                        listaTypeTra = oDocBLL.CargarListaTrabajadoresClaveTra(PageBase.plantaAdmin, CInt(docsHistorico(i).txtcorrecto))
                        docsHistorico(i).txtcorrecto = listaTypeTra(0).Nombre
                        docsHistorico(i).Abrev = listaTypeTra(0).DescResponsable

                        docsHistorico(i).NIF = txtEmpresa.Text

                    Next





                    docsHistorico(i).nomtra = contalinea.ToString
                    contalinea = 0

                Next





                contador0 = contador1 + contador2 + contador3 + contador4 + contador5 + contador6 + contador7 + contador8 + contador9 + contador10 + contador11
                final(1).NIF = ""
                final(1).nomtra = ""
                final(1).xdoc2 = ""
                final(1).xdoc3 = ""
                final(1).xdoc4 = ""
                final(1).xdoc5 = ""
                final(1).xdoc6 = ""
                final(1).xdoc7 = ""
                final(1).xdoc8 = ""
                final(1).xdoc9 = ""
                final(1).xdoc10 = ""
                final(1).xdoc11 = ""
                final(1).xdoc12 = ""


                final(0).NIF = "Total"
                final(0).nomtra = contador0.ToString
                final(0).ubicacion = ""
                If contador3 > 0 Then
                    final(0).xdoc4 = contador3.ToString
                End If
                If contador1 > 0 Then
                    final(0).xdoc2 = contador1.ToString
                End If
                If contador2 > 0 Then
                    final(0).xdoc3 = contador2.ToString
                End If
                If contador4 > 0 Then
                    final(0).xdoc5 = contador4.ToString
                End If
                If contador5 > 0 Then
                    final(0).xdoc6 = contador5.ToString
                End If
                If contador6 > 0 Then
                    final(0).xdoc7 = contador6.ToString
                End If
                If contador7 > 0 Then
                    final(0).xdoc8 = contador7.ToString
                End If
                If contador8 > 0 Then
                    final(0).xdoc9 = contador8.ToString
                End If
                If contador9 > 0 Then
                    final(0).xdoc10 = contador9.ToString
                End If
                If contador10 > 0 Then
                    final(0).xdoc11 = contador10.ToString
                End If
                If contador11 > 0 Then
                    final(0).xdoc12 = contador11.ToString
                End If

                docsHistorico.RemoveRange(RegMatriz.Count, docsHistorico.Count - RegMatriz.Count)

                Registros.Text = docsHistorico.Count.ToString

                docsHistorico.Add(final(1))
                docsHistorico.Add(final(0))

                For k = 0 To docsHistorico.Count - 1
                    docsHistorico(k).Comentario = k.ToString
                Next


                gvType2.DataSource = docsHistorico
                gvType2.DataBind()
                gvType2.Caption = String.Empty
            Else
                Registros.Text = "0"
                '     Registros2.Text = "0"
                gvType2.DataSource = Nothing
                gvType2.DataBind()
                gvType2.Caption = "No hay cursos asignados"
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
            PageBase.plantaAdmin = 1
            mView.ActiveViewIndex = 1
            If txtEmpresa.Text = "" Then
                txtProfesion.ReadOnly = True
                txtResponsable.ReadOnly = True
                ddlTodos.Visible = False
                Label13.Visible = False
            Else
                txtProfesion.ReadOnly = False
                txtResponsable.ReadOnly = False
                ddlTodos.Visible = True
                Label13.Visible = True
            End If
            BindDataView2()


        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar el estado de documentos de un trabajador", ex)
        End Try
    End Sub



    Protected Sub gvType2_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvType2.RowCommand

        If e.CommandName = "Curso2" Then
            Dim oDocBLL As New BLL.DocumentosBLL
            Dim DesEmpre As String = e.CommandArgument '  DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).nombre

            Dim idDocumento As String
            If DesEmpre <> "" Then
                hfurl.Value = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & DesEmpre
                idDocumento = DirFicherosBajar & "Ficheros_Matriz_Puestos/Documentos/" & DesEmpre
                Dim script As String = "<script type=""text/javascript"">window.open('" & idDocumento & "');</script>"
                ClientScript.RegisterStartupScript(Me.GetType, "openWindow", script)
            Else
                '         Master.MensajeError = ItzultzaileWeb.Itzuli("No se ha encontrado el documento").ToUpper 'error 
            End If
        End If

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


    Private Sub gvType2_PreRender(sender As Object, e As EventArgs) Handles gvType2.PreRender
        gvType2.Columns(5).HeaderText = cab0
        gvType2.Columns(6).HeaderText = cab1
        gvType2.Columns(7).HeaderText = cab2
        gvType2.Columns(8).HeaderText = cab3
        gvType2.Columns(9).HeaderText = cab4
        gvType2.Columns(10).HeaderText = cab5
        gvType2.Columns(11).HeaderText = cab6
        gvType2.Columns(12).HeaderText = cab7
        gvType2.Columns(13).HeaderText = cab8
        gvType2.Columns(14).HeaderText = cab9
        gvType2.Columns(15).HeaderText = cab10
        gvType2.Columns(16).HeaderText = "Responsable"
        gvType2.DataBind()
    End Sub
End Class