
'===============================================================================
'BATZ, Koop. - 14/09/2010 8:09:54
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

    Public MustInherit Class _LINEASCOSTE
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "INCIDENCIAS."
            Me.QuerySource = "LINEASCOSTE"
            Me.MappingName = "LINEASCOSTE"
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


            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_LINEASCOSTE", parameters)
        End Function

        '=================================================================
        ' Public Overridable Function LoadByPrimaryKey()  As Boolean
        '=================================================================
        '  Loads a single row of via the primary key
        '=================================================================
        Public Overridable Function LoadByPrimaryKey(ByVal ID As Decimal) As Boolean

            Dim parameters As ListDictionary = New ListDictionary()
            parameters.Add(_LINEASCOSTE.Parameters.ID, ID)


            Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
            p.Direction = ParameterDirection.Output
            parameters.Add(p, Nothing)

            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_LINEASCOSTE", parameters)

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

            Public Shared ReadOnly Property IDOFMARCA As OracleParameter
                Get
                    Return New OracleParameter("p_IDOFMARCA", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property ORIGEN As OracleParameter
                Get
                    Return New OracleParameter("p_ORIGEN", OracleDbType.Char, 1)
                End Get
            End Property

            Public Shared ReadOnly Property NUMORD As OracleParameter
                Get
                    Return New OracleParameter("p_NUMORD", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property NUMOPE As OracleParameter
                Get
                    Return New OracleParameter("p_NUMOPE", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property NUMMAR As OracleParameter
                Get
                    Return New OracleParameter("p_NUMMAR", OracleDbType.Char, 11)
                End Get
            End Property

            Public Shared ReadOnly Property CODPRO As OracleParameter
                Get
                    Return New OracleParameter("p_CODPRO", OracleDbType.Char, 12)
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

            Public Shared ReadOnly Property HORAS As OracleParameter
                Get
                    Return New OracleParameter("p_HORAS", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property IMPORTE As OracleParameter
                Get
                    Return New OracleParameter("p_IMPORTE", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property SECCION As OracleParameter
                Get
                    Return New OracleParameter("p_SECCION", OracleDbType.Varchar2, 4)
                End Get
            End Property

            Public Shared ReadOnly Property MAQUINA As OracleParameter
                Get
                    Return New OracleParameter("p_MAQUINA", OracleDbType.Varchar2, 10)
                End Get
            End Property

            Public Shared ReadOnly Property FASE As OracleParameter
                Get
                    Return New OracleParameter("p_FASE", OracleDbType.Varchar2, 4)
                End Get
            End Property

            Public Shared ReadOnly Property PROCESO As OracleParameter
                Get
                    Return New OracleParameter("p_PROCESO", OracleDbType.Varchar2, 4)
                End Get
            End Property

            Public Shared ReadOnly Property CAPACIDADPROV As OracleParameter
                Get
                    Return New OracleParameter("p_CAPACIDADPROV", OracleDbType.Varchar2, 50)
                End Get
            End Property

            Public Shared ReadOnly Property CANTIDADFAC As OracleParameter
                Get
                    Return New OracleParameter("p_CANTIDADFAC", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property DESCRIPCION As OracleParameter
                Get
                    Return New OracleParameter("p_DESCRIPCION", OracleDbType.NVarchar2, 2000)
                End Get
            End Property

            Public Shared ReadOnly Property CANTIDADPED As OracleParameter
                Get
                    Return New OracleParameter("p_CANTIDADPED", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property CODART As OracleParameter
                Get
                    Return New OracleParameter("p_CODART", OracleDbType.Char, 12)
                End Get
            End Property

            Public Shared ReadOnly Property NUMPEDORIGEN As OracleParameter
                Get
                    Return New OracleParameter("p_NUMPEDORIGEN", OracleDbType.Int32, 0)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const ID As String = "ID"
            Public Const IDINCIDENCIA As String = "IDINCIDENCIA"
            Public Const IDOFMARCA As String = "IDOFMARCA"
            Public Const ORIGEN As String = "ORIGEN"
            Public Const NUMORD As String = "NUMORD"
            Public Const NUMOPE As String = "NUMOPE"
            Public Const NUMMAR As String = "NUMMAR"
            Public Const CODPRO As String = "CODPRO"
            Public Const NUMPED As String = "NUMPED"
            Public Const NUMLIN As String = "NUMLIN"
            Public Const HORAS As String = "HORAS"
            Public Const IMPORTE As String = "IMPORTE"
            Public Const SECCION As String = "SECCION"
            Public Const MAQUINA As String = "MAQUINA"
            Public Const FASE As String = "FASE"
            Public Const PROCESO As String = "PROCESO"
            Public Const CAPACIDADPROV As String = "CAPACIDADPROV"
            Public Const CANTIDADFAC As String = "CANTIDADFAC"
            Public Const DESCRIPCION As String = "DESCRIPCION"
            Public Const CANTIDADPED As String = "CANTIDADPED"
            Public Const CODART As String = "CODART"
            Public Const NUMPEDORIGEN As String = "NUMPEDORIGEN"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(ID) = _LINEASCOSTE.PropertyNames.ID
                    ht(IDINCIDENCIA) = _LINEASCOSTE.PropertyNames.IDINCIDENCIA
                    ht(IDOFMARCA) = _LINEASCOSTE.PropertyNames.IDOFMARCA
                    ht(ORIGEN) = _LINEASCOSTE.PropertyNames.ORIGEN
                    ht(NUMORD) = _LINEASCOSTE.PropertyNames.NUMORD
                    ht(NUMOPE) = _LINEASCOSTE.PropertyNames.NUMOPE
                    ht(NUMMAR) = _LINEASCOSTE.PropertyNames.NUMMAR
                    ht(CODPRO) = _LINEASCOSTE.PropertyNames.CODPRO
                    ht(NUMPED) = _LINEASCOSTE.PropertyNames.NUMPED
                    ht(NUMLIN) = _LINEASCOSTE.PropertyNames.NUMLIN
                    ht(HORAS) = _LINEASCOSTE.PropertyNames.HORAS
                    ht(IMPORTE) = _LINEASCOSTE.PropertyNames.IMPORTE
                    ht(SECCION) = _LINEASCOSTE.PropertyNames.SECCION
                    ht(MAQUINA) = _LINEASCOSTE.PropertyNames.MAQUINA
                    ht(FASE) = _LINEASCOSTE.PropertyNames.FASE
                    ht(PROCESO) = _LINEASCOSTE.PropertyNames.PROCESO
                    ht(CAPACIDADPROV) = _LINEASCOSTE.PropertyNames.CAPACIDADPROV
                    ht(CANTIDADFAC) = _LINEASCOSTE.PropertyNames.CANTIDADFAC
                    ht(DESCRIPCION) = _LINEASCOSTE.PropertyNames.DESCRIPCION
                    ht(CANTIDADPED) = _LINEASCOSTE.PropertyNames.CANTIDADPED
                    ht(CODART) = _LINEASCOSTE.PropertyNames.CODART
                    ht(NUMPEDORIGEN) = _LINEASCOSTE.PropertyNames.NUMPEDORIGEN

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
            Public Const IDOFMARCA As String = "IDOFMARCA"
            Public Const ORIGEN As String = "ORIGEN"
            Public Const NUMORD As String = "NUMORD"
            Public Const NUMOPE As String = "NUMOPE"
            Public Const NUMMAR As String = "NUMMAR"
            Public Const CODPRO As String = "CODPRO"
            Public Const NUMPED As String = "NUMPED"
            Public Const NUMLIN As String = "NUMLIN"
            Public Const HORAS As String = "HORAS"
            Public Const IMPORTE As String = "IMPORTE"
            Public Const SECCION As String = "SECCION"
            Public Const MAQUINA As String = "MAQUINA"
            Public Const FASE As String = "FASE"
            Public Const PROCESO As String = "PROCESO"
            Public Const CAPACIDADPROV As String = "CAPACIDADPROV"
            Public Const CANTIDADFAC As String = "CANTIDADFAC"
            Public Const DESCRIPCION As String = "DESCRIPCION"
            Public Const CANTIDADPED As String = "CANTIDADPED"
            Public Const CODART As String = "CODART"
            Public Const NUMPEDORIGEN As String = "NUMPEDORIGEN"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(ID) = _LINEASCOSTE.ColumnNames.ID
                    ht(IDINCIDENCIA) = _LINEASCOSTE.ColumnNames.IDINCIDENCIA
                    ht(IDOFMARCA) = _LINEASCOSTE.ColumnNames.IDOFMARCA
                    ht(ORIGEN) = _LINEASCOSTE.ColumnNames.ORIGEN
                    ht(NUMORD) = _LINEASCOSTE.ColumnNames.NUMORD
                    ht(NUMOPE) = _LINEASCOSTE.ColumnNames.NUMOPE
                    ht(NUMMAR) = _LINEASCOSTE.ColumnNames.NUMMAR
                    ht(CODPRO) = _LINEASCOSTE.ColumnNames.CODPRO
                    ht(NUMPED) = _LINEASCOSTE.ColumnNames.NUMPED
                    ht(NUMLIN) = _LINEASCOSTE.ColumnNames.NUMLIN
                    ht(HORAS) = _LINEASCOSTE.ColumnNames.HORAS
                    ht(IMPORTE) = _LINEASCOSTE.ColumnNames.IMPORTE
                    ht(SECCION) = _LINEASCOSTE.ColumnNames.SECCION
                    ht(MAQUINA) = _LINEASCOSTE.ColumnNames.MAQUINA
                    ht(FASE) = _LINEASCOSTE.ColumnNames.FASE
                    ht(PROCESO) = _LINEASCOSTE.ColumnNames.PROCESO
                    ht(CAPACIDADPROV) = _LINEASCOSTE.ColumnNames.CAPACIDADPROV
                    ht(CANTIDADFAC) = _LINEASCOSTE.ColumnNames.CANTIDADFAC
                    ht(DESCRIPCION) = _LINEASCOSTE.ColumnNames.DESCRIPCION
                    ht(CANTIDADPED) = _LINEASCOSTE.ColumnNames.CANTIDADPED
                    ht(CODART) = _LINEASCOSTE.ColumnNames.CODART
                    ht(NUMPEDORIGEN) = _LINEASCOSTE.ColumnNames.NUMPEDORIGEN

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
            Public Const IDOFMARCA As String = "s_IDOFMARCA"
            Public Const ORIGEN As String = "s_ORIGEN"
            Public Const NUMORD As String = "s_NUMORD"
            Public Const NUMOPE As String = "s_NUMOPE"
            Public Const NUMMAR As String = "s_NUMMAR"
            Public Const CODPRO As String = "s_CODPRO"
            Public Const NUMPED As String = "s_NUMPED"
            Public Const NUMLIN As String = "s_NUMLIN"
            Public Const HORAS As String = "s_HORAS"
            Public Const IMPORTE As String = "s_IMPORTE"
            Public Const SECCION As String = "s_SECCION"
            Public Const MAQUINA As String = "s_MAQUINA"
            Public Const FASE As String = "s_FASE"
            Public Const PROCESO As String = "s_PROCESO"
            Public Const CAPACIDADPROV As String = "s_CAPACIDADPROV"
            Public Const CANTIDADFAC As String = "s_CANTIDADFAC"
            Public Const DESCRIPCION As String = "s_DESCRIPCION"
            Public Const CANTIDADPED As String = "s_CANTIDADPED"
            Public Const CODART As String = "s_CODART"
            Public Const NUMPEDORIGEN As String = "s_NUMPEDORIGEN"

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

        Public Overridable Property IDOFMARCA As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IDOFMARCA)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IDOFMARCA, Value)
            End Set
        End Property

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

        Public Overridable Property SECCION As String
            Get
                Return MyBase.GetString(ColumnNames.SECCION)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.SECCION, Value)
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

        Public Overridable Property CAPACIDADPROV As String
            Get
                Return MyBase.GetString(ColumnNames.CAPACIDADPROV)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CAPACIDADPROV, Value)
            End Set
        End Property

        Public Overridable Property CANTIDADFAC As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.CANTIDADFAC)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.CANTIDADFAC, Value)
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

        Public Overridable Property CANTIDADPED As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.CANTIDADPED)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.CANTIDADPED, Value)
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

        Public Overridable Property NUMPEDORIGEN As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.NUMPEDORIGEN)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.NUMPEDORIGEN, Value)
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

        Public Overridable Property s_SECCION As String
            Get
                If Me.IsColumnNull(ColumnNames.SECCION) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.SECCION)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.SECCION)
                Else
                    Me.SECCION = MyBase.SetStringAsString(ColumnNames.SECCION, Value)
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

        Public Overridable Property s_CAPACIDADPROV As String
            Get
                If Me.IsColumnNull(ColumnNames.CAPACIDADPROV) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CAPACIDADPROV)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CAPACIDADPROV)
                Else
                    Me.CAPACIDADPROV = MyBase.SetStringAsString(ColumnNames.CAPACIDADPROV, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CANTIDADFAC As String
            Get
                If Me.IsColumnNull(ColumnNames.CANTIDADFAC) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.CANTIDADFAC)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CANTIDADFAC)
                Else
                    Me.CANTIDADFAC = MyBase.SetDecimalAsString(ColumnNames.CANTIDADFAC, Value)
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

        Public Overridable Property s_CANTIDADPED As String
            Get
                If Me.IsColumnNull(ColumnNames.CANTIDADPED) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.CANTIDADPED)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CANTIDADPED)
                Else
                    Me.CANTIDADPED = MyBase.SetDecimalAsString(ColumnNames.CANTIDADPED, Value)
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

        Public Overridable Property s_NUMPEDORIGEN As String
            Get
                If Me.IsColumnNull(ColumnNames.NUMPEDORIGEN) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.NUMPEDORIGEN)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NUMPEDORIGEN)
                Else
                    Me.NUMPEDORIGEN = MyBase.SetDecimalAsString(ColumnNames.NUMPEDORIGEN, Value)
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

                Public ReadOnly Property IDOFMARCA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IDOFMARCA, Parameters.IDOFMARCA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

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

                Public ReadOnly Property SECCION() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.SECCION, Parameters.SECCION)
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

                Public ReadOnly Property CAPACIDADPROV() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CAPACIDADPROV, Parameters.CAPACIDADPROV)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CANTIDADFAC() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CANTIDADFAC, Parameters.CANTIDADFAC)
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

                Public ReadOnly Property CANTIDADPED() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CANTIDADPED, Parameters.CANTIDADPED)
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

                Public ReadOnly Property NUMPEDORIGEN() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMPEDORIGEN, Parameters.NUMPEDORIGEN)
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

            Public ReadOnly Property IDOFMARCA() As WhereParameter
                Get
                    If _IDOFMARCA_W Is Nothing Then
                        _IDOFMARCA_W = TearOff.IDOFMARCA
                    End If
                    Return _IDOFMARCA_W
                End Get
            End Property

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

            Public ReadOnly Property SECCION() As WhereParameter
                Get
                    If _SECCION_W Is Nothing Then
                        _SECCION_W = TearOff.SECCION
                    End If
                    Return _SECCION_W
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

            Public ReadOnly Property CAPACIDADPROV() As WhereParameter
                Get
                    If _CAPACIDADPROV_W Is Nothing Then
                        _CAPACIDADPROV_W = TearOff.CAPACIDADPROV
                    End If
                    Return _CAPACIDADPROV_W
                End Get
            End Property

            Public ReadOnly Property CANTIDADFAC() As WhereParameter
                Get
                    If _CANTIDADFAC_W Is Nothing Then
                        _CANTIDADFAC_W = TearOff.CANTIDADFAC
                    End If
                    Return _CANTIDADFAC_W
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

            Public ReadOnly Property CANTIDADPED() As WhereParameter
                Get
                    If _CANTIDADPED_W Is Nothing Then
                        _CANTIDADPED_W = TearOff.CANTIDADPED
                    End If
                    Return _CANTIDADPED_W
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

            Public ReadOnly Property NUMPEDORIGEN() As WhereParameter
                Get
                    If _NUMPEDORIGEN_W Is Nothing Then
                        _NUMPEDORIGEN_W = TearOff.NUMPEDORIGEN
                    End If
                    Return _NUMPEDORIGEN_W
                End Get
            End Property

            Private _ID_W As WhereParameter = Nothing
            Private _IDINCIDENCIA_W As WhereParameter = Nothing
            Private _IDOFMARCA_W As WhereParameter = Nothing
            Private _ORIGEN_W As WhereParameter = Nothing
            Private _NUMORD_W As WhereParameter = Nothing
            Private _NUMOPE_W As WhereParameter = Nothing
            Private _NUMMAR_W As WhereParameter = Nothing
            Private _CODPRO_W As WhereParameter = Nothing
            Private _NUMPED_W As WhereParameter = Nothing
            Private _NUMLIN_W As WhereParameter = Nothing
            Private _HORAS_W As WhereParameter = Nothing
            Private _IMPORTE_W As WhereParameter = Nothing
            Private _SECCION_W As WhereParameter = Nothing
            Private _MAQUINA_W As WhereParameter = Nothing
            Private _FASE_W As WhereParameter = Nothing
            Private _PROCESO_W As WhereParameter = Nothing
            Private _CAPACIDADPROV_W As WhereParameter = Nothing
            Private _CANTIDADFAC_W As WhereParameter = Nothing
            Private _DESCRIPCION_W As WhereParameter = Nothing
            Private _CANTIDADPED_W As WhereParameter = Nothing
            Private _CODART_W As WhereParameter = Nothing
            Private _NUMPEDORIGEN_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _ID_W = Nothing
                _IDINCIDENCIA_W = Nothing
                _IDOFMARCA_W = Nothing
                _ORIGEN_W = Nothing
                _NUMORD_W = Nothing
                _NUMOPE_W = Nothing
                _NUMMAR_W = Nothing
                _CODPRO_W = Nothing
                _NUMPED_W = Nothing
                _NUMLIN_W = Nothing
                _HORAS_W = Nothing
                _IMPORTE_W = Nothing
                _SECCION_W = Nothing
                _MAQUINA_W = Nothing
                _FASE_W = Nothing
                _PROCESO_W = Nothing
                _CAPACIDADPROV_W = Nothing
                _CANTIDADFAC_W = Nothing
                _DESCRIPCION_W = Nothing
                _CANTIDADPED_W = Nothing
                _CODART_W = Nothing
                _NUMPEDORIGEN_W = Nothing
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

                Public ReadOnly Property IDOFMARCA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDOFMARCA, Parameters.IDOFMARCA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

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

                Public ReadOnly Property SECCION() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.SECCION, Parameters.SECCION)
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

                Public ReadOnly Property CAPACIDADPROV() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CAPACIDADPROV, Parameters.CAPACIDADPROV)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CANTIDADFAC() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CANTIDADFAC, Parameters.CANTIDADFAC)
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

                Public ReadOnly Property CANTIDADPED() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CANTIDADPED, Parameters.CANTIDADPED)
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

                Public ReadOnly Property NUMPEDORIGEN() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMPEDORIGEN, Parameters.NUMPEDORIGEN)
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

            Public ReadOnly Property IDOFMARCA() As AggregateParameter
                Get
                    If _IDOFMARCA_W Is Nothing Then
                        _IDOFMARCA_W = TearOff.IDOFMARCA
                    End If
                    Return _IDOFMARCA_W
                End Get
            End Property

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

            Public ReadOnly Property SECCION() As AggregateParameter
                Get
                    If _SECCION_W Is Nothing Then
                        _SECCION_W = TearOff.SECCION
                    End If
                    Return _SECCION_W
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

            Public ReadOnly Property CAPACIDADPROV() As AggregateParameter
                Get
                    If _CAPACIDADPROV_W Is Nothing Then
                        _CAPACIDADPROV_W = TearOff.CAPACIDADPROV
                    End If
                    Return _CAPACIDADPROV_W
                End Get
            End Property

            Public ReadOnly Property CANTIDADFAC() As AggregateParameter
                Get
                    If _CANTIDADFAC_W Is Nothing Then
                        _CANTIDADFAC_W = TearOff.CANTIDADFAC
                    End If
                    Return _CANTIDADFAC_W
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

            Public ReadOnly Property CANTIDADPED() As AggregateParameter
                Get
                    If _CANTIDADPED_W Is Nothing Then
                        _CANTIDADPED_W = TearOff.CANTIDADPED
                    End If
                    Return _CANTIDADPED_W
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

            Public ReadOnly Property NUMPEDORIGEN() As AggregateParameter
                Get
                    If _NUMPEDORIGEN_W Is Nothing Then
                        _NUMPEDORIGEN_W = TearOff.NUMPEDORIGEN
                    End If
                    Return _NUMPEDORIGEN_W
                End Get
            End Property

            Private _ID_W As AggregateParameter = Nothing
            Private _IDINCIDENCIA_W As AggregateParameter = Nothing
            Private _IDOFMARCA_W As AggregateParameter = Nothing
            Private _ORIGEN_W As AggregateParameter = Nothing
            Private _NUMORD_W As AggregateParameter = Nothing
            Private _NUMOPE_W As AggregateParameter = Nothing
            Private _NUMMAR_W As AggregateParameter = Nothing
            Private _CODPRO_W As AggregateParameter = Nothing
            Private _NUMPED_W As AggregateParameter = Nothing
            Private _NUMLIN_W As AggregateParameter = Nothing
            Private _HORAS_W As AggregateParameter = Nothing
            Private _IMPORTE_W As AggregateParameter = Nothing
            Private _SECCION_W As AggregateParameter = Nothing
            Private _MAQUINA_W As AggregateParameter = Nothing
            Private _FASE_W As AggregateParameter = Nothing
            Private _PROCESO_W As AggregateParameter = Nothing
            Private _CAPACIDADPROV_W As AggregateParameter = Nothing
            Private _CANTIDADFAC_W As AggregateParameter = Nothing
            Private _DESCRIPCION_W As AggregateParameter = Nothing
            Private _CANTIDADPED_W As AggregateParameter = Nothing
            Private _CODART_W As AggregateParameter = Nothing
            Private _NUMPEDORIGEN_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _ID_W = Nothing
                _IDINCIDENCIA_W = Nothing
                _IDOFMARCA_W = Nothing
                _ORIGEN_W = Nothing
                _NUMORD_W = Nothing
                _NUMOPE_W = Nothing
                _NUMMAR_W = Nothing
                _CODPRO_W = Nothing
                _NUMPED_W = Nothing
                _NUMLIN_W = Nothing
                _HORAS_W = Nothing
                _IMPORTE_W = Nothing
                _SECCION_W = Nothing
                _MAQUINA_W = Nothing
                _FASE_W = Nothing
                _PROCESO_W = Nothing
                _CAPACIDADPROV_W = Nothing
                _CANTIDADFAC_W = Nothing
                _DESCRIPCION_W = Nothing
                _CANTIDADPED_W = Nothing
                _CODART_W = Nothing
                _NUMPEDORIGEN_W = Nothing
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
            cmd.CommandText = Me.SchemaStoredProcedure + "PI_LINEASCOSTE"

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
            cmd.CommandText = Me.SchemaStoredProcedure + "PU_LINEASCOSTE"

            CreateParameters(cmd)

            Return cmd

        End Function

        Protected Overrides Function GetDeleteCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PD_LINEASCOSTE"

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

            p = cmd.Parameters.Add(Parameters.IDINCIDENCIA)
            p.SourceColumn = ColumnNames.IDINCIDENCIA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IDOFMARCA)
            p.SourceColumn = ColumnNames.IDOFMARCA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.ORIGEN)
            p.SourceColumn = ColumnNames.ORIGEN
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMORD)
            p.SourceColumn = ColumnNames.NUMORD
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMOPE)
            p.SourceColumn = ColumnNames.NUMOPE
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMMAR)
            p.SourceColumn = ColumnNames.NUMMAR
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CODPRO)
            p.SourceColumn = ColumnNames.CODPRO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMPED)
            p.SourceColumn = ColumnNames.NUMPED
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMLIN)
            p.SourceColumn = ColumnNames.NUMLIN
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.HORAS)
            p.SourceColumn = ColumnNames.HORAS
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IMPORTE)
            p.SourceColumn = ColumnNames.IMPORTE
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.SECCION)
            p.SourceColumn = ColumnNames.SECCION
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.MAQUINA)
            p.SourceColumn = ColumnNames.MAQUINA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.FASE)
            p.SourceColumn = ColumnNames.FASE
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.PROCESO)
            p.SourceColumn = ColumnNames.PROCESO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CAPACIDADPROV)
            p.SourceColumn = ColumnNames.CAPACIDADPROV
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CANTIDADFAC)
            p.SourceColumn = ColumnNames.CANTIDADFAC
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.DESCRIPCION)
            p.SourceColumn = ColumnNames.DESCRIPCION
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CANTIDADPED)
            p.SourceColumn = ColumnNames.CANTIDADPED
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CODART)
            p.SourceColumn = ColumnNames.CODART
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMPEDORIGEN)
            p.SourceColumn = ColumnNames.NUMPEDORIGEN
            p.SourceVersion = DataRowVersion.Current


        End Sub

    End Class

End Namespace

