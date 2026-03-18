Namespace ELL

    ''' <summary>
    ''' Grupo de extension: extensiones que dependiendo de las 2 primeras cifras de las mismas, indican a que planta estan asociadas
    ''' </summary>
    ''' <remarks></remarks>
    Public Class GrupoExtension
        Inherits Tipo

#Region "Variables miembro"

        Private _idPlanta As Integer = Integer.MinValue
        Private _planta As String = String.Empty
        Private _libre As Boolean = False

#End Region

#Region "Propiedades"

        ''' <summary>
        ''' Identificador de la planta a la que pertenece
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
        ''' Nombre de la planta a la que pertenece
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


        ''' <summary>
        ''' Indica si un grupo de extension es libre o esta asociado a una planta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Libre() As Boolean
            Get
                Return _libre
            End Get
            Set(ByVal value As Boolean)
                _libre = value
            End Set
        End Property

#End Region

#Region "SortClass"

        ''' <summary>
        ''' Clase de ordenacion
        ''' </summary>
        Public Class SortClass
            Implements System.Collections.Generic.IComparer(Of GrupoExtension)

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
            Public Function Compare1(ByVal ge1 As GrupoExtension, ByVal ge2 As GrupoExtension) As Integer Implements System.Collections.Generic.IComparer(Of GrupoExtension).Compare

                If (_direccion = Direction.ASC) Then    'ORDEN ASCENDENTE
                    Select Case _NombreCampo.ToLower
                        Case "nombre"
                            Return ge1.Nombre < ge2.Nombre
                        Case "planta"
                            Return ge1.Planta < ge2.Planta
                    End Select
                Else                                    'ORDEN DESCENDENTE
                    Select Case _NombreCampo.ToLower
                        Case "nombre"
                            Return ge1.Nombre > ge2.Nombre
                        Case "planta"
                            Return ge1.Planta > ge2.Planta
                    End Select
                End If
            End Function

        End Class

#End Region

    End Class

End Namespace