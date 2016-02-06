<%@ Page Title="Basic Web Form" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BasicWebForm.aspx.cs" Inherits="BasicWebForm.BasicRecords" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Basic Web Form</h2>
    <h3>A basic web form that takes input data, saves it to a database and displays a list of saved records.  The form should use JQuery to validate input and display errors.  The framework should also validate the data before it inserts it into the database.  That makes sure the info is validated if JavaScript isn't enabled on the front end.</h3>
    <asp:GridView ID="grdRecords" runat="server"  >
    </asp:GridView>
</asp:Content>