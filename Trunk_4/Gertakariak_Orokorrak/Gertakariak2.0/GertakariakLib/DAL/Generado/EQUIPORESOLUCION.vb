Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Specialized
Imports AccesoAutomaticoBD

Imports Oracle.ManagedDataAccess.Client

Namespace DAL
    Public MustInherit Class _EQUIPORESOLUCION
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "INCIDENCIAS."
            Me.QuerySource = "EQUIPORESOLUCION"
            Me.MappingName = "EQUIPORESOLUCION"
        End Sub

        '=================================================================
        '  Public Overrides Sub AddNew()
        '=================================================================
        '
        '=================================================================
        Public Overrides Sub AddNew()
            MyBase.AddNew()

        End Sub

        Public Overrides Sub FlushData()
            Me._whereClause = Nothing
            Me._aggregateClause = Nothing
            MyBase.FlushData()
        End Sub


        '=================================================================
        '  	Public Function LoadAll() As Boolean
        '=================================================================
        '  Loads all of the records in the database, and sets the currentRow to the first row
        '=================================================================
        Public Function LoadAll() As Boolean

            Dim parameters As ListDictionary = Nothing

            parameters = New ListDictionary
            Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
            p.Direction = ParameterDirection.Output
            parameters.Add(p, Nothing)


            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_EQUIPORESOLUCION", parameters)

        End Function

        '=================================================================
        ' Public Overridable Function LoadByPrimaryKey()  As Boolean
        '=================================================================
        '  Loads a single row of via the primary key
        '=================================================================
        Public Overridable Function LoadByPrimaryKey(ByVal IDASISTENTE As Decimal, ByVal IDINCIDENCIA As Decimal) As Boolean

            Dim parameters As ListDictionary = New ListDictionary()
            parameters.Add(_EQUIPORESOLUCION.Parameters.IDASISTENTE, IDASISTENTE)

            parameters.Add(_EQUIPORESOLUCION.Parameters.IDINCIDENCIA, IDINCIDENCIA)


            Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
            p.Direction = ParameterDirection.Output
            parameters.Add(p, Nothing)

            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_EQUIPORESOLUCION", parameters)

        End Function

#Region "Parameters"
        Protected Class Parameters

            Public Shared ReadOnly Property IDASISTENTE() As OracleParameter
                Get
                    Return New OracleParameter("p_IDASISTENTE", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property IDINCIDENCIA() As OracleParameter
                Get
                    Return New OracleParameter("p_IDINCIDENCIA", OracleDbType.Int32, 0)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const IDASISTENTE As String = "IDASISTENTE"
            Public Const IDINCIDENCIA As String = "IDINCIDENCIA"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(IDASISTENTE) = _EQUIPORESOLUCION.PropertyNames.IDASISTENTE
                    ht(IDINCIDENCIA) = _EQUIPORESOLUCION.PropertyNames.IDINCIDENCIA

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const IDASISTENTE As String = "IDASISTENTE"
            Public Const IDINCIDENCIA As String = "IDINCIDENCIA"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(IDASISTENTE) = _EQUIPORESOLUCION.ColumnNames.IDASISTENTE
                    ht(IDINCIDENCIA) = _EQUIPORESOLUCION.ColumnNames.IDINCIDENCIA

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const IDASISTENTE As String = "s_IDASISTENTE"
            Public Const IDINCIDENCIA As String = "s_IDINCIDENCIA"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property IDASISTENTE() As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IDASISTENTE)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IDASISTENTE, Value)
            End Set
        End Property

        Public Overridable Property IDINCIDENCIA() As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IDINCIDENCIA)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IDINCIDENCIA, Value)
            End Set
        End Property


#End Region

#Region "String Properties"

        Public Overridable Property s_IDASISTENTE() As String
            Get
                If Me.IsColumnNull(ColumnNames.IDASISTENTE) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.IDASISTENTE)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDASISTENTE)
                Else
                    Me.IDASISTENTE = MyBase.SetDecimalAsString(ColumnNames.IDASISTENTE, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_IDINCIDENCIA() As String
            Get
                If Me.IsColumnNull(ColumnNames.IDINCIDENCIA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.IDINCIDENCIA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDINCIDENCIA)
                Else
                    Me.IDINCIDENCIA = MyBase.SetDecimalAsString(ColumnNames.IDINCIDENCIA, Value)
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


                Public ReadOnly Property IDASISTENTE() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDASISTENTE, Parameters.IDASISTENTE)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDINCIDENCIA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDINCIDENCIA, Parameters.IDINCIDENCIA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property


                Private _clause As WhereClause
            End Class
#End Region

            Public ReadOnly Property IDASISTENTE() As WhereParameter
                Get
                    If _IDASISTENTE_W Is Nothing Then
                        _IDASISTENTE_W = TearOff.IDASISTENTE
                    End If
                    Return _IDASISTENTE_W
                End Get
            End Property

            Public ReadOnly Property IDINCIDENCIA() As WhereParameter
                Get
                    If _IDINCIDENCIA_W Is Nothing Then
                        _IDINCIDENCIA_W = TearOff.IDINCIDENCIA
                    End If
                    Return _IDINCIDENCIA_W
                End Get
            End Property

            Private _IDASISTENTE_W As WhereParameter = Nothing
            Private _IDINCIDENCIA_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _IDASISTENTE_W = Nothing
                _IDINCIDENCIA_W = Nothing
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


                Public ReadOnly Property IDASISTENTE() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDASISTENTE, Parameters.IDASISTENTE)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDINCIDENCIA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDINCIDENCIA, Parameters.IDINCIDENCIA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property


                Private _clause As AggregateClause
            End Class
#End Region

            Public ReadOnly Property IDASISTENTE() As AggregateParameter
                Get
                    If _IDASISTENTE_W Is Nothing Then
                        _IDASISTENTE_W = TearOff.IDASISTENTE
                    End If
                    Return _IDASISTENTE_W
                End Get
            End Property

            Public ReadOnly Property IDINCIDENCIA() As AggregateParameter
                Get
                    If _IDINCIDENCIA_W Is Nothing Then
                        _IDINCIDENCIA_W = TearOff.IDINCIDENCIA
                    End If
                    Return _IDINCIDENCIA_W
                End Get
            End Property

            Private _IDASISTENTE_W As AggregateParameter = Nothing
            Private _IDINCIDENCIA_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _IDASISTENTE_W = Nothing
                _IDINCIDENCIA_W = Nothing
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

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PI_EQUIPORESOLUCION"

            CreateParameters(cmd)

            Return cmd

        End Function

        Protected Overrides Function GetUpdateCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PU_EQUIPORESOLUCION"

            CreateParameters(cmd)

            Return cmd

        End Function

        Protected Overrides Function GetDeleteCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PD_EQUIPORESOLUCION"

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.IDASISTENTE)
            p.SourceColumn = ColumnNames.IDASISTENTE
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDINCIDENCIA)
            p.SourceColumn = ColumnNames.IDINCIDENCIA
            p.SourceVersion = DataRowVersion.Current


            Return cmd

        End Function

        Private Sub CreateParameters(ByVal cmd As OracleCommand)

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.IDASISTENTE)
            p.SourceColumn = ColumnNames.IDASISTENTE
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDINCIDENCIA)
            p.SourceColumn = ColumnNames.IDINCIDENCIA
            p.SourceVersion = DataRowVersion.Current


        End Sub

    End Class

End Namespace

