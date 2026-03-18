Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Specialized
Imports AccesoAutomaticoBD

Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public MustInherit Class _TIPONOCONFORMIDADKULTURA
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "INCIDENCIAS."
            Me.QuerySource = "TIPONOCONFORMIDADKULTURA"
            Me.MappingName = "TIPONOCONFORMIDADKULTURA"
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


            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_TIPONOCONFORMIDADKULTURA", parameters)

        End Function

        '=================================================================
        ' Public Overridable Function LoadByPrimaryKey()  As Boolean
        '=================================================================
        '  Loads a single row of via the primary key
        '=================================================================
        Public Overridable Function LoadByPrimaryKey(ByVal IDCULTURA As String, ByVal IDNOCONFORMIDAD As Decimal) As Boolean

            Dim parameters As ListDictionary = New ListDictionary()
            parameters.Add(_TIPONOCONFORMIDADKULTURA.Parameters.IDCULTURA, IDCULTURA)

            parameters.Add(_TIPONOCONFORMIDADKULTURA.Parameters.IDNOCONFORMIDAD, IDNOCONFORMIDAD)


            Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
            p.Direction = ParameterDirection.Output
            parameters.Add(p, Nothing)

            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_TIPONOCONFORMIDADKULTURA", parameters)

        End Function

#Region "Parameters"
        Protected Class Parameters

            Public Shared ReadOnly Property IDNOCONFORMIDAD As OracleParameter
                Get
                    Return New OracleParameter("p_IDNOCONFORMIDAD", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property IDCULTURA As OracleParameter
                Get
                    Return New OracleParameter("p_IDCULTURA", OracleDbType.Varchar2, 5)
                End Get
            End Property

            Public Shared ReadOnly Property TIPONOCONFORMIDAD As OracleParameter
                Get
                    Return New OracleParameter("p_TIPONOCONFORMIDAD", OracleDbType.NVarchar2, 2000)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const IDNOCONFORMIDAD As String = "IDNOCONFORMIDAD"
            Public Const IDCULTURA As String = "IDCULTURA"
            Public Const TIPONOCONFORMIDAD As String = "TIPONOCONFORMIDAD"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(IDNOCONFORMIDAD) = _TIPONOCONFORMIDADKULTURA.PropertyNames.IDNOCONFORMIDAD
                    ht(IDCULTURA) = _TIPONOCONFORMIDADKULTURA.PropertyNames.IDCULTURA
                    ht(TIPONOCONFORMIDAD) = _TIPONOCONFORMIDADKULTURA.PropertyNames.TIPONOCONFORMIDAD

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const IDNOCONFORMIDAD As String = "IDNOCONFORMIDAD"
            Public Const IDCULTURA As String = "IDCULTURA"
            Public Const TIPONOCONFORMIDAD As String = "TIPONOCONFORMIDAD"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(IDNOCONFORMIDAD) = _TIPONOCONFORMIDADKULTURA.ColumnNames.IDNOCONFORMIDAD
                    ht(IDCULTURA) = _TIPONOCONFORMIDADKULTURA.ColumnNames.IDCULTURA
                    ht(TIPONOCONFORMIDAD) = _TIPONOCONFORMIDADKULTURA.ColumnNames.TIPONOCONFORMIDAD

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const IDNOCONFORMIDAD As String = "s_IDNOCONFORMIDAD"
            Public Const IDCULTURA As String = "s_IDCULTURA"
            Public Const TIPONOCONFORMIDAD As String = "s_TIPONOCONFORMIDAD"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property IDNOCONFORMIDAD As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IDNOCONFORMIDAD)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IDNOCONFORMIDAD, Value)
            End Set
        End Property

        Public Overridable Property IDCULTURA As String
            Get
                Return MyBase.GetString(ColumnNames.IDCULTURA)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.IDCULTURA, Value)
            End Set
        End Property

        Public Overridable Property TIPONOCONFORMIDAD As String
            Get
                Return MyBase.GetString(ColumnNames.TIPONOCONFORMIDAD)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.TIPONOCONFORMIDAD, Value)
            End Set
        End Property


#End Region

#Region "String Properties"

        Public Overridable Property s_IDNOCONFORMIDAD As String
            Get
                If Me.IsColumnNull(ColumnNames.IDNOCONFORMIDAD) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.IDNOCONFORMIDAD)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDNOCONFORMIDAD)
                Else
                    Me.IDNOCONFORMIDAD = MyBase.SetDecimalAsString(ColumnNames.IDNOCONFORMIDAD, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_IDCULTURA As String
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

        Public Overridable Property s_TIPONOCONFORMIDAD As String
            Get
                If Me.IsColumnNull(ColumnNames.TIPONOCONFORMIDAD) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.TIPONOCONFORMIDAD)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.TIPONOCONFORMIDAD)
                Else
                    Me.TIPONOCONFORMIDAD = MyBase.SetStringAsString(ColumnNames.TIPONOCONFORMIDAD, Value)
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


                Public ReadOnly Property IDNOCONFORMIDAD() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDNOCONFORMIDAD, Parameters.IDNOCONFORMIDAD)
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

                Public ReadOnly Property TIPONOCONFORMIDAD() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.TIPONOCONFORMIDAD, Parameters.TIPONOCONFORMIDAD)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property


                Private _clause As WhereClause
            End Class
#End Region

            Public ReadOnly Property IDNOCONFORMIDAD() As WhereParameter
                Get
                    If _IDNOCONFORMIDAD_W Is Nothing Then
                        _IDNOCONFORMIDAD_W = TearOff.IDNOCONFORMIDAD
                    End If
                    Return _IDNOCONFORMIDAD_W
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

            Public ReadOnly Property TIPONOCONFORMIDAD() As WhereParameter
                Get
                    If _TIPONOCONFORMIDAD_W Is Nothing Then
                        _TIPONOCONFORMIDAD_W = TearOff.TIPONOCONFORMIDAD
                    End If
                    Return _TIPONOCONFORMIDAD_W
                End Get
            End Property

            Private _IDNOCONFORMIDAD_W As WhereParameter = Nothing
            Private _IDCULTURA_W As WhereParameter = Nothing
            Private _TIPONOCONFORMIDAD_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _IDNOCONFORMIDAD_W = Nothing
                _IDCULTURA_W = Nothing
                _TIPONOCONFORMIDAD_W = Nothing
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


                Public ReadOnly Property IDNOCONFORMIDAD() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDNOCONFORMIDAD, Parameters.IDNOCONFORMIDAD)
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

                Public ReadOnly Property TIPONOCONFORMIDAD() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TIPONOCONFORMIDAD, Parameters.TIPONOCONFORMIDAD)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property


                Private _clause As AggregateClause
            End Class
#End Region

            Public ReadOnly Property IDNOCONFORMIDAD() As AggregateParameter
                Get
                    If _IDNOCONFORMIDAD_W Is Nothing Then
                        _IDNOCONFORMIDAD_W = TearOff.IDNOCONFORMIDAD
                    End If
                    Return _IDNOCONFORMIDAD_W
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

            Public ReadOnly Property TIPONOCONFORMIDAD() As AggregateParameter
                Get
                    If _TIPONOCONFORMIDAD_W Is Nothing Then
                        _TIPONOCONFORMIDAD_W = TearOff.TIPONOCONFORMIDAD
                    End If
                    Return _TIPONOCONFORMIDAD_W
                End Get
            End Property

            Private _IDNOCONFORMIDAD_W As AggregateParameter = Nothing
            Private _IDCULTURA_W As AggregateParameter = Nothing
            Private _TIPONOCONFORMIDAD_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _IDNOCONFORMIDAD_W = Nothing
                _IDCULTURA_W = Nothing
                _TIPONOCONFORMIDAD_W = Nothing
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
            cmd.CommandText = Me.SchemaStoredProcedure + "PI_TIPONOCONFORMIDADKULTURA"

            CreateParameters(cmd)

            Return cmd

        End Function

        Protected Overrides Function GetUpdateCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PU_TIPONOCONFORMIDADKULTURA"

            CreateParameters(cmd)

            Return cmd

        End Function

        Protected Overrides Function GetDeleteCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PD_TIPONOCONFORMIDADKULTURA"

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.IDCULTURA)
            p.SourceColumn = ColumnNames.IDCULTURA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDNOCONFORMIDAD)
            p.SourceColumn = ColumnNames.IDNOCONFORMIDAD
            p.SourceVersion = DataRowVersion.Current


            Return cmd

        End Function

        Private Sub CreateParameters(ByVal cmd As OracleCommand)

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.IDNOCONFORMIDAD)
            p.SourceColumn = ColumnNames.IDNOCONFORMIDAD
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDCULTURA)
            p.SourceColumn = ColumnNames.IDCULTURA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.TIPONOCONFORMIDAD)
            p.SourceColumn = ColumnNames.TIPONOCONFORMIDAD
            p.SourceVersion = DataRowVersion.Current


        End Sub

    End Class

End Namespace

