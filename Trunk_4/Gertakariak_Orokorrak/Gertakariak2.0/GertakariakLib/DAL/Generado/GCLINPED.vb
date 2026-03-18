
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

    Public MustInherit Class _GCLINPED
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "XBAT."
            Me.QuerySource = "GCLINPED"
            Me.MappingName = "GCLINPED"
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


            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_GCLINPED", parameters)
        End Function

        '=================================================================
        ' Public Overridable Function LoadByPrimaryKey()  As Boolean
        '=================================================================
        '  Loads a single row of via the primary key
        '=================================================================
        Public Overridable Function LoadByPrimaryKey(ByVal NUMLINLIN As Decimal, ByVal NUMPEDLIN As Decimal) As Boolean

            Dim parameters As ListDictionary = New ListDictionary()
            parameters.Add(_GCLINPED.Parameters.NUMLINLIN, NUMLINLIN)

            parameters.Add(_GCLINPED.Parameters.NUMPEDLIN, NUMPEDLIN)


            Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
            p.Direction = ParameterDirection.Output
            parameters.Add(p, Nothing)

            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_GCLINPED", parameters)

        End Function

#Region "Parameters"
        Protected Class Parameters

            Public Shared ReadOnly Property NUMPEDLIN As OracleParameter
                Get
                    Return New OracleParameter("p_NUMPEDLIN", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property NUMLINLIN As OracleParameter
                Get
                    Return New OracleParameter("p_NUMLINLIN", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property CODPROLIN As OracleParameter
                Get
                    Return New OracleParameter("p_CODPROLIN", OracleDbType.Char, 12)
                End Get
            End Property

            Public Shared ReadOnly Property CODART As OracleParameter
                Get
                    Return New OracleParameter("p_CODART", OracleDbType.Char, 12)
                End Get
            End Property

            Public Shared ReadOnly Property DESCART As OracleParameter
                Get
                    Return New OracleParameter("p_DESCART", OracleDbType.Char, 30)
                End Get
            End Property

            Public Shared ReadOnly Property DESCLISTA As OracleParameter
                Get
                    Return New OracleParameter("p_DESCLISTA", OracleDbType.Char, 20)
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

            Public Shared ReadOnly Property NUMMAR As OracleParameter
                Get
                    Return New OracleParameter("p_NUMMAR", OracleDbType.Char, 11)
                End Get
            End Property

            Public Shared ReadOnly Property CANPED As OracleParameter
                Get
                    Return New OracleParameter("p_CANPED", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property CANREC As OracleParameter
                Get
                    Return New OracleParameter("p_CANREC", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property CANFAC As OracleParameter
                Get
                    Return New OracleParameter("p_CANFAC", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property PREUNI As OracleParameter
                Get
                    Return New OracleParameter("p_PREUNI", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property DESCTO As OracleParameter
                Get
                    Return New OracleParameter("p_DESCTO", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property ALEACION As OracleParameter
                Get
                    Return New OracleParameter("p_ALEACION", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property IMPPED As OracleParameter
                Get
                    Return New OracleParameter("p_IMPPED", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property IMPREC As OracleParameter
                Get
                    Return New OracleParameter("p_IMPREC", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property IMPFAC As OracleParameter
                Get
                    Return New OracleParameter("p_IMPFAC", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property FECENTSOL As OracleParameter
                Get
                    Return New OracleParameter("p_FECENTSOL", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property FECENTVIG As OracleParameter
                Get
                    Return New OracleParameter("p_FECENTVIG", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property CODARPRO As OracleParameter
                Get
                    Return New OracleParameter("p_CODARPRO", OracleDbType.Char, 15)
                End Get
            End Property

            Public Shared ReadOnly Property NUMNECES As OracleParameter
                Get
                    Return New OracleParameter("p_NUMNECES", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property PDTE_PREC As OracleParameter
                Get
                    Return New OracleParameter("p_PDTE_PREC", OracleDbType.Char, 1)
                End Get
            End Property

            Public Shared ReadOnly Property LANTEGI As OracleParameter
                Get
                    Return New OracleParameter("p_LANTEGI", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property LANTEGI_H As OracleParameter
                Get
                    Return New OracleParameter("p_LANTEGI_H", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property EPREUNI As OracleParameter
                Get
                    Return New OracleParameter("p_EPREUNI", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property EDIMPRE As OracleParameter
                Get
                    Return New OracleParameter("p_EDIMPRE", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property EALEACION As OracleParameter
                Get
                    Return New OracleParameter("p_EALEACION", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property EIMPPED As OracleParameter
                Get
                    Return New OracleParameter("p_EIMPPED", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property EIMPREC As OracleParameter
                Get
                    Return New OracleParameter("p_EIMPREC", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property EIMPFAC As OracleParameter
                Get
                    Return New OracleParameter("p_EIMPFAC", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property FECPDTEPRE As OracleParameter
                Get
                    Return New OracleParameter("p_FECPDTEPRE", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property FECCALIDAD As OracleParameter
                Get
                    Return New OracleParameter("p_FECCALIDAD", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property ID_ESTADO As OracleParameter
                Get
                    Return New OracleParameter("p_ID_ESTADO", OracleDbType.Int32, 0)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const NUMPEDLIN As String = "NUMPEDLIN"
            Public Const NUMLINLIN As String = "NUMLINLIN"
            Public Const CODPROLIN As String = "CODPROLIN"
            Public Const CODART As String = "CODART"
            Public Const DESCART As String = "DESCART"
            Public Const DESCLISTA As String = "DESCLISTA"
            Public Const NUMORDF As String = "NUMORDF"
            Public Const NUMOPE As String = "NUMOPE"
            Public Const NUMMAR As String = "NUMMAR"
            Public Const CANPED As String = "CANPED"
            Public Const CANREC As String = "CANREC"
            Public Const CANFAC As String = "CANFAC"
            Public Const PREUNI As String = "PREUNI"
            Public Const DESCTO As String = "DESCTO"
            Public Const ALEACION As String = "ALEACION"
            Public Const IMPPED As String = "IMPPED"
            Public Const IMPREC As String = "IMPREC"
            Public Const IMPFAC As String = "IMPFAC"
            Public Const FECENTSOL As String = "FECENTSOL"
            Public Const FECENTVIG As String = "FECENTVIG"
            Public Const CODARPRO As String = "CODARPRO"
            Public Const NUMNECES As String = "NUMNECES"
            Public Const PDTE_PREC As String = "PDTE_PREC"
            Public Const LANTEGI As String = "LANTEGI"
            Public Const LANTEGI_H As String = "LANTEGI_H"
            Public Const EPREUNI As String = "EPREUNI"
            Public Const EDIMPRE As String = "EDIMPRE"
            Public Const EALEACION As String = "EALEACION"
            Public Const EIMPPED As String = "EIMPPED"
            Public Const EIMPREC As String = "EIMPREC"
            Public Const EIMPFAC As String = "EIMPFAC"
            Public Const FECPDTEPRE As String = "FECPDTEPRE"
            Public Const FECCALIDAD As String = "FECCALIDAD"
            Public Const ID_ESTADO As String = "ID_ESTADO"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(NUMPEDLIN) = _GCLINPED.PropertyNames.NUMPEDLIN
                    ht(NUMLINLIN) = _GCLINPED.PropertyNames.NUMLINLIN
                    ht(CODPROLIN) = _GCLINPED.PropertyNames.CODPROLIN
                    ht(CODART) = _GCLINPED.PropertyNames.CODART
                    ht(DESCART) = _GCLINPED.PropertyNames.DESCART
                    ht(DESCLISTA) = _GCLINPED.PropertyNames.DESCLISTA
                    ht(NUMORDF) = _GCLINPED.PropertyNames.NUMORDF
                    ht(NUMOPE) = _GCLINPED.PropertyNames.NUMOPE
                    ht(NUMMAR) = _GCLINPED.PropertyNames.NUMMAR
                    ht(CANPED) = _GCLINPED.PropertyNames.CANPED
                    ht(CANREC) = _GCLINPED.PropertyNames.CANREC
                    ht(CANFAC) = _GCLINPED.PropertyNames.CANFAC
                    ht(PREUNI) = _GCLINPED.PropertyNames.PREUNI
                    ht(DESCTO) = _GCLINPED.PropertyNames.DESCTO
                    ht(ALEACION) = _GCLINPED.PropertyNames.ALEACION
                    ht(IMPPED) = _GCLINPED.PropertyNames.IMPPED
                    ht(IMPREC) = _GCLINPED.PropertyNames.IMPREC
                    ht(IMPFAC) = _GCLINPED.PropertyNames.IMPFAC
                    ht(FECENTSOL) = _GCLINPED.PropertyNames.FECENTSOL
                    ht(FECENTVIG) = _GCLINPED.PropertyNames.FECENTVIG
                    ht(CODARPRO) = _GCLINPED.PropertyNames.CODARPRO
                    ht(NUMNECES) = _GCLINPED.PropertyNames.NUMNECES
                    ht(PDTE_PREC) = _GCLINPED.PropertyNames.PDTE_PREC
                    ht(LANTEGI) = _GCLINPED.PropertyNames.LANTEGI
                    ht(LANTEGI_H) = _GCLINPED.PropertyNames.LANTEGI_H
                    ht(EPREUNI) = _GCLINPED.PropertyNames.EPREUNI
                    ht(EDIMPRE) = _GCLINPED.PropertyNames.EDIMPRE
                    ht(EALEACION) = _GCLINPED.PropertyNames.EALEACION
                    ht(EIMPPED) = _GCLINPED.PropertyNames.EIMPPED
                    ht(EIMPREC) = _GCLINPED.PropertyNames.EIMPREC
                    ht(EIMPFAC) = _GCLINPED.PropertyNames.EIMPFAC
                    ht(FECPDTEPRE) = _GCLINPED.PropertyNames.FECPDTEPRE
                    ht(FECCALIDAD) = _GCLINPED.PropertyNames.FECCALIDAD
                    ht(ID_ESTADO) = _GCLINPED.PropertyNames.ID_ESTADO

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const NUMPEDLIN As String = "NUMPEDLIN"
            Public Const NUMLINLIN As String = "NUMLINLIN"
            Public Const CODPROLIN As String = "CODPROLIN"
            Public Const CODART As String = "CODART"
            Public Const DESCART As String = "DESCART"
            Public Const DESCLISTA As String = "DESCLISTA"
            Public Const NUMORDF As String = "NUMORDF"
            Public Const NUMOPE As String = "NUMOPE"
            Public Const NUMMAR As String = "NUMMAR"
            Public Const CANPED As String = "CANPED"
            Public Const CANREC As String = "CANREC"
            Public Const CANFAC As String = "CANFAC"
            Public Const PREUNI As String = "PREUNI"
            Public Const DESCTO As String = "DESCTO"
            Public Const ALEACION As String = "ALEACION"
            Public Const IMPPED As String = "IMPPED"
            Public Const IMPREC As String = "IMPREC"
            Public Const IMPFAC As String = "IMPFAC"
            Public Const FECENTSOL As String = "FECENTSOL"
            Public Const FECENTVIG As String = "FECENTVIG"
            Public Const CODARPRO As String = "CODARPRO"
            Public Const NUMNECES As String = "NUMNECES"
            Public Const PDTE_PREC As String = "PDTE_PREC"
            Public Const LANTEGI As String = "LANTEGI"
            Public Const LANTEGI_H As String = "LANTEGI_H"
            Public Const EPREUNI As String = "EPREUNI"
            Public Const EDIMPRE As String = "EDIMPRE"
            Public Const EALEACION As String = "EALEACION"
            Public Const EIMPPED As String = "EIMPPED"
            Public Const EIMPREC As String = "EIMPREC"
            Public Const EIMPFAC As String = "EIMPFAC"
            Public Const FECPDTEPRE As String = "FECPDTEPRE"
            Public Const FECCALIDAD As String = "FECCALIDAD"
            Public Const ID_ESTADO As String = "ID_ESTADO"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(NUMPEDLIN) = _GCLINPED.ColumnNames.NUMPEDLIN
                    ht(NUMLINLIN) = _GCLINPED.ColumnNames.NUMLINLIN
                    ht(CODPROLIN) = _GCLINPED.ColumnNames.CODPROLIN
                    ht(CODART) = _GCLINPED.ColumnNames.CODART
                    ht(DESCART) = _GCLINPED.ColumnNames.DESCART
                    ht(DESCLISTA) = _GCLINPED.ColumnNames.DESCLISTA
                    ht(NUMORDF) = _GCLINPED.ColumnNames.NUMORDF
                    ht(NUMOPE) = _GCLINPED.ColumnNames.NUMOPE
                    ht(NUMMAR) = _GCLINPED.ColumnNames.NUMMAR
                    ht(CANPED) = _GCLINPED.ColumnNames.CANPED
                    ht(CANREC) = _GCLINPED.ColumnNames.CANREC
                    ht(CANFAC) = _GCLINPED.ColumnNames.CANFAC
                    ht(PREUNI) = _GCLINPED.ColumnNames.PREUNI
                    ht(DESCTO) = _GCLINPED.ColumnNames.DESCTO
                    ht(ALEACION) = _GCLINPED.ColumnNames.ALEACION
                    ht(IMPPED) = _GCLINPED.ColumnNames.IMPPED
                    ht(IMPREC) = _GCLINPED.ColumnNames.IMPREC
                    ht(IMPFAC) = _GCLINPED.ColumnNames.IMPFAC
                    ht(FECENTSOL) = _GCLINPED.ColumnNames.FECENTSOL
                    ht(FECENTVIG) = _GCLINPED.ColumnNames.FECENTVIG
                    ht(CODARPRO) = _GCLINPED.ColumnNames.CODARPRO
                    ht(NUMNECES) = _GCLINPED.ColumnNames.NUMNECES
                    ht(PDTE_PREC) = _GCLINPED.ColumnNames.PDTE_PREC
                    ht(LANTEGI) = _GCLINPED.ColumnNames.LANTEGI
                    ht(LANTEGI_H) = _GCLINPED.ColumnNames.LANTEGI_H
                    ht(EPREUNI) = _GCLINPED.ColumnNames.EPREUNI
                    ht(EDIMPRE) = _GCLINPED.ColumnNames.EDIMPRE
                    ht(EALEACION) = _GCLINPED.ColumnNames.EALEACION
                    ht(EIMPPED) = _GCLINPED.ColumnNames.EIMPPED
                    ht(EIMPREC) = _GCLINPED.ColumnNames.EIMPREC
                    ht(EIMPFAC) = _GCLINPED.ColumnNames.EIMPFAC
                    ht(FECPDTEPRE) = _GCLINPED.ColumnNames.FECPDTEPRE
                    ht(FECCALIDAD) = _GCLINPED.ColumnNames.FECCALIDAD
                    ht(ID_ESTADO) = _GCLINPED.ColumnNames.ID_ESTADO

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const NUMPEDLIN As String = "s_NUMPEDLIN"
            Public Const NUMLINLIN As String = "s_NUMLINLIN"
            Public Const CODPROLIN As String = "s_CODPROLIN"
            Public Const CODART As String = "s_CODART"
            Public Const DESCART As String = "s_DESCART"
            Public Const DESCLISTA As String = "s_DESCLISTA"
            Public Const NUMORDF As String = "s_NUMORDF"
            Public Const NUMOPE As String = "s_NUMOPE"
            Public Const NUMMAR As String = "s_NUMMAR"
            Public Const CANPED As String = "s_CANPED"
            Public Const CANREC As String = "s_CANREC"
            Public Const CANFAC As String = "s_CANFAC"
            Public Const PREUNI As String = "s_PREUNI"
            Public Const DESCTO As String = "s_DESCTO"
            Public Const ALEACION As String = "s_ALEACION"
            Public Const IMPPED As String = "s_IMPPED"
            Public Const IMPREC As String = "s_IMPREC"
            Public Const IMPFAC As String = "s_IMPFAC"
            Public Const FECENTSOL As String = "s_FECENTSOL"
            Public Const FECENTVIG As String = "s_FECENTVIG"
            Public Const CODARPRO As String = "s_CODARPRO"
            Public Const NUMNECES As String = "s_NUMNECES"
            Public Const PDTE_PREC As String = "s_PDTE_PREC"
            Public Const LANTEGI As String = "s_LANTEGI"
            Public Const LANTEGI_H As String = "s_LANTEGI_H"
            Public Const EPREUNI As String = "s_EPREUNI"
            Public Const EDIMPRE As String = "s_EDIMPRE"
            Public Const EALEACION As String = "s_EALEACION"
            Public Const EIMPPED As String = "s_EIMPPED"
            Public Const EIMPREC As String = "s_EIMPREC"
            Public Const EIMPFAC As String = "s_EIMPFAC"
            Public Const FECPDTEPRE As String = "s_FECPDTEPRE"
            Public Const FECCALIDAD As String = "s_FECCALIDAD"
            Public Const ID_ESTADO As String = "s_ID_ESTADO"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property NUMPEDLIN As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.NUMPEDLIN)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.NUMPEDLIN, Value)
            End Set
        End Property

        Public Overridable Property NUMLINLIN As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.NUMLINLIN)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.NUMLINLIN, Value)
            End Set
        End Property

        Public Overridable Property CODPROLIN As String
            Get
                Return MyBase.GetString(ColumnNames.CODPROLIN)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CODPROLIN, Value)
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

        Public Overridable Property DESCART As String
            Get
                Return MyBase.GetString(ColumnNames.DESCART)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.DESCART, Value)
            End Set
        End Property

        Public Overridable Property DESCLISTA As String
            Get
                Return MyBase.GetString(ColumnNames.DESCLISTA)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.DESCLISTA, Value)
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

        Public Overridable Property NUMMAR As String
            Get
                Return MyBase.GetString(ColumnNames.NUMMAR)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.NUMMAR, Value)
            End Set
        End Property

        Public Overridable Property CANPED As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.CANPED)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.CANPED, Value)
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

        Public Overridable Property CANFAC As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.CANFAC)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.CANFAC, Value)
            End Set
        End Property

        Public Overridable Property PREUNI As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.PREUNI)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.PREUNI, Value)
            End Set
        End Property

        Public Overridable Property DESCTO As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.DESCTO)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.DESCTO, Value)
            End Set
        End Property

        Public Overridable Property ALEACION As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.ALEACION)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.ALEACION, Value)
            End Set
        End Property

        Public Overridable Property IMPPED As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IMPPED)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IMPPED, Value)
            End Set
        End Property

        Public Overridable Property IMPREC As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IMPREC)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IMPREC, Value)
            End Set
        End Property

        Public Overridable Property IMPFAC As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.IMPFAC)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.IMPFAC, Value)
            End Set
        End Property

        Public Overridable Property FECENTSOL As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECENTSOL)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECENTSOL, Value)
            End Set
        End Property

        Public Overridable Property FECENTVIG As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECENTVIG)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECENTVIG, Value)
            End Set
        End Property

        Public Overridable Property CODARPRO As String
            Get
                Return MyBase.GetString(ColumnNames.CODARPRO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CODARPRO, Value)
            End Set
        End Property

        Public Overridable Property NUMNECES As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.NUMNECES)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.NUMNECES, Value)
            End Set
        End Property

        Public Overridable Property PDTE_PREC As String
            Get
                Return MyBase.GetString(ColumnNames.PDTE_PREC)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.PDTE_PREC, Value)
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

        Public Overridable Property LANTEGI_H As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.LANTEGI_H)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.LANTEGI_H, Value)
            End Set
        End Property

        Public Overridable Property EPREUNI As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.EPREUNI)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.EPREUNI, Value)
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

        Public Overridable Property EALEACION As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.EALEACION)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.EALEACION, Value)
            End Set
        End Property

        Public Overridable Property EIMPPED As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.EIMPPED)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.EIMPPED, Value)
            End Set
        End Property

        Public Overridable Property EIMPREC As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.EIMPREC)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.EIMPREC, Value)
            End Set
        End Property

        Public Overridable Property EIMPFAC As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.EIMPFAC)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.EIMPFAC, Value)
            End Set
        End Property

        Public Overridable Property FECPDTEPRE As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECPDTEPRE)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECPDTEPRE, Value)
            End Set
        End Property

        Public Overridable Property FECCALIDAD As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECCALIDAD)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECCALIDAD, Value)
            End Set
        End Property

        Public Overridable Property ID_ESTADO As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.ID_ESTADO)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.ID_ESTADO, Value)
            End Set
        End Property


#End Region

#Region "String Properties"

        Public Overridable Property s_NUMPEDLIN As String
            Get
                If Me.IsColumnNull(ColumnNames.NUMPEDLIN) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.NUMPEDLIN)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NUMPEDLIN)
                Else
                    Me.NUMPEDLIN = MyBase.SetDecimalAsString(ColumnNames.NUMPEDLIN, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_NUMLINLIN As String
            Get
                If Me.IsColumnNull(ColumnNames.NUMLINLIN) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.NUMLINLIN)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NUMLINLIN)
                Else
                    Me.NUMLINLIN = MyBase.SetDecimalAsString(ColumnNames.NUMLINLIN, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CODPROLIN As String
            Get
                If Me.IsColumnNull(ColumnNames.CODPROLIN) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CODPROLIN)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CODPROLIN)
                Else
                    Me.CODPROLIN = MyBase.SetStringAsString(ColumnNames.CODPROLIN, Value)
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

        Public Overridable Property s_DESCART As String
            Get
                If Me.IsColumnNull(ColumnNames.DESCART) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.DESCART)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.DESCART)
                Else
                    Me.DESCART = MyBase.SetStringAsString(ColumnNames.DESCART, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_DESCLISTA As String
            Get
                If Me.IsColumnNull(ColumnNames.DESCLISTA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.DESCLISTA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.DESCLISTA)
                Else
                    Me.DESCLISTA = MyBase.SetStringAsString(ColumnNames.DESCLISTA, Value)
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

        Public Overridable Property s_CANPED As String
            Get
                If Me.IsColumnNull(ColumnNames.CANPED) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.CANPED)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CANPED)
                Else
                    Me.CANPED = MyBase.SetDecimalAsString(ColumnNames.CANPED, Value)
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

        Public Overridable Property s_CANFAC As String
            Get
                If Me.IsColumnNull(ColumnNames.CANFAC) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.CANFAC)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CANFAC)
                Else
                    Me.CANFAC = MyBase.SetDecimalAsString(ColumnNames.CANFAC, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_PREUNI As String
            Get
                If Me.IsColumnNull(ColumnNames.PREUNI) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.PREUNI)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.PREUNI)
                Else
                    Me.PREUNI = MyBase.SetDecimalAsString(ColumnNames.PREUNI, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_DESCTO As String
            Get
                If Me.IsColumnNull(ColumnNames.DESCTO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.DESCTO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.DESCTO)
                Else
                    Me.DESCTO = MyBase.SetDecimalAsString(ColumnNames.DESCTO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_ALEACION As String
            Get
                If Me.IsColumnNull(ColumnNames.ALEACION) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.ALEACION)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.ALEACION)
                Else
                    Me.ALEACION = MyBase.SetDecimalAsString(ColumnNames.ALEACION, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_IMPPED As String
            Get
                If Me.IsColumnNull(ColumnNames.IMPPED) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.IMPPED)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IMPPED)
                Else
                    Me.IMPPED = MyBase.SetDecimalAsString(ColumnNames.IMPPED, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_IMPREC As String
            Get
                If Me.IsColumnNull(ColumnNames.IMPREC) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.IMPREC)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IMPREC)
                Else
                    Me.IMPREC = MyBase.SetDecimalAsString(ColumnNames.IMPREC, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_IMPFAC As String
            Get
                If Me.IsColumnNull(ColumnNames.IMPFAC) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.IMPFAC)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.IMPFAC)
                Else
                    Me.IMPFAC = MyBase.SetDecimalAsString(ColumnNames.IMPFAC, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FECENTSOL As String
            Get
                If Me.IsColumnNull(ColumnNames.FECENTSOL) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FECENTSOL)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FECENTSOL)
                Else
                    Me.FECENTSOL = MyBase.SetDateTimeAsString(ColumnNames.FECENTSOL, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FECENTVIG As String
            Get
                If Me.IsColumnNull(ColumnNames.FECENTVIG) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FECENTVIG)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FECENTVIG)
                Else
                    Me.FECENTVIG = MyBase.SetDateTimeAsString(ColumnNames.FECENTVIG, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CODARPRO As String
            Get
                If Me.IsColumnNull(ColumnNames.CODARPRO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CODARPRO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CODARPRO)
                Else
                    Me.CODARPRO = MyBase.SetStringAsString(ColumnNames.CODARPRO, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_NUMNECES As String
            Get
                If Me.IsColumnNull(ColumnNames.NUMNECES) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.NUMNECES)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NUMNECES)
                Else
                    Me.NUMNECES = MyBase.SetDecimalAsString(ColumnNames.NUMNECES, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_PDTE_PREC As String
            Get
                If Me.IsColumnNull(ColumnNames.PDTE_PREC) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.PDTE_PREC)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.PDTE_PREC)
                Else
                    Me.PDTE_PREC = MyBase.SetStringAsString(ColumnNames.PDTE_PREC, Value)
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

        Public Overridable Property s_LANTEGI_H As String
            Get
                If Me.IsColumnNull(ColumnNames.LANTEGI_H) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.LANTEGI_H)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.LANTEGI_H)
                Else
                    Me.LANTEGI_H = MyBase.SetDecimalAsString(ColumnNames.LANTEGI_H, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_EPREUNI As String
            Get
                If Me.IsColumnNull(ColumnNames.EPREUNI) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.EPREUNI)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.EPREUNI)
                Else
                    Me.EPREUNI = MyBase.SetDecimalAsString(ColumnNames.EPREUNI, Value)
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

        Public Overridable Property s_EALEACION As String
            Get
                If Me.IsColumnNull(ColumnNames.EALEACION) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.EALEACION)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.EALEACION)
                Else
                    Me.EALEACION = MyBase.SetDecimalAsString(ColumnNames.EALEACION, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_EIMPPED As String
            Get
                If Me.IsColumnNull(ColumnNames.EIMPPED) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.EIMPPED)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.EIMPPED)
                Else
                    Me.EIMPPED = MyBase.SetDecimalAsString(ColumnNames.EIMPPED, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_EIMPREC As String
            Get
                If Me.IsColumnNull(ColumnNames.EIMPREC) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.EIMPREC)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.EIMPREC)
                Else
                    Me.EIMPREC = MyBase.SetDecimalAsString(ColumnNames.EIMPREC, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_EIMPFAC As String
            Get
                If Me.IsColumnNull(ColumnNames.EIMPFAC) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.EIMPFAC)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.EIMPFAC)
                Else
                    Me.EIMPFAC = MyBase.SetDecimalAsString(ColumnNames.EIMPFAC, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FECPDTEPRE As String
            Get
                If Me.IsColumnNull(ColumnNames.FECPDTEPRE) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FECPDTEPRE)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FECPDTEPRE)
                Else
                    Me.FECPDTEPRE = MyBase.SetDateTimeAsString(ColumnNames.FECPDTEPRE, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_FECCALIDAD As String
            Get
                If Me.IsColumnNull(ColumnNames.FECCALIDAD) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FECCALIDAD)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FECCALIDAD)
                Else
                    Me.FECCALIDAD = MyBase.SetDateTimeAsString(ColumnNames.FECCALIDAD, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_ID_ESTADO As String
            Get
                If Me.IsColumnNull(ColumnNames.ID_ESTADO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.ID_ESTADO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.ID_ESTADO)
                Else
                    Me.ID_ESTADO = MyBase.SetDecimalAsString(ColumnNames.ID_ESTADO, Value)
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


                Public ReadOnly Property NUMPEDLIN() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMPEDLIN, Parameters.NUMPEDLIN)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMLINLIN() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMLINLIN, Parameters.NUMLINLIN)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODPROLIN() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CODPROLIN, Parameters.CODPROLIN)
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

                Public ReadOnly Property DESCART() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.DESCART, Parameters.DESCART)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DESCLISTA() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.DESCLISTA, Parameters.DESCLISTA)
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

                Public ReadOnly Property NUMMAR() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMMAR, Parameters.NUMMAR)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CANPED() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CANPED, Parameters.CANPED)
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

                Public ReadOnly Property CANFAC() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CANFAC, Parameters.CANFAC)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PREUNI() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.PREUNI, Parameters.PREUNI)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DESCTO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.DESCTO, Parameters.DESCTO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ALEACION() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.ALEACION, Parameters.ALEACION)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IMPPED() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IMPPED, Parameters.IMPPED)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IMPREC() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IMPREC, Parameters.IMPREC)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IMPFAC() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.IMPFAC, Parameters.IMPFAC)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECENTSOL() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FECENTSOL, Parameters.FECENTSOL)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECENTVIG() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FECENTVIG, Parameters.FECENTVIG)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODARPRO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CODARPRO, Parameters.CODARPRO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMNECES() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMNECES, Parameters.NUMNECES)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PDTE_PREC() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.PDTE_PREC, Parameters.PDTE_PREC)
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

                Public ReadOnly Property LANTEGI_H() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.LANTEGI_H, Parameters.LANTEGI_H)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property EPREUNI() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.EPREUNI, Parameters.EPREUNI)
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

                Public ReadOnly Property EALEACION() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.EALEACION, Parameters.EALEACION)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property EIMPPED() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.EIMPPED, Parameters.EIMPPED)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property EIMPREC() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.EIMPREC, Parameters.EIMPREC)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property EIMPFAC() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.EIMPFAC, Parameters.EIMPFAC)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECPDTEPRE() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FECPDTEPRE, Parameters.FECPDTEPRE)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECCALIDAD() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FECCALIDAD, Parameters.FECCALIDAD)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ID_ESTADO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_ESTADO, Parameters.ID_ESTADO)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property


                Private _clause As WhereClause
            End Class
#End Region

            Public ReadOnly Property NUMPEDLIN() As WhereParameter
                Get
                    If _NUMPEDLIN_W Is Nothing Then
                        _NUMPEDLIN_W = TearOff.NUMPEDLIN
                    End If
                    Return _NUMPEDLIN_W
                End Get
            End Property

            Public ReadOnly Property NUMLINLIN() As WhereParameter
                Get
                    If _NUMLINLIN_W Is Nothing Then
                        _NUMLINLIN_W = TearOff.NUMLINLIN
                    End If
                    Return _NUMLINLIN_W
                End Get
            End Property

            Public ReadOnly Property CODPROLIN() As WhereParameter
                Get
                    If _CODPROLIN_W Is Nothing Then
                        _CODPROLIN_W = TearOff.CODPROLIN
                    End If
                    Return _CODPROLIN_W
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

            Public ReadOnly Property DESCART() As WhereParameter
                Get
                    If _DESCART_W Is Nothing Then
                        _DESCART_W = TearOff.DESCART
                    End If
                    Return _DESCART_W
                End Get
            End Property

            Public ReadOnly Property DESCLISTA() As WhereParameter
                Get
                    If _DESCLISTA_W Is Nothing Then
                        _DESCLISTA_W = TearOff.DESCLISTA
                    End If
                    Return _DESCLISTA_W
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

            Public ReadOnly Property NUMMAR() As WhereParameter
                Get
                    If _NUMMAR_W Is Nothing Then
                        _NUMMAR_W = TearOff.NUMMAR
                    End If
                    Return _NUMMAR_W
                End Get
            End Property

            Public ReadOnly Property CANPED() As WhereParameter
                Get
                    If _CANPED_W Is Nothing Then
                        _CANPED_W = TearOff.CANPED
                    End If
                    Return _CANPED_W
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

            Public ReadOnly Property CANFAC() As WhereParameter
                Get
                    If _CANFAC_W Is Nothing Then
                        _CANFAC_W = TearOff.CANFAC
                    End If
                    Return _CANFAC_W
                End Get
            End Property

            Public ReadOnly Property PREUNI() As WhereParameter
                Get
                    If _PREUNI_W Is Nothing Then
                        _PREUNI_W = TearOff.PREUNI
                    End If
                    Return _PREUNI_W
                End Get
            End Property

            Public ReadOnly Property DESCTO() As WhereParameter
                Get
                    If _DESCTO_W Is Nothing Then
                        _DESCTO_W = TearOff.DESCTO
                    End If
                    Return _DESCTO_W
                End Get
            End Property

            Public ReadOnly Property ALEACION() As WhereParameter
                Get
                    If _ALEACION_W Is Nothing Then
                        _ALEACION_W = TearOff.ALEACION
                    End If
                    Return _ALEACION_W
                End Get
            End Property

            Public ReadOnly Property IMPPED() As WhereParameter
                Get
                    If _IMPPED_W Is Nothing Then
                        _IMPPED_W = TearOff.IMPPED
                    End If
                    Return _IMPPED_W
                End Get
            End Property

            Public ReadOnly Property IMPREC() As WhereParameter
                Get
                    If _IMPREC_W Is Nothing Then
                        _IMPREC_W = TearOff.IMPREC
                    End If
                    Return _IMPREC_W
                End Get
            End Property

            Public ReadOnly Property IMPFAC() As WhereParameter
                Get
                    If _IMPFAC_W Is Nothing Then
                        _IMPFAC_W = TearOff.IMPFAC
                    End If
                    Return _IMPFAC_W
                End Get
            End Property

            Public ReadOnly Property FECENTSOL() As WhereParameter
                Get
                    If _FECENTSOL_W Is Nothing Then
                        _FECENTSOL_W = TearOff.FECENTSOL
                    End If
                    Return _FECENTSOL_W
                End Get
            End Property

            Public ReadOnly Property FECENTVIG() As WhereParameter
                Get
                    If _FECENTVIG_W Is Nothing Then
                        _FECENTVIG_W = TearOff.FECENTVIG
                    End If
                    Return _FECENTVIG_W
                End Get
            End Property

            Public ReadOnly Property CODARPRO() As WhereParameter
                Get
                    If _CODARPRO_W Is Nothing Then
                        _CODARPRO_W = TearOff.CODARPRO
                    End If
                    Return _CODARPRO_W
                End Get
            End Property

            Public ReadOnly Property NUMNECES() As WhereParameter
                Get
                    If _NUMNECES_W Is Nothing Then
                        _NUMNECES_W = TearOff.NUMNECES
                    End If
                    Return _NUMNECES_W
                End Get
            End Property

            Public ReadOnly Property PDTE_PREC() As WhereParameter
                Get
                    If _PDTE_PREC_W Is Nothing Then
                        _PDTE_PREC_W = TearOff.PDTE_PREC
                    End If
                    Return _PDTE_PREC_W
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

            Public ReadOnly Property LANTEGI_H() As WhereParameter
                Get
                    If _LANTEGI_H_W Is Nothing Then
                        _LANTEGI_H_W = TearOff.LANTEGI_H
                    End If
                    Return _LANTEGI_H_W
                End Get
            End Property

            Public ReadOnly Property EPREUNI() As WhereParameter
                Get
                    If _EPREUNI_W Is Nothing Then
                        _EPREUNI_W = TearOff.EPREUNI
                    End If
                    Return _EPREUNI_W
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

            Public ReadOnly Property EALEACION() As WhereParameter
                Get
                    If _EALEACION_W Is Nothing Then
                        _EALEACION_W = TearOff.EALEACION
                    End If
                    Return _EALEACION_W
                End Get
            End Property

            Public ReadOnly Property EIMPPED() As WhereParameter
                Get
                    If _EIMPPED_W Is Nothing Then
                        _EIMPPED_W = TearOff.EIMPPED
                    End If
                    Return _EIMPPED_W
                End Get
            End Property

            Public ReadOnly Property EIMPREC() As WhereParameter
                Get
                    If _EIMPREC_W Is Nothing Then
                        _EIMPREC_W = TearOff.EIMPREC
                    End If
                    Return _EIMPREC_W
                End Get
            End Property

            Public ReadOnly Property EIMPFAC() As WhereParameter
                Get
                    If _EIMPFAC_W Is Nothing Then
                        _EIMPFAC_W = TearOff.EIMPFAC
                    End If
                    Return _EIMPFAC_W
                End Get
            End Property

            Public ReadOnly Property FECPDTEPRE() As WhereParameter
                Get
                    If _FECPDTEPRE_W Is Nothing Then
                        _FECPDTEPRE_W = TearOff.FECPDTEPRE
                    End If
                    Return _FECPDTEPRE_W
                End Get
            End Property

            Public ReadOnly Property FECCALIDAD() As WhereParameter
                Get
                    If _FECCALIDAD_W Is Nothing Then
                        _FECCALIDAD_W = TearOff.FECCALIDAD
                    End If
                    Return _FECCALIDAD_W
                End Get
            End Property

            Public ReadOnly Property ID_ESTADO() As WhereParameter
                Get
                    If _ID_ESTADO_W Is Nothing Then
                        _ID_ESTADO_W = TearOff.ID_ESTADO
                    End If
                    Return _ID_ESTADO_W
                End Get
            End Property

            Private _NUMPEDLIN_W As WhereParameter = Nothing
            Private _NUMLINLIN_W As WhereParameter = Nothing
            Private _CODPROLIN_W As WhereParameter = Nothing
            Private _CODART_W As WhereParameter = Nothing
            Private _DESCART_W As WhereParameter = Nothing
            Private _DESCLISTA_W As WhereParameter = Nothing
            Private _NUMORDF_W As WhereParameter = Nothing
            Private _NUMOPE_W As WhereParameter = Nothing
            Private _NUMMAR_W As WhereParameter = Nothing
            Private _CANPED_W As WhereParameter = Nothing
            Private _CANREC_W As WhereParameter = Nothing
            Private _CANFAC_W As WhereParameter = Nothing
            Private _PREUNI_W As WhereParameter = Nothing
            Private _DESCTO_W As WhereParameter = Nothing
            Private _ALEACION_W As WhereParameter = Nothing
            Private _IMPPED_W As WhereParameter = Nothing
            Private _IMPREC_W As WhereParameter = Nothing
            Private _IMPFAC_W As WhereParameter = Nothing
            Private _FECENTSOL_W As WhereParameter = Nothing
            Private _FECENTVIG_W As WhereParameter = Nothing
            Private _CODARPRO_W As WhereParameter = Nothing
            Private _NUMNECES_W As WhereParameter = Nothing
            Private _PDTE_PREC_W As WhereParameter = Nothing
            Private _LANTEGI_W As WhereParameter = Nothing
            Private _LANTEGI_H_W As WhereParameter = Nothing
            Private _EPREUNI_W As WhereParameter = Nothing
            Private _EDIMPRE_W As WhereParameter = Nothing
            Private _EALEACION_W As WhereParameter = Nothing
            Private _EIMPPED_W As WhereParameter = Nothing
            Private _EIMPREC_W As WhereParameter = Nothing
            Private _EIMPFAC_W As WhereParameter = Nothing
            Private _FECPDTEPRE_W As WhereParameter = Nothing
            Private _FECCALIDAD_W As WhereParameter = Nothing
            Private _ID_ESTADO_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _NUMPEDLIN_W = Nothing
                _NUMLINLIN_W = Nothing
                _CODPROLIN_W = Nothing
                _CODART_W = Nothing
                _DESCART_W = Nothing
                _DESCLISTA_W = Nothing
                _NUMORDF_W = Nothing
                _NUMOPE_W = Nothing
                _NUMMAR_W = Nothing
                _CANPED_W = Nothing
                _CANREC_W = Nothing
                _CANFAC_W = Nothing
                _PREUNI_W = Nothing
                _DESCTO_W = Nothing
                _ALEACION_W = Nothing
                _IMPPED_W = Nothing
                _IMPREC_W = Nothing
                _IMPFAC_W = Nothing
                _FECENTSOL_W = Nothing
                _FECENTVIG_W = Nothing
                _CODARPRO_W = Nothing
                _NUMNECES_W = Nothing
                _PDTE_PREC_W = Nothing
                _LANTEGI_W = Nothing
                _LANTEGI_H_W = Nothing
                _EPREUNI_W = Nothing
                _EDIMPRE_W = Nothing
                _EALEACION_W = Nothing
                _EIMPPED_W = Nothing
                _EIMPREC_W = Nothing
                _EIMPFAC_W = Nothing
                _FECPDTEPRE_W = Nothing
                _FECCALIDAD_W = Nothing
                _ID_ESTADO_W = Nothing
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


                Public ReadOnly Property NUMPEDLIN() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMPEDLIN, Parameters.NUMPEDLIN)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMLINLIN() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMLINLIN, Parameters.NUMLINLIN)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODPROLIN() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CODPROLIN, Parameters.CODPROLIN)
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

                Public ReadOnly Property DESCART() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DESCART, Parameters.DESCART)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DESCLISTA() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DESCLISTA, Parameters.DESCLISTA)
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

                Public ReadOnly Property NUMMAR() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMMAR, Parameters.NUMMAR)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CANPED() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CANPED, Parameters.CANPED)
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

                Public ReadOnly Property CANFAC() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CANFAC, Parameters.CANFAC)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PREUNI() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.PREUNI, Parameters.PREUNI)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property DESCTO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.DESCTO, Parameters.DESCTO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ALEACION() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ALEACION, Parameters.ALEACION)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IMPPED() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IMPPED, Parameters.IMPPED)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IMPREC() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IMPREC, Parameters.IMPREC)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property IMPFAC() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IMPFAC, Parameters.IMPFAC)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECENTSOL() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECENTSOL, Parameters.FECENTSOL)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECENTVIG() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECENTVIG, Parameters.FECENTVIG)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CODARPRO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CODARPRO, Parameters.CODARPRO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property NUMNECES() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMNECES, Parameters.NUMNECES)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PDTE_PREC() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.PDTE_PREC, Parameters.PDTE_PREC)
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

                Public ReadOnly Property LANTEGI_H() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.LANTEGI_H, Parameters.LANTEGI_H)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property EPREUNI() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.EPREUNI, Parameters.EPREUNI)
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

                Public ReadOnly Property EALEACION() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.EALEACION, Parameters.EALEACION)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property EIMPPED() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.EIMPPED, Parameters.EIMPPED)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property EIMPREC() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.EIMPREC, Parameters.EIMPREC)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property EIMPFAC() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.EIMPFAC, Parameters.EIMPFAC)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECPDTEPRE() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECPDTEPRE, Parameters.FECPDTEPRE)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECCALIDAD() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECCALIDAD, Parameters.FECCALIDAD)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property ID_ESTADO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_ESTADO, Parameters.ID_ESTADO)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property


                Private _clause As AggregateClause
            End Class
#End Region

            Public ReadOnly Property NUMPEDLIN() As AggregateParameter
                Get
                    If _NUMPEDLIN_W Is Nothing Then
                        _NUMPEDLIN_W = TearOff.NUMPEDLIN
                    End If
                    Return _NUMPEDLIN_W
                End Get
            End Property

            Public ReadOnly Property NUMLINLIN() As AggregateParameter
                Get
                    If _NUMLINLIN_W Is Nothing Then
                        _NUMLINLIN_W = TearOff.NUMLINLIN
                    End If
                    Return _NUMLINLIN_W
                End Get
            End Property

            Public ReadOnly Property CODPROLIN() As AggregateParameter
                Get
                    If _CODPROLIN_W Is Nothing Then
                        _CODPROLIN_W = TearOff.CODPROLIN
                    End If
                    Return _CODPROLIN_W
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

            Public ReadOnly Property DESCART() As AggregateParameter
                Get
                    If _DESCART_W Is Nothing Then
                        _DESCART_W = TearOff.DESCART
                    End If
                    Return _DESCART_W
                End Get
            End Property

            Public ReadOnly Property DESCLISTA() As AggregateParameter
                Get
                    If _DESCLISTA_W Is Nothing Then
                        _DESCLISTA_W = TearOff.DESCLISTA
                    End If
                    Return _DESCLISTA_W
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

            Public ReadOnly Property NUMMAR() As AggregateParameter
                Get
                    If _NUMMAR_W Is Nothing Then
                        _NUMMAR_W = TearOff.NUMMAR
                    End If
                    Return _NUMMAR_W
                End Get
            End Property

            Public ReadOnly Property CANPED() As AggregateParameter
                Get
                    If _CANPED_W Is Nothing Then
                        _CANPED_W = TearOff.CANPED
                    End If
                    Return _CANPED_W
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

            Public ReadOnly Property CANFAC() As AggregateParameter
                Get
                    If _CANFAC_W Is Nothing Then
                        _CANFAC_W = TearOff.CANFAC
                    End If
                    Return _CANFAC_W
                End Get
            End Property

            Public ReadOnly Property PREUNI() As AggregateParameter
                Get
                    If _PREUNI_W Is Nothing Then
                        _PREUNI_W = TearOff.PREUNI
                    End If
                    Return _PREUNI_W
                End Get
            End Property

            Public ReadOnly Property DESCTO() As AggregateParameter
                Get
                    If _DESCTO_W Is Nothing Then
                        _DESCTO_W = TearOff.DESCTO
                    End If
                    Return _DESCTO_W
                End Get
            End Property

            Public ReadOnly Property ALEACION() As AggregateParameter
                Get
                    If _ALEACION_W Is Nothing Then
                        _ALEACION_W = TearOff.ALEACION
                    End If
                    Return _ALEACION_W
                End Get
            End Property

            Public ReadOnly Property IMPPED() As AggregateParameter
                Get
                    If _IMPPED_W Is Nothing Then
                        _IMPPED_W = TearOff.IMPPED
                    End If
                    Return _IMPPED_W
                End Get
            End Property

            Public ReadOnly Property IMPREC() As AggregateParameter
                Get
                    If _IMPREC_W Is Nothing Then
                        _IMPREC_W = TearOff.IMPREC
                    End If
                    Return _IMPREC_W
                End Get
            End Property

            Public ReadOnly Property IMPFAC() As AggregateParameter
                Get
                    If _IMPFAC_W Is Nothing Then
                        _IMPFAC_W = TearOff.IMPFAC
                    End If
                    Return _IMPFAC_W
                End Get
            End Property

            Public ReadOnly Property FECENTSOL() As AggregateParameter
                Get
                    If _FECENTSOL_W Is Nothing Then
                        _FECENTSOL_W = TearOff.FECENTSOL
                    End If
                    Return _FECENTSOL_W
                End Get
            End Property

            Public ReadOnly Property FECENTVIG() As AggregateParameter
                Get
                    If _FECENTVIG_W Is Nothing Then
                        _FECENTVIG_W = TearOff.FECENTVIG
                    End If
                    Return _FECENTVIG_W
                End Get
            End Property

            Public ReadOnly Property CODARPRO() As AggregateParameter
                Get
                    If _CODARPRO_W Is Nothing Then
                        _CODARPRO_W = TearOff.CODARPRO
                    End If
                    Return _CODARPRO_W
                End Get
            End Property

            Public ReadOnly Property NUMNECES() As AggregateParameter
                Get
                    If _NUMNECES_W Is Nothing Then
                        _NUMNECES_W = TearOff.NUMNECES
                    End If
                    Return _NUMNECES_W
                End Get
            End Property

            Public ReadOnly Property PDTE_PREC() As AggregateParameter
                Get
                    If _PDTE_PREC_W Is Nothing Then
                        _PDTE_PREC_W = TearOff.PDTE_PREC
                    End If
                    Return _PDTE_PREC_W
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

            Public ReadOnly Property LANTEGI_H() As AggregateParameter
                Get
                    If _LANTEGI_H_W Is Nothing Then
                        _LANTEGI_H_W = TearOff.LANTEGI_H
                    End If
                    Return _LANTEGI_H_W
                End Get
            End Property

            Public ReadOnly Property EPREUNI() As AggregateParameter
                Get
                    If _EPREUNI_W Is Nothing Then
                        _EPREUNI_W = TearOff.EPREUNI
                    End If
                    Return _EPREUNI_W
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

            Public ReadOnly Property EALEACION() As AggregateParameter
                Get
                    If _EALEACION_W Is Nothing Then
                        _EALEACION_W = TearOff.EALEACION
                    End If
                    Return _EALEACION_W
                End Get
            End Property

            Public ReadOnly Property EIMPPED() As AggregateParameter
                Get
                    If _EIMPPED_W Is Nothing Then
                        _EIMPPED_W = TearOff.EIMPPED
                    End If
                    Return _EIMPPED_W
                End Get
            End Property

            Public ReadOnly Property EIMPREC() As AggregateParameter
                Get
                    If _EIMPREC_W Is Nothing Then
                        _EIMPREC_W = TearOff.EIMPREC
                    End If
                    Return _EIMPREC_W
                End Get
            End Property

            Public ReadOnly Property EIMPFAC() As AggregateParameter
                Get
                    If _EIMPFAC_W Is Nothing Then
                        _EIMPFAC_W = TearOff.EIMPFAC
                    End If
                    Return _EIMPFAC_W
                End Get
            End Property

            Public ReadOnly Property FECPDTEPRE() As AggregateParameter
                Get
                    If _FECPDTEPRE_W Is Nothing Then
                        _FECPDTEPRE_W = TearOff.FECPDTEPRE
                    End If
                    Return _FECPDTEPRE_W
                End Get
            End Property

            Public ReadOnly Property FECCALIDAD() As AggregateParameter
                Get
                    If _FECCALIDAD_W Is Nothing Then
                        _FECCALIDAD_W = TearOff.FECCALIDAD
                    End If
                    Return _FECCALIDAD_W
                End Get
            End Property

            Public ReadOnly Property ID_ESTADO() As AggregateParameter
                Get
                    If _ID_ESTADO_W Is Nothing Then
                        _ID_ESTADO_W = TearOff.ID_ESTADO
                    End If
                    Return _ID_ESTADO_W
                End Get
            End Property

            Private _NUMPEDLIN_W As AggregateParameter = Nothing
            Private _NUMLINLIN_W As AggregateParameter = Nothing
            Private _CODPROLIN_W As AggregateParameter = Nothing
            Private _CODART_W As AggregateParameter = Nothing
            Private _DESCART_W As AggregateParameter = Nothing
            Private _DESCLISTA_W As AggregateParameter = Nothing
            Private _NUMORDF_W As AggregateParameter = Nothing
            Private _NUMOPE_W As AggregateParameter = Nothing
            Private _NUMMAR_W As AggregateParameter = Nothing
            Private _CANPED_W As AggregateParameter = Nothing
            Private _CANREC_W As AggregateParameter = Nothing
            Private _CANFAC_W As AggregateParameter = Nothing
            Private _PREUNI_W As AggregateParameter = Nothing
            Private _DESCTO_W As AggregateParameter = Nothing
            Private _ALEACION_W As AggregateParameter = Nothing
            Private _IMPPED_W As AggregateParameter = Nothing
            Private _IMPREC_W As AggregateParameter = Nothing
            Private _IMPFAC_W As AggregateParameter = Nothing
            Private _FECENTSOL_W As AggregateParameter = Nothing
            Private _FECENTVIG_W As AggregateParameter = Nothing
            Private _CODARPRO_W As AggregateParameter = Nothing
            Private _NUMNECES_W As AggregateParameter = Nothing
            Private _PDTE_PREC_W As AggregateParameter = Nothing
            Private _LANTEGI_W As AggregateParameter = Nothing
            Private _LANTEGI_H_W As AggregateParameter = Nothing
            Private _EPREUNI_W As AggregateParameter = Nothing
            Private _EDIMPRE_W As AggregateParameter = Nothing
            Private _EALEACION_W As AggregateParameter = Nothing
            Private _EIMPPED_W As AggregateParameter = Nothing
            Private _EIMPREC_W As AggregateParameter = Nothing
            Private _EIMPFAC_W As AggregateParameter = Nothing
            Private _FECPDTEPRE_W As AggregateParameter = Nothing
            Private _FECCALIDAD_W As AggregateParameter = Nothing
            Private _ID_ESTADO_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _NUMPEDLIN_W = Nothing
                _NUMLINLIN_W = Nothing
                _CODPROLIN_W = Nothing
                _CODART_W = Nothing
                _DESCART_W = Nothing
                _DESCLISTA_W = Nothing
                _NUMORDF_W = Nothing
                _NUMOPE_W = Nothing
                _NUMMAR_W = Nothing
                _CANPED_W = Nothing
                _CANREC_W = Nothing
                _CANFAC_W = Nothing
                _PREUNI_W = Nothing
                _DESCTO_W = Nothing
                _ALEACION_W = Nothing
                _IMPPED_W = Nothing
                _IMPREC_W = Nothing
                _IMPFAC_W = Nothing
                _FECENTSOL_W = Nothing
                _FECENTVIG_W = Nothing
                _CODARPRO_W = Nothing
                _NUMNECES_W = Nothing
                _PDTE_PREC_W = Nothing
                _LANTEGI_W = Nothing
                _LANTEGI_H_W = Nothing
                _EPREUNI_W = Nothing
                _EDIMPRE_W = Nothing
                _EALEACION_W = Nothing
                _EIMPPED_W = Nothing
                _EIMPREC_W = Nothing
                _EIMPFAC_W = Nothing
                _FECPDTEPRE_W = Nothing
                _FECCALIDAD_W = Nothing
                _ID_ESTADO_W = Nothing
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
            cmd.CommandText = Me.SchemaStoredProcedure + "PI_GCLINPED"

            CreateParameters(cmd)


            Return cmd

        End Function

        Protected Overrides Function GetUpdateCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PU_GCLINPED"

            CreateParameters(cmd)

            Return cmd

        End Function

        Protected Overrides Function GetDeleteCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PD_GCLINPED"

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.NUMLINLIN)
            p.SourceColumn = ColumnNames.NUMLINLIN
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMPEDLIN)
            p.SourceColumn = ColumnNames.NUMPEDLIN
            p.SourceVersion = DataRowVersion.Current


            Return cmd

        End Function

        Private Sub CreateParameters(ByVal cmd As OracleCommand)

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.NUMPEDLIN)
            p.SourceColumn = ColumnNames.NUMPEDLIN
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMLINLIN)
            p.SourceColumn = ColumnNames.NUMLINLIN
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CODPROLIN)
            p.SourceColumn = ColumnNames.CODPROLIN
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CODART)
            p.SourceColumn = ColumnNames.CODART
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.DESCART)
            p.SourceColumn = ColumnNames.DESCART
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.DESCLISTA)
            p.SourceColumn = ColumnNames.DESCLISTA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMORDF)
            p.SourceColumn = ColumnNames.NUMORDF
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMOPE)
            p.SourceColumn = ColumnNames.NUMOPE
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMMAR)
            p.SourceColumn = ColumnNames.NUMMAR
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CANPED)
            p.SourceColumn = ColumnNames.CANPED
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CANREC)
            p.SourceColumn = ColumnNames.CANREC
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CANFAC)
            p.SourceColumn = ColumnNames.CANFAC
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.PREUNI)
            p.SourceColumn = ColumnNames.PREUNI
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.DESCTO)
            p.SourceColumn = ColumnNames.DESCTO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.ALEACION)
            p.SourceColumn = ColumnNames.ALEACION
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IMPPED)
            p.SourceColumn = ColumnNames.IMPPED
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IMPREC)
            p.SourceColumn = ColumnNames.IMPREC
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.IMPFAC)
            p.SourceColumn = ColumnNames.IMPFAC
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.FECENTSOL)
            p.SourceColumn = ColumnNames.FECENTSOL
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.FECENTVIG)
            p.SourceColumn = ColumnNames.FECENTVIG
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CODARPRO)
            p.SourceColumn = ColumnNames.CODARPRO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMNECES)
            p.SourceColumn = ColumnNames.NUMNECES
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.PDTE_PREC)
            p.SourceColumn = ColumnNames.PDTE_PREC
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.LANTEGI)
            p.SourceColumn = ColumnNames.LANTEGI
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.LANTEGI_H)
            p.SourceColumn = ColumnNames.LANTEGI_H
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.EPREUNI)
            p.SourceColumn = ColumnNames.EPREUNI
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.EDIMPRE)
            p.SourceColumn = ColumnNames.EDIMPRE
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.EALEACION)
            p.SourceColumn = ColumnNames.EALEACION
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.EIMPPED)
            p.SourceColumn = ColumnNames.EIMPPED
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.EIMPREC)
            p.SourceColumn = ColumnNames.EIMPREC
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.EIMPFAC)
            p.SourceColumn = ColumnNames.EIMPFAC
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.FECPDTEPRE)
            p.SourceColumn = ColumnNames.FECPDTEPRE
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.FECCALIDAD)
            p.SourceColumn = ColumnNames.FECCALIDAD
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.ID_ESTADO)
            p.SourceColumn = ColumnNames.ID_ESTADO
            p.SourceVersion = DataRowVersion.Current


        End Sub

    End Class

End Namespace

