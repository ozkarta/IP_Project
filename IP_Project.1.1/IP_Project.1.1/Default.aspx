<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="IP_Project._1._1._Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ozkartas dynamic Asp.net Form</title>
	<link href="css/newcss.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="https://code.jquery.com/jquery-1.10.2.js"></script>
    
     <script type="text/javascript">
         function hideShow(myObject) {
            // alert(myObject.src.indexOf("plus.jpg") > -1);

             if(myObject.src.indexOf("plus.gif")>-1)
             {
                 $(myObject).closest("tr").after("<tr><td></td><td colspan = '100%'>" + $(myObject).next().html() + "</td></tr>");
                 $(myObject).next().remove();
                 myObject.src = "images/minus.png";
                 return;
             }
             
             if (myObject.src.indexOf("minus.png") > -1) {
                 $(myObject).after("<div style=\"display:none\">" + $(myObject).closest("tr").next().html()+ "</div>")
                 $(myObject).closest("tr").next().remove();
                 myObject.src = "images/plus.gif";
                 return;
             }
         }
    </script>

</head>
<body>
    <form id="mainForm" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
       
        <hr />

        <center>
            <h1>
                This Is Dynamic  GridView  In VB <br />
                Author: Ozbegi Kartvelishvili
            </h1>
        </center>
        <hr />

    </form>
   
</body>
</html>
