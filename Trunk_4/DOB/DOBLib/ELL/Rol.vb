Namespace ELL

    Public Class Rol

#Region "Enumerados"

        ''' <summary>
        ''' Rol del usuario
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum RolUsuario
            Administrador = 1
            Lider_de_objetivos = 2
            Responsable = 3
            Consultor = 4
        End Enum

#End Region

#Region "Properties"

        ''' <summary>
        ''' Id del rol
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Id() As Integer

        ''' <summary>
        ''' Descripción del rol
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Descripcion() As String

#End Region

    End Class

End Namespace
