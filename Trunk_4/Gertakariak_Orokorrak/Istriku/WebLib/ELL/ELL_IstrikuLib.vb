Imports System.Web.UI.WebControls

''' <remarks>ELL</remarks>
Public MustInherit Class gtkIstrikuELL
	Inherits gtkIstrikuDAL

	''' <summary>
	''' Identificador del tipo de incidencia (5 = gtkIstriku).
	''' </summary>
	Private Const _IdTipoIncidencia As Integer = 5
	''' <summary>
	''' Identificador del suceso.
	''' </summary>
	Private _ID As Nullable(Of Integer)
	''' <summary>
	''' Descripcion del suceso.
	''' </summary>
	Private _Descripcion As String
	''' <summary>
	''' Fecha cuando se da por cerrado el suceso.
	''' </summary>
	''' <remarks>Aprovecha los segundos de la fecha para indicar en que hora de trabajo se produjo el suceso.</remarks>
	Private _FechaSuceso As Date
	''' <summary>
	''' Fecha y Hora cuando se produjo el suceso.
	''' </summary>
	''' <remarks></remarks>
	Private _FechaCierre As Nullable(Of Date)
	''' <summary>
	''' Hora de trabajo en la que se produjo el succeso.
	''' </summary>
	Private _HoraTrabajo As Integer = 0
	''' <summary>
	''' Usuario que ha creado el suceso.
	''' </summary>
	Private _usrCreador As Sablib.ELL.Usuario
	''' <summary>
	''' Identificador de la procedencia del Suceso. (4-Accidente, 5-Incidente).
	''' </summary>
	Private _ProcedenciaNC As Nullable(Of Integer)
	''' <summary>
	''' Listado de Usuarios afectados por el suceso.
	''' </summary>
	''' <remarks></remarks>
	Private _Afectados2 As List(Of gtkAfectado2)
	''' <summary>
	''' Listado de Usuarios afectados por el suceso.
	''' </summary>
	''' <remarks></remarks>
	Private _Afectados As List(Of gtkAfectado)

	''' <summary>
	''' Identificador del suceso.
	''' Propiedad de busqueda.
	''' </summary>
	''' <remarks></remarks>
	''' <value></value>
	Public Property ID() As Nullable(Of Integer)
		Get
			Return _ID
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_ID = value
		End Set
	End Property
	''' <summary>
	''' Fecha y Hora cuando se produjo el suceso.
	''' </summary>
	''' <remarks>Aprovecha los segundos de la fecha para indicar en que hora de trabajo se produjo el suceso.</remarks>
	Public Property FechaSuceso() As Date
		Get
			Return _FechaSuceso.AddSeconds(_HoraTrabajo)
		End Get
		Set(ByVal value As DateTime)
			_FechaSuceso = value
		End Set
	End Property
	''' <summary>
	''' Fecha cuando se da por cerrado el suceso.
	''' </summary>
	''' <remarks></remarks>
	Public Property FechaCierre() As Nullable(Of Date)
		Get
			Return _FechaCierre
		End Get
		Set(ByVal value As Nullable(Of Date))
			_FechaCierre = value
		End Set
	End Property
	''' <summary>
	''' Descripcion del suceso.
	''' </summary>
	Public Property Descripcion() As String
		Get
			Return _Descripcion
		End Get
		Set(ByVal value As String)
			_Descripcion = value
		End Set
	End Property
	''' <summary>
	''' Identificador del tipo de incidencia (5 = gtkIstriku).
	''' </summary>
	Public ReadOnly Property IdTipoIncidencia() As Integer
		Get
			Return _IdTipoIncidencia
		End Get
	End Property
	''' <summary>
	''' Hora de trabajo en la que se produjo el succeso.
	''' </summary>
	Public Property HoraTrabajo() As Integer
		Get
			Return _HoraTrabajo
		End Get
		Set(ByVal value As Integer)
			_HoraTrabajo = value
		End Set
	End Property
	''' <summary>
	''' Usuario que ha creado el suceso.
	''' </summary>
	Public Overloads Property usrCreador() As Sablib.ELL.Usuario
		Get
			Return _usrCreador
		End Get
		Set(ByVal value As Sablib.ELL.Usuario)
			_usrCreador = value
		End Set
	End Property
	''' <summary>
	''' Usuario que ha creado el suceso.
	''' </summary>
	''' <param name="Id">Idebtificador del usuariom</param>
	''' <value></value>
	''' <returns>Devuelve un objeto SabLib.ELL.Usuario</returns>
	''' <remarks></remarks>
	Public Overloads ReadOnly Property usrCreador(ByVal Id As Integer) As Sablib.ELL.Usuario
		Get
			Dim fUsuario As New Sablib.BLL.UsuariosComponent
            Return fUsuario.GetUsuario(New Sablib.ELL.Usuario With {.Id = Id}, False)
		End Get
	End Property
	''' <summary>
	''' Identificador de la procedencia del Suceso. (4-Accidente, 5-Incidente).
	''' </summary>
	Public Property ProcedenciaNC() As Nullable(Of Integer)
		Get
			Return _ProcedenciaNC
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_ProcedenciaNC = value
		End Set
	End Property
	''' <summary>
	''' Listado de Usuarios afectados por el suceso.
	''' </summary>
	Private Property Afectados2 As System.Collections.Generic.List(Of IstrikuLib.gtkAfectado2)
		Get
			Dim ListaAfectados As New List(Of gtkAfectado2)
			If _Afectados2 Is Nothing OrElse _Afectados2.Count <= 0 Then
				Dim gtkafectado As New gtkAfectado2 With {.IdSuceso = Me.ID}
				_Afectados2 = gtkafectado.Cargar()
			End If
			Return _Afectados2
		End Get
		Set(ByVal value As System.Collections.Generic.List(Of IstrikuLib.gtkAfectado2))
			_Afectados2 = value
		End Set
	End Property

	Public Property Afectados As List(Of gtkAfectado)
		Get
			Dim ListaAfectados As New List(Of gtkAfectado)
			If _Afectados Is Nothing OrElse _Afectados.Count <= 0 Then
				Dim gtkafectado As New gtkAfectado With {.IdIncidencia = Me.ID}

				_Afectados = gtkafectado.Lista()

			End If
			Return _Afectados
		End Get
		Set(ByVal value As System.Collections.Generic.List(Of IstrikuLib.gtkAfectado))
			_Afectados = value
		End Set
	End Property

	Public Property gtkAfectado As List(Of IstrikuLib.gtkAfectado)
		Get
			Return _Afectados
		End Get
		Set(ByVal value As List(Of IstrikuLib.gtkAfectado))
			_Afectados = value
		End Set
	End Property

End Class

#Region "Enumeracion ProcedenciaNC"
''' <summary>
''' Procedencia del Suceso (Accidente/Incidente).
''' </summary>
Public Enum ProcedenciaNC
	''' <summary>
    ''' Procedencia del Suceso (Con lesion).
	''' </summary>
    Accidente = 4
    ''' <summary>
    ''' Procedencia del Suceso (Sin lesion).
    ''' </summary>
	Incidente = 5
End Enum
#End Region

Public Structure gtkGridView
	''' <summary>
	''' Identificador del Registro Seleccionado.
	''' </summary>
	Private _IdSeleccionado As Nullable(Of Integer)
	''' <summary>
	''' Nombre de la PROPIEDAD por la que se quiere ordenar los objetos.
	''' </summary>
	Private _CampoOrdenacion As String
	''' <summary>
	''' Direccion de Ordenacion para el Campo de Ordenacion (Nombre de la Propiedad).
	''' </summary>
	Private _DireccionOrdenacion As Nullable(Of System.ComponentModel.ListSortDirection)
	''' <summary>
	''' Indice de la página en curso.
	''' </summary>
	Private _Pagina As Nullable(Of Integer)

	''' <summary>
	''' Identificador del Registro Seleccionado.
	''' </summary>
	Public Property IdSeleccionado() As Nullable(Of Integer)
		Get
			Return _IdSeleccionado
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_IdSeleccionado = value
		End Set
	End Property
	''' <summary>
	''' Nombre de la PROPIEDAD por la que se quiere ordenar los objetos.
	''' </summary>
	Public Property CampoOrdenacion() As String
		Get
			Return _CampoOrdenacion
		End Get
		Set(ByVal value As String)
			_CampoOrdenacion = value
		End Set
	End Property
	''' <summary>
	''' Direccion de Ordenacion para el Campo de Ordenacion (Nombre de la Propiedad).
	''' </summary>
	Public Property DireccionOrdenacion() As Nullable(Of System.ComponentModel.ListSortDirection)
		Get
			Return _DireccionOrdenacion
		End Get
		Set(ByVal value As Nullable(Of System.ComponentModel.ListSortDirection))
			_DireccionOrdenacion = value
		End Set
	End Property

	''' <summary>
	''' Indice de la página en curso.
	''' </summary>
	Public Property Pagina As Nullable(Of Integer)
		Get
			Return _Pagina
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_Pagina = value
		End Set
	End Property

End Structure

''' <remarks>ELL</remarks>
<Obsolete("NO USAR")> Public MustInherit Class gtkAfectadoELL
    Inherits SabLib.ELL.Usuario
    Private _idDepartamento As EstadoParte
    ''' <summary>
    ''' Identificador del Suceos (Incidencia) al que pertenece el usuario.
    ''' Campo de BUSQUEDA.
    ''' </summary>
    Private _idSuceso As Nullable(Of Integer)
    ''' <summary>
    ''' Identificador único del Afectado en el Suceso (Incidencia).
    ''' Campo de BUSQUEDA (DETECCION.ID).
    ''' </summary>
    Private _EstadoParte As Nullable(Of Integer)
    Public Property EstadoParte As EstadoParte
        Get
            Return _idDepartamento
        End Get
        Set(ByVal value As EstadoParte)
            _idDepartamento = value
        End Set
    End Property
    ''' <summary>
    ''' Identificador del Suceos (Incidencia) al que pertenece el usuario.
    ''' Campo de BUSQUEDA.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdSuceso As Nullable(Of Integer)
        Get
            Return _idSuceso
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _idSuceso = value
        End Set
    End Property
    ''' <summary>
    ''' Identificador único del Afectado en el Suceso (Incidencia).
    ''' Campo de BUSQUEDA (DETECCION.ID).
    ''' </summary>
    Public Property IdAfectadoSuceso As Nullable(Of Integer)
        Get
            Return _EstadoParte
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _EstadoParte = value
        End Set
    End Property
End Class
Public Enum EstadoParte
	Pendiente = 0
	Aceptado = 1
	Denegado = -1
End Enum
Public Structure Filtro
	Private _ProcedenciaNC As Nullable(Of ProcedenciaNC)

	Public Property Procedencia As Nullable(Of ProcedenciaNC)
		Get
			Return _ProcedenciaNC
		End Get
		Set(ByVal value As Nullable(Of ProcedenciaNC))
			_ProcedenciaNC = value
		End Set
	End Property
End Structure