Imports Oracle.DataAccess.Client
Imports Oracle.ManagedDataAccess.Client
Imports System.Configuration

Namespace DAL

    Public Class DocumentosDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene todos los tipos
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarTiposChar() As List(Of ELL.Kaplan)

            Dim query As String = "Select simbolo, id, Nombre, Descripcion FROM Characteristics where  obsoleto=0 "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
            New ELL.Kaplan With {.Id = CInt(r("id")), .textolibre = SabLib.BLL.Utils.stringNull(r("simbolo")), .Nombre = SabLib.BLL.Utils.stringNull(r("Nombre")), .Descripcion = SabLib.BLL.Utils.stringNull(r("Descripcion"))}, query, CadenaConexionSQL)

        End Function
        Public Function CargarTiposCharSimb(ByVal simb As String) As List(Of ELL.Kaplan)

            Dim query As String = "Select simbolo, id, Nombre, Descripcion FROM Characteristics where  obsoleto=0 and  simbolo = '" & simb & "'"

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
            New ELL.Kaplan With {.Id = CInt(r("id")), .textolibre = SabLib.BLL.Utils.stringNull(r("simbolo")), .Nombre = SabLib.BLL.Utils.stringNull(r("Nombre")), .Descripcion = SabLib.BLL.Utils.stringNull(r("Descripcion"))}, query, CadenaConexionSQL)

        End Function
        Public Function CargarTiposCharSimb2(ByVal simb As String) As List(Of ELL.Kaplan)

            Dim query As String = "Select id, Nombre, Descripcion FROM Components where obsoleto=0 and  nombre = '" & simb & "'"

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
            New ELL.Kaplan With {.Id = CInt(r("id")), .Nombre = SabLib.BLL.Utils.stringNull(r("Nombre")), .Descripcion = SabLib.BLL.Utils.stringNull(r("Descripcion"))}, query, CadenaConexionSQL)

        End Function







        'SAS
        Public Function loadProveedor(ByVal plantaAdmin As Integer, ByVal consulta As String) As List(Of ELL.Empresas)
            Dim query As String = " select id,nombre, cif from empresas where nombre = '" & consulta & "' or nombre = '" & UCase(consulta) & "' and (fechabaja > sysdate or fechabaja is null) and idplanta = " & plantaAdmin

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                               New ELL.Empresas With {.Id = CInt(r("id")), .empSAB = CInt(r("id")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Nif = SabLib.BLL.Utils.stringNull(r("cif"))}, query, CadenaConexionSAB)



        End Function
        Public Function loadProveedorExacto(ByVal plantaAdmin As Integer, ByVal consulta As String) As List(Of ELL.Empresas)


            'Si QUEREMOS HACER LA SELECT CON SAB
            Dim query As String = " select id,nombre, cif from empresas where nombre like '%" & consulta & "%' or nombre like '%" & UCase(consulta) & "%' and (fechabaja > sysdate or fechabaja is null) and idplanta = " & plantaAdmin
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                               New ELL.Empresas With {.Id = CInt(r("id")), .empSAB = CInt(r("id")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Nif = SabLib.BLL.Utils.stringNull(r("cif"))}, query, CadenaConexionSAB)




        End Function


        Public Function CargarRol(ByVal idUser As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Rol)



            Dim query As String = "Select rol001, rol002 FROM roles  WHERE  rol000=" & idUser


            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Rol)(Function(r As SqlClient.SqlDataReader) _
                         New ELL.Rol With {.rol = CInt(r("rol001")), .Id = CInt(r("rol001")), .NombreUser = SabLib.BLL.Utils.stringNull(r("rol002"))}, query, CadenaConexionSQL)


        End Function


        Public Function CargarTiposEmpresa2(ByVal nombreusuario As String) As List(Of ELL.Empresas)

            Dim query As String = "select id, fechaalta from usuarios  WHERE nombreusuario like '%" & nombreusuario & "%' order by fechaalta desc"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                             New ELL.Empresas With {.Id = SabLib.BLL.Utils.integerNull(r("id"))}, query, CadenaConexionSAB)



        End Function
        Public Function CargarTiposEmpresa(ByVal id As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)

            Dim query As String = "Select TipoC, ClaseC, Caracteristica, Caracteristica2, (max * 1000) as max, (min * 1000) as min, TrabajoSTD, Parametro, id, work, desc_work, Nombre, descripcion, obsoleto, idproceso FROM steps  WHERE  obsoleto=0 and  id =" & id

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .textolibre = SabLib.BLL.Utils.stringNull(r("desc_work")), .Work = SabLib.BLL.Utils.integerNull(r("work")), .Descripcion = SabLib.BLL.Utils.stringNull(r("descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto")), .Proceso = SabLib.BLL.Utils.integerNull(r("idproceso"))}, query, CadenaConexionSQL)


        End Function
        Public Function CargarTiposEmpresaStep(ByVal id As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)

            Dim query As String = "Select TipoC, ClaseC, Caracteristica, Caracteristica2, (max * 1000) as max, (min * 1000) as min, TrabajoSTD, Parametro, id, work, desc_work, Nombre, descripcion, obsoleto, idproceso FROM steps  WHERE  obsoleto=0 and  idProceso = " & plantaAdmin & " and  id_step =" & id

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.TipoC = SabLib.BLL.Utils.integerNull(r("TipoC")), .ClaseC = SabLib.BLL.Utils.integerNull(r("ClaseC")), .Max = SabLib.BLL.Utils.integerNull(r("Max")), .Min = SabLib.BLL.Utils.integerNull(r("Min")), .Caracteristica = SabLib.BLL.Utils.stringNull(r("Caracteristica")), .Caracteristica2 = SabLib.BLL.Utils.stringNull(r("Caracteristica2")), .TrabajoSTD = SabLib.BLL.Utils.stringNull(r("TrabajoSTD")), .Parametro = SabLib.BLL.Utils.stringNull(r("Parametro")), .Id = CInt(r("id")), .textolibre = SabLib.BLL.Utils.stringNull(r("desc_work")), .Work = SabLib.BLL.Utils.integerNull(r("work")), .Descripcion = SabLib.BLL.Utils.stringNull(r("descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto")), .Proceso = SabLib.BLL.Utils.integerNull(r("idproceso"))}, query, CadenaConexionSQL)


        End Function
        Public Function CargarTiposEmpresaStep2(ByVal id As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)

            Dim query As String = "Select TipoC, ClaseC, Caracteristica, Caracteristica2, (max * 1000) as max, (min * 1000) as min, TrabajoSTD, Parametro, id, work, desc_work, Nombre, descripcion, obsoleto, idproceso FROM steps  WHERE  obsoleto=0 and   id_step =" & id

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.TipoC = SabLib.BLL.Utils.integerNull(r("TipoC")), .ClaseC = SabLib.BLL.Utils.integerNull(r("ClaseC")), .Max = SabLib.BLL.Utils.integerNull(r("Max")), .Min = SabLib.BLL.Utils.integerNull(r("Min")), .Caracteristica = SabLib.BLL.Utils.stringNull(r("Caracteristica")), .Caracteristica2 = SabLib.BLL.Utils.stringNull(r("Caracteristica2")), .TrabajoSTD = SabLib.BLL.Utils.stringNull(r("TrabajoSTD")), .Parametro = SabLib.BLL.Utils.stringNull(r("Parametro")), .Id = CInt(r("id")), .textolibre = SabLib.BLL.Utils.stringNull(r("desc_work")), .Work = SabLib.BLL.Utils.integerNull(r("work")), .Descripcion = SabLib.BLL.Utils.stringNull(r("descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto")), .Proceso = SabLib.BLL.Utils.integerNull(r("idproceso"))}, query, CadenaConexionSQL)


        End Function
        Public Function CargarTiposEmpresaWork(ByVal idEmpresas As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)

            Dim query As String = "Select id, Nombre, descripcion, obsoleto FROM work  WHERE  obsoleto=0 and  id =" & idEmpresas

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)


        End Function

        Public Function CargarTiposEmpresaProcess(ByVal idEmpresas As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)

            Dim query As String = "Select id, Nombre, DescripcionP, DescripcionC, DescripcionU, obsoleto FROM Processes  WHERE  obsoleto=0 and  id =" & idEmpresas

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("DescripcionP")), .textolibre = SabLib.BLL.Utils.stringNull(r("DescripcionC")), .textolibre2 = SabLib.BLL.Utils.stringNull(r("DescripcionU")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)


        End Function
        Public Function CargarTiposEmpresaChar(ByVal idEmpresas As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)

            Dim query As String = "Select id, Nombre, descripcion, obsoleto, Simbolo, Clientes FROM Characteristics  WHERE obsoleto=0 and  id =" & idEmpresas

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("descripcion")), .textolibre2 = SabLib.BLL.Utils.stringNull(r("Simbolo")), .textolibre = SabLib.BLL.Utils.stringNull(r("Clientes")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)


        End Function
        Public Function CargarTiposEmpresaResulting(ByVal idEmpresas As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)

            Dim query As String = "Select id, Nombre, descripcion, obsoleto FROM Resulting  WHERE  obsoleto=0 and  id =" & idEmpresas

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)


        End Function
        Public Function CargarTiposEmpresaComponent(ByVal idEmpresas As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)

            Dim query As String = "Select id, Nombre, descripcion, obsoleto FROM Components  WHERE  obsoleto=0 and  id =" & idEmpresas

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)


        End Function

        Public Function CargarTiposStepsWork(ByVal idEmpresas As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)

            Dim query As String = "Select id,  descripcion, obsoleto, idstep, idproceso FROM stepswork  WHERE obsoleto=0 and  id =" & idEmpresas

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("descripcion")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto")), .Steps = SabLib.BLL.Utils.integerNull(r("idstep")), .Work = SabLib.BLL.Utils.integerNull(r("idproceso"))}, query, CadenaConexionSQL)


        End Function
        Public Function CargarTiposStepsWorkP(ByVal idEmpresas As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)

            Dim query As String = "Select id,  descripcion, obsoleto, idstep, idproceso FROM stepsworkParameters  WHERE  obsoleto=0 and id =" & idEmpresas

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("descripcion")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto")), .Steps = SabLib.BLL.Utils.integerNull(r("idstep")), .Work = SabLib.BLL.Utils.integerNull(r("idproceso"))}, query, CadenaConexionSQL)


        End Function

        Public Function CargarTiposStepsWorkC(ByVal idEmpresas As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)

            Dim query As String = "Select id,  descripcion, obsoleto, idstep, idcomponent FROM step_component  WHERE obsoleto=0 and  id =" & idEmpresas

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("descripcion")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto")), .Steps = SabLib.BLL.Utils.integerNull(r("idstep")), .Work = SabLib.BLL.Utils.integerNull(r("idcomponent"))}, query, CadenaConexionSQL)


        End Function


        Public Function CargarTiposStepsWorkR(ByVal idEmpresas As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)

            Dim query As String = "Select id,  descripcion, obsoleto, idstep, idresult FROM step_resulting  WHERE  obsoleto=0 and id =" & idEmpresas

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("descripcion")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto")), .Steps = SabLib.BLL.Utils.integerNull(r("idstep")), .Work = SabLib.BLL.Utils.integerNull(r("idresult"))}, query, CadenaConexionSQL)


        End Function



        Public Function CargarTiposTrabajadorXBATCIF(ByVal dni As String) As List(Of ELL.Empresas)

            Dim query As String = "select id, codpersona, fechabaja from usuarios  WHERE dni like '%" & dni & "%' " & "  and (FECHABAJA > sysdate or FECHABAJA is null ) "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                             New ELL.Empresas With {.Id = SabLib.BLL.Utils.integerNull(r("codpersona")), .FecRec = SabLib.BLL.Utils.DateNull(r("fechabaja"))}, query, CadenaConexionSAB)


        End Function
        Public Function CargarTiposTrabajadorXBATCIFNoCad(ByVal dni As String) As List(Of ELL.Empresas)

            Dim query As String = "select id, codpersona, fechabaja from usuarios  WHERE dni like '%" & dni & "%' "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                             New ELL.Empresas With {.Id = SabLib.BLL.Utils.integerNull(r("codpersona")), .FecRec = SabLib.BLL.Utils.DateNull(r("fechabaja"))}, query, CadenaConexionSAB)


        End Function
        Public Function CargarTiposTrabajadorXBATCIFTODOS(ByVal dni As String) As List(Of ELL.Empresas)

            Dim query As String = "select id, codpersona from usuarios  WHERE dni like '%" & dni & "%' "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                             New ELL.Empresas With {.Id = SabLib.BLL.Utils.integerNull(r("codpersona"))}, query, CadenaConexionSAB)


        End Function
        Public Function CargarTiposTrabajadorXBATuserPlanta(ByVal planta As Integer, ByVal user As String) As List(Of ELL.Empresas)
            If planta = 230 Then
                planta = 1
            End If
            Dim lParameters1 As New List(Of OracleParameter)

            lParameters1.Add(New OracleParameter("Fecha", OracleDbType.Date, Now, ParameterDirection.Input))

            Dim query As String = "select id from usuarios  WHERE UPPER(NOMBREUSUARIO) like '%" & user & "%' and idplanta = " & planta & "  and (FECHABAJA > :Fecha or FECHABAJA is null ) "

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                             New ELL.Empresas With {.Id = SabLib.BLL.Utils.integerNull(r("id"))}, query, CadenaConexionSAB, lParameters1.ToArray)


        End Function
        Public Function CargarTiposEmpresaXBATTroqueleria(ByVal id As Integer) As List(Of ELL.Empresas)


            Dim query As String = "select id, idtroqueleria from empresas  WHERE id = " & id


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                             New ELL.Empresas With {.Id = SabLib.BLL.Utils.integerNull(r("idtroqueleria"))}, query, CadenaConexionSAB)


        End Function
        Public Function CargarTiposTrabajadorXBATTroqueleria(ByVal id As Integer) As List(Of ELL.Empresas)


            Dim query As String = "select  id from usuarios  WHERE CODPERSONA = " & id


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Empresas)(Function(r As OracleDataReader) _
                             New ELL.Empresas With {.Id = SabLib.BLL.Utils.integerNull(r("id"))}, query, CadenaConexionSAB)


        End Function


        Public Function loadListPre(ByVal plantaAdmin As Integer) As List(Of ELL.Preventiva)



            Dim query As String = "Select id, nombre  FROM Processes where  obsoleto=0   order by id"

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Preventiva)(Function(r As SqlClient.SqlDataReader) _
            New ELL.Preventiva With {.Id = CInt(r("id")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre"))}, query, CadenaConexionSQL)


        End Function

        Public Function loadListAsociaciones0(ByVal referencia As String, ByVal componente As String, ByVal work As Integer, ByVal steps As Integer) As List(Of ELL.Asociacion)

            Dim query As String = "Select cont, TipoC, id, referencia, componente, work, step, caracacteristicas, caracacteristicas2, hijos  FROM Asociaciones where obsoleto=0  and  referencia = '" & referencia & "' and componente = '" & componente & "'  and work = " & work & " and step = " & steps & " Order by cont desc "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Asociacion)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Asociacion With {.Id = CInt(r("id")), .cont = SabLib.BLL.Utils.integerNull(r("cont")), .TipoC = SabLib.BLL.Utils.integerNull(r("tipoC")), .referencia = SabLib.BLL.Utils.stringNull(r("referencia")), .componente = SabLib.BLL.Utils.stringNull(r("componente")), .Steps = SabLib.BLL.Utils.integerNull(r("Step")), .Work = SabLib.BLL.Utils.integerNull(r("Work")), .Caracteristica = SabLib.BLL.Utils.stringNull(r("caracacteristicas")), .Caracteristica2 = SabLib.BLL.Utils.stringNull(r("caracacteristicas2")), .Hijos = SabLib.BLL.Utils.stringNull(r("Hijos"))}, query, CadenaConexionSQL)
        End Function
        Public Function loadListAsociaciones(ByVal cont As Integer, ByVal referencia As String, ByVal componente As String, ByVal work As Integer, ByVal steps As Integer) As List(Of ELL.Asociacion)

            Dim query As String = "Select cont, TipoC, id, referencia, componente, work, step, caracacteristicas, caracacteristicas2, hijos  FROM Asociaciones where obsoleto=0 and cont = " & cont & " and  referencia = '" & referencia & "' and componente = '" & componente & "'  and work = " & work & " and step = " & steps & " Order by cont desc "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Asociacion)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Asociacion With {.Id = CInt(r("id")), .cont = SabLib.BLL.Utils.integerNull(r("cont")), .TipoC = SabLib.BLL.Utils.integerNull(r("tipoC")), .referencia = SabLib.BLL.Utils.stringNull(r("referencia")), .componente = SabLib.BLL.Utils.stringNull(r("componente")), .Steps = SabLib.BLL.Utils.integerNull(r("Step")), .Work = SabLib.BLL.Utils.integerNull(r("Work")), .Caracteristica = SabLib.BLL.Utils.stringNull(r("caracacteristicas")), .Caracteristica2 = SabLib.BLL.Utils.stringNull(r("caracacteristicas2")), .Hijos = SabLib.BLL.Utils.stringNull(r("Hijos"))}, query, CadenaConexionSQL)
        End Function
        Public Function loadListAsociaciones4(ByVal cont As Integer, ByVal referencia As String, ByVal componente As String, ByVal work As Integer, ByVal steps As Integer) As List(Of ELL.Asociacion)

            Dim query As String = "Select Condicion1, Condicion2, Condicion3, Condicion4, Condicion5, Condicion6, trabajoSTD, parametro, ttipoC, tClaseC, (max * 1000) as max, (min * 1000) as min, TipoC, id, referencia, componente, work, step, caracacteristicas, caracacteristicas2, hijos  FROM Asociaciones where obsoleto=0 and cont = " & cont & " and TipoC <> 666 and referencia = '" & referencia & "' and componente = '" & componente & "'  and work = " & work & " and step = " & steps & " Order by componente asc "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Asociacion)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Asociacion With {.Id = CInt(r("id")), .Condicion1 = SabLib.BLL.Utils.stringNull(r("Condicion1")), .Condicion2 = SabLib.BLL.Utils.stringNull(r("Condicion2")), .Condicion3 = SabLib.BLL.Utils.stringNull(r("Condicion3")), .Condicion4 = SabLib.BLL.Utils.stringNull(r("Condicion4")), .Condicion5 = SabLib.BLL.Utils.stringNull(r("Condicion5")), .Condicion6 = SabLib.BLL.Utils.stringNull(r("Condicion6")), .TrabajoSTD = SabLib.BLL.Utils.stringNull(r("trabajoSTD")), .Parametro = SabLib.BLL.Utils.stringNull(r("parametro")), .tTipoC = SabLib.BLL.Utils.stringNull(r("ttipoC")), .tClaseC = SabLib.BLL.Utils.stringNull(r("tClaseC")), .Max = SabLib.BLL.Utils.integerNull(r("max")), .Min = SabLib.BLL.Utils.integerNull(r("min")), .TipoC = SabLib.BLL.Utils.integerNull(r("tipoC")), .referencia = SabLib.BLL.Utils.stringNull(r("referencia")), .componente = SabLib.BLL.Utils.stringNull(r("componente")), .Steps = SabLib.BLL.Utils.integerNull(r("Step")), .Work = SabLib.BLL.Utils.integerNull(r("Work")), .Caracteristica = SabLib.BLL.Utils.stringNull(r("caracacteristicas")), .Caracteristica2 = SabLib.BLL.Utils.stringNull(r("caracacteristicas2")), .Hijos = SabLib.BLL.Utils.stringNull(r("Hijos"))}, query, CadenaConexionSQL)
        End Function

        Public Function loadListAsociaciones2Ref(ByVal referencia As String, ByVal componente As String) As List(Of ELL.Asociacion)

            Dim query As String = "Select cont, id, descripcion, referencia, componente, work, step, caracacteristicas, caracacteristicas2, hijos  FROM Asociaciones where obsoleto=0 and  tipoC=666 and origen = '" & referencia & "'  Order by componente asc, Step asc, cont asc" 'and componente = '" & componente & "' 

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Asociacion)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Asociacion With {.Id = CInt(r("id")), .cont = SabLib.BLL.Utils.integerNull(r("cont")), .Textolibre3 = SabLib.BLL.Utils.stringNull(r("descripcion")), .referencia = SabLib.BLL.Utils.stringNull(r("referencia")), .componente = SabLib.BLL.Utils.stringNull(r("componente")), .Steps = SabLib.BLL.Utils.integerNull(r("Step")), .Work = SabLib.BLL.Utils.integerNull(r("Work")), .Caracteristica = SabLib.BLL.Utils.stringNull(r("caracacteristicas")), .Caracteristica2 = SabLib.BLL.Utils.stringNull(r("caracacteristicas2")), .Hijos = SabLib.BLL.Utils.stringNull(r("Hijos"))}, query, CadenaConexionSQL)
        End Function

        Public Function loadListAsociaciones2(ByVal referencia As String, ByVal componente As String) As List(Of ELL.Asociacion)

            Dim query As String = "Select cont, id, descripcion, referencia, componente, work, step, caracacteristicas, caracacteristicas2, hijos  FROM Asociaciones where obsoleto=0 and  tipoC=666 and referencia = '" & referencia & "' and componente = '" & componente & "'  Order by work asc, Step asc, cont asc"

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Asociacion)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Asociacion With {.Id = CInt(r("id")), .cont = SabLib.BLL.Utils.integerNull(r("cont")), .Textolibre3 = SabLib.BLL.Utils.stringNull(r("descripcion")), .referencia = SabLib.BLL.Utils.stringNull(r("referencia")), .componente = SabLib.BLL.Utils.stringNull(r("componente")), .Steps = SabLib.BLL.Utils.integerNull(r("Step")), .Work = SabLib.BLL.Utils.integerNull(r("Work")), .Caracteristica = SabLib.BLL.Utils.stringNull(r("caracacteristicas")), .Caracteristica2 = SabLib.BLL.Utils.stringNull(r("caracacteristicas2")), .Hijos = SabLib.BLL.Utils.stringNull(r("Hijos"))}, query, CadenaConexionSQL)
        End Function
        Public Function CargarListaRefComp2(ByVal referencia As String) As List(Of ELL.Asociacion)

            Dim query As String = "Select id, referencia, componente, padre,  Descripcion, hijos FROM ComponentesRef where  referencia = '" & referencia & "'  Order by componente asc"

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Asociacion)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Asociacion With {.Id = CInt(r("id")), .Textolibre = SabLib.BLL.Utils.stringNull(r("padre")), .referencia = SabLib.BLL.Utils.stringNull(r("referencia")), .componente = SabLib.BLL.Utils.stringNull(r("componente")), .desc_comp = SabLib.BLL.Utils.stringNull(r("Descripcion")), .Hijos = SabLib.BLL.Utils.stringNull(r("Hijos"))}, query, CadenaConexionSQL)
        End Function
        Public Function CargarListaRefComp(ByVal referencia As String) As List(Of ELL.Asociacion)

            Dim query As String = "Select id, referencia, componente, padre,  Descripcion, hijos FROM ComponentesRef where  componente = '" & referencia & "'  Order by componente asc"

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Asociacion)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Asociacion With {.Id = CInt(r("id")), .Textolibre = SabLib.BLL.Utils.stringNull(r("padre")), .referencia = SabLib.BLL.Utils.stringNull(r("referencia")), .componente = SabLib.BLL.Utils.stringNull(r("componente")), .desc_comp = SabLib.BLL.Utils.stringNull(r("Descripcion")), .Hijos = SabLib.BLL.Utils.stringNull(r("Hijos"))}, query, CadenaConexionSQL)
        End Function
        Public Function CargarListaRefComp0(ByVal referencia As String) As List(Of ELL.Asociacion)

            Dim query As String = " Select distinct(padre) FROM ComponentesRef where padre <> '" & referencia & "'  and   referencia =  '" & referencia & "' Order by padre asc"

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Asociacion)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Asociacion With {.referencia = SabLib.BLL.Utils.stringNull(r("padre"))}, query, CadenaConexionSQL)
        End Function

        Public Function loadListAsociaciones4(ByVal referencia As String, ByVal id As String) As List(Of ELL.Asociacion)

            Dim query As String = "Select cont, id, descripcion, referencia, componente, work, step, caracacteristicas, caracacteristicas2, hijos  FROM Asociaciones where obsoleto=0 and  tipoC=666 and id = " & id & " Order by work asc, Step asc"

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Asociacion)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Asociacion With {.Id = CInt(r("id")), .cont = SabLib.BLL.Utils.integerNull(r("cont")), .componente = SabLib.BLL.Utils.stringNull(r("componente")), .Work = SabLib.BLL.Utils.integerNull(r("Work")), .Textolibre3 = SabLib.BLL.Utils.stringNull(r("descripcion")), .referencia = SabLib.BLL.Utils.stringNull(r("referencia")), .Steps = SabLib.BLL.Utils.integerNull(r("Step")), .Hijos = SabLib.BLL.Utils.stringNull(r("Hijos"))}, query, CadenaConexionSQL)
        End Function

        Public Function loadListAsociaciones5(ByVal id As String) As List(Of ELL.Asociacion)

            Dim query As String = "Select condicion1, condicion2, Condicion3, Condicion4, Condicion5, Condicion6, cont, id, descripcion, referencia, componente, work, step, caracacteristicas, caracacteristicas2, hijos  FROM Asociaciones where obsoleto=0  and id = " & id & " Order by work asc, Step asc"

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Asociacion)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Asociacion With {.Id = CInt(r("id")), .Condicion1 = SabLib.BLL.Utils.stringNull(r("Condicion1")), .Condicion2 = SabLib.BLL.Utils.stringNull(r("Condicion2")), .Condicion3 = SabLib.BLL.Utils.stringNull(r("Condicion3")), .Condicion4 = SabLib.BLL.Utils.stringNull(r("Condicion4")), .Condicion5 = SabLib.BLL.Utils.stringNull(r("Condicion5")), .Condicion6 = SabLib.BLL.Utils.stringNull(r("Condicion6")), .cont = SabLib.BLL.Utils.integerNull(r("cont")), .componente = SabLib.BLL.Utils.stringNull(r("componente")), .Work = SabLib.BLL.Utils.integerNull(r("Work")), .Textolibre3 = SabLib.BLL.Utils.stringNull(r("descripcion")), .referencia = SabLib.BLL.Utils.stringNull(r("referencia")), .Steps = SabLib.BLL.Utils.integerNull(r("Step")), .Hijos = SabLib.BLL.Utils.stringNull(r("Hijos"))}, query, CadenaConexionSQL)
        End Function
        Public Function loadListAsociaciones7(ByVal id As String, ByVal cond As String) As List(Of ELL.Asociacion)

            Dim query As String = "select step from asociaciones where id =" & id

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Asociacion)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Asociacion With {.Id = CInt(r("step"))}, query, CadenaConexionSQL)
        End Function

        Public Function loadListAsociacionesControl(ByVal id As String, ByVal cond As String) As List(Of ELL.Asociacion)

            Dim query As String = "Select Metodo, Detectabilidad FROM Deteccion where obsoleto=0  and idCondicion=" & cond & " and idAsociacion = " & id & " Order by id desc"

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Asociacion)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Asociacion With {.Work = SabLib.BLL.Utils.integerNull(r("Detectabilidad")), .Condicion1 = SabLib.BLL.Utils.stringNull(r("Metodo"))}, query, CadenaConexionSQL)
        End Function

        Public Function loadListAsociaciones6(ByVal id As String, ByVal cond As String) As List(Of ELL.Asociacion)

            Dim query As String = "Select Calificacion, Ocurrencias, Efecto1, Efecto1_valor, Efecto2, Efecto2_valor, Efecto3, Efecto3_valor, id, idAsociacion, causa, prevencion, produccion FROM Causas where obsoleto=0  and idCondicion=" & cond & " and idAsociacion = " & id & " Order by id desc"

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Asociacion)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Asociacion With {.Work = SabLib.BLL.Utils.integerNull(r("Ocurrencias")), .Condicion3 = SabLib.BLL.Utils.stringNull(r("Efecto1")), .TipoC = SabLib.BLL.Utils.integerNull(r("Efecto1_valor")), .Condicion4 = SabLib.BLL.Utils.stringNull(r("Efecto2")), .ClaseC = SabLib.BLL.Utils.integerNull(r("Efecto2_valor")), .Condicion5 = SabLib.BLL.Utils.stringNull(r("Efecto3")), .cont = SabLib.BLL.Utils.integerNull(r("Efecto3_valor")), .Id = CInt(r("id")), .Condicion1 = SabLib.BLL.Utils.stringNull(r("causa")), .Condicion2 = SabLib.BLL.Utils.stringNull(r("prevencion")), .Textolibre2 = SabLib.BLL.Utils.stringNull(r("Calificacion")), .Textolibre = SabLib.BLL.Utils.stringNull(r("produccion"))}, query, CadenaConexionSQL)
        End Function
        Public Function loadListAsociaciones6Detec(ByVal id As Integer, ByVal cond As Integer) As List(Of ELL.Asociacion)

            Dim query As String = "Select Metodo, id, causa, Detectabilidad FROM Deteccion where obsoleto=0  and idCondicion=" & cond & " and idAsociacion = " & id & " Order by id desc"

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Asociacion)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Asociacion With {.Id = CInt(r("id")), .Work = SabLib.BLL.Utils.integerNull(r("Detectabilidad")), .Condicion1 = SabLib.BLL.Utils.stringNull(r("Metodo")), .TipoC = SabLib.BLL.Utils.integerNull(r("causa"))}, query, CadenaConexionSQL)
        End Function

        Public Function loadListAsociaciones6Detec3(ByVal id As Integer, ByVal cond As Integer) As List(Of ELL.Asociacion)

            Dim query As String = "Select Metodo, id, causa, Detectabilidad FROM Deteccion where causa=0 and obsoleto=0  and idCondicion=" & cond & " and idAsociacion = " & id & " Order by Detectabilidad asc"

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Asociacion)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Asociacion With {.Id = CInt(r("id")), .Work = SabLib.BLL.Utils.integerNull(r("Detectabilidad")), .Condicion1 = SabLib.BLL.Utils.stringNull(r("Metodo")), .TipoC = SabLib.BLL.Utils.integerNull(r("causa"))}, query, CadenaConexionSQL)
        End Function
        Public Function loadListAsociaciones6Detec2(ByVal id As Integer) As List(Of ELL.Asociacion)

            Dim query As String = "Select Metodo, id, causa, Detectabilidad FROM Deteccion where obsoleto = 0 and causa=" & id & " Order by Detectabilidad asc"

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Asociacion)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Asociacion With {.Id = CInt(r("id")), .Work = SabLib.BLL.Utils.integerNull(r("Detectabilidad")), .Condicion1 = SabLib.BLL.Utils.stringNull(r("Metodo")), .TipoC = SabLib.BLL.Utils.integerNull(r("causa"))}, query, CadenaConexionSQL)
        End Function

        Public Function loadListAsociaciones3(ByVal cont As Integer, ByVal referencia As String, ByVal componente As String, ByVal proceso As Integer, ByVal steps As Integer) As List(Of ELL.Asociacion)

            Dim query As String = "Select tipoC, id, referencia, componente, work, Step, caracacteristicas, caracacteristicas2, hijos  FROM Asociaciones where obsoleto=0 And cont = " & cont & " And tipoC<>666 And referencia = '" & referencia & "' and componente = '" & componente & "' and work = " & proceso & " and step = " & steps & " Order by work asc, Step asc"

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Asociacion)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Asociacion With {.Id = CInt(r("id")), .referencia = SabLib.BLL.Utils.stringNull(r("referencia")), .componente = SabLib.BLL.Utils.stringNull(r("componente")), .TipoC = SabLib.BLL.Utils.integerNull(r("tipoC")), .Steps = SabLib.BLL.Utils.integerNull(r("Step")), .Work = SabLib.BLL.Utils.integerNull(r("Work")), .Caracteristica = SabLib.BLL.Utils.stringNull(r("caracacteristicas")), .Caracteristica2 = SabLib.BLL.Utils.stringNull(r("caracacteristicas2")), .Hijos = SabLib.BLL.Utils.stringNull(r("Hijos"))}, query, CadenaConexionSQL)
        End Function

        Public Function loadListWork(ByVal idstep As Integer) As List(Of ELL.Asociacion)

            Dim query As String = "Select idproceso  FROM stepswork where obsoleto=0 and  idstep = " & idstep

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Asociacion)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Asociacion With {.Work = SabLib.BLL.Utils.integerNull(r("idproceso"))}, query, CadenaConexionSQL)
        End Function

        Public Function loadListStepswork() As List(Of ELL.Kaplan)

            Dim query As String = "Select id, descripcion, obsoleto, idstep, idproceso  FROM stepsWork where  obsoleto=0  Order by id asc "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("descripcion")), .Steps = SabLib.BLL.Utils.integerNull(r("idstep")), .Work = SabLib.BLL.Utils.integerNull(r("idproceso")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)
        End Function


        Public Function loadListStepsworkP() As List(Of ELL.Kaplan)

            Dim query As String = "Select id, descripcion, obsoleto, idstep, idproceso  FROM stepsWorkParameters where  obsoleto=0   Order by id asc "



            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("descripcion")), .Steps = SabLib.BLL.Utils.integerNull(r("idstep")), .Work = SabLib.BLL.Utils.integerNull(r("idproceso")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)
        End Function


        Public Function loadListStepsworkC() As List(Of ELL.Kaplan)

            Dim query As String = "Select id, descripcion, obsoleto, idstep, idcomponent  FROM step_Component where  obsoleto=0   Order by id asc "



            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("descripcion")), .Steps = SabLib.BLL.Utils.integerNull(r("idstep")), .Work = SabLib.BLL.Utils.integerNull(r("idcomponent")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)
        End Function
        Public Function loadListStepsR() As List(Of ELL.Kaplan)

            Dim query As String = "Select id, descripcion, obsoleto, idstep, idresult  FROM step_Resulting where  obsoleto=0   Order by id asc "



            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("descripcion")), .Steps = SabLib.BLL.Utils.integerNull(r("idstep")), .Work = SabLib.BLL.Utils.integerNull(r("idresult")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)
        End Function
        Public Function loadListSteps() As List(Of ELL.Kaplan)

            Dim query As String = "Select a.id, a.work, a.idProceso, a.Descripcion, a.Nombre, b.nombre as proceso, a.obsoleto FROM steps a, Processes b  WHERE a.obsoleto = 0 and a.idproceso=b.id  and id_step is null Order by a.idProceso  "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Kaplan With {.Id = CInt(r("id")), .textolibre = SabLib.BLL.Utils.stringNull(r("proceso")), .Descripcion = SabLib.BLL.Utils.stringNull(r("nombre")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto")), .Work = SabLib.BLL.Utils.integerNull(r("Work")), .Proceso = SabLib.BLL.Utils.integerNull(r("idProceso"))}, query, CadenaConexionSQL)
        End Function
        Public Function loadListSteps2(ByVal process As Integer) As List(Of ELL.Kaplan)

            Dim query As String = "Select a.id, a.idProceso, a.Descripcion, a.Nombre, b.nombre as proceso, a.obsoleto FROM steps a, Processes b  WHERE a.obsoleto = 0 and  a.idproceso=b.id and IdProceso = " & process & " and id_step is null Order by a.id "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Kaplan With {.Id = CInt(r("id")), .textolibre = SabLib.BLL.Utils.stringNull(r("proceso")), .Descripcion = SabLib.BLL.Utils.stringNull(r("nombre")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto")), .Proceso = SabLib.BLL.Utils.integerNull(r("idProceso"))}, query, CadenaConexionSQL)
        End Function
        Public Function loadListStepsTotal(ByVal id As Integer) As List(Of ELL.Asociacion)

            Dim query As String = "Select a.id, (a.max * 1000) as max, (a.min * 1000) as min, a.Caracteristica, a.Caracteristica2, a.tipoC, a.ClaseC, a.idProceso, a.Descripcion, a.Nombre, b.nombre as proceso, a.obsoleto FROM steps a, Processes b  WHERE a.obsoleto = 0 and a.idproceso=b.id  Order by a.id "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Asociacion)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Asociacion With {.Id = CInt(r("id")), .Caracteristica2 = SabLib.BLL.Utils.stringNull(r("Caracteristica2")), .Max = SabLib.BLL.Utils.integerNull(r("max")), .Min = SabLib.BLL.Utils.integerNull(r("min")), .Caracteristica = SabLib.BLL.Utils.stringNull(r("Caracteristica")), .TipoC = SabLib.BLL.Utils.integerNull(r("TipoC")), .ClaseC = SabLib.BLL.Utils.integerNull(r("ClaseC"))}, query, CadenaConexionSQL)
        End Function

        Public Function loadListStepsChar777(ByVal proceso As Integer, ByVal steps As Integer, ByVal work As Integer) As List(Of ELL.Asociacion)

            Dim query As String = "Select a.causa_valor1, a.causa_valor2, a.Causa_desc, a.Causa_ControlP, a.id, (a.max * 1000) as max, (a.min * 1000) as min, a.Caracteristica, a.Caracteristica2, a.tipoC, a.ClaseC, a.idProceso, a.Descripcion, a.Nombre, a.obsoleto, b.simbolo, b.nombre as nombre2, c.nombre as nombre3 FROM steps a left join Characteristics b on a.tipoC=b.id left join COMPONENTS c on a.ClaseC = c.id   WHERE  a.obsoleto = 0 and tipoC = 777 and a.id_step = " & steps & "  Order by a.id "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Asociacion)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Asociacion With {.Condicion1 = SabLib.BLL.Utils.stringNull(r("causa_valor1")), .Condicion2 = SabLib.BLL.Utils.stringNull(r("causa_valor2")), .Textolibre = SabLib.BLL.Utils.stringNull(r("Causa_desc")), .Textolibre2 = SabLib.BLL.Utils.stringNull(r("Causa_ControlP")), .Id = CInt(r("id")), .referencia = SabLib.BLL.Utils.stringNull(r("nombre3")), .componente = SabLib.BLL.Utils.stringNull(r("simbolo")) & " " & SabLib.BLL.Utils.stringNull(r("nombre2")), .Caracteristica2 = SabLib.BLL.Utils.stringNull(r("Caracteristica2")), .Max = SabLib.BLL.Utils.integerNull(r("max")), .Min = SabLib.BLL.Utils.integerNull(r("min")), .Caracteristica = SabLib.BLL.Utils.stringNull(r("Caracteristica")), .TipoC = SabLib.BLL.Utils.integerNull(r("TipoC")), .ClaseC = SabLib.BLL.Utils.integerNull(r("ClaseC")), .obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)
        End Function

        Public Function loadListStepsChar(ByVal proceso As Integer, ByVal steps As Integer, ByVal work As Integer) As List(Of ELL.Asociacion)

            Dim query As String = "Select a.Causa_desc, a.Causa_ControlP, a.id, (a.max * 1000) as max, (a.min * 1000) as min, a.Caracteristica, a.Caracteristica2, a.tipoC, a.ClaseC, a.idProceso, a.Descripcion, a.Nombre, a.obsoleto, b.simbolo, b.nombre as nombre2, c.nombre as nombre3 FROM steps a left join Characteristics b on a.tipoC=b.id left join COMPONENTS c on a.ClaseC = c.id   WHERE  a.obsoleto = 0 and tipoC <> 999 and a.id_step = " & steps & "  Order by a.id "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Asociacion)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Asociacion With {.Textolibre = SabLib.BLL.Utils.stringNull(r("Causa_desc")), .Textolibre2 = SabLib.BLL.Utils.stringNull(r("Causa_ControlP")), .Id = CInt(r("id")), .referencia = SabLib.BLL.Utils.stringNull(r("nombre3")), .componente = SabLib.BLL.Utils.stringNull(r("simbolo")) & " " & SabLib.BLL.Utils.stringNull(r("nombre2")), .Caracteristica2 = SabLib.BLL.Utils.stringNull(r("Caracteristica2")), .Max = SabLib.BLL.Utils.integerNull(r("max")), .Min = SabLib.BLL.Utils.integerNull(r("min")), .Caracteristica = SabLib.BLL.Utils.stringNull(r("Caracteristica")), .TipoC = SabLib.BLL.Utils.integerNull(r("TipoC")), .ClaseC = SabLib.BLL.Utils.integerNull(r("ClaseC")), .obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)
        End Function
        Public Function loadListStepsChart(ByVal proceso As Integer, ByVal steps As Integer, ByVal work As Integer) As List(Of ELL.Asociacion)

            Dim query As String = "Select a.tTipoC, a.tClaseC, a.id,  (a.max) as max, (a.min) as min, a.Caracteristica, a.Caracteristica2, a.tipoC, a.ClaseC, a.idProceso, a.Descripcion, a.Nombre, a.obsoleto, b.simbolo, b.nombre as nombre2, c.nombre as nombre3 FROM stepst a left join Characteristics b on a.tipoC=b.id left join COMPONENTS c on a.ClaseC = c.id   WHERE  a.obsoleto = 0 and tipoC <> 999 and a.id_step = " & steps & "  Order by a.id "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Asociacion)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Asociacion With {.Id = CInt(r("id")), .tTipoC = SabLib.BLL.Utils.stringNull(r("tTipoC")), .tClaseC = SabLib.BLL.Utils.stringNull(r("tClaseC")), .referencia = SabLib.BLL.Utils.stringNull(r("nombre3")), .componente = SabLib.BLL.Utils.stringNull(r("simbolo")) & " " & SabLib.BLL.Utils.stringNull(r("nombre2")), .Caracteristica2 = SabLib.BLL.Utils.stringNull(r("Caracteristica2")), .Max = SabLib.BLL.Utils.integerNull(r("max")), .Min = SabLib.BLL.Utils.integerNull(r("min")), .Caracteristica = SabLib.BLL.Utils.stringNull(r("Caracteristica")), .TipoC = SabLib.BLL.Utils.integerNull(r("TipoC")), .ClaseC = SabLib.BLL.Utils.integerNull(r("ClaseC")), .obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)
        End Function

        Public Function loadListStepsChartVV(ByVal cont As Integer, ByVal referencia As String, ByVal componente As String, ByVal work As Integer, ByVal steps As Integer) As List(Of ELL.Asociacion)

            Dim query As String = "Select a.componente, a.condicion1, a.condicion2, a.Condicion3, a.Condicion4, a.Condicion5, a.Condicion6, a.caracacteristicas, a.caracacteristicas2, a.tTipoC, a.tClaseC, a.id,  (a.max * 1000) as max, (a.min * 1000) as min, a.tipoC,  a.Descripcion, a.obsoleto, b.simbolo, b.nombre as nombre2 FROM asociaciones a left join Characteristics b on a.tipoC=b.id  WHERE  a.obsoleto = 0  and tipoC <> 666 and tipoC <> 999  and cont = " & cont & " and  referencia = '" & referencia & "' and componente = '" & componente & "'  and work = " & work & " and step = " & steps & " Order by cont desc "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Asociacion)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Asociacion With {.Id = CInt(r("id")), .Condicion1 = SabLib.BLL.Utils.stringNull(r("Condicion1")), .Condicion2 = SabLib.BLL.Utils.stringNull(r("Condicion2")), .Condicion3 = SabLib.BLL.Utils.stringNull(r("Condicion3")), .Condicion4 = SabLib.BLL.Utils.stringNull(r("Condicion4")), .Condicion5 = SabLib.BLL.Utils.stringNull(r("Condicion5")), .Condicion6 = SabLib.BLL.Utils.stringNull(r("Condicion6")), .Caracteristica = SabLib.BLL.Utils.stringNull(r("caracacteristicas")), .Caracteristica2 = SabLib.BLL.Utils.stringNull(r("caracacteristicas2")), .tTipoC = SabLib.BLL.Utils.stringNull(r("tTipoC")), .tClaseC = SabLib.BLL.Utils.stringNull(r("tClaseC")), .componente = SabLib.BLL.Utils.stringNull(r("simbolo")) & " " & SabLib.BLL.Utils.stringNull(r("nombre2")), .Max = SabLib.BLL.Utils.integerNull(r("max")), .Min = SabLib.BLL.Utils.integerNull(r("min")), .TipoC = SabLib.BLL.Utils.integerNull(r("TipoC"))}, query, CadenaConexionSQL)
        End Function
        Public Function loadListStepsChar2(ByVal proceso As Integer, ByVal steps As Integer, ByVal work As Integer) As List(Of ELL.Asociacion)

            Dim query As String = "Select a.id, a.obsoleto, (a.max * 1000) as max, (a.min * 1000) as min, a.Caracteristica, a.Caracteristica2, a.TrabajoSTD, a.Parametro, a.idProceso, a.Descripcion, a.Nombre, a.obsoleto FROM steps a  WHERE a.obsoleto= 0 and tipoC = 999 and a.id_step = " & steps & "  Order by a.id "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Asociacion)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Asociacion With {.Id = CInt(r("id")), .Caracteristica2 = SabLib.BLL.Utils.stringNull(r("Caracteristica2")), .Max = SabLib.BLL.Utils.integerNull(r("max")), .Min = SabLib.BLL.Utils.integerNull(r("min")), .Caracteristica = SabLib.BLL.Utils.stringNull(r("Caracteristica")), .TrabajoSTD = SabLib.BLL.Utils.stringNull(r("TrabajoSTD")), .Parametro = SabLib.BLL.Utils.stringNull(r("Parametro")), .obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)
        End Function
        Public Function loadListStepsChar2t(ByVal proceso As Integer, ByVal steps As Integer, ByVal work As Integer) As List(Of ELL.Asociacion)

            Dim query As String = "Select a.id, a.obsoleto, (a.max) as max, (a.min) as min, a.Caracteristica, a.Caracteristica2, a.TrabajoSTD, a.Parametro, a.idProceso, a.Descripcion, a.Nombre, a.obsoleto FROM stepst a  WHERE a.obsoleto= 0 and tipoC = 999 and a.idproceso = " & proceso & " and a.id_step = " & steps & "  Order by a.id "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Asociacion)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Asociacion With {.Id = CInt(r("id")), .Caracteristica2 = SabLib.BLL.Utils.stringNull(r("Caracteristica2")), .Max = SabLib.BLL.Utils.integerNull(r("max")), .Min = SabLib.BLL.Utils.integerNull(r("min")), .Caracteristica = SabLib.BLL.Utils.stringNull(r("Caracteristica")), .TrabajoSTD = SabLib.BLL.Utils.stringNull(r("TrabajoSTD")), .Parametro = SabLib.BLL.Utils.stringNull(r("Parametro")), .obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)
        End Function
        Public Function loadListStepsChar2tVV(ByVal cont As Integer, ByVal referencia As String, ByVal componente As String, ByVal work As Integer, ByVal steps As Integer) As List(Of ELL.Asociacion)

            Dim query As String = "Select a.trabajoSTD, a.parametro, a.componente, a.condicion1, a.condicion2, a.Condicion3, a.Condicion4, a.Condicion5, a.Condicion6, a.caracacteristicas, a.caracacteristicas2,  a.tTipoC, a.tClaseC, a.id,  (a.max * 1000) as max, (a.min * 1000) as min, a.tipoC,  a.Descripcion, a.obsoleto, b.simbolo, b.nombre as nombre2 FROM asociaciones a left join Characteristics b on a.tipoC=b.id  WHERE  a.obsoleto = 0 and tipoC = 999 and tipoC <> 666 and cont = " & cont & " and  referencia = '" & referencia & "' and componente = '" & componente & "'  and work = " & work & " and step = " & steps & " Order by cont desc "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Asociacion)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Asociacion With {.Id = CInt(r("id")), .TrabajoSTD = SabLib.BLL.Utils.stringNull(r("TrabajoSTD")), .Parametro = SabLib.BLL.Utils.stringNull(r("parametro")), .Condicion1 = SabLib.BLL.Utils.stringNull(r("Condicion1")), .Condicion2 = SabLib.BLL.Utils.stringNull(r("Condicion2")), .Condicion3 = SabLib.BLL.Utils.stringNull(r("Condicion3")), .Condicion4 = SabLib.BLL.Utils.stringNull(r("Condicion4")), .Condicion5 = SabLib.BLL.Utils.stringNull(r("Condicion5")), .Condicion6 = SabLib.BLL.Utils.stringNull(r("Condicion6")), .Caracteristica = SabLib.BLL.Utils.stringNull(r("caracacteristicas")), .Caracteristica2 = SabLib.BLL.Utils.stringNull(r("caracacteristicas2")), .tTipoC = SabLib.BLL.Utils.stringNull(r("tTipoC")), .tClaseC = SabLib.BLL.Utils.stringNull(r("tClaseC")), .componente = SabLib.BLL.Utils.stringNull(r("simbolo")) & " " & SabLib.BLL.Utils.stringNull(r("nombre2")), .Max = SabLib.BLL.Utils.integerNull(r("max")), .Min = SabLib.BLL.Utils.integerNull(r("min")), .TipoC = SabLib.BLL.Utils.integerNull(r("TipoC"))}, query, CadenaConexionSQL)
        End Function
        Public Function loadListEmpresasResulting(ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)

            Dim query As String = "Select id, nombre, obsoleto, descripcion  FROM Resulting where obsoleto = 0   Order by id asc "


            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)
        End Function
        Public Function loadListEmpresasCharacteristic(ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)

            Dim query As String = "Select id, nombre, obsoleto, descripcion  FROM Characteristics  where obsoleto = 0   Order by id asc "


            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)
        End Function

        Public Function loadListEmpresasWork(ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)

            Dim query As String = "Select id, nombre, obsoleto  FROM Work  where obsoleto = 0   Order by id asc "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("nombre")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)
        End Function


        Public Function loadListEmpresasProcess(ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)

            Dim query As String = "Select id, nombre, obsoleto  FROM Processes  where obsoleto = 0   Order by id asc "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("nombre")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)
        End Function
        Public Function loadListEmpresasResult(ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)

            Dim query As String = "Select id, nombre, obsoleto  FROM Resulting  where obsoleto = 0   Order by id asc "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("nombre")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)
        End Function

        Public Function loadListEmpresasComponent(ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)

            Dim query As String = "Select id, nombre, obsoleto  FROM Components   where obsoleto = 0  Order by id asc "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("nombre")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)
        End Function

        Public Function CargarListaCausas777(ByVal referencia As String, ByVal componente As String, ByVal work As Integer, ByVal steps As Integer) As List(Of ELL.Kaplan)

            Dim query As String = "Select id, Causa_desc, Causa_ControlP, causa_valor1, causa_valor2, obsoleto  FROM steps   where obsoleto = 0 and tipoC = 777 and id_step = " & steps & " Order by id desc "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
   New ELL.Kaplan With {.Id = CInt(r("id")), .textolibre = SabLib.BLL.Utils.stringNull(r("causa_valor1")), .textolibre2 = SabLib.BLL.Utils.stringNull(r("causa_valor2")), .Descripcion = SabLib.BLL.Utils.stringNull(r("Causa_desc")), .Nombre = SabLib.BLL.Utils.stringNull(r("Causa_ControlP")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)
        End Function

        Public Function loadListCausas(ByVal causa As Integer) As List(Of ELL.Kaplan)

            Dim query As String = "Select causa FROM Causas  WHERE  id = " & causa   ' obsoleto = 0 and 

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Descripcion = SabLib.BLL.Utils.stringNull(r("causa")), .Nombre = SabLib.BLL.Utils.stringNull(r("causa"))}, query, CadenaConexionSQL)


        End Function

        Public Function loadListEmpresas3(ByVal steps As Integer) As List(Of ELL.Kaplan)

            Dim query As String = "Select id, IdProceso, Nombre, obsoleto FROM steps  WHERE   obsoleto = 0 and id = " & steps

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("nombre")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Proceso = SabLib.BLL.Utils.integerNull(r("IdProceso")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)


        End Function
        Public Function loadListEmpresas4(ByVal workelement As Integer) As List(Of ELL.Kaplan)

            Dim query As String = "Select id, Nombre, obsoleto FROM work  WHERE  obsoleto = 0 and id = " & workelement

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("nombre")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)

        End Function

        Public Function loadListEmpresas4C(ByVal workelement As Integer) As List(Of ELL.Kaplan)

            Dim query As String = "Select id, Nombre, obsoleto FROM components  WHERE obsoleto = 0 and  id = " & workelement

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("nombre")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)

        End Function


        Public Function loadListEmpresas4R(ByVal workelement As Integer) As List(Of ELL.Kaplan)

            Dim query As String = "Select id, Nombre, obsoleto FROM resulting  WHERE obsoleto = 0 and  id = " & workelement

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("nombre")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)

        End Function


        Public Function loadListEmpresasTexto(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)

            'Dim query As String = "Select id, idProceso, Descripcion, Nombre, obsoleto FROM steps  WHERE  UPPER(nombre) like UPPER('%" & texto & "%') Order by nombre "
            Dim query As String = "Select a.id, a.idProceso, a.Descripcion, a.Nombre, b.nombre as proceso, a.obsoleto FROM steps a, Processes b  WHERE a.obsoleto = 0 and  a.idproceso=b.id And  UPPER(a.nombre) Like UPPER('%" & texto & "%') Order by a.nombre"
            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Proceso = SabLib.BLL.Utils.integerNull(r("idProceso")), .Descripcion = SabLib.BLL.Utils.stringNull(r("Descripcion")), .textolibre = SabLib.BLL.Utils.stringNull(r("proceso")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)

        End Function

        Public Function loadListEmpresasTextoProcess(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)

            Dim query As String = "Select id, DescripcionP, Nombre, obsoleto FROM Processes  WHERE obsoleto = 0 and  UPPER(nombre) like UPPER('%" & texto & "%') Order by nombre "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("Descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)

        End Function
        Public Function loadListEmpresasTextoResulting(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)

            Dim query As String = "Select id, Descripcion, Nombre, obsoleto FROM Resulting  WHERE obsoleto = 0 and  UPPER(nombre) like UPPER('%" & texto & "%') Order by nombre "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("Descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)

        End Function

        Public Function loadListEmpresasTextoCharacteristics(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)

            Dim query As String = "Select id, Descripcion, Nombre, obsoleto FROM Characteristics  WHERE obsoleto = 0 and  UPPER(nombre) like UPPER('%" & texto & "%') Order by nombre "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("Descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)

        End Function

        Public Function loadListEmpresasTextoComponent(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)

            Dim query As String = "Select id, Descripcion, Nombre, obsoleto FROM Components  WHERE obsoleto = 0 and  UPPER(nombre) like UPPER('%" & texto & "%') Order by nombre "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("Descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)

        End Function
        Public Function loadListEmpresasTextoWork(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)

            Dim query As String = "Select id, Descripcion, Nombre, obsoleto FROM work  WHERE obsoleto = 0 and   UPPER(nombre) like UPPER('%" & texto & "%') Order by nombre "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("Descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("nombre")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)

        End Function

        Public Function loadListEmpresasTexto22(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)


            Dim query As String = "Select id, Descripcion, obsoleto FROM stepsWork  WHERE obsoleto = 0 and  UPPER(Descripcion) like UPPER('%" & texto & "%') Order by Descripcion "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("Descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("Descripcion")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)

        End Function
        Public Function loadListEmpresasTexto22P(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)


            Dim query As String = "Select id, Descripcion, obsoleto FROM stepsWorkParameters  WHERE obsoleto = 0 and  UPPER(Descripcion) like UPPER('%" & texto & "%') Order by Descripcion "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("Descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("Descripcion")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)

        End Function

        Public Function loadListEmpresasTexto22C(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)


            Dim query As String = "Select id, Descripcion, obsoleto FROM step_Component  WHERE obsoleto = 0 and  UPPER(Descripcion) like UPPER('%" & texto & "%') Order by Descripcion "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("Descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("Descripcion")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)

        End Function


        Public Function loadListEmpresasTexto22R(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)


            Dim query As String = "Select id, Descripcion, obsoleto FROM step_Resulting  WHERE obsoleto = 0 and  UPPER(Descripcion) like UPPER('%" & texto & "%') Order by Descripcion "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("Descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("Descripcion")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)

        End Function

        Public Function loadListEmpresasTexto2(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)


            Dim query As String = "Select id, Descripcion, obsoleto, idStep, idproceso FROM stepsWork  WHERE obsoleto = 0 and  UPPER(Descripcion) like UPPER('%" & texto & "%') Order by Descripcion "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Steps = SabLib.BLL.Utils.integerNull(r("idStep")), .work = SabLib.BLL.Utils.integerNull(r("idproceso")), .Proceso = SabLib.BLL.Utils.integerNull(r("idproceso")), .Descripcion = SabLib.BLL.Utils.stringNull(r("Descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("Descripcion")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)

        End Function

        Public Function loadListEmpresasTexto2P(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)


            Dim query As String = "Select id, Descripcion, obsoleto, idStep, idproceso FROM stepsWorkParameters  WHERE obsoleto = 0 and  UPPER(Descripcion) like UPPER('%" & texto & "%') Order by Descripcion "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Steps = SabLib.BLL.Utils.integerNull(r("idStep")), .Work = SabLib.BLL.Utils.integerNull(r("idproceso")), .Proceso = SabLib.BLL.Utils.integerNull(r("idproceso")), .Descripcion = SabLib.BLL.Utils.stringNull(r("Descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("Descripcion")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)

        End Function

        Public Function loadListEmpresasTexto2R(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)


            Dim query As String = "Select id, Descripcion, obsoleto, idStep, idresul FROM step_Resulting  WHERE obsoleto = 0 and  UPPER(Descripcion) like UPPER('%" & texto & "%') Order by Descripcion "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Steps = SabLib.BLL.Utils.integerNull(r("idStep")), .Work = SabLib.BLL.Utils.integerNull(r("idresul")), .Descripcion = SabLib.BLL.Utils.stringNull(r("Descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("Descripcion")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)

        End Function

        Public Function loadListEmpresasTexto2C(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)


            Dim query As String = "Select id, Descripcion, obsoleto, idStep, idcomponent FROM step_Component  WHERE obsoleto = 0 and  UPPER(Descripcion) like UPPER('%" & texto & "%') Order by Descripcion "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Steps = SabLib.BLL.Utils.integerNull(r("idStep")), .Work = SabLib.BLL.Utils.integerNull(r("idcomponent")), .Proceso = SabLib.BLL.Utils.integerNull(r("idcomponent")), .Descripcion = SabLib.BLL.Utils.stringNull(r("Descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("Descripcion")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)

        End Function

        Public Function loadListDocumentosTexto(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)


            Dim query As String = "Select id, Nombre, Descripcion, idproceso, obsoleto FROM steps  WHERE obsoleto = 0 and  UPPER(Nombre) like UPPER('%" & texto & "%') Order by Nombre "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("Descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("Nombre")), .Proceso = SabLib.BLL.Utils.integerNull(r("idproceso")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)

        End Function
        Public Function loadListDocumentosTextoP(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)


            Dim query As String = "Select id, Nombre, Descripcion, idproceso, obsoleto FROM stepsWorkParameters  WHERE obsoleto = 0 and  UPPER(Nombre) like UPPER('%" & texto & "%') Order by Nombre "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("Descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("Nombre")), .Proceso = SabLib.BLL.Utils.integerNull(r("idproceso")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)

        End Function
        Public Function loadListDocumentosTextoWork(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)


            Dim query As String = "Select id, Nombre, Descripcion, obsoleto FROM work  WHERE obsoleto = 0 and  UPPER(Nombre) like UPPER('%" & texto & "%') Order by Nombre "

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.Kaplan)(Function(r As SqlClient.SqlDataReader) _
             New ELL.Kaplan With {.Id = CInt(r("id")), .Descripcion = SabLib.BLL.Utils.stringNull(r("Descripcion")), .Nombre = SabLib.BLL.Utils.stringNull(r("Nombre")), .Obsoleto = SabLib.BLL.Utils.integerNull(r("obsoleto"))}, query, CadenaConexionSQL)

        End Function
        Public Function loadListPlanta() As List(Of ELL.Documentos)

            Dim query As String = "SELECT ID, ID_BRAIN, NOMBRE FROM PLANTAS WHERE ID_BRAIN IS NOT NULL ORDER BY ID ASC"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Documentos)(Function(r As OracleDataReader) _
                     New ELL.Documentos With {.Id = CInt(r("ID")), .Abrev = SabLib.BLL.Utils.stringNull(r("ID_BRAIN")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE"))}, query, CadenaConexionSAB)

        End Function
#End Region


#Region "Modificaciones"
        ''' <summary>
        ''' Guardar un nuevo registro
        ''' </summary>
        ''' <param name="docu">Objeto Type</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SaveResulting(ByVal docu As ELL.Kaplan) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                query = "INSERT INTO resulting (Nombre,  Descripcion, Obsoleto) VALUES('" & docu.Nombre & "','" & docu.Descripcion & "'," & docu.Obsoleto & ")"

                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)


                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al grabar", ex)
            End Try
        End Function


        Public Function SaveCompRef(ByVal docu As ELL.Asociacion) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                query = "INSERT INTO ComponentesRef (referencia, componente, padre,  Descripcion, hijos) VALUES('" & docu.referencia & "','" & docu.componente & "','" & docu.Textolibre & "','" & docu.desc_comp & "','" & docu.Hijos & "')"

                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)


                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al grabar", ex)
            End Try
        End Function

        Public Function SaveCompRef0() As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "DELETE FROM ComponentesRef "

                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)


                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error", ex)
            End Try
        End Function
        ''' <summary>
        ''' Guardar un nuevo registro
        ''' </summary>
        ''' <param name="docu">Objeto Type</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SaveChar(ByVal docu As ELL.Kaplan) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                query = "INSERT INTO Characteristics (Nombre,  Descripcion, Obsoleto) VALUES('" & docu.Nombre & "','" & docu.Descripcion & "'," & docu.Obsoleto & ")"

                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)


                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al grabar un documento", ex)
            End Try
        End Function

        ''' <summary>
        ''' Guardar un nuevo registro
        ''' </summary>
        ''' <param name="docu">Objeto Type</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SaveEmp(ByVal docu As ELL.Kaplan) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try


                query = "INSERT INTO steps (Nombre,  Descripcion, IdProceso, Obsoleto) VALUES('" & docu.Nombre & "','" & docu.Descripcion & "'," & docu.Proceso & "," & docu.Obsoleto & ")"


                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)


                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al grabar un documento", ex)
            End Try
        End Function

        Public Function SaveEmp2(ByVal docu As ELL.Kaplan) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "INSERT INTO stepsWork (Descripcion, IdProceso, idstep,  Obsoleto) VALUES('" & docu.Descripcion & "'," & docu.Proceso & "," & docu.Steps & "," & docu.Obsoleto & ")"

                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)


                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error", ex)
            End Try
        End Function


        Public Function SaveOpeBorrar2(ByVal id As Integer) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "DELETE FROM Asociaciones WHERE id = " & id

                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)



                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error", ex)
            End Try
        End Function


        Public Function SaveOpeBorrarDatos(ByVal id As String) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "DELETE FROM Datos WHERE referencia = '" & id & "'"

                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)



                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error", ex)
            End Try
        End Function


        Public Function SaveOpeBorrar(ByVal docu As ELL.Asociacion) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "DELETE FROM Asociaciones WHERE cont = " & docu.cont & " and referencia = '" & docu.referencia & "' and componente = '" & docu.componente & "' and Work = " & docu.Process & " and step = " & docu.Steps

                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)
                'meto un registro unico por proceso y step
                query = "INSERT INTO Asociaciones (origen, cont, condicion1, condicion2, Condicion3, Condicion4, Condicion5, Condicion6, descripcion, TipoC,  referencia, componente, Work,  Step, hijos) VALUES('" & docu.origen & "'," & docu.cont & ", '" & docu.Textolibre2 & "','" & docu.Textolibre3 & "', '" & docu.Condicion3 & "','" & docu.Condicion4 & "', '" & docu.Condicion5 & "','" & docu.Condicion6 & "','" & docu.Textolibre & "',666,'" & docu.referencia & "','" & docu.componente & "'," & docu.Process & "," & docu.Steps & ",'" & docu.Hijos & "')"

                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)


                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error", ex)
            End Try
        End Function

        Public Function SaveOpe(ByVal docu As ELL.Asociacion) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty

            Dim maximo As String = docu.Max.ToString
            Dim minimo As String = docu.Min.ToString
            maximo = Replace(maximo, ",", ".")
            minimo = Replace(minimo, ",", ".")

            Dim lParameters1 As New List(Of OracleParameter)

            Try

                query = "INSERT INTO Asociaciones (origen, Cont, Condicion1, Condicion2, Condicion3, Condicion4, Condicion5, Condicion6,  TipoC, TrabajoSTD, Parametro, tTipoC, tClaseC, Max, Min, referencia, componente, Work,  Step, caracacteristicas, caracacteristicas2, hijos) VALUES('" & docu.origen & "'," & docu.cont & ",'" & docu.Condicion1 & "','" & docu.Condicion2 & "', '" & docu.Condicion3 & "','" & docu.Condicion4 & "', '" & docu.Condicion5 & "','" & docu.Condicion6 & "'," & docu.TipoC & ",'" & docu.TrabajoSTD & "','" & docu.Parametro & "','" & docu.tTipoC & "','" & docu.tClaseC & "'," & maximo & "," & minimo & ",'" & docu.referencia & "','" & docu.componente & "'," & docu.Process & "," & docu.Steps & ",'" & docu.Caracteristica & "','" & docu.Caracteristica2 & "','" & docu.Hijos & "')"

                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)


                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error", ex)
            End Try
        End Function


        Public Function SaveOpeDatos(ByVal docu As ELL.Datos) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty


            Dim lParameters1 As New List(Of OracleParameter)

            Try

                query = "INSERT INTO Datos (Severity, Desc_work_Larga, Desc_step_Larga, desc_ref, desc_comp, desc_work, Referencia, componente, proceso, step, codcaracteristica, codModoFallo, Works, elementos, caracteristica, ModoFallo, Atributo, Descripcion) VALUES('" & docu.Severity & "', '" & docu.Desc_work_Larga & "', '" & docu.Desc_step_Larga & "', '" & docu.Desc_Ref & "', '" & docu.Desc_comp & "', '" & docu.Desc_work & "', '" & docu.Referencia & "', '" & docu.componente & "','" & docu.proceso & "','" & docu.steps & "'," & docu.codcaracteristica & "," & docu.codModoFallo & ",'" & docu.Work & "','" & docu.elementos & "','" & docu.caracteristica & "','" & docu.ModoFallo & "','" & docu.Atributo & "','" & docu.Descripcion & "')"

                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)


                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error", ex)
            End Try
        End Function


        'Public Function SaveOpeAtri(ByVal docu As ELL.Asociacion) As Boolean

        '    Dim resultado As Boolean = False
        '    Dim query As String = String.Empty

        '    Dim maximo As String = docu.Max.ToString
        '    Dim minimo As String = docu.Min.ToString
        '    maximo = Replace(maximo, ",", ".")
        '    minimo = Replace(minimo, ",", ".")

        '    Dim lParameters1 As New List(Of OracleParameter)

        '    Try

        '        query = "INSERT INTO Asociaciones (Cont, Condicion1, Condicion2,  TipoC, TrabajoSTD, Parametro, tTipoC, tClaseC, Max, Min, referencia, componente, Work,  Step, caracacteristicas, caracacteristicas2, hijos) VALUES(" & docu.cont & ",'" & docu.Condicion1 & "','" & docu.Condicion2 & "'," & docu.TipoC & ",'" & docu.TrabajoSTD & "','" & docu.Parametro & "','" & docu.tTipoC & "','" & docu.tClaseC & "'," & maximo & "," & minimo & ",'" & docu.referencia & "','" & docu.componente & "'," & docu.Process & "," & docu.Steps & ",'" & docu.Caracteristica & "','" & docu.Caracteristica2 & "','" & docu.Hijos & "')"

        '        Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)


        '        Return True
        '    Catch ex As Exception
        '        Throw New SabLib.BatzException("Error", ex)
        '    End Try
        'End Function


        Public Function SaveOpe2Borrar(ByVal docu As ELL.Asociacion) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)
            'referencia componente seran nombre descripcion
            Try
                query = "DELETE FROM stepsT " ' WHERE nombre = '" & docu.referencia & "' and descripcion = '" & docu.componente & "'"

                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)


                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error", ex)
            End Try
        End Function

        Public Function SaveOpe3Borrar(ByVal docu As ELL.Asociacion) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)
            'referencia componente seran nombre descripcion
            Try
                query = "DELETE FROM stepsT WHERE id = " & docu.Id

                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)


                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error", ex)
            End Try
        End Function


        Public Function SaveOpe3BorrarAsociacion(ByVal docu As ELL.Asociacion) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)
            'referencia componente seran nombre descripcion
            Try
                query = "DELETE FROM asociaciones WHERE id = " & docu.Id

                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)


                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error", ex)
            End Try
        End Function

        Public Function SaveOpe2(ByVal docu As ELL.Asociacion) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)
            Dim maximo As String = docu.Max.ToString
            Dim minimo As String = docu.Min.ToString
            maximo = Replace(maximo, ",", ".")
            minimo = Replace(minimo, ",", ".")
            Try

                query = "INSERT INTO stepsT (Comentario, nombre, descripcion, idProceso, Work,  id_Step, Caracteristica, Caracteristica2, TrabajoSTD, Parametro, TipoC, ClaseC, Max, Min) VALUES('" & docu.Textolibre & "','" & docu.referencia & "','" & docu.componente & "'," & docu.Process & "," & docu.Work & "," & docu.Steps & ",'" & docu.Caracteristica & "','" & docu.Caracteristica2 & "','" & docu.TrabajoSTD & "','" & docu.Parametro & "'," & docu.TipoC & "," & docu.ClaseC & "," & maximo & "," & minimo & ")"

                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)


                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error", ex)
            End Try
        End Function



        Public Function ModOpe(ByVal docu As ELL.Asociacion) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                'si existe modificar, si no crear
                'query =
                query = "UPDATE Asociaciones SET tclasec = '" & docu.tClaseC & "', ttipoC = '" & docu.tTipoC & "', caracacteristicas2 = '" & docu.Caracteristica2 & "'  WHERE referencia = '" & docu.referencia & "' and componente = '" & docu.componente & "' and Work = " & docu.Work & " and step = " & docu.Steps

                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)


                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error", ex)
            End Try
        End Function


        Public Function SaveEmp2P(ByVal docu As ELL.Kaplan) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "INSERT INTO stepsWorkParameters (Descripcion, IdProceso, idstep,  Obsoleto) VALUES('" & docu.Descripcion & "'," & docu.Proceso & "," & docu.Steps & "," & docu.Obsoleto & ")"

                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)


                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error", ex)
            End Try
        End Function

        Public Function SaveEmp2R(ByVal docu As ELL.Kaplan) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "INSERT INTO step_resulting (Descripcion, IdResult, idstep,  Obsoleto) VALUES('" & docu.Descripcion & "'," & docu.Proceso & "," & docu.Steps & "," & docu.Obsoleto & ")"

                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)


                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error", ex)
            End Try
        End Function


        Public Function SaveEmp2C(ByVal docu As ELL.Kaplan) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                query = "INSERT INTO step_component (Descripcion, IdComponent, idstep,  Obsoleto) VALUES('" & docu.Descripcion & "'," & docu.Proceso & "," & docu.Steps & "," & docu.Obsoleto & ")"

                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)


                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error", ex)
            End Try
        End Function

        Public Function SaveEmpWork(ByVal docu As ELL.Kaplan) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "INSERT INTO work (Nombre,  Descripcion, Obsoleto) VALUES('" & docu.Nombre & "','" & docu.Descripcion & "'," & docu.Obsoleto & ")"


                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)

                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al grabar un documento", ex)
            End Try
        End Function


        Public Function SaveEmpProcess(ByVal docu As ELL.Kaplan) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try


                query = "INSERT INTO processes (Nombre,  DescripcionP, DescripcionC, DescripcionU, Obsoleto) VALUES('" & docu.Nombre & "','" & docu.Descripcion & "','" & docu.textolibre & "','" & docu.textolibre2 & "'," & docu.Obsoleto & ")"


                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)

                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al grabar un documento", ex)
            End Try
        End Function


        Public Function SaveEmpComponent(ByVal docu As ELL.Kaplan) As Boolean

            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                query = "INSERT INTO Components (Nombre,  Descripcion, Obsoleto) VALUES('" & docu.Nombre & "','" & docu.Descripcion & "'," & docu.Obsoleto & ")"
                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)

                Return True
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al grabar un documento", ex)
            End Try
        End Function


        Public Function SaveEmpSAB(ByVal docu As ELL.Empresas) As List(Of String())

            '       Dim resultado As Boolean = False
            Dim query As String = String.Empty
            '  Dim param1, param2 As SqlClient.SqlParameter
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "INSERT INTO empresas (idplanta,  cif, nombre ,fechaalta) VALUES(:planta,  :cif,  :nombre, sysdate)"

                lParameters1.Add(New OracleParameter("planta", OracleDbType.Int32, docu.Planta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("CIF", OracleDbType.Varchar2, 12, Trim(docu.Nif).ToUpper, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, 100, Trim(docu.Nombre).ToUpper, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexionSAB, lParameters1.ToArray)
                'se mira el cod empresa que se ha asignado

                Dim query2 As String = " Select EmpresasSequence.nextval   From DUAL "
                'Dim query2 As String = " Select id From empresas where id = 1141 "  'esto lo quitare
                Return Memcached.OracleDirectAccess.Seleccionar(query2, CadenaConexionSAB)

            Catch ex As Exception
                If DirectCast(ex, System.Runtime.InteropServices.ExternalException).ErrorCode = -2147467259 Then
                    Throw New SabLib.BatzException("Ya existe una empresa con ese CIF ".ToUpper & docu.Nif, ex)
                End If

                Throw New SabLib.BatzException("Error al grabar una empresa", ex)
            End Try
        End Function

        Public Function UpdateResulting(ByVal docu As ELL.Kaplan, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try

                Dim strSql As String = String.Empty

                strSql = "UPDATE Resulting SET nombre ='" & docu.Nombre & "', descripcion='" & docu.Descripcion & "', obsoleto= " & docu.Obsoleto & "  WHERE id= " & codigo


                Memcached.SQLServerDirectAccess.NoQuery(strSql, CadenaConexionSQL)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function

        Public Function UpdateChar(ByVal docu As ELL.Kaplan, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try

                Dim strSql As String = String.Empty

                strSql = "UPDATE Characteristics SET Simbolo ='" & docu.textolibre2 & "', clientes='" & docu.textolibre & "', nombre ='" & docu.Nombre & "', descripcion='" & docu.Descripcion & "', obsoleto= " & docu.Obsoleto & "  WHERE id= " & codigo


                Memcached.SQLServerDirectAccess.NoQuery(strSql, CadenaConexionSQL)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function

        Public Function UpdateEmp(ByVal docu As ELL.Kaplan, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try

                Dim strSql As String = String.Empty

                strSql = "UPDATE steps SET descripcion='" & docu.Descripcion & "', desc_work='" & docu.textolibre & "', work= " & docu.Work & ", obsoleto= " & docu.Obsoleto & "  WHERE id= " & docu.Steps


                Memcached.SQLServerDirectAccess.NoQuery(strSql, CadenaConexionSQL)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function


        Public Function UpdateEmpTotal(ByVal docu As ELL.Asociacion, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try

                Dim strSql As String = String.Empty
                Dim maximo As String = docu.Max.ToString
                Dim minimo As String = docu.Min.ToString
                maximo = Replace(maximo, ",", ".")
                minimo = Replace(minimo, ",", ".")

                'mirar si existe proces, step, work, tipoc, clasec y hacer update, else insert, si es
                'strSql = "UPDATE steps SET descripcion='" & docu.desc_comp & "', work= " & docu.Work & ", TipoC= " & docu.TipoC & ", ClaseC= " & docu.ClaseC & ", Max= " & maximo & ", Min= " & minimo & ", caracteristica= '" & docu.Caracteristica & "'  WHERE id= " & codigo
                strSql = "INSERT INTO steps (id_step, TipoC, ClaseC, caracteristica, caracteristica2, max, min, obsoleto, parametro,TrabajoSTD ) VALUES(" & docu.Steps & "," & docu.TipoC & "," & docu.ClaseC & ",'" & docu.Caracteristica & "','" & docu.Caracteristica2 & "'," & maximo & "," & minimo & ",0,'" & docu.Parametro & "','" & docu.TrabajoSTD & "')"


                Memcached.SQLServerDirectAccess.NoQuery(strSql, CadenaConexionSQL)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function


        Public Function UpdateEmpTotal2Efectos(ByVal docu As ELL.Asociacion, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try

                Dim strSql As String = String.Empty
                'Ocurrencias =" & docu.Work & ", prevencion ='" & docu.Textolibre & "'",
                strSql = "UPDATE causas SET  Efecto1 ='" & docu.Textolibre & "', Efecto2='" & docu.Textolibre2 & "', Efecto3='" & docu.Parametro & "', Efecto1_valor=" & docu.cont & ", Efecto2_valor=" & docu.ClaseC & ", Efecto3_valor=" & docu.TipoC & "  WHERE IdAsociacion= " & codigo & " AND idCondicion = " & docu.Process

                Memcached.SQLServerDirectAccess.NoQuery(strSql, CadenaConexionSQL)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function


        Public Function UpdateEmpTotal2Efectos2(ByVal docu As ELL.Asociacion, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try

                Dim strSql As String = String.Empty
                'Ocurrencias =" & docu.Work & ", prevencion ='" & docu.Textolibre & "'",
                strSql = "UPDATE causas SET  Calificacion ='" & docu.Textolibre2 & "'  WHERE causa = '" & docu.Textolibre & "' and IdAsociacion= " & codigo & " AND idCondicion = " & docu.Process

                Memcached.SQLServerDirectAccess.NoQuery(strSql, CadenaConexionSQL)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function


        Public Function UpdateEmpTotal2(ByVal docu As ELL.Asociacion, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try

                Dim strSql As String = String.Empty


                strSql = "INSERT INTO causas (ocurrencias, prevencion, idAsociacion, causa, Efecto1, Efecto2, Efecto3, Efecto1_valor, Efecto2_valor, Efecto3_valor, produccion, idCondicion ) VALUES(" & docu.Work & ",'" & docu.Condicion1 & "'," & codigo & ",'" & docu.Caracteristica & "','" & docu.Textolibre2 & "','" & docu.Parametro & "'," & docu.cont & "," & docu.ClaseC & "," & docu.TipoC & ",'" & docu.Textolibre3 & "','" & docu.Caracteristica2 & "'," & docu.Process & ")"


                Memcached.SQLServerDirectAccess.NoQuery(strSql, CadenaConexionSQL)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function


        Public Function UpdateEmpTotal2Metodo(ByVal docu As ELL.Asociacion, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try

                Dim strSql As String = String.Empty


                strSql = "INSERT INTO deteccion  (Detectabilidad, Metodo, idAsociacion, causa, idCondicion ) VALUES(" & docu.cont & ",'" & docu.Textolibre & "'," & codigo & "," & docu.Work & "," & docu.Process & ")"


                Memcached.SQLServerDirectAccess.NoQuery(strSql, CadenaConexionSQL)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function


        Public Function UpdateEmpTotalR(ByVal docu As ELL.Asociacion, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try

                Dim strSql As String = String.Empty
                Dim maximo As String = docu.Max.ToString
                Dim minimo As String = docu.Min.ToString
                maximo = Replace(maximo, ",", ".")
                minimo = Replace(minimo, ",", ".")

                'mirar si existe proces, step, work, tipoc, clasec y hacer update, else insert, si es
                'strSql = "UPDATE steps SET descripcion='" & docu.desc_comp & "', work= " & docu.Work & ", TipoC= " & docu.TipoC & ", ClaseC= " & docu.ClaseC & ", Max= " & maximo & ", Min= " & minimo & ", caracteristica= '" & docu.Caracteristica & "'  WHERE id= " & codigo
                strSql = "INSERT INTO steps (id_step, TipoC, causa_valor1, causa_valor2, causa_desc, causa_controlP,  obsoleto, parametro,TrabajoSTD ) VALUES(" & docu.Steps & "," & 777 & ",'" & docu.Textolibre & "','" & docu.Textolibre2 & "','" & docu.Caracteristica & "','" & docu.Caracteristica2 & "',0,'" & docu.Parametro & "','" & docu.TrabajoSTD & "')"


                Memcached.SQLServerDirectAccess.NoQuery(strSql, CadenaConexionSQL)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function



        Public Function UpdateEmpWork(ByVal docu As ELL.Kaplan, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try

                Dim strSql As String = String.Empty

                strSql = "UPDATE work SET nombre ='" & docu.Nombre & "', descripcion='" & docu.Descripcion & "', obsoleto= " & docu.Obsoleto & "  WHERE id= " & codigo

                Memcached.SQLServerDirectAccess.NoQuery(strSql, CadenaConexionSQL)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function




        Public Function UpdateEmpProcess(ByVal docu As ELL.Kaplan, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try

                Dim strSql As String = String.Empty

                strSql = "UPDATE processes SET nombre ='" & docu.Nombre & "', descripcionC='" & docu.textolibre & "', descripcionU='" & docu.textolibre2 & "', descripcionP='" & docu.Descripcion & "', obsoleto= " & docu.Obsoleto & "  WHERE id= " & codigo

                Memcached.SQLServerDirectAccess.NoQuery(strSql, CadenaConexionSQL)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function


        Public Function UpdateEmpComponent(ByVal docu As ELL.Kaplan, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try

                Dim strSql As String = String.Empty

                strSql = "UPDATE Components SET nombre ='" & docu.Nombre & "', descripcion='" & docu.Descripcion & "', obsoleto= " & docu.Obsoleto & "  WHERE id= " & codigo

                Memcached.SQLServerDirectAccess.NoQuery(strSql, CadenaConexionSQL)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function



        Public Function UpdateEmp2(ByVal docu As ELL.Kaplan, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try

                Dim strSql As String = String.Empty

                strSql = "UPDATE stepsWork SET nombre ='" & docu.Nombre & "', descripcion='" & docu.Descripcion & "', idstep=" & docu.Steps & ", idproceso= " & docu.Proceso & ", obsoleto= " & docu.Obsoleto & "  WHERE id= " & codigo

                Memcached.SQLServerDirectAccess.NoQuery(strSql, CadenaConexionSQL)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function

        Public Function UpdateEmp2C(ByVal docu As ELL.Kaplan, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try

                Dim strSql As String = String.Empty

                strSql = "UPDATE step_component SET nombre ='" & docu.Nombre & "', descripcion='" & docu.Descripcion & "', idstep=" & docu.Steps & ", idproceso= " & docu.Proceso & ", obsoleto= " & docu.Obsoleto & "  WHERE id= " & codigo

                Memcached.SQLServerDirectAccess.NoQuery(strSql, CadenaConexionSQL)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function


        Public Function UpdateEmp2R(ByVal docu As ELL.Kaplan, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try

                Dim strSql As String = String.Empty

                strSql = "UPDATE step_resulting SET nombre ='" & docu.Nombre & "', descripcion='" & docu.Descripcion & "', idstep=" & docu.Steps & ", idresult= " & docu.Proceso & ", obsoleto= " & docu.Obsoleto & "  WHERE id= " & codigo

                Memcached.SQLServerDirectAccess.NoQuery(strSql, CadenaConexionSQL)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function


        Public Function UpdateEmp2P(ByVal docu As ELL.Kaplan, ByVal codigo As Int32) As Boolean
            Dim resultado As Boolean = False
            Try

                Dim strSql As String = String.Empty

                strSql = "UPDATE stepsWorkParameters SET nombre ='" & docu.Nombre & "', descripcion='" & docu.Descripcion & "', idstep=" & docu.Steps & ", idproceso= " & docu.Proceso & ", obsoleto= " & docu.Obsoleto & "  WHERE id= " & codigo

                Memcached.SQLServerDirectAccess.NoQuery(strSql, CadenaConexionSQL)

                Return True
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function




        Public Function UpdateEmp(ByVal docu As ELL.Kaplan) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                'actualizamos campo tra012 por desactivar = 1
                query = "UPDATE steps SET obsoleto = " & docu.Obsoleto & " WHERE id= " & docu.Id


                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error ", ex)
            End Try
            Return resultado
        End Function



        Public Function UpdateEmpResulting(ByVal docu As ELL.Kaplan) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                'actualizamos campo tra012 por desactivar = 1
                query = "UPDATE Resulting SET obsoleto = " & docu.Obsoleto & " WHERE id= " & docu.Id


                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error ", ex)
            End Try
            Return resultado
        End Function



        Public Function UpdateSteps(ByVal docu As ELL.Kaplan) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                'actualizamos campo tra012 por desactivar = 1
                query = "UPDATE steps SET obsoleto = " & docu.Obsoleto & " WHERE id= " & docu.Id


                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error ", ex)
            End Try
            Return resultado
        End Function



        Public Function UpdateCausa(ByVal docu As ELL.Kaplan) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                'actualizamos campo tra012 por desactivar = 1
                query = "UPDATE Causas SET obsoleto = " & docu.Obsoleto & " WHERE id= " & docu.Id


                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error ", ex)
            End Try
            Return resultado
        End Function


        Public Function UpdateDetec(ByVal docu As ELL.Kaplan) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                'actualizamos campo tra012 por desactivar = 1
                query = "UPDATE Deteccion SET obsoleto = " & docu.Obsoleto & " WHERE id= " & docu.Id


                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error ", ex)
            End Try
            Return resultado
        End Function


        Public Function UpdateEmpChar(ByVal docu As ELL.Kaplan) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                'actualizamos campo tra012 por desactivar = 1
                query = "UPDATE Characteristics SET obsoleto = " & docu.Obsoleto & " WHERE id= " & docu.Id


                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error ", ex)
            End Try
            Return resultado
        End Function

        Public Function UpdateEmpWork(ByVal docu As ELL.Kaplan) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                'actualizamos campo tra012 por desactivar = 1
                query = "UPDATE work SET obsoleto = " & docu.Obsoleto & " WHERE id= " & docu.Id


                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error ", ex)
            End Try
            Return resultado
        End Function


        Public Function UpdateEmpProcess(ByVal docu As ELL.Kaplan) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                'actualizamos campo tra012 por desactivar = 1
                query = "UPDATE processes SET obsoleto = " & docu.Obsoleto & " WHERE id= " & docu.Id


                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error ", ex)
            End Try
            Return resultado
        End Function


        Public Function UpdateEmpComponent(ByVal docu As ELL.Kaplan) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                'actualizamos campo tra012 por desactivar = 1
                query = "UPDATE Components SET obsoleto = " & docu.Obsoleto & " WHERE id= " & docu.Id


                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error ", ex)
            End Try
            Return resultado
        End Function


        Public Function UpdateEmp2(ByVal docu As ELL.Kaplan) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                'actualizamos campo tra012 por desactivar = 1
                query = "UPDATE stepsWork SET obsoleto = " & docu.Obsoleto & " WHERE id= " & docu.Id


                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error", ex)
            End Try
            Return resultado
        End Function



        Public Function UpdateEmp2CharATR(ByVal docu As ELL.Asociacion) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Dim maximo As String = docu.Max.ToString
            Dim minimo As String = docu.Min.ToString
            maximo = Replace(maximo, ",", ".")
            minimo = Replace(minimo, ",", ".")
            Try

                'actualizamos campo tra012 por desactivar = 1
                query = "UPDATE stepst SET  TrabajoSTD = '" & docu.TrabajoSTD & "', Parametro = '" & docu.Parametro & "', Caracteristica = '" & docu.Caracteristica & "', Caracteristica2 = '" & docu.Caracteristica2 & "', Max = " & maximo & ", Min = " & minimo & " WHERE id= " & docu.Id


                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error", ex)
            End Try
            Return resultado
        End Function



        Public Function UpdateEmp2Char(ByVal docu As ELL.Asociacion) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Dim maximo As String = docu.Max.ToString
            Dim minimo As String = docu.Min.ToString
            maximo = Replace(maximo, ",", ".")
            minimo = Replace(minimo, ",", ".")
            Try

                'actualizamos campo tra012 por desactivar = 1
                query = "UPDATE stepst SET  Caracteristica = '" & docu.Caracteristica & "', Caracteristica2 = '" & docu.Caracteristica2 & "', tClaseC = '" & docu.tClaseC & "', tTipoC = '" & docu.tTipoC & "', Max = " & maximo & ", Min = " & minimo & " WHERE id= " & docu.Id


                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error", ex)
            End Try
            Return resultado
        End Function


        Public Function UpdateEmp2CharVVV(ByVal docu As ELL.Asociacion) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Dim maximo As String = docu.Max.ToString
            Dim minimo As String = docu.Min.ToString
            maximo = Replace(maximo, ",", ".")
            minimo = Replace(minimo, ",", ".")
            Try

                'actualizamos campo tra012 por desactivar = 1
                query = "UPDATE Asociaciones SET  TrabajoSTD = '" & docu.Caracteristica & "', parametro = '" & docu.Caracteristica2 & "', caracacteristicas = '" & docu.tClaseC & "', caracacteristicas2 = '" & docu.tTipoC & "', Max = " & maximo & ", Min = " & minimo & " WHERE id= " & docu.Id


                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error", ex)
            End Try
            Return resultado
        End Function


        Public Function UpdateEmp2CharVV(ByVal docu As ELL.Asociacion) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Dim maximo As String = docu.Max.ToString
            Dim minimo As String = docu.Min.ToString
            maximo = Replace(maximo, ",", ".")
            minimo = Replace(minimo, ",", ".")
            Try

                'actualizamos campo tra012 por desactivar = 1
                query = "UPDATE Asociaciones SET  Caracacteristicas = '" & docu.Caracteristica & "', Caracacteristicas2 = '" & docu.Caracteristica2 & "', tClaseC = '" & docu.tClaseC & "', tTipoC = '" & docu.tTipoC & "', Max = " & maximo & ", Min = " & minimo & " WHERE id= " & docu.Id


                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error", ex)
            End Try
            Return resultado
        End Function


        Public Function UpdateEmp3Char(ByVal docu As ELL.Asociacion) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Dim maximo As String = docu.Max.ToString
            Dim minimo As String = docu.Min.ToString
            maximo = Replace(maximo, ",", ".")
            minimo = Replace(minimo, ",", ".")
            Try

                'actualizamos campo tra012 por desactivar = 1
                query = "UPDATE stepst SET Condicion1 = '" & docu.Condicion1 & "', Condicion2 = '" & docu.Condicion2 & "', Condicion3 = '" & docu.Condicion3 & "', Condicion4 = '" & docu.Condicion4 & "', Condicion5 = '" & docu.Condicion5 & "', Condicion6 = '" & docu.Condicion6 & "' WHERE id= " & docu.Id


                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error", ex)
            End Try
            Return resultado
        End Function



        Public Function ModificarCondiciones(ByVal docu As ELL.Asociacion) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Dim maximo As String = docu.Max.ToString
            Dim minimo As String = docu.Min.ToString
            maximo = Replace(maximo, ",", ".")
            minimo = Replace(minimo, ",", ".")
            Try

                'actualizamos campo tra012 por desactivar = 1
                query = "UPDATE stepst SET Condicion1 = '" & docu.Caracteristica & "', Condicion1 = '" & docu.Caracteristica2 & "', Condicion1 = '" & docu.tClaseC & "', Condicion1 = '" & docu.tTipoC & "', Max = " & 0 & ", Min = " & 0 & " WHERE id= " & docu.Id


                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error", ex)
            End Try
            Return resultado
        End Function



        Public Function UpdateEmp2P(ByVal docu As ELL.Kaplan) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                'actualizamos campo tra012 por desactivar = 1
                query = "UPDATE stepsWorkParameters SET obsoleto = " & docu.Obsoleto & " WHERE id= " & docu.Id


                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error", ex)
            End Try
            Return resultado
        End Function



        Public Function UpdateEmp2C(ByVal docu As ELL.Kaplan) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                'actualizamos campo tra012 por desactivar = 1
                query = "UPDATE step_component SET obsoleto = " & docu.Obsoleto & " WHERE id= " & docu.Id


                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error", ex)
            End Try
            Return resultado
        End Function



        Public Function UpdateEmp2R(ByVal docu As ELL.Kaplan) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try

                'actualizamos campo tra012 por desactivar = 1
                query = "UPDATE step_resulting SET obsoleto = " & docu.Obsoleto & " WHERE id= " & docu.Id


                Memcached.SQLServerDirectAccess.NoQuery(query, CadenaConexionSQL)

                Return True

            Catch ex As Exception
                Throw New SabLib.BatzException("Error", ex)
            End Try
            Return resultado
        End Function





#End Region

    End Class

End Namespace