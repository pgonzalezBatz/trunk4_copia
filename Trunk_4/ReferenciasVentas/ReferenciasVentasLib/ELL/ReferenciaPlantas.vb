Namespace ELL

    Public Class ReferenciaPlantas

#Region "Variables miembro"

        Private _IdReferencia As Integer = Integer.MinValue
        Private _IdPlanta As String = String.Empty

#End Region

#Region "Properties"

        ''' <summary>
        ''' Id de la referencia
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdReferencia() As Integer
            Get
                Return _IdReferencia
            End Get
            Set(ByVal value As Integer)
                _IdReferencia = value
            End Set
        End Property

        ''' <summary>
        ''' Id de la planta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdPlanta() As String
            Get
                Return _IdPlanta
            End Get
            Set(ByVal value As String)
                _IdPlanta = value
            End Set
        End Property

#End Region

    End Class

End Namespace

