
'===============================================================================
'BATZ, Koop. - 19/12/2008 8:23:24
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

    Public MustInherit Class _W_PROCESOS
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "INCIDENCIAS."
            Me.QuerySource = "W_PROCESOS"
            Me.MappingName = "W_PROCESOS"
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

            Public Shared ReadOnly Property CODARE As OracleParameter
                Get
                    Return New OracleParameter("CODARE", OracleDbType.Varchar2, 1)
                End Get
            End Property

            Public Shared ReadOnly Property DESCRI As OracleParameter
                Get
                    Return New OracleParameter("DESCRI", OracleDbType.Varchar2, 30)
                End Get
            End Property

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

            Public Shared ReadOnly Property CODPROC As OracleParameter
                Get
                    Return New OracleParameter("CODPROC", OracleDbType.Char, 4)
                End Get
            End Property

            Public Shared ReadOnly Property DESCPROC As OracleParameter
                Get
                    Return New OracleParameter("DESCPROC", OracleDbType.Varchar2, 30)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const CODARE As String = "CODARE"
            Public Const DESCRI As String = "DESCRI"
            Public Const CODSEC As String = "CODSEC"
            Public Const DESCSECCIO As String = "DESCSECCIO"
            Public Const CODPROC As String = "CODPROC"
            Public Const DESCPROC As String = "DESCPROC"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(CODARE) = _W_PROCESOS.PropertyNames.CODARE
                    ht(DESCRI) = _W_PROCESOS.PropertyNames.DESCRI
                    ht(CODSEC) = _W_PROCESOS.PropertyNames.CODSEC
                    ht(DESCSECCIO) = _W_PROCESOS.PropertyNames.DESCSECCIO
                    ht(CODPROC) = _W_PROCESOS.PropertyNames.CODPROC
                    ht(DESCPROC) = _W_PROCESOS.PropertyNames.DESCPROC

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const CODARE As String = "CODARE"
            Public Const DESCRI As String = "DESCRI"
            Public Const CODSEC As String = "CODSEC"
            Public Const DESCSECCIO As String = "DESCSECCIO"
            Public Const CODPROC As String = "CODPROC"
            Public Const DESCPROC As String = "DESCPROC"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(CODARE) = _W_PROCESOS.ColumnNames.CODARE
                    ht(DESCRI) = _W_PROCESOS.ColumnNames.DESCRI
                    ht(CODSEC) = _W_PROCESOS.ColumnNames.CODSEC
                    ht(DESCSECCIO) = _W_PROCESOS.ColumnNames.DESCSECCIO
                    ht(CODPROC) = _W_PROCESOS.ColumnNames.CODPROC
                    ht(DESCPROC) = _W_PROCESOS.ColumnNames.DESCPROC

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const CODARE As String = "s_CODARE"
            Public Const DESCRI As String = "s_DESCRI"
            Public Const CODSEC As String = "s_CODSEC"
            Public Const DESCSECCIO As String = "s_DESCSECCIO"
            Public Const CODPROC As String = "s_CODPROC"
            Public Const DESCPROC As String = "s_DESCPROC"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property CODARE As String
            Get
                Return MyBase.GetString(ColumnNames.CODARE)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CODARE, Value)
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

        Public Overridable Property CODPROC As String
            Get
                Return MyBase.GetString(ColumnNames.CODPROC)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CODPROC, Value)
            End Set
        End Property

        Public Overridable Property DESCPROC As String
            Get
                Return MyBase.GetString(ColumnNames.DESCPROC)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.DESCPROC, Value)
            End Set
        End Property


#End Region

#Region "String Properties"

        Public Overridable Property s_CODARE As String
            Get
                If Me.IsColumnNull(ColumnNames.CODARE) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CODARE)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CODARE)
                Else
                    Me.CODARE = MyBase.SetStringAsString(ColumnNames.CODARE, Value)
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

        Public Overridable Property s_CODPROC As String
            Get
                If Me.IsColumnNull(ColumnNames.CODPROC) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CODPROC)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CODPROC)
                Else
                    Me.CODPROC = MyBase.SetStringAsString(ColumnNames.CODPROC, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_DESCPROC As String
            Get
                If Me.IsColumnNull(ColumnNames.DESCPROC) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.DESCPROC)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.DESCPROC)
                Else
                    Me.DESCPROC = MyBase.SetStringAsString(ColumnNames.DESCPROC, Value)
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


                Public ReadOnly Property CODARE() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CODARE, Parameters.CODARE)
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

                Public ReadOnly Property CODPROC() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CODPROC, Parameters.CODPROC)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DESCPROC() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.DESCPROC, Parameters.DESCPROC)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property


                Private _clause As WhereClause
            End Class
#End Region

            Public ReadOnly Property CODARE() As WhereParameter
                Get
                    If _CODARE_W Is Nothing Then
                        _CODARE_W = TearOff.CODARE
                    End If
                    Return _CODARE_W
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

            Public ReadOnly Property CODPROC() As WhereParameter
                Get
                    If _CODPROC_W Is Nothing Then
                        _CODPROC_W = TearOff.CODPROC
                    End If
                    Return _CODPROC_W
                End Get
            End Property

            Public ReadOnly Property DESCPROC() As WhereParameter
                Get
                    If _DESCPROC_W Is Nothing Then
                        _DESCPROC_W = TearOff.DESCPROC
                    End If
                    Return _DESCPROC_W
                End Get
            End Property

            Private _CODARE_W As WhereParameter = Nothing
            Private _DESCRI_W As WhereParameter = Nothing
            Private _CODSEC_W As WhereParameter = Nothing
            Private _DESCSECCIO_W As WhereParameter = Nothing
            Private _CODPROC_W As WhereParameter = Nothing
            Private _DESCPROC_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _CODARE_W = Nothing
                _DESCRI_W = Nothing
                _CODSEC_W = Nothing
                _DESCSECCIO_W = Nothing
                _CODPROC_W = Nothing
                _DESCPROC_W = Nothing
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


                Public ReadOnly Property CODARE() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CODARE, Parameters.CODARE)
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

                Public ReadOnly Property CODPROC() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CODPROC, Parameters.CODPROC)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DESCPROC() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DESCPROC, Parameters.DESCPROC)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property


                Private _clause As AggregateClause
            End Class
#End Region

            Public ReadOnly Property CODARE() As AggregateParameter
                Get
                    If _CODARE_W Is Nothing Then
                        _CODARE_W = TearOff.CODARE
                    End If
                    Return _CODARE_W
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

            Public ReadOnly Property CODPROC() As AggregateParameter
                Get
                    If _CODPROC_W Is Nothing Then
                        _CODPROC_W = TearOff.CODPROC
                    End If
                    Return _CODPROC_W
                End Get
            End Property

            Public ReadOnly Property DESCPROC() As AggregateParameter
                Get
                    If _DESCPROC_W Is Nothing Then
                        _DESCPROC_W = TearOff.DESCPROC
                    End If
                    Return _DESCPROC_W
                End Get
            End Property

            Private _CODARE_W As AggregateParameter = Nothing
            Private _DESCRI_W As AggregateParameter = Nothing
            Private _CODSEC_W As AggregateParameter = Nothing
            Private _DESCSECCIO_W As AggregateParameter = Nothing
            Private _CODPROC_W As AggregateParameter = Nothing
            Private _DESCPROC_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _CODARE_W = Nothing
                _DESCRI_W = Nothing
                _CODSEC_W = Nothing
                _DESCSECCIO_W = Nothing
                _CODPROC_W = Nothing
                _DESCPROC_W = Nothing
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

