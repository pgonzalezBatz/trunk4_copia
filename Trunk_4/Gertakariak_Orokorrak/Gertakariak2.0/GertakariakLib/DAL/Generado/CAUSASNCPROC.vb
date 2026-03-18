
'===============================================================================
'BATZ, Koop. - 20/01/2009 15:10:09
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

    Public MustInherit Class _CAUSASNCPROC
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "INCIDENCIAS."
            Me.QuerySource = "CAUSASNCPROC"
            Me.MappingName = "CAUSASNCPROC"
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


            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_CAUSASNCPROC", parameters)
        End Function

        '=================================================================
        ' Public Overridable Function LoadByPrimaryKey()  As Boolean
        '=================================================================
        '  Loads a single row of via the primary key
        '=================================================================
        Public Overridable Function LoadByPrimaryKey(ByVal CODPROC As String, ByVal IDCAUSA As Decimal) As Boolean

            Dim parameters As ListDictionary = New ListDictionary()
            parameters.Add(_CAUSASNCPROC.Parameters.CODPROC, CODPROC)

            parameters.Add(_CAUSASNCPROC.Parameters.IDCAUSA, IDCAUSA)


            Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
            p.Direction = ParameterDirection.Output
            parameters.Add(p, Nothing)

            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_CAUSASNCPROC", parameters)

        End Function

#Region "Parameters"
        Protected Class Parameters

            Public Shared ReadOnly Property IDCAUSA As OracleParameter
                Get
                    Return New OracleParameter("p_IDCAUSA", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property CODPROC As OracleParameter
                Get
                    Return New OracleParameter("p_CODPROC", OracleDbType.Char, 4)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const IDCAUSA As String = "IDCAUSA"
            Public Const CODPROC As String = "CODPROC"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(IDCAUSA) = _CAUSASNCPROC.PropertyNames.IDCAUSA
                    ht(CODPROC) = _CAUSASNCPROC.PropertyNames.CODPROC

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const IDCAUSA As String = "IDCAUSA"
            Public Const CODPROC As String = "CODPROC"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(IDCAUSA) = _CAUSASNCPROC.ColumnNames.IDCAUSA
                    ht(CODPROC) = _CAUSASNCPROC.ColumnNames.CODPROC

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const IDCAUSA As String = "s_IDCAUSA"
            Public Const CODPROC As String = "s_CODPROC"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property IDCAUSA As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IDCAUSA)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IDCAUSA, Value)
            End Set
        End Property

        Public Overridable Property CODPROC As String
            Get
                Return MyBase.GetString(ColumnNames.CODPROC)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CODPROC, Value)
            End Set
        End Property


#End Region

#Region "String Properties"

        Public Overridable Property s_IDCAUSA As String
            Get
                If Me.IsColumnNull(ColumnNames.IDCAUSA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.IDCAUSA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDCAUSA)
                Else
                    Me.IDCAUSA = MyBase.SetDecimalAsString(ColumnNames.IDCAUSA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CODPROC As String
            Get
                If Me.IsColumnNull(ColumnNames.CODPROC) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CODPROC)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CODPROC)
                Else
                    Me.CODPROC = MyBase.SetStringAsString(ColumnNames.CODPROC, Value)
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


                Public ReadOnly Property IDCAUSA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDCAUSA, Parameters.IDCAUSA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODPROC() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CODPROC, Parameters.CODPROC)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property


                Private _clause As WhereClause
            End Class
#End Region

            Public ReadOnly Property IDCAUSA() As WhereParameter
                Get
                    If _IDCAUSA_W Is Nothing Then
                        _IDCAUSA_W = TearOff.IDCAUSA
                    End If
                    Return _IDCAUSA_W
                End Get
            End Property

            Public ReadOnly Property CODPROC() As WhereParameter
                Get
                    If _CODPROC_W Is Nothing Then
                        _CODPROC_W = TearOff.CODPROC
                    End If
                    Return _CODPROC_W
                End Get
            End Property

            Private _IDCAUSA_W As WhereParameter = Nothing
            Private _CODPROC_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _IDCAUSA_W = Nothing
                _CODPROC_W = Nothing
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


                Public ReadOnly Property IDCAUSA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDCAUSA, Parameters.IDCAUSA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODPROC() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CODPROC, Parameters.CODPROC)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property


                Private _clause As AggregateClause
            End Class
#End Region

            Public ReadOnly Property IDCAUSA() As AggregateParameter
                Get
                    If _IDCAUSA_W Is Nothing Then
                        _IDCAUSA_W = TearOff.IDCAUSA
                    End If
                    Return _IDCAUSA_W
                End Get
            End Property

            Public ReadOnly Property CODPROC() As AggregateParameter
                Get
                    If _CODPROC_W Is Nothing Then
                        _CODPROC_W = TearOff.CODPROC
                    End If
                    Return _CODPROC_W
                End Get
            End Property

            Private _IDCAUSA_W As AggregateParameter = Nothing
            Private _CODPROC_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _IDCAUSA_W = Nothing
                _CODPROC_W = Nothing
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
            cmd.CommandText = Me.SchemaStoredProcedure + "PI_CAUSASNCPROC"

            CreateParameters(cmd)


            Return cmd

        End Function

        Protected Overrides Function GetUpdateCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PU_CAUSASNCPROC"

            CreateParameters(cmd)

            Return cmd

        End Function

        Protected Overrides Function GetDeleteCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PD_CAUSASNCPROC"

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.CODPROC)
            p.SourceColumn = ColumnNames.CODPROC
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDCAUSA)
            p.SourceColumn = ColumnNames.IDCAUSA
            p.SourceVersion = DataRowVersion.Current


            Return cmd

        End Function

        Private Sub CreateParameters(ByVal cmd As OracleCommand)

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.IDCAUSA)
            p.SourceColumn = ColumnNames.IDCAUSA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CODPROC)
            p.SourceColumn = ColumnNames.CODPROC
            p.SourceVersion = DataRowVersion.Current


        End Sub

    End Class

End Namespace

