Imports System.Reflection
Imports IstrikuLib.Funciones


''' <remarks>BLL</remarks>
<Obsolete("No usar esta clase. Usar Entity FrameWork")> Public Class gtkIstriku
    Inherits gtkIstrikuELL

    Private Log As log4net.ILog = log4net.LogManager.GetLogger("root.IstrikuLib")

    ''' <summary>
    ''' Obtiene los datos en base al campo ID.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Cargar()
        Try
            Dim Resultado As List(Of gtkIstriku) = Me.Load(Me)
            Dim Suceso As New gtkIstriku
            If Resultado.Count = 1 Then Suceso = Resultado.Item(0)
            For Each Propiedad As PropertyInfo In Me.GetType.GetProperties
                If Propiedad.GetSetMethod IsNot Nothing Then
                    For Each rPropiedad As PropertyInfo In Suceso.GetType.GetProperties
                        If Propiedad.Name = rPropiedad.Name And rPropiedad.CanWrite = True Then
                            Propiedad.SetValue(Me, rPropiedad.GetValue(Suceso, Nothing), Nothing)
                        End If
                    Next
                End If
            Next
        Catch ex As Exception
            Throw New BatzException("Error al cargar los registros", ex)
            'Throw New Exception("Error al cargar los registros", New System.Exception)
        End Try
    End Sub
    ''' <summary>
    ''' Obtiene los datos en base al campo ID.
    ''' </summary>
    ''' <param name="ID">Identificador del suceso. Propiedad de busqueda.</param>
    ''' <remarks></remarks>
    Public Sub Cargar(ByVal ID As Integer)
        Me.ID = ID
        Me.Cargar()
    End Sub
    ''' <summary>
    ''' Devuelve una lista con todos los objetos.
    ''' </summary>
    ''' <returns>List(Of gtkIstriku)</returns>
    ''' <remarks></remarks>
    Public Function CargarTodo() As List(Of gtkIstriku)
        Return Me.Load
    End Function

    Public Sub Insertar()
        Me.Insert(Me)
    End Sub
    ''' <summary>
    ''' Modifica los datos en base al campo ID.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Modificar()
        Me.Save(Me)
        Cargar()
    End Sub
    ''' <summary>
    ''' Elimina los datos en base al campo ID.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Eliminar()
        Me.Delete(Me)
        Cargar()
    End Sub
End Class
''' <remarks>BLL</remarks>
<Obsolete()> _
Public Class gtkAfectado2
    Inherits gtkAfectadoDAL
    ''' <summary>
    ''' Cargamos el objeto declarado para el Identificador del Afectado en el Suceso (Incidencia).
    ''' </summary>
    ''' <param name="IdAfectadoSuceso">
    ''' Identificador único del Afectado en el Suceso (Incidencia). 
    ''' Campo de BUSQUEDA (DETECCION.ID).
    ''' </param>
    ''' <remarks></remarks>
    Public Sub Cargar(ByVal IdAfectadoSuceso As Integer)
        Me.IdAfectadoSuceso = IdAfectadoSuceso
        Dim Resultado As List(Of gtkAfectado2) = Me.Load()
        If Resultado IsNot Nothing And Resultado.Count = 1 Then
            For Each Propiedad As PropertyInfo In Me.GetType.GetProperties
                If Propiedad.GetSetMethod IsNot Nothing Then
                    For Each rPropiedad As PropertyInfo In Resultado(0).GetType.GetProperties
                        If Propiedad.Name = rPropiedad.Name And rPropiedad.CanWrite = True Then
                            Propiedad.SetValue(Me, rPropiedad.GetValue(Resultado(0), Nothing), Nothing)
                        End If
                    Next
                End If
            Next
        End If
        'Dim Resultado As List(Of gtkIstriku) = Me.Load(Me)
        'Dim Suceso As New gtkIstriku
        'If Resultado.Count = 1 Then Suceso = Resultado.Item(0)
        'For Each Propiedad As PropertyInfo In Me.GetType.GetProperties
        '	If Propiedad.GetSetMethod IsNot Nothing Then
        '		For Each rPropiedad As PropertyInfo In Suceso.GetType.GetProperties
        '			If Propiedad.Name = rPropiedad.Name And rPropiedad.CanWrite = True Then
        '				Propiedad.SetValue(Me, rPropiedad.GetValue(Suceso, Nothing), Nothing)
        '			End If
        '		Next
        '	End If
        'Next
    End Sub
    ''' <summary>
    ''' Listado de Afectados. 
    ''' Obtiene los datos en base a los campos de busqueda rellenados en el objeto.
    ''' </summary>
    ''' <remarks></remarks>
    Public Function Cargar() As List(Of gtkAfectado2)
        Cargar = Me.Load()
        Return Cargar
    End Function
    ''' <summary>
    ''' Modificamos el objeto para el edentificador unico del registro.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Modificar()
        Me.Save()
    End Sub
End Class
Public Class gtkAfectado
    Inherits GertakariakLib2.Entidades.Deteccion_DAL

    ''' <summary>
    ''' Identificador único del Afectado en el Suceso (Incidencia).
    ''' Campo de BUSQUEDA (DETECCION.ID).
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdAfectadoSuceso As Nullable(Of Integer)
        Get
            Return Id
        End Get
        Set(ByVal value As Nullable(Of Integer))
            Id = value
        End Set
    End Property

    Public Property EstadoParte As EstadoParte
        Get
            Return If(IdDepartamento Is Nothing, EstadoParte.Pendiente, IdDepartamento)
        End Get
        Set(ByVal value As EstadoParte)
            IdDepartamento = value
        End Set
    End Property

    ''' <summary>
    ''' Carga el objeto en curso para los parametros indicados en sus propiedades.
    ''' </summary>
    ''' <remarks></remarks>
    Function Cargar()
        Dim Resultado As List(Of gtkAfectado) = Me.Lista
        If Resultado IsNot Nothing AndAlso Resultado.Count = 1 Then
            Dim f As New Funciones
            f.CopiarPropiedades(Resultado(0), Me)
            Return True
        Else
            Return False
        End If
    End Function
    ''' <summary>
    ''' Carga el objeto en curso para los parametros indicados en sus propiedades.
    ''' </summary>
    ''' <param name="IdAfectadoSuceso">Identificador único del Afectado en el Suceso (Incidencia). Campo de BUSQUEDA (DETECCION.ID).</param>
    ''' <remarks></remarks>
    Sub Cargar(ByRef IdAfectadoSuceso As Integer)
        Me.Id = IdAfectadoSuceso
        Me.Cargar()
    End Sub
    ''' <summary>
    ''' Devuelve una lista de Afectados para los parametros especificados.
    ''' </summary>
    ''' <returns>List(Of gtkAfectado)</returns>
    ''' <remarks></remarks>
    Public Function Lista() As List(Of gtkAfectado)
        Dim lDeteccion As List(Of GertakariakLib2.Entidades.Deteccion_DAL) = Me.Load
        Lista = Nothing
        If lDeteccion IsNot Nothing AndAlso lDeteccion.Count > 0 Then
            Lista = New List(Of gtkAfectado)
            For Each item As GertakariakLib2.Entidades.Deteccion_DAL In lDeteccion
                Dim Afectado As New gtkAfectado
                Dim f As New Funciones
                f.CopiarPropiedades(item, Afectado)
                Lista.Add(Afectado)
            Next
        End If
        Return Lista
    End Function
    Public Sub Modificar()
        Me.Save()
    End Sub

    Public Sub Eliminar()
        Me.Delete()
    End Sub
End Class

Public Class Funciones
    ''' <summary>
    ''' Proceso para el copiado de los valores de las propiedades de un objeto en otro.
    ''' Cargamos los datos de los campos coincidentes del objeto Origen al Destino.
    ''' </summary>
    ''' <param name="Origen">Objeto donde 'OBTENEMOS' los valores.</param>
    ''' <param name="Destino">Objeto donde 'CARGAMOS' los valores.</param>
    ''' <remarks></remarks>
    Public Sub CopiarPropiedades(ByVal Origen As Object, ByVal Destino As Object)
        '------------------------------------------------------------------------------
        'Cargamos los datos de los campos coincidentes del objeto Origen al Destino.
        '------------------------------------------------------------------------------
        For Each Propiedad As PropertyInfo In Destino.GetType.GetProperties(BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)
            If Propiedad.GetSetMethod(True) IsNot Nothing Then
                For Each rPropiedad As PropertyInfo In Origen.GetType.GetProperties(BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)
                    If Propiedad.Name = rPropiedad.Name And rPropiedad.CanWrite = True Then
                        Propiedad.SetValue(Destino, rPropiedad.GetValue(Origen, Nothing), Nothing)
                    End If
                Next
            End If
        Next
        '------------------------------------------------------------------------------
    End Sub
End Class