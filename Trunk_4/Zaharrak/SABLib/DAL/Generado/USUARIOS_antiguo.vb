Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Specialized
Imports Oracle.DataAccess.Client
Imports AccesoAutomaticoBD

Namespace DAL

    Public MustInherit Class _USUARIOS
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "SAB."
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
        Public Overridable Function LoadByPrimaryKey(ByVal ID As Decimal) As Boolean

            Dim parameters As ListDictionary = New ListDictionary()
            parameters.Add(_USUARIOS.Parameters.ID, ID)


            Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
            p.Direction = ParameterDirection.Output
            parameters.Add(p, Nothing)

            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_USUARIOS", parameters)

        End Function

#Region "Parameters"
        Protected Class Parameters

            Public Shared ReadOnly Property ID() As OracleParameter
                Get
                    Return New OracleParameter("p_ID", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property IDEMPRESAS() As OracleParameter
                Get
                    Return New OracleParameter("p_IDEMPRESAS", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property IDCULTURAS() As OracleParameter
                Get
                    Return New OracleParameter("p_IDCULTURAS", OracleDbType.Varchar2, 5)
                End Get
            End Property

            Public Shared ReadOnly Property NOMBREUSUARIO() As OracleParameter
                Get
                    Return New OracleParameter("p_NOMBREUSUARIO", OracleDbType.Varchar2, 40)
                End Get
            End Property

            Public Shared ReadOnly Property IDDIRECTORIOACTIVO() As OracleParameter
                Get
                    Return New OracleParameter("p_IDDIRECTORIOACTIVO", OracleDbType.Varchar2, 100)
                End Get
            End Property

            Public Shared ReadOnly Property CODPERSONA() As OracleParameter
                Get
                    Return New OracleParameter("p_CODPERSONA", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property PWD() As OracleParameter
                Get
                    Return New OracleParameter("p_PWD", OracleDbType.Varchar2, 20)
                End Get
            End Property

            Public Shared ReadOnly Property FECHAALTA() As OracleParameter
                Get
                    Return New OracleParameter("p_FECHAALTA", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property FECHABAJA() As OracleParameter
                Get
                    Return New OracleParameter("p_FECHABAJA", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property IDFAPERSONAL() As OracleParameter
                Get
                    Return New OracleParameter("p_IDFAPERSONAL", OracleDbType.Varchar2, 12)
                End Get
            End Property

            Public Shared ReadOnly Property IDRECURSOSHUMANOS() As OracleParameter
                Get
                    Return New OracleParameter("p_IDRECURSOSHUMANOS", OracleDbType.Varchar2, 12)
                End Get
            End Property

            Public Shared ReadOnly Property IDMATRIX() As OracleParameter
                Get
                    Return New OracleParameter("p_IDMATRIX", OracleDbType.Varchar2, 12)
                End Get
            End Property

            Public Shared ReadOnly Property IDFTP() As OracleParameter
                Get
                    Return New OracleParameter("p_IDFTP", OracleDbType.Varchar2, 20)
                End Get
            End Property

            Public Shared ReadOnly Property EMAIL() As OracleParameter
                Get
                    Return New OracleParameter("p_EMAIL", OracleDbType.Varchar2, 40)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const ID As String = "ID"
            Public Const IDEMPRESAS As String = "IDEMPRESAS"
            Public Const IDCULTURAS As String = "IDCULTURAS"
            Public Const NOMBREUSUARIO As String = "NOMBREUSUARIO"
            Public Const IDDIRECTORIOACTIVO As String = "IDDIRECTORIOACTIVO"
            Public Const CODPERSONA As String = "CODPERSONA"
            Public Const PWD As String = "PWD"
            Public Const FECHAALTA As String = "FECHAALTA"
            Public Const FECHABAJA As String = "FECHABAJA"
            Public Const IDFAPERSONAL As String = "IDFAPERSONAL"
            Public Const IDRECURSOSHUMANOS As String = "IDRECURSOSHUMANOS"
            Public Const IDMATRIX As String = "IDMATRIX"
            Public Const IDFTP As String = "IDFTP"
            Public Const EMAIL As String = "EMAIL"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(ID) = _USUARIOS.PropertyNames.ID
                    ht(IDEMPRESAS) = _USUARIOS.PropertyNames.IDEMPRESAS
                    ht(IDCULTURAS) = _USUARIOS.PropertyNames.IDCULTURAS
                    ht(NOMBREUSUARIO) = _USUARIOS.PropertyNames.NOMBREUSUARIO
                    ht(IDDIRECTORIOACTIVO) = _USUARIOS.PropertyNames.IDDIRECTORIOACTIVO
                    ht(CODPERSONA) = _USUARIOS.PropertyNames.CODPERSONA
                    ht(PWD) = _USUARIOS.PropertyNames.PWD
                    ht(FECHAALTA) = _USUARIOS.PropertyNames.FECHAALTA
                    ht(FECHABAJA) = _USUARIOS.PropertyNames.FECHABAJA
                    ht(IDFAPERSONAL) = _USUARIOS.PropertyNames.IDFAPERSONAL
                    ht(IDRECURSOSHUMANOS) = _USUARIOS.PropertyNames.IDRECURSOSHUMANOS
                    ht(IDMATRIX) = _USUARIOS.PropertyNames.IDMATRIX
                    ht(IDFTP) = _USUARIOS.PropertyNames.IDFTP
                    ht(EMAIL) = _USUARIOS.PropertyNames.EMAIL

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const ID As String = "ID"
            Public Const IDEMPRESAS As String = "IDEMPRESAS"
            Public Const IDCULTURAS As String = "IDCULTURAS"
            Public Const NOMBREUSUARIO As String = "NOMBREUSUARIO"
            Public Const IDDIRECTORIOACTIVO As String = "IDDIRECTORIOACTIVO"
            Public Const CODPERSONA As String = "CODPERSONA"
            Public Const PWD As String = "PWD"
            Public Const FECHAALTA As String = "FECHAALTA"
            Public Const FECHABAJA As String = "FECHABAJA"
            Public Const IDFAPERSONAL As String = "IDFAPERSONAL"
            Public Const IDRECURSOSHUMANOS As String = "IDRECURSOSHUMANOS"
            Public Const IDMATRIX As String = "IDMATRIX"
            Public Const IDFTP As String = "IDFTP"
            Public Const EMAIL As String = "EMAIL"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(ID) = _USUARIOS.ColumnNames.ID
                    ht(IDEMPRESAS) = _USUARIOS.ColumnNames.IDEMPRESAS
                    ht(IDCULTURAS) = _USUARIOS.ColumnNames.IDCULTURAS
                    ht(NOMBREUSUARIO) = _USUARIOS.ColumnNames.NOMBREUSUARIO
                    ht(IDDIRECTORIOACTIVO) = _USUARIOS.ColumnNames.IDDIRECTORIOACTIVO
                    ht(CODPERSONA) = _USUARIOS.ColumnNames.CODPERSONA
                    ht(PWD) = _USUARIOS.ColumnNames.PWD
                    ht(FECHAALTA) = _USUARIOS.ColumnNames.FECHAALTA
                    ht(FECHABAJA) = _USUARIOS.ColumnNames.FECHABAJA
                    ht(IDFAPERSONAL) = _USUARIOS.ColumnNames.IDFAPERSONAL
                    ht(IDRECURSOSHUMANOS) = _USUARIOS.ColumnNames.IDRECURSOSHUMANOS
                    ht(IDMATRIX) = _USUARIOS.ColumnNames.IDMATRIX
                    ht(IDFTP) = _USUARIOS.ColumnNames.IDFTP
                    ht(EMAIL) = _USUARIOS.ColumnNames.EMAIL

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const ID As String = "s_ID"
            Public Const IDEMPRESAS As String = "s_IDEMPRESAS"
            Public Const IDCULTURAS As String = "s_IDCULTURAS"
            Public Const NOMBREUSUARIO As String = "s_NOMBREUSUARIO"
            Public Const IDDIRECTORIOACTIVO As String = "s_IDDIRECTORIOACTIVO"
            Public Const CODPERSONA As String = "s_CODPERSONA"
            Public Const PWD As String = "s_PWD"
            Public Const FECHAALTA As String = "s_FECHAALTA"
            Public Const FECHABAJA As String = "s_FECHABAJA"
            Public Const IDFAPERSONAL As String = "s_IDFAPERSONAL"
            Public Const IDRECURSOSHUMANOS As String = "s_IDRECURSOSHUMANOS"
            Public Const IDMATRIX As String = "s_IDMATRIX"
            Public Const IDFTP As String = "s_IDFTP"
            Public Const EMAIL As String = "s_EMAIL"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property ID() As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.ID)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.ID, Value)
            End Set
        End Property

        Public Overridable Property IDEMPRESAS() As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IDEMPRESAS)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IDEMPRESAS, Value)
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

        Public Overridable Property NOMBREUSUARIO() As String
            Get
                Return MyBase.GetString(ColumnNames.NOMBREUSUARIO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.NOMBREUSUARIO, Value)
            End Set
        End Property

        Public Overridable Property IDDIRECTORIOACTIVO() As String
            Get
                Return MyBase.GetString(ColumnNames.IDDIRECTORIOACTIVO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.IDDIRECTORIOACTIVO, Value)
            End Set
        End Property

        Public Overridable Property CODPERSONA() As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.CODPERSONA)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.CODPERSONA, Value)
            End Set
        End Property

        Public Overridable Property PWD() As String
            Get
                Return MyBase.GetString(ColumnNames.PWD)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.PWD, Value)
            End Set
        End Property

        Public Overridable Property FECHAALTA() As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECHAALTA)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECHAALTA, Value)
            End Set
        End Property

        Public Overridable Property FECHABAJA() As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECHABAJA)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECHABAJA, Value)
            End Set
        End Property

        Public Overridable Property IDFAPERSONAL() As String
            Get
                Return MyBase.GetString(ColumnNames.IDFAPERSONAL)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.IDFAPERSONAL, Value)
            End Set
        End Property

        Public Overridable Property IDRECURSOSHUMANOS() As String
            Get
                Return MyBase.GetString(ColumnNames.IDRECURSOSHUMANOS)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.IDRECURSOSHUMANOS, Value)
            End Set
        End Property

        Public Overridable Property IDMATRIX() As String
            Get
                Return MyBase.GetString(ColumnNames.IDMATRIX)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.IDMATRIX, Value)
            End Set
        End Property

        Public Overridable Property IDFTP() As String
            Get
                Return MyBase.GetString(ColumnNames.IDFTP)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.IDFTP, Value)
            End Set
        End Property

        Public Overridable Property EMAIL() As String
            Get
                Return MyBase.GetString(ColumnNames.EMAIL)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.EMAIL, Value)
            End Set
        End Property


#End Region

#Region "String Properties"

        Public Overridable Property s_ID() As String
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

        Public Overridable Property s_IDEMPRESAS() As String
            Get
                If Me.IsColumnNull(ColumnNames.IDEMPRESAS) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.IDEMPRESAS)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDEMPRESAS)
                Else
                    Me.IDEMPRESAS = MyBase.SetDecimalAsString(ColumnNames.IDEMPRESAS, Value)
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

        Public Overridable Property s_NOMBREUSUARIO() As String
            Get
                If Me.IsColumnNull(ColumnNames.NOMBREUSUARIO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.NOMBREUSUARIO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NOMBREUSUARIO)
                Else
                    Me.NOMBREUSUARIO = MyBase.SetStringAsString(ColumnNames.NOMBREUSUARIO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_IDDIRECTORIOACTIVO() As String
            Get
                If Me.IsColumnNull(ColumnNames.IDDIRECTORIOACTIVO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.IDDIRECTORIOACTIVO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDDIRECTORIOACTIVO)
                Else
                    Me.IDDIRECTORIOACTIVO = MyBase.SetStringAsString(ColumnNames.IDDIRECTORIOACTIVO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CODPERSONA() As String
            Get
                If Me.IsColumnNull(ColumnNames.CODPERSONA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.CODPERSONA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CODPERSONA)
                Else
                    Me.CODPERSONA = MyBase.SetDecimalAsString(ColumnNames.CODPERSONA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_PWD() As String
            Get
                If Me.IsColumnNull(ColumnNames.PWD) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.PWD)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.PWD)
                Else
                    Me.PWD = MyBase.SetStringAsString(ColumnNames.PWD, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FECHAALTA() As String
            Get
                If Me.IsColumnNull(ColumnNames.FECHAALTA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FECHAALTA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FECHAALTA)
                Else
                    Me.FECHAALTA = MyBase.SetDateTimeAsString(ColumnNames.FECHAALTA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FECHABAJA() As String
            Get
                If Me.IsColumnNull(ColumnNames.FECHABAJA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FECHABAJA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FECHABAJA)
                Else
                    Me.FECHABAJA = MyBase.SetDateTimeAsString(ColumnNames.FECHABAJA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_IDFAPERSONAL() As String
            Get
                If Me.IsColumnNull(ColumnNames.IDFAPERSONAL) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.IDFAPERSONAL)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDFAPERSONAL)
                Else
                    Me.IDFAPERSONAL = MyBase.SetStringAsString(ColumnNames.IDFAPERSONAL, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_IDRECURSOSHUMANOS() As String
            Get
                If Me.IsColumnNull(ColumnNames.IDRECURSOSHUMANOS) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.IDRECURSOSHUMANOS)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDRECURSOSHUMANOS)
                Else
                    Me.IDRECURSOSHUMANOS = MyBase.SetStringAsString(ColumnNames.IDRECURSOSHUMANOS, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_IDMATRIX() As String
            Get
                If Me.IsColumnNull(ColumnNames.IDMATRIX) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.IDMATRIX)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDMATRIX)
                Else
                    Me.IDMATRIX = MyBase.SetStringAsString(ColumnNames.IDMATRIX, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_IDFTP() As String
            Get
                If Me.IsColumnNull(ColumnNames.IDFTP) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.IDFTP)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDFTP)
                Else
                    Me.IDFTP = MyBase.SetStringAsString(ColumnNames.IDFTP, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_EMAIL() As String
            Get
                If Me.IsColumnNull(ColumnNames.EMAIL) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.EMAIL)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.EMAIL)
                Else
                    Me.EMAIL = MyBase.SetStringAsString(ColumnNames.EMAIL, Value)
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


                Public ReadOnly Property ID() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.ID, Parameters.ID)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDEMPRESAS() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDEMPRESAS, Parameters.IDEMPRESAS)
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

                Public ReadOnly Property NOMBREUSUARIO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NOMBREUSUARIO, Parameters.NOMBREUSUARIO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDDIRECTORIOACTIVO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDDIRECTORIOACTIVO, Parameters.IDDIRECTORIOACTIVO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODPERSONA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CODPERSONA, Parameters.CODPERSONA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PWD() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.PWD, Parameters.PWD)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECHAALTA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FECHAALTA, Parameters.FECHAALTA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECHABAJA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FECHABAJA, Parameters.FECHABAJA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDFAPERSONAL() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDFAPERSONAL, Parameters.IDFAPERSONAL)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDRECURSOSHUMANOS() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDRECURSOSHUMANOS, Parameters.IDRECURSOSHUMANOS)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDMATRIX() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDMATRIX, Parameters.IDMATRIX)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDFTP() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDFTP, Parameters.IDFTP)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property EMAIL() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.EMAIL, Parameters.EMAIL)
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

            Public ReadOnly Property IDEMPRESAS() As WhereParameter
                Get
                    If _IDEMPRESAS_W Is Nothing Then
                        _IDEMPRESAS_W = TearOff.IDEMPRESAS
                    End If
                    Return _IDEMPRESAS_W
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

            Public ReadOnly Property NOMBREUSUARIO() As WhereParameter
                Get
                    If _NOMBREUSUARIO_W Is Nothing Then
                        _NOMBREUSUARIO_W = TearOff.NOMBREUSUARIO
                    End If
                    Return _NOMBREUSUARIO_W
                End Get
            End Property

            Public ReadOnly Property IDDIRECTORIOACTIVO() As WhereParameter
                Get
                    If _IDDIRECTORIOACTIVO_W Is Nothing Then
                        _IDDIRECTORIOACTIVO_W = TearOff.IDDIRECTORIOACTIVO
                    End If
                    Return _IDDIRECTORIOACTIVO_W
                End Get
            End Property

            Public ReadOnly Property CODPERSONA() As WhereParameter
                Get
                    If _CODPERSONA_W Is Nothing Then
                        _CODPERSONA_W = TearOff.CODPERSONA
                    End If
                    Return _CODPERSONA_W
                End Get
            End Property

            Public ReadOnly Property PWD() As WhereParameter
                Get
                    If _PWD_W Is Nothing Then
                        _PWD_W = TearOff.PWD
                    End If
                    Return _PWD_W
                End Get
            End Property

            Public ReadOnly Property FECHAALTA() As WhereParameter
                Get
                    If _FECHAALTA_W Is Nothing Then
                        _FECHAALTA_W = TearOff.FECHAALTA
                    End If
                    Return _FECHAALTA_W
                End Get
            End Property

            Public ReadOnly Property FECHABAJA() As WhereParameter
                Get
                    If _FECHABAJA_W Is Nothing Then
                        _FECHABAJA_W = TearOff.FECHABAJA
                    End If
                    Return _FECHABAJA_W
                End Get
            End Property

            Public ReadOnly Property IDFAPERSONAL() As WhereParameter
                Get
                    If _IDFAPERSONAL_W Is Nothing Then
                        _IDFAPERSONAL_W = TearOff.IDFAPERSONAL
                    End If
                    Return _IDFAPERSONAL_W
                End Get
            End Property

            Public ReadOnly Property IDRECURSOSHUMANOS() As WhereParameter
                Get
                    If _IDRECURSOSHUMANOS_W Is Nothing Then
                        _IDRECURSOSHUMANOS_W = TearOff.IDRECURSOSHUMANOS
                    End If
                    Return _IDRECURSOSHUMANOS_W
                End Get
            End Property

            Public ReadOnly Property IDMATRIX() As WhereParameter
                Get
                    If _IDMATRIX_W Is Nothing Then
                        _IDMATRIX_W = TearOff.IDMATRIX
                    End If
                    Return _IDMATRIX_W
                End Get
            End Property

            Public ReadOnly Property IDFTP() As WhereParameter
                Get
                    If _IDFTP_W Is Nothing Then
                        _IDFTP_W = TearOff.IDFTP
                    End If
                    Return _IDFTP_W
                End Get
            End Property

            Public ReadOnly Property EMAIL() As WhereParameter
                Get
                    If _EMAIL_W Is Nothing Then
                        _EMAIL_W = TearOff.EMAIL
                    End If
                    Return _EMAIL_W
                End Get
            End Property

            Private _ID_W As WhereParameter = Nothing
            Private _IDEMPRESAS_W As WhereParameter = Nothing
            Private _IDCULTURAS_W As WhereParameter = Nothing
            Private _NOMBREUSUARIO_W As WhereParameter = Nothing
            Private _IDDIRECTORIOACTIVO_W As WhereParameter = Nothing
            Private _CODPERSONA_W As WhereParameter = Nothing
            Private _PWD_W As WhereParameter = Nothing
            Private _FECHAALTA_W As WhereParameter = Nothing
            Private _FECHABAJA_W As WhereParameter = Nothing
            Private _IDFAPERSONAL_W As WhereParameter = Nothing
            Private _IDRECURSOSHUMANOS_W As WhereParameter = Nothing
            Private _IDMATRIX_W As WhereParameter = Nothing
            Private _IDFTP_W As WhereParameter = Nothing
            Private _EMAIL_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _ID_W = Nothing
                _IDEMPRESAS_W = Nothing
                _IDCULTURAS_W = Nothing
                _NOMBREUSUARIO_W = Nothing
                _IDDIRECTORIOACTIVO_W = Nothing
                _CODPERSONA_W = Nothing
                _PWD_W = Nothing
                _FECHAALTA_W = Nothing
                _FECHABAJA_W = Nothing
                _IDFAPERSONAL_W = Nothing
                _IDRECURSOSHUMANOS_W = Nothing
                _IDMATRIX_W = Nothing
                _IDFTP_W = Nothing
                _EMAIL_W = Nothing
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


                Public ReadOnly Property ID() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID, Parameters.ID)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDEMPRESAS() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDEMPRESAS, Parameters.IDEMPRESAS)
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

                Public ReadOnly Property NOMBREUSUARIO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NOMBREUSUARIO, Parameters.NOMBREUSUARIO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDDIRECTORIOACTIVO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDDIRECTORIOACTIVO, Parameters.IDDIRECTORIOACTIVO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODPERSONA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CODPERSONA, Parameters.CODPERSONA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PWD() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.PWD, Parameters.PWD)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECHAALTA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECHAALTA, Parameters.FECHAALTA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECHABAJA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECHABAJA, Parameters.FECHABAJA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDFAPERSONAL() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDFAPERSONAL, Parameters.IDFAPERSONAL)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDRECURSOSHUMANOS() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDRECURSOSHUMANOS, Parameters.IDRECURSOSHUMANOS)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDMATRIX() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDMATRIX, Parameters.IDMATRIX)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDFTP() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDFTP, Parameters.IDFTP)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property EMAIL() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.EMAIL, Parameters.EMAIL)
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

            Public ReadOnly Property IDEMPRESAS() As AggregateParameter
                Get
                    If _IDEMPRESAS_W Is Nothing Then
                        _IDEMPRESAS_W = TearOff.IDEMPRESAS
                    End If
                    Return _IDEMPRESAS_W
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

            Public ReadOnly Property NOMBREUSUARIO() As AggregateParameter
                Get
                    If _NOMBREUSUARIO_W Is Nothing Then
                        _NOMBREUSUARIO_W = TearOff.NOMBREUSUARIO
                    End If
                    Return _NOMBREUSUARIO_W
                End Get
            End Property

            Public ReadOnly Property IDDIRECTORIOACTIVO() As AggregateParameter
                Get
                    If _IDDIRECTORIOACTIVO_W Is Nothing Then
                        _IDDIRECTORIOACTIVO_W = TearOff.IDDIRECTORIOACTIVO
                    End If
                    Return _IDDIRECTORIOACTIVO_W
                End Get
            End Property

            Public ReadOnly Property CODPERSONA() As AggregateParameter
                Get
                    If _CODPERSONA_W Is Nothing Then
                        _CODPERSONA_W = TearOff.CODPERSONA
                    End If
                    Return _CODPERSONA_W
                End Get
            End Property

            Public ReadOnly Property PWD() As AggregateParameter
                Get
                    If _PWD_W Is Nothing Then
                        _PWD_W = TearOff.PWD
                    End If
                    Return _PWD_W
                End Get
            End Property

            Public ReadOnly Property FECHAALTA() As AggregateParameter
                Get
                    If _FECHAALTA_W Is Nothing Then
                        _FECHAALTA_W = TearOff.FECHAALTA
                    End If
                    Return _FECHAALTA_W
                End Get
            End Property

            Public ReadOnly Property FECHABAJA() As AggregateParameter
                Get
                    If _FECHABAJA_W Is Nothing Then
                        _FECHABAJA_W = TearOff.FECHABAJA
                    End If
                    Return _FECHABAJA_W
                End Get
            End Property

            Public ReadOnly Property IDFAPERSONAL() As AggregateParameter
                Get
                    If _IDFAPERSONAL_W Is Nothing Then
                        _IDFAPERSONAL_W = TearOff.IDFAPERSONAL
                    End If
                    Return _IDFAPERSONAL_W
                End Get
            End Property

            Public ReadOnly Property IDRECURSOSHUMANOS() As AggregateParameter
                Get
                    If _IDRECURSOSHUMANOS_W Is Nothing Then
                        _IDRECURSOSHUMANOS_W = TearOff.IDRECURSOSHUMANOS
                    End If
                    Return _IDRECURSOSHUMANOS_W
                End Get
            End Property

            Public ReadOnly Property IDMATRIX() As AggregateParameter
                Get
                    If _IDMATRIX_W Is Nothing Then
                        _IDMATRIX_W = TearOff.IDMATRIX
                    End If
                    Return _IDMATRIX_W
                End Get
            End Property

            Public ReadOnly Property IDFTP() As AggregateParameter
                Get
                    If _IDFTP_W Is Nothing Then
                        _IDFTP_W = TearOff.IDFTP
                    End If
                    Return _IDFTP_W
                End Get
            End Property

            Public ReadOnly Property EMAIL() As AggregateParameter
                Get
                    If _EMAIL_W Is Nothing Then
                        _EMAIL_W = TearOff.EMAIL
                    End If
                    Return _EMAIL_W
                End Get
            End Property

            Private _ID_W As AggregateParameter = Nothing
            Private _IDEMPRESAS_W As AggregateParameter = Nothing
            Private _IDCULTURAS_W As AggregateParameter = Nothing
            Private _NOMBREUSUARIO_W As AggregateParameter = Nothing
            Private _IDDIRECTORIOACTIVO_W As AggregateParameter = Nothing
            Private _CODPERSONA_W As AggregateParameter = Nothing
            Private _PWD_W As AggregateParameter = Nothing
            Private _FECHAALTA_W As AggregateParameter = Nothing
            Private _FECHABAJA_W As AggregateParameter = Nothing
            Private _IDFAPERSONAL_W As AggregateParameter = Nothing
            Private _IDRECURSOSHUMANOS_W As AggregateParameter = Nothing
            Private _IDMATRIX_W As AggregateParameter = Nothing
            Private _IDFTP_W As AggregateParameter = Nothing
            Private _EMAIL_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _ID_W = Nothing
                _IDEMPRESAS_W = Nothing
                _IDCULTURAS_W = Nothing
                _NOMBREUSUARIO_W = Nothing
                _IDDIRECTORIOACTIVO_W = Nothing
                _CODPERSONA_W = Nothing
                _PWD_W = Nothing
                _FECHAALTA_W = Nothing
                _FECHABAJA_W = Nothing
                _IDFAPERSONAL_W = Nothing
                _IDRECURSOSHUMANOS_W = Nothing
                _IDMATRIX_W = Nothing
                _IDFTP_W = Nothing
                _EMAIL_W = Nothing
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
            Dim p As OracleParameter
            p = cmd.Parameters(Parameters.ID.ParameterName)
            p.Direction = ParameterDirection.Output
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

            p = cmd.Parameters.Add(Parameters.IDEMPRESAS)
            p.SourceColumn = ColumnNames.IDEMPRESAS
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDCULTURAS)
            p.SourceColumn = ColumnNames.IDCULTURAS
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NOMBREUSUARIO)
            p.SourceColumn = ColumnNames.NOMBREUSUARIO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDDIRECTORIOACTIVO)
            p.SourceColumn = ColumnNames.IDDIRECTORIOACTIVO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CODPERSONA)
            p.SourceColumn = ColumnNames.CODPERSONA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.PWD)
            p.SourceColumn = ColumnNames.PWD
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.FECHAALTA)
            p.SourceColumn = ColumnNames.FECHAALTA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.FECHABAJA)
            p.SourceColumn = ColumnNames.FECHABAJA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDFAPERSONAL)
            p.SourceColumn = ColumnNames.IDFAPERSONAL
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDRECURSOSHUMANOS)
            p.SourceColumn = ColumnNames.IDRECURSOSHUMANOS
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDMATRIX)
            p.SourceColumn = ColumnNames.IDMATRIX
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDFTP)
            p.SourceColumn = ColumnNames.IDFTP
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.EMAIL)
            p.SourceColumn = ColumnNames.EMAIL
            p.SourceVersion = DataRowVersion.Current


        End Sub

    End Class

End Namespace

