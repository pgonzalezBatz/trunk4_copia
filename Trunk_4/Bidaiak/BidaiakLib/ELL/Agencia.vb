Namespace ELL

	Public Class SolicitudAgencia

#Region "Variables miembro"

		Private _idViaje As Integer = Integer.MinValue
		Private _comentarioUser As String = String.Empty
        Private _estado As EstadoAgencia = EstadoAgencia.solicitado
        Private _validada As Boolean = False
        Private _fechaLimiteTarifas As Date = Date.MinValue
        Private _lServiciosSol As List(Of ELL.SolicitudAgencia.Linea) = Nothing
        Private _lAlbaranes As List(Of String)
        Private _presupuesto As ELL.Presupuesto = Nothing

        ''' <summary>
        ''' Estados posibles de un servicio solicitado a una agencia
        ''' </summary>
        Public Enum EstadoAgencia As Integer
            cancelada = 0
            solicitado = 1
            Tramite = 2
            Gestionado = 3
            Cerrada = 4
        End Enum

#End Region

#Region "Properties"

        ''' <summary>
        ''' Id del viaje a traves del cual, se relaciona una solicitud de viaje con unos servicios de agencia
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
        ''' Comentarios del usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property ComentariosUsuario() As String
            Get
                Return _comentarioUser
            End Get
            Set(ByVal value As String)
                _comentarioUser = value
            End Set
        End Property

        ''' <summary>
        ''' Comentarios puestos por el responsable de la agencia
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public ReadOnly Property ComentariosDeLaAgencia() As String
            Get
                Dim comen As String = String.Empty
                If (ServiciosSolicitados IsNot Nothing) Then
                    For Each serv As Linea In ServiciosSolicitados
                        If (serv.Comentario.Trim <> String.Empty) Then
                            comen &= If(comen <> String.Empty, "<hr />", String.Empty)
                            comen &= If(serv.ServicioAgencia Is Nothing, "Otras opciones".ToUpper, serv.ServicioAgencia.Nombre.ToUpper) & "<br />" & serv.Comentario.Trim
                        End If
                    Next
                End If

                Return comen.Replace(vbCrLf, "<br />")
            End Get
        End Property

        ''' <summary>
        ''' Estado de la solicitud de agencia
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Estado As EstadoAgencia
            Get
                Return _estado
            End Get
            Set(ByVal value As EstadoAgencia)
                _estado = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si esta validada por un responsable o no
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Validada() As Boolean
            Get
                Return _validada
            End Get
            Set(ByVal value As Boolean)
                _validada = value
            End Set
        End Property

        ''' <summary>
        ''' Indica la fecha limite de vigencia de las tarifas puestas por el de la agencia
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        Public Property FechaLimiteTarifas As Date
            Get
                Return _fechaLimiteTarifas
            End Get
            Set(ByVal value As Date)
                _fechaLimiteTarifas = value
            End Set
        End Property

        ''' <summary>
        ''' Servicios de agencia solicitados
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property ServiciosSolicitados() As List(Of ELL.SolicitudAgencia.Linea)
            Get
                Return _lServiciosSol
            End Get
            Set(ByVal value As List(Of ELL.SolicitudAgencia.Linea))
                _lServiciosSol = value
            End Set
        End Property

        ''' <summary>
        ''' Lista de albaranes
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Albaranes() As List(Of String)
            Get
                Return _lAlbaranes
            End Get
            Set(ByVal value As List(Of String))
                _lAlbaranes = value
            End Set
        End Property

        ''' <summary>
        ''' Presupuesto de agencia
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Presupuesto() As ELL.Presupuesto
            Get
                Return _presupuesto
            End Get
            Set(ByVal value As ELL.Presupuesto)
                _presupuesto = value
            End Set
        End Property

        ''' <summary>
        ''' Clona el objeto en uno nuevo
        ''' </summary>
        ''' <returns></returns>        
        Public Function Clone() As SolicitudAgencia
            Dim oSolNew As New SolicitudAgencia
            oSolNew.IdViaje = IdViaje
            oSolNew.ComentariosUsuario = ComentariosUsuario
            oSolNew.Estado = Estado
            oSolNew.FechaLimiteTarifas = FechaLimiteTarifas
            oSolNew.Validada = Validada
            If (Albaranes IsNot Nothing AndAlso Albaranes.Count > 0) Then
                oSolNew.Albaranes = New List(Of String)
                For Each sAlb As String In Albaranes
                    oSolNew.Albaranes.Add(sAlb)
                Next
            End If
            If (ServiciosSolicitados IsNot Nothing AndAlso ServiciosSolicitados.Count > 0) Then
                oSolNew.ServiciosSolicitados = New List(Of Linea)
                For Each lin As Linea In ServiciosSolicitados
                    oSolNew.ServiciosSolicitados.Add(lin.Clone)
                Next
            End If
            If (Presupuesto IsNot Nothing) Then oSolNew.Presupuesto = Presupuesto.Clone
            Return oSolNew
        End Function

#End Region

        <Serializable()> _
        Public Class Linea

#Region "Variables miembro"

            Private _id As Integer = Integer.MinValue
            Private _idViaje As Integer = Integer.MinValue
            Private _serviceAgencia As ServicioDeAgencia = Nothing
            Private _coste As Decimal = 0
            Private _moneda As ELL.Moneda = Nothing
            Private _comentario As String = String.Empty
            Private _tipo As TipoLinea = TipoLinea.Servicios
            Private _idUserReq As Integer = Integer.MinValue
            Private _conNavegador As Boolean = False

            Public Enum TipoLinea As Integer
                Servicios = 1
                Otras_Opciones = 2
                Otros_Gastos = 3
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
            ''' Id del viaje
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
            ''' Servicio de agencia solicitado (p.ej:hotel, avion, etc..)
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property ServicioAgencia() As ServicioDeAgencia
                Get
                    Return _serviceAgencia
                End Get
                Set(ByVal value As ServicioDeAgencia)
                    _serviceAgencia = value
                End Set
            End Property

            ''' <summary>
            ''' Coste del servicio asignado por el de la agencia
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Coste() As Decimal
                Get
                    Return _coste
                End Get
                Set(ByVal value As Decimal)
                    _coste = value
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
            ''' Comentario del servicio de agencia
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Comentario() As String
                Get
                    Return _comentario
                End Get
                Set(ByVal value As String)
                    _comentario = value
                End Set
            End Property

            ''' <summary>
            ''' Indicara el tipo de la linea
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>        
            Public Property Tipo() As TipoLinea
                Get
                    Return _tipo
                End Get
                Set(ByVal value As TipoLinea)
                    _tipo = value
                End Set
            End Property

            ''' <summary>
            ''' Identificador del usuario requerido para dicho servicio
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property IdUserReq() As Integer
                Get
                    Return _idUserReq
                End Get
                Set(ByVal value As Integer)
                    _idUserReq = value
                End Set
            End Property

            ''' <summary>
            ''' Indica si se pide navegador GPS o no
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property NavegadorGPS() As Integer
                Get
                    Return _conNavegador
                End Get
                Set(ByVal value As Integer)
                    _conNavegador = value
                End Set
            End Property

            ''' <summary>
            ''' Clona el objeto en uno nuevo
            ''' </summary>
            ''' <returns></returns>        
            Public Function Clone() As Linea
                Dim oLineaNew As New Linea
                oLineaNew.Comentario = Comentario
                oLineaNew.Coste = Coste
                oLineaNew.Id = Id
                oLineaNew.IdViaje = IdViaje
                If (Moneda IsNot Nothing) Then oLineaNew.Moneda = Moneda.Clone                
                If (ServicioAgencia IsNot Nothing) Then oLineaNew.ServicioAgencia = ServicioAgencia.Clone
                oLineaNew.IdUserReq = IdUserReq
                oLineaNew.NavegadorGPS = NavegadorGPS
                oLineaNew.Tipo = Tipo
                Return oLineaNew
            End Function

#End Region

        End Class

    End Class

    <Serializable()> _
    Public Class ServicioDeAgencia
        Inherits MantBasico

#Region "Variables miembro"

        Private _reqUsuario As Boolean = False

        Public Enum TipoServicioAgencia
            Avion = 1
            Hotel = 2
            Coche_Alquiler = 3
        End Enum

#End Region

#Region "Properties"

        ''' <summary>
        ''' Indica si requiere que se seleccione un usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property RequiereUsuario() As Boolean
            Get
                Return _reqUsuario
            End Get
            Set(ByVal value As Boolean)
                _reqUsuario = value
            End Set
        End Property

        ''' <summary>
        ''' Clona el objeto en uno nuevo
        ''' </summary>
        ''' <returns></returns>        
        Public Overloads Function Clone() As ServicioDeAgencia
            Dim oServNew As New ServicioDeAgencia
            oServNew.Id = Id
            oServNew.Nombre = Nombre
            oServNew.Descripcion = Descripcion
            oServNew.Obsoleto = Obsoleto
            oServNew.IdPlanta = IdPlanta
            oServNew.RequiereUsuario = RequiereUsuario            
            Return oServNew
        End Function

#End Region

    End Class

    Public Class Presupuesto

#Region "Variables miembro"

        Private _idViaje As Integer = Integer.MinValue
        Private _fLimiteEmision As Date = Date.MinValue
        Private _observacion As String = String.Empty
        Private _observacionVal As String = String.Empty
        Private _estado As EstadoPresup = EstadoPresup.Creado
        Private _idUserRespuesta As Integer = Integer.MinValue
        Private _idUserResponsable As Integer = Integer.MinValue
        Private _integrantes As List(Of ELL.Viaje.Integrante) = Nothing
        Private _servicios As List(Of Servicio) = Nothing
        Private _estados As List(Of HistoricoEstado) = Nothing
        Private _presupNuevo As Boolean = False

        ''' <summary>
        ''' Posibles estados del presupuesto
        ''' </summary>        
        Public Enum EstadoPresup As Integer
            Generado = 4
            Creado = 0
            Enviado = 1
            Validado = 2
            Rechazado = 3
        End Enum

#End Region

#Region "Properties"

        ''' <summary>
        ''' Identificador unico
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
        ''' Fecha limite de la emision
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property FechaLimiteEmision() As Date
            Get
                Return _fLimiteEmision
            End Get
            Set(ByVal value As Date)
                _fLimiteEmision = value
            End Set
        End Property

        ''' <summary>
        ''' Observaciones
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Observaciones() As String
            Get
                Return _observacion
            End Get
            Set(ByVal value As String)
                _observacion = value
            End Set
        End Property

        ''' <summary>
        ''' Observaciones
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property ObservacionesValidador() As String
            Get
                Return _observacionVal
            End Get
            Set(ByVal value As String)
                _observacionVal = value
            End Set
        End Property

        ''' <summary>
        ''' Estado del presupuesto
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
        ''' Identificador del usuario que ha dado una respuesta, la ha aceptado, la ha denegado
        ''' </summary>
        ''' <value></value>        
        Public Property IdUsuarioRespuesta() As Integer
            Get
                Return _idUserRespuesta
            End Get
            Set(ByVal value As Integer)
                _idUserRespuesta = value
            End Set
        End Property

        ''' <summary>
        ''' Identificador del usuario responsable que ha de validar el presupuesto
        ''' </summary>
        ''' <value></value>        
        Public Property IdUsuarioResponsable() As Integer
            Get
                Return _idUserResponsable
            End Get
            Set(ByVal value As Integer)
                _idUserResponsable = value
            End Set
        End Property

        ''' <summary>
        ''' Lista de los integrantes asociados al presupuesto
        ''' </summary>
        ''' <value></value>        
        Public Property Integrantes() As List(Of ELL.Viaje.Integrante)
            Get
                Return _integrantes
            End Get
            Set(ByVal value As List(Of ELL.Viaje.Integrante))
                _integrantes = value
            End Set
        End Property

        ''' <summary>
        ''' Lista de los servicios asociados al presupuesto
        ''' </summary>
        ''' <value></value>        
        Public Property Servicios() As List(Of Servicio)
            Get
                Return _servicios
            End Get
            Set(ByVal value As List(Of Servicio))
                _servicios = value
            End Set
        End Property

        ''' <summary>
        ''' Lista de los estados por los que ha pasado
        ''' </summary>
        ''' <value></value>        
        Public Property Estados() As List(Of HistoricoEstado)
            Get
                Return _estados
            End Get
            Set(ByVal value As List(Of HistoricoEstado))
                _estados = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si el presupuesto es nuevo (nuevo formato a partir de abril 2019) o es de los antiguos
        ''' </summary>
        ''' <returns></returns>
        Public Property PresupuestoNuevo As Boolean
            Get
                Return _presupNuevo
            End Get
            Set(value As Boolean)
                _presupNuevo = value
            End Set
        End Property

        ''' <summary>
        ''' Clona el objeto en uno nuevo
        ''' </summary>
        ''' <returns></returns>        
        Public Function Clone() As Presupuesto
            Dim PresupNew As New Presupuesto
            PresupNew.IdViaje = IdViaje
            PresupNew.FechaLimiteEmision = FechaLimiteEmision
            PresupNew.Observaciones = _observacion
            PresupNew.Estado = Estado
            PresupNew.IdUsuarioResponsable = IdUsuarioResponsable
            PresupNew.IdUsuarioRespuesta = IdUsuarioRespuesta
            PresupNew.PresupuestoNuevo = PresupuestoNuevo
            If (Integrantes IsNot Nothing AndAlso Integrantes.Count > 0) Then
                PresupNew.Integrantes = New List(Of ELL.Viaje.Integrante)
                For Each oInt As ELL.Viaje.Integrante In Integrantes
                    PresupNew.Integrantes.Add(oInt)
                Next
            End If
            If (Servicios IsNot Nothing AndAlso Servicios.Count > 0) Then
                PresupNew.Servicios = New List(Of Servicio)
                For Each serv As Servicio In Servicios
                    PresupNew.Servicios.Add(serv.Clone)
                Next
            End If
            Return PresupNew
        End Function

#End Region

        Public Class Servicio

#Region "Variables miembro"

            Private _id As Integer = Integer.MinValue
            Private _idViaje As Integer = Integer.MinValue
            Private _tipo As Tipo_Servicio
            Private _ciudad1 As String = String.Empty
            Private _ciudad2 As String = String.Empty
            Private _fecha1 As DateTime = DateTime.MinValue
            Private _fecha2 As DateTime = DateTime.MinValue
            Private _tarifaReal As Decimal = 0
            Private _tipoHabitacion As Tipo_Habitacion = Tipo_Habitacion.DUI
            Private _regimen As eRegimen = eRegimen.Alojamiento_y_desayuno
            Private _categoria As String = String.Empty
            Private _clase As String = String.Empty
            Private _nombre As String = String.Empty
            Private _numPlan As Integer = Integer.MinValue
            Private _idTarifaDestino As Integer = Integer.MinValue
            Private _tarifaObj As Decimal = 0
            Private _numDias As Integer = 0


            Public Enum Tipo_Servicio As Integer
                Avion = 0
                Hotel = 1
                Tren = 2
                Coche_Alquiler = 3
            End Enum

            ''' <summary>
            ''' Tipo de habitacion
            ''' </summary>
            Public Enum Tipo_Habitacion As Integer
                DUI = 0
                Doble = 1
                Individual = 2
            End Enum

            ''' <summary>
            ''' Regimen de una habitacion
            ''' </summary>            
            Public Enum eRegimen As Integer
                Alojamiento_y_desayuno = 0
                Solo_alojamiento = 1
            End Enum

#End Region

#Region "Properties"

            ''' <summary>
            ''' Identificador del servicio
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
            ''' Identificador del viaje con el que esta relacionado
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
            ''' Tipo del servicio. Indicara si es de avion, tren, etc..
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property TipoServicio() As Tipo_Servicio
                Get
                    Return _tipo
                End Get
                Set(ByVal value As Tipo_Servicio)
                    _tipo = value
                End Set
            End Property

            ''' <summary>
            ''' Primera ciudad
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Ciudad1() As String
                Get
                    Return _ciudad1
                End Get
                Set(ByVal value As String)
                    _ciudad1 = value
                End Set
            End Property

            ''' <summary>
            ''' Segunda ciudad
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Ciudad2() As String
                Get
                    Return _ciudad2
                End Get
                Set(ByVal value As String)
                    _ciudad2 = value
                End Set
            End Property

            ''' <summary>
            ''' Primera fecha y hora
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Fecha1() As DateTime
                Get
                    Return _fecha1
                End Get
                Set(ByVal value As DateTime)
                    _fecha1 = value
                End Set
            End Property

            ''' <summary>
            ''' Segunda fecha y hora
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Fecha2() As DateTime
                Get
                    Return _fecha2
                End Get
                Set(ByVal value As DateTime)
                    _fecha2 = value
                End Set
            End Property

            ''' <summary>
            ''' Tipo habitacion
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property TipoHabitacion() As Tipo_Habitacion
                Get
                    Return _tipoHabitacion
                End Get
                Set(ByVal value As Tipo_Habitacion)
                    _tipoHabitacion = value
                End Set
            End Property

            ''' <summary>
            ''' Regimen de dietas en el hotel
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Regimen() As eRegimen
                Get
                    Return _regimen
                End Get
                Set(ByVal value As eRegimen)
                    _regimen = value
                End Set
            End Property

            ''' <summary>
            ''' Tarifa que se le ha aplicado
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property TarifaReal() As Decimal
                Get
                    Return _tarifaReal
                End Get
                Set(ByVal value As Decimal)
                    _tarifaReal = value
                End Set
            End Property

            ''' <summary>
            ''' Campo categoria
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Categoria() As String
                Get
                    Return _categoria
                End Get
                Set(ByVal value As String)
                    _categoria = value
                End Set
            End Property

            ''' <summary>
            ''' Campo clase
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Clase() As String
                Get
                    Return _clase
                End Get
                Set(ByVal value As String)
                    _clase = value
                End Set
            End Property

            ''' <summary>
            ''' Campo nombre
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
            ''' Numero del plan al que pertenece
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property NumeroPlan() As Integer
                Get
                    Return _numPlan
                End Get
                Set(ByVal value As Integer)
                    _numPlan = value
                End Set
            End Property

            ''' <summary>
            ''' Id de la tarifa de destino
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property IdTarifaDestino() As Integer
                Get
                    Return _idTarifaDestino
                End Get
                Set(ByVal value As Integer)
                    _idTarifaDestino = value
                End Set
            End Property

            ''' <summary>
            ''' Numero de dias de uso del servicio. Hotel (nº noches),  coches alquiler (nº dias)
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property NumeroDias() As Integer
                Get
                    Return _numDias
                End Get
                Set(ByVal value As Integer)
                    _numDias = value
                End Set
            End Property

            ''' <summary>
            ''' Tarifa objetivo que se guarda cuando esta aprobado
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property TarifaObjetivo() As Decimal
                Get
                    Return _tarifaObj
                End Get
                Set(ByVal value As Decimal)
                    _tarifaObj = value
                End Set
            End Property

            ''' <summary>
            ''' Clona el objeto en uno nuevo
            ''' </summary>
            ''' <returns></returns>        
            Public Function Clone() As Servicio
                Dim servNew As New Servicio
                servNew.Id = Id : servNew.IdViaje = IdViaje
                servNew.TipoServicio = TipoServicio
                servNew.Ciudad1 = Ciudad1 : servNew.Ciudad2 = Ciudad2
                servNew.Fecha1 = Fecha1 : servNew.Fecha2 = Fecha2
                servNew.TarifaReal = TarifaReal : servNew.TipoHabitacion = TipoHabitacion
                servNew.Regimen = Regimen : servNew.Categoria = Categoria
                servNew.Clase = Clase : servNew.Nombre = Nombre
                servNew.NumeroPlan = NumeroPlan : servNew.IdTarifaDestino = IdTarifaDestino
                servNew.NumeroDias = NumeroDias
                servNew.TarifaObjetivo = TarifaObjetivo
                Return servNew
            End Function

            ''' <summary>
            ''' Obtiene la tarifa objetivo total de los PRESUPUESTOS NUEVOS
            ''' </summary>
            ''' <param name="numInteg">Numero de integrantes</param>
            ''' <returns></returns>
            Public ReadOnly Property TarifaObjetivoTotal(ByVal numInteg As Integer)
                Get
                    Dim tarifa As Decimal = 0
                    Select Case TipoServicio
                        Case Presupuesto.Servicio.Tipo_Servicio.Avion
                            tarifa = TarifaObjetivo * numInteg
                        Case Presupuesto.Servicio.Tipo_Servicio.Hotel
                            tarifa = (TarifaObjetivo * NumeroDias * numInteg)
                        Case Presupuesto.Servicio.Tipo_Servicio.Coche_Alquiler
                            tarifa = (TarifaObjetivo * NumeroDias)
                    End Select
                    Return tarifa
                End Get
            End Property

            ''' <summary>
            ''' Obtiene la tarifa real total de los PRESUPUESTOS NUEVOS
            ''' </summary>
            ''' <param name="numInteg">Numero de integrantes</param>
            ''' <returns></returns>
            Public ReadOnly Property TarifaRealTotal(ByVal numInteg As Integer)
                Get
                    Dim tarifa As Decimal = 0
                    Select Case TipoServicio
                        Case Presupuesto.Servicio.Tipo_Servicio.Avion
                            tarifa = TarifaReal * numInteg
                        Case Presupuesto.Servicio.Tipo_Servicio.Hotel
                            tarifa = (TarifaReal * NumeroDias * numInteg)
                        Case Presupuesto.Servicio.Tipo_Servicio.Coche_Alquiler
                            tarifa = (TarifaReal * NumeroDias)
                    End Select
                    Return tarifa
                End Get
            End Property

#End Region

        End Class

        Public Class HistoricoEstado

            Public Property IdViaje As Integer
            Public Property IdUser As Integer
            Public Property ChangeDate As DateTime
            Public Property State As EstadoPresup = -1

        End Class

    End Class

    Public Class TarifaServicios

#Region "Variables miembro"

        Private _id As Integer = Integer.MinValue
        Private _destino As String = String.Empty
        Private _nivel As ELL.Viaje.eNivel = -1
        Private _idPlanta As Integer = Integer.MinValue
        Private _obsoleta As Boolean = False
        Private _lineas As List(Of Lineas) = Nothing

#End Region

#Region "Properties"

        ''' <summary>
        ''' Identificador de la tarifa
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
        ''' Destino de la tarifa
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Destino() As String
            Get
                Return _destino
            End Get
            Set(ByVal value As String)
                _destino = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si es nacional, internacional o resto del mundo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Nivel() As ELL.Viaje.eNivel
            Get
                Return _nivel
            End Get
            Set(ByVal value As ELL.Viaje.eNivel)
                _nivel = value
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
        ''' Indica si la tarifa esta obsoleta o no
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
        ''' Lineas de una tarifa
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property LineasTarifa() As List(Of Lineas)
            Get
                Return _lineas
            End Get
            Set(ByVal value As List(Of Lineas))
                _lineas = value
            End Set
        End Property

#End Region

        Public Class Lineas

#Region "Variables miembro"

            Private _idTarifa As Integer = Integer.MinValue
            Private _anno As Integer = 0
            Private _tarifaAvion As Decimal = 0
            Private _tarifaHotel As Decimal = 0
            Private _tarifaCocheAlquiler As Decimal = 0

#End Region

#Region "Properties"

            ''' <summary>
            ''' Identificador de la tarifa
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property IdTarifa() As Integer
                Get
                    Return _idTarifa
                End Get
                Set(ByVal value As Integer)
                    _idTarifa = value
                End Set
            End Property

            ''' <summary>
            ''' Año de la tarifa
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Anno() As Integer
                Get
                    Return _anno
                End Get
                Set(ByVal value As Integer)
                    _anno = value
                End Set
            End Property

            ''' <summary>
            ''' Tarifa del avion
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property TarifaAvion() As Decimal
                Get
                    Return _tarifaAvion
                End Get
                Set(ByVal value As Decimal)
                    _tarifaAvion = value
                End Set
            End Property

            ''' <summary>
            ''' Tarifa del hotel
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property TarifaHotel() As Decimal
                Get
                    Return _tarifaHotel
                End Get
                Set(ByVal value As Decimal)
                    _tarifaHotel = value
                End Set
            End Property

            ''' <summary>
            ''' Tarifa del coche de alquiler
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property TarifaCocheAlquiler() As Decimal
                Get
                    Return _tarifaCocheAlquiler
                End Get
                Set(ByVal value As Decimal)
                    _tarifaCocheAlquiler = value
                End Set
            End Property

#End Region

        End Class

    End Class

    Public Class TarifaServiciosGenericas

#Region "Variables miembro"

        Private _id As Integer = Integer.MinValue
        Private _tipoServicio As ELL.Presupuesto.Servicio.Tipo_Servicio = -1
        Private _nombreServicio As String = String.Empty
        Private _nivel As ELL.Viaje.eNivel = -1
        Private _idPlanta As Integer = Integer.MinValue
        Private _obsoleta As Boolean = False
        Private _lineas As List(Of Lineas) = Nothing

#End Region

#Region "Properties"

            ''' <summary>
            ''' Identificador de la tarifa
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
        ''' Tipo del servicio al que hace referencia
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property TipoServicio() As ELL.Presupuesto.Servicio.Tipo_Servicio
            Get
                Return _tipoServicio
            End Get
            Set(ByVal value As ELL.Presupuesto.Servicio.Tipo_Servicio)
                _tipoServicio = value
            End Set
        End Property

        ''' <summary>
        ''' Destino de la tarifa
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property NombreServicio() As String
                Get
                    Return _nombreServicio
                End Get
                Set(ByVal value As String)
                    _nombreServicio = value
                End Set
            End Property

            ''' <summary>
            ''' Indica si es nacional, internacional o resto del mundo
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Nivel() As ELL.Viaje.eNivel
                Get
                    Return _nivel
                End Get
                Set(ByVal value As ELL.Viaje.eNivel)
                    _nivel = value
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
            ''' Indica si la tarifa esta obsoleta o no
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
            ''' Lineas de una tarifa
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property LineasTarifa() As List(Of Lineas)
                Get
                    Return _lineas
                End Get
                Set(ByVal value As List(Of Lineas))
                    _lineas = value
                End Set
            End Property

#End Region

            Public Class Lineas

#Region "Variables miembro"

                Private _idTarifa As Integer = Integer.MinValue
                Private _anno As Integer = 0
                Private _tarifa As Decimal = 0

#End Region

#Region "Properties"

                ''' <summary>
                ''' Identificador de la tarifa
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>		
                Public Property IdTarifa() As Integer
                    Get
                        Return _idTarifa
                    End Get
                    Set(ByVal value As Integer)
                        _idTarifa = value
                    End Set
                End Property

                ''' <summary>
                ''' Año de la tarifa
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>		
                Public Property Anno() As Integer
                    Get
                        Return _anno
                    End Get
                    Set(ByVal value As Integer)
                        _anno = value
                    End Set
                End Property

                ''' <summary>
                ''' Tarifa del servicio
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>		
                Public Property Tarifa() As Decimal
                    Get
                        Return _tarifa
                    End Get
                    Set(ByVal value As Decimal)
                        _tarifa = value
                    End Set
                End Property

#End Region

            End Class

        End Class

End Namespace