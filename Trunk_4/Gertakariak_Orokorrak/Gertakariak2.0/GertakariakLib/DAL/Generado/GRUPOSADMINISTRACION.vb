
'===============================================================================
'BATZ, Koop. - 04/11/2008 8:17:21
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

    Public MustInherit Class _GRUPOSADMINISTRACION
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "INCIDENCIAS."
            Me.QuerySource = "GRUPOSADMINISTRACION"
            Me.MappingName = "GRUPOSADMINISTRACION"
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


            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_GRUPOSADMINISTRACION", parameters)
        End Function

        '=================================================================
        ' Public Overridable Function LoadByPrimaryKey()  As Boolean
        '=================================================================
        '  Loads a single row of via the primary key
        '=================================================================
        Public Overridable Function LoadByPrimaryKey(ByVal IDGRUPO As Decimal) As Boolean

            Dim parameters As ListDictionary = New ListDictionary()
            parameters.Add(_GRUPOSADMINISTRACION.Parameters.IDGRUPO, IDGRUPO)


            Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
            p.Direction = ParameterDirection.Output
            parameters.Add(p, Nothing)

            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_GRUPOSADMINISTRACION", parameters)

        End Function

#Region "Parameters"
        Protected Class Parameters

            Public Shared ReadOnly Property IDGRUPO As OracleParameter
                Get
                    Return New OracleParameter("p_IDGRUPO", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property LKZGRUPO As OracleParameter
                Get
                    Return New OracleParameter("p_LKZGRUPO", OracleDbType.NVarchar2, 20)
                End Get
            End Property

            Public Shared ReadOnly Property LKZDESCRIPCION As OracleParameter
                Get
                    Return New OracleParameter("p_LKZDESCRIPCION", OracleDbType.NVarchar2, 30)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const IDGRUPO As String = "IDGRUPO"
            Public Const LKZGRUPO As String = "LKZGRUPO"
            Public Const LKZDESCRIPCION As String = "LKZDESCRIPCION"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(IDGRUPO) = _GRUPOSADMINISTRACION.PropertyNames.IDGRUPO
                    ht(LKZGRUPO) = _GRUPOSADMINISTRACION.PropertyNames.LKZGRUPO
                    ht(LKZDESCRIPCION) = _GRUPOSADMINISTRACION.PropertyNames.LKZDESCRIPCION

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const IDGRUPO As String = "IDGRUPO"
            Public Const LKZGRUPO As String = "LKZGRUPO"
            Public Const LKZDESCRIPCION As String = "LKZDESCRIPCION"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(IDGRUPO) = _GRUPOSADMINISTRACION.ColumnNames.IDGRUPO
                    ht(LKZGRUPO) = _GRUPOSADMINISTRACION.ColumnNames.LKZGRUPO
                    ht(LKZDESCRIPCION) = _GRUPOSADMINISTRACION.ColumnNames.LKZDESCRIPCION

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const IDGRUPO As String = "s_IDGRUPO"
            Public Const LKZGRUPO As String = "s_LKZGRUPO"
            Public Const LKZDESCRIPCION As String = "s_LKZDESCRIPCION"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property IDGRUPO As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IDGRUPO)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IDGRUPO, Value)
            End Set
        End Property

        Public Overridable Property LKZGRUPO As String
            Get
                Return MyBase.GetString(ColumnNames.LKZGRUPO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.LKZGRUPO, Value)
            End Set
        End Property

        Public Overridable Property LKZDESCRIPCION As String
            Get
                Return MyBase.GetString(ColumnNames.LKZDESCRIPCION)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.LKZDESCRIPCION, Value)
            End Set
        End Property


#End Region

#Region "String Properties"

        Public Overridable Property s_IDGRUPO As String
            Get
                If Me.IsColumnNull(ColumnNames.IDGRUPO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.IDGRUPO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDGRUPO)
                Else
                    Me.IDGRUPO = MyBase.SetDecimalAsString(ColumnNames.IDGRUPO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_LKZGRUPO As String
            Get
                If Me.IsColumnNull(ColumnNames.LKZGRUPO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.LKZGRUPO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.LKZGRUPO)
                Else
                    Me.LKZGRUPO = MyBase.SetStringAsString(ColumnNames.LKZGRUPO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_LKZDESCRIPCION As String
            Get
                If Me.IsColumnNull(ColumnNames.LKZDESCRIPCION) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.LKZDESCRIPCION)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.LKZDESCRIPCION)
                Else
                    Me.LKZDESCRIPCION = MyBase.SetStringAsString(ColumnNames.LKZDESCRIPCION, Value)
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


                Public ReadOnly Property IDGRUPO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDGRUPO, Parameters.IDGRUPO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property LKZGRUPO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.LKZGRUPO, Parameters.LKZGRUPO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property LKZDESCRIPCION() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.LKZDESCRIPCION, Parameters.LKZDESCRIPCION)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property


                Private _clause As WhereClause
            End Class
#End Region

            Public ReadOnly Property IDGRUPO() As WhereParameter
                Get
                    If _IDGRUPO_W Is Nothing Then
                        _IDGRUPO_W = TearOff.IDGRUPO
                    End If
                    Return _IDGRUPO_W
                End Get
            End Property

            Public ReadOnly Property LKZGRUPO() As WhereParameter
                Get
                    If _LKZGRUPO_W Is Nothing Then
                        _LKZGRUPO_W = TearOff.LKZGRUPO
                    End If
                    Return _LKZGRUPO_W
                End Get
            End Property

            Public ReadOnly Property LKZDESCRIPCION() As WhereParameter
                Get
                    If _LKZDESCRIPCION_W Is Nothing Then
                        _LKZDESCRIPCION_W = TearOff.LKZDESCRIPCION
                    End If
                    Return _LKZDESCRIPCION_W
                End Get
            End Property

            Private _IDGRUPO_W As WhereParameter = Nothing
            Private _LKZGRUPO_W As WhereParameter = Nothing
            Private _LKZDESCRIPCION_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _IDGRUPO_W = Nothing
                _LKZGRUPO_W = Nothing
                _LKZDESCRIPCION_W = Nothing
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


                Public ReadOnly Property IDGRUPO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDGRUPO, Parameters.IDGRUPO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property LKZGRUPO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.LKZGRUPO, Parameters.LKZGRUPO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property LKZDESCRIPCION() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.LKZDESCRIPCION, Parameters.LKZDESCRIPCION)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property


                Private _clause As AggregateClause
            End Class
#End Region

            Public ReadOnly Property IDGRUPO() As AggregateParameter
                Get
                    If _IDGRUPO_W Is Nothing Then
                        _IDGRUPO_W = TearOff.IDGRUPO
                    End If
                    Return _IDGRUPO_W
                End Get
            End Property

            Public ReadOnly Property LKZGRUPO() As AggregateParameter
                Get
                    If _LKZGRUPO_W Is Nothing Then
                        _LKZGRUPO_W = TearOff.LKZGRUPO
                    End If
                    Return _LKZGRUPO_W
                End Get
            End Property

            Public ReadOnly Property LKZDESCRIPCION() As AggregateParameter
                Get
                    If _LKZDESCRIPCION_W Is Nothing Then
                        _LKZDESCRIPCION_W = TearOff.LKZDESCRIPCION
                    End If
                    Return _LKZDESCRIPCION_W
                End Get
            End Property

            Private _IDGRUPO_W As AggregateParameter = Nothing
            Private _LKZGRUPO_W As AggregateParameter = Nothing
            Private _LKZDESCRIPCION_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _IDGRUPO_W = Nothing
                _LKZGRUPO_W = Nothing
                _LKZDESCRIPCION_W = Nothing
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
            cmd.CommandText = Me.SchemaStoredProcedure + "PI_GRUPOSADMINISTRACION"

            CreateParameters(cmd)


            Dim p As OracleParameter
            p = cmd.Parameters(Parameters.IDGRUPO.ParameterName)
            p.Direction = ParameterDirection.Output

            Return cmd

        End Function

        Protected Overrides Function GetUpdateCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PU_GRUPOSADMINISTRACION"

            CreateParameters(cmd)

            Return cmd

        End Function

        Protected Overrides Function GetDeleteCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PD_GRUPOSADMINISTRACION"

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.IDGRUPO)
            p.SourceColumn = ColumnNames.IDGRUPO
            p.SourceVersion = DataRowVersion.Current


            Return cmd

        End Function

        Private Sub CreateParameters(ByVal cmd As OracleCommand)

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.IDGRUPO)
            p.SourceColumn = ColumnNames.IDGRUPO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.LKZGRUPO)
            p.SourceColumn = ColumnNames.LKZGRUPO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.LKZDESCRIPCION)
            p.SourceColumn = ColumnNames.LKZDESCRIPCION
            p.SourceVersion = DataRowVersion.Current


        End Sub

    End Class

End Namespace

