Partial Public Class SelectorOperacion
    Inherits System.Web.UI.UserControl

#Region "Propiedades"

    ''' <summary>
    ''' Codigo de operacion en activo. Guardada en session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CodigoOperacion() As String
        Get
            If (Session("CodOperacion") Is Nothing) Then
                Return String.Empty
            Else
                Return Session("CodOperacion").ToString
            End If
        End Get
        Set(ByVal value As String)
            Session("CodOperacion") = value.Trim
        End Set
    End Property

    ''' <summary>
    ''' Descripcion del codigo de operacion en activo. Guardada en session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Descripcion() As String
        Get
            If (Session("DescripcionOpe") Is Nothing) Then
                Return String.Empty
            Else
                Return Session("DescripcionOpe").ToString
            End If
        End Get
        Set(ByVal value As String)
            Session("DescripcionOpe") = value.Trim
            lblDescripcion.Text = Session("DescripcionOpe")
        End Set
    End Property

    ''' <summary>
    ''' OperacionGeneral del codigo de operacion en activo. Guardada en session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OperacionGeneral() As String
        Get
            If (Session("OperacionGeneral") Is Nothing) Then
                Return String.Empty
            Else
                Return Session("OperacionGeneral").ToString
            End If
        End Get
        Set(ByVal value As String)
            Session("OperacionGeneral") = value.Trim
        End Set
    End Property

    ''' <summary>
    ''' Id seccion de la operacion
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdSeccion() As Integer
        Get
            If (Session("IdSeccionOpe") Is Nothing) Then
                Return Integer.MinValue
            Else
                Return CInt(Session("IdSeccionOpe"))
            End If
        End Get
        Set(ByVal value As Integer)
            Session("IdSeccionOpe") = value
        End Set
    End Property

    ''' <summary>
    ''' Indica si el codigo de operacion activa es valida. Guardada en session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property EsValido() As Boolean
        Get
            If (Session("EsValidoOpe") Is Nothing) Then
                Return False
            Else
                Return CType(Session("EsValidoOpe"), Boolean)
            End If
        End Get
        Set(ByVal value As Boolean)
            Session("EsValidoOpe") = value
        End Set
    End Property

    ''' <summary>
    ''' Para acceder al cuadro de texto de la operacion
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Operacion() As String
        Get
            Return txtOperacion.Text
        End Get
        Set(ByVal value As String)
            txtOperacion.Text = value.Trim
        End Set
    End Property

    ''' <summary>
    ''' Referencia seleccionada en la aplicacion
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Referencia() As String
        Get
            If (Session("IdReferencia") Is Nothing) Then
                Return String.Empty
            Else
                Return Session("IdReferencia").ToString
            End If
        End Get
    End Property

    ''' <summary>
    ''' Se produce al comprobar que la operacion no existe
    ''' </summary>
    ''' <remarks></remarks>
    Public Event OperacionNoExistente(ex As Exception)

    ''' <summary>
    ''' Evento que se produce al seleccionar una operacion (Al dar al boton aceptar o al salir)
    ''' </summary>
    ''' <remarks></remarks>
    Public Event OperacionSeleccionada()

    ''' <summary>
    ''' Inicializa el control
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub inicializar()
        Operacion = String.Empty
        CodigoOperacion = String.Empty
        Descripcion = String.Empty
        OperacionGeneral = String.Empty
        IdSeccion = Integer.MinValue
        EsValido = False
    End Sub

#End Region

#Region "Eventos de Pagina"

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If (Session("CodOperacion") IsNot Nothing) Then Operacion = Session("CodOperacion").ToString
    End Sub

    ''' <summary>
    ''' Registra un script para que al salir de la caja de texto se compruebe la referencia y al final, se recargue la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txtOperacion.Attributes.Add("onkeypress", "comprobarEnterOp();")
        'txtOperacion.Attributes.Add("onblur", "comprobarOperacion();")
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "comprobar", "function comprobarOperacion(){" & Page.ClientScript.GetPostBackEventReference(Me.btnComprobar, Nothing) & "}", True)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "comprobarEnter", "function comprobarEnterOp() {if (event.keyCode == 13) {comprobarOperacion();}}", True)
        ace_txtOperacion.OnClientItemSelected = "comprobarOperacion"
        txtOperacion.Text = Operacion
        lblDescripcion.Text = Descripcion
    End Sub

    ''' <summary>
    ''' Carga la operacion si es correcta y pertenece a la referencia especificada
    ''' </summary>
    ''' <param name="ref"></param>
    ''' <param name="cod"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CargarOperacion(ByVal ref As String, ByVal cod As String) As Boolean
        Try
            CodigoOperacion = cod
            If (CodigoOperacion <> String.Empty) Then
                Dim linqComp As New KaPlanLib.BLL.LinqComponent
                'Dim regOperacion = linqComp.consultarCodigoOperacion(ref, cod)
                Dim regOperacion = consultarCodigoOperacion(cod)

                If (regOperacion IsNot Nothing) Then
                    lblDescripcion.Text = regOperacion.OPERACION_GENERAL & " - " & regOperacion.OPERACION_TIPO
                    Descripcion = lblDescripcion.Text
                    OperacionGeneral = regOperacion.OPERACION_GENERAL
                    IdSeccion = regOperacion.ID_SECCION
                    EsValido = True
                Else
                    lblDescripcion.Text = String.Empty
                    Descripcion = String.Empty
                    OperacionGeneral = String.Empty
                    'IdSeccion = String.Empty
                    IdSeccion = Nothing
                    EsValido = False
                End If
            Else  'Se deja en blanco pero hay que lanzar un evento de operacion no existente
                lblDescripcion.Text = String.Empty
                Descripcion = String.Empty
                OperacionGeneral = String.Empty
                'IdSeccion = String.Empty
                IdSeccion = Nothing
                EsValido = False
            End If

            Return EsValido
        Catch ex As ApplicationException
            Throw ex
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        '-------------------------------------------------------
        'ItzultzaileWeb.Itzuli(lblCodOperacion)
        'ItzultzaileWeb.Itzuli(lblCod)
        'ItzultzaileWeb.Itzuli(btnBuscar)
        'ItzultzaileWeb.Itzuli(btnComprobar)
        'ItzultzaileWeb.Itzuli(imgCerrar)
        'ItzultzaileWeb.Itzuli(gv_Operaciones)
        '-------------------------------------------------------
    End Sub
#End Region

#Region "Eventos de Objetos"
    Private Sub ace_txtOperacion_PreRender(sender As Object, e As EventArgs) Handles ace_txtOperacion.PreRender
        Dim obj As AjaxControlToolkit.AutoCompleteExtender = sender
        Dim sPlanta As String() = Session("Planta")
        obj.ContextKey = sPlanta(2) & "|" & Session("IdReferencia")
        obj.UseContextKey = True
    End Sub
#End Region

#Region "GridView Articulos"
    Private Sub gv_Operaciones_Init(sender As Object, e As System.EventArgs) Handles gv_Operaciones.Init
        Dim Tabla As GridView = sender
        Tabla.CrearBotones()
    End Sub
    Private Sub gv_Operaciones_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_Operaciones.RowDataBound
        Dim Fila As GridViewRow = e.Row
        Fila.CrearAccionesFila()
    End Sub
    ''' <summary>
    ''' Pagina el listado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gv_Operaciones_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Operaciones.PageIndexChanging
        gv_Operaciones.PageIndex = e.NewPageIndex
        consultarOperaciones()
    End Sub
    ''' <summary>
    ''' Evento surgido al seleccionar los artículos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub gvOperaciones_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles gv_Operaciones.SelectedIndexChanged
        Dim Tabla As GridView = sender
        ViewState("CodOpeSel") = Tabla.SelectedDataKey.Item("COD_OPERACION").ToString.Trim
        ViewState("DescOpeSel") = Tabla.SelectedDataKey.Item("OPERACION_TIPO")
        ViewState("OpeGenSel") = Tabla.SelectedDataKey.Item("OPERACION_GENERAL")
        ViewState("IdSeccOpeSel") = Tabla.SelectedDataKey.Item("IDSECCION")

        ComprobarOperacion(New Button With {.CommandName = "A"}, Nothing)
        upSelector.Update()
    End Sub
#End Region

#Region "Botones"
    ''' <summary>
    ''' Boton oculto que comprobará que el codigo de operacion sea correcta en dos situaciones:
    '''    -Al perder el foco el texto del codigo 
    '''    -Al clickear el boton aceptar despues de haber seleccionado una operacion
    ''' </summary>    
    ''' <remarks></remarks>
    Protected Sub ComprobarOperacion(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim myButton As Button = CType(sender, Button)
            If (myButton.CommandName = "A") Then  'Se ha pulsado el boton aceptar
                If (ViewState("CodOpeSel") IsNot Nothing AndAlso ViewState("CodOpeSel") <> String.Empty) Then
                    CodigoOperacion = ViewState("CodOpeSel")
                    Descripcion = ViewState("DescOpeSel")
                    OperacionGeneral = ViewState("OpeGenSel")
                    IdSeccion = ViewState("IdSeccOpeSel")
                    EsValido = True

                    txtOperacion.Text = CodigoOperacion
                    lblDescripcion.Text = OperacionGeneral & " - " & Descripcion

                    RaiseEvent OperacionSeleccionada()
                End If
            Else  ' Se ha lanzado este evento, al perder el foco del cuadro de texto del codigo de operacion
                CodigoOperacion = txtOperacion.Text
                If (txtOperacion.Text <> String.Empty) Then
                    Dim linqComp As New KaPlanLib.BLL.LinqComponent
                    Dim log As log4net.ILog = log4net.LogManager.GetLogger("root.Ktrol")
                    'Dim regOperacion = linqComp.consultarCodigoOperacion(Referencia, CodigoOperacion)
                    Dim regOperacion = consultarCodigoOperacion(CodigoOperacion)
                    If (regOperacion IsNot Nothing) Then
                        lblDescripcion.Text = regOperacion.OPERACION_GENERAL & " - " & regOperacion.OPERACION_TIPO
                        Descripcion = lblDescripcion.Text
                        OperacionGeneral = regOperacion.OPERACION_GENERAL
                        IdSeccion = regOperacion.ID_SECCION
                        EsValido = True
                        RaiseEvent OperacionSeleccionada()
                    Else
                        lblDescripcion.Text = String.Empty
                        Descripcion = String.Empty
                        OperacionGeneral = String.Empty
                        IdSeccion = Nothing
                        EsValido = False
                        RaiseEvent OperacionNoExistente(Nothing)
                    End If
                Else  'Se deja en blanco pero hay que lanzar un evento de operacion no existente
                    lblDescripcion.Text = String.Empty
                    Descripcion = String.Empty
                    OperacionGeneral = String.Empty
                    IdSeccion = Nothing
                    EsValido = False
                    RaiseEvent OperacionNoExistente(Nothing)
                End If
            End If

            'Se limpian las variables del viewstate de Seleccion de nuevos codigos de operaciones para que entre distintas iteraciones, no de problemas
            ViewState("CodOpeSel") = Nothing
            ViewState("DescOpeSel") = Nothing
            ViewState("OpeGenSel") = Nothing
            ViewState("IdSeccOpeSel") = Nothing
        Catch ex As ApplicationException
            lblDescripcion.Text = String.Empty
            RaiseEvent OperacionNoExistente(ex)
        Catch ex As Exception
            'Para indicar que no existe            
            lblDescripcion.Text = String.Empty
            RaiseEvent OperacionNoExistente(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Abre un pop up con las operaciones de la referencia en uso
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnBuscar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        consultarOperaciones()
        mpe_SelectorOp.Show()
    End Sub

    ''' <summary>
    ''' Carga las operaciones de la referencia en uso
    ''' </summary>
    Private Sub consultarOperaciones()
        Try
            Dim linqComp As New KaPlanLib.BLL.LinqComponent
            gv_Operaciones.DataSource = linqComp.consultarListadoCodigosOperacion(String.Empty, GridViewSortExpresion, GridViewSortDirection)
            gv_Operaciones.Ordenar(GridViewSortExpresion, GridViewSortDirection)
            gv_Operaciones.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region "Orden"
    ''' <summary>
    ''' Evento para ordenar las columnas del resultado de la busqueda
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
	Private Sub gv_Operaciones_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gv_Operaciones.Sorting
		If (GridViewSortDirection = SortDirection.Ascending) Then
			GridViewSortDirection = SortDirection.Descending
		Else
			GridViewSortDirection = SortDirection.Ascending
		End If

		If (GridViewSortExpresion Is Nothing) Then
			GridViewSortExpresion = String.Empty
		Else
			GridViewSortExpresion = e.SortExpression
		End If

		consultarOperaciones()
	End Sub

	''' <summary>
	''' Devuelve el tipo de ordenacion que se va a realizar
	''' </summary>
	''' <param name="sortDirection">Orden</param>
	''' <returns>String con el orden a realizar ("ASC o DESC")</returns>
	Private Function GetSortDirection(ByVal sortDirection As SortDirection) As String
		Dim newSortDirection As String = String.Empty
		Select Case sortDirection
			Case sortDirection.Ascending
				newSortDirection = "ASC"
			Case Else
				newSortDirection = "DESC"
		End Select

		Return newSortDirection
	End Function

	''' <summary>
	''' Obtiene el indice de una columna
	''' </summary>
	''' <param name="sortExp">Expresion de orden</param>
	''' <returns>Indice</returns>
	Private Function getColumnIndex(ByVal sortExp As String) As Integer
		For index As Integer = 0 To gv_Operaciones.Columns.Count - 1
			If (gv_Operaciones.Columns(index).SortExpression = sortExp And gv_Operaciones.Columns(index).Visible) Then
				Return index
			End If
		Next index
		Return Integer.MinValue
	End Function

	''' <summary>
	''' Añade una imagen a la cabecera, indicando si el orden es ascendente o descendente
	''' </summary>
	''' <param name="headerRow"></param>
	''' <remarks></remarks>
	Private Sub AddSortImage(ByVal headerRow As GridViewRow)
        If (GridViewSortExpresion <> String.Empty) Then
            Dim sortExp As String = GridViewSortExpresion
            Dim idCol As Integer = getColumnIndex(sortExp)
            If (idCol <> -1) Then
                Dim sortImage As New Image()
                If (GridViewSortDirection = SortDirection.Ascending) Then
                    sortImage.ImageUrl = "~/App_Themes/Tema1/Imagenes/sortascending.gif"
                    sortImage.AlternateText = "ordenAscendente"
                Else
                    sortImage.ImageUrl = "~/App_Themes/Tema1/Imagenes/sortdescending.gif"
                    sortImage.AlternateText = "ordenDescendente"
                End If

                headerRow.Cells(idCol).Controls.Add(sortImage)
            End If
        End If
	End Sub

	''' <summary>
	''' Indica la direccion en la que hay que ordenar
	''' </summary>
	''' <value></value>
	''' <returns>Orden</returns>
	Public Property GridViewSortDirection() As SortDirection
		Get
			If (ViewState("sortDirection") Is Nothing) Then
				ViewState("sortDirection") = SortDirection.Ascending
			End If
			Return CType(ViewState("sortDirection"), SortDirection)
		End Get
		Set(ByVal value As SortDirection)
			ViewState("sortDirection") = value
		End Set
	End Property

	''' <summary>
	''' Indica la expresion de ordenacion
	''' </summary>
	''' <value></value>
	''' <returns>Expresion</returns>
	Public Property GridViewSortExpresion() As String
		Get
			If (ViewState("sortExpresion") Is Nothing) Then
                ViewState("sortExpresion") = "NUM_OP"
			End If
			Return ViewState("sortExpresion").ToString()
		End Get
		Set(ByVal value As String)
			ViewState("sortExpresion") = value
		End Set
	End Property
#End Region

    ''' <summary>
    ''' Al realizarse el onblur, llama a comprobar referencia
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnComprobar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnComprobar.Click
        ComprobarOperacion(sender, e)
    End Sub

#Region "Pruebas"

    ''' <summary>
    ''' Devuelve el tipo de operacion
    ''' </summary>
    ''' <param name="codOpe">Codigo de operacion</param>
    ''' <returns></returns>
    Function consultarCodigoOperacion(ByVal codOpe As String)
        Try
            Dim BBDD As New KaPlanLib.DAL.ELL
            '-----------------------------------------------------------------------------------------------------------------
            'Unificamos el metodo de busqueda para las operaciones por que no da el mismo resultado 
            'si se usa el selector de Operaciones o si se busca una operacion directamente en la caja de texto.
            '-----------------------------------------------------------------------------------------------------------------
            ' Dim lObjetos As Object = consultarListadoCodigosOperacion(ref)
            'Dim OperacionTipo As KaPlanLib.Registro.OPERACIONES_TIPO = Nothing

            'If lObjetos Is Nothing AndAlso lObjetos.Count = 0 Then
            '    Throw New ApplicationException("""" & ref & """ sin operaciones asignadas o """ & codOpe & """ no tiene maquina asignada.")
            'Else
            '    OperacionTipo = (From ot As Registro.OPERACIONES_TIPO In BBDD.OPERACIONES_TIPO Where ot.COD_OPERACION = codOpe Select ot).FirstOrDefault
            '    If OperacionTipo Is Nothing Then Throw New ApplicationException(codOpe & " - NO existente.")

            '    '--------------------------------------------------------------------------------------------------------------------------------
            '    'Nos recorremos los objetos de tipo anonimo para poder obtener el "COD_OPERACION" y la "OPERACIONES_TIPO" que le corresponde.
            '    '--------------------------------------------------------------------------------------------------------------------------------
            '    Dim lOperaciones As New List(Of Registro.OPERACIONES_TIPO)
            '    For Each obj As Object In lObjetos
            '        Dim OBJ_Type As Type = obj.GetType
            '        Dim ValorPropiedad As String = ""
            '        Dim Propiedades As List(Of System.Reflection.PropertyInfo) = OBJ_Type.GetProperties.ToList
            '        Dim COD_OPERACION As System.Reflection.PropertyInfo = Propiedades.Find(Function(pi As System.Reflection.PropertyInfo) StrComp(pi.Name, "COD_OPERACION", CompareMethod.Text) = 0)

            '        If COD_OPERACION.GetValue(obj, Nothing) IsNot Nothing Then
            '            ValorPropiedad = COD_OPERACION.GetValue(obj, Nothing).ToString.Trim
            '            '------------------------------------------------------------------------------------------
            '            'Dim OperacionTipoB As Registro.OPERACIONES_TIPO = consultarCodigoOperacion(ValorPropiedad)
            '            '------------------------------------------------------------------------------------------
            '            'FROGA: 2013-05-21: Si no da error dejar esto.
            '            '------------------------------------------------------------------------------------------
            '            Dim OperacionTipoB As Registro.OPERACIONES_TIPO = _
            '                (From Opt As Registro.OPERACIONES_TIPO In BBDD.OPERACIONES_TIPO Where Opt.COD_OPERACION = ValorPropiedad Select Opt).SingleOrDefault
            '            '------------------------------------------------------------------------------------------
            '            If OperacionTipoB IsNot Nothing Then lOperaciones.Add(OperacionTipoB)
            '        End If
            '    Next

            '    OperacionTipo = (From ot As Registro.OPERACIONES_TIPO In lOperaciones Where ot.COD_OPERACION.Trim = codOpe Select ot).FirstOrDefault
            '    If OperacionTipo Is Nothing Then Throw New ApplicationException(codOpe & " - NO tiene maquina asignada.")
            '--------------------------------------------------------------------------------------------------------------------------------
            'End If

            Dim OperacionTipo As KaPlanLib.Registro.OPERACIONES_TIPO = _
                            (From Opt As KaPlanLib.Registro.OPERACIONES_TIPO In BBDD.OPERACIONES_TIPO Where Opt.COD_OPERACION = codOpe Select Opt).SingleOrDefault

            Return OperacionTipo
            '-----------------------------------------------------------------------------------------------------------------
        Catch ex As ApplicationException
            Throw ex
        Catch ex As Exception
            Throw New Exception("error", ex)
        End Try
    End Function

#End Region
End Class