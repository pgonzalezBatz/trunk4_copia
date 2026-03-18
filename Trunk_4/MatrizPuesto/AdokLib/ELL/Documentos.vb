Namespace ELL

    Public Class CEtico

        Private _planta As Integer = Integer.MinValue
        Private _id As Integer = Integer.MinValue
        Private _plantaDesc As String = String.Empty
        Private _idTra As String = String.Empty
        Private _fecha As Date = Date.MinValue
        Private _fechaMod As Date = Date.MinValue
        Private _codCategoria As String = String.Empty
        Private _comentario As String = String.Empty
        Private _comentariocorto As String = String.Empty
        Private _Accion As Integer = Integer.MinValue
        Private _email As String = String.Empty
        Private _traduccion As String = String.Empty
        Private _traduccioncorto As String = String.Empty
        Private _cierre As Integer = Integer.MinValue
        Private _campo2 As String = String.Empty
        Private _campo1 As String = String.Empty
        Private _sel1 As String = String.Empty



#Region "Enumeracion"

        Public Enum TipoJornada As Integer
            Partida = 0
            Intensiva = 1
        End Enum

#End Region

        ''' <summary>
        ''' Identificador del Empleado
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property planta() As Integer
            Get
                Return _planta
            End Get
            Set(ByVal value As Integer)
                _planta = value
            End Set
        End Property
        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property
        Public Property campo1() As String
            Get
                Return _campo1
            End Get
            Set(ByVal value As String)
                _campo1 = value
            End Set
        End Property
        Public Property campo2() As String
            Get
                Return _campo2
            End Get
            Set(ByVal value As String)
                _campo2 = value
            End Set
        End Property
        Public Property plantaDesc() As String
            Get
                Return _plantaDesc
            End Get
            Set(ByVal value As String)
                _plantaDesc = value
            End Set
        End Property
        Public Property Idtra() As String
            Get
                Return _idTra
            End Get
            Set(ByVal value As String)
                _idTra = value
            End Set
        End Property

        Public Property codCategoria() As String
            Get
                Return _codCategoria
            End Get
            Set(ByVal value As String)
                _codCategoria = value
            End Set
        End Property
        Public Property Accion() As Integer
            Get
                Return _Accion
            End Get
            Set(ByVal value As Integer)
                _Accion = value
            End Set
        End Property
        ''' <summary>
        ''' Fecha del registro
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Fecha() As Date
            Get
                Return _fecha
            End Get
            Set(ByVal value As Date)
                _fecha = value
            End Set
        End Property
        Public Property FechaMod() As Date
            Get
                Return _fechaMod
            End Get
            Set(ByVal value As Date)
                _fechaMod = value
            End Set
        End Property



        ''' <summary>
        ''' comentario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property comentario() As String
            Get
                Return _comentario
            End Get
            Set(ByVal value As String)
                _comentario = value
            End Set
        End Property
        Public Property comentariocorto() As String
            Get
                Return _comentariocorto
            End Get
            Set(ByVal value As String)
                _comentariocorto = value
            End Set
        End Property

        Public Property email() As String
            Get
                Return _email
            End Get
            Set(ByVal value As String)
                _email = value
            End Set
        End Property
        Public Property sel1() As String
            Get
                Return _sel1
            End Get
            Set(ByVal value As String)
                _sel1 = value
            End Set
        End Property
        Public Property traduccion() As String
            Get
                Return _traduccion
            End Get
            Set(ByVal value As String)
                _traduccion = value
            End Set
        End Property
        Public Property traduccioncorto() As String
            Get
                Return _traduccioncorto
            End Get
            Set(ByVal value As String)
                _traduccioncorto = value
            End Set
        End Property
        Public Property cierre() As Integer
            Get
                Return _cierre
            End Get
            Set(ByVal value As Integer)
                _cierre = value
            End Set
        End Property
    End Class



    Public Class TarjetaIZARO
        Private _planta As Integer = Integer.MinValue

        Private _FecIni As Date = Date.MinValue
        Private _FecFin As Date = Date.MaxValue
        Private _responsable As String = String.Empty
        Private _Empresa As String = String.Empty
        Private _tDNI As String = String.Empty
        'departamento, DesdeFecha, HastaFecha, validador0 (pongo responsable), tdni, qTrabajador
        Private _departamento As String = String.Empty
        Private _qTrabajador As Integer = Integer.MinValue
        'nombre, apellidos
        Private _nombre As String = String.Empty
        Private _tarjeta As String = String.Empty
        Private _apellidos As String = String.Empty

        Public Property FecIni() As Date
            Get
                Return _FecIni
            End Get
            Set(ByVal value As Date)
                _FecIni = value
            End Set
        End Property

        Public Property FecFin() As Date
            Get
                Return _FecFin
            End Get
            Set(ByVal value As Date)
                _FecFin = value
            End Set
        End Property

        Public Property responsable() As String
            Get
                Return _responsable
            End Get
            Set(ByVal value As String)
                _responsable = value
            End Set
        End Property
        Public Property qTrabajador() As Integer
            Get
                Return _qTrabajador
            End Get
            Set(ByVal value As Integer)
                _qTrabajador = value
            End Set
        End Property


        Public Property Empresa() As String
            Get
                Return _Empresa
            End Get
            Set(ByVal value As String)
                _Empresa = value
            End Set
        End Property

        Public Property tDNI() As String
            Get
                Return _tDNI
            End Get
            Set(ByVal value As String)
                _tDNI = value
            End Set
        End Property

        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property
        Public Property Tarjeta() As String
            Get
                Return _Tarjeta
            End Get
            Set(ByVal value As String)
                _Tarjeta = value
            End Set
        End Property

        Public Property apellidos() As String
            Get
                Return _apellidos
            End Get
            Set(ByVal value As String)
                _apellidos = value
            End Set
        End Property

        Public Property departamento() As String
            Get
                Return _departamento
            End Get
            Set(ByVal value As String)
                _departamento = value
            End Set
        End Property



        Public Property Planta() As Integer
            Get
                Return _planta
            End Get
            Set(ByVal value As Integer)
                _planta = value
            End Set
        End Property



    End Class


    Public Class Caducidades

#Region "Variables miembro"

        Private _planta As Integer = Integer.MinValue
        Private _id As Integer = Integer.MinValue
        Private _nombre As String = String.Empty
        Private _intervalo As Integer = Integer.MinValue
        Private _cantidad As Integer = Integer.MinValue
        Private _nombreInt As String = String.Empty



        'Private _obsoleto As Boolean = False

#End Region

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Planta() As Integer
            Get
                Return _planta
            End Get
            Set(ByVal value As Integer)
                _planta = value
            End Set
        End Property
        Public Property id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property intervalo() As Integer
            Get
                Return _intervalo
            End Get
            Set(ByVal value As Integer)
                _intervalo = value
            End Set
        End Property

        Public Property cantidad() As Integer
            Get
                Return _cantidad
            End Get
            Set(ByVal value As Integer)
                _cantidad = value
            End Set
        End Property

        Public Property nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property
        Public Property nombreInt() As String
            Get
                Return _nombreInt
            End Get
            Set(ByVal value As String)
                _nombreInt = value
            End Set
        End Property


#End Region

    End Class

    Public Class FinSemana

        Private _planta As Integer = Integer.MinValue
        Private _id As Integer = Integer.MinValue
        Private _trabajador As Integer = Integer.MinValue
        Private _empresa As Integer = Integer.MinValue
        Private _Fecha As Date = Date.MinValue
        Private _nombre As String = String.Empty
        Private _empresatxt As String = String.Empty
        Private _Fechatxt As String = String.Empty

        Public Property Planta() As Integer
            Get
                Return _planta
            End Get
            Set(ByVal value As Integer)
                _planta = value
            End Set
        End Property
        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property empresa() As Integer
            Get
                Return _empresa
            End Get
            Set(ByVal value As Integer)
                _empresa = value
            End Set
        End Property

        Public Property trabajador() As Integer
            Get
                Return _trabajador
            End Get
            Set(ByVal value As Integer)
                _trabajador = value
            End Set
        End Property

        Public Property Fecha() As Date
            Get
                Return _Fecha
            End Get
            Set(ByVal value As Date)
                _Fecha = value
            End Set
        End Property
        Public Property nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property
        Public Property empresatxt() As String
            Get
                Return _empresatxt
            End Get
            Set(ByVal value As String)
                _empresatxt = value
            End Set
        End Property
        Public Property Fechatxt() As String
            Get
                Return _Fechatxt
            End Get
            Set(ByVal value As String)
                _Fechatxt = value
            End Set
        End Property

    End Class

    Public Class Preventiva

        Private _id As Integer = Integer.MinValue
        Private _nombre As String = String.Empty


        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property



    End Class

    Public Class Responsables
        Private _planta As Integer = Integer.MinValue
        Private _id As Integer = Integer.MinValue
        Private _Abrev As String = String.Empty
        Private _nombre As String = String.Empty
        Private _mail As String = String.Empty

        Public Property Planta() As Integer
            Get
                Return _planta
            End Get
            Set(ByVal value As Integer)
                _planta = value
            End Set
        End Property
        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

        Public Property Abrev() As String
            Get
                Return _Abrev
            End Get
            Set(ByVal value As String)
                _Abrev = value
            End Set
        End Property
        Public Property Mail() As String
            Get
                Return _mail
            End Get
            Set(ByVal value As String)
                _mail = value
            End Set
        End Property

    End Class





    Public Class Empresas

#Region "Variables miembro"

        Private _planta As Integer = Integer.MinValue

        Private _id As Integer = Integer.MinValue
        Private _nombre As String = String.Empty

        Private _Nif As String = String.Empty
        Private _DocsOL As Integer = Integer.MinValue
        Private _FecSolEnv As Date = Date.MinValue
        Private _medio As String = String.Empty
        Private _DocEnv As Integer = Integer.MinValue
        Private _FecEnv As Date = Date.MinValue
        Private _medio2 As String = String.Empty
        Private _DocRec As Integer = Integer.MinValue
        Private _FecRec As Date = Date.MinValue
        Private _recibi As Integer = Integer.MinValue

        Private _Autonomo As Integer = Integer.MinValue
        Private _Asignada As Integer = Integer.MinValue
        Private _Asignada1 As Boolean = False
        Private _Asignada2 As Boolean = False
        Private _preventiva As String = String.Empty
        Private _interlocutor As String = String.Empty
        Private _telefono As String = String.Empty
        Private _email As String = String.Empty
        Private _notificar As String = String.Empty

        Private _fax As String = String.Empty
        Private _subcontrata As String = String.Empty
        Private _contacto As String = String.Empty
        Private _activo As Integer = Integer.MinValue
        Private _empSAB As Integer = Integer.MinValue

        'Private _obsoleto As Boolean = False

#End Region

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Planta() As Integer
            Get
                Return _planta
            End Get
            Set(ByVal value As Integer)
                _planta = value
            End Set
        End Property

        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property


        Public Property FecSolEnv() As Date
            Get
                Return _FecSolEnv
            End Get
            Set(ByVal value As Date)
                _FecSolEnv = value
            End Set
        End Property

        Public Property FecEnv() As Date
            Get
                Return _FecEnv
            End Get
            Set(ByVal value As Date)
                _FecEnv = value
            End Set
        End Property

        Public Property FecRec() As Date
            Get
                Return _FecRec
            End Get
            Set(ByVal value As Date)
                _FecRec = value
            End Set
        End Property

        Public Property Nif() As String
            Get
                Return _Nif
            End Get
            Set(ByVal value As String)
                _Nif = value
            End Set
        End Property
        Public Property preventiva() As String
            Get
                Return _preventiva
            End Get
            Set(ByVal value As String)
                _preventiva = value
            End Set
        End Property
        Public Property interlocutor() As String
            Get
                Return _interlocutor
            End Get
            Set(ByVal value As String)
                _interlocutor = value
            End Set
        End Property


        Public Property email() As String
            Get
                Return _email
            End Get
            Set(ByVal value As String)
                _email = value
            End Set
        End Property
        Public Property notificar() As String
            Get
                Return _notificar
            End Get
            Set(ByVal value As String)
                _notificar = value
            End Set
        End Property

        Public Property telefono() As String
            Get
                Return _telefono
            End Get
            Set(ByVal value As String)
                _telefono = value
            End Set
        End Property
        Public Property fax() As String
            Get
                Return _fax
            End Get
            Set(ByVal value As String)
                _fax = value
            End Set
        End Property

        Public Property contacto() As String
            Get
                Return _contacto
            End Get
            Set(ByVal value As String)
                _contacto = value
            End Set
        End Property

        Public Property subcontrata() As String
            Get
                Return _subcontrata
            End Get
            Set(ByVal value As String)
                _subcontrata = value
            End Set
        End Property

        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

        Public Property medio() As String
            Get
                Return _medio
            End Get
            Set(ByVal value As String)
                _medio = value
            End Set
        End Property

        Public Property DocEnv() As Integer
            Get
                Return _DocEnv
            End Get
            Set(ByVal value As Integer)
                _DocEnv = value
            End Set
        End Property

        Public Property DocSol() As Integer
            Get
                Return _DocsOL
            End Get
            Set(ByVal value As Integer)
                _DocsOL = value
            End Set
        End Property

        Public Property DocRec() As Integer
            Get
                Return _DocRec
            End Get
            Set(ByVal value As Integer)
                _DocRec = value
            End Set
        End Property
        Public Property recibi() As Integer
            Get
                Return _recibi
            End Get
            Set(ByVal value As Integer)
                _recibi = value
            End Set
        End Property

        Public Property medio2() As String
            Get
                Return _medio2
            End Get
            Set(ByVal value As String)
                _medio2 = value
            End Set
        End Property

        Public Property Autonomo() As Integer
            Get
                Return _Autonomo
            End Get
            Set(ByVal value As Integer)
                _Autonomo = value
            End Set
        End Property

        Public Property Asignada() As Integer
            Get
                Return _Asignada
            End Get
            Set(ByVal value As Integer)
                _Asignada = value
            End Set
        End Property

        Public Property Asignada2() As Boolean
            Get
                Return _Asignada2
            End Get
            Set(ByVal value As Boolean)
                _Asignada2 = value
            End Set
        End Property

        Public Property Asignada1() As Boolean
            Get
                Return _Asignada1
            End Get
            Set(ByVal value As Boolean)
                _Asignada1 = value
            End Set
        End Property
        Public Property activo() As Integer
            Get
                Return _activo
            End Get
            Set(ByVal value As Integer)
                _activo = value
            End Set
        End Property

        Public Property empSAB() As Integer
            Get
                Return _empSAB
            End Get
            Set(ByVal value As Integer)
                _empSAB = value
            End Set
        End Property

        '''' <summary>
        '''' Nombre
        '''' </summary>
        '''' <value></value>
        '''' <returns></returns>        
        'Public Property Nombre() As String
        '    Get
        '        Return If(String.IsNullOrEmpty(_nombre), String.Empty, _nombre.Trim())
        '    End Get
        '    Set(ByVal value As String)
        '        _nombre = If(String.IsNullOrEmpty(value), String.Empty, value.Trim())
        '    End Set
        'End Property

        '''' <summary>
        '''' Descripción
        '''' </summary>
        '''' <value></value>
        '''' <returns></returns>        
        'Public Property Descripcion() As String
        '    Get
        '        Return _descripcion
        '    End Get
        '    Set(ByVal value As String)
        '        _descripcion = value
        '    End Set
        'End Property

        '''' <summary>
        '''' Obsoleto
        '''' </summary>
        '''' <value></value>
        '''' <returns></returns>        
        'Public Property Obsoleto() As Boolean
        '    Get
        '        Return _obsoleto
        '    End Get
        '    Set(ByVal value As Boolean)
        '        _obsoleto = value
        '    End Set
        'End Property

#End Region

    End Class

    Public Class Documentos

#Region "Variables miembro"

        Private _planta As Integer = Integer.MinValue
        Private _id As Integer = Integer.MinValue
        Private _nombre As String = String.Empty
        Private _Abrev As String = String.Empty
        Private _Periodo As Integer = Integer.MinValue
        Private _Obligatorio As Integer = Integer.MinValue
        Private _Trabajador As Integer = Integer.MinValue
        Private _comentario As String = String.Empty
        Private _ubicacion As String = String.Empty
        Private _Responsable As Integer = Integer.MinValue
        Private _Margen As Integer = Integer.MinValue
        Private _Plantilla As Integer = Integer.MinValue
        Private _EsDocumento As Integer = Integer.MinValue
        Private _tipotrabajo As Integer = Integer.MinValue

        Private _Empresa As Integer = Integer.MinValue
        Private _Asignada As Integer = Integer.MinValue
        Private _activo As Integer = Integer.MinValue
        Private _ETT As Integer = Integer.MinValue
        Private _area As Integer = Integer.MinValue
        Private _listacorreos As String = String.Empty
        Private _textoSolicitud As String = String.Empty



#End Region

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Planta() As Integer
            Get
                Return _planta
            End Get
            Set(ByVal value As Integer)
                _planta = value
            End Set
        End Property
        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

        Public Property Abrev() As String
            Get
                Return _Abrev
            End Get
            Set(ByVal value As String)
                _Abrev = value
            End Set
        End Property

        Public Property Periodo() As Integer
            Get
                Return _Periodo
            End Get
            Set(ByVal value As Integer)
                _Periodo = value
            End Set
        End Property
        Public Property Obligatorio() As Integer
            Get
                Return _Obligatorio
            End Get
            Set(ByVal value As Integer)
                _Obligatorio = value
            End Set
        End Property
        Public Property Trabajador() As Integer
            Get
                Return _Trabajador
            End Get
            Set(ByVal value As Integer)
                _Trabajador = value
            End Set
        End Property

        Public Property comentario() As String
            Get
                Return _comentario
            End Get
            Set(ByVal value As String)
                _comentario = value
            End Set
        End Property


        Public Property ubicacion() As String
            Get
                Return _ubicacion
            End Get
            Set(ByVal value As String)
                _ubicacion = value
            End Set
        End Property

        Public Property Responsable() As Integer
            Get
                Return _Responsable
            End Get
            Set(ByVal value As Integer)
                _Responsable = value
            End Set
        End Property

        Public Property Margen() As Integer
            Get
                Return _Margen
            End Get
            Set(ByVal value As Integer)
                _Margen = value
            End Set
        End Property
        Public Property Plantilla() As Integer
            Get
                Return _Plantilla
            End Get
            Set(ByVal value As Integer)
                _Plantilla = value
            End Set
        End Property
        Public Property EsDocumento() As Integer
            Get
                Return _EsDocumento
            End Get
            Set(ByVal value As Integer)
                _EsDocumento = value
            End Set
        End Property
        Public Property tipotrabajo() As Integer
            Get
                Return _tipotrabajo
            End Get
            Set(ByVal value As Integer)
                _tipotrabajo = value
            End Set
        End Property



        Public Property Empresa() As Integer
            Get
                Return _Empresa
            End Get
            Set(ByVal value As Integer)
                _Empresa = value
            End Set
        End Property
        Public Property Asignada() As Integer
            Get
                Return _Asignada
            End Get
            Set(ByVal value As Integer)
                _Asignada = value
            End Set
        End Property

        Public Property activo() As Integer
            Get
                Return _activo
            End Get
            Set(ByVal value As Integer)
                _activo = value
            End Set
        End Property
        Public Property ETT() As Integer
            Get
                Return _ETT
            End Get
            Set(ByVal value As Integer)
                _ETT = value
            End Set
        End Property

        Public Property area() As Integer
            Get
                Return _area
            End Get
            Set(ByVal value As Integer)
                _area = value
            End Set
        End Property
        Public Property listacorreos() As String
            Get
                Return _listacorreos
            End Get
            Set(ByVal value As String)
                _listacorreos = value
            End Set
        End Property
        Public Property textoSolicitud() As String
            Get
                Return _textoSolicitud
            End Get
            Set(ByVal value As String)
                _textoSolicitud = value
            End Set
        End Property

#End Region

    End Class

    Public Class Rol

#Region "Variables miembro"

        Private _planta As Integer = Integer.MinValue
        Private _id As Integer = Integer.MinValue
        Private _rol As Integer = Integer.MinValue
        Private _NombreRol As String = String.Empty
        Private _NombreUser As String = String.Empty
        Private _NombreDepto As String = String.Empty


#End Region

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Planta() As Integer
            Get
                Return _planta
            End Get
            Set(ByVal value As Integer)
                _planta = value
            End Set
        End Property
        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property
        Public Property rol() As Integer
            Get
                Return _rol
            End Get
            Set(ByVal value As Integer)
                _rol = value
            End Set
        End Property

        Public Property NombreRol() As String
            Get
                Return _nombreRol
            End Get
            Set(ByVal value As String)
                _nombreRol = value
            End Set
        End Property
        Public Property NombreDepto() As String
            Get
                Return _NombreDepto
            End Get
            Set(ByVal value As String)
                _NombreDepto = value
            End Set
        End Property
        Public Property NombreUser() As String
            Get
                Return _NombreUser
            End Get
            Set(ByVal value As String)
                _NombreUser = value
            End Set
        End Property
#End Region

    End Class


    Public Class Dorlet

#Region "Variables miembro"

        Private _planta As Integer = Integer.MinValue
        Private _id As Integer = Integer.MinValue
        Private _DNI As String = String.Empty

        Private _nombre As String = String.Empty
        Private _Apellidos As String = String.Empty

        Private _FecIni As Date = Date.MinValue
        Private _FecFin As Date = Date.MaxValue
        Private _Tarjeta As String = String.Empty

        Private _Empresa As String = String.Empty
        Private _Centro As String = String.Empty

        Private _rutas As String = String.Empty
        Private _matricula As String = String.Empty
        Private _contrata As Integer = Integer.MinValue







#End Region

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Planta() As Integer
            Get
                Return _planta
            End Get
            Set(ByVal value As Integer)
                _planta = value
            End Set
        End Property
        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

        Public Property DNI() As String
            Get
                Return _DNI
            End Get
            Set(ByVal value As String)
                _DNI = value
            End Set
        End Property

        Public Property Apellidos() As String
            Get
                Return _Apellidos
            End Get
            Set(ByVal value As String)
                _Apellidos = value
            End Set
        End Property

        Public Property Tarjeta() As String
            Get
                Return _Tarjeta
            End Get
            Set(ByVal value As String)
                _Tarjeta = value
            End Set
        End Property

        Public Property Empresa() As Integer
            Get
                Return _Empresa
            End Get
            Set(ByVal value As Integer)
                _Empresa = value
            End Set
        End Property
        Public Property FecIni() As Date
            Get
                Return _FecIni
            End Get
            Set(ByVal value As Date)
                _FecIni = value
            End Set
        End Property

        Public Property FecFin() As Date
            Get
                Return _FecFin
            End Get
            Set(ByVal value As Date)
                _FecFin = value
            End Set
        End Property

        Public Property rutas() As String
            Get
                Return _rutas
            End Get
            Set(ByVal value As String)
                _rutas = value
            End Set
        End Property

        Public Property matricula() As String
            Get
                Return _matricula
            End Get
            Set(ByVal value As String)
                _matricula = value
            End Set
        End Property
        Public Property contrata() As Integer
            Get
                Return _contrata
            End Get
            Set(ByVal value As Integer)
                _contrata = value
            End Set
        End Property
#End Region

    End Class

    Public Class Solicitudes

#Region "Variables miembro"

        Private _planta As Integer = Integer.MinValue
        Private _plantaSeleccionada As Integer = Integer.MinValue
        Private _id As Integer = Integer.MinValue
        Private _descripcion As String = String.Empty
        Private _Empresa As Integer = Integer.MinValue
        Private _Subcontrata As String = String.Empty
        Private _EmpresaTroquelaje As String = String.Empty
        Private _responsable As Integer = Integer.MinValue

        Private _FecIni As Date = Date.MinValue
        Private _FecFin As Date = Date.MaxValue
        Private _activo As Integer = Integer.MinValue
        Private _activo2 As Integer = Integer.MinValue
        Private _soldadura As Integer = Integer.MinValue
        Private _altura As Integer = Integer.MinValue
        Private _salas As Integer = Integer.MinValue
        Private _gases As Integer = Integer.MinValue
        Private _elevados As String = String.Empty
        Private _fosas As Integer = Integer.MinValue
        Private _X7 As Integer = Integer.MinValue
        Private _X8 As Integer = Integer.MinValue
        Private _X9 As Integer = Integer.MinValue
        Private _X10 As Integer = Integer.MinValue
        Private _X11 As Integer = Integer.MinValue
        Private _email As String = String.Empty
        Private _otros As String = String.Empty
        Private _DescResponsable As String = String.Empty
        Private _DescSolicitante As String = String.Empty
        Private _DescEmpresa As String = String.Empty
        Private _FechaInicio As String = String.Empty
        Private _FechaFin As String = String.Empty
        Private _Area As Integer = Integer.MinValue
        Private _Numero As Integer = Integer.MinValue






#End Region

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Planta() As Integer
            Get
                Return _planta
            End Get
            Set(ByVal value As Integer)
                _planta = value
            End Set
        End Property
        Public Property plantaSeleccionada() As Integer
            Get
                Return _plantaSeleccionada
            End Get
            Set(ByVal value As Integer)
                _plantaSeleccionada = value
            End Set
        End Property
        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property
        Public Property descripcion() As String
            Get
                Return _descripcion
            End Get
            Set(ByVal value As String)
                _descripcion = value
            End Set
        End Property
        Public Property EmpresaTroquelaje() As String
            Get
                Return _EmpresaTroquelaje
            End Get
            Set(ByVal value As String)
                _EmpresaTroquelaje = value
            End Set
        End Property

        Public Property Empresa() As Integer
            Get
                Return _Empresa
            End Get
            Set(ByVal value As Integer)
                _Empresa = value
            End Set
        End Property
        Public Property Subcontrata() As String
            Get
                Return _Subcontrata
            End Get
            Set(ByVal value As String)
                _Subcontrata = value
            End Set
        End Property

        Public Property activo() As Integer
            Get
                Return _activo
            End Get
            Set(ByVal value As Integer)
                _activo = value
            End Set
        End Property
        Public Property activo2() As Integer
            Get
                Return _activo2
            End Get
            Set(ByVal value As Integer)
                _activo2 = value
            End Set
        End Property
        Public Property FecIni() As Date
            Get
                Return _FecIni
            End Get
            Set(ByVal value As Date)
                _FecIni = value
            End Set
        End Property

        Public Property FecFin() As Date
            Get
                Return _FecFin
            End Get
            Set(ByVal value As Date)
                _FecFin = value
            End Set
        End Property

        Public Property responsable() As Integer
            Get
                Return _responsable
            End Get
            Set(ByVal value As Integer)
                _responsable = value
            End Set
        End Property

        Public Property soldadura() As Integer
            Get
                Return _soldadura
            End Get
            Set(ByVal value As Integer)
                _soldadura = value
            End Set
        End Property

        Public Property altura() As Integer
            Get
                Return _altura
            End Get
            Set(ByVal value As Integer)
                _altura = value
            End Set
        End Property

        Public Property salas() As Integer
            Get
                Return _salas
            End Get
            Set(ByVal value As Integer)
                _salas = value
            End Set
        End Property
        Public Property gases() As Integer
            Get
                Return _gases
            End Get
            Set(ByVal value As Integer)
                _gases = value
            End Set
        End Property
        Public Property elevados() As Integer
            Get
                Return _elevados
            End Get
            Set(ByVal value As Integer)
                _elevados = value
            End Set
        End Property
        Public Property fosas() As Integer
            Get
                Return _fosas
            End Get
            Set(ByVal value As Integer)
                _fosas = value
            End Set
        End Property
        Public Property X7() As Integer
            Get
                Return _X7
            End Get
            Set(ByVal value As Integer)
                _X7 = value
            End Set
        End Property
        Public Property X8() As Integer
            Get
                Return _X8
            End Get
            Set(ByVal value As Integer)
                _X8 = value
            End Set
        End Property
        Public Property X9() As Integer
            Get
                Return _X9
            End Get
            Set(ByVal value As Integer)
                _X9 = value
            End Set
        End Property
        Public Property X10() As Integer
            Get
                Return _X10
            End Get
            Set(ByVal value As Integer)
                _X10 = value
            End Set
        End Property
        Public Property X11() As Integer
            Get
                Return _X11
            End Get
            Set(ByVal value As Integer)
                _X11 = value
            End Set
        End Property

        Public Property email() As String
            Get
                Return _email
            End Get
            Set(ByVal value As String)
                _email = value
            End Set
        End Property

        Public Property otros() As String
            Get
                Return _otros
            End Get
            Set(ByVal value As String)
                _otros = value
            End Set
        End Property

        Public Property DescResponsable() As String
            Get
                Return _DescResponsable
            End Get
            Set(ByVal value As String)
                _DescResponsable = value
            End Set
        End Property
        Public Property DescSolicitante() As String
            Get
                Return _DescSolicitante
            End Get
            Set(ByVal value As String)
                _DescSolicitante = value
            End Set
        End Property

        Public Property DescEmpresa() As String
            Get
                Return _DescEmpresa
            End Get
            Set(ByVal value As String)
                _DescEmpresa = value
            End Set
        End Property

        Public Property FechaInicio() As String
            Get
                Return _FechaInicio
            End Get
            Set(ByVal value As String)
                _FechaInicio = value
            End Set
        End Property


        Public Property FechaFin() As String
            Get
                Return _FechaFin
            End Get
            Set(ByVal value As String)
                _FechaFin = value
            End Set
        End Property
        Public Property Area() As Integer
            Get
                Return _Area
            End Get
            Set(ByVal value As Integer)
                _Area = value
            End Set
        End Property
        Public Property Numero() As Integer
            Get
                Return _Numero
            End Get
            Set(ByVal value As Integer)
                _Numero = value
            End Set
        End Property

#End Region

    End Class


    Public Class Certificados

#Region "Variables miembro"

        Private _planta As Integer = Integer.MinValue
        Private _id As Integer = Integer.MinValue
        Private _tipo As String = String.Empty
        Private _descripcion As String = String.Empty
        Private _Empresa As String = String.Empty
        Private _Fecha As Date = Date.MinValue
        Private _Estado As String = String.Empty
        Private _Clave As String = String.Empty







#End Region

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Planta() As Integer
            Get
                Return _planta
            End Get
            Set(ByVal value As Integer)
                _planta = value
            End Set
        End Property
        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property
        Public Property Tipo() As String
            Get
                Return _tipo
            End Get
            Set(ByVal value As String)
                _tipo = value
            End Set
        End Property
        Public Property descripcion() As String
            Get
                Return _descripcion
            End Get
            Set(ByVal value As String)
                _descripcion = value
            End Set
        End Property

        Public Property Empresa() As String
            Get
                Return _Empresa
            End Get
            Set(ByVal value As String)
                _Empresa = value
            End Set
        End Property


        Public Property Fecha() As Date
            Get
                Return _Fecha
            End Get
            Set(ByVal value As Date)
                _Fecha = value
            End Set
        End Property


        Public Property Estado() As String
            Get
                Return _Estado
            End Get
            Set(ByVal value As String)
                _Estado = value
            End Set
        End Property

        Public Property Clave() As String
            Get
                Return _Clave
            End Get
            Set(ByVal value As String)
                _Clave = value
            End Set
        End Property

#End Region

    End Class


    Public Class Trabajadores

#Region "Variables miembro"

        Private _planta As Integer = Integer.MinValue
        Private _id As Integer = Integer.MinValue
        Private _nDNI As String = String.Empty
        Private _tDNI As String = String.Empty
        Private _nombre As String = String.Empty
        Private _Apellidos As String = String.Empty
        Private _Empresa As Integer = Integer.MinValue
        Private _FecIni As Date = Date.MinValue
        Private _FecFin As Date = Date.MaxValue
        'Private _Activado As Integer = Integer.MinValue
        Private _Autonomo As Integer = Integer.MinValue
        Private _activo As Integer = Integer.MinValue
        Private _activo2 As Integer = Integer.MinValue
        Private _puesto As String = String.Empty
        Private _puesto2 As String = String.Empty
        Private _funcion As String = String.Empty
        Private _responsable As Integer = Integer.MinValue
        Private _tarjeta As String = String.Empty
        Private _estado As Integer = Integer.MinValue
        Private _estado2 As Integer = Integer.MinValue
        Private _solicitud As String = String.Empty
        Private _DescResponsable As String = String.Empty
        Private _DescEmpresa As String = String.Empty
        Private _FechaInicio As String = String.Empty
        Private _FechaFin As String = String.Empty





#End Region

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Planta() As Integer
            Get
                Return _planta
            End Get
            Set(ByVal value As Integer)
                _planta = value
            End Set
        End Property
        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

        Public Property nDNI() As String
            Get
                Return _nDNI
            End Get
            Set(ByVal value As String)
                _nDNI = value
            End Set
        End Property
        Public Property tDNI() As String
            Get
                Return _tDNI
            End Get
            Set(ByVal value As String)
                _tDNI = value
            End Set
        End Property
        Public Property Apellidos() As String
            Get
                Return _Apellidos
            End Get
            Set(ByVal value As String)
                _Apellidos = value
            End Set
        End Property

        Public Property Empresa() As Integer
            Get
                Return _Empresa
            End Get
            Set(ByVal value As Integer)
                _Empresa = value
            End Set
        End Property

        'Public Property Activado() As Integer
        '    Get
        '        Return _Activado
        '    End Get
        '    Set(ByVal value As Integer)
        '        _Activado = value
        '    End Set
        'End Property

        Public Property Autonomo() As Integer
            Get
                Return _Autonomo
            End Get
            Set(ByVal value As Integer)
                _Autonomo = value
            End Set
        End Property

        Public Property activo() As Integer
            Get
                Return _activo
            End Get
            Set(ByVal value As Integer)
                _activo = value
            End Set
        End Property
        Public Property activo2() As Integer
            Get
                Return _activo2
            End Get
            Set(ByVal value As Integer)
                _activo2 = value
            End Set
        End Property

        Public Property FecIni() As Date
            Get
                Return _FecIni
            End Get
            Set(ByVal value As Date)
                _FecIni = value
            End Set
        End Property

        Public Property FecFin() As Date
            Get
                Return _FecFin
            End Get
            Set(ByVal value As Date)
                _FecFin = value
            End Set
        End Property
        Public Property puesto() As String
            Get
                Return _puesto
            End Get
            Set(ByVal value As String)
                _puesto = value
            End Set
        End Property
        Public Property puesto2() As String
            Get
                Return _puesto2
            End Get
            Set(ByVal value As String)
                _puesto2 = value
            End Set
        End Property
        Public Property funcion() As String
            Get
                Return _funcion
            End Get
            Set(ByVal value As String)
                _funcion = value
            End Set
        End Property

        Public Property responsable() As Integer
            Get
                Return _responsable
            End Get
            Set(ByVal value As Integer)
                _responsable = value
            End Set
        End Property
        Public Property estado() As Integer
            Get
                Return _estado
            End Get
            Set(ByVal value As Integer)
                _estado = value
            End Set
        End Property
        Public Property estado2() As Integer
            Get
                Return _estado2
            End Get
            Set(ByVal value As Integer)
                _estado2 = value
            End Set
        End Property

        Public Property solicitud() As String
            Get
                Return _solicitud
            End Get
            Set(ByVal value As String)
                _solicitud = value
            End Set
        End Property
        Public Property tarjeta() As String
            Get
                Return _tarjeta
            End Get
            Set(ByVal value As String)
                _tarjeta = value
            End Set
        End Property

        Public Property DescResponsable() As String
            Get
                Return _DescResponsable
            End Get
            Set(ByVal value As String)
                _DescResponsable = value
            End Set
        End Property
        Public Property DescEmpresa() As String
            Get
                Return _DescEmpresa
            End Get
            Set(ByVal value As String)
                _DescEmpresa = value
            End Set
        End Property

        Public Property FechaInicio() As String
            Get
                Return _FechaInicio
            End Get
            Set(ByVal value As String)
                _FechaInicio = value
            End Set
        End Property


        Public Property FechaFin() As String
            Get
                Return _FechaFin
            End Get
            Set(ByVal value As String)
                _FechaFin = value
            End Set
        End Property

#End Region

    End Class

    Public Class EmpresasDoc

#Region "Variables miembro"

        Private _planta As Integer = Integer.MinValue
        Private _nombre As String = String.Empty
        Private _abrev As String = String.Empty
        Private _codemp As Integer = Integer.MinValue
        Private _coddoc As Integer = Integer.MinValue
        Private _FecRec As Date = Date.MinValue
        Private _correcto As Integer = Integer.MinValue
        Private _txtcorrecto As String = String.Empty
        Private _obligatorio As Integer = Integer.MinValue
        Private _FecCad As Date = Date.MaxValue
        Private _ubicacion As String = String.Empty
        Private _estado As String = String.Empty
        Private _FecIni As Date = Date.MinValue
        Private _clave As Integer = Integer.MinValue
        Private _periodicidad As Integer = Integer.MinValue
        Private _plantilla As Integer = Integer.MinValue
        Private _ubicacionfisica As String = String.Empty
        Private _EsDocumento As Integer = Integer.MinValue
        Private _Margen As Integer = Integer.MinValue
        Private _Impuestos As Integer = Integer.MinValue
        Private _Listacorreos As String = String.Empty
        Private _CIF As String = String.Empty
        Private _nomemp As String = String.Empty
        Private _vacio As String = String.Empty
        Private _Comentario As String = String.Empty
        Private _tipodoc As Integer = Integer.MinValue


#End Region

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Planta() As Integer
            Get
                Return _planta
            End Get
            Set(ByVal value As Integer)
                _planta = value
            End Set
        End Property
        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

        Public Property Abrev() As String
            Get
                Return _abrev
            End Get
            Set(ByVal value As String)
                _abrev = value
            End Set
        End Property

        Public Property codemp() As Integer
            Get
                Return _codemp
            End Get
            Set(ByVal value As Integer)
                _codemp = value
            End Set
        End Property
        Public Property coddoc() As Integer
            Get
                Return _coddoc
            End Get
            Set(ByVal value As Integer)
                _coddoc = value
            End Set
        End Property


        Public Property FecRec() As Date
            Get
                Return _FecRec
            End Get
            Set(ByVal value As Date)
                _FecRec = value
            End Set
        End Property

        Public Property FecIni() As Date
            Get
                Return _FecIni
            End Get
            Set(ByVal value As Date)
                _FecIni = value
            End Set
        End Property

        Public Property FecCad() As Date
            Get
                Return _FecCad
            End Get
            Set(ByVal value As Date)
                _FecCad = value
            End Set
        End Property

        Public Property correcto() As Integer
            Get
                Return _correcto
            End Get
            Set(ByVal value As Integer)
                _correcto = value
            End Set
        End Property
        Public Property txtcorrecto() As String
            Get
                Return _txtcorrecto
            End Get
            Set(ByVal value As String)
                _txtcorrecto = value
            End Set
        End Property

        Public Property obligatorio() As Integer
            Get
                Return _obligatorio
            End Get
            Set(ByVal value As Integer)
                _obligatorio = value
            End Set
        End Property
        Public Property ubicacion() As String
            Get
                Return _ubicacion
            End Get
            Set(ByVal value As String)
                _ubicacion = value
            End Set
        End Property

        Public Property estado() As String
            Get
                Return _estado
            End Get
            Set(ByVal value As String)
                _estado = value
            End Set
        End Property

        Public Property ubicacionfisica() As String
            Get
                Return _ubicacionfisica
            End Get
            Set(ByVal value As String)
                _ubicacionfisica = value
            End Set
        End Property


        Public Property clave() As Integer
            Get
                Return _clave
            End Get
            Set(ByVal value As Integer)
                _clave = value
            End Set
        End Property
        Public Property periodicidad() As Integer
            Get
                Return _periodicidad
            End Get
            Set(ByVal value As Integer)
                _periodicidad = value
            End Set
        End Property
        Public Property tipodoc() As Integer
            Get
                Return _tipodoc
            End Get
            Set(ByVal value As Integer)
                _tipodoc = value
            End Set
        End Property

        Public Property plantilla() As Integer
            Get
                Return _plantilla
            End Get
            Set(ByVal value As Integer)
                _plantilla = value
            End Set
        End Property

        Public Property EsDocumento() As Integer
            Get
                Return _EsDocumento
            End Get
            Set(ByVal value As Integer)
                _EsDocumento = value
            End Set
        End Property
        Public Property Margen() As Integer
            Get
                Return _Margen
            End Get
            Set(ByVal value As Integer)
                _Margen = value
            End Set
        End Property
        Public Property Impuestos() As Integer
            Get
                Return _Impuestos
            End Get
            Set(ByVal value As Integer)
                _Impuestos = value
            End Set
        End Property
        Public Property Listacorreos() As String
            Get
                Return _Listacorreos
            End Get
            Set(ByVal value As String)
                _Listacorreos = value
            End Set
        End Property
        Public Property CIF() As String
            Get
                Return _CIF
            End Get
            Set(ByVal value As String)
                _CIF = value
            End Set
        End Property
        Public Property nomemp() As String
            Get
                Return _nomemp
            End Get
            Set(ByVal value As String)
                _nomemp = value
            End Set
        End Property
        Public Property vacio() As String
            Get
                Return _vacio
            End Get
            Set(ByVal value As String)
                _vacio = value
            End Set
        End Property
        Public Property Comentario() As String
            Get
                Return _Comentario
            End Get
            Set(ByVal value As String)
                _Comentario = value
            End Set
        End Property
#End Region

    End Class

    Public Class TrabajadoresDoc

#Region "Variables miembro"

        Private _planta As Integer = Integer.MinValue
        Private _nombre As String = String.Empty
        Private _abrev As String = String.Empty
        Private _codtra As Integer = Integer.MinValue
        Private _coddoc As Integer = Integer.MinValue
        Private _FecRec As Date = Date.MinValue
        Private _correcto As Integer = Integer.MinValue
        Private _txtcorrecto As String = String.Empty
        Private _obligatorio As Integer = Integer.MinValue
        Private _FecCad As Date = Date.MaxValue
        Private _FecRev As Date = Date.MaxValue
        Private _ubicacion As String = String.Empty
        Private _ubicacionHist As String = String.Empty
        Private _estado As String = String.Empty
        Private _FecIni As Date = Date.MinValue
        Private _clave As Integer = Integer.MinValue
        Private _periodicidad As Integer = Integer.MinValue
        Private _ubicacionfisica As String = String.Empty
        Private _plantilla As Integer = Integer.MinValue
        Private _EsDocumento As Integer = Integer.MinValue
        Private _Margen As Integer = Integer.MinValue
        Private _Aptitud As Integer = Integer.MinValue
        Private _Listacorreos As String = String.Empty
        Private _NIF As String = String.Empty
        Private _nomtra As String = String.Empty
        Private _Comentario As String = String.Empty
        Private _tiposCarne As String = String.Empty
        Private _necesario As Integer = Integer.MinValue



#End Region

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>   
        ''' 
        Public Property necesario() As Integer
            Get
                Return _necesario
            End Get
            Set(ByVal value As Integer)
                _necesario = value
            End Set
        End Property

        Public Property Planta() As Integer
            Get
                Return _planta
            End Get
            Set(ByVal value As Integer)
                _planta = value
            End Set
        End Property
        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

        Public Property Abrev() As String
            Get
                Return _abrev
            End Get
            Set(ByVal value As String)
                _abrev = value
            End Set
        End Property

        Public Property codtra() As Integer
            Get
                Return _codtra
            End Get
            Set(ByVal value As Integer)
                _codtra = value
            End Set
        End Property
        Public Property coddoc() As Integer
            Get
                Return _coddoc
            End Get
            Set(ByVal value As Integer)
                _coddoc = value
            End Set
        End Property


        Public Property FecRec() As Date
            Get
                Return _FecRec
            End Get
            Set(ByVal value As Date)
                _FecRec = value
            End Set
        End Property

        Public Property FecIni() As Date
            Get
                Return _FecIni
            End Get
            Set(ByVal value As Date)
                _FecIni = value
            End Set
        End Property

        Public Property FecCad() As Date
            Get
                Return _FecCad
            End Get
            Set(ByVal value As Date)
                _FecCad = value
            End Set
        End Property

        Public Property FecRev() As Date
            Get
                Return _FecRev
            End Get
            Set(ByVal value As Date)
                _FecRev = value
            End Set
        End Property
        Public Property correcto() As Integer
            Get
                Return _correcto
            End Get
            Set(ByVal value As Integer)
                _correcto = value
            End Set
        End Property
        Public Property txtcorrecto() As String
            Get
                Return _txtcorrecto
            End Get
            Set(ByVal value As String)
                _txtcorrecto = value
            End Set
        End Property

        Public Property obligatorio() As Integer
            Get
                Return _obligatorio
            End Get
            Set(ByVal value As Integer)
                _obligatorio = value
            End Set
        End Property
        Public Property ubicacion() As String
            Get
                Return _ubicacion
            End Get
            Set(ByVal value As String)
                _ubicacion = value
            End Set
        End Property
        Public Property ubicacionHist() As String
            Get
                Return _ubicacionHist
            End Get
            Set(ByVal value As String)
                _ubicacionHist = value
            End Set
        End Property

        Public Property estado() As String
            Get
                Return _estado
            End Get
            Set(ByVal value As String)
                _estado = value
            End Set
        End Property

        Public Property ubicacionfisica() As String
            Get
                Return _ubicacionfisica
            End Get
            Set(ByVal value As String)
                _ubicacionfisica = value
            End Set
        End Property


        Public Property clave() As Integer
            Get
                Return _clave
            End Get
            Set(ByVal value As Integer)
                _clave = value
            End Set
        End Property
        Public Property periodicidad() As Integer
            Get
                Return _periodicidad
            End Get
            Set(ByVal value As Integer)
                _periodicidad = value
            End Set
        End Property
        Public Property plantilla() As Integer
            Get
                Return _plantilla
            End Get
            Set(ByVal value As Integer)
                _plantilla = value
            End Set
        End Property
        Public Property EsDocumento() As Integer
            Get
                Return _EsDocumento
            End Get
            Set(ByVal value As Integer)
                _EsDocumento = value
            End Set
        End Property

        Public Property Margen() As Integer
            Get
                Return _Margen
            End Get
            Set(ByVal value As Integer)
                _Margen = value
            End Set
        End Property

        Public Property Aptitud() As Integer
            Get
                Return _Aptitud
            End Get
            Set(ByVal value As Integer)
                _Aptitud = value
            End Set
        End Property
        Public Property Listacorreos() As String
            Get
                Return _Listacorreos
            End Get
            Set(ByVal value As String)
                _Listacorreos = value
            End Set
        End Property
        Public Property NIF() As String
            Get
                Return _NIF
            End Get
            Set(ByVal value As String)
                _NIF = value
            End Set
        End Property
        Public Property nomtra() As String
            Get
                Return _nomtra
            End Get
            Set(ByVal value As String)
                _nomtra = value
            End Set
        End Property

        Public Property Comentario() As String
            Get
                Return _Comentario
            End Get
            Set(ByVal value As String)
                _Comentario = value
            End Set
        End Property

        Public Property tiposCarne() As String
            Get
                Return _tiposCarne
            End Get
            Set(ByVal value As String)
                _tiposCarne = value
            End Set
        End Property

#End Region

    End Class
    Public Class TrabajadoresDocMatriz

#Region "Variables miembro"

        Private _planta As Integer = Integer.MinValue
        Private _nombre As String = String.Empty
        Private _abrev As String = String.Empty
        Private _codtra As Integer = Integer.MinValue
        Private _coddoc As Integer = Integer.MinValue
        Private _FecRec As Date = Date.MinValue
        Private _correcto As Integer = Integer.MinValue
        Private _txtcorrecto As String = String.Empty
        Private _obligatorio As Integer = Integer.MinValue
        Private _FecCad As Date = Date.MaxValue
        Private _ubicacion As String = String.Empty
        Private _estado As String = String.Empty
        Private _FecIni As Date = Date.MinValue
        Private _clave As Integer = Integer.MinValue
        Private _periodicidad As Integer = Integer.MinValue
        Private _ubicacionfisica As String = String.Empty
        Private _plantilla As Integer = Integer.MinValue
        Private _EsDocumento As Integer = Integer.MinValue
        Private _Margen As Integer = Integer.MinValue
        Private _Aptitud As Integer = Integer.MinValue
        Private _Listacorreos As String = String.Empty
        Private _NIF As String = String.Empty
        Private _nomtra As String = String.Empty
        Private _Comentario As String = String.Empty
        Private _tiposCarne As String = String.Empty

        Private _doc1 As Integer = Integer.MinValue
        Private _xdoc2 As String = String.Empty
        Private _doc2 As Integer = Integer.MinValue
        Private _xdoc3 As String = String.Empty
        Private _fdoc2 As String = String.Empty
        Private _fdoc3 As String = String.Empty
        Private _fdoc4 As String = String.Empty
        Private _fdoc5 As String = String.Empty
        Private _fdoc6 As String = String.Empty
        Private _fdoc7 As String = String.Empty
        Private _fdoc8 As String = String.Empty
        Private _fdoc9 As String = String.Empty
        Private _fdoc10 As String = String.Empty
        Private _fdoc11 As String = String.Empty
        Private _fdoc12 As String = String.Empty

        Private _doc3 As Integer = Integer.MinValue
        Private _xdoc4 As String = String.Empty
        Private _doc4 As Integer = Integer.MinValue
        Private _xdoc5 As String = String.Empty
        Private _doc5 As Integer = Integer.MinValue
        Private _xdoc6 As String = String.Empty
        Private _doc6 As Integer = Integer.MinValue
        Private _xdoc7 As String = String.Empty
        Private _doc7 As Integer = Integer.MinValue

        Private _doc8 As Integer = Integer.MinValue
        Private _xdoc8 As String = String.Empty
        Private _doc9 As Integer = Integer.MinValue
        Private _xdoc9 As String = String.Empty
        Private _doc10 As Integer = Integer.MinValue
        Private _xdoc10 As String = String.Empty
        Private _doc11 As Integer = Integer.MinValue
        Private _xdoc11 As String = String.Empty
        Private _doc12 As Integer = Integer.MinValue
        Private _xdoc12 As String = String.Empty
        Private _doc13 As Integer = Integer.MinValue
        Private _doc14 As Integer = Integer.MinValue
        Private _doc15 As Integer = Integer.MinValue
        Private _doc16 As Integer = Integer.MinValue
        Private _doc17 As Integer = Integer.MinValue
        Private _doc18 As Integer = Integer.MinValue
        Private _doc19 As Integer = Integer.MinValue
        Private _nec1 As Integer = Integer.MinValue
        Private _nec2 As Integer = Integer.MinValue
        Private _nec3 As Integer = Integer.MinValue
        Private _nec4 As Integer = Integer.MinValue
        Private _nec5 As Integer = Integer.MinValue
        Private _nec6 As Integer = Integer.MinValue
        Private _nec7 As Integer = Integer.MinValue
        Private _nec8 As Integer = Integer.MinValue
        Private _nec9 As Integer = Integer.MinValue
        Private _nec10 As Integer = Integer.MinValue
        Private _nec11 As Integer = Integer.MinValue
        Private _nec12 As Integer = Integer.MinValue
        Private _nec13 As Integer = Integer.MinValue
        Private _nec14 As Integer = Integer.MinValue
        Private _nec15 As Integer = Integer.MinValue
        Private _nec16 As Integer = Integer.MinValue








#End Region

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>  
        ''' 

        Public Property Planta() As Integer
            Get
                Return _planta
            End Get
            Set(ByVal value As Integer)
                _planta = value
            End Set
        End Property
        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

        Public Property Abrev() As String
            Get
                Return _abrev
            End Get
            Set(ByVal value As String)
                _abrev = value
            End Set
        End Property

        Public Property codtra() As Integer
            Get
                Return _codtra
            End Get
            Set(ByVal value As Integer)
                _codtra = value
            End Set
        End Property
        Public Property coddoc() As Integer
            Get
                Return _coddoc
            End Get
            Set(ByVal value As Integer)
                _coddoc = value
            End Set
        End Property


        Public Property FecRec() As Date
            Get
                Return _FecRec
            End Get
            Set(ByVal value As Date)
                _FecRec = value
            End Set
        End Property

        Public Property FecIni() As Date
            Get
                Return _FecIni
            End Get
            Set(ByVal value As Date)
                _FecIni = value
            End Set
        End Property

        Public Property FecCad() As Date
            Get
                Return _FecCad
            End Get
            Set(ByVal value As Date)
                _FecCad = value
            End Set
        End Property

        Public Property correcto() As Integer
            Get
                Return _correcto
            End Get
            Set(ByVal value As Integer)
                _correcto = value
            End Set
        End Property
        Public Property txtcorrecto() As String
            Get
                Return _txtcorrecto
            End Get
            Set(ByVal value As String)
                _txtcorrecto = value
            End Set
        End Property

        Public Property obligatorio() As Integer
            Get
                Return _obligatorio
            End Get
            Set(ByVal value As Integer)
                _obligatorio = value
            End Set
        End Property
        Public Property ubicacion() As String
            Get
                Return _ubicacion
            End Get
            Set(ByVal value As String)
                _ubicacion = value
            End Set
        End Property

        Public Property estado() As String
            Get
                Return _estado
            End Get
            Set(ByVal value As String)
                _estado = value
            End Set
        End Property

        Public Property ubicacionfisica() As String
            Get
                Return _ubicacionfisica
            End Get
            Set(ByVal value As String)
                _ubicacionfisica = value
            End Set
        End Property


        Public Property clave() As Integer
            Get
                Return _clave
            End Get
            Set(ByVal value As Integer)
                _clave = value
            End Set
        End Property
        Public Property periodicidad() As Integer
            Get
                Return _periodicidad
            End Get
            Set(ByVal value As Integer)
                _periodicidad = value
            End Set
        End Property
        Public Property plantilla() As Integer
            Get
                Return _plantilla
            End Get
            Set(ByVal value As Integer)
                _plantilla = value
            End Set
        End Property
        Public Property EsDocumento() As Integer
            Get
                Return _EsDocumento
            End Get
            Set(ByVal value As Integer)
                _EsDocumento = value
            End Set
        End Property

        Public Property Margen() As Integer
            Get
                Return _Margen
            End Get
            Set(ByVal value As Integer)
                _Margen = value
            End Set
        End Property

        Public Property Listacorreos() As String
            Get
                Return _Listacorreos
            End Get
            Set(ByVal value As String)
                _Listacorreos = value
            End Set
        End Property
        Public Property NIF() As String
            Get
                Return _NIF
            End Get
            Set(ByVal value As String)
                _NIF = value
            End Set
        End Property
        Public Property nomtra() As String
            Get
                Return _nomtra
            End Get
            Set(ByVal value As String)
                _nomtra = value
            End Set
        End Property

        Public Property Comentario() As String
            Get
                Return _Comentario
            End Get
            Set(ByVal value As String)
                _Comentario = value
            End Set
        End Property

        Public Property tiposCarne() As String
            Get
                Return _tiposCarne
            End Get
            Set(ByVal value As String)
                _tiposCarne = value
            End Set
        End Property



        Public Property Aptitud() As Integer
            Get
                Return _Aptitud
            End Get
            Set(ByVal value As Integer)
                _Aptitud = value
            End Set
        End Property



        Public Property doc1() As Integer
            Get
                Return _doc1
            End Get
            Set(ByVal value As Integer)
                _doc1 = value
            End Set
        End Property

        Public Property nec1() As Integer
            Get
                Return _nec1
            End Get
            Set(ByVal value As Integer)
                _nec1 = value
            End Set
        End Property
        Public Property doc2() As Integer
            Get
                Return _doc2
            End Get
            Set(ByVal value As Integer)
                _doc2 = value
            End Set
        End Property
        Public Property xdoc2() As String
            Get
                Return _xdoc2
            End Get
            Set(ByVal value As String)
                _xdoc2 = value
            End Set
        End Property
        Public Property fdoc2() As String
            Get
                Return _fdoc2
            End Get
            Set(ByVal value As String)
                _fdoc2 = value
            End Set
        End Property
        Public Property fdoc3() As String
            Get
                Return _fdoc3
            End Get
            Set(ByVal value As String)
                _fdoc3 = value
            End Set
        End Property
        Public Property fdoc4() As String
            Get
                Return _fdoc4
            End Get
            Set(ByVal value As String)
                _fdoc4 = value
            End Set
        End Property
        Public Property fdoc5() As String
            Get
                Return _fdoc5
            End Get
            Set(ByVal value As String)
                _fdoc5 = value
            End Set
        End Property
        Public Property fdoc6() As String
            Get
                Return _fdoc6
            End Get
            Set(ByVal value As String)
                _fdoc6 = value
            End Set
        End Property
        Public Property fdoc7() As String
            Get
                Return _fdoc7
            End Get
            Set(ByVal value As String)
                _fdoc7 = value
            End Set
        End Property
        Public Property fdoc8() As String
            Get
                Return _fdoc8
            End Get
            Set(ByVal value As String)
                _fdoc8 = value
            End Set
        End Property
        Public Property fdoc9() As String
            Get
                Return _fdoc9
            End Get
            Set(ByVal value As String)
                _fdoc9 = value
            End Set
        End Property
        Public Property fdoc10() As String
            Get
                Return _fdoc10
            End Get
            Set(ByVal value As String)
                _fdoc10 = value
            End Set
        End Property
        Public Property fdoc11() As String
            Get
                Return _fdoc11
            End Get
            Set(ByVal value As String)
                _fdoc11 = value
            End Set
        End Property
        Public Property fdoc12() As String
            Get
                Return _fdoc12
            End Get
            Set(ByVal value As String)
                _fdoc12 = value
            End Set
        End Property
        Public Property nec2() As Integer
            Get
                Return _nec2
            End Get
            Set(ByVal value As Integer)
                _nec2 = value
            End Set
        End Property
        Public Property doc3() As Integer
            Get
                Return _doc3
            End Get
            Set(ByVal value As Integer)
                _doc3 = value
            End Set
        End Property
        Public Property xdoc3() As String
            Get
                Return _xdoc3
            End Get
            Set(ByVal value As String)
                _xdoc3 = value
            End Set
        End Property
        Public Property nec3() As String
            Get
                Return _nec3
            End Get
            Set(ByVal value As String)
                _nec3 = value
            End Set
        End Property
        Public Property doc4() As Integer
            Get
                Return _doc4
            End Get
            Set(ByVal value As Integer)
                _doc4 = value
            End Set
        End Property
        Public Property xdoc4() As String
            Get
                Return _xdoc4
            End Get
            Set(ByVal value As String)
                _xdoc4 = value
            End Set
        End Property
        Public Property nec4() As Integer
            Get
                Return _nec4
            End Get
            Set(ByVal value As Integer)
                _nec4 = value
            End Set
        End Property
        Public Property doc5() As Integer
            Get
                Return _doc5
            End Get
            Set(ByVal value As Integer)
                _doc5 = value
            End Set
        End Property
        Public Property xdoc5() As String
            Get
                Return _xdoc5
            End Get
            Set(ByVal value As String)
                _xdoc5 = value
            End Set
        End Property
        Public Property nec5() As Integer
            Get
                Return _nec5
            End Get
            Set(ByVal value As Integer)
                _nec5 = value
            End Set
        End Property
        Public Property doc6() As Integer
            Get
                Return _doc6
            End Get
            Set(ByVal value As Integer)
                _doc6 = value
            End Set
        End Property
        Public Property xdoc6() As String
            Get
                Return _xdoc6
            End Get
            Set(ByVal value As String)
                _xdoc6 = value
            End Set
        End Property
        Public Property nec6() As Integer
            Get
                Return _nec6
            End Get
            Set(ByVal value As Integer)
                _nec6 = value
            End Set
        End Property
        Public Property doc7() As Integer
            Get
                Return _doc7
            End Get
            Set(ByVal value As Integer)
                _doc7 = value
            End Set
        End Property
        Public Property xdoc7() As String
            Get
                Return _xdoc7
            End Get
            Set(ByVal value As String)
                _xdoc7 = value
            End Set
        End Property
        Public Property nec7() As Integer
            Get
                Return _nec7
            End Get
            Set(ByVal value As Integer)
                _nec7 = value
            End Set
        End Property
        Public Property nec8() As Integer
            Get
                Return _nec8
            End Get
            Set(ByVal value As Integer)
                _nec8 = value
            End Set
        End Property

        Public Property doc8() As Integer
            Get
                Return _doc8
            End Get
            Set(ByVal value As Integer)
                _doc8 = value
            End Set
        End Property
        Public Property xdoc8() As String
            Get
                Return _xdoc8
            End Get
            Set(ByVal value As String)
                _xdoc8 = value
            End Set
        End Property
        Public Property nec9() As Integer
            Get
                Return _nec9
            End Get
            Set(ByVal value As Integer)
                _nec9 = value
            End Set
        End Property

        Public Property doc9() As Integer
            Get
                Return _doc9
            End Get
            Set(ByVal value As Integer)
                _doc9 = value
            End Set
        End Property
        Public Property xdoc9() As String
            Get
                Return _xdoc9
            End Get
            Set(ByVal value As String)
                _xdoc9 = value
            End Set
        End Property

        Public Property doc10() As Integer
            Get
                Return _doc10
            End Get
            Set(ByVal value As Integer)
                _doc10 = value
            End Set
        End Property


        Public Property doc11() As Integer
            Get
                Return _doc11
            End Get
            Set(ByVal value As Integer)
                _doc11 = value
            End Set
        End Property
        Public Property xdoc11() As String
            Get
                Return _xdoc11
            End Get
            Set(ByVal value As String)
                _xdoc11 = value
            End Set
        End Property
        Public Property nec11() As Integer
            Get
                Return _nec11
            End Get
            Set(ByVal value As Integer)
                _nec11 = value
            End Set
        End Property
        Public Property nec12() As Integer
            Get
                Return _nec12
            End Get
            Set(ByVal value As Integer)
                _nec12 = value
            End Set
        End Property
        Public Property nec13() As Integer
            Get
                Return _nec13
            End Get
            Set(ByVal value As Integer)
                _nec13 = value
            End Set
        End Property
        Public Property nec14() As Integer
            Get
                Return _nec14
            End Get
            Set(ByVal value As Integer)
                _nec14 = value
            End Set
        End Property
        Public Property nec15() As Integer
            Get
                Return _nec15
            End Get
            Set(ByVal value As Integer)
                _nec15 = value
            End Set
        End Property
        Public Property nec16() As Integer
            Get
                Return _nec16
            End Get
            Set(ByVal value As Integer)
                _nec16 = value
            End Set
        End Property
        Public Property doc12() As Integer
            Get
                Return _doc12
            End Get
            Set(ByVal value As Integer)
                _doc12 = value
            End Set
        End Property
        Public Property xdoc12() As String
            Get
                Return _xdoc12
            End Get
            Set(ByVal value As String)
                _xdoc12 = value
            End Set
        End Property

        Public Property doc13() As Integer
            Get
                Return _doc13
            End Get
            Set(ByVal value As Integer)
                _doc13 = value
            End Set
        End Property

        Public Property xdoc10() As String
            Get
                Return _xdoc10
            End Get
            Set(ByVal value As String)
                _xdoc10 = value
            End Set
        End Property
        Public Property nec10() As Integer
            Get
                Return _nec10
            End Get
            Set(ByVal value As Integer)
                _nec10 = value
            End Set
        End Property
        Public Property doc14() As Integer
            Get
                Return _doc14
            End Get
            Set(ByVal value As Integer)
                _doc14 = value
            End Set
        End Property


        Public Property doc15() As Integer
            Get
                Return _doc15
            End Get
            Set(ByVal value As Integer)
                _doc15 = value
            End Set
        End Property


        Public Property doc16() As Integer
            Get
                Return _doc16
            End Get
            Set(ByVal value As Integer)
                _doc16 = value
            End Set
        End Property


        Public Property doc17() As Integer
            Get
                Return _doc17
            End Get
            Set(ByVal value As Integer)
                _doc17 = value
            End Set
        End Property


        Public Property doc18() As Integer
            Get
                Return _doc18
            End Get
            Set(ByVal value As Integer)
                _doc18 = value
            End Set
        End Property


        Public Property doc19() As Integer
            Get
                Return _doc19
            End Get
            Set(ByVal value As Integer)
                _doc19 = value
            End Set
        End Property




#End Region

    End Class
    Public Class Plantillas

#Region "Variables miembro"

        Private _planta As Integer = Integer.MinValue
        Private _documento As Integer = Integer.MinValue
        Private _version As Integer = Integer.MinValue
        Private _nombre As String = String.Empty
        Private _Fecha As Date = Date.MinValue
        Private _FechaCad As Date = Date.MinValue



        'Private _obsoleto As Boolean = False

#End Region

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Planta() As Integer
            Get
                Return _planta
            End Get
            Set(ByVal value As Integer)
                _planta = value
            End Set
        End Property
        Public Property documento() As Integer
            Get
                Return _documento
            End Get
            Set(ByVal value As Integer)
                _documento = value
            End Set
        End Property



        Public Property version() As Integer
            Get
                Return _version
            End Get
            Set(ByVal value As Integer)
                _version = value
            End Set
        End Property
        Public Property nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

        Public Property Fecha() As Date
            Get
                Return _Fecha
            End Get
            Set(ByVal value As Date)
                _Fecha = value
            End Set
        End Property
        Public Property FechaCad() As Date
            Get
                Return _FechaCad
            End Get
            Set(ByVal value As Date)
                _FechaCad = value
            End Set
        End Property

        '''' <summary>
        '''' Nombre
        '''' </summary>
        '''' <value></value>
        '''' <returns></returns>        
        'Public Property Nombre() As String
        '    Get
        '        Return If(String.IsNullOrEmpty(_nombre), String.Empty, _nombre.Trim())
        '    End Get
        '    Set(ByVal value As String)
        '        _nombre = If(String.IsNullOrEmpty(value), String.Empty, value.Trim())
        '    End Set
        'End Property

        '''' <summary>
        '''' Descripción
        '''' </summary>
        '''' <value></value>
        '''' <returns></returns>        
        'Public Property Descripcion() As String
        '    Get
        '        Return _descripcion
        '    End Get
        '    Set(ByVal value As String)
        '        _descripcion = value
        '    End Set
        'End Property

        '''' <summary>
        '''' Obsoleto
        '''' </summary>
        '''' <value></value>
        '''' <returns></returns>        
        'Public Property Obsoleto() As Boolean
        '    Get
        '        Return _obsoleto
        '    End Get
        '    Set(ByVal value As Boolean)
        '        _obsoleto = value
        '    End Set
        'End Property

#End Region

    End Class


End Namespace

