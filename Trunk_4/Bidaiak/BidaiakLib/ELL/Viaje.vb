Namespace ELL

	Public Class Viaje

#Region "Variables miembro"

		Private _idViaje As Integer = Integer.MinValue
        Private _destino As String = String.Empty
        Private _nivel As eNivel = eNivel.Europa_y_norte_Africa
        Private _descripcion As String = String.Empty
        Private _fechaIda As Date = Date.MinValue
        Private _fechaVuelta As Date = Date.MinValue
        Private _idUserSolicitador As Integer = Integer.MinValue
        Private _fechaSolicitud As Date = Date.MinValue
        Private _unidadOrganizativa As ELL.UnidadOrg = Nothing
        Private _idPlanta As Integer = Integer.MinValue
        Private _tipoViaje As eTipoViaje = eTipoViaje.Programado
        Private _anticipo As ELL.Anticipo = Nothing
        Private _solicitudAgencia As SolicitudAgencia = Nothing
        Private _integrantes As List(Of Integrante) = Nothing
        Private _hojasGasto As List(Of HojaGastos) = Nothing
        Private _estado As eEstadoViaje = eEstadoViaje.Pendiente_validacion
        Private _ResponsableLiquidacion As SabLib.ELL.Usuario = Nothing
        Private _proyectos As List(Of Proyecto) = Nothing
        Private _solicitudPlantaFilial As List(Of SolicitudPlantaFilial)
        Private _tipoDesplazamiento As TipoDesplaz
        Private _documentosCliente As List(Of DocumentoCliente)
        Private _documentosProy As List(Of DocumentoProyecto)
        Private _pais As Integer = Integer.MinValue
        Private _idTarifaDestino As Integer = Integer.MinValue
        Private _nombreTarifaDestino As String = String.Empty

        ''' <summary>
        ''' Nivel:Indica si es nacional, europeo/norte de Africa o resto del mundo
        ''' </summary>		
        Public Enum eNivel As Integer
            Nacional = 1
            Europa_y_norte_Africa = 2
            Resto_del_mundo = 3
        End Enum

        ''' <summary>
        ''' Tipo de viaje: Indica si es un viaje programado o es un viaje creado para un anticipo sin viaje real
        ''' </summary>		
        Public Enum eTipoViaje As Integer
            Programado = 1
            Anticipo = 2
        End Enum

        ''' <summary>
        ''' Indica el posible estado del viaje
        ''' </summary>		
        Public Enum eEstadoViaje As Integer
            Cancelado = 0
            Validado = 1
            Pendiente_validacion = 2
            No_validado = 3
        End Enum

        ''' <summary>
        ''' Indica el tipo de desplazamiento
        ''' </summary>		
        Public Enum TipoDesplaz As Integer
            Sin_especificar = 0
            Cliente = 1
            Plantas_Filiales = 2
            Otros = 3
        End Enum

#End Region

#Region "Properties"

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
        ''' Destino
		''' </summary>
		''' <value></value>
		''' <returns></returns>		
        Public Property Destino() As String
            Get
                Dim dest As String = NombreTarifaDestino
                If (dest.Length = 0) Then dest = _destino
                Return dest
            End Get
            Set(ByVal value As String)
                _destino = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si un viaje es nivel europeo, nacional o resto del mundo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Nivel() As eNivel
            Get
                Return _nivel
            End Get
            Set(ByVal value As eNivel)
                _nivel = value
            End Set
        End Property

        ''' <summary>
        ''' Descripcion del viaje
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Descripcion() As String
            Get
                Return _descripcion
            End Get
            Set(ByVal value As String)
                _descripcion = value
            End Set
        End Property

		''' <summary>
        ''' Indica si es un viaje programado o es un viaje creado para un anticipo sin viaje real
		''' </summary>
		''' <value></value>
		''' <returns></returns>		
        Public Property TipoViaje() As eTipoViaje
            Get
                Return _tipoViaje
            End Get
            Set(ByVal value As eTipoViaje)
                _tipoViaje = value
            End Set
        End Property

		''' <summary>
		''' Fecha de ida del viaje
		''' </summary>
		''' <value></value>
		''' <returns></returns>		
		Public Property FechaIda() As Date
			Get
				Return _fechaIda
			End Get
			Set(ByVal value As Date)
				_fechaIda = value
			End Set
		End Property

		''' <summary>
		''' Fecha de vuelta del viaje
		''' </summary>
		''' <value></value>
		''' <returns></returns>		
		Public Property FechaVuelta() As Date
			Get
				Return _fechaVuelta
			End Get
			Set(ByVal value As Date)
				_fechaVuelta = value
			End Set
		End Property

		''' <summary>
		''' Id del usuario que ha solicitado el viaje
		''' </summary>
		''' <value></value>
		''' <returns></returns>		
		Public Property IdUserSolicitador() As Integer
			Get
				Return _idUserSolicitador
			End Get
			Set(ByVal value As Integer)
				_idUserSolicitador = value
			End Set
		End Property

		''' <summary>
		''' Fecha de solicitud del viaje
		''' </summary>
		''' <value></value>
		''' <returns></returns>		
		Public Property FechaSolicitud() As Date
			Get
				Return _fechaSolicitud
			End Get
			Set(ByVal value As Date)
				_fechaSolicitud = value
			End Set
		End Property

		''' <summary>
        ''' Unidad organizativa
		''' </summary>
		''' <value></value>
		''' <returns></returns>		
        Public Property UnidadOrganizativa() As ELL.UnidadOrg
            Get
                Return _unidadOrganizativa
            End Get
            Set(ByVal value As ELL.UnidadOrg)
                _unidadOrganizativa = value
            End Set
        End Property

        ''' <summary>
        ''' Id de la planta a la que pertenece el viaje
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
		''' Anticipo asociado al viaje
		''' </summary>
		''' <value></value>
		''' <returns></returns>		
		Public Property Anticipo() As ELL.Anticipo
			Get
				Return _anticipo
			End Get
			Set(ByVal value As ELL.Anticipo)
				_anticipo = value
			End Set
		End Property

		''' <summary>
		''' Lista de servicios de agencia solicitados
		''' </summary>
		''' <value></value>
		''' <returns></returns>		
		Public Property SolicitudAgencia() As ELL.SolicitudAgencia
			Get
				Return _solicitudAgencia
			End Get
			Set(ByVal value As ELL.SolicitudAgencia)
				_solicitudAgencia = value
			End Set
		End Property

		''' <summary>
		''' Lista de integrantes del viaje
		''' </summary>
		''' <value></value>
		''' <returns></returns>		
        Public Property ListaIntegrantes() As List(Of Integrante)
            Get
                Return _integrantes
            End Get
            Set(ByVal value As List(Of Integrante))
                _integrantes = value
            End Set
        End Property

        ''' <summary>
        ''' Hojas de gastos asociadas al viaje
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property HojasGastos() As List(Of HojaGastos)
            Get
                Return _hojasGasto
            End Get
            Set(ByVal value As List(Of HojaGastos))
                _hojasGasto = value
            End Set
        End Property

        ''' <summary>
        ''' Indica el estado del viaje
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Estado() As eEstadoViaje
            Get
                Return _estado
            End Get
            Set(ByVal value As eEstadoViaje)
                _estado = value
            End Set
        End Property

        ''' <summary>
        ''' Usuario responsable de recoger o pedir el anticipo y realizar la liquidacion
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property ResponsableLiquidacion() As Sablib.ELL.Usuario
            Get
                Return _ResponsableLiquidacion
            End Get
            Set(ByVal value As Sablib.ELL.Usuario)
                _ResponsableLiquidacion = value
            End Set
        End Property

        ''' <summary>
        ''' Proyectos que tiene asociados el viaje
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Proyectos() As List(Of Proyecto)
            Get
                Return _proyectos
            End Get
            Set(ByVal value As List(Of Proyecto))
                _proyectos = value
            End Set
        End Property

        ''' <summary>
        ''' Obtiene de los integrantes pasados como parametros, el listado de los usuarios, omitiendo el resto de informacion
        ''' </summary>
        ''' <returns></returns>
        Public Function getUsuariosIntegrantes() As List(Of Sablib.ELL.Usuario)
            Dim lUsuariosIntegrantes As New List(Of Sablib.ELL.Usuario)
            If (ListaIntegrantes IsNot Nothing AndAlso ListaIntegrantes.Count > 0) Then
                For Each Integ As ELL.Viaje.Integrante In ListaIntegrantes
                    lUsuariosIntegrantes.Add(Integ.Usuario)
                Next
            End If
            Return lUsuariosIntegrantes
        End Function

        ''' <summary>
        ''' Si el tipo de desplazamiento es de tipo Planta filial, tendra el listado de solicitudes de plantas filiales
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property SolicitudesPlantasFilial() As List(Of SolicitudPlantaFilial)
            Get
                Return _solicitudPlantaFilial
            End Get
            Set(ByVal value As List(Of SolicitudPlantaFilial))
                _solicitudPlantaFilial = value
            End Set
        End Property

        ''' <summary>
        ''' Si el tipo de desplazamiento es de tipo cliente, tendra el listado de documentos
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property DocumentosCliente() As List(Of DocumentoCliente)
            Get
                Return _documentosCliente
            End Get
            Set(ByVal value As List(Of DocumentoCliente))
                _documentosCliente = value
            End Set
        End Property

        ''' <summary>
        ''' Documentos relacionados en xbat contra un proyecto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property DocumentosProyecto() As List(Of DocumentoProyecto)
            Get
                Return _documentosProy
            End Get
            Set(ByVal value As List(Of DocumentoProyecto))
                _documentosProy = value
            End Set
        End Property

        ''' <summary>
        ''' Indica el tipo de desplazamiento. Si no es extranjero, sera sin especificar
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property TipoDesplazamiento() As TipoDesplaz
            Get
                Return _tipoDesplazamiento
            End Get
            Set(ByVal value As TipoDesplaz)
                _tipoDesplazamiento = value
            End Set
        End Property

        ''' <summary>
        ''' Indica el país de destino. Si es nacional, sera sin especificar
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Pais() As Integer
            Get
                Return _pais
            End Get
            Set(ByVal value As Integer)
                _pais = value
            End Set
        End Property

        ''' <summary>
        ''' Identificador de la tarifa destino
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
        ''' Nombre del destino de la tarifa
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property NombreTarifaDestino() As String
            Get
                Return _nombreTarifaDestino
            End Get
            Set(ByVal value As String)
                _nombreTarifaDestino = value
            End Set
        End Property

#End Region

        <Serializable()> _
        Public Class Integrante

#Region "Variables miembro"

            Private _user As Sablib.ELL.Usuario = Nothing
            Private _idActividad As Integer = Integer.MinValue
            Private _observaciones As String = String.Empty
            Private _idValidador As Integer = Integer.MinValue
            Private _fechaIda As Date = Date.MinValue
            Private _fechaVuelta As Date = Date.MinValue
            Private _numPlan As Integer = 1
            Private _esPaP As Boolean = False
            Private _idCondEsp As Integer = Integer.MinValue

#End Region

#Region "Properties"

            ''' <summary>
            ''' Integrante
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
            ''' Validador del usuario
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property IdValidador() As Integer
                Get
                    Return _idValidador
                End Get
                Set(ByVal value As Integer)
                    _idValidador = value
                End Set
            End Property

            ''' <summary>
            ''' Actividad con la que esta relacionado
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property IdActividad() As Integer
                Get
                    Return _idActividad
                End Get
                Set(ByVal value As Integer)
                    _idActividad = value
                End Set
            End Property

            ''' <summary>
            ''' Observaciones introducidas acerca de la actividad
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Observaciones() As String
                Get
                    Return _observaciones
                End Get
                Set(ByVal value As String)
                    _observaciones = value
                End Set
            End Property

            ''' <summary>
            ''' Fecha de ida del viaje
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property FechaIda() As Date
                Get
                    Return _fechaIda
                End Get
                Set(ByVal value As Date)
                    _fechaIda = value
                End Set
            End Property

            ''' <summary>
            ''' Fecha de vuelta del viaje
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property FechaVuelta() As Date
                Get
                    Return _fechaVuelta
                End Get
                Set(ByVal value As Date)
                    _fechaVuelta = value
                End Set
            End Property

            ''' <summary>
            ''' Numero del plan al que puede asociarse en el presupuesto de la agencia
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property NumPlan() As Integer
                Get
                    Return _numPlan
                End Get
                Set(ByVal value As Integer)
                    _numPlan = value
                End Set
            End Property

            ''' <summary>
            ''' Indica si es una puesta a punto para los desarraigados
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property esPaP_Desarraigados() As Boolean
                Get
                    Return _esPaP
                End Get
                Set(ByVal value As Boolean)
                    _esPaP = value
                End Set
            End Property

            ''' <summary>
            ''' Id de la condicion especial de un desarraigado
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property CondicionesEspeciales_Desarraigados() As Integer
                Get
                    Return _idCondEsp
                End Get
                Set(ByVal value As Integer)
                    _idCondEsp = value
                End Set
            End Property

            ''' <summary>
            ''' Indica si un integrante es un desarraigado
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public ReadOnly Property EsDesarraigado() As Boolean
                Get
                    Return (CondicionesEspeciales_Desarraigados <> Integer.MinValue)
                End Get
            End Property

#End Region

        End Class

        Public Class Proyecto

#Region "Variables miembro"

            Private _idViaje As Integer = Integer.MinValue
            Private _porcentaje As Integer = Integer.MinValue
            Private _idPrograma As Integer = Integer.MinValue           
            Private _numOF As String = String.Empty
            Private _numOP As Integer = Integer.MinValue
            Private _descripcion As String = String.Empty

#End Region

#Region "Properties"

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
            ''' Porcentaje del proyecto que se repartira para los costos
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Porcentaje() As Integer
                Get
                    Return _porcentaje
                End Get
                Set(ByVal value As Integer)
                    _porcentaje = value
                End Set
            End Property

            ''' <summary>
            ''' Id del programa (proyecto)
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property IdPrograma() As Integer
                Get
                    Return _idPrograma
                End Get
                Set(ByVal value As Integer)
                    _idPrograma = value
                End Set
            End Property

            ''' <summary>
            ''' Nº de la of
            ''' Se le ha puesto string, para que tambien soporta la of de brain que tiene letras
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property NumOF() As String
                Get
                    Return _numOF
                End Get
                Set(ByVal value As String)
                    _numOF = value
                End Set
            End Property

            ''' <summary>
            ''' Campo creado para Cognos. Tendra la descripcion del proyecto (XBAT) o de la OF (Brain)            
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Descripcion() As String
                Get
                    Return _descripcion
                End Get
                Set(ByVal value As String)
                    _descripcion = value
                End Set
            End Property

#End Region

        End Class

        <Serializable()> _
        Public Class SolicitudPlantaFilial

#Region "Variables miembro"

            Private _idViaje As Integer = Integer.MinValue
            Private _idPlanta As Integer = Integer.MinValue
            Private _observaciones As String = String.Empty
            Private _estado As EstadoSolFilial = EstadoSolFilial.Solicitado

            ''' <summary>
            ''' Posibles estados de una solicitud de planta filial
            ''' </summary>
            Public Enum EstadoSolFilial
                Solicitado = 0
                Aprobada = 1
                Rechazada = 2
                Sin_Solicitud = 3
            End Enum
#End Region

#Region "Properties"

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
            ''' Id de la planta
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property IdPlantaFilial() As Integer
                Get
                    Return _idPlanta
                End Get
                Set(ByVal value As Integer)
                    _idPlanta = value
                End Set
            End Property

            ''' <summary>
            ''' Observaciones introducidas acerca de la actividad
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Observaciones() As String
                Get
                    Return _observaciones
                End Get
                Set(ByVal value As String)
                    _observaciones = value
                End Set
            End Property

            ''' <summary>
            ''' Estado en la que se encuentra la solicitud
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property EstadoSolicitud() As EstadoSolFilial
                Get
                    Return _estado
                End Get
                Set(ByVal value As EstadoSolFilial)
                    _estado = value
                End Set
            End Property

#End Region

        End Class

        Public Class DocumentoCliente

#Region "Variables miembro"

            Private _id As Integer = Integer.MinValue
            Private _idViaje As Integer = Integer.MinValue
            Private _titulo As String = String.Empty
            Private _contentType As String = String.Empty
            Private _nombreFichero As String = String.Empty
            Private _documento As Byte() = Nothing
            Private _fSubida As DateTime = DateTime.MinValue

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
            ''' Titulo del documento
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Titulo() As String
                Get
                    Return _titulo
                End Get
                Set(ByVal value As String)
                    _titulo = value
                End Set
            End Property

            ''' <summary>
            ''' Tipo de contenido
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property ContentType() As String
                Get
                    Return _contentType
                End Get
                Set(ByVal value As String)
                    _contentType = value
                End Set
            End Property

            ''' <summary>
            ''' Nombre y extension del fichero
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property NombreFichero() As String
                Get
                    Return _nombreFichero
                End Get
                Set(ByVal value As String)
                    _nombreFichero = value
                End Set
            End Property

            ''' <summary>
            ''' Documento adjunto
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Documento() As Byte()
                Get
                    Return _documento
                End Get
                Set(ByVal value As Byte())
                    _documento = value
                End Set
            End Property

            ''' <summary>
            ''' Fecha de subida del documento
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property FechaSubida() As DateTime
                Get
                    Return _fSubida
                End Get
                Set(ByVal value As DateTime)
                    _fSubida = value
                End Set
            End Property

#End Region

        End Class

        Public Class DocumentoIntegrante

#Region "Variables miembro"

            Private _id As Integer = Integer.MinValue
            Private _idViaje As Integer = Integer.MinValue
            Private _idIntegrante As Integer = Integer.MinValue
            Private _titulo As String = String.Empty
            Private _contentType As String = String.Empty
            Private _nombreFichero As String = String.Empty
            Private _documento As Byte() = Nothing
            Private _fSubida As DateTime = DateTime.MinValue

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
            ''' Identificador del integrante
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property IdIntegrante() As Integer
                Get
                    Return _idIntegrante
                End Get
                Set(ByVal value As Integer)
                    _idIntegrante = value
                End Set
            End Property

            ''' <summary>
            ''' Titulo del documento
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Titulo() As String
                Get
                    Return _titulo
                End Get
                Set(ByVal value As String)
                    _titulo = value
                End Set
            End Property

            ''' <summary>
            ''' Tipo de contenido
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property ContentType() As String
                Get
                    Return _contentType
                End Get
                Set(ByVal value As String)
                    _contentType = value
                End Set
            End Property

            ''' <summary>
            ''' Nombre y extension del fichero
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property NombreFichero() As String
                Get
                    Return _nombreFichero
                End Get
                Set(ByVal value As String)
                    _nombreFichero = value
                End Set
            End Property

            ''' <summary>
            ''' Documento adjunto
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property Documento() As Byte()
                Get
                    Return _documento
                End Get
                Set(ByVal value As Byte())
                    _documento = value
                End Set
            End Property

            ''' <summary>
            ''' Fecha de subida del documento
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>		
            Public Property FechaSubida() As DateTime
                Get
                    Return _fSubida
                End Get
                Set(ByVal value As DateTime)
                    _fSubida = value
                End Set
            End Property

#End Region

        End Class

	End Class

End Namespace