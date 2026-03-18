
'===============================================================================
'BATZ, Koop. - 01/12/2008 8:21:29
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

    Public MustInherit Class _EMPRESAS
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "SAB."
            Me.QuerySource = "EMPRESAS"
            Me.MappingName = "EMPRESAS"
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


            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_EMPRESAS", parameters)
        End Function

        '=================================================================
        ' Public Overridable Function LoadByPrimaryKey()  As Boolean
        '=================================================================
        '  Loads a single row of via the primary key
        '=================================================================
        Public Overridable Function LoadByPrimaryKey(ByVal ID As Decimal) As Boolean

            Dim parameters As ListDictionary = New ListDictionary()
            parameters.Add(_EMPRESAS.Parameters.ID, ID)


            Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
            p.Direction = ParameterDirection.Output
            parameters.Add(p, Nothing)

            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_EMPRESAS", parameters)

        End Function

#Region "Parameters"
        Protected Class Parameters

            Public Shared ReadOnly Property ID As OracleParameter
                Get
                    Return New OracleParameter("p_ID", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property NOMBRE As OracleParameter
                Get
                    Return New OracleParameter("p_NOMBRE", OracleDbType.Varchar2, 100)
                End Get
            End Property

            Public Shared ReadOnly Property DIRECCION As OracleParameter
                Get
                    Return New OracleParameter("p_DIRECCION", OracleDbType.Varchar2, 250)
                End Get
            End Property

            Public Shared ReadOnly Property CIF As OracleParameter
                Get
                    Return New OracleParameter("p_CIF", OracleDbType.Varchar2, 15)
                End Get
            End Property

            Public Shared ReadOnly Property TELEFONO As OracleParameter
                Get
                    Return New OracleParameter("p_TELEFONO", OracleDbType.Varchar2, 20)
                End Get
            End Property

            Public Shared ReadOnly Property IDTROQUELERIA As OracleParameter
                Get
                    Return New OracleParameter("p_IDTROQUELERIA", OracleDbType.Varchar2, 12)
                End Get
            End Property

            Public Shared ReadOnly Property IDSISTEMAS As OracleParameter
                Get
                    Return New OracleParameter("p_IDSISTEMAS", OracleDbType.Varchar2, 12)
                End Get
            End Property

            Public Shared ReadOnly Property FECHAALTA As OracleParameter
                Get
                    Return New OracleParameter("p_FECHAALTA", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property FECHABAJA As OracleParameter
                Get
                    Return New OracleParameter("p_FECHABAJA", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property CPOSTAL As OracleParameter
                Get
                    Return New OracleParameter("p_CPOSTAL", OracleDbType.Varchar2, 20)
                End Get
            End Property

            Public Shared ReadOnly Property LOCALIDAD As OracleParameter
                Get
                    Return New OracleParameter("p_LOCALIDAD", OracleDbType.Varchar2, 50)
                End Get
            End Property

            Public Shared ReadOnly Property PROVINCIA As OracleParameter
                Get
                    Return New OracleParameter("p_PROVINCIA", OracleDbType.Varchar2, 50)
                End Get
            End Property

            Public Shared ReadOnly Property ID_DOMICI As OracleParameter
                Get
                    Return New OracleParameter("p_ID_DOMICI", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property ID_FPAGO As OracleParameter
                Get
                    Return New OracleParameter("p_ID_FPAGO", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property ID_PAIS As OracleParameter
                Get
                    Return New OracleParameter("p_ID_PAIS", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property CONTACTO As OracleParameter
                Get
                    Return New OracleParameter("p_CONTACTO", OracleDbType.Varchar2, 50)
                End Get
            End Property

            Public Shared ReadOnly Property NOTIFICADO As OracleParameter
                Get
                    Return New OracleParameter("p_NOTIFICADO", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property FAX As OracleParameter
                Get
                    Return New OracleParameter("p_FAX", OracleDbType.Varchar2, 25)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const ID As String = "ID"
            Public Const NOMBRE As String = "NOMBRE"
            Public Const DIRECCION As String = "DIRECCION"
            Public Const CIF As String = "CIF"
            Public Const TELEFONO As String = "TELEFONO"
            Public Const IDTROQUELERIA As String = "IDTROQUELERIA"
            Public Const IDSISTEMAS As String = "IDSISTEMAS"
            Public Const FECHAALTA As String = "FECHAALTA"
            Public Const FECHABAJA As String = "FECHABAJA"
            Public Const CPOSTAL As String = "CPOSTAL"
            Public Const LOCALIDAD As String = "LOCALIDAD"
            Public Const PROVINCIA As String = "PROVINCIA"
            Public Const ID_DOMICI As String = "ID_DOMICI"
            Public Const ID_FPAGO As String = "ID_FPAGO"
            Public Const ID_PAIS As String = "ID_PAIS"
            Public Const CONTACTO As String = "CONTACTO"
            Public Const NOTIFICADO As String = "NOTIFICADO"
            Public Const FAX As String = "FAX"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(ID) = _EMPRESAS.PropertyNames.ID
                    ht(NOMBRE) = _EMPRESAS.PropertyNames.NOMBRE
                    ht(DIRECCION) = _EMPRESAS.PropertyNames.DIRECCION
                    ht(CIF) = _EMPRESAS.PropertyNames.CIF
                    ht(TELEFONO) = _EMPRESAS.PropertyNames.TELEFONO
                    ht(IDTROQUELERIA) = _EMPRESAS.PropertyNames.IDTROQUELERIA
                    ht(IDSISTEMAS) = _EMPRESAS.PropertyNames.IDSISTEMAS
                    ht(FECHAALTA) = _EMPRESAS.PropertyNames.FECHAALTA
                    ht(FECHABAJA) = _EMPRESAS.PropertyNames.FECHABAJA
                    ht(CPOSTAL) = _EMPRESAS.PropertyNames.CPOSTAL
                    ht(LOCALIDAD) = _EMPRESAS.PropertyNames.LOCALIDAD
                    ht(PROVINCIA) = _EMPRESAS.PropertyNames.PROVINCIA
                    ht(ID_DOMICI) = _EMPRESAS.PropertyNames.ID_DOMICI
                    ht(ID_FPAGO) = _EMPRESAS.PropertyNames.ID_FPAGO
                    ht(ID_PAIS) = _EMPRESAS.PropertyNames.ID_PAIS
                    ht(CONTACTO) = _EMPRESAS.PropertyNames.CONTACTO
                    ht(NOTIFICADO) = _EMPRESAS.PropertyNames.NOTIFICADO
                    ht(FAX) = _EMPRESAS.PropertyNames.FAX

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const ID As String = "ID"
            Public Const NOMBRE As String = "NOMBRE"
            Public Const DIRECCION As String = "DIRECCION"
            Public Const CIF As String = "CIF"
            Public Const TELEFONO As String = "TELEFONO"
            Public Const IDTROQUELERIA As String = "IDTROQUELERIA"
            Public Const IDSISTEMAS As String = "IDSISTEMAS"
            Public Const FECHAALTA As String = "FECHAALTA"
            Public Const FECHABAJA As String = "FECHABAJA"
            Public Const CPOSTAL As String = "CPOSTAL"
            Public Const LOCALIDAD As String = "LOCALIDAD"
            Public Const PROVINCIA As String = "PROVINCIA"
            Public Const ID_DOMICI As String = "ID_DOMICI"
            Public Const ID_FPAGO As String = "ID_FPAGO"
            Public Const ID_PAIS As String = "ID_PAIS"
            Public Const CONTACTO As String = "CONTACTO"
            Public Const NOTIFICADO As String = "NOTIFICADO"
            Public Const FAX As String = "FAX"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(ID) = _EMPRESAS.ColumnNames.ID
                    ht(NOMBRE) = _EMPRESAS.ColumnNames.NOMBRE
                    ht(DIRECCION) = _EMPRESAS.ColumnNames.DIRECCION
                    ht(CIF) = _EMPRESAS.ColumnNames.CIF
                    ht(TELEFONO) = _EMPRESAS.ColumnNames.TELEFONO
                    ht(IDTROQUELERIA) = _EMPRESAS.ColumnNames.IDTROQUELERIA
                    ht(IDSISTEMAS) = _EMPRESAS.ColumnNames.IDSISTEMAS
                    ht(FECHAALTA) = _EMPRESAS.ColumnNames.FECHAALTA
                    ht(FECHABAJA) = _EMPRESAS.ColumnNames.FECHABAJA
                    ht(CPOSTAL) = _EMPRESAS.ColumnNames.CPOSTAL
                    ht(LOCALIDAD) = _EMPRESAS.ColumnNames.LOCALIDAD
                    ht(PROVINCIA) = _EMPRESAS.ColumnNames.PROVINCIA
                    ht(ID_DOMICI) = _EMPRESAS.ColumnNames.ID_DOMICI
                    ht(ID_FPAGO) = _EMPRESAS.ColumnNames.ID_FPAGO
                    ht(ID_PAIS) = _EMPRESAS.ColumnNames.ID_PAIS
                    ht(CONTACTO) = _EMPRESAS.ColumnNames.CONTACTO
                    ht(NOTIFICADO) = _EMPRESAS.ColumnNames.NOTIFICADO
                    ht(FAX) = _EMPRESAS.ColumnNames.FAX

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const ID As String = "s_ID"
            Public Const NOMBRE As String = "s_NOMBRE"
            Public Const DIRECCION As String = "s_DIRECCION"
            Public Const CIF As String = "s_CIF"
            Public Const TELEFONO As String = "s_TELEFONO"
            Public Const IDTROQUELERIA As String = "s_IDTROQUELERIA"
            Public Const IDSISTEMAS As String = "s_IDSISTEMAS"
            Public Const FECHAALTA As String = "s_FECHAALTA"
            Public Const FECHABAJA As String = "s_FECHABAJA"
            Public Const CPOSTAL As String = "s_CPOSTAL"
            Public Const LOCALIDAD As String = "s_LOCALIDAD"
            Public Const PROVINCIA As String = "s_PROVINCIA"
            Public Const ID_DOMICI As String = "s_ID_DOMICI"
            Public Const ID_FPAGO As String = "s_ID_FPAGO"
            Public Const ID_PAIS As String = "s_ID_PAIS"
            Public Const CONTACTO As String = "s_CONTACTO"
            Public Const NOTIFICADO As String = "s_NOTIFICADO"
            Public Const FAX As String = "s_FAX"

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

        Public Overridable Property NOMBRE As String
            Get
                Return MyBase.GetString(ColumnNames.NOMBRE)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.NOMBRE, Value)
            End Set
        End Property

        Public Overridable Property DIRECCION As String
            Get
                Return MyBase.GetString(ColumnNames.DIRECCION)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.DIRECCION, Value)
            End Set
        End Property

        Public Overridable Property CIF As String
            Get
                Return MyBase.GetString(ColumnNames.CIF)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CIF, Value)
            End Set
        End Property

        Public Overridable Property TELEFONO As String
            Get
                Return MyBase.GetString(ColumnNames.TELEFONO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.TELEFONO, Value)
            End Set
        End Property

        Public Overridable Property IDTROQUELERIA As String
            Get
                Return MyBase.GetString(ColumnNames.IDTROQUELERIA)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.IDTROQUELERIA, Value)
            End Set
        End Property

        Public Overridable Property IDSISTEMAS As String
            Get
                Return MyBase.GetString(ColumnNames.IDSISTEMAS)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.IDSISTEMAS, Value)
            End Set
        End Property

        Public Overridable Property FECHAALTA As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECHAALTA)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECHAALTA, Value)
            End Set
        End Property

        Public Overridable Property FECHABAJA As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECHABAJA)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECHABAJA, Value)
            End Set
        End Property

        Public Overridable Property CPOSTAL As String
            Get
                Return MyBase.GetString(ColumnNames.CPOSTAL)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CPOSTAL, Value)
            End Set
        End Property

        Public Overridable Property LOCALIDAD As String
            Get
                Return MyBase.GetString(ColumnNames.LOCALIDAD)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.LOCALIDAD, Value)
            End Set
        End Property

        Public Overridable Property PROVINCIA As String
            Get
                Return MyBase.GetString(ColumnNames.PROVINCIA)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.PROVINCIA, Value)
            End Set
        End Property

        Public Overridable Property ID_DOMICI As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.ID_DOMICI)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.ID_DOMICI, Value)
            End Set
        End Property

        Public Overridable Property ID_FPAGO As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.ID_FPAGO)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.ID_FPAGO, Value)
            End Set
        End Property

        Public Overridable Property ID_PAIS As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.ID_PAIS)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.ID_PAIS, Value)
            End Set
        End Property

        Public Overridable Property CONTACTO As String
            Get
                Return MyBase.GetString(ColumnNames.CONTACTO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CONTACTO, Value)
            End Set
        End Property

        Public Overridable Property NOTIFICADO As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.NOTIFICADO)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.NOTIFICADO, Value)
            End Set
        End Property

        Public Overridable Property FAX As String
            Get
                Return MyBase.GetString(ColumnNames.FAX)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.FAX, Value)
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

        Public Overridable Property s_NOMBRE As String
            Get
                If Me.IsColumnNull(ColumnNames.NOMBRE) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.NOMBRE)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NOMBRE)
                Else
                    Me.NOMBRE = MyBase.SetStringAsString(ColumnNames.NOMBRE, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_DIRECCION As String
            Get
                If Me.IsColumnNull(ColumnNames.DIRECCION) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.DIRECCION)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.DIRECCION)
                Else
                    Me.DIRECCION = MyBase.SetStringAsString(ColumnNames.DIRECCION, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CIF As String
            Get
                If Me.IsColumnNull(ColumnNames.CIF) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CIF)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CIF)
                Else
                    Me.CIF = MyBase.SetStringAsString(ColumnNames.CIF, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_TELEFONO As String
            Get
                If Me.IsColumnNull(ColumnNames.TELEFONO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.TELEFONO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.TELEFONO)
                Else
                    Me.TELEFONO = MyBase.SetStringAsString(ColumnNames.TELEFONO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_IDTROQUELERIA As String
            Get
                If Me.IsColumnNull(ColumnNames.IDTROQUELERIA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.IDTROQUELERIA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDTROQUELERIA)
                Else
                    Me.IDTROQUELERIA = MyBase.SetStringAsString(ColumnNames.IDTROQUELERIA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_IDSISTEMAS As String
            Get
                If Me.IsColumnNull(ColumnNames.IDSISTEMAS) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.IDSISTEMAS)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDSISTEMAS)
                Else
                    Me.IDSISTEMAS = MyBase.SetStringAsString(ColumnNames.IDSISTEMAS, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FECHAALTA As String
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

        Public Overridable Property s_FECHABAJA As String
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

        Public Overridable Property s_CPOSTAL As String
            Get
                If Me.IsColumnNull(ColumnNames.CPOSTAL) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CPOSTAL)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CPOSTAL)
                Else
                    Me.CPOSTAL = MyBase.SetStringAsString(ColumnNames.CPOSTAL, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_LOCALIDAD As String
            Get
                If Me.IsColumnNull(ColumnNames.LOCALIDAD) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.LOCALIDAD)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.LOCALIDAD)
                Else
                    Me.LOCALIDAD = MyBase.SetStringAsString(ColumnNames.LOCALIDAD, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_PROVINCIA As String
            Get
                If Me.IsColumnNull(ColumnNames.PROVINCIA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.PROVINCIA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.PROVINCIA)
                Else
                    Me.PROVINCIA = MyBase.SetStringAsString(ColumnNames.PROVINCIA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_ID_DOMICI As String
            Get
                If Me.IsColumnNull(ColumnNames.ID_DOMICI) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.ID_DOMICI)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.ID_DOMICI)
                Else
                    Me.ID_DOMICI = MyBase.SetDecimalAsString(ColumnNames.ID_DOMICI, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_ID_FPAGO As String
            Get
                If Me.IsColumnNull(ColumnNames.ID_FPAGO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.ID_FPAGO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.ID_FPAGO)
                Else
                    Me.ID_FPAGO = MyBase.SetDecimalAsString(ColumnNames.ID_FPAGO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_ID_PAIS As String
            Get
                If Me.IsColumnNull(ColumnNames.ID_PAIS) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.ID_PAIS)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.ID_PAIS)
                Else
                    Me.ID_PAIS = MyBase.SetDecimalAsString(ColumnNames.ID_PAIS, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CONTACTO As String
            Get
                If Me.IsColumnNull(ColumnNames.CONTACTO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CONTACTO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CONTACTO)
                Else
                    Me.CONTACTO = MyBase.SetStringAsString(ColumnNames.CONTACTO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_NOTIFICADO As String
            Get
                If Me.IsColumnNull(ColumnNames.NOTIFICADO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.NOTIFICADO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NOTIFICADO)
                Else
                    Me.NOTIFICADO = MyBase.SetDecimalAsString(ColumnNames.NOTIFICADO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FAX As String
            Get
                If Me.IsColumnNull(ColumnNames.FAX) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.FAX)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FAX)
                Else
                    Me.FAX = MyBase.SetStringAsString(ColumnNames.FAX, Value)
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

                Public ReadOnly Property NOMBRE() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NOMBRE, Parameters.NOMBRE)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DIRECCION() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.DIRECCION, Parameters.DIRECCION)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CIF() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CIF, Parameters.CIF)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TELEFONO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.TELEFONO, Parameters.TELEFONO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDTROQUELERIA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDTROQUELERIA, Parameters.IDTROQUELERIA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDSISTEMAS() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDSISTEMAS, Parameters.IDSISTEMAS)
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

                Public ReadOnly Property CPOSTAL() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CPOSTAL, Parameters.CPOSTAL)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property LOCALIDAD() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.LOCALIDAD, Parameters.LOCALIDAD)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PROVINCIA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.PROVINCIA, Parameters.PROVINCIA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ID_DOMICI() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_DOMICI, Parameters.ID_DOMICI)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ID_FPAGO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_FPAGO, Parameters.ID_FPAGO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ID_PAIS() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_PAIS, Parameters.ID_PAIS)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CONTACTO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CONTACTO, Parameters.CONTACTO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NOTIFICADO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NOTIFICADO, Parameters.NOTIFICADO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FAX() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FAX, Parameters.FAX)
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

            Public ReadOnly Property NOMBRE() As WhereParameter
                Get
                    If _NOMBRE_W Is Nothing Then
                        _NOMBRE_W = TearOff.NOMBRE
                    End If
                    Return _NOMBRE_W
                End Get
            End Property

            Public ReadOnly Property DIRECCION() As WhereParameter
                Get
                    If _DIRECCION_W Is Nothing Then
                        _DIRECCION_W = TearOff.DIRECCION
                    End If
                    Return _DIRECCION_W
                End Get
            End Property

            Public ReadOnly Property CIF() As WhereParameter
                Get
                    If _CIF_W Is Nothing Then
                        _CIF_W = TearOff.CIF
                    End If
                    Return _CIF_W
                End Get
            End Property

            Public ReadOnly Property TELEFONO() As WhereParameter
                Get
                    If _TELEFONO_W Is Nothing Then
                        _TELEFONO_W = TearOff.TELEFONO
                    End If
                    Return _TELEFONO_W
                End Get
            End Property

            Public ReadOnly Property IDTROQUELERIA() As WhereParameter
                Get
                    If _IDTROQUELERIA_W Is Nothing Then
                        _IDTROQUELERIA_W = TearOff.IDTROQUELERIA
                    End If
                    Return _IDTROQUELERIA_W
                End Get
            End Property

            Public ReadOnly Property IDSISTEMAS() As WhereParameter
                Get
                    If _IDSISTEMAS_W Is Nothing Then
                        _IDSISTEMAS_W = TearOff.IDSISTEMAS
                    End If
                    Return _IDSISTEMAS_W
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

            Public ReadOnly Property CPOSTAL() As WhereParameter
                Get
                    If _CPOSTAL_W Is Nothing Then
                        _CPOSTAL_W = TearOff.CPOSTAL
                    End If
                    Return _CPOSTAL_W
                End Get
            End Property

            Public ReadOnly Property LOCALIDAD() As WhereParameter
                Get
                    If _LOCALIDAD_W Is Nothing Then
                        _LOCALIDAD_W = TearOff.LOCALIDAD
                    End If
                    Return _LOCALIDAD_W
                End Get
            End Property

            Public ReadOnly Property PROVINCIA() As WhereParameter
                Get
                    If _PROVINCIA_W Is Nothing Then
                        _PROVINCIA_W = TearOff.PROVINCIA
                    End If
                    Return _PROVINCIA_W
                End Get
            End Property

            Public ReadOnly Property ID_DOMICI() As WhereParameter
                Get
                    If _ID_DOMICI_W Is Nothing Then
                        _ID_DOMICI_W = TearOff.ID_DOMICI
                    End If
                    Return _ID_DOMICI_W
                End Get
            End Property

            Public ReadOnly Property ID_FPAGO() As WhereParameter
                Get
                    If _ID_FPAGO_W Is Nothing Then
                        _ID_FPAGO_W = TearOff.ID_FPAGO
                    End If
                    Return _ID_FPAGO_W
                End Get
            End Property

            Public ReadOnly Property ID_PAIS() As WhereParameter
                Get
                    If _ID_PAIS_W Is Nothing Then
                        _ID_PAIS_W = TearOff.ID_PAIS
                    End If
                    Return _ID_PAIS_W
                End Get
            End Property

            Public ReadOnly Property CONTACTO() As WhereParameter
                Get
                    If _CONTACTO_W Is Nothing Then
                        _CONTACTO_W = TearOff.CONTACTO
                    End If
                    Return _CONTACTO_W
                End Get
            End Property

            Public ReadOnly Property NOTIFICADO() As WhereParameter
                Get
                    If _NOTIFICADO_W Is Nothing Then
                        _NOTIFICADO_W = TearOff.NOTIFICADO
                    End If
                    Return _NOTIFICADO_W
                End Get
            End Property

            Public ReadOnly Property FAX() As WhereParameter
                Get
                    If _FAX_W Is Nothing Then
                        _FAX_W = TearOff.FAX
                    End If
                    Return _FAX_W
                End Get
            End Property

            Private _ID_W As WhereParameter = Nothing
            Private _NOMBRE_W As WhereParameter = Nothing
            Private _DIRECCION_W As WhereParameter = Nothing
            Private _CIF_W As WhereParameter = Nothing
            Private _TELEFONO_W As WhereParameter = Nothing
            Private _IDTROQUELERIA_W As WhereParameter = Nothing
            Private _IDSISTEMAS_W As WhereParameter = Nothing
            Private _FECHAALTA_W As WhereParameter = Nothing
            Private _FECHABAJA_W As WhereParameter = Nothing
            Private _CPOSTAL_W As WhereParameter = Nothing
            Private _LOCALIDAD_W As WhereParameter = Nothing
            Private _PROVINCIA_W As WhereParameter = Nothing
            Private _ID_DOMICI_W As WhereParameter = Nothing
            Private _ID_FPAGO_W As WhereParameter = Nothing
            Private _ID_PAIS_W As WhereParameter = Nothing
            Private _CONTACTO_W As WhereParameter = Nothing
            Private _NOTIFICADO_W As WhereParameter = Nothing
            Private _FAX_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _ID_W = Nothing
                _NOMBRE_W = Nothing
                _DIRECCION_W = Nothing
                _CIF_W = Nothing
                _TELEFONO_W = Nothing
                _IDTROQUELERIA_W = Nothing
                _IDSISTEMAS_W = Nothing
                _FECHAALTA_W = Nothing
                _FECHABAJA_W = Nothing
                _CPOSTAL_W = Nothing
                _LOCALIDAD_W = Nothing
                _PROVINCIA_W = Nothing
                _ID_DOMICI_W = Nothing
                _ID_FPAGO_W = Nothing
                _ID_PAIS_W = Nothing
                _CONTACTO_W = Nothing
                _NOTIFICADO_W = Nothing
                _FAX_W = Nothing
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

                Public ReadOnly Property NOMBRE() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NOMBRE, Parameters.NOMBRE)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DIRECCION() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DIRECCION, Parameters.DIRECCION)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CIF() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CIF, Parameters.CIF)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TELEFONO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TELEFONO, Parameters.TELEFONO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDTROQUELERIA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDTROQUELERIA, Parameters.IDTROQUELERIA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDSISTEMAS() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDSISTEMAS, Parameters.IDSISTEMAS)
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

                Public ReadOnly Property CPOSTAL() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CPOSTAL, Parameters.CPOSTAL)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property LOCALIDAD() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.LOCALIDAD, Parameters.LOCALIDAD)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PROVINCIA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.PROVINCIA, Parameters.PROVINCIA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ID_DOMICI() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_DOMICI, Parameters.ID_DOMICI)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ID_FPAGO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_FPAGO, Parameters.ID_FPAGO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ID_PAIS() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_PAIS, Parameters.ID_PAIS)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CONTACTO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CONTACTO, Parameters.CONTACTO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NOTIFICADO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NOTIFICADO, Parameters.NOTIFICADO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FAX() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FAX, Parameters.FAX)
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

            Public ReadOnly Property NOMBRE() As AggregateParameter
                Get
                    If _NOMBRE_W Is Nothing Then
                        _NOMBRE_W = TearOff.NOMBRE
                    End If
                    Return _NOMBRE_W
                End Get
            End Property

            Public ReadOnly Property DIRECCION() As AggregateParameter
                Get
                    If _DIRECCION_W Is Nothing Then
                        _DIRECCION_W = TearOff.DIRECCION
                    End If
                    Return _DIRECCION_W
                End Get
            End Property

            Public ReadOnly Property CIF() As AggregateParameter
                Get
                    If _CIF_W Is Nothing Then
                        _CIF_W = TearOff.CIF
                    End If
                    Return _CIF_W
                End Get
            End Property

            Public ReadOnly Property TELEFONO() As AggregateParameter
                Get
                    If _TELEFONO_W Is Nothing Then
                        _TELEFONO_W = TearOff.TELEFONO
                    End If
                    Return _TELEFONO_W
                End Get
            End Property

            Public ReadOnly Property IDTROQUELERIA() As AggregateParameter
                Get
                    If _IDTROQUELERIA_W Is Nothing Then
                        _IDTROQUELERIA_W = TearOff.IDTROQUELERIA
                    End If
                    Return _IDTROQUELERIA_W
                End Get
            End Property

            Public ReadOnly Property IDSISTEMAS() As AggregateParameter
                Get
                    If _IDSISTEMAS_W Is Nothing Then
                        _IDSISTEMAS_W = TearOff.IDSISTEMAS
                    End If
                    Return _IDSISTEMAS_W
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

            Public ReadOnly Property CPOSTAL() As AggregateParameter
                Get
                    If _CPOSTAL_W Is Nothing Then
                        _CPOSTAL_W = TearOff.CPOSTAL
                    End If
                    Return _CPOSTAL_W
                End Get
            End Property

            Public ReadOnly Property LOCALIDAD() As AggregateParameter
                Get
                    If _LOCALIDAD_W Is Nothing Then
                        _LOCALIDAD_W = TearOff.LOCALIDAD
                    End If
                    Return _LOCALIDAD_W
                End Get
            End Property

            Public ReadOnly Property PROVINCIA() As AggregateParameter
                Get
                    If _PROVINCIA_W Is Nothing Then
                        _PROVINCIA_W = TearOff.PROVINCIA
                    End If
                    Return _PROVINCIA_W
                End Get
            End Property

            Public ReadOnly Property ID_DOMICI() As AggregateParameter
                Get
                    If _ID_DOMICI_W Is Nothing Then
                        _ID_DOMICI_W = TearOff.ID_DOMICI
                    End If
                    Return _ID_DOMICI_W
                End Get
            End Property

            Public ReadOnly Property ID_FPAGO() As AggregateParameter
                Get
                    If _ID_FPAGO_W Is Nothing Then
                        _ID_FPAGO_W = TearOff.ID_FPAGO
                    End If
                    Return _ID_FPAGO_W
                End Get
            End Property

            Public ReadOnly Property ID_PAIS() As AggregateParameter
                Get
                    If _ID_PAIS_W Is Nothing Then
                        _ID_PAIS_W = TearOff.ID_PAIS
                    End If
                    Return _ID_PAIS_W
                End Get
            End Property

            Public ReadOnly Property CONTACTO() As AggregateParameter
                Get
                    If _CONTACTO_W Is Nothing Then
                        _CONTACTO_W = TearOff.CONTACTO
                    End If
                    Return _CONTACTO_W
                End Get
            End Property

            Public ReadOnly Property NOTIFICADO() As AggregateParameter
                Get
                    If _NOTIFICADO_W Is Nothing Then
                        _NOTIFICADO_W = TearOff.NOTIFICADO
                    End If
                    Return _NOTIFICADO_W
                End Get
            End Property

            Public ReadOnly Property FAX() As AggregateParameter
                Get
                    If _FAX_W Is Nothing Then
                        _FAX_W = TearOff.FAX
                    End If
                    Return _FAX_W
                End Get
            End Property

            Private _ID_W As AggregateParameter = Nothing
            Private _NOMBRE_W As AggregateParameter = Nothing
            Private _DIRECCION_W As AggregateParameter = Nothing
            Private _CIF_W As AggregateParameter = Nothing
            Private _TELEFONO_W As AggregateParameter = Nothing
            Private _IDTROQUELERIA_W As AggregateParameter = Nothing
            Private _IDSISTEMAS_W As AggregateParameter = Nothing
            Private _FECHAALTA_W As AggregateParameter = Nothing
            Private _FECHABAJA_W As AggregateParameter = Nothing
            Private _CPOSTAL_W As AggregateParameter = Nothing
            Private _LOCALIDAD_W As AggregateParameter = Nothing
            Private _PROVINCIA_W As AggregateParameter = Nothing
            Private _ID_DOMICI_W As AggregateParameter = Nothing
            Private _ID_FPAGO_W As AggregateParameter = Nothing
            Private _ID_PAIS_W As AggregateParameter = Nothing
            Private _CONTACTO_W As AggregateParameter = Nothing
            Private _NOTIFICADO_W As AggregateParameter = Nothing
            Private _FAX_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _ID_W = Nothing
                _NOMBRE_W = Nothing
                _DIRECCION_W = Nothing
                _CIF_W = Nothing
                _TELEFONO_W = Nothing
                _IDTROQUELERIA_W = Nothing
                _IDSISTEMAS_W = Nothing
                _FECHAALTA_W = Nothing
                _FECHABAJA_W = Nothing
                _CPOSTAL_W = Nothing
                _LOCALIDAD_W = Nothing
                _PROVINCIA_W = Nothing
                _ID_DOMICI_W = Nothing
                _ID_FPAGO_W = Nothing
                _ID_PAIS_W = Nothing
                _CONTACTO_W = Nothing
                _NOTIFICADO_W = Nothing
                _FAX_W = Nothing
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
            cmd.CommandText = Me.SchemaStoredProcedure + "PI_EMPRESAS"

            CreateParameters(cmd)


            Return cmd

        End Function

        Protected Overrides Function GetUpdateCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PU_EMPRESAS"

            CreateParameters(cmd)

            Return cmd

        End Function

        Protected Overrides Function GetDeleteCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PD_EMPRESAS"

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

            p = cmd.Parameters.Add(Parameters.NOMBRE)
            p.SourceColumn = ColumnNames.NOMBRE
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.DIRECCION)
            p.SourceColumn = ColumnNames.DIRECCION
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CIF)
            p.SourceColumn = ColumnNames.CIF
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.TELEFONO)
            p.SourceColumn = ColumnNames.TELEFONO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDTROQUELERIA)
            p.SourceColumn = ColumnNames.IDTROQUELERIA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDSISTEMAS)
            p.SourceColumn = ColumnNames.IDSISTEMAS
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.FECHAALTA)
            p.SourceColumn = ColumnNames.FECHAALTA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.FECHABAJA)
            p.SourceColumn = ColumnNames.FECHABAJA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CPOSTAL)
            p.SourceColumn = ColumnNames.CPOSTAL
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.LOCALIDAD)
            p.SourceColumn = ColumnNames.LOCALIDAD
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.PROVINCIA)
            p.SourceColumn = ColumnNames.PROVINCIA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.ID_DOMICI)
            p.SourceColumn = ColumnNames.ID_DOMICI
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.ID_FPAGO)
            p.SourceColumn = ColumnNames.ID_FPAGO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.ID_PAIS)
            p.SourceColumn = ColumnNames.ID_PAIS
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CONTACTO)
            p.SourceColumn = ColumnNames.CONTACTO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NOTIFICADO)
            p.SourceColumn = ColumnNames.NOTIFICADO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.FAX)
            p.SourceColumn = ColumnNames.FAX
            p.SourceVersion = DataRowVersion.Current


        End Sub

    End Class

End Namespace

