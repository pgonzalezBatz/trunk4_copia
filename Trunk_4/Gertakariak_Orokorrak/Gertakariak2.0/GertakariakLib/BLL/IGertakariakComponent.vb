Namespace BLL
    Public Interface IGertakariakComponent
        ''' <summary>
        ''' Clase de la incidencia que usamos.
        ''' </summary>
        ''' <remarks></remarks>
        Enum ClaseIncidencia
            gtkTroqueleria
            gtkSerciosGenerales
        End Enum
        ''' <summary>
        ''' Tipo de Orden de Fabricacion.
        ''' </summary>
        Enum TipoORD
            Todas
            Productivas
            NoProductivas
        End Enum


        ''' <summary>
        ''' Funcion para insertar Incidencias.
        ''' </summary>
        ''' <param name="Gertakaria"></param>
        ''' <returns>
        ''' True --&gt; Si la insercion es correcta.
        ''' False --&gt;Si la inserccion es erronea.
        ''' </returns>
        ''' <remarks>No importa de que tipo sea la Incidencia puesto que la funcion la identifica.</remarks>
        Function Insertar(ByVal Gertakaria As Object) As Object
        ''' <summary>
        ''' Consulta de Incidencias.
        ''' </summary>
        ''' <returns>Devuelve una lista de Incidencias</returns>
        Overloads Function Consultar(ByVal Gertakaria As Object) As List(Of Object)
        ''' <summary>
        ''' Consulta de Incidencias.
        ''' </summary>
        ''' <returns>Devuelve una lista de Incidencias</returns>
        Overloads Function Consultar(ByVal tIncidencia As GertakariakLib.BLL.GertakariakComponent.ClaseIncidencia) As List(Of Object)
        ''' <summary>
        ''' Funcion para Modificar incidencias
        ''' </summary>
        ''' <param name="Gertakaria"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Modificar(ByVal Gertakaria As Object) As Object
        ''' <summary>
        ''' Funcion para la eliminacion de Incidencias.
        ''' </summary>
        ''' <param name="Gertakaria"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function Borrar(ByVal Gertakaria As Object) As Object

        Function ValidarOF(ByVal OrdenFabricacion As Integer, ByVal Operacion As Integer, Optional ByVal Productivas As TipoORD = TipoORD.Todas) As Boolean

    End Interface
End Namespace
