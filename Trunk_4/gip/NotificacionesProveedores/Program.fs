// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open Oracle.ManagedDataAccess
open Oracle.ManagedDataAccess.Client
open System.Net.Mail
open System.IO
open log4net

let getTodosProveedores() =  seq { 
    use conn = new Oracle.ManagedDataAccess.Client.OracleConnection("user id=sab;password=sab12;data source=//oracle-cluster:1523/xbat")
    
    use cmdNacionales = new Oracle.ManagedDataAccess.Client.OracleCommand("select e.idtroqueleria as codigo, e.nombre, u.email from empresas e inner join usuarios u on u.idempresas=e.id where e.idplanta=1 and usuario_empresa=1 and e.id<>1 and u.email is not null and e.fechabaja is null and u.fechabaja is null and  REGEXP_LIKE (u.EMAIL,'@')  and e.idtroqueleria>8204 order by e.idtroqueleria" , conn)
    conn.Open()
    use reader = cmdNacionales.ExecuteReader()
    while reader.Read() do
        yield (unbox(reader.["codigo"]), unbox(reader.["nombre"]), unbox(reader.["email"]))
    conn.Close()
    }

let getNacionales() =  seq { 
    use conn = new Oracle.ManagedDataAccess.Client.OracleConnection("user id=sab;password=sab12;data source=//igalirrintza:1521/garapen")
    
    use cmdNacionales = new Oracle.ManagedDataAccess.Client.OracleCommand("select u.email,e.id_pais from (select u.idempresas,count(*)  as c from usuarios u where u.idempresas<>1 and (u.fechabaja is null or u.fechabaja>sysdate) and u.email is not null and iddirectorioactivo is null  and codpersona is null group by u.idempresas) a1 inner join usuarios u on a1.idempresas=u.idempresas and (u.fechabaja is null or u.fechabaja>sysdate) and u.email is not null and iddirectorioactivo is null and codpersona is null inner join empresas e on a1.idempresas=e.id where a1.c=1 and REGEXP_LIKE (u.EMAIL,'@')" , conn)
    conn.Open()
    use reader = cmdNacionales.ExecuteReader()
    while reader.Read() do
        yield (unbox(reader.["email"]), reader.GetInt32(1))
    conn.Close()
    }
let mensajeNacional(msgTo : string)=
    let msg = new MailMessage("noreply@batz.es", msgTo , @"FACTURACION ELECTRONICA", """Estimado proveedor,
            
 Le escribimos en relación al envío de facturas de venta a Batz, S. Coop. por correo electrónico. En caso de utilizar esta vía para la emisión de facturas le solicitamos cumpla los requisitos indicados en la carta adjunta.
                 

Ana Camacho
Directora Financiera """)
    msg.Attachments.Add(new Attachment(@"C:\Users\aazkuenaga.BATZNT\Documents\dotnet_code\Trunk_4\SBatz\gip\NotificacionesProveedores\español.pdf","application/pdf"))
    msg.Attachments.Add(new Attachment(@"C:\Users\aazkuenaga.BATZNT\Documents\dotnet_code\Trunk_4\SBatz\gip\NotificacionesProveedores\euskera.pdf","application/pdf"))
    msg

let mensajeNoNacional(msgTo:string)=
    let msg = new MailMessage("noreply@batz.es", msgTo, @"ELECTRONIC INVOICING", """Dear supplier,
            
In case of emailing sales invoices to Batz, S. Coop.,  please meet the requirements supplied in the attached letter.
 

Ana Camacho
Financial Manager """)
    msg.Attachments.Add(new Attachment(@"C:\Users\aazkuenaga.BATZNT\Documents\dotnet_code\Trunk_4\SBatz\gip\NotificacionesProveedores\english.pdf","application/pdf")) |> ignore
    msg

let SendEmail(msg:MailMessage) = 
   use client = new SmtpClient(@"aranekoarria.elkarekin.com")
   client.Send msg

//let notificarEnvioFacturas() =
//    for (email:string, pais:int) in getNacionales() do
//        try 
//            match pais with
//            | 34 -> //System.Console.WriteLine(email + " --nacional")
//                    //SendEmail(mensajeNacional(email)) |> ignore
//                    //log.Info("Envio nacional OK " + email)
//            | _ ->  //System.Console.WriteLine(email + " --extranjero")
//                    //SendEmail(mensajeNoNacional(email)) |> ignore
//                    //log.Info("Envio NO nacional OK " + email)
//        with
//            | _ as ex -> log.Error(email)
//    SendEmail(mensajeNacional("lpzubero@batz.es")) |> ignore
//    SendEmail(mensajeNoNacional("lpzubero@batz.es")) |> ignore
let composeEmailAraluce msgTo = 
    let msg = new MailMessage("noreply@batz.es", msgTo, @"BATZ ARALUCE Acquisition", """Estimado Proveedor,

 
En adjunto la comunicación en relación a la adquisición por parte de BATZ de la empresa ARALUCE.

 
Un saludo
 


Dear Supplier,
 

Please find attached the communication regarding Batz acquisition of Araluce company.
 

Best Regards.""")
    msg.Attachments.Add(new Attachment(@"E:\BATZ_ARALUCE_Acquisition.pdf","application/pdf")) |> ignore
    msg
let notificarAdquisicionAraluce =
    for (codigo:string, nombre:string, email:string) in getTodosProveedores() do
        printf "%s " email
        composeEmailAraluce email |> SendEmail
        

let msgPortalProveedor = sprintf """Dear Sir/Madam,

The aim of this email is to inform you about the new supplier portal of BATZ Automotive Systems. Find attached a brief explanation where it is explained how to access to the portal and the different options on it. The username and password needed to access to the portal are indicated below:

Username: %s
Password: %s

Best Regards,

¿BATZ?"""
let emailNotificacionPortalProveedores msgTo username password = 
    //let msg = new MailMessage("noreply@batz.es", msgTo, @"BATZ ARALUCE Acquisition", msgPortalProveedor  username  password)
    //msg.Attachments.Add(new Attachment(@"E:\BATZ_ARALUCE_Acquisition.pdf","application/pdf")) |> ignore
     new MailMessage("noreply@batz.es", msgTo, @"BATZ Automotive Systems web portal", msgPortalProveedor  username  password)

[<EntryPoint>]
let main argv =
    //log4net.Config.XmlConfigurator.Configure( System.IO.FileInfo(@"C:\Users\aazkuenaga.BATZNT\Documents\dotnet_code\Trunk_4\SBatz\gip\NotificacionesProveedores\log4net.config")) |> ignore
    //let log = log4net.LogManager.GetLogger("root")
    // notificarEnvioFacturas()
    //notificarAdquisicionAraluce
    System.Console.WriteLine("Proceder con el encio de email?")
    System.Console.ReadLine() |> ignore
    0


