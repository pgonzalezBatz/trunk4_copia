Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Specialized
Imports AccesoAutomaticoBD

Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public MustInherit Class _GERTAKARIAK_TIPONOCONFORMIDAD
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "INCIDENCIAS."
            Me.QuerySource = "GERTAKARIAK_TIPONOCONFORMIDAD"
            Me.MappingName = "GERTAKARIAK_TIPONOCONFORMIDAD"
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


            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_GERTAKARIAK_TIPONOCONFORMIDAD", parameters)

        End Function

        '=================================================================
        ' Public Overridable Function LoadByPrimaryKey()  As Boolean
        '=================================================================
        '  Loads a single row of via the primary key
        '=================================================================
        Public Overridable Function LoadByPrimaryKey(ByVal IDINCIDENCIA As Decimal, ByVal IDTIPONOCONFORMIDAD As Decimal) As Boolean

            Dim parameters As ListDictionary = New ListDictionary()
            parameters.Add(_GERTAKARIAK_TIPONOCONFORMIDAD.Parameters.IDINCIDENCIA, IDINCIDENCIA)

            parameters.Add(_GERTAKARIAK_TIPONOCONFORMIDAD.Parameters.IDTIPONOCONFORMIDAD, IDTIPONOCONFORMIDAD)


            Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
            p.Direction = ParameterDirection.Output
            parameters.Add(p, Nothing)

            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_GERTAKARIAK_TIPONOCONFORMIDAD", parameters)

        End Function

#Region "Parameters"
        Protected Class Parameters

            Public Shared ReadOnly Property IDINCIDENCIA As OracleParameter
                Get
                    Return New OracleParameter("p_IDINCIDENCIA", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property IDTIPONOCONFORMIDAD() As OracleParameter
                Get
                    Return New OracleParameter("p_IDTIPONOCONFORMIDAD", OracleDbType.Int32, 0)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const IDINCIDENCIA As String = "IDINCIDENCIA"
            Public Const IDTIPONOCONFORMIDAD As String = "IDTIPONOCONFORMIDAD"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(IDINCIDENCIA) = _GERTAKARIAK_TIPONOCONFORMIDAD.PropertyNames.IDINCIDENCIA
                    ht(IDTIPONOCONFORMIDAD) = _GERTAKARIAK_TIPONOCONFORMIDAD.PropertyNames.IDTIPONOCONFORMIDAD

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const IDINCIDENCIA As String = "IDINCIDENCIA"
            Public Const IDTIPONOCONFORMIDAD As String = "IDTIPONOCONFORMIDAD"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(IDINCIDENCIA) = _GERTAKARIAK_TIPONOCONFORMIDAD.ColumnNames.IDINCIDENCIA
                    ht(IDTIPONOCONFORMIDAD) = _GERTAKARIAK_TIPONOCONFORMIDAD.ColumnNames.IDTIPONOCONFORMIDAD

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const IDINCIDENCIA As String = "s_IDINCIDENCIA"
            Public Const IDTIPONOCONFORMIDAD As String = "s_IDTIPONOCONFORMIDAD"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property IDINCIDENCIA As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IDINCIDENCIA)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IDINCIDENCIA, Value)
            End Set
        End Property

        Public Overridable Property IDTIPONOCONFORMIDAD As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IDTIPONOCONFORMIDAD)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IDTIPONOCONFORMIDAD, Value)
            End Set
        End Property


#End Region

#Region "String Properties"

        Public Overridable Property s_IDINCIDENCIA As String
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

        Public Overridable Property s_IDTIPONOCONFORMIDAD As String
            Get
                If Me.IsColumnNull(ColumnNames.IDTIPONOCONFORMIDAD) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.IDTIPONOCONFORMIDAD)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDTIPONOCONFORMIDAD)
                Else
                    Me.IDTIPONOCONFORMIDAD = MyBase.SetDecimalAsString(ColumnNames.IDTIPONOCONFORMIDAD, Value)
                End If
            End Set
        End Property


#End Region

#Region "Where Clause"
        Public Class WhereClause

            Public Sub New(ByVal entity As BusinessEntity)
                Me._entity = entity
            End Sub

            Public ReadOnly Property TearOff As TearOffWhereParameter
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


                Public ReadOnly Property IDINCIDENCIA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDINCIDENCIA, Parameters.IDINCIDENCIA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDTIPONOCONFORMIDAD() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDTIPONOCONFORMIDAD, Parameters.IDTIPONOCONFORMIDAD)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property


                Private _clause As WhereClause
            End Class
#End Region

            Public ReadOnly Property IDINCIDENCIA() As WhereParameter
                Get
                    If _IDINCIDENCIA_W Is Nothing Then
                        _IDINCIDENCIA_W = TearOff.IDINCIDENCIA
                    End If
                    Return _IDINCIDENCIA_W
                End Get
            End Property

            Public ReadOnly Property IDTIPONOCONFORMIDAD() As WhereParameter
                Get
                    If _IDTIPONOCONFORMIDAD_W Is Nothing Then
                        _IDTIPONOCONFORMIDAD_W = TearOff.IDTIPONOCONFORMIDAD
                    End If
                    Return _IDTIPONOCONFORMIDAD_W
                End Get
            End Property

            Private _IDINCIDENCIA_W As WhereParameter = Nothing
            Private _IDTIPONOCONFORMIDAD_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _IDINCIDENCIA_W = Nothing
                _IDTIPONOCONFORMIDAD_W = Nothing
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

            Public ReadOnly Property TearOff As TearOffAggregateParameter
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


                Public ReadOnly Property IDINCIDENCIA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDINCIDENCIA, Parameters.IDINCIDENCIA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDTIPONOCONFORMIDAD() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDTIPONOCONFORMIDAD, Parameters.IDTIPONOCONFORMIDAD)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property


                Private _clause As AggregateClause
            End Class
#End Region

            Public ReadOnly Property IDINCIDENCIA() As AggregateParameter
                Get
                    If _IDINCIDENCIA_W Is Nothing Then
                        _IDINCIDENCIA_W = TearOff.IDINCIDENCIA
                    End If
                    Return _IDINCIDENCIA_W
                End Get
            End Property

            Public ReadOnly Property IDTIPONOCONFORMIDAD() As AggregateParameter
                Get
                    If _IDTIPONOCONFORMIDAD_W Is Nothing Then
                        _IDTIPONOCONFORMIDAD_W = TearOff.IDTIPONOCONFORMIDAD
                    End If
                    Return _IDTIPONOCONFORMIDAD_W
                End Get
            End Property

            Private _IDINCIDENCIA_W As AggregateParameter = Nothing
            Private _IDTIPONOCONFORMIDAD_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _IDINCIDENCIA_W = Nothing
                _IDTIPONOCONFORMIDAD_W = Nothing
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
            cmd.CommandText = Me.SchemaStoredProcedure + "PI_GERTAKARIAK_TIPONOCONFORMIDAD"

            CreateParameters(cmd)

            Return cmd

        End Function

        Protected Overrides Function GetUpdateCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PU_GERTAKARIAK_TIPONOCONFORMIDAD"

            CreateParameters(cmd)

            Return cmd

        End Function

        Protected Overrides Function GetDeleteCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PD_GERTAKARIAK_TIPONOCONFORMIDAD"

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.IDINCIDENCIA)
            p.SourceColumn = ColumnNames.IDINCIDENCIA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDTIPONOCONFORMIDAD)
            p.SourceColumn = ColumnNames.IDTIPONOCONFORMIDAD
            p.SourceVersion = DataRowVersion.Current


            Return cmd

        End Function

        Private Sub CreateParameters(ByVal cmd As OracleCommand)

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.IDINCIDENCIA)
            p.SourceColumn = ColumnNames.IDINCIDENCIA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDTIPONOCONFORMIDAD)
            p.SourceColumn = ColumnNames.IDTIPONOCONFORMIDAD
            p.SourceVersion = DataRowVersion.Current


        End Sub

    End Class

End Namespace

