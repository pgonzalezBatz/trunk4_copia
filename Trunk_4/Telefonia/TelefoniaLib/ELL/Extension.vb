Namespace ELL

    Public Class Extension

#Region "Variables miembro"

        Private _id As Integer = Integer.MinValue
        Private _extension As Integer = Integer.MinValue
        Private _nombre As String = String.Empty
        Private _idTipoExt As Integer = Integer.MinValue
        Private _idAlveolo As Integer = Integer.MinValue
        Private _idTipoLinea As Integer = Integer.MinValue
        Private _visible As Boolean = False        
        Private _idPlanta As Integer = Integer.MinValue
        Private _idTlfno As Integer = Integer.MinValue
        Private _idExtInterna As Integer = Integer.MinValue
        Private _idDepartentoFac As String = String.Empty
        Private _tipoAsig As AsociarA = AsociarA.personal
        Private _listPersonasAsig As List(Of ELL.ExtensionUsuDep) = Nothing
        Private _listDepartAsig As List(Of ELL.ExtensionUsuDep) = Nothing
        Private _listOtrosAsig As List(Of ELL.ExtensionUsuDep) = Nothing
        Private _idTipoAsignacionMovil As AsignarA = Nothing
        Private _obsoleto As Boolean = False
        Private _prestamo As Boolean = False

#End Region

#Region "Enumeracion"

        Public Enum TipoExt As Integer
            interna = 1
            movil = 2
        End Enum

        Public Enum AsociarA As Integer
            personal = 0
            departamental = 1
            otros = 2
        End Enum

        Public Enum AsignarA As Integer
            extensionInterna = 0
            persona = 1
            otros = 2
            sinAsignar = 3
        End Enum

#End Region

#Region "Propiedades"

        ''' <summary>
        ''' Identificador unico
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property


        ''' <summary>
        ''' Extension numerica
        ''' <example>1145</example>
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Extension() As Integer
            Get
                Return _extension
            End Get
            Set(ByVal value As Integer)
                _extension = value
            End Set
        End Property


        ''' <summary>
        ''' Nombre que se le da a la extension
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property


        ''' <summary>
        ''' Identificador de la extension
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdTipoExtension() As Integer
            Get
                Return _idTipoExt
            End Get
            Set(ByVal value As Integer)
                _idTipoExt = value
            End Set
        End Property


        ''' <summary>
        ''' Identificador del alveolo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdAlveolo() As Integer
            Get
                Return _idAlveolo
            End Get
            Set(ByVal value As Integer)
                _idAlveolo = value
            End Set
        End Property

        ''' <summary>
        ''' Identificador de la linea
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdTipoLinea() As Integer
            Get
                Return _idTipoLinea
            End Get
            Set(ByVal value As Integer)
                _idTipoLinea = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si es visible en la generacion de algunas consultas/informes
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Visible() As Boolean
            Get
                Return _visible
            End Get
            Set(ByVal value As Boolean)
                _visible = value
            End Set
        End Property


        ''' <summary>
        ''' Identificador de la planta
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
        ''' Identificador del telefono al que se ha asignado la extension
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdTelefono() As Integer
            Get
                Return _idTlfno
            End Get
            Set(ByVal value As Integer)
                _idTlfno = value
            End Set
        End Property


        ''' <summary>
        ''' Identificador de la extension interna a la que se asocia una extension movil
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdExtensionInterna() As Integer
            Get
                Return _idExtInterna
            End Get
            Set(ByVal value As Integer)
                _idExtInterna = value
            End Set
        End Property


        ''' <summary>
        ''' Identificador del departamento al que se va a facturar
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdDepartamentoFac() As String
            Get
                Return _idDepartentoFac
            End Get
            Set(ByVal value As String)
                _idDepartentoFac = value
            End Set
        End Property


        ''' <summary>
        ''' Indica si esta asociado a personas, departamentos u otros
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdTipoAsignacion() As AsociarA
            Get
                Return _tipoAsig
            End Get
            Set(ByVal value As AsociarA)
                _tipoAsig = value
            End Set
        End Property


        ''' <summary>
        ''' Lista de las personas a las que se le ha asignado la extension
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ListaPersonasAsig() As List(Of ELL.ExtensionUsuDep)
            Get
                Return _listPersonasAsig
            End Get
            Set(ByVal value As List(Of ELL.ExtensionUsuDep))
                _listPersonasAsig = value
            End Set
        End Property


        ''' <summary>
        ''' Lista de los departamentos a las que se he asignado la extension
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ListaDepartamentosAsig() As List(Of ELL.ExtensionUsuDep)
            Get
                Return _listDepartAsig
            End Get
            Set(ByVal value As List(Of ELL.ExtensionUsuDep))
                _listDepartAsig = value
            End Set
        End Property


        ''' <summary>
        ''' Lista de los otros elementos a las que se he asignado la extension
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ListaOtrosAsig() As List(Of ELL.ExtensionUsuDep)
            Get
                Return _listOtrosAsig
            End Get
            Set(ByVal value As List(Of ELL.ExtensionUsuDep))
                _listOtrosAsig = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si el telefono esta obsoleto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Obsoleto() As Boolean
            Get
                Return _obsoleto
            End Get
            Set(ByVal value As Boolean)
                _obsoleto = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si una extension de prestamo o de asignacion a personas
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Prestamo() As Boolean
            Get
                Return _prestamo
            End Get
            Set(ByVal value As Boolean)
                _prestamo = value
            End Set
        End Property

        ''' <summary>
        ''' Indica a quien esta asignada la extension movil: Personas,otros, extension interna u sin asignar
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TipoAsignacionMovil() As AsignarA
            Get
                If (Me.IdTipoExtension = ELL.Extension.TipoExt.interna) Then
                    Return AsignarA.sinAsignar
                Else
                    If (Me.IdExtensionInterna <> Integer.MinValue) Then
                        Return AsignarA.extensionInterna
                    ElseIf (Me.ListaPersonasAsig IsNot Nothing AndAlso Me.ListaPersonasAsig.Count > 0) Then
                        Return AsignarA.persona
                    ElseIf (Me.ListaOtrosAsig IsNot Nothing AndAlso Me.ListaOtrosAsig.Count > 0) Then
                        Return AsignarA.otros
                    Else
                        Return AsignarA.sinAsignar
                    End If
                end if
            End Get
            Set(ByVal value As AsignarA)
                _idTipoAsignacionMovil = value
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
            Private Const _EXTENSION As String = "EXTENSION"
            Private Const _NOMBRE As String = "NOMBRE"

            Public Shared ReadOnly Property ID() As String
                Get
                    Return _ID
                End Get
            End Property

            Public Shared ReadOnly Property EXTENSION() As String
                Get
                    Return _EXTENSION
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

End Namespace