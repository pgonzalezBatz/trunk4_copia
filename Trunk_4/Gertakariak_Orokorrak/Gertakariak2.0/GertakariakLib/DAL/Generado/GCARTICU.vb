



'===============================================================================
'BATZ, Koop. - 29/09/2009 8:52:18
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

    Public MustInherit Class _GCARTICU
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "XBAT."
            Me.QuerySource = "GCARTICU"
            Me.MappingName = "GCARTICU"
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


            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_GCARTICU", parameters)
        End Function

        '=================================================================
        ' Public Overridable Function LoadByPrimaryKey()  As Boolean
        '=================================================================
        '  Loads a single row of via the primary key
        '=================================================================
        Public Overridable Function LoadByPrimaryKey(ByVal CODART As String) As Boolean

            Dim parameters As ListDictionary = New ListDictionary()
            parameters.Add(_GCARTICU.Parameters.CODART, CODART)


            Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
            p.Direction = ParameterDirection.Output
            parameters.Add(p, Nothing)

            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_GCARTICU", parameters)

        End Function

#Region "Parameters"
        Protected Class Parameters

            Public Shared ReadOnly Property CODART As OracleParameter
                Get
                    Return New OracleParameter("p_CODART", OracleDbType.Char, 12)
                End Get
            End Property

            Public Shared ReadOnly Property DESCRI As OracleParameter
                Get
                    Return New OracleParameter("p_DESCRI", OracleDbType.Char, 30)
                End Get
            End Property

            Public Shared ReadOnly Property DESCRI2 As OracleParameter
                Get
                    Return New OracleParameter("p_DESCRI2", OracleDbType.Char, 30)
                End Get
            End Property

            Public Shared ReadOnly Property CODFAM As OracleParameter
                Get
                    Return New OracleParameter("p_CODFAM", OracleDbType.Char, 4)
                End Get
            End Property

            Public Shared ReadOnly Property SUBFAM As OracleParameter
                Get
                    Return New OracleParameter("p_SUBFAM", OracleDbType.Char, 4)
                End Get
            End Property

            Public Shared ReadOnly Property CRIAGRU As OracleParameter
                Get
                    Return New OracleParameter("p_CRIAGRU", OracleDbType.Char, 4)
                End Get
            End Property

            Public Shared ReadOnly Property CODPRO As OracleParameter
                Get
                    Return New OracleParameter("p_CODPRO", OracleDbType.Char, 12)
                End Get
            End Property

            Public Shared ReadOnly Property CODALM As OracleParameter
                Get
                    Return New OracleParameter("p_CODALM", OracleDbType.Char, 4)
                End Get
            End Property

            Public Shared ReadOnly Property TIPIVA As OracleParameter
                Get
                    Return New OracleParameter("p_TIPIVA", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property STKMIN As OracleParameter
                Get
                    Return New OracleParameter("p_STKMIN", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property STKMAX As OracleParameter
                Get
                    Return New OracleParameter("p_STKMAX", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property ULTPC As OracleParameter
                Get
                    Return New OracleParameter("p_ULTPC", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property PREMED As OracleParameter
                Get
                    Return New OracleParameter("p_PREMED", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property FECULTC As OracleParameter
                Get
                    Return New OracleParameter("p_FECULTC", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property ULTCANT As OracleParameter
                Get
                    Return New OracleParameter("p_ULTCANT", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property TOTPED As OracleParameter
                Get
                    Return New OracleParameter("p_TOTPED", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property STOCK As OracleParameter
                Get
                    Return New OracleParameter("p_STOCK", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property GESTOCK As OracleParameter
                Get
                    Return New OracleParameter("p_GESTOCK", OracleDbType.Char, 1)
                End Get
            End Property

            Public Shared ReadOnly Property CTA_OF As OracleParameter
                Get
                    Return New OracleParameter("p_CTA_OF", OracleDbType.Char, 1)
                End Get
            End Property

            Public Shared ReadOnly Property CTACOMP As OracleParameter
                Get
                    Return New OracleParameter("p_CTACOMP", OracleDbType.Varchar2, 15)
                End Get
            End Property

            Public Shared ReadOnly Property TIPIMPUT As OracleParameter
                Get
                    Return New OracleParameter("p_TIPIMPUT", OracleDbType.Char, 1)
                End Get
            End Property

            Public Shared ReadOnly Property CORTES As OracleParameter
                Get
                    Return New OracleParameter("p_CORTES", OracleDbType.Char, 1)
                End Get
            End Property

            Public Shared ReadOnly Property ALEACIONES As OracleParameter
                Get
                    Return New OracleParameter("p_ALEACIONES", OracleDbType.Char, 1)
                End Get
            End Property

            Public Shared ReadOnly Property OBSOLETO As OracleParameter
                Get
                    Return New OracleParameter("p_OBSOLETO", OracleDbType.Char, 1)
                End Get
            End Property

            Public Shared ReadOnly Property PRODUCTIVO As OracleParameter
                Get
                    Return New OracleParameter("p_PRODUCTIVO", OracleDbType.Char, 1)
                End Get
            End Property

            Public Shared ReadOnly Property BALDA1 As OracleParameter
                Get
                    Return New OracleParameter("p_BALDA1", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property CAJON1 As OracleParameter
                Get
                    Return New OracleParameter("p_CAJON1", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property BALDA2 As OracleParameter
                Get
                    Return New OracleParameter("p_BALDA2", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property CAJON2 As OracleParameter
                Get
                    Return New OracleParameter("p_CAJON2", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property NORMA As OracleParameter
                Get
                    Return New OracleParameter("p_NORMA", OracleDbType.Char, 10)
                End Get
            End Property

            Public Shared ReadOnly Property UD_PRESTAMO As OracleParameter
                Get
                    Return New OracleParameter("p_UD_PRESTAMO", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property REF_CLI As OracleParameter
                Get
                    Return New OracleParameter("p_REF_CLI", OracleDbType.Varchar2, 80)
                End Get
            End Property

            Public Shared ReadOnly Property COD_CLI As OracleParameter
                Get
                    Return New OracleParameter("p_COD_CLI", OracleDbType.Char, 12)
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

            Public Shared ReadOnly Property EULTPC As OracleParameter
                Get
                    Return New OracleParameter("p_EULTPC", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property EPREMED As OracleParameter
                Get
                    Return New OracleParameter("p_EPREMED", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property EDIMPRE As OracleParameter
                Get
                    Return New OracleParameter("p_EDIMPRE", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property GENERIKO As OracleParameter
                Get
                    Return New OracleParameter("p_GENERIKO", OracleDbType.Varchar2, 1)
                End Get
            End Property

            Public Shared ReadOnly Property REFERENCIA As OracleParameter
                Get
                    Return New OracleParameter("p_REFERENCIA", OracleDbType.Varchar2, 80)
                End Get
            End Property

            Public Shared ReadOnly Property MARCA As OracleParameter
                Get
                    Return New OracleParameter("p_MARCA", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property TIPO As OracleParameter
                Get
                    Return New OracleParameter("p_TIPO", OracleDbType.Varchar2, 1)
                End Get
            End Property

            Public Shared ReadOnly Property POTENCIA As OracleParameter
                Get
                    Return New OracleParameter("p_POTENCIA", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property CARRERA As OracleParameter
                Get
                    Return New OracleParameter("p_CARRERA", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property DATA As OracleParameter
                Get
                    Return New OracleParameter("p_DATA", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property CODPER As OracleParameter
                Get
                    Return New OracleParameter("p_CODPER", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property CERT_CALIDAD As OracleParameter
                Get
                    Return New OracleParameter("p_CERT_CALIDAD", OracleDbType.Int32, 0)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const CODART As String = "CODART"
            Public Const DESCRI As String = "DESCRI"
            Public Const DESCRI2 As String = "DESCRI2"
            Public Const CODFAM As String = "CODFAM"
            Public Const SUBFAM As String = "SUBFAM"
            Public Const CRIAGRU As String = "CRIAGRU"
            Public Const CODPRO As String = "CODPRO"
            Public Const CODALM As String = "CODALM"
            Public Const TIPIVA As String = "TIPIVA"
            Public Const STKMIN As String = "STKMIN"
            Public Const STKMAX As String = "STKMAX"
            Public Const ULTPC As String = "ULTPC"
            Public Const PREMED As String = "PREMED"
            Public Const FECULTC As String = "FECULTC"
            Public Const ULTCANT As String = "ULTCANT"
            Public Const TOTPED As String = "TOTPED"
            Public Const STOCK As String = "STOCK"
            Public Const GESTOCK As String = "GESTOCK"
            Public Const CTA_OF As String = "CTA_OF"
            Public Const CTACOMP As String = "CTACOMP"
            Public Const TIPIMPUT As String = "TIPIMPUT"
            Public Const CORTES As String = "CORTES"
            Public Const ALEACIONES As String = "ALEACIONES"
            Public Const OBSOLETO As String = "OBSOLETO"
            Public Const PRODUCTIVO As String = "PRODUCTIVO"
            Public Const BALDA1 As String = "BALDA1"
            Public Const CAJON1 As String = "CAJON1"
            Public Const BALDA2 As String = "BALDA2"
            Public Const CAJON2 As String = "CAJON2"
            Public Const NORMA As String = "NORMA"
            Public Const UD_PRESTAMO As String = "UD_PRESTAMO"
            Public Const REF_CLI As String = "REF_CLI"
            Public Const COD_CLI As String = "COD_CLI"
            Public Const NUMORD As String = "NUMORD"
            Public Const NUMOPE As String = "NUMOPE"
            Public Const EULTPC As String = "EULTPC"
            Public Const EPREMED As String = "EPREMED"
            Public Const EDIMPRE As String = "EDIMPRE"
            Public Const GENERIKO As String = "GENERIKO"
            Public Const REFERENCIA As String = "REFERENCIA"
            Public Const MARCA As String = "MARCA"
            Public Const TIPO As String = "TIPO"
            Public Const POTENCIA As String = "POTENCIA"
            Public Const CARRERA As String = "CARRERA"
            Public Const DATA As String = "DATA"
            Public Const CODPER As String = "CODPER"
            Public Const CERT_CALIDAD As String = "CERT_CALIDAD"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(CODART) = _GCARTICU.PropertyNames.CODART
                    ht(DESCRI) = _GCARTICU.PropertyNames.DESCRI
                    ht(DESCRI2) = _GCARTICU.PropertyNames.DESCRI2
                    ht(CODFAM) = _GCARTICU.PropertyNames.CODFAM
                    ht(SUBFAM) = _GCARTICU.PropertyNames.SUBFAM
                    ht(CRIAGRU) = _GCARTICU.PropertyNames.CRIAGRU
                    ht(CODPRO) = _GCARTICU.PropertyNames.CODPRO
                    ht(CODALM) = _GCARTICU.PropertyNames.CODALM
                    ht(TIPIVA) = _GCARTICU.PropertyNames.TIPIVA
                    ht(STKMIN) = _GCARTICU.PropertyNames.STKMIN
                    ht(STKMAX) = _GCARTICU.PropertyNames.STKMAX
                    ht(ULTPC) = _GCARTICU.PropertyNames.ULTPC
                    ht(PREMED) = _GCARTICU.PropertyNames.PREMED
                    ht(FECULTC) = _GCARTICU.PropertyNames.FECULTC
                    ht(ULTCANT) = _GCARTICU.PropertyNames.ULTCANT
                    ht(TOTPED) = _GCARTICU.PropertyNames.TOTPED
                    ht(STOCK) = _GCARTICU.PropertyNames.STOCK
                    ht(GESTOCK) = _GCARTICU.PropertyNames.GESTOCK
                    ht(CTA_OF) = _GCARTICU.PropertyNames.CTA_OF
                    ht(CTACOMP) = _GCARTICU.PropertyNames.CTACOMP
                    ht(TIPIMPUT) = _GCARTICU.PropertyNames.TIPIMPUT
                    ht(CORTES) = _GCARTICU.PropertyNames.CORTES
                    ht(ALEACIONES) = _GCARTICU.PropertyNames.ALEACIONES
                    ht(OBSOLETO) = _GCARTICU.PropertyNames.OBSOLETO
                    ht(PRODUCTIVO) = _GCARTICU.PropertyNames.PRODUCTIVO
                    ht(BALDA1) = _GCARTICU.PropertyNames.BALDA1
                    ht(CAJON1) = _GCARTICU.PropertyNames.CAJON1
                    ht(BALDA2) = _GCARTICU.PropertyNames.BALDA2
                    ht(CAJON2) = _GCARTICU.PropertyNames.CAJON2
                    ht(NORMA) = _GCARTICU.PropertyNames.NORMA
                    ht(UD_PRESTAMO) = _GCARTICU.PropertyNames.UD_PRESTAMO
                    ht(REF_CLI) = _GCARTICU.PropertyNames.REF_CLI
                    ht(COD_CLI) = _GCARTICU.PropertyNames.COD_CLI
                    ht(NUMORD) = _GCARTICU.PropertyNames.NUMORD
                    ht(NUMOPE) = _GCARTICU.PropertyNames.NUMOPE
                    ht(EULTPC) = _GCARTICU.PropertyNames.EULTPC
                    ht(EPREMED) = _GCARTICU.PropertyNames.EPREMED
                    ht(EDIMPRE) = _GCARTICU.PropertyNames.EDIMPRE
                    ht(GENERIKO) = _GCARTICU.PropertyNames.GENERIKO
                    ht(REFERENCIA) = _GCARTICU.PropertyNames.REFERENCIA
                    ht(MARCA) = _GCARTICU.PropertyNames.MARCA
                    ht(TIPO) = _GCARTICU.PropertyNames.TIPO
                    ht(POTENCIA) = _GCARTICU.PropertyNames.POTENCIA
                    ht(CARRERA) = _GCARTICU.PropertyNames.CARRERA
                    ht(DATA) = _GCARTICU.PropertyNames.DATA
                    ht(CODPER) = _GCARTICU.PropertyNames.CODPER
                    ht(CERT_CALIDAD) = _GCARTICU.PropertyNames.CERT_CALIDAD

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const CODART As String = "CODART"
            Public Const DESCRI As String = "DESCRI"
            Public Const DESCRI2 As String = "DESCRI2"
            Public Const CODFAM As String = "CODFAM"
            Public Const SUBFAM As String = "SUBFAM"
            Public Const CRIAGRU As String = "CRIAGRU"
            Public Const CODPRO As String = "CODPRO"
            Public Const CODALM As String = "CODALM"
            Public Const TIPIVA As String = "TIPIVA"
            Public Const STKMIN As String = "STKMIN"
            Public Const STKMAX As String = "STKMAX"
            Public Const ULTPC As String = "ULTPC"
            Public Const PREMED As String = "PREMED"
            Public Const FECULTC As String = "FECULTC"
            Public Const ULTCANT As String = "ULTCANT"
            Public Const TOTPED As String = "TOTPED"
            Public Const STOCK As String = "STOCK"
            Public Const GESTOCK As String = "GESTOCK"
            Public Const CTA_OF As String = "CTA_OF"
            Public Const CTACOMP As String = "CTACOMP"
            Public Const TIPIMPUT As String = "TIPIMPUT"
            Public Const CORTES As String = "CORTES"
            Public Const ALEACIONES As String = "ALEACIONES"
            Public Const OBSOLETO As String = "OBSOLETO"
            Public Const PRODUCTIVO As String = "PRODUCTIVO"
            Public Const BALDA1 As String = "BALDA1"
            Public Const CAJON1 As String = "CAJON1"
            Public Const BALDA2 As String = "BALDA2"
            Public Const CAJON2 As String = "CAJON2"
            Public Const NORMA As String = "NORMA"
            Public Const UD_PRESTAMO As String = "UD_PRESTAMO"
            Public Const REF_CLI As String = "REF_CLI"
            Public Const COD_CLI As String = "COD_CLI"
            Public Const NUMORD As String = "NUMORD"
            Public Const NUMOPE As String = "NUMOPE"
            Public Const EULTPC As String = "EULTPC"
            Public Const EPREMED As String = "EPREMED"
            Public Const EDIMPRE As String = "EDIMPRE"
            Public Const GENERIKO As String = "GENERIKO"
            Public Const REFERENCIA As String = "REFERENCIA"
            Public Const MARCA As String = "MARCA"
            Public Const TIPO As String = "TIPO"
            Public Const POTENCIA As String = "POTENCIA"
            Public Const CARRERA As String = "CARRERA"
            Public Const DATA As String = "DATA"
            Public Const CODPER As String = "CODPER"
            Public Const CERT_CALIDAD As String = "CERT_CALIDAD"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(CODART) = _GCARTICU.ColumnNames.CODART
                    ht(DESCRI) = _GCARTICU.ColumnNames.DESCRI
                    ht(DESCRI2) = _GCARTICU.ColumnNames.DESCRI2
                    ht(CODFAM) = _GCARTICU.ColumnNames.CODFAM
                    ht(SUBFAM) = _GCARTICU.ColumnNames.SUBFAM
                    ht(CRIAGRU) = _GCARTICU.ColumnNames.CRIAGRU
                    ht(CODPRO) = _GCARTICU.ColumnNames.CODPRO
                    ht(CODALM) = _GCARTICU.ColumnNames.CODALM
                    ht(TIPIVA) = _GCARTICU.ColumnNames.TIPIVA
                    ht(STKMIN) = _GCARTICU.ColumnNames.STKMIN
                    ht(STKMAX) = _GCARTICU.ColumnNames.STKMAX
                    ht(ULTPC) = _GCARTICU.ColumnNames.ULTPC
                    ht(PREMED) = _GCARTICU.ColumnNames.PREMED
                    ht(FECULTC) = _GCARTICU.ColumnNames.FECULTC
                    ht(ULTCANT) = _GCARTICU.ColumnNames.ULTCANT
                    ht(TOTPED) = _GCARTICU.ColumnNames.TOTPED
                    ht(STOCK) = _GCARTICU.ColumnNames.STOCK
                    ht(GESTOCK) = _GCARTICU.ColumnNames.GESTOCK
                    ht(CTA_OF) = _GCARTICU.ColumnNames.CTA_OF
                    ht(CTACOMP) = _GCARTICU.ColumnNames.CTACOMP
                    ht(TIPIMPUT) = _GCARTICU.ColumnNames.TIPIMPUT
                    ht(CORTES) = _GCARTICU.ColumnNames.CORTES
                    ht(ALEACIONES) = _GCARTICU.ColumnNames.ALEACIONES
                    ht(OBSOLETO) = _GCARTICU.ColumnNames.OBSOLETO
                    ht(PRODUCTIVO) = _GCARTICU.ColumnNames.PRODUCTIVO
                    ht(BALDA1) = _GCARTICU.ColumnNames.BALDA1
                    ht(CAJON1) = _GCARTICU.ColumnNames.CAJON1
                    ht(BALDA2) = _GCARTICU.ColumnNames.BALDA2
                    ht(CAJON2) = _GCARTICU.ColumnNames.CAJON2
                    ht(NORMA) = _GCARTICU.ColumnNames.NORMA
                    ht(UD_PRESTAMO) = _GCARTICU.ColumnNames.UD_PRESTAMO
                    ht(REF_CLI) = _GCARTICU.ColumnNames.REF_CLI
                    ht(COD_CLI) = _GCARTICU.ColumnNames.COD_CLI
                    ht(NUMORD) = _GCARTICU.ColumnNames.NUMORD
                    ht(NUMOPE) = _GCARTICU.ColumnNames.NUMOPE
                    ht(EULTPC) = _GCARTICU.ColumnNames.EULTPC
                    ht(EPREMED) = _GCARTICU.ColumnNames.EPREMED
                    ht(EDIMPRE) = _GCARTICU.ColumnNames.EDIMPRE
                    ht(GENERIKO) = _GCARTICU.ColumnNames.GENERIKO
                    ht(REFERENCIA) = _GCARTICU.ColumnNames.REFERENCIA
                    ht(MARCA) = _GCARTICU.ColumnNames.MARCA
                    ht(TIPO) = _GCARTICU.ColumnNames.TIPO
                    ht(POTENCIA) = _GCARTICU.ColumnNames.POTENCIA
                    ht(CARRERA) = _GCARTICU.ColumnNames.CARRERA
                    ht(DATA) = _GCARTICU.ColumnNames.DATA
                    ht(CODPER) = _GCARTICU.ColumnNames.CODPER
                    ht(CERT_CALIDAD) = _GCARTICU.ColumnNames.CERT_CALIDAD

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const CODART As String = "s_CODART"
            Public Const DESCRI As String = "s_DESCRI"
            Public Const DESCRI2 As String = "s_DESCRI2"
            Public Const CODFAM As String = "s_CODFAM"
            Public Const SUBFAM As String = "s_SUBFAM"
            Public Const CRIAGRU As String = "s_CRIAGRU"
            Public Const CODPRO As String = "s_CODPRO"
            Public Const CODALM As String = "s_CODALM"
            Public Const TIPIVA As String = "s_TIPIVA"
            Public Const STKMIN As String = "s_STKMIN"
            Public Const STKMAX As String = "s_STKMAX"
            Public Const ULTPC As String = "s_ULTPC"
            Public Const PREMED As String = "s_PREMED"
            Public Const FECULTC As String = "s_FECULTC"
            Public Const ULTCANT As String = "s_ULTCANT"
            Public Const TOTPED As String = "s_TOTPED"
            Public Const STOCK As String = "s_STOCK"
            Public Const GESTOCK As String = "s_GESTOCK"
            Public Const CTA_OF As String = "s_CTA_OF"
            Public Const CTACOMP As String = "s_CTACOMP"
            Public Const TIPIMPUT As String = "s_TIPIMPUT"
            Public Const CORTES As String = "s_CORTES"
            Public Const ALEACIONES As String = "s_ALEACIONES"
            Public Const OBSOLETO As String = "s_OBSOLETO"
            Public Const PRODUCTIVO As String = "s_PRODUCTIVO"
            Public Const BALDA1 As String = "s_BALDA1"
            Public Const CAJON1 As String = "s_CAJON1"
            Public Const BALDA2 As String = "s_BALDA2"
            Public Const CAJON2 As String = "s_CAJON2"
            Public Const NORMA As String = "s_NORMA"
            Public Const UD_PRESTAMO As String = "s_UD_PRESTAMO"
            Public Const REF_CLI As String = "s_REF_CLI"
            Public Const COD_CLI As String = "s_COD_CLI"
            Public Const NUMORD As String = "s_NUMORD"
            Public Const NUMOPE As String = "s_NUMOPE"
            Public Const EULTPC As String = "s_EULTPC"
            Public Const EPREMED As String = "s_EPREMED"
            Public Const EDIMPRE As String = "s_EDIMPRE"
            Public Const GENERIKO As String = "s_GENERIKO"
            Public Const REFERENCIA As String = "s_REFERENCIA"
            Public Const MARCA As String = "s_MARCA"
            Public Const TIPO As String = "s_TIPO"
            Public Const POTENCIA As String = "s_POTENCIA"
            Public Const CARRERA As String = "s_CARRERA"
            Public Const DATA As String = "s_DATA"
            Public Const CODPER As String = "s_CODPER"
            Public Const CERT_CALIDAD As String = "s_CERT_CALIDAD"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property CODART As String
            Get
                Return MyBase.GetString(ColumnNames.CODART)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CODART, Value)
            End Set
        End Property

        Public Overridable Property DESCRI As String
            Get
                Return MyBase.GetString(ColumnNames.DESCRI)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.DESCRI, Value)
            End Set
        End Property

        Public Overridable Property DESCRI2 As String
            Get
                Return MyBase.GetString(ColumnNames.DESCRI2)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.DESCRI2, Value)
            End Set
        End Property

        Public Overridable Property CODFAM As String
            Get
                Return MyBase.GetString(ColumnNames.CODFAM)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CODFAM, Value)
            End Set
        End Property

        Public Overridable Property SUBFAM As String
            Get
                Return MyBase.GetString(ColumnNames.SUBFAM)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.SUBFAM, Value)
            End Set
        End Property

        Public Overridable Property CRIAGRU As String
            Get
                Return MyBase.GetString(ColumnNames.CRIAGRU)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CRIAGRU, Value)
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

        Public Overridable Property CODALM As String
            Get
                Return MyBase.GetString(ColumnNames.CODALM)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CODALM, Value)
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

        Public Overridable Property STKMIN As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.STKMIN)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.STKMIN, Value)
            End Set
        End Property

        Public Overridable Property STKMAX As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.STKMAX)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.STKMAX, Value)
            End Set
        End Property

        Public Overridable Property ULTPC As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.ULTPC)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.ULTPC, Value)
            End Set
        End Property

        Public Overridable Property PREMED As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.PREMED)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.PREMED, Value)
            End Set
        End Property

        Public Overridable Property FECULTC As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECULTC)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECULTC, Value)
            End Set
        End Property

        Public Overridable Property ULTCANT As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.ULTCANT)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.ULTCANT, Value)
            End Set
        End Property

        Public Overridable Property TOTPED As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.TOTPED)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.TOTPED, Value)
            End Set
        End Property

        Public Overridable Property STOCK As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.STOCK)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.STOCK, Value)
            End Set
        End Property

        Public Overridable Property GESTOCK As String
            Get
                Return MyBase.GetString(ColumnNames.GESTOCK)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.GESTOCK, Value)
            End Set
        End Property

        Public Overridable Property CTA_OF As String
            Get
                Return MyBase.GetString(ColumnNames.CTA_OF)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CTA_OF, Value)
            End Set
        End Property

        Public Overridable Property CTACOMP As String
            Get
                Return MyBase.GetString(ColumnNames.CTACOMP)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CTACOMP, Value)
            End Set
        End Property

        Public Overridable Property TIPIMPUT As String
            Get
                Return MyBase.GetString(ColumnNames.TIPIMPUT)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.TIPIMPUT, Value)
            End Set
        End Property

        Public Overridable Property CORTES As String
            Get
                Return MyBase.GetString(ColumnNames.CORTES)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CORTES, Value)
            End Set
        End Property

        Public Overridable Property ALEACIONES As String
            Get
                Return MyBase.GetString(ColumnNames.ALEACIONES)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.ALEACIONES, Value)
            End Set
        End Property

        Public Overridable Property OBSOLETO As String
            Get
                Return MyBase.GetString(ColumnNames.OBSOLETO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.OBSOLETO, Value)
            End Set
        End Property

        Public Overridable Property PRODUCTIVO As String
            Get
                Return MyBase.GetString(ColumnNames.PRODUCTIVO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.PRODUCTIVO, Value)
            End Set
        End Property

        Public Overridable Property BALDA1 As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.BALDA1)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.BALDA1, Value)
            End Set
        End Property

        Public Overridable Property CAJON1 As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.CAJON1)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.CAJON1, Value)
            End Set
        End Property

        Public Overridable Property BALDA2 As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.BALDA2)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.BALDA2, Value)
            End Set
        End Property

        Public Overridable Property CAJON2 As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.CAJON2)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.CAJON2, Value)
            End Set
        End Property

        Public Overridable Property NORMA As String
            Get
                Return MyBase.GetString(ColumnNames.NORMA)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.NORMA, Value)
            End Set
        End Property

        Public Overridable Property UD_PRESTAMO As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.UD_PRESTAMO)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.UD_PRESTAMO, Value)
            End Set
        End Property

        Public Overridable Property REF_CLI As String
            Get
                Return MyBase.GetString(ColumnNames.REF_CLI)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.REF_CLI, Value)
            End Set
        End Property

        Public Overridable Property COD_CLI As String
            Get
                Return MyBase.GetString(ColumnNames.COD_CLI)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.COD_CLI, Value)
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

        Public Overridable Property EULTPC As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.EULTPC)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.EULTPC, Value)
            End Set
        End Property

        Public Overridable Property EPREMED As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.EPREMED)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.EPREMED, Value)
            End Set
        End Property

        Public Overridable Property EDIMPRE As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.EDIMPRE)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.EDIMPRE, Value)
            End Set
        End Property

        Public Overridable Property GENERIKO As String
            Get
                Return MyBase.GetString(ColumnNames.GENERIKO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.GENERIKO, Value)
            End Set
        End Property

        Public Overridable Property REFERENCIA As String
            Get
                Return MyBase.GetString(ColumnNames.REFERENCIA)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.REFERENCIA, Value)
            End Set
        End Property

        Public Overridable Property MARCA As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.MARCA)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.MARCA, Value)
            End Set
        End Property

        Public Overridable Property TIPO As String
            Get
                Return MyBase.GetString(ColumnNames.TIPO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.TIPO, Value)
            End Set
        End Property

        Public Overridable Property POTENCIA As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.POTENCIA)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.POTENCIA, Value)
            End Set
        End Property

        Public Overridable Property CARRERA As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.CARRERA)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.CARRERA, Value)
            End Set
        End Property

        Public Overridable Property DATA As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.DATA)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.DATA, Value)
            End Set
        End Property

        Public Overridable Property CODPER As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.CODPER)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.CODPER, Value)
            End Set
        End Property

        Public Overridable Property CERT_CALIDAD As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.CERT_CALIDAD)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.CERT_CALIDAD, Value)
            End Set
        End Property


#End Region

#Region "String Properties"

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

        Public Overridable Property s_DESCRI As String
            Get
                If Me.IsColumnNull(ColumnNames.DESCRI) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.DESCRI)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.DESCRI)
                Else
                    Me.DESCRI = MyBase.SetStringAsString(ColumnNames.DESCRI, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_DESCRI2 As String
            Get
                If Me.IsColumnNull(ColumnNames.DESCRI2) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.DESCRI2)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.DESCRI2)
                Else
                    Me.DESCRI2 = MyBase.SetStringAsString(ColumnNames.DESCRI2, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CODFAM As String
            Get
                If Me.IsColumnNull(ColumnNames.CODFAM) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CODFAM)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CODFAM)
                Else
                    Me.CODFAM = MyBase.SetStringAsString(ColumnNames.CODFAM, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_SUBFAM As String
            Get
                If Me.IsColumnNull(ColumnNames.SUBFAM) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.SUBFAM)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.SUBFAM)
                Else
                    Me.SUBFAM = MyBase.SetStringAsString(ColumnNames.SUBFAM, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CRIAGRU As String
            Get
                If Me.IsColumnNull(ColumnNames.CRIAGRU) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CRIAGRU)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CRIAGRU)
                Else
                    Me.CRIAGRU = MyBase.SetStringAsString(ColumnNames.CRIAGRU, Value)
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

        Public Overridable Property s_CODALM As String
            Get
                If Me.IsColumnNull(ColumnNames.CODALM) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CODALM)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CODALM)
                Else
                    Me.CODALM = MyBase.SetStringAsString(ColumnNames.CODALM, Value)
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

        Public Overridable Property s_STKMIN As String
            Get
                If Me.IsColumnNull(ColumnNames.STKMIN) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.STKMIN)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.STKMIN)
                Else
                    Me.STKMIN = MyBase.SetDecimalAsString(ColumnNames.STKMIN, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_STKMAX As String
            Get
                If Me.IsColumnNull(ColumnNames.STKMAX) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.STKMAX)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.STKMAX)
                Else
                    Me.STKMAX = MyBase.SetDecimalAsString(ColumnNames.STKMAX, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_ULTPC As String
            Get
                If Me.IsColumnNull(ColumnNames.ULTPC) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.ULTPC)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.ULTPC)
                Else
                    Me.ULTPC = MyBase.SetDecimalAsString(ColumnNames.ULTPC, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_PREMED As String
            Get
                If Me.IsColumnNull(ColumnNames.PREMED) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.PREMED)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.PREMED)
                Else
                    Me.PREMED = MyBase.SetDecimalAsString(ColumnNames.PREMED, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FECULTC As String
            Get
                If Me.IsColumnNull(ColumnNames.FECULTC) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FECULTC)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FECULTC)
                Else
                    Me.FECULTC = MyBase.SetDateTimeAsString(ColumnNames.FECULTC, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_ULTCANT As String
            Get
                If Me.IsColumnNull(ColumnNames.ULTCANT) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.ULTCANT)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.ULTCANT)
                Else
                    Me.ULTCANT = MyBase.SetDecimalAsString(ColumnNames.ULTCANT, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_TOTPED As String
            Get
                If Me.IsColumnNull(ColumnNames.TOTPED) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.TOTPED)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.TOTPED)
                Else
                    Me.TOTPED = MyBase.SetDecimalAsString(ColumnNames.TOTPED, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_STOCK As String
            Get
                If Me.IsColumnNull(ColumnNames.STOCK) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.STOCK)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.STOCK)
                Else
                    Me.STOCK = MyBase.SetDecimalAsString(ColumnNames.STOCK, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_GESTOCK As String
            Get
                If Me.IsColumnNull(ColumnNames.GESTOCK) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.GESTOCK)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.GESTOCK)
                Else
                    Me.GESTOCK = MyBase.SetStringAsString(ColumnNames.GESTOCK, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CTA_OF As String
            Get
                If Me.IsColumnNull(ColumnNames.CTA_OF) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CTA_OF)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CTA_OF)
                Else
                    Me.CTA_OF = MyBase.SetStringAsString(ColumnNames.CTA_OF, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CTACOMP As String
            Get
                If Me.IsColumnNull(ColumnNames.CTACOMP) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CTACOMP)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CTACOMP)
                Else
                    Me.CTACOMP = MyBase.SetStringAsString(ColumnNames.CTACOMP, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_TIPIMPUT As String
            Get
                If Me.IsColumnNull(ColumnNames.TIPIMPUT) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.TIPIMPUT)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.TIPIMPUT)
                Else
                    Me.TIPIMPUT = MyBase.SetStringAsString(ColumnNames.TIPIMPUT, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CORTES As String
            Get
                If Me.IsColumnNull(ColumnNames.CORTES) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CORTES)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CORTES)
                Else
                    Me.CORTES = MyBase.SetStringAsString(ColumnNames.CORTES, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_ALEACIONES As String
            Get
                If Me.IsColumnNull(ColumnNames.ALEACIONES) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.ALEACIONES)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.ALEACIONES)
                Else
                    Me.ALEACIONES = MyBase.SetStringAsString(ColumnNames.ALEACIONES, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_OBSOLETO As String
            Get
                If Me.IsColumnNull(ColumnNames.OBSOLETO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.OBSOLETO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.OBSOLETO)
                Else
                    Me.OBSOLETO = MyBase.SetStringAsString(ColumnNames.OBSOLETO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_PRODUCTIVO As String
            Get
                If Me.IsColumnNull(ColumnNames.PRODUCTIVO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.PRODUCTIVO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.PRODUCTIVO)
                Else
                    Me.PRODUCTIVO = MyBase.SetStringAsString(ColumnNames.PRODUCTIVO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_BALDA1 As String
            Get
                If Me.IsColumnNull(ColumnNames.BALDA1) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.BALDA1)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.BALDA1)
                Else
                    Me.BALDA1 = MyBase.SetDecimalAsString(ColumnNames.BALDA1, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CAJON1 As String
            Get
                If Me.IsColumnNull(ColumnNames.CAJON1) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.CAJON1)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CAJON1)
                Else
                    Me.CAJON1 = MyBase.SetDecimalAsString(ColumnNames.CAJON1, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_BALDA2 As String
            Get
                If Me.IsColumnNull(ColumnNames.BALDA2) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.BALDA2)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.BALDA2)
                Else
                    Me.BALDA2 = MyBase.SetDecimalAsString(ColumnNames.BALDA2, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CAJON2 As String
            Get
                If Me.IsColumnNull(ColumnNames.CAJON2) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.CAJON2)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CAJON2)
                Else
                    Me.CAJON2 = MyBase.SetDecimalAsString(ColumnNames.CAJON2, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_NORMA As String
            Get
                If Me.IsColumnNull(ColumnNames.NORMA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.NORMA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NORMA)
                Else
                    Me.NORMA = MyBase.SetStringAsString(ColumnNames.NORMA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_UD_PRESTAMO As String
            Get
                If Me.IsColumnNull(ColumnNames.UD_PRESTAMO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.UD_PRESTAMO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.UD_PRESTAMO)
                Else
                    Me.UD_PRESTAMO = MyBase.SetDecimalAsString(ColumnNames.UD_PRESTAMO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_REF_CLI As String
            Get
                If Me.IsColumnNull(ColumnNames.REF_CLI) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.REF_CLI)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.REF_CLI)
                Else
                    Me.REF_CLI = MyBase.SetStringAsString(ColumnNames.REF_CLI, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_COD_CLI As String
            Get
                If Me.IsColumnNull(ColumnNames.COD_CLI) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.COD_CLI)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.COD_CLI)
                Else
                    Me.COD_CLI = MyBase.SetStringAsString(ColumnNames.COD_CLI, Value)
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

        Public Overridable Property s_EULTPC As String
            Get
                If Me.IsColumnNull(ColumnNames.EULTPC) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.EULTPC)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.EULTPC)
                Else
                    Me.EULTPC = MyBase.SetDecimalAsString(ColumnNames.EULTPC, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_EPREMED As String
            Get
                If Me.IsColumnNull(ColumnNames.EPREMED) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.EPREMED)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.EPREMED)
                Else
                    Me.EPREMED = MyBase.SetDecimalAsString(ColumnNames.EPREMED, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_EDIMPRE As String
            Get
                If Me.IsColumnNull(ColumnNames.EDIMPRE) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.EDIMPRE)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.EDIMPRE)
                Else
                    Me.EDIMPRE = MyBase.SetDecimalAsString(ColumnNames.EDIMPRE, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_GENERIKO As String
            Get
                If Me.IsColumnNull(ColumnNames.GENERIKO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.GENERIKO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.GENERIKO)
                Else
                    Me.GENERIKO = MyBase.SetStringAsString(ColumnNames.GENERIKO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_REFERENCIA As String
            Get
                If Me.IsColumnNull(ColumnNames.REFERENCIA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.REFERENCIA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.REFERENCIA)
                Else
                    Me.REFERENCIA = MyBase.SetStringAsString(ColumnNames.REFERENCIA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_MARCA As String
            Get
                If Me.IsColumnNull(ColumnNames.MARCA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.MARCA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.MARCA)
                Else
                    Me.MARCA = MyBase.SetDecimalAsString(ColumnNames.MARCA, Value)
                End If
            End Set
        End Property

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

        Public Overridable Property s_POTENCIA As String
            Get
                If Me.IsColumnNull(ColumnNames.POTENCIA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.POTENCIA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.POTENCIA)
                Else
                    Me.POTENCIA = MyBase.SetDecimalAsString(ColumnNames.POTENCIA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CARRERA As String
            Get
                If Me.IsColumnNull(ColumnNames.CARRERA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.CARRERA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CARRERA)
                Else
                    Me.CARRERA = MyBase.SetDecimalAsString(ColumnNames.CARRERA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_DATA As String
            Get
                If Me.IsColumnNull(ColumnNames.DATA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.DATA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.DATA)
                Else
                    Me.DATA = MyBase.SetDateTimeAsString(ColumnNames.DATA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CODPER As String
            Get
                If Me.IsColumnNull(ColumnNames.CODPER) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.CODPER)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CODPER)
                Else
                    Me.CODPER = MyBase.SetDecimalAsString(ColumnNames.CODPER, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CERT_CALIDAD As String
            Get
                If Me.IsColumnNull(ColumnNames.CERT_CALIDAD) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.CERT_CALIDAD)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CERT_CALIDAD)
                Else
                    Me.CERT_CALIDAD = MyBase.SetDecimalAsString(ColumnNames.CERT_CALIDAD, Value)
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


                Public ReadOnly Property CODART() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CODART, Parameters.CODART)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DESCRI() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.DESCRI, Parameters.DESCRI)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DESCRI2() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.DESCRI2, Parameters.DESCRI2)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODFAM() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CODFAM, Parameters.CODFAM)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property SUBFAM() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.SUBFAM, Parameters.SUBFAM)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CRIAGRU() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CRIAGRU, Parameters.CRIAGRU)
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

                Public ReadOnly Property CODALM() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CODALM, Parameters.CODALM)
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

                Public ReadOnly Property STKMIN() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.STKMIN, Parameters.STKMIN)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property STKMAX() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.STKMAX, Parameters.STKMAX)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ULTPC() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.ULTPC, Parameters.ULTPC)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PREMED() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.PREMED, Parameters.PREMED)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECULTC() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FECULTC, Parameters.FECULTC)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ULTCANT() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.ULTCANT, Parameters.ULTCANT)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TOTPED() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.TOTPED, Parameters.TOTPED)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property STOCK() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.STOCK, Parameters.STOCK)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property GESTOCK() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.GESTOCK, Parameters.GESTOCK)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CTA_OF() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CTA_OF, Parameters.CTA_OF)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CTACOMP() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CTACOMP, Parameters.CTACOMP)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TIPIMPUT() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.TIPIMPUT, Parameters.TIPIMPUT)
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

                Public ReadOnly Property ALEACIONES() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.ALEACIONES, Parameters.ALEACIONES)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property OBSOLETO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.OBSOLETO, Parameters.OBSOLETO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PRODUCTIVO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.PRODUCTIVO, Parameters.PRODUCTIVO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property BALDA1() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.BALDA1, Parameters.BALDA1)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CAJON1() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CAJON1, Parameters.CAJON1)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property BALDA2() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.BALDA2, Parameters.BALDA2)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CAJON2() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CAJON2, Parameters.CAJON2)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NORMA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NORMA, Parameters.NORMA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property UD_PRESTAMO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.UD_PRESTAMO, Parameters.UD_PRESTAMO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property REF_CLI() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.REF_CLI, Parameters.REF_CLI)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property COD_CLI() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.COD_CLI, Parameters.COD_CLI)
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

                Public ReadOnly Property EULTPC() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.EULTPC, Parameters.EULTPC)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property EPREMED() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.EPREMED, Parameters.EPREMED)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property EDIMPRE() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.EDIMPRE, Parameters.EDIMPRE)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property GENERIKO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.GENERIKO, Parameters.GENERIKO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property REFERENCIA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.REFERENCIA, Parameters.REFERENCIA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property MARCA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.MARCA, Parameters.MARCA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TIPO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.TIPO, Parameters.TIPO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property POTENCIA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.POTENCIA, Parameters.POTENCIA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CARRERA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CARRERA, Parameters.CARRERA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DATA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.DATA, Parameters.DATA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODPER() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CODPER, Parameters.CODPER)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CERT_CALIDAD() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CERT_CALIDAD, Parameters.CERT_CALIDAD)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property


                Private _clause As WhereClause
            End Class
#End Region

            Public ReadOnly Property CODART() As WhereParameter
                Get
                    If _CODART_W Is Nothing Then
                        _CODART_W = TearOff.CODART
                    End If
                    Return _CODART_W
                End Get
            End Property

            Public ReadOnly Property DESCRI() As WhereParameter
                Get
                    If _DESCRI_W Is Nothing Then
                        _DESCRI_W = TearOff.DESCRI
                    End If
                    Return _DESCRI_W
                End Get
            End Property

            Public ReadOnly Property DESCRI2() As WhereParameter
                Get
                    If _DESCRI2_W Is Nothing Then
                        _DESCRI2_W = TearOff.DESCRI2
                    End If
                    Return _DESCRI2_W
                End Get
            End Property

            Public ReadOnly Property CODFAM() As WhereParameter
                Get
                    If _CODFAM_W Is Nothing Then
                        _CODFAM_W = TearOff.CODFAM
                    End If
                    Return _CODFAM_W
                End Get
            End Property

            Public ReadOnly Property SUBFAM() As WhereParameter
                Get
                    If _SUBFAM_W Is Nothing Then
                        _SUBFAM_W = TearOff.SUBFAM
                    End If
                    Return _SUBFAM_W
                End Get
            End Property

            Public ReadOnly Property CRIAGRU() As WhereParameter
                Get
                    If _CRIAGRU_W Is Nothing Then
                        _CRIAGRU_W = TearOff.CRIAGRU
                    End If
                    Return _CRIAGRU_W
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

            Public ReadOnly Property CODALM() As WhereParameter
                Get
                    If _CODALM_W Is Nothing Then
                        _CODALM_W = TearOff.CODALM
                    End If
                    Return _CODALM_W
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

            Public ReadOnly Property STKMIN() As WhereParameter
                Get
                    If _STKMIN_W Is Nothing Then
                        _STKMIN_W = TearOff.STKMIN
                    End If
                    Return _STKMIN_W
                End Get
            End Property

            Public ReadOnly Property STKMAX() As WhereParameter
                Get
                    If _STKMAX_W Is Nothing Then
                        _STKMAX_W = TearOff.STKMAX
                    End If
                    Return _STKMAX_W
                End Get
            End Property

            Public ReadOnly Property ULTPC() As WhereParameter
                Get
                    If _ULTPC_W Is Nothing Then
                        _ULTPC_W = TearOff.ULTPC
                    End If
                    Return _ULTPC_W
                End Get
            End Property

            Public ReadOnly Property PREMED() As WhereParameter
                Get
                    If _PREMED_W Is Nothing Then
                        _PREMED_W = TearOff.PREMED
                    End If
                    Return _PREMED_W
                End Get
            End Property

            Public ReadOnly Property FECULTC() As WhereParameter
                Get
                    If _FECULTC_W Is Nothing Then
                        _FECULTC_W = TearOff.FECULTC
                    End If
                    Return _FECULTC_W
                End Get
            End Property

            Public ReadOnly Property ULTCANT() As WhereParameter
                Get
                    If _ULTCANT_W Is Nothing Then
                        _ULTCANT_W = TearOff.ULTCANT
                    End If
                    Return _ULTCANT_W
                End Get
            End Property

            Public ReadOnly Property TOTPED() As WhereParameter
                Get
                    If _TOTPED_W Is Nothing Then
                        _TOTPED_W = TearOff.TOTPED
                    End If
                    Return _TOTPED_W
                End Get
            End Property

            Public ReadOnly Property STOCK() As WhereParameter
                Get
                    If _STOCK_W Is Nothing Then
                        _STOCK_W = TearOff.STOCK
                    End If
                    Return _STOCK_W
                End Get
            End Property

            Public ReadOnly Property GESTOCK() As WhereParameter
                Get
                    If _GESTOCK_W Is Nothing Then
                        _GESTOCK_W = TearOff.GESTOCK
                    End If
                    Return _GESTOCK_W
                End Get
            End Property

            Public ReadOnly Property CTA_OF() As WhereParameter
                Get
                    If _CTA_OF_W Is Nothing Then
                        _CTA_OF_W = TearOff.CTA_OF
                    End If
                    Return _CTA_OF_W
                End Get
            End Property

            Public ReadOnly Property CTACOMP() As WhereParameter
                Get
                    If _CTACOMP_W Is Nothing Then
                        _CTACOMP_W = TearOff.CTACOMP
                    End If
                    Return _CTACOMP_W
                End Get
            End Property

            Public ReadOnly Property TIPIMPUT() As WhereParameter
                Get
                    If _TIPIMPUT_W Is Nothing Then
                        _TIPIMPUT_W = TearOff.TIPIMPUT
                    End If
                    Return _TIPIMPUT_W
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

            Public ReadOnly Property ALEACIONES() As WhereParameter
                Get
                    If _ALEACIONES_W Is Nothing Then
                        _ALEACIONES_W = TearOff.ALEACIONES
                    End If
                    Return _ALEACIONES_W
                End Get
            End Property

            Public ReadOnly Property OBSOLETO() As WhereParameter
                Get
                    If _OBSOLETO_W Is Nothing Then
                        _OBSOLETO_W = TearOff.OBSOLETO
                    End If
                    Return _OBSOLETO_W
                End Get
            End Property

            Public ReadOnly Property PRODUCTIVO() As WhereParameter
                Get
                    If _PRODUCTIVO_W Is Nothing Then
                        _PRODUCTIVO_W = TearOff.PRODUCTIVO
                    End If
                    Return _PRODUCTIVO_W
                End Get
            End Property

            Public ReadOnly Property BALDA1() As WhereParameter
                Get
                    If _BALDA1_W Is Nothing Then
                        _BALDA1_W = TearOff.BALDA1
                    End If
                    Return _BALDA1_W
                End Get
            End Property

            Public ReadOnly Property CAJON1() As WhereParameter
                Get
                    If _CAJON1_W Is Nothing Then
                        _CAJON1_W = TearOff.CAJON1
                    End If
                    Return _CAJON1_W
                End Get
            End Property

            Public ReadOnly Property BALDA2() As WhereParameter
                Get
                    If _BALDA2_W Is Nothing Then
                        _BALDA2_W = TearOff.BALDA2
                    End If
                    Return _BALDA2_W
                End Get
            End Property

            Public ReadOnly Property CAJON2() As WhereParameter
                Get
                    If _CAJON2_W Is Nothing Then
                        _CAJON2_W = TearOff.CAJON2
                    End If
                    Return _CAJON2_W
                End Get
            End Property

            Public ReadOnly Property NORMA() As WhereParameter
                Get
                    If _NORMA_W Is Nothing Then
                        _NORMA_W = TearOff.NORMA
                    End If
                    Return _NORMA_W
                End Get
            End Property

            Public ReadOnly Property UD_PRESTAMO() As WhereParameter
                Get
                    If _UD_PRESTAMO_W Is Nothing Then
                        _UD_PRESTAMO_W = TearOff.UD_PRESTAMO
                    End If
                    Return _UD_PRESTAMO_W
                End Get
            End Property

            Public ReadOnly Property REF_CLI() As WhereParameter
                Get
                    If _REF_CLI_W Is Nothing Then
                        _REF_CLI_W = TearOff.REF_CLI
                    End If
                    Return _REF_CLI_W
                End Get
            End Property

            Public ReadOnly Property COD_CLI() As WhereParameter
                Get
                    If _COD_CLI_W Is Nothing Then
                        _COD_CLI_W = TearOff.COD_CLI
                    End If
                    Return _COD_CLI_W
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

            Public ReadOnly Property EULTPC() As WhereParameter
                Get
                    If _EULTPC_W Is Nothing Then
                        _EULTPC_W = TearOff.EULTPC
                    End If
                    Return _EULTPC_W
                End Get
            End Property

            Public ReadOnly Property EPREMED() As WhereParameter
                Get
                    If _EPREMED_W Is Nothing Then
                        _EPREMED_W = TearOff.EPREMED
                    End If
                    Return _EPREMED_W
                End Get
            End Property

            Public ReadOnly Property EDIMPRE() As WhereParameter
                Get
                    If _EDIMPRE_W Is Nothing Then
                        _EDIMPRE_W = TearOff.EDIMPRE
                    End If
                    Return _EDIMPRE_W
                End Get
            End Property

            Public ReadOnly Property GENERIKO() As WhereParameter
                Get
                    If _GENERIKO_W Is Nothing Then
                        _GENERIKO_W = TearOff.GENERIKO
                    End If
                    Return _GENERIKO_W
                End Get
            End Property

            Public ReadOnly Property REFERENCIA() As WhereParameter
                Get
                    If _REFERENCIA_W Is Nothing Then
                        _REFERENCIA_W = TearOff.REFERENCIA
                    End If
                    Return _REFERENCIA_W
                End Get
            End Property

            Public ReadOnly Property MARCA() As WhereParameter
                Get
                    If _MARCA_W Is Nothing Then
                        _MARCA_W = TearOff.MARCA
                    End If
                    Return _MARCA_W
                End Get
            End Property

            Public ReadOnly Property TIPO() As WhereParameter
                Get
                    If _TIPO_W Is Nothing Then
                        _TIPO_W = TearOff.TIPO
                    End If
                    Return _TIPO_W
                End Get
            End Property

            Public ReadOnly Property POTENCIA() As WhereParameter
                Get
                    If _POTENCIA_W Is Nothing Then
                        _POTENCIA_W = TearOff.POTENCIA
                    End If
                    Return _POTENCIA_W
                End Get
            End Property

            Public ReadOnly Property CARRERA() As WhereParameter
                Get
                    If _CARRERA_W Is Nothing Then
                        _CARRERA_W = TearOff.CARRERA
                    End If
                    Return _CARRERA_W
                End Get
            End Property

            Public ReadOnly Property DATA() As WhereParameter
                Get
                    If _DATA_W Is Nothing Then
                        _DATA_W = TearOff.DATA
                    End If
                    Return _DATA_W
                End Get
            End Property

            Public ReadOnly Property CODPER() As WhereParameter
                Get
                    If _CODPER_W Is Nothing Then
                        _CODPER_W = TearOff.CODPER
                    End If
                    Return _CODPER_W
                End Get
            End Property

            Public ReadOnly Property CERT_CALIDAD() As WhereParameter
                Get
                    If _CERT_CALIDAD_W Is Nothing Then
                        _CERT_CALIDAD_W = TearOff.CERT_CALIDAD
                    End If
                    Return _CERT_CALIDAD_W
                End Get
            End Property

            Private _CODART_W As WhereParameter = Nothing
            Private _DESCRI_W As WhereParameter = Nothing
            Private _DESCRI2_W As WhereParameter = Nothing
            Private _CODFAM_W As WhereParameter = Nothing
            Private _SUBFAM_W As WhereParameter = Nothing
            Private _CRIAGRU_W As WhereParameter = Nothing
            Private _CODPRO_W As WhereParameter = Nothing
            Private _CODALM_W As WhereParameter = Nothing
            Private _TIPIVA_W As WhereParameter = Nothing
            Private _STKMIN_W As WhereParameter = Nothing
            Private _STKMAX_W As WhereParameter = Nothing
            Private _ULTPC_W As WhereParameter = Nothing
            Private _PREMED_W As WhereParameter = Nothing
            Private _FECULTC_W As WhereParameter = Nothing
            Private _ULTCANT_W As WhereParameter = Nothing
            Private _TOTPED_W As WhereParameter = Nothing
            Private _STOCK_W As WhereParameter = Nothing
            Private _GESTOCK_W As WhereParameter = Nothing
            Private _CTA_OF_W As WhereParameter = Nothing
            Private _CTACOMP_W As WhereParameter = Nothing
            Private _TIPIMPUT_W As WhereParameter = Nothing
            Private _CORTES_W As WhereParameter = Nothing
            Private _ALEACIONES_W As WhereParameter = Nothing
            Private _OBSOLETO_W As WhereParameter = Nothing
            Private _PRODUCTIVO_W As WhereParameter = Nothing
            Private _BALDA1_W As WhereParameter = Nothing
            Private _CAJON1_W As WhereParameter = Nothing
            Private _BALDA2_W As WhereParameter = Nothing
            Private _CAJON2_W As WhereParameter = Nothing
            Private _NORMA_W As WhereParameter = Nothing
            Private _UD_PRESTAMO_W As WhereParameter = Nothing
            Private _REF_CLI_W As WhereParameter = Nothing
            Private _COD_CLI_W As WhereParameter = Nothing
            Private _NUMORD_W As WhereParameter = Nothing
            Private _NUMOPE_W As WhereParameter = Nothing
            Private _EULTPC_W As WhereParameter = Nothing
            Private _EPREMED_W As WhereParameter = Nothing
            Private _EDIMPRE_W As WhereParameter = Nothing
            Private _GENERIKO_W As WhereParameter = Nothing
            Private _REFERENCIA_W As WhereParameter = Nothing
            Private _MARCA_W As WhereParameter = Nothing
            Private _TIPO_W As WhereParameter = Nothing
            Private _POTENCIA_W As WhereParameter = Nothing
            Private _CARRERA_W As WhereParameter = Nothing
            Private _DATA_W As WhereParameter = Nothing
            Private _CODPER_W As WhereParameter = Nothing
            Private _CERT_CALIDAD_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _CODART_W = Nothing
                _DESCRI_W = Nothing
                _DESCRI2_W = Nothing
                _CODFAM_W = Nothing
                _SUBFAM_W = Nothing
                _CRIAGRU_W = Nothing
                _CODPRO_W = Nothing
                _CODALM_W = Nothing
                _TIPIVA_W = Nothing
                _STKMIN_W = Nothing
                _STKMAX_W = Nothing
                _ULTPC_W = Nothing
                _PREMED_W = Nothing
                _FECULTC_W = Nothing
                _ULTCANT_W = Nothing
                _TOTPED_W = Nothing
                _STOCK_W = Nothing
                _GESTOCK_W = Nothing
                _CTA_OF_W = Nothing
                _CTACOMP_W = Nothing
                _TIPIMPUT_W = Nothing
                _CORTES_W = Nothing
                _ALEACIONES_W = Nothing
                _OBSOLETO_W = Nothing
                _PRODUCTIVO_W = Nothing
                _BALDA1_W = Nothing
                _CAJON1_W = Nothing
                _BALDA2_W = Nothing
                _CAJON2_W = Nothing
                _NORMA_W = Nothing
                _UD_PRESTAMO_W = Nothing
                _REF_CLI_W = Nothing
                _COD_CLI_W = Nothing
                _NUMORD_W = Nothing
                _NUMOPE_W = Nothing
                _EULTPC_W = Nothing
                _EPREMED_W = Nothing
                _EDIMPRE_W = Nothing
                _GENERIKO_W = Nothing
                _REFERENCIA_W = Nothing
                _MARCA_W = Nothing
                _TIPO_W = Nothing
                _POTENCIA_W = Nothing
                _CARRERA_W = Nothing
                _DATA_W = Nothing
                _CODPER_W = Nothing
                _CERT_CALIDAD_W = Nothing
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


                Public ReadOnly Property CODART() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CODART, Parameters.CODART)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DESCRI() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DESCRI, Parameters.DESCRI)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DESCRI2() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DESCRI2, Parameters.DESCRI2)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODFAM() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CODFAM, Parameters.CODFAM)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property SUBFAM() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.SUBFAM, Parameters.SUBFAM)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CRIAGRU() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CRIAGRU, Parameters.CRIAGRU)
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

                Public ReadOnly Property CODALM() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CODALM, Parameters.CODALM)
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

                Public ReadOnly Property STKMIN() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.STKMIN, Parameters.STKMIN)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property STKMAX() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.STKMAX, Parameters.STKMAX)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ULTPC() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ULTPC, Parameters.ULTPC)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PREMED() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.PREMED, Parameters.PREMED)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECULTC() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECULTC, Parameters.FECULTC)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ULTCANT() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ULTCANT, Parameters.ULTCANT)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TOTPED() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TOTPED, Parameters.TOTPED)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property STOCK() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.STOCK, Parameters.STOCK)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property GESTOCK() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.GESTOCK, Parameters.GESTOCK)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CTA_OF() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CTA_OF, Parameters.CTA_OF)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CTACOMP() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CTACOMP, Parameters.CTACOMP)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TIPIMPUT() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TIPIMPUT, Parameters.TIPIMPUT)
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

                Public ReadOnly Property ALEACIONES() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ALEACIONES, Parameters.ALEACIONES)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property OBSOLETO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.OBSOLETO, Parameters.OBSOLETO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PRODUCTIVO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.PRODUCTIVO, Parameters.PRODUCTIVO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property BALDA1() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.BALDA1, Parameters.BALDA1)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CAJON1() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CAJON1, Parameters.CAJON1)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property BALDA2() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.BALDA2, Parameters.BALDA2)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CAJON2() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CAJON2, Parameters.CAJON2)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NORMA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NORMA, Parameters.NORMA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property UD_PRESTAMO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.UD_PRESTAMO, Parameters.UD_PRESTAMO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property REF_CLI() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.REF_CLI, Parameters.REF_CLI)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property COD_CLI() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.COD_CLI, Parameters.COD_CLI)
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

                Public ReadOnly Property EULTPC() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.EULTPC, Parameters.EULTPC)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property EPREMED() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.EPREMED, Parameters.EPREMED)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property EDIMPRE() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.EDIMPRE, Parameters.EDIMPRE)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property GENERIKO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.GENERIKO, Parameters.GENERIKO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property REFERENCIA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.REFERENCIA, Parameters.REFERENCIA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property MARCA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.MARCA, Parameters.MARCA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TIPO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TIPO, Parameters.TIPO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property POTENCIA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.POTENCIA, Parameters.POTENCIA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CARRERA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CARRERA, Parameters.CARRERA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DATA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DATA, Parameters.DATA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODPER() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CODPER, Parameters.CODPER)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CERT_CALIDAD() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CERT_CALIDAD, Parameters.CERT_CALIDAD)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property


                Private _clause As AggregateClause
            End Class
#End Region

            Public ReadOnly Property CODART() As AggregateParameter
                Get
                    If _CODART_W Is Nothing Then
                        _CODART_W = TearOff.CODART
                    End If
                    Return _CODART_W
                End Get
            End Property

            Public ReadOnly Property DESCRI() As AggregateParameter
                Get
                    If _DESCRI_W Is Nothing Then
                        _DESCRI_W = TearOff.DESCRI
                    End If
                    Return _DESCRI_W
                End Get
            End Property

            Public ReadOnly Property DESCRI2() As AggregateParameter
                Get
                    If _DESCRI2_W Is Nothing Then
                        _DESCRI2_W = TearOff.DESCRI2
                    End If
                    Return _DESCRI2_W
                End Get
            End Property

            Public ReadOnly Property CODFAM() As AggregateParameter
                Get
                    If _CODFAM_W Is Nothing Then
                        _CODFAM_W = TearOff.CODFAM
                    End If
                    Return _CODFAM_W
                End Get
            End Property

            Public ReadOnly Property SUBFAM() As AggregateParameter
                Get
                    If _SUBFAM_W Is Nothing Then
                        _SUBFAM_W = TearOff.SUBFAM
                    End If
                    Return _SUBFAM_W
                End Get
            End Property

            Public ReadOnly Property CRIAGRU() As AggregateParameter
                Get
                    If _CRIAGRU_W Is Nothing Then
                        _CRIAGRU_W = TearOff.CRIAGRU
                    End If
                    Return _CRIAGRU_W
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

            Public ReadOnly Property CODALM() As AggregateParameter
                Get
                    If _CODALM_W Is Nothing Then
                        _CODALM_W = TearOff.CODALM
                    End If
                    Return _CODALM_W
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

            Public ReadOnly Property STKMIN() As AggregateParameter
                Get
                    If _STKMIN_W Is Nothing Then
                        _STKMIN_W = TearOff.STKMIN
                    End If
                    Return _STKMIN_W
                End Get
            End Property

            Public ReadOnly Property STKMAX() As AggregateParameter
                Get
                    If _STKMAX_W Is Nothing Then
                        _STKMAX_W = TearOff.STKMAX
                    End If
                    Return _STKMAX_W
                End Get
            End Property

            Public ReadOnly Property ULTPC() As AggregateParameter
                Get
                    If _ULTPC_W Is Nothing Then
                        _ULTPC_W = TearOff.ULTPC
                    End If
                    Return _ULTPC_W
                End Get
            End Property

            Public ReadOnly Property PREMED() As AggregateParameter
                Get
                    If _PREMED_W Is Nothing Then
                        _PREMED_W = TearOff.PREMED
                    End If
                    Return _PREMED_W
                End Get
            End Property

            Public ReadOnly Property FECULTC() As AggregateParameter
                Get
                    If _FECULTC_W Is Nothing Then
                        _FECULTC_W = TearOff.FECULTC
                    End If
                    Return _FECULTC_W
                End Get
            End Property

            Public ReadOnly Property ULTCANT() As AggregateParameter
                Get
                    If _ULTCANT_W Is Nothing Then
                        _ULTCANT_W = TearOff.ULTCANT
                    End If
                    Return _ULTCANT_W
                End Get
            End Property

            Public ReadOnly Property TOTPED() As AggregateParameter
                Get
                    If _TOTPED_W Is Nothing Then
                        _TOTPED_W = TearOff.TOTPED
                    End If
                    Return _TOTPED_W
                End Get
            End Property

            Public ReadOnly Property STOCK() As AggregateParameter
                Get
                    If _STOCK_W Is Nothing Then
                        _STOCK_W = TearOff.STOCK
                    End If
                    Return _STOCK_W
                End Get
            End Property

            Public ReadOnly Property GESTOCK() As AggregateParameter
                Get
                    If _GESTOCK_W Is Nothing Then
                        _GESTOCK_W = TearOff.GESTOCK
                    End If
                    Return _GESTOCK_W
                End Get
            End Property

            Public ReadOnly Property CTA_OF() As AggregateParameter
                Get
                    If _CTA_OF_W Is Nothing Then
                        _CTA_OF_W = TearOff.CTA_OF
                    End If
                    Return _CTA_OF_W
                End Get
            End Property

            Public ReadOnly Property CTACOMP() As AggregateParameter
                Get
                    If _CTACOMP_W Is Nothing Then
                        _CTACOMP_W = TearOff.CTACOMP
                    End If
                    Return _CTACOMP_W
                End Get
            End Property

            Public ReadOnly Property TIPIMPUT() As AggregateParameter
                Get
                    If _TIPIMPUT_W Is Nothing Then
                        _TIPIMPUT_W = TearOff.TIPIMPUT
                    End If
                    Return _TIPIMPUT_W
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

            Public ReadOnly Property ALEACIONES() As AggregateParameter
                Get
                    If _ALEACIONES_W Is Nothing Then
                        _ALEACIONES_W = TearOff.ALEACIONES
                    End If
                    Return _ALEACIONES_W
                End Get
            End Property

            Public ReadOnly Property OBSOLETO() As AggregateParameter
                Get
                    If _OBSOLETO_W Is Nothing Then
                        _OBSOLETO_W = TearOff.OBSOLETO
                    End If
                    Return _OBSOLETO_W
                End Get
            End Property

            Public ReadOnly Property PRODUCTIVO() As AggregateParameter
                Get
                    If _PRODUCTIVO_W Is Nothing Then
                        _PRODUCTIVO_W = TearOff.PRODUCTIVO
                    End If
                    Return _PRODUCTIVO_W
                End Get
            End Property

            Public ReadOnly Property BALDA1() As AggregateParameter
                Get
                    If _BALDA1_W Is Nothing Then
                        _BALDA1_W = TearOff.BALDA1
                    End If
                    Return _BALDA1_W
                End Get
            End Property

            Public ReadOnly Property CAJON1() As AggregateParameter
                Get
                    If _CAJON1_W Is Nothing Then
                        _CAJON1_W = TearOff.CAJON1
                    End If
                    Return _CAJON1_W
                End Get
            End Property

            Public ReadOnly Property BALDA2() As AggregateParameter
                Get
                    If _BALDA2_W Is Nothing Then
                        _BALDA2_W = TearOff.BALDA2
                    End If
                    Return _BALDA2_W
                End Get
            End Property

            Public ReadOnly Property CAJON2() As AggregateParameter
                Get
                    If _CAJON2_W Is Nothing Then
                        _CAJON2_W = TearOff.CAJON2
                    End If
                    Return _CAJON2_W
                End Get
            End Property

            Public ReadOnly Property NORMA() As AggregateParameter
                Get
                    If _NORMA_W Is Nothing Then
                        _NORMA_W = TearOff.NORMA
                    End If
                    Return _NORMA_W
                End Get
            End Property

            Public ReadOnly Property UD_PRESTAMO() As AggregateParameter
                Get
                    If _UD_PRESTAMO_W Is Nothing Then
                        _UD_PRESTAMO_W = TearOff.UD_PRESTAMO
                    End If
                    Return _UD_PRESTAMO_W
                End Get
            End Property

            Public ReadOnly Property REF_CLI() As AggregateParameter
                Get
                    If _REF_CLI_W Is Nothing Then
                        _REF_CLI_W = TearOff.REF_CLI
                    End If
                    Return _REF_CLI_W
                End Get
            End Property

            Public ReadOnly Property COD_CLI() As AggregateParameter
                Get
                    If _COD_CLI_W Is Nothing Then
                        _COD_CLI_W = TearOff.COD_CLI
                    End If
                    Return _COD_CLI_W
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

            Public ReadOnly Property EULTPC() As AggregateParameter
                Get
                    If _EULTPC_W Is Nothing Then
                        _EULTPC_W = TearOff.EULTPC
                    End If
                    Return _EULTPC_W
                End Get
            End Property

            Public ReadOnly Property EPREMED() As AggregateParameter
                Get
                    If _EPREMED_W Is Nothing Then
                        _EPREMED_W = TearOff.EPREMED
                    End If
                    Return _EPREMED_W
                End Get
            End Property

            Public ReadOnly Property EDIMPRE() As AggregateParameter
                Get
                    If _EDIMPRE_W Is Nothing Then
                        _EDIMPRE_W = TearOff.EDIMPRE
                    End If
                    Return _EDIMPRE_W
                End Get
            End Property

            Public ReadOnly Property GENERIKO() As AggregateParameter
                Get
                    If _GENERIKO_W Is Nothing Then
                        _GENERIKO_W = TearOff.GENERIKO
                    End If
                    Return _GENERIKO_W
                End Get
            End Property

            Public ReadOnly Property REFERENCIA() As AggregateParameter
                Get
                    If _REFERENCIA_W Is Nothing Then
                        _REFERENCIA_W = TearOff.REFERENCIA
                    End If
                    Return _REFERENCIA_W
                End Get
            End Property

            Public ReadOnly Property MARCA() As AggregateParameter
                Get
                    If _MARCA_W Is Nothing Then
                        _MARCA_W = TearOff.MARCA
                    End If
                    Return _MARCA_W
                End Get
            End Property

            Public ReadOnly Property TIPO() As AggregateParameter
                Get
                    If _TIPO_W Is Nothing Then
                        _TIPO_W = TearOff.TIPO
                    End If
                    Return _TIPO_W
                End Get
            End Property

            Public ReadOnly Property POTENCIA() As AggregateParameter
                Get
                    If _POTENCIA_W Is Nothing Then
                        _POTENCIA_W = TearOff.POTENCIA
                    End If
                    Return _POTENCIA_W
                End Get
            End Property

            Public ReadOnly Property CARRERA() As AggregateParameter
                Get
                    If _CARRERA_W Is Nothing Then
                        _CARRERA_W = TearOff.CARRERA
                    End If
                    Return _CARRERA_W
                End Get
            End Property

            Public ReadOnly Property DATA() As AggregateParameter
                Get
                    If _DATA_W Is Nothing Then
                        _DATA_W = TearOff.DATA
                    End If
                    Return _DATA_W
                End Get
            End Property

            Public ReadOnly Property CODPER() As AggregateParameter
                Get
                    If _CODPER_W Is Nothing Then
                        _CODPER_W = TearOff.CODPER
                    End If
                    Return _CODPER_W
                End Get
            End Property

            Public ReadOnly Property CERT_CALIDAD() As AggregateParameter
                Get
                    If _CERT_CALIDAD_W Is Nothing Then
                        _CERT_CALIDAD_W = TearOff.CERT_CALIDAD
                    End If
                    Return _CERT_CALIDAD_W
                End Get
            End Property

            Private _CODART_W As AggregateParameter = Nothing
            Private _DESCRI_W As AggregateParameter = Nothing
            Private _DESCRI2_W As AggregateParameter = Nothing
            Private _CODFAM_W As AggregateParameter = Nothing
            Private _SUBFAM_W As AggregateParameter = Nothing
            Private _CRIAGRU_W As AggregateParameter = Nothing
            Private _CODPRO_W As AggregateParameter = Nothing
            Private _CODALM_W As AggregateParameter = Nothing
            Private _TIPIVA_W As AggregateParameter = Nothing
            Private _STKMIN_W As AggregateParameter = Nothing
            Private _STKMAX_W As AggregateParameter = Nothing
            Private _ULTPC_W As AggregateParameter = Nothing
            Private _PREMED_W As AggregateParameter = Nothing
            Private _FECULTC_W As AggregateParameter = Nothing
            Private _ULTCANT_W As AggregateParameter = Nothing
            Private _TOTPED_W As AggregateParameter = Nothing
            Private _STOCK_W As AggregateParameter = Nothing
            Private _GESTOCK_W As AggregateParameter = Nothing
            Private _CTA_OF_W As AggregateParameter = Nothing
            Private _CTACOMP_W As AggregateParameter = Nothing
            Private _TIPIMPUT_W As AggregateParameter = Nothing
            Private _CORTES_W As AggregateParameter = Nothing
            Private _ALEACIONES_W As AggregateParameter = Nothing
            Private _OBSOLETO_W As AggregateParameter = Nothing
            Private _PRODUCTIVO_W As AggregateParameter = Nothing
            Private _BALDA1_W As AggregateParameter = Nothing
            Private _CAJON1_W As AggregateParameter = Nothing
            Private _BALDA2_W As AggregateParameter = Nothing
            Private _CAJON2_W As AggregateParameter = Nothing
            Private _NORMA_W As AggregateParameter = Nothing
            Private _UD_PRESTAMO_W As AggregateParameter = Nothing
            Private _REF_CLI_W As AggregateParameter = Nothing
            Private _COD_CLI_W As AggregateParameter = Nothing
            Private _NUMORD_W As AggregateParameter = Nothing
            Private _NUMOPE_W As AggregateParameter = Nothing
            Private _EULTPC_W As AggregateParameter = Nothing
            Private _EPREMED_W As AggregateParameter = Nothing
            Private _EDIMPRE_W As AggregateParameter = Nothing
            Private _GENERIKO_W As AggregateParameter = Nothing
            Private _REFERENCIA_W As AggregateParameter = Nothing
            Private _MARCA_W As AggregateParameter = Nothing
            Private _TIPO_W As AggregateParameter = Nothing
            Private _POTENCIA_W As AggregateParameter = Nothing
            Private _CARRERA_W As AggregateParameter = Nothing
            Private _DATA_W As AggregateParameter = Nothing
            Private _CODPER_W As AggregateParameter = Nothing
            Private _CERT_CALIDAD_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _CODART_W = Nothing
                _DESCRI_W = Nothing
                _DESCRI2_W = Nothing
                _CODFAM_W = Nothing
                _SUBFAM_W = Nothing
                _CRIAGRU_W = Nothing
                _CODPRO_W = Nothing
                _CODALM_W = Nothing
                _TIPIVA_W = Nothing
                _STKMIN_W = Nothing
                _STKMAX_W = Nothing
                _ULTPC_W = Nothing
                _PREMED_W = Nothing
                _FECULTC_W = Nothing
                _ULTCANT_W = Nothing
                _TOTPED_W = Nothing
                _STOCK_W = Nothing
                _GESTOCK_W = Nothing
                _CTA_OF_W = Nothing
                _CTACOMP_W = Nothing
                _TIPIMPUT_W = Nothing
                _CORTES_W = Nothing
                _ALEACIONES_W = Nothing
                _OBSOLETO_W = Nothing
                _PRODUCTIVO_W = Nothing
                _BALDA1_W = Nothing
                _CAJON1_W = Nothing
                _BALDA2_W = Nothing
                _CAJON2_W = Nothing
                _NORMA_W = Nothing
                _UD_PRESTAMO_W = Nothing
                _REF_CLI_W = Nothing
                _COD_CLI_W = Nothing
                _NUMORD_W = Nothing
                _NUMOPE_W = Nothing
                _EULTPC_W = Nothing
                _EPREMED_W = Nothing
                _EDIMPRE_W = Nothing
                _GENERIKO_W = Nothing
                _REFERENCIA_W = Nothing
                _MARCA_W = Nothing
                _TIPO_W = Nothing
                _POTENCIA_W = Nothing
                _CARRERA_W = Nothing
                _DATA_W = Nothing
                _CODPER_W = Nothing
                _CERT_CALIDAD_W = Nothing
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
            cmd.CommandText = Me.SchemaStoredProcedure + "PI_GCARTICU"

            CreateParameters(cmd)


            Return cmd

        End Function

        Protected Overrides Function GetUpdateCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PU_GCARTICU"

            CreateParameters(cmd)

            Return cmd

        End Function

        Protected Overrides Function GetDeleteCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PD_GCARTICU"

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.CODART)
            p.SourceColumn = ColumnNames.CODART
            p.SourceVersion = DataRowVersion.Current


            Return cmd

        End Function

        Private Sub CreateParameters(ByVal cmd As OracleCommand)

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.CODART)
            p.SourceColumn = ColumnNames.CODART
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.DESCRI)
            p.SourceColumn = ColumnNames.DESCRI
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.DESCRI2)
            p.SourceColumn = ColumnNames.DESCRI2
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CODFAM)
            p.SourceColumn = ColumnNames.CODFAM
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.SUBFAM)
            p.SourceColumn = ColumnNames.SUBFAM
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CRIAGRU)
            p.SourceColumn = ColumnNames.CRIAGRU
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CODPRO)
            p.SourceColumn = ColumnNames.CODPRO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CODALM)
            p.SourceColumn = ColumnNames.CODALM
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.TIPIVA)
            p.SourceColumn = ColumnNames.TIPIVA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.STKMIN)
            p.SourceColumn = ColumnNames.STKMIN
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.STKMAX)
            p.SourceColumn = ColumnNames.STKMAX
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.ULTPC)
            p.SourceColumn = ColumnNames.ULTPC
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.PREMED)
            p.SourceColumn = ColumnNames.PREMED
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.FECULTC)
            p.SourceColumn = ColumnNames.FECULTC
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.ULTCANT)
            p.SourceColumn = ColumnNames.ULTCANT
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.TOTPED)
            p.SourceColumn = ColumnNames.TOTPED
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.STOCK)
            p.SourceColumn = ColumnNames.STOCK
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.GESTOCK)
            p.SourceColumn = ColumnNames.GESTOCK
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CTA_OF)
            p.SourceColumn = ColumnNames.CTA_OF
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CTACOMP)
            p.SourceColumn = ColumnNames.CTACOMP
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.TIPIMPUT)
            p.SourceColumn = ColumnNames.TIPIMPUT
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CORTES)
            p.SourceColumn = ColumnNames.CORTES
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.ALEACIONES)
            p.SourceColumn = ColumnNames.ALEACIONES
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.OBSOLETO)
            p.SourceColumn = ColumnNames.OBSOLETO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.PRODUCTIVO)
            p.SourceColumn = ColumnNames.PRODUCTIVO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.BALDA1)
            p.SourceColumn = ColumnNames.BALDA1
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CAJON1)
            p.SourceColumn = ColumnNames.CAJON1
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.BALDA2)
            p.SourceColumn = ColumnNames.BALDA2
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CAJON2)
            p.SourceColumn = ColumnNames.CAJON2
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NORMA)
            p.SourceColumn = ColumnNames.NORMA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.UD_PRESTAMO)
            p.SourceColumn = ColumnNames.UD_PRESTAMO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.REF_CLI)
            p.SourceColumn = ColumnNames.REF_CLI
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.COD_CLI)
            p.SourceColumn = ColumnNames.COD_CLI
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMORD)
            p.SourceColumn = ColumnNames.NUMORD
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMOPE)
            p.SourceColumn = ColumnNames.NUMOPE
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.EULTPC)
            p.SourceColumn = ColumnNames.EULTPC
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.EPREMED)
            p.SourceColumn = ColumnNames.EPREMED
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.EDIMPRE)
            p.SourceColumn = ColumnNames.EDIMPRE
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.GENERIKO)
            p.SourceColumn = ColumnNames.GENERIKO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.REFERENCIA)
            p.SourceColumn = ColumnNames.REFERENCIA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.MARCA)
            p.SourceColumn = ColumnNames.MARCA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.TIPO)
            p.SourceColumn = ColumnNames.TIPO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.POTENCIA)
            p.SourceColumn = ColumnNames.POTENCIA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CARRERA)
            p.SourceColumn = ColumnNames.CARRERA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.DATA)
            p.SourceColumn = ColumnNames.DATA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CODPER)
            p.SourceColumn = ColumnNames.CODPER
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CERT_CALIDAD)
            p.SourceColumn = ColumnNames.CERT_CALIDAD
            p.SourceVersion = DataRowVersion.Current


        End Sub

    End Class

End Namespace

