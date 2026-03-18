
'===============================================================================
'BATZ, Koop. - 20/11/2008 8:19:29
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

    Public MustInherit Class _EMPRESAS_CAPACIDADES
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "SAB."
            Me.QuerySource = "EMPRESAS_CAPACIDADES"
            Me.MappingName = "EMPRESAS_CAPACIDADES"
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


            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_EMPRESAS_CAPACIDADES", parameters)
        End Function

        '=================================================================
        ' Public Overridable Function LoadByPrimaryKey()  As Boolean
        '=================================================================
        '  Loads a single row of via the primary key
        '=================================================================
        Public Overridable Function LoadByPrimaryKey(ByVal ID_CAPACIDADES As String, ByVal ID_EMPRESA As Decimal) As Boolean

            Dim parameters As ListDictionary = New ListDictionary()
            parameters.Add(_EMPRESAS_CAPACIDADES.Parameters.ID_CAPACIDADES, ID_CAPACIDADES)

            parameters.Add(_EMPRESAS_CAPACIDADES.Parameters.ID_EMPRESA, ID_EMPRESA)


            Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
            p.Direction = ParameterDirection.Output
            parameters.Add(p, Nothing)

            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_EMPRESAS_CAPACIDADES", parameters)

        End Function

#Region "Parameters"
        Protected Class Parameters

            Public Shared ReadOnly Property ID_EMPRESA As OracleParameter
                Get
                    Return New OracleParameter("p_ID_EMPRESA", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property ID_CAPACIDADES As OracleParameter
                Get
                    Return New OracleParameter("p_ID_CAPACIDADES", OracleDbType.Varchar2, 25)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const ID_EMPRESA As String = "ID_EMPRESA"
            Public Const ID_CAPACIDADES As String = "ID_CAPACIDADES"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(ID_EMPRESA) = _EMPRESAS_CAPACIDADES.PropertyNames.ID_EMPRESA
                    ht(ID_CAPACIDADES) = _EMPRESAS_CAPACIDADES.PropertyNames.ID_CAPACIDADES

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const ID_EMPRESA As String = "ID_EMPRESA"
            Public Const ID_CAPACIDADES As String = "ID_CAPACIDADES"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(ID_EMPRESA) = _EMPRESAS_CAPACIDADES.ColumnNames.ID_EMPRESA
                    ht(ID_CAPACIDADES) = _EMPRESAS_CAPACIDADES.ColumnNames.ID_CAPACIDADES

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const ID_EMPRESA As String = "s_ID_EMPRESA"
            Public Const ID_CAPACIDADES As String = "s_ID_CAPACIDADES"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property ID_EMPRESA As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.ID_EMPRESA)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.ID_EMPRESA, Value)
            End Set
        End Property

        Public Overridable Property ID_CAPACIDADES As String
            Get
                Return MyBase.GetString(ColumnNames.ID_CAPACIDADES)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.ID_CAPACIDADES, Value)
            End Set
        End Property


#End Region

#Region "String Properties"

        Public Overridable Property s_ID_EMPRESA As String
            Get
                If Me.IsColumnNull(ColumnNames.ID_EMPRESA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.ID_EMPRESA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.ID_EMPRESA)
                Else
                    Me.ID_EMPRESA = MyBase.SetDecimalAsString(ColumnNames.ID_EMPRESA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_ID_CAPACIDADES As String
            Get
                If Me.IsColumnNull(ColumnNames.ID_CAPACIDADES) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.ID_CAPACIDADES)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.ID_CAPACIDADES)
                Else
                    Me.ID_CAPACIDADES = MyBase.SetStringAsString(ColumnNames.ID_CAPACIDADES, Value)
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


                Public ReadOnly Property ID_EMPRESA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_EMPRESA, Parameters.ID_EMPRESA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ID_CAPACIDADES() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_CAPACIDADES, Parameters.ID_CAPACIDADES)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property


                Private _clause As WhereClause
            End Class
#End Region

            Public ReadOnly Property ID_EMPRESA() As WhereParameter
                Get
                    If _ID_EMPRESA_W Is Nothing Then
                        _ID_EMPRESA_W = TearOff.ID_EMPRESA
                    End If
                    Return _ID_EMPRESA_W
                End Get
            End Property

            Public ReadOnly Property ID_CAPACIDADES() As WhereParameter
                Get
                    If _ID_CAPACIDADES_W Is Nothing Then
                        _ID_CAPACIDADES_W = TearOff.ID_CAPACIDADES
                    End If
                    Return _ID_CAPACIDADES_W
                End Get
            End Property

            Private _ID_EMPRESA_W As WhereParameter = Nothing
            Private _ID_CAPACIDADES_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _ID_EMPRESA_W = Nothing
                _ID_CAPACIDADES_W = Nothing
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


                Public ReadOnly Property ID_EMPRESA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_EMPRESA, Parameters.ID_EMPRESA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ID_CAPACIDADES() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_CAPACIDADES, Parameters.ID_CAPACIDADES)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property


                Private _clause As AggregateClause
            End Class
#End Region

            Public ReadOnly Property ID_EMPRESA() As AggregateParameter
                Get
                    If _ID_EMPRESA_W Is Nothing Then
                        _ID_EMPRESA_W = TearOff.ID_EMPRESA
                    End If
                    Return _ID_EMPRESA_W
                End Get
            End Property

            Public ReadOnly Property ID_CAPACIDADES() As AggregateParameter
                Get
                    If _ID_CAPACIDADES_W Is Nothing Then
                        _ID_CAPACIDADES_W = TearOff.ID_CAPACIDADES
                    End If
                    Return _ID_CAPACIDADES_W
                End Get
            End Property

            Private _ID_EMPRESA_W As AggregateParameter = Nothing
            Private _ID_CAPACIDADES_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _ID_EMPRESA_W = Nothing
                _ID_CAPACIDADES_W = Nothing
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
            cmd.CommandText = Me.SchemaStoredProcedure + "PI_EMPRESAS_CAPACIDADES"

            CreateParameters(cmd)


            Return cmd

        End Function

        Protected Overrides Function GetUpdateCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PU_EMPRESAS_CAPACIDADES"

            CreateParameters(cmd)

            Return cmd

        End Function

        Protected Overrides Function GetDeleteCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PD_EMPRESAS_CAPACIDADES"

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.ID_CAPACIDADES)
            p.SourceColumn = ColumnNames.ID_CAPACIDADES
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.ID_EMPRESA)
            p.SourceColumn = ColumnNames.ID_EMPRESA
            p.SourceVersion = DataRowVersion.Current


            Return cmd

        End Function

        Private Sub CreateParameters(ByVal cmd As OracleCommand)

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.ID_EMPRESA)
            p.SourceColumn = ColumnNames.ID_EMPRESA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.ID_CAPACIDADES)
            p.SourceColumn = ColumnNames.ID_CAPACIDADES
            p.SourceVersion = DataRowVersion.Current


        End Sub

    End Class

End Namespace

