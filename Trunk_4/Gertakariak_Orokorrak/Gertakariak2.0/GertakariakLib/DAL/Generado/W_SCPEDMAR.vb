
'===============================================================================
'BATZ, Koop. - 06/07/2009 7:38:32
' Generado por MyGeneration Version # (1.3.0.9)
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

    Public MustInherit Class _W_SCPEDMAR
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "INCIDENCIAS."
            Me.QuerySource = "W_SCPEDMAR"
            Me.MappingName = "W_SCPEDMAR"
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

            Public Shared ReadOnly Property NUMPEDMAR As OracleParameter
                Get
                    Return New OracleParameter("NUMPEDMAR", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property NUMLINMAR As OracleParameter
                Get
                    Return New OracleParameter("NUMLINMAR", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property NUMMARMAR As OracleParameter
                Get
                    Return New OracleParameter("NUMMARMAR", OracleDbType.Char, 11)
                End Get
            End Property

            Public Shared ReadOnly Property CANTIDMAR As OracleParameter
                Get
                    Return New OracleParameter("CANTIDMAR", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property DESCRIMAR As OracleParameter
                Get
                    Return New OracleParameter("DESCRIMAR", OracleDbType.Char, 20)
                End Get
            End Property

            Public Shared ReadOnly Property FECHA_ENVIO As OracleParameter
                Get
                    Return New OracleParameter("FECHA_ENVIO", OracleDbType.Date, 0)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const NUMPEDMAR As String = "NUMPEDMAR"
            Public Const NUMLINMAR As String = "NUMLINMAR"
            Public Const NUMMARMAR As String = "NUMMARMAR"
            Public Const CANTIDMAR As String = "CANTIDMAR"
            Public Const DESCRIMAR As String = "DESCRIMAR"
            Public Const FECHA_ENVIO As String = "FECHA_ENVIO"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(NUMPEDMAR) = _W_SCPEDMAR.PropertyNames.NUMPEDMAR
                    ht(NUMLINMAR) = _W_SCPEDMAR.PropertyNames.NUMLINMAR
                    ht(NUMMARMAR) = _W_SCPEDMAR.PropertyNames.NUMMARMAR
                    ht(CANTIDMAR) = _W_SCPEDMAR.PropertyNames.CANTIDMAR
                    ht(DESCRIMAR) = _W_SCPEDMAR.PropertyNames.DESCRIMAR
                    ht(FECHA_ENVIO) = _W_SCPEDMAR.PropertyNames.FECHA_ENVIO

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const NUMPEDMAR As String = "NUMPEDMAR"
            Public Const NUMLINMAR As String = "NUMLINMAR"
            Public Const NUMMARMAR As String = "NUMMARMAR"
            Public Const CANTIDMAR As String = "CANTIDMAR"
            Public Const DESCRIMAR As String = "DESCRIMAR"
            Public Const FECHA_ENVIO As String = "FECHA_ENVIO"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(NUMPEDMAR) = _W_SCPEDMAR.ColumnNames.NUMPEDMAR
                    ht(NUMLINMAR) = _W_SCPEDMAR.ColumnNames.NUMLINMAR
                    ht(NUMMARMAR) = _W_SCPEDMAR.ColumnNames.NUMMARMAR
                    ht(CANTIDMAR) = _W_SCPEDMAR.ColumnNames.CANTIDMAR
                    ht(DESCRIMAR) = _W_SCPEDMAR.ColumnNames.DESCRIMAR
                    ht(FECHA_ENVIO) = _W_SCPEDMAR.ColumnNames.FECHA_ENVIO

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const NUMPEDMAR As String = "s_NUMPEDMAR"
            Public Const NUMLINMAR As String = "s_NUMLINMAR"
            Public Const NUMMARMAR As String = "s_NUMMARMAR"
            Public Const CANTIDMAR As String = "s_CANTIDMAR"
            Public Const DESCRIMAR As String = "s_DESCRIMAR"
            Public Const FECHA_ENVIO As String = "s_FECHA_ENVIO"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property NUMPEDMAR As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.NUMPEDMAR)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.NUMPEDMAR, Value)
            End Set
        End Property

        Public Overridable Property NUMLINMAR As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.NUMLINMAR)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.NUMLINMAR, Value)
            End Set
        End Property

        Public Overridable Property NUMMARMAR As String
            Get
                Return MyBase.GetString(ColumnNames.NUMMARMAR)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.NUMMARMAR, Value)
            End Set
        End Property

        Public Overridable Property CANTIDMAR As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.CANTIDMAR)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.CANTIDMAR, Value)
            End Set
        End Property

        Public Overridable Property DESCRIMAR As String
            Get
                Return MyBase.GetString(ColumnNames.DESCRIMAR)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.DESCRIMAR, Value)
            End Set
        End Property

        Public Overridable Property FECHA_ENVIO As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECHA_ENVIO)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECHA_ENVIO, Value)
            End Set
        End Property


#End Region

#Region "String Properties"

        Public Overridable Property s_NUMPEDMAR As String
            Get
                If Me.IsColumnNull(ColumnNames.NUMPEDMAR) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.NUMPEDMAR)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NUMPEDMAR)
                Else
                    Me.NUMPEDMAR = MyBase.SetDecimalAsString(ColumnNames.NUMPEDMAR, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_NUMLINMAR As String
            Get
                If Me.IsColumnNull(ColumnNames.NUMLINMAR) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.NUMLINMAR)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NUMLINMAR)
                Else
                    Me.NUMLINMAR = MyBase.SetDecimalAsString(ColumnNames.NUMLINMAR, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_NUMMARMAR As String
            Get
                If Me.IsColumnNull(ColumnNames.NUMMARMAR) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.NUMMARMAR)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NUMMARMAR)
                Else
                    Me.NUMMARMAR = MyBase.SetStringAsString(ColumnNames.NUMMARMAR, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CANTIDMAR As String
            Get
                If Me.IsColumnNull(ColumnNames.CANTIDMAR) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.CANTIDMAR)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CANTIDMAR)
                Else
                    Me.CANTIDMAR = MyBase.SetDecimalAsString(ColumnNames.CANTIDMAR, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_DESCRIMAR As String
            Get
                If Me.IsColumnNull(ColumnNames.DESCRIMAR) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.DESCRIMAR)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.DESCRIMAR)
                Else
                    Me.DESCRIMAR = MyBase.SetStringAsString(ColumnNames.DESCRIMAR, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FECHA_ENVIO As String
            Get
                If Me.IsColumnNull(ColumnNames.FECHA_ENVIO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FECHA_ENVIO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FECHA_ENVIO)
                Else
                    Me.FECHA_ENVIO = MyBase.SetDateTimeAsString(ColumnNames.FECHA_ENVIO, Value)
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


                Public ReadOnly Property NUMPEDMAR() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMPEDMAR, Parameters.NUMPEDMAR)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMLINMAR() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMLINMAR, Parameters.NUMLINMAR)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMMARMAR() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMMARMAR, Parameters.NUMMARMAR)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CANTIDMAR() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CANTIDMAR, Parameters.CANTIDMAR)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DESCRIMAR() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.DESCRIMAR, Parameters.DESCRIMAR)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECHA_ENVIO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FECHA_ENVIO, Parameters.FECHA_ENVIO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property


                Private _clause As WhereClause
            End Class
#End Region

            Public ReadOnly Property NUMPEDMAR() As WhereParameter
                Get
                    If _NUMPEDMAR_W Is Nothing Then
                        _NUMPEDMAR_W = TearOff.NUMPEDMAR
                    End If
                    Return _NUMPEDMAR_W
                End Get
            End Property

            Public ReadOnly Property NUMLINMAR() As WhereParameter
                Get
                    If _NUMLINMAR_W Is Nothing Then
                        _NUMLINMAR_W = TearOff.NUMLINMAR
                    End If
                    Return _NUMLINMAR_W
                End Get
            End Property

            Public ReadOnly Property NUMMARMAR() As WhereParameter
                Get
                    If _NUMMARMAR_W Is Nothing Then
                        _NUMMARMAR_W = TearOff.NUMMARMAR
                    End If
                    Return _NUMMARMAR_W
                End Get
            End Property

            Public ReadOnly Property CANTIDMAR() As WhereParameter
                Get
                    If _CANTIDMAR_W Is Nothing Then
                        _CANTIDMAR_W = TearOff.CANTIDMAR
                    End If
                    Return _CANTIDMAR_W
                End Get
            End Property

            Public ReadOnly Property DESCRIMAR() As WhereParameter
                Get
                    If _DESCRIMAR_W Is Nothing Then
                        _DESCRIMAR_W = TearOff.DESCRIMAR
                    End If
                    Return _DESCRIMAR_W
                End Get
            End Property

            Public ReadOnly Property FECHA_ENVIO() As WhereParameter
                Get
                    If _FECHA_ENVIO_W Is Nothing Then
                        _FECHA_ENVIO_W = TearOff.FECHA_ENVIO
                    End If
                    Return _FECHA_ENVIO_W
                End Get
            End Property

            Private _NUMPEDMAR_W As WhereParameter = Nothing
            Private _NUMLINMAR_W As WhereParameter = Nothing
            Private _NUMMARMAR_W As WhereParameter = Nothing
            Private _CANTIDMAR_W As WhereParameter = Nothing
            Private _DESCRIMAR_W As WhereParameter = Nothing
            Private _FECHA_ENVIO_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _NUMPEDMAR_W = Nothing
                _NUMLINMAR_W = Nothing
                _NUMMARMAR_W = Nothing
                _CANTIDMAR_W = Nothing
                _DESCRIMAR_W = Nothing
                _FECHA_ENVIO_W = Nothing
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


                Public ReadOnly Property NUMPEDMAR() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMPEDMAR, Parameters.NUMPEDMAR)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMLINMAR() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMLINMAR, Parameters.NUMLINMAR)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMMARMAR() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMMARMAR, Parameters.NUMMARMAR)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CANTIDMAR() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CANTIDMAR, Parameters.CANTIDMAR)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DESCRIMAR() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DESCRIMAR, Parameters.DESCRIMAR)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECHA_ENVIO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECHA_ENVIO, Parameters.FECHA_ENVIO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property


                Private _clause As AggregateClause
            End Class
#End Region

            Public ReadOnly Property NUMPEDMAR() As AggregateParameter
                Get
                    If _NUMPEDMAR_W Is Nothing Then
                        _NUMPEDMAR_W = TearOff.NUMPEDMAR
                    End If
                    Return _NUMPEDMAR_W
                End Get
            End Property

            Public ReadOnly Property NUMLINMAR() As AggregateParameter
                Get
                    If _NUMLINMAR_W Is Nothing Then
                        _NUMLINMAR_W = TearOff.NUMLINMAR
                    End If
                    Return _NUMLINMAR_W
                End Get
            End Property

            Public ReadOnly Property NUMMARMAR() As AggregateParameter
                Get
                    If _NUMMARMAR_W Is Nothing Then
                        _NUMMARMAR_W = TearOff.NUMMARMAR
                    End If
                    Return _NUMMARMAR_W
                End Get
            End Property

            Public ReadOnly Property CANTIDMAR() As AggregateParameter
                Get
                    If _CANTIDMAR_W Is Nothing Then
                        _CANTIDMAR_W = TearOff.CANTIDMAR
                    End If
                    Return _CANTIDMAR_W
                End Get
            End Property

            Public ReadOnly Property DESCRIMAR() As AggregateParameter
                Get
                    If _DESCRIMAR_W Is Nothing Then
                        _DESCRIMAR_W = TearOff.DESCRIMAR
                    End If
                    Return _DESCRIMAR_W
                End Get
            End Property

            Public ReadOnly Property FECHA_ENVIO() As AggregateParameter
                Get
                    If _FECHA_ENVIO_W Is Nothing Then
                        _FECHA_ENVIO_W = TearOff.FECHA_ENVIO
                    End If
                    Return _FECHA_ENVIO_W
                End Get
            End Property

            Private _NUMPEDMAR_W As AggregateParameter = Nothing
            Private _NUMLINMAR_W As AggregateParameter = Nothing
            Private _NUMMARMAR_W As AggregateParameter = Nothing
            Private _CANTIDMAR_W As AggregateParameter = Nothing
            Private _DESCRIMAR_W As AggregateParameter = Nothing
            Private _FECHA_ENVIO_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _NUMPEDMAR_W = Nothing
                _NUMLINMAR_W = Nothing
                _NUMMARMAR_W = Nothing
                _CANTIDMAR_W = Nothing
                _DESCRIMAR_W = Nothing
                _FECHA_ENVIO_W = Nothing
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

