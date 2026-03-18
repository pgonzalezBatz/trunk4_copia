Imports System
Imports System.Data

Imports System.Collections
Imports System.Collections.Specialized
Imports AccesoAutomaticoBD
Imports log4net

Namespace DAL.Views

    Public Class W_GRUPOS_USUARIO
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "SAB."
            Me.QuerySource = "W_GRUPOS_USUARIO"
            Me.MappingName = "W_GRUPOS_USUARIO"

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

            Public Shared ReadOnly Property IDUSUARIOS() As OracleParameter
                Get
                    Return New OracleParameter("IDUSUARIOS", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property IDGRUPOS() As OracleParameter
                Get
                    Return New OracleParameter("IDGRUPOS", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property IDCULTURAS() As OracleParameter
                Get
                    Return New OracleParameter("IDCULTURAS", OracleDbType.NVarchar2, 5)
                End Get
            End Property

            Public Shared ReadOnly Property GRUPO() As OracleParameter
                Get
                    Return New OracleParameter("GRUPO", OracleDbType.NVarchar2, 150)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const IDUSUARIOS As String = "IDUSUARIOS"
            Public Const IDGRUPOS As String = "IDGRUPOS"
            Public Const IDCULTURAS As String = "IDCULTURAS"
            Public Const GRUPO As String = "GRUPO"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(IDUSUARIOS) = W_GRUPOS_USUARIO.PropertyNames.IDUSUARIOS
                    ht(IDGRUPOS) = W_GRUPOS_USUARIO.PropertyNames.IDGRUPOS
                    ht(IDCULTURAS) = W_GRUPOS_USUARIO.PropertyNames.IDCULTURAS
                    ht(GRUPO) = W_GRUPOS_USUARIO.PropertyNames.GRUPO

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const IDUSUARIOS As String = "IDUSUARIOS"
            Public Const IDGRUPOS As String = "IDGRUPOS"
            Public Const IDCULTURAS As String = "IDCULTURAS"
            Public Const GRUPO As String = "GRUPO"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(IDUSUARIOS) = W_GRUPOS_USUARIO.ColumnNames.IDUSUARIOS
                    ht(IDGRUPOS) = W_GRUPOS_USUARIO.ColumnNames.IDGRUPOS
                    ht(IDCULTURAS) = W_GRUPOS_USUARIO.ColumnNames.IDCULTURAS
                    ht(GRUPO) = W_GRUPOS_USUARIO.ColumnNames.GRUPO

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const IDUSUARIOS As String = "s_IDUSUARIOS"
            Public Const IDGRUPOS As String = "s_IDGRUPOS"
            Public Const IDCULTURAS As String = "s_IDCULTURAS"
            Public Const GRUPO As String = "s_GRUPO"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property IDUSUARIOS() As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IDUSUARIOS)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IDUSUARIOS, Value)
            End Set
        End Property

        Public Overridable Property IDGRUPOS() As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IDGRUPOS)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IDGRUPOS, Value)
            End Set
        End Property

        Public Overridable Property IDCULTURAS() As String
            Get
                Return MyBase.GetString(ColumnNames.IDCULTURAS)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.IDCULTURAS, Value)
            End Set
        End Property

        Public Overridable Property GRUPO() As String
            Get
                Return MyBase.GetString(ColumnNames.GRUPO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.GRUPO, Value)
            End Set
        End Property


#End Region

#Region "String Properties"

        Public Overridable Property s_IDUSUARIOS() As String
            Get
                If Me.IsColumnNull(ColumnNames.IDUSUARIOS) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.IDUSUARIOS)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDUSUARIOS)
                Else
                    Me.IDUSUARIOS = MyBase.SetDecimalAsString(ColumnNames.IDUSUARIOS, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_IDGRUPOS() As String
            Get
                If Me.IsColumnNull(ColumnNames.IDGRUPOS) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.IDGRUPOS)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDGRUPOS)
                Else
                    Me.IDGRUPOS = MyBase.SetDecimalAsString(ColumnNames.IDGRUPOS, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_IDCULTURAS() As String
            Get
                If Me.IsColumnNull(ColumnNames.IDCULTURAS) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.IDCULTURAS)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDCULTURAS)
                Else
                    Me.IDCULTURAS = MyBase.SetStringAsString(ColumnNames.IDCULTURAS, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_GRUPO() As String
            Get
                If Me.IsColumnNull(ColumnNames.GRUPO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.GRUPO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.GRUPO)
                Else
                    Me.GRUPO = MyBase.SetStringAsString(ColumnNames.GRUPO, Value)
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


                Public ReadOnly Property IDUSUARIOS() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDUSUARIOS, Parameters.IDUSUARIOS)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDGRUPOS() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDGRUPOS, Parameters.IDGRUPOS)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDCULTURAS() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDCULTURAS, Parameters.IDCULTURAS)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property GRUPO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.GRUPO, Parameters.GRUPO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property


                Private _clause As WhereClause
            End Class
#End Region

            Public ReadOnly Property IDUSUARIOS() As WhereParameter
                Get
                    If _IDUSUARIOS_W Is Nothing Then
                        _IDUSUARIOS_W = TearOff.IDUSUARIOS
                    End If
                    Return _IDUSUARIOS_W
                End Get
            End Property

            Public ReadOnly Property IDGRUPOS() As WhereParameter
                Get
                    If _IDGRUPOS_W Is Nothing Then
                        _IDGRUPOS_W = TearOff.IDGRUPOS
                    End If
                    Return _IDGRUPOS_W
                End Get
            End Property

            Public ReadOnly Property IDCULTURAS() As WhereParameter
                Get
                    If _IDCULTURAS_W Is Nothing Then
                        _IDCULTURAS_W = TearOff.IDCULTURAS
                    End If
                    Return _IDCULTURAS_W
                End Get
            End Property

            Public ReadOnly Property GRUPO() As WhereParameter
                Get
                    If _GRUPO_W Is Nothing Then
                        _GRUPO_W = TearOff.GRUPO
                    End If
                    Return _GRUPO_W
                End Get
            End Property

            Private _IDUSUARIOS_W As WhereParameter = Nothing
            Private _IDGRUPOS_W As WhereParameter = Nothing
            Private _IDCULTURAS_W As WhereParameter = Nothing
            Private _GRUPO_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _IDUSUARIOS_W = Nothing
                _IDGRUPOS_W = Nothing
                _IDCULTURAS_W = Nothing
                _GRUPO_W = Nothing
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


                Public ReadOnly Property IDUSUARIOS() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDUSUARIOS, Parameters.IDUSUARIOS)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDGRUPOS() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDGRUPOS, Parameters.IDGRUPOS)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDCULTURAS() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDCULTURAS, Parameters.IDCULTURAS)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property GRUPO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.GRUPO, Parameters.GRUPO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property


                Private _clause As AggregateClause
            End Class
#End Region

            Public ReadOnly Property IDUSUARIOS() As AggregateParameter
                Get
                    If _IDUSUARIOS_W Is Nothing Then
                        _IDUSUARIOS_W = TearOff.IDUSUARIOS
                    End If
                    Return _IDUSUARIOS_W
                End Get
            End Property

            Public ReadOnly Property IDGRUPOS() As AggregateParameter
                Get
                    If _IDGRUPOS_W Is Nothing Then
                        _IDGRUPOS_W = TearOff.IDGRUPOS
                    End If
                    Return _IDGRUPOS_W
                End Get
            End Property

            Public ReadOnly Property IDCULTURAS() As AggregateParameter
                Get
                    If _IDCULTURAS_W Is Nothing Then
                        _IDCULTURAS_W = TearOff.IDCULTURAS
                    End If
                    Return _IDCULTURAS_W
                End Get
            End Property

            Public ReadOnly Property GRUPO() As AggregateParameter
                Get
                    If _GRUPO_W Is Nothing Then
                        _GRUPO_W = TearOff.GRUPO
                    End If
                    Return _GRUPO_W
                End Get
            End Property

            Private _IDUSUARIOS_W As AggregateParameter = Nothing
            Private _IDGRUPOS_W As AggregateParameter = Nothing
            Private _IDCULTURAS_W As AggregateParameter = Nothing
            Private _GRUPO_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _IDUSUARIOS_W = Nothing
                _IDGRUPOS_W = Nothing
                _IDCULTURAS_W = Nothing
                _GRUPO_W = Nothing
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

