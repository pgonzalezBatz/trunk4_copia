
'===============================================================================
'BATZ, Koop. - 08/09/2010 8:02:04
' Generado por MyGeneration Version # (1.3.0.3)
' Generado desde Batz_VbNet_SQL_dOOdads_BusinessEntity.vbgen
' El soporte de la clase OracleClientEntity esta en el directorio Architecture  en "dOOdads".
'===============================================================================

Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Specialized
Imports AccesoAutomaticoBD


NameSpace DAL

Public MustInherit Class _EXTENSION
	Inherits OracleClientEntity

		Public Sub New() 
			Me.SchemaGlobal = "TELEFONIA."
			Me.QuerySource = "EXTENSION"
			Me.MappingName = "EXTENSION"
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
		Me._whereClause = nothing
		Me._aggregateClause = nothing		
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
		
		
    	Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_EXTENSION", parameters)		
	End Function

	'=================================================================
	' Public Overridable Function LoadByPrimaryKey()  As Boolean
	'=================================================================
	'  Loads a single row of via the primary key
	'=================================================================
	Public Overridable Function LoadByPrimaryKey(ByVal ID As Decimal) As Boolean

		Dim parameters As ListDictionary = New ListDictionary()
		parameters.Add(_EXTENSION.Parameters.ID, ID)


		Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
		p.Direction = ParameterDirection.Output
		parameters.Add(p, Nothing)
				
		Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_EXTENSION", parameters)

	End Function

	#Region "Parameters"
	Protected class Parameters 
		
		Public Shared ReadOnly Property ID As OracleParameter
			Get
				Return New OracleParameter("p_ID", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property EXTENSION As OracleParameter
			Get
				Return New OracleParameter("p_EXTENSION", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property NOMBRE As OracleParameter
			Get
				Return New OracleParameter("p_NOMBRE", OracleDbType.VARCHAR2, 4000)
			End Get
		End Property
		
		Public Shared ReadOnly Property ID_ALVEOLO As OracleParameter
			Get
				Return New OracleParameter("p_ID_ALVEOLO", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property ID_TIPOEXT As OracleParameter
			Get
				Return New OracleParameter("p_ID_TIPOEXT", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property ID_PLANTA As OracleParameter
			Get
				Return New OracleParameter("p_ID_PLANTA", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property VISIBLE As OracleParameter
			Get
				Return New OracleParameter("p_VISIBLE", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property ID_TELEFONO As OracleParameter
			Get
				Return New OracleParameter("p_ID_TELEFONO", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property ID_EXT_INTERNA As OracleParameter
			Get
				Return New OracleParameter("p_ID_EXT_INTERNA", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property ID_TIPOASIG As OracleParameter
			Get
				Return New OracleParameter("p_ID_TIPOASIG", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property ID_DEPART_FAC As OracleParameter
			Get
				Return New OracleParameter("p_ID_DEPART_FAC", OracleDbType.VARCHAR2, 10)
			End Get
		End Property
		
		Public Shared ReadOnly Property ID_TIPOLINEA As OracleParameter
			Get
				Return New OracleParameter("p_ID_TIPOLINEA", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property OBSOLETO As OracleParameter
			Get
				Return New OracleParameter("p_OBSOLETO", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property PRESTAMO As OracleParameter
			Get
				Return New OracleParameter("p_PRESTAMO", OracleDbType.Int32, 0)
			End Get
		End Property
		
	End Class
	#End Region	

	#Region "ColumnNames"
	Public class ColumnNames
		
        Public Const ID As String = "ID"
        Public Const EXTENSION As String = "EXTENSION"
        Public Const NOMBRE As String = "NOMBRE"
        Public Const ID_ALVEOLO As String = "ID_ALVEOLO"
        Public Const ID_TIPOEXT As String = "ID_TIPOEXT"
        Public Const ID_PLANTA As String = "ID_PLANTA"
        Public Const VISIBLE As String = "VISIBLE"
        Public Const ID_TELEFONO As String = "ID_TELEFONO"
        Public Const ID_EXT_INTERNA As String = "ID_EXT_INTERNA"
        Public Const ID_TIPOASIG As String = "ID_TIPOASIG"
        Public Const ID_DEPART_FAC As String = "ID_DEPART_FAC"
        Public Const ID_TIPOLINEA As String = "ID_TIPOLINEA"
        Public Const OBSOLETO As String = "OBSOLETO"
		Public Const PRESTAMO As String = "PRESTAMO"

		Shared Public Function ToPropertyName(ByVal columnName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(ID) = _EXTENSION.PropertyNames.ID
				ht(EXTENSION) = _EXTENSION.PropertyNames.EXTENSION
				ht(NOMBRE) = _EXTENSION.PropertyNames.NOMBRE
				ht(ID_ALVEOLO) = _EXTENSION.PropertyNames.ID_ALVEOLO
				ht(ID_TIPOEXT) = _EXTENSION.PropertyNames.ID_TIPOEXT
				ht(ID_PLANTA) = _EXTENSION.PropertyNames.ID_PLANTA
				ht(VISIBLE) = _EXTENSION.PropertyNames.VISIBLE
				ht(ID_TELEFONO) = _EXTENSION.PropertyNames.ID_TELEFONO
				ht(ID_EXT_INTERNA) = _EXTENSION.PropertyNames.ID_EXT_INTERNA
				ht(ID_TIPOASIG) = _EXTENSION.PropertyNames.ID_TIPOASIG
				ht(ID_DEPART_FAC) = _EXTENSION.PropertyNames.ID_DEPART_FAC
				ht(ID_TIPOLINEA) = _EXTENSION.PropertyNames.ID_TIPOLINEA
				ht(OBSOLETO) = _EXTENSION.PropertyNames.OBSOLETO
				ht(PRESTAMO) = _EXTENSION.PropertyNames.PRESTAMO
			End If
			
			Return CType(ht(columnName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing		 
	End Class
	#End Region	
	
	#Region "PropertyNames"
	Public class PropertyNames
		
        Public Const ID As String = "ID"
        Public Const EXTENSION As String = "EXTENSION"
        Public Const NOMBRE As String = "NOMBRE"
        Public Const ID_ALVEOLO As String = "ID_ALVEOLO"
        Public Const ID_TIPOEXT As String = "ID_TIPOEXT"
        Public Const ID_PLANTA As String = "ID_PLANTA"
        Public Const VISIBLE As String = "VISIBLE"
        Public Const ID_TELEFONO As String = "ID_TELEFONO"
        Public Const ID_EXT_INTERNA As String = "ID_EXT_INTERNA"
        Public Const ID_TIPOASIG As String = "ID_TIPOASIG"
        Public Const ID_DEPART_FAC As String = "ID_DEPART_FAC"
        Public Const ID_TIPOLINEA As String = "ID_TIPOLINEA"
        Public Const OBSOLETO As String = "OBSOLETO"
		Public Const PRESTAMO As string="PRESTAMO"

		Shared Public Function ToColumnName(ByVal propertyName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(ID) = _EXTENSION.ColumnNames.ID
				ht(EXTENSION) = _EXTENSION.ColumnNames.EXTENSION
				ht(NOMBRE) = _EXTENSION.ColumnNames.NOMBRE
				ht(ID_ALVEOLO) = _EXTENSION.ColumnNames.ID_ALVEOLO
				ht(ID_TIPOEXT) = _EXTENSION.ColumnNames.ID_TIPOEXT
				ht(ID_PLANTA) = _EXTENSION.ColumnNames.ID_PLANTA
				ht(VISIBLE) = _EXTENSION.ColumnNames.VISIBLE
				ht(ID_TELEFONO) = _EXTENSION.ColumnNames.ID_TELEFONO
				ht(ID_EXT_INTERNA) = _EXTENSION.ColumnNames.ID_EXT_INTERNA
				ht(ID_TIPOASIG) = _EXTENSION.ColumnNames.ID_TIPOASIG
				ht(ID_DEPART_FAC) = _EXTENSION.ColumnNames.ID_DEPART_FAC
				ht(ID_TIPOLINEA) = _EXTENSION.ColumnNames.ID_TIPOLINEA
				ht(OBSOLETO) = _EXTENSION.ColumnNames.OBSOLETO
				ht(PRESTAMO) = _EXTENSION.ColumnNames.PRESTAMO
			End If
			
			Return CType(ht(propertyName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing
		
	End Class
	#End Region	
	
	#Region "StringPropertyNames"
	Public class StringPropertyNames
		
        Public Const ID As String = "s_ID"
        Public Const EXTENSION As String = "s_EXTENSION"
        Public Const NOMBRE As String = "s_NOMBRE"
        Public Const ID_ALVEOLO As String = "s_ID_ALVEOLO"
        Public Const ID_TIPOEXT As String = "s_ID_TIPOEXT"
        Public Const ID_PLANTA As String = "s_ID_PLANTA"
        Public Const VISIBLE As String = "s_VISIBLE"
        Public Const ID_TELEFONO As String = "s_ID_TELEFONO"
        Public Const ID_EXT_INTERNA As String = "s_ID_EXT_INTERNA"
        Public Const ID_TIPOASIG As String = "s_ID_TIPOASIG"
        Public Const ID_DEPART_FAC As String = "s_ID_DEPART_FAC"
        Public Const ID_TIPOLINEA As String = "s_ID_TIPOLINEA"
        Public Const OBSOLETO As String = "s_OBSOLETO"
		Public Const PRESTAMO As String = "s_PRESTAMO"
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

		Public Overridable Property EXTENSION As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.EXTENSION)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.EXTENSION, Value)
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

		Public Overridable Property ID_ALVEOLO As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ID_ALVEOLO)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ID_ALVEOLO, Value)
			End Set
		End Property

		Public Overridable Property ID_TIPOEXT As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ID_TIPOEXT)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ID_TIPOEXT, Value)
			End Set
		End Property

		Public Overridable Property ID_PLANTA As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ID_PLANTA)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ID_PLANTA, Value)
			End Set
		End Property

		Public Overridable Property VISIBLE As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.VISIBLE)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.VISIBLE, Value)
			End Set
		End Property

		Public Overridable Property ID_TELEFONO As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ID_TELEFONO)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ID_TELEFONO, Value)
			End Set
		End Property

		Public Overridable Property ID_EXT_INTERNA As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ID_EXT_INTERNA)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ID_EXT_INTERNA, Value)
			End Set
		End Property

		Public Overridable Property ID_TIPOASIG As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ID_TIPOASIG)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ID_TIPOASIG, Value)
			End Set
		End Property

		Public Overridable Property ID_DEPART_FAC As String
			Get
				Return MyBase.GetString(ColumnNames.ID_DEPART_FAC)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.ID_DEPART_FAC, Value)
			End Set
		End Property

		Public Overridable Property ID_TIPOLINEA As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ID_TIPOLINEA)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ID_TIPOLINEA, Value)
			End Set
		End Property

		Public Overridable Property OBSOLETO As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.OBSOLETO)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.OBSOLETO, Value)
			End Set
		End Property

		Public Overridable Property PRESTAMO As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.PRESTAMO)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.PRESTAMO, Value)
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
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ID)
				Else
					Me.ID = MyBase.SetDecimalAsString(ColumnNames.ID, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_EXTENSION As String
			Get
				If Me.IsColumnNull(ColumnNames.EXTENSION) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.EXTENSION)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.EXTENSION)
				Else
					Me.EXTENSION = MyBase.SetDecimalAsString(ColumnNames.EXTENSION, Value)
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
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.NOMBRE)
				Else
					Me.NOMBRE = MyBase.SetStringAsString(ColumnNames.NOMBRE, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_ID_ALVEOLO As String
			Get
				If Me.IsColumnNull(ColumnNames.ID_ALVEOLO) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.ID_ALVEOLO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ID_ALVEOLO)
				Else
					Me.ID_ALVEOLO = MyBase.SetDecimalAsString(ColumnNames.ID_ALVEOLO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_ID_TIPOEXT As String
			Get
				If Me.IsColumnNull(ColumnNames.ID_TIPOEXT) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.ID_TIPOEXT)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ID_TIPOEXT)
				Else
					Me.ID_TIPOEXT = MyBase.SetDecimalAsString(ColumnNames.ID_TIPOEXT, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_ID_PLANTA As String
			Get
				If Me.IsColumnNull(ColumnNames.ID_PLANTA) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.ID_PLANTA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ID_PLANTA)
				Else
					Me.ID_PLANTA = MyBase.SetDecimalAsString(ColumnNames.ID_PLANTA, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_VISIBLE As String
			Get
				If Me.IsColumnNull(ColumnNames.VISIBLE) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.VISIBLE)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.VISIBLE)
				Else
					Me.VISIBLE = MyBase.SetDecimalAsString(ColumnNames.VISIBLE, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_ID_TELEFONO As String
			Get
				If Me.IsColumnNull(ColumnNames.ID_TELEFONO) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.ID_TELEFONO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ID_TELEFONO)
				Else
					Me.ID_TELEFONO = MyBase.SetDecimalAsString(ColumnNames.ID_TELEFONO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_ID_EXT_INTERNA As String
			Get
				If Me.IsColumnNull(ColumnNames.ID_EXT_INTERNA) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.ID_EXT_INTERNA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ID_EXT_INTERNA)
				Else
					Me.ID_EXT_INTERNA = MyBase.SetDecimalAsString(ColumnNames.ID_EXT_INTERNA, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_ID_TIPOASIG As String
			Get
				If Me.IsColumnNull(ColumnNames.ID_TIPOASIG) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.ID_TIPOASIG)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ID_TIPOASIG)
				Else
					Me.ID_TIPOASIG = MyBase.SetDecimalAsString(ColumnNames.ID_TIPOASIG, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_ID_DEPART_FAC As String
			Get
				If Me.IsColumnNull(ColumnNames.ID_DEPART_FAC) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.ID_DEPART_FAC)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ID_DEPART_FAC)
				Else
					Me.ID_DEPART_FAC = MyBase.SetStringAsString(ColumnNames.ID_DEPART_FAC, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_ID_TIPOLINEA As String
			Get
				If Me.IsColumnNull(ColumnNames.ID_TIPOLINEA) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.ID_TIPOLINEA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ID_TIPOLINEA)
				Else
					Me.ID_TIPOLINEA = MyBase.SetDecimalAsString(ColumnNames.ID_TIPOLINEA, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_OBSOLETO As String
			Get
				If Me.IsColumnNull(ColumnNames.OBSOLETO) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.OBSOLETO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.OBSOLETO)
				Else
					Me.OBSOLETO = MyBase.SetDecimalAsString(ColumnNames.OBSOLETO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_PRESTAMO As String
			Get
				If Me.IsColumnNull(ColumnNames.PRESTAMO) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.PRESTAMO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.PRESTAMO)
				Else
					Me.PRESTAMO = MyBase.SetDecimalAsString(ColumnNames.PRESTAMO, Value)
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
					_tearOff = new TearOffWhereParameter(Me)
				End If

				Return _tearOff
			End Get
		End Property

		#Region "TearOff's"
		Public class TearOffWhereParameter

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

			Public ReadOnly Property EXTENSION() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.EXTENSION, Parameters.EXTENSION)
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

			Public ReadOnly Property ID_ALVEOLO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_ALVEOLO, Parameters.ID_ALVEOLO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property ID_TIPOEXT() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_TIPOEXT, Parameters.ID_TIPOEXT)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property ID_PLANTA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_PLANTA, Parameters.ID_PLANTA)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property VISIBLE() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.VISIBLE, Parameters.VISIBLE)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property ID_TELEFONO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_TELEFONO, Parameters.ID_TELEFONO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property ID_EXT_INTERNA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_EXT_INTERNA, Parameters.ID_EXT_INTERNA)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property ID_TIPOASIG() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_TIPOASIG, Parameters.ID_TIPOASIG)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property ID_DEPART_FAC() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_DEPART_FAC, Parameters.ID_DEPART_FAC)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property ID_TIPOLINEA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_TIPOLINEA, Parameters.ID_TIPOLINEA)
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

			Public ReadOnly Property PRESTAMO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.PRESTAMO, Parameters.PRESTAMO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Private _clause as WhereClause
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

		Public ReadOnly Property EXTENSION() As WhereParameter 
			Get
				If _EXTENSION_W Is Nothing Then
					_EXTENSION_W = TearOff.EXTENSION
				End If
				Return _EXTENSION_W
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

		Public ReadOnly Property ID_ALVEOLO() As WhereParameter 
			Get
				If _ID_ALVEOLO_W Is Nothing Then
					_ID_ALVEOLO_W = TearOff.ID_ALVEOLO
				End If
				Return _ID_ALVEOLO_W
			End Get
		End Property

		Public ReadOnly Property ID_TIPOEXT() As WhereParameter 
			Get
				If _ID_TIPOEXT_W Is Nothing Then
					_ID_TIPOEXT_W = TearOff.ID_TIPOEXT
				End If
				Return _ID_TIPOEXT_W
			End Get
		End Property

		Public ReadOnly Property ID_PLANTA() As WhereParameter 
			Get
				If _ID_PLANTA_W Is Nothing Then
					_ID_PLANTA_W = TearOff.ID_PLANTA
				End If
				Return _ID_PLANTA_W
			End Get
		End Property

		Public ReadOnly Property VISIBLE() As WhereParameter 
			Get
				If _VISIBLE_W Is Nothing Then
					_VISIBLE_W = TearOff.VISIBLE
				End If
				Return _VISIBLE_W
			End Get
		End Property

		Public ReadOnly Property ID_TELEFONO() As WhereParameter 
			Get
				If _ID_TELEFONO_W Is Nothing Then
					_ID_TELEFONO_W = TearOff.ID_TELEFONO
				End If
				Return _ID_TELEFONO_W
			End Get
		End Property

		Public ReadOnly Property ID_EXT_INTERNA() As WhereParameter 
			Get
				If _ID_EXT_INTERNA_W Is Nothing Then
					_ID_EXT_INTERNA_W = TearOff.ID_EXT_INTERNA
				End If
				Return _ID_EXT_INTERNA_W
			End Get
		End Property

		Public ReadOnly Property ID_TIPOASIG() As WhereParameter 
			Get
				If _ID_TIPOASIG_W Is Nothing Then
					_ID_TIPOASIG_W = TearOff.ID_TIPOASIG
				End If
				Return _ID_TIPOASIG_W
			End Get
		End Property

		Public ReadOnly Property ID_DEPART_FAC() As WhereParameter 
			Get
				If _ID_DEPART_FAC_W Is Nothing Then
					_ID_DEPART_FAC_W = TearOff.ID_DEPART_FAC
				End If
				Return _ID_DEPART_FAC_W
			End Get
		End Property

		Public ReadOnly Property ID_TIPOLINEA() As WhereParameter 
			Get
				If _ID_TIPOLINEA_W Is Nothing Then
					_ID_TIPOLINEA_W = TearOff.ID_TIPOLINEA
				End If
				Return _ID_TIPOLINEA_W
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
		
		Public ReadOnly Property PRESTAMO() As WhereParameter 
			Get
				If _PRESTAMO_W Is Nothing Then
					_PRESTAMO_W = TearOff.PRESTAMO
				End If
				Return _PRESTAMO_W
			End Get
		End Property

		Private _ID_W As WhereParameter = Nothing
		Private _EXTENSION_W As WhereParameter = Nothing
		Private _NOMBRE_W As WhereParameter = Nothing
		Private _ID_ALVEOLO_W As WhereParameter = Nothing
		Private _ID_TIPOEXT_W As WhereParameter = Nothing
		Private _ID_PLANTA_W As WhereParameter = Nothing
		Private _VISIBLE_W As WhereParameter = Nothing
		Private _ID_TELEFONO_W As WhereParameter = Nothing
		Private _ID_EXT_INTERNA_W As WhereParameter = Nothing
		Private _ID_TIPOASIG_W As WhereParameter = Nothing
		Private _ID_DEPART_FAC_W As WhereParameter = Nothing
		Private _ID_TIPOLINEA_W As WhereParameter = Nothing
		Private _OBSOLETO_W As WhereParameter = Nothing
		Private _PRESTAMO_W As WhereParameter = Nothing
		
			Public Sub WhereClauseReset()

			_ID_W = Nothing
			_EXTENSION_W = Nothing
			_NOMBRE_W = Nothing
			_ID_ALVEOLO_W = Nothing
			_ID_TIPOEXT_W = Nothing
			_ID_PLANTA_W = Nothing
			_VISIBLE_W = Nothing
			_ID_TELEFONO_W = Nothing
			_ID_EXT_INTERNA_W = Nothing
			_ID_TIPOASIG_W = Nothing
			_ID_DEPART_FAC_W = Nothing
			_ID_TIPOLINEA_W = Nothing
			_OBSOLETO_W = Nothing
			_PRESTAMO_W = Nothing
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
					_tearOff = new TearOffAggregateParameter(Me)
				End If

				Return _tearOff
			End Get
		End Property

		#Region "AggregateParameter TearOff's"
		Public class TearOffAggregateParameter

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

		Public ReadOnly Property EXTENSION() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.EXTENSION, Parameters.EXTENSION)
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

		Public ReadOnly Property ID_ALVEOLO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_ALVEOLO, Parameters.ID_ALVEOLO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property ID_TIPOEXT() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_TIPOEXT, Parameters.ID_TIPOEXT)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property ID_PLANTA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_PLANTA, Parameters.ID_PLANTA)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property VISIBLE() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.VISIBLE, Parameters.VISIBLE)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property ID_TELEFONO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_TELEFONO, Parameters.ID_TELEFONO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property ID_EXT_INTERNA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_EXT_INTERNA, Parameters.ID_EXT_INTERNA)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property ID_TIPOASIG() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_TIPOASIG, Parameters.ID_TIPOASIG)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property ID_DEPART_FAC() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_DEPART_FAC, Parameters.ID_DEPART_FAC)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property ID_TIPOLINEA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_TIPOLINEA, Parameters.ID_TIPOLINEA)
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

		Public ReadOnly Property PRESTAMO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.PRESTAMO, Parameters.PRESTAMO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

			Private _clause as AggregateClause
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

		Public ReadOnly Property EXTENSION() As AggregateParameter 
			Get
				If _EXTENSION_W Is Nothing Then
					_EXTENSION_W = TearOff.EXTENSION
				End If
				Return _EXTENSION_W
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

		Public ReadOnly Property ID_ALVEOLO() As AggregateParameter 
			Get
				If _ID_ALVEOLO_W Is Nothing Then
					_ID_ALVEOLO_W = TearOff.ID_ALVEOLO
				End If
				Return _ID_ALVEOLO_W
			End Get
		End Property

		Public ReadOnly Property ID_TIPOEXT() As AggregateParameter 
			Get
				If _ID_TIPOEXT_W Is Nothing Then
					_ID_TIPOEXT_W = TearOff.ID_TIPOEXT
				End If
				Return _ID_TIPOEXT_W
			End Get
		End Property

		Public ReadOnly Property ID_PLANTA() As AggregateParameter 
			Get
				If _ID_PLANTA_W Is Nothing Then
					_ID_PLANTA_W = TearOff.ID_PLANTA
				End If
				Return _ID_PLANTA_W
			End Get
		End Property

		Public ReadOnly Property VISIBLE() As AggregateParameter 
			Get
				If _VISIBLE_W Is Nothing Then
					_VISIBLE_W = TearOff.VISIBLE
				End If
				Return _VISIBLE_W
			End Get
		End Property

		Public ReadOnly Property ID_TELEFONO() As AggregateParameter 
			Get
				If _ID_TELEFONO_W Is Nothing Then
					_ID_TELEFONO_W = TearOff.ID_TELEFONO
				End If
				Return _ID_TELEFONO_W
			End Get
		End Property

		Public ReadOnly Property ID_EXT_INTERNA() As AggregateParameter 
			Get
				If _ID_EXT_INTERNA_W Is Nothing Then
					_ID_EXT_INTERNA_W = TearOff.ID_EXT_INTERNA
				End If
				Return _ID_EXT_INTERNA_W
			End Get
		End Property

		Public ReadOnly Property ID_TIPOASIG() As AggregateParameter 
			Get
				If _ID_TIPOASIG_W Is Nothing Then
					_ID_TIPOASIG_W = TearOff.ID_TIPOASIG
				End If
				Return _ID_TIPOASIG_W
			End Get
		End Property

		Public ReadOnly Property ID_DEPART_FAC() As AggregateParameter 
			Get
				If _ID_DEPART_FAC_W Is Nothing Then
					_ID_DEPART_FAC_W = TearOff.ID_DEPART_FAC
				End If
				Return _ID_DEPART_FAC_W
			End Get
		End Property

		Public ReadOnly Property ID_TIPOLINEA() As AggregateParameter 
			Get
				If _ID_TIPOLINEA_W Is Nothing Then
					_ID_TIPOLINEA_W = TearOff.ID_TIPOLINEA
				End If
				Return _ID_TIPOLINEA_W
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
		
		Public ReadOnly Property PRESTAMO() As AggregateParameter 
			Get
				If _PRESTAMO_W Is Nothing Then
					_PRESTAMO_W = TearOff.VISIBLE
				End If
				Return _PRESTAMO_W
			End Get
		End Property

		Private _ID_W As AggregateParameter = Nothing
		Private _EXTENSION_W As AggregateParameter = Nothing
		Private _NOMBRE_W As AggregateParameter = Nothing
		Private _ID_ALVEOLO_W As AggregateParameter = Nothing
		Private _ID_TIPOEXT_W As AggregateParameter = Nothing
		Private _ID_PLANTA_W As AggregateParameter = Nothing
		Private _VISIBLE_W As AggregateParameter = Nothing
		Private _ID_TELEFONO_W As AggregateParameter = Nothing
		Private _ID_EXT_INTERNA_W As AggregateParameter = Nothing
		Private _ID_TIPOASIG_W As AggregateParameter = Nothing
		Private _ID_DEPART_FAC_W As AggregateParameter = Nothing
		Private _ID_TIPOLINEA_W As AggregateParameter = Nothing
		Private _OBSOLETO_W As AggregateParameter = Nothing
		Private _PRESTAMO_W As AggregateParameter = Nothing

		Public Sub AggregateClauseReset()

		_ID_W = Nothing
		_EXTENSION_W = Nothing
		_NOMBRE_W = Nothing
		_ID_ALVEOLO_W = Nothing
		_ID_TIPOEXT_W = Nothing
		_ID_PLANTA_W = Nothing
		_VISIBLE_W = Nothing
		_ID_TELEFONO_W = Nothing
		_ID_EXT_INTERNA_W = Nothing
		_ID_TIPOASIG_W = Nothing
		_ID_DEPART_FAC_W = Nothing
		_ID_TIPOLINEA_W = Nothing
		_OBSOLETO_W = Nothing
		_PRESTAMO_W = Nothing
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
		cmd.CommandText = Me.SchemaStoredProcedure + "PI_EXTENSION" 
	    
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
		cmd.CommandText = Me.SchemaStoredProcedure + "PU_EXTENSION" 
		
		CreateParameters(cmd) 
		    
		Return cmd
	
	End Function	
	
	Protected Overrides Function GetDeleteCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PD_EXTENSION" 
		
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

		p = cmd.Parameters.Add(Parameters.EXTENSION)
		p.SourceColumn = ColumnNames.EXTENSION
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.NOMBRE)
		p.SourceColumn = ColumnNames.NOMBRE
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID_ALVEOLO)
		p.SourceColumn = ColumnNames.ID_ALVEOLO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID_TIPOEXT)
		p.SourceColumn = ColumnNames.ID_TIPOEXT
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID_PLANTA)
		p.SourceColumn = ColumnNames.ID_PLANTA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.VISIBLE)
		p.SourceColumn = ColumnNames.VISIBLE
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID_TELEFONO)
		p.SourceColumn = ColumnNames.ID_TELEFONO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID_EXT_INTERNA)
		p.SourceColumn = ColumnNames.ID_EXT_INTERNA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID_TIPOASIG)
		p.SourceColumn = ColumnNames.ID_TIPOASIG
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID_DEPART_FAC)
		p.SourceColumn = ColumnNames.ID_DEPART_FAC
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID_TIPOLINEA)
		p.SourceColumn = ColumnNames.ID_TIPOLINEA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.OBSOLETO)
		p.SourceColumn = ColumnNames.OBSOLETO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.PRESTAMO)
		p.SourceColumn = ColumnNames.PRESTAMO
		p.SourceVersion = DataRowVersion.Current

	End Sub	

End Class

End NameSpace

