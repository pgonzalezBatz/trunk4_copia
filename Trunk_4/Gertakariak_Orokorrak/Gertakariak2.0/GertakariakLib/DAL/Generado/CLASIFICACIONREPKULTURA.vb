
'===============================================================================
'BATZ, Koop. - 10/12/2008 15:12:34
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

    Public MustInherit Class _CLASIFICACIONREPKULTURA
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "INCIDENCIAS."
            Me.QuerySource = "CLASIFICACIONREPKULTURA"
            Me.MappingName = "CLASIFICACIONREPKULTURA"
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


            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_CLASIFICACIONREPKULTURA", parameters)
        End Function

        '=================================================================
        ' Public Overridable Function LoadByPrimaryKey()  As Boolean
        '=================================================================
        '  Loads a single row of via the primary key
        '=================================================================
        Public Overridable Function LoadByPrimaryKey(ByVal ID As Decimal, ByVal IDCULTURA As String) As Boolean

            Dim parameters As ListDictionary = New ListDictionary()
            parameters.Add(_CLASIFICACIONREPKULTURA.Parameters.ID, ID)

            parameters.Add(_CLASIFICACIONREPKULTURA.Parameters.IDCULTURA, IDCULTURA)


            Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
            p.Direction = ParameterDirection.Output
            parameters.Add(p, Nothing)

            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_CLASIFICACIONREPKULTURA", parameters)

        End Function

#Region "Parameters"
        Protected Class Parameters

            Public Shared ReadOnly Property ID As OracleParameter
                Get
                    Return New OracleParameter("p_ID", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property IDCULTURA As OracleParameter
                Get
                    Return New OracleParameter("p_IDCULTURA", OracleDbType.Varchar2, 5)
                End Get
            End Property

            Public Shared ReadOnly Property DESCRIPCION As OracleParameter
                Get
                    Return New OracleParameter("p_DESCRIPCION", OracleDbType.NVarchar2, 2000)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const ID As String = "ID"
            Public Const IDCULTURA As String = "IDCULTURA"
            Public Const DESCRIPCION As String = "DESCRIPCION"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(ID) = _CLASIFICACIONREPKULTURA.PropertyNames.ID
                    ht(IDCULTURA) = _CLASIFICACIONREPKULTURA.PropertyNames.IDCULTURA
                    ht(DESCRIPCION) = _CLASIFICACIONREPKULTURA.PropertyNames.DESCRIPCION

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const ID As String = "ID"
            Public Const IDCULTURA As String = "IDCULTURA"
            Public Const DESCRIPCION As String = "DESCRIPCION"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(ID) = _CLASIFICACIONREPKULTURA.ColumnNames.ID
                    ht(IDCULTURA) = _CLASIFICACIONREPKULTURA.ColumnNames.IDCULTURA
                    ht(DESCRIPCION) = _CLASIFICACIONREPKULTURA.ColumnNames.DESCRIPCION

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const ID As String = "s_ID"
            Public Const IDCULTURA As String = "s_IDCULTURA"
            Public Const DESCRIPCION As String = "s_DESCRIPCION"

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

        Public Overridable Property IDCULTURA As String
            Get
                Return MyBase.GetString(ColumnNames.IDCULTURA)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.IDCULTURA, Value)
            End Set
        End Property

        Public Overridable Property DESCRIPCION As String
            Get
                Return MyBase.GetString(ColumnNames.DESCRIPCION)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.DESCRIPCION, Value)
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

        Public Overridable Property s_DESCRIPCION As String
            Get
                If Me.IsColumnNull(ColumnNames.DESCRIPCION) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.DESCRIPCION)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.DESCRIPCION)
                Else
                    Me.DESCRIPCION = MyBase.SetStringAsString(ColumnNames.DESCRIPCION, Value)
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

                Public ReadOnly Property IDCULTURA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDCULTURA, Parameters.IDCULTURA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DESCRIPCION() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.DESCRIPCION, Parameters.DESCRIPCION)
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

            Public ReadOnly Property IDCULTURA() As WhereParameter
                Get
                    If _IDCULTURA_W Is Nothing Then
                        _IDCULTURA_W = TearOff.IDCULTURA
                    End If
                    Return _IDCULTURA_W
                End Get
            End Property

            Public ReadOnly Property DESCRIPCION() As WhereParameter
                Get
                    If _DESCRIPCION_W Is Nothing Then
                        _DESCRIPCION_W = TearOff.DESCRIPCION
                    End If
                    Return _DESCRIPCION_W
                End Get
            End Property

            Private _ID_W As WhereParameter = Nothing
            Private _IDCULTURA_W As WhereParameter = Nothing
            Private _DESCRIPCION_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _ID_W = Nothing
                _IDCULTURA_W = Nothing
                _DESCRIPCION_W = Nothing
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

                Public ReadOnly Property IDCULTURA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDCULTURA, Parameters.IDCULTURA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DESCRIPCION() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DESCRIPCION, Parameters.DESCRIPCION)
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

            Public ReadOnly Property IDCULTURA() As AggregateParameter
                Get
                    If _IDCULTURA_W Is Nothing Then
                        _IDCULTURA_W = TearOff.IDCULTURA
                    End If
                    Return _IDCULTURA_W
                End Get
            End Property

            Public ReadOnly Property DESCRIPCION() As AggregateParameter
                Get
                    If _DESCRIPCION_W Is Nothing Then
                        _DESCRIPCION_W = TearOff.DESCRIPCION
                    End If
                    Return _DESCRIPCION_W
                End Get
            End Property

            Private _ID_W As AggregateParameter = Nothing
            Private _IDCULTURA_W As AggregateParameter = Nothing
            Private _DESCRIPCION_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _ID_W = Nothing
                _IDCULTURA_W = Nothing
                _DESCRIPCION_W = Nothing
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
            cmd.CommandText = Me.SchemaStoredProcedure + "PI_CLASIFICACIONREPKULTURA"

            CreateParameters(cmd)


            Return cmd

        End Function

        Protected Overrides Function GetUpdateCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PU_CLASIFICACIONREPKULTURA"

            CreateParameters(cmd)

            Return cmd

        End Function

        Protected Overrides Function GetDeleteCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PD_CLASIFICACIONREPKULTURA"

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.ID)
            p.SourceColumn = ColumnNames.ID
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDCULTURA)
            p.SourceColumn = ColumnNames.IDCULTURA
            p.SourceVersion = DataRowVersion.Current


            Return cmd

        End Function

        Private Sub CreateParameters(ByVal cmd As OracleCommand)

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.ID)
            p.SourceColumn = ColumnNames.ID
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDCULTURA)
            p.SourceColumn = ColumnNames.IDCULTURA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.DESCRIPCION)
            p.SourceColumn = ColumnNames.DESCRIPCION
            p.SourceVersion = DataRowVersion.Current


        End Sub

    End Class

End Namespace

