<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Asp-Net-Sample.aspx.cs" Inherits="SampleWeb.Asp_Net_Sample" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Button ID="btnSearchAccount" runat="server" 
            Text="Search accounts ny name:" onclick="btnSearchAccount_Click" 
             />
        <asp:TextBox ID="txtAccountSearch" runat="server"></asp:TextBox>
    
        <br />
        <asp:Label ID="lblAccountResult" runat="server"></asp:Label>
        <br />
        <br />
    
    </div>
    </form>
</body>
</html>
