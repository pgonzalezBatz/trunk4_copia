
'===============================================================================
'BATZ, Koop. - 27/11/2008 13:05:35
' Generado por MyGeneration Version # (1.3.0.3)
' Generado desde Batz_VbNet_SQL_dOOdads_View.vbgen
' El soporte de la clase OracleClientEntity esta en el directorio Architecture  en "dOOdads".
'===============================================================================

Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Specialized


Imports AccesoAutomaticoBD


Namespace DAL.Views

    Public Class W_RECURSOS_GRUPO
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "SAB."
            Me.QuerySource = "W_RECURSOS_GRUPO"
            Me.MappingName = "W_RECURSOS_GRUPO"

            'Decide connection string depending on state
            If Configuration.ConfigurationManager.AppSettings.Get("CurrentStatus") = "Live" Then
                Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("SABLIVE").ConnectionString
            Else
                Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("SABTEST").ConnectionString
            End If
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

            Public Shared ReadOnly Property IDGRUPOS() As OracleParameter
                Get
                    Return New OracleParameter("IDGRUPOS", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property IDRECURSOS() As OracleParameter
                Get
                    Return New OracleParameter("IDRECURSOS", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property IDCULTURAS() As OracleParameter
                Get
                    Return New OracleParameter("IDCULTURAS", OracleDbType.Varchar2, 5)
                End Get
            End Property

            Public Shared ReadOnly Property NOMBRERECURSO() As OracleParameter
                Get
                    Return New OracleParameter("NOMBRERECURSO", OracleDbType.Varchar2, 150)
                End Get
            End Property

            Public Shared ReadOnly Property DESCRIPRECURSO() As OracleParameter
                Get
                    Return New OracleParameter("DESCRIPRECURSO", OracleDbType.Varchar2, 200)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const IDGRUPOS As String = "IDGRUPOS"
            Public Const IDRECURSOS As String = "IDRECURSOS"
            Public Const IDCULTURAS As String = "IDCULTURAS"
            Public Const NOMBRERECURSO As String = "NOMBRERECURSO"
            Public Const DESCRIPRECURSO As String = "DESCRIPRECURSO"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(IDGRUPOS) = W_RECURSOS_GRUPO.PropertyNames.IDGRUPOS
                    ht(IDRECURSOS) = W_RECURSOS_GRUPO.PropertyNames.IDRECURSOS
                    ht(IDCULTURAS) = W_RECURSOS_GRUPO.PropertyNames.IDCULTURAS
                    ht(NOMBRERECURSO) = W_RECURSOS_GRUPO.PropertyNames.NOMBRERECURSO
                    ht(DESCRIPRECURSO) = W_RECURSOS_GRUPO.PropertyNames.DESCRIPRECURSO

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const IDGRUPOS As String = "IDGRUPOS"
            Public Const IDRECURSOS As String = "IDRECURSOS"
            Public Const IDCULTURAS As String = "IDCULTURAS"
            Public Const NOMBRERECURSO As String = "NOMBRERECURSO"
            Public Const DESCRIPRECURSO As String = "DESCRIPRECURSO"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(IDGRUPOS) = W_RECURSOS_GRUPO.ColumnNames.IDGRUPOS
                    ht(IDRECURSOS) = W_RECURSOS_GRUPO.ColumnNames.IDRECURSOS
                    ht(IDCULTURAS) = W_RECURSOS_GRUPO.ColumnNames.IDCULTURAS
                    ht(NOMBRERECURSO) = W_RECURSOS_GRUPO.ColumnNames.NOMBRERECURSO
                    ht(DESCRIPRECURSO) = W_RECURSOS_GRUPO.ColumnNames.DESCRIPRECURSO

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const IDGRUPOS As String = "s_IDGRUPOS"
            Public Const IDRECURSOS As String = "s_IDRECURSOS"
            Public Const IDCULTURAS As String = "s_IDCULTURAS"
            Public Const NOMBRERECURSO As String = "s_NOMBRERECURSO"
            Public Const DESCRIPRECURSO As String = "s_DESCRIPRECURSO"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property IDGRUPOS() As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IDGRUPOS)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IDGRUPOS, Value)
            End Set
        End Property

        Public Overridable Property IDRECURSOS() As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IDRECURSOS)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IDRECURSOS, Value)
            End Set
        End Property

        Public Overridable Property IDCULTURAS() As String
            Get
                Return MyBase.GetString(ColumnNames.IDCULTURAS)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.IDCULTURAS, Value)
            End Set
        End Property

        Public Overridable Property NOMBRERECURSO() As String
            Get
                Return MyBase.GetString(ColumnNames.NOMBRERECURSO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.NOMBRERECURSO, Value)
            End Set
        End Property

        Public Overridable Property DESCRIPRECURSO() As String
            Get
                Return MyBase.GetString(ColumnNames.DESCRIPRECURSO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.DESCRIPRECURSO, Value)
            End Set
        End Property


#End Region

#Region "String Properties"

        Public Overridable Property s_IDGRUPOS() As String
            Get
                If Me.IsColumnNull(ColumnNames.IDGRUPOS) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.IDGRUPOS)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDGRUPOS)
                Else
                    Me.IDGRUPOS = MyBase.SetDecimalAsString(ColumnNames.IDGRUPOS, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_IDRECURSOS() As String
            Get
                If Me.IsColumnNull(ColumnNames.IDRECURSOS) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.IDRECURSOS)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDRECURSOS)
                Else
                    Me.IDRECURSOS = MyBase.SetDecimalAsString(ColumnNames.IDRECURSOS, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_IDCULTURAS() As String
            Get
                If Me.IsColumnNull(ColumnNames.IDCULTURAS) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.IDCULTURAS)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDCULTURAS)
                Else
                    Me.IDCULTURAS = MyBase.SetStringAsString(ColumnNames.IDCULTURAS, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_NOMBRERECURSO() As String
            Get
                If Me.IsColumnNull(ColumnNames.NOMBRERECURSO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.NOMBRERECURSO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NOMBRERECURSO)
                Else
                    Me.NOMBRERECURSO = MyBase.SetStringAsString(ColumnNames.NOMBRERECURSO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_DESCRIPRECURSO() As String
            Get
                If Me.IsColumnNull(ColumnNames.DESCRIPRECURSO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.DESCRIPRECURSO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.DESCRIPRECURSO)
                Else
                    Me.DESCRIPRECURSO = MyBase.SetStringAsString(ColumnNames.DESCRIPRECURSO, Value)
                End If
            End Set
        End Property


#End Region

#Region "Where Clause"
        Public Class WhereClause

            Public Sub New(ByVal entity As BusinessEntity)
                Me._entity = entity
            End Sub

            Public ReadOnly Property TearOff() As TearOffWhereParameter
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


                Public ReadOnly Property IDGRUPOS() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDGRUPOS, Parameters.IDGRUPOS)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDRECURSOS() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDRECURSOS, Parameters.IDRECURSOS)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDCULTURAS() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDCULTURAS, Parameters.IDCULTURAS)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NOMBRERECURSO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NOMBRERECURSO, Parameters.NOMBRERECURSO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DESCRIPRECURSO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.DESCRIPRECURSO, Parameters.DESCRIPRECURSO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property


                Private _clause As WhereClause
            End Class
#End Region

            Public ReadOnly Property IDGRUPOS() As WhereParameter
                Get
                    If _IDGRUPOS_W Is Nothing Then
                        _IDGRUPOS_W = TearOff.IDGRUPOS
                    End If
                    Return _IDGRUPOS_W
                End Get
            End Property

            Public ReadOnly Property IDRECURSOS() As WhereParameter
                Get
                    If _IDRECURSOS_W Is Nothing Then
                        _IDRECURSOS_W = TearOff.IDRECURSOS
                    End If
                    Return _IDRECURSOS_W
                End Get
            End Property

            Public ReadOnly Property IDCULTURAS() As WhereParameter
                Get
                    If _IDCULTURAS_W Is Nothing Then
                        _IDCULTURAS_W = TearOff.IDCULTURAS
                    End If
                    Return _IDCULTURAS_W
                End Get
            End Property

            Public ReadOnly Property NOMBRERECURSO() As WhereParameter
                Get
                    If _NOMBRERECURSO_W Is Nothing Then
                        _NOMBRERECURSO_W = TearOff.NOMBRERECURSO
                    End If
                    Return _NOMBRERECURSO_W
                End Get
            End Property

            Public ReadOnly Property DESCRIPRECURSO() As WhereParameter
                Get
                    If _DESCRIPRECURSO_W Is Nothing Then
                        _DESCRIPRECURSO_W = TearOff.DESCRIPRECURSO
                    End If
                    Return _DESCRIPRECURSO_W
                End Get
            End Property

            Private _IDGRUPOS_W As WhereParameter = Nothing
            Private _IDRECURSOS_W As WhereParameter = Nothing
            Private _IDCULTURAS_W As WhereParameter = Nothing
            Private _NOMBRERECURSO_W As WhereParameter = Nothing
            Private _DESCRIPRECURSO_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _IDGRUPOS_W = Nothing
                _IDRECURSOS_W = Nothing
                _IDCULTURAS_W = Nothing
                _NOMBRERECURSO_W = Nothing
                _DESCRIPRECURSO_W = Nothing
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

            Public ReadOnly Property TearOff() As TearOffAggregateParameter
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


                Public ReadOnly Property IDGRUPOS() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDGRUPOS, Parameters.IDGRUPOS)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDRECURSOS() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDRECURSOS, Parameters.IDRECURSOS)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDCULTURAS() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDCULTURAS, Parameters.IDCULTURAS)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NOMBRERECURSO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NOMBRERECURSO, Parameters.NOMBRERECURSO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DESCRIPRECURSO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DESCRIPRECURSO, Parameters.DESCRIPRECURSO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property


                Private _clause As AggregateClause
            End Class
#End Region

            Public ReadOnly Property IDGRUPOS() As AggregateParameter
                Get
                    If _IDGRUPOS_W Is Nothing Then
                        _IDGRUPOS_W = TearOff.IDGRUPOS
                    End If
                    Return _IDGRUPOS_W
                End Get
            End Property

            Public ReadOnly Property IDRECURSOS() As AggregateParameter
                Get
                    If _IDRECURSOS_W Is Nothing Then
                        _IDRECURSOS_W = TearOff.IDRECURSOS
                    End If
                    Return _IDRECURSOS_W
                End Get
            End Property

            Public ReadOnly Property IDCULTURAS() As AggregateParameter
                Get
                    If _IDCULTURAS_W Is Nothing Then
                        _IDCULTURAS_W = TearOff.IDCULTURAS
                    End If
                    Return _IDCULTURAS_W
                End Get
            End Property

            Public ReadOnly Property NOMBRERECURSO() As AggregateParameter
                Get
                    If _NOMBRERECURSO_W Is Nothing Then
                        _NOMBRERECURSO_W = TearOff.NOMBRERECURSO
                    End If
                    Return _NOMBRERECURSO_W
                End Get
            End Property

            Public ReadOnly Property DESCRIPRECURSO() As AggregateParameter
                Get
                    If _DESCRIPRECURSO_W Is Nothing Then
                        _DESCRIPRECURSO_W = TearOff.DESCRIPRECURSO
                    End If
                    Return _DESCRIPRECURSO_W
                End Get
            End Property

            Private _IDGRUPOS_W As AggregateParameter = Nothing
            Private _IDRECURSOS_W As AggregateParameter = Nothing
            Private _IDCULTURAS_W As AggregateParameter = Nothing
            Private _NOMBRERECURSO_W As AggregateParameter = Nothing
            Private _DESCRIPRECURSO_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _IDGRUPOS_W = Nothing
                _IDRECURSOS_W = Nothing
                _IDCULTURAS_W = Nothing
                _NOMBRERECURSO_W = Nothing
                _DESCRIPRECURSO_W = Nothing
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

