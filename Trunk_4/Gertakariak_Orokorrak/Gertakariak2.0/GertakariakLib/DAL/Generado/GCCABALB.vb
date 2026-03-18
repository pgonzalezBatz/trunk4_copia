



'===============================================================================
'BATZ, Koop. - 08/06/2009 10:11:25
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

    Public MustInherit Class _GCCABALB
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "XBAT."
            Me.QuerySource = "GCCABALB"
            Me.MappingName = "GCCABALB"
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


            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_GCCABALB", parameters)
        End Function

        '=================================================================
        ' Public Overridable Function LoadByPrimaryKey()  As Boolean
        '=================================================================
        '  Loads a single row of via the primary key
        '=================================================================
        Public Overridable Function LoadByPrimaryKey(ByVal ANNO As Decimal, ByVal CODPROV As String, ByVal NUMALBAR As Decimal, ByVal TIPO As String) As Boolean

            Dim parameters As ListDictionary = New ListDictionary()
            parameters.Add(_GCCABALB.Parameters.ANNO, ANNO)

            parameters.Add(_GCCABALB.Parameters.CODPROV, CODPROV)

            parameters.Add(_GCCABALB.Parameters.NUMALBAR, NUMALBAR)

            parameters.Add(_GCCABALB.Parameters.TIPO, TIPO)


            Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
            p.Direction = ParameterDirection.Output
            parameters.Add(p, Nothing)

            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_GCCABALB", parameters)

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

            Public Shared ReadOnly Property FECHAREC As OracleParameter
                Get
                    Return New OracleParameter("p_FECHAREC", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property PORTES As OracleParameter
                Get
                    Return New OracleParameter("p_PORTES", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property NFACTU As OracleParameter
                Get
                    Return New OracleParameter("p_NFACTU", OracleDbType.Char, 15)
                End Get
            End Property

            Public Shared ReadOnly Property FECHACON As OracleParameter
                Get
                    Return New OracleParameter("p_FECHACON", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property DOCBATZ As OracleParameter
                Get
                    Return New OracleParameter("p_DOCBATZ", OracleDbType.Varchar2, 15)
                End Get
            End Property

            Public Shared ReadOnly Property EPORTES As OracleParameter
                Get
                    Return New OracleParameter("p_EPORTES", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property FECVENCI As OracleParameter
                Get
                    Return New OracleParameter("p_FECVENCI", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property USUARIO As OracleParameter
                Get
                    Return New OracleParameter("p_USUARIO", OracleDbType.Varchar2, 20)
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
            Public Const FECHAREC As String = "FECHAREC"
            Public Const PORTES As String = "PORTES"
            Public Const NFACTU As String = "NFACTU"
            Public Const FECHACON As String = "FECHACON"
            Public Const DOCBATZ As String = "DOCBATZ"
            Public Const EPORTES As String = "EPORTES"
            Public Const FECVENCI As String = "FECVENCI"
            Public Const USUARIO As String = "USUARIO"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(TIPO) = _GCCABALB.PropertyNames.TIPO
                    ht(CODPROV) = _GCCABALB.PropertyNames.CODPROV
                    ht(ANNO) = _GCCABALB.PropertyNames.ANNO
                    ht(NUMALBAR) = _GCCABALB.PropertyNames.NUMALBAR
                    ht(FECHAREC) = _GCCABALB.PropertyNames.FECHAREC
                    ht(PORTES) = _GCCABALB.PropertyNames.PORTES
                    ht(NFACTU) = _GCCABALB.PropertyNames.NFACTU
                    ht(FECHACON) = _GCCABALB.PropertyNames.FECHACON
                    ht(DOCBATZ) = _GCCABALB.PropertyNames.DOCBATZ
                    ht(EPORTES) = _GCCABALB.PropertyNames.EPORTES
                    ht(FECVENCI) = _GCCABALB.PropertyNames.FECVENCI
                    ht(USUARIO) = _GCCABALB.PropertyNames.USUARIO

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
            Public Const FECHAREC As String = "FECHAREC"
            Public Const PORTES As String = "PORTES"
            Public Const NFACTU As String = "NFACTU"
            Public Const FECHACON As String = "FECHACON"
            Public Const DOCBATZ As String = "DOCBATZ"
            Public Const EPORTES As String = "EPORTES"
            Public Const FECVENCI As String = "FECVENCI"
            Public Const USUARIO As String = "USUARIO"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(TIPO) = _GCCABALB.ColumnNames.TIPO
                    ht(CODPROV) = _GCCABALB.ColumnNames.CODPROV
                    ht(ANNO) = _GCCABALB.ColumnNames.ANNO
                    ht(NUMALBAR) = _GCCABALB.ColumnNames.NUMALBAR
                    ht(FECHAREC) = _GCCABALB.ColumnNames.FECHAREC
                    ht(PORTES) = _GCCABALB.ColumnNames.PORTES
                    ht(NFACTU) = _GCCABALB.ColumnNames.NFACTU
                    ht(FECHACON) = _GCCABALB.ColumnNames.FECHACON
                    ht(DOCBATZ) = _GCCABALB.ColumnNames.DOCBATZ
                    ht(EPORTES) = _GCCABALB.ColumnNames.EPORTES
                    ht(FECVENCI) = _GCCABALB.ColumnNames.FECVENCI
                    ht(USUARIO) = _GCCABALB.ColumnNames.USUARIO

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
            Public Const FECHAREC As String = "s_FECHAREC"
            Public Const PORTES As String = "s_PORTES"
            Public Const NFACTU As String = "s_NFACTU"
            Public Const FECHACON As String = "s_FECHACON"
            Public Const DOCBATZ As String = "s_DOCBATZ"
            Public Const EPORTES As String = "s_EPORTES"
            Public Const FECVENCI As String = "s_FECVENCI"
            Public Const USUARIO As String = "s_USUARIO"

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

        Public Overridable Property FECHAREC As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECHAREC)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECHAREC, Value)
            End Set
        End Property

        Public Overridable Property PORTES As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.PORTES)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.PORTES, Value)
            End Set
        End Property

        Public Overridable Property NFACTU As String
            Get
                Return MyBase.GetString(ColumnNames.NFACTU)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.NFACTU, Value)
            End Set
        End Property

        Public Overridable Property FECHACON As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECHACON)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECHACON, Value)
            End Set
        End Property

        Public Overridable Property DOCBATZ As String
            Get
                Return MyBase.GetString(ColumnNames.DOCBATZ)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.DOCBATZ, Value)
            End Set
        End Property

        Public Overridable Property EPORTES As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.EPORTES)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.EPORTES, Value)
            End Set
        End Property

        Public Overridable Property FECVENCI As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECVENCI)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECVENCI, Value)
            End Set
        End Property

        Public Overridable Property USUARIO As String
            Get
                Return MyBase.GetString(ColumnNames.USUARIO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.USUARIO, Value)
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

        Public Overridable Property s_FECHAREC As String
            Get
                If Me.IsColumnNull(ColumnNames.FECHAREC) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FECHAREC)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FECHAREC)
                Else
                    Me.FECHAREC = MyBase.SetDateTimeAsString(ColumnNames.FECHAREC, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_PORTES As String
            Get
                If Me.IsColumnNull(ColumnNames.PORTES) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.PORTES)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.PORTES)
                Else
                    Me.PORTES = MyBase.SetDecimalAsString(ColumnNames.PORTES, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_NFACTU As String
            Get
                If Me.IsColumnNull(ColumnNames.NFACTU) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.NFACTU)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NFACTU)
                Else
                    Me.NFACTU = MyBase.SetStringAsString(ColumnNames.NFACTU, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FECHACON As String
            Get
                If Me.IsColumnNull(ColumnNames.FECHACON) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FECHACON)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FECHACON)
                Else
                    Me.FECHACON = MyBase.SetDateTimeAsString(ColumnNames.FECHACON, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_DOCBATZ As String
            Get
                If Me.IsColumnNull(ColumnNames.DOCBATZ) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.DOCBATZ)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.DOCBATZ)
                Else
                    Me.DOCBATZ = MyBase.SetStringAsString(ColumnNames.DOCBATZ, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_EPORTES As String
            Get
                If Me.IsColumnNull(ColumnNames.EPORTES) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.EPORTES)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.EPORTES)
                Else
                    Me.EPORTES = MyBase.SetDecimalAsString(ColumnNames.EPORTES, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FECVENCI As String
            Get
                If Me.IsColumnNull(ColumnNames.FECVENCI) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FECVENCI)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FECVENCI)
                Else
                    Me.FECVENCI = MyBase.SetDateTimeAsString(ColumnNames.FECVENCI, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_USUARIO As String
            Get
                If Me.IsColumnNull(ColumnNames.USUARIO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.USUARIO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.USUARIO)
                Else
                    Me.USUARIO = MyBase.SetStringAsString(ColumnNames.USUARIO, Value)
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

                Public ReadOnly Property FECHAREC() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FECHAREC, Parameters.FECHAREC)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PORTES() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.PORTES, Parameters.PORTES)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NFACTU() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NFACTU, Parameters.NFACTU)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECHACON() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FECHACON, Parameters.FECHACON)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DOCBATZ() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.DOCBATZ, Parameters.DOCBATZ)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property EPORTES() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.EPORTES, Parameters.EPORTES)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECVENCI() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FECVENCI, Parameters.FECVENCI)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property USUARIO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.USUARIO, Parameters.USUARIO)
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

            Public ReadOnly Property FECHAREC() As WhereParameter
                Get
                    If _FECHAREC_W Is Nothing Then
                        _FECHAREC_W = TearOff.FECHAREC
                    End If
                    Return _FECHAREC_W
                End Get
            End Property

            Public ReadOnly Property PORTES() As WhereParameter
                Get
                    If _PORTES_W Is Nothing Then
                        _PORTES_W = TearOff.PORTES
                    End If
                    Return _PORTES_W
                End Get
            End Property

            Public ReadOnly Property NFACTU() As WhereParameter
                Get
                    If _NFACTU_W Is Nothing Then
                        _NFACTU_W = TearOff.NFACTU
                    End If
                    Return _NFACTU_W
                End Get
            End Property

            Public ReadOnly Property FECHACON() As WhereParameter
                Get
                    If _FECHACON_W Is Nothing Then
                        _FECHACON_W = TearOff.FECHACON
                    End If
                    Return _FECHACON_W
                End Get
            End Property

            Public ReadOnly Property DOCBATZ() As WhereParameter
                Get
                    If _DOCBATZ_W Is Nothing Then
                        _DOCBATZ_W = TearOff.DOCBATZ
                    End If
                    Return _DOCBATZ_W
                End Get
            End Property

            Public ReadOnly Property EPORTES() As WhereParameter
                Get
                    If _EPORTES_W Is Nothing Then
                        _EPORTES_W = TearOff.EPORTES
                    End If
                    Return _EPORTES_W
                End Get
            End Property

            Public ReadOnly Property FECVENCI() As WhereParameter
                Get
                    If _FECVENCI_W Is Nothing Then
                        _FECVENCI_W = TearOff.FECVENCI
                    End If
                    Return _FECVENCI_W
                End Get
            End Property

            Public ReadOnly Property USUARIO() As WhereParameter
                Get
                    If _USUARIO_W Is Nothing Then
                        _USUARIO_W = TearOff.USUARIO
                    End If
                    Return _USUARIO_W
                End Get
            End Property

            Private _TIPO_W As WhereParameter = Nothing
            Private _CODPROV_W As WhereParameter = Nothing
            Private _ANNO_W As WhereParameter = Nothing
            Private _NUMALBAR_W As WhereParameter = Nothing
            Private _FECHAREC_W As WhereParameter = Nothing
            Private _PORTES_W As WhereParameter = Nothing
            Private _NFACTU_W As WhereParameter = Nothing
            Private _FECHACON_W As WhereParameter = Nothing
            Private _DOCBATZ_W As WhereParameter = Nothing
            Private _EPORTES_W As WhereParameter = Nothing
            Private _FECVENCI_W As WhereParameter = Nothing
            Private _USUARIO_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _TIPO_W = Nothing
                _CODPROV_W = Nothing
                _ANNO_W = Nothing
                _NUMALBAR_W = Nothing
                _FECHAREC_W = Nothing
                _PORTES_W = Nothing
                _NFACTU_W = Nothing
                _FECHACON_W = Nothing
                _DOCBATZ_W = Nothing
                _EPORTES_W = Nothing
                _FECVENCI_W = Nothing
                _USUARIO_W = Nothing
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

                Public ReadOnly Property FECHAREC() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECHAREC, Parameters.FECHAREC)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PORTES() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.PORTES, Parameters.PORTES)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NFACTU() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NFACTU, Parameters.NFACTU)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECHACON() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECHACON, Parameters.FECHACON)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DOCBATZ() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DOCBATZ, Parameters.DOCBATZ)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property EPORTES() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.EPORTES, Parameters.EPORTES)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECVENCI() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECVENCI, Parameters.FECVENCI)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property USUARIO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.USUARIO, Parameters.USUARIO)
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

            Public ReadOnly Property FECHAREC() As AggregateParameter
                Get
                    If _FECHAREC_W Is Nothing Then
                        _FECHAREC_W = TearOff.FECHAREC
                    End If
                    Return _FECHAREC_W
                End Get
            End Property

            Public ReadOnly Property PORTES() As AggregateParameter
                Get
                    If _PORTES_W Is Nothing Then
                        _PORTES_W = TearOff.PORTES
                    End If
                    Return _PORTES_W
                End Get
            End Property

            Public ReadOnly Property NFACTU() As AggregateParameter
                Get
                    If _NFACTU_W Is Nothing Then
                        _NFACTU_W = TearOff.NFACTU
                    End If
                    Return _NFACTU_W
                End Get
            End Property

            Public ReadOnly Property FECHACON() As AggregateParameter
                Get
                    If _FECHACON_W Is Nothing Then
                        _FECHACON_W = TearOff.FECHACON
                    End If
                    Return _FECHACON_W
                End Get
            End Property

            Public ReadOnly Property DOCBATZ() As AggregateParameter
                Get
                    If _DOCBATZ_W Is Nothing Then
                        _DOCBATZ_W = TearOff.DOCBATZ
                    End If
                    Return _DOCBATZ_W
                End Get
            End Property

            Public ReadOnly Property EPORTES() As AggregateParameter
                Get
                    If _EPORTES_W Is Nothing Then
                        _EPORTES_W = TearOff.EPORTES
                    End If
                    Return _EPORTES_W
                End Get
            End Property

            Public ReadOnly Property FECVENCI() As AggregateParameter
                Get
                    If _FECVENCI_W Is Nothing Then
                        _FECVENCI_W = TearOff.FECVENCI
                    End If
                    Return _FECVENCI_W
                End Get
            End Property

            Public ReadOnly Property USUARIO() As AggregateParameter
                Get
                    If _USUARIO_W Is Nothing Then
                        _USUARIO_W = TearOff.USUARIO
                    End If
                    Return _USUARIO_W
                End Get
            End Property

            Private _TIPO_W As AggregateParameter = Nothing
            Private _CODPROV_W As AggregateParameter = Nothing
            Private _ANNO_W As AggregateParameter = Nothing
            Private _NUMALBAR_W As AggregateParameter = Nothing
            Private _FECHAREC_W As AggregateParameter = Nothing
            Private _PORTES_W As AggregateParameter = Nothing
            Private _NFACTU_W As AggregateParameter = Nothing
            Private _FECHACON_W As AggregateParameter = Nothing
            Private _DOCBATZ_W As AggregateParameter = Nothing
            Private _EPORTES_W As AggregateParameter = Nothing
            Private _FECVENCI_W As AggregateParameter = Nothing
            Private _USUARIO_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _TIPO_W = Nothing
                _CODPROV_W = Nothing
                _ANNO_W = Nothing
                _NUMALBAR_W = Nothing
                _FECHAREC_W = Nothing
                _PORTES_W = Nothing
                _NFACTU_W = Nothing
                _FECHACON_W = Nothing
                _DOCBATZ_W = Nothing
                _EPORTES_W = Nothing
                _FECVENCI_W = Nothing
                _USUARIO_W = Nothing
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
            cmd.CommandText = Me.SchemaStoredProcedure + "PI_GCCABALB"

            CreateParameters(cmd)


            Return cmd

        End Function

        Protected Overrides Function GetUpdateCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PU_GCCABALB"

            CreateParameters(cmd)

            Return cmd

        End Function

        Protected Overrides Function GetDeleteCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PD_GCCABALB"

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

            p = cmd.Parameters.Add(Parameters.FECHAREC)
            p.SourceColumn = ColumnNames.FECHAREC
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.PORTES)
            p.SourceColumn = ColumnNames.PORTES
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NFACTU)
            p.SourceColumn = ColumnNames.NFACTU
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.FECHACON)
            p.SourceColumn = ColumnNames.FECHACON
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.DOCBATZ)
            p.SourceColumn = ColumnNames.DOCBATZ
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.EPORTES)
            p.SourceColumn = ColumnNames.EPORTES
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.FECVENCI)
            p.SourceColumn = ColumnNames.FECVENCI
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.USUARIO)
            p.SourceColumn = ColumnNames.USUARIO
            p.SourceVersion = DataRowVersion.Current


        End Sub

    End Class

End Namespace

