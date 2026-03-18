
'===============================================================================
'BATZ, Koop. - 18/03/2010 10:45:58
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

    Public MustInherit Class _W_COSTES
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "INCIDENCIAS."
            Me.QuerySource = "W_COSTES"
            Me.MappingName = "W_COSTES"
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

            Public Shared ReadOnly Property ORIGEN As OracleParameter
                Get
                    Return New OracleParameter("ORIGEN", OracleDbType.Char, 1)
                End Get
            End Property

            Public Shared ReadOnly Property NUMORD As OracleParameter
                Get
                    Return New OracleParameter("NUMORD", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property NUMOPE As OracleParameter
                Get
                    Return New OracleParameter("NUMOPE", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property NUMMAR As OracleParameter
                Get
                    Return New OracleParameter("NUMMAR", OracleDbType.Char, 11)
                End Get
            End Property

            Public Shared ReadOnly Property CODPRO As OracleParameter
                Get
                    Return New OracleParameter("CODPRO", OracleDbType.Char, 12)
                End Get
            End Property

            Public Shared ReadOnly Property NUMPED As OracleParameter
                Get
                    Return New OracleParameter("NUMPED", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property NUMLIN As OracleParameter
                Get
                    Return New OracleParameter("NUMLIN", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property HORAS As OracleParameter
                Get
                    Return New OracleParameter("HORAS", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property IMPORTE As OracleParameter
                Get
                    Return New OracleParameter("IMPORTE", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property SECCIO As OracleParameter
                Get
                    Return New OracleParameter("SECCIO", OracleDbType.Varchar2, 4)
                End Get
            End Property

            Public Shared ReadOnly Property MAQUINA As OracleParameter
                Get
                    Return New OracleParameter("MAQUINA", OracleDbType.Varchar2, 10)
                End Get
            End Property

            Public Shared ReadOnly Property FASE As OracleParameter
                Get
                    Return New OracleParameter("FASE", OracleDbType.Varchar2, 4)
                End Get
            End Property

            Public Shared ReadOnly Property PROCESO As OracleParameter
                Get
                    Return New OracleParameter("PROCESO", OracleDbType.Varchar2, 4)
                End Get
            End Property

            Public Shared ReadOnly Property CANPED As OracleParameter
                Get
                    Return New OracleParameter("CANPED", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property CODART As OracleParameter
                Get
                    Return New OracleParameter("CODART", OracleDbType.Char, 12)
                End Get
            End Property

            Public Shared ReadOnly Property FECHA As OracleParameter
                Get
                    Return New OracleParameter("FECHA", OracleDbType.Date, 0)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const ORIGEN As String = "ORIGEN"
            Public Const NUMORD As String = "NUMORD"
            Public Const NUMOPE As String = "NUMOPE"
            Public Const NUMMAR As String = "NUMMAR"
            Public Const CODPRO As String = "CODPRO"
            Public Const NUMPED As String = "NUMPED"
            Public Const NUMLIN As String = "NUMLIN"
            Public Const HORAS As String = "HORAS"
            Public Const IMPORTE As String = "IMPORTE"
            Public Const SECCIO As String = "SECCIO"
            Public Const MAQUINA As String = "MAQUINA"
            Public Const FASE As String = "FASE"
            Public Const PROCESO As String = "PROCESO"
            Public Const CANPED As String = "CANPED"
            Public Const CODART As String = "CODART"
            Public Const FECHA As String = "FECHA"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(ORIGEN) = _W_COSTES.PropertyNames.ORIGEN
                    ht(NUMORD) = _W_COSTES.PropertyNames.NUMORD
                    ht(NUMOPE) = _W_COSTES.PropertyNames.NUMOPE
                    ht(NUMMAR) = _W_COSTES.PropertyNames.NUMMAR
                    ht(CODPRO) = _W_COSTES.PropertyNames.CODPRO
                    ht(NUMPED) = _W_COSTES.PropertyNames.NUMPED
                    ht(NUMLIN) = _W_COSTES.PropertyNames.NUMLIN
                    ht(HORAS) = _W_COSTES.PropertyNames.HORAS
                    ht(IMPORTE) = _W_COSTES.PropertyNames.IMPORTE
                    ht(SECCIO) = _W_COSTES.PropertyNames.SECCIO
                    ht(MAQUINA) = _W_COSTES.PropertyNames.MAQUINA
                    ht(FASE) = _W_COSTES.PropertyNames.FASE
                    ht(PROCESO) = _W_COSTES.PropertyNames.PROCESO
                    ht(CANPED) = _W_COSTES.PropertyNames.CANPED
                    ht(CODART) = _W_COSTES.PropertyNames.CODART
                    ht(FECHA) = _W_COSTES.PropertyNames.FECHA

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const ORIGEN As String = "ORIGEN"
            Public Const NUMORD As String = "NUMORD"
            Public Const NUMOPE As String = "NUMOPE"
            Public Const NUMMAR As String = "NUMMAR"
            Public Const CODPRO As String = "CODPRO"
            Public Const NUMPED As String = "NUMPED"
            Public Const NUMLIN As String = "NUMLIN"
            Public Const HORAS As String = "HORAS"
            Public Const IMPORTE As String = "IMPORTE"
            Public Const SECCIO As String = "SECCIO"
            Public Const MAQUINA As String = "MAQUINA"
            Public Const FASE As String = "FASE"
            Public Const PROCESO As String = "PROCESO"
            Public Const CANPED As String = "CANPED"
            Public Const CODART As String = "CODART"
            Public Const FECHA As String = "FECHA"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(ORIGEN) = _W_COSTES.ColumnNames.ORIGEN
                    ht(NUMORD) = _W_COSTES.ColumnNames.NUMORD
                    ht(NUMOPE) = _W_COSTES.ColumnNames.NUMOPE
                    ht(NUMMAR) = _W_COSTES.ColumnNames.NUMMAR
                    ht(CODPRO) = _W_COSTES.ColumnNames.CODPRO
                    ht(NUMPED) = _W_COSTES.ColumnNames.NUMPED
                    ht(NUMLIN) = _W_COSTES.ColumnNames.NUMLIN
                    ht(HORAS) = _W_COSTES.ColumnNames.HORAS
                    ht(IMPORTE) = _W_COSTES.ColumnNames.IMPORTE
                    ht(SECCIO) = _W_COSTES.ColumnNames.SECCIO
                    ht(MAQUINA) = _W_COSTES.ColumnNames.MAQUINA
                    ht(FASE) = _W_COSTES.ColumnNames.FASE
                    ht(PROCESO) = _W_COSTES.ColumnNames.PROCESO
                    ht(CANPED) = _W_COSTES.ColumnNames.CANPED
                    ht(CODART) = _W_COSTES.ColumnNames.CODART
                    ht(FECHA) = _W_COSTES.ColumnNames.FECHA

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const ORIGEN As String = "s_ORIGEN"
            Public Const NUMORD As String = "s_NUMORD"
            Public Const NUMOPE As String = "s_NUMOPE"
            Public Const NUMMAR As String = "s_NUMMAR"
            Public Const CODPRO As String = "s_CODPRO"
            Public Const NUMPED As String = "s_NUMPED"
            Public Const NUMLIN As String = "s_NUMLIN"
            Public Const HORAS As String = "s_HORAS"
            Public Const IMPORTE As String = "s_IMPORTE"
            Public Const SECCIO As String = "s_SECCIO"
            Public Const MAQUINA As String = "s_MAQUINA"
            Public Const FASE As String = "s_FASE"
            Public Const PROCESO As String = "s_PROCESO"
            Public Const CANPED As String = "s_CANPED"
            Public Const CODART As String = "s_CODART"
            Public Const FECHA As String = "s_FECHA"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property ORIGEN As String
            Get
                Return MyBase.GetString(ColumnNames.ORIGEN)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.ORIGEN, Value)
            End Set
        End Property

        Public Overridable Property NUMORD As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.NUMORD)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.NUMORD, Value)
            End Set
        End Property

        Public Overridable Property NUMOPE As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.NUMOPE)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.NUMOPE, Value)
            End Set
        End Property

        Public Overridable Property NUMMAR As String
            Get
                Return MyBase.GetString(ColumnNames.NUMMAR)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.NUMMAR, Value)
            End Set
        End Property

        Public Overridable Property CODPRO As String
            Get
                Return MyBase.GetString(ColumnNames.CODPRO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CODPRO, Value)
            End Set
        End Property

        Public Overridable Property NUMPED As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.NUMPED)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.NUMPED, Value)
            End Set
        End Property

        Public Overridable Property NUMLIN As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.NUMLIN)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.NUMLIN, Value)
            End Set
        End Property

        Public Overridable Property HORAS As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.HORAS)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.HORAS, Value)
            End Set
        End Property

        Public Overridable Property IMPORTE As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IMPORTE)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IMPORTE, Value)
            End Set
        End Property

        Public Overridable Property SECCIO As String
            Get
                Return MyBase.GetString(ColumnNames.SECCIO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.SECCIO, Value)
            End Set
        End Property

        Public Overridable Property MAQUINA As String
            Get
                Return MyBase.GetString(ColumnNames.MAQUINA)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.MAQUINA, Value)
            End Set
        End Property

        Public Overridable Property FASE As String
            Get
                Return MyBase.GetString(ColumnNames.FASE)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.FASE, Value)
            End Set
        End Property

        Public Overridable Property PROCESO As String
            Get
                Return MyBase.GetString(ColumnNames.PROCESO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.PROCESO, Value)
            End Set
        End Property

        Public Overridable Property CANPED As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.CANPED)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.CANPED, Value)
            End Set
        End Property

        Public Overridable Property CODART As String
            Get
                Return MyBase.GetString(ColumnNames.CODART)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CODART, Value)
            End Set
        End Property

        Public Overridable Property FECHA As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECHA)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECHA, Value)
            End Set
        End Property


#End Region

#Region "String Properties"

        Public Overridable Property s_ORIGEN As String
            Get
                If Me.IsColumnNull(ColumnNames.ORIGEN) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.ORIGEN)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.ORIGEN)
                Else
                    Me.ORIGEN = MyBase.SetStringAsString(ColumnNames.ORIGEN, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_NUMORD As String
            Get
                If Me.IsColumnNull(ColumnNames.NUMORD) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.NUMORD)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NUMORD)
                Else
                    Me.NUMORD = MyBase.SetDecimalAsString(ColumnNames.NUMORD, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_NUMOPE As String
            Get
                If Me.IsColumnNull(ColumnNames.NUMOPE) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.NUMOPE)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NUMOPE)
                Else
                    Me.NUMOPE = MyBase.SetDecimalAsString(ColumnNames.NUMOPE, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_NUMMAR As String
            Get
                If Me.IsColumnNull(ColumnNames.NUMMAR) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.NUMMAR)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NUMMAR)
                Else
                    Me.NUMMAR = MyBase.SetStringAsString(ColumnNames.NUMMAR, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CODPRO As String
            Get
                If Me.IsColumnNull(ColumnNames.CODPRO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CODPRO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CODPRO)
                Else
                    Me.CODPRO = MyBase.SetStringAsString(ColumnNames.CODPRO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_NUMPED As String
            Get
                If Me.IsColumnNull(ColumnNames.NUMPED) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.NUMPED)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NUMPED)
                Else
                    Me.NUMPED = MyBase.SetDecimalAsString(ColumnNames.NUMPED, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_NUMLIN As String
            Get
                If Me.IsColumnNull(ColumnNames.NUMLIN) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.NUMLIN)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NUMLIN)
                Else
                    Me.NUMLIN = MyBase.SetDecimalAsString(ColumnNames.NUMLIN, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_HORAS As String
            Get
                If Me.IsColumnNull(ColumnNames.HORAS) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.HORAS)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.HORAS)
                Else
                    Me.HORAS = MyBase.SetDecimalAsString(ColumnNames.HORAS, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_IMPORTE As String
            Get
                If Me.IsColumnNull(ColumnNames.IMPORTE) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.IMPORTE)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IMPORTE)
                Else
                    Me.IMPORTE = MyBase.SetDecimalAsString(ColumnNames.IMPORTE, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_SECCIO As String
            Get
                If Me.IsColumnNull(ColumnNames.SECCIO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.SECCIO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.SECCIO)
                Else
                    Me.SECCIO = MyBase.SetStringAsString(ColumnNames.SECCIO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_MAQUINA As String
            Get
                If Me.IsColumnNull(ColumnNames.MAQUINA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.MAQUINA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.MAQUINA)
                Else
                    Me.MAQUINA = MyBase.SetStringAsString(ColumnNames.MAQUINA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FASE As String
            Get
                If Me.IsColumnNull(ColumnNames.FASE) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.FASE)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FASE)
                Else
                    Me.FASE = MyBase.SetStringAsString(ColumnNames.FASE, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_PROCESO As String
            Get
                If Me.IsColumnNull(ColumnNames.PROCESO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.PROCESO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.PROCESO)
                Else
                    Me.PROCESO = MyBase.SetStringAsString(ColumnNames.PROCESO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CANPED As String
            Get
                If Me.IsColumnNull(ColumnNames.CANPED) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.CANPED)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CANPED)
                Else
                    Me.CANPED = MyBase.SetDecimalAsString(ColumnNames.CANPED, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CODART As String
            Get
                If Me.IsColumnNull(ColumnNames.CODART) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CODART)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CODART)
                Else
                    Me.CODART = MyBase.SetStringAsString(ColumnNames.CODART, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FECHA As String
            Get
                If Me.IsColumnNull(ColumnNames.FECHA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FECHA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FECHA)
                Else
                    Me.FECHA = MyBase.SetDateTimeAsString(ColumnNames.FECHA, Value)
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


                Public ReadOnly Property ORIGEN() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.ORIGEN, Parameters.ORIGEN)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMORD() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMORD, Parameters.NUMORD)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMOPE() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMOPE, Parameters.NUMOPE)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMMAR() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMMAR, Parameters.NUMMAR)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODPRO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CODPRO, Parameters.CODPRO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMPED() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMPED, Parameters.NUMPED)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMLIN() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMLIN, Parameters.NUMLIN)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property HORAS() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.HORAS, Parameters.HORAS)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IMPORTE() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IMPORTE, Parameters.IMPORTE)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property SECCIO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.SECCIO, Parameters.SECCIO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property MAQUINA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.MAQUINA, Parameters.MAQUINA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FASE() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FASE, Parameters.FASE)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PROCESO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.PROCESO, Parameters.PROCESO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CANPED() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CANPED, Parameters.CANPED)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODART() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CODART, Parameters.CODART)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECHA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FECHA, Parameters.FECHA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property


                Private _clause As WhereClause
            End Class
#End Region

            Public ReadOnly Property ORIGEN() As WhereParameter
                Get
                    If _ORIGEN_W Is Nothing Then
                        _ORIGEN_W = TearOff.ORIGEN
                    End If
                    Return _ORIGEN_W
                End Get
            End Property

            Public ReadOnly Property NUMORD() As WhereParameter
                Get
                    If _NUMORD_W Is Nothing Then
                        _NUMORD_W = TearOff.NUMORD
                    End If
                    Return _NUMORD_W
                End Get
            End Property

            Public ReadOnly Property NUMOPE() As WhereParameter
                Get
                    If _NUMOPE_W Is Nothing Then
                        _NUMOPE_W = TearOff.NUMOPE
                    End If
                    Return _NUMOPE_W
                End Get
            End Property

            Public ReadOnly Property NUMMAR() As WhereParameter
                Get
                    If _NUMMAR_W Is Nothing Then
                        _NUMMAR_W = TearOff.NUMMAR
                    End If
                    Return _NUMMAR_W
                End Get
            End Property

            Public ReadOnly Property CODPRO() As WhereParameter
                Get
                    If _CODPRO_W Is Nothing Then
                        _CODPRO_W = TearOff.CODPRO
                    End If
                    Return _CODPRO_W
                End Get
            End Property

            Public ReadOnly Property NUMPED() As WhereParameter
                Get
                    If _NUMPED_W Is Nothing Then
                        _NUMPED_W = TearOff.NUMPED
                    End If
                    Return _NUMPED_W
                End Get
            End Property

            Public ReadOnly Property NUMLIN() As WhereParameter
                Get
                    If _NUMLIN_W Is Nothing Then
                        _NUMLIN_W = TearOff.NUMLIN
                    End If
                    Return _NUMLIN_W
                End Get
            End Property

            Public ReadOnly Property HORAS() As WhereParameter
                Get
                    If _HORAS_W Is Nothing Then
                        _HORAS_W = TearOff.HORAS
                    End If
                    Return _HORAS_W
                End Get
            End Property

            Public ReadOnly Property IMPORTE() As WhereParameter
                Get
                    If _IMPORTE_W Is Nothing Then
                        _IMPORTE_W = TearOff.IMPORTE
                    End If
                    Return _IMPORTE_W
                End Get
            End Property

            Public ReadOnly Property SECCIO() As WhereParameter
                Get
                    If _SECCIO_W Is Nothing Then
                        _SECCIO_W = TearOff.SECCIO
                    End If
                    Return _SECCIO_W
                End Get
            End Property

            Public ReadOnly Property MAQUINA() As WhereParameter
                Get
                    If _MAQUINA_W Is Nothing Then
                        _MAQUINA_W = TearOff.MAQUINA
                    End If
                    Return _MAQUINA_W
                End Get
            End Property

            Public ReadOnly Property FASE() As WhereParameter
                Get
                    If _FASE_W Is Nothing Then
                        _FASE_W = TearOff.FASE
                    End If
                    Return _FASE_W
                End Get
            End Property

            Public ReadOnly Property PROCESO() As WhereParameter
                Get
                    If _PROCESO_W Is Nothing Then
                        _PROCESO_W = TearOff.PROCESO
                    End If
                    Return _PROCESO_W
                End Get
            End Property

            Public ReadOnly Property CANPED() As WhereParameter
                Get
                    If _CANPED_W Is Nothing Then
                        _CANPED_W = TearOff.CANPED
                    End If
                    Return _CANPED_W
                End Get
            End Property

            Public ReadOnly Property CODART() As WhereParameter
                Get
                    If _CODART_W Is Nothing Then
                        _CODART_W = TearOff.CODART
                    End If
                    Return _CODART_W
                End Get
            End Property

            Public ReadOnly Property FECHA() As WhereParameter
                Get
                    If _FECHA_W Is Nothing Then
                        _FECHA_W = TearOff.FECHA
                    End If
                    Return _FECHA_W
                End Get
            End Property

            Private _ORIGEN_W As WhereParameter = Nothing
            Private _NUMORD_W As WhereParameter = Nothing
            Private _NUMOPE_W As WhereParameter = Nothing
            Private _NUMMAR_W As WhereParameter = Nothing
            Private _CODPRO_W As WhereParameter = Nothing
            Private _NUMPED_W As WhereParameter = Nothing
            Private _NUMLIN_W As WhereParameter = Nothing
            Private _HORAS_W As WhereParameter = Nothing
            Private _IMPORTE_W As WhereParameter = Nothing
            Private _SECCIO_W As WhereParameter = Nothing
            Private _MAQUINA_W As WhereParameter = Nothing
            Private _FASE_W As WhereParameter = Nothing
            Private _PROCESO_W As WhereParameter = Nothing
            Private _CANPED_W As WhereParameter = Nothing
            Private _CODART_W As WhereParameter = Nothing
            Private _FECHA_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _ORIGEN_W = Nothing
                _NUMORD_W = Nothing
                _NUMOPE_W = Nothing
                _NUMMAR_W = Nothing
                _CODPRO_W = Nothing
                _NUMPED_W = Nothing
                _NUMLIN_W = Nothing
                _HORAS_W = Nothing
                _IMPORTE_W = Nothing
                _SECCIO_W = Nothing
                _MAQUINA_W = Nothing
                _FASE_W = Nothing
                _PROCESO_W = Nothing
                _CANPED_W = Nothing
                _CODART_W = Nothing
                _FECHA_W = Nothing
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


                Public ReadOnly Property ORIGEN() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ORIGEN, Parameters.ORIGEN)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMORD() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMORD, Parameters.NUMORD)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMOPE() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMOPE, Parameters.NUMOPE)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMMAR() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMMAR, Parameters.NUMMAR)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODPRO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CODPRO, Parameters.CODPRO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMPED() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMPED, Parameters.NUMPED)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMLIN() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMLIN, Parameters.NUMLIN)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property HORAS() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.HORAS, Parameters.HORAS)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IMPORTE() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IMPORTE, Parameters.IMPORTE)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property SECCIO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.SECCIO, Parameters.SECCIO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property MAQUINA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.MAQUINA, Parameters.MAQUINA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FASE() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FASE, Parameters.FASE)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PROCESO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.PROCESO, Parameters.PROCESO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CANPED() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CANPED, Parameters.CANPED)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODART() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CODART, Parameters.CODART)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECHA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECHA, Parameters.FECHA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property


                Private _clause As AggregateClause
            End Class
#End Region

            Public ReadOnly Property ORIGEN() As AggregateParameter
                Get
                    If _ORIGEN_W Is Nothing Then
                        _ORIGEN_W = TearOff.ORIGEN
                    End If
                    Return _ORIGEN_W
                End Get
            End Property

            Public ReadOnly Property NUMORD() As AggregateParameter
                Get
                    If _NUMORD_W Is Nothing Then
                        _NUMORD_W = TearOff.NUMORD
                    End If
                    Return _NUMORD_W
                End Get
            End Property

            Public ReadOnly Property NUMOPE() As AggregateParameter
                Get
                    If _NUMOPE_W Is Nothing Then
                        _NUMOPE_W = TearOff.NUMOPE
                    End If
                    Return _NUMOPE_W
                End Get
            End Property

            Public ReadOnly Property NUMMAR() As AggregateParameter
                Get
                    If _NUMMAR_W Is Nothing Then
                        _NUMMAR_W = TearOff.NUMMAR
                    End If
                    Return _NUMMAR_W
                End Get
            End Property

            Public ReadOnly Property CODPRO() As AggregateParameter
                Get
                    If _CODPRO_W Is Nothing Then
                        _CODPRO_W = TearOff.CODPRO
                    End If
                    Return _CODPRO_W
                End Get
            End Property

            Public ReadOnly Property NUMPED() As AggregateParameter
                Get
                    If _NUMPED_W Is Nothing Then
                        _NUMPED_W = TearOff.NUMPED
                    End If
                    Return _NUMPED_W
                End Get
            End Property

            Public ReadOnly Property NUMLIN() As AggregateParameter
                Get
                    If _NUMLIN_W Is Nothing Then
                        _NUMLIN_W = TearOff.NUMLIN
                    End If
                    Return _NUMLIN_W
                End Get
            End Property

            Public ReadOnly Property HORAS() As AggregateParameter
                Get
                    If _HORAS_W Is Nothing Then
                        _HORAS_W = TearOff.HORAS
                    End If
                    Return _HORAS_W
                End Get
            End Property

            Public ReadOnly Property IMPORTE() As AggregateParameter
                Get
                    If _IMPORTE_W Is Nothing Then
                        _IMPORTE_W = TearOff.IMPORTE
                    End If
                    Return _IMPORTE_W
                End Get
            End Property

            Public ReadOnly Property SECCIO() As AggregateParameter
                Get
                    If _SECCIO_W Is Nothing Then
                        _SECCIO_W = TearOff.SECCIO
                    End If
                    Return _SECCIO_W
                End Get
            End Property

            Public ReadOnly Property MAQUINA() As AggregateParameter
                Get
                    If _MAQUINA_W Is Nothing Then
                        _MAQUINA_W = TearOff.MAQUINA
                    End If
                    Return _MAQUINA_W
                End Get
            End Property

            Public ReadOnly Property FASE() As AggregateParameter
                Get
                    If _FASE_W Is Nothing Then
                        _FASE_W = TearOff.FASE
                    End If
                    Return _FASE_W
                End Get
            End Property

            Public ReadOnly Property PROCESO() As AggregateParameter
                Get
                    If _PROCESO_W Is Nothing Then
                        _PROCESO_W = TearOff.PROCESO
                    End If
                    Return _PROCESO_W
                End Get
            End Property

            Public ReadOnly Property CANPED() As AggregateParameter
                Get
                    If _CANPED_W Is Nothing Then
                        _CANPED_W = TearOff.CANPED
                    End If
                    Return _CANPED_W
                End Get
            End Property

            Public ReadOnly Property CODART() As AggregateParameter
                Get
                    If _CODART_W Is Nothing Then
                        _CODART_W = TearOff.CODART
                    End If
                    Return _CODART_W
                End Get
            End Property

            Public ReadOnly Property FECHA() As AggregateParameter
                Get
                    If _FECHA_W Is Nothing Then
                        _FECHA_W = TearOff.FECHA
                    End If
                    Return _FECHA_W
                End Get
            End Property

            Private _ORIGEN_W As AggregateParameter = Nothing
            Private _NUMORD_W As AggregateParameter = Nothing
            Private _NUMOPE_W As AggregateParameter = Nothing
            Private _NUMMAR_W As AggregateParameter = Nothing
            Private _CODPRO_W As AggregateParameter = Nothing
            Private _NUMPED_W As AggregateParameter = Nothing
            Private _NUMLIN_W As AggregateParameter = Nothing
            Private _HORAS_W As AggregateParameter = Nothing
            Private _IMPORTE_W As AggregateParameter = Nothing
            Private _SECCIO_W As AggregateParameter = Nothing
            Private _MAQUINA_W As AggregateParameter = Nothing
            Private _FASE_W As AggregateParameter = Nothing
            Private _PROCESO_W As AggregateParameter = Nothing
            Private _CANPED_W As AggregateParameter = Nothing
            Private _CODART_W As AggregateParameter = Nothing
            Private _FECHA_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _ORIGEN_W = Nothing
                _NUMORD_W = Nothing
                _NUMOPE_W = Nothing
                _NUMMAR_W = Nothing
                _CODPRO_W = Nothing
                _NUMPED_W = Nothing
                _NUMLIN_W = Nothing
                _HORAS_W = Nothing
                _IMPORTE_W = Nothing
                _SECCIO_W = Nothing
                _MAQUINA_W = Nothing
                _FASE_W = Nothing
                _PROCESO_W = Nothing
                _CANPED_W = Nothing
                _CODART_W = Nothing
                _FECHA_W = Nothing
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

