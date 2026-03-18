Namespace ELL
    <Serializable()> _
    Public Class Usuario

#Region "Variables miembro"
        Private _id As Integer = Integer.MinValue
        Private _nombreUsuario As String = String.Empty
        Private _idEmpresa As Integer = Integer.MinValue
        Private _cultura As String = String.Empty
        Private _codPersona As Integer = Integer.MinValue
        Private _email As String = String.Empty
        Private _fechaAlta As DateTime = DateTime.MinValue
        Private _fechaBaja As DateTime = DateTime.MinValue
        Private _idDirectorioActivo As String = String.Empty
        Private _idFtp As String = String.Empty
        Private _idMatrix As String = String.Empty
        Private _pwd As String = String.Empty
        Private _idDepartamento As String = String.Empty
        Private _nombre As String = String.Empty
        Private _idPlanta As Integer = Integer.MinValue
        Private _apellido1 As String = String.Empty
        Private _apellido2 As String = String.Empty
        Private _lPlantas As List(Of ELL.Planta) = Nothing
        Private _foto As Byte() = Nothing
        Private _dni As String = String.Empty
#End Region

#Region "Propiedades"
        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property NombreUsuario() As String
            Get
                Return _nombreUsuario
            End Get
            Set(ByVal value As String)
                _nombreUsuario = value
            End Set
        End Property

        Public Property IdEmpresa() As Integer
            Get
                Return _idEmpresa
            End Get
            Set(ByVal value As Integer)
                _idEmpresa = value
            End Set
        End Property

        Public Property Cultura() As String
            Get
                Return _cultura
            End Get
            Set(ByVal value As String)
                _cultura = value
            End Set
        End Property

        Public Property CodPersona() As Integer
            Get
                Return _codPersona
            End Get
            Set(ByVal value As Integer)
                _codPersona = value
            End Set
        End Property

        Public Property Email() As String
            Get
                Return _email
            End Get
            Set(ByVal value As String)
                _email = value
            End Set
        End Property


        Public Property FechaAlta() As DateTime
            Get
                Return _fechaAlta
            End Get
            Set(ByVal value As DateTime)
                _fechaAlta = value
            End Set
        End Property


        Public Property FechaBaja() As DateTime
            Get
                Return _fechaBaja
            End Get
            Set(ByVal value As DateTime)
                _fechaBaja = value
            End Set
        End Property


        Public Property IdDirectorioActivo() As String
            Get
                Return _idDirectorioActivo
            End Get
            Set(ByVal value As String)
                _idDirectorioActivo = value
            End Set
        End Property

        Public Property IdFTP() As String
            Get
                Return _idFtp
            End Get
            Set(ByVal value As String)
                _idFtp = value
            End Set
        End Property


        Public Property IdMatrix() As String
            Get
                Return _idMatrix
            End Get
            Set(ByVal value As String)
                _idMatrix = value
            End Set
        End Property


        Public Property PWD() As String
            Get
                Return _pwd
            End Get
            Set(ByVal value As String)
                _pwd = value
            End Set
        End Property


        Public Property IdDepartamento() As String
            Get
                Return _idDepartamento
            End Get
            Set(ByVal value As String)
                _idDepartamento = value
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

        Public Property Apellido1() As String
            Get
                Return _apellido1
            End Get
            Set(ByVal value As String)
                _apellido1 = value
            End Set
        End Property

        Public Property Apellido2() As String
            Get
                Return _apellido2
            End Get
            Set(ByVal value As String)
                _apellido2 = value
            End Set
        End Property

        Public ReadOnly Property NombreCompleto() As String
            Get
                Dim nombreComp As String = Nombre & " " & Apellido1 & " " & Apellido2
                Return nombreComp.Trim
            End Get
        End Property

        Public Property IdPlanta() As Integer
            Get
                Return _idPlanta
            End Get
            Set(ByVal value As Integer)
                _idPlanta = value
            End Set
        End Property

        Public Property Plantas() As List(Of ELL.Planta)
            Get
                Return _lPlantas
            End Get
            Set(ByVal value As List(Of ELL.Planta))
                _lPlantas = value
            End Set
        End Property

        Public ReadOnly Property DadoBaja() As Boolean
            Get
                If (_fechaBaja = DateTime.MinValue) Then
                    Return False
                ElseIf (_fechaBaja <> DateTime.MinValue) Then
                    If (_fechaBaja < DateTime.Now) Then
                        Return True
                    Else
                        Return False
                    End If
                End If
            End Get
        End Property

        Public Property Foto() As Byte()
            Get
                Return _foto
            End Get
            Set(ByVal value As Byte())
                _foto = value
            End Set
        End Property

        Public Property Dni() As String
            Get
                Return _dni
            End Get
            Set(ByVal value As String)
                _dni = value
            End Set
        End Property

#End Region

#Region "Columns Names"

        Public Class ColumnNames
            Private Const _ID As String = "Id"
            Private Const _NOMBRE As String = "Nombre"
            Private Const _NOMBRECOMPLETO As String = "NombreCompleto"
            Private Const _NOMBREUSUARIO As String = "NombreUsuario"
            Private Const _CODPERSONA As String = "CodPersona"
			Private Const _DNI As String = "Dni"
			Private Const _FOTO As String = "Foto"

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

            Public Shared ReadOnly Property NOMBREUSUARIO() As String
                Get
                    Return _NOMBREUSUARIO
                End Get
            End Property

            Public Shared ReadOnly Property NOMBRECOMPLETO() As String
                Get
                    Return _NOMBRECOMPLETO
                End Get
            End Property

            Public Shared ReadOnly Property CODPERSONA() As String
                Get
                    Return _CODPERSONA
                End Get
            End Property

            Public Shared ReadOnly Property DNI() As String
                Get
                    Return _DNI
                End Get
			End Property

			Public Shared ReadOnly Property FOTO() As String
				Get
					Return _FOTO
				End Get
			End Property


        End Class

#End Region

#Region "Orden"
        <Obsolete("ORDENAR EN LA INTERFAZ")> _
        Public Class SortClass
            Implements System.Collections.Generic.IComparer(Of Usuario)

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
            ''' <param name="user1">Objeto 1 a comparar</param>
            ''' <param name="user2">Objeto 2 a comparar</param>
            ''' <returns></returns>
            Public Function Compare1(ByVal user1 As Usuario, ByVal user2 As Usuario) As Integer Implements System.Collections.Generic.IComparer(Of Usuario).Compare

                If (_direccion = Direction.ASC) Then    'ORDEN ASCENDENTE
                    Select Case _NombreCampo
                        Case "Id"
                            Return user1.Id < user2.Id
                        Case "NombreUsuario"
                            Return user1.NombreUsuario < user2.NombreUsuario
                        Case "Nombre"
                            Return user1.Nombre < user2.Nombre
                        Case "CodPersona"
                            Return user1.CodPersona < user2.CodPersona
                    End Select
                Else                                    'ORDEN DESCENDENTE
                    Select Case _NombreCampo
                        Case "Id"
                            Return user1.Id > user2.Id
                        Case "NombreUsuario"
                            Return user1.NombreUsuario > user2.NombreUsuario
                        Case "Nombre"
                            Return user1.Nombre > user2.Nombre
                        Case "CodPersona"
                            Return user1.CodPersona > user2.CodPersona
                    End Select
                End If
            End Function

        End Class
#End Region

    End Class
End Namespace

