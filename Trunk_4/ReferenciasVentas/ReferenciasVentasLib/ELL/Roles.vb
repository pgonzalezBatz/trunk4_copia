Namespace ELL

    Public Class Roles

#Region "Enumerados"

        ''' <summary>
        ''' Rol del usuario
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum RolUsuario
            ProductEngineer = 1
            Administrador = 2
            ProjectLeader = 7
            DocumentationTechnician = 21
            ProjectLeaderManager = 245
            ProductManager = 265
        End Enum

#End Region

        Private _IdRol As Integer
        Private _Nombre As String

        Public Property IdRol() As Integer
            Get
                Return _IdRol
            End Get
            Set(ByVal value As Integer)
                _IdRol = value
            End Set
        End Property

        Public Property Nombre() As String
            Get
                Return _Nombre
            End Get
            Set(ByVal value As String)
                _Nombre = value
            End Set
        End Property

    End Class
End Namespace