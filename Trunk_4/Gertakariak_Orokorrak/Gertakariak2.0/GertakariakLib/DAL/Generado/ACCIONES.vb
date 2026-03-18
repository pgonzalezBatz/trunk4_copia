
'===============================================================================
'BATZ, Koop. - 14/09/2010 8:16:51
' Generado por MyGeneration Version # (1.3.0.9)
' Generado desde Batz_VbNet_SQL_dOOdads_BusinessEntity.vbgen
' El soporte de la clase OracleClientEntity esta en el directorio Architecture  en "dOOdads".
'===============================================================================

Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Specialized

Imports AccesoAutomaticoBD
Imports System.Data.OracleClient
Imports Oracle.ManagedDataAccess.Client
Imports OracleParameter = Oracle.ManagedDataAccess.Client.OracleParameter
Imports OracleCommand = Oracle.ManagedDataAccess.Client.OracleCommand

Namespace DAL

    Public MustInherit Class _ACCIONES
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "INCIDENCIAS."
            Me.QuerySource = "ACCIONES"
            Me.MappingName = "ACCIONES"
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


            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_ACCIONES", parameters)
        End Function

        '=================================================================
        ' Public Overridable Function LoadByPrimaryKey()  As Boolean
        '=================================================================
        '  Loads a single row of via the primary key
        '=================================================================
        Public Overridable Function LoadByPrimaryKey(ByVal ID As Decimal) As Boolean

            Dim parameters As ListDictionary = New ListDictionary()
            parameters.Add(_ACCIONES.Parameters.ID, ID)


            Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
            p.Direction = ParameterDirection.Output
            parameters.Add(p, Nothing)

            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_ACCIONES", parameters)

        End Function

#Region "Parameters"
        Protected Class Parameters

            Public Shared ReadOnly Property ID As OracleParameter
                Get
                    Return New OracleParameter("p_ID", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property DESCRIPCION As OracleParameter
                Get
                    Return New OracleParameter("p_DESCRIPCION", OracleDbType.Varchar2, 4000)
                End Get
            End Property

            Public Shared ReadOnly Property IDTIPOACCION As OracleParameter
                Get
                    Return New OracleParameter("p_IDTIPOACCION", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property FECHAINICIO As OracleParameter
                Get
                    Return New OracleParameter("p_FECHAINICIO", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property FECHAFIN As OracleParameter
                Get
                    Return New OracleParameter("p_FECHAFIN", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property FECHAPREVISTA As OracleParameter
                Get
                    Return New OracleParameter("p_FECHAPREVISTA", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property EFICACIA As OracleParameter
                Get
                    Return New OracleParameter("p_EFICACIA", OracleDbType.Varchar2, 4000)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const ID As String = "ID"
            Public Const DESCRIPCION As String = "DESCRIPCION"
            Public Const IDTIPOACCION As String = "IDTIPOACCION"
            Public Const FECHAINICIO As String = "FECHAINICIO"
            Public Const FECHAFIN As String = "FECHAFIN"
            Public Const FECHAPREVISTA As String = "FECHAPREVISTA"
            Public Const EFICACIA As String = "EFICACIA"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(ID) = _ACCIONES.PropertyNames.ID
                    ht(DESCRIPCION) = _ACCIONES.PropertyNames.DESCRIPCION
                    ht(IDTIPOACCION) = _ACCIONES.PropertyNames.IDTIPOACCION
                    ht(FECHAINICIO) = _ACCIONES.PropertyNames.FECHAINICIO
                    ht(FECHAFIN) = _ACCIONES.PropertyNames.FECHAFIN
                    ht(FECHAPREVISTA) = _ACCIONES.PropertyNames.FECHAPREVISTA
                    ht(EFICACIA) = _ACCIONES.PropertyNames.EFICACIA

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const ID As String = "ID"
            Public Const DESCRIPCION As String = "DESCRIPCION"
            Public Const IDTIPOACCION As String = "IDTIPOACCION"
            Public Const FECHAINICIO As String = "FECHAINICIO"
            Public Const FECHAFIN As String = "FECHAFIN"
            Public Const FECHAPREVISTA As String = "FECHAPREVISTA"
            Public Const EFICACIA As String = "EFICACIA"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(ID) = _ACCIONES.ColumnNames.ID
                    ht(DESCRIPCION) = _ACCIONES.ColumnNames.DESCRIPCION
                    ht(IDTIPOACCION) = _ACCIONES.ColumnNames.IDTIPOACCION
                    ht(FECHAINICIO) = _ACCIONES.ColumnNames.FECHAINICIO
                    ht(FECHAFIN) = _ACCIONES.ColumnNames.FECHAFIN
                    ht(FECHAPREVISTA) = _ACCIONES.ColumnNames.FECHAPREVISTA
                    ht(EFICACIA) = _ACCIONES.ColumnNames.EFICACIA

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const ID As String = "s_ID"
            Public Const DESCRIPCION As String = "s_DESCRIPCION"
            Public Const IDTIPOACCION As String = "s_IDTIPOACCION"
            Public Const FECHAINICIO As String = "s_FECHAINICIO"
            Public Const FECHAFIN As String = "s_FECHAFIN"
            Public Const FECHAPREVISTA As String = "s_FECHAPREVISTA"
            Public Const EFICACIA As String = "s_EFICACIA"

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

        Public Overridable Property DESCRIPCION As String
            Get
                Return MyBase.GetString(ColumnNames.DESCRIPCION)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.DESCRIPCION, Value)
            End Set
        End Property

        Public Overridable Property IDTIPOACCION As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IDTIPOACCION)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IDTIPOACCION, Value)
            End Set
        End Property

        Public Overridable Property FECHAINICIO As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECHAINICIO)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECHAINICIO, Value)
            End Set
        End Property

        Public Overridable Property FECHAFIN As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECHAFIN)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECHAFIN, Value)
            End Set
        End Property

        Public Overridable Property FECHAPREVISTA As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECHAPREVISTA)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECHAPREVISTA, Value)
            End Set
        End Property

        Public Overridable Property EFICACIA As String
            Get
                Return MyBase.GetString(ColumnNames.EFICACIA)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.EFICACIA, Value)
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

        Public Overridable Property s_IDTIPOACCION As String
            Get
                If Me.IsColumnNull(ColumnNames.IDTIPOACCION) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.IDTIPOACCION)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDTIPOACCION)
                Else
                    Me.IDTIPOACCION = MyBase.SetDecimalAsString(ColumnNames.IDTIPOACCION, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FECHAINICIO As String
            Get
                If Me.IsColumnNull(ColumnNames.FECHAINICIO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FECHAINICIO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FECHAINICIO)
                Else
                    Me.FECHAINICIO = MyBase.SetDateTimeAsString(ColumnNames.FECHAINICIO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FECHAFIN As String
            Get
                If Me.IsColumnNull(ColumnNames.FECHAFIN) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FECHAFIN)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FECHAFIN)
                Else
                    Me.FECHAFIN = MyBase.SetDateTimeAsString(ColumnNames.FECHAFIN, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FECHAPREVISTA As String
            Get
                If Me.IsColumnNull(ColumnNames.FECHAPREVISTA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FECHAPREVISTA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FECHAPREVISTA)
                Else
                    Me.FECHAPREVISTA = MyBase.SetDateTimeAsString(ColumnNames.FECHAPREVISTA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_EFICACIA As String
            Get
                If Me.IsColumnNull(ColumnNames.EFICACIA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.EFICACIA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.EFICACIA)
                Else
                    Me.EFICACIA = MyBase.SetStringAsString(ColumnNames.EFICACIA, Value)
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

                Public ReadOnly Property DESCRIPCION() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.DESCRIPCION, Parameters.DESCRIPCION)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDTIPOACCION() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDTIPOACCION, Parameters.IDTIPOACCION)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECHAINICIO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FECHAINICIO, Parameters.FECHAINICIO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECHAFIN() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FECHAFIN, Parameters.FECHAFIN)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECHAPREVISTA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FECHAPREVISTA, Parameters.FECHAPREVISTA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property EFICACIA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.EFICACIA, Parameters.EFICACIA)
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

            Public ReadOnly Property DESCRIPCION() As WhereParameter
                Get
                    If _DESCRIPCION_W Is Nothing Then
                        _DESCRIPCION_W = TearOff.DESCRIPCION
                    End If
                    Return _DESCRIPCION_W
                End Get
            End Property

            Public ReadOnly Property IDTIPOACCION() As WhereParameter
                Get
                    If _IDTIPOACCION_W Is Nothing Then
                        _IDTIPOACCION_W = TearOff.IDTIPOACCION
                    End If
                    Return _IDTIPOACCION_W
                End Get
            End Property

            Public ReadOnly Property FECHAINICIO() As WhereParameter
                Get
                    If _FECHAINICIO_W Is Nothing Then
                        _FECHAINICIO_W = TearOff.FECHAINICIO
                    End If
                    Return _FECHAINICIO_W
                End Get
            End Property

            Public ReadOnly Property FECHAFIN() As WhereParameter
                Get
                    If _FECHAFIN_W Is Nothing Then
                        _FECHAFIN_W = TearOff.FECHAFIN
                    End If
                    Return _FECHAFIN_W
                End Get
            End Property

            Public ReadOnly Property FECHAPREVISTA() As WhereParameter
                Get
                    If _FECHAPREVISTA_W Is Nothing Then
                        _FECHAPREVISTA_W = TearOff.FECHAPREVISTA
                    End If
                    Return _FECHAPREVISTA_W
                End Get
            End Property

            Public ReadOnly Property EFICACIA() As WhereParameter
                Get
                    If _EFICACIA_W Is Nothing Then
                        _EFICACIA_W = TearOff.EFICACIA
                    End If
                    Return _EFICACIA_W
                End Get
            End Property

            Private _ID_W As WhereParameter = Nothing
            Private _DESCRIPCION_W As WhereParameter = Nothing
            Private _IDTIPOACCION_W As WhereParameter = Nothing
            Private _FECHAINICIO_W As WhereParameter = Nothing
            Private _FECHAFIN_W As WhereParameter = Nothing
            Private _FECHAPREVISTA_W As WhereParameter = Nothing
            Private _EFICACIA_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _ID_W = Nothing
                _DESCRIPCION_W = Nothing
                _IDTIPOACCION_W = Nothing
                _FECHAINICIO_W = Nothing
                _FECHAFIN_W = Nothing
                _FECHAPREVISTA_W = Nothing
                _EFICACIA_W = Nothing
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

                Public ReadOnly Property DESCRIPCION() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DESCRIPCION, Parameters.DESCRIPCION)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDTIPOACCION() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDTIPOACCION, Parameters.IDTIPOACCION)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECHAINICIO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECHAINICIO, Parameters.FECHAINICIO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECHAFIN() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECHAFIN, Parameters.FECHAFIN)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECHAPREVISTA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECHAPREVISTA, Parameters.FECHAPREVISTA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property EFICACIA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.EFICACIA, Parameters.EFICACIA)
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

            Public ReadOnly Property DESCRIPCION() As AggregateParameter
                Get
                    If _DESCRIPCION_W Is Nothing Then
                        _DESCRIPCION_W = TearOff.DESCRIPCION
                    End If
                    Return _DESCRIPCION_W
                End Get
            End Property

            Public ReadOnly Property IDTIPOACCION() As AggregateParameter
                Get
                    If _IDTIPOACCION_W Is Nothing Then
                        _IDTIPOACCION_W = TearOff.IDTIPOACCION
                    End If
                    Return _IDTIPOACCION_W
                End Get
            End Property

            Public ReadOnly Property FECHAINICIO() As AggregateParameter
                Get
                    If _FECHAINICIO_W Is Nothing Then
                        _FECHAINICIO_W = TearOff.FECHAINICIO
                    End If
                    Return _FECHAINICIO_W
                End Get
            End Property

            Public ReadOnly Property FECHAFIN() As AggregateParameter
                Get
                    If _FECHAFIN_W Is Nothing Then
                        _FECHAFIN_W = TearOff.FECHAFIN
                    End If
                    Return _FECHAFIN_W
                End Get
            End Property

            Public ReadOnly Property FECHAPREVISTA() As AggregateParameter
                Get
                    If _FECHAPREVISTA_W Is Nothing Then
                        _FECHAPREVISTA_W = TearOff.FECHAPREVISTA
                    End If
                    Return _FECHAPREVISTA_W
                End Get
            End Property

            Public ReadOnly Property EFICACIA() As AggregateParameter
                Get
                    If _EFICACIA_W Is Nothing Then
                        _EFICACIA_W = TearOff.EFICACIA
                    End If
                    Return _EFICACIA_W
                End Get
            End Property

            Private _ID_W As AggregateParameter = Nothing
            Private _DESCRIPCION_W As AggregateParameter = Nothing
            Private _IDTIPOACCION_W As AggregateParameter = Nothing
            Private _FECHAINICIO_W As AggregateParameter = Nothing
            Private _FECHAFIN_W As AggregateParameter = Nothing
            Private _FECHAPREVISTA_W As AggregateParameter = Nothing
            Private _EFICACIA_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _ID_W = Nothing
                _DESCRIPCION_W = Nothing
                _IDTIPOACCION_W = Nothing
                _FECHAINICIO_W = Nothing
                _FECHAFIN_W = Nothing
                _FECHAPREVISTA_W = Nothing
                _EFICACIA_W = Nothing
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
            cmd.CommandText = Me.SchemaStoredProcedure + "PI_ACCIONES"

            CreateParameters(cmd)


            Dim p As OracleParameter
            p = cmd.Parameters(Parameters.ID.ParameterName)
            p.Direction = ParameterDirection.Output
            p.DbType = DbType.Decimal

            Return cmd

        End Function

        Protected Overrides Function GetUpdateCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PU_ACCIONES"

            CreateParameters(cmd)

            Return cmd

        End Function

        Protected Overrides Function GetDeleteCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PD_ACCIONES"

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.ID)
            p.SourceColumn = ColumnNames.ID
            p.SourceVersion = DataRowVersion.Current


            Return cmd

        End Function

        Private Sub CreateParameters(ByVal cmd As OracleCommand)

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.ID)
            p.SourceColumn = ColumnNames.ID
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.DESCRIPCION)
            p.SourceColumn = ColumnNames.DESCRIPCION
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDTIPOACCION)
            p.SourceColumn = ColumnNames.IDTIPOACCION
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.FECHAINICIO)
            p.SourceColumn = ColumnNames.FECHAINICIO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.FECHAFIN)
            p.SourceColumn = ColumnNames.FECHAFIN
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.FECHAPREVISTA)
            p.SourceColumn = ColumnNames.FECHAPREVISTA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.EFICACIA)
            p.SourceColumn = ColumnNames.EFICACIA
            p.SourceVersion = DataRowVersion.Current


        End Sub

    End Class

End Namespace

