<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ProcesandoLiq.ascx.vb" Inherits="WebRaiz.ProcesandoLiq" %>
<script src="../../../js/jquery/jquery-1.6.4.min.js" type="text/javascript"></script>
<script src="../../../Scripts/jquery.signalR-2.3.0.js" type="text/javascript"></script>    
<script src="../../../signalr/hubs" type="text/javascript"></script>  
<script type="text/javascript">
    $(function () {     
        var btn = document.createElement("progress");
        btn.setAttribute("id", "myProgress1");
        btn.setAttribute("value","0");
        btn.setAttribute("max", "100");
        btn.setAttribute("class", "progress-liquidacion");
        $('#divProgress1').append(btn); 
        var btn4 = document.createElement("progress");
        btn4.setAttribute("id", "myProgress4");
        btn4.setAttribute("value","0");
        btn4.setAttribute("max", "100");
        btn4.setAttribute("class","progress-liquidacion")
        $('#divProgress4').append(btn4); 

        var myhub = $.connection.signalRHub;   
        $('#' + '<%=hfConnectionId.ClientID%>').val(myhub.connection.id); 
        myhub.client.showMessage = function (message,numStep) {               
            var mensa = $('<div />').text(message).html();    
            var imgSrcOk="../../../App_Themes/Tema1/IconosBotones/Guardar.png"
            switch (numStep) {
                case 1:
                    $('#divStep1').css({"visibility":"visible"});
                    $('#spanMessage1').text(mensa);                    
                    break;
                case 2:
                    $('#divStep1').addClass("bg-success");
                    $("#imgResul1").attr("src",imgSrcOk);
                    $('#divStep2').css({"visibility":"visible"});
                    $('#spanMessage2').text(mensa);                    
                    break;
                case 3:
                    $("#imgResul2").attr("src",imgSrcOk);
                    $('#divStep2').addClass("bg-success");
                    $('#divStep3').css({"visibility":"visible"});
                    $('#spanMessage3').text(mensa);                    
                    break;
                case 4:
                    $("#imgResul3").attr("src",imgSrcOk);
                    $('#divStep3').addClass("bg-success");
                    $('#divStep4').css({"visibility":"visible"});
                    $('#spanMessage4').text(mensa);                    
            }            
        };     
        
        myhub.client.showProgress = function (numProg, numTotal,numStep) {
            var prog;
            if(numStep==1)
               prog = document.getElementById('myProgress1');
             else if(numStep==4)
               prog = document.getElementById('myProgress4');                                  
            prog.setAttribute("value", numProg);                    
            prog.setAttribute("max", numTotal);   
        }; 

        $.connection.hub.start().done(function () {
             $('#' + '<%=btnProcesarHidden.ClientID%>').click(function () {                
                 $('#' + '<%=hfConnectionId.ClientID%>').val(myhub.connection.id);                                  
                 $('#' + '<%=labelEspere.ClientID%>').show();
                 $('#' + '<%=btnProcesarHidden.ClientID%>').hide();                 
                 $('#' + '<%=btnProcesarHidden.ClientID%>').click();                                 
            });                               
        });       
    });
</script>
<div>
    <h3><asp:Label runat="server" ID="labelInfo" Text="Procesando hojas de gastos"></asp:Label></h3>
</div><br />
<div class="form-group">
    <asp:Button runat="server" ID="btnProcesarHidden" CssClass="form-control btn btn-primary" />
    <asp:Label runat="server" ID="labelEspere" Text="Sea paciente, espere..." style="display:none"></asp:Label><br />
</div>
<div style="width:90%"> 
    <div class="row" id="divStep1" style="visibility:hidden">
        <div class="col-sm-1">                            
            <img id="imgResul1"></img>
        </div>
        <div class="col-sm-2">                            
            <asp:Label runat="server" ID="labelStep1" Text="Paso 1/4"></asp:Label>
        </div>
        <div class="col-sm-4">        
            <b><asp:Label runat="server" ID="labelInfo1" Text="Integrando las hojas en el sistema"></asp:Label></b>
        </div>  
        <div class="col-sm-2">
            <b><span id="spanMessage1"></span></b>
        </div>
        <div class="col-sm-2">
            <div id="divProgress1"></div>
        </div>    
    </div>
    <div class="row" id="divStep2" style="visibility:hidden">
        <div class="col-sm-1">                            
            <img id="imgResul2"></img>
        </div>
        <div class="col-sm-2">                            
            <asp:Label runat="server" ID="labelStep2" Text="Paso 2/4"></asp:Label>
        </div>
        <div class="col-sm-4">        
            <b><asp:Label runat="server" ID="labelInfo2" Text="Generando fichero del banco"></asp:Label></b>
        </div>  
        <div class="col-sm-2">
            <b><span id="spanMessage2"></span></b>
        </div>
        <div class="col-sm-2">
           <div><span></span></div>
        </div>    
    </div>
    <div class="row" id="divStep3" style="visibility:hidden">
        <div class="col-sm-1">                            
            <img id="imgResul3"></img>
        </div>
        <div class="col-sm-2">                            
            <asp:Label runat="server" ID="labelStep3" Text="Paso 3/4"></asp:Label>
        </div>
        <div class="col-sm-4">        
            <b><asp:Label runat="server" ID="labelInfo3" Text="Guardando los datos de la liquidacion actual"></asp:Label></b>
        </div>  
        <div class="col-sm-2">
            <b><span id="spanMessage3"></span></b>
        </div>
        <div class="col-sm-2">
            <div><span></span></div>
        </div>    
    </div>
    <div class="row" id="divStep4" style="visibility:hidden">
        <div class="col-sm-1">                            
            <img id="imgResul4"></img>
        </div>
        <div class="col-sm-2">                            
            <asp:Label runat="server" ID="labelStep4" Text="Paso 4/4"></asp:Label>
        </div>
        <div class="col-sm-4">        
            <b><asp:Label runat="server" ID="labelInfo4" Text="Enviando emails"></asp:Label></b>
        </div>  
        <div class="col-sm-2">
            <b><span id="spanMessage4"></span></b>
        </div>
        <div class="col-sm-2">
            <div id="divProgress4"></div>
        </div>    
    </div>
</div>
<asp:HiddenField runat="server" ID="hfConnectionId" />