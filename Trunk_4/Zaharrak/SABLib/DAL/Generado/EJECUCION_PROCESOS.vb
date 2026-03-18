
'===============================================================================
'BATZ, Koop. - 25/05/2010 10:24:04
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

Public MustInherit Class _EJECUCION_PROCESOS
	Inherits OracleClientEntity

		Public Sub New() 
			Me.SchemaGlobal = "SAB."
			Me.QuerySource = "EJECUCION_PROCESOS"
			Me.MappingName = "EJECUCION_PROCESOS"
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
		
		
    	Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_EJECUCION_PROCESOS", parameters)		
	End Function

	'=================================================================
	' Public Overridable Function LoadByPrimaryKey()  As Boolean
	'=================================================================
	'  Loads a single row of via the primary key
	'=================================================================
	Public Overridable Function LoadByPrimaryKey(ByVal ID As Decimal) As Boolean

		Dim parameters As ListDictionary = New ListDictionary()
		parameters.Add(_EJECUCION_PROCESOS.Parameters.ID, ID)


		Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
		p.Direction = ParameterDirection.Output
		parameters.Add(p, Nothing)
				
		Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_EJECUCION_PROCESOS", parameters)

	End Function

	#Region "Parameters"
	Protected class Parameters 
		
		Public Shared ReadOnly Property ID As OracleParameter
			Get
				Return New OracleParameter("p_ID", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property ID_USUARIO As OracleParameter
			Get
				Return New OracleParameter("p_ID_USUARIO", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property FECHA As OracleParameter
			Get
				Return New OracleParameter("p_FECHA", OracleDbType.Date, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property FLAG As OracleParameter
			Get
				Return New OracleParameter("p_FLAG", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property DESCRIPCION As OracleParameter
			Get
				Return New OracleParameter("p_DESCRIPCION", OracleDbType.VARCHAR2, 100)
			End Get
		End Property
		
	End Class
	#End Region	

	#Region "ColumnNames"
	Public class ColumnNames
		
        Public Const ID As String = "ID"
        Public Const ID_USUARIO As String = "ID_USUARIO"
        Public Const FECHA As String = "FECHA"
        Public Const FLAG As String = "FLAG"
        Public Const DESCRIPCION As String = "DESCRIPCION"

		Shared Public Function ToPropertyName(ByVal columnName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(ID) = _EJECUCION_PROCESOS.PropertyNames.ID
				ht(ID_USUARIO) = _EJECUCION_PROCESOS.PropertyNames.ID_USUARIO
				ht(FECHA) = _EJECUCION_PROCESOS.PropertyNames.FECHA
				ht(FLAG) = _EJECUCION_PROCESOS.PropertyNames.FLAG
				ht(DESCRIPCION) = _EJECUCION_PROCESOS.PropertyNames.DESCRIPCION

			End If
			
			Return CType(ht(columnName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing		 
	End Class
	#End Region	
	
	#Region "PropertyNames"
	Public class PropertyNames
		
        Public Const ID As String = "ID"
        Public Const ID_USUARIO As String = "ID_USUARIO"
        Public Const FECHA As String = "FECHA"
        Public Const FLAG As String = "FLAG"
        Public Const DESCRIPCION As String = "DESCRIPCION"

		Shared Public Function ToColumnName(ByVal propertyName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(ID) = _EJECUCION_PROCESOS.ColumnNames.ID
				ht(ID_USUARIO) = _EJECUCION_PROCESOS.ColumnNames.ID_USUARIO
				ht(FECHA) = _EJECUCION_PROCESOS.ColumnNames.FECHA
				ht(FLAG) = _EJECUCION_PROCESOS.ColumnNames.FLAG
				ht(DESCRIPCION) = _EJECUCION_PROCESOS.ColumnNames.DESCRIPCION

			End If
			
			Return CType(ht(propertyName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing
		
	End Class
	#End Region	
	
	#Region "StringPropertyNames"
	Public class StringPropertyNames
		
        Public Const ID As String = "s_ID"
        Public Const ID_USUARIO As String = "s_ID_USUARIO"
        Public Const FECHA As String = "s_FECHA"
        Public Const FLAG As String = "s_FLAG"
        Public Const DESCRIPCION As String = "s_DESCRIPCION"

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

		Public Overridable Property ID_USUARIO As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ID_USUARIO)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ID_USUARIO, Value)
			End Set
		End Property

		Public Overridable Property FECHA As DateTime
			Get
				Return MyBase.GetDateTime(ColumnNames.FECHA)
			End Get
			Set(ByVal Value As DateTime)
				MyBase.SetDateTime(ColumnNames.FECHA, Value)
			End Set
		End Property

		Public Overridable Property FLAG As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.FLAG)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.FLAG, Value)
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

		Public Overridable Property s_ID_USUARIO As String
			Get
				If Me.IsColumnNull(ColumnNames.ID_USUARIO) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.ID_USUARIO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ID_USUARIO)
				Else
					Me.ID_USUARIO = MyBase.SetDecimalAsString(ColumnNames.ID_USUARIO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_FECHA As String
			Get
				If Me.IsColumnNull(ColumnNames.FECHA) Then
					Return String.Empty
				Else
					Return MyBase.GetDateTimeAsString(ColumnNames.FECHA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.FECHA)
				Else
					Me.FECHA = MyBase.SetDateTimeAsString(ColumnNames.FECHA, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_FLAG As String
			Get
				If Me.IsColumnNull(ColumnNames.FLAG) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.FLAG)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.FLAG)
				Else
					Me.FLAG = MyBase.SetDecimalAsString(ColumnNames.FLAG, Value)
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
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.DESCRIPCION)
				Else
					Me.DESCRIPCION = MyBase.SetStringAsString(ColumnNames.DESCRIPCION, Value)
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

			Public ReadOnly Property ID_USUARIO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_USUARIO, Parameters.ID_USUARIO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property FECHA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.FECHA, Parameters.FECHA)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property FLAG() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.FLAG, Parameters.FLAG)
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

		Public ReadOnly Property ID_USUARIO() As WhereParameter 
			Get
				If _ID_USUARIO_W Is Nothing Then
					_ID_USUARIO_W = TearOff.ID_USUARIO
				End If
				Return _ID_USUARIO_W
			End Get
		End Property

		Public ReadOnly Property FECHA() As WhereParameter 
			Get
				If _FECHA_W Is Nothing Then
					_FECHA_W = TearOff.FECHA
				End If
				Return _FECHA_W
			End Get
		End Property

		Public ReadOnly Property FLAG() As WhereParameter 
			Get
				If _FLAG_W Is Nothing Then
					_FLAG_W = TearOff.FLAG
				End If
				Return _FLAG_W
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

		Private _ID_W As WhereParameter = Nothing
		Private _ID_USUARIO_W As WhereParameter = Nothing
		Private _FECHA_W As WhereParameter = Nothing
		Private _FLAG_W As WhereParameter = Nothing
		Private _DESCRIPCION_W As WhereParameter = Nothing

			Public Sub WhereClauseReset()

			_ID_W = Nothing
			_ID_USUARIO_W = Nothing
			_FECHA_W = Nothing
			_FLAG_W = Nothing
			_DESCRIPCION_W = Nothing
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

		Public ReadOnly Property ID_USUARIO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_USUARIO, Parameters.ID_USUARIO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property FECHA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FECHA, Parameters.FECHA)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property FLAG() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.FLAG, Parameters.FLAG)
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

		Public ReadOnly Property ID_USUARIO() As AggregateParameter 
			Get
				If _ID_USUARIO_W Is Nothing Then
					_ID_USUARIO_W = TearOff.ID_USUARIO
				End If
				Return _ID_USUARIO_W
			End Get
		End Property

		Public ReadOnly Property FECHA() As AggregateParameter 
			Get
				If _FECHA_W Is Nothing Then
					_FECHA_W = TearOff.FECHA
				End If
				Return _FECHA_W
			End Get
		End Property

		Public ReadOnly Property FLAG() As AggregateParameter 
			Get
				If _FLAG_W Is Nothing Then
					_FLAG_W = TearOff.FLAG
				End If
				Return _FLAG_W
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

		Private _ID_W As AggregateParameter = Nothing
		Private _ID_USUARIO_W As AggregateParameter = Nothing
		Private _FECHA_W As AggregateParameter = Nothing
		Private _FLAG_W As AggregateParameter = Nothing
		Private _DESCRIPCION_W As AggregateParameter = Nothing

		Public Sub AggregateClauseReset()

		_ID_W = Nothing
		_ID_USUARIO_W = Nothing
		_FECHA_W = Nothing
		_FLAG_W = Nothing
		_DESCRIPCION_W = Nothing
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
		cmd.CommandText = Me.SchemaStoredProcedure + "PI_EJECUCION_PROCESOS" 
	    
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
		cmd.CommandText = Me.SchemaStoredProcedure + "PU_EJECUCION_PROCESOS" 
		
		CreateParameters(cmd) 
		    
		Return cmd
	
	End Function	
	
	Protected Overrides Function GetDeleteCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PD_EJECUCION_PROCESOS" 
		
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

		p = cmd.Parameters.Add(Parameters.ID_USUARIO)
		p.SourceColumn = ColumnNames.ID_USUARIO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.FECHA)
		p.SourceColumn = ColumnNames.FECHA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.FLAG)
		p.SourceColumn = ColumnNames.FLAG
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.DESCRIPCION)
		p.SourceColumn = ColumnNames.DESCRIPCION
		p.SourceVersion = DataRowVersion.Current


	End Sub	

End Class

End NameSpace

