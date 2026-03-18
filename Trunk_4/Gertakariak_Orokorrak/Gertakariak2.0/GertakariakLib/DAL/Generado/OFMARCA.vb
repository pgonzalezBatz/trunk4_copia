
'===============================================================================
'BATZ, Koop. - 14/09/2010 8:09:55
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

    Public MustInherit Class _OFMARCA
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "INCIDENCIAS."
            Me.QuerySource = "OFMARCA"
            Me.MappingName = "OFMARCA"
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


            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_OFMARCA", parameters)
        End Function

        '=================================================================
        ' Public Overridable Function LoadByPrimaryKey()  As Boolean
        '=================================================================
        '  Loads a single row of via the primary key
        '=================================================================
        Public Overridable Function LoadByPrimaryKey(ByVal IDOFMARCA As Decimal) As Boolean

            Dim parameters As ListDictionary = New ListDictionary()
            parameters.Add(_OFMARCA.Parameters.IDOFMARCA, IDOFMARCA)


            Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
            p.Direction = ParameterDirection.Output
            parameters.Add(p, Nothing)

            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_OFMARCA", parameters)

        End Function

#Region "Parameters"
        Protected Class Parameters

            Public Shared ReadOnly Property IDINCIDENCIA As OracleParameter
                Get
                    Return New OracleParameter("p_IDINCIDENCIA", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property NUMOF As OracleParameter
                Get
                    Return New OracleParameter("p_NUMOF", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property OP As OracleParameter
                Get
                    Return New OracleParameter("p_OP", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property MARCA As OracleParameter
                Get
                    Return New OracleParameter("p_MARCA", OracleDbType.Char, 11)
                End Get
            End Property

            Public Shared ReadOnly Property IDOFMARCA As OracleParameter
                Get
                    Return New OracleParameter("p_IDOFMARCA", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property DENOMINACION As OracleParameter
                Get
                    Return New OracleParameter("p_DENOMINACION", OracleDbType.Varchar2, 200)
                End Get
            End Property

            Public Shared ReadOnly Property CANTIDAD As OracleParameter
                Get
                    Return New OracleParameter("p_CANTIDAD", OracleDbType.Int32, 0)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const IDINCIDENCIA As String = "IDINCIDENCIA"
            Public Const NUMOF As String = "NUMOF"
            Public Const OP As String = "OP"
            Public Const MARCA As String = "MARCA"
            Public Const IDOFMARCA As String = "IDOFMARCA"
            Public Const DENOMINACION As String = "DENOMINACION"
            Public Const CANTIDAD As String = "CANTIDAD"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(IDINCIDENCIA) = _OFMARCA.PropertyNames.IDINCIDENCIA
                    ht(NUMOF) = _OFMARCA.PropertyNames.NUMOF
                    ht(OP) = _OFMARCA.PropertyNames.OP
                    ht(MARCA) = _OFMARCA.PropertyNames.MARCA
                    ht(IDOFMARCA) = _OFMARCA.PropertyNames.IDOFMARCA
                    ht(DENOMINACION) = _OFMARCA.PropertyNames.DENOMINACION
                    ht(CANTIDAD) = _OFMARCA.PropertyNames.CANTIDAD

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const IDINCIDENCIA As String = "IDINCIDENCIA"
            Public Const NUMOF As String = "NUMOF"
            Public Const OP As String = "OP"
            Public Const MARCA As String = "MARCA"
            Public Const IDOFMARCA As String = "IDOFMARCA"
            Public Const DENOMINACION As String = "DENOMINACION"
            Public Const CANTIDAD As String = "CANTIDAD"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(IDINCIDENCIA) = _OFMARCA.ColumnNames.IDINCIDENCIA
                    ht(NUMOF) = _OFMARCA.ColumnNames.NUMOF
                    ht(OP) = _OFMARCA.ColumnNames.OP
                    ht(MARCA) = _OFMARCA.ColumnNames.MARCA
                    ht(IDOFMARCA) = _OFMARCA.ColumnNames.IDOFMARCA
                    ht(DENOMINACION) = _OFMARCA.ColumnNames.DENOMINACION
                    ht(CANTIDAD) = _OFMARCA.ColumnNames.CANTIDAD

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const IDINCIDENCIA As String = "s_IDINCIDENCIA"
            Public Const NUMOF As String = "s_NUMOF"
            Public Const OP As String = "s_OP"
            Public Const MARCA As String = "s_MARCA"
            Public Const IDOFMARCA As String = "s_IDOFMARCA"
            Public Const DENOMINACION As String = "s_DENOMINACION"
            Public Const CANTIDAD As String = "s_CANTIDAD"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property IDINCIDENCIA As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IDINCIDENCIA)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IDINCIDENCIA, Value)
            End Set
        End Property

        Public Overridable Property NUMOF As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.NUMOF)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.NUMOF, Value)
            End Set
        End Property

        Public Overridable Property OP As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.OP)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.OP, Value)
            End Set
        End Property

        Public Overridable Property MARCA As String
            Get
                Return MyBase.GetString(ColumnNames.MARCA)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.MARCA, Value)
            End Set
        End Property

        Public Overridable Property IDOFMARCA As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IDOFMARCA)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IDOFMARCA, Value)
            End Set
        End Property

        Public Overridable Property DENOMINACION As String
            Get
                Return MyBase.GetString(ColumnNames.DENOMINACION)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.DENOMINACION, Value)
            End Set
        End Property

        Public Overridable Property CANTIDAD As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.CANTIDAD)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.CANTIDAD, Value)
            End Set
        End Property


#End Region

#Region "String Properties"

        Public Overridable Property s_IDINCIDENCIA As String
            Get
                If Me.IsColumnNull(ColumnNames.IDINCIDENCIA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.IDINCIDENCIA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDINCIDENCIA)
                Else
                    Me.IDINCIDENCIA = MyBase.SetDecimalAsString(ColumnNames.IDINCIDENCIA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_NUMOF As String
            Get
                If Me.IsColumnNull(ColumnNames.NUMOF) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.NUMOF)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NUMOF)
                Else
                    Me.NUMOF = MyBase.SetDecimalAsString(ColumnNames.NUMOF, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_OP As String
            Get
                If Me.IsColumnNull(ColumnNames.OP) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.OP)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.OP)
                Else
                    Me.OP = MyBase.SetDecimalAsString(ColumnNames.OP, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_MARCA As String
            Get
                If Me.IsColumnNull(ColumnNames.MARCA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.MARCA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.MARCA)
                Else
                    Me.MARCA = MyBase.SetStringAsString(ColumnNames.MARCA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_IDOFMARCA As String
            Get
                If Me.IsColumnNull(ColumnNames.IDOFMARCA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.IDOFMARCA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDOFMARCA)
                Else
                    Me.IDOFMARCA = MyBase.SetDecimalAsString(ColumnNames.IDOFMARCA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_DENOMINACION As String
            Get
                If Me.IsColumnNull(ColumnNames.DENOMINACION) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.DENOMINACION)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.DENOMINACION)
                Else
                    Me.DENOMINACION = MyBase.SetStringAsString(ColumnNames.DENOMINACION, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CANTIDAD As String
            Get
                If Me.IsColumnNull(ColumnNames.CANTIDAD) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.CANTIDAD)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CANTIDAD)
                Else
                    Me.CANTIDAD = MyBase.SetDecimalAsString(ColumnNames.CANTIDAD, Value)
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


                Public ReadOnly Property IDINCIDENCIA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDINCIDENCIA, Parameters.IDINCIDENCIA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMOF() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMOF, Parameters.NUMOF)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property OP() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.OP, Parameters.OP)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property MARCA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.MARCA, Parameters.MARCA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDOFMARCA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDOFMARCA, Parameters.IDOFMARCA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DENOMINACION() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.DENOMINACION, Parameters.DENOMINACION)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CANTIDAD() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CANTIDAD, Parameters.CANTIDAD)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property


                Private _clause As WhereClause
            End Class
#End Region

            Public ReadOnly Property IDINCIDENCIA() As WhereParameter
                Get
                    If _IDINCIDENCIA_W Is Nothing Then
                        _IDINCIDENCIA_W = TearOff.IDINCIDENCIA
                    End If
                    Return _IDINCIDENCIA_W
                End Get
            End Property

            Public ReadOnly Property NUMOF() As WhereParameter
                Get
                    If _NUMOF_W Is Nothing Then
                        _NUMOF_W = TearOff.NUMOF
                    End If
                    Return _NUMOF_W
                End Get
            End Property

            Public ReadOnly Property OP() As WhereParameter
                Get
                    If _OP_W Is Nothing Then
                        _OP_W = TearOff.OP
                    End If
                    Return _OP_W
                End Get
            End Property

            Public ReadOnly Property MARCA() As WhereParameter
                Get
                    If _MARCA_W Is Nothing Then
                        _MARCA_W = TearOff.MARCA
                    End If
                    Return _MARCA_W
                End Get
            End Property

            Public ReadOnly Property IDOFMARCA() As WhereParameter
                Get
                    If _IDOFMARCA_W Is Nothing Then
                        _IDOFMARCA_W = TearOff.IDOFMARCA
                    End If
                    Return _IDOFMARCA_W
                End Get
            End Property

            Public ReadOnly Property DENOMINACION() As WhereParameter
                Get
                    If _DENOMINACION_W Is Nothing Then
                        _DENOMINACION_W = TearOff.DENOMINACION
                    End If
                    Return _DENOMINACION_W
                End Get
            End Property

            Public ReadOnly Property CANTIDAD() As WhereParameter
                Get
                    If _CANTIDAD_W Is Nothing Then
                        _CANTIDAD_W = TearOff.CANTIDAD
                    End If
                    Return _CANTIDAD_W
                End Get
            End Property

            Private _IDINCIDENCIA_W As WhereParameter = Nothing
            Private _NUMOF_W As WhereParameter = Nothing
            Private _OP_W As WhereParameter = Nothing
            Private _MARCA_W As WhereParameter = Nothing
            Private _IDOFMARCA_W As WhereParameter = Nothing
            Private _DENOMINACION_W As WhereParameter = Nothing
            Private _CANTIDAD_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _IDINCIDENCIA_W = Nothing
                _NUMOF_W = Nothing
                _OP_W = Nothing
                _MARCA_W = Nothing
                _IDOFMARCA_W = Nothing
                _DENOMINACION_W = Nothing
                _CANTIDAD_W = Nothing
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


                Public ReadOnly Property IDINCIDENCIA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDINCIDENCIA, Parameters.IDINCIDENCIA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMOF() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMOF, Parameters.NUMOF)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property OP() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.OP, Parameters.OP)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property MARCA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.MARCA, Parameters.MARCA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDOFMARCA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDOFMARCA, Parameters.IDOFMARCA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DENOMINACION() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DENOMINACION, Parameters.DENOMINACION)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CANTIDAD() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CANTIDAD, Parameters.CANTIDAD)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property


                Private _clause As AggregateClause
            End Class
#End Region

            Public ReadOnly Property IDINCIDENCIA() As AggregateParameter
                Get
                    If _IDINCIDENCIA_W Is Nothing Then
                        _IDINCIDENCIA_W = TearOff.IDINCIDENCIA
                    End If
                    Return _IDINCIDENCIA_W
                End Get
            End Property

            Public ReadOnly Property NUMOF() As AggregateParameter
                Get
                    If _NUMOF_W Is Nothing Then
                        _NUMOF_W = TearOff.NUMOF
                    End If
                    Return _NUMOF_W
                End Get
            End Property

            Public ReadOnly Property OP() As AggregateParameter
                Get
                    If _OP_W Is Nothing Then
                        _OP_W = TearOff.OP
                    End If
                    Return _OP_W
                End Get
            End Property

            Public ReadOnly Property MARCA() As AggregateParameter
                Get
                    If _MARCA_W Is Nothing Then
                        _MARCA_W = TearOff.MARCA
                    End If
                    Return _MARCA_W
                End Get
            End Property

            Public ReadOnly Property IDOFMARCA() As AggregateParameter
                Get
                    If _IDOFMARCA_W Is Nothing Then
                        _IDOFMARCA_W = TearOff.IDOFMARCA
                    End If
                    Return _IDOFMARCA_W
                End Get
            End Property

            Public ReadOnly Property DENOMINACION() As AggregateParameter
                Get
                    If _DENOMINACION_W Is Nothing Then
                        _DENOMINACION_W = TearOff.DENOMINACION
                    End If
                    Return _DENOMINACION_W
                End Get
            End Property

            Public ReadOnly Property CANTIDAD() As AggregateParameter
                Get
                    If _CANTIDAD_W Is Nothing Then
                        _CANTIDAD_W = TearOff.CANTIDAD
                    End If
                    Return _CANTIDAD_W
                End Get
            End Property

            Private _IDINCIDENCIA_W As AggregateParameter = Nothing
            Private _NUMOF_W As AggregateParameter = Nothing
            Private _OP_W As AggregateParameter = Nothing
            Private _MARCA_W As AggregateParameter = Nothing
            Private _IDOFMARCA_W As AggregateParameter = Nothing
            Private _DENOMINACION_W As AggregateParameter = Nothing
            Private _CANTIDAD_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _IDINCIDENCIA_W = Nothing
                _NUMOF_W = Nothing
                _OP_W = Nothing
                _MARCA_W = Nothing
                _IDOFMARCA_W = Nothing
                _DENOMINACION_W = Nothing
                _CANTIDAD_W = Nothing
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
            cmd.CommandText = Me.SchemaStoredProcedure + "PI_OFMARCA"

            CreateParameters(cmd)


            Dim p As OracleParameter
            p = cmd.Parameters(Parameters.IDOFMARCA.ParameterName)
            p.Direction = ParameterDirection.Output
            p.DbType = DbType.Decimal

            Return cmd

        End Function

        Protected Overrides Function GetUpdateCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PU_OFMARCA"

            CreateParameters(cmd)

            Return cmd

        End Function

        Protected Overrides Function GetDeleteCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PD_OFMARCA"

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.IDOFMARCA)
            p.SourceColumn = ColumnNames.IDOFMARCA
            p.SourceVersion = DataRowVersion.Current


            Return cmd

        End Function

        Private Sub CreateParameters(ByVal cmd As OracleCommand)

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.IDINCIDENCIA)
            p.SourceColumn = ColumnNames.IDINCIDENCIA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMOF)
            p.SourceColumn = ColumnNames.NUMOF
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.OP)
            p.SourceColumn = ColumnNames.OP
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.MARCA)
            p.SourceColumn = ColumnNames.MARCA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDOFMARCA)
            p.SourceColumn = ColumnNames.IDOFMARCA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.DENOMINACION)
            p.SourceColumn = ColumnNames.DENOMINACION
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CANTIDAD)
            p.SourceColumn = ColumnNames.CANTIDAD
            p.SourceVersion = DataRowVersion.Current


        End Sub

    End Class

End Namespace

