Namespace ELL

    Public Class Roles

#Region "Enumerados"

        ''' <summary>
        ''' Rol del usuario
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum RolUsuario
            Administrador = 2
            Administrador2 = 12
            Recepcion = 3
            Financiero = 4
            RRHH = 5
            Prevencion = 21
            Consultor = 12
            Usuario = 11
            Extranet = 99
        End Enum

        Public Enum RolUsuarioTodos
            Todos = 0
            Administrador = 1
            HelpDesk = 2
            IKS = 3
            Usuario = 4

        End Enum
#End Region

        Private _Id As Integer
        Private _Nombre As String

        Public Property Id() As Integer
            Get
                Return _Id
            End Get
            Set(ByVal value As Integer)
                _Id = value
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