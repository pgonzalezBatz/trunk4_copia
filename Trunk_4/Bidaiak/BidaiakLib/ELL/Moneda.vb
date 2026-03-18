Namespace ELL

    <Serializable()> _
    Public Class Moneda

#Region "Variables miembro"
        Public Shared EURO As String = "EUR"

        Private _id As Integer = Integer.MinValue
        Private _nombre As String = String.Empty
        Private _rate As Decimal = 0
        Private _abreviatura As String = String.Empty
        Private _codISO As String = String.Empty

#End Region

#Region "Properties"

        ''' <summary>
        ''' Id de la moneda
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        ''' <summary>
        ''' Nombre de la moneda (Euro,Dolar Usa,etc...)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

        ''' <summary>
        ''' Valor de la moneda en euros en el dia actual
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property ConversionEuros() As Decimal
            Get
                Return _rate
            End Get
            Set(ByVal value As Decimal)
                _rate = value
            End Set
        End Property

        ''' <summary>
        ''' Abreviatura de la moneda(p.e. Euro es EUR, Libra es GBP)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Abreviatura() As String
            Get
                Return _abreviatura
            End Get
            Set(ByVal value As String)
                _abreviatura = value
            End Set
        End Property

        ''' <summary>
        ''' Codigo ISO 4207 de la moneda
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property CodISO() As String
            Get
                Return _codISO
            End Get
            Set(ByVal value As String)
                _codISO = value
            End Set
        End Property

        ''' <summary>
        ''' Clona el objeto en uno nuevo
        ''' </summary>
        ''' <returns></returns>        
        Public Function Clone() As Moneda
            Return New Moneda With {.Id = Me.Id, .Nombre = Me.Nombre, .Abreviatura = Me.Abreviatura, .ConversionEuros = Me.ConversionEuros, .CodISO = Me.CodISO}
        End Function

#End Region

    End Class

End Namespace