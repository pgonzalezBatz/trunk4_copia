
'===============================================================================
'BATZ, Koop. - 29/01/2009 10:17:03
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

    Public MustInherit Class _W_CPLANTEGI
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "INCIDENCIAS."
            Me.QuerySource = "W_CPLANTEGI"
            Me.MappingName = "W_CPLANTEGI"
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

            Public Shared ReadOnly Property LANTEGI As OracleParameter
                Get
                    Return New OracleParameter("LANTEGI", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property DESCRI As OracleParameter
                Get
                    Return New OracleParameter("DESCRI", OracleDbType.Varchar2, 20)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const LANTEGI As String = "LANTEGI"
            Public Const DESCRI As String = "DESCRI"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(LANTEGI) = _W_CPLANTEGI.PropertyNames.LANTEGI
                    ht(DESCRI) = _W_CPLANTEGI.PropertyNames.DESCRI

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const LANTEGI As String = "LANTEGI"
            Public Const DESCRI As String = "DESCRI"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(LANTEGI) = _W_CPLANTEGI.ColumnNames.LANTEGI
                    ht(DESCRI) = _W_CPLANTEGI.ColumnNames.DESCRI

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const LANTEGI As String = "s_LANTEGI"
            Public Const DESCRI As String = "s_DESCRI"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property LANTEGI As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.LANTEGI)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.LANTEGI, Value)
            End Set
        End Property

        Public Overridable Property DESCRI As String
            Get
                Return MyBase.GetString(ColumnNames.DESCRI)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.DESCRI, Value)
            End Set
        End Property


#End Region

#Region "String Properties"

        Public Overridable Property s_LANTEGI As String
            Get
                If Me.IsColumnNull(ColumnNames.LANTEGI) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.LANTEGI)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.LANTEGI)
                Else
                    Me.LANTEGI = MyBase.SetDecimalAsString(ColumnNames.LANTEGI, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_DESCRI As String
            Get
                If Me.IsColumnNull(ColumnNames.DESCRI) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.DESCRI)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.DESCRI)
                Else
                    Me.DESCRI = MyBase.SetStringAsString(ColumnNames.DESCRI, Value)
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


                Public ReadOnly Property LANTEGI() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.LANTEGI, Parameters.LANTEGI)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DESCRI() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.DESCRI, Parameters.DESCRI)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property


                Private _clause As WhereClause
            End Class
#End Region

            Public ReadOnly Property LANTEGI() As WhereParameter
                Get
                    If _LANTEGI_W Is Nothing Then
                        _LANTEGI_W = TearOff.LANTEGI
                    End If
                    Return _LANTEGI_W
                End Get
            End Property

            Public ReadOnly Property DESCRI() As WhereParameter
                Get
                    If _DESCRI_W Is Nothing Then
                        _DESCRI_W = TearOff.DESCRI
                    End If
                    Return _DESCRI_W
                End Get
            End Property

            Private _LANTEGI_W As WhereParameter = Nothing
            Private _DESCRI_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _LANTEGI_W = Nothing
                _DESCRI_W = Nothing
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


                Public ReadOnly Property LANTEGI() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.LANTEGI, Parameters.LANTEGI)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DESCRI() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DESCRI, Parameters.DESCRI)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property


                Private _clause As AggregateClause
            End Class
#End Region

            Public ReadOnly Property LANTEGI() As AggregateParameter
                Get
                    If _LANTEGI_W Is Nothing Then
                        _LANTEGI_W = TearOff.LANTEGI
                    End If
                    Return _LANTEGI_W
                End Get
            End Property

            Public ReadOnly Property DESCRI() As AggregateParameter
                Get
                    If _DESCRI_W Is Nothing Then
                        _DESCRI_W = TearOff.DESCRI
                    End If
                    Return _DESCRI_W
                End Get
            End Property

            Private _LANTEGI_W As AggregateParameter = Nothing
            Private _DESCRI_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _LANTEGI_W = Nothing
                _DESCRI_W = Nothing
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

