Namespace ELL
    Public Class cultura

#Region "VARIABLES MIEMBRO"
        Private _id As String
        Private _idioma As String
        Private _region As String
#End Region

#Region "CONSTRUCTOR"
        Public Sub New()
            _id = ""
            _idioma = ""
            _region = ""
        End Sub
#End Region


#Region "PROPERTIES"
        Public Property Id() As String
            Get
                Return _id
            End Get
            Set(ByVal value As String)
                _id = value
            End Set
        End Property

        Public Property Idioma() As String
            Get
                Return _idioma
            End Get
            Set(ByVal value As String)
                _idioma = value
            End Set
        End Property

        Public Property Region() As String
            Get
                Return _region
            End Get
            Set(ByVal value As String)
                _region = value
            End Set
        End Property

#End Region


    End Class
End Namespace
