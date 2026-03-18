Namespace ELL

    Public Class OperacionKaplanPrisma

#Region "Variables miembro"

        Private _id As Integer = Integer.MinValue
        Private _codOperacionKaplan As Integer = Integer.MinValue
        Private _codOperacionPrisma As Integer = Integer.MinValue
        Private _descripcionPrisma As String = String.Empty

#End Region

#Region "Properties"

        ''' <summary>
        ''' Id del departamento
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
        ''' Código de operación de Kaplan
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property CodOperacionKaplan() As Integer
            Get
                Return _codOperacionKaplan
            End Get
            Set(ByVal value As Integer)
                _codOperacionKaplan = value
            End Set
        End Property

        ''' <summary>
        ''' Código de operación de Prisma
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property CodOperacionPrisma() As Integer
            Get
                Return _codOperacionPrisma
            End Get
            Set(ByVal value As Integer)
                _codOperacionPrisma = value
            End Set
        End Property

        ''' <summary>
        ''' Descripción de operación de Prisma
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property DescripcionPrisma() As String
            Get
                Return _descripcionPrisma
            End Get
            Set(ByVal value As String)
                _descripcionPrisma = value
            End Set
        End Property
#End Region

    End Class

End Namespace

