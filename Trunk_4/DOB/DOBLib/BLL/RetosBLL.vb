Imports DOBLib.DAL

Namespace BLL

    Public Class RetosBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un reto
        ''' </summary>
        ''' <param name="idReto">Id del reto</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Shared Function ObtenerReto(ByVal idReto As Integer) As ELL.Reto
            Return RetosDAL.getReto(idReto)
        End Function

        ''' <summary>
        ''' Obtiene un listado de roles
        ''' </summary>
        ''' <param name="idPlanta"></param>
        ''' <param name="plantaDeBaja"></param>
        ''' <param name="nombre"></param> 
        ''' <param name="descripcion"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado(Optional ByVal idPlanta As Integer? = Nothing, Optional ByVal plantaDeBaja As Boolean? = False, Optional ByVal nombre As String = "", Optional ByVal descripcion As String = "", Optional ByVal retoDeBaja? As Boolean = Nothing) As List(Of ELL.Reto)
            Dim idPlantaAux As Integer = BLL.PlantasBLL.ObtenerPlantaPadre(idPlanta)

            Return RetosDAL.loadList(idPlantaAux, plantaDeBaja, nombre, descripcion, retoDeBaja)
        End Function

        ''' <summary>
        ''' Comprueba si existe una planta
        ''' </summary>
        ''' <param name="idPlanta"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ExistePlanta(ByVal idPlanta As Integer) As Boolean
            Return RetosDAL.existsPlanta(idPlanta)
        End Function

        ''' <summary>
        ''' Comprueba si existe el codigo
        ''' </summary>
        ''' <param name="codigo"></param>
        ''' <param name="idPlanta"></param>
        ''' <param name="idReto"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ExisteCodigo(ByVal codigo As String, ByVal idPlanta As Integer, Optional ByVal idReto As Integer? = Nothing) As Boolean
            Return RetosDAL.existsCodigo(codigo, idPlanta, idReto)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda un reto
        ''' </summary>
        ''' <param name="reto">Reto</param>    
        ''' <param name="buffer">Fichero</param> 
        Public Shared Sub Guardar(ByVal reto As ELL.Reto, ByVal buffer() As Byte)
            RetosDAL.Save(reto, buffer)
        End Sub

        ''' <summary>
        ''' Da de baja un reto
        ''' </summary>
        ''' <param name="idReto">Id del rol</param>
        ''' <param name="idUsuario">Id del ususario</param>
        ''' <remarks></remarks>
        Public Shared Sub DarDeBaja(ByVal idReto As Integer, ByVal idUsuario As Integer)
            RetosDAL.Unsubscribe(idReto, idUsuario)
        End Sub

        ''' <summary>
        ''' Da de alta un reto
        ''' </summary>
        ''' <param name="idReto">Id del rol</param> 
        ''' <remarks></remarks>
        Public Shared Sub DarDeAlta(ByVal idReto As Integer)
            RetosDAL.Subscribe(idReto)
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina un reto
        ''' </summary>
        ''' <param name="idReto">Id del rol</param> 
        ''' <remarks></remarks>
        Public Shared Sub Eliminar(ByVal idReto As Integer)
            RetosDAL.DeleteReto(idReto)
        End Sub

#End Region

    End Class

End Namespace