Imports System
Imports System.Data

Imports System.Collections
Imports System.Collections.Specialized
Imports AccesoAutomaticoBD

Namespace DAL.Views

    Public Class W_RECURSOS_USUARIO
        Inherits OracleClientEntity


        Public Sub New()
            Me.SchemaGlobal = "SAB."
            Me.QuerySource = "W_RECURSOS_USUARIO"
            Me.MappingName = "W_RECURSOS_USUARIO"

            'Decide connection string depending on state
            If Configuration.ConfigurationManager.AppSettings.Get("CurrentStatus") = "Live" Then
                Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("SABLIVE").ConnectionString
            Else
                Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("SABTEST").ConnectionString
            End If
        End Sub

        '=================================================================
        '  	Public Function LoadAll() As Boolean
        '=================================================================
        '  Loads all of the records in the database, and sets the currentRow to the first row
        '=================================================================
        Public Function LoadAll() As Boolean
            Return MyBase.Query.Load()
        End Function

        Public Overrides Sub FlushData()
            Me._whereClause = Nothing
            Me._aggregateClause = Nothing
            MyBase.FlushData()
        End Sub

#Region "Parameters"
        Protected Class Parameters

            Public Shared ReadOnly Property IDUSUARIO() As OracleParameter
                Get
                    Return New OracleParameter("IDUSUARIO", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property IDCULTURA() As OracleParameter
                Get
                    Return New OracleParameter("IDCULTURA", OracleDbType.Varchar2, 5)
                End Get
            End Property

            Public Shared ReadOnly Property IDRECURSO() As OracleParameter
                Get
                    Return New OracleParameter("IDRECURSO", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property RECURSO() As OracleParameter
                Get
                    Return New OracleParameter("RECURSO", OracleDbType.Varchar2, 150)
                End Get
            End Property

            Public Shared ReadOnly Property URL() As OracleParameter
                Get
                    Return New OracleParameter("URL", OracleDbType.Varchar2, 150)
                End Get
            End Property

            Public Shared ReadOnly Property IDPARENT() As OracleParameter
                Get
                    Return New OracleParameter("IDPARENT", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property URL_IMAGEN() As OracleParameter
                Get
                    Return New OracleParameter("URL_IMAGEN", OracleDbType.Varchar2, 150)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const IDUSUARIO As String = "IDUSUARIO"
            Public Const IDCULTURA As String = "IDCULTURA"
            Public Const IDRECURSO As String = "IDRECURSO"
            Public Const RECURSO As String = "RECURSO"
            Public Const URL As String = "URL"
            Public Const IDPARENT As String = "IDPARENT"
            Public Const URL_IMAGEN As String = "URL_IMAGEN"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(IDUSUARIO) = W_RECURSOS_USUARIO.PropertyNames.IDUSUARIO
                    ht(IDCULTURA) = W_RECURSOS_USUARIO.PropertyNames.IDCULTURA
                    ht(IDRECURSO) = W_RECURSOS_USUARIO.PropertyNames.IDRECURSO
                    ht(RECURSO) = W_RECURSOS_USUARIO.PropertyNames.RECURSO
                    ht(URL) = W_RECURSOS_USUARIO.PropertyNames.URL
                    ht(IDPARENT) = W_RECURSOS_USUARIO.PropertyNames.IDPARENT
                    ht(URL_IMAGEN) = W_RECURSOS_USUARIO.PropertyNames.URL_IMAGEN

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const IDUSUARIO As String = "IDUSUARIO"
            Public Const IDCULTURA As String = "IDCULTURA"
            Public Const IDRECURSO As String = "IDRECURSO"
            Public Const RECURSO As String = "RECURSO"
            Public Const URL As String = "URL"
            Public Const IDPARENT As String = "IDPARENT"
            Public Const URL_IMAGEN As String = "URL_IMAGEN"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(IDUSUARIO) = W_RECURSOS_USUARIO.ColumnNames.IDUSUARIO
                    ht(IDCULTURA) = W_RECURSOS_USUARIO.ColumnNames.IDCULTURA
                    ht(IDRECURSO) = W_RECURSOS_USUARIO.ColumnNames.IDRECURSO
                    ht(RECURSO) = W_RECURSOS_USUARIO.ColumnNames.RECURSO
                    ht(URL) = W_RECURSOS_USUARIO.ColumnNames.URL
                    ht(IDPARENT) = W_RECURSOS_USUARIO.ColumnNames.IDPARENT
                    ht(URL_IMAGEN) = W_RECURSOS_USUARIO.ColumnNames.URL_IMAGEN

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const IDUSUARIO As String = "s_IDUSUARIO"
            Public Const IDCULTURA As String = "s_IDCULTURA"
            Public Const IDRECURSO As String = "s_IDRECURSO"
            Public Const RECURSO As String = "s_RECURSO"
            Public Const URL As String = "s_URL"
            Public Const IDPARENT As String = "s_IDPARENT"
            Public Const URL_IMAGEN As String = "s_URL_IMAGEN"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property IDUSUARIO() As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IDUSUARIO)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IDUSUARIO, Value)
            End Set
        End Property

        Public Overridable Property IDCULTURA() As String
            Get
                Return MyBase.GetString(ColumnNames.IDCULTURA)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.IDCULTURA, Value)
            End Set
        End Property

        Public Overridable Property IDRECURSO() As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IDRECURSO)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IDRECURSO, Value)
            End Set
        End Property

        Public Overridable Property RECURSO() As String
            Get
                Return MyBase.GetString(ColumnNames.RECURSO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.RECURSO, Value)
            End Set
        End Property

        Public Overridable Property URL() As String
            Get
                Return MyBase.GetString(ColumnNames.URL)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.URL, Value)
            End Set
        End Property

        Public Overridable Property IDPARENT() As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IDPARENT)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IDPARENT, Value)
            End Set
        End Property

        Public Overridable Property URL_IMAGEN() As String
            Get
                Return MyBase.GetString(ColumnNames.URL_IMAGEN)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.URL_IMAGEN, Value)
            End Set
        End Property


#End Region

#Region "String Properties"

        Public Overridable Property s_IDUSUARIO() As String
            Get
                If Me.IsColumnNull(ColumnNames.IDUSUARIO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.IDUSUARIO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDUSUARIO)
                Else
                    Me.IDUSUARIO = MyBase.SetDecimalAsString(ColumnNames.IDUSUARIO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_IDCULTURA() As String
            Get
                If Me.IsColumnNull(ColumnNames.IDCULTURA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.IDCULTURA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDCULTURA)
                Else
                    Me.IDCULTURA = MyBase.SetStringAsString(ColumnNames.IDCULTURA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_IDRECURSO() As String
            Get
                If Me.IsColumnNull(ColumnNames.IDRECURSO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.IDRECURSO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDRECURSO)
                Else
                    Me.IDRECURSO = MyBase.SetDecimalAsString(ColumnNames.IDRECURSO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_RECURSO() As String
            Get
                If Me.IsColumnNull(ColumnNames.RECURSO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.RECURSO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.RECURSO)
                Else
                    Me.RECURSO = MyBase.SetStringAsString(ColumnNames.RECURSO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_URL() As String
            Get
                If Me.IsColumnNull(ColumnNames.URL) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.URL)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.URL)
                Else
                    Me.URL = MyBase.SetStringAsString(ColumnNames.URL, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_IDPARENT() As String
            Get
                If Me.IsColumnNull(ColumnNames.IDPARENT) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.IDPARENT)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDPARENT)
                Else
                    Me.IDPARENT = MyBase.SetDecimalAsString(ColumnNames.IDPARENT, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_URL_IMAGEN() As String
            Get
                If Me.IsColumnNull(ColumnNames.URL_IMAGEN) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.URL_IMAGEN)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.URL_IMAGEN)
                Else
                    Me.URL_IMAGEN = MyBase.SetStringAsString(ColumnNames.URL_IMAGEN, Value)
                End If
            End Set
        End Property


#End Region

#Region "Where Clause"
        Public Class WhereClause

            Public Sub New(ByVal entity As BusinessEntity)
                Me._entity = entity
            End Sub

            Public ReadOnly Property TearOff() As TearOffWhereParameter
                Get
                    If _tearOff Is Nothing Then
                        _tearOff = New TearOffWhereParameter(Me)
                    End If

                    Return _tearOff
                End Get
            End Property

#Region "TearOff's"
            Public Class TearOffWhereParameter

                Public Sub New(ByVal clause As WhereClause)
                    Me._clause = clause
                End Sub


                Public ReadOnly Property IDUSUARIO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDUSUARIO, Parameters.IDUSUARIO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDCULTURA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDCULTURA, Parameters.IDCULTURA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDRECURSO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDRECURSO, Parameters.IDRECURSO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property RECURSO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.RECURSO, Parameters.RECURSO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property URL() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.URL, Parameters.URL)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDPARENT() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDPARENT, Parameters.IDPARENT)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property URL_IMAGEN() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.URL_IMAGEN, Parameters.URL_IMAGEN)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property


                Private _clause As WhereClause
            End Class
#End Region

            Public ReadOnly Property IDUSUARIO() As WhereParameter
                Get
                    If _IDUSUARIO_W Is Nothing Then
                        _IDUSUARIO_W = TearOff.IDUSUARIO
                    End If
                    Return _IDUSUARIO_W
                End Get
            End Property

            Public ReadOnly Property IDCULTURA() As WhereParameter
                Get
                    If _IDCULTURA_W Is Nothing Then
                        _IDCULTURA_W = TearOff.IDCULTURA
                    End If
                    Return _IDCULTURA_W
                End Get
            End Property

            Public ReadOnly Property IDRECURSO() As WhereParameter
                Get
                    If _IDRECURSO_W Is Nothing Then
                        _IDRECURSO_W = TearOff.IDRECURSO
                    End If
                    Return _IDRECURSO_W
                End Get
            End Property

            Public ReadOnly Property RECURSO() As WhereParameter
                Get
                    If _RECURSO_W Is Nothing Then
                        _RECURSO_W = TearOff.RECURSO
                    End If
                    Return _RECURSO_W
                End Get
            End Property

            Public ReadOnly Property URL() As WhereParameter
                Get
                    If _URL_W Is Nothing Then
                        _URL_W = TearOff.URL
                    End If
                    Return _URL_W
                End Get
            End Property

            Public ReadOnly Property IDPARENT() As WhereParameter
                Get
                    If _IDPARENT_W Is Nothing Then
                        _IDPARENT_W = TearOff.IDPARENT
                    End If
                    Return _IDPARENT_W
                End Get
            End Property

            Public ReadOnly Property URL_IMAGEN() As WhereParameter
                Get
                    If _URL_IMAGEN_W Is Nothing Then
                        _URL_IMAGEN_W = TearOff.URL_IMAGEN
                    End If
                    Return _URL_IMAGEN_W
                End Get
            End Property

            Private _IDUSUARIO_W As WhereParameter = Nothing
            Private _IDCULTURA_W As WhereParameter = Nothing
            Private _IDRECURSO_W As WhereParameter = Nothing
            Private _RECURSO_W As WhereParameter = Nothing
            Private _URL_W As WhereParameter = Nothing
            Private _IDPARENT_W As WhereParameter = Nothing
            Private _URL_IMAGEN_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _IDUSUARIO_W = Nothing
                _IDCULTURA_W = Nothing
                _IDRECURSO_W = Nothing
                _RECURSO_W = Nothing
                _URL_W = Nothing
                _IDPARENT_W = Nothing
                _URL_IMAGEN_W = Nothing
                Me._entity.Query.FlushWhereParameters()

            End Sub

            Private _entity As BusinessEntity
            Private _tearOff As TearOffWhereParameter
        End Class

        Public ReadOnly Property Where() As WhereClause
            Get
                If _whereClause Is Nothing Then
                    _whereClause = New WhereClause(Me)
                End If

                Return _whereClause
            End Get
        End Property

        Private _whereClause As WhereClause = Nothing
#End Region

#Region "Aggregate Clause"
        Public Class AggregateClause

            Public Sub New(ByVal entity As BusinessEntity)
                Me._entity = entity
            End Sub

            Public ReadOnly Property TearOff() As TearOffAggregateParameter
                Get
                    If _tearOff Is Nothing Then
                        _tearOff = New TearOffAggregateParameter(Me)
                    End If

                    Return _tearOff
                End Get
            End Property

#Region "AggregateParameter TearOff's"
            Public Class TearOffAggregateParameter

                Public Sub New(ByVal clause As AggregateClause)
                    Me._clause = clause
                End Sub


                Public ReadOnly Property IDUSUARIO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDUSUARIO, Parameters.IDUSUARIO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDCULTURA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDCULTURA, Parameters.IDCULTURA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDRECURSO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDRECURSO, Parameters.IDRECURSO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property RECURSO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.RECURSO, Parameters.RECURSO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property URL() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.URL, Parameters.URL)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDPARENT() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDPARENT, Parameters.IDPARENT)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property URL_IMAGEN() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.URL_IMAGEN, Parameters.URL_IMAGEN)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property


                Private _clause As AggregateClause
            End Class
#End Region

            Public ReadOnly Property IDUSUARIO() As AggregateParameter
                Get
                    If _IDUSUARIO_W Is Nothing Then
                        _IDUSUARIO_W = TearOff.IDUSUARIO
                    End If
                    Return _IDUSUARIO_W
                End Get
            End Property

            Public ReadOnly Property IDCULTURA() As AggregateParameter
                Get
                    If _IDCULTURA_W Is Nothing Then
                        _IDCULTURA_W = TearOff.IDCULTURA
                    End If
                    Return _IDCULTURA_W
                End Get
            End Property

            Public ReadOnly Property IDRECURSO() As AggregateParameter
                Get
                    If _IDRECURSO_W Is Nothing Then
                        _IDRECURSO_W = TearOff.IDRECURSO
                    End If
                    Return _IDRECURSO_W
                End Get
            End Property

            Public ReadOnly Property RECURSO() As AggregateParameter
                Get
                    If _RECURSO_W Is Nothing Then
                        _RECURSO_W = TearOff.RECURSO
                    End If
                    Return _RECURSO_W
                End Get
            End Property

            Public ReadOnly Property URL() As AggregateParameter
                Get
                    If _URL_W Is Nothing Then
                        _URL_W = TearOff.URL
                    End If
                    Return _URL_W
                End Get
            End Property

            Public ReadOnly Property IDPARENT() As AggregateParameter
                Get
                    If _IDPARENT_W Is Nothing Then
                        _IDPARENT_W = TearOff.IDPARENT
                    End If
                    Return _IDPARENT_W
                End Get
            End Property

            Public ReadOnly Property URL_IMAGEN() As AggregateParameter
                Get
                    If _URL_IMAGEN_W Is Nothing Then
                        _URL_IMAGEN_W = TearOff.URL_IMAGEN
                    End If
                    Return _URL_IMAGEN_W
                End Get
            End Property

            Private _IDUSUARIO_W As AggregateParameter = Nothing
            Private _IDCULTURA_W As AggregateParameter = Nothing
            Private _IDRECURSO_W As AggregateParameter = Nothing
            Private _RECURSO_W As AggregateParameter = Nothing
            Private _URL_W As AggregateParameter = Nothing
            Private _IDPARENT_W As AggregateParameter = Nothing
            Private _URL_IMAGEN_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _IDUSUARIO_W = Nothing
                _IDCULTURA_W = Nothing
                _IDRECURSO_W = Nothing
                _RECURSO_W = Nothing
                _URL_W = Nothing
                _IDPARENT_W = Nothing
                _URL_IMAGEN_W = Nothing
                Me._entity.Query.FlushAggregateParameters()

            End Sub

            Private _entity As BusinessEntity
            Private _tearOff As TearOffAggregateParameter
        End Class

        Public ReadOnly Property Aggregate() As AggregateClause
            Get
                If _aggregateClause Is Nothing Then
                    _aggregateClause = New AggregateClause(Me)
                End If

                Return _aggregateClause
            End Get
        End Property

        Private _aggregateClause As AggregateClause = Nothing
#End Region

        Protected Overrides Function GetInsertCommand() As IDbCommand
            Return Nothing
        End Function

        Protected Overrides Function GetUpdateCommand() As IDbCommand
            Return Nothing
        End Function

        Protected Overrides Function GetDeleteCommand() As IDbCommand
            Return Nothing
        End Function

    End Class


End Namespace

