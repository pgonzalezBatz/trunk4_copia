
'===============================================================================
'BATZ, Koop. - 07/03/2008 8:17:35
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

    Public Class W_FCPWTRA
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "SAB."
            Me.QuerySource = "W_FCPWTRA"
            Me.MappingName = "W_FCPWTRA"
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

            Public Shared ReadOnly Property TR000() As OracleParameter
                Get
                    Return New OracleParameter("TR000", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property TR010() As OracleParameter
                Get
                    Return New OracleParameter("TR010", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property TR060() As OracleParameter
                Get
                    Return New OracleParameter("TR060", OracleDbType.VARCHAR2, 6)
                End Get
            End Property

            Public Shared ReadOnly Property TR190() As OracleParameter
                Get
                    Return New OracleParameter("TR190", OracleDbType.VARCHAR2, 50)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const TR000 As String = "TR000"
            Public Const TR010 As String = "TR010"
            Public Const TR060 As String = "TR060"
            Public Const TR190 As String = "TR190"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(TR000) = W_FCPWTRA.PropertyNames.TR000
                    ht(TR010) = W_FCPWTRA.PropertyNames.TR010
                    ht(TR060) = W_FCPWTRA.PropertyNames.TR060
                    ht(TR190) = W_FCPWTRA.PropertyNames.TR190

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const TR000 As String = "TR000"
            Public Const TR010 As String = "TR010"
            Public Const TR060 As String = "TR060"
            Public Const TR190 As String = "TR190"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(TR000) = W_FCPWTRA.ColumnNames.TR000
                    ht(TR010) = W_FCPWTRA.ColumnNames.TR010
                    ht(TR060) = W_FCPWTRA.ColumnNames.TR060
                    ht(TR190) = W_FCPWTRA.ColumnNames.TR190

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const TR000 As String = "s_TR000"
            Public Const TR010 As String = "s_TR010"
            Public Const TR060 As String = "s_TR060"
            Public Const TR190 As String = "s_TR190"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property TR000() As Integer
            Get
                Return MyBase.GetInteger(ColumnNames.TR000)
            End Get
            Set(ByVal Value As Integer)
                MyBase.SetInteger(ColumnNames.TR000, Value)
            End Set
        End Property

        Public Overridable Property TR010() As Integer
            Get
                Return MyBase.GetInteger(ColumnNames.TR010)
            End Get
            Set(ByVal Value As Integer)
                MyBase.SetInteger(ColumnNames.TR010, Value)
            End Set
        End Property

        Public Overridable Property TR060() As String
            Get
                Return MyBase.GetString(ColumnNames.TR060)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.TR060, Value)
            End Set
        End Property

        Public Overridable Property TR190() As String
            Get
                Return MyBase.GetString(ColumnNames.TR190)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.TR190, Value)
            End Set
        End Property


#End Region

#Region "String Properties"

        Public Overridable Property s_TR000() As String
            Get
                If Me.IsColumnNull(ColumnNames.TR000) Then
                    Return String.Empty
                Else
                    Return MyBase.GetIntegerAsString(ColumnNames.TR000)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.TR000)
                Else
                    Me.TR000 = MyBase.SetIntegerAsString(ColumnNames.TR000, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_TR010() As String
            Get
                If Me.IsColumnNull(ColumnNames.TR010) Then
                    Return String.Empty
                Else
                    Return MyBase.GetIntegerAsString(ColumnNames.TR010)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.TR010)
                Else
                    Me.TR010 = MyBase.SetIntegerAsString(ColumnNames.TR010, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_TR060() As String
            Get
                If Me.IsColumnNull(ColumnNames.TR060) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.TR060)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.TR060)
                Else
                    Me.TR060 = MyBase.SetStringAsString(ColumnNames.TR060, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_TR190() As String
            Get
                If Me.IsColumnNull(ColumnNames.TR190) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.TR190)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.TR190)
                Else
                    Me.TR190 = MyBase.SetStringAsString(ColumnNames.TR190, Value)
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


                Public ReadOnly Property TR000() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.TR000, Parameters.TR000)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TR010() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.TR010, Parameters.TR010)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TR060() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.TR060, Parameters.TR060)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TR190() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.TR190, Parameters.TR190)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property


                Private _clause As WhereClause
            End Class
#End Region

            Public ReadOnly Property TR000() As WhereParameter
                Get
                    If _TR000_W Is Nothing Then
                        _TR000_W = TearOff.TR000
                    End If
                    Return _TR000_W
                End Get
            End Property

            Public ReadOnly Property TR010() As WhereParameter
                Get
                    If _TR010_W Is Nothing Then
                        _TR010_W = TearOff.TR010
                    End If
                    Return _TR010_W
                End Get
            End Property

            Public ReadOnly Property TR060() As WhereParameter
                Get
                    If _TR060_W Is Nothing Then
                        _TR060_W = TearOff.TR060
                    End If
                    Return _TR060_W
                End Get
            End Property

            Public ReadOnly Property TR190() As WhereParameter
                Get
                    If _TR190_W Is Nothing Then
                        _TR190_W = TearOff.TR190
                    End If
                    Return _TR190_W
                End Get
            End Property

            Private _TR000_W As WhereParameter = Nothing
            Private _TR010_W As WhereParameter = Nothing
            Private _TR060_W As WhereParameter = Nothing
            Private _TR190_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _TR000_W = Nothing
                _TR010_W = Nothing
                _TR060_W = Nothing
                _TR190_W = Nothing
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


                Public ReadOnly Property TR000() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TR000, Parameters.TR000)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TR010() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TR010, Parameters.TR010)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TR060() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TR060, Parameters.TR060)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TR190() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TR190, Parameters.TR190)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property


                Private _clause As AggregateClause
            End Class
#End Region

            Public ReadOnly Property TR000() As AggregateParameter
                Get
                    If _TR000_W Is Nothing Then
                        _TR000_W = TearOff.TR000
                    End If
                    Return _TR000_W
                End Get
            End Property

            Public ReadOnly Property TR010() As AggregateParameter
                Get
                    If _TR010_W Is Nothing Then
                        _TR010_W = TearOff.TR010
                    End If
                    Return _TR010_W
                End Get
            End Property

            Public ReadOnly Property TR060() As AggregateParameter
                Get
                    If _TR060_W Is Nothing Then
                        _TR060_W = TearOff.TR060
                    End If
                    Return _TR060_W
                End Get
            End Property

            Public ReadOnly Property TR190() As AggregateParameter
                Get
                    If _TR190_W Is Nothing Then
                        _TR190_W = TearOff.TR190
                    End If
                    Return _TR190_W
                End Get
            End Property

            Private _TR000_W As AggregateParameter = Nothing
            Private _TR010_W As AggregateParameter = Nothing
            Private _TR060_W As AggregateParameter = Nothing
            Private _TR190_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _TR000_W = Nothing
                _TR010_W = Nothing
                _TR060_W = Nothing
                _TR190_W = Nothing
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

