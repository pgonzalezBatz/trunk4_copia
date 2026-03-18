
Imports System.Data


Public Class Calculo
    Inherits PageBase
    'Inherits System.Web.UI.Page

    Dim oDocumentosBLL As New KaplanLib.BLL.DocumentosBLL



#Region "Propiedades"

    Protected Overrides ReadOnly Property RolesAcceso As System.Collections.Generic.List(Of KaplanLib.ELL.Roles.RolUsuario)
        Get
            Dim roles As New List(Of KaplanLib.ELL.Roles.RolUsuario)
            roles.Add(KaplanLib.ELL.Roles.RolUsuario.Administrador)
            roles.Add(KaplanLib.ELL.Roles.RolUsuario.Administrador2)
            roles.Add(KaplanLib.ELL.Roles.RolUsuario.Recepcion)
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

    ''' <summary>
    ''' Vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BindDataView()
        Try
            Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            'Dim FechaDesde As Date = Now.AddMonths(-6).Date
            Dim strSql As String = "select * from cubos.DESPIECES where t2tenr='" & hfReferencia.Value & "'  and T2KOMP ='" & hfComponente.Value & "'"
            Dim cn As New OleDb.OleDbConnection()
            Dim dr As OleDb.OleDbDataReader
            Dim dr2 As OleDb.OleDbDataReader

            cn.ConnectionString = ConfigurationManager.ConnectionStrings("AS400").ConnectionString
            Dim cm As New OleDb.OleDbCommand(strSql, cn)
            cm.CommandTimeout = 30
            cn.Open()
            dr = cm.ExecuteReader()
            Dim plantas As String = ""
            'Dim aNHI As New List(Of String)

            Dim cont As Integer = -1

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
                txtDescref.Text = If(dr("T2DENO") Is Nothing OrElse dr("T2DENO") Is DBNull.Value, txtDescref.Text, dr("T2DENO"))

            End While

            'fin prueba 3 carac listaType2, 2 atrib listaType
            Dim listaType2 As List(Of KaplanLib.ELL.Asociacion)
            'Dim listaTypeDatos As List(Of KaplanLib.ELL.Asociacion)
            'listaTypeDatos.Clear()
            'listaTypeDatos = oDocBLL.CargarListaAsociaciones3(hfReferencia.Value, hfComponente.Value)
            Dim listaType As New List(Of KaplanLib.ELL.Asociacion)
            'listaType.Clear()

            'una linea por cada cont, proceso step, listaType2. Los que tienen 666. el grid tienen listaType2.count lineas
            listaType2 = oDocBLL.CargarListaAsociaciones2(hfReferencia.Value, hfComponente.Value) 'work, step


            dr.Close()
            cn.Close()
            cn.Dispose()

            Dim listaTypeStep As New List(Of KaplanLib.ELL.Asociacion)
            listaTypeStep.Clear()
            For i = 0 To listaType2.Count - 1
                'voy a sacar para cada 666 distinto, las lineas que tiene
                'listaType = oDocBLL.CargarListaAsociaciones3(listaType2(i).cont, hfReferencia.Value, hfComponente.Value, listaType2(i).Work, listaType2(i).Steps)

                listaTypeStep = oDocBLL.CargarListaAsociaciones4(listaType2(i).cont, hfReferencia.Value, listaType2(i).componente, listaType2(i).Work, listaType2(i).Steps)
                For k = 0 To listaTypeStep.Count - 1
                    listaTypeStep(k).Max = listaTypeStep(k).Max / 1000
                    listaTypeStep(k).Min = listaTypeStep(k).Min / 1000
                Next

                'aqui leo descriciones de codigos y los meto, y añado lineas por hijos
                'sacar desc del proceso
                Dim listaType4 As New List(Of KaplanLib.ELL.Kaplan)
                listaType4 = oDocBLL.CargarTiposEmpresaProcess(listaType2(i).Work, 0)
                If listaType4.Count > 0 Then
                    listaType2(i).desc_process = listaType4(0).Nombre
                Else
                    listaType2(i).desc_process = ""
                End If
                Dim lista As List(Of KaplanLib.ELL.Kaplan)
                lista = oDocBLL.CargarTiposEmpresa(listaType2(i).Steps, 0)
                listaType2(i).Textolibre = lista(0).Nombre 'lo uso para nombre step
                listaType2(i).Textolibre2 = lista(0).textolibre 'lo uso para nombre work
                If listaTypeStep.Count > 0 Then


                    Dim arrHijos As String() = Nothing
                    arrHijos = Split(listaTypeStep(0).Hijos, ";")

                    If arrHijos.Length > 0 Then
                        listaType2(i).Hijos = "" 'le voy a añadir la desc
                        For j = 0 To arrHijos.Length - 2
                            Dim item As New KaplanLib.ELL.Asociacion

                            item.referencia = listaType2(i).referencia
                            'item.Id = listaType2(i).componente
                            item.componente = listaType2(i).componente
                            item.Process = listaType2(i).Work  'el campo work hace referencia al proceso



                            item.Steps = listaType2(i).Steps
                            'sacar el work del step de la tabla stepswork
                            Dim listaType3 As New List(Of KaplanLib.ELL.Asociacion)
                            listaType3 = oDocBLL.CargarListaWork(item.Steps)
                            If listaType3.Count > 0 Then
                                item.Work = listaType3(0).Work
                            Else
                                item.Work = 0
                            End If




                            item.Hijos = arrHijos(j)
                            'mirar la desc de este hijo EN AS400
                            Dim strSql2 As String = "select T2DENO from cubos.DESPIECES where T2KOMP='" & arrHijos(j) & "'"
                            Dim cn2 As New OleDb.OleDbConnection()

                            cn2.ConnectionString = ConfigurationManager.ConnectionStrings("AS400").ConnectionString
                            Dim cm2 As New OleDb.OleDbCommand(strSql2, cn2)
                            cm2.CommandTimeout = 30
                            cn2.Open()


                            dr2 = cm2.ExecuteReader()
                            While dr2.Read
                                'item.desc_comp = dr2("T2DENO")
                                item.desc_comp = If(dr2("T2DENO") Is Nothing OrElse dr2("T2DENO") Is DBNull.Value, item.desc_comp, dr2("T2DENO"))
                            End While

                            dr2.Close()
                            cn2.Close()
                            cn2.Dispose()

                            'item.desc_comp = arrHijos(j) & "mirar desc hijo"


                            item.Caracteristica = listaType2(i).Caracteristica & "<br>" & listaType2(i).Caracteristica2
                            'listaTypeDatos.Add(item)




                            listaType2(i).Hijos = listaType2(i).Hijos & arrHijos(j) & " " & item.desc_comp & "<br>"


                        Next
                    End If


                    'descripciones  , steps , work    (desc_process ya esta)
                    Dim condiciones As String = ""
                    Dim caracteristic As String = ""
                    Dim sumURL As String = ""
                    Dim atrib As String = "<table class='BatzFont' style='text-align:left';font-size:small;><tbody><tr><td><strong>Trabajo STD&nbsp;&nbsp;&nbsp;</strong></td><td><strong>Parámetro Proceso&nbsp;&nbsp;&nbsp;</strong></td><td><strong>Specification&nbsp;&nbsp;&nbsp;</strong></td><td><strong>Clase&nbsp;&nbsp;&nbsp;</strong></td> <td><strong>Max&nbsp;&nbsp;&nbsp;</strong></td><td><strong>Min</strong></td></tr>"

                    'clase, tipo, descripcion, specificacion, max, min


                    'desglose de caracteristicas
                    For m = 0 To listaTypeStep.Count - 1
                        sumURL = hfReferencia.Value & "&idComp=" & listaTypeStep(m).componente & "&idCaract=" & listaTypeStep(m).Id
                        If listaTypeStep(m).TipoC <> 666 Then
                            If listaTypeStep(m).TipoC = 999 Then 'atributos. 
                                atrib = atrib & "&nbsp;<tr><td><span style='font-size:small; color: #3366ff;'>" & listaTypeStep(m).TrabajoSTD & "&nbsp;&nbsp;&nbsp;</td><td><span style='font-size:small; color: #3366ff;'>" & listaTypeStep(m).Parametro & "&nbsp;&nbsp;&nbsp;</td><td><span style='font-size:small; color: #3366ff;'>" & listaTypeStep(m).Caracteristica & "&nbsp;&nbsp;&nbsp;</td><td><span style='font-size:small;color: #3366ff;'>" & listaTypeStep(m).Caracteristica2 & "&nbsp;&nbsp;&nbsp;</td><td><span style='font-size:small;color: #3366ff;'>" & listaTypeStep(m).Max & "&nbsp;&nbsp;&nbsp;</td><td><span style='font-size:small;color: #3366ff;'>" & listaTypeStep(m).Min & "</td></tr>"

                            Else
                                'lo de abajo se suman cada linea de modo fallo
                                'condiciones = "<br>" & listaTypeStep(m).Condicion1 & ", <br>" & listaTypeStep(m).Condicion2
                                If listaTypeStep(m).Condicion1 <> "" Then
                                    condiciones = "<br><a href='./CrearCausas.aspx?idCond=1&idRef=" & listaTypeStep(m).Id & "'><input type ='button' value='" & listaTypeStep(m).Condicion1 & "'/></a>"
                                End If
                                If listaTypeStep(m).Condicion2 <> "" Then
                                    condiciones = condiciones & "<br><a href='./CrearCausas.aspx?idCond=2&idRef=" & listaTypeStep(m).Id & "'><input type ='button' value='" & listaTypeStep(m).Condicion2 & "'/></a>"
                                End If
                                If listaTypeStep(m).Condicion3 <> "" Then
                                    condiciones = condiciones & "<br><a href='./CrearCausas.aspx?idCond=3&idRef=" & listaTypeStep(m).Id & "'><input type ='button' value='" & listaTypeStep(m).Condicion3 & "'/></a>"
                                End If
                                If listaTypeStep(m).Condicion4 <> "" Then
                                    condiciones = condiciones & "<br><a href='./CrearCausas.aspx?idCond=4&idRef=" & listaTypeStep(m).Id & "'><input type ='button' value='" & listaTypeStep(m).Condicion4 & "'/></a>"
                                End If
                                If listaTypeStep(m).Condicion5 <> "" Then
                                    condiciones = condiciones & "<br><a href='./CrearCausas.aspx?idCond=5&idRef=" & listaTypeStep(m).Id & "'><input type ='button' value='" & listaTypeStep(m).Condicion5 & "'/></a>"
                                End If
                                If listaTypeStep(m).Condicion6 <> "" Then
                                    condiciones = condiciones & "<br><a href='./CrearCausas.aspx?idCond=6&idRef=" & listaTypeStep(m).Id & "'><input type ='button' value='" & listaTypeStep(m).Condicion6 & "'/></a>"
                                End If
                                'lo de arriba se suman cada linea de modo fallo

                                caracteristic = caracteristic & "<table class='BatzFont' style='text-align:left';font-size:small;><tbody><tr><td><strong>Modo Fallo&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</strong></td><td>Clase&nbsp;&nbsp;&nbsp;</td><td>Tipo&nbsp;&nbsp;&nbsp;</td><td>Característica&nbsp;&nbsp;&nbsp;</td><td>Specificacion&nbsp;&nbsp;&nbsp;</td> <td>Max&nbsp;&nbsp;&nbsp;</td><td>Min</td></tr>"

                                caracteristic = caracteristic & "&nbsp;<tr><td><span style='font-size:small;color: #3366ff;'>" & "" & "&nbsp;&nbsp;&nbsp;</td><td><span style='font-size:small;color: #3366ff;'>" & listaTypeStep(m).tClaseC & "&nbsp;&nbsp;&nbsp;</td><td><span style='font-size:small;color: #3366ff;'>" & listaTypeStep(m).tTipoC & "&nbsp;&nbsp;&nbsp;</td><td><span style='font-size:small;color: #3366ff;'>" & listaTypeStep(m).Caracteristica & "&nbsp;&nbsp;&nbsp;</td><td><span style='font-size:small;color: #3366ff;'>" & listaTypeStep(m).Caracteristica2 & "&nbsp;&nbsp;&nbsp;</td><td><span style='font-size:small;color: #3366ff;'>" & listaTypeStep(m).Max & "&nbsp;&nbsp;&nbsp;</td><td><span style='font-size:small;color: #3366ff;'>" & listaTypeStep(m).Min & "</td></tr>"
                                caracteristic = caracteristic & "</tbody></table>"


                                caracteristic = caracteristic & "<table class='BatzFont' style='text-align:left';font-size:small;><tbody><tr><td><strong></strong></td></tr>"

                                'una linea para todas los modo de fallo lo de abajo
                                caracteristic = caracteristic & "<tr><td><span style='font-size:small;color: #ff6600;'>" & condiciones & "&nbsp;&nbsp;&nbsp;</td></tr>"

                                caracteristic = caracteristic & "</tbody></table>"
                                condiciones = ""
                            End If
                        End If
                    Next

                    atrib = atrib & "</tbody></table>"
                    listaType2(i).tClaseC = caracteristic  'lo uso para caracter
                    listaType2(i).tTipoC = atrib


                    listaType2(i).Condicion1 = condiciones
                Else
                    'puede ser que solo tenga hijos y ninguna linea de caracteristicas, por eso no sale la descripcion del hijo
                    'se buscaria si existe linea con hijos de 666 (caso raro)
                End If


                'hay que poner nombre work, no desc, lo saco de lista(0).id en steps
                Dim desc As List(Of KaplanLib.ELL.Kaplan)
                desc = oDocBLL.CargarTiposEmpresaWork(lista(0).Work, 0)
                If desc.Count > 0 Then
                    listaType2(i).Textolibre2 = desc(0).Nombre
                Else
                    listaType2(i).Textolibre2 = ""
                End If


            Next







            If (listaType2.Count > 0) Then 'listatype tendra tantos registros como hijos, el 2 solo como steps

                gvType.DataSource = listaType2
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

    Protected Sub BindDataView2()
        Try
            'Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            'Dim i As Int16
            'Dim listaType2 As List(Of KaplanLib.ELL.Documentos)

            'listaType2 = oDocBLL.CargarListaEmpDocTot(PageBase.plantaAdmin)  'codemp

            'If (listaType2.Count > 0) Then
            '    For i = 0 To listaType2.Count - 1
            '        'leer si existe el registro en adok_emd
            '        listaType2(i).Asignada = 0
            '        If idEmpresa.Value > 0 Then
            '            If oDocumentosBLL.CargarListaEmpDoc(PageBase.plantaAdmin, CInt(idEmpresa.Value), listaType2(i).Id).Count > 0 Then
            '                listaType2(i).Asignada = 1
            '                'Else
            '                '    listaType2(i).Asignada = 0
            '            End If
            '        End If
            '        listaType2(i).Empresa = idEmpresa.Value

            '    Next

            '    gvType2.DataSource = listaType2
            '    gvType2.DataBind()
            '    gvType2.Caption = String.Empty
            'Else

            '    gvType2.DataSource = Nothing
            '    gvType2.DataBind()
            '    gvType2.Caption = "No hay registros"
            'End If


        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar un documento", ex)
        End Try
    End Sub
    Protected Sub BindDataViewEmp(ByVal Codemp As Integer)
        Try
            'Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            'Dim i As Int16
            'Dim listaType2 As List(Of KaplanLib.ELL.Documentos)

            'listaType2 = oDocBLL.CargarListaEmpDocTot(PageBase.plantaAdmin)  'codemp

            'If (listaType2.Count > 0) Then
            '    For i = 0 To listaType2.Count - 1
            '        'leer si existe el registro en adok_emd

            '        If oDocumentosBLL.CargarListaEmpDoc(PageBase.plantaAdmin, Codemp, listaType2(i).Id).Count > 0 Then

            '            listaType2(i).Asignada = 1
            '        Else
            '            listaType2(i).Asignada = 0
            '        End If
            '        listaType2(i).Empresa = Codemp

            '    Next
            '    gvType2.DataSource = listaType2
            '    gvType2.DataBind()
            '    gvType2.Caption = String.Empty
            'Else
            '    gvType2.DataSource = Nothing
            '    gvType2.DataBind()
            '    gvType2.Caption = "No hay registros"
            'End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar un documento", ex)
        End Try
    End Sub





    'Protected Sub BtnMarcarEmpresas_Click(sender As Object, e As EventArgs) Handles BtnMarcarEmpresas.Click
    '    BindDataView3()
    '    ConfiguracionProduct3()
    'End Sub



    Private Sub ConfiguracionProduct3()
        'aqui poner los datos de la empresa
        Try
            Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL

            Dim userBLL As New SabLib.BLL.UsuariosComponent

            idEmpresa.Value = -1
            txtCIF.Text = ""
            txtNombre.Text = "Todas Empresas"

            mView.ActiveViewIndex = 1
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar la empresa", ex)
        End Try

    End Sub



    Protected Sub BindDataView3()
        Try
            'Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            'Dim i As Int16
            'Dim listaType2 As List(Of KaplanLib.ELL.Documentos)

            'listaType2 = oDocBLL.CargarListaEmpDocTot(PageBase.plantaAdmin)  'codemp

            'If (listaType2.Count > 0) Then
            '    For i = 0 To listaType2.Count - 1
            '        listaType2(i).Asignada = 0
            '        listaType2(i).comentario = "true"

            '    Next
            '    gvType2.DataSource = listaType2
            '    gvType2.DataBind()
            '    gvType2.Caption = String.Empty
            'Else

            '    gvType2.DataSource = Nothing
            '    gvType2.DataBind()
            '    gvType2.Caption = "No hay registros"
            'End If


        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar un documento", ex)
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
    Protected Sub gvType2_OnRowCommand(sender As Object, e As GridViewCommandEventArgs)

    End Sub


    ''' <summary>
    ''' Cancelación de la edición del grid
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvType_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)

    End Sub
    Protected Sub gvType2_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)

    End Sub
    ''' <summary>
    ''' Habilitar la edición de un registro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvType_RowEditing(sender As Object, e As GridViewEditEventArgs)

    End Sub
    Protected Sub gvType2_RowEditing(sender As Object, e As GridViewEditEventArgs)

    End Sub
    ''' <summary>
    ''' Edición de un registro
    ''' </summary>
    ''' <param name="sender"></param> 
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvType_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)

    End Sub
    Protected Sub gvType2_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)


    End Sub



#End Region
    ''' <remarks></remarks>
    Protected Sub btnGuardarNuevaReferencia_Click(sender As Object, e As EventArgs) Handles btnGuardarNuevaSolicitud.Click
        Try
            'Dim cantidad As Int32
            'Dim intervalo As String
            'Dim periodo As Int32
            'Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            'Dim fechaCad As Date
            ''codigo 13 = no tiene intervalo
            ''calcular caducidad emd004
            ''leer de adok_doc con  idDoc.Value los valores de intervalo cantidad y si tiene caducidad. el doc003 -> per000 se saca per003 como cantidad y per002 ->int000 que saca int001=intervalo

            'periodo = ddlCaducidad.SelectedValue

            'Dim listaCad As List(Of KaplanLib.ELL.Caducidades)
            'listaCad = oDocBLL.CargarCad(PageBase.plantaAdmin, periodo)

            'cantidad = listaCad(0).cantidad

            ''leer int001
            'listaCad = oDocBLL.CargarInt(PageBase.plantaAdmin, listaCad(0).intervalo)
            'intervalo = Trim(listaCad(0).nombre)

            ''leer fecini de emd que voy a calcular
            'Dim fechaini As Date = oDocumentosBLL.CargarListaEmpDoc(PageBase.plantaAdmin, idEmpresa.Value, CInt(idDocumento.Value))(0).FecIni


            'If intervalo <> "nnnn" And fechaini > CDate("01/01/1900") Then
            '    If cantidad = 0 Then
            '        fechaCad = Date.MaxValue
            '    Else
            '        fechaCad = DateAdd(intervalo, cantidad, fechaini)
            '    End If

            'Else
            '    fechaCad = Date.MaxValue
            'End If


            'Dim docum As Int32 = CInt(idDocumento.Value)
            'Dim tipo2 As New KaplanLib.ELL.EmpresasDoc With {.Planta = PageBase.plantaAdmin, .FecCad = fechaCad, .periodicidad = ddlCaducidad.SelectedValue, .codemp = CInt(idEmpresa.Value), .coddoc = CInt(idDocumento.Value)}
            'If (oDocumentosBLL.ModificarEmdEmp(tipo2)) Then

            '    Master.MensajeInfo = "El documento se ha modificado correctamente".ToUpper
            '    BindDataView()
            'Else
            '    Master.MensajeError = "Un error ha ocurrido cuando se modificaba el documento".ToUpper
            'End If


        Catch ex As Exception
            Throw New SabLib.BatzException("Error al grabar un documento", ex)
        End Try

    End Sub
    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        mView.ActiveViewIndex = 1

    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            hfReferencia.Value = Request.QueryString("idRef")
            hfcomponente.Value = Request.QueryString("idComp")
            'ComprobarAcceso()

            If Not (Page.IsPostBack) Then
                idEmpresa.Value = 0
                '   hdnTxtEmpresa.Value = ""
                chkMarcados.Value = 0
                txtEmpresa.Focus()
                mView.ActiveViewIndex = 0

            End If

            Initialize()


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

        If e.CommandName = "Operacion" Then

            'Dim IdRef As Int32 = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).id
            Dim IdComp As Int32 = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).id
            'Response.Redirect("~/Operaciones.aspx?idRef=" & hfReferencia.Value & "&idComp=" & IdComp, False)
            oDocumentosBLL.GuardarOpeBorrar2(IdComp)
            BindDataView()

        End If

        If e.CommandName = "Operacion2" Then
            'cojo el reg 666 de asociaciones, de aqui tengo 
            Dim IdComp As Int32 = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).id
            Response.Redirect("~/OperacionesMod.aspx?idRef=" & hfReferencia.Value & "&idComp=" & IdComp, False)
            BindDataView()
        End If
        If e.CommandName = "Nivel" Then
            Dim IdComp As Int32 = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).id
            ' vamos a trabajadordocujento con ese codigo
            Response.Redirect("~/Referencia.aspx?idEmp=" & IdComp, False)
        End If
    End Sub

    Protected Sub gvType_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvType.PageIndexChanging
        gvType.PageIndex = e.NewPageIndex
        BindDataView()
    End Sub



    Protected Sub gvType2_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvType2.RowCommand
        Try


            'If e.CommandName = "Edit" Then
            '    Dim listaDoc As List(Of KaplanLib.ELL.Documentos)
            '    Dim Iddoc As Int32 = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).id
            '    listaDoc = oDocumentosBLL.CargarDocumentos(Iddoc, PageBase.plantaAdmin)

            '    Dim chkMarcado As Int16 = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).Asignada

            '    'hay que poner emd007 con tipo de periodicidad que lo da el documento

            '    Dim tipo As New KaplanLib.ELL.EmpresasDoc With {.Planta = PageBase.plantaAdmin, .codemp = idEmpresa.Value, .coddoc = Iddoc, .periodicidad = listaDoc(0).Periodo}

            '    If (oDocumentosBLL.ModificarEmpDoc(tipo, chkMarcado, Session("Ticket").nombreusuario)) Then
            '        If chkMarcado = 0 Then
            '            Master.MensajeInfo = ("Se ha asignado el documento" & " " & Iddoc & " " & "a la empresa" & " " & txtNombre.Text & " " & "correctamente").ToUpper
            '        Else
            '            Master.MensajeInfo = ("Se ha desasignado el documento" & " " & Iddoc & " " & "a la empresa" & " " & txtNombre.Text & " " & "correctamente").ToUpper
            '        End If

            '        BindDataView2()
            '    Else
            '        If chkMarcado = 0 Then
            '            Master.MensajeError = "Un error ha ocurrido cuando se asignaba el documento" & " " & Iddoc & " " & "a la empresa" & " " & txtNombre.Text
            '        Else
            '            Master.MensajeError = "Un error ha ocurrido cuando se desasignaba el documento" & " " & Iddoc & " " & "a la empresa" & " " & txtNombre.Text
            '        End If

            '    End If
            'End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar un documento", ex)
        End Try
    End Sub




    ''' <summary>
    ''' Cargar los tipos y Habilitar/Deshabilitar Transmission Mode dependiendo del producto seleccionado
    ''' </summary>
    ''' <param name="idDocumento"></param>
    ''' <remarks></remarks>
    Private Sub ConfiguracionProduct(ByVal idDocumento As Integer)
        'aqui poner los datos de la empresa
        Try
            Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            Dim lista As List(Of KaplanLib.ELL.Kaplan)
            Dim userBLL As New SabLib.BLL.UsuariosComponent

            lista = oDocBLL.CargarTiposEmpresa(idDocumento, PageBase.plantaAdmin)

            'txtCIF.Text = lista(0).Nif
            'txtNombre.Text = lista(0).Nombre

            mView.ActiveViewIndex = 1
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar la empresa", ex)
        End Try

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


    Protected Sub btnQuitarSeleccionados_Click(sender As Object, e As EventArgs) Handles btnQuitarSeleccionados.Click
        Try



            'Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            'Dim i As Int16
            'Dim listaType2 As List(Of KaplanLib.ELL.Documentos)
            'listaType2 = oDocBLL.CargarListaEmpDocTot(PageBase.plantaAdmin)  'codemp


            'If idEmpresa.Value = -1 Then 'inserto si no existe
            '    'recorrer todas las empresas y poner emd si no existe
            '    'For i = 0 To gvType2.Rows.Count - 1


            '    '    Dim CheckBoxElim As CheckBox = CType(gvType2.Rows(i).FindControl("chkMarcado"), CheckBox)

            '    '    'recorrer todo el grid y mandarlo a ModificarEmpDoc si no esta a 1 en caso de marcar
            '    '    'y mandarlo como 0 a todos 
            '    '    If CheckBoxElim.Checked = True Then



            '    '        'principio bucle empresas
            '    '        Dim listaType As List(Of KaplanLib.ELL.Kaplan)

            '    '        listaType = oDocBLL.CargarListaEmpresas(PageBase.plantaAdmin)

            '    '        If (listaType.Count > 0) Then
            '    '            For m = 0 To listaType.Count - 1


            '    '                Dim listaDoc As List(Of KaplanLib.ELL.Documentos)

            '    '                listaDoc = oDocumentosBLL.CargarDocumentos(listaType2(i).Id, PageBase.plantaAdmin)

            '    '                Dim tipo As New KaplanLib.ELL.EmpresasDoc With {.Planta = PageBase.plantaAdmin, .codemp = listaType(m).Id, .coddoc = listaType2(i).Id, .periodicidad = listaDoc(0).Periodo, .tipodoc = 0}

            '    '                If oDocumentosBLL.CargarListaEmpDoc(PageBase.plantaAdmin, listaType(m).Id, listaType2(i).Id).Count > 0 Then
            '    '                    'esta asignada por lo que no hago nada
            '    '                Else
            '    '                    oDocumentosBLL.ModificarEmpDoc(tipo, 0, Session("Ticket").nombreusuario)
            '    '                End If

            '    '            Next
            '    '        End If

            '    '    End If

            '    'Next

            'Else

            '    '    Dim j As Integer
            '    For i = 0 To gvType2.Rows.Count - 1

            '        Dim CheckBoxElim As CheckBox = CType(gvType2.Rows(i).FindControl("chkMarcado"), CheckBox)

            '        'recorrer todo el grid y mandarlo a ModificarEmpDoc si no esta a 1 en caso de marcar
            '        'y mandarlo como 0 a todos 
            '        If CheckBoxElim.Checked = 0 Then


            '            'leer si existe el registro en adok_emd
            '            'listaType2(i).Asignada = 0
            '            'If idEmpresa.Value > 0 Then
            '            '    If oDocumentosBLL.CargarListaEmpDoc(PageBase.plantaAdmin, idEmpresa.Value, listaType2(i).Id).Count > 0 Then
            '            '        'esta asignada por lo que lo borro
            '            '        Dim tipo As New KaplanLib.ELL.EmpresasDoc With {.Planta = PageBase.plantaAdmin, .codemp = idEmpresa.Value, .coddoc = listaType2(i).Id}
            '            '        oDocumentosBLL.ModificarEmpDoc(tipo, 1, Session("Ticket").nombreusuario)

            '            '        'si es de certificado hay que quitar el certificado
            '            '    Else
            '            '        'esta desasignada por lo que no hago nada
            '            '    End If
            '            'End If

            '        Else

            '            'leer si existe el registro en adok_emd
            '            'listaType2(i).Asignada = 0
            '            If idEmpresa.Value > 0 Then
            '                'If oDocumentosBLL.CargarListaEmpDoc(PageBase.plantaAdmin, idEmpresa.Value, listaType2(i).Id).Count > 0 Then
            '                '    'esta asignada por lo que no hago nada
            '                'Else
            '                '    'esta desasignada por lo que lo añado
            '                '    'hay que poner emd007 con tipo de periodicidad que lo da el documento
            '                '    Dim listaDoc As List(Of KaplanLib.ELL.Documentos)

            '                '    listaDoc = oDocumentosBLL.CargarDocumentos(listaType2(i).Id, PageBase.plantaAdmin)

            '                '    Dim tipo As New KaplanLib.ELL.EmpresasDoc With {.Planta = PageBase.plantaAdmin, .codemp = idEmpresa.Value, .coddoc = listaType2(i).Id, .periodicidad = listaDoc(0).Periodo}

            '                '    oDocumentosBLL.ModificarEmpDoc(tipo, 0, Session("Ticket").nombreusuario)
            '                'End If
            '            End If

            '        End If

            '    Next


            'End If


            'BindDataView2()

            'Master.MensajeInfo = ("Se han asignado los documentos a la empresa" & " " & txtNombre.Text & " " & "correctamente").ToUpper

            '    BindDataView2()
        Catch ex As Exception
            If chkMarcados.Value = 0 Then
                Master.MensajeError = "Un error ha ocurrido cuando se asignaban los documentos a la empresa" & " " & txtNombre.Text
            Else
                Master.MensajeError = "Un error ha ocurrido cuando se desasignaban los documentos a la empresa" & " " & txtNombre.Text
            End If
        End Try
    End Sub
    Protected Sub btnMarcarTodos_Click(sender As Object, e As EventArgs) Handles btnMarcarTodos.Click
        Try

            'Dim oDocBLL As New KaplanLib.BLL.DocumentosBLL
            ''Dim i As Int16
            'Dim j As Integer
            'Dim listaType2 As List(Of KaplanLib.ELL.Documentos)

            'listaType2 = oDocBLL.CargarListaEmpDocTot(PageBase.plantaAdmin)  'codemp
            'If (listaType2.Count > 0) Then

            '    'recorrer todo el grid y mandarlo a ModificarEmpDoc si no esta a 1 en caso de marcar
            '    'y mandarlo como 0 a todos 
            '    If chkMarcados.Value = 0 Then

            '        For j = 0 To gvType2.Rows.Count - 1
            '            Dim CheckBoxElim As CheckBox = CType(gvType2.Rows(j).FindControl("chkMarcado"), CheckBox)
            '            CheckBoxElim.Checked = True
            '        Next
            '        chkMarcados.Value = 1


            '    Else

            '        For j = 0 To gvType2.Rows.Count - 1
            '            Dim CheckBoxElim As CheckBox = CType(gvType2.Rows(j).FindControl("chkMarcado"), CheckBox)
            '            CheckBoxElim.Checked = False
            '        Next
            '        chkMarcados.Value = 0
            '    End If
            'End If


        Catch ex As Exception
            'If chkMarcados.Value = 0 Then
            '    Master.MensajeError = "Un error ha ocurrido cuando se asignaban los documentos a la empresa " & txtNombre.Text
            'Else
            '    Master.MensajeError = "Un error ha ocurrido cuando se desasignaban los documentos a la empresa " & txtNombre.Text
            'End If
        End Try
    End Sub


    Private Sub gvType2_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvType2.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim imgEstado As LinkButton = CType(e.Row.FindControl("lbCaducidad"), LinkButton)
            If imgEstado.CommandName = "Plantilla" Then

                imgEstado.Attributes.Add("onClick", "return false;")
            End If

        End If
    End Sub

    Protected Sub btnVolver_click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Try

            Response.Redirect("~/Operaciones.aspx?idRef=" & hfReferencia.Value & "&idComp=" & hfComponente.Value, False)

        Catch ex As Exception
            Master.MensajeError = ItzultzaileWeb.Itzuli("Error")
        End Try



    End Sub
End Class