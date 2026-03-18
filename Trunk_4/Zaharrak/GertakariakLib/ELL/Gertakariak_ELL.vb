Public MustInherit Class Deteccion_ELL

	''' <summary>
	''' Identificador del Suceos (Incidencia) al que pertenece el usuario.
	''' Campo de BUSQUEDA.
	''' </summary>
	Private _idIncidencia As Nullable(Of Integer)
	''' <summary>
	''' Identificador único del Afectado en el Suceso (Incidencia).
	''' Campo de BUSQUEDA (DETECCION.ID).
	''' </summary>
	Private _id As Nullable(Of Integer)
	Private _idUsuario As Nullable(Of Integer)
	Private _idDepartamento As Nullable(Of Integer)

	''' <summary>
	''' Identificador del Suceso (Incidencia) al que pertenece el usuario.
	''' Campo de BUSQUEDA.
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Property IdIncidencia As Nullable(Of Integer)
		Get
			Return _idIncidencia
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_idIncidencia = value
		End Set
	End Property

	''' <summary>
	''' Identificador único del Afectado en el Suceso (Incidencia).
	''' Campo de BUSQUEDA (DETECCION.ID).
	''' </summary>
	Protected Property Id As Nullable(Of Integer)
		Get
			Return _id
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_id = value
		End Set
	End Property

	Public Property IdUsuario As Nullable(Of Integer)
		Get
			Return _idUsuario
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_idUsuario = value
		End Set
	End Property

	Public Property IdDepartamento As Nullable(Of Integer)
		Get
			Return _idDepartamento
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_idDepartamento = value
		End Set
	End Property

	Private _Usuario As SABLib.ELL.Usuario

	Public ReadOnly Property Usuario As SABLib.ELL.Usuario
		Get
			'------------------------------------------------------------------------
			'Dim fUsr As New Sablib.BLL.UsuariosComponent
			'Return fUsr.GetUsuario(New Sablib.ELL.Usuario With {.Id = IdUsuario})
			'------------------------------------------------------------------------
			'FROGA: Aceleracion de carga.
			'------------------------------------------------------------------------
			If _Usuario Is Nothing Then
				Dim fUsr As New SABLib.BLL.UsuariosComponent
				_Usuario = fUsr.GetUsuario(New SABLib.ELL.Usuario With {.Id = IdUsuario}, False)
			End If
			Return _Usuario
			'------------------------------------------------------------------------
		End Get
	End Property
End Class

Public MustInherit Class Gertakariak_ELL
	''' <summary>
	''' Identificador único del registro.
	''' Campo de BUSQUEDA (GERTAKARIAK.ID).
	''' </summary>
	Private _id As Nullable(Of Integer)
	Private _titulo As String
	Private _fechaApertura As Nullable(Of Date)
	Private _fechaCierre As Nullable(Of Date)
	Private _descripcionProblema As String
	''' <summary>
	''' Identificador del Tipo de Incidencia y Tipo de Documento:
	''' 1-Troqueleria (gtkTroqueleria),
	''' 2-Servicios Generales (gtkSerciviosGenerales),
	''' 3-Txokos (gtkTxokos),
	''' 4-Sugerencias (gtkSugerencias),
	''' 5-Seguridad Laboral-Sucesos (gtkIstriku),
	''' 6-Sistemas Mantenimiento (gtkMantenimientoSist).
	''' </summary>
	Private _idTipoIncidencia As Integer
	''' <summary>
	''' Identificador del usuario que a creado la No Conformidad (SAB.USUARIOS.ID).
	''' Campo de BUSQUEDA (GERTAKARIAK.IDCREADOR).
	''' </summary>
	Private _idCreador As Nullable(Of Integer)
	''' <summary>
	''' Identificador del "Activo" en Prisma (ASSET.ASSET).
	''' </summary>
	Private _idActivo As String

	''' <summary>
	''' Identificador único del registro.
	''' Campo de BUSQUEDA (GERTAKARIAK.ID).
	''' </summary>
	Protected Property Id As Nullable(Of Integer)
		Get
			Return _id
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_id = value
		End Set
	End Property

	Public Property Titulo As String
		Get
			Return _titulo
		End Get
		Set(ByVal value As String)
			_titulo = value
		End Set
	End Property

	Public Property FechaApertura As Nullable(Of Date)
		Get
			Return _fechaApertura
		End Get
		Set(ByVal value As Nullable(Of Date))
			_fechaApertura = value
		End Set
	End Property

	Public Property FechaCierre As Nullable(Of Date)
		Get
			Return _fechaCierre
		End Get
		Set(ByVal value As Nullable(Of Date))
			_fechaCierre = value
		End Set
	End Property

	Public Property DescripcionProblema As String
		Get
			Return _descripcionProblema
		End Get
		Set(ByVal value As String)
			_descripcionProblema = value
		End Set
	End Property

	''' <summary>
	''' Identificador del Tipo de Incidencia y Tipo de Documento:
	''' 1-Troqueleria (gtkTroqueleria),
	''' 2-Servicios Generales (gtkSerciviosGenerales),
	''' 3-Txokos (gtkTxokos),
	''' 4-Sugerencias (gtkSugerencias),
	''' 5-Seguridad Laboral-Sucesos (gtkIstriku),
	''' 6-Sistemas Mantenimiento (gtkMantenimientoSist).
	''' Campo de BUSQUEDA (GERTAKARIAK.IDTIPOINCIDENCIA).
	''' </summary>
	Protected Property IdTipoIncidencia As Nullable(Of Integer)
		Get
			Return _idTipoIncidencia
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_idTipoIncidencia = value
		End Set
	End Property
	''' <summary>
	''' Identificador del usuario que a creado la No Conformidad (SAB.USUARIOS.ID).
	''' Campo de BUSQUEDA (GERTAKARIAK.IDCREADOR).
	''' </summary>
	Public Property IdCreador As Nullable(Of Integer)
		Get
			Return _idCreador
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_idCreador = value
		End Set
	End Property
	''' <summary>
	''' Identificador del "Activo" en Prisma (ASSET.ASSET).
	''' </summary>
	Protected Property IdActivo As String
		Get
			Return _idActivo
		End Get
		Set(ByVal value As String)
			_idActivo = value
		End Set
	End Property
End Class

''' <summary>
''' Identificador del Tipo de Incidencia y Tipo de Documento:
''' 1-Troqueleria (gtkTroqueleria),
''' 2-Servicios Generales (gtkSerciviosGenerales),
''' 3-Txokos (gtkTxokos),
''' 4-Sugerencias (gtkSugerencias),
''' 5-Seguridad Laboral-Sucesos (gtkIstriku),
''' 6-Sistemas Mantenimiento (gtkMantenimientoSist).
''' </summary>
Public Enum TipoIncidencia
	''' <summary>
	''' Incidencia del tipo de "Troqueleria".
	''' </summary>
	gtkTroqueleria = 1
	''' <summary>
	''' Incidencia del tipo de "Servicios Generales".
	''' </summary>
	gtkServiciosGenerales = 2
	''' <summary>
	''' Incidencia del tipo de "Txokos".
	''' </summary>
	gtkTxokos = 3
	''' <summary>
	''' Incidencia del tipo de "Sugerencias".
	''' </summary>
	gtkSugerencias = 4
	''' <summary>
	''' Incidencia del tipo de "Seguridad Laboral-Sucesos".
	''' </summary>
	gtkIstriku = 5
	''' <summary>
	''' Incidencia del tipo de "Sistemas Mantenimiento".
	''' </summary>
	gtkMantenimientoSist = 6
End Enum
''' <summary>
''' Tipo de Acciones Correctoras.
''' </summary>
''' <remarks>
''' 4 tipos:
''' - Contenedora (Inmediata)
''' - Provisionales
''' - Definitivas
''' - Preventivas
''' </remarks>
Public Enum TipoAcciones
	''' <summary>
	''' Contenedora o Inmediata
	''' </summary>
	Contenedora = 1
	Provisional = 2
	Definitiva = 3
	Preventiva = 4
End Enum

<Serializable()> _
Public Structure gtkGridView
    ''' <summary>
    ''' Identificador del Registro Seleccionado.
    ''' </summary>
    Private _IdSeleccionado As String
    'Private _IdSeleccionado As Nullable(Of Integer)
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
    Public Property IdSeleccionado() As String
        Get
            Return _IdSeleccionado
        End Get
        Set(ByVal value As String)
            _IdSeleccionado = value
        End Set
    End Property
    'Public Property IdSeleccionado() As Nullable(Of Integer)
    '    Get
    '        Return _IdSeleccionado
    '    End Get
    '    Set(ByVal value As Nullable(Of Integer))
    '        _IdSeleccionado = value
    '    End Set
    'End Property

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

Public MustInherit Class Responsables_Gertakariak_ELL
	''' <summary>
	''' Identificador único del registro.
	''' Campo de BUSQUEDA (RESPONSABLES_GERTAKARIAK.ID).
	''' </summary>
	Private _id As Nullable(Of Integer)
	Private _idIncidencia As Nullable(Of Integer)
	Private _idUsuario As Nullable(Of Integer)

	''' <summary>
	''' Identificador único del registro.
	''' Campo de BUSQUEDA (RESPONSABLES_GERTAKARIAK.ID).
	''' </summary>
	Protected Property Id As Nullable(Of Integer)
		Get
			Return _id
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_id = value
		End Set
	End Property

	''' <summary>
	''' Identificador del usuario en SAB.
	''' </summary>
	Public Property IdUsuario As Nullable(Of Integer)
		Get
			Return _idUsuario
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_idUsuario = value
		End Set
	End Property

	''' <summary>
	''' Identificador de la Incidencia.
	''' </summary>
	Public Property IdIncidencia As Nullable(Of Integer)
		Get
			Return _idIncidencia
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_idIncidencia = value
		End Set
	End Property

End Class

Public MustInherit Class Estructura_ELL
	''' <summary>
	''' Identificador del registro
	''' </summary>
	Private _Id As Nullable(Of Integer)
	''' <summary>
	''' Origen del registro. Registro padre.
	''' </summary>
	Private _IdIturria As Nullable(Of Integer)
	''' <summary>
	''' Descripcion del elemento.
	''' </summary>
    Private _Descripcion As String

    ''' <summary>
    ''' Identificador del Tipo de Incidencia y Tipo de Documento:
    ''' 1-Troqueleria (gtkTroqueleria),
    ''' 2-Servicios Generales (gtkSerciviosGenerales),
    ''' 3-Txokos (gtkTxokos),
    ''' 4-Sugerencias (gtkSugerencias),
    ''' 5-Seguridad Laboral-Sucesos (gtkIstriku),
    ''' 6-Sistemas Mantenimiento (gtkMantenimientoSist).
    ''' </summary>
    Private _idTipoIncidencia As Nullable(Of GertakariakLib2.TipoIncidencia)

	''' <summary>
	''' Identificador del registro.
	''' </summary>
	Protected Property Id As Nullable(Of Integer)
		Get
			Return _Id
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_Id = value
		End Set
	End Property
	''' <summary>
	''' Origen del registro. Registro padre.
	''' </summary>
	Public Property IdIturria As Nullable(Of Integer)
		Get
			Return _IdIturria
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_IdIturria = value
		End Set
	End Property
	''' <summary>
	''' Descripcion del elemento.
	''' </summary>
	Public Property Descripcion As String
		Get
			Return _Descripcion
		End Get
		Set(ByVal value As String)
			_Descripcion = value
		End Set
    End Property

    ''' <summary>
    ''' Identificador del Tipo de Incidencia y Tipo de Documento:
    ''' 1-Troqueleria (gtkTroqueleria),
    ''' 2-Servicios Generales (gtkSerciviosGenerales),
    ''' 3-Txokos (gtkTxokos),
    ''' 4-Sugerencias (gtkSugerencias),
    ''' 5-Seguridad Laboral-Sucesos (gtkIstriku),
    ''' 6-Sistemas Mantenimiento (gtkMantenimientoSist).
    ''' Campo de BUSQUEDA (GERTAKARIAK.IDTIPOINCIDENCIA).
    ''' </summary>
    Protected Property IdTipoIncidencia As Nullable(Of GertakariakLib2.TipoIncidencia)
        Get
            Return _idTipoIncidencia
        End Get
        Set(ByVal value As Nullable(Of GertakariakLib2.TipoIncidencia))
            _idTipoIncidencia = value
        End Set
    End Property
End Class


Public MustInherit Class Clasificacion_ELL
    ''' <summary>
    ''' Identificador del registro
    ''' </summary>
    Private _Id As Nullable(Of Integer)
    ''' <summary>
    ''' Origen del registro. Registro padre.
    ''' </summary>
    Private _IdIturria As Nullable(Of Integer)
    ''' <summary>
    ''' Descripcion del elemento.
    ''' </summary>
    Private _Descripcion As String

    ''' <summary>
    ''' Identificador del registro.
    ''' </summary>
    Protected Property Id As Nullable(Of Integer)
        Get
            Return _Id
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _Id = value
        End Set
    End Property
    ''' <summary>
    ''' Origen del registro. Registro padre.
    ''' </summary>
    Public Property IdIturria As Nullable(Of Integer)
        Get
            Return _IdIturria
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _IdIturria = value
        End Set
    End Property
    ''' <summary>
    ''' Descripcion del elemento.
    ''' </summary>
    Public Property Descripcion As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            _Descripcion = value
        End Set
    End Property
End Class
Public MustInherit Class Caracteristicas_ELL
	Private _IdEstructura As Nullable(Of Integer)
	Private _IdIncidencia As Nullable(Of Integer)

	Public Property IdIncidencia As Nullable(Of Integer)
		Get
			Return _IdIncidencia
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_IdIncidencia = value
		End Set
	End Property

	Public Property IdEstructura As Nullable(Of Integer)
		Get
			Return _IdEstructura
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_IdEstructura = value
		End Set
	End Property

End Class

Public MustInherit Class Acciones_ELL
	Private _Descripcion As String
	''' <summary>
	''' Valoración de la Accion (Medida Correctora) realizada.
	''' </summary>
	Private _Eficacia As String
	Private _FechaFin As Nullable(Of Date)
	Private _FechaInicio As Nullable(Of Date)
	''' <summary>
	''' Fecha prevista de finalizacion de la accion o fecha de revison de la accion.
	''' Campo de Busqueda (ACCION.FECHAPREVISTA)
	''' </summary>
	''' <remarks></remarks>
	Private _FechaPrevista As Nullable(Of Date)
	''' <summary>
	''' Identificador unico del registro.
	''' Campo de Busqueda (ACCIONES.ID)
	''' </summary>
	Private _Id As Nullable(Of Integer)
	''' <summary>
	''' Tipo de Acciones Correctoras.
	''' </summary>
	Private _IdTipoAccion As Nullable(Of GertakariakLib2.TipoAcciones)
	''' <summary>
	''' Porcentaje de realizacionde la accion.
	''' </summary>
	Private _Realizacion As Nullable(Of Integer)

	''' <summary>
	''' Identificador unico del registro.
	''' Campo de Busqueda (ACCIONES.ID)
	''' </summary>
	Protected Property Id As Nullable(Of Integer)
		Get
			Return _Id
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_Id = value
		End Set
	End Property
	''' <summary>
	''' Tipo de Acciones Correctoras.
	''' </summary>
	''' <remarks>
	''' 4 tipos:
	''' - Contenedora (Inmediata)
	''' - Provisionales
	''' - Definitivas
	''' - Preventiva
	''' </remarks>
	Public Property IdTipoAccion As Nullable(Of TipoAcciones)
		Get
			Return _IdTipoAccion
		End Get
		Set(ByVal value As Nullable(Of TipoAcciones))
			_IdTipoAccion = value
		End Set
	End Property
	Public Property FechaInicio As Nullable(Of Date)
		Get
			Return _FechaInicio
		End Get
		Set(ByVal value As Nullable(Of Date))
			_FechaInicio = value
		End Set
	End Property
	Public Property FechaFin As Nullable(Of Date)
		Get
			Return _FechaFin
		End Get
		Set(ByVal value As Nullable(Of Date))
			_FechaFin = value
		End Set
	End Property
	''' <summary>
	''' Fecha prevista de finalizacion de la accion o fecha de revison de la accion.
	''' Campo de Busqueda (ACCION.FECHAPREVISTA)
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Property FechaPrevista As Nullable(Of Date)
		Get
			Return _FechaPrevista
		End Get
		Set(ByVal value As Nullable(Of Date))
			_FechaPrevista = value
		End Set
	End Property
	Public Property Descripcion As String
		Get
			Return _Descripcion
		End Get
		Set(ByVal value As String)
			_Descripcion = value
		End Set
	End Property
	''' <summary>
	''' Valoración de la Accion (Medida Correctora) realizada.
	''' </summary>
	Public Property Eficacia As String
		Get
			Return _Eficacia
		End Get
		Set(ByVal value As String)
			_Eficacia = value
		End Set
	End Property
	''' <summary>
	''' Porcentaje de realizacionde la accion.
	''' </summary>
	Public Property Realizacion As Nullable(Of Integer)
		Get
			Return _Realizacion
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_Realizacion = value
		End Set
	End Property
End Class

Public MustInherit Class Gertakariak_Acciones_ELL
	Private _IdAccion As Nullable(Of Integer)
	Private _IdIncidencia As Nullable(Of Integer)

	Public Property IdAccion As Nullable(Of Integer)
		Get
			Return _IdAccion
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_IdAccion = value
		End Set
	End Property
	Public Property IdIncidencia As Nullable(Of Integer)
		Get
			Return _IdIncidencia
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_IdIncidencia = value
		End Set
	End Property
End Class

Public MustInherit Class Acciones_Usuario_ELL
	Private _IdAccion As Nullable(Of Integer)
	Private _IdUsuario As Nullable(Of Integer)

	''' <summary>
	''' Identificador de la accion.
	''' Campo de Busqueda.
	''' </summary>
	Public Property IdAccion As Nullable(Of Integer)
		Get
			Return _IdAccion
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_IdAccion = value
		End Set
	End Property
	''' <summary>
	''' Identificador del usuario en SAB.
	''' Campo de Busqueda.
	''' </summary>
	Public Property IdUsuario As Nullable(Of Integer)
		Get
			Return _IdUsuario
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_IdUsuario = value
		End Set
	End Property
End Class


Public MustInherit Class Acciones_Ejecucion_ELL
    Private _IdAccion As Nullable(Of Integer)
    Private _IdUsuario As Nullable(Of Integer)

    ''' <summary>
    ''' Identificador de la accion.
    ''' Campo de Busqueda.
    ''' </summary>
    Public Property IdAccion As Nullable(Of Integer)
        Get
            Return _IdAccion
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _IdAccion = value
        End Set
    End Property
    ''' <summary>
    ''' Identificador del usuario en SAB.
    ''' Campo de Busqueda.
    ''' </summary>
    Public Property IdUsuario As Nullable(Of Integer)
        Get
            Return _IdUsuario
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _IdUsuario = value
        End Set
    End Property
End Class
Public MustInherit Class Acciones_Observaciones_ELL

	Private _Descripcion As String

	''' <summary>
	''' Identificador unico del registro.
	''' Campo de Busqueda (ACCIONES_OBSERVACIONES.ID)
	''' </summary>
	Private _Id As Nullable(Of Integer)
	''' <summary>
	''' Fecha en la que se creo el registro.
	''' </summary>
	Private _Fecha As Nullable(Of Date)
	Private _IdAccion As Integer?
	Private _IdUsuario As Integer?

	''' <summary>
	''' Identificador unico del registro.
	''' Campo de Busqueda (ACCIONES_OBSERVACIONES.ID)
	''' </summary>
	Protected Property Id As Nullable(Of Integer)
		Get
			Return _Id
		End Get
		Set(value As Nullable(Of Integer))
			_Id = value
		End Set
	End Property
    ''' <summary>
    ''' Identificador de la acción a la que pertenece el registro.
    ''' Campo de Busqueda (ACCIONES_OBSERVACIONES.IDACCION)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
	Public Property IdAccion As Nullable(Of Integer)
		Get
			Return _IdAccion
		End Get
		Set(value As Nullable(Of Integer))
			_IdAccion = value
		End Set
	End Property

	Public Property IdUsuario As Nullable(Of Integer)
		Get
			Return _IdUsuario
		End Get
		Set(value As Nullable(Of Integer))
			_IdUsuario = value
		End Set
	End Property

	''' <summary>
	''' Fecha en la que se creo el registro.
	''' </summary>
	Public Property Fecha As Nullable(Of Date)
		Get
			Return _Fecha
		End Get
		Set(value As Nullable(Of Date))
			_Fecha = value
		End Set
	End Property

	Public Property Descripcion As String
		Get
			Return _Descripcion
		End Get
		Set(value As String)
			_Descripcion = value
		End Set
	End Property


End Class


Public MustInherit Class Actas_MS_ELL



    ''' <summary>
    ''' Identificador unico del registro.
    ''' Campo de Busqueda (Actas_MS.ID)
    ''' </summary>
    Private _Id As Nullable(Of Integer)
    ''' <summary>
    ''' Fecha en la que se creo el registro.
    ''' </summary>
    Private _Fecha As Nullable(Of Date)
    Private _IdIncidencia As Integer?
    Private _IdAccion As Integer?
    Private _IdObservacion As Integer?
    Private _IdUsuario As Integer?
    Private _Nuevo As Integer?

    ''' <summary>
    ''' Identificador unico del registro.
    ''' </summary>
    Protected Property Id As Nullable(Of Integer)
        Get
            Return _Id
        End Get
        Set(value As Nullable(Of Integer))
            _Id = value
        End Set
    End Property
    ''' <summary>
    ''' Identificador de la acción a la que pertenece el registro.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdAccion As Nullable(Of Integer)
        Get
            Return _IdAccion
        End Get
        Set(value As Nullable(Of Integer))
            _IdAccion = value
        End Set
    End Property
    Public Property IdIncidencia As Nullable(Of Integer)
        Get
            Return _IdIncidencia
        End Get
        Set(value As Nullable(Of Integer))
            _IdIncidencia = value
        End Set
    End Property
    ''' <summary>
    ''' Fecha en la que se creo el registro.
    ''' </summary>
    Public Property Fecha As Nullable(Of Date)
        Get
            Return _Fecha
        End Get
        Set(value As Nullable(Of Date))
            _Fecha = value
        End Set
    End Property
    Public Property IdObservacion As Nullable(Of Integer)
        Get
            Return _IdObservacion
        End Get
        Set(value As Nullable(Of Integer))
            _IdObservacion = value
        End Set
    End Property
    Public Property IdUsuario As Nullable(Of Integer)
        Get
            Return _IdUsuario
        End Get
        Set(value As Nullable(Of Integer))
            _IdUsuario = value
        End Set
    End Property
    Public Property Nuevo As Nullable(Of Integer)
        Get
            Return _Nuevo
        End Get
        Set(value As Nullable(Of Integer))
            _Nuevo = value
        End Set
    End Property
End Class

Public Class gtkTroqueleria
    Inherits GertakariakLib2.Entidades.Gertakariak_DAL
    ''' <summary>
    ''' Acciones de la Incidencia.
    ''' </summary>
    ''' <remarks></remarks>
    Private _Acciones As List(Of gtkAcciones)
    ''' <summary>
    ''' Identificador único del registro.
    ''' Campo de BUSQUEDA (GERTAKARIAK.ID).
    ''' </summary>
    Public Shadows ReadOnly Property Id As Nullable(Of Integer)
        Get
            Return MyBase.Id
        End Get
    End Property

    ''' <summary>
    ''' Identificador del Tipo de Incidencia y Tipo de Documento:
    ''' 1- Troqueleria (gtkTroqueleria).
    ''' </summary>
    Public Overloads ReadOnly Property idTipoIncidencia As TipoIncidencia
        Get
            Return MyBase.IdTipoIncidencia
        End Get
    End Property

    ''' <summary>
    ''' Acciones de la Incidencia.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Acciones As List(Of gtkAcciones)
        Get
			If _Acciones Is Nothing OrElse Not _Acciones.Any Then
				_Acciones = New gtkAcciones() With {.IdIncidencia = Me.Id}.Listado
				_Acciones = If(_Acciones Is Nothing OrElse Not _Acciones.Any, Nothing, _Acciones)
			End If
            Return _Acciones
        End Get
    End Property

    Sub New()
        MyBase.IdTipoIncidencia = GertakariakLib2.TipoIncidencia.gtkTroqueleria
    End Sub

    ''' <summary>
    ''' Devuelve una lista de incidencias para los parametros especificados.
    ''' </summary>
    ''' <returns>List(Of gtkMatenimientoSist)</returns>
    ''' <remarks></remarks>
    Public Function Listado(Optional FiltroGTK As gtkFiltro = Nothing) As List(Of gtkTroqueleria)
        Dim ListaObjetos As List(Of GertakariakLib2.Entidades.Gertakariak_DAL) = Me.Load(FiltroGTK)
        Listado = Nothing
        If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Count > 0 Then
            Listado = New List(Of gtkTroqueleria)
            For Each item As GertakariakLib2.Entidades.Gertakariak_DAL In ListaObjetos
                Dim CopiaObjeto As New gtkTroqueleria
                Dim f As New Funciones
                f.CopiarPropiedades(item, CopiaObjeto)
                Listado.Add(CopiaObjeto)
            Next
        End If
        Return Listado
    End Function


   

End Class
