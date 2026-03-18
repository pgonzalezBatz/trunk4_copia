
'===============================================================================
'BATZ, Koop. - 09/02/2009 8:55:52
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

    Public MustInherit Class _GCALBARA
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "XBAT."
            Me.QuerySource = "GCALBARA"
            Me.MappingName = "GCALBARA"
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


            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_GCALBARA", parameters)
        End Function

        '=================================================================
        ' Public Overridable Function LoadByPrimaryKey()  As Boolean
        '=================================================================
        '  Loads a single row of via the primary key
        '=================================================================
        Public Overridable Function LoadByPrimaryKey(ByVal ANNO As Decimal, ByVal CODPROV As String, ByVal NUMALBAR As Decimal, ByVal NUMLIN As Decimal, ByVal NUMPED As Decimal, ByVal TIPO As String) As Boolean

            Dim parameters As ListDictionary = New ListDictionary()
            parameters.Add(_GCALBARA.Parameters.ANNO, ANNO)

            parameters.Add(_GCALBARA.Parameters.CODPROV, CODPROV)

            parameters.Add(_GCALBARA.Parameters.NUMALBAR, NUMALBAR)

            parameters.Add(_GCALBARA.Parameters.NUMLIN, NUMLIN)

            parameters.Add(_GCALBARA.Parameters.NUMPED, NUMPED)

            parameters.Add(_GCALBARA.Parameters.TIPO, TIPO)


            Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
            p.Direction = ParameterDirection.Output
            parameters.Add(p, Nothing)

            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_GCALBARA", parameters)

        End Function

#Region "Parameters"
        Protected Class Parameters

            Public Shared ReadOnly Property TIPO As OracleParameter
                Get
                    Return New OracleParameter("p_TIPO", OracleDbType.Char, 1)
                End Get
            End Property

            Public Shared ReadOnly Property CODPROV As OracleParameter
                Get
                    Return New OracleParameter("p_CODPROV", OracleDbType.Char, 12)
                End Get
            End Property

            Public Shared ReadOnly Property ANNO As OracleParameter
                Get
                    Return New OracleParameter("p_ANNO", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property NUMALBAR As OracleParameter
                Get
                    Return New OracleParameter("p_NUMALBAR", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property NUMPED As OracleParameter
                Get
                    Return New OracleParameter("p_NUMPED", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property NUMLIN As OracleParameter
                Get
                    Return New OracleParameter("p_NUMLIN", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property CANREC As OracleParameter
                Get
                    Return New OracleParameter("p_CANREC", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property CORTES As OracleParameter
                Get
                    Return New OracleParameter("p_CORTES", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property PORRAT As OracleParameter
                Get
                    Return New OracleParameter("p_PORRAT", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property IMPORTE As OracleParameter
                Get
                    Return New OracleParameter("p_IMPORTE", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property NUMMOV_E As OracleParameter
                Get
                    Return New OracleParameter("p_NUMMOV_E", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property NUMMOV_S As OracleParameter
                Get
                    Return New OracleParameter("p_NUMMOV_S", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property ECORTES As OracleParameter
                Get
                    Return New OracleParameter("p_ECORTES", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property EPORRAT As OracleParameter
                Get
                    Return New OracleParameter("p_EPORRAT", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property EIMPORTE As OracleParameter
                Get
                    Return New OracleParameter("p_EIMPORTE", OracleDbType.Decimal, 0)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const TIPO As String = "TIPO"
            Public Const CODPROV As String = "CODPROV"
            Public Const ANNO As String = "ANNO"
            Public Const NUMALBAR As String = "NUMALBAR"
            Public Const NUMPED As String = "NUMPED"
            Public Const NUMLIN As String = "NUMLIN"
            Public Const CANREC As String = "CANREC"
            Public Const CORTES As String = "CORTES"
            Public Const PORRAT As String = "PORRAT"
            Public Const IMPORTE As String = "IMPORTE"
            Public Const NUMMOV_E As String = "NUMMOV_E"
            Public Const NUMMOV_S As String = "NUMMOV_S"
            Public Const ECORTES As String = "ECORTES"
            Public Const EPORRAT As String = "EPORRAT"
            Public Const EIMPORTE As String = "EIMPORTE"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(TIPO) = _GCALBARA.PropertyNames.TIPO
                    ht(CODPROV) = _GCALBARA.PropertyNames.CODPROV
                    ht(ANNO) = _GCALBARA.PropertyNames.ANNO
                    ht(NUMALBAR) = _GCALBARA.PropertyNames.NUMALBAR
                    ht(NUMPED) = _GCALBARA.PropertyNames.NUMPED
                    ht(NUMLIN) = _GCALBARA.PropertyNames.NUMLIN
                    ht(CANREC) = _GCALBARA.PropertyNames.CANREC
                    ht(CORTES) = _GCALBARA.PropertyNames.CORTES
                    ht(PORRAT) = _GCALBARA.PropertyNames.PORRAT
                    ht(IMPORTE) = _GCALBARA.PropertyNames.IMPORTE
                    ht(NUMMOV_E) = _GCALBARA.PropertyNames.NUMMOV_E
                    ht(NUMMOV_S) = _GCALBARA.PropertyNames.NUMMOV_S
                    ht(ECORTES) = _GCALBARA.PropertyNames.ECORTES
                    ht(EPORRAT) = _GCALBARA.PropertyNames.EPORRAT
                    ht(EIMPORTE) = _GCALBARA.PropertyNames.EIMPORTE

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const TIPO As String = "TIPO"
            Public Const CODPROV As String = "CODPROV"
            Public Const ANNO As String = "ANNO"
            Public Const NUMALBAR As String = "NUMALBAR"
            Public Const NUMPED As String = "NUMPED"
            Public Const NUMLIN As String = "NUMLIN"
            Public Const CANREC As String = "CANREC"
            Public Const CORTES As String = "CORTES"
            Public Const PORRAT As String = "PORRAT"
            Public Const IMPORTE As String = "IMPORTE"
            Public Const NUMMOV_E As String = "NUMMOV_E"
            Public Const NUMMOV_S As String = "NUMMOV_S"
            Public Const ECORTES As String = "ECORTES"
            Public Const EPORRAT As String = "EPORRAT"
            Public Const EIMPORTE As String = "EIMPORTE"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(TIPO) = _GCALBARA.ColumnNames.TIPO
                    ht(CODPROV) = _GCALBARA.ColumnNames.CODPROV
                    ht(ANNO) = _GCALBARA.ColumnNames.ANNO
                    ht(NUMALBAR) = _GCALBARA.ColumnNames.NUMALBAR
                    ht(NUMPED) = _GCALBARA.ColumnNames.NUMPED
                    ht(NUMLIN) = _GCALBARA.ColumnNames.NUMLIN
                    ht(CANREC) = _GCALBARA.ColumnNames.CANREC
                    ht(CORTES) = _GCALBARA.ColumnNames.CORTES
                    ht(PORRAT) = _GCALBARA.ColumnNames.PORRAT
                    ht(IMPORTE) = _GCALBARA.ColumnNames.IMPORTE
                    ht(NUMMOV_E) = _GCALBARA.ColumnNames.NUMMOV_E
                    ht(NUMMOV_S) = _GCALBARA.ColumnNames.NUMMOV_S
                    ht(ECORTES) = _GCALBARA.ColumnNames.ECORTES
                    ht(EPORRAT) = _GCALBARA.ColumnNames.EPORRAT
                    ht(EIMPORTE) = _GCALBARA.ColumnNames.EIMPORTE

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const TIPO As String = "s_TIPO"
            Public Const CODPROV As String = "s_CODPROV"
            Public Const ANNO As String = "s_ANNO"
            Public Const NUMALBAR As String = "s_NUMALBAR"
            Public Const NUMPED As String = "s_NUMPED"
            Public Const NUMLIN As String = "s_NUMLIN"
            Public Const CANREC As String = "s_CANREC"
            Public Const CORTES As String = "s_CORTES"
            Public Const PORRAT As String = "s_PORRAT"
            Public Const IMPORTE As String = "s_IMPORTE"
            Public Const NUMMOV_E As String = "s_NUMMOV_E"
            Public Const NUMMOV_S As String = "s_NUMMOV_S"
            Public Const ECORTES As String = "s_ECORTES"
            Public Const EPORRAT As String = "s_EPORRAT"
            Public Const EIMPORTE As String = "s_EIMPORTE"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property TIPO As String
            Get
                Return MyBase.GetString(ColumnNames.TIPO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.TIPO, Value)
            End Set
        End Property

        Public Overridable Property CODPROV As String
            Get
                Return MyBase.GetString(ColumnNames.CODPROV)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CODPROV, Value)
            End Set
        End Property

        Public Overridable Property ANNO As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.ANNO)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.ANNO, Value)
            End Set
        End Property

        Public Overridable Property NUMALBAR As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.NUMALBAR)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.NUMALBAR, Value)
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

        Public Overridable Property CANREC As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.CANREC)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.CANREC, Value)
            End Set
        End Property

        Public Overridable Property CORTES As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.CORTES)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.CORTES, Value)
            End Set
        End Property

        Public Overridable Property PORRAT As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.PORRAT)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.PORRAT, Value)
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

        Public Overridable Property NUMMOV_E As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.NUMMOV_E)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.NUMMOV_E, Value)
            End Set
        End Property

        Public Overridable Property NUMMOV_S As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.NUMMOV_S)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.NUMMOV_S, Value)
            End Set
        End Property

        Public Overridable Property ECORTES As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.ECORTES)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.ECORTES, Value)
            End Set
        End Property

        Public Overridable Property EPORRAT As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.EPORRAT)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.EPORRAT, Value)
            End Set
        End Property

        Public Overridable Property EIMPORTE As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.EIMPORTE)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.EIMPORTE, Value)
            End Set
        End Property


#End Region

#Region "String Properties"

        Public Overridable Property s_TIPO As String
            Get
                If Me.IsColumnNull(ColumnNames.TIPO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.TIPO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.TIPO)
                Else
                    Me.TIPO = MyBase.SetStringAsString(ColumnNames.TIPO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CODPROV As String
            Get
                If Me.IsColumnNull(ColumnNames.CODPROV) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CODPROV)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CODPROV)
                Else
                    Me.CODPROV = MyBase.SetStringAsString(ColumnNames.CODPROV, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_ANNO As String
            Get
                If Me.IsColumnNull(ColumnNames.ANNO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.ANNO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.ANNO)
                Else
                    Me.ANNO = MyBase.SetDecimalAsString(ColumnNames.ANNO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_NUMALBAR As String
            Get
                If Me.IsColumnNull(ColumnNames.NUMALBAR) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.NUMALBAR)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NUMALBAR)
                Else
                    Me.NUMALBAR = MyBase.SetDecimalAsString(ColumnNames.NUMALBAR, Value)
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

        Public Overridable Property s_CANREC As String
            Get
                If Me.IsColumnNull(ColumnNames.CANREC) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.CANREC)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CANREC)
                Else
                    Me.CANREC = MyBase.SetDecimalAsString(ColumnNames.CANREC, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CORTES As String
            Get
                If Me.IsColumnNull(ColumnNames.CORTES) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.CORTES)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CORTES)
                Else
                    Me.CORTES = MyBase.SetDecimalAsString(ColumnNames.CORTES, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_PORRAT As String
            Get
                If Me.IsColumnNull(ColumnNames.PORRAT) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.PORRAT)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.PORRAT)
                Else
                    Me.PORRAT = MyBase.SetDecimalAsString(ColumnNames.PORRAT, Value)
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

        Public Overridable Property s_NUMMOV_E As String
            Get
                If Me.IsColumnNull(ColumnNames.NUMMOV_E) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.NUMMOV_E)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NUMMOV_E)
                Else
                    Me.NUMMOV_E = MyBase.SetDecimalAsString(ColumnNames.NUMMOV_E, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_NUMMOV_S As String
            Get
                If Me.IsColumnNull(ColumnNames.NUMMOV_S) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.NUMMOV_S)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NUMMOV_S)
                Else
                    Me.NUMMOV_S = MyBase.SetDecimalAsString(ColumnNames.NUMMOV_S, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_ECORTES As String
            Get
                If Me.IsColumnNull(ColumnNames.ECORTES) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.ECORTES)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.ECORTES)
                Else
                    Me.ECORTES = MyBase.SetDecimalAsString(ColumnNames.ECORTES, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_EPORRAT As String
            Get
                If Me.IsColumnNull(ColumnNames.EPORRAT) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.EPORRAT)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.EPORRAT)
                Else
                    Me.EPORRAT = MyBase.SetDecimalAsString(ColumnNames.EPORRAT, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_EIMPORTE As String
            Get
                If Me.IsColumnNull(ColumnNames.EIMPORTE) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.EIMPORTE)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.EIMPORTE)
                Else
                    Me.EIMPORTE = MyBase.SetDecimalAsString(ColumnNames.EIMPORTE, Value)
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


                Public ReadOnly Property TIPO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.TIPO, Parameters.TIPO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODPROV() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CODPROV, Parameters.CODPROV)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ANNO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.ANNO, Parameters.ANNO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMALBAR() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMALBAR, Parameters.NUMALBAR)
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

                Public ReadOnly Property CANREC() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CANREC, Parameters.CANREC)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CORTES() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CORTES, Parameters.CORTES)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PORRAT() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.PORRAT, Parameters.PORRAT)
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

                Public ReadOnly Property NUMMOV_E() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMMOV_E, Parameters.NUMMOV_E)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMMOV_S() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMMOV_S, Parameters.NUMMOV_S)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ECORTES() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.ECORTES, Parameters.ECORTES)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property EPORRAT() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.EPORRAT, Parameters.EPORRAT)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property EIMPORTE() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.EIMPORTE, Parameters.EIMPORTE)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property


                Private _clause As WhereClause
            End Class
#End Region

            Public ReadOnly Property TIPO() As WhereParameter
                Get
                    If _TIPO_W Is Nothing Then
                        _TIPO_W = TearOff.TIPO
                    End If
                    Return _TIPO_W
                End Get
            End Property

            Public ReadOnly Property CODPROV() As WhereParameter
                Get
                    If _CODPROV_W Is Nothing Then
                        _CODPROV_W = TearOff.CODPROV
                    End If
                    Return _CODPROV_W
                End Get
            End Property

            Public ReadOnly Property ANNO() As WhereParameter
                Get
                    If _ANNO_W Is Nothing Then
                        _ANNO_W = TearOff.ANNO
                    End If
                    Return _ANNO_W
                End Get
            End Property

            Public ReadOnly Property NUMALBAR() As WhereParameter
                Get
                    If _NUMALBAR_W Is Nothing Then
                        _NUMALBAR_W = TearOff.NUMALBAR
                    End If
                    Return _NUMALBAR_W
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

            Public ReadOnly Property CANREC() As WhereParameter
                Get
                    If _CANREC_W Is Nothing Then
                        _CANREC_W = TearOff.CANREC
                    End If
                    Return _CANREC_W
                End Get
            End Property

            Public ReadOnly Property CORTES() As WhereParameter
                Get
                    If _CORTES_W Is Nothing Then
                        _CORTES_W = TearOff.CORTES
                    End If
                    Return _CORTES_W
                End Get
            End Property

            Public ReadOnly Property PORRAT() As WhereParameter
                Get
                    If _PORRAT_W Is Nothing Then
                        _PORRAT_W = TearOff.PORRAT
                    End If
                    Return _PORRAT_W
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

            Public ReadOnly Property NUMMOV_E() As WhereParameter
                Get
                    If _NUMMOV_E_W Is Nothing Then
                        _NUMMOV_E_W = TearOff.NUMMOV_E
                    End If
                    Return _NUMMOV_E_W
                End Get
            End Property

            Public ReadOnly Property NUMMOV_S() As WhereParameter
                Get
                    If _NUMMOV_S_W Is Nothing Then
                        _NUMMOV_S_W = TearOff.NUMMOV_S
                    End If
                    Return _NUMMOV_S_W
                End Get
            End Property

            Public ReadOnly Property ECORTES() As WhereParameter
                Get
                    If _ECORTES_W Is Nothing Then
                        _ECORTES_W = TearOff.ECORTES
                    End If
                    Return _ECORTES_W
                End Get
            End Property

            Public ReadOnly Property EPORRAT() As WhereParameter
                Get
                    If _EPORRAT_W Is Nothing Then
                        _EPORRAT_W = TearOff.EPORRAT
                    End If
                    Return _EPORRAT_W
                End Get
            End Property

            Public ReadOnly Property EIMPORTE() As WhereParameter
                Get
                    If _EIMPORTE_W Is Nothing Then
                        _EIMPORTE_W = TearOff.EIMPORTE
                    End If
                    Return _EIMPORTE_W
                End Get
            End Property

            Private _TIPO_W As WhereParameter = Nothing
            Private _CODPROV_W As WhereParameter = Nothing
            Private _ANNO_W As WhereParameter = Nothing
            Private _NUMALBAR_W As WhereParameter = Nothing
            Private _NUMPED_W As WhereParameter = Nothing
            Private _NUMLIN_W As WhereParameter = Nothing
            Private _CANREC_W As WhereParameter = Nothing
            Private _CORTES_W As WhereParameter = Nothing
            Private _PORRAT_W As WhereParameter = Nothing
            Private _IMPORTE_W As WhereParameter = Nothing
            Private _NUMMOV_E_W As WhereParameter = Nothing
            Private _NUMMOV_S_W As WhereParameter = Nothing
            Private _ECORTES_W As WhereParameter = Nothing
            Private _EPORRAT_W As WhereParameter = Nothing
            Private _EIMPORTE_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _TIPO_W = Nothing
                _CODPROV_W = Nothing
                _ANNO_W = Nothing
                _NUMALBAR_W = Nothing
                _NUMPED_W = Nothing
                _NUMLIN_W = Nothing
                _CANREC_W = Nothing
                _CORTES_W = Nothing
                _PORRAT_W = Nothing
                _IMPORTE_W = Nothing
                _NUMMOV_E_W = Nothing
                _NUMMOV_S_W = Nothing
                _ECORTES_W = Nothing
                _EPORRAT_W = Nothing
                _EIMPORTE_W = Nothing
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


                Public ReadOnly Property TIPO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TIPO, Parameters.TIPO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODPROV() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CODPROV, Parameters.CODPROV)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ANNO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ANNO, Parameters.ANNO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMALBAR() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMALBAR, Parameters.NUMALBAR)
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

                Public ReadOnly Property CANREC() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CANREC, Parameters.CANREC)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CORTES() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CORTES, Parameters.CORTES)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PORRAT() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.PORRAT, Parameters.PORRAT)
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

                Public ReadOnly Property NUMMOV_E() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMMOV_E, Parameters.NUMMOV_E)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMMOV_S() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMMOV_S, Parameters.NUMMOV_S)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ECORTES() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ECORTES, Parameters.ECORTES)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property EPORRAT() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.EPORRAT, Parameters.EPORRAT)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property EIMPORTE() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.EIMPORTE, Parameters.EIMPORTE)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property


                Private _clause As AggregateClause
            End Class
#End Region

            Public ReadOnly Property TIPO() As AggregateParameter
                Get
                    If _TIPO_W Is Nothing Then
                        _TIPO_W = TearOff.TIPO
                    End If
                    Return _TIPO_W
                End Get
            End Property

            Public ReadOnly Property CODPROV() As AggregateParameter
                Get
                    If _CODPROV_W Is Nothing Then
                        _CODPROV_W = TearOff.CODPROV
                    End If
                    Return _CODPROV_W
                End Get
            End Property

            Public ReadOnly Property ANNO() As AggregateParameter
                Get
                    If _ANNO_W Is Nothing Then
                        _ANNO_W = TearOff.ANNO
                    End If
                    Return _ANNO_W
                End Get
            End Property

            Public ReadOnly Property NUMALBAR() As AggregateParameter
                Get
                    If _NUMALBAR_W Is Nothing Then
                        _NUMALBAR_W = TearOff.NUMALBAR
                    End If
                    Return _NUMALBAR_W
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

            Public ReadOnly Property CANREC() As AggregateParameter
                Get
                    If _CANREC_W Is Nothing Then
                        _CANREC_W = TearOff.CANREC
                    End If
                    Return _CANREC_W
                End Get
            End Property

            Public ReadOnly Property CORTES() As AggregateParameter
                Get
                    If _CORTES_W Is Nothing Then
                        _CORTES_W = TearOff.CORTES
                    End If
                    Return _CORTES_W
                End Get
            End Property

            Public ReadOnly Property PORRAT() As AggregateParameter
                Get
                    If _PORRAT_W Is Nothing Then
                        _PORRAT_W = TearOff.PORRAT
                    End If
                    Return _PORRAT_W
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

            Public ReadOnly Property NUMMOV_E() As AggregateParameter
                Get
                    If _NUMMOV_E_W Is Nothing Then
                        _NUMMOV_E_W = TearOff.NUMMOV_E
                    End If
                    Return _NUMMOV_E_W
                End Get
            End Property

            Public ReadOnly Property NUMMOV_S() As AggregateParameter
                Get
                    If _NUMMOV_S_W Is Nothing Then
                        _NUMMOV_S_W = TearOff.NUMMOV_S
                    End If
                    Return _NUMMOV_S_W
                End Get
            End Property

            Public ReadOnly Property ECORTES() As AggregateParameter
                Get
                    If _ECORTES_W Is Nothing Then
                        _ECORTES_W = TearOff.ECORTES
                    End If
                    Return _ECORTES_W
                End Get
            End Property

            Public ReadOnly Property EPORRAT() As AggregateParameter
                Get
                    If _EPORRAT_W Is Nothing Then
                        _EPORRAT_W = TearOff.EPORRAT
                    End If
                    Return _EPORRAT_W
                End Get
            End Property

            Public ReadOnly Property EIMPORTE() As AggregateParameter
                Get
                    If _EIMPORTE_W Is Nothing Then
                        _EIMPORTE_W = TearOff.EIMPORTE
                    End If
                    Return _EIMPORTE_W
                End Get
            End Property

            Private _TIPO_W As AggregateParameter = Nothing
            Private _CODPROV_W As AggregateParameter = Nothing
            Private _ANNO_W As AggregateParameter = Nothing
            Private _NUMALBAR_W As AggregateParameter = Nothing
            Private _NUMPED_W As AggregateParameter = Nothing
            Private _NUMLIN_W As AggregateParameter = Nothing
            Private _CANREC_W As AggregateParameter = Nothing
            Private _CORTES_W As AggregateParameter = Nothing
            Private _PORRAT_W As AggregateParameter = Nothing
            Private _IMPORTE_W As AggregateParameter = Nothing
            Private _NUMMOV_E_W As AggregateParameter = Nothing
            Private _NUMMOV_S_W As AggregateParameter = Nothing
            Private _ECORTES_W As AggregateParameter = Nothing
            Private _EPORRAT_W As AggregateParameter = Nothing
            Private _EIMPORTE_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _TIPO_W = Nothing
                _CODPROV_W = Nothing
                _ANNO_W = Nothing
                _NUMALBAR_W = Nothing
                _NUMPED_W = Nothing
                _NUMLIN_W = Nothing
                _CANREC_W = Nothing
                _CORTES_W = Nothing
                _PORRAT_W = Nothing
                _IMPORTE_W = Nothing
                _NUMMOV_E_W = Nothing
                _NUMMOV_S_W = Nothing
                _ECORTES_W = Nothing
                _EPORRAT_W = Nothing
                _EIMPORTE_W = Nothing
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
            cmd.CommandText = Me.SchemaStoredProcedure + "PI_GCALBARA"

            CreateParameters(cmd)


            Return cmd

        End Function

        Protected Overrides Function GetUpdateCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PU_GCALBARA"

            CreateParameters(cmd)

            Return cmd

        End Function

        Protected Overrides Function GetDeleteCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PD_GCALBARA"

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.ANNO)
            p.SourceColumn = ColumnNames.ANNO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CODPROV)
            p.SourceColumn = ColumnNames.CODPROV
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMALBAR)
            p.SourceColumn = ColumnNames.NUMALBAR
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMLIN)
            p.SourceColumn = ColumnNames.NUMLIN
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMPED)
            p.SourceColumn = ColumnNames.NUMPED
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.TIPO)
            p.SourceColumn = ColumnNames.TIPO
            p.SourceVersion = DataRowVersion.Current


            Return cmd

        End Function

        Private Sub CreateParameters(ByVal cmd As OracleCommand)

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.TIPO)
            p.SourceColumn = ColumnNames.TIPO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CODPROV)
            p.SourceColumn = ColumnNames.CODPROV
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.ANNO)
            p.SourceColumn = ColumnNames.ANNO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMALBAR)
            p.SourceColumn = ColumnNames.NUMALBAR
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMPED)
            p.SourceColumn = ColumnNames.NUMPED
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMLIN)
            p.SourceColumn = ColumnNames.NUMLIN
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CANREC)
            p.SourceColumn = ColumnNames.CANREC
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CORTES)
            p.SourceColumn = ColumnNames.CORTES
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.PORRAT)
            p.SourceColumn = ColumnNames.PORRAT
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IMPORTE)
            p.SourceColumn = ColumnNames.IMPORTE
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMMOV_E)
            p.SourceColumn = ColumnNames.NUMMOV_E
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMMOV_S)
            p.SourceColumn = ColumnNames.NUMMOV_S
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.ECORTES)
            p.SourceColumn = ColumnNames.ECORTES
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.EPORRAT)
            p.SourceColumn = ColumnNames.EPORRAT
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.EIMPORTE)
            p.SourceColumn = ColumnNames.EIMPORTE
            p.SourceVersion = DataRowVersion.Current


        End Sub

    End Class

End Namespace

