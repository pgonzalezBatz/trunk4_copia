
'===============================================================================
'BATZ, Koop. - 23/02/2009 14:24:46
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

    Public MustInherit Class _GCHISMOV
        Inherits OracleClientEntity

        Public Sub New()
            Me.SchemaGlobal = "XBAT."
            Me.QuerySource = "GCHISMOV"
            Me.MappingName = "GCHISMOV"
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


            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_GCHISMOV", parameters)
        End Function

        '=================================================================
        ' Public Overridable Function LoadByPrimaryKey()  As Boolean
        '=================================================================
        '  Loads a single row of via the primary key
        '=================================================================
        Public Overridable Function LoadByPrimaryKey(ByVal NUMMOVES As Decimal) As Boolean

            Dim parameters As ListDictionary = New ListDictionary()
            parameters.Add(_GCHISMOV.Parameters.NUMMOVES, NUMMOVES)


            Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
            p.Direction = ParameterDirection.Output
            parameters.Add(p, Nothing)

            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_GCHISMOV", parameters)

        End Function

#Region "Parameters"
        Protected Class Parameters

            Public Shared ReadOnly Property NUMMOVES As OracleParameter
                Get
                    Return New OracleParameter("p_NUMMOVES", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property TIPOES As OracleParameter
                Get
                    Return New OracleParameter("p_TIPOES", OracleDbType.Char, 1)
                End Get
            End Property

            Public Shared ReadOnly Property TIPOMOV As OracleParameter
                Get
                    Return New OracleParameter("p_TIPOMOV", OracleDbType.Char, 1)
                End Get
            End Property

            Public Shared ReadOnly Property CODART As OracleParameter
                Get
                    Return New OracleParameter("p_CODART", OracleDbType.Char, 12)
                End Get
            End Property

            Public Shared ReadOnly Property CODALM As OracleParameter
                Get
                    Return New OracleParameter("p_CODALM", OracleDbType.Char, 4)
                End Get
            End Property

            Public Shared ReadOnly Property FECMOV As OracleParameter
                Get
                    Return New OracleParameter("p_FECMOV", OracleDbType.Date, 0)
                End Get
            End Property

            Public Shared ReadOnly Property CANTID As OracleParameter
                Get
                    Return New OracleParameter("p_CANTID", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property PRECIO As OracleParameter
                Get
                    Return New OracleParameter("p_PRECIO", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property PREMED As OracleParameter
                Get
                    Return New OracleParameter("p_PREMED", OracleDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property CODPRO As OracleParameter
                Get
                    Return New OracleParameter("p_CODPRO", OracleDbType.Char, 12)
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

            Public Shared ReadOnly Property NUMDOCU As OracleParameter
                Get
                    Return New OracleParameter("p_NUMDOCU", OracleDbType.Int32, 0)
                End Get
            End Property

            Public Shared ReadOnly Property MARCA As OracleParameter
                Get
                    Return New OracleParameter("p_MARCA", OracleDbType.Char, 11)
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

            Public Shared ReadOnly Property EPRECIO As OracleParameter
                Get
                    Return New OracleParameter("p_EPRECIO", OracleDbType.Decimal, 0)
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

            Public Shared ReadOnly Property CERTI_CALIDAD As OracleParameter
                Get
                    Return New OracleParameter("p_CERTI_CALIDAD", OracleDbType.Char, 10)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const NUMMOVES As String = "NUMMOVES"
            Public Const TIPOES As String = "TIPOES"
            Public Const TIPOMOV As String = "TIPOMOV"
            Public Const CODART As String = "CODART"
            Public Const CODALM As String = "CODALM"
            Public Const FECMOV As String = "FECMOV"
            Public Const CANTID As String = "CANTID"
            Public Const PRECIO As String = "PRECIO"
            Public Const PREMED As String = "PREMED"
            Public Const CODPRO As String = "CODPRO"
            Public Const NUMORDF As String = "NUMORDF"
            Public Const NUMOPE As String = "NUMOPE"
            Public Const NUMDOCU As String = "NUMDOCU"
            Public Const MARCA As String = "MARCA"
            Public Const LANTEGI As String = "LANTEGI"
            Public Const LANTEGI_H As String = "LANTEGI_H"
            Public Const EPRECIO As String = "EPRECIO"
            Public Const EPREMED As String = "EPREMED"
            Public Const EDIMPRE As String = "EDIMPRE"
            Public Const CERTI_CALIDAD As String = "CERTI_CALIDAD"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(NUMMOVES) = _GCHISMOV.PropertyNames.NUMMOVES
                    ht(TIPOES) = _GCHISMOV.PropertyNames.TIPOES
                    ht(TIPOMOV) = _GCHISMOV.PropertyNames.TIPOMOV
                    ht(CODART) = _GCHISMOV.PropertyNames.CODART
                    ht(CODALM) = _GCHISMOV.PropertyNames.CODALM
                    ht(FECMOV) = _GCHISMOV.PropertyNames.FECMOV
                    ht(CANTID) = _GCHISMOV.PropertyNames.CANTID
                    ht(PRECIO) = _GCHISMOV.PropertyNames.PRECIO
                    ht(PREMED) = _GCHISMOV.PropertyNames.PREMED
                    ht(CODPRO) = _GCHISMOV.PropertyNames.CODPRO
                    ht(NUMORDF) = _GCHISMOV.PropertyNames.NUMORDF
                    ht(NUMOPE) = _GCHISMOV.PropertyNames.NUMOPE
                    ht(NUMDOCU) = _GCHISMOV.PropertyNames.NUMDOCU
                    ht(MARCA) = _GCHISMOV.PropertyNames.MARCA
                    ht(LANTEGI) = _GCHISMOV.PropertyNames.LANTEGI
                    ht(LANTEGI_H) = _GCHISMOV.PropertyNames.LANTEGI_H
                    ht(EPRECIO) = _GCHISMOV.PropertyNames.EPRECIO
                    ht(EPREMED) = _GCHISMOV.PropertyNames.EPREMED
                    ht(EDIMPRE) = _GCHISMOV.PropertyNames.EDIMPRE
                    ht(CERTI_CALIDAD) = _GCHISMOV.PropertyNames.CERTI_CALIDAD

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const NUMMOVES As String = "NUMMOVES"
            Public Const TIPOES As String = "TIPOES"
            Public Const TIPOMOV As String = "TIPOMOV"
            Public Const CODART As String = "CODART"
            Public Const CODALM As String = "CODALM"
            Public Const FECMOV As String = "FECMOV"
            Public Const CANTID As String = "CANTID"
            Public Const PRECIO As String = "PRECIO"
            Public Const PREMED As String = "PREMED"
            Public Const CODPRO As String = "CODPRO"
            Public Const NUMORDF As String = "NUMORDF"
            Public Const NUMOPE As String = "NUMOPE"
            Public Const NUMDOCU As String = "NUMDOCU"
            Public Const MARCA As String = "MARCA"
            Public Const LANTEGI As String = "LANTEGI"
            Public Const LANTEGI_H As String = "LANTEGI_H"
            Public Const EPRECIO As String = "EPRECIO"
            Public Const EPREMED As String = "EPREMED"
            Public Const EDIMPRE As String = "EDIMPRE"
            Public Const CERTI_CALIDAD As String = "CERTI_CALIDAD"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(NUMMOVES) = _GCHISMOV.ColumnNames.NUMMOVES
                    ht(TIPOES) = _GCHISMOV.ColumnNames.TIPOES
                    ht(TIPOMOV) = _GCHISMOV.ColumnNames.TIPOMOV
                    ht(CODART) = _GCHISMOV.ColumnNames.CODART
                    ht(CODALM) = _GCHISMOV.ColumnNames.CODALM
                    ht(FECMOV) = _GCHISMOV.ColumnNames.FECMOV
                    ht(CANTID) = _GCHISMOV.ColumnNames.CANTID
                    ht(PRECIO) = _GCHISMOV.ColumnNames.PRECIO
                    ht(PREMED) = _GCHISMOV.ColumnNames.PREMED
                    ht(CODPRO) = _GCHISMOV.ColumnNames.CODPRO
                    ht(NUMORDF) = _GCHISMOV.ColumnNames.NUMORDF
                    ht(NUMOPE) = _GCHISMOV.ColumnNames.NUMOPE
                    ht(NUMDOCU) = _GCHISMOV.ColumnNames.NUMDOCU
                    ht(MARCA) = _GCHISMOV.ColumnNames.MARCA
                    ht(LANTEGI) = _GCHISMOV.ColumnNames.LANTEGI
                    ht(LANTEGI_H) = _GCHISMOV.ColumnNames.LANTEGI_H
                    ht(EPRECIO) = _GCHISMOV.ColumnNames.EPRECIO
                    ht(EPREMED) = _GCHISMOV.ColumnNames.EPREMED
                    ht(EDIMPRE) = _GCHISMOV.ColumnNames.EDIMPRE
                    ht(CERTI_CALIDAD) = _GCHISMOV.ColumnNames.CERTI_CALIDAD

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const NUMMOVES As String = "s_NUMMOVES"
            Public Const TIPOES As String = "s_TIPOES"
            Public Const TIPOMOV As String = "s_TIPOMOV"
            Public Const CODART As String = "s_CODART"
            Public Const CODALM As String = "s_CODALM"
            Public Const FECMOV As String = "s_FECMOV"
            Public Const CANTID As String = "s_CANTID"
            Public Const PRECIO As String = "s_PRECIO"
            Public Const PREMED As String = "s_PREMED"
            Public Const CODPRO As String = "s_CODPRO"
            Public Const NUMORDF As String = "s_NUMORDF"
            Public Const NUMOPE As String = "s_NUMOPE"
            Public Const NUMDOCU As String = "s_NUMDOCU"
            Public Const MARCA As String = "s_MARCA"
            Public Const LANTEGI As String = "s_LANTEGI"
            Public Const LANTEGI_H As String = "s_LANTEGI_H"
            Public Const EPRECIO As String = "s_EPRECIO"
            Public Const EPREMED As String = "s_EPREMED"
            Public Const EDIMPRE As String = "s_EDIMPRE"
            Public Const CERTI_CALIDAD As String = "s_CERTI_CALIDAD"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property NUMMOVES As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.NUMMOVES)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.NUMMOVES, Value)
            End Set
        End Property

        Public Overridable Property TIPOES As String
            Get
                Return MyBase.GetString(ColumnNames.TIPOES)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.TIPOES, Value)
            End Set
        End Property

        Public Overridable Property TIPOMOV As String
            Get
                Return MyBase.GetString(ColumnNames.TIPOMOV)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.TIPOMOV, Value)
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

        Public Overridable Property CODALM As String
            Get
                Return MyBase.GetString(ColumnNames.CODALM)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CODALM, Value)
            End Set
        End Property

        Public Overridable Property FECMOV As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.FECMOV)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.FECMOV, Value)
            End Set
        End Property

        Public Overridable Property CANTID As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.CANTID)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.CANTID, Value)
            End Set
        End Property

        Public Overridable Property PRECIO As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.PRECIO)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.PRECIO, Value)
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

        Public Overridable Property CODPRO As String
            Get
                Return MyBase.GetString(ColumnNames.CODPRO)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CODPRO, Value)
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

        Public Overridable Property NUMDOCU As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.NUMDOCU)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.NUMDOCU, Value)
            End Set
        End Property

        Public Overridable Property MARCA As String
            Get
                Return MyBase.GetString(ColumnNames.MARCA)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.MARCA, Value)
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

        Public Overridable Property EPRECIO As Decimal
            Get
                Return MyBase.GetDecimal(ColumnNames.EPRECIO)
            End Get
            Set(ByVal Value As Decimal)
                MyBase.SetDecimal(ColumnNames.EPRECIO, Value)
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

        Public Overridable Property CERTI_CALIDAD As String
            Get
                Return MyBase.GetString(ColumnNames.CERTI_CALIDAD)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.CERTI_CALIDAD, Value)
            End Set
        End Property


#End Region

#Region "String Properties"

        Public Overridable Property s_NUMMOVES As String
            Get
                If Me.IsColumnNull(ColumnNames.NUMMOVES) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.NUMMOVES)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NUMMOVES)
                Else
                    Me.NUMMOVES = MyBase.SetDecimalAsString(ColumnNames.NUMMOVES, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_TIPOES As String
            Get
                If Me.IsColumnNull(ColumnNames.TIPOES) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.TIPOES)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.TIPOES)
                Else
                    Me.TIPOES = MyBase.SetStringAsString(ColumnNames.TIPOES, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_TIPOMOV As String
            Get
                If Me.IsColumnNull(ColumnNames.TIPOMOV) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.TIPOMOV)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.TIPOMOV)
                Else
                    Me.TIPOMOV = MyBase.SetStringAsString(ColumnNames.TIPOMOV, Value)
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

        Public Overridable Property s_FECMOV As String
            Get
                If Me.IsColumnNull(ColumnNames.FECMOV) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.FECMOV)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.FECMOV)
                Else
                    Me.FECMOV = MyBase.SetDateTimeAsString(ColumnNames.FECMOV, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_CANTID As String
            Get
                If Me.IsColumnNull(ColumnNames.CANTID) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.CANTID)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CANTID)
                Else
                    Me.CANTID = MyBase.SetDecimalAsString(ColumnNames.CANTID, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_PRECIO As String
            Get
                If Me.IsColumnNull(ColumnNames.PRECIO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.PRECIO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.PRECIO)
                Else
                    Me.PRECIO = MyBase.SetDecimalAsString(ColumnNames.PRECIO, Value)
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

        Public Overridable Property s_NUMDOCU As String
            Get
                If Me.IsColumnNull(ColumnNames.NUMDOCU) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.NUMDOCU)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.NUMDOCU)
                Else
                    Me.NUMDOCU = MyBase.SetDecimalAsString(ColumnNames.NUMDOCU, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_MARCA As String
            Get
                If Me.IsColumnNull(ColumnNames.MARCA) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.MARCA)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.MARCA)
                Else
                    Me.MARCA = MyBase.SetStringAsString(ColumnNames.MARCA, Value)
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

        Public Overridable Property s_EPRECIO As String
            Get
                If Me.IsColumnNull(ColumnNames.EPRECIO) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDecimalAsString(ColumnNames.EPRECIO)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.EPRECIO)
                Else
                    Me.EPRECIO = MyBase.SetDecimalAsString(ColumnNames.EPRECIO, Value)
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

        Public Overridable Property s_CERTI_CALIDAD As String
            Get
                If Me.IsColumnNull(ColumnNames.CERTI_CALIDAD) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.CERTI_CALIDAD)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.CERTI_CALIDAD)
                Else
                    Me.CERTI_CALIDAD = MyBase.SetStringAsString(ColumnNames.CERTI_CALIDAD, Value)
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


                Public ReadOnly Property NUMMOVES() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMMOVES, Parameters.NUMMOVES)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TIPOES() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.TIPOES, Parameters.TIPOES)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TIPOMOV() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.TIPOMOV, Parameters.TIPOMOV)
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

                Public ReadOnly Property CODALM() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CODALM, Parameters.CODALM)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECMOV() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.FECMOV, Parameters.FECMOV)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CANTID() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CANTID, Parameters.CANTID)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PRECIO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.PRECIO, Parameters.PRECIO)
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

                Public ReadOnly Property CODPRO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CODPRO, Parameters.CODPRO)
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

                Public ReadOnly Property NUMDOCU() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.NUMDOCU, Parameters.NUMDOCU)
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

                Public ReadOnly Property EPRECIO() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.EPRECIO, Parameters.EPRECIO)
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

                Public ReadOnly Property CERTI_CALIDAD() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.CERTI_CALIDAD, Parameters.CERTI_CALIDAD)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property


                Private _clause As WhereClause
            End Class
#End Region

            Public ReadOnly Property NUMMOVES() As WhereParameter
                Get
                    If _NUMMOVES_W Is Nothing Then
                        _NUMMOVES_W = TearOff.NUMMOVES
                    End If
                    Return _NUMMOVES_W
                End Get
            End Property

            Public ReadOnly Property TIPOES() As WhereParameter
                Get
                    If _TIPOES_W Is Nothing Then
                        _TIPOES_W = TearOff.TIPOES
                    End If
                    Return _TIPOES_W
                End Get
            End Property

            Public ReadOnly Property TIPOMOV() As WhereParameter
                Get
                    If _TIPOMOV_W Is Nothing Then
                        _TIPOMOV_W = TearOff.TIPOMOV
                    End If
                    Return _TIPOMOV_W
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

            Public ReadOnly Property CODALM() As WhereParameter
                Get
                    If _CODALM_W Is Nothing Then
                        _CODALM_W = TearOff.CODALM
                    End If
                    Return _CODALM_W
                End Get
            End Property

            Public ReadOnly Property FECMOV() As WhereParameter
                Get
                    If _FECMOV_W Is Nothing Then
                        _FECMOV_W = TearOff.FECMOV
                    End If
                    Return _FECMOV_W
                End Get
            End Property

            Public ReadOnly Property CANTID() As WhereParameter
                Get
                    If _CANTID_W Is Nothing Then
                        _CANTID_W = TearOff.CANTID
                    End If
                    Return _CANTID_W
                End Get
            End Property

            Public ReadOnly Property PRECIO() As WhereParameter
                Get
                    If _PRECIO_W Is Nothing Then
                        _PRECIO_W = TearOff.PRECIO
                    End If
                    Return _PRECIO_W
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

            Public ReadOnly Property CODPRO() As WhereParameter
                Get
                    If _CODPRO_W Is Nothing Then
                        _CODPRO_W = TearOff.CODPRO
                    End If
                    Return _CODPRO_W
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

            Public ReadOnly Property NUMDOCU() As WhereParameter
                Get
                    If _NUMDOCU_W Is Nothing Then
                        _NUMDOCU_W = TearOff.NUMDOCU
                    End If
                    Return _NUMDOCU_W
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

            Public ReadOnly Property EPRECIO() As WhereParameter
                Get
                    If _EPRECIO_W Is Nothing Then
                        _EPRECIO_W = TearOff.EPRECIO
                    End If
                    Return _EPRECIO_W
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

            Public ReadOnly Property CERTI_CALIDAD() As WhereParameter
                Get
                    If _CERTI_CALIDAD_W Is Nothing Then
                        _CERTI_CALIDAD_W = TearOff.CERTI_CALIDAD
                    End If
                    Return _CERTI_CALIDAD_W
                End Get
            End Property

            Private _NUMMOVES_W As WhereParameter = Nothing
            Private _TIPOES_W As WhereParameter = Nothing
            Private _TIPOMOV_W As WhereParameter = Nothing
            Private _CODART_W As WhereParameter = Nothing
            Private _CODALM_W As WhereParameter = Nothing
            Private _FECMOV_W As WhereParameter = Nothing
            Private _CANTID_W As WhereParameter = Nothing
            Private _PRECIO_W As WhereParameter = Nothing
            Private _PREMED_W As WhereParameter = Nothing
            Private _CODPRO_W As WhereParameter = Nothing
            Private _NUMORDF_W As WhereParameter = Nothing
            Private _NUMOPE_W As WhereParameter = Nothing
            Private _NUMDOCU_W As WhereParameter = Nothing
            Private _MARCA_W As WhereParameter = Nothing
            Private _LANTEGI_W As WhereParameter = Nothing
            Private _LANTEGI_H_W As WhereParameter = Nothing
            Private _EPRECIO_W As WhereParameter = Nothing
            Private _EPREMED_W As WhereParameter = Nothing
            Private _EDIMPRE_W As WhereParameter = Nothing
            Private _CERTI_CALIDAD_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _NUMMOVES_W = Nothing
                _TIPOES_W = Nothing
                _TIPOMOV_W = Nothing
                _CODART_W = Nothing
                _CODALM_W = Nothing
                _FECMOV_W = Nothing
                _CANTID_W = Nothing
                _PRECIO_W = Nothing
                _PREMED_W = Nothing
                _CODPRO_W = Nothing
                _NUMORDF_W = Nothing
                _NUMOPE_W = Nothing
                _NUMDOCU_W = Nothing
                _MARCA_W = Nothing
                _LANTEGI_W = Nothing
                _LANTEGI_H_W = Nothing
                _EPRECIO_W = Nothing
                _EPREMED_W = Nothing
                _EDIMPRE_W = Nothing
                _CERTI_CALIDAD_W = Nothing
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


                Public ReadOnly Property NUMMOVES() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMMOVES, Parameters.NUMMOVES)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TIPOES() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TIPOES, Parameters.TIPOES)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property TIPOMOV() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.TIPOMOV, Parameters.TIPOMOV)
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

                Public ReadOnly Property CODALM() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CODALM, Parameters.CODALM)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property FECMOV() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECMOV, Parameters.FECMOV)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property CANTID() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CANTID, Parameters.CANTID)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property PRECIO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.PRECIO, Parameters.PRECIO)
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

                Public ReadOnly Property CODPRO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CODPRO, Parameters.CODPRO)
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

                Public ReadOnly Property NUMDOCU() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NUMDOCU, Parameters.NUMDOCU)
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

                Public ReadOnly Property EPRECIO() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.EPRECIO, Parameters.EPRECIO)
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

                Public ReadOnly Property CERTI_CALIDAD() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.CERTI_CALIDAD, Parameters.CERTI_CALIDAD)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property


                Private _clause As AggregateClause
            End Class
#End Region

            Public ReadOnly Property NUMMOVES() As AggregateParameter
                Get
                    If _NUMMOVES_W Is Nothing Then
                        _NUMMOVES_W = TearOff.NUMMOVES
                    End If
                    Return _NUMMOVES_W
                End Get
            End Property

            Public ReadOnly Property TIPOES() As AggregateParameter
                Get
                    If _TIPOES_W Is Nothing Then
                        _TIPOES_W = TearOff.TIPOES
                    End If
                    Return _TIPOES_W
                End Get
            End Property

            Public ReadOnly Property TIPOMOV() As AggregateParameter
                Get
                    If _TIPOMOV_W Is Nothing Then
                        _TIPOMOV_W = TearOff.TIPOMOV
                    End If
                    Return _TIPOMOV_W
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

            Public ReadOnly Property CODALM() As AggregateParameter
                Get
                    If _CODALM_W Is Nothing Then
                        _CODALM_W = TearOff.CODALM
                    End If
                    Return _CODALM_W
                End Get
            End Property

            Public ReadOnly Property FECMOV() As AggregateParameter
                Get
                    If _FECMOV_W Is Nothing Then
                        _FECMOV_W = TearOff.FECMOV
                    End If
                    Return _FECMOV_W
                End Get
            End Property

            Public ReadOnly Property CANTID() As AggregateParameter
                Get
                    If _CANTID_W Is Nothing Then
                        _CANTID_W = TearOff.CANTID
                    End If
                    Return _CANTID_W
                End Get
            End Property

            Public ReadOnly Property PRECIO() As AggregateParameter
                Get
                    If _PRECIO_W Is Nothing Then
                        _PRECIO_W = TearOff.PRECIO
                    End If
                    Return _PRECIO_W
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

            Public ReadOnly Property CODPRO() As AggregateParameter
                Get
                    If _CODPRO_W Is Nothing Then
                        _CODPRO_W = TearOff.CODPRO
                    End If
                    Return _CODPRO_W
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

            Public ReadOnly Property NUMDOCU() As AggregateParameter
                Get
                    If _NUMDOCU_W Is Nothing Then
                        _NUMDOCU_W = TearOff.NUMDOCU
                    End If
                    Return _NUMDOCU_W
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

            Public ReadOnly Property EPRECIO() As AggregateParameter
                Get
                    If _EPRECIO_W Is Nothing Then
                        _EPRECIO_W = TearOff.EPRECIO
                    End If
                    Return _EPRECIO_W
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

            Public ReadOnly Property CERTI_CALIDAD() As AggregateParameter
                Get
                    If _CERTI_CALIDAD_W Is Nothing Then
                        _CERTI_CALIDAD_W = TearOff.CERTI_CALIDAD
                    End If
                    Return _CERTI_CALIDAD_W
                End Get
            End Property

            Private _NUMMOVES_W As AggregateParameter = Nothing
            Private _TIPOES_W As AggregateParameter = Nothing
            Private _TIPOMOV_W As AggregateParameter = Nothing
            Private _CODART_W As AggregateParameter = Nothing
            Private _CODALM_W As AggregateParameter = Nothing
            Private _FECMOV_W As AggregateParameter = Nothing
            Private _CANTID_W As AggregateParameter = Nothing
            Private _PRECIO_W As AggregateParameter = Nothing
            Private _PREMED_W As AggregateParameter = Nothing
            Private _CODPRO_W As AggregateParameter = Nothing
            Private _NUMORDF_W As AggregateParameter = Nothing
            Private _NUMOPE_W As AggregateParameter = Nothing
            Private _NUMDOCU_W As AggregateParameter = Nothing
            Private _MARCA_W As AggregateParameter = Nothing
            Private _LANTEGI_W As AggregateParameter = Nothing
            Private _LANTEGI_H_W As AggregateParameter = Nothing
            Private _EPRECIO_W As AggregateParameter = Nothing
            Private _EPREMED_W As AggregateParameter = Nothing
            Private _EDIMPRE_W As AggregateParameter = Nothing
            Private _CERTI_CALIDAD_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _NUMMOVES_W = Nothing
                _TIPOES_W = Nothing
                _TIPOMOV_W = Nothing
                _CODART_W = Nothing
                _CODALM_W = Nothing
                _FECMOV_W = Nothing
                _CANTID_W = Nothing
                _PRECIO_W = Nothing
                _PREMED_W = Nothing
                _CODPRO_W = Nothing
                _NUMORDF_W = Nothing
                _NUMOPE_W = Nothing
                _NUMDOCU_W = Nothing
                _MARCA_W = Nothing
                _LANTEGI_W = Nothing
                _LANTEGI_H_W = Nothing
                _EPRECIO_W = Nothing
                _EPREMED_W = Nothing
                _EDIMPRE_W = Nothing
                _CERTI_CALIDAD_W = Nothing
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
            cmd.CommandText = Me.SchemaStoredProcedure + "PI_GCHISMOV"

            CreateParameters(cmd)


            Return cmd

        End Function

        Protected Overrides Function GetUpdateCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PU_GCHISMOV"

            CreateParameters(cmd)

            Return cmd

        End Function

        Protected Overrides Function GetDeleteCommand() As IDbCommand

            Dim cmd As OracleCommand = New OracleCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "PD_GCHISMOV"

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.NUMMOVES)
            p.SourceColumn = ColumnNames.NUMMOVES
            p.SourceVersion = DataRowVersion.Current


            Return cmd

        End Function

        Private Sub CreateParameters(ByVal cmd As OracleCommand)

            Dim p As OracleParameter
            p = cmd.Parameters.Add(Parameters.NUMMOVES)
            p.SourceColumn = ColumnNames.NUMMOVES
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.TIPOES)
            p.SourceColumn = ColumnNames.TIPOES
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.TIPOMOV)
            p.SourceColumn = ColumnNames.TIPOMOV
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CODART)
            p.SourceColumn = ColumnNames.CODART
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CODALM)
            p.SourceColumn = ColumnNames.CODALM
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.FECMOV)
            p.SourceColumn = ColumnNames.FECMOV
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CANTID)
            p.SourceColumn = ColumnNames.CANTID
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.PRECIO)
            p.SourceColumn = ColumnNames.PRECIO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.PREMED)
            p.SourceColumn = ColumnNames.PREMED
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CODPRO)
            p.SourceColumn = ColumnNames.CODPRO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMORDF)
            p.SourceColumn = ColumnNames.NUMORDF
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMOPE)
            p.SourceColumn = ColumnNames.NUMOPE
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.NUMDOCU)
            p.SourceColumn = ColumnNames.NUMDOCU
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.MARCA)
            p.SourceColumn = ColumnNames.MARCA
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.LANTEGI)
            p.SourceColumn = ColumnNames.LANTEGI
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.LANTEGI_H)
            p.SourceColumn = ColumnNames.LANTEGI_H
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.EPRECIO)
            p.SourceColumn = ColumnNames.EPRECIO
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.EPREMED)
            p.SourceColumn = ColumnNames.EPREMED
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.EDIMPRE)
            p.SourceColumn = ColumnNames.EDIMPRE
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.CERTI_CALIDAD)
            p.SourceColumn = ColumnNames.CERTI_CALIDAD
            p.SourceVersion = DataRowVersion.Current


        End Sub

    End Class

End Namespace

