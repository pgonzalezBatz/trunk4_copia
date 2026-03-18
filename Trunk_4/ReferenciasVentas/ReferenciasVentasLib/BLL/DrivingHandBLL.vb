Imports Oracle.ManagedDataAccess.Client

Namespace BLL

    Public Class DrivingHandBLL

        Private drivingHandDAL As New DAL.DrivingHandDAL

#Region "Consultas"

        ' ''' <summary>
        ' ''' Obtiene un usuario
        ' ''' </summary>
        ' ''' <param name="idUsuario">Id del usuario</param>        
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function ObtenerUsuario(ByVal idUsuario As Integer) As ELL.Usuarios
        '    Return usuariosDAL.getUsuario(idUsuario)
        'End Function

        ' ''' <summary>
        ' ''' Obtiene un usuario
        ' ''' </summary>
        ' ''' <param name="idUsuario">Id del usuario</param>        
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function ObtenerUsuarioPorId(ByVal idUsuario As Integer) As ELL.Usuarios
        '    Return usuariosDAL.getUsuarioPorId(idUsuario)
        'End Function

        ''' <summary>
        ''' Obtiene un listado de usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarLista() As List(Of ELL.DrivingHand)
            Return drivingHandDAL.loadList()
        End Function

        ''' <summary>
        ''' Obtiene el listado de DrivingHand activos
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarDrivingHandActivos() As List(Of ELL.DrivingHand)
            Return drivingHandDAL.CargarDrivingHandActivos()
        End Function

        ''' <summary>
        ''' Obtiene un DrivingHand
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarDrivingHand(ByVal id As Integer) As ELL.DrivingHand
            Return drivingHandDAL.CargarDrivingHand(id)
        End Function

        ''' <summary>
        ''' Comprobar si una cadena existe en la tabla
        ''' </summary>
        ''' <param name="nombre"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Existe(ByVal nombre As String) As Boolean
            If (drivingHandDAL.existe(nombre) > 0) Then
                Return True
            Else : Return False
            End If
        End Function

        ' ''' <summary>
        ' ''' Obtiene un listado de usuarios
        ' ''' </summary>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function CargarUsuario(ByVal idUsuarioTabla As Integer) As ELL.Usuarios
        '    Return usuariosDAL.getUsuario(idUsuarioTabla)
        'End Function

        ' ''' <summary>
        ' ''' Obtiene un listado de usuarios
        ' ''' </summary>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function CargarUsuariosSAB() As List(Of ELL.Usuarios)
        '    Return usuariosDAL.loadListSAB()
        'End Function

        ' ''' <summary>
        ' ''' Obtiene un usuario
        ' ''' </summary>
        ' ''' <param name="codPersona">Código persona</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>        
        'Public Function ObtenerUsuarioPorCodPersona(ByVal codPersona As Integer) As ELL.Usuarios
        '    Return usuariosDAL.getUsuarioByCodPersona(codPersona)
        'End Function

        ' ''' <summary>
        ' ''' Verifica si un usuario es administrador
        ' ''' </summary>
        ' ''' <param name="codPersona">Código persona</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>        
        'Public Function esUsuarioAdministrador(ByVal codPersona As Integer) As Boolean
        '    Dim usuario As ELL.Usuarios = usuariosDAL.getUsuarioByCodPersona(codPersona)
        '    If (usuario IsNot Nothing) Then
        '        Return True
        '    Else
        '        Return False
        '    End If
        'End Function
#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda un nuevo registro
        ''' </summary>
        ''' <param name="drivingHand">Objeto usuario</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GuardarDrivingHand(ByVal drivingHand As ELL.DrivingHand) As Boolean
            Return drivingHandDAL.Save(drivingHand)
        End Function

        ''' <summary>
        ''' Modifica los datos de un registro
        ''' </summary>
        ''' <param name="drivingHand">Objeto DrivingHand</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ModificarDrivingHand(ByVal drivingHand As ELL.DrivingHand) As Boolean
            Return drivingHandDAL.Update(drivingHand)
        End Function

        ' ''' <summary>
        ' ''' Elimina un registro
        ' ''' </summary>
        ' ''' <param name="idDrivingHand">Identificador del DrivingHand</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function EliminarDrivingHand(ByVal idDrivingHand As Integer) As Boolean
        '    Return drivingHandDAL.Delete(idDrivingHand)
        'End Function

        ' ''' <summary>
        ' ''' 
        ' ''' </summary>
        ' ''' <param name="idUsuario"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function ExisteUsuario(ByVal idUsuario As Integer) As Boolean
        '    If (usuariosDAL.existUsuario(idUsuario) > 0) Then
        '        Return True
        '    Else
        '        Return False
        '    End If
        'End Function

#End Region

    End Class

End Namespace
