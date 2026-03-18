Namespace ELL

    Public Class UsuarioRol

#Region "Variables miembro"

        Private _idSab As Integer = Integer.MinValue
        Private _idRol As Integer = Integer.MinValue

#End Region

#Region "Properties"

        ''' <summary>
        ''' Id de usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdSab() As Integer
            Get
                Return _idSab
            End Get
            Set(ByVal value As Integer)
                _idSab = value
            End Set
        End Property

        ''' <summary>
        ''' Id del rol
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdRol() As Integer
            Get
                Return _idRol
            End Get
            Set(ByVal value As Integer)
                _idRol = value
            End Set
        End Property

#End Region

    End Class

End Namespace
