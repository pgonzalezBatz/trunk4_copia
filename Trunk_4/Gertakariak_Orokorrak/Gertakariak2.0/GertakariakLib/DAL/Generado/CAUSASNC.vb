
'===============================================================================
'BATZ, Koop. - 14/09/2010 7:58:48
' Generado por MyGeneration Version # (1.3.0.9)
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

    Public MustInherit Class _CAUSASNC
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "INCIDENCIAS."
            Me.QuerySource = "CAUSASNC"
            Me.MappingName = "CAUSASNC"
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


            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_CAUSASNC", parameters)
        End Function

        '=================================================================
        ' Public Overridable Function LoadByPrimaryKey()  As Boolean
        '=================================================================
        '  Loads a single row of via the primary key
        '=================================================================
        Public Overridable Function LoadByPrimaryKey(ByVal ID As Decimal) As Boolean

            Dim parameters As ListDictionary = New ListDictionary()
            parameters.Add(_CAUSASNC.Parameters.ID, ID)


            Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
            p.Direction = ParameterDirection.Output
            parameters.Add(p, Nothing)

            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_CAUSASNC", parameters)

        End Function

#Region "Parameters"
        Protected Class Parameters

            Public Shared ReadOnly Property ID As OracleParameter
                Get
                    Return New OracleParameter("p_ID", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property ORDEN As OracleParameter
                Get
                    Return New OracleParameter("p_ORDEN", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property OBSOLETO As OracleParameter
                Get
                    Return New OracleParameter("p_OBSOLETO", OracleDbType.Int32, 0)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const ID As String = "ID"
            Public Const ORDEN As String = "ORDEN"
            Public Const OBSOLETO As String = "OBSOLETO"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(ID) = _CAUSASNC.PropertyNames.ID
                    ht(ORDEN) = _CAUSASNC.PropertyNames.ORDEN
                    ht(OBSOLETO) = _CAUSASNC.PropertyNames.OBSOLETO

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const ID As String = "ID"
            Public Const ORDEN As String = "ORDEN"
            Public Const OBSOLETO As String = "OBSOLETO"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(ID) = _CAUSASNC.ColumnNames.ID
                    ht(ORDEN) = _CAUSASNC.ColumnNames.ORDEN
                    ht(OBSOLETO) = _CAUSASNC.ColumnNames.OBSOLETO

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const ID As String = "s_ID"
            Public Const ORDEN As String = "s_ORDEN"
            Public Const OBSOLETO As String = "s_OBSOLETO"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property ID As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.ID)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.ID, Value)
            End Set
        End Property

        Public Overridable Property ORDEN As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.ORDEN)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.ORDEN, Value)
            End Set
        End Property

        Public Overridable Property OBSOLETO As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.OBSOLETO)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.OBSOLETO, Value)
            End Set
        End Property


#End Region

#Region "String Properties"

        Public Overridable Property s_ID As String
            Get
                If Me.IsColumnNull(ColumnNames.ID) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.ID)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.ID)
                Else
                    Me.ID = MyBase.SetDecimalAsString(ColumnNames.ID, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_ORDEN As String
            Get
                If Me.IsColumnNull(ColumnNames.ORDEN) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.ORDEN)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.ORDEN)
                Else
                    Me.ORDEN = MyBase.SetDecimalAsString(ColumnNames.ORDEN, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_OBSOLETO As String
            Get
                If Me.IsColumnNull(ColumnNames.OBSOLETO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.OBSOLETO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.OBSOLETO)
                Else
                    Me.OBSOLETO = MyBase.SetDecimalAsString(ColumnNames.OBSOLETO, Value)
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


                Public ReadOnly Property ID() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.ID, Parameters.ID)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ORDEN() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.ORDEN, Parameters.ORDEN)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property OBSOLETO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.OBSOLETO, Parameters.OBSOLETO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property


                Private _clause As WhereClause
            End Class
#End Region

            Public ReadOnly Property ID() As WhereParameter
                Get
                    If _ID_W Is Nothing Then
                        _ID_W = TearOff.ID
                    End If
                    Return _ID_W
                End Get
            End Property

            Public ReadOnly Property ORDEN() As WhereParameter
                Get
                    If _ORDEN_W Is Nothing Then
                        _ORDEN_W = TearOff.ORDEN
                    End If
                    Return _ORDEN_W
                End Get
            End Property

            Public ReadOnly Property OBSOLETO() As WhereParameter
                Get
                    If _OBSOLETO_W Is Nothing Then
                        _OBSOLETO_W = TearOff.OBSOLETO
                    End If
                    Return _OBSOLETO_W
                End Get
            End Property

            Private _ID_W As WhereParameter = Nothing
            Private _ORDEN_W As WhereParameter = Nothing
            Private _OBSOLETO_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _ID_W = Nothing
                _ORDEN_W = Nothing
                _OBSOLETO_W = Nothing
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


                Public ReadOnly Property ID() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID, Parameters.ID)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ORDEN() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ORDEN, Parameters.ORDEN)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property OBSOLETO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.OBSOLETO, Parameters.OBSOLETO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property


                Private _clause As AggregateClause
            End Class
#End Region

            Public ReadOnly Property ID() As AggregateParameter
                Get
                    If _ID_W Is Nothing Then
                        _ID_W = TearOff.ID
                    End If
                    Return _ID_W
                End Get
            End Property

            Public ReadOnly Property ORDEN() As AggregateParameter
                Get
                    If _ORDEN_W Is Nothing Then
                        _ORDEN_W = TearOff.ORDEN
                    End If
                    Return _ORDEN_W
                End Get
            End Property

            Public ReadOnly Property OBSOLETO() As AggregateParameter
                Get
                    If _OBSOLETO_W Is Nothing Then
                        _OBSOLETO_W = TearOff.OBSOLETO
                    End If
                    Return _OBSOLETO_W
                End Get
            End Property

            Private _ID_W As AggregateParameter = Nothing
            Private _ORDEN_W As AggregateParameter = Nothing
            Private _OBSOLETO_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _ID_W = Nothing
                _ORDEN_W = Nothing
                _OBSOLETO_W = Nothing
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
            cmd.CommandText = Me.SchemaStoredProcedure + "PI_CAUSASNC"

            CreateParameters(cmd)


            Dim p As OracleParameter
            p = cmd.Parameters(Parameters.ID.ParameterName)
            p.Direction = ParameterDirection.Output
            p.DbType = DbType.Decimal

            Return cmd

        End Function

        Protected Overrides Function GetUpdateCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PU_CAUSASNC"

            CreateParameters(cmd)

            Return cmd

        End Function

        Protected Overrides Function GetDeleteCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PD_CAUSASNC"

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.ID)
            p.SourceColumn = ColumnNames.ID
            p.SourceVersion = DataRowVersion.Current


            Return cmd

        End Function

        Private Sub CreateParameters(ByVal cmd As OracleCommand)

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.ID)
            p.SourceColumn = ColumnNames.ID
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.ORDEN)
            p.SourceColumn = ColumnNames.ORDEN
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.OBSOLETO)
            p.SourceColumn = ColumnNames.OBSOLETO
            p.SourceVersion = DataRowVersion.Current


        End Sub

    End Class

End Namespace

