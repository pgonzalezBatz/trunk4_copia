
'===============================================================================
'BATZ, Koop. - 27/01/2012 8:00:40
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

    Public MustInherit Class _W_GCPROVEE
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "INCIDENCIAS."
            Me.QuerySource = "W_GCPROVEE"
            Me.MappingName = "W_GCPROVEE"
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

            Public Shared ReadOnly Property CODPRO As OracleParameter
                Get
                    Return New OracleParameter("CODPRO", OracleDbType.Char, 12)
                End Get
            End Property

            Public Shared ReadOnly Property NOMPROV As OracleParameter
                Get
                    Return New OracleParameter("NOMPROV", OracleDbType.NVarchar2, 35)
                End Get
            End Property

            Public Shared ReadOnly Property RAZON As OracleParameter
                Get
                    Return New OracleParameter("RAZON", OracleDbType.Char, 35)
                End Get
            End Property

            Public Shared ReadOnly Property CIF As OracleParameter
                Get
                    Return New OracleParameter("CIF", OracleDbType.Varchar2, 15)
                End Get
            End Property

            Public Shared ReadOnly Property DOMICI As OracleParameter
                Get
                    Return New OracleParameter("DOMICI", OracleDbType.Char, 35)
                End Get
            End Property

            Public Shared ReadOnly Property DISTRI As OracleParameter
                Get
                    Return New OracleParameter("DISTRI", OracleDbType.Char, 10)
                End Get
            End Property

            Public Shared ReadOnly Property LOCALI As OracleParameter
                Get
                    Return New OracleParameter("LOCALI", OracleDbType.Char, 30)
                End Get
            End Property

            Public Shared ReadOnly Property PROVIN As OracleParameter
                Get
                    Return New OracleParameter("PROVIN", OracleDbType.Char, 30)
                End Get
            End Property

            Public Shared ReadOnly Property CODPAI As OracleParameter
                Get
                    Return New OracleParameter("CODPAI", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property CODMON As OracleParameter
                Get
                    Return New OracleParameter("CODMON", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property PREPME As OracleParameter
                Get
                    Return New OracleParameter("PREPME", OracleDbType.Char, 1)
                End Get
            End Property

            Public Shared ReadOnly Property HABPOT As OracleParameter
                Get
                    Return New OracleParameter("HABPOT", OracleDbType.Char, 1)
                End Get
            End Property

            Public Shared ReadOnly Property TIPIVA As OracleParameter
                Get
                    Return New OracleParameter("TIPIVA", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property PORSIST As OracleParameter
                Get
                    Return New OracleParameter("PORSIST", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property PORTROQ As OracleParameter
                Get
                    Return New OracleParameter("PORTROQ", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property FORPAG As OracleParameter
                Get
                    Return New OracleParameter("FORPAG", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property NUMCAR As OracleParameter
                Get
                    Return New OracleParameter("NUMCAR", OracleDbType.Char, 30)
                End Get
            End Property

            Public Shared ReadOnly Property HOMOLOGADO As OracleParameter
                Get
                    Return New OracleParameter("HOMOLOGADO", OracleDbType.Char, 1)
                End Get
            End Property

            Public Shared ReadOnly Property TELEFO As OracleParameter
                Get
                    Return New OracleParameter("TELEFO", OracleDbType.Char, 20)
                End Get
            End Property

            Public Shared ReadOnly Property NUMFAX As OracleParameter
                Get
                    Return New OracleParameter("NUMFAX", OracleDbType.Char, 20)
                End Get
            End Property

            Public Shared ReadOnly Property CONTAC As OracleParameter
                Get
                    Return New OracleParameter("CONTAC", OracleDbType.Char, 35)
                End Get
            End Property

            Public Shared ReadOnly Property DEPART As OracleParameter
                Get
                    Return New OracleParameter("DEPART", OracleDbType.Char, 35)
                End Get
            End Property

            Public Shared ReadOnly Property CLASIFI As OracleParameter
                Get
                    Return New OracleParameter("CLASIFI", OracleDbType.Char, 1)
                End Get
            End Property

            Public Shared ReadOnly Property EMILIO As OracleParameter
                Get
                    Return New OracleParameter("EMILIO", OracleDbType.Varchar2, 50)
                End Get
            End Property

            Public Shared ReadOnly Property PUNTOS_SC As OracleParameter
                Get
                    Return New OracleParameter("PUNTOS_SC", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property OBSCAL As OracleParameter
                Get
                    Return New OracleParameter("OBSCAL", OracleDbType.Char, 50)
                End Get
            End Property

            Public Shared ReadOnly Property FECHOMO As OracleParameter
                Get
                    Return New OracleParameter("FECHOMO", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property ENCALIDAD As OracleParameter
                Get
                    Return New OracleParameter("ENCALIDAD", OracleDbType.Char, 1)
                End Get
            End Property

            Public Shared ReadOnly Property DOMICILIA As OracleParameter
                Get
                    Return New OracleParameter("DOMICILIA", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property CONDICOMPRA As OracleParameter
                Get
                    Return New OracleParameter("CONDICOMPRA", OracleDbType.Char, 1)
                End Get
            End Property

            Public Shared ReadOnly Property MEDIOAMBIENTAL As OracleParameter
                Get
                    Return New OracleParameter("MEDIOAMBIENTAL", OracleDbType.Char, 1)
                End Get
            End Property

            Public Shared ReadOnly Property FECENVAMB As OracleParameter
                Get
                    Return New OracleParameter("FECENVAMB", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property FECRECAMB As OracleParameter
                Get
                    Return New OracleParameter("FECRECAMB", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property PRODUCTIVO As OracleParameter
                Get
                    Return New OracleParameter("PRODUCTIVO", OracleDbType.Char, 1)
                End Get
            End Property

            Public Shared ReadOnly Property DOCUM As OracleParameter
                Get
                    Return New OracleParameter("DOCUM", OracleDbType.Char, 2)
                End Get
            End Property

            Public Shared ReadOnly Property OBSERV As OracleParameter
                Get
                    Return New OracleParameter("OBSERV", OracleDbType.Char, 50)
                End Get
            End Property

            Public Shared ReadOnly Property SULOK As OracleParameter
                Get
                    Return New OracleParameter("SULOK", OracleDbType.Char, 1)
                End Get
            End Property

            Public Shared ReadOnly Property EMILIO1 As OracleParameter
                Get
                    Return New OracleParameter("EMILIO1", OracleDbType.Varchar2, 50)
                End Get
            End Property

            Public Shared ReadOnly Property FECHA_CADUC As OracleParameter
                Get
                    Return New OracleParameter("FECHA_CADUC", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property FEC_CREACION As OracleParameter
                Get
                    Return New OracleParameter("FEC_CREACION", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property CODPER As OracleParameter
                Get
                    Return New OracleParameter("CODPER", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property EMILIO_CALIDAD As OracleParameter
                Get
                    Return New OracleParameter("EMILIO_CALIDAD", OracleDbType.Varchar2, 50)
                End Get
            End Property

            Public Shared ReadOnly Property PORTES As OracleParameter
                Get
                    Return New OracleParameter("PORTES", OracleDbType.Char, 1)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const CODPRO As String = "CODPRO"
            Public Const NOMPROV As String = "NOMPROV"
            Public Const RAZON As String = "RAZON"
            Public Const CIF As String = "CIF"
            Public Const DOMICI As String = "DOMICI"
            Public Const DISTRI As String = "DISTRI"
            Public Const LOCALI As String = "LOCALI"
            Public Const PROVIN As String = "PROVIN"
            Public Const CODPAI As String = "CODPAI"
            Public Const CODMON As String = "CODMON"
            Public Const PREPME As String = "PREPME"
            Public Const HABPOT As String = "HABPOT"
            Public Const TIPIVA As String = "TIPIVA"
            Public Const PORSIST As String = "PORSIST"
            Public Const PORTROQ As String = "PORTROQ"
            Public Const FORPAG As String = "FORPAG"
            Public Const NUMCAR As String = "NUMCAR"
            Public Const HOMOLOGADO As String = "HOMOLOGADO"
            Public Const TELEFO As String = "TELEFO"
            Public Const NUMFAX As String = "NUMFAX"
            Public Const CONTAC As String = "CONTAC"
            Public Const DEPART As String = "DEPART"
            Public Const CLASIFI As String = "CLASIFI"
            Public Const EMILIO As String = "EMILIO"
            Public Const PUNTOS_SC As String = "PUNTOS_SC"
            Public Const OBSCAL As String = "OBSCAL"
            Public Const FECHOMO As String = "FECHOMO"
            Public Const ENCALIDAD As String = "ENCALIDAD"
            Public Const DOMICILIA As String = "DOMICILIA"
            Public Const CONDICOMPRA As String = "CONDICOMPRA"
            Public Const MEDIOAMBIENTAL As String = "MEDIOAMBIENTAL"
            Public Const FECENVAMB As String = "FECENVAMB"
            Public Const FECRECAMB As String = "FECRECAMB"
            Public Const PRODUCTIVO As String = "PRODUCTIVO"
            Public Const DOCUM As String = "DOCUM"
            Public Const OBSERV As String = "OBSERV"
            Public Const SULOK As String = "SULOK"
            Public Const EMILIO1 As String = "EMILIO1"
            Public Const FECHA_CADUC As String = "FECHA_CADUC"
            Public Const FEC_CREACION As String = "FEC_CREACION"
            Public Const CODPER As String = "CODPER"
            Public Const EMILIO_CALIDAD As String = "EMILIO_CALIDAD"
            Public Const PORTES As String = "PORTES"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(CODPRO) = W_GCPROVEE.PropertyNames.CODPRO
                    ht(NOMPROV) = W_GCPROVEE.PropertyNames.NOMPROV
                    ht(RAZON) = W_GCPROVEE.PropertyNames.RAZON
                    ht(CIF) = W_GCPROVEE.PropertyNames.CIF
                    ht(DOMICI) = W_GCPROVEE.PropertyNames.DOMICI
                    ht(DISTRI) = W_GCPROVEE.PropertyNames.DISTRI
                    ht(LOCALI) = W_GCPROVEE.PropertyNames.LOCALI
                    ht(PROVIN) = W_GCPROVEE.PropertyNames.PROVIN
                    ht(CODPAI) = W_GCPROVEE.PropertyNames.CODPAI
                    ht(CODMON) = W_GCPROVEE.PropertyNames.CODMON
                    ht(PREPME) = W_GCPROVEE.PropertyNames.PREPME
                    ht(HABPOT) = W_GCPROVEE.PropertyNames.HABPOT
                    ht(TIPIVA) = W_GCPROVEE.PropertyNames.TIPIVA
                    ht(PORSIST) = W_GCPROVEE.PropertyNames.PORSIST
                    ht(PORTROQ) = W_GCPROVEE.PropertyNames.PORTROQ
                    ht(FORPAG) = W_GCPROVEE.PropertyNames.FORPAG
                    ht(NUMCAR) = W_GCPROVEE.PropertyNames.NUMCAR
                    ht(HOMOLOGADO) = W_GCPROVEE.PropertyNames.HOMOLOGADO
                    ht(TELEFO) = W_GCPROVEE.PropertyNames.TELEFO
                    ht(NUMFAX) = W_GCPROVEE.PropertyNames.NUMFAX
                    ht(CONTAC) = W_GCPROVEE.PropertyNames.CONTAC
                    ht(DEPART) = W_GCPROVEE.PropertyNames.DEPART
                    ht(CLASIFI) = W_GCPROVEE.PropertyNames.CLASIFI
                    ht(EMILIO) = W_GCPROVEE.PropertyNames.EMILIO
                    ht(PUNTOS_SC) = W_GCPROVEE.PropertyNames.PUNTOS_SC
                    ht(OBSCAL) = W_GCPROVEE.PropertyNames.OBSCAL
                    ht(FECHOMO) = W_GCPROVEE.PropertyNames.FECHOMO
                    ht(ENCALIDAD) = W_GCPROVEE.PropertyNames.ENCALIDAD
                    ht(DOMICILIA) = W_GCPROVEE.PropertyNames.DOMICILIA
                    ht(CONDICOMPRA) = W_GCPROVEE.PropertyNames.CONDICOMPRA
                    ht(MEDIOAMBIENTAL) = W_GCPROVEE.PropertyNames.MEDIOAMBIENTAL
                    ht(FECENVAMB) = W_GCPROVEE.PropertyNames.FECENVAMB
                    ht(FECRECAMB) = W_GCPROVEE.PropertyNames.FECRECAMB
                    ht(PRODUCTIVO) = W_GCPROVEE.PropertyNames.PRODUCTIVO
                    ht(DOCUM) = W_GCPROVEE.PropertyNames.DOCUM
                    ht(OBSERV) = W_GCPROVEE.PropertyNames.OBSERV
                    ht(SULOK) = W_GCPROVEE.PropertyNames.SULOK
                    ht(EMILIO1) = W_GCPROVEE.PropertyNames.EMILIO1
                    ht(FECHA_CADUC) = W_GCPROVEE.PropertyNames.FECHA_CADUC
                    ht(FEC_CREACION) = W_GCPROVEE.PropertyNames.FEC_CREACION
                    ht(CODPER) = W_GCPROVEE.PropertyNames.CODPER
                    ht(EMILIO_CALIDAD) = W_GCPROVEE.PropertyNames.EMILIO_CALIDAD
                    ht(PORTES) = W_GCPROVEE.PropertyNames.PORTES

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const CODPRO As String = "CODPRO"
            Public Const NOMPROV As String = "NOMPROV"
            Public Const RAZON As String = "RAZON"
            Public Const CIF As String = "CIF"
            Public Const DOMICI As String = "DOMICI"
            Public Const DISTRI As String = "DISTRI"
            Public Const LOCALI As String = "LOCALI"
            Public Const PROVIN As String = "PROVIN"
            Public Const CODPAI As String = "CODPAI"
            Public Const CODMON As String = "CODMON"
            Public Const PREPME As String = "PREPME"
            Public Const HABPOT As String = "HABPOT"
            Public Const TIPIVA As String = "TIPIVA"
            Public Const PORSIST As String = "PORSIST"
            Public Const PORTROQ As String = "PORTROQ"
            Public Const FORPAG As String = "FORPAG"
            Public Const NUMCAR As String = "NUMCAR"
            Public Const HOMOLOGADO As String = "HOMOLOGADO"
            Public Const TELEFO As String = "TELEFO"
            Public Const NUMFAX As String = "NUMFAX"
            Public Const CONTAC As String = "CONTAC"
            Public Const DEPART As String = "DEPART"
            Public Const CLASIFI As String = "CLASIFI"
            Public Const EMILIO As String = "EMILIO"
            Public Const PUNTOS_SC As String = "PUNTOS_SC"
            Public Const OBSCAL As String = "OBSCAL"
            Public Const FECHOMO As String = "FECHOMO"
            Public Const ENCALIDAD As String = "ENCALIDAD"
            Public Const DOMICILIA As String = "DOMICILIA"
            Public Const CONDICOMPRA As String = "CONDICOMPRA"
            Public Const MEDIOAMBIENTAL As String = "MEDIOAMBIENTAL"
            Public Const FECENVAMB As String = "FECENVAMB"
            Public Const FECRECAMB As String = "FECRECAMB"
            Public Const PRODUCTIVO As String = "PRODUCTIVO"
            Public Const DOCUM As String = "DOCUM"
            Public Const OBSERV As String = "OBSERV"
            Public Const SULOK As String = "SULOK"
            Public Const EMILIO1 As String = "EMILIO1"
            Public Const FECHA_CADUC As String = "FECHA_CADUC"
            Public Const FEC_CREACION As String = "FEC_CREACION"
            Public Const CODPER As String = "CODPER"
            Public Const EMILIO_CALIDAD As String = "EMILIO_CALIDAD"
            Public Const PORTES As String = "PORTES"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(CODPRO) = W_GCPROVEE.ColumnNames.CODPRO
                    ht(NOMPROV) = W_GCPROVEE.ColumnNames.NOMPROV
                    ht(RAZON) = W_GCPROVEE.ColumnNames.RAZON
                    ht(CIF) = W_GCPROVEE.ColumnNames.CIF
                    ht(DOMICI) = W_GCPROVEE.ColumnNames.DOMICI
                    ht(DISTRI) = W_GCPROVEE.ColumnNames.DISTRI
                    ht(LOCALI) = W_GCPROVEE.ColumnNames.LOCALI
                    ht(PROVIN) = W_GCPROVEE.ColumnNames.PROVIN
                    ht(CODPAI) = W_GCPROVEE.ColumnNames.CODPAI
                    ht(CODMON) = W_GCPROVEE.ColumnNames.CODMON
                    ht(PREPME) = W_GCPROVEE.ColumnNames.PREPME
                    ht(HABPOT) = W_GCPROVEE.ColumnNames.HABPOT
                    ht(TIPIVA) = W_GCPROVEE.ColumnNames.TIPIVA
                    ht(PORSIST) = W_GCPROVEE.ColumnNames.PORSIST
                    ht(PORTROQ) = W_GCPROVEE.ColumnNames.PORTROQ
                    ht(FORPAG) = W_GCPROVEE.ColumnNames.FORPAG
                    ht(NUMCAR) = W_GCPROVEE.ColumnNames.NUMCAR
                    ht(HOMOLOGADO) = W_GCPROVEE.ColumnNames.HOMOLOGADO
                    ht(TELEFO) = W_GCPROVEE.ColumnNames.TELEFO
                    ht(NUMFAX) = W_GCPROVEE.ColumnNames.NUMFAX
                    ht(CONTAC) = W_GCPROVEE.ColumnNames.CONTAC
                    ht(DEPART) = W_GCPROVEE.ColumnNames.DEPART
                    ht(CLASIFI) = W_GCPROVEE.ColumnNames.CLASIFI
                    ht(EMILIO) = W_GCPROVEE.ColumnNames.EMILIO
                    ht(PUNTOS_SC) = W_GCPROVEE.ColumnNames.PUNTOS_SC
                    ht(OBSCAL) = W_GCPROVEE.ColumnNames.OBSCAL
                    ht(FECHOMO) = W_GCPROVEE.ColumnNames.FECHOMO
                    ht(ENCALIDAD) = W_GCPROVEE.ColumnNames.ENCALIDAD
                    ht(DOMICILIA) = W_GCPROVEE.ColumnNames.DOMICILIA
                    ht(CONDICOMPRA) = W_GCPROVEE.ColumnNames.CONDICOMPRA
                    ht(MEDIOAMBIENTAL) = W_GCPROVEE.ColumnNames.MEDIOAMBIENTAL
                    ht(FECENVAMB) = W_GCPROVEE.ColumnNames.FECENVAMB
                    ht(FECRECAMB) = W_GCPROVEE.ColumnNames.FECRECAMB
                    ht(PRODUCTIVO) = W_GCPROVEE.ColumnNames.PRODUCTIVO
                    ht(DOCUM) = W_GCPROVEE.ColumnNames.DOCUM
                    ht(OBSERV) = W_GCPROVEE.ColumnNames.OBSERV
                    ht(SULOK) = W_GCPROVEE.ColumnNames.SULOK
                    ht(EMILIO1) = W_GCPROVEE.ColumnNames.EMILIO1
                    ht(FECHA_CADUC) = W_GCPROVEE.ColumnNames.FECHA_CADUC
                    ht(FEC_CREACION) = W_GCPROVEE.ColumnNames.FEC_CREACION
                    ht(CODPER) = W_GCPROVEE.ColumnNames.CODPER
                    ht(EMILIO_CALIDAD) = W_GCPROVEE.ColumnNames.EMILIO_CALIDAD
                    ht(PORTES) = W_GCPROVEE.ColumnNames.PORTES

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const CODPRO As String = "s_CODPRO"
            Public Const NOMPROV As String = "s_NOMPROV"
            Public Const RAZON As String = "s_RAZON"
            Public Const CIF As String = "s_CIF"
            Public Const DOMICI As String = "s_DOMICI"
            Public Const DISTRI As String = "s_DISTRI"
            Public Const LOCALI As String = "s_LOCALI"
            Public Const PROVIN As String = "s_PROVIN"
            Public Const CODPAI As String = "s_CODPAI"
            Public Const CODMON As String = "s_CODMON"
            Public Const PREPME As String = "s_PREPME"
            Public Const HABPOT As String = "s_HABPOT"
            Public Const TIPIVA As String = "s_TIPIVA"
            Public Const PORSIST As String = "s_PORSIST"
            Public Const PORTROQ As String = "s_PORTROQ"
            Public Const FORPAG As String = "s_FORPAG"
            Public Const NUMCAR As String = "s_NUMCAR"
            Public Const HOMOLOGADO As String = "s_HOMOLOGADO"
            Public Const TELEFO As String = "s_TELEFO"
            Public Const NUMFAX As String = "s_NUMFAX"
            Public Const CONTAC As String = "s_CONTAC"
            Public Const DEPART As String = "s_DEPART"
            Public Const CLASIFI As String = "s_CLASIFI"
            Public Const EMILIO As String = "s_EMILIO"
            Public Const PUNTOS_SC As String = "s_PUNTOS_SC"
            Public Const OBSCAL As String = "s_OBSCAL"
            Public Const FECHOMO As String = "s_FECHOMO"
            Public Const ENCALIDAD As String = "s_ENCALIDAD"
            Public Const DOMICILIA As String = "s_DOMICILIA"
            Public Const CONDICOMPRA As String = "s_CONDICOMPRA"
            Public Const MEDIOAMBIENTAL As String = "s_MEDIOAMBIENTAL"
            Public Const FECENVAMB As String = "s_FECENVAMB"
            Public Const FECRECAMB As String = "s_FECRECAMB"
            Public Const PRODUCTIVO As String = "s_PRODUCTIVO"
            Public Const DOCUM As String = "s_DOCUM"
            Public Const OBSERV As String = "s_OBSERV"
            Public Const SULOK As String = "s_SULOK"
            Public Const EMILIO1 As String = "s_EMILIO1"
            Public Const FECHA_CADUC As String = "s_FECHA_CADUC"
            Public Const FEC_CREACION As String = "s_FEC_CREACION"
            Public Const CODPER As String = "s_CODPER"
            Public Const EMILIO_CALIDAD As String = "s_EMILIO_CALIDAD"
            Public Const PORTES As String = "s_PORTES"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property CODPRO As String
            Get
                Return MyBase.GetString(ColumnNames.CODPRO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CODPRO, Value)
            End Set
        End Property

        Public Overridable Property NOMPROV As String
            Get
                Return MyBase.GetString(ColumnNames.NOMPROV)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.NOMPROV, Value)
            End Set
        End Property

        Public Overridable Property RAZON As String
            Get
                Return MyBase.GetString(ColumnNames.RAZON)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.RAZON, Value)
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

        Public Overridable Property DOMICI As String
            Get
                Return MyBase.GetString(ColumnNames.DOMICI)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.DOMICI, Value)
            End Set
        End Property

        Public Overridable Property DISTRI As String
            Get
                Return MyBase.GetString(ColumnNames.DISTRI)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.DISTRI, Value)
            End Set
        End Property

        Public Overridable Property LOCALI As String
            Get
                Return MyBase.GetString(ColumnNames.LOCALI)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.LOCALI, Value)
            End Set
        End Property

        Public Overridable Property PROVIN As String
            Get
                Return MyBase.GetString(ColumnNames.PROVIN)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.PROVIN, Value)
            End Set
        End Property

        Public Overridable Property CODPAI As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.CODPAI)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.CODPAI, Value)
            End Set
        End Property

        Public Overridable Property CODMON As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.CODMON)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.CODMON, Value)
            End Set
        End Property

        Public Overridable Property PREPME As String
            Get
                Return MyBase.GetString(ColumnNames.PREPME)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.PREPME, Value)
            End Set
        End Property

        Public Overridable Property HABPOT As String
            Get
                Return MyBase.GetString(ColumnNames.HABPOT)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.HABPOT, Value)
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

        Public Overridable Property PORSIST As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.PORSIST)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.PORSIST, Value)
            End Set
        End Property

        Public Overridable Property PORTROQ As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.PORTROQ)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.PORTROQ, Value)
            End Set
        End Property

        Public Overridable Property FORPAG As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.FORPAG)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.FORPAG, Value)
            End Set
        End Property

        Public Overridable Property NUMCAR As String
            Get
                Return MyBase.GetString(ColumnNames.NUMCAR)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.NUMCAR, Value)
            End Set
        End Property

        Public Overridable Property HOMOLOGADO As String
            Get
                Return MyBase.GetString(ColumnNames.HOMOLOGADO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.HOMOLOGADO, Value)
            End Set
        End Property

        Public Overridable Property TELEFO As String
            Get
                Return MyBase.GetString(ColumnNames.TELEFO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.TELEFO, Value)
            End Set
        End Property

        Public Overridable Property NUMFAX As String
            Get
                Return MyBase.GetString(ColumnNames.NUMFAX)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.NUMFAX, Value)
            End Set
        End Property

        Public Overridable Property CONTAC As String
            Get
                Return MyBase.GetString(ColumnNames.CONTAC)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CONTAC, Value)
            End Set
        End Property

        Public Overridable Property DEPART As String
            Get
                Return MyBase.GetString(ColumnNames.DEPART)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.DEPART, Value)
            End Set
        End Property

        Public Overridable Property CLASIFI As String
            Get
                Return MyBase.GetString(ColumnNames.CLASIFI)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CLASIFI, Value)
            End Set
        End Property

        Public Overridable Property EMILIO As String
            Get
                Return MyBase.GetString(ColumnNames.EMILIO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.EMILIO, Value)
            End Set
        End Property

        Public Overridable Property PUNTOS_SC As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.PUNTOS_SC)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.PUNTOS_SC, Value)
            End Set
        End Property

        Public Overridable Property OBSCAL As String
            Get
                Return MyBase.GetString(ColumnNames.OBSCAL)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.OBSCAL, Value)
            End Set
        End Property

        Public Overridable Property FECHOMO As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECHOMO)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECHOMO, Value)
            End Set
        End Property

        Public Overridable Property ENCALIDAD As String
            Get
                Return MyBase.GetString(ColumnNames.ENCALIDAD)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.ENCALIDAD, Value)
            End Set
        End Property

        Public Overridable Property DOMICILIA As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.DOMICILIA)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.DOMICILIA, Value)
            End Set
        End Property

        Public Overridable Property CONDICOMPRA As String
            Get
                Return MyBase.GetString(ColumnNames.CONDICOMPRA)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CONDICOMPRA, Value)
            End Set
        End Property

        Public Overridable Property MEDIOAMBIENTAL As String
            Get
                Return MyBase.GetString(ColumnNames.MEDIOAMBIENTAL)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.MEDIOAMBIENTAL, Value)
            End Set
        End Property

        Public Overridable Property FECENVAMB As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECENVAMB)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECENVAMB, Value)
            End Set
        End Property

        Public Overridable Property FECRECAMB As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECRECAMB)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECRECAMB, Value)
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

        Public Overridable Property DOCUM As String
            Get
                Return MyBase.GetString(ColumnNames.DOCUM)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.DOCUM, Value)
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

        Public Overridable Property SULOK As String
            Get
                Return MyBase.GetString(ColumnNames.SULOK)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.SULOK, Value)
            End Set
        End Property

        Public Overridable Property EMILIO1 As String
            Get
                Return MyBase.GetString(ColumnNames.EMILIO1)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.EMILIO1, Value)
            End Set
        End Property

        Public Overridable Property FECHA_CADUC As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECHA_CADUC)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECHA_CADUC, Value)
            End Set
        End Property

        Public Overridable Property FEC_CREACION As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FEC_CREACION)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FEC_CREACION, Value)
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

        Public Overridable Property EMILIO_CALIDAD As String
            Get
                Return MyBase.GetString(ColumnNames.EMILIO_CALIDAD)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.EMILIO_CALIDAD, Value)
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

        Public Overridable Property s_NOMPROV As String
            Get
                If Me.IsColumnNull(ColumnNames.NOMPROV) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.NOMPROV)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NOMPROV)
                Else
                    Me.NOMPROV = MyBase.SetStringAsString(ColumnNames.NOMPROV, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_RAZON As String
            Get
                If Me.IsColumnNull(ColumnNames.RAZON) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.RAZON)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.RAZON)
                Else
                    Me.RAZON = MyBase.SetStringAsString(ColumnNames.RAZON, Value)
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

        Public Overridable Property s_DOMICI As String
            Get
                If Me.IsColumnNull(ColumnNames.DOMICI) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.DOMICI)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.DOMICI)
                Else
                    Me.DOMICI = MyBase.SetStringAsString(ColumnNames.DOMICI, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_DISTRI As String
            Get
                If Me.IsColumnNull(ColumnNames.DISTRI) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.DISTRI)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.DISTRI)
                Else
                    Me.DISTRI = MyBase.SetStringAsString(ColumnNames.DISTRI, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_LOCALI As String
            Get
                If Me.IsColumnNull(ColumnNames.LOCALI) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.LOCALI)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.LOCALI)
                Else
                    Me.LOCALI = MyBase.SetStringAsString(ColumnNames.LOCALI, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_PROVIN As String
            Get
                If Me.IsColumnNull(ColumnNames.PROVIN) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.PROVIN)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.PROVIN)
                Else
                    Me.PROVIN = MyBase.SetStringAsString(ColumnNames.PROVIN, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CODPAI As String
            Get
                If Me.IsColumnNull(ColumnNames.CODPAI) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.CODPAI)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CODPAI)
                Else
                    Me.CODPAI = MyBase.SetDecimalAsString(ColumnNames.CODPAI, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CODMON As String
            Get
                If Me.IsColumnNull(ColumnNames.CODMON) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.CODMON)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CODMON)
                Else
                    Me.CODMON = MyBase.SetDecimalAsString(ColumnNames.CODMON, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_PREPME As String
            Get
                If Me.IsColumnNull(ColumnNames.PREPME) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.PREPME)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.PREPME)
                Else
                    Me.PREPME = MyBase.SetStringAsString(ColumnNames.PREPME, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_HABPOT As String
            Get
                If Me.IsColumnNull(ColumnNames.HABPOT) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.HABPOT)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.HABPOT)
                Else
                    Me.HABPOT = MyBase.SetStringAsString(ColumnNames.HABPOT, Value)
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

        Public Overridable Property s_PORSIST As String
            Get
                If Me.IsColumnNull(ColumnNames.PORSIST) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.PORSIST)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.PORSIST)
                Else
                    Me.PORSIST = MyBase.SetDecimalAsString(ColumnNames.PORSIST, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_PORTROQ As String
            Get
                If Me.IsColumnNull(ColumnNames.PORTROQ) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.PORTROQ)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.PORTROQ)
                Else
                    Me.PORTROQ = MyBase.SetDecimalAsString(ColumnNames.PORTROQ, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FORPAG As String
            Get
                If Me.IsColumnNull(ColumnNames.FORPAG) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.FORPAG)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FORPAG)
                Else
                    Me.FORPAG = MyBase.SetDecimalAsString(ColumnNames.FORPAG, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_NUMCAR As String
            Get
                If Me.IsColumnNull(ColumnNames.NUMCAR) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.NUMCAR)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NUMCAR)
                Else
                    Me.NUMCAR = MyBase.SetStringAsString(ColumnNames.NUMCAR, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_HOMOLOGADO As String
            Get
                If Me.IsColumnNull(ColumnNames.HOMOLOGADO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.HOMOLOGADO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.HOMOLOGADO)
                Else
                    Me.HOMOLOGADO = MyBase.SetStringAsString(ColumnNames.HOMOLOGADO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_TELEFO As String
            Get
                If Me.IsColumnNull(ColumnNames.TELEFO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.TELEFO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.TELEFO)
                Else
                    Me.TELEFO = MyBase.SetStringAsString(ColumnNames.TELEFO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_NUMFAX As String
            Get
                If Me.IsColumnNull(ColumnNames.NUMFAX) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.NUMFAX)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NUMFAX)
                Else
                    Me.NUMFAX = MyBase.SetStringAsString(ColumnNames.NUMFAX, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CONTAC As String
            Get
                If Me.IsColumnNull(ColumnNames.CONTAC) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CONTAC)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CONTAC)
                Else
                    Me.CONTAC = MyBase.SetStringAsString(ColumnNames.CONTAC, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_DEPART As String
            Get
                If Me.IsColumnNull(ColumnNames.DEPART) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.DEPART)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.DEPART)
                Else
                    Me.DEPART = MyBase.SetStringAsString(ColumnNames.DEPART, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CLASIFI As String
            Get
                If Me.IsColumnNull(ColumnNames.CLASIFI) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CLASIFI)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CLASIFI)
                Else
                    Me.CLASIFI = MyBase.SetStringAsString(ColumnNames.CLASIFI, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_EMILIO As String
            Get
                If Me.IsColumnNull(ColumnNames.EMILIO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.EMILIO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.EMILIO)
                Else
                    Me.EMILIO = MyBase.SetStringAsString(ColumnNames.EMILIO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_PUNTOS_SC As String
            Get
                If Me.IsColumnNull(ColumnNames.PUNTOS_SC) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.PUNTOS_SC)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.PUNTOS_SC)
                Else
                    Me.PUNTOS_SC = MyBase.SetDecimalAsString(ColumnNames.PUNTOS_SC, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_OBSCAL As String
            Get
                If Me.IsColumnNull(ColumnNames.OBSCAL) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.OBSCAL)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.OBSCAL)
                Else
                    Me.OBSCAL = MyBase.SetStringAsString(ColumnNames.OBSCAL, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FECHOMO As String
            Get
                If Me.IsColumnNull(ColumnNames.FECHOMO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FECHOMO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FECHOMO)
                Else
                    Me.FECHOMO = MyBase.SetDateTimeAsString(ColumnNames.FECHOMO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_ENCALIDAD As String
            Get
                If Me.IsColumnNull(ColumnNames.ENCALIDAD) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.ENCALIDAD)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.ENCALIDAD)
                Else
                    Me.ENCALIDAD = MyBase.SetStringAsString(ColumnNames.ENCALIDAD, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_DOMICILIA As String
            Get
                If Me.IsColumnNull(ColumnNames.DOMICILIA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.DOMICILIA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.DOMICILIA)
                Else
                    Me.DOMICILIA = MyBase.SetDecimalAsString(ColumnNames.DOMICILIA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CONDICOMPRA As String
            Get
                If Me.IsColumnNull(ColumnNames.CONDICOMPRA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CONDICOMPRA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CONDICOMPRA)
                Else
                    Me.CONDICOMPRA = MyBase.SetStringAsString(ColumnNames.CONDICOMPRA, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_MEDIOAMBIENTAL As String
            Get
                If Me.IsColumnNull(ColumnNames.MEDIOAMBIENTAL) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.MEDIOAMBIENTAL)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.MEDIOAMBIENTAL)
                Else
                    Me.MEDIOAMBIENTAL = MyBase.SetStringAsString(ColumnNames.MEDIOAMBIENTAL, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FECENVAMB As String
            Get
                If Me.IsColumnNull(ColumnNames.FECENVAMB) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FECENVAMB)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FECENVAMB)
                Else
                    Me.FECENVAMB = MyBase.SetDateTimeAsString(ColumnNames.FECENVAMB, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FECRECAMB As String
            Get
                If Me.IsColumnNull(ColumnNames.FECRECAMB) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FECRECAMB)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FECRECAMB)
                Else
                    Me.FECRECAMB = MyBase.SetDateTimeAsString(ColumnNames.FECRECAMB, Value)
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

        Public Overridable Property s_DOCUM As String
            Get
                If Me.IsColumnNull(ColumnNames.DOCUM) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.DOCUM)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.DOCUM)
                Else
                    Me.DOCUM = MyBase.SetStringAsString(ColumnNames.DOCUM, Value)
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

        Public Overridable Property s_SULOK As String
            Get
                If Me.IsColumnNull(ColumnNames.SULOK) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.SULOK)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.SULOK)
                Else
                    Me.SULOK = MyBase.SetStringAsString(ColumnNames.SULOK, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_EMILIO1 As String
            Get
                If Me.IsColumnNull(ColumnNames.EMILIO1) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.EMILIO1)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.EMILIO1)
                Else
                    Me.EMILIO1 = MyBase.SetStringAsString(ColumnNames.EMILIO1, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FECHA_CADUC As String
            Get
                If Me.IsColumnNull(ColumnNames.FECHA_CADUC) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FECHA_CADUC)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FECHA_CADUC)
                Else
                    Me.FECHA_CADUC = MyBase.SetDateTimeAsString(ColumnNames.FECHA_CADUC, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FEC_CREACION As String
            Get
                If Me.IsColumnNull(ColumnNames.FEC_CREACION) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FEC_CREACION)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FEC_CREACION)
                Else
                    Me.FEC_CREACION = MyBase.SetDateTimeAsString(ColumnNames.FEC_CREACION, Value)
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

        Public Overridable Property s_EMILIO_CALIDAD As String
            Get
                If Me.IsColumnNull(ColumnNames.EMILIO_CALIDAD) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.EMILIO_CALIDAD)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.EMILIO_CALIDAD)
                Else
                    Me.EMILIO_CALIDAD = MyBase.SetStringAsString(ColumnNames.EMILIO_CALIDAD, Value)
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


                Public ReadOnly Property CODPRO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CODPRO, Parameters.CODPRO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NOMPROV() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NOMPROV, Parameters.NOMPROV)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property RAZON() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.RAZON, Parameters.RAZON)
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

                Public ReadOnly Property DOMICI() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.DOMICI, Parameters.DOMICI)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DISTRI() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.DISTRI, Parameters.DISTRI)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property LOCALI() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.LOCALI, Parameters.LOCALI)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PROVIN() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.PROVIN, Parameters.PROVIN)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODPAI() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CODPAI, Parameters.CODPAI)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODMON() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CODMON, Parameters.CODMON)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PREPME() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.PREPME, Parameters.PREPME)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property HABPOT() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.HABPOT, Parameters.HABPOT)
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

                Public ReadOnly Property PORSIST() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.PORSIST, Parameters.PORSIST)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PORTROQ() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.PORTROQ, Parameters.PORTROQ)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FORPAG() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FORPAG, Parameters.FORPAG)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMCAR() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMCAR, Parameters.NUMCAR)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property HOMOLOGADO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.HOMOLOGADO, Parameters.HOMOLOGADO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TELEFO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.TELEFO, Parameters.TELEFO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMFAX() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMFAX, Parameters.NUMFAX)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CONTAC() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CONTAC, Parameters.CONTAC)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DEPART() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.DEPART, Parameters.DEPART)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CLASIFI() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CLASIFI, Parameters.CLASIFI)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property EMILIO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.EMILIO, Parameters.EMILIO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PUNTOS_SC() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.PUNTOS_SC, Parameters.PUNTOS_SC)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property OBSCAL() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.OBSCAL, Parameters.OBSCAL)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECHOMO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FECHOMO, Parameters.FECHOMO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ENCALIDAD() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.ENCALIDAD, Parameters.ENCALIDAD)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DOMICILIA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.DOMICILIA, Parameters.DOMICILIA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CONDICOMPRA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CONDICOMPRA, Parameters.CONDICOMPRA)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property MEDIOAMBIENTAL() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.MEDIOAMBIENTAL, Parameters.MEDIOAMBIENTAL)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECENVAMB() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FECENVAMB, Parameters.FECENVAMB)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECRECAMB() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FECRECAMB, Parameters.FECRECAMB)
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

                Public ReadOnly Property DOCUM() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.DOCUM, Parameters.DOCUM)
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

                Public ReadOnly Property SULOK() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.SULOK, Parameters.SULOK)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property EMILIO1() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.EMILIO1, Parameters.EMILIO1)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECHA_CADUC() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FECHA_CADUC, Parameters.FECHA_CADUC)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FEC_CREACION() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FEC_CREACION, Parameters.FEC_CREACION)
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

                Public ReadOnly Property EMILIO_CALIDAD() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.EMILIO_CALIDAD, Parameters.EMILIO_CALIDAD)
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

            Public ReadOnly Property CODPRO() As WhereParameter
                Get
                    If _CODPRO_W Is Nothing Then
                        _CODPRO_W = TearOff.CODPRO
                    End If
                    Return _CODPRO_W
                End Get
            End Property

            Public ReadOnly Property NOMPROV() As WhereParameter
                Get
                    If _NOMPROV_W Is Nothing Then
                        _NOMPROV_W = TearOff.NOMPROV
                    End If
                    Return _NOMPROV_W
                End Get
            End Property

            Public ReadOnly Property RAZON() As WhereParameter
                Get
                    If _RAZON_W Is Nothing Then
                        _RAZON_W = TearOff.RAZON
                    End If
                    Return _RAZON_W
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

            Public ReadOnly Property DOMICI() As WhereParameter
                Get
                    If _DOMICI_W Is Nothing Then
                        _DOMICI_W = TearOff.DOMICI
                    End If
                    Return _DOMICI_W
                End Get
            End Property

            Public ReadOnly Property DISTRI() As WhereParameter
                Get
                    If _DISTRI_W Is Nothing Then
                        _DISTRI_W = TearOff.DISTRI
                    End If
                    Return _DISTRI_W
                End Get
            End Property

            Public ReadOnly Property LOCALI() As WhereParameter
                Get
                    If _LOCALI_W Is Nothing Then
                        _LOCALI_W = TearOff.LOCALI
                    End If
                    Return _LOCALI_W
                End Get
            End Property

            Public ReadOnly Property PROVIN() As WhereParameter
                Get
                    If _PROVIN_W Is Nothing Then
                        _PROVIN_W = TearOff.PROVIN
                    End If
                    Return _PROVIN_W
                End Get
            End Property

            Public ReadOnly Property CODPAI() As WhereParameter
                Get
                    If _CODPAI_W Is Nothing Then
                        _CODPAI_W = TearOff.CODPAI
                    End If
                    Return _CODPAI_W
                End Get
            End Property

            Public ReadOnly Property CODMON() As WhereParameter
                Get
                    If _CODMON_W Is Nothing Then
                        _CODMON_W = TearOff.CODMON
                    End If
                    Return _CODMON_W
                End Get
            End Property

            Public ReadOnly Property PREPME() As WhereParameter
                Get
                    If _PREPME_W Is Nothing Then
                        _PREPME_W = TearOff.PREPME
                    End If
                    Return _PREPME_W
                End Get
            End Property

            Public ReadOnly Property HABPOT() As WhereParameter
                Get
                    If _HABPOT_W Is Nothing Then
                        _HABPOT_W = TearOff.HABPOT
                    End If
                    Return _HABPOT_W
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

            Public ReadOnly Property PORSIST() As WhereParameter
                Get
                    If _PORSIST_W Is Nothing Then
                        _PORSIST_W = TearOff.PORSIST
                    End If
                    Return _PORSIST_W
                End Get
            End Property

            Public ReadOnly Property PORTROQ() As WhereParameter
                Get
                    If _PORTROQ_W Is Nothing Then
                        _PORTROQ_W = TearOff.PORTROQ
                    End If
                    Return _PORTROQ_W
                End Get
            End Property

            Public ReadOnly Property FORPAG() As WhereParameter
                Get
                    If _FORPAG_W Is Nothing Then
                        _FORPAG_W = TearOff.FORPAG
                    End If
                    Return _FORPAG_W
                End Get
            End Property

            Public ReadOnly Property NUMCAR() As WhereParameter
                Get
                    If _NUMCAR_W Is Nothing Then
                        _NUMCAR_W = TearOff.NUMCAR
                    End If
                    Return _NUMCAR_W
                End Get
            End Property

            Public ReadOnly Property HOMOLOGADO() As WhereParameter
                Get
                    If _HOMOLOGADO_W Is Nothing Then
                        _HOMOLOGADO_W = TearOff.HOMOLOGADO
                    End If
                    Return _HOMOLOGADO_W
                End Get
            End Property

            Public ReadOnly Property TELEFO() As WhereParameter
                Get
                    If _TELEFO_W Is Nothing Then
                        _TELEFO_W = TearOff.TELEFO
                    End If
                    Return _TELEFO_W
                End Get
            End Property

            Public ReadOnly Property NUMFAX() As WhereParameter
                Get
                    If _NUMFAX_W Is Nothing Then
                        _NUMFAX_W = TearOff.NUMFAX
                    End If
                    Return _NUMFAX_W
                End Get
            End Property

            Public ReadOnly Property CONTAC() As WhereParameter
                Get
                    If _CONTAC_W Is Nothing Then
                        _CONTAC_W = TearOff.CONTAC
                    End If
                    Return _CONTAC_W
                End Get
            End Property

            Public ReadOnly Property DEPART() As WhereParameter
                Get
                    If _DEPART_W Is Nothing Then
                        _DEPART_W = TearOff.DEPART
                    End If
                    Return _DEPART_W
                End Get
            End Property

            Public ReadOnly Property CLASIFI() As WhereParameter
                Get
                    If _CLASIFI_W Is Nothing Then
                        _CLASIFI_W = TearOff.CLASIFI
                    End If
                    Return _CLASIFI_W
                End Get
            End Property

            Public ReadOnly Property EMILIO() As WhereParameter
                Get
                    If _EMILIO_W Is Nothing Then
                        _EMILIO_W = TearOff.EMILIO
                    End If
                    Return _EMILIO_W
                End Get
            End Property

            Public ReadOnly Property PUNTOS_SC() As WhereParameter
                Get
                    If _PUNTOS_SC_W Is Nothing Then
                        _PUNTOS_SC_W = TearOff.PUNTOS_SC
                    End If
                    Return _PUNTOS_SC_W
                End Get
            End Property

            Public ReadOnly Property OBSCAL() As WhereParameter
                Get
                    If _OBSCAL_W Is Nothing Then
                        _OBSCAL_W = TearOff.OBSCAL
                    End If
                    Return _OBSCAL_W
                End Get
            End Property

            Public ReadOnly Property FECHOMO() As WhereParameter
                Get
                    If _FECHOMO_W Is Nothing Then
                        _FECHOMO_W = TearOff.FECHOMO
                    End If
                    Return _FECHOMO_W
                End Get
            End Property

            Public ReadOnly Property ENCALIDAD() As WhereParameter
                Get
                    If _ENCALIDAD_W Is Nothing Then
                        _ENCALIDAD_W = TearOff.ENCALIDAD
                    End If
                    Return _ENCALIDAD_W
                End Get
            End Property

            Public ReadOnly Property DOMICILIA() As WhereParameter
                Get
                    If _DOMICILIA_W Is Nothing Then
                        _DOMICILIA_W = TearOff.DOMICILIA
                    End If
                    Return _DOMICILIA_W
                End Get
            End Property

            Public ReadOnly Property CONDICOMPRA() As WhereParameter
                Get
                    If _CONDICOMPRA_W Is Nothing Then
                        _CONDICOMPRA_W = TearOff.CONDICOMPRA
                    End If
                    Return _CONDICOMPRA_W
                End Get
            End Property

            Public ReadOnly Property MEDIOAMBIENTAL() As WhereParameter
                Get
                    If _MEDIOAMBIENTAL_W Is Nothing Then
                        _MEDIOAMBIENTAL_W = TearOff.MEDIOAMBIENTAL
                    End If
                    Return _MEDIOAMBIENTAL_W
                End Get
            End Property

            Public ReadOnly Property FECENVAMB() As WhereParameter
                Get
                    If _FECENVAMB_W Is Nothing Then
                        _FECENVAMB_W = TearOff.FECENVAMB
                    End If
                    Return _FECENVAMB_W
                End Get
            End Property

            Public ReadOnly Property FECRECAMB() As WhereParameter
                Get
                    If _FECRECAMB_W Is Nothing Then
                        _FECRECAMB_W = TearOff.FECRECAMB
                    End If
                    Return _FECRECAMB_W
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

            Public ReadOnly Property DOCUM() As WhereParameter
                Get
                    If _DOCUM_W Is Nothing Then
                        _DOCUM_W = TearOff.DOCUM
                    End If
                    Return _DOCUM_W
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

            Public ReadOnly Property SULOK() As WhereParameter
                Get
                    If _SULOK_W Is Nothing Then
                        _SULOK_W = TearOff.SULOK
                    End If
                    Return _SULOK_W
                End Get
            End Property

            Public ReadOnly Property EMILIO1() As WhereParameter
                Get
                    If _EMILIO1_W Is Nothing Then
                        _EMILIO1_W = TearOff.EMILIO1
                    End If
                    Return _EMILIO1_W
                End Get
            End Property

            Public ReadOnly Property FECHA_CADUC() As WhereParameter
                Get
                    If _FECHA_CADUC_W Is Nothing Then
                        _FECHA_CADUC_W = TearOff.FECHA_CADUC
                    End If
                    Return _FECHA_CADUC_W
                End Get
            End Property

            Public ReadOnly Property FEC_CREACION() As WhereParameter
                Get
                    If _FEC_CREACION_W Is Nothing Then
                        _FEC_CREACION_W = TearOff.FEC_CREACION
                    End If
                    Return _FEC_CREACION_W
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

            Public ReadOnly Property EMILIO_CALIDAD() As WhereParameter
                Get
                    If _EMILIO_CALIDAD_W Is Nothing Then
                        _EMILIO_CALIDAD_W = TearOff.EMILIO_CALIDAD
                    End If
                    Return _EMILIO_CALIDAD_W
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

            Private _CODPRO_W As WhereParameter = Nothing
            Private _NOMPROV_W As WhereParameter = Nothing
            Private _RAZON_W As WhereParameter = Nothing
            Private _CIF_W As WhereParameter = Nothing
            Private _DOMICI_W As WhereParameter = Nothing
            Private _DISTRI_W As WhereParameter = Nothing
            Private _LOCALI_W As WhereParameter = Nothing
            Private _PROVIN_W As WhereParameter = Nothing
            Private _CODPAI_W As WhereParameter = Nothing
            Private _CODMON_W As WhereParameter = Nothing
            Private _PREPME_W As WhereParameter = Nothing
            Private _HABPOT_W As WhereParameter = Nothing
            Private _TIPIVA_W As WhereParameter = Nothing
            Private _PORSIST_W As WhereParameter = Nothing
            Private _PORTROQ_W As WhereParameter = Nothing
            Private _FORPAG_W As WhereParameter = Nothing
            Private _NUMCAR_W As WhereParameter = Nothing
            Private _HOMOLOGADO_W As WhereParameter = Nothing
            Private _TELEFO_W As WhereParameter = Nothing
            Private _NUMFAX_W As WhereParameter = Nothing
            Private _CONTAC_W As WhereParameter = Nothing
            Private _DEPART_W As WhereParameter = Nothing
            Private _CLASIFI_W As WhereParameter = Nothing
            Private _EMILIO_W As WhereParameter = Nothing
            Private _PUNTOS_SC_W As WhereParameter = Nothing
            Private _OBSCAL_W As WhereParameter = Nothing
            Private _FECHOMO_W As WhereParameter = Nothing
            Private _ENCALIDAD_W As WhereParameter = Nothing
            Private _DOMICILIA_W As WhereParameter = Nothing
            Private _CONDICOMPRA_W As WhereParameter = Nothing
            Private _MEDIOAMBIENTAL_W As WhereParameter = Nothing
            Private _FECENVAMB_W As WhereParameter = Nothing
            Private _FECRECAMB_W As WhereParameter = Nothing
            Private _PRODUCTIVO_W As WhereParameter = Nothing
            Private _DOCUM_W As WhereParameter = Nothing
            Private _OBSERV_W As WhereParameter = Nothing
            Private _SULOK_W As WhereParameter = Nothing
            Private _EMILIO1_W As WhereParameter = Nothing
            Private _FECHA_CADUC_W As WhereParameter = Nothing
            Private _FEC_CREACION_W As WhereParameter = Nothing
            Private _CODPER_W As WhereParameter = Nothing
            Private _EMILIO_CALIDAD_W As WhereParameter = Nothing
            Private _PORTES_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _CODPRO_W = Nothing
                _NOMPROV_W = Nothing
                _RAZON_W = Nothing
                _CIF_W = Nothing
                _DOMICI_W = Nothing
                _DISTRI_W = Nothing
                _LOCALI_W = Nothing
                _PROVIN_W = Nothing
                _CODPAI_W = Nothing
                _CODMON_W = Nothing
                _PREPME_W = Nothing
                _HABPOT_W = Nothing
                _TIPIVA_W = Nothing
                _PORSIST_W = Nothing
                _PORTROQ_W = Nothing
                _FORPAG_W = Nothing
                _NUMCAR_W = Nothing
                _HOMOLOGADO_W = Nothing
                _TELEFO_W = Nothing
                _NUMFAX_W = Nothing
                _CONTAC_W = Nothing
                _DEPART_W = Nothing
                _CLASIFI_W = Nothing
                _EMILIO_W = Nothing
                _PUNTOS_SC_W = Nothing
                _OBSCAL_W = Nothing
                _FECHOMO_W = Nothing
                _ENCALIDAD_W = Nothing
                _DOMICILIA_W = Nothing
                _CONDICOMPRA_W = Nothing
                _MEDIOAMBIENTAL_W = Nothing
                _FECENVAMB_W = Nothing
                _FECRECAMB_W = Nothing
                _PRODUCTIVO_W = Nothing
                _DOCUM_W = Nothing
                _OBSERV_W = Nothing
                _SULOK_W = Nothing
                _EMILIO1_W = Nothing
                _FECHA_CADUC_W = Nothing
                _FEC_CREACION_W = Nothing
                _CODPER_W = Nothing
                _EMILIO_CALIDAD_W = Nothing
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


                Public ReadOnly Property CODPRO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CODPRO, Parameters.CODPRO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NOMPROV() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NOMPROV, Parameters.NOMPROV)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property RAZON() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.RAZON, Parameters.RAZON)
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

                Public ReadOnly Property DOMICI() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DOMICI, Parameters.DOMICI)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DISTRI() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DISTRI, Parameters.DISTRI)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property LOCALI() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.LOCALI, Parameters.LOCALI)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PROVIN() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.PROVIN, Parameters.PROVIN)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODPAI() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CODPAI, Parameters.CODPAI)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODMON() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CODMON, Parameters.CODMON)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PREPME() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.PREPME, Parameters.PREPME)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property HABPOT() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.HABPOT, Parameters.HABPOT)
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

                Public ReadOnly Property PORSIST() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.PORSIST, Parameters.PORSIST)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PORTROQ() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.PORTROQ, Parameters.PORTROQ)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FORPAG() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FORPAG, Parameters.FORPAG)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMCAR() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMCAR, Parameters.NUMCAR)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property HOMOLOGADO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.HOMOLOGADO, Parameters.HOMOLOGADO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TELEFO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TELEFO, Parameters.TELEFO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMFAX() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMFAX, Parameters.NUMFAX)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CONTAC() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CONTAC, Parameters.CONTAC)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DEPART() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DEPART, Parameters.DEPART)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CLASIFI() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CLASIFI, Parameters.CLASIFI)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property EMILIO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.EMILIO, Parameters.EMILIO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PUNTOS_SC() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.PUNTOS_SC, Parameters.PUNTOS_SC)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property OBSCAL() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.OBSCAL, Parameters.OBSCAL)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECHOMO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECHOMO, Parameters.FECHOMO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ENCALIDAD() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ENCALIDAD, Parameters.ENCALIDAD)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DOMICILIA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DOMICILIA, Parameters.DOMICILIA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CONDICOMPRA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CONDICOMPRA, Parameters.CONDICOMPRA)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property MEDIOAMBIENTAL() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.MEDIOAMBIENTAL, Parameters.MEDIOAMBIENTAL)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECENVAMB() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECENVAMB, Parameters.FECENVAMB)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECRECAMB() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECRECAMB, Parameters.FECRECAMB)
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

                Public ReadOnly Property DOCUM() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DOCUM, Parameters.DOCUM)
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

                Public ReadOnly Property SULOK() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.SULOK, Parameters.SULOK)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property EMILIO1() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.EMILIO1, Parameters.EMILIO1)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECHA_CADUC() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECHA_CADUC, Parameters.FECHA_CADUC)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FEC_CREACION() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FEC_CREACION, Parameters.FEC_CREACION)
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

                Public ReadOnly Property EMILIO_CALIDAD() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.EMILIO_CALIDAD, Parameters.EMILIO_CALIDAD)
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

            Public ReadOnly Property CODPRO() As AggregateParameter
                Get
                    If _CODPRO_W Is Nothing Then
                        _CODPRO_W = TearOff.CODPRO
                    End If
                    Return _CODPRO_W
                End Get
            End Property

            Public ReadOnly Property NOMPROV() As AggregateParameter
                Get
                    If _NOMPROV_W Is Nothing Then
                        _NOMPROV_W = TearOff.NOMPROV
                    End If
                    Return _NOMPROV_W
                End Get
            End Property

            Public ReadOnly Property RAZON() As AggregateParameter
                Get
                    If _RAZON_W Is Nothing Then
                        _RAZON_W = TearOff.RAZON
                    End If
                    Return _RAZON_W
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

            Public ReadOnly Property DOMICI() As AggregateParameter
                Get
                    If _DOMICI_W Is Nothing Then
                        _DOMICI_W = TearOff.DOMICI
                    End If
                    Return _DOMICI_W
                End Get
            End Property

            Public ReadOnly Property DISTRI() As AggregateParameter
                Get
                    If _DISTRI_W Is Nothing Then
                        _DISTRI_W = TearOff.DISTRI
                    End If
                    Return _DISTRI_W
                End Get
            End Property

            Public ReadOnly Property LOCALI() As AggregateParameter
                Get
                    If _LOCALI_W Is Nothing Then
                        _LOCALI_W = TearOff.LOCALI
                    End If
                    Return _LOCALI_W
                End Get
            End Property

            Public ReadOnly Property PROVIN() As AggregateParameter
                Get
                    If _PROVIN_W Is Nothing Then
                        _PROVIN_W = TearOff.PROVIN
                    End If
                    Return _PROVIN_W
                End Get
            End Property

            Public ReadOnly Property CODPAI() As AggregateParameter
                Get
                    If _CODPAI_W Is Nothing Then
                        _CODPAI_W = TearOff.CODPAI
                    End If
                    Return _CODPAI_W
                End Get
            End Property

            Public ReadOnly Property CODMON() As AggregateParameter
                Get
                    If _CODMON_W Is Nothing Then
                        _CODMON_W = TearOff.CODMON
                    End If
                    Return _CODMON_W
                End Get
            End Property

            Public ReadOnly Property PREPME() As AggregateParameter
                Get
                    If _PREPME_W Is Nothing Then
                        _PREPME_W = TearOff.PREPME
                    End If
                    Return _PREPME_W
                End Get
            End Property

            Public ReadOnly Property HABPOT() As AggregateParameter
                Get
                    If _HABPOT_W Is Nothing Then
                        _HABPOT_W = TearOff.HABPOT
                    End If
                    Return _HABPOT_W
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

            Public ReadOnly Property PORSIST() As AggregateParameter
                Get
                    If _PORSIST_W Is Nothing Then
                        _PORSIST_W = TearOff.PORSIST
                    End If
                    Return _PORSIST_W
                End Get
            End Property

            Public ReadOnly Property PORTROQ() As AggregateParameter
                Get
                    If _PORTROQ_W Is Nothing Then
                        _PORTROQ_W = TearOff.PORTROQ
                    End If
                    Return _PORTROQ_W
                End Get
            End Property

            Public ReadOnly Property FORPAG() As AggregateParameter
                Get
                    If _FORPAG_W Is Nothing Then
                        _FORPAG_W = TearOff.FORPAG
                    End If
                    Return _FORPAG_W
                End Get
            End Property

            Public ReadOnly Property NUMCAR() As AggregateParameter
                Get
                    If _NUMCAR_W Is Nothing Then
                        _NUMCAR_W = TearOff.NUMCAR
                    End If
                    Return _NUMCAR_W
                End Get
            End Property

            Public ReadOnly Property HOMOLOGADO() As AggregateParameter
                Get
                    If _HOMOLOGADO_W Is Nothing Then
                        _HOMOLOGADO_W = TearOff.HOMOLOGADO
                    End If
                    Return _HOMOLOGADO_W
                End Get
            End Property

            Public ReadOnly Property TELEFO() As AggregateParameter
                Get
                    If _TELEFO_W Is Nothing Then
                        _TELEFO_W = TearOff.TELEFO
                    End If
                    Return _TELEFO_W
                End Get
            End Property

            Public ReadOnly Property NUMFAX() As AggregateParameter
                Get
                    If _NUMFAX_W Is Nothing Then
                        _NUMFAX_W = TearOff.NUMFAX
                    End If
                    Return _NUMFAX_W
                End Get
            End Property

            Public ReadOnly Property CONTAC() As AggregateParameter
                Get
                    If _CONTAC_W Is Nothing Then
                        _CONTAC_W = TearOff.CONTAC
                    End If
                    Return _CONTAC_W
                End Get
            End Property

            Public ReadOnly Property DEPART() As AggregateParameter
                Get
                    If _DEPART_W Is Nothing Then
                        _DEPART_W = TearOff.DEPART
                    End If
                    Return _DEPART_W
                End Get
            End Property

            Public ReadOnly Property CLASIFI() As AggregateParameter
                Get
                    If _CLASIFI_W Is Nothing Then
                        _CLASIFI_W = TearOff.CLASIFI
                    End If
                    Return _CLASIFI_W
                End Get
            End Property

            Public ReadOnly Property EMILIO() As AggregateParameter
                Get
                    If _EMILIO_W Is Nothing Then
                        _EMILIO_W = TearOff.EMILIO
                    End If
                    Return _EMILIO_W
                End Get
            End Property

            Public ReadOnly Property PUNTOS_SC() As AggregateParameter
                Get
                    If _PUNTOS_SC_W Is Nothing Then
                        _PUNTOS_SC_W = TearOff.PUNTOS_SC
                    End If
                    Return _PUNTOS_SC_W
                End Get
            End Property

            Public ReadOnly Property OBSCAL() As AggregateParameter
                Get
                    If _OBSCAL_W Is Nothing Then
                        _OBSCAL_W = TearOff.OBSCAL
                    End If
                    Return _OBSCAL_W
                End Get
            End Property

            Public ReadOnly Property FECHOMO() As AggregateParameter
                Get
                    If _FECHOMO_W Is Nothing Then
                        _FECHOMO_W = TearOff.FECHOMO
                    End If
                    Return _FECHOMO_W
                End Get
            End Property

            Public ReadOnly Property ENCALIDAD() As AggregateParameter
                Get
                    If _ENCALIDAD_W Is Nothing Then
                        _ENCALIDAD_W = TearOff.ENCALIDAD
                    End If
                    Return _ENCALIDAD_W
                End Get
            End Property

            Public ReadOnly Property DOMICILIA() As AggregateParameter
                Get
                    If _DOMICILIA_W Is Nothing Then
                        _DOMICILIA_W = TearOff.DOMICILIA
                    End If
                    Return _DOMICILIA_W
                End Get
            End Property

            Public ReadOnly Property CONDICOMPRA() As AggregateParameter
                Get
                    If _CONDICOMPRA_W Is Nothing Then
                        _CONDICOMPRA_W = TearOff.CONDICOMPRA
                    End If
                    Return _CONDICOMPRA_W
                End Get
            End Property

            Public ReadOnly Property MEDIOAMBIENTAL() As AggregateParameter
                Get
                    If _MEDIOAMBIENTAL_W Is Nothing Then
                        _MEDIOAMBIENTAL_W = TearOff.MEDIOAMBIENTAL
                    End If
                    Return _MEDIOAMBIENTAL_W
                End Get
            End Property

            Public ReadOnly Property FECENVAMB() As AggregateParameter
                Get
                    If _FECENVAMB_W Is Nothing Then
                        _FECENVAMB_W = TearOff.FECENVAMB
                    End If
                    Return _FECENVAMB_W
                End Get
            End Property

            Public ReadOnly Property FECRECAMB() As AggregateParameter
                Get
                    If _FECRECAMB_W Is Nothing Then
                        _FECRECAMB_W = TearOff.FECRECAMB
                    End If
                    Return _FECRECAMB_W
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

            Public ReadOnly Property DOCUM() As AggregateParameter
                Get
                    If _DOCUM_W Is Nothing Then
                        _DOCUM_W = TearOff.DOCUM
                    End If
                    Return _DOCUM_W
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

            Public ReadOnly Property SULOK() As AggregateParameter
                Get
                    If _SULOK_W Is Nothing Then
                        _SULOK_W = TearOff.SULOK
                    End If
                    Return _SULOK_W
                End Get
            End Property

            Public ReadOnly Property EMILIO1() As AggregateParameter
                Get
                    If _EMILIO1_W Is Nothing Then
                        _EMILIO1_W = TearOff.EMILIO1
                    End If
                    Return _EMILIO1_W
                End Get
            End Property

            Public ReadOnly Property FECHA_CADUC() As AggregateParameter
                Get
                    If _FECHA_CADUC_W Is Nothing Then
                        _FECHA_CADUC_W = TearOff.FECHA_CADUC
                    End If
                    Return _FECHA_CADUC_W
                End Get
            End Property

            Public ReadOnly Property FEC_CREACION() As AggregateParameter
                Get
                    If _FEC_CREACION_W Is Nothing Then
                        _FEC_CREACION_W = TearOff.FEC_CREACION
                    End If
                    Return _FEC_CREACION_W
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

            Public ReadOnly Property EMILIO_CALIDAD() As AggregateParameter
                Get
                    If _EMILIO_CALIDAD_W Is Nothing Then
                        _EMILIO_CALIDAD_W = TearOff.EMILIO_CALIDAD
                    End If
                    Return _EMILIO_CALIDAD_W
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

            Private _CODPRO_W As AggregateParameter = Nothing
            Private _NOMPROV_W As AggregateParameter = Nothing
            Private _RAZON_W As AggregateParameter = Nothing
            Private _CIF_W As AggregateParameter = Nothing
            Private _DOMICI_W As AggregateParameter = Nothing
            Private _DISTRI_W As AggregateParameter = Nothing
            Private _LOCALI_W As AggregateParameter = Nothing
            Private _PROVIN_W As AggregateParameter = Nothing
            Private _CODPAI_W As AggregateParameter = Nothing
            Private _CODMON_W As AggregateParameter = Nothing
            Private _PREPME_W As AggregateParameter = Nothing
            Private _HABPOT_W As AggregateParameter = Nothing
            Private _TIPIVA_W As AggregateParameter = Nothing
            Private _PORSIST_W As AggregateParameter = Nothing
            Private _PORTROQ_W As AggregateParameter = Nothing
            Private _FORPAG_W As AggregateParameter = Nothing
            Private _NUMCAR_W As AggregateParameter = Nothing
            Private _HOMOLOGADO_W As AggregateParameter = Nothing
            Private _TELEFO_W As AggregateParameter = Nothing
            Private _NUMFAX_W As AggregateParameter = Nothing
            Private _CONTAC_W As AggregateParameter = Nothing
            Private _DEPART_W As AggregateParameter = Nothing
            Private _CLASIFI_W As AggregateParameter = Nothing
            Private _EMILIO_W As AggregateParameter = Nothing
            Private _PUNTOS_SC_W As AggregateParameter = Nothing
            Private _OBSCAL_W As AggregateParameter = Nothing
            Private _FECHOMO_W As AggregateParameter = Nothing
            Private _ENCALIDAD_W As AggregateParameter = Nothing
            Private _DOMICILIA_W As AggregateParameter = Nothing
            Private _CONDICOMPRA_W As AggregateParameter = Nothing
            Private _MEDIOAMBIENTAL_W As AggregateParameter = Nothing
            Private _FECENVAMB_W As AggregateParameter = Nothing
            Private _FECRECAMB_W As AggregateParameter = Nothing
            Private _PRODUCTIVO_W As AggregateParameter = Nothing
            Private _DOCUM_W As AggregateParameter = Nothing
            Private _OBSERV_W As AggregateParameter = Nothing
            Private _SULOK_W As AggregateParameter = Nothing
            Private _EMILIO1_W As AggregateParameter = Nothing
            Private _FECHA_CADUC_W As AggregateParameter = Nothing
            Private _FEC_CREACION_W As AggregateParameter = Nothing
            Private _CODPER_W As AggregateParameter = Nothing
            Private _EMILIO_CALIDAD_W As AggregateParameter = Nothing
            Private _PORTES_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _CODPRO_W = Nothing
                _NOMPROV_W = Nothing
                _RAZON_W = Nothing
                _CIF_W = Nothing
                _DOMICI_W = Nothing
                _DISTRI_W = Nothing
                _LOCALI_W = Nothing
                _PROVIN_W = Nothing
                _CODPAI_W = Nothing
                _CODMON_W = Nothing
                _PREPME_W = Nothing
                _HABPOT_W = Nothing
                _TIPIVA_W = Nothing
                _PORSIST_W = Nothing
                _PORTROQ_W = Nothing
                _FORPAG_W = Nothing
                _NUMCAR_W = Nothing
                _HOMOLOGADO_W = Nothing
                _TELEFO_W = Nothing
                _NUMFAX_W = Nothing
                _CONTAC_W = Nothing
                _DEPART_W = Nothing
                _CLASIFI_W = Nothing
                _EMILIO_W = Nothing
                _PUNTOS_SC_W = Nothing
                _OBSCAL_W = Nothing
                _FECHOMO_W = Nothing
                _ENCALIDAD_W = Nothing
                _DOMICILIA_W = Nothing
                _CONDICOMPRA_W = Nothing
                _MEDIOAMBIENTAL_W = Nothing
                _FECENVAMB_W = Nothing
                _FECRECAMB_W = Nothing
                _PRODUCTIVO_W = Nothing
                _DOCUM_W = Nothing
                _OBSERV_W = Nothing
                _SULOK_W = Nothing
                _EMILIO1_W = Nothing
                _FECHA_CADUC_W = Nothing
                _FEC_CREACION_W = Nothing
                _CODPER_W = Nothing
                _EMILIO_CALIDAD_W = Nothing
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

