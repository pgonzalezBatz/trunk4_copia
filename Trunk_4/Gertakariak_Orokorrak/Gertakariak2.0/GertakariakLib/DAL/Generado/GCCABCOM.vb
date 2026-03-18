



'===============================================================================
'BATZ, Koop. - 11/05/2009 10:14:28
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

    Public MustInherit Class _GCCABCOM
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "XBAT."
            Me.QuerySource = "GCCABCOM"
            Me.MappingName = "GCCABCOM"
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


            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_GCCABCOM", parameters)
        End Function

        '=================================================================
        ' Public Overridable Function LoadByPrimaryKey()  As Boolean
        '=================================================================
        '  Loads a single row of via the primary key
        '=================================================================
        Public Overridable Function LoadByPrimaryKey() As Boolean

            Dim parameters As ListDictionary = New ListDictionary()

            Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
            p.Direction = ParameterDirection.Output
            parameters.Add(p, Nothing)

            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_GCCABCOM", parameters)

        End Function

#Region "Parameters"
        Protected Class Parameters

            Public Shared ReadOnly Property NUMPED As OracleParameter
                Get
                    Return New OracleParameter("p_NUMPED", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property NUMCOM As OracleParameter
                Get
                    Return New OracleParameter("p_NUMCOM", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property COMENTA As OracleParameter
                Get
                    Return New OracleParameter("p_COMENTA", OracleDbType.Varchar2, 75)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const NUMPED As String = "NUMPED"
            Public Const NUMCOM As String = "NUMCOM"
            Public Const COMENTA As String = "COMENTA"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(NUMPED) = _GCCABCOM.PropertyNames.NUMPED
                    ht(NUMCOM) = _GCCABCOM.PropertyNames.NUMCOM
                    ht(COMENTA) = _GCCABCOM.PropertyNames.COMENTA

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const NUMPED As String = "NUMPED"
            Public Const NUMCOM As String = "NUMCOM"
            Public Const COMENTA As String = "COMENTA"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(NUMPED) = _GCCABCOM.ColumnNames.NUMPED
                    ht(NUMCOM) = _GCCABCOM.ColumnNames.NUMCOM
                    ht(COMENTA) = _GCCABCOM.ColumnNames.COMENTA

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const NUMPED As String = "s_NUMPED"
            Public Const NUMCOM As String = "s_NUMCOM"
            Public Const COMENTA As String = "s_COMENTA"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property NUMPED As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.NUMPED)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.NUMPED, Value)
            End Set
        End Property

        Public Overridable Property NUMCOM As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.NUMCOM)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.NUMCOM, Value)
            End Set
        End Property

        Public Overridable Property COMENTA As String
            Get
                Return MyBase.GetString(ColumnNames.COMENTA)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.COMENTA, Value)
            End Set
        End Property


#End Region

#Region "String Properties"

        Public Overridable Property s_NUMPED As String
            Get
                If Me.IsColumnNull(ColumnNames.NUMPED) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.NUMPED)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NUMPED)
                Else
                    Me.NUMPED = MyBase.SetDecimalAsString(ColumnNames.NUMPED, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_NUMCOM As String
            Get
                If Me.IsColumnNull(ColumnNames.NUMCOM) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.NUMCOM)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NUMCOM)
                Else
                    Me.NUMCOM = MyBase.SetDecimalAsString(ColumnNames.NUMCOM, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_COMENTA As String
            Get
                If Me.IsColumnNull(ColumnNames.COMENTA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.COMENTA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.COMENTA)
                Else
                    Me.COMENTA = MyBase.SetStringAsString(ColumnNames.COMENTA, Value)
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


                Public ReadOnly Property NUMPED() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMPED, Parameters.NUMPED)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMCOM() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMCOM, Parameters.NUMCOM)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property COMENTA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.COMENTA, Parameters.COMENTA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property


                Private _clause As WhereClause
            End Class
#End Region

            Public ReadOnly Property NUMPED() As WhereParameter
                Get
                    If _NUMPED_W Is Nothing Then
                        _NUMPED_W = TearOff.NUMPED
                    End If
                    Return _NUMPED_W
                End Get
            End Property

            Public ReadOnly Property NUMCOM() As WhereParameter
                Get
                    If _NUMCOM_W Is Nothing Then
                        _NUMCOM_W = TearOff.NUMCOM
                    End If
                    Return _NUMCOM_W
                End Get
            End Property

            Public ReadOnly Property COMENTA() As WhereParameter
                Get
                    If _COMENTA_W Is Nothing Then
                        _COMENTA_W = TearOff.COMENTA
                    End If
                    Return _COMENTA_W
                End Get
            End Property

            Private _NUMPED_W As WhereParameter = Nothing
            Private _NUMCOM_W As WhereParameter = Nothing
            Private _COMENTA_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _NUMPED_W = Nothing
                _NUMCOM_W = Nothing
                _COMENTA_W = Nothing
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


                Public ReadOnly Property NUMPED() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMPED, Parameters.NUMPED)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMCOM() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMCOM, Parameters.NUMCOM)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property COMENTA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.COMENTA, Parameters.COMENTA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property


                Private _clause As AggregateClause
            End Class
#End Region

            Public ReadOnly Property NUMPED() As AggregateParameter
                Get
                    If _NUMPED_W Is Nothing Then
                        _NUMPED_W = TearOff.NUMPED
                    End If
                    Return _NUMPED_W
                End Get
            End Property

            Public ReadOnly Property NUMCOM() As AggregateParameter
                Get
                    If _NUMCOM_W Is Nothing Then
                        _NUMCOM_W = TearOff.NUMCOM
                    End If
                    Return _NUMCOM_W
                End Get
            End Property

            Public ReadOnly Property COMENTA() As AggregateParameter
                Get
                    If _COMENTA_W Is Nothing Then
                        _COMENTA_W = TearOff.COMENTA
                    End If
                    Return _COMENTA_W
                End Get
            End Property

            Private _NUMPED_W As AggregateParameter = Nothing
            Private _NUMCOM_W As AggregateParameter = Nothing
            Private _COMENTA_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _NUMPED_W = Nothing
                _NUMCOM_W = Nothing
                _COMENTA_W = Nothing
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
            cmd.CommandText = Me.SchemaStoredProcedure + "PI_GCCABCOM"

            CreateParameters(cmd)


            Return cmd

        End Function

        Protected Overrides Function GetUpdateCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PU_GCCABCOM"

            CreateParameters(cmd)

            Return cmd

        End Function

        Protected Overrides Function GetDeleteCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PD_GCCABCOM"

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.NUMPED)
            p.SourceColumn = ColumnNames.NUMPED
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMCOM)
            p.SourceColumn = ColumnNames.NUMCOM
            p.SourceVersion = DataRowVersion.Current

            Return cmd

        End Function

        Private Sub CreateParameters(ByVal cmd As OracleCommand)

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.NUMPED)
            p.SourceColumn = ColumnNames.NUMPED
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMCOM)
            p.SourceColumn = ColumnNames.NUMCOM
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.COMENTA)
            p.SourceColumn = ColumnNames.COMENTA
            p.SourceVersion = DataRowVersion.Current


        End Sub

    End Class

End Namespace

