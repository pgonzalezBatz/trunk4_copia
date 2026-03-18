
'===============================================================================
'BATZ, Koop. - 14/09/2010 8:51:03
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

    Public MustInherit Class _GERTAKARIAK
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "INCIDENCIAS."
            Me.QuerySource = "GERTAKARIAK"
            Me.MappingName = "GERTAKARIAK"
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


            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_GERTAKARIAK", parameters)
        End Function

        '=================================================================
        ' Public Overridable Function LoadByPrimaryKey()  As Boolean
        '=================================================================
        '  Loads a single row of via the primary key
        '=================================================================
        Public Overridable Function LoadByPrimaryKey(ByVal ID As Decimal) As Boolean

            Dim parameters As ListDictionary = New ListDictionary()
            parameters.Add(_GERTAKARIAK.Parameters.ID, ID)


            Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
            p.Direction = ParameterDirection.Output
            parameters.Add(p, Nothing)

            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_GERTAKARIAK", parameters)

        End Function

#Region "Parameters"
        Protected Class Parameters

            Public Shared ReadOnly Property ID As OracleParameter
                Get
                    Return New OracleParameter("p_ID", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property IDREPETITIVA As OracleParameter
                Get
                    Return New OracleParameter("p_IDREPETITIVA", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property FECHAAPERTURA As OracleParameter
                Get
                    Return New OracleParameter("p_FECHAAPERTURA", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property FECHACIERRE As OracleParameter
                Get
                    Return New OracleParameter("p_FECHACIERRE", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property IDTIPOINCIDENCIA As OracleParameter
                Get
                    Return New OracleParameter("p_IDTIPOINCIDENCIA", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property REFERENCIACLIENTE As OracleParameter
                Get
                    Return New OracleParameter("p_REFERENCIACLIENTE", OracleDbType.NVarchar2, 2000)
                End Get
            End Property

            Public Shared ReadOnly Property CLIENTE As OracleParameter
                Get
                    Return New OracleParameter("p_CLIENTE", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property DESCRIPCIONPROBLEMA As OracleParameter
                Get
                    Return New OracleParameter("p_DESCRIPCIONPROBLEMA", OracleDbType.NVarchar2, 2000)
                End Get
            End Property

            Public Shared ReadOnly Property RAZONESNODETECCION As OracleParameter
                Get
                    Return New OracleParameter("p_RAZONESNODETECCION", OracleDbType.NVarchar2, 2000)
                End Get
            End Property

            Public Shared ReadOnly Property TITULO As OracleParameter
                Get
                    Return New OracleParameter("p_TITULO", OracleDbType.NVarchar2, 2000)
                End Get
            End Property

            Public Shared ReadOnly Property CAUSAPROBLEMA As OracleParameter
                Get
                    Return New OracleParameter("p_CAUSAPROBLEMA", OracleDbType.NVarchar2, 2000)
                End Get
            End Property

            Public Shared ReadOnly Property IDPROVEEDOR As OracleParameter
                Get
                    Return New OracleParameter("p_IDPROVEEDOR", OracleDbType.Varchar2, 4)
                End Get
            End Property

            Public Shared ReadOnly Property TOTALACORDADO As OracleParameter
                Get
                    Return New OracleParameter("p_TOTALACORDADO", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property PROCEDENCIANC As OracleParameter
                Get
                    Return New OracleParameter("p_PROCEDENCIANC", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property OBSERVACIONESCOSTE As OracleParameter
                Get
                    Return New OracleParameter("p_OBSERVACIONESCOSTE", OracleDbType.NVarchar2, 2000)
                End Get
            End Property

            Public Shared ReadOnly Property NUMPEDCAB As OracleParameter
                Get
                    Return New OracleParameter("p_NUMPEDCAB", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property COMPENSADO As OracleParameter
                Get
                    Return New OracleParameter("p_COMPENSADO", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property CAPID As OracleParameter
                Get
                    Return New OracleParameter("p_CAPID", OracleDbType.Varchar2, 25)
                End Get
            End Property

            Public Shared ReadOnly Property IDCREADOR As OracleParameter
                Get
                    Return New OracleParameter("p_IDCREADOR", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property CODAREA As OracleParameter
                Get
                    Return New OracleParameter("p_CODAREA", OracleDbType.Varchar2, 1)
                End Get
            End Property

            Public Shared ReadOnly Property CODSECCION As OracleParameter
                Get
                    Return New OracleParameter("p_CODSECCION", OracleDbType.Char, 4)
                End Get
            End Property

            Public Shared ReadOnly Property CODPROCESO As OracleParameter
                Get
                    Return New OracleParameter("p_CODPROCESO", OracleDbType.Char, 4)
                End Get
            End Property

            Public Shared ReadOnly Property LANTEGI As OracleParameter
                Get
                    Return New OracleParameter("p_LANTEGI", OracleDbType.Int32, 0)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const ID As String = "ID"
            Public Const IDREPETITIVA As String = "IDREPETITIVA"
            Public Const FECHAAPERTURA As String = "FECHAAPERTURA"
            Public Const FECHACIERRE As String = "FECHACIERRE"
            Public Const IDTIPOINCIDENCIA As String = "IDTIPOINCIDENCIA"
            Public Const REFERENCIACLIENTE As String = "REFERENCIACLIENTE"
            Public Const CLIENTE As String = "CLIENTE"
            Public Const DESCRIPCIONPROBLEMA As String = "DESCRIPCIONPROBLEMA"
            Public Const RAZONESNODETECCION As String = "RAZONESNODETECCION"
            Public Const TITULO As String = "TITULO"
            Public Const CAUSAPROBLEMA As String = "CAUSAPROBLEMA"
            Public Const IDPROVEEDOR As String = "IDPROVEEDOR"
            Public Const TOTALACORDADO As String = "TOTALACORDADO"
            Public Const PROCEDENCIANC As String = "PROCEDENCIANC"
            Public Const OBSERVACIONESCOSTE As String = "OBSERVACIONESCOSTE"
            Public Const NUMPEDCAB As String = "NUMPEDCAB"
            Public Const COMPENSADO As String = "COMPENSADO"
            Public Const CAPID As String = "CAPID"
            Public Const IDCREADOR As String = "IDCREADOR"
            Public Const CODAREA As String = "CODAREA"
            Public Const CODSECCION As String = "CODSECCION"
            Public Const CODPROCESO As String = "CODPROCESO"
            Public Const LANTEGI As String = "LANTEGI"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(ID) = _GERTAKARIAK.PropertyNames.ID
                    ht(IDREPETITIVA) = _GERTAKARIAK.PropertyNames.IDREPETITIVA
                    ht(FECHAAPERTURA) = _GERTAKARIAK.PropertyNames.FECHAAPERTURA
                    ht(FECHACIERRE) = _GERTAKARIAK.PropertyNames.FECHACIERRE
                    ht(IDTIPOINCIDENCIA) = _GERTAKARIAK.PropertyNames.IDTIPOINCIDENCIA
                    ht(REFERENCIACLIENTE) = _GERTAKARIAK.PropertyNames.REFERENCIACLIENTE
                    ht(CLIENTE) = _GERTAKARIAK.PropertyNames.CLIENTE
                    ht(DESCRIPCIONPROBLEMA) = _GERTAKARIAK.PropertyNames.DESCRIPCIONPROBLEMA
                    ht(RAZONESNODETECCION) = _GERTAKARIAK.PropertyNames.RAZONESNODETECCION
                    ht(TITULO) = _GERTAKARIAK.PropertyNames.TITULO
                    ht(CAUSAPROBLEMA) = _GERTAKARIAK.PropertyNames.CAUSAPROBLEMA
                    ht(IDPROVEEDOR) = _GERTAKARIAK.PropertyNames.IDPROVEEDOR
                    ht(TOTALACORDADO) = _GERTAKARIAK.PropertyNames.TOTALACORDADO
                    ht(PROCEDENCIANC) = _GERTAKARIAK.PropertyNames.PROCEDENCIANC
                    ht(OBSERVACIONESCOSTE) = _GERTAKARIAK.PropertyNames.OBSERVACIONESCOSTE
                    ht(NUMPEDCAB) = _GERTAKARIAK.PropertyNames.NUMPEDCAB
                    ht(COMPENSADO) = _GERTAKARIAK.PropertyNames.COMPENSADO
                    ht(CAPID) = _GERTAKARIAK.PropertyNames.CAPID
                    ht(IDCREADOR) = _GERTAKARIAK.PropertyNames.IDCREADOR
                    ht(CODAREA) = _GERTAKARIAK.PropertyNames.CODAREA
                    ht(CODSECCION) = _GERTAKARIAK.PropertyNames.CODSECCION
                    ht(CODPROCESO) = _GERTAKARIAK.PropertyNames.CODPROCESO
                    ht(LANTEGI) = _GERTAKARIAK.PropertyNames.LANTEGI

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const ID As String = "ID"
            Public Const IDREPETITIVA As String = "IDREPETITIVA"
            Public Const FECHAAPERTURA As String = "FECHAAPERTURA"
            Public Const FECHACIERRE As String = "FECHACIERRE"
            Public Const IDTIPOINCIDENCIA As String = "IDTIPOINCIDENCIA"
            Public Const REFERENCIACLIENTE As String = "REFERENCIACLIENTE"
            Public Const CLIENTE As String = "CLIENTE"
            Public Const DESCRIPCIONPROBLEMA As String = "DESCRIPCIONPROBLEMA"
            Public Const RAZONESNODETECCION As String = "RAZONESNODETECCION"
            Public Const TITULO As String = "TITULO"
            Public Const CAUSAPROBLEMA As String = "CAUSAPROBLEMA"
            Public Const IDPROVEEDOR As String = "IDPROVEEDOR"
            Public Const TOTALACORDADO As String = "TOTALACORDADO"
            Public Const PROCEDENCIANC As String = "PROCEDENCIANC"
            Public Const OBSERVACIONESCOSTE As String = "OBSERVACIONESCOSTE"
            Public Const NUMPEDCAB As String = "NUMPEDCAB"
            Public Const COMPENSADO As String = "COMPENSADO"
            Public Const CAPID As String = "CAPID"
            Public Const IDCREADOR As String = "IDCREADOR"
            Public Const CODAREA As String = "CODAREA"
            Public Const CODSECCION As String = "CODSECCION"
            Public Const CODPROCESO As String = "CODPROCESO"
            Public Const LANTEGI As String = "LANTEGI"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(ID) = _GERTAKARIAK.ColumnNames.ID
                    ht(IDREPETITIVA) = _GERTAKARIAK.ColumnNames.IDREPETITIVA
                    ht(FECHAAPERTURA) = _GERTAKARIAK.ColumnNames.FECHAAPERTURA
                    ht(FECHACIERRE) = _GERTAKARIAK.ColumnNames.FECHACIERRE
                    ht(IDTIPOINCIDENCIA) = _GERTAKARIAK.ColumnNames.IDTIPOINCIDENCIA
                    ht(REFERENCIACLIENTE) = _GERTAKARIAK.ColumnNames.REFERENCIACLIENTE
                    ht(CLIENTE) = _GERTAKARIAK.ColumnNames.CLIENTE
                    ht(DESCRIPCIONPROBLEMA) = _GERTAKARIAK.ColumnNames.DESCRIPCIONPROBLEMA
                    ht(RAZONESNODETECCION) = _GERTAKARIAK.ColumnNames.RAZONESNODETECCION
                    ht(TITULO) = _GERTAKARIAK.ColumnNames.TITULO
                    ht(CAUSAPROBLEMA) = _GERTAKARIAK.ColumnNames.CAUSAPROBLEMA
                    ht(IDPROVEEDOR) = _GERTAKARIAK.ColumnNames.IDPROVEEDOR
                    ht(TOTALACORDADO) = _GERTAKARIAK.ColumnNames.TOTALACORDADO
                    ht(PROCEDENCIANC) = _GERTAKARIAK.ColumnNames.PROCEDENCIANC
                    ht(OBSERVACIONESCOSTE) = _GERTAKARIAK.ColumnNames.OBSERVACIONESCOSTE
                    ht(NUMPEDCAB) = _GERTAKARIAK.ColumnNames.NUMPEDCAB
                    ht(COMPENSADO) = _GERTAKARIAK.ColumnNames.COMPENSADO
                    ht(CAPID) = _GERTAKARIAK.ColumnNames.CAPID
                    ht(IDCREADOR) = _GERTAKARIAK.ColumnNames.IDCREADOR
                    ht(CODAREA) = _GERTAKARIAK.ColumnNames.CODAREA
                    ht(CODSECCION) = _GERTAKARIAK.ColumnNames.CODSECCION
                    ht(CODPROCESO) = _GERTAKARIAK.ColumnNames.CODPROCESO
                    ht(LANTEGI) = _GERTAKARIAK.ColumnNames.LANTEGI

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const ID As String = "s_ID"
            Public Const IDREPETITIVA As String = "s_IDREPETITIVA"
            Public Const FECHAAPERTURA As String = "s_FECHAAPERTURA"
            Public Const FECHACIERRE As String = "s_FECHACIERRE"
            Public Const IDTIPOINCIDENCIA As String = "s_IDTIPOINCIDENCIA"
            Public Const REFERENCIACLIENTE As String = "s_REFERENCIACLIENTE"
            Public Const CLIENTE As String = "s_CLIENTE"
            Public Const DESCRIPCIONPROBLEMA As String = "s_DESCRIPCIONPROBLEMA"
            Public Const RAZONESNODETECCION As String = "s_RAZONESNODETECCION"
            Public Const TITULO As String = "s_TITULO"
            Public Const CAUSAPROBLEMA As String = "s_CAUSAPROBLEMA"
            Public Const IDPROVEEDOR As String = "s_IDPROVEEDOR"
            Public Const TOTALACORDADO As String = "s_TOTALACORDADO"
            Public Const PROCEDENCIANC As String = "s_PROCEDENCIANC"
            Public Const OBSERVACIONESCOSTE As String = "s_OBSERVACIONESCOSTE"
            Public Const NUMPEDCAB As String = "s_NUMPEDCAB"
            Public Const COMPENSADO As String = "s_COMPENSADO"
            Public Const CAPID As String = "s_CAPID"
            Public Const IDCREADOR As String = "s_IDCREADOR"
            Public Const CODAREA As String = "s_CODAREA"
            Public Const CODSECCION As String = "s_CODSECCION"
            Public Const CODPROCESO As String = "s_CODPROCESO"
            Public Const LANTEGI As String = "s_LANTEGI"

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

        Public Overridable Property IDREPETITIVA As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IDREPETITIVA)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IDREPETITIVA, Value)
            End Set
        End Property

        Public Overridable Property FECHAAPERTURA As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECHAAPERTURA)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECHAAPERTURA, Value)
            End Set
        End Property

        Public Overridable Property FECHACIERRE As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECHACIERRE)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECHACIERRE, Value)
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

        Public Overridable Property REFERENCIACLIENTE As String
            Get
                Return MyBase.GetString(ColumnNames.REFERENCIACLIENTE)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.REFERENCIACLIENTE, Value)
            End Set
        End Property

        Public Overridable Property CLIENTE As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.CLIENTE)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.CLIENTE, Value)
            End Set
        End Property

        Public Overridable Property DESCRIPCIONPROBLEMA As String
            Get
                Return MyBase.GetString(ColumnNames.DESCRIPCIONPROBLEMA)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.DESCRIPCIONPROBLEMA, Value)
            End Set
        End Property

        Public Overridable Property RAZONESNODETECCION As String
            Get
                Return MyBase.GetString(ColumnNames.RAZONESNODETECCION)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.RAZONESNODETECCION, Value)
            End Set
        End Property

        Public Overridable Property TITULO As String
            Get
                Return MyBase.GetString(ColumnNames.TITULO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.TITULO, Value)
            End Set
        End Property

        Public Overridable Property CAUSAPROBLEMA As String
            Get
                Return MyBase.GetString(ColumnNames.CAUSAPROBLEMA)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CAUSAPROBLEMA, Value)
            End Set
        End Property

        Public Overridable Property IDPROVEEDOR As String
            Get
                Return MyBase.GetString(ColumnNames.IDPROVEEDOR)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.IDPROVEEDOR, Value)
            End Set
        End Property

        Public Overridable Property TOTALACORDADO As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.TOTALACORDADO)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.TOTALACORDADO, Value)
            End Set
        End Property

        Public Overridable Property PROCEDENCIANC As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.PROCEDENCIANC)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.PROCEDENCIANC, Value)
            End Set
        End Property

        Public Overridable Property OBSERVACIONESCOSTE As String
            Get
                Return MyBase.GetString(ColumnNames.OBSERVACIONESCOSTE)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.OBSERVACIONESCOSTE, Value)
            End Set
        End Property

        Public Overridable Property NUMPEDCAB As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.NUMPEDCAB)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.NUMPEDCAB, Value)
            End Set
        End Property

        Public Overridable Property COMPENSADO As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.COMPENSADO)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.COMPENSADO, Value)
            End Set
        End Property

        Public Overridable Property CAPID As String
            Get
                Return MyBase.GetString(ColumnNames.CAPID)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CAPID, Value)
            End Set
        End Property

        Public Overridable Property IDCREADOR As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IDCREADOR)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IDCREADOR, Value)
            End Set
        End Property

        Public Overridable Property CODAREA As String
            Get
                Return MyBase.GetString(ColumnNames.CODAREA)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CODAREA, Value)
            End Set
        End Property

        Public Overridable Property CODSECCION As String
            Get
                Return MyBase.GetString(ColumnNames.CODSECCION)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CODSECCION, Value)
            End Set
        End Property

        Public Overridable Property CODPROCESO As String
            Get
                Return MyBase.GetString(ColumnNames.CODPROCESO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CODPROCESO, Value)
            End Set
        End Property

        Public Overridable Property LANTEGI As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.LANTEGI)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.LANTEGI, Value)
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

        Public Overridable Property s_IDREPETITIVA As String
            Get
                If Me.IsColumnNull(ColumnNames.IDREPETITIVA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.IDREPETITIVA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDREPETITIVA)
                Else
                    Me.IDREPETITIVA = MyBase.SetDecimalAsString(ColumnNames.IDREPETITIVA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FECHAAPERTURA As String
            Get
                If Me.IsColumnNull(ColumnNames.FECHAAPERTURA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FECHAAPERTURA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FECHAAPERTURA)
                Else
                    Me.FECHAAPERTURA = MyBase.SetDateTimeAsString(ColumnNames.FECHAAPERTURA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FECHACIERRE As String
            Get
                If Me.IsColumnNull(ColumnNames.FECHACIERRE) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FECHACIERRE)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FECHACIERRE)
                Else
                    Me.FECHACIERRE = MyBase.SetDateTimeAsString(ColumnNames.FECHACIERRE, Value)
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

        Public Overridable Property s_REFERENCIACLIENTE As String
            Get
                If Me.IsColumnNull(ColumnNames.REFERENCIACLIENTE) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.REFERENCIACLIENTE)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.REFERENCIACLIENTE)
                Else
                    Me.REFERENCIACLIENTE = MyBase.SetStringAsString(ColumnNames.REFERENCIACLIENTE, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CLIENTE As String
            Get
                If Me.IsColumnNull(ColumnNames.CLIENTE) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.CLIENTE)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CLIENTE)
                Else
                    Me.CLIENTE = MyBase.SetDecimalAsString(ColumnNames.CLIENTE, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_DESCRIPCIONPROBLEMA As String
            Get
                If Me.IsColumnNull(ColumnNames.DESCRIPCIONPROBLEMA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.DESCRIPCIONPROBLEMA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.DESCRIPCIONPROBLEMA)
                Else
                    Me.DESCRIPCIONPROBLEMA = MyBase.SetStringAsString(ColumnNames.DESCRIPCIONPROBLEMA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_RAZONESNODETECCION As String
            Get
                If Me.IsColumnNull(ColumnNames.RAZONESNODETECCION) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.RAZONESNODETECCION)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.RAZONESNODETECCION)
                Else
                    Me.RAZONESNODETECCION = MyBase.SetStringAsString(ColumnNames.RAZONESNODETECCION, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_TITULO As String
            Get
                If Me.IsColumnNull(ColumnNames.TITULO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.TITULO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.TITULO)
                Else
                    Me.TITULO = MyBase.SetStringAsString(ColumnNames.TITULO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CAUSAPROBLEMA As String
            Get
                If Me.IsColumnNull(ColumnNames.CAUSAPROBLEMA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CAUSAPROBLEMA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CAUSAPROBLEMA)
                Else
                    Me.CAUSAPROBLEMA = MyBase.SetStringAsString(ColumnNames.CAUSAPROBLEMA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_IDPROVEEDOR As String
            Get
                If Me.IsColumnNull(ColumnNames.IDPROVEEDOR) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.IDPROVEEDOR)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDPROVEEDOR)
                Else
                    Me.IDPROVEEDOR = MyBase.SetStringAsString(ColumnNames.IDPROVEEDOR, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_TOTALACORDADO As String
            Get
                If Me.IsColumnNull(ColumnNames.TOTALACORDADO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.TOTALACORDADO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.TOTALACORDADO)
                Else
                    Me.TOTALACORDADO = MyBase.SetDecimalAsString(ColumnNames.TOTALACORDADO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_PROCEDENCIANC As String
            Get
                If Me.IsColumnNull(ColumnNames.PROCEDENCIANC) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.PROCEDENCIANC)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.PROCEDENCIANC)
                Else
                    Me.PROCEDENCIANC = MyBase.SetDecimalAsString(ColumnNames.PROCEDENCIANC, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_OBSERVACIONESCOSTE As String
            Get
                If Me.IsColumnNull(ColumnNames.OBSERVACIONESCOSTE) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.OBSERVACIONESCOSTE)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.OBSERVACIONESCOSTE)
                Else
                    Me.OBSERVACIONESCOSTE = MyBase.SetStringAsString(ColumnNames.OBSERVACIONESCOSTE, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_NUMPEDCAB As String
            Get
                If Me.IsColumnNull(ColumnNames.NUMPEDCAB) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.NUMPEDCAB)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NUMPEDCAB)
                Else
                    Me.NUMPEDCAB = MyBase.SetDecimalAsString(ColumnNames.NUMPEDCAB, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_COMPENSADO As String
            Get
                If Me.IsColumnNull(ColumnNames.COMPENSADO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.COMPENSADO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.COMPENSADO)
                Else
                    Me.COMPENSADO = MyBase.SetDecimalAsString(ColumnNames.COMPENSADO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CAPID As String
            Get
                If Me.IsColumnNull(ColumnNames.CAPID) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CAPID)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CAPID)
                Else
                    Me.CAPID = MyBase.SetStringAsString(ColumnNames.CAPID, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_IDCREADOR As String
            Get
                If Me.IsColumnNull(ColumnNames.IDCREADOR) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.IDCREADOR)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IDCREADOR)
                Else
                    Me.IDCREADOR = MyBase.SetDecimalAsString(ColumnNames.IDCREADOR, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CODAREA As String
            Get
                If Me.IsColumnNull(ColumnNames.CODAREA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CODAREA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CODAREA)
                Else
                    Me.CODAREA = MyBase.SetStringAsString(ColumnNames.CODAREA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CODSECCION As String
            Get
                If Me.IsColumnNull(ColumnNames.CODSECCION) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CODSECCION)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CODSECCION)
                Else
                    Me.CODSECCION = MyBase.SetStringAsString(ColumnNames.CODSECCION, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CODPROCESO As String
            Get
                If Me.IsColumnNull(ColumnNames.CODPROCESO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CODPROCESO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CODPROCESO)
                Else
                    Me.CODPROCESO = MyBase.SetStringAsString(ColumnNames.CODPROCESO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_LANTEGI As String
            Get
                If Me.IsColumnNull(ColumnNames.LANTEGI) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.LANTEGI)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.LANTEGI)
                Else
                    Me.LANTEGI = MyBase.SetDecimalAsString(ColumnNames.LANTEGI, Value)
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

                Public ReadOnly Property IDREPETITIVA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDREPETITIVA, Parameters.IDREPETITIVA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECHAAPERTURA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FECHAAPERTURA, Parameters.FECHAAPERTURA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECHACIERRE() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FECHACIERRE, Parameters.FECHACIERRE)
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

                Public ReadOnly Property REFERENCIACLIENTE() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.REFERENCIACLIENTE, Parameters.REFERENCIACLIENTE)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CLIENTE() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CLIENTE, Parameters.CLIENTE)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DESCRIPCIONPROBLEMA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.DESCRIPCIONPROBLEMA, Parameters.DESCRIPCIONPROBLEMA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property RAZONESNODETECCION() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.RAZONESNODETECCION, Parameters.RAZONESNODETECCION)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TITULO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.TITULO, Parameters.TITULO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CAUSAPROBLEMA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CAUSAPROBLEMA, Parameters.CAUSAPROBLEMA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDPROVEEDOR() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDPROVEEDOR, Parameters.IDPROVEEDOR)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TOTALACORDADO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.TOTALACORDADO, Parameters.TOTALACORDADO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PROCEDENCIANC() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.PROCEDENCIANC, Parameters.PROCEDENCIANC)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property OBSERVACIONESCOSTE() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.OBSERVACIONESCOSTE, Parameters.OBSERVACIONESCOSTE)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMPEDCAB() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMPEDCAB, Parameters.NUMPEDCAB)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property COMPENSADO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.COMPENSADO, Parameters.COMPENSADO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CAPID() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CAPID, Parameters.CAPID)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDCREADOR() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDCREADOR, Parameters.IDCREADOR)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODAREA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CODAREA, Parameters.CODAREA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODSECCION() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CODSECCION, Parameters.CODSECCION)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODPROCESO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CODPROCESO, Parameters.CODPROCESO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property LANTEGI() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.LANTEGI, Parameters.LANTEGI)
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

            Public ReadOnly Property IDREPETITIVA() As WhereParameter
                Get
                    If _IDREPETITIVA_W Is Nothing Then
                        _IDREPETITIVA_W = TearOff.IDREPETITIVA
                    End If
                    Return _IDREPETITIVA_W
                End Get
            End Property

            Public ReadOnly Property FECHAAPERTURA() As WhereParameter
                Get
                    If _FECHAAPERTURA_W Is Nothing Then
                        _FECHAAPERTURA_W = TearOff.FECHAAPERTURA
                    End If
                    Return _FECHAAPERTURA_W
                End Get
            End Property

            Public ReadOnly Property FECHACIERRE() As WhereParameter
                Get
                    If _FECHACIERRE_W Is Nothing Then
                        _FECHACIERRE_W = TearOff.FECHACIERRE
                    End If
                    Return _FECHACIERRE_W
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

            Public ReadOnly Property REFERENCIACLIENTE() As WhereParameter
                Get
                    If _REFERENCIACLIENTE_W Is Nothing Then
                        _REFERENCIACLIENTE_W = TearOff.REFERENCIACLIENTE
                    End If
                    Return _REFERENCIACLIENTE_W
                End Get
            End Property

            Public ReadOnly Property CLIENTE() As WhereParameter
                Get
                    If _CLIENTE_W Is Nothing Then
                        _CLIENTE_W = TearOff.CLIENTE
                    End If
                    Return _CLIENTE_W
                End Get
            End Property

            Public ReadOnly Property DESCRIPCIONPROBLEMA() As WhereParameter
                Get
                    If _DESCRIPCIONPROBLEMA_W Is Nothing Then
                        _DESCRIPCIONPROBLEMA_W = TearOff.DESCRIPCIONPROBLEMA
                    End If
                    Return _DESCRIPCIONPROBLEMA_W
                End Get
            End Property

            Public ReadOnly Property RAZONESNODETECCION() As WhereParameter
                Get
                    If _RAZONESNODETECCION_W Is Nothing Then
                        _RAZONESNODETECCION_W = TearOff.RAZONESNODETECCION
                    End If
                    Return _RAZONESNODETECCION_W
                End Get
            End Property

            Public ReadOnly Property TITULO() As WhereParameter
                Get
                    If _TITULO_W Is Nothing Then
                        _TITULO_W = TearOff.TITULO
                    End If
                    Return _TITULO_W
                End Get
            End Property

            Public ReadOnly Property CAUSAPROBLEMA() As WhereParameter
                Get
                    If _CAUSAPROBLEMA_W Is Nothing Then
                        _CAUSAPROBLEMA_W = TearOff.CAUSAPROBLEMA
                    End If
                    Return _CAUSAPROBLEMA_W
                End Get
            End Property

            Public ReadOnly Property IDPROVEEDOR() As WhereParameter
                Get
                    If _IDPROVEEDOR_W Is Nothing Then
                        _IDPROVEEDOR_W = TearOff.IDPROVEEDOR
                    End If
                    Return _IDPROVEEDOR_W
                End Get
            End Property

            Public ReadOnly Property TOTALACORDADO() As WhereParameter
                Get
                    If _TOTALACORDADO_W Is Nothing Then
                        _TOTALACORDADO_W = TearOff.TOTALACORDADO
                    End If
                    Return _TOTALACORDADO_W
                End Get
            End Property

            Public ReadOnly Property PROCEDENCIANC() As WhereParameter
                Get
                    If _PROCEDENCIANC_W Is Nothing Then
                        _PROCEDENCIANC_W = TearOff.PROCEDENCIANC
                    End If
                    Return _PROCEDENCIANC_W
                End Get
            End Property

            Public ReadOnly Property OBSERVACIONESCOSTE() As WhereParameter
                Get
                    If _OBSERVACIONESCOSTE_W Is Nothing Then
                        _OBSERVACIONESCOSTE_W = TearOff.OBSERVACIONESCOSTE
                    End If
                    Return _OBSERVACIONESCOSTE_W
                End Get
            End Property

            Public ReadOnly Property NUMPEDCAB() As WhereParameter
                Get
                    If _NUMPEDCAB_W Is Nothing Then
                        _NUMPEDCAB_W = TearOff.NUMPEDCAB
                    End If
                    Return _NUMPEDCAB_W
                End Get
            End Property

            Public ReadOnly Property COMPENSADO() As WhereParameter
                Get
                    If _COMPENSADO_W Is Nothing Then
                        _COMPENSADO_W = TearOff.COMPENSADO
                    End If
                    Return _COMPENSADO_W
                End Get
            End Property

            Public ReadOnly Property CAPID() As WhereParameter
                Get
                    If _CAPID_W Is Nothing Then
                        _CAPID_W = TearOff.CAPID
                    End If
                    Return _CAPID_W
                End Get
            End Property

            Public ReadOnly Property IDCREADOR() As WhereParameter
                Get
                    If _IDCREADOR_W Is Nothing Then
                        _IDCREADOR_W = TearOff.IDCREADOR
                    End If
                    Return _IDCREADOR_W
                End Get
            End Property

            Public ReadOnly Property CODAREA() As WhereParameter
                Get
                    If _CODAREA_W Is Nothing Then
                        _CODAREA_W = TearOff.CODAREA
                    End If
                    Return _CODAREA_W
                End Get
            End Property

            Public ReadOnly Property CODSECCION() As WhereParameter
                Get
                    If _CODSECCION_W Is Nothing Then
                        _CODSECCION_W = TearOff.CODSECCION
                    End If
                    Return _CODSECCION_W
                End Get
            End Property

            Public ReadOnly Property CODPROCESO() As WhereParameter
                Get
                    If _CODPROCESO_W Is Nothing Then
                        _CODPROCESO_W = TearOff.CODPROCESO
                    End If
                    Return _CODPROCESO_W
                End Get
            End Property

            Public ReadOnly Property LANTEGI() As WhereParameter
                Get
                    If _LANTEGI_W Is Nothing Then
                        _LANTEGI_W = TearOff.LANTEGI
                    End If
                    Return _LANTEGI_W
                End Get
            End Property

            Private _ID_W As WhereParameter = Nothing
            Private _IDREPETITIVA_W As WhereParameter = Nothing
            Private _FECHAAPERTURA_W As WhereParameter = Nothing
            Private _FECHACIERRE_W As WhereParameter = Nothing
            Private _IDTIPOINCIDENCIA_W As WhereParameter = Nothing
            Private _REFERENCIACLIENTE_W As WhereParameter = Nothing
            Private _CLIENTE_W As WhereParameter = Nothing
            Private _DESCRIPCIONPROBLEMA_W As WhereParameter = Nothing
            Private _RAZONESNODETECCION_W As WhereParameter = Nothing
            Private _TITULO_W As WhereParameter = Nothing
            Private _CAUSAPROBLEMA_W As WhereParameter = Nothing
            Private _IDPROVEEDOR_W As WhereParameter = Nothing
            Private _TOTALACORDADO_W As WhereParameter = Nothing
            Private _PROCEDENCIANC_W As WhereParameter = Nothing
            Private _OBSERVACIONESCOSTE_W As WhereParameter = Nothing
            Private _NUMPEDCAB_W As WhereParameter = Nothing
            Private _COMPENSADO_W As WhereParameter = Nothing
            Private _CAPID_W As WhereParameter = Nothing
            Private _IDCREADOR_W As WhereParameter = Nothing
            Private _CODAREA_W As WhereParameter = Nothing
            Private _CODSECCION_W As WhereParameter = Nothing
            Private _CODPROCESO_W As WhereParameter = Nothing
            Private _LANTEGI_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _ID_W = Nothing
                _IDREPETITIVA_W = Nothing
                _FECHAAPERTURA_W = Nothing
                _FECHACIERRE_W = Nothing
                _IDTIPOINCIDENCIA_W = Nothing
                _REFERENCIACLIENTE_W = Nothing
                _CLIENTE_W = Nothing
                _DESCRIPCIONPROBLEMA_W = Nothing
                _RAZONESNODETECCION_W = Nothing
                _TITULO_W = Nothing
                _CAUSAPROBLEMA_W = Nothing
                _IDPROVEEDOR_W = Nothing
                _TOTALACORDADO_W = Nothing
                _PROCEDENCIANC_W = Nothing
                _OBSERVACIONESCOSTE_W = Nothing
                _NUMPEDCAB_W = Nothing
                _COMPENSADO_W = Nothing
                _CAPID_W = Nothing
                _IDCREADOR_W = Nothing
                _CODAREA_W = Nothing
                _CODSECCION_W = Nothing
                _CODPROCESO_W = Nothing
                _LANTEGI_W = Nothing
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

                Public ReadOnly Property IDREPETITIVA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDREPETITIVA, Parameters.IDREPETITIVA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECHAAPERTURA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECHAAPERTURA, Parameters.FECHAAPERTURA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECHACIERRE() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECHACIERRE, Parameters.FECHACIERRE)
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

                Public ReadOnly Property REFERENCIACLIENTE() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.REFERENCIACLIENTE, Parameters.REFERENCIACLIENTE)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CLIENTE() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CLIENTE, Parameters.CLIENTE)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DESCRIPCIONPROBLEMA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DESCRIPCIONPROBLEMA, Parameters.DESCRIPCIONPROBLEMA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property RAZONESNODETECCION() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.RAZONESNODETECCION, Parameters.RAZONESNODETECCION)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TITULO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TITULO, Parameters.TITULO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CAUSAPROBLEMA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CAUSAPROBLEMA, Parameters.CAUSAPROBLEMA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDPROVEEDOR() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDPROVEEDOR, Parameters.IDPROVEEDOR)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TOTALACORDADO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TOTALACORDADO, Parameters.TOTALACORDADO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PROCEDENCIANC() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.PROCEDENCIANC, Parameters.PROCEDENCIANC)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property OBSERVACIONESCOSTE() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.OBSERVACIONESCOSTE, Parameters.OBSERVACIONESCOSTE)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMPEDCAB() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMPEDCAB, Parameters.NUMPEDCAB)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property COMPENSADO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.COMPENSADO, Parameters.COMPENSADO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CAPID() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CAPID, Parameters.CAPID)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IDCREADOR() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDCREADOR, Parameters.IDCREADOR)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODAREA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CODAREA, Parameters.CODAREA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODSECCION() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CODSECCION, Parameters.CODSECCION)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODPROCESO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CODPROCESO, Parameters.CODPROCESO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property LANTEGI() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.LANTEGI, Parameters.LANTEGI)
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

            Public ReadOnly Property IDREPETITIVA() As AggregateParameter
                Get
                    If _IDREPETITIVA_W Is Nothing Then
                        _IDREPETITIVA_W = TearOff.IDREPETITIVA
                    End If
                    Return _IDREPETITIVA_W
                End Get
            End Property

            Public ReadOnly Property FECHAAPERTURA() As AggregateParameter
                Get
                    If _FECHAAPERTURA_W Is Nothing Then
                        _FECHAAPERTURA_W = TearOff.FECHAAPERTURA
                    End If
                    Return _FECHAAPERTURA_W
                End Get
            End Property

            Public ReadOnly Property FECHACIERRE() As AggregateParameter
                Get
                    If _FECHACIERRE_W Is Nothing Then
                        _FECHACIERRE_W = TearOff.FECHACIERRE
                    End If
                    Return _FECHACIERRE_W
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

            Public ReadOnly Property REFERENCIACLIENTE() As AggregateParameter
                Get
                    If _REFERENCIACLIENTE_W Is Nothing Then
                        _REFERENCIACLIENTE_W = TearOff.REFERENCIACLIENTE
                    End If
                    Return _REFERENCIACLIENTE_W
                End Get
            End Property

            Public ReadOnly Property CLIENTE() As AggregateParameter
                Get
                    If _CLIENTE_W Is Nothing Then
                        _CLIENTE_W = TearOff.CLIENTE
                    End If
                    Return _CLIENTE_W
                End Get
            End Property

            Public ReadOnly Property DESCRIPCIONPROBLEMA() As AggregateParameter
                Get
                    If _DESCRIPCIONPROBLEMA_W Is Nothing Then
                        _DESCRIPCIONPROBLEMA_W = TearOff.DESCRIPCIONPROBLEMA
                    End If
                    Return _DESCRIPCIONPROBLEMA_W
                End Get
            End Property

            Public ReadOnly Property RAZONESNODETECCION() As AggregateParameter
                Get
                    If _RAZONESNODETECCION_W Is Nothing Then
                        _RAZONESNODETECCION_W = TearOff.RAZONESNODETECCION
                    End If
                    Return _RAZONESNODETECCION_W
                End Get
            End Property

            Public ReadOnly Property TITULO() As AggregateParameter
                Get
                    If _TITULO_W Is Nothing Then
                        _TITULO_W = TearOff.TITULO
                    End If
                    Return _TITULO_W
                End Get
            End Property

            Public ReadOnly Property CAUSAPROBLEMA() As AggregateParameter
                Get
                    If _CAUSAPROBLEMA_W Is Nothing Then
                        _CAUSAPROBLEMA_W = TearOff.CAUSAPROBLEMA
                    End If
                    Return _CAUSAPROBLEMA_W
                End Get
            End Property

            Public ReadOnly Property IDPROVEEDOR() As AggregateParameter
                Get
                    If _IDPROVEEDOR_W Is Nothing Then
                        _IDPROVEEDOR_W = TearOff.IDPROVEEDOR
                    End If
                    Return _IDPROVEEDOR_W
                End Get
            End Property

            Public ReadOnly Property TOTALACORDADO() As AggregateParameter
                Get
                    If _TOTALACORDADO_W Is Nothing Then
                        _TOTALACORDADO_W = TearOff.TOTALACORDADO
                    End If
                    Return _TOTALACORDADO_W
                End Get
            End Property

            Public ReadOnly Property PROCEDENCIANC() As AggregateParameter
                Get
                    If _PROCEDENCIANC_W Is Nothing Then
                        _PROCEDENCIANC_W = TearOff.PROCEDENCIANC
                    End If
                    Return _PROCEDENCIANC_W
                End Get
            End Property

            Public ReadOnly Property OBSERVACIONESCOSTE() As AggregateParameter
                Get
                    If _OBSERVACIONESCOSTE_W Is Nothing Then
                        _OBSERVACIONESCOSTE_W = TearOff.OBSERVACIONESCOSTE
                    End If
                    Return _OBSERVACIONESCOSTE_W
                End Get
            End Property

            Public ReadOnly Property NUMPEDCAB() As AggregateParameter
                Get
                    If _NUMPEDCAB_W Is Nothing Then
                        _NUMPEDCAB_W = TearOff.NUMPEDCAB
                    End If
                    Return _NUMPEDCAB_W
                End Get
            End Property

            Public ReadOnly Property COMPENSADO() As AggregateParameter
                Get
                    If _COMPENSADO_W Is Nothing Then
                        _COMPENSADO_W = TearOff.COMPENSADO
                    End If
                    Return _COMPENSADO_W
                End Get
            End Property

            Public ReadOnly Property CAPID() As AggregateParameter
                Get
                    If _CAPID_W Is Nothing Then
                        _CAPID_W = TearOff.CAPID
                    End If
                    Return _CAPID_W
                End Get
            End Property

            Public ReadOnly Property IDCREADOR() As AggregateParameter
                Get
                    If _IDCREADOR_W Is Nothing Then
                        _IDCREADOR_W = TearOff.IDCREADOR
                    End If
                    Return _IDCREADOR_W
                End Get
            End Property

            Public ReadOnly Property CODAREA() As AggregateParameter
                Get
                    If _CODAREA_W Is Nothing Then
                        _CODAREA_W = TearOff.CODAREA
                    End If
                    Return _CODAREA_W
                End Get
            End Property

            Public ReadOnly Property CODSECCION() As AggregateParameter
                Get
                    If _CODSECCION_W Is Nothing Then
                        _CODSECCION_W = TearOff.CODSECCION
                    End If
                    Return _CODSECCION_W
                End Get
            End Property

            Public ReadOnly Property CODPROCESO() As AggregateParameter
                Get
                    If _CODPROCESO_W Is Nothing Then
                        _CODPROCESO_W = TearOff.CODPROCESO
                    End If
                    Return _CODPROCESO_W
                End Get
            End Property

            Public ReadOnly Property LANTEGI() As AggregateParameter
                Get
                    If _LANTEGI_W Is Nothing Then
                        _LANTEGI_W = TearOff.LANTEGI
                    End If
                    Return _LANTEGI_W
                End Get
            End Property

            Private _ID_W As AggregateParameter = Nothing
            Private _IDREPETITIVA_W As AggregateParameter = Nothing
            Private _FECHAAPERTURA_W As AggregateParameter = Nothing
            Private _FECHACIERRE_W As AggregateParameter = Nothing
            Private _IDTIPOINCIDENCIA_W As AggregateParameter = Nothing
            Private _REFERENCIACLIENTE_W As AggregateParameter = Nothing
            Private _CLIENTE_W As AggregateParameter = Nothing
            Private _DESCRIPCIONPROBLEMA_W As AggregateParameter = Nothing
            Private _RAZONESNODETECCION_W As AggregateParameter = Nothing
            Private _TITULO_W As AggregateParameter = Nothing
            Private _CAUSAPROBLEMA_W As AggregateParameter = Nothing
            Private _IDPROVEEDOR_W As AggregateParameter = Nothing
            Private _TOTALACORDADO_W As AggregateParameter = Nothing
            Private _PROCEDENCIANC_W As AggregateParameter = Nothing
            Private _OBSERVACIONESCOSTE_W As AggregateParameter = Nothing
            Private _NUMPEDCAB_W As AggregateParameter = Nothing
            Private _COMPENSADO_W As AggregateParameter = Nothing
            Private _CAPID_W As AggregateParameter = Nothing
            Private _IDCREADOR_W As AggregateParameter = Nothing
            Private _CODAREA_W As AggregateParameter = Nothing
            Private _CODSECCION_W As AggregateParameter = Nothing
            Private _CODPROCESO_W As AggregateParameter = Nothing
            Private _LANTEGI_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _ID_W = Nothing
                _IDREPETITIVA_W = Nothing
                _FECHAAPERTURA_W = Nothing
                _FECHACIERRE_W = Nothing
                _IDTIPOINCIDENCIA_W = Nothing
                _REFERENCIACLIENTE_W = Nothing
                _CLIENTE_W = Nothing
                _DESCRIPCIONPROBLEMA_W = Nothing
                _RAZONESNODETECCION_W = Nothing
                _TITULO_W = Nothing
                _CAUSAPROBLEMA_W = Nothing
                _IDPROVEEDOR_W = Nothing
                _TOTALACORDADO_W = Nothing
                _PROCEDENCIANC_W = Nothing
                _OBSERVACIONESCOSTE_W = Nothing
                _NUMPEDCAB_W = Nothing
                _COMPENSADO_W = Nothing
                _CAPID_W = Nothing
                _IDCREADOR_W = Nothing
                _CODAREA_W = Nothing
                _CODSECCION_W = Nothing
                _CODPROCESO_W = Nothing
                _LANTEGI_W = Nothing
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
            cmd.CommandText = Me.SchemaStoredProcedure + "PI_GERTAKARIAK"

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
            cmd.CommandText = Me.SchemaStoredProcedure + "PU_GERTAKARIAK"

            CreateParameters(cmd)

            Return cmd

        End Function

        Protected Overrides Function GetDeleteCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PD_GERTAKARIAK"

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

            p = cmd.Parameters.Add(Parameters.IDREPETITIVA)
            p.SourceColumn = ColumnNames.IDREPETITIVA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.FECHAAPERTURA)
            p.SourceColumn = ColumnNames.FECHAAPERTURA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.FECHACIERRE)
            p.SourceColumn = ColumnNames.FECHACIERRE
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDTIPOINCIDENCIA)
            p.SourceColumn = ColumnNames.IDTIPOINCIDENCIA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.REFERENCIACLIENTE)
            p.SourceColumn = ColumnNames.REFERENCIACLIENTE
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CLIENTE)
            p.SourceColumn = ColumnNames.CLIENTE
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.DESCRIPCIONPROBLEMA)
            p.SourceColumn = ColumnNames.DESCRIPCIONPROBLEMA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.RAZONESNODETECCION)
            p.SourceColumn = ColumnNames.RAZONESNODETECCION
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.TITULO)
            p.SourceColumn = ColumnNames.TITULO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CAUSAPROBLEMA)
            p.SourceColumn = ColumnNames.CAUSAPROBLEMA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDPROVEEDOR)
            p.SourceColumn = ColumnNames.IDPROVEEDOR
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.TOTALACORDADO)
            p.SourceColumn = ColumnNames.TOTALACORDADO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.PROCEDENCIANC)
            p.SourceColumn = ColumnNames.PROCEDENCIANC
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.OBSERVACIONESCOSTE)
            p.SourceColumn = ColumnNames.OBSERVACIONESCOSTE
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMPEDCAB)
            p.SourceColumn = ColumnNames.NUMPEDCAB
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.COMPENSADO)
            p.SourceColumn = ColumnNames.COMPENSADO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CAPID)
            p.SourceColumn = ColumnNames.CAPID
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDCREADOR)
            p.SourceColumn = ColumnNames.IDCREADOR
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CODAREA)
            p.SourceColumn = ColumnNames.CODAREA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CODSECCION)
            p.SourceColumn = ColumnNames.CODSECCION
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CODPROCESO)
            p.SourceColumn = ColumnNames.CODPROCESO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.LANTEGI)
            p.SourceColumn = ColumnNames.LANTEGI
            p.SourceVersion = DataRowVersion.Current


        End Sub

    End Class

End Namespace

