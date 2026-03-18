
'===============================================================================
'BATZ, Koop. - 15/12/2008 7:42:32
' Generado por MyGeneration Version # (1.3.0.3)
' Generado desde Batz_VbNet_SQL_dOOdads_View.vbgen
' El soporte de la clase OracleClientEntity esta en el directorio Architecture  en "dOOdads".
'===============================================================================

Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Specialized


Imports AccesoAutomaticoBD
Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public MustInherit Class _W_CPCABEC
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "INCIDENCIAS."
            Me.QuerySource = "W_CPCABEC"
            Me.MappingName = "W_CPCABEC"
        End Sub

        '=================================================================
        '  	Public Function LoadAll() As Boolean
        '=================================================================
        '  Loads all of the records in the database, and sets the currentRow to the first row
        '=================================================================
        Public Function LoadAll() As Boolean
            Return MyBase.Query.Load()
        End Function

        Public Overrides Sub FlushData()
            Me._whereClause = Nothing
            Me._aggregateClause = Nothing
            MyBase.FlushData()
        End Sub

#Region "Parameters"
        Protected Class Parameters

            Public Shared ReadOnly Property NUMORD As OracleParameter
                Get
                    Return New OracleParameter("NUMORD", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property NUMOPE As OracleParameter
                Get
                    Return New OracleParameter("NUMOPE", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property TIPOPE As OracleParameter
                Get
                    Return New OracleParameter("TIPOPE", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property DESCR As OracleParameter
                Get
                    Return New OracleParameter("DESCR", OracleDbType.Char, 30)
                End Get
            End Property

            Public Shared ReadOnly Property LANTEGI_AC As OracleParameter
                Get
                    Return New OracleParameter("LANTEGI_AC", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property TIPORD As OracleParameter
                Get
                    Return New OracleParameter("TIPORD", OracleDbType.Int32, 0)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const NUMORD As String = "NUMORD"
            Public Const NUMOPE As String = "NUMOPE"
            Public Const TIPOPE As String = "TIPOPE"
            Public Const DESCR As String = "DESCR"
            Public Const LANTEGI_AC As String = "LANTEGI_AC"
            Public Const TIPORD As String = "TIPORD"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(NUMORD) = _W_CPCABEC.PropertyNames.NUMORD
                    ht(NUMOPE) = _W_CPCABEC.PropertyNames.NUMOPE
                    ht(TIPOPE) = _W_CPCABEC.PropertyNames.TIPOPE
                    ht(DESCR) = _W_CPCABEC.PropertyNames.DESCR
                    ht(LANTEGI_AC) = _W_CPCABEC.PropertyNames.LANTEGI_AC
                    ht(TIPORD) = _W_CPCABEC.PropertyNames.TIPORD

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const NUMORD As String = "NUMORD"
            Public Const NUMOPE As String = "NUMOPE"
            Public Const TIPOPE As String = "TIPOPE"
            Public Const DESCR As String = "DESCR"
            Public Const LANTEGI_AC As String = "LANTEGI_AC"
            Public Const TIPORD As String = "TIPORD"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(NUMORD) = _W_CPCABEC.ColumnNames.NUMORD
                    ht(NUMOPE) = _W_CPCABEC.ColumnNames.NUMOPE
                    ht(TIPOPE) = _W_CPCABEC.ColumnNames.TIPOPE
                    ht(DESCR) = _W_CPCABEC.ColumnNames.DESCR
                    ht(LANTEGI_AC) = _W_CPCABEC.ColumnNames.LANTEGI_AC
                    ht(TIPORD) = _W_CPCABEC.ColumnNames.TIPORD

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const NUMORD As String = "s_NUMORD"
            Public Const NUMOPE As String = "s_NUMOPE"
            Public Const TIPOPE As String = "s_TIPOPE"
            Public Const DESCR As String = "s_DESCR"
            Public Const LANTEGI_AC As String = "s_LANTEGI_AC"
            Public Const TIPORD As String = "s_TIPORD"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property NUMORD As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.NUMORD)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.NUMORD, Value)
            End Set
        End Property

        Public Overridable Property NUMOPE As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.NUMOPE)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.NUMOPE, Value)
            End Set
        End Property

        Public Overridable Property TIPOPE As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.TIPOPE)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.TIPOPE, Value)
            End Set
        End Property

        Public Overridable Property DESCR As String
            Get
                Return MyBase.GetString(ColumnNames.DESCR)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.DESCR, Value)
            End Set
        End Property

        Public Overridable Property LANTEGI_AC As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.LANTEGI_AC)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.LANTEGI_AC, Value)
            End Set
        End Property

        Public Overridable Property TIPORD As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.TIPORD)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.TIPORD, Value)
            End Set
        End Property


#End Region

#Region "String Properties"

        Public Overridable Property s_NUMORD As String
            Get
                If Me.IsColumnNull(ColumnNames.NUMORD) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.NUMORD)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NUMORD)
                Else
                    Me.NUMORD = MyBase.SetDecimalAsString(ColumnNames.NUMORD, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_NUMOPE As String
            Get
                If Me.IsColumnNull(ColumnNames.NUMOPE) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.NUMOPE)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NUMOPE)
                Else
                    Me.NUMOPE = MyBase.SetDecimalAsString(ColumnNames.NUMOPE, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_TIPOPE As String
            Get
                If Me.IsColumnNull(ColumnNames.TIPOPE) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.TIPOPE)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.TIPOPE)
                Else
                    Me.TIPOPE = MyBase.SetDecimalAsString(ColumnNames.TIPOPE, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_DESCR As String
            Get
                If Me.IsColumnNull(ColumnNames.DESCR) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.DESCR)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.DESCR)
                Else
                    Me.DESCR = MyBase.SetStringAsString(ColumnNames.DESCR, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_LANTEGI_AC As String
            Get
                If Me.IsColumnNull(ColumnNames.LANTEGI_AC) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.LANTEGI_AC)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.LANTEGI_AC)
                Else
                    Me.LANTEGI_AC = MyBase.SetDecimalAsString(ColumnNames.LANTEGI_AC, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_TIPORD As String
            Get
                If Me.IsColumnNull(ColumnNames.TIPORD) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.TIPORD)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.TIPORD)
                Else
                    Me.TIPORD = MyBase.SetDecimalAsString(ColumnNames.TIPORD, Value)
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


                Public ReadOnly Property NUMORD() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMORD, Parameters.NUMORD)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMOPE() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMOPE, Parameters.NUMOPE)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TIPOPE() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.TIPOPE, Parameters.TIPOPE)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DESCR() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.DESCR, Parameters.DESCR)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property LANTEGI_AC() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.LANTEGI_AC, Parameters.LANTEGI_AC)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TIPORD() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.TIPORD, Parameters.TIPORD)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property


                Private _clause As WhereClause
            End Class
#End Region

            Public ReadOnly Property NUMORD() As WhereParameter
                Get
                    If _NUMORD_W Is Nothing Then
                        _NUMORD_W = TearOff.NUMORD
                    End If
                    Return _NUMORD_W
                End Get
            End Property

            Public ReadOnly Property NUMOPE() As WhereParameter
                Get
                    If _NUMOPE_W Is Nothing Then
                        _NUMOPE_W = TearOff.NUMOPE
                    End If
                    Return _NUMOPE_W
                End Get
            End Property

            Public ReadOnly Property TIPOPE() As WhereParameter
                Get
                    If _TIPOPE_W Is Nothing Then
                        _TIPOPE_W = TearOff.TIPOPE
                    End If
                    Return _TIPOPE_W
                End Get
            End Property

            Public ReadOnly Property DESCR() As WhereParameter
                Get
                    If _DESCR_W Is Nothing Then
                        _DESCR_W = TearOff.DESCR
                    End If
                    Return _DESCR_W
                End Get
            End Property

            Public ReadOnly Property LANTEGI_AC() As WhereParameter
                Get
                    If _LANTEGI_AC_W Is Nothing Then
                        _LANTEGI_AC_W = TearOff.LANTEGI_AC
                    End If
                    Return _LANTEGI_AC_W
                End Get
            End Property

            Public ReadOnly Property TIPORD() As WhereParameter
                Get
                    If _TIPORD_W Is Nothing Then
                        _TIPORD_W = TearOff.TIPORD
                    End If
                    Return _TIPORD_W
                End Get
            End Property

            Private _NUMORD_W As WhereParameter = Nothing
            Private _NUMOPE_W As WhereParameter = Nothing
            Private _TIPOPE_W As WhereParameter = Nothing
            Private _DESCR_W As WhereParameter = Nothing
            Private _LANTEGI_AC_W As WhereParameter = Nothing
            Private _TIPORD_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _NUMORD_W = Nothing
                _NUMOPE_W = Nothing
                _TIPOPE_W = Nothing
                _DESCR_W = Nothing
                _LANTEGI_AC_W = Nothing
                _TIPORD_W = Nothing
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


                Public ReadOnly Property NUMORD() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMORD, Parameters.NUMORD)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMOPE() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMOPE, Parameters.NUMOPE)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TIPOPE() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TIPOPE, Parameters.TIPOPE)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DESCR() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DESCR, Parameters.DESCR)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property LANTEGI_AC() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.LANTEGI_AC, Parameters.LANTEGI_AC)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TIPORD() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TIPORD, Parameters.TIPORD)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property


                Private _clause As AggregateClause
            End Class
#End Region

            Public ReadOnly Property NUMORD() As AggregateParameter
                Get
                    If _NUMORD_W Is Nothing Then
                        _NUMORD_W = TearOff.NUMORD
                    End If
                    Return _NUMORD_W
                End Get
            End Property

            Public ReadOnly Property NUMOPE() As AggregateParameter
                Get
                    If _NUMOPE_W Is Nothing Then
                        _NUMOPE_W = TearOff.NUMOPE
                    End If
                    Return _NUMOPE_W
                End Get
            End Property

            Public ReadOnly Property TIPOPE() As AggregateParameter
                Get
                    If _TIPOPE_W Is Nothing Then
                        _TIPOPE_W = TearOff.TIPOPE
                    End If
                    Return _TIPOPE_W
                End Get
            End Property

            Public ReadOnly Property DESCR() As AggregateParameter
                Get
                    If _DESCR_W Is Nothing Then
                        _DESCR_W = TearOff.DESCR
                    End If
                    Return _DESCR_W
                End Get
            End Property

            Public ReadOnly Property LANTEGI_AC() As AggregateParameter
                Get
                    If _LANTEGI_AC_W Is Nothing Then
                        _LANTEGI_AC_W = TearOff.LANTEGI_AC
                    End If
                    Return _LANTEGI_AC_W
                End Get
            End Property

            Public ReadOnly Property TIPORD() As AggregateParameter
                Get
                    If _TIPORD_W Is Nothing Then
                        _TIPORD_W = TearOff.TIPORD
                    End If
                    Return _TIPORD_W
                End Get
            End Property

            Private _NUMORD_W As AggregateParameter = Nothing
            Private _NUMOPE_W As AggregateParameter = Nothing
            Private _TIPOPE_W As AggregateParameter = Nothing
            Private _DESCR_W As AggregateParameter = Nothing
            Private _LANTEGI_AC_W As AggregateParameter = Nothing
            Private _TIPORD_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _NUMORD_W = Nothing
                _NUMOPE_W = Nothing
                _TIPOPE_W = Nothing
                _DESCR_W = Nothing
                _LANTEGI_AC_W = Nothing
                _TIPORD_W = Nothing
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
            Return Nothing
        End Function

        Protected Overrides Function GetUpdateCommand() As IDbCommand
            Return Nothing
        End Function

        Protected Overrides Function GetDeleteCommand() As IDbCommand
            Return Nothing
        End Function

    End Class

End Namespace

