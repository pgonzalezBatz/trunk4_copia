<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Test_SignalR.aspx.vb" Inherits="WebRaiz.Test_SignalR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>    
    <link id="ctl00_linkCSS" rel="stylesheet" type="text/css" href="http://intranet.batz.es/BaliabideOrokorrak/estiloIntranet.css" />
    <link href="App_Themes/Tema1/style.css" rel="stylesheet" type="text/css" />    
    <title></title>
    <script>
        function test() {
            var btn = document.createElement("progress");
            btn.setAttribute("id", "myProgress");
            btn.setAttribute("value","50");
            btn.setAttribute("max", "100");
            $('#divProgress').append(btn);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Button runat="server" ID="btnTest" OnClientClick="test();" />
        <asp:Button runat="server" ID="btnTestHidden"  />
        <asp:Panel runat="server" ID="pnlTest" visible="false">
            Hola
        </asp:Panel>
         <span id="spanMessage" class="labelDetalle"></span>
        <div id="divProgress"></div>
        <asp:HiddenField runat="server" ID="hfResul" />
        <script src="js/jquery/jquery-1.6.4.min.js"></script>
        <script src="js/jquery/jquery.signalR-2.3.0.js"></script>        
        <script src="signalr/hubs"></script>        
        <script type="text/javascript">
            $(function () {                
                var myhub = $.connection.signalRHub;   
                myhub.client.showMessage = function (message) {
                    // Html encode display step and message.                
                    var message = $('<div />').text(message).html();
                    $('#spanMessage').text(message);
                }; 
                myhub.client.sendMessage = function (value, max) {                    
                    var prog = document.getElementById('myProgress');
                    prog.setAttribute("value", value);                    
                    prog.setAttribute("max", max);                                        
                }; 
                myhub.client.redirectTo = function (url) {
                   window.location.href= '<%= Page.ResolveUrl("' + url +'") %>';
                }
                $.connection.hub.logging = true;
                $.connection.hub.start().done(function () {
                    $('#' + '<%=btnTest.ClientID%>').click(function () {   
                        var btn = document.createElement("progress");
                        btn.setAttribute("id", "myProgress");
                        btn.setAttribute("value", "0");
                        btn.setAttribute("max", "100");
                        $('#divProgress').append(btn);
                        //myhub.server.send("Mensaje enviado por el boton");                
                        $('#' + '<%=btnTestHidden.ClientID%>').click();                        
                    });
                });
            });
        </script>
    </form>
</body>
</html>
