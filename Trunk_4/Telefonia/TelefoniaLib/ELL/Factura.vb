Namespace ELL

    Public Class Factura

        Private _numFactura As String = String.Empty
        Private _fechaFactura As DateTime = DateTime.MinValue
        Private _total As Decimal = 0
        Private _descuento As Decimal = 0
        Private _iva As Decimal = 0
        Private _totalPagar As Decimal = 0
        Private _lineas As List(Of ELL.LineaFactura)
        Private _cifEmpresa As String = String.Empty
        Private _idPlanta As Integer = Integer.MinValue
        Private _nombrePlanta As String = String.Empty
        Private _idImportacion As Integer = 0
        Private _numLineas As Integer = 0

#Region "Properties"

        Public Property NumFactura() As String
            Get
                Return _numFactura
            End Get
            Set(ByVal value As String)
                _numFactura = value
            End Set
        End Property

        Public Property FechaFactura() As DateTime
            Get
                Return _fechaFactura
            End Get
            Set(ByVal value As DateTime)
                _fechaFactura = value
            End Set
        End Property

        Public Property Total() As Decimal
            Get
                Return _total
            End Get
            Set(ByVal value As Decimal)
                _total = value
            End Set
        End Property

        Public Property Descuento() As Decimal
            Get
                Return _descuento
            End Get
            Set(ByVal value As Decimal)
                _descuento = value
            End Set
        End Property

        Public Property IVA() As Decimal
            Get
                Return _iva
            End Get
            Set(ByVal value As Decimal)
                _iva = value
            End Set
        End Property

        Public Property TotalPagar() As Decimal
            Get
                Return _totalPagar
            End Get
            Set(ByVal value As Decimal)
                _totalPagar = value
            End Set
        End Property

        Public Property CifEmpresa() As String
            Get
                Return _cifEmpresa
            End Get
            Set(ByVal value As String)
                _cifEmpresa = value
            End Set
        End Property

        Public Property Lineas() As List(Of ELL.LineaFactura)
            Get
                Return _lineas
            End Get
            Set(ByVal value As List(Of ELL.LineaFactura))
                _lineas = value
            End Set
        End Property

        Public Property IdPlanta() As Integer
            Get
                Return _idPlanta
            End Get
            Set(ByVal value As Integer)
                _idPlanta = value
            End Set
        End Property

        Public Property NombrePlanta() As String
            Get
                Return _nombrePlanta
            End Get
            Set(ByVal value As String)
                _nombrePlanta = value
            End Set
        End Property

        Public Property IdImportacion() As Integer
            Get
                Return _idImportacion
            End Get
            Set(ByVal value As Integer)
                _idImportacion = value
            End Set
        End Property

        Public Property NumLineas() As Integer
            Get
                Return _numLineas
            End Get
            Set(ByVal value As Integer)
                _numLineas = value
            End Set
        End Property

#End Region

    End Class

End Namespace