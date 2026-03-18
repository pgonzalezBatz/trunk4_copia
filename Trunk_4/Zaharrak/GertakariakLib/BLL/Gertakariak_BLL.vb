'Imports System.Linq

''' <summary>
''' Incidencias del Mantenimiento de Sistemas de Automoción
''' </summary>
Public Class gtkMatenimientoSist
	Inherits GertakariakLib2.Entidades.Gertakariak_DAL
	''' <summary>
	''' Caracteristica de la NC que indica el tipo.
	''' </summary>
	Private _TipoIncidencia As Nullable(Of Integer)
	''' <summary>
	''' Responsables de la NC.
	''' </summary>
	''' <remarks></remarks>
	Private _Responsables As List(Of SABLib.ELL.Usuario)
	''' <summary>
	''' Acciones de la Incidencia.
	''' </summary>
	''' <remarks></remarks>
	Private _Acciones As List(Of gtkAcciones)
	''' <summary>
	''' Avance de la incidencia en base a la media ponderada. "Avance Incidencia"="Suma Avance Acciones"/"Nº Acciones".
	''' </summary>
	Private _Avance As Nullable(Of Integer)

	Sub New()
		MyBase.IdTipoIncidencia = GertakariakLib2.TipoIncidencia.gtkMantenimientoSist
	End Sub

	''' <summary>
	''' Identificador del Tipo de Incidencia y Tipo de Documento:
	''' 6-Sistemas Mantenimiento (gtkMantenimientoSist).
	''' </summary>
	Public Overloads ReadOnly Property idTipoIncidencia As TipoIncidencia
		Get
			Return MyBase.IdTipoIncidencia
		End Get
	End Property

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
	''' Nº de semanas entre la "FechaApertura" y "FechaCierre" o la fecha actual.
	''' </summary>
	''' <remarks>Consideramos el lunes 1º día de la semana y la 1º semana del año la que tenga 4 dias (= comienza en Jueves).</remarks>
	Public ReadOnly Property Demora As Nullable(Of Long)
		Get
			If MyBase.FechaApertura IsNot Nothing Then
				Demora = DateDiff(DateInterval.WeekOfYear, MyBase.FechaApertura.Value, If(IsDate(MyBase.FechaCierre), MyBase.FechaCierre.Value, Now), FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays)
			End If
			Return Demora
		End Get
	End Property
	''' <summary>
	''' Responsables de la NC.
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public ReadOnly Property Responsables As List(Of SABLib.ELL.Usuario)
		Get
			'--------------------------------------------------------------------
			'Dim gtkRes As New gtkResponsable With {.IdIncidencia = Me.Id}
			'Dim f As New Sablib.BLL.UsuariosComponent
			'Dim ListaResp As List(Of gtkResponsable) = gtkRes.Listado
			'Responsables = Nothing
			'If ListaResp IsNot Nothing Then
			'	Responsables = New List(Of Sablib.ELL.Usuario)
			'	For Each item As gtkResponsable In ListaResp
			'		Responsables.Add(f.GetUsuario(New Sablib.ELL.Usuario With {.Id = item.IdUsuario}))
			'	Next
			'End If
			'Return Responsables
			'--------------------------------------------------------------------
			'FROGA: Aceleracion del proceso de carga.
			'--------------------------------------------------------------------
			If _Responsables Is Nothing OrElse Not _Responsables.Any Then
				Dim gtkRes As New gtkResponsable With {.IdIncidencia = Me.Id}
				Dim f As New SabLib.BLL.UsuariosComponent
				Dim ListaResp As List(Of gtkResponsable) = gtkRes.Listado
				_Responsables = Nothing
				If ListaResp IsNot Nothing Then
					_Responsables = New List(Of SabLib.ELL.Usuario)
					For Each item As gtkResponsable In ListaResp
						_Responsables.Add(f.GetUsuario(New SabLib.ELL.Usuario With {.Id = item.IdUsuario}, False))
					Next
				End If
			End If
			Return _Responsables
			'--------------------------------------------------------------------
		End Get
	End Property
	''' <summary>
	''' Identificador del "Activo" en Prisma (ASSET.ASSET).
	''' </summary>
	''' <remarks>
	''' Consta de 3 niveles:
	''' 1.	Instalacion	(CompanyLevel = 2)
	''' 2.	Familia		(CompanyLevel = 3)
	''' 3.	Linea		(CompanyLevel = 4)
	''' </remarks>
	Public Property Instalacion As String
		Get
			Return IdActivo
		End Get
		Set(ByVal value As String)
			IdActivo = value
		End Set
	End Property
	''' <summary>
	''' Identificador de la Caracteristica para la estructura "Tipo de Incidencia" de la NC.
	''' </summary>
	Public Property TipoIncidencia As Nullable(Of Integer)
		Get
			Return _TipoIncidencia
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_TipoIncidencia = value
		End Set
	End Property
	''' <summary>
	''' Acciones de la Incidencia.
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public ReadOnly Property Acciones As List(Of gtkAcciones)
		Get
			''Acciones = New List(Of gtkAcciones)
			''Dim ListaGA As List(Of gtkGertakariak_Acciones) = New gtkGertakariak_Acciones() With {.IdIncidencia = Me.Id}.Listado
			''If ListaGA IsNot Nothing Then
			''	Dim ListaGA_LJ_ListaAcciones = From Incidencia_Accion As gtkGertakariak_Acciones In ListaGA _
			''	  Join Accion As gtkAcciones In New gtkAcciones().Listado On Incidencia_Accion.IdAccion Equals Accion.Id _
			''	  Select Accion
			''	ListaGA_LJ_ListaAcciones.ToList.ForEach(Sub(o) Acciones.Add(o))
			''End If
			''-------------------------------------------------------------------------
			'Acciones = New gtkAcciones() With {.IdIncidencia = Me.Id}.Listado
			'Return If(Acciones Is Nothing OrElse Acciones.Count = 0, Nothing, Acciones)
			'--------------------------------------------------------------------
			'FROGA: Aceleracion del proceso de carga.
			'--------------------------------------------------------------------
			If _Acciones Is Nothing OrElse Not _Acciones.Any Then
				_Acciones = New gtkAcciones() With {.IdIncidencia = Me.Id}.Listado
				_Acciones = If(_Acciones Is Nothing OrElse Not _Acciones.Any, Nothing, _Acciones)
			End If
			'--------------------------------------------------------------------
			Return _Acciones
		End Get
	End Property
	''' <summary>
	''' Avance de la incidencia en base a la media ponderada. "Avance Incidencia"="Suma Avance Acciones"/"Nº Acciones".
	''' </summary>
	Public ReadOnly Property Avance As Nullable(Of Integer)
		Get
			If Acciones Is Nothing OrElse Not Acciones.Any Then
				_Avance = If(FechaCierre Is Nothing, 0, 100)
			Else
				_Avance = Acciones.Sum(Function(o) o.Realizacion) / Acciones.Count
			End If
			Return _Avance
		End Get
	End Property

	''' <summary>
	''' Devuelve una lista de incidencias para los parametros especificados.
	''' </summary>
	''' <returns>List(Of gtkMatenimientoSist)</returns>
	''' <remarks></remarks>
	<Obsolete("Dejar de usar esta funcion para usar LINQ to Entities.")> _
	Public Function Listado(Optional FiltroGTK As gtkFiltro = Nothing) As List(Of gtkMatenimientoSist)
		Dim ListaObjetos As List(Of GertakariakLib2.Entidades.Gertakariak_DAL) = Me.Load(FiltroGTK)
		Listado = Nothing
		If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Any Then
			Listado = New List(Of gtkMatenimientoSist)
			For Each item As GertakariakLib2.Entidades.Gertakariak_DAL In ListaObjetos
				Dim CopiaObjeto As New gtkMatenimientoSist
				Dim f As New Funciones
				f.CopiarPropiedades(item, CopiaObjeto)
				Listado.Add(CopiaObjeto)
			Next
		End If
		Return Listado
	End Function

	''' <summary>
	''' Obtiene los datos en base al campo ID.
	''' Si no encuentra ningun resultado el campo ID es NOTHING.
	''' </summary>
	''' <param name="ID">Identificador del Registro.</param>
	Public Sub Cargar(ByVal ID As Integer)
		Dim ListaObjetos As New List(Of gtkMatenimientoSist)
		Dim funcGTK As New GertakariakLib2.Funciones
		MyBase.Id = ID
		ListaObjetos = Me.Listado()
		If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Count = 1 Then
			'-------------------------------------------------
			'Tipo de Incidencia
			'-------------------------------------------------
			Dim Listado As New List(Of gtkCaracteristica)
			Dim Caracteristicas As New gtkCaracteristica
			Caracteristicas.IdIncidencia = Me.Id
			Listado = Caracteristicas.Listado()
			If Listado IsNot Nothing AndAlso Listado.Any Then Me.TipoIncidencia = Listado(0).IdEstructura
			'-------------------------------------------------
			funcGTK.CopiarPropiedades(ListaObjetos(0), Me)
		Else
			MyBase.Id = Nothing
		End If
	End Sub

	''' <summary>
	''' Si el objeto tiene su identificador informado y existe el registro modificara los datos.
	''' En caso contrario realizara un insercion.
	''' </summary>
	''' <remarks></remarks>
	Public Sub Guardar()
		If Me.Id Is Nothing Or Me.Listado() Is Nothing Then Me.Insert() Else Me.Update()
		'--------------------------------------------------------------------
		'Guardamos el "Tipo de Incidencia" (Caracteristica).
		'--------------------------------------------------------------------
		'Dim Listado As New List(Of gtkCaracteristica)
		'Dim Caracteristicas As New gtkCaracteristica
		'Caracteristicas.IdIncidencia = Me.Id
		'Caracteristicas.IdEstructura = Me.TipoIncidencia
		'Caracteristicas.Guardar()
		'Listado = Caracteristicas.Listado()
		'If Listado IsNot Nothing AndAlso Listado.Count > 0 Then Me.TipoIncidencia = Listado(0).IdEstructura
		'--------------------------------------------------------------------
	End Sub

	''' <summary>
	''' Eliminar el registro para el identificador indicado.
	''' </summary>
	Public Sub Eliminar()
		Dim Transaccion As New Transaccion
		Try
			If My.Settings.ConexionOracle.State = ConnectionState.Closed Then Transaccion.Abrir()
			'---------------------------------------------------------------------
			'Buscamos las acciones relacionadas con la Incidencia para borrarlas.
			'---------------------------------------------------------------------
			Dim lAccionesIncidencia As New List(Of gtkGertakariak_Acciones)
			Dim Acciones As New gtkGertakariak_Acciones With {.IdIncidencia = Me.Id}
			lAccionesIncidencia = Acciones.Listado
			If lAccionesIncidencia IsNot Nothing AndAlso lAccionesIncidencia.Any Then
				'---------------------------------------------------------------------------------------
				'Comprobamos que la accion no esta relacionada con mas incidencias para poder borrarla.
				'---------------------------------------------------------------------------------------
				For Each GerAcc As gtkGertakariak_Acciones In lAccionesIncidencia
					Dim GerAccB As New gtkGertakariak_Acciones With {.IdAccion = GerAcc.IdAccion}
					Dim lGerAcc As List(Of gtkGertakariak_Acciones) = GerAccB.Listado

					If lGerAcc IsNot Nothing AndAlso lGerAcc.Count = 1 Then
						Dim Accion As New gtkAcciones
						Accion.Cargar(GerAcc.IdAccion)
						Accion.Eliminar()
					End If
				Next
			End If
			'---------------------------------------------------------------------
			Me.Delete()
			If Transaccion.Estado = ConnectionState.Open Then Transaccion.Cerrar()
			MyBase.Id = Nothing
		Catch ex As Exception
			If Transaccion.Estado = ConnectionState.Open Then Transaccion.Rollback()
			throw 
		End Try
	End Sub
End Class
''' <summary>
''' NO UTILIZAR.
''' EN DESARROLLO.
''' </summary>
Public Class gtkIstriku
	Inherits GertakariakLib2.Entidades.Gertakariak_DAL

End Class
Public Class Prueba
	Inherits GertakariakLib2.Entidades.Deteccion_DAL
	''' <summary>
	''' Devuelve una lista de Afectados para los parametros especificados.
	''' </summary>
	''' <returns>List(Of gtkAfectado)</returns>
	''' <remarks></remarks>
	Public Function Listado() As List(Of Prueba)
		Dim ListaObjetos As List(Of GertakariakLib2.Entidades.Deteccion_DAL) = Me.Load
		Listado = Nothing
		If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Any Then
			Listado = New List(Of Prueba)
			For Each item As GertakariakLib2.Entidades.Deteccion_DAL In ListaObjetos
				Dim CopiaObjeto As New Prueba
				Dim f As New Funciones
				f.CopiarPropiedades(item, CopiaObjeto)
				Listado.Add(CopiaObjeto)
			Next
		End If
		Return Listado
	End Function

End Class
Public Class gtkResponsable
	Inherits GertakariakLib2.Entidades.Responsables_Gertakariak_DAL
	''' <summary>
	''' Identificador único del registro.
	''' Campo de BUSQUEDA (RESPONSABLES_GERTAKARIAK.ID).
	''' </summary>
	Public Shadows ReadOnly Property Id As Nullable(Of Integer)
		Get
			Return MyBase.Id
		End Get
	End Property
	''' <summary>
	''' Devuelve una lista de "Responsables" para los parametros especificados en el propio objeto.
	''' </summary>
	''' <returns>List(Of gtkResponsable)</returns>
	''' <remarks></remarks>
	Public Function Listado() As List(Of gtkResponsable)
		Dim ListaObjetos As List(Of GertakariakLib2.Entidades.Responsables_Gertakariak_DAL) = Me.Load
		Listado = Nothing
		If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Any Then
			Listado = New List(Of gtkResponsable)
			For Each item As GertakariakLib2.Entidades.Responsables_Gertakariak_DAL In ListaObjetos
				Dim CopiaObjeto As New gtkResponsable
				Dim f As New Funciones
				f.CopiarPropiedades(item, CopiaObjeto)
				Listado.Add(CopiaObjeto)
			Next
		End If
		Return Listado
	End Function
	''' <summary>
	''' Obtiene los datos en base al campo ID.
	''' Si no encuentra ningun resultado el campo ID es NOTHING.
	''' </summary>
	''' <param name="ID">Identificador del Registro.</param>
	''' <remarks></remarks>
	Public Sub Cargar(ByVal ID As Integer)
		Dim ListaObjetos As New List(Of gtkResponsable)
		Dim funcGTK As New GertakariakLib2.Funciones
		MyBase.Id = ID
		ListaObjetos = Me.Listado()
		If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Count = 1 Then
			funcGTK.CopiarPropiedades(ListaObjetos(0), Me)
		Else
			MyBase.Id = Nothing
		End If
	End Sub
	''' <summary>
	''' Eliminar el registro para el identificador indicado.
	''' </summary>
	Public Sub Eliminar()
		Me.Delete()
	End Sub
	''' <summary>
	''' Si el objeto tiene su identificador informado y existe el registro modificara los datos.
	''' En caso contrario realizara un insercion.
	''' </summary>
	''' <remarks></remarks>
	Public Sub Guardar()
		If Me.Id Is Nothing Or Me.Listado() Is Nothing Then Me.Insert() Else Me.Update()
	End Sub
End Class

Public Class gtkGCCILINDROS
	Inherits XBATLib.Entidades.GCCilindros_DAL

	Public Shadows ReadOnly Property ID As Nullable(Of Integer)
		Get
			Return MyBase.ID
		End Get
	End Property

	''' <summary>
	''' Obtiene los datos en base al campo ID.
	''' Si no encuentra ningun resultado el campo ID es NOTHING.
	''' </summary>
	''' <param name="ID">Identificador del Registro.</param>
	''' <remarks></remarks>
	Public Sub Cargar(ByVal ID As Integer)
		Dim ListaObjetos As New List(Of gtkGCCILINDROS)
		Dim funcGTK As New GertakariakLib2.Funciones
		MyBase.ID = ID
		ListaObjetos = Me.Listado()
		If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Count = 1 Then
			funcGTK.CopiarPropiedades(ListaObjetos(0), Me)
		Else
			MyBase.ID = Nothing
		End If
	End Sub
	''' <summary>
	''' Eliminar el registro para el identificador indicado.
	''' </summary>
	Public Sub Eliminar()
		Me.Delete()
	End Sub
	''' <summary>
	''' Devuelve una lista de Afectados para los parametros especificados.
	''' </summary>
	''' <returns>List(Of gtkAfectado)</returns>
	''' <remarks></remarks>
	Public Function Listado() As List(Of gtkGCCILINDROS)
		Dim ListaObjetos As List(Of XBATLib.Entidades.GCCilindros_DAL) = Me.Load
		Listado = Nothing
		If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Any Then
			Listado = New List(Of gtkGCCILINDROS)
			For Each item As XBATLib.Entidades.GCCilindros_DAL In ListaObjetos
				Dim CopiaObjeto As New gtkGCCILINDROS
				Dim f As New Funciones
				f.CopiarPropiedades(item, CopiaObjeto)
				Listado.Add(CopiaObjeto)
			Next
		End If
		Return Listado
	End Function
End Class


Public Class gtkGCBULONES
	Inherits XBATLib.Entidades.GCBulones_DAL

	Public Shadows ReadOnly Property ID As Nullable(Of Integer)
		Get
			Return MyBase.ID
		End Get
	End Property

	''' <summary>
	''' Obtiene los datos en base al campo ID.
	''' Si no encuentra ningun resultado el campo ID es NOTHING.
	''' </summary>
	''' <param name="ID">Identificador del Registro.</param>
	''' <remarks></remarks>
	Public Sub Cargar(ByVal ID As Integer)
		Dim ListaObjetos As New List(Of gtkGCBULONES)
		Dim funcGTK As New GertakariakLib2.Funciones
		MyBase.ID = ID
		ListaObjetos = Me.Listado()
		If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Count = 1 Then
			funcGTK.CopiarPropiedades(ListaObjetos(0), Me)
		Else
			MyBase.ID = Nothing
		End If
	End Sub
	''' <summary>
	''' Eliminar el registro para el identificador indicado.
	''' </summary>
	Public Sub Eliminar()
		Me.Delete()
	End Sub
	''' <summary>
	''' Devuelve una lista de Afectados para los parametros especificados.
	''' </summary>
	''' <returns>List(Of gtkAfectado)</returns>
	''' <remarks></remarks>
	Public Function Listado() As List(Of gtkGCBULONES)
		Dim ListaObjetos As List(Of XBATLib.Entidades.GCBulones_DAL) = Me.Load
		Listado = Nothing
		If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Any Then
			Listado = New List(Of gtkGCBULONES)
			For Each item As XBATLib.Entidades.GCBulones_DAL In ListaObjetos
				Dim CopiaObjeto As New gtkGCBULONES
				Dim f As New Funciones
				f.CopiarPropiedades(item, CopiaObjeto)
				Listado.Add(CopiaObjeto)
			Next
		End If
		Return Listado
	End Function
End Class

Public Class gtkEstructura
	Inherits GertakariakLib2.Entidades.Estructura_DAL
	''' <summary>
	''' Identificador único del registro.
	''' Campo de BUSQUEDA (ESTRUCTURA.ID).
	''' </summary>
	Public Shadows ReadOnly Property Id As Nullable(Of Integer)
		Get
			Return MyBase.Id
		End Get
	End Property

	''' <summary>
	'''     ''' Listado de Nodos relacionados con esta estructura.
	''' </summary>
	''' <remarks></remarks>
	Private _lEstructura As List(Of gtkEstructura)
	''' <summary>
	''' Listado de Sub-Nodos relacionados con esta estructura.
	''' </summary>
	Public ReadOnly Property Nodos As List(Of gtkEstructura)
		Get
			If _lEstructura Is Nothing AndAlso Me.Id IsNot Nothing Then
				'Dim Nodo As New gtkEstructura
				'Nodo.IdIturria = Me.Id
				'_lEstructura = Nodo.Listado()
				_lEstructura = New gtkEstructura() With {.IdIturria = Me.Id}.Listado
			End If
			Return _lEstructura
		End Get
	End Property
	''' <summary>
	''' Obtiene los datos en base al campo ESTRUCTURA.ID.
	''' Si no encuentra ningun resultado el campo ID es NOTHING.
	''' </summary>
	''' <param name="ID">Identificador del Registro.</param>
	''' <remarks></remarks>
	Public Sub Cargar(ByVal ID As Integer)
		Dim ListaObjetos As New List(Of gtkEstructura)
		Dim funcGTK As New GertakariakLib2.Funciones
		MyBase.Id = ID
		ListaObjetos = Me.Listado()
		If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Count = 1 Then
			funcGTK.CopiarPropiedades(ListaObjetos(0), Me)
		Else
			MyBase.Id = Nothing
		End If
	End Sub

	''' <summary>
	''' Eliminar el registro para el identificador indicado.
	''' </summary>
	Public Sub Eliminar()
		Me.Delete()
	End Sub

	''' <summary>
	''' Si el objeto tiene su identificador informado y existe el registro modificara los datos.
	''' En caso contrario realizara un insercion.
	''' </summary>
	''' <remarks></remarks>
	Public Sub Guardar()
		If Me.Id Is Nothing Or Me.Listado() Is Nothing Then Me.Insert() Else Me.Update()
	End Sub

	''' <summary>
	''' Devuelve una lista de "Tipos de Incidencia" para los parametros especificados.
	''' </summary>
	''' <returns>List(Of gtkTipoIncidencia)</returns>
	''' <remarks></remarks>
	Public Function Listado() As List(Of gtkEstructura)
		Dim ListaObjetos As List(Of GertakariakLib2.Entidades.Estructura_DAL) = Me.Load
		Listado = Nothing
		If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Any Then
			Listado = New List(Of gtkEstructura)
			For Each item As GertakariakLib2.Entidades.Estructura_DAL In ListaObjetos
				Dim CopiaObjeto As New gtkEstructura
				Dim f As New Funciones
				f.CopiarPropiedades(item, CopiaObjeto)
				Listado.Add(CopiaObjeto)
			Next
		End If
		Return Listado
	End Function

	''' <summary>
	''' Identificador del Tipo de Incidencia y Tipo de Documento:
	''' Ej.: 6-Sistemas Mantenimiento (gtkMantenimientoSist).
	''' </summary>
	Public Overloads Property idTipoIncidencia As Nullable(Of GertakariakLib2.TipoIncidencia)
		Get
			Return MyBase.IdTipoIncidencia
		End Get
		Set(value As Nullable(Of GertakariakLib2.TipoIncidencia))
			MyBase.IdTipoIncidencia = value
		End Set

	End Property

End Class
Public Class gtkCaracteristica
	Inherits GertakariakLib2.Entidades.Caracteristica_DAL
	''' <summary>
	''' Obtiene los datos en base a los campos ID.
	''' Si no encuentra ningun resultado el campo ID es NOTHING.
	''' </summary>
	''' <param name="IDINCIDENCIA">Identificador de la NC.</param>
	''' <param name="IDESTRUCTURA">Identificador de la Caracteristica.</param>
	''' <remarks></remarks>
	Public Sub Cargar(ByVal IDINCIDENCIA As Integer, ByVal IDESTRUCTURA As Integer)
		'Public Sub Cargar(Optional ByVal IDINCIDENCIA As Nullable(Of Integer) = Nothing, Optional ByVal IDESTRUCTURA As Nullable(Of Integer) = Nothing)
		Dim ListaObjetos As New List(Of gtkCaracteristica)
		Dim funcGTK As New GertakariakLib2.Funciones
		Me.IdIncidencia = IDINCIDENCIA
		Me.IdEstructura = IDESTRUCTURA
		ListaObjetos = Me.Listado()
		If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Count = 1 Then
			funcGTK.CopiarPropiedades(ListaObjetos(0), Me)
		Else
			Me.IdIncidencia = Nothing
			Me.IdEstructura = Nothing
		End If
	End Sub
	''' <summary>
	''' Eliminar el registro para el identificador indicado.
	''' </summary>
	Public Sub Eliminar()
		Me.Delete()
	End Sub
	''' <summary>
	''' Si el objeto tiene su identificador informado y existe el registro modificara los datos.
	''' En caso contrario realizara un insercion.
	''' </summary>
	''' <remarks></remarks>
	Public Sub Guardar()
		If Me.IdEstructura Is Nothing Or Me.IdIncidencia Is Nothing Or Me.Listado() Is Nothing Then Me.Insert() Else Me.Update()
	End Sub
	''' <summary>
	''' Devuelve una lista de las "Caracteristicas" para los parametros especificados.
	''' </summary>
	''' <returns>List(Of gtkCaracteristica)</returns>
	''' <remarks></remarks>
	Public Function Listado() As List(Of gtkCaracteristica)
		Dim ListaObjetos As List(Of GertakariakLib2.Entidades.Caracteristica_DAL) = Me.Load
		Listado = Nothing
		If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Any Then
			Listado = New List(Of gtkCaracteristica)
			For Each item As GertakariakLib2.Entidades.Caracteristica_DAL In ListaObjetos
				Dim CopiaObjeto As New gtkCaracteristica
				Dim f As New Funciones
				f.CopiarPropiedades(item, CopiaObjeto)
				Listado.Add(CopiaObjeto)
			Next
		End If
		Return Listado
	End Function
End Class
<Obsolete("Dejar de usar este objeto. Usar 'LINQ to Entities' (BatzBBDD.ACCIONES)", False)> _
Public Class gtkAcciones
	Inherits GertakariakLib2.Entidades.Acciones_DAL
	''' <summary>
	''' Identificador único del registro.
	''' Campo de BUSQUEDA (ACCIONES.ID).
	''' </summary>
	Public Shadows ReadOnly Property Id As Nullable(Of Integer)
		Get
			Return MyBase.Id
		End Get
	End Property

	Private _IdIncidencia As Nullable(Of Integer)
	Public Property IdIncidencia As Nullable(Of Integer)
		Get
			Return _IdIncidencia
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_IdIncidencia = value
		End Set
	End Property

	''' <summary>
	''' Nº de semanas entre la "FechaApertura" y "FechaCierre" o la fecha actual.
	''' </summary>
	''' <remarks>Consideramos el lunes 1º día de la semana y la 1º semana del año la que tenga 4 dias (= comienza en Jueves).</remarks>
	Public ReadOnly Property Demora As Nullable(Of Long)
		Get
			If MyBase.FechaInicio IsNot Nothing Then
				Demora = DateDiff(DateInterval.WeekOfYear, MyBase.FechaInicio.Value, If(IsDate(MyBase.FechaFin), MyBase.FechaFin.Value, Now), FirstDayOfWeek.Monday, FirstWeekOfYear.FirstFourDays)
			End If
			Return Demora
		End Get
	End Property
	''' <summary>
	''' Obtiene los datos en base al campo ID.
	''' Si no encuentra ningun resultado el campo ID es NOTHING.
	''' </summary>
	''' <param name="ID">Identificador del Registro.</param>
	''' <remarks></remarks>
	Public Sub Cargar(ByVal ID As Integer)
		Dim ListaObjetos As New List(Of gtkAcciones)
		Dim funcGTK As New GertakariakLib2.Funciones
		MyBase.Id = ID
		ListaObjetos = Me.Listado()
		If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Count = 1 Then
			funcGTK.CopiarPropiedades(ListaObjetos(0), Me)
		Else
			MyBase.Id = Nothing
		End If
	End Sub
	''' <summary>
	''' Eliminar el registro para el identificador indicado.
	''' </summary>
	Public Sub Eliminar()
		Me.Delete()
	End Sub
	''' <summary>
	''' Si el objeto tiene su identificador informado y existe el registro modificara los datos.
	''' En caso contrario realizara un insercion.
	''' </summary>
	''' <remarks></remarks>
	Public Sub Guardar()
		If Me.Id Is Nothing Or Me.Listado() Is Nothing Then
			Me.Insert()
			'------------------------------------------------------
			'Relacionamos la Accion con la Incidencia.
			'------------------------------------------------------
			Dim GertakariakAcciones As New gtkGertakariak_Acciones
			GertakariakAcciones.IdAccion = Me.Id
			GertakariakAcciones.IdIncidencia = Me.IdIncidencia
			GertakariakAcciones.Guardar()
			'------------------------------------------------------
		Else
			Me.Update()
		End If
	End Sub
	''' <summary>
	''' Devuelve una lista de "Acciones" para los parametros especificados.
	''' </summary>
	''' <returns>List(Of gtkTipoIncidencia)</returns>
	''' <remarks></remarks>
	Public Function Listado() As List(Of gtkAcciones)
		'------------------------------------------------------------------------------
		'1º- Si el IdIncidencia esta informado Buscamos en la tabla Gertakariak_Acciones
		'si no buscamos en Acciones.
		'------------------------------------------------------------------------------
		Listado = New List(Of gtkAcciones)
		If Me.IdIncidencia Is Nothing Then
			Dim ListaObjetos As List(Of GertakariakLib2.Entidades.Acciones_DAL) = Me.Load
			If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Any Then
				For Each item As GertakariakLib2.Entidades.Acciones_DAL In ListaObjetos
					Dim CopiaObjeto As New gtkAcciones
					Dim f As New Funciones
					f.CopiarPropiedades(item, CopiaObjeto)
					Listado.Add(CopiaObjeto)

					'--------------------------------------------------------------
					'Obtenemos el identificador de la incidencia para esta accion.
					'--------------------------------------------------------------

					'Dim x As Reflection.PropertyInfo
					'x = item.GetType.BaseType.GetProperty("Id", Reflection.BindingFlags.Public Or Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance) '.GetValue(item, Nothing)
					''Dim i As Integer = x.GetValue(item, Nothing)
					'Dim i As Integer = item.GetType.BaseType.GetProperty("Id", Reflection.BindingFlags.Public Or Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance).GetValue(item, Nothing)

					'For Each Propiedad As Reflection.PropertyInfo In item.GetType.BaseType.GetProperties(Reflection.BindingFlags.Public Or Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance)
					'	Dim n As String = Propiedad.Name
					'	Dim v As Integer = Propiedad.GetValue(item, Nothing)
					'	'Propiedad.SetValue(Destino, rPropiedad.GetValue(Origen, Nothing), Nothing)
					'Next

					'For Each Propiedad As Reflection.PropertyInfo In item.GetType.BaseType.GetProperties(Reflection.BindingFlags.Public Or Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance)
					'	If Propiedad.GetSetMethod(True) IsNot Nothing Then
					'		For Each rPropiedad As Reflection.PropertyInfo In item.GetType.GetProperties(Reflection.BindingFlags.Public Or Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance)
					'			If Propiedad.Name = rPropiedad.Name And rPropiedad.CanWrite = True Then
					'				Propiedad.SetValue(item, rPropiedad.GetValue(item, Nothing), Nothing)
					'			End If
					'		Next
					'	End If
					'Next

					'Dim ListadoGA As List(Of gtkGertakariak_Acciones) = New gtkGertakariak_Acciones() With {.IdAccion = MyBase.Id}.Listado
					'If ListadoGA IsNot Nothing AndAlso ListadoGA.Count > 0 Then
					'For Each ga As gtkGertakariak_Acciones In ListadoGA
					'CopiaObjeto.IdIncidencia = ga.IdIncidencia
					'f.CopiarPropiedades(item, CopiaObjeto)
					'Listado.Add(CopiaObjeto)
					'Next
					'End If
					'--------------------------------------------------------------
				Next
			End If
		Else
			Dim ListadoGA As New List(Of gtkGertakariak_Acciones)
			Dim ListadoAcciones As List(Of gtkAcciones) = Nothing
			Dim GA As New gtkGertakariak_Acciones With {.IdAccion = Me.Id, .IdIncidencia = Me.IdIncidencia}
			ListadoGA = GA.Listado

			If ListadoGA IsNot Nothing AndAlso ListadoGA.Any Then
				For Each item As gtkGertakariak_Acciones In ListadoGA
					Dim Accion As New gtkAcciones
					Accion.Cargar(item.IdAccion)
					Accion.IdIncidencia = Me.IdIncidencia
					Listado.Add(Accion)
				Next
			End If
		End If
		'------------------------------------------------------------------------------
		Return Listado
	End Function

	''' <summary>
	''' Responsables de la NC.
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public ReadOnly Property Responsables As List(Of SabLib.ELL.Usuario)
		Get
			If _Responsables Is Nothing OrElse Not _Responsables.Any Then
				Dim gtkRes As New gtkAccionesUsuarios With {.IdAccion = Me.Id}
				Dim f As New SabLib.BLL.UsuariosComponent
				Dim ListaResp As List(Of gtkAccionesUsuarios) = gtkRes.Listado
				_Responsables = Nothing
				If ListaResp IsNot Nothing Then
					_Responsables = New List(Of SabLib.ELL.Usuario)
					For Each item As gtkAccionesUsuarios In ListaResp
						_Responsables.Add(f.GetUsuario(New SabLib.ELL.Usuario With {.Id = item.IdUsuario}, False))
					Next
				End If
			End If
			Return _Responsables
		End Get
	End Property

	''' <summary>
	''' Responsables de la NC.
	''' </summary>
	''' <remarks></remarks>
	Private _Responsables As List(Of SabLib.ELL.Usuario)
End Class
Public Class gtkGertakariak_Acciones
	Inherits GertakariakLib2.Entidades.Gertakariak_Acciones_DAL
	''' <summary>
	''' Obtiene los datos en base a los campos ID.
	''' Si no encuentra ningun resultado el campo ID es NOTHING.
	''' </summary>
	''' <param name="IDINCIDENCIA">Identificador de la NC.</param>
	''' <param name="IDACCION">Identificador de la Accion.</param>
	''' <remarks></remarks>
	Public Sub Cargar(ByVal IDINCIDENCIA As Integer, ByVal IDACCION As Integer)
		Dim ListaObjetos As New List(Of gtkGertakariak_Acciones)
		Dim funcGTK As New GertakariakLib2.Funciones
		Me.IdIncidencia = IDINCIDENCIA
		Me.IdAccion = IDACCION
		ListaObjetos = Me.Listado()
		If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Count = 1 Then
			funcGTK.CopiarPropiedades(ListaObjetos(0), Me)
		Else
			Me.IdIncidencia = Nothing
			Me.IdAccion = Nothing
		End If
	End Sub
	''' <summary>
	''' Eliminar el registro para el identificador indicado.
	''' </summary>
	Public Sub Eliminar()
		Dim Transaccion As New Transaccion
		Try
			If My.Settings.ConexionOracle.State = ConnectionState.Closed Then Transaccion.Abrir()
			'---------------------------------------------------------------------
			'Buscamos las acciones relacionadas con la Incidencia para borrarlas.
			'---------------------------------------------------------------------
			Dim lAccionesIncidencia As New List(Of gtkGertakariak_Acciones)
			Dim Acciones As New gtkGertakariak_Acciones With {.IdIncidencia = Me.IdIncidencia}
			lAccionesIncidencia = Acciones.Listado
			If lAccionesIncidencia IsNot Nothing AndAlso lAccionesIncidencia.Any Then
				'---------------------------------------------------------------------------------------
				'Comprobamos que la accion no esta relacionada con mas incidencias para poder borrarla.
				'---------------------------------------------------------------------------------------
				For Each GerAcc As gtkGertakariak_Acciones In lAccionesIncidencia
					Dim GerAccB As New gtkGertakariak_Acciones With {.IdAccion = GerAcc.IdAccion}
					Dim lGerAcc As List(Of gtkGertakariak_Acciones) = GerAccB.Listado

					If lGerAcc IsNot Nothing AndAlso lGerAcc.Count = 1 Then
						Dim Accion As New gtkAcciones
						Accion.Cargar(GerAcc.IdAccion)
						Accion.Eliminar()
					End If
				Next
			End If
			'---------------------------------------------------------------------
			Me.Delete()
			If Transaccion.Estado = ConnectionState.Open Then Transaccion.Cerrar()
		Catch ex As Exception
			If Transaccion.Estado = ConnectionState.Open Then Transaccion.Rollback()
			throw 
		End Try
	End Sub
	''' <summary>
	''' Si el objeto tiene su identificador informado y existe el registro modificara los datos.
	''' En caso contrario realizara un insercion.
	''' </summary>
	''' <remarks></remarks>
	Public Sub Guardar()
		'If Me.IdAccion Is Nothing Or Me.IdIncidencia Is Nothing Or Me.Listado() Is Nothing Then Me.Insert() Else Me.Update()
		If Me.Listado() Is Nothing Then
			Me.Insert()
		Else
			Throw New ApplicationException("No se permite Modificacion.", New ApplicationException)
		End If
	End Sub
	''' <summary>
	''' Devuelve una lista para los parametros especificados.
	''' </summary>
	''' <returns>List(Of gtkCaracteristica)</returns>
	''' <remarks></remarks>
	Public Function Listado() As List(Of gtkGertakariak_Acciones)
		Dim ListaObjetos As List(Of GertakariakLib2.Entidades.Gertakariak_Acciones_DAL) = Me.Load
		Listado = Nothing
		If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Any Then
			Listado = New List(Of gtkGertakariak_Acciones)
			For Each item As GertakariakLib2.Entidades.Gertakariak_Acciones_DAL In ListaObjetos
				Dim CopiaObjeto As New gtkGertakariak_Acciones
				Dim f As New Funciones
				f.CopiarPropiedades(item, CopiaObjeto)
				Listado.Add(CopiaObjeto)
			Next
		End If
		Return Listado
	End Function
End Class
Public Class gtkAccionesUsuarios
	Inherits GertakariakLib2.Entidades.Acciones_Usuarios_DAL
	''' <summary>
	''' Obtiene los datos en base a los campos ID.
	''' Si no encuentra ningun resultado el campo ID es NOTHING.
	''' </summary>
	''' <param name="IDUSUARIO">Identificador del usuario en SAB.</param>
	''' <param name="IDACCION">Identificador de la Accion.</param>
	''' <remarks></remarks>
	Public Sub Cargar(ByVal IDUSUARIO As Integer, ByVal IDACCION As Integer)
		'Evitar que las funciones de cargar tengan valores "Opcionales" o de tipo "Nullable".
		'Public Sub Cargar(Optional ByVal IDUSUARIO As Nullable(Of Integer) = Nothing, Optional ByVal IDACCION As Nullable(Of Integer) = Nothing)
		Dim ListaObjetos As New List(Of gtkAccionesUsuarios)
		Dim funcGTK As New GertakariakLib2.Funciones
		Me.IdUsuario = IDUSUARIO
		Me.IdAccion = IDACCION
		ListaObjetos = Me.Listado()
		If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Count = 1 Then
			funcGTK.CopiarPropiedades(ListaObjetos(0), Me)
		Else
			Me.IdUsuario = Nothing
			Me.IdAccion = Nothing
		End If
	End Sub
	''' <summary>
	''' Eliminar el registro para el identificador indicado.
	''' </summary>
	Public Sub Eliminar()
		Me.Delete()
	End Sub
	''' <summary>
	''' Si el objeto tiene su identificador informado y existe el registro modificara los datos.
	''' En caso contrario realizara un insercion.
	''' </summary>
	''' <remarks></remarks>
	Public Sub Guardar()
		If Me.Listado() Is Nothing Then
			Me.Insert()
		Else
			Throw New ApplicationException("No se permite Modificacion.", New ApplicationException)
		End If
	End Sub
	''' <summary>
	''' Devuelve una lista de objetos para los parametros especificados.
	''' </summary>
	''' <returns>List(Of gtkAccionesUsuarios)</returns>
	''' <remarks></remarks>
	Public Function Listado() As List(Of gtkAccionesUsuarios)
		Dim ListaObjetos As List(Of GertakariakLib2.Entidades.Acciones_Usuarios_DAL) = Me.Load
		Listado = Nothing
		If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Count > 0 Then
			Listado = New List(Of gtkAccionesUsuarios)
			For Each item As GertakariakLib2.Entidades.Acciones_Usuarios_DAL In ListaObjetos
				Dim CopiaObjeto As New gtkAccionesUsuarios
				Dim f As New Funciones
				f.CopiarPropiedades(item, CopiaObjeto)
				Listado.Add(CopiaObjeto)
			Next
		End If
		Return Listado
	End Function
End Class
Public Class gtkAccionesEjecucion
	Inherits GertakariakLib2.Entidades.Acciones_Ejecucion_DAL
	''' <summary>
	''' Obtiene los datos en base a los campos ID.
	''' Si no encuentra ningun resultado el campo ID es NOTHING.
	''' </summary>
	''' <param name="IDUSUARIO">Identificador del usuario en SAB.</param>
	''' <param name="IDACCION">Identificador de la Accion.</param>
	''' <remarks></remarks>
	Public Sub Cargar(ByVal IDUSUARIO As Integer, ByVal IDACCION As Integer)
		'Evitar que las funciones de cargar tengan valores "Opcionales" o de tipo "Nullable".
		'Public Sub Cargar(Optional ByVal IDUSUARIO As Nullable(Of Integer) = Nothing, Optional ByVal IDACCION As Nullable(Of Integer) = Nothing)
		Dim ListaObjetos As New List(Of gtkAccionesEjecucion)
		Dim funcGTK As New GertakariakLib2.Funciones
		Me.IdUsuario = IDUSUARIO
		Me.IdAccion = IDACCION
		ListaObjetos = Me.Listado()
		If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Count = 1 Then
			funcGTK.CopiarPropiedades(ListaObjetos(0), Me)
		Else
			Me.IdUsuario = Nothing
			Me.IdAccion = Nothing
		End If
	End Sub
	''' <summary>
	''' Eliminar el registro para el identificador indicado.
	''' </summary>
	Public Sub Eliminar()
		Me.Delete()
	End Sub
	''' <summary>
	''' Si el objeto tiene su identificador informado y existe el registro modificara los datos.
	''' En caso contrario realizara un insercion.
	''' </summary>
	''' <remarks></remarks>
	Public Sub Guardar()
		If Me.Listado() Is Nothing Then
			Me.Insert()
		Else
			Throw New ApplicationException("No se permite Modificacion.", New ApplicationException)
		End If
	End Sub
	''' <summary>
	''' Devuelve una lista de objetos para los parametros especificados.
	''' </summary>
	''' <returns>List(Of CopyOfgtkAccionesUsuarios)</returns>
	''' <remarks></remarks>
	Public Function Listado() As List(Of gtkAccionesEjecucion)
		Dim ListaObjetos As List(Of GertakariakLib2.Entidades.Acciones_Ejecucion_DAL) = Me.Load
		Listado = Nothing
		If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Any Then
			Listado = New List(Of gtkAccionesEjecucion)
			For Each item As GertakariakLib2.Entidades.Acciones_Ejecucion_DAL In ListaObjetos
				Dim CopiaObjeto As New gtkAccionesEjecucion
				Dim f As New Funciones
				f.CopiarPropiedades(item, CopiaObjeto)
				Listado.Add(CopiaObjeto)
			Next
		End If
		Return Listado
	End Function
End Class

'Friend puede tener acceso desde dentro del mismo ensamblado, pero no desde fuera del ensamblado.
<Obsolete("No USAR fuera de la libreria.")> _
Public Class gtkFiltro
	''' <summary>
	''' Texto que se va a buscar en diferentes tablas.
	''' </summary>
	Private _descripcion As String
	''' <summary>
	''' Campo para realizar busquedas sobre las Acciones.
	''' </summary>
	Private _FechaPrevista As Nullable(Of Date)
	''' <summary>
	''' Estado de las Incidencias.
	''' </summary>
	Private _Estado As Nullable(Of EstadoIncidencia)
	''' <summary>
	''' Identificadores de los responsables de las Incidencias y Acciones.
	''' </summary>
	Private _Responsables As List(Of Integer)
	''' <summary>
	''' Identificadores de los responsables de las Incidencias y Acciones.
	''' </summary>
	Private _Caracteristicas As List(Of Integer)
	''' <summary>
	''' Identificadores de los responsables de las Incidencias y Acciones.
	''' </summary>
	Private _Activos As List(Of String)

	''' <summary>
	''' Texto que se va a buscar en diferentes tablas.
	''' </summary>
	Public Property Descripcion As String
		Get
			Return _descripcion
		End Get
		Set(ByVal value As String)
			_descripcion = value
		End Set
	End Property

	''' <summary>
	''' Campo para realizar busquedas sobre las Acciones.
	''' Representa la "Fecha Prevista de Fin" de la accion o "Fecha de Revision" de la accion.
	''' </summary>
	<Obsolete("Esta campo se deja de utilizar para usar LINQ.", False)> _
	Public Property FechaPrevista As Nullable(Of Date)
		Get
			Return _FechaPrevista
		End Get
		Set(value As Nullable(Of Date))
			_FechaPrevista = value
		End Set
	End Property

	''' <summary>
	''' Estado de las Incidencias.
	''' </summary>
	''' <value></value>
	Public Property Estado As Nullable(Of EstadoIncidencia)
		Get
			Return _Estado
		End Get
		Set(value As Nullable(Of EstadoIncidencia))
			_Estado = value
		End Set
	End Property

	''' <summary>
	''' Identificadores de los responsables de las Incidencias y Acciones.
	''' </summary>
	Public Property Responsables As List(Of Integer)
		Get
			Return _Responsables
		End Get
		Set(value As List(Of Integer))
			_Responsables = value
		End Set
	End Property

	''' <summary>
	''' Identificadores de caracteristicas o propiedades (gtkEstructuras) que tiene asignada una Incidencia.
	''' </summary>
	Public Property Caracteristicas As List(Of Integer)
		Get
			If _Caracteristicas Is Nothing Then
				_Caracteristicas = New List(Of Integer)
			End If
			Return _Caracteristicas
		End Get
		Set(value As List(Of Integer))
			_Caracteristicas = value
		End Set
	End Property

	''' <summary>
	''' Identificadores de los responsables de las Incidencias y Acciones.
	''' </summary>
	Public Property Activos As List(Of String)
		Get
			If _Activos Is Nothing Then
				_Activos = New List(Of String)
			End If
			Return _Activos
		End Get
		Set(value As List(Of String))
			_Activos = value
		End Set
	End Property

	''' <summary>
	''' Estado de la Incidencia. 
	''' </summary>
	''' <remarks></remarks>
	Public Enum EstadoIncidencia
		''' <summary>
		''' Incidencia SIN "Fecha de Cierre" (GERTAKARIA.FECHACIERRE = NULL)
		''' </summary>
		''' <remarks></remarks>
		Abierta
		''' <summary>
		''' Incidencia CON "Fecha de Cierre" (GERTAKARIA.FECHACIERRE != NULL)
		''' </summary>
		''' <remarks></remarks>
		Cerrada
	End Enum
End Class

Public Class gtkAccionesObservaciones
	Inherits GertakariakLib2.Entidades.Acciones_Observaciones_DAL
	''' <summary>
	''' Identificador único del registro.
	''' Campo de BUSQUEDA (ACCIONES_OBSERVACIONES.ID).
	''' </summary>
	Public Shadows ReadOnly Property Id As Nullable(Of Integer)
		Get
			Return MyBase.Id
		End Get
	End Property

	Private _Usuario As SabLib.ELL.Usuario
	Public ReadOnly Property Usuario As SabLib.ELL.Usuario
		Get
			If Me.IdUsuario IsNot Nothing Then
				Dim eUsuario As New SabLib.ELL.Usuario With {.Id = Me.IdUsuario}
				Dim fUsuario As New SabLib.BLL.UsuariosComponent
				_Usuario = fUsuario.GetUsuario(eUsuario, False)
			End If
			Return _Usuario
		End Get
	End Property

	Private _Usuario2 As SabLib.ELL.Usuario
	Public ReadOnly Property Usuario2 As SabLib.ELL.Usuario
		Get
			If Me.IdUsuario IsNot Nothing Then
				Dim eUsuario As New SabLib.ELL.Usuario With {.Id = Me.IdUsuario}
				Dim fUsuario As New SabLib.BLL.UsuariosComponent
				_Usuario2 = fUsuario.GetUsuario(eUsuario, False)
			End If
			Return _Usuario2
		End Get
	End Property

	''' <summary>
	''' Obtiene los datos en base al campo ID.
	''' Si no encuentra ningun resultado el campo ID es NOTHING.
	''' </summary>
	''' <param name="ID">Identificador del Registro.</param>
	''' <remarks></remarks>
	Public Sub Cargar(ByVal ID As Integer)
		Dim ListaObjetos As New List(Of gtkAccionesObservaciones)
		Dim funcGTK As New GertakariakLib2.Funciones
		MyBase.Id = ID
		ListaObjetos = Me.Listado()
		If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Count = 1 Then
			funcGTK.CopiarPropiedades(ListaObjetos(0), Me)
		Else
			MyBase.Id = Nothing
		End If
	End Sub

	''' <summary>
	''' Eliminar el registro para el identificador indicado.
	''' </summary>
	Public Sub Eliminar()
		Me.Delete()
	End Sub

	''' <summary>
	''' Si el objeto tiene su identificador informado y existe el registro modificara los datos.
	''' En caso contrario realizara un insercion.
	''' </summary>
	''' <remarks></remarks>
	Public Sub Guardar()
		If Me.Id Is Nothing Or Me.Listado() Is Nothing Then Me.Insert() Else Me.Update()
	End Sub

	''' <summary>
	''' Devuelve una lista de "Observaciones" para los parametros especificados en el propio objeto.
	''' </summary>
	''' <returns>List(Of gtkAccionesObservaciones)</returns>
	''' <remarks></remarks>
	Public Function Listado() As List(Of gtkAccionesObservaciones)
		Dim ListaObjetos As List(Of GertakariakLib2.Entidades.Acciones_Observaciones_DAL) = Me.Load
		Listado = Nothing
		If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Any Then
			Listado = New List(Of gtkAccionesObservaciones)
			For Each item As GertakariakLib2.Entidades.Acciones_Observaciones_DAL In ListaObjetos
				Dim CopiaObjeto As New gtkAccionesObservaciones
				Dim f As New Funciones
				f.CopiarPropiedades(item, CopiaObjeto)
				Listado.Add(CopiaObjeto)
			Next
		End If
		Return Listado
	End Function
End Class

Public Class gtkActas_MS
    Inherits GertakariakLib2.Entidades.Actas_MS_DAL
    Public Sub Guardar()
        Me.Insert()
    End Sub
End Class

Public Class gtkFAPERSONAL
	Inherits XBATLib.Entidades.FaPersonal_DAL

	Public Shadows ReadOnly Property CODPER As Nullable(Of Integer)
		Get
			Return MyBase.CODPER
		End Get
	End Property

	''' <summary>
	''' Obtiene los datos en base al campo CODPER.
	''' Si no encuentra ningun resultado el campo CODPER es NOTHING.
	''' </summary>
	''' <param name="CODPER">Identificador del Registro.</param>
	''' <remarks></remarks>
	Public Sub Cargar(ByVal CODPER As Integer)
		Dim ListaObjetos As New List(Of gtkFAPERSONAL)
		Dim funcGTK As New GertakariakLib2.Funciones
		MyBase.CODPER = CODPER
		ListaObjetos = Me.Listado()
		If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Count = 1 Then
			funcGTK.CopiarPropiedades(ListaObjetos(0), Me)
		Else
			MyBase.CODPER = Nothing
		End If
	End Sub

	''' <summary>
	''' Devuelve una lista de personas para los parametros especificados.
	''' </summary>
	''' <returns>List(Of gtkFAPERSONAL)</returns>
	''' <remarks></remarks>
	Public Function Listado() As List(Of gtkFAPERSONAL)
		Dim ListaObjetos As List(Of XBATLib.Entidades.FaPersonal_DAL) = Me.Load
		Listado = Nothing
		If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Any Then
			Listado = New List(Of gtkFAPERSONAL)
			For Each item As XBATLib.Entidades.FaPersonal_DAL In ListaObjetos
				Dim CopiaObjeto As New gtkFAPERSONAL
				Dim f As New Funciones
				f.CopiarPropiedades(item, CopiaObjeto)
				Listado.Add(CopiaObjeto)
			Next
		End If
		Return Listado
	End Function

End Class