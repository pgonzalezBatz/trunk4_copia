Namespace ELL

    Public Class HojaGastos

#Region "Variables miembro"

        Private _id As Integer = Integer.MinValue
        Private _idViaje As Integer = Integer.MinValue        
        Private _user As Sablib.ELL.Usuario = Nothing
        Private _estado As eEstado = eEstado.Rellenada        
        Private _lineas As List(Of ELL.HojaGastos.Linea) = Nothing
        Private _movimientosVisa As List(Of ELL.Visa.Movimiento) = Nothing
        Private _userValidador As Sablib.ELL.Usuario = Nothing        
        Private _fechaDesde As Date = Date.MinValue
        Private _fechaHasta As Date = Date.MinValue
        Private _idSinViaje As Integer = Integer.MinValue
        Private _lEstados As List(Of String()) = Nothing

        Public Enum eEstado As Integer
            Rellenada = 1
            Enviada = 2
            Validada = 3
            NoValidada = 4
            Liquidada = 5
            Entregada_Administracion = 6
            Transferida = 7
        End Enum

#End Region

#Region "Properties"

        ''' <summary>
        ''' Identificador unico
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
        ''' Identificador del viaje con el que esta asociado. Puede ser null
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
        ''' Usuario que ha rellenado de la hoja de gastos
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Usuario() As Sablib.ELL.Usuario
            Get
                Return _user
            End Get
            Set(ByVal value As Sablib.ELL.Usuario)
                _user = value
            End Set
        End Property

        ''' <summary>
        ''' Usuario que tendra que validar la hoja de gastos. Sera el responsable
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Validador() As Sablib.ELL.Usuario
            Get
                Return _userValidador
            End Get
            Set(ByVal value As Sablib.ELL.Usuario)
                _userValidador = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si esta rellenada, validada, etc.
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
        ''' Lineas de la hoja de gastos
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Lineas() As List(Of ELL.HojaGastos.Linea)
            Get
                Return _lineas
            End Get
            Set(ByVal value As List(Of ELL.HojaGastos.Linea))
                _lineas = value
            End Set
        End Property

        ''' <summary>
        ''' Movimientos de la visa
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property MovimientosVisa() As List(Of ELL.Visa.Movimiento)
            Get
                Return _movimientosVisa
            End Get
            Set(ByVal value As List(Of ELL.Visa.Movimiento))
                _movimientosVisa = value
            End Set
        End Property

        ''' <summary>
        ''' Si la hoja no esta asociada a un viaje, sera la fecha desde la que tiene efecto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property FechaDesde() As Date
            Get
                Return _fechaDesde
            End Get
            Set(ByVal value As Date)
                _fechaDesde = value
            End Set
        End Property

        ''' <summary>
        ''' Si la hoja no esta asociada a un viaje, sera la fecha hasta la que tiene efecto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property FechaHasta() As Date
            Get
                Return _fechaHasta
            End Get
            Set(ByVal value As Date)
                _fechaHasta = value
            End Set
        End Property

        ''' <summary>
        ''' Identificador autonumerico que se les da a las hojas de gasto sin viaje asociado.Este numero sera el que utilicen para identificar rapidamente una hoja
        ''' Su control esta en el mismo trigger que el del campo ID. Si el viaje es null, se incrementa la secuencia
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property IdSinViaje() As Integer
            Get
                Return _idSinViaje
            End Get
            Set(ByVal value As Integer)
                _idSinViaje = value
            End Set
        End Property

        ''' <summary>
        ''' Comprueba si la hoja actual, tiene algun dia en comun con el rango dado
        ''' </summary>
        ''' <param name="fDesde">Fecha desde de comprobacion</param>
        ''' <param name="fHasta">Fecha hasta de comprobacion</param>
        ''' <returns></returns>            
        Public Function EstaEnElRango(ByVal fDesde As Date, ByVal fHasta As Date) As Boolean
            Return Not ((Me.FechaHasta < fDesde) Or (Me.FechaDesde > fHasta))
        End Function

        ''' <summary>
        ''' Lista con la informacion de los distintos estados por los que ha pasado-> (0):Estado,(1):Fecha
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Estados() As List(Of String())
            Get
                Return _lEstados
            End Get
            Set(ByVal value As List(Of String()))
                _lEstados = value
            End Set
        End Property

        ''' <summary>
        ''' Obtiene la informacion de la ultima aparicion de ese estado
        ''' </summary>
        ''' <param name="est">Estado a buscar</param>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property GetFechaEstado(ByVal est As ELL.HojaGastos.eEstado) As Date
            Get
                Dim fecha As DateTime = Date.MinValue
                Dim lEstados As List(Of String()) = Estados.FindAll(Function(o As String()) CInt(o(0)) = est)
                If (lEstados.Count = 1) Then fecha = CDate(lEstados.Item(0)(1))
                
                Return fecha
            End Get
        End Property

#End Region

        Public Class Linea

#Region "Variables miembro"

            Private _id As Integer = Integer.MinValue
            Private _idHoja As Integer = Integer.MinValue
            Private _usuario As Sablib.ELL.Usuario = Nothing
            Private _concepto As String = String.Empty
            Private _cantidad As Decimal = 0
            Private _moneda As ELL.Moneda = Nothing
            Private _fecha As Date = Date.MinValue
            Private _lugarOrigen As String = String.Empty
            Private _lugarDestino As String = String.Empty
            Private _numKilometros As Decimal = Decimal.MinValue
            Private _tipoGasto As eTipoGasto = Nothing
            Private _conceptoBatz As ELL.Concepto = Nothing
            Private _bRecibo As Boolean = False
            Private _importeEuros As Decimal = 0
            Private _cambioMoneda As Decimal = 0

            Public Enum eTipoGasto As Integer
                Metalico = 1            
                Kilometraje = 2
            End Enum

#End Region

#Region "Properties"

            ''' <summary>
            ''' Identificador unico
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
            ''' Identificador de la cabecera de la hoja
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
            ''' Usuario de la linea
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>        
            Public Property Usuario() As Sablib.ELL.Usuario
                Get
                    Return _usuario
                End Get
                Set(ByVal value As Sablib.ELL.Usuario)
                    _usuario = value
                End Set
            End Property

            ''' <summary>
            ''' Concepto de la linea. Este texto solo se informara cuando el concepto seleccionado, requiera introducir una descripcion
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Concepto() As String
                Get
                    Return _concepto
                End Get
                Set(ByVal value As String)
                    _concepto = value
                End Set
            End Property

            ''' <summary>
            ''' Cantidad de la linea
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Cantidad() As Decimal
                Get
                    Return _cantidad
                End Get
                Set(ByVal value As Decimal)
                    _cantidad = value
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
            ''' Fecha de la linea
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
            ''' Cuando es un gasto de kilometraje, el lugar origen
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>        
            Public Property LugarOrigen() As String
                Get
                    Return _lugarOrigen
                End Get
                Set(ByVal value As String)
                    _lugarOrigen = value
                End Set
            End Property

            ''' <summary>
            ''' Cuando es un gasto de kilometraje, el lugar destino
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>        
            Public Property LugarDestino() As String
                Get
                    Return _lugarDestino
                End Get
                Set(ByVal value As String)
                    _lugarDestino = value
                End Set
            End Property

            ''' <summary>
            ''' Numero de kilometros recorridos
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>        
            Public Property Kilometros() As Decimal
                Get
                    Return _numKilometros
                End Get
                Set(ByVal value As Decimal)
                    _numKilometros = value
                End Set
            End Property

            ''' <summary>
            ''' Tipo de gasto
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property TipoGasto() As eTipoGasto
                Get
                    Return _tipoGasto
                End Get
                Set(ByVal value As eTipoGasto)
                    _tipoGasto = value
                End Set
            End Property

            ''' <summary>
            ''' Concepto de batz seleccionado
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            Public Property ConceptoBatz As ELL.Concepto
                Get
                    Return _conceptoBatz
                End Get
                Set(ByVal value As ELL.Concepto)
                    _conceptoBatz = value
                End Set
            End Property

            ''' <summary>
            ''' Se calcula con el cambio que habia en el dia del movimiento
            ''' </summary>
            Public Property ImporteEuros() As Decimal
                Get
                    If (_importeEuros <= 0) Then
                        If (TipoGasto = eTipoGasto.Metalico) Then
                            Dim hojaBLL As New BLL.HojasGastosBLL
                            Dim xbatBLL As New BLL.XbatBLL
                            Dim myHoja As ELL.HojaGastos = hojaBLL.loadHoja(IdHoja, False)
                            Dim fechaCambio As Date = Fecha
                            If (myHoja.IdViaje > 0) Then
                                Dim anticipBLL As New BLL.AnticiposBLL
                                fechaCambio = anticipBLL.loadAnticipoFechaEntrega(myHoja.IdViaje)
                                If (fechaCambio = DateTime.MinValue) Then fechaCambio = Fecha
                            End If
                            _importeEuros = Math.Round(xbatBLL.ObtenerRateEuros(Me.Moneda.Id, Me.Cantidad, fechaCambio, 0), 2)
                        Else 'Kilometraje: Siempre es en euros
                            _importeEuros = Me.Cantidad
                        End If
                        Return _importeEuros
                    Else
                        Return _importeEuros
                    End If
                End Get
                Set(ByVal value As Decimal)
                    _importeEuros = value
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
            ''' Indica si tiene recibo o no
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            Public Property Recibo As Boolean
                Get
                    Return _bRecibo
                End Get
                Set(ByVal value As Boolean)
                    _bRecibo = value
                End Set
            End Property

            ''' <summary>
            ''' Muestra el cambio de moneda usado para el euro
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property CambioMonedaEUR() As Decimal
                Get
                    Return _cambioMoneda
                End Get
                Set(ByVal value As Decimal)
                    _cambioMoneda = value
                End Set
            End Property

#End Region

        End Class

        <Serializable()>
        Public Class Liquidacion

#Region "Variables miembro"

            Private _usuario As Sablib.ELL.Usuario = Nothing
            Private _cuentaContable As Integer = 0
            Private _importeTotalEuros As Decimal = 0
            Private _tipoLiq As TipoLiq
            Private _hojas As List(Of Liquidacion.Hoja) = Nothing

            ''' <summary>
            ''' Tipos de liquidacion existentes
            ''' </summary>            
            Public Enum TipoLiq As Integer
                Metalico = 0
                Factura = 1
                Comision_Servicios = 2
            End Enum

#End Region

#Region "Properties"

            ''' <summary>
            ''' Usuario de la liquidacion
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Usuario() As Sablib.ELL.Usuario
                Get
                    Return _usuario
                End Get
                Set(ByVal value As Sablib.ELL.Usuario)
                    _usuario = value
                End Set
            End Property

            ''' <summary>
            ''' Importe total en euros de todas las hojas
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property ImporteTotalEuros() As Decimal
                Get
                    Return _importeTotalEuros
                End Get
                Set(ByVal value As Decimal)
                    _importeTotalEuros = value
                End Set
            End Property

            ''' <summary>
            ''' Cuenta contable de la liquidacion asociada al usuario. Es la cuenta sin IVA de su departamento
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property CuentaContable() As Integer
                Get
                    Return _cuentaContable
                End Get
                Set(ByVal value As Integer)
                    _cuentaContable = value
                End Set
            End Property

            ''' <summary>
            ''' Tipo de liquidacion
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property TipoLiquidacion() As TipoLiq
                Get
                    Return _tipoLiq
                End Get
                Set(ByVal value As TipoLiq)
                    _tipoLiq = value
                End Set
            End Property

            ''' <summary>
            ''' Lista de hojas de la liquidacion
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Hojas() As List(Of Liquidacion.Hoja)
                Get
                    Return _hojas
                End Get
                Set(ByVal value As List(Of Liquidacion.Hoja))
                    _hojas = value
                End Set
            End Property

#End Region

            Public Class Cabecera

                Public Property id As Integer = Integer.MinValue
                Public Property FechaEmision As DateTime = DateTime.MinValue                
                Public Property FechaCierre As DateTime = DateTime.MinValue
                Public Property Fichero As Byte() = Nothing
                Public Property IdPlanta As Integer = Integer.MinValue
                Public Property IdPlantaFactura As Integer = Integer.MinValue
                Public Property IdConvCatEmpresaFactura As Integer = Integer.MinValue
                Public Property TipoLiquidacion As TipoLiq = TipoLiq.Metalico

            End Class

            <Serializable()>
            Public Class Hoja

#Region "Variables miembro"

                Private _idHoja As Integer = Integer.MinValue
                Private _idViaje As Integer = Integer.MinValue
                Private _idHojaLibre As Integer = Integer.MinValue
                Private _importeEuros As Decimal = 0
                Private _fValidacion As Date = Date.MinValue
                Private _estado As Integer = 0
                Private _numFactura As String = String.Empty
                Private _entregada As Boolean = False

#End Region

#Region "Properties"

                ''' <summary>
                ''' Id de la hoja
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
                ''' Id del viaje si lo tuviera
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
                ''' Id de la hoja de gastos libre si tuviera
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>		
                Public Property IdHojaLibre() As Integer
                    Get
                        Return _idHojaLibre
                    End Get
                    Set(ByVal value As Integer)
                        _idHojaLibre = value
                    End Set
                End Property

                ''' <summary>
                ''' Importe de la hoja
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>		
                Public Property ImporteEuros() As Decimal
                    Get
                        Return _importeEuros
                    End Get
                    Set(ByVal value As Decimal)
                        _importeEuros = value
                    End Set
                End Property

                ''' <summary>
                ''' Fecha de validacion de la hoja
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>		
                Public Property FechaValidacion() As Date
                    Get
                        Return _fValidacion
                    End Get
                    Set(ByVal value As Date)
                        _fValidacion = value
                    End Set
                End Property

                ''' <summary>
                ''' Estado de la hoja
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>		
                Public Property Estado() As Integer
                    Get
                        Return _estado
                    End Get
                    Set(ByVal value As Integer)
                        _estado = value
                    End Set
                End Property

                ''' <summary>
                ''' Numero de factura
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>		
                Public Property NumFactura() As String
                    Get
                        Return _numFactura
                    End Get
                    Set(ByVal value As String)
                        _numFactura = value
                    End Set
                End Property

                ''' <summary>
                ''' Indica si la hg esta entregada o no
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>		
                Public Property Entregada() As Boolean
                    Get
                        Return _entregada
                    End Get
                    Set(ByVal value As Boolean)
                        _entregada = value
                    End Set
                End Property

#End Region

            End Class

        End Class

    End Class

End Namespace