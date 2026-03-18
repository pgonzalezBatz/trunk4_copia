Namespace ELL

	Public Class Anticipo

#Region "Variables miembro"

		Private _idViaje As Integer = Integer.MinValue
		Private _fechaNecesidad As Date = Date.MinValue
		Private _estado As EstadoAnticipo = EstadoAnticipo.solicitado		
        Private _lMovimientos As List(Of ELL.Anticipo.Movimiento) = Nothing
        Private _lEstados As List(Of String()) = Nothing        
        Private _eurosPendientesHojasGastosLiq As Decimal = 0

        ''' <summary>
        ''' Estados posibles de un anticipo
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum EstadoAnticipo As Integer
            cancelada = 0
            solicitado = 1
            '            Tramitando = 2 200818:Se quita
            Preparado = 3
            Entregado = 4
            cerrado = 5
        End Enum

#End Region

#Region "Properties"

        ''' <summary>
        ''' Identificador del viaje. Puede que no tenga
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdViaje() As Integer
            Get
                Return _idViaje
            End Get
            Set(ByVal value As Integer)
                _idViaje = value
            End Set
        End Property

        ''' <summary>
        ''' Fecha para la cual se necesita el importe
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property FechaNecesidad() As Date
            Get
                Return _fechaNecesidad
            End Get
            Set(ByVal value As Date)
                _fechaNecesidad = value
            End Set
        End Property

        ''' <summary>
        ''' Estado del anticipo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Estado() As EstadoAnticipo
            Get
                Return _estado
            End Get
            Set(ByVal value As EstadoAnticipo)
                _estado = value
            End Set
        End Property

        ''' <summary>
        ''' Devuelve la lista de movimientos del anticipo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Movimientos() As List(Of ELL.Anticipo.Movimiento)
            Get
                Return _lMovimientos
            End Get
            Set(ByVal value As List(Of ELL.Anticipo.Movimiento))
                _lMovimientos = value
            End Set
        End Property

        ''' <summary>
        ''' Devuelve las lineas de anticipos solicitados
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public ReadOnly Property AnticiposSolicitados() As List(Of ELL.Anticipo.Movimiento)
            Get
                Dim lAnticipos As List(Of ELL.Anticipo.Movimiento) = Nothing
                If (Movimientos IsNot Nothing) Then
                    lAnticipos = Movimientos.FindAll(Function(o As ELL.Anticipo.Movimiento) o.TipoMov = Movimiento.TipoMovimiento.Solicitado)
                End If
                Return lAnticipos
            End Get
        End Property

        ''' <summary>
        ''' Devuelve las lineas de anticipos recibidos
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Private ReadOnly Property AnticiposRecibidos() As List(Of ELL.Anticipo.Movimiento)
            Get
                Dim lAnticipos As List(Of ELL.Anticipo.Movimiento) = Nothing
                If (Movimientos IsNot Nothing) Then
                    lAnticipos = Movimientos.FindAll(Function(o As ELL.Anticipo.Movimiento) o.TipoMov = Movimiento.TipoMovimiento.Entregado)
                End If
                Return lAnticipos
            End Get
        End Property

        ''' <summary>
        ''' Devuelve las lineas de entrega
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public ReadOnly Property Entregas() As List(Of ELL.Anticipo.Movimiento)
            Get
                Dim lEntregas As List(Of ELL.Anticipo.Movimiento) = Nothing
                If (Movimientos IsNot Nothing) Then
                    lEntregas = Movimientos.FindAll(Function(o As ELL.Anticipo.Movimiento) o.TipoMov = Movimiento.TipoMovimiento.Entregado)
                End If
                Return lEntregas
            End Get
        End Property

        ''' <summary>
        ''' Devuelve las lineas de devoluciones
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public ReadOnly Property Devoluciones() As List(Of ELL.Anticipo.Movimiento)
            Get
                Dim lDevoluciones As List(Of ELL.Anticipo.Movimiento) = Nothing
                If (Movimientos IsNot Nothing) Then
                    lDevoluciones = Movimientos.FindAll(Function(o As ELL.Anticipo.Movimiento) o.TipoMov = Movimiento.TipoMovimiento.Devolucion)
                End If
                Return lDevoluciones
            End Get
        End Property

        ''' <summary>
        ''' Devuelve las lineas de transferencias
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public ReadOnly Property Transferencias() As List(Of ELL.Anticipo.Movimiento)
            Get
                Dim lTransferencias As List(Of ELL.Anticipo.Movimiento) = Nothing
                If (Movimientos IsNot Nothing) Then
                    lTransferencias = Movimientos.FindAll(Function(o As ELL.Anticipo.Movimiento) o.TipoMov = Movimiento.TipoMovimiento.Transferencia)
                End If
                Return lTransferencias
            End Get
        End Property

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
        ''' Calcula los euros solicitados        
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property EurosSolicitados() As Decimal
            Get
                Return AnticiposSolicitados.Sum(Function(o As ELL.Anticipo.Movimiento) o.ImporteEuros)
                'For Each mov As ELL.Anticipo.Movimiento In AnticiposSolicitados                
                'euros += mov.ImporteEuros
                'Next                
            End Get
        End Property

        ''' <summary>
        ''' Calcula los euros que se le han entregado        
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property EurosEntregados() As Decimal
            Get
                If (AnticiposRecibidos IsNot Nothing) Then
                    Return AnticiposRecibidos.Sum(Function(o As ELL.Anticipo.Movimiento) o.ImporteEuros)
                Else
                    Return 0
                End If
            End Get
        End Property

        ''' <summary>
        ''' Calcula los euros que faltan por pagar
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property EurosPendientes As Decimal
            Get
                Dim euros As Decimal = 0
                For Each mov As ELL.Anticipo.Movimiento In Movimientos
                    Select Case mov.TipoMov
                        Case Movimiento.TipoMovimiento.Entregado
                            'euros += mov.ConversionEuros
                            euros += mov.ImporteEuros
                        Case Movimiento.TipoMovimiento.Devolucion
                            'euros -= mov.ConversionEuros
                            euros -= mov.ImporteEuros
                        Case Movimiento.TipoMovimiento.Transferencia
                            If (mov.IdViajeOrigen = IdViaje) Then
                                'euros -= mov.ConversionEuros
                                euros -= mov.ImporteEuros
                            Else
                                'euros += mov.ConversionEuros
                                euros += mov.ImporteEuros
                            End If
                        Case Movimiento.TipoMovimiento.Diferencia_Cambio
                            euros += mov.Cantidad  'La diferencia de cambio siempre es en euros y tiene un signo                        
                        Case Movimiento.TipoMovimiento.Manual
                            euros += mov.Cantidad  'El movimiento manual siempre es en euros y tiene un signo                        
                    End Select
                Next
                Return euros
            End Get
        End Property

        ''' <summary>
        ''' Calcula los euros pendientes de la hoja de gastos del liquidador de un anticipo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property EurosPendientesHojaGastosLiq() As Decimal
            Get
                Return _eurosPendientesHojasGastosLiq
            End Get
            Set(ByVal value As Decimal)
                _eurosPendientesHojasGastosLiq = value
            End Set
        End Property

        ''' <summary>
        ''' Clona el objeto en uno nuevo
        ''' </summary>
        ''' <returns></returns>        
        Public Function Clone() As Anticipo
            Dim oAnticNew As New Anticipo
            oAnticNew.IdViaje = IdViaje
            oAnticNew.Estado = Estado
            oAnticNew.EurosPendientesHojaGastosLiq = EurosPendientesHojaGastosLiq
            oAnticNew.FechaNecesidad = FechaNecesidad
            If (Estados IsNot Nothing AndAlso Estados.Count > 0) Then
                oAnticNew.Estados = New List(Of String())
                For Each sEstado As String() In Estados
                    oAnticNew.Estados.Add(New String() {sEstado(0), sEstado(1)})
                Next
            End If
            If (Movimientos IsNot Nothing AndAlso Movimientos.Count > 0) Then
                oAnticNew.Movimientos = New List(Of ELL.Anticipo.Movimiento)
                For Each oMov As ELL.Anticipo.Movimiento In Movimientos
                    oAnticNew.Movimientos.Add(oMov.Clone)
                Next
            End If
            Return oAnticNew
        End Function

#End Region

        <Serializable()> _
        Public Class Movimiento

#Region "Variables miembro"

            Private _id As Integer = Integer.MinValue
            Private _idAnt As Integer = Integer.MinValue
            Private _moneda As ELL.Moneda = Nothing
            Private _cantidad As Decimal = 0
            Private _fecha As Date = Date.MinValue
            Private _userOrigen As Sablib.ELL.Usuario = Nothing
            Private _userDestino As Sablib.ELL.Usuario = Nothing
            Private _idViajeOrig As Integer = Integer.MinValue
            Private _idViajeDest As Integer = Integer.MinValue
            Private _comentario As String = String.Empty
            Private _tipoMov As TipoMovimiento = TipoMovimiento.Solicitado
            Private _importeEuros As Decimal = 0
            Private _cambioMoneda As Decimal = 0

            ''' <summary>
            ''' Enumeracion para saber el tipo de movimiento
            ''' </summary>		
            Public Enum TipoMovimiento As Integer
                Solicitado = 1
                Entregado = 2
                Devolucion = 3
                Transferencia = 4
                Diferencia_Cambio = 5
                Hoja_Gastos = 6
                Manual = 7
            End Enum

#End Region

#Region "Properties"

            ''' <summary>
            ''' Identificador del movimiento
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
            ''' Identificador del anticipo al que pertenece
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property IdAnticipo() As Integer
                Get
                    Return _idAnt
                End Get
                Set(ByVal value As Integer)
                    _idAnt = value
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
            ''' Cantidad/Importe del movimiento
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
            ''' La primera vez, es decir cuando no se haya insertado todavia, calculara el importe en euros de la informacion de la moneda:
            '''    Obtiene el valor en euros de la cantidad
            '''    Se dividira el valor del importe por el rate especificado de la moneda
            ''' Una vez que ya haya sido guardada, devolvera el importe en euros correspondiente al dia en que se inserto el movimiento
            ''' </summary>
            Public Property ImporteEuros() As Decimal
                Get
                    If (_importeEuros <= 0) Then
                        Dim anticipBLL As New BLL.AnticiposBLL
                        Dim fechaCambio As Date = anticipBLL.loadAnticipoFechaEntrega(IdAnticipo)
                        If (fechaCambio = DateTime.MinValue) Then fechaCambio = Fecha
                        Dim xbatBLL As New BLL.XbatBLL
                        Dim cambioUtilizado As Decimal = 0
                        _importeEuros = Math.Round(xbatBLL.ObtenerRateEuros(Me.Moneda.Id, Me.Cantidad, fechaCambio, cambioUtilizado), 2)
                        If (_cambioMoneda = 0) Then _cambioMoneda = cambioUtilizado
                    End If
                    Return _importeEuros
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
            ''' Cuando el tipo de movimiento sea una devolucion, indicara el usuario que la ha realizado.
            ''' Sin embargo, cuando sea una transferencia, sera el usuario que la ha transferido		
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property UserOrigen() As Sablib.ELL.Usuario
                Get
                    Return _userOrigen
                End Get
                Set(ByVal value As Sablib.ELL.Usuario)
                    _userOrigen = value
                End Set
            End Property

            ''' <summary>
            ''' Cuando el tipo de movimiento sea una devolucion, indicara la fecha de devolucion
            ''' Sin embargo, cuando sea una transferencia, sera la fecha de la transferencia
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
            ''' Cuando es de tipo transferencia, indica el usuario que ha recibido el anticipo
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property UserDestino() As Sablib.ELL.Usuario
                Get
                    Return _userDestino
                End Get
                Set(ByVal value As Sablib.ELL.Usuario)
                    _userDestino = value
                End Set
            End Property

            ''' <summary>
            ''' Cuando es de tipo transferencia, indica el id del viaje de origen de la misma
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property IdViajeOrigen() As Integer
                Get
                    Return _idViajeOrig
                End Get
                Set(ByVal value As Integer)
                    _idViajeOrig = value
                End Set
            End Property

            ''' <summary>
            ''' Cuando es de tipo transferencia, indica el id del viaje de destino de la misma
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property IdViajeDestino() As Integer
                Get
                    Return _idViajeDest
                End Get
                Set(ByVal value As Integer)
                    _idViajeDest = value
                End Set
            End Property

            ''' <summary>
            ''' Comentario del movimiento
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Comentarios() As String
                Get
                    Return _comentario
                End Get
                Set(ByVal value As String)
                    _comentario = value
                End Set
            End Property

            ''' <summary>
            ''' Indica el tipo de movimiento (anticipo solicitado, devolucion, transferencia)
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property TipoMov() As TipoMovimiento
                Get
                    Return _tipoMov
                End Get
                Set(ByVal value As TipoMovimiento)
                    _tipoMov = value
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

            ''' <summary>
            ''' Clona el objeto en uno nuevo
            ''' </summary>
            ''' <returns></returns>        
            Public Function Clone() As Movimiento
                Dim oMovNew As New Movimiento
                oMovNew.Id = Me.Id
                oMovNew.Cantidad = Me.Cantidad
                oMovNew.Comentarios = Me.Comentarios
                oMovNew.Fecha = Me.Fecha
                oMovNew.IdAnticipo = Me.IdAnticipo
                oMovNew.IdViajeDestino = Me.IdViajeDestino
                oMovNew.IdViajeOrigen = Me.IdViajeOrigen
                If (Me.Moneda IsNot Nothing) Then oMovNew.Moneda = Me.Moneda.Clone
                oMovNew.TipoMov = Me.TipoMov
                oMovNew.UserDestino = Me.UserDestino
                oMovNew.UserOrigen = Me.UserOrigen
                oMovNew.ImporteEuros = Me.ImporteEuros
                oMovNew.CambioMonedaEUR = Me.CambioMonedaEUR
                Return oMovNew
            End Function

#End Region

        End Class

    End Class

End Namespace