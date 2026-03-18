
'===============================================================================
'BATZ, Koop. - 08/05/2008 15:33:37
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

    Public MustInherit Class _W_CPSECCIO
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "INCIDENCIAS."
            Me.QuerySource = "W_CPSECCIO"
            Me.MappingName = "W_CPSECCIO"
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

            Public Shared ReadOnly Property CODSEC As OracleParameter
                Get
                    Return New OracleParameter("CODSEC", OracleDbType.Char, 4)
                End Get
            End Property

            Public Shared ReadOnly Property DESCSECCIO As OracleParameter
                Get
                    Return New OracleParameter("DESCSECCIO", OracleDbType.Varchar2, 30)
                End Get
            End Property

            Public Shared ReadOnly Property ETASAHORA As OracleParameter
                Get
                    Return New OracleParameter("ETASAHORA", OracleDbType.Int32, 0)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const CODSEC As String = "CODSEC"
            Public Const DESCSECCIO As String = "DESCSECCIO"
            Public Const ETASAHORA As String = "ETASAHORA"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(CODSEC) = _W_CPSECCIO.PropertyNames.CODSEC
                    ht(DESCSECCIO) = _W_CPSECCIO.PropertyNames.DESCSECCIO
                    ht(ETASAHORA) = _W_CPSECCIO.PropertyNames.ETASAHORA

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const CODSEC As String = "CODSEC"
            Public Const DESCSECCIO As String = "DESCSECCIO"
            Public Const ETASAHORA As String = "ETASAHORA"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(CODSEC) = _W_CPSECCIO.ColumnNames.CODSEC
                    ht(DESCSECCIO) = _W_CPSECCIO.ColumnNames.DESCSECCIO
                    ht(ETASAHORA) = _W_CPSECCIO.ColumnNames.ETASAHORA

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const CODSEC As String = "s_CODSEC"
            Public Const DESCSECCIO As String = "s_DESCSECCIO"
            Public Const ETASAHORA As String = "s_ETASAHORA"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property CODSEC As String
            Get
                Return MyBase.GetString(ColumnNames.CODSEC)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CODSEC, Value)
            End Set
        End Property

        Public Overridable Property DESCSECCIO As String
            Get
                Return MyBase.GetString(ColumnNames.DESCSECCIO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.DESCSECCIO, Value)
            End Set
        End Property

        Public Overridable Property ETASAHORA As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.ETASAHORA)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.ETASAHORA, Value)
            End Set
        End Property


#End Region

#Region "String Properties"

        Public Overridable Property s_CODSEC As String
            Get
                If Me.IsColumnNull(ColumnNames.CODSEC) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CODSEC)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CODSEC)
                Else
                    Me.CODSEC = MyBase.SetStringAsString(ColumnNames.CODSEC, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_DESCSECCIO As String
            Get
                If Me.IsColumnNull(ColumnNames.DESCSECCIO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.DESCSECCIO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.DESCSECCIO)
                Else
                    Me.DESCSECCIO = MyBase.SetStringAsString(ColumnNames.DESCSECCIO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_ETASAHORA As String
            Get
                If Me.IsColumnNull(ColumnNames.ETASAHORA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.ETASAHORA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.ETASAHORA)
                Else
                    Me.ETASAHORA = MyBase.SetDecimalAsString(ColumnNames.ETASAHORA, Value)
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


                Public ReadOnly Property CODSEC() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CODSEC, Parameters.CODSEC)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DESCSECCIO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.DESCSECCIO, Parameters.DESCSECCIO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ETASAHORA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.ETASAHORA, Parameters.ETASAHORA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property


                Private _clause As WhereClause
            End Class
#End Region

            Public ReadOnly Property CODSEC() As WhereParameter
                Get
                    If _CODSEC_W Is Nothing Then
                        _CODSEC_W = TearOff.CODSEC
                    End If
                    Return _CODSEC_W
                End Get
            End Property

            Public ReadOnly Property DESCSECCIO() As WhereParameter
                Get
                    If _DESCSECCIO_W Is Nothing Then
                        _DESCSECCIO_W = TearOff.DESCSECCIO
                    End If
                    Return _DESCSECCIO_W
                End Get
            End Property

            Public ReadOnly Property ETASAHORA() As WhereParameter
                Get
                    If _ETASAHORA_W Is Nothing Then
                        _ETASAHORA_W = TearOff.ETASAHORA
                    End If
                    Return _ETASAHORA_W
                End Get
            End Property

            Private _CODSEC_W As WhereParameter = Nothing
            Private _DESCSECCIO_W As WhereParameter = Nothing
            Private _ETASAHORA_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _CODSEC_W = Nothing
                _DESCSECCIO_W = Nothing
                _ETASAHORA_W = Nothing
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


                Public ReadOnly Property CODSEC() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CODSEC, Parameters.CODSEC)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DESCSECCIO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DESCSECCIO, Parameters.DESCSECCIO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ETASAHORA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ETASAHORA, Parameters.ETASAHORA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property


                Private _clause As AggregateClause
            End Class
#End Region

            Public ReadOnly Property CODSEC() As AggregateParameter
                Get
                    If _CODSEC_W Is Nothing Then
                        _CODSEC_W = TearOff.CODSEC
                    End If
                    Return _CODSEC_W
                End Get
            End Property

            Public ReadOnly Property DESCSECCIO() As AggregateParameter
                Get
                    If _DESCSECCIO_W Is Nothing Then
                        _DESCSECCIO_W = TearOff.DESCSECCIO
                    End If
                    Return _DESCSECCIO_W
                End Get
            End Property

            Public ReadOnly Property ETASAHORA() As AggregateParameter
                Get
                    If _ETASAHORA_W Is Nothing Then
                        _ETASAHORA_W = TearOff.ETASAHORA
                    End If
                    Return _ETASAHORA_W
                End Get
            End Property

            Private _CODSEC_W As AggregateParameter = Nothing
            Private _DESCSECCIO_W As AggregateParameter = Nothing
            Private _ETASAHORA_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _CODSEC_W = Nothing
                _DESCSECCIO_W = Nothing
                _ETASAHORA_W = Nothing
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

