Namespace ELL

    Public Class Telefono

#Region "Variables miembro"

        Private _id As Integer = Integer.MinValue
        Private _numero As String = String.Empty
        Private _fAlta As Date = Date.MinValue
        Private _fBaja As Date = Date.MinValue
        Private _idCiaTlfno As Integer = Integer.MinValue        
        Private _idPlanta As Integer = Integer.MinValue
        Private _idUsuarioGestor As Integer = Integer.MinValue
        Private _pin As String = String.Empty
        Private _puk As String = String.Empty
        Private _dualizado As Boolean = False
        Private _vozDatos As VozDatos = VozDatos.null
        Private _fijoMovil As FijoMovil = FijoMovil.null
        Private _modelo As String = String.Empty
        Private _roaming As Boolean = False
        Private _comentarios As String = String.Empty
        Private _tipoLineaFijo As tipoLineaFijo = tipoLineaFijo.null
        Private _listPersonasAsig As List(Of ELL.TelefonoUsuDep) = Nothing
        Private _obsoleto As Boolean = False
        Private _idPerfilMov As Integer = Integer.MinValue
        Private _idTarifaDatos As Integer = Integer.MinValue

#End Region

#Region "Constructores"

        Public Sub New()

        End Sub

        Public Sub New(ByVal pId As Integer)
            _id = pId
        End Sub

        Public Sub New(ByVal pNumero As String)
            _numero = pNumero
        End Sub

#End Region

#Region "Enumeracion"

        ''' <summary>
        ''' Especifica de que tipo seran los tlfnos moviles
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum VozDatos As Integer
            null = -1
            voz = 0
            datos = 1
        End Enum


        ''' <summary>        
        ''' Especifica de que tipo sera un tlfno
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum FijoMovil As Integer
            null = -1
            fijo = 0
            movil = 1
        End Enum


        ''' <summary>
        ''' Especifica el tipo de linea de los tlfnos moviles
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum tipoLineaFijo As Integer
            null = -1
            analogico = 0
            rdsi = 1
        End Enum

#End Region

#Region "Propiedades"

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
        ''' Numero de telefono
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        Public Property Numero() As String
            Get
                Return _numero
            End Get
            Set(ByVal value As String)
                _numero = value
            End Set
        End Property

        ''' <summary>
        ''' Fecha en la que se da de alta el numero de telefono
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        Public Property FechaAlta() As Date
            Get
                Return _fAlta
            End Get
            Set(ByVal value As Date)
                _fAlta = value
            End Set
        End Property

        ''' <summary>
        ''' Fecha en la que se da de baja el numero de telefono
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        Public Property FechaBaja() As Date
            Get
                Return _fBaja
            End Get
            Set(ByVal value As Date)
                _fBaja = value
            End Set
        End Property

        ''' <summary>
        ''' Identificador de la compañia telefonica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        Public Property IdCiaTlfno() As Integer
            Get
                Return _idCiaTlfno
            End Get
            Set(ByVal value As Integer)
                _idCiaTlfno = value
            End Set
        End Property

        ''' <summary>
        ''' Identificador de la planta a la que pertenece el telefono
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
        ''' Identificador del usuario gestor del telefono
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        Public Property IdUsuarioGestor() As Integer
            Get
                Return _idUsuarioGestor
            End Get
            Set(ByVal value As Integer)
                _idUsuarioGestor = value
            End Set
        End Property

        ''' <summary>
        ''' Codigo PIN del telefono
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        Public Property PIN() As String
            Get
                Return _pin
            End Get
            Set(ByVal value As String)
                _pin = value
            End Set
        End Property

        ''' <summary>
        ''' Codigo PUK del telefono
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        Public Property PUK() As String
            Get
                Return _puk
            End Get
            Set(ByVal value As String)
                _puk = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si es dualizado o no, en caso de que sea un movil
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        Public Property Dualizado() As Boolean
            Get
                Return _dualizado
            End Get
            Set(ByVal value As Boolean)
                _dualizado = value
            End Set
        End Property

        ''' <summary>
        ''' Comprueba si el tipo es por voz o por datos, si es un telefono movil
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        Public Property VozODatos() As VozDatos
            Get
                Return _vozDatos
            End Get
            Set(ByVal value As VozDatos)
                _vozDatos = value
            End Set
        End Property

        ''' <summary>
        ''' Comprueba si el telefono es fijo o movil
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        Public Property FijoOMovil() As FijoMovil
            Get
                Return _fijoMovil
            End Get
            Set(ByVal value As FijoMovil)
                _fijoMovil = value
            End Set
        End Property

        ''' <summary>
        ''' Modelo del movil
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        Public Property Modelo() As String
            Get
                Return _modelo
            End Get
            Set(ByVal value As String)
                _modelo = value
            End Set
        End Property

        ''' <summary>
        ''' Devuelve true si es movil
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        Public ReadOnly Property EsMovil()
            Get
                Return (FijoOMovil = FijoMovil.movil)
            End Get
        End Property

        ''' <summary>
        ''' Lista de las personas a las que se le ha asignado al telefono
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        Public Property ListaPersonasAsig() As List(Of ELL.TelefonoUsuDep)
            Get
                Return _listPersonasAsig
            End Get
            Set(ByVal value As List(Of ELL.TelefonoUsuDep))
                _listPersonasAsig = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si el telefono tiene activado el roaming o no
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        Public Property Roaming() As Boolean
            Get
                Return _roaming
            End Get
            Set(ByVal value As Boolean)
                _roaming = value
            End Set
        End Property

        ''' <summary>
        ''' Comentarios acerca de un telefono
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
        ''' Si un telefono es fijo, indicará el tipo de linea
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        Public Property Tipo_LineaFijo() As tipoLineaFijo
            Get
                Return _tipoLineaFijo
            End Get
            Set(ByVal value As tipoLineaFijo)
                _tipoLineaFijo = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si el telefono esta obsoleto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        Public Property Obsoleto() As Boolean
            Get
                Return _obsoleto
            End Get
            Set(ByVal value As Boolean)
                _obsoleto = value
            End Set
        End Property

        ''' <summary>
        ''' Identificador del perfil movil
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        Public Property IdPerfilMovil() As Integer
            Get
                Return _idPerfilMov
            End Get
            Set(ByVal value As Integer)
                _idPerfilMov = value
            End Set
        End Property

        ''' <summary>
        ''' Identificador la tarifa
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        Public Property IdTarifaDatos() As Integer
            Get
                Return _idTarifaDatos
            End Get
            Set(ByVal value As Integer)
                _idTarifaDatos = value
            End Set
        End Property

#End Region

#Region "Property Names"

        ''' <summary>
        ''' Clase para definir los nombres de las propiedades de la clase
        ''' </summary>
        ''' <remarks></remarks>
        Public Class PropertyNames
            Private Const _ID As String = "ID"
            Private Const _NUMERO As String = "NUMERO"

            Public Shared ReadOnly Property ID() As String
                Get
                    Return _ID
                End Get
            End Property

            Public Shared ReadOnly Property NUMERO() As String
                Get
                    Return _NUMERO
                End Get
            End Property

        End Class

#End Region

#Region "Clase Gestor"        
        Public Class GestorTlfno

#Region "Variables miembro"

            Private _idUsuarioGestor As Integer = Integer.MinValue
            Private _nombreUsuario As String = String.Empty
            Private _idPlanta As Integer = Integer.MinValue
            Private _planta As String = String.Empty            

#End Region

#Region "Constructores"

            Public Sub New()

            End Sub

            Public Sub New(ByVal idG As Integer, ByVal idP As Integer)
                _idUsuarioGestor = idG
                _idPlanta = idP
            End Sub
#End Region

#Region "Propiedades"


            ''' <summary>
            ''' Identificador del usuario gestor del telefono
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property IdUsuarioGestor() As Integer
                Get
                    Return _idUsuarioGestor
                End Get
                Set(ByVal value As Integer)
                    _idUsuarioGestor = value
                End Set
            End Property


            ''' <summary>
            ''' Identificador de la planta a la que pertenece el telefono
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property IdPlanta() As Integer
                Get
                    Return _idPlanta
                End Get
                Set(ByVal value As Integer)
                    _idPlanta = value
                End Set
            End Property



            ''' <summary>
            ''' Nombre del usuario gestor
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property UsuarioGestor() As String
                Get
                    Return _nombreUsuario
                End Get
                Set(ByVal value As String)
                    _nombreUsuario = value
                End Set
            End Property


            ''' <summary>
            ''' Nombre de la planta
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Planta() As String
                Get
                    Return _planta
                End Get
                Set(ByVal value As String)
                    _planta = value
                End Set
            End Property

#End Region

#Region "Property Names"

            ''' <summary>
            ''' Clase para definir los nombres de las propiedades de la clase
            ''' </summary>
            ''' <remarks></remarks>
            Public Class PropertyNames
                Private Const _ID_USER As String = "IdUsuarioGestor"
                Private Const _USER As String = "usuariogestor"
                Private Const _PLANTA As String = "Planta"

                Public Shared ReadOnly Property ID_USER() As String
                    Get
                        Return _ID_USER
                    End Get
                End Property

                Public Shared ReadOnly Property USER() As String
                    Get
                        Return _USER
                    End Get
                End Property

                Public Shared ReadOnly Property PLANTA() As String
                    Get
                        Return _PLANTA
                    End Get
                End Property

            End Class

#End Region

#Region "SortClass"

            ''' <summary>
            ''' Clase de ordenacion
            ''' </summary>
            Public Class SortClass
                Implements System.Collections.Generic.IComparer(Of Telefono.GestorTlfno)

                ''' <summary>
                ''' Nombre del campo por el que hay que ordenar
                ''' </summary>
                Private _NombreCampo As String
                ''' <summary>
                ''' Direccion de ordenamiento
                ''' </summary>
                ''' <remarks></remarks>
                Private _direccion As Direction

                ''' <summary>
                ''' Constructor que inicializa lka clase
                ''' </summary>
                ''' <param name="nombre">Nombre del campo</param>
                ''' <param name="dir">Direccion del ordenamiento</param>
                Public Sub New(ByVal nombre As String, ByVal dir As Direction)
                    _NombreCampo = nombre
                    _direccion = dir
                End Sub

                ''' <summary>
                ''' Enumeracion para indicar el tipo de orden que se va a realizar (Ascendente o descendente)
                ''' </summary>
                Public Enum Direction
                    ASC
                    DESC
                End Enum


                ''' <summary>
                ''' Funcion de comparacion que se usara al ordenar
                ''' </summary>
                ''' <param name="ge1">Objeto 1 a comparar</param>
                ''' <param name="ge2">Objeto 2 a comparar</param>
                ''' <returns></returns>
                Public Function Compare1(ByVal ge1 As Telefono.GestorTlfno, ByVal ge2 As Telefono.GestorTlfno) As Integer Implements System.Collections.Generic.IComparer(Of Telefono.GestorTlfno).Compare

                    If (_direccion = Direction.ASC) Then    'ORDEN ASCENDENTE
                        Select Case _NombreCampo.ToLower
                            Case "usuariogestor"
                                Return ge1.UsuarioGestor < ge2.UsuarioGestor
                        End Select
                    Else                                    'ORDEN DESCENDENTE
                        Select Case _NombreCampo.ToLower
                            Case "usuariogestor"
                                Return ge1.UsuarioGestor > ge2.UsuarioGestor
                        End Select
                    End If
                End Function

            End Class

#End Region

        End Class

#End Region

#Region "Tarifa Datos"

        Public Class TarifaDatos

#Region "Variables miembro"

            Private _id As Integer = Integer.MinValue
            Private _nombre As String = String.Empty
            Private _idPlanta As Integer = Integer.MinValue
            Private _obsoleto As Boolean = False

#End Region

#Region "Columnas"

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
            ''' Nombre de la tarifa
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
            ''' Identificador de la planta a la que pertenece el alveolo
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
            ''' Indica si el tipo esta obsoleto o no
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            Public Property Obsoleto() As Boolean
                Get
                    Return _obsoleto
                End Get
                Set(ByVal value As Boolean)
                    _obsoleto = value
                End Set
            End Property

#End Region

#Region "Property Names"

            ''' <summary>
            ''' Clase para definir los nombres de las propiedades de la clase
            ''' </summary>
            Public Class PropertyNames
                Private Const _ID As String = "ID"
                Private Const _NOMBRE As String = "NOMBRE"

                Public Shared ReadOnly Property ID() As String
                    Get
                        Return _ID
                    End Get
                End Property

                Public Shared ReadOnly Property NOMBRE() As String
                    Get
                        Return _NOMBRE
                    End Get
                End Property

            End Class

#End Region

        End Class

#End Region

    End Class

End Namespace