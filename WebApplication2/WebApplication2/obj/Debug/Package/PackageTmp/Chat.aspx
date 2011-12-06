<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Chat.aspx.vb" Inherits="WebApplication2.Chat" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2 align="left">
        Welcome To EFC Chat!</h2>
   </align>
    
    <p>
        <align ="right" >
        <asp:ListBox ID="ListBox1" runat="server" AppendDataBoundItems="True" 
            AutoPostBack="True" CausesValidation="True" Height="200px" Width="100px" 
            align="right" style="text-align: right" ></asp:ListBox>
    </p>
    </align>
</asp:Content>

   