
'===============================================================================
'BATZ, Koop. - 29/01/2009 10:16:34
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

    Public MustInherit Class _USUARIOS
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "INCIDENCIAS."
            Me.QuerySource = "USUARIOS"
            Me.MappingName = "USUARIOS"
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


            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_USUARIOS", parameters)
        End Function

        '=================================================================
        ' Public Overridable Function LoadByPrimaryKey()  As Boolean
        '=================================================================
        '  Loads a single row of via the primary key
        '=================================================================
        Public Overridable Function LoadByPrimaryKey(ByVal IDCLASFAMILIA As Decimal, ByVal IDGRUPO As Decimal, ByVal IDTIPOINCIDENCIA As Decimal, ByVal IDUSRSAB As Decimal) As Boolean

            Dim parameters As ListDictionary = New ListDictionary()
            parameters.Add(_USUARIOS.Parameters.IDCLASFAMILIA, IDCLASFAMILIA)

            parameters.Add(_USUARIOS.Parameters.IDGRUPO, IDGRUPO)

            parameters.Add(_USUARIOS.Parameters.IDTIPOINCIDENCIA, IDTIPOINCIDENCIA)

            parameters.Add(_USUARIOS.Parameters.IDUSRSAB, IDUSRSAB)


            Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
            p.Direction = ParameterDirection.Output
            parameters.Add(p, Nothing)

            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_USUARIOS", parameters)

        End Function

#Region "Parameters"
        Protected Class Parameters

            Public Shared ReadOnly Property IDUSRSAB As OracleParameter
                Get
                    Return New OracleParameter("p_IDUSRSAB", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property IDCLASFAMILIA As OracleParameter
                Get
                    Return New OracleParameter("p_IDCLASFAMILIA", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property IDGRUPO As OracleParameter
                Get
                    Return New OracleParameter("p_IDGRUPO", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property IDTIPOINCIDENCIA As OracleParameter
                Get
                    Return New OracleParameter("p_IDTIPOINCIDENCIA", OracleDbType.Int32, 0)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const IDUSRSAB As String = "IDUSRSAB"
            Public Const IDCLASFAMILIA As String = "IDCLASFAMILIA"
            Public Const IDGRUPO As String = "IDGRUPO"
            Public Const IDTIPOINCIDENCIA As String = "IDTIPOINCIDENCIA"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(IDUSRSAB) = _USUARIOS.PropertyNames.IDUSRSAB
                    ht(IDCLASFAMILIA) = _USUARIOS.PropertyNames.IDCLASFAMILIA
                    ht(IDGRUPO) = _USUARIOS.PropertyNames.IDGRUPO
                    ht(IDTIPOINCIDENCIA) = _USUARIOS.PropertyNames.IDTIPOINCIDENCIA

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const IDUSRSAB As String = "IDUSRSAB"
            Public Const IDCLASFAMILIA As String = "IDCLASFAMILIA"
            Public Const IDGRUPO As String = "IDGRUPO"
            Public Const IDTIPOINCIDENCIA As String = "IDTIPOINCIDENCIA"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(IDUSRSAB) = _USUARIOS.ColumnNames.IDUSRSAB
                    ht(IDCLASFAMILIA) = _USUARIOS.ColumnNames.IDCLASFAMILIA
                    ht(IDGRUPO) = _USUARIOS.ColumnNames.IDGRUPO
                    ht(IDTIPOINCIDENCIA) = _USUARIOS.ColumnNames.IDTIPOINCIDENCIA

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const IDUSRSAB As String = "s_IDUSRSAB"
            Public Const IDCLASFAMILIA As String = "s_IDCLASFAMILIA"
            Public Const IDGRUPO As String = "s_IDGRUPO"
            Public Const IDTIPOINCIDENCIA As String = "s_IDTIPOINCIDENCIA"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property IDUSRSAB As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IDUSRSAB)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IDUSRSAB, Value)
            End Set
        End Property

        Public Overridable Property IDCLASFAMILIA As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IDCLASFAMILIA)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IDCLASFAMILIA, Value)
            End Set
        End Property

        Public Overridable Property IDGRUPO As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IDGRUPO)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IDGRUPO, Value)
            End Set
        End Property

        Public Overridable Property IDTIPOINCIDENCIA As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IDTIPOINCIDENCIA)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IDTIPOINCIDENCIA, Value)
            End Set
        End Property


#End Region

#Region "String Properties"

        Public Overridable Property s_IDUSRSAB As String
            Get
                If Me.IsColumnNull(ColumnNames.IDUSRSAB) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.IDUSRSAB)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDUSRSAB)
                Else
                    Me.IDUSRSAB = MyBase.SetDecimalAsString(ColumnNames.IDUSRSAB, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_IDCLASFAMILIA As String
            Get
                If Me.IsColumnNull(ColumnNames.IDCLASFAMILIA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.IDCLASFAMILIA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDCLASFAMILIA)
                Else
                    Me.IDCLASFAMILIA = MyBase.SetDecimalAsString(ColumnNames.IDCLASFAMILIA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_IDGRUPO As String
            Get
                If Me.IsColumnNull(ColumnNames.IDGRUPO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.IDGRUPO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDGRUPO)
                Else
                    Me.IDGRUPO = MyBase.SetDecimalAsString(ColumnNames.IDGRUPO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_IDTIPOINCIDENCIA As String
            Get
                If Me.IsColumnNull(ColumnNames.IDTIPOINCIDENCIA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.IDTIPOINCIDENCIA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDTIPOINCIDENCIA)
                Else
                    Me.IDTIPOINCIDENCIA = MyBase.SetDecimalAsString(ColumnNames.IDTIPOINCIDENCIA, Value)
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


                Public ReadOnly Property IDUSRSAB() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDUSRSAB, Parameters.IDUSRSAB)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDCLASFAMILIA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDCLASFAMILIA, Parameters.IDCLASFAMILIA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDGRUPO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDGRUPO, Parameters.IDGRUPO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDTIPOINCIDENCIA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDTIPOINCIDENCIA, Parameters.IDTIPOINCIDENCIA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property


                Private _clause As WhereClause
            End Class
#End Region

            Public ReadOnly Property IDUSRSAB() As WhereParameter
                Get
                    If _IDUSRSAB_W Is Nothing Then
                        _IDUSRSAB_W = TearOff.IDUSRSAB
                    End If
                    Return _IDUSRSAB_W
                End Get
            End Property

            Public ReadOnly Property IDCLASFAMILIA() As WhereParameter
                Get
                    If _IDCLASFAMILIA_W Is Nothing Then
                        _IDCLASFAMILIA_W = TearOff.IDCLASFAMILIA
                    End If
                    Return _IDCLASFAMILIA_W
                End Get
            End Property

            Public ReadOnly Property IDGRUPO() As WhereParameter
                Get
                    If _IDGRUPO_W Is Nothing Then
                        _IDGRUPO_W = TearOff.IDGRUPO
                    End If
                    Return _IDGRUPO_W
                End Get
            End Property

            Public ReadOnly Property IDTIPOINCIDENCIA() As WhereParameter
                Get
                    If _IDTIPOINCIDENCIA_W Is Nothing Then
                        _IDTIPOINCIDENCIA_W = TearOff.IDTIPOINCIDENCIA
                    End If
                    Return _IDTIPOINCIDENCIA_W
                End Get
            End Property

            Private _IDUSRSAB_W As WhereParameter = Nothing
            Private _IDCLASFAMILIA_W As WhereParameter = Nothing
            Private _IDGRUPO_W As WhereParameter = Nothing
            Private _IDTIPOINCIDENCIA_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _IDUSRSAB_W = Nothing
                _IDCLASFAMILIA_W = Nothing
                _IDGRUPO_W = Nothing
                _IDTIPOINCIDENCIA_W = Nothing
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


                Public ReadOnly Property IDUSRSAB() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDUSRSAB, Parameters.IDUSRSAB)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDCLASFAMILIA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDCLASFAMILIA, Parameters.IDCLASFAMILIA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDGRUPO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDGRUPO, Parameters.IDGRUPO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDTIPOINCIDENCIA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDTIPOINCIDENCIA, Parameters.IDTIPOINCIDENCIA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property


                Private _clause As AggregateClause
            End Class
#End Region

            Public ReadOnly Property IDUSRSAB() As AggregateParameter
                Get
                    If _IDUSRSAB_W Is Nothing Then
                        _IDUSRSAB_W = TearOff.IDUSRSAB
                    End If
                    Return _IDUSRSAB_W
                End Get
            End Property

            Public ReadOnly Property IDCLASFAMILIA() As AggregateParameter
                Get
                    If _IDCLASFAMILIA_W Is Nothing Then
                        _IDCLASFAMILIA_W = TearOff.IDCLASFAMILIA
                    End If
                    Return _IDCLASFAMILIA_W
                End Get
            End Property

            Public ReadOnly Property IDGRUPO() As AggregateParameter
                Get
                    If _IDGRUPO_W Is Nothing Then
                        _IDGRUPO_W = TearOff.IDGRUPO
                    End If
                    Return _IDGRUPO_W
                End Get
            End Property

            Public ReadOnly Property IDTIPOINCIDENCIA() As AggregateParameter
                Get
                    If _IDTIPOINCIDENCIA_W Is Nothing Then
                        _IDTIPOINCIDENCIA_W = TearOff.IDTIPOINCIDENCIA
                    End If
                    Return _IDTIPOINCIDENCIA_W
                End Get
            End Property

            Private _IDUSRSAB_W As AggregateParameter = Nothing
            Private _IDCLASFAMILIA_W As AggregateParameter = Nothing
            Private _IDGRUPO_W As AggregateParameter = Nothing
            Private _IDTIPOINCIDENCIA_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _IDUSRSAB_W = Nothing
                _IDCLASFAMILIA_W = Nothing
                _IDGRUPO_W = Nothing
                _IDTIPOINCIDENCIA_W = Nothing
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
            cmd.CommandText = Me.SchemaStoredProcedure + "PI_USUARIOS"

            CreateParameters(cmd)


            Return cmd

        End Function

        Protected Overrides Function GetUpdateCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PU_USUARIOS"

            CreateParameters(cmd)

            Return cmd

        End Function

        Protected Overrides Function GetDeleteCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PD_USUARIOS"

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.IDCLASFAMILIA)
            p.SourceColumn = ColumnNames.IDCLASFAMILIA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDGRUPO)
            p.SourceColumn = ColumnNames.IDGRUPO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDTIPOINCIDENCIA)
            p.SourceColumn = ColumnNames.IDTIPOINCIDENCIA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDUSRSAB)
            p.SourceColumn = ColumnNames.IDUSRSAB
            p.SourceVersion = DataRowVersion.Current


            Return cmd

        End Function

        Private Sub CreateParameters(ByVal cmd As OracleCommand)

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.IDUSRSAB)
            p.SourceColumn = ColumnNames.IDUSRSAB
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDCLASFAMILIA)
            p.SourceColumn = ColumnNames.IDCLASFAMILIA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDGRUPO)
            p.SourceColumn = ColumnNames.IDGRUPO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDTIPOINCIDENCIA)
            p.SourceColumn = ColumnNames.IDTIPOINCIDENCIA
            p.SourceVersion = DataRowVersion.Current


        End Sub

    End Class

End Namespace

