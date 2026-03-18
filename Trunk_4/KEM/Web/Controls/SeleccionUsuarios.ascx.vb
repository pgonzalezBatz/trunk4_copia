Imports SABLib
Imports KEM.PageBase

Partial Public Class SeleccionUsuarios
    Inherits System.Web.UI.UserControl

#Region "Property"

	''' <summary>
	''' Indicamos el tipo de filtro. 0-Todos, 1-Usuarios de Batz, 2-Trabajadores de batz sin usuario
	''' </summary>
	Private _trabajador As eTrabajador = eTrabajador.Todos
	''' <summary>
	''' Email del usuario
	''' </summary>	
	Private _email As Nullable(Of Boolean) = Nothing
	''' <summary>
	''' Indicamos si queremos que nos muestre los usuarios Vigentes de SAB (Sin fecha de baja o que la fecha de baja mayor o igual a hoy).
	''' </summary>	
	Private _vigentes As Boolean = False
	''' <summary>
	''' Identificamos la empresa por la que se quiere filtrar. IdEmpresa = 1 (Batz).
	''' </summary>	
	Private _idEmpresa As Integer
	''' <summary>
	''' Indicamos si los usuarios a buscar tienen Codigo de Persona asignado.
	''' 0-Todos (Por defecto), 1- codPersona NO NULL, 2-codPersona = NULL
	''' </summary>
	Private _codPersona As eCodPersona = eCodPersona.Todos
	''' <summary>
	''' Tipo de Seleccion para el control.
	''' </summary>
	Private _tipoSeleccion As System.Web.UI.WebControls.ListSelectionMode = ListSelectionMode.Multiple
	''' <summary>
	''' Evento que se produce cuando se busca, se pasa a la izquierda y se pasa a la derecha
	''' </summary>
	Public Event EventoSelUsuario()

	''' <summary>
	''' Indicamos el tipo de filtro 
	''' </summary>
	''' <value>0-Todos, 1-Usuarios de Batz, 2-Trabajadores de batz sin usuario</value>
	Public Property Trabajador() As eTrabajador
		Get
			Return _trabajador
		End Get
		Set(ByVal value As eTrabajador)
			_trabajador = value
		End Set
	End Property

	''' <summary>
	''' Email del usuario
	''' </summary>
	''' <value></value>
	''' <returns></returns>	
	Public Property Email() As Nullable(Of Boolean)
		Get
			Return _email
		End Get
		Set(ByVal value As Nullable(Of Boolean))
			_trabajador = value
		End Set
	End Property

	''' <summary>
	''' Indicamos si queremos que nos muestre los usuarios Vigentes de SAB (Sin fecha de baja o que la fecha de baja mayor o igual a hoy).
	''' </summary>
	''' <value></value>
	''' <returns></returns>	
	Public Property Vigentes() As Boolean
		Get
			Return _vigentes
		End Get
		Set(ByVal value As Boolean)
			_vigentes = value
		End Set
	End Property

	''' <summary>
	''' Identificamos la empresa por la que se quiere filtrar. IdEmpresa = 1 (Batz).
	''' </summary>	
	Public Property idEmpresa() As Integer
		Get
			Return _idEmpresa
		End Get
		Set(ByVal value As Integer)
			_idEmpresa = value
		End Set
	End Property

	''' <summary>
	''' Indicamos si los usuarios a buscar tienen Codigo de Persona asignado.
	''' 0-Todos (Por defecto), 1- codPersona NO NULL, 2-codPersona = NULL
	''' </summary>	
	Public Property CodPersona() As eCodPersona
		Get
			Return _codPersona
		End Get
		Set(ByVal value As eCodPersona)
			_codPersona = value
		End Set
	End Property

	''' <summary>
	''' Tipo de Seleccion para el control.
	''' </summary>
	''' <value></value>
	''' <returns></returns>	
	Public Property TipoSeleccion() As System.Web.UI.WebControls.ListSelectionMode
		Get
			Return _tipoSeleccion
		End Get
		Set(ByVal value As System.Web.UI.WebControls.ListSelectionMode)
			_tipoSeleccion = value
		End Set
	End Property

	''' <summary>
	''' Id de la planta de la que se mostraran los usuarios
	''' Esta variable se guarda en el viewstate porque no se puede asignar en tiempo de Diseño. Solo se puede hacer en tiempo de ejecucion y por tanto con los postback, se inicializa la variable idPlanta
	''' </summary>	
	Public Property IdPlanta() As Integer
		Get
			Return ViewState("SelUsuIdPlanta")
		End Get
		Set(ByVal value As Integer)
			ViewState("SelUsuIdPlanta") = value
		End Set
	End Property

	''' <summary>
	''' Lista de responsables que se pueden seleccionar
	''' </summary>
	''' <value></value>
	''' <returns></returns>	
	Public ReadOnly Property ListaResponsablesSeleccionables() As ListBox
		Get
			Return lsbFiltroUsuarios
		End Get
	End Property

	''' <summary>
	''' Lista de los responsables elegidos
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	Public ReadOnly Property ListaResponsablesElegidos() As ListBox
		Get
			Return lsbUsuariosElegidos
		End Get
    End Property

    Private pg As New PageBase

#End Region

#Region "Enumeraciones"

	Enum eCodPersona
		Todos = 0
		NoNulo = 1
		Nulo = 2
    End Enum

	Enum eTrabajador
		Todos = 0
		UsuariosBatz = 1
		TrabajadoresBatz = 2
	End Enum

#End Region

#Region "Page_Load"

	''' <summary>
	''' Se asigna un tooltip compuesto a un textbox
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>	
	Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        txtResponsable.ToolTip = pg.itzultzaileWeb.Itzuli("textoabuscar") & "." & pg.itzultzaileWeb.Itzuli("Se puede buscar por nombre, apellidos, nombre de usuario, id de sab o numero de trabajador")
    End Sub

    ''' <summary>
    ''' Traduccion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        pg.itzultzaileWeb.Itzuli(labelUsuario) : pg.itzultzaileWeb.Itzuli(btnBilatu)
        pg.itzultzaileWeb.Itzuli(btnDcha) : pg.itzultzaileWeb.Itzuli(btnIzqda)
    End Sub

	''' <summary>
	''' Funcion para filtrar los usuarios que se pueden seleccionar
	''' </summary>  
	Private Sub BuscarUsuarios()
		Try
			Dim oUser As New SABLib.ELL.Usuario
			Dim oUserFunc As New BLL.UsuariosComponent
			Dim listaUsuarios As New List(Of ELL.Usuario)
			Dim bConIdDirectorioActivo, bConCodPersona As Nullable(Of Boolean)
			Dim myIdEmpresa As Integer = Integer.MinValue
			If (idEmpresa > 0) Then myIdEmpresa = idEmpresa
			bConIdDirectorioActivo = Nothing : bConCodPersona = Nothing
			If Trabajador = eTrabajador.UsuariosBatz Then 'Usuarios de Batz
				bConIdDirectorioActivo = True
			ElseIf Trabajador = eTrabajador.TrabajadoresBatz Then 'Trabajadores de Batz
				bConIdDirectorioActivo = False
			End If
			If (CodPersona = eCodPersona.NoNulo) Then
				bConCodPersona = True
			ElseIf (CodPersona = eCodPersona.Nulo) Then
				bConCodPersona = False
			End If
			listaUsuarios = oUserFunc.GetUsuariosBusquedaSAB(txtResponsable.Text.Trim, bConIdDirectorioActivo, Email, bConCodPersona, Vigentes, myIdEmpresa, IdPlanta)
			If Not (listaUsuarios Is Nothing) Then
				CargarFiltroUsuarios(listaUsuarios)
			End If
        Catch batzEx As BatzException
            Throw batzEx
		Catch ex As Exception
            Dim be As New BatzException("Error al cargar los registros", ex)
			lblMensaje.Text = be.Termino
			upMensaje.Update()
		End Try
	End Sub

#End Region

#Region "Cargar Lista"

    ''' <summary>
    ''' Carga el filtro de usuarios con el nombre completo, sino con el usuario y sino con el email
    ''' </summary>
    ''' <param name="listaUser">Lista de usuarios</param>    
    Private Sub CargarFiltroUsuarios(ByVal listaUser As List(Of ELL.Usuario))
        Dim item As String
        lsbFiltroUsuarios.Items.Clear()
        For Each oUser As ELL.Usuario In listaUser
            item = oUser.NombreCompleto
            If (item = String.Empty) Then
                If (oUser.Nombre <> String.Empty) Then
                    item = oUser.Nombre
                Else
                    If (oUser.Email <> String.Empty) Then
                        item = oUser.Email
                    End If
                End If
            End If
            If (item.Trim <> String.Empty) Then lsbFiltroUsuarios.Items.Add(New ListItem(item, oUser.Id))
        Next
	End Sub

#End Region

#Region "Botones izquierda y derecha"

	''' <summary>
	''' Se selecciona el usuario y se manda a la lista de la derecha
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>	
    Private Sub btnDcha_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDcha.Click
        Try
            Dim lst As New ListItemCollection
            'Creamos una lista con los elementos que queremos pasar.
            For Each item As Integer In lsbFiltroUsuarios.GetSelectedIndices()
                Dim itemSeleccionado As New ListItem
                itemSeleccionado = lsbFiltroUsuarios.Items.Item(item)
                lst.Add(itemSeleccionado)
            Next
            'Pasamos los elementos de la lista que hemos seleccionado.
            If lst.Count > 0 Then
                'Si la seleccion es Simple (Single) pasamos los Elegidos a la lista de busqueda.
                If Me.TipoSeleccion = ListSelectionMode.Single Then
                    Dim licUsuariosElegidos As New ListItemCollection
                    For Each itemElegidos As ListItem In lsbUsuariosElegidos.Items
                        licUsuariosElegidos.Add(itemElegidos)
                    Next
                    For Each itemSeleccionado As ListItem In licUsuariosElegidos
                        pasarIzquierda(itemSeleccionado)
                    Next
                End If
                For Each itemSeleccionado As ListItem In lst
                    pasarDerecha(itemSeleccionado)
                Next
            End If
        Catch ex As Exception
			Dim be As New BatzException("ErrorAñadir", ex)
            lblMensaje.Text = be.Termino
            upMensaje.Update()
		End Try
		RaiseEvent EventoSelUsuario()
    End Sub

	''' <summary>
	''' Se selecciona el usuario y se m anda a la lista de la izquierda
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>	
    Private Sub btnIzqda_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnIzqda.Click
        Try
            Dim lst As New ListItemCollection
            'Creamos una lista con los elementos que queremos pasar.
            For Each item As Integer In lsbUsuariosElegidos.GetSelectedIndices()
                Dim itemSeleccionado As New ListItem
                itemSeleccionado = lsbUsuariosElegidos.Items.Item(item)
                lst.Add(itemSeleccionado)
            Next
            'Pasamos los elementos de la lista que hemos seleccionado.
            If lst IsNot Nothing Then
                For Each itemSeleccionado As ListItem In lst
                    pasarIzquierda(itemSeleccionado)
                Next
            End If
        Catch ex As Exception
			Dim be As New BatzException("ErrorQuitar", ex)
            lblMensaje.Text = be.Termino
            upMensaje.Update()
		End Try
		RaiseEvent EventoSelUsuario()
    End Sub

	''' <summary>
	''' Pasa el item a la lista de la derecha
	''' </summary>
	''' <param name="item"></param>	
    Public Sub pasarDerecha(ByVal item As ListItem)
        lsbUsuariosElegidos.Items.Add(item)
        lsbUsuariosElegidos.SelectedIndex = -1  'Para que no de error por solo poder estar seleccionado un elemento
        lsbFiltroUsuarios.Items.Remove(item)
    End Sub

	''' <summary>
	''' Pasa el item a la lista de la izquierda
	''' </summary>
	''' <param name="item"></param>	
    Public Sub pasarIzquierda(ByVal item As ListItem)
        lsbFiltroUsuarios.Items.Add(item)
        lsbFiltroUsuarios.SelectedIndex = -1  'Para que no de error por solo poder estar seleccionado un elemento
        lsbUsuariosElegidos.Items.Remove(item)
    End Sub

#End Region

#Region "Inicializar"

	''' <summary>
	''' Inicializa los controles
	''' </summary>	
	Public Sub Inicializar()
		txtResponsable.Text = String.Empty
        lsbFiltroUsuarios.Items.Clear() : lsbUsuariosElegidos.Items.Clear()
	End Sub

#End Region

#Region "Consultar"

	''' <summary>
	''' Se buscan los usuarios que cumplan la condicion del nombre
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	Protected Sub btnBilatu_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBilatu.Click
		BuscarUsuarios()
		RaiseEvent EventoSelUsuario()
	End Sub

#End Region

#Region "Ordenacion de Listas"

	''' <summary>
	''' Se ordenan las listas de los usuarios elegidos
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>	
	Protected Sub lsbUsuariosElegidos_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles lsbUsuariosElegidos.PreRender
		SortListControl(lsbUsuariosElegidos, True)
	End Sub

	''' <summary>
	''' Se ordenan las listas de los usuarios seleccionables
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>	
	Protected Sub lsbFiltroUsuarios_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles lsbFiltroUsuarios.PreRender
		SortListControl(lsbFiltroUsuarios, True)
	End Sub

	''' <summary>
	''' Metodo de ordenacion
	''' </summary>
	''' <param name="SourceList"></param>
	''' <param name="Ascending"></param>
	''' <remarks></remarks>
	Sub SortListControl(ByRef SourceList As ListControl, ByVal Ascending As Boolean)
		'sorts listcontrols
		Dim array1 As New ArrayList
		Dim loop1 As Integer
		For loop1 = 0 To SourceList.Items.Count - 1
			array1.Add(SourceList.Items(loop1))
		Next
		Dim myComparer = New SortListArray(Ascending)
		array1.Sort(myComparer)
		SourceList.Items.Clear()
		For loop1 = 0 To array1.Count - 1
			SourceList.Items.Add(array1(loop1))
		Next
	End Sub

	''' <summary>
	''' Clase para ordenar los items del listbox
	''' </summary>	
	Public Class SortListArray
		Implements IComparer
		Private _Ascending As Boolean = True
		Public Sub New()
		End Sub
		Public Sub New(ByVal Ascending As Boolean)
			_Ascending = Ascending
		End Sub
		Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
			If _Ascending Then
				Return New CaseInsensitiveComparer().Compare(x.ToString, y.ToString)
			Else
				Return New CaseInsensitiveComparer().Compare(y.ToString, x.ToString)
			End If
		End Function
	End Class

#End Region

End Class