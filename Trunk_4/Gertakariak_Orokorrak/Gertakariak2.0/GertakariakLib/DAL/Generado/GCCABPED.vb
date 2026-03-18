
'===============================================================================
'BATZ, Koop. - 27/01/2012 8:02:36
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

    Public MustInherit Class _GCCABPED
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "XBAT."
            Me.QuerySource = "GCCABPED"
            Me.MappingName = "GCCABPED"
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


            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_GCCABPED", parameters)
        End Function

        '=================================================================
        ' Public Overridable Function LoadByPrimaryKey()  As Boolean
        '=================================================================
        '  Loads a single row of via the primary key
        '=================================================================
        Public Overridable Function LoadByPrimaryKey(ByVal NUMPEDCAB As Decimal) As Boolean

            Dim parameters As ListDictionary = New ListDictionary()
            parameters.Add(_GCCABPED.Parameters.NUMPEDCAB, NUMPEDCAB)


            Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
            p.Direction = ParameterDirection.Output
            parameters.Add(p, Nothing)

            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_GCCABPED", parameters)

        End Function

#Region "Parameters"
        Protected Class Parameters

            Public Shared ReadOnly Property NUMPEDCAB As OracleParameter
                Get
                    Return New OracleParameter("p_NUMPEDCAB", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property CODPROCAB As OracleParameter
                Get
                    Return New OracleParameter("p_CODPROCAB", OracleDbType.Char, 12)
                End Get
            End Property

            Public Shared ReadOnly Property CODFORPAG As OracleParameter
                Get
                    Return New OracleParameter("p_CODFORPAG", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property CODPUERTA As OracleParameter
                Get
                    Return New OracleParameter("p_CODPUERTA", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property DTOFIJO As OracleParameter
                Get
                    Return New OracleParameter("p_DTOFIJO", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property DTOPP As OracleParameter
                Get
                    Return New OracleParameter("p_DTOPP", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property RECFINAN As OracleParameter
                Get
                    Return New OracleParameter("p_RECFINAN", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property CODMONEDA As OracleParameter
                Get
                    Return New OracleParameter("p_CODMONEDA", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property TIPIVA As OracleParameter
                Get
                    Return New OracleParameter("p_TIPIVA", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property FECPEDIDO As OracleParameter
                Get
                    Return New OracleParameter("p_FECPEDIDO", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property FECLANZ As OracleParameter
                Get
                    Return New OracleParameter("p_FECLANZ", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property FECENTREG As OracleParameter
                Get
                    Return New OracleParameter("p_FECENTREG", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property ENVIO As OracleParameter
                Get
                    Return New OracleParameter("p_ENVIO", OracleDbType.Char, 30)
                End Get
            End Property

            Public Shared ReadOnly Property TEXTO As OracleParameter
                Get
                    Return New OracleParameter("p_TEXTO", OracleDbType.Char, 30)
                End Get
            End Property

            Public Shared ReadOnly Property LANGILE As OracleParameter
                Get
                    Return New OracleParameter("p_LANGILE", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property NUMORDF As OracleParameter
                Get
                    Return New OracleParameter("p_NUMORDF", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property NUMOPE As OracleParameter
                Get
                    Return New OracleParameter("p_NUMOPE", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property BLOKEO As OracleParameter
                Get
                    Return New OracleParameter("p_BLOKEO", OracleDbType.Char, 1)
                End Get
            End Property

            Public Shared ReadOnly Property USUARIO_EMISION As OracleParameter
                Get
                    Return New OracleParameter("p_USUARIO_EMISION", OracleDbType.Varchar2, 20)
                End Get
            End Property

            Public Shared ReadOnly Property FECHA_EMISION As OracleParameter
                Get
                    Return New OracleParameter("p_FECHA_EMISION", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property FICHERO_EMISION As OracleParameter
                Get
                    Return New OracleParameter("p_FICHERO_EMISION", OracleDbType.Varchar2, 30)
                End Get
            End Property

            Public Shared ReadOnly Property URGENTE As OracleParameter
                Get
                    Return New OracleParameter("p_URGENTE", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property CONFIRMADO As OracleParameter
                Get
                    Return New OracleParameter("p_CONFIRMADO", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property PORTES As OracleParameter
                Get
                    Return New OracleParameter("p_PORTES", OracleDbType.Varchar2, 1)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const NUMPEDCAB As String = "NUMPEDCAB"
            Public Const CODPROCAB As String = "CODPROCAB"
            Public Const CODFORPAG As String = "CODFORPAG"
            Public Const CODPUERTA As String = "CODPUERTA"
            Public Const DTOFIJO As String = "DTOFIJO"
            Public Const DTOPP As String = "DTOPP"
            Public Const RECFINAN As String = "RECFINAN"
            Public Const CODMONEDA As String = "CODMONEDA"
            Public Const TIPIVA As String = "TIPIVA"
            Public Const FECPEDIDO As String = "FECPEDIDO"
            Public Const FECLANZ As String = "FECLANZ"
            Public Const FECENTREG As String = "FECENTREG"
            Public Const ENVIO As String = "ENVIO"
            Public Const TEXTO As String = "TEXTO"
            Public Const LANGILE As String = "LANGILE"
            Public Const NUMORDF As String = "NUMORDF"
            Public Const NUMOPE As String = "NUMOPE"
            Public Const BLOKEO As String = "BLOKEO"
            Public Const USUARIO_EMISION As String = "USUARIO_EMISION"
            Public Const FECHA_EMISION As String = "FECHA_EMISION"
            Public Const FICHERO_EMISION As String = "FICHERO_EMISION"
            Public Const URGENTE As String = "URGENTE"
            Public Const CONFIRMADO As String = "CONFIRMADO"
            Public Const PORTES As String = "PORTES"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(NUMPEDCAB) = _GCCABPED.PropertyNames.NUMPEDCAB
                    ht(CODPROCAB) = _GCCABPED.PropertyNames.CODPROCAB
                    ht(CODFORPAG) = _GCCABPED.PropertyNames.CODFORPAG
                    ht(CODPUERTA) = _GCCABPED.PropertyNames.CODPUERTA
                    ht(DTOFIJO) = _GCCABPED.PropertyNames.DTOFIJO
                    ht(DTOPP) = _GCCABPED.PropertyNames.DTOPP
                    ht(RECFINAN) = _GCCABPED.PropertyNames.RECFINAN
                    ht(CODMONEDA) = _GCCABPED.PropertyNames.CODMONEDA
                    ht(TIPIVA) = _GCCABPED.PropertyNames.TIPIVA
                    ht(FECPEDIDO) = _GCCABPED.PropertyNames.FECPEDIDO
                    ht(FECLANZ) = _GCCABPED.PropertyNames.FECLANZ
                    ht(FECENTREG) = _GCCABPED.PropertyNames.FECENTREG
                    ht(ENVIO) = _GCCABPED.PropertyNames.ENVIO
                    ht(TEXTO) = _GCCABPED.PropertyNames.TEXTO
                    ht(LANGILE) = _GCCABPED.PropertyNames.LANGILE
                    ht(NUMORDF) = _GCCABPED.PropertyNames.NUMORDF
                    ht(NUMOPE) = _GCCABPED.PropertyNames.NUMOPE
                    ht(BLOKEO) = _GCCABPED.PropertyNames.BLOKEO
                    ht(USUARIO_EMISION) = _GCCABPED.PropertyNames.USUARIO_EMISION
                    ht(FECHA_EMISION) = _GCCABPED.PropertyNames.FECHA_EMISION
                    ht(FICHERO_EMISION) = _GCCABPED.PropertyNames.FICHERO_EMISION
                    ht(URGENTE) = _GCCABPED.PropertyNames.URGENTE
                    ht(CONFIRMADO) = _GCCABPED.PropertyNames.CONFIRMADO
                    ht(PORTES) = _GCCABPED.PropertyNames.PORTES

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const NUMPEDCAB As String = "NUMPEDCAB"
            Public Const CODPROCAB As String = "CODPROCAB"
            Public Const CODFORPAG As String = "CODFORPAG"
            Public Const CODPUERTA As String = "CODPUERTA"
            Public Const DTOFIJO As String = "DTOFIJO"
            Public Const DTOPP As String = "DTOPP"
            Public Const RECFINAN As String = "RECFINAN"
            Public Const CODMONEDA As String = "CODMONEDA"
            Public Const TIPIVA As String = "TIPIVA"
            Public Const FECPEDIDO As String = "FECPEDIDO"
            Public Const FECLANZ As String = "FECLANZ"
            Public Const FECENTREG As String = "FECENTREG"
            Public Const ENVIO As String = "ENVIO"
            Public Const TEXTO As String = "TEXTO"
            Public Const LANGILE As String = "LANGILE"
            Public Const NUMORDF As String = "NUMORDF"
            Public Const NUMOPE As String = "NUMOPE"
            Public Const BLOKEO As String = "BLOKEO"
            Public Const USUARIO_EMISION As String = "USUARIO_EMISION"
            Public Const FECHA_EMISION As String = "FECHA_EMISION"
            Public Const FICHERO_EMISION As String = "FICHERO_EMISION"
            Public Const URGENTE As String = "URGENTE"
            Public Const CONFIRMADO As String = "CONFIRMADO"
            Public Const PORTES As String = "PORTES"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(NUMPEDCAB) = _GCCABPED.ColumnNames.NUMPEDCAB
                    ht(CODPROCAB) = _GCCABPED.ColumnNames.CODPROCAB
                    ht(CODFORPAG) = _GCCABPED.ColumnNames.CODFORPAG
                    ht(CODPUERTA) = _GCCABPED.ColumnNames.CODPUERTA
                    ht(DTOFIJO) = _GCCABPED.ColumnNames.DTOFIJO
                    ht(DTOPP) = _GCCABPED.ColumnNames.DTOPP
                    ht(RECFINAN) = _GCCABPED.ColumnNames.RECFINAN
                    ht(CODMONEDA) = _GCCABPED.ColumnNames.CODMONEDA
                    ht(TIPIVA) = _GCCABPED.ColumnNames.TIPIVA
                    ht(FECPEDIDO) = _GCCABPED.ColumnNames.FECPEDIDO
                    ht(FECLANZ) = _GCCABPED.ColumnNames.FECLANZ
                    ht(FECENTREG) = _GCCABPED.ColumnNames.FECENTREG
                    ht(ENVIO) = _GCCABPED.ColumnNames.ENVIO
                    ht(TEXTO) = _GCCABPED.ColumnNames.TEXTO
                    ht(LANGILE) = _GCCABPED.ColumnNames.LANGILE
                    ht(NUMORDF) = _GCCABPED.ColumnNames.NUMORDF
                    ht(NUMOPE) = _GCCABPED.ColumnNames.NUMOPE
                    ht(BLOKEO) = _GCCABPED.ColumnNames.BLOKEO
                    ht(USUARIO_EMISION) = _GCCABPED.ColumnNames.USUARIO_EMISION
                    ht(FECHA_EMISION) = _GCCABPED.ColumnNames.FECHA_EMISION
                    ht(FICHERO_EMISION) = _GCCABPED.ColumnNames.FICHERO_EMISION
                    ht(URGENTE) = _GCCABPED.ColumnNames.URGENTE
                    ht(CONFIRMADO) = _GCCABPED.ColumnNames.CONFIRMADO
                    ht(PORTES) = _GCCABPED.ColumnNames.PORTES

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const NUMPEDCAB As String = "s_NUMPEDCAB"
            Public Const CODPROCAB As String = "s_CODPROCAB"
            Public Const CODFORPAG As String = "s_CODFORPAG"
            Public Const CODPUERTA As String = "s_CODPUERTA"
            Public Const DTOFIJO As String = "s_DTOFIJO"
            Public Const DTOPP As String = "s_DTOPP"
            Public Const RECFINAN As String = "s_RECFINAN"
            Public Const CODMONEDA As String = "s_CODMONEDA"
            Public Const TIPIVA As String = "s_TIPIVA"
            Public Const FECPEDIDO As String = "s_FECPEDIDO"
            Public Const FECLANZ As String = "s_FECLANZ"
            Public Const FECENTREG As String = "s_FECENTREG"
            Public Const ENVIO As String = "s_ENVIO"
            Public Const TEXTO As String = "s_TEXTO"
            Public Const LANGILE As String = "s_LANGILE"
            Public Const NUMORDF As String = "s_NUMORDF"
            Public Const NUMOPE As String = "s_NUMOPE"
            Public Const BLOKEO As String = "s_BLOKEO"
            Public Const USUARIO_EMISION As String = "s_USUARIO_EMISION"
            Public Const FECHA_EMISION As String = "s_FECHA_EMISION"
            Public Const FICHERO_EMISION As String = "s_FICHERO_EMISION"
            Public Const URGENTE As String = "s_URGENTE"
            Public Const CONFIRMADO As String = "s_CONFIRMADO"
            Public Const PORTES As String = "s_PORTES"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property NUMPEDCAB As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.NUMPEDCAB)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.NUMPEDCAB, Value)
            End Set
        End Property

        Public Overridable Property CODPROCAB As String
            Get
                Return MyBase.GetString(ColumnNames.CODPROCAB)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CODPROCAB, Value)
            End Set
        End Property

        Public Overridable Property CODFORPAG As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.CODFORPAG)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.CODFORPAG, Value)
            End Set
        End Property

        Public Overridable Property CODPUERTA As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.CODPUERTA)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.CODPUERTA, Value)
            End Set
        End Property

        Public Overridable Property DTOFIJO As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.DTOFIJO)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.DTOFIJO, Value)
            End Set
        End Property

        Public Overridable Property DTOPP As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.DTOPP)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.DTOPP, Value)
            End Set
        End Property

        Public Overridable Property RECFINAN As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.RECFINAN)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.RECFINAN, Value)
            End Set
        End Property

        Public Overridable Property CODMONEDA As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.CODMONEDA)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.CODMONEDA, Value)
            End Set
        End Property

        Public Overridable Property TIPIVA As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.TIPIVA)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.TIPIVA, Value)
            End Set
        End Property

        Public Overridable Property FECPEDIDO As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECPEDIDO)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECPEDIDO, Value)
            End Set
        End Property

        Public Overridable Property FECLANZ As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECLANZ)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECLANZ, Value)
            End Set
        End Property

        Public Overridable Property FECENTREG As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECENTREG)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECENTREG, Value)
            End Set
        End Property

        Public Overridable Property ENVIO As String
            Get
                Return MyBase.GetString(ColumnNames.ENVIO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.ENVIO, Value)
            End Set
        End Property

        Public Overridable Property TEXTO As String
            Get
                Return MyBase.GetString(ColumnNames.TEXTO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.TEXTO, Value)
            End Set
        End Property

        Public Overridable Property LANGILE As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.LANGILE)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.LANGILE, Value)
            End Set
        End Property

        Public Overridable Property NUMORDF As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.NUMORDF)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.NUMORDF, Value)
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

        Public Overridable Property BLOKEO As String
            Get
                Return MyBase.GetString(ColumnNames.BLOKEO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.BLOKEO, Value)
            End Set
        End Property

        Public Overridable Property USUARIO_EMISION As String
            Get
                Return MyBase.GetString(ColumnNames.USUARIO_EMISION)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.USUARIO_EMISION, Value)
            End Set
        End Property

        Public Overridable Property FECHA_EMISION As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECHA_EMISION)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECHA_EMISION, Value)
            End Set
        End Property

        Public Overridable Property FICHERO_EMISION As String
            Get
                Return MyBase.GetString(ColumnNames.FICHERO_EMISION)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.FICHERO_EMISION, Value)
            End Set
        End Property

        Public Overridable Property URGENTE As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.URGENTE)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.URGENTE, Value)
            End Set
        End Property

        Public Overridable Property CONFIRMADO As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.CONFIRMADO)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.CONFIRMADO, Value)
            End Set
        End Property

        Public Overridable Property PORTES As String
            Get
                Return MyBase.GetString(ColumnNames.PORTES)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.PORTES, Value)
            End Set
        End Property


#End Region

#Region "String Properties"

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

        Public Overridable Property s_CODPROCAB As String
            Get
                If Me.IsColumnNull(ColumnNames.CODPROCAB) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CODPROCAB)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CODPROCAB)
                Else
                    Me.CODPROCAB = MyBase.SetStringAsString(ColumnNames.CODPROCAB, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CODFORPAG As String
            Get
                If Me.IsColumnNull(ColumnNames.CODFORPAG) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.CODFORPAG)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CODFORPAG)
                Else
                    Me.CODFORPAG = MyBase.SetDecimalAsString(ColumnNames.CODFORPAG, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CODPUERTA As String
            Get
                If Me.IsColumnNull(ColumnNames.CODPUERTA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.CODPUERTA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CODPUERTA)
                Else
                    Me.CODPUERTA = MyBase.SetDecimalAsString(ColumnNames.CODPUERTA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_DTOFIJO As String
            Get
                If Me.IsColumnNull(ColumnNames.DTOFIJO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.DTOFIJO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.DTOFIJO)
                Else
                    Me.DTOFIJO = MyBase.SetDecimalAsString(ColumnNames.DTOFIJO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_DTOPP As String
            Get
                If Me.IsColumnNull(ColumnNames.DTOPP) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.DTOPP)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.DTOPP)
                Else
                    Me.DTOPP = MyBase.SetDecimalAsString(ColumnNames.DTOPP, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_RECFINAN As String
            Get
                If Me.IsColumnNull(ColumnNames.RECFINAN) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.RECFINAN)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.RECFINAN)
                Else
                    Me.RECFINAN = MyBase.SetDecimalAsString(ColumnNames.RECFINAN, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CODMONEDA As String
            Get
                If Me.IsColumnNull(ColumnNames.CODMONEDA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.CODMONEDA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CODMONEDA)
                Else
                    Me.CODMONEDA = MyBase.SetDecimalAsString(ColumnNames.CODMONEDA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_TIPIVA As String
            Get
                If Me.IsColumnNull(ColumnNames.TIPIVA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.TIPIVA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.TIPIVA)
                Else
                    Me.TIPIVA = MyBase.SetDecimalAsString(ColumnNames.TIPIVA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FECPEDIDO As String
            Get
                If Me.IsColumnNull(ColumnNames.FECPEDIDO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FECPEDIDO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FECPEDIDO)
                Else
                    Me.FECPEDIDO = MyBase.SetDateTimeAsString(ColumnNames.FECPEDIDO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FECLANZ As String
            Get
                If Me.IsColumnNull(ColumnNames.FECLANZ) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FECLANZ)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FECLANZ)
                Else
                    Me.FECLANZ = MyBase.SetDateTimeAsString(ColumnNames.FECLANZ, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FECENTREG As String
            Get
                If Me.IsColumnNull(ColumnNames.FECENTREG) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FECENTREG)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FECENTREG)
                Else
                    Me.FECENTREG = MyBase.SetDateTimeAsString(ColumnNames.FECENTREG, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_ENVIO As String
            Get
                If Me.IsColumnNull(ColumnNames.ENVIO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.ENVIO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.ENVIO)
                Else
                    Me.ENVIO = MyBase.SetStringAsString(ColumnNames.ENVIO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_TEXTO As String
            Get
                If Me.IsColumnNull(ColumnNames.TEXTO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.TEXTO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.TEXTO)
                Else
                    Me.TEXTO = MyBase.SetStringAsString(ColumnNames.TEXTO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_LANGILE As String
            Get
                If Me.IsColumnNull(ColumnNames.LANGILE) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.LANGILE)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.LANGILE)
                Else
                    Me.LANGILE = MyBase.SetDecimalAsString(ColumnNames.LANGILE, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_NUMORDF As String
            Get
                If Me.IsColumnNull(ColumnNames.NUMORDF) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.NUMORDF)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NUMORDF)
                Else
                    Me.NUMORDF = MyBase.SetDecimalAsString(ColumnNames.NUMORDF, Value)
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

        Public Overridable Property s_BLOKEO As String
            Get
                If Me.IsColumnNull(ColumnNames.BLOKEO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.BLOKEO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.BLOKEO)
                Else
                    Me.BLOKEO = MyBase.SetStringAsString(ColumnNames.BLOKEO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_USUARIO_EMISION As String
            Get
                If Me.IsColumnNull(ColumnNames.USUARIO_EMISION) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.USUARIO_EMISION)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.USUARIO_EMISION)
                Else
                    Me.USUARIO_EMISION = MyBase.SetStringAsString(ColumnNames.USUARIO_EMISION, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FECHA_EMISION As String
            Get
                If Me.IsColumnNull(ColumnNames.FECHA_EMISION) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FECHA_EMISION)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FECHA_EMISION)
                Else
                    Me.FECHA_EMISION = MyBase.SetDateTimeAsString(ColumnNames.FECHA_EMISION, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FICHERO_EMISION As String
            Get
                If Me.IsColumnNull(ColumnNames.FICHERO_EMISION) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.FICHERO_EMISION)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FICHERO_EMISION)
                Else
                    Me.FICHERO_EMISION = MyBase.SetStringAsString(ColumnNames.FICHERO_EMISION, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_URGENTE As String
            Get
                If Me.IsColumnNull(ColumnNames.URGENTE) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.URGENTE)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.URGENTE)
                Else
                    Me.URGENTE = MyBase.SetDecimalAsString(ColumnNames.URGENTE, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CONFIRMADO As String
            Get
                If Me.IsColumnNull(ColumnNames.CONFIRMADO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.CONFIRMADO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CONFIRMADO)
                Else
                    Me.CONFIRMADO = MyBase.SetDecimalAsString(ColumnNames.CONFIRMADO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_PORTES As String
            Get
                If Me.IsColumnNull(ColumnNames.PORTES) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.PORTES)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.PORTES)
                Else
                    Me.PORTES = MyBase.SetStringAsString(ColumnNames.PORTES, Value)
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


                Public ReadOnly Property NUMPEDCAB() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMPEDCAB, Parameters.NUMPEDCAB)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODPROCAB() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CODPROCAB, Parameters.CODPROCAB)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODFORPAG() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CODFORPAG, Parameters.CODFORPAG)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODPUERTA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CODPUERTA, Parameters.CODPUERTA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DTOFIJO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.DTOFIJO, Parameters.DTOFIJO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DTOPP() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.DTOPP, Parameters.DTOPP)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property RECFINAN() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.RECFINAN, Parameters.RECFINAN)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODMONEDA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CODMONEDA, Parameters.CODMONEDA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TIPIVA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.TIPIVA, Parameters.TIPIVA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECPEDIDO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FECPEDIDO, Parameters.FECPEDIDO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECLANZ() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FECLANZ, Parameters.FECLANZ)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECENTREG() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FECENTREG, Parameters.FECENTREG)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ENVIO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.ENVIO, Parameters.ENVIO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TEXTO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.TEXTO, Parameters.TEXTO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property LANGILE() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.LANGILE, Parameters.LANGILE)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMORDF() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMORDF, Parameters.NUMORDF)
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

                Public ReadOnly Property BLOKEO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.BLOKEO, Parameters.BLOKEO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property USUARIO_EMISION() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.USUARIO_EMISION, Parameters.USUARIO_EMISION)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECHA_EMISION() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FECHA_EMISION, Parameters.FECHA_EMISION)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FICHERO_EMISION() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FICHERO_EMISION, Parameters.FICHERO_EMISION)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property URGENTE() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.URGENTE, Parameters.URGENTE)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CONFIRMADO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CONFIRMADO, Parameters.CONFIRMADO)
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


                Private _clause As WhereClause
            End Class
#End Region

            Public ReadOnly Property NUMPEDCAB() As WhereParameter
                Get
                    If _NUMPEDCAB_W Is Nothing Then
                        _NUMPEDCAB_W = TearOff.NUMPEDCAB
                    End If
                    Return _NUMPEDCAB_W
                End Get
            End Property

            Public ReadOnly Property CODPROCAB() As WhereParameter
                Get
                    If _CODPROCAB_W Is Nothing Then
                        _CODPROCAB_W = TearOff.CODPROCAB
                    End If
                    Return _CODPROCAB_W
                End Get
            End Property

            Public ReadOnly Property CODFORPAG() As WhereParameter
                Get
                    If _CODFORPAG_W Is Nothing Then
                        _CODFORPAG_W = TearOff.CODFORPAG
                    End If
                    Return _CODFORPAG_W
                End Get
            End Property

            Public ReadOnly Property CODPUERTA() As WhereParameter
                Get
                    If _CODPUERTA_W Is Nothing Then
                        _CODPUERTA_W = TearOff.CODPUERTA
                    End If
                    Return _CODPUERTA_W
                End Get
            End Property

            Public ReadOnly Property DTOFIJO() As WhereParameter
                Get
                    If _DTOFIJO_W Is Nothing Then
                        _DTOFIJO_W = TearOff.DTOFIJO
                    End If
                    Return _DTOFIJO_W
                End Get
            End Property

            Public ReadOnly Property DTOPP() As WhereParameter
                Get
                    If _DTOPP_W Is Nothing Then
                        _DTOPP_W = TearOff.DTOPP
                    End If
                    Return _DTOPP_W
                End Get
            End Property

            Public ReadOnly Property RECFINAN() As WhereParameter
                Get
                    If _RECFINAN_W Is Nothing Then
                        _RECFINAN_W = TearOff.RECFINAN
                    End If
                    Return _RECFINAN_W
                End Get
            End Property

            Public ReadOnly Property CODMONEDA() As WhereParameter
                Get
                    If _CODMONEDA_W Is Nothing Then
                        _CODMONEDA_W = TearOff.CODMONEDA
                    End If
                    Return _CODMONEDA_W
                End Get
            End Property

            Public ReadOnly Property TIPIVA() As WhereParameter
                Get
                    If _TIPIVA_W Is Nothing Then
                        _TIPIVA_W = TearOff.TIPIVA
                    End If
                    Return _TIPIVA_W
                End Get
            End Property

            Public ReadOnly Property FECPEDIDO() As WhereParameter
                Get
                    If _FECPEDIDO_W Is Nothing Then
                        _FECPEDIDO_W = TearOff.FECPEDIDO
                    End If
                    Return _FECPEDIDO_W
                End Get
            End Property

            Public ReadOnly Property FECLANZ() As WhereParameter
                Get
                    If _FECLANZ_W Is Nothing Then
                        _FECLANZ_W = TearOff.FECLANZ
                    End If
                    Return _FECLANZ_W
                End Get
            End Property

            Public ReadOnly Property FECENTREG() As WhereParameter
                Get
                    If _FECENTREG_W Is Nothing Then
                        _FECENTREG_W = TearOff.FECENTREG
                    End If
                    Return _FECENTREG_W
                End Get
            End Property

            Public ReadOnly Property ENVIO() As WhereParameter
                Get
                    If _ENVIO_W Is Nothing Then
                        _ENVIO_W = TearOff.ENVIO
                    End If
                    Return _ENVIO_W
                End Get
            End Property

            Public ReadOnly Property TEXTO() As WhereParameter
                Get
                    If _TEXTO_W Is Nothing Then
                        _TEXTO_W = TearOff.TEXTO
                    End If
                    Return _TEXTO_W
                End Get
            End Property

            Public ReadOnly Property LANGILE() As WhereParameter
                Get
                    If _LANGILE_W Is Nothing Then
                        _LANGILE_W = TearOff.LANGILE
                    End If
                    Return _LANGILE_W
                End Get
            End Property

            Public ReadOnly Property NUMORDF() As WhereParameter
                Get
                    If _NUMORDF_W Is Nothing Then
                        _NUMORDF_W = TearOff.NUMORDF
                    End If
                    Return _NUMORDF_W
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

            Public ReadOnly Property BLOKEO() As WhereParameter
                Get
                    If _BLOKEO_W Is Nothing Then
                        _BLOKEO_W = TearOff.BLOKEO
                    End If
                    Return _BLOKEO_W
                End Get
            End Property

            Public ReadOnly Property USUARIO_EMISION() As WhereParameter
                Get
                    If _USUARIO_EMISION_W Is Nothing Then
                        _USUARIO_EMISION_W = TearOff.USUARIO_EMISION
                    End If
                    Return _USUARIO_EMISION_W
                End Get
            End Property

            Public ReadOnly Property FECHA_EMISION() As WhereParameter
                Get
                    If _FECHA_EMISION_W Is Nothing Then
                        _FECHA_EMISION_W = TearOff.FECHA_EMISION
                    End If
                    Return _FECHA_EMISION_W
                End Get
            End Property

            Public ReadOnly Property FICHERO_EMISION() As WhereParameter
                Get
                    If _FICHERO_EMISION_W Is Nothing Then
                        _FICHERO_EMISION_W = TearOff.FICHERO_EMISION
                    End If
                    Return _FICHERO_EMISION_W
                End Get
            End Property

            Public ReadOnly Property URGENTE() As WhereParameter
                Get
                    If _URGENTE_W Is Nothing Then
                        _URGENTE_W = TearOff.URGENTE
                    End If
                    Return _URGENTE_W
                End Get
            End Property

            Public ReadOnly Property CONFIRMADO() As WhereParameter
                Get
                    If _CONFIRMADO_W Is Nothing Then
                        _CONFIRMADO_W = TearOff.CONFIRMADO
                    End If
                    Return _CONFIRMADO_W
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

            Private _NUMPEDCAB_W As WhereParameter = Nothing
            Private _CODPROCAB_W As WhereParameter = Nothing
            Private _CODFORPAG_W As WhereParameter = Nothing
            Private _CODPUERTA_W As WhereParameter = Nothing
            Private _DTOFIJO_W As WhereParameter = Nothing
            Private _DTOPP_W As WhereParameter = Nothing
            Private _RECFINAN_W As WhereParameter = Nothing
            Private _CODMONEDA_W As WhereParameter = Nothing
            Private _TIPIVA_W As WhereParameter = Nothing
            Private _FECPEDIDO_W As WhereParameter = Nothing
            Private _FECLANZ_W As WhereParameter = Nothing
            Private _FECENTREG_W As WhereParameter = Nothing
            Private _ENVIO_W As WhereParameter = Nothing
            Private _TEXTO_W As WhereParameter = Nothing
            Private _LANGILE_W As WhereParameter = Nothing
            Private _NUMORDF_W As WhereParameter = Nothing
            Private _NUMOPE_W As WhereParameter = Nothing
            Private _BLOKEO_W As WhereParameter = Nothing
            Private _USUARIO_EMISION_W As WhereParameter = Nothing
            Private _FECHA_EMISION_W As WhereParameter = Nothing
            Private _FICHERO_EMISION_W As WhereParameter = Nothing
            Private _URGENTE_W As WhereParameter = Nothing
            Private _CONFIRMADO_W As WhereParameter = Nothing
            Private _PORTES_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _NUMPEDCAB_W = Nothing
                _CODPROCAB_W = Nothing
                _CODFORPAG_W = Nothing
                _CODPUERTA_W = Nothing
                _DTOFIJO_W = Nothing
                _DTOPP_W = Nothing
                _RECFINAN_W = Nothing
                _CODMONEDA_W = Nothing
                _TIPIVA_W = Nothing
                _FECPEDIDO_W = Nothing
                _FECLANZ_W = Nothing
                _FECENTREG_W = Nothing
                _ENVIO_W = Nothing
                _TEXTO_W = Nothing
                _LANGILE_W = Nothing
                _NUMORDF_W = Nothing
                _NUMOPE_W = Nothing
                _BLOKEO_W = Nothing
                _USUARIO_EMISION_W = Nothing
                _FECHA_EMISION_W = Nothing
                _FICHERO_EMISION_W = Nothing
                _URGENTE_W = Nothing
                _CONFIRMADO_W = Nothing
                _PORTES_W = Nothing
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


                Public ReadOnly Property NUMPEDCAB() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMPEDCAB, Parameters.NUMPEDCAB)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODPROCAB() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CODPROCAB, Parameters.CODPROCAB)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODFORPAG() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CODFORPAG, Parameters.CODFORPAG)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODPUERTA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CODPUERTA, Parameters.CODPUERTA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DTOFIJO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DTOFIJO, Parameters.DTOFIJO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DTOPP() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DTOPP, Parameters.DTOPP)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property RECFINAN() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.RECFINAN, Parameters.RECFINAN)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODMONEDA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CODMONEDA, Parameters.CODMONEDA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TIPIVA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TIPIVA, Parameters.TIPIVA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECPEDIDO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECPEDIDO, Parameters.FECPEDIDO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECLANZ() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECLANZ, Parameters.FECLANZ)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECENTREG() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECENTREG, Parameters.FECENTREG)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ENVIO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ENVIO, Parameters.ENVIO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TEXTO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TEXTO, Parameters.TEXTO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property LANGILE() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.LANGILE, Parameters.LANGILE)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMORDF() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMORDF, Parameters.NUMORDF)
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

                Public ReadOnly Property BLOKEO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.BLOKEO, Parameters.BLOKEO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property USUARIO_EMISION() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.USUARIO_EMISION, Parameters.USUARIO_EMISION)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECHA_EMISION() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECHA_EMISION, Parameters.FECHA_EMISION)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FICHERO_EMISION() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FICHERO_EMISION, Parameters.FICHERO_EMISION)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property URGENTE() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.URGENTE, Parameters.URGENTE)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CONFIRMADO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CONFIRMADO, Parameters.CONFIRMADO)
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


                Private _clause As AggregateClause
            End Class
#End Region

            Public ReadOnly Property NUMPEDCAB() As AggregateParameter
                Get
                    If _NUMPEDCAB_W Is Nothing Then
                        _NUMPEDCAB_W = TearOff.NUMPEDCAB
                    End If
                    Return _NUMPEDCAB_W
                End Get
            End Property

            Public ReadOnly Property CODPROCAB() As AggregateParameter
                Get
                    If _CODPROCAB_W Is Nothing Then
                        _CODPROCAB_W = TearOff.CODPROCAB
                    End If
                    Return _CODPROCAB_W
                End Get
            End Property

            Public ReadOnly Property CODFORPAG() As AggregateParameter
                Get
                    If _CODFORPAG_W Is Nothing Then
                        _CODFORPAG_W = TearOff.CODFORPAG
                    End If
                    Return _CODFORPAG_W
                End Get
            End Property

            Public ReadOnly Property CODPUERTA() As AggregateParameter
                Get
                    If _CODPUERTA_W Is Nothing Then
                        _CODPUERTA_W = TearOff.CODPUERTA
                    End If
                    Return _CODPUERTA_W
                End Get
            End Property

            Public ReadOnly Property DTOFIJO() As AggregateParameter
                Get
                    If _DTOFIJO_W Is Nothing Then
                        _DTOFIJO_W = TearOff.DTOFIJO
                    End If
                    Return _DTOFIJO_W
                End Get
            End Property

            Public ReadOnly Property DTOPP() As AggregateParameter
                Get
                    If _DTOPP_W Is Nothing Then
                        _DTOPP_W = TearOff.DTOPP
                    End If
                    Return _DTOPP_W
                End Get
            End Property

            Public ReadOnly Property RECFINAN() As AggregateParameter
                Get
                    If _RECFINAN_W Is Nothing Then
                        _RECFINAN_W = TearOff.RECFINAN
                    End If
                    Return _RECFINAN_W
                End Get
            End Property

            Public ReadOnly Property CODMONEDA() As AggregateParameter
                Get
                    If _CODMONEDA_W Is Nothing Then
                        _CODMONEDA_W = TearOff.CODMONEDA
                    End If
                    Return _CODMONEDA_W
                End Get
            End Property

            Public ReadOnly Property TIPIVA() As AggregateParameter
                Get
                    If _TIPIVA_W Is Nothing Then
                        _TIPIVA_W = TearOff.TIPIVA
                    End If
                    Return _TIPIVA_W
                End Get
            End Property

            Public ReadOnly Property FECPEDIDO() As AggregateParameter
                Get
                    If _FECPEDIDO_W Is Nothing Then
                        _FECPEDIDO_W = TearOff.FECPEDIDO
                    End If
                    Return _FECPEDIDO_W
                End Get
            End Property

            Public ReadOnly Property FECLANZ() As AggregateParameter
                Get
                    If _FECLANZ_W Is Nothing Then
                        _FECLANZ_W = TearOff.FECLANZ
                    End If
                    Return _FECLANZ_W
                End Get
            End Property

            Public ReadOnly Property FECENTREG() As AggregateParameter
                Get
                    If _FECENTREG_W Is Nothing Then
                        _FECENTREG_W = TearOff.FECENTREG
                    End If
                    Return _FECENTREG_W
                End Get
            End Property

            Public ReadOnly Property ENVIO() As AggregateParameter
                Get
                    If _ENVIO_W Is Nothing Then
                        _ENVIO_W = TearOff.ENVIO
                    End If
                    Return _ENVIO_W
                End Get
            End Property

            Public ReadOnly Property TEXTO() As AggregateParameter
                Get
                    If _TEXTO_W Is Nothing Then
                        _TEXTO_W = TearOff.TEXTO
                    End If
                    Return _TEXTO_W
                End Get
            End Property

            Public ReadOnly Property LANGILE() As AggregateParameter
                Get
                    If _LANGILE_W Is Nothing Then
                        _LANGILE_W = TearOff.LANGILE
                    End If
                    Return _LANGILE_W
                End Get
            End Property

            Public ReadOnly Property NUMORDF() As AggregateParameter
                Get
                    If _NUMORDF_W Is Nothing Then
                        _NUMORDF_W = TearOff.NUMORDF
                    End If
                    Return _NUMORDF_W
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

            Public ReadOnly Property BLOKEO() As AggregateParameter
                Get
                    If _BLOKEO_W Is Nothing Then
                        _BLOKEO_W = TearOff.BLOKEO
                    End If
                    Return _BLOKEO_W
                End Get
            End Property

            Public ReadOnly Property USUARIO_EMISION() As AggregateParameter
                Get
                    If _USUARIO_EMISION_W Is Nothing Then
                        _USUARIO_EMISION_W = TearOff.USUARIO_EMISION
                    End If
                    Return _USUARIO_EMISION_W
                End Get
            End Property

            Public ReadOnly Property FECHA_EMISION() As AggregateParameter
                Get
                    If _FECHA_EMISION_W Is Nothing Then
                        _FECHA_EMISION_W = TearOff.FECHA_EMISION
                    End If
                    Return _FECHA_EMISION_W
                End Get
            End Property

            Public ReadOnly Property FICHERO_EMISION() As AggregateParameter
                Get
                    If _FICHERO_EMISION_W Is Nothing Then
                        _FICHERO_EMISION_W = TearOff.FICHERO_EMISION
                    End If
                    Return _FICHERO_EMISION_W
                End Get
            End Property

            Public ReadOnly Property URGENTE() As AggregateParameter
                Get
                    If _URGENTE_W Is Nothing Then
                        _URGENTE_W = TearOff.URGENTE
                    End If
                    Return _URGENTE_W
                End Get
            End Property

            Public ReadOnly Property CONFIRMADO() As AggregateParameter
                Get
                    If _CONFIRMADO_W Is Nothing Then
                        _CONFIRMADO_W = TearOff.CONFIRMADO
                    End If
                    Return _CONFIRMADO_W
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

            Private _NUMPEDCAB_W As AggregateParameter = Nothing
            Private _CODPROCAB_W As AggregateParameter = Nothing
            Private _CODFORPAG_W As AggregateParameter = Nothing
            Private _CODPUERTA_W As AggregateParameter = Nothing
            Private _DTOFIJO_W As AggregateParameter = Nothing
            Private _DTOPP_W As AggregateParameter = Nothing
            Private _RECFINAN_W As AggregateParameter = Nothing
            Private _CODMONEDA_W As AggregateParameter = Nothing
            Private _TIPIVA_W As AggregateParameter = Nothing
            Private _FECPEDIDO_W As AggregateParameter = Nothing
            Private _FECLANZ_W As AggregateParameter = Nothing
            Private _FECENTREG_W As AggregateParameter = Nothing
            Private _ENVIO_W As AggregateParameter = Nothing
            Private _TEXTO_W As AggregateParameter = Nothing
            Private _LANGILE_W As AggregateParameter = Nothing
            Private _NUMORDF_W As AggregateParameter = Nothing
            Private _NUMOPE_W As AggregateParameter = Nothing
            Private _BLOKEO_W As AggregateParameter = Nothing
            Private _USUARIO_EMISION_W As AggregateParameter = Nothing
            Private _FECHA_EMISION_W As AggregateParameter = Nothing
            Private _FICHERO_EMISION_W As AggregateParameter = Nothing
            Private _URGENTE_W As AggregateParameter = Nothing
            Private _CONFIRMADO_W As AggregateParameter = Nothing
            Private _PORTES_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _NUMPEDCAB_W = Nothing
                _CODPROCAB_W = Nothing
                _CODFORPAG_W = Nothing
                _CODPUERTA_W = Nothing
                _DTOFIJO_W = Nothing
                _DTOPP_W = Nothing
                _RECFINAN_W = Nothing
                _CODMONEDA_W = Nothing
                _TIPIVA_W = Nothing
                _FECPEDIDO_W = Nothing
                _FECLANZ_W = Nothing
                _FECENTREG_W = Nothing
                _ENVIO_W = Nothing
                _TEXTO_W = Nothing
                _LANGILE_W = Nothing
                _NUMORDF_W = Nothing
                _NUMOPE_W = Nothing
                _BLOKEO_W = Nothing
                _USUARIO_EMISION_W = Nothing
                _FECHA_EMISION_W = Nothing
                _FICHERO_EMISION_W = Nothing
                _URGENTE_W = Nothing
                _CONFIRMADO_W = Nothing
                _PORTES_W = Nothing
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
            cmd.CommandText = Me.SchemaStoredProcedure + "PI_GCCABPED"

            CreateParameters(cmd)


            Return cmd

        End Function

        Protected Overrides Function GetUpdateCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PU_GCCABPED"

            CreateParameters(cmd)

            Return cmd

        End Function

        Protected Overrides Function GetDeleteCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PD_GCCABPED"

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.NUMPEDCAB)
            p.SourceColumn = ColumnNames.NUMPEDCAB
            p.SourceVersion = DataRowVersion.Current


            Return cmd

        End Function

        Private Sub CreateParameters(ByVal cmd As OracleCommand)

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.NUMPEDCAB)
            p.SourceColumn = ColumnNames.NUMPEDCAB
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CODPROCAB)
            p.SourceColumn = ColumnNames.CODPROCAB
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CODFORPAG)
            p.SourceColumn = ColumnNames.CODFORPAG
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CODPUERTA)
            p.SourceColumn = ColumnNames.CODPUERTA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.DTOFIJO)
            p.SourceColumn = ColumnNames.DTOFIJO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.DTOPP)
            p.SourceColumn = ColumnNames.DTOPP
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.RECFINAN)
            p.SourceColumn = ColumnNames.RECFINAN
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CODMONEDA)
            p.SourceColumn = ColumnNames.CODMONEDA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.TIPIVA)
            p.SourceColumn = ColumnNames.TIPIVA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.FECPEDIDO)
            p.SourceColumn = ColumnNames.FECPEDIDO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.FECLANZ)
            p.SourceColumn = ColumnNames.FECLANZ
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.FECENTREG)
            p.SourceColumn = ColumnNames.FECENTREG
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.ENVIO)
            p.SourceColumn = ColumnNames.ENVIO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.TEXTO)
            p.SourceColumn = ColumnNames.TEXTO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.LANGILE)
            p.SourceColumn = ColumnNames.LANGILE
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMORDF)
            p.SourceColumn = ColumnNames.NUMORDF
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMOPE)
            p.SourceColumn = ColumnNames.NUMOPE
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.BLOKEO)
            p.SourceColumn = ColumnNames.BLOKEO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.USUARIO_EMISION)
            p.SourceColumn = ColumnNames.USUARIO_EMISION
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.FECHA_EMISION)
            p.SourceColumn = ColumnNames.FECHA_EMISION
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.FICHERO_EMISION)
            p.SourceColumn = ColumnNames.FICHERO_EMISION
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.URGENTE)
            p.SourceColumn = ColumnNames.URGENTE
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CONFIRMADO)
            p.SourceColumn = ColumnNames.CONFIRMADO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.PORTES)
            p.SourceColumn = ColumnNames.PORTES
            p.SourceVersion = DataRowVersion.Current


        End Sub

    End Class

End Namespace

