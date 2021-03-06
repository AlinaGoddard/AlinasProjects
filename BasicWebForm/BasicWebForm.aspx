﻿<%@ Page Title="Basic Web Form" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BasicWebForm.aspx.cs" Inherits="BasicWebForm.BasicRecords" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Basic Web Form</h2>
    <h3>A basic web form that takes input data, saves it to a database and displays a list of saved records.  The form should use JQuery to validate input and display errors.  The framework should also validate the data before it inserts it into the database.  That makes sure the info is validated if JavaScript isn't enabled on the front end.</h3>
    <script>  
        $(function () {
            $('table[id*=grdRecords] input[id*=txtName]').blur(function () {
                var itemValue = $(this).val();
                var minLength = 2;
                if (!itemValue)
                    alert('Name is a required field.');
                else if (itemValue.length < minLength)
                    alert('Name must have at least ' + (minLength - 1).toString() + ' character.');
            });
        });
        function DeleteConfirm(id) {
            id = id.substring(id.lastIndexOf("_") + 1, id.length);
            var rowsCount = <%=grdRecords.Rows.Count %> -1;
            if (id.toString() == rowsCount.toString())
                alert("This row has not been added yet.");
            else {
                if (confirm("Are you sure you want to delete this name?")) {
                    window.location.replace("BasicWebForm.aspx?id=" + id);
                }
            }
        }
    </script>
    <asp:GridView ID="grdRecords" runat="server" AutoGenerateColumns="false" OnRowEditing="grdRecords_RowEditing" EnableViewState="false">
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="Id" />
            <asp:TemplateField HeaderText="Name">
                <ItemTemplate>
                    <asp:TextBox ID="txtName" runat="server" Text='<%# Eval("Name") %>'></asp:TextBox>
                    <asp:RequiredFieldValidator ForeColor="Red" runat="server" ID="ValidatorRequiredField" ControlToValidate="txtName" Text="!"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ForeColor="Red" ControlToValidate="txtName" ID="RegularExpressionValidator" ValidationExpression="^[a-zA-Z0-9'@&#.\s]{2,}$" runat="server" ErrorMessage="!"></asp:RegularExpressionValidator>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:ButtonField runat="server" DataTextField="Type" ButtonType="Button" DataTextFormatString="{0}" CommandName="Edit" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Button ID="lnk_Remove" runat="server" Text="Delete" OnClientClick="DeleteConfirm(id)"></asp:Button>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <RowStyle BackColor="#EFF3FB" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Center"  />
    </asp:GridView>
    <asp:Label runat="server" ID="lblErrors" ForeColor="Red"></asp:Label>
</asp:Content>
