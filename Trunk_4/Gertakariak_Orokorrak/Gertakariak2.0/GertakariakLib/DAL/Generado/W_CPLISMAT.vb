
'===============================================================================
'BATZ, Koop. - 03/06/2008 11:54:02
' Generado por MyGeneration Version # (1.3.0.3)
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

    Public MustInherit Class _W_CPLISMAT
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "INCIDENCIAS."
            Me.QuerySource = "W_CPLISMAT"
            Me.MappingName = "W_CPLISMAT"
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

            Public Shared ReadOnly Property NUMLISTA As OracleParameter
                Get
                    Return New OracleParameter("NUMLISTA", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property TIPOLISTA As OracleParameter
                Get
                    Return New OracleParameter("TIPOLISTA", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property NPLANO As OracleParameter
                Get
                    Return New OracleParameter("NPLANO", OracleDbType.Char, 2)
                End Get
            End Property

            Public Shared ReadOnly Property CODART As OracleParameter
                Get
                    Return New OracleParameter("CODART", OracleDbType.Char, 12)
                End Get
            End Property

            Public Shared ReadOnly Property MATERIAL As OracleParameter
                Get
                    Return New OracleParameter("MATERIAL", OracleDbType.Char, 20)
                End Get
            End Property

            Public Shared ReadOnly Property CANNEC As OracleParameter
                Get
                    Return New OracleParameter("CANNEC", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property FECNEC As OracleParameter
                Get
                    Return New OracleParameter("FECNEC", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property CANTIMPUT As OracleParameter
                Get
                    Return New OracleParameter("CANTIMPUT", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property FECIMPUT As OracleParameter
                Get
                    Return New OracleParameter("FECIMPUT", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property ALMACEN As OracleParameter
                Get
                    Return New OracleParameter("ALMACEN", OracleDbType.Char, 1)
                End Get
            End Property

            Public Shared ReadOnly Property DIAMETRO As OracleParameter
                Get
                    Return New OracleParameter("DIAMETRO", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property LARGO As OracleParameter
                Get
                    Return New OracleParameter("LARGO", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property ANCHO As OracleParameter
                Get
                    Return New OracleParameter("ANCHO", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property GRUESO As OracleParameter
                Get
                    Return New OracleParameter("GRUESO", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property TRATAM As OracleParameter
                Get
                    Return New OracleParameter("TRATAM", OracleDbType.Char, 20)
                End Get
            End Property

            Public Shared ReadOnly Property TRATAM2 As OracleParameter
                Get
                    Return New OracleParameter("TRATAM2", OracleDbType.Char, 20)
                End Get
            End Property

            Public Shared ReadOnly Property OBSERV As OracleParameter
                Get
                    Return New OracleParameter("OBSERV", OracleDbType.Char, 20)
                End Get
            End Property

            Public Shared ReadOnly Property OBSERV2 As OracleParameter
                Get
                    Return New OracleParameter("OBSERV2", OracleDbType.Char, 20)
                End Get
            End Property

            Public Shared ReadOnly Property SITUAC As OracleParameter
                Get
                    Return New OracleParameter("SITUAC", OracleDbType.Char, 1)
                End Get
            End Property

            Public Shared ReadOnly Property FECDEF As OracleParameter
                Get
                    Return New OracleParameter("FECDEF", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property FASE As OracleParameter
                Get
                    Return New OracleParameter("FASE", OracleDbType.Char, 4)
                End Get
            End Property

            Public Shared ReadOnly Property DIAMDEF As OracleParameter
                Get
                    Return New OracleParameter("DIAMDEF", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property LARGDEF As OracleParameter
                Get
                    Return New OracleParameter("LARGDEF", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property ANCHDEF As OracleParameter
                Get
                    Return New OracleParameter("ANCHDEF", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property GRUEDEF As OracleParameter
                Get
                    Return New OracleParameter("GRUEDEF", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property MOTIVO As OracleParameter
                Get
                    Return New OracleParameter("MOTIVO", OracleDbType.Char, 40)
                End Get
            End Property

            Public Shared ReadOnly Property REALIZA As OracleParameter
                Get
                    Return New OracleParameter("REALIZA", OracleDbType.Char, 1)
                End Get
            End Property

            Public Shared ReadOnly Property CPLISDENO As OracleParameter
                Get
                    Return New OracleParameter("CPLISDENO", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property OTMARDES As OracleParameter
                Get
                    Return New OracleParameter("OTMARDES", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property ORDEN As OracleParameter
                Get
                    Return New OracleParameter("ORDEN", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property OTDUREZA As OracleParameter
                Get
                    Return New OracleParameter("OTDUREZA", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property OTTRASEC As OracleParameter
                Get
                    Return New OracleParameter("OTTRASEC", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property OTTRATAM As OracleParameter
                Get
                    Return New OracleParameter("OTTRATAM", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property OTMATESPE As OracleParameter
                Get
                    Return New OracleParameter("OTMATESPE", OracleDbType.Int32, 0)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const NUMORD As String = "NUMORD"
            Public Const NUMOPE As String = "NUMOPE"
            Public Const NUMMAR As String = "NUMMAR"
            Public Const NUMLISTA As String = "NUMLISTA"
            Public Const TIPOLISTA As String = "TIPOLISTA"
            Public Const NPLANO As String = "NPLANO"
            Public Const CODART As String = "CODART"
            Public Const MATERIAL As String = "MATERIAL"
            Public Const CANNEC As String = "CANNEC"
            Public Const FECNEC As String = "FECNEC"
            Public Const CANTIMPUT As String = "CANTIMPUT"
            Public Const FECIMPUT As String = "FECIMPUT"
            Public Const ALMACEN As String = "ALMACEN"
            Public Const DIAMETRO As String = "DIAMETRO"
            Public Const LARGO As String = "LARGO"
            Public Const ANCHO As String = "ANCHO"
            Public Const GRUESO As String = "GRUESO"
            Public Const TRATAM As String = "TRATAM"
            Public Const TRATAM2 As String = "TRATAM2"
            Public Const OBSERV As String = "OBSERV"
            Public Const OBSERV2 As String = "OBSERV2"
            Public Const SITUAC As String = "SITUAC"
            Public Const FECDEF As String = "FECDEF"
            Public Const FASE As String = "FASE"
            Public Const DIAMDEF As String = "DIAMDEF"
            Public Const LARGDEF As String = "LARGDEF"
            Public Const ANCHDEF As String = "ANCHDEF"
            Public Const GRUEDEF As String = "GRUEDEF"
            Public Const MOTIVO As String = "MOTIVO"
            Public Const REALIZA As String = "REALIZA"
            Public Const CPLISDENO As String = "CPLISDENO"
            Public Const OTMARDES As String = "OTMARDES"
            Public Const ORDEN As String = "ORDEN"
            Public Const OTDUREZA As String = "OTDUREZA"
            Public Const OTTRASEC As String = "OTTRASEC"
            Public Const OTTRATAM As String = "OTTRATAM"
            Public Const OTMATESPE As String = "OTMATESPE"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(NUMORD) = _W_CPLISMAT.PropertyNames.NUMORD
                    ht(NUMOPE) = _W_CPLISMAT.PropertyNames.NUMOPE
                    ht(NUMMAR) = _W_CPLISMAT.PropertyNames.NUMMAR
                    ht(NUMLISTA) = _W_CPLISMAT.PropertyNames.NUMLISTA
                    ht(TIPOLISTA) = _W_CPLISMAT.PropertyNames.TIPOLISTA
                    ht(NPLANO) = _W_CPLISMAT.PropertyNames.NPLANO
                    ht(CODART) = _W_CPLISMAT.PropertyNames.CODART
                    ht(MATERIAL) = _W_CPLISMAT.PropertyNames.MATERIAL
                    ht(CANNEC) = _W_CPLISMAT.PropertyNames.CANNEC
                    ht(FECNEC) = _W_CPLISMAT.PropertyNames.FECNEC
                    ht(CANTIMPUT) = _W_CPLISMAT.PropertyNames.CANTIMPUT
                    ht(FECIMPUT) = _W_CPLISMAT.PropertyNames.FECIMPUT
                    ht(ALMACEN) = _W_CPLISMAT.PropertyNames.ALMACEN
                    ht(DIAMETRO) = _W_CPLISMAT.PropertyNames.DIAMETRO
                    ht(LARGO) = _W_CPLISMAT.PropertyNames.LARGO
                    ht(ANCHO) = _W_CPLISMAT.PropertyNames.ANCHO
                    ht(GRUESO) = _W_CPLISMAT.PropertyNames.GRUESO
                    ht(TRATAM) = _W_CPLISMAT.PropertyNames.TRATAM
                    ht(TRATAM2) = _W_CPLISMAT.PropertyNames.TRATAM2
                    ht(OBSERV) = _W_CPLISMAT.PropertyNames.OBSERV
                    ht(OBSERV2) = _W_CPLISMAT.PropertyNames.OBSERV2
                    ht(SITUAC) = _W_CPLISMAT.PropertyNames.SITUAC
                    ht(FECDEF) = _W_CPLISMAT.PropertyNames.FECDEF
                    ht(FASE) = _W_CPLISMAT.PropertyNames.FASE
                    ht(DIAMDEF) = _W_CPLISMAT.PropertyNames.DIAMDEF
                    ht(LARGDEF) = _W_CPLISMAT.PropertyNames.LARGDEF
                    ht(ANCHDEF) = _W_CPLISMAT.PropertyNames.ANCHDEF
                    ht(GRUEDEF) = _W_CPLISMAT.PropertyNames.GRUEDEF
                    ht(MOTIVO) = _W_CPLISMAT.PropertyNames.MOTIVO
                    ht(REALIZA) = _W_CPLISMAT.PropertyNames.REALIZA
                    ht(CPLISDENO) = _W_CPLISMAT.PropertyNames.CPLISDENO
                    ht(OTMARDES) = _W_CPLISMAT.PropertyNames.OTMARDES
                    ht(ORDEN) = _W_CPLISMAT.PropertyNames.ORDEN
                    ht(OTDUREZA) = _W_CPLISMAT.PropertyNames.OTDUREZA
                    ht(OTTRASEC) = _W_CPLISMAT.PropertyNames.OTTRASEC
                    ht(OTTRATAM) = _W_CPLISMAT.PropertyNames.OTTRATAM
                    ht(OTMATESPE) = _W_CPLISMAT.PropertyNames.OTMATESPE

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const NUMORD As String = "NUMORD"
            Public Const NUMOPE As String = "NUMOPE"
            Public Const NUMMAR As String = "NUMMAR"
            Public Const NUMLISTA As String = "NUMLISTA"
            Public Const TIPOLISTA As String = "TIPOLISTA"
            Public Const NPLANO As String = "NPLANO"
            Public Const CODART As String = "CODART"
            Public Const MATERIAL As String = "MATERIAL"
            Public Const CANNEC As String = "CANNEC"
            Public Const FECNEC As String = "FECNEC"
            Public Const CANTIMPUT As String = "CANTIMPUT"
            Public Const FECIMPUT As String = "FECIMPUT"
            Public Const ALMACEN As String = "ALMACEN"
            Public Const DIAMETRO As String = "DIAMETRO"
            Public Const LARGO As String = "LARGO"
            Public Const ANCHO As String = "ANCHO"
            Public Const GRUESO As String = "GRUESO"
            Public Const TRATAM As String = "TRATAM"
            Public Const TRATAM2 As String = "TRATAM2"
            Public Const OBSERV As String = "OBSERV"
            Public Const OBSERV2 As String = "OBSERV2"
            Public Const SITUAC As String = "SITUAC"
            Public Const FECDEF As String = "FECDEF"
            Public Const FASE As String = "FASE"
            Public Const DIAMDEF As String = "DIAMDEF"
            Public Const LARGDEF As String = "LARGDEF"
            Public Const ANCHDEF As String = "ANCHDEF"
            Public Const GRUEDEF As String = "GRUEDEF"
            Public Const MOTIVO As String = "MOTIVO"
            Public Const REALIZA As String = "REALIZA"
            Public Const CPLISDENO As String = "CPLISDENO"
            Public Const OTMARDES As String = "OTMARDES"
            Public Const ORDEN As String = "ORDEN"
            Public Const OTDUREZA As String = "OTDUREZA"
            Public Const OTTRASEC As String = "OTTRASEC"
            Public Const OTTRATAM As String = "OTTRATAM"
            Public Const OTMATESPE As String = "OTMATESPE"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(NUMORD) = _W_CPLISMAT.ColumnNames.NUMORD
                    ht(NUMOPE) = _W_CPLISMAT.ColumnNames.NUMOPE
                    ht(NUMMAR) = _W_CPLISMAT.ColumnNames.NUMMAR
                    ht(NUMLISTA) = _W_CPLISMAT.ColumnNames.NUMLISTA
                    ht(TIPOLISTA) = _W_CPLISMAT.ColumnNames.TIPOLISTA
                    ht(NPLANO) = _W_CPLISMAT.ColumnNames.NPLANO
                    ht(CODART) = _W_CPLISMAT.ColumnNames.CODART
                    ht(MATERIAL) = _W_CPLISMAT.ColumnNames.MATERIAL
                    ht(CANNEC) = _W_CPLISMAT.ColumnNames.CANNEC
                    ht(FECNEC) = _W_CPLISMAT.ColumnNames.FECNEC
                    ht(CANTIMPUT) = _W_CPLISMAT.ColumnNames.CANTIMPUT
                    ht(FECIMPUT) = _W_CPLISMAT.ColumnNames.FECIMPUT
                    ht(ALMACEN) = _W_CPLISMAT.ColumnNames.ALMACEN
                    ht(DIAMETRO) = _W_CPLISMAT.ColumnNames.DIAMETRO
                    ht(LARGO) = _W_CPLISMAT.ColumnNames.LARGO
                    ht(ANCHO) = _W_CPLISMAT.ColumnNames.ANCHO
                    ht(GRUESO) = _W_CPLISMAT.ColumnNames.GRUESO
                    ht(TRATAM) = _W_CPLISMAT.ColumnNames.TRATAM
                    ht(TRATAM2) = _W_CPLISMAT.ColumnNames.TRATAM2
                    ht(OBSERV) = _W_CPLISMAT.ColumnNames.OBSERV
                    ht(OBSERV2) = _W_CPLISMAT.ColumnNames.OBSERV2
                    ht(SITUAC) = _W_CPLISMAT.ColumnNames.SITUAC
                    ht(FECDEF) = _W_CPLISMAT.ColumnNames.FECDEF
                    ht(FASE) = _W_CPLISMAT.ColumnNames.FASE
                    ht(DIAMDEF) = _W_CPLISMAT.ColumnNames.DIAMDEF
                    ht(LARGDEF) = _W_CPLISMAT.ColumnNames.LARGDEF
                    ht(ANCHDEF) = _W_CPLISMAT.ColumnNames.ANCHDEF
                    ht(GRUEDEF) = _W_CPLISMAT.ColumnNames.GRUEDEF
                    ht(MOTIVO) = _W_CPLISMAT.ColumnNames.MOTIVO
                    ht(REALIZA) = _W_CPLISMAT.ColumnNames.REALIZA
                    ht(CPLISDENO) = _W_CPLISMAT.ColumnNames.CPLISDENO
                    ht(OTMARDES) = _W_CPLISMAT.ColumnNames.OTMARDES
                    ht(ORDEN) = _W_CPLISMAT.ColumnNames.ORDEN
                    ht(OTDUREZA) = _W_CPLISMAT.ColumnNames.OTDUREZA
                    ht(OTTRASEC) = _W_CPLISMAT.ColumnNames.OTTRASEC
                    ht(OTTRATAM) = _W_CPLISMAT.ColumnNames.OTTRATAM
                    ht(OTMATESPE) = _W_CPLISMAT.ColumnNames.OTMATESPE

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const NUMORD As String = "s_NUMORD"
            Public Const NUMOPE As String = "s_NUMOPE"
            Public Const NUMMAR As String = "s_NUMMAR"
            Public Const NUMLISTA As String = "s_NUMLISTA"
            Public Const TIPOLISTA As String = "s_TIPOLISTA"
            Public Const NPLANO As String = "s_NPLANO"
            Public Const CODART As String = "s_CODART"
            Public Const MATERIAL As String = "s_MATERIAL"
            Public Const CANNEC As String = "s_CANNEC"
            Public Const FECNEC As String = "s_FECNEC"
            Public Const CANTIMPUT As String = "s_CANTIMPUT"
            Public Const FECIMPUT As String = "s_FECIMPUT"
            Public Const ALMACEN As String = "s_ALMACEN"
            Public Const DIAMETRO As String = "s_DIAMETRO"
            Public Const LARGO As String = "s_LARGO"
            Public Const ANCHO As String = "s_ANCHO"
            Public Const GRUESO As String = "s_GRUESO"
            Public Const TRATAM As String = "s_TRATAM"
            Public Const TRATAM2 As String = "s_TRATAM2"
            Public Const OBSERV As String = "s_OBSERV"
            Public Const OBSERV2 As String = "s_OBSERV2"
            Public Const SITUAC As String = "s_SITUAC"
            Public Const FECDEF As String = "s_FECDEF"
            Public Const FASE As String = "s_FASE"
            Public Const DIAMDEF As String = "s_DIAMDEF"
            Public Const LARGDEF As String = "s_LARGDEF"
            Public Const ANCHDEF As String = "s_ANCHDEF"
            Public Const GRUEDEF As String = "s_GRUEDEF"
            Public Const MOTIVO As String = "s_MOTIVO"
            Public Const REALIZA As String = "s_REALIZA"
            Public Const CPLISDENO As String = "s_CPLISDENO"
            Public Const OTMARDES As String = "s_OTMARDES"
            Public Const ORDEN As String = "s_ORDEN"
            Public Const OTDUREZA As String = "s_OTDUREZA"
            Public Const OTTRASEC As String = "s_OTTRASEC"
            Public Const OTTRATAM As String = "s_OTTRATAM"
            Public Const OTMATESPE As String = "s_OTMATESPE"

        End Class
#End Region

#Region "Properties"
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

        Public Overridable Property NUMLISTA As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.NUMLISTA)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.NUMLISTA, Value)
            End Set
        End Property

        Public Overridable Property TIPOLISTA As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.TIPOLISTA)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.TIPOLISTA, Value)
            End Set
        End Property

        Public Overridable Property NPLANO As String
            Get
                Return MyBase.GetString(ColumnNames.NPLANO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.NPLANO, Value)
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

        Public Overridable Property MATERIAL As String
            Get
                Return MyBase.GetString(ColumnNames.MATERIAL)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.MATERIAL, Value)
            End Set
        End Property

        Public Overridable Property CANNEC As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.CANNEC)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.CANNEC, Value)
            End Set
        End Property

        Public Overridable Property FECNEC As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECNEC)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECNEC, Value)
            End Set
        End Property

        Public Overridable Property CANTIMPUT As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.CANTIMPUT)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.CANTIMPUT, Value)
            End Set
        End Property

        Public Overridable Property FECIMPUT As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECIMPUT)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECIMPUT, Value)
            End Set
        End Property

        Public Overridable Property ALMACEN As String
            Get
                Return MyBase.GetString(ColumnNames.ALMACEN)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.ALMACEN, Value)
            End Set
        End Property

        Public Overridable Property DIAMETRO As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.DIAMETRO)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.DIAMETRO, Value)
            End Set
        End Property

        Public Overridable Property LARGO As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.LARGO)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.LARGO, Value)
            End Set
        End Property

        Public Overridable Property ANCHO As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.ANCHO)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.ANCHO, Value)
            End Set
        End Property

        Public Overridable Property GRUESO As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.GRUESO)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.GRUESO, Value)
            End Set
        End Property

        Public Overridable Property TRATAM As String
            Get
                Return MyBase.GetString(ColumnNames.TRATAM)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.TRATAM, Value)
            End Set
        End Property

        Public Overridable Property TRATAM2 As String
            Get
                Return MyBase.GetString(ColumnNames.TRATAM2)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.TRATAM2, Value)
            End Set
        End Property

        Public Overridable Property OBSERV As String
            Get
                Return MyBase.GetString(ColumnNames.OBSERV)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.OBSERV, Value)
            End Set
        End Property

        Public Overridable Property OBSERV2 As String
            Get
                Return MyBase.GetString(ColumnNames.OBSERV2)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.OBSERV2, Value)
            End Set
        End Property

        Public Overridable Property SITUAC As String
            Get
                Return MyBase.GetString(ColumnNames.SITUAC)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.SITUAC, Value)
            End Set
        End Property

        Public Overridable Property FECDEF As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECDEF)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECDEF, Value)
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

        Public Overridable Property DIAMDEF As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.DIAMDEF)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.DIAMDEF, Value)
            End Set
        End Property

        Public Overridable Property LARGDEF As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.LARGDEF)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.LARGDEF, Value)
            End Set
        End Property

        Public Overridable Property ANCHDEF As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.ANCHDEF)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.ANCHDEF, Value)
            End Set
        End Property

        Public Overridable Property GRUEDEF As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.GRUEDEF)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.GRUEDEF, Value)
            End Set
        End Property

        Public Overridable Property MOTIVO As String
            Get
                Return MyBase.GetString(ColumnNames.MOTIVO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.MOTIVO, Value)
            End Set
        End Property

        Public Overridable Property REALIZA As String
            Get
                Return MyBase.GetString(ColumnNames.REALIZA)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.REALIZA, Value)
            End Set
        End Property

        Public Overridable Property CPLISDENO As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.CPLISDENO)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.CPLISDENO, Value)
            End Set
        End Property

        Public Overridable Property OTMARDES As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.OTMARDES)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.OTMARDES, Value)
            End Set
        End Property

        Public Overridable Property ORDEN As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.ORDEN)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.ORDEN, Value)
            End Set
        End Property

        Public Overridable Property OTDUREZA As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.OTDUREZA)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.OTDUREZA, Value)
            End Set
        End Property

        Public Overridable Property OTTRASEC As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.OTTRASEC)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.OTTRASEC, Value)
            End Set
        End Property

        Public Overridable Property OTTRATAM As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.OTTRATAM)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.OTTRATAM, Value)
            End Set
        End Property

        Public Overridable Property OTMATESPE As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.OTMATESPE)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.OTMATESPE, Value)
            End Set
        End Property


#End Region

#Region "String Properties"

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

        Public Overridable Property s_NUMLISTA As String
            Get
                If Me.IsColumnNull(ColumnNames.NUMLISTA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.NUMLISTA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NUMLISTA)
                Else
                    Me.NUMLISTA = MyBase.SetDecimalAsString(ColumnNames.NUMLISTA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_TIPOLISTA As String
            Get
                If Me.IsColumnNull(ColumnNames.TIPOLISTA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.TIPOLISTA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.TIPOLISTA)
                Else
                    Me.TIPOLISTA = MyBase.SetDecimalAsString(ColumnNames.TIPOLISTA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_NPLANO As String
            Get
                If Me.IsColumnNull(ColumnNames.NPLANO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.NPLANO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NPLANO)
                Else
                    Me.NPLANO = MyBase.SetStringAsString(ColumnNames.NPLANO, Value)
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

        Public Overridable Property s_MATERIAL As String
            Get
                If Me.IsColumnNull(ColumnNames.MATERIAL) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.MATERIAL)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.MATERIAL)
                Else
                    Me.MATERIAL = MyBase.SetStringAsString(ColumnNames.MATERIAL, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CANNEC As String
            Get
                If Me.IsColumnNull(ColumnNames.CANNEC) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.CANNEC)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CANNEC)
                Else
                    Me.CANNEC = MyBase.SetDecimalAsString(ColumnNames.CANNEC, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FECNEC As String
            Get
                If Me.IsColumnNull(ColumnNames.FECNEC) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FECNEC)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FECNEC)
                Else
                    Me.FECNEC = MyBase.SetDateTimeAsString(ColumnNames.FECNEC, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CANTIMPUT As String
            Get
                If Me.IsColumnNull(ColumnNames.CANTIMPUT) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.CANTIMPUT)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CANTIMPUT)
                Else
                    Me.CANTIMPUT = MyBase.SetDecimalAsString(ColumnNames.CANTIMPUT, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FECIMPUT As String
            Get
                If Me.IsColumnNull(ColumnNames.FECIMPUT) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FECIMPUT)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FECIMPUT)
                Else
                    Me.FECIMPUT = MyBase.SetDateTimeAsString(ColumnNames.FECIMPUT, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_ALMACEN As String
            Get
                If Me.IsColumnNull(ColumnNames.ALMACEN) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.ALMACEN)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.ALMACEN)
                Else
                    Me.ALMACEN = MyBase.SetStringAsString(ColumnNames.ALMACEN, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_DIAMETRO As String
            Get
                If Me.IsColumnNull(ColumnNames.DIAMETRO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.DIAMETRO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.DIAMETRO)
                Else
                    Me.DIAMETRO = MyBase.SetDecimalAsString(ColumnNames.DIAMETRO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_LARGO As String
            Get
                If Me.IsColumnNull(ColumnNames.LARGO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.LARGO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.LARGO)
                Else
                    Me.LARGO = MyBase.SetDecimalAsString(ColumnNames.LARGO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_ANCHO As String
            Get
                If Me.IsColumnNull(ColumnNames.ANCHO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.ANCHO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.ANCHO)
                Else
                    Me.ANCHO = MyBase.SetDecimalAsString(ColumnNames.ANCHO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_GRUESO As String
            Get
                If Me.IsColumnNull(ColumnNames.GRUESO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.GRUESO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.GRUESO)
                Else
                    Me.GRUESO = MyBase.SetDecimalAsString(ColumnNames.GRUESO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_TRATAM As String
            Get
                If Me.IsColumnNull(ColumnNames.TRATAM) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.TRATAM)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.TRATAM)
                Else
                    Me.TRATAM = MyBase.SetStringAsString(ColumnNames.TRATAM, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_TRATAM2 As String
            Get
                If Me.IsColumnNull(ColumnNames.TRATAM2) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.TRATAM2)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.TRATAM2)
                Else
                    Me.TRATAM2 = MyBase.SetStringAsString(ColumnNames.TRATAM2, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_OBSERV As String
            Get
                If Me.IsColumnNull(ColumnNames.OBSERV) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.OBSERV)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.OBSERV)
                Else
                    Me.OBSERV = MyBase.SetStringAsString(ColumnNames.OBSERV, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_OBSERV2 As String
            Get
                If Me.IsColumnNull(ColumnNames.OBSERV2) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.OBSERV2)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.OBSERV2)
                Else
                    Me.OBSERV2 = MyBase.SetStringAsString(ColumnNames.OBSERV2, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_SITUAC As String
            Get
                If Me.IsColumnNull(ColumnNames.SITUAC) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.SITUAC)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.SITUAC)
                Else
                    Me.SITUAC = MyBase.SetStringAsString(ColumnNames.SITUAC, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FECDEF As String
            Get
                If Me.IsColumnNull(ColumnNames.FECDEF) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FECDEF)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FECDEF)
                Else
                    Me.FECDEF = MyBase.SetDateTimeAsString(ColumnNames.FECDEF, Value)
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

        Public Overridable Property s_DIAMDEF As String
            Get
                If Me.IsColumnNull(ColumnNames.DIAMDEF) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.DIAMDEF)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.DIAMDEF)
                Else
                    Me.DIAMDEF = MyBase.SetDecimalAsString(ColumnNames.DIAMDEF, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_LARGDEF As String
            Get
                If Me.IsColumnNull(ColumnNames.LARGDEF) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.LARGDEF)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.LARGDEF)
                Else
                    Me.LARGDEF = MyBase.SetDecimalAsString(ColumnNames.LARGDEF, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_ANCHDEF As String
            Get
                If Me.IsColumnNull(ColumnNames.ANCHDEF) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.ANCHDEF)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.ANCHDEF)
                Else
                    Me.ANCHDEF = MyBase.SetDecimalAsString(ColumnNames.ANCHDEF, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_GRUEDEF As String
            Get
                If Me.IsColumnNull(ColumnNames.GRUEDEF) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.GRUEDEF)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.GRUEDEF)
                Else
                    Me.GRUEDEF = MyBase.SetDecimalAsString(ColumnNames.GRUEDEF, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_MOTIVO As String
            Get
                If Me.IsColumnNull(ColumnNames.MOTIVO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.MOTIVO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.MOTIVO)
                Else
                    Me.MOTIVO = MyBase.SetStringAsString(ColumnNames.MOTIVO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_REALIZA As String
            Get
                If Me.IsColumnNull(ColumnNames.REALIZA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.REALIZA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.REALIZA)
                Else
                    Me.REALIZA = MyBase.SetStringAsString(ColumnNames.REALIZA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CPLISDENO As String
            Get
                If Me.IsColumnNull(ColumnNames.CPLISDENO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.CPLISDENO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CPLISDENO)
                Else
                    Me.CPLISDENO = MyBase.SetDecimalAsString(ColumnNames.CPLISDENO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_OTMARDES As String
            Get
                If Me.IsColumnNull(ColumnNames.OTMARDES) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.OTMARDES)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.OTMARDES)
                Else
                    Me.OTMARDES = MyBase.SetDecimalAsString(ColumnNames.OTMARDES, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_ORDEN As String
            Get
                If Me.IsColumnNull(ColumnNames.ORDEN) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.ORDEN)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.ORDEN)
                Else
                    Me.ORDEN = MyBase.SetDecimalAsString(ColumnNames.ORDEN, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_OTDUREZA As String
            Get
                If Me.IsColumnNull(ColumnNames.OTDUREZA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.OTDUREZA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.OTDUREZA)
                Else
                    Me.OTDUREZA = MyBase.SetDecimalAsString(ColumnNames.OTDUREZA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_OTTRASEC As String
            Get
                If Me.IsColumnNull(ColumnNames.OTTRASEC) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.OTTRASEC)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.OTTRASEC)
                Else
                    Me.OTTRASEC = MyBase.SetDecimalAsString(ColumnNames.OTTRASEC, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_OTTRATAM As String
            Get
                If Me.IsColumnNull(ColumnNames.OTTRATAM) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.OTTRATAM)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.OTTRATAM)
                Else
                    Me.OTTRATAM = MyBase.SetDecimalAsString(ColumnNames.OTTRATAM, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_OTMATESPE As String
            Get
                If Me.IsColumnNull(ColumnNames.OTMATESPE) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.OTMATESPE)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.OTMATESPE)
                Else
                    Me.OTMATESPE = MyBase.SetDecimalAsString(ColumnNames.OTMATESPE, Value)
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

                Public ReadOnly Property NUMLISTA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMLISTA, Parameters.NUMLISTA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TIPOLISTA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.TIPOLISTA, Parameters.TIPOLISTA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NPLANO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NPLANO, Parameters.NPLANO)
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

                Public ReadOnly Property MATERIAL() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.MATERIAL, Parameters.MATERIAL)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CANNEC() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CANNEC, Parameters.CANNEC)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECNEC() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FECNEC, Parameters.FECNEC)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CANTIMPUT() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CANTIMPUT, Parameters.CANTIMPUT)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECIMPUT() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FECIMPUT, Parameters.FECIMPUT)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ALMACEN() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.ALMACEN, Parameters.ALMACEN)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DIAMETRO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.DIAMETRO, Parameters.DIAMETRO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property LARGO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.LARGO, Parameters.LARGO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ANCHO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.ANCHO, Parameters.ANCHO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property GRUESO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.GRUESO, Parameters.GRUESO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TRATAM() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.TRATAM, Parameters.TRATAM)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TRATAM2() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.TRATAM2, Parameters.TRATAM2)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property OBSERV() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.OBSERV, Parameters.OBSERV)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property OBSERV2() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.OBSERV2, Parameters.OBSERV2)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property SITUAC() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.SITUAC, Parameters.SITUAC)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECDEF() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FECDEF, Parameters.FECDEF)
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

                Public ReadOnly Property DIAMDEF() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.DIAMDEF, Parameters.DIAMDEF)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property LARGDEF() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.LARGDEF, Parameters.LARGDEF)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ANCHDEF() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.ANCHDEF, Parameters.ANCHDEF)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property GRUEDEF() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.GRUEDEF, Parameters.GRUEDEF)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property MOTIVO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.MOTIVO, Parameters.MOTIVO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property REALIZA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.REALIZA, Parameters.REALIZA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CPLISDENO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CPLISDENO, Parameters.CPLISDENO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property OTMARDES() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.OTMARDES, Parameters.OTMARDES)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ORDEN() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.ORDEN, Parameters.ORDEN)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property OTDUREZA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.OTDUREZA, Parameters.OTDUREZA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property OTTRASEC() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.OTTRASEC, Parameters.OTTRASEC)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property OTTRATAM() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.OTTRATAM, Parameters.OTTRATAM)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property OTMATESPE() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.OTMATESPE, Parameters.OTMATESPE)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property


                Private _clause As WhereClause
            End Class
#End Region

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

            Public ReadOnly Property NUMLISTA() As WhereParameter
                Get
                    If _NUMLISTA_W Is Nothing Then
                        _NUMLISTA_W = TearOff.NUMLISTA
                    End If
                    Return _NUMLISTA_W
                End Get
            End Property

            Public ReadOnly Property TIPOLISTA() As WhereParameter
                Get
                    If _TIPOLISTA_W Is Nothing Then
                        _TIPOLISTA_W = TearOff.TIPOLISTA
                    End If
                    Return _TIPOLISTA_W
                End Get
            End Property

            Public ReadOnly Property NPLANO() As WhereParameter
                Get
                    If _NPLANO_W Is Nothing Then
                        _NPLANO_W = TearOff.NPLANO
                    End If
                    Return _NPLANO_W
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

            Public ReadOnly Property MATERIAL() As WhereParameter
                Get
                    If _MATERIAL_W Is Nothing Then
                        _MATERIAL_W = TearOff.MATERIAL
                    End If
                    Return _MATERIAL_W
                End Get
            End Property

            Public ReadOnly Property CANNEC() As WhereParameter
                Get
                    If _CANNEC_W Is Nothing Then
                        _CANNEC_W = TearOff.CANNEC
                    End If
                    Return _CANNEC_W
                End Get
            End Property

            Public ReadOnly Property FECNEC() As WhereParameter
                Get
                    If _FECNEC_W Is Nothing Then
                        _FECNEC_W = TearOff.FECNEC
                    End If
                    Return _FECNEC_W
                End Get
            End Property

            Public ReadOnly Property CANTIMPUT() As WhereParameter
                Get
                    If _CANTIMPUT_W Is Nothing Then
                        _CANTIMPUT_W = TearOff.CANTIMPUT
                    End If
                    Return _CANTIMPUT_W
                End Get
            End Property

            Public ReadOnly Property FECIMPUT() As WhereParameter
                Get
                    If _FECIMPUT_W Is Nothing Then
                        _FECIMPUT_W = TearOff.FECIMPUT
                    End If
                    Return _FECIMPUT_W
                End Get
            End Property

            Public ReadOnly Property ALMACEN() As WhereParameter
                Get
                    If _ALMACEN_W Is Nothing Then
                        _ALMACEN_W = TearOff.ALMACEN
                    End If
                    Return _ALMACEN_W
                End Get
            End Property

            Public ReadOnly Property DIAMETRO() As WhereParameter
                Get
                    If _DIAMETRO_W Is Nothing Then
                        _DIAMETRO_W = TearOff.DIAMETRO
                    End If
                    Return _DIAMETRO_W
                End Get
            End Property

            Public ReadOnly Property LARGO() As WhereParameter
                Get
                    If _LARGO_W Is Nothing Then
                        _LARGO_W = TearOff.LARGO
                    End If
                    Return _LARGO_W
                End Get
            End Property

            Public ReadOnly Property ANCHO() As WhereParameter
                Get
                    If _ANCHO_W Is Nothing Then
                        _ANCHO_W = TearOff.ANCHO
                    End If
                    Return _ANCHO_W
                End Get
            End Property

            Public ReadOnly Property GRUESO() As WhereParameter
                Get
                    If _GRUESO_W Is Nothing Then
                        _GRUESO_W = TearOff.GRUESO
                    End If
                    Return _GRUESO_W
                End Get
            End Property

            Public ReadOnly Property TRATAM() As WhereParameter
                Get
                    If _TRATAM_W Is Nothing Then
                        _TRATAM_W = TearOff.TRATAM
                    End If
                    Return _TRATAM_W
                End Get
            End Property

            Public ReadOnly Property TRATAM2() As WhereParameter
                Get
                    If _TRATAM2_W Is Nothing Then
                        _TRATAM2_W = TearOff.TRATAM2
                    End If
                    Return _TRATAM2_W
                End Get
            End Property

            Public ReadOnly Property OBSERV() As WhereParameter
                Get
                    If _OBSERV_W Is Nothing Then
                        _OBSERV_W = TearOff.OBSERV
                    End If
                    Return _OBSERV_W
                End Get
            End Property

            Public ReadOnly Property OBSERV2() As WhereParameter
                Get
                    If _OBSERV2_W Is Nothing Then
                        _OBSERV2_W = TearOff.OBSERV2
                    End If
                    Return _OBSERV2_W
                End Get
            End Property

            Public ReadOnly Property SITUAC() As WhereParameter
                Get
                    If _SITUAC_W Is Nothing Then
                        _SITUAC_W = TearOff.SITUAC
                    End If
                    Return _SITUAC_W
                End Get
            End Property

            Public ReadOnly Property FECDEF() As WhereParameter
                Get
                    If _FECDEF_W Is Nothing Then
                        _FECDEF_W = TearOff.FECDEF
                    End If
                    Return _FECDEF_W
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

            Public ReadOnly Property DIAMDEF() As WhereParameter
                Get
                    If _DIAMDEF_W Is Nothing Then
                        _DIAMDEF_W = TearOff.DIAMDEF
                    End If
                    Return _DIAMDEF_W
                End Get
            End Property

            Public ReadOnly Property LARGDEF() As WhereParameter
                Get
                    If _LARGDEF_W Is Nothing Then
                        _LARGDEF_W = TearOff.LARGDEF
                    End If
                    Return _LARGDEF_W
                End Get
            End Property

            Public ReadOnly Property ANCHDEF() As WhereParameter
                Get
                    If _ANCHDEF_W Is Nothing Then
                        _ANCHDEF_W = TearOff.ANCHDEF
                    End If
                    Return _ANCHDEF_W
                End Get
            End Property

            Public ReadOnly Property GRUEDEF() As WhereParameter
                Get
                    If _GRUEDEF_W Is Nothing Then
                        _GRUEDEF_W = TearOff.GRUEDEF
                    End If
                    Return _GRUEDEF_W
                End Get
            End Property

            Public ReadOnly Property MOTIVO() As WhereParameter
                Get
                    If _MOTIVO_W Is Nothing Then
                        _MOTIVO_W = TearOff.MOTIVO
                    End If
                    Return _MOTIVO_W
                End Get
            End Property

            Public ReadOnly Property REALIZA() As WhereParameter
                Get
                    If _REALIZA_W Is Nothing Then
                        _REALIZA_W = TearOff.REALIZA
                    End If
                    Return _REALIZA_W
                End Get
            End Property

            Public ReadOnly Property CPLISDENO() As WhereParameter
                Get
                    If _CPLISDENO_W Is Nothing Then
                        _CPLISDENO_W = TearOff.CPLISDENO
                    End If
                    Return _CPLISDENO_W
                End Get
            End Property

            Public ReadOnly Property OTMARDES() As WhereParameter
                Get
                    If _OTMARDES_W Is Nothing Then
                        _OTMARDES_W = TearOff.OTMARDES
                    End If
                    Return _OTMARDES_W
                End Get
            End Property

            Public ReadOnly Property ORDEN() As WhereParameter
                Get
                    If _ORDEN_W Is Nothing Then
                        _ORDEN_W = TearOff.ORDEN
                    End If
                    Return _ORDEN_W
                End Get
            End Property

            Public ReadOnly Property OTDUREZA() As WhereParameter
                Get
                    If _OTDUREZA_W Is Nothing Then
                        _OTDUREZA_W = TearOff.OTDUREZA
                    End If
                    Return _OTDUREZA_W
                End Get
            End Property

            Public ReadOnly Property OTTRASEC() As WhereParameter
                Get
                    If _OTTRASEC_W Is Nothing Then
                        _OTTRASEC_W = TearOff.OTTRASEC
                    End If
                    Return _OTTRASEC_W
                End Get
            End Property

            Public ReadOnly Property OTTRATAM() As WhereParameter
                Get
                    If _OTTRATAM_W Is Nothing Then
                        _OTTRATAM_W = TearOff.OTTRATAM
                    End If
                    Return _OTTRATAM_W
                End Get
            End Property

            Public ReadOnly Property OTMATESPE() As WhereParameter
                Get
                    If _OTMATESPE_W Is Nothing Then
                        _OTMATESPE_W = TearOff.OTMATESPE
                    End If
                    Return _OTMATESPE_W
                End Get
            End Property

            Private _NUMORD_W As WhereParameter = Nothing
            Private _NUMOPE_W As WhereParameter = Nothing
            Private _NUMMAR_W As WhereParameter = Nothing
            Private _NUMLISTA_W As WhereParameter = Nothing
            Private _TIPOLISTA_W As WhereParameter = Nothing
            Private _NPLANO_W As WhereParameter = Nothing
            Private _CODART_W As WhereParameter = Nothing
            Private _MATERIAL_W As WhereParameter = Nothing
            Private _CANNEC_W As WhereParameter = Nothing
            Private _FECNEC_W As WhereParameter = Nothing
            Private _CANTIMPUT_W As WhereParameter = Nothing
            Private _FECIMPUT_W As WhereParameter = Nothing
            Private _ALMACEN_W As WhereParameter = Nothing
            Private _DIAMETRO_W As WhereParameter = Nothing
            Private _LARGO_W As WhereParameter = Nothing
            Private _ANCHO_W As WhereParameter = Nothing
            Private _GRUESO_W As WhereParameter = Nothing
            Private _TRATAM_W As WhereParameter = Nothing
            Private _TRATAM2_W As WhereParameter = Nothing
            Private _OBSERV_W As WhereParameter = Nothing
            Private _OBSERV2_W As WhereParameter = Nothing
            Private _SITUAC_W As WhereParameter = Nothing
            Private _FECDEF_W As WhereParameter = Nothing
            Private _FASE_W As WhereParameter = Nothing
            Private _DIAMDEF_W As WhereParameter = Nothing
            Private _LARGDEF_W As WhereParameter = Nothing
            Private _ANCHDEF_W As WhereParameter = Nothing
            Private _GRUEDEF_W As WhereParameter = Nothing
            Private _MOTIVO_W As WhereParameter = Nothing
            Private _REALIZA_W As WhereParameter = Nothing
            Private _CPLISDENO_W As WhereParameter = Nothing
            Private _OTMARDES_W As WhereParameter = Nothing
            Private _ORDEN_W As WhereParameter = Nothing
            Private _OTDUREZA_W As WhereParameter = Nothing
            Private _OTTRASEC_W As WhereParameter = Nothing
            Private _OTTRATAM_W As WhereParameter = Nothing
            Private _OTMATESPE_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _NUMORD_W = Nothing
                _NUMOPE_W = Nothing
                _NUMMAR_W = Nothing
                _NUMLISTA_W = Nothing
                _TIPOLISTA_W = Nothing
                _NPLANO_W = Nothing
                _CODART_W = Nothing
                _MATERIAL_W = Nothing
                _CANNEC_W = Nothing
                _FECNEC_W = Nothing
                _CANTIMPUT_W = Nothing
                _FECIMPUT_W = Nothing
                _ALMACEN_W = Nothing
                _DIAMETRO_W = Nothing
                _LARGO_W = Nothing
                _ANCHO_W = Nothing
                _GRUESO_W = Nothing
                _TRATAM_W = Nothing
                _TRATAM2_W = Nothing
                _OBSERV_W = Nothing
                _OBSERV2_W = Nothing
                _SITUAC_W = Nothing
                _FECDEF_W = Nothing
                _FASE_W = Nothing
                _DIAMDEF_W = Nothing
                _LARGDEF_W = Nothing
                _ANCHDEF_W = Nothing
                _GRUEDEF_W = Nothing
                _MOTIVO_W = Nothing
                _REALIZA_W = Nothing
                _CPLISDENO_W = Nothing
                _OTMARDES_W = Nothing
                _ORDEN_W = Nothing
                _OTDUREZA_W = Nothing
                _OTTRASEC_W = Nothing
                _OTTRATAM_W = Nothing
                _OTMATESPE_W = Nothing
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

                Public ReadOnly Property NUMLISTA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMLISTA, Parameters.NUMLISTA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TIPOLISTA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TIPOLISTA, Parameters.TIPOLISTA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NPLANO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NPLANO, Parameters.NPLANO)
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

                Public ReadOnly Property MATERIAL() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.MATERIAL, Parameters.MATERIAL)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CANNEC() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CANNEC, Parameters.CANNEC)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECNEC() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECNEC, Parameters.FECNEC)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CANTIMPUT() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CANTIMPUT, Parameters.CANTIMPUT)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECIMPUT() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECIMPUT, Parameters.FECIMPUT)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ALMACEN() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ALMACEN, Parameters.ALMACEN)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DIAMETRO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DIAMETRO, Parameters.DIAMETRO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property LARGO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.LARGO, Parameters.LARGO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ANCHO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ANCHO, Parameters.ANCHO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property GRUESO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.GRUESO, Parameters.GRUESO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TRATAM() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TRATAM, Parameters.TRATAM)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TRATAM2() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TRATAM2, Parameters.TRATAM2)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property OBSERV() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.OBSERV, Parameters.OBSERV)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property OBSERV2() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.OBSERV2, Parameters.OBSERV2)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property SITUAC() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.SITUAC, Parameters.SITUAC)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECDEF() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECDEF, Parameters.FECDEF)
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

                Public ReadOnly Property DIAMDEF() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DIAMDEF, Parameters.DIAMDEF)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property LARGDEF() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.LARGDEF, Parameters.LARGDEF)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ANCHDEF() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ANCHDEF, Parameters.ANCHDEF)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property GRUEDEF() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.GRUEDEF, Parameters.GRUEDEF)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property MOTIVO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.MOTIVO, Parameters.MOTIVO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property REALIZA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.REALIZA, Parameters.REALIZA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CPLISDENO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CPLISDENO, Parameters.CPLISDENO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property OTMARDES() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.OTMARDES, Parameters.OTMARDES)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ORDEN() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ORDEN, Parameters.ORDEN)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property OTDUREZA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.OTDUREZA, Parameters.OTDUREZA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property OTTRASEC() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.OTTRASEC, Parameters.OTTRASEC)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property OTTRATAM() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.OTTRATAM, Parameters.OTTRATAM)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property OTMATESPE() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.OTMATESPE, Parameters.OTMATESPE)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property


                Private _clause As AggregateClause
            End Class
#End Region

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

            Public ReadOnly Property NUMLISTA() As AggregateParameter
                Get
                    If _NUMLISTA_W Is Nothing Then
                        _NUMLISTA_W = TearOff.NUMLISTA
                    End If
                    Return _NUMLISTA_W
                End Get
            End Property

            Public ReadOnly Property TIPOLISTA() As AggregateParameter
                Get
                    If _TIPOLISTA_W Is Nothing Then
                        _TIPOLISTA_W = TearOff.TIPOLISTA
                    End If
                    Return _TIPOLISTA_W
                End Get
            End Property

            Public ReadOnly Property NPLANO() As AggregateParameter
                Get
                    If _NPLANO_W Is Nothing Then
                        _NPLANO_W = TearOff.NPLANO
                    End If
                    Return _NPLANO_W
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

            Public ReadOnly Property MATERIAL() As AggregateParameter
                Get
                    If _MATERIAL_W Is Nothing Then
                        _MATERIAL_W = TearOff.MATERIAL
                    End If
                    Return _MATERIAL_W
                End Get
            End Property

            Public ReadOnly Property CANNEC() As AggregateParameter
                Get
                    If _CANNEC_W Is Nothing Then
                        _CANNEC_W = TearOff.CANNEC
                    End If
                    Return _CANNEC_W
                End Get
            End Property

            Public ReadOnly Property FECNEC() As AggregateParameter
                Get
                    If _FECNEC_W Is Nothing Then
                        _FECNEC_W = TearOff.FECNEC
                    End If
                    Return _FECNEC_W
                End Get
            End Property

            Public ReadOnly Property CANTIMPUT() As AggregateParameter
                Get
                    If _CANTIMPUT_W Is Nothing Then
                        _CANTIMPUT_W = TearOff.CANTIMPUT
                    End If
                    Return _CANTIMPUT_W
                End Get
            End Property

            Public ReadOnly Property FECIMPUT() As AggregateParameter
                Get
                    If _FECIMPUT_W Is Nothing Then
                        _FECIMPUT_W = TearOff.FECIMPUT
                    End If
                    Return _FECIMPUT_W
                End Get
            End Property

            Public ReadOnly Property ALMACEN() As AggregateParameter
                Get
                    If _ALMACEN_W Is Nothing Then
                        _ALMACEN_W = TearOff.ALMACEN
                    End If
                    Return _ALMACEN_W
                End Get
            End Property

            Public ReadOnly Property DIAMETRO() As AggregateParameter
                Get
                    If _DIAMETRO_W Is Nothing Then
                        _DIAMETRO_W = TearOff.DIAMETRO
                    End If
                    Return _DIAMETRO_W
                End Get
            End Property

            Public ReadOnly Property LARGO() As AggregateParameter
                Get
                    If _LARGO_W Is Nothing Then
                        _LARGO_W = TearOff.LARGO
                    End If
                    Return _LARGO_W
                End Get
            End Property

            Public ReadOnly Property ANCHO() As AggregateParameter
                Get
                    If _ANCHO_W Is Nothing Then
                        _ANCHO_W = TearOff.ANCHO
                    End If
                    Return _ANCHO_W
                End Get
            End Property

            Public ReadOnly Property GRUESO() As AggregateParameter
                Get
                    If _GRUESO_W Is Nothing Then
                        _GRUESO_W = TearOff.GRUESO
                    End If
                    Return _GRUESO_W
                End Get
            End Property

            Public ReadOnly Property TRATAM() As AggregateParameter
                Get
                    If _TRATAM_W Is Nothing Then
                        _TRATAM_W = TearOff.TRATAM
                    End If
                    Return _TRATAM_W
                End Get
            End Property

            Public ReadOnly Property TRATAM2() As AggregateParameter
                Get
                    If _TRATAM2_W Is Nothing Then
                        _TRATAM2_W = TearOff.TRATAM2
                    End If
                    Return _TRATAM2_W
                End Get
            End Property

            Public ReadOnly Property OBSERV() As AggregateParameter
                Get
                    If _OBSERV_W Is Nothing Then
                        _OBSERV_W = TearOff.OBSERV
                    End If
                    Return _OBSERV_W
                End Get
            End Property

            Public ReadOnly Property OBSERV2() As AggregateParameter
                Get
                    If _OBSERV2_W Is Nothing Then
                        _OBSERV2_W = TearOff.OBSERV2
                    End If
                    Return _OBSERV2_W
                End Get
            End Property

            Public ReadOnly Property SITUAC() As AggregateParameter
                Get
                    If _SITUAC_W Is Nothing Then
                        _SITUAC_W = TearOff.SITUAC
                    End If
                    Return _SITUAC_W
                End Get
            End Property

            Public ReadOnly Property FECDEF() As AggregateParameter
                Get
                    If _FECDEF_W Is Nothing Then
                        _FECDEF_W = TearOff.FECDEF
                    End If
                    Return _FECDEF_W
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

            Public ReadOnly Property DIAMDEF() As AggregateParameter
                Get
                    If _DIAMDEF_W Is Nothing Then
                        _DIAMDEF_W = TearOff.DIAMDEF
                    End If
                    Return _DIAMDEF_W
                End Get
            End Property

            Public ReadOnly Property LARGDEF() As AggregateParameter
                Get
                    If _LARGDEF_W Is Nothing Then
                        _LARGDEF_W = TearOff.LARGDEF
                    End If
                    Return _LARGDEF_W
                End Get
            End Property

            Public ReadOnly Property ANCHDEF() As AggregateParameter
                Get
                    If _ANCHDEF_W Is Nothing Then
                        _ANCHDEF_W = TearOff.ANCHDEF
                    End If
                    Return _ANCHDEF_W
                End Get
            End Property

            Public ReadOnly Property GRUEDEF() As AggregateParameter
                Get
                    If _GRUEDEF_W Is Nothing Then
                        _GRUEDEF_W = TearOff.GRUEDEF
                    End If
                    Return _GRUEDEF_W
                End Get
            End Property

            Public ReadOnly Property MOTIVO() As AggregateParameter
                Get
                    If _MOTIVO_W Is Nothing Then
                        _MOTIVO_W = TearOff.MOTIVO
                    End If
                    Return _MOTIVO_W
                End Get
            End Property

            Public ReadOnly Property REALIZA() As AggregateParameter
                Get
                    If _REALIZA_W Is Nothing Then
                        _REALIZA_W = TearOff.REALIZA
                    End If
                    Return _REALIZA_W
                End Get
            End Property

            Public ReadOnly Property CPLISDENO() As AggregateParameter
                Get
                    If _CPLISDENO_W Is Nothing Then
                        _CPLISDENO_W = TearOff.CPLISDENO
                    End If
                    Return _CPLISDENO_W
                End Get
            End Property

            Public ReadOnly Property OTMARDES() As AggregateParameter
                Get
                    If _OTMARDES_W Is Nothing Then
                        _OTMARDES_W = TearOff.OTMARDES
                    End If
                    Return _OTMARDES_W
                End Get
            End Property

            Public ReadOnly Property ORDEN() As AggregateParameter
                Get
                    If _ORDEN_W Is Nothing Then
                        _ORDEN_W = TearOff.ORDEN
                    End If
                    Return _ORDEN_W
                End Get
            End Property

            Public ReadOnly Property OTDUREZA() As AggregateParameter
                Get
                    If _OTDUREZA_W Is Nothing Then
                        _OTDUREZA_W = TearOff.OTDUREZA
                    End If
                    Return _OTDUREZA_W
                End Get
            End Property

            Public ReadOnly Property OTTRASEC() As AggregateParameter
                Get
                    If _OTTRASEC_W Is Nothing Then
                        _OTTRASEC_W = TearOff.OTTRASEC
                    End If
                    Return _OTTRASEC_W
                End Get
            End Property

            Public ReadOnly Property OTTRATAM() As AggregateParameter
                Get
                    If _OTTRATAM_W Is Nothing Then
                        _OTTRATAM_W = TearOff.OTTRATAM
                    End If
                    Return _OTTRATAM_W
                End Get
            End Property

            Public ReadOnly Property OTMATESPE() As AggregateParameter
                Get
                    If _OTMATESPE_W Is Nothing Then
                        _OTMATESPE_W = TearOff.OTMATESPE
                    End If
                    Return _OTMATESPE_W
                End Get
            End Property

            Private _NUMORD_W As AggregateParameter = Nothing
            Private _NUMOPE_W As AggregateParameter = Nothing
            Private _NUMMAR_W As AggregateParameter = Nothing
            Private _NUMLISTA_W As AggregateParameter = Nothing
            Private _TIPOLISTA_W As AggregateParameter = Nothing
            Private _NPLANO_W As AggregateParameter = Nothing
            Private _CODART_W As AggregateParameter = Nothing
            Private _MATERIAL_W As AggregateParameter = Nothing
            Private _CANNEC_W As AggregateParameter = Nothing
            Private _FECNEC_W As AggregateParameter = Nothing
            Private _CANTIMPUT_W As AggregateParameter = Nothing
            Private _FECIMPUT_W As AggregateParameter = Nothing
            Private _ALMACEN_W As AggregateParameter = Nothing
            Private _DIAMETRO_W As AggregateParameter = Nothing
            Private _LARGO_W As AggregateParameter = Nothing
            Private _ANCHO_W As AggregateParameter = Nothing
            Private _GRUESO_W As AggregateParameter = Nothing
            Private _TRATAM_W As AggregateParameter = Nothing
            Private _TRATAM2_W As AggregateParameter = Nothing
            Private _OBSERV_W As AggregateParameter = Nothing
            Private _OBSERV2_W As AggregateParameter = Nothing
            Private _SITUAC_W As AggregateParameter = Nothing
            Private _FECDEF_W As AggregateParameter = Nothing
            Private _FASE_W As AggregateParameter = Nothing
            Private _DIAMDEF_W As AggregateParameter = Nothing
            Private _LARGDEF_W As AggregateParameter = Nothing
            Private _ANCHDEF_W As AggregateParameter = Nothing
            Private _GRUEDEF_W As AggregateParameter = Nothing
            Private _MOTIVO_W As AggregateParameter = Nothing
            Private _REALIZA_W As AggregateParameter = Nothing
            Private _CPLISDENO_W As AggregateParameter = Nothing
            Private _OTMARDES_W As AggregateParameter = Nothing
            Private _ORDEN_W As AggregateParameter = Nothing
            Private _OTDUREZA_W As AggregateParameter = Nothing
            Private _OTTRASEC_W As AggregateParameter = Nothing
            Private _OTTRATAM_W As AggregateParameter = Nothing
            Private _OTMATESPE_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _NUMORD_W = Nothing
                _NUMOPE_W = Nothing
                _NUMMAR_W = Nothing
                _NUMLISTA_W = Nothing
                _TIPOLISTA_W = Nothing
                _NPLANO_W = Nothing
                _CODART_W = Nothing
                _MATERIAL_W = Nothing
                _CANNEC_W = Nothing
                _FECNEC_W = Nothing
                _CANTIMPUT_W = Nothing
                _FECIMPUT_W = Nothing
                _ALMACEN_W = Nothing
                _DIAMETRO_W = Nothing
                _LARGO_W = Nothing
                _ANCHO_W = Nothing
                _GRUESO_W = Nothing
                _TRATAM_W = Nothing
                _TRATAM2_W = Nothing
                _OBSERV_W = Nothing
                _OBSERV2_W = Nothing
                _SITUAC_W = Nothing
                _FECDEF_W = Nothing
                _FASE_W = Nothing
                _DIAMDEF_W = Nothing
                _LARGDEF_W = Nothing
                _ANCHDEF_W = Nothing
                _GRUEDEF_W = Nothing
                _MOTIVO_W = Nothing
                _REALIZA_W = Nothing
                _CPLISDENO_W = Nothing
                _OTMARDES_W = Nothing
                _ORDEN_W = Nothing
                _OTDUREZA_W = Nothing
                _OTTRASEC_W = Nothing
                _OTTRATAM_W = Nothing
                _OTMATESPE_W = Nothing
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

