
'===============================================================================
'BATZ, Koop. - 03/12/2008 9:36:41
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

    Public MustInherit Class _CAPACIDADES
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "SAB."
            Me.QuerySource = "CAPACIDADES"
            Me.MappingName = "CAPACIDADES"
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


            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_CAPACIDADES", parameters)
        End Function

        '=================================================================
        ' Public Overridable Function LoadByPrimaryKey()  As Boolean
        '=================================================================
        '  Loads a single row of via the primary key
        '=================================================================
        Public Overridable Function LoadByPrimaryKey(ByVal CAPID As String) As Boolean

            Dim parameters As ListDictionary = New ListDictionary()
            parameters.Add(_CAPACIDADES.Parameters.CAPID, CAPID)


            Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
            p.Direction = ParameterDirection.Output
            parameters.Add(p, Nothing)

            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_CAPACIDADES", parameters)

        End Function

#Region "Parameters"
        Protected Class Parameters

            Public Shared ReadOnly Property CAPID As OracleParameter
                Get
                    Return New OracleParameter("p_CAPID", OracleDbType.Varchar2, 25)
                End Get
            End Property

            Public Shared ReadOnly Property NOMBRE As OracleParameter
                Get
                    Return New OracleParameter("p_NOMBRE", OracleDbType.NVarchar2, 40)
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

            Public Const CAPID As String = "CAPID"
            Public Const NOMBRE As String = "NOMBRE"
            Public Const OBSOLETO As String = "OBSOLETO"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(CAPID) = _CAPACIDADES.PropertyNames.CAPID
                    ht(NOMBRE) = _CAPACIDADES.PropertyNames.NOMBRE
                    ht(OBSOLETO) = _CAPACIDADES.PropertyNames.OBSOLETO

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const CAPID As String = "CAPID"
            Public Const NOMBRE As String = "NOMBRE"
            Public Const OBSOLETO As String = "OBSOLETO"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(CAPID) = _CAPACIDADES.ColumnNames.CAPID
                    ht(NOMBRE) = _CAPACIDADES.ColumnNames.NOMBRE
                    ht(OBSOLETO) = _CAPACIDADES.ColumnNames.OBSOLETO

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const CAPID As String = "s_CAPID"
            Public Const NOMBRE As String = "s_NOMBRE"
            Public Const OBSOLETO As String = "s_OBSOLETO"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property CAPID As String
            Get
                Return MyBase.GetString(ColumnNames.CAPID)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CAPID, Value)
            End Set
        End Property

        Public Overridable Property NOMBRE As String
            Get
                Return MyBase.GetString(ColumnNames.NOMBRE)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.NOMBRE, Value)
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

        Public Overridable Property s_CAPID As String
            Get
                If Me.IsColumnNull(ColumnNames.CAPID) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CAPID)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CAPID)
                Else
                    Me.CAPID = MyBase.SetStringAsString(ColumnNames.CAPID, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_NOMBRE As String
            Get
                If Me.IsColumnNull(ColumnNames.NOMBRE) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.NOMBRE)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NOMBRE)
                Else
                    Me.NOMBRE = MyBase.SetStringAsString(ColumnNames.NOMBRE, Value)
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


                Public ReadOnly Property CAPID() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CAPID, Parameters.CAPID)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NOMBRE() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NOMBRE, Parameters.NOMBRE)
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

            Public ReadOnly Property CAPID() As WhereParameter
                Get
                    If _CAPID_W Is Nothing Then
                        _CAPID_W = TearOff.CAPID
                    End If
                    Return _CAPID_W
                End Get
            End Property

            Public ReadOnly Property NOMBRE() As WhereParameter
                Get
                    If _NOMBRE_W Is Nothing Then
                        _NOMBRE_W = TearOff.NOMBRE
                    End If
                    Return _NOMBRE_W
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

            Private _CAPID_W As WhereParameter = Nothing
            Private _NOMBRE_W As WhereParameter = Nothing
            Private _OBSOLETO_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _CAPID_W = Nothing
                _NOMBRE_W = Nothing
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


                Public ReadOnly Property CAPID() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CAPID, Parameters.CAPID)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NOMBRE() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NOMBRE, Parameters.NOMBRE)
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

            Public ReadOnly Property CAPID() As AggregateParameter
                Get
                    If _CAPID_W Is Nothing Then
                        _CAPID_W = TearOff.CAPID
                    End If
                    Return _CAPID_W
                End Get
            End Property

            Public ReadOnly Property NOMBRE() As AggregateParameter
                Get
                    If _NOMBRE_W Is Nothing Then
                        _NOMBRE_W = TearOff.NOMBRE
                    End If
                    Return _NOMBRE_W
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

            Private _CAPID_W As AggregateParameter = Nothing
            Private _NOMBRE_W As AggregateParameter = Nothing
            Private _OBSOLETO_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _CAPID_W = Nothing
                _NOMBRE_W = Nothing
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
            cmd.CommandText = Me.SchemaStoredProcedure + "PI_CAPACIDADES"

            CreateParameters(cmd)


            Return cmd

        End Function

        Protected Overrides Function GetUpdateCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PU_CAPACIDADES"

            CreateParameters(cmd)

            Return cmd

        End Function

        Protected Overrides Function GetDeleteCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PD_CAPACIDADES"

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.CAPID)
            p.SourceColumn = ColumnNames.CAPID
            p.SourceVersion = DataRowVersion.Current


            Return cmd

        End Function

        Private Sub CreateParameters(ByVal cmd As OracleCommand)

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.CAPID)
            p.SourceColumn = ColumnNames.CAPID
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NOMBRE)
            p.SourceColumn = ColumnNames.NOMBRE
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.OBSOLETO)
            p.SourceColumn = ColumnNames.OBSOLETO
            p.SourceVersion = DataRowVersion.Current


        End Sub

    End Class

End Namespace

