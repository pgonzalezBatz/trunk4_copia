Namespace ELL
    Public Class grupo

#Region "VARIABLES MIEMBRO"
        Private _idGrupo As Integer
        Private _idCultura As String
        Private _nombre As String
#End Region

#Region "CONSTRUCTOR"
        Public Sub New()
            _idGrupo = Integer.MinValue
            _idCultura = String.Empty
            _nombre = String.Empty
        End Sub

        Public Sub New(ByVal pIdGrupo As Integer, ByVal pIdCultura As String, ByVal pNombre As String)
            _idGrupo = pIdGrupo
            _idCultura = pIdCultura
            _nombre = pNombre
        End Sub
#End Region

#Region "PROPERTIES"
        Public Property IdGrupo() As Integer
            Get
                Return _idGrupo
            End Get
            Set(ByVal value As Integer)
                _idGrupo = value
            End Set
        End Property

        Public Property IdCultura() As String
            Get
                Return _idCultura
            End Get
            Set(ByVal value As String)
                _idCultura = value
            End Set
        End Property

        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property
#End Region

#Region "Columns Names"

        Public Class ColumnNames
            Private Const _IDGRUPO As String = "IdGrupo"
            Private Const _NOMBRE As String = "Nombre"
            Private Const _IDCULTURA As String = "IdCultura"

            Public Shared ReadOnly Property IDGRUPO() As String
                Get
                    Return _IDGRUPO
                End Get
            End Property

            Public Shared ReadOnly Property NOMBRE() As String
                Get
                    Return _NOMBRE
                End Get
            End Property

            Public Shared ReadOnly Property IDCULTURA() As String
                Get
                    Return _IDCULTURA
                End Get
            End Property

        End Class

#End Region

#Region "Orden"
        <Obsolete("ORDENAR EN LA INTERFAZ")> _
        Public Class SortClass
            Implements System.Collections.Generic.IComparer(Of grupo)

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
            ''' <param name="grup1">Objeto 1 a comparar</param>
            ''' <param name="grup2">Objeto 2 a comparar</param>
            ''' <returns></returns>
            Public Function Compare1(ByVal grup1 As grupo, ByVal grup2 As grupo) As Integer Implements System.Collections.Generic.IComparer(Of grupo).Compare

                If (_direccion = Direction.ASC) Then    'ORDEN ASCENDENTE
                    Select Case _NombreCampo
                        Case "Nombre"
                            Return grup1.Nombre < grup2.Nombre
                    End Select
                Else                                    'ORDEN DESCENDENTE
                    Select Case _NombreCampo
                        Case "Nombre"
                            Return grup1.Nombre > grup2.Nombre
                    End Select
                End If
            End Function

        End Class
#End Region


    End Class
End Namespace
