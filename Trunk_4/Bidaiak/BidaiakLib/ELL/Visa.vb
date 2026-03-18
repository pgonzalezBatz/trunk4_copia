Namespace ELL

    Public Class Visa

#Region "Variables miembro"

        Private _id As Integer = Integer.MinValue
        Private _numTarjeta As String = String.Empty
        Private _user As Sablib.ELL.Usuario = Nothing
        Private _fechaEntrega As Date = Date.MinValue
        Private _obsoleta As Boolean = False
        Private _idPlanta As Integer = Integer.MinValue

#End Region

#Region "Properties"

        ''' <summary>
        ''' Identificador
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
        ''' Numero de la tarjeta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property NumTarjeta() As String
            Get
                Return _numTarjeta
            End Get
            Set(ByVal value As String)
                _numTarjeta = value
            End Set
        End Property

        ''' <summary>
        ''' Usuario propietario de la visa
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Propietario() As Sablib.ELL.Usuario
            Get
                Return _user
            End Get
            Set(ByVal value As Sablib.ELL.Usuario)
                _user = value
            End Set
        End Property

        ''' <summary>
        ''' Fecha en la que se le entrega la tarjeta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property FechaEntrega() As Date
            Get
                Return _fechaEntrega
            End Get
            Set(ByVal value As Date)
                _fechaEntrega = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si esta obsoleta o no
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Obsoleta() As Boolean
            Get
                Return _obsoleta
            End Get
            Set(ByVal value As Boolean)
                _obsoleta = value
            End Set
        End Property

        ''' <summary>
        ''' Identificador de la planta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property IdPlanta() As Integer
            Get
                Return _idPlanta
            End Get
            Set(ByVal value As Integer)
                _idPlanta = value
            End Set
        End Property

#End Region

        Public Class Movimiento
            Implements IComparer(Of Movimiento)

#Region "Variables miembro"

            Private _id As Integer = Integer.MinValue
            Private _nombreUsuario As String = String.Empty
            Private _numTarjeta As String = String.Empty
            Private _sector As String = String.Empty
            Private _establecimiento As String = String.Empty
            Private _localidad As String = String.Empty
            Private _fecha As Date = Date.MinValue
            Private _moneda As Moneda = Nothing
            Private _importe As Decimal = 0
            Private _idViaje As Integer = Integer.MinValue
            Private _idHoja As Integer = Integer.MinValue
            Private _idPlanta As Integer = Integer.MinValue
            Private _IdResponsable As Integer = Integer.MinValue
            Private _IdUsuario As Integer = Integer.MinValue
            Private _estado As eEstado = eEstado.Cargado
            Private _idImportacion As Integer = Integer.MinValue
            Private _comentarios As String = String.Empty
            Private _monedaGasto As Moneda = Nothing
            Private _importeMonedaGasto As Decimal = 0
            Private _tipo As Integer = 0
            Private _cuenta As Integer = 0
            Private _lantegi As String = String.Empty

            Public Enum eEstado As Integer
                Cargado = 1
                Conforme = 2
                No_Conforme = 3
                Liquidado = 4
                Justificado = 5
            End Enum

#End Region

#Region "Properties"

            ''' <summary>
            ''' Id
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
            ''' Nombre del usuario
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property NombreUsuario() As String
                Get
                    Return _nombreUsuario
                End Get
                Set(ByVal value As String)
                    _nombreUsuario = value
                End Set
            End Property

            ''' <summary>
            ''' Numero de la tarjeta
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property NumTarjeta() As String
                Get
                    Return _numTarjeta
                End Get
                Set(ByVal value As String)
                    _numTarjeta = value
                End Set
            End Property

            ''' <summary>
            ''' Sector
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Sector() As String
                Get
                    Return _sector
                End Get
                Set(ByVal value As String)
                    _sector = value
                End Set
            End Property

            ''' <summary>
            ''' Establecimiento
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Establecimiento() As String
                Get
                    Return _establecimiento
                End Get
                Set(ByVal value As String)
                    _establecimiento = value
                End Set
            End Property

            ''' <summary>
            ''' Localidad
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Localidad() As String
                Get
                    Return _localidad
                End Get
                Set(ByVal value As String)
                    _localidad = value
                End Set
            End Property

            ''' <summary>
            ''' Fecha
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Fecha() As Date
                Get
                    Return _fecha
                End Get
                Set(ByVal value As Date)
                    _fecha = value
                End Set
            End Property

            ''' <summary>
            ''' Moneda
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Moneda() As ELL.Moneda
                Get
                    Return _moneda
                End Get
                Set(ByVal value As ELL.Moneda)
                    _moneda = value
                End Set
            End Property

            ''' <summary>
            ''' Moneda del gasto
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property MonedaGasto() As ELL.Moneda
                Get
                    Return _monedaGasto
                End Get
                Set(ByVal value As ELL.Moneda)
                    _monedaGasto = value
                End Set
            End Property

            ''' <summary>
            ''' Importe
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property ImporteEuros() As Decimal
                Get
                    Return _importe
                End Get
                Set(ByVal value As Decimal)
                    _importe = value
                End Set
            End Property

            ''' <summary>
            ''' Importe
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property ImporteMonedaGasto() As Decimal
                Get
                    Return _importeMonedaGasto
                End Get
                Set(ByVal value As Decimal)
                    _importeMonedaGasto = value
                End Set
            End Property

            ''' <summary>
            ''' Identificador del viaje
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property IdViaje() As Integer
                Get
                    Return _idViaje
                End Get
                Set(ByVal value As Integer)
                    _idViaje = value
                End Set
            End Property

            ''' <summary>
            ''' Identificador de la hoja libre con la que esta enlazada
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property IdHoja() As Integer
                Get
                    Return _idHoja
                End Get
                Set(ByVal value As Integer)
                    _idHoja = value
                End Set
            End Property

            ''' <summary>
            ''' Identificador de la planta
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property IdPlanta() As Integer
                Get
                    Return _idPlanta
                End Get
                Set(ByVal value As Integer)
                    _idPlanta = value
                End Set
            End Property

            ''' <summary>
            ''' Dada una cultura y el formato de los decimales, devuelve el decimal con la coma o punto
            ''' </summary>
            ''' <param name="sDec">Numero a convertir</param>
            ''' <returns></returns>	
            Private Shared Function DecimalValue(ByVal sDec As String) As Decimal
                If (Not String.IsNullOrEmpty(sDec)) Then
                    Dim myDec As String = String.Empty
                    If (Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ",") Then
                        myDec = sDec.Trim.Replace(".", ",")
                    ElseIf (Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".") Then
                        myDec = sDec.Trim.Replace(",", ".")
                    End If
                    myDec = If(myDec = String.Empty, "0", myDec)
                    Return Convert.ToDecimal(myDec, Threading.Thread.CurrentThread.CurrentCulture.NumberFormat)
                Else
                    Return 0
                End If
            End Function

            ''' <summary>
            ''' Id de la persona del gasto
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property IdUsuario() As Integer
                Get
                    Return _IdUsuario
                End Get
                Set(ByVal value As Integer)
                    _IdUsuario = value
                End Set
            End Property

            ''' <summary>
            ''' Id del responsable de la persona
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property IdResponsable() As Integer
                Get
                    Return _IdResponsable
                End Get
                Set(ByVal value As Integer)
                    _IdResponsable = value
                End Set
            End Property

            ''' <summary>
            ''' Estado de la linea
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Estado() As eEstado
                Get
                    Return _estado
                End Get
                Set(ByVal value As eEstado)
                    _estado = value
                End Set
            End Property

            ''' <summary>
            ''' Id de la importacion
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property IdImportacion() As String
                Get
                    Return _idImportacion
                End Get
                Set(ByVal value As String)
                    _idImportacion = value
                End Set
            End Property

            ''' <summary>
            ''' Comentarios puestos por el usuario a modo de explicacion
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Comentarios() As String
                Get
                    Return _comentarios
                End Get
                Set(ByVal value As String)
                    _comentarios = value
                End Set
            End Property

            ''' <summary>
            ''' Tipo de movimiento. 0:Gasto,1:Visas
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Tipo() As String
                Get
                    Return _tipo
                End Get
                Set(ByVal value As String)
                    _tipo = value
                End Set
            End Property

            ''' <summary>
            ''' Cuenta contable. Suele estar informada solamente para las excepciones
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Cuenta() As Integer
                Get
                    Return _cuenta
                End Get
                Set(ByVal value As Integer)
                    _cuenta = value
                End Set
            End Property

            ''' <summary>
            ''' Lantegi. Suele estar informada solamente para las excepciones
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Lantegi() As String
                Get
                    Return _lantegi
                End Get
                Set(ByVal value As String)
                    _lantegi = value
                End Set
            End Property

#End Region

#Region "Sort"

            ''' <summary>
            ''' Metodo para la ordenacion por usuario y por fecha
            ''' </summary>
            ''' <param name="x"></param>
            ''' <param name="y"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Function CompareUserNameAndData(ByVal x As Movimiento, ByVal y As Movimiento) As Integer Implements System.Collections.Generic.IComparer(Of Movimiento).Compare
                If x.NombreUsuario <= y.NombreUsuario Then
                    If (x.Fecha < y.Fecha) Then
                        Return 1
                    Else
                        Return 0
                    End If
                Else
                    Return 0
                End If

            End Function
#End Region

        End Class

    End Class

End Namespace