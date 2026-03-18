
'===============================================================================
'BATZ, Koop. - 15/10/2008 9:34:27
' Generado por MyGeneration Version # (1.3.0.3)
' Generado desde Batz_VbNet_SQL_dOOdads_BusinessEntity.vbgen
' El soporte de la clase OracleClientEntity esta en el directorio Architecture  en "dOOdads".
'===============================================================================

Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Specialized

Imports AccesoAutomaticoBD
Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public MustInherit Class _ACCIONES_EJECUCION
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "INCIDENCIAS."
            Me.QuerySource = "ACCIONES_EJECUCION"
            Me.MappingName = "ACCIONES_EJECUCION"
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


            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_ACCIONES_EJECUCION", parameters)
        End Function

        '=================================================================
        ' Public Overridable Function LoadByPrimaryKey()  As Boolean
        '=================================================================
        '  Loads a single row of via the primary key
        '=================================================================
        Public Overridable Function LoadByPrimaryKey(ByVal IDACCION As Decimal, ByVal IDUSUARIO As Decimal) As Boolean

            Dim parameters As ListDictionary = New ListDictionary()
            parameters.Add(_ACCIONES_EJECUCION.Parameters.IDACCION, IDACCION)

            parameters.Add(_ACCIONES_EJECUCION.Parameters.IDUSUARIO, IDUSUARIO)


            Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
            p.Direction = ParameterDirection.Output
            parameters.Add(p, Nothing)

            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_ACCIONES_EJECUCION", parameters)

        End Function

#Region "Parameters"
        Protected Class Parameters

            Public Shared ReadOnly Property IDACCION As OracleParameter
                Get
                    Return New OracleParameter("p_IDACCION", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property IDUSUARIO As OracleParameter
                Get
                    Return New OracleParameter("p_IDUSUARIO", OracleDbType.Int32, 0)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const IDACCION As String = "IDACCION"
            Public Const IDUSUARIO As String = "IDUSUARIO"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(IDACCION) = _ACCIONES_EJECUCION.PropertyNames.IDACCION
                    ht(IDUSUARIO) = _ACCIONES_EJECUCION.PropertyNames.IDUSUARIO

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const IDACCION As String = "IDACCION"
            Public Const IDUSUARIO As String = "IDUSUARIO"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(IDACCION) = _ACCIONES_EJECUCION.ColumnNames.IDACCION
                    ht(IDUSUARIO) = _ACCIONES_EJECUCION.ColumnNames.IDUSUARIO

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const IDACCION As String = "s_IDACCION"
            Public Const IDUSUARIO As String = "s_IDUSUARIO"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property IDACCION As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IDACCION)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IDACCION, Value)
            End Set
        End Property

        Public Overridable Property IDUSUARIO As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IDUSUARIO)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IDUSUARIO, Value)
            End Set
        End Property


#End Region

#Region "String Properties"

        Public Overridable Property s_IDACCION As String
            Get
                If Me.IsColumnNull(ColumnNames.IDACCION) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.IDACCION)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDACCION)
                Else
                    Me.IDACCION = MyBase.SetDecimalAsString(ColumnNames.IDACCION, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_IDUSUARIO As String
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


                Public ReadOnly Property IDACCION() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDACCION, Parameters.IDACCION)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDUSUARIO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDUSUARIO, Parameters.IDUSUARIO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property


                Private _clause As WhereClause
            End Class
#End Region

            Public ReadOnly Property IDACCION() As WhereParameter
                Get
                    If _IDACCION_W Is Nothing Then
                        _IDACCION_W = TearOff.IDACCION
                    End If
                    Return _IDACCION_W
                End Get
            End Property

            Public ReadOnly Property IDUSUARIO() As WhereParameter
                Get
                    If _IDUSUARIO_W Is Nothing Then
                        _IDUSUARIO_W = TearOff.IDUSUARIO
                    End If
                    Return _IDUSUARIO_W
                End Get
            End Property

            Private _IDACCION_W As WhereParameter = Nothing
            Private _IDUSUARIO_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _IDACCION_W = Nothing
                _IDUSUARIO_W = Nothing
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


                Public ReadOnly Property IDACCION() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDACCION, Parameters.IDACCION)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDUSUARIO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDUSUARIO, Parameters.IDUSUARIO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property


                Private _clause As AggregateClause
            End Class
#End Region

            Public ReadOnly Property IDACCION() As AggregateParameter
                Get
                    If _IDACCION_W Is Nothing Then
                        _IDACCION_W = TearOff.IDACCION
                    End If
                    Return _IDACCION_W
                End Get
            End Property

            Public ReadOnly Property IDUSUARIO() As AggregateParameter
                Get
                    If _IDUSUARIO_W Is Nothing Then
                        _IDUSUARIO_W = TearOff.IDUSUARIO
                    End If
                    Return _IDUSUARIO_W
                End Get
            End Property

            Private _IDACCION_W As AggregateParameter = Nothing
            Private _IDUSUARIO_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _IDACCION_W = Nothing
                _IDUSUARIO_W = Nothing
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
            cmd.CommandText = Me.SchemaStoredProcedure + "PI_ACCIONES_EJECUCION"

            CreateParameters(cmd)


            Return cmd

        End Function

        Protected Overrides Function GetUpdateCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PU_ACCIONES_EJECUCION"

            CreateParameters(cmd)

            Return cmd

        End Function

        Protected Overrides Function GetDeleteCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PD_ACCIONES_EJECUCION"

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.IDACCION)
            p.SourceColumn = ColumnNames.IDACCION
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDUSUARIO)
            p.SourceColumn = ColumnNames.IDUSUARIO
            p.SourceVersion = DataRowVersion.Current


            Return cmd

        End Function

        Private Sub CreateParameters(ByVal cmd As OracleCommand)

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.IDACCION)
            p.SourceColumn = ColumnNames.IDACCION
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDUSUARIO)
            p.SourceColumn = ColumnNames.IDUSUARIO
            p.SourceVersion = DataRowVersion.Current


        End Sub

    End Class

End Namespace

