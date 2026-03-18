
'===============================================================================
'BATZ, Koop. - 13/09/2010 16:31:00
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

    Public MustInherit Class _GERTAKARIAK_IRUDIAK
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "INCIDENCIAS."
            Me.QuerySource = "GERTAKARIAK_IRUDIAK"
            Me.MappingName = "GERTAKARIAK_IRUDIAK"
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


            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_GERTAKARIAK_IRUDIAK", parameters)
        End Function

        '=================================================================
        ' Public Overridable Function LoadByPrimaryKey()  As Boolean
        '=================================================================
        '  Loads a single row of via the primary key
        '=================================================================
        Public Overridable Function LoadByPrimaryKey(ByVal ID As Decimal, ByVal IDINCIDENCIA As Decimal) As Boolean

            Dim parameters As ListDictionary = New ListDictionary()
            parameters.Add(_GERTAKARIAK_IRUDIAK.Parameters.ID, ID)

            parameters.Add(_GERTAKARIAK_IRUDIAK.Parameters.IDINCIDENCIA, IDINCIDENCIA)


            Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
            p.Direction = ParameterDirection.Output
            parameters.Add(p, Nothing)

            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_GERTAKARIAK_IRUDIAK", parameters)

        End Function

#Region "Parameters"
        Protected Class Parameters

            Public Shared ReadOnly Property ID As OracleParameter
                Get
                    Return New OracleParameter("p_ID", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property IDINCIDENCIA As OracleParameter
                Get
                    Return New OracleParameter("p_IDINCIDENCIA", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property IMAGEN As OracleParameter
                Get
                    Return New OracleParameter("p_IMAGEN", OracleDbType.Blob, 2147483647)
                End Get
            End Property

            Public Shared ReadOnly Property DESCRIPCION As OracleParameter
                Get
                    Return New OracleParameter("p_DESCRIPCION", OracleDbType.NVarchar2, 2000)
                End Get
            End Property

            Public Shared ReadOnly Property EXTENSION As OracleParameter
                Get
                    Return New OracleParameter("p_EXTENSION", OracleDbType.NVarchar2, 4)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const ID As String = "ID"
            Public Const IDINCIDENCIA As String = "IDINCIDENCIA"
            Public Const IMAGEN As String = "IMAGEN"
            Public Const DESCRIPCION As String = "DESCRIPCION"
            Public Const EXTENSION As String = "EXTENSION"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(ID) = _GERTAKARIAK_IRUDIAK.PropertyNames.ID
                    ht(IDINCIDENCIA) = _GERTAKARIAK_IRUDIAK.PropertyNames.IDINCIDENCIA
                    ht(IMAGEN) = _GERTAKARIAK_IRUDIAK.PropertyNames.IMAGEN
                    ht(DESCRIPCION) = _GERTAKARIAK_IRUDIAK.PropertyNames.DESCRIPCION
                    ht(EXTENSION) = _GERTAKARIAK_IRUDIAK.PropertyNames.EXTENSION

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const ID As String = "ID"
            Public Const IDINCIDENCIA As String = "IDINCIDENCIA"
            Public Const IMAGEN As String = "IMAGEN"
            Public Const DESCRIPCION As String = "DESCRIPCION"
            Public Const EXTENSION As String = "EXTENSION"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(ID) = _GERTAKARIAK_IRUDIAK.ColumnNames.ID
                    ht(IDINCIDENCIA) = _GERTAKARIAK_IRUDIAK.ColumnNames.IDINCIDENCIA
                    ht(IMAGEN) = _GERTAKARIAK_IRUDIAK.ColumnNames.IMAGEN
                    ht(DESCRIPCION) = _GERTAKARIAK_IRUDIAK.ColumnNames.DESCRIPCION
                    ht(EXTENSION) = _GERTAKARIAK_IRUDIAK.ColumnNames.EXTENSION

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const ID As String = "s_ID"
            Public Const IDINCIDENCIA As String = "s_IDINCIDENCIA"
            Public Const IMAGEN As String = "s_IMAGEN"
            Public Const DESCRIPCION As String = "s_DESCRIPCION"
            Public Const EXTENSION As String = "s_EXTENSION"

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

        Public Overridable Property IDINCIDENCIA As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IDINCIDENCIA)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IDINCIDENCIA, Value)
            End Set
        End Property

        Public Overridable Property IMAGEN As Byte()
            Get
                Return MyBase.GetByteArray(ColumnNames.IMAGEN)
            End Get
            Set(ByVal Value As Byte())
                MyBase.SetByteArray(ColumnNames.IMAGEN, Value)
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

        Public Overridable Property EXTENSION As String
            Get
                Return MyBase.GetString(ColumnNames.EXTENSION)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.EXTENSION, Value)
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

        Public Overridable Property s_EXTENSION As String
            Get
                If Me.IsColumnNull(ColumnNames.EXTENSION) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.EXTENSION)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.EXTENSION)
                Else
                    Me.EXTENSION = MyBase.SetStringAsString(ColumnNames.EXTENSION, Value)
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

                Public ReadOnly Property IDINCIDENCIA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDINCIDENCIA, Parameters.IDINCIDENCIA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IMAGEN() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IMAGEN, Parameters.IMAGEN)
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

                Public ReadOnly Property EXTENSION() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.EXTENSION, Parameters.EXTENSION)
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

            Public ReadOnly Property IDINCIDENCIA() As WhereParameter
                Get
                    If _IDINCIDENCIA_W Is Nothing Then
                        _IDINCIDENCIA_W = TearOff.IDINCIDENCIA
                    End If
                    Return _IDINCIDENCIA_W
                End Get
            End Property

            Public ReadOnly Property IMAGEN() As WhereParameter
                Get
                    If _IMAGEN_W Is Nothing Then
                        _IMAGEN_W = TearOff.IMAGEN
                    End If
                    Return _IMAGEN_W
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

            Public ReadOnly Property EXTENSION() As WhereParameter
                Get
                    If _EXTENSION_W Is Nothing Then
                        _EXTENSION_W = TearOff.EXTENSION
                    End If
                    Return _EXTENSION_W
                End Get
            End Property

            Private _ID_W As WhereParameter = Nothing
            Private _IDINCIDENCIA_W As WhereParameter = Nothing
            Private _IMAGEN_W As WhereParameter = Nothing
            Private _DESCRIPCION_W As WhereParameter = Nothing
            Private _EXTENSION_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _ID_W = Nothing
                _IDINCIDENCIA_W = Nothing
                _IMAGEN_W = Nothing
                _DESCRIPCION_W = Nothing
                _EXTENSION_W = Nothing
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

                Public ReadOnly Property IDINCIDENCIA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDINCIDENCIA, Parameters.IDINCIDENCIA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IMAGEN() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IMAGEN, Parameters.IMAGEN)
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

                Public ReadOnly Property EXTENSION() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.EXTENSION, Parameters.EXTENSION)
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

            Public ReadOnly Property IDINCIDENCIA() As AggregateParameter
                Get
                    If _IDINCIDENCIA_W Is Nothing Then
                        _IDINCIDENCIA_W = TearOff.IDINCIDENCIA
                    End If
                    Return _IDINCIDENCIA_W
                End Get
            End Property

            Public ReadOnly Property IMAGEN() As AggregateParameter
                Get
                    If _IMAGEN_W Is Nothing Then
                        _IMAGEN_W = TearOff.IMAGEN
                    End If
                    Return _IMAGEN_W
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

            Public ReadOnly Property EXTENSION() As AggregateParameter
                Get
                    If _EXTENSION_W Is Nothing Then
                        _EXTENSION_W = TearOff.EXTENSION
                    End If
                    Return _EXTENSION_W
                End Get
            End Property

            Private _ID_W As AggregateParameter = Nothing
            Private _IDINCIDENCIA_W As AggregateParameter = Nothing
            Private _IMAGEN_W As AggregateParameter = Nothing
            Private _DESCRIPCION_W As AggregateParameter = Nothing
            Private _EXTENSION_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _ID_W = Nothing
                _IDINCIDENCIA_W = Nothing
                _IMAGEN_W = Nothing
                _DESCRIPCION_W = Nothing
                _EXTENSION_W = Nothing
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
            cmd.CommandText = Me.SchemaStoredProcedure + "PI_GERTAKARIAK_IRUDIAK"

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
            cmd.CommandText = Me.SchemaStoredProcedure + "PU_GERTAKARIAK_IRUDIAK"

            CreateParameters(cmd)

            Return cmd

        End Function

        Protected Overrides Function GetDeleteCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PD_GERTAKARIAK_IRUDIAK"

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.ID)
            p.SourceColumn = ColumnNames.ID
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDINCIDENCIA)
            p.SourceColumn = ColumnNames.IDINCIDENCIA
            p.SourceVersion = DataRowVersion.Current


            Return cmd

        End Function

        Private Sub CreateParameters(ByVal cmd As OracleCommand)

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.ID)
            p.SourceColumn = ColumnNames.ID
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDINCIDENCIA)
            p.SourceColumn = ColumnNames.IDINCIDENCIA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IMAGEN)
            p.SourceColumn = ColumnNames.IMAGEN
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.DESCRIPCION)
            p.SourceColumn = ColumnNames.DESCRIPCION
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.EXTENSION)
            p.SourceColumn = ColumnNames.EXTENSION
            p.SourceVersion = DataRowVersion.Current


        End Sub

    End Class

End Namespace

