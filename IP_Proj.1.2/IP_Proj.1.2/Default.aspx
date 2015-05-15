<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="IP_Proj._1._2._Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ozkartas dynamic Asp.net Form</title>
	<link href="css/newcss.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="https://code.jquery.com/jquery-1.10.2.js"></script>
    
     <script>
         function hideShow(myObject) {
             // alert(myObject.src.indexOf("plus.jpg") > -1);
             var o=$(myObject).closest("tr").next()
             if (myObject.src.indexOf("plus.gif") > -1) {
                 //alert(o)
                 //o.show()
                 o.css("display","")
                 myObject.src = "images/minus.png";
                 return;
             }

             if (myObject.src.indexOf("minus.png") > -1) {
                 //alert(o)
                 //o.hide()
                 o.css("display", "none")
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
