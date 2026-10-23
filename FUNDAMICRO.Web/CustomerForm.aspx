
<%@ Page Language="VB" AutoEventWireup="false"
    CodeBehind="CustomerForm.aspx.vb"
    Inherits="FUNDAMICRO.Web.CustomerForm" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Datos del cliente</title>
    <meta charset="utf-8" />
    <style>
        body { font-family: Arial; margin: 30px; }
        .field { display: block; width: 350px; max-width: 100%;
                 padding: 9px; margin: 7px 0 15px; box-sizing: border-box; }
        .error { color: #b91c1c; }
    </style>
</head>
<body>
<form id="form1" runat="server">
    <h2>
        <asp:Label ID="lblTitle" runat="server"
            Text="Nuevo cliente" />
    </h2>

    <asp:Label runat="server" AssociatedControlID="txtFirstName"
        Text="Nombre" />
    <asp:TextBox ID="txtFirstName" runat="server"
        CssClass="field" MaxLength="250" />
    <asp:RequiredFieldValidator runat="server"
        ControlToValidate="txtFirstName"
        ErrorMessage="Ingrese el nombre."
        CssClass="error" Display="Dynamic" />

    <asp:Label runat="server" AssociatedControlID="txtLastName"
        Text="Apellido" />
    <asp:TextBox ID="txtLastName" runat="server"
        CssClass="field" MaxLength="250" />
    <asp:RequiredFieldValidator runat="server"
        ControlToValidate="txtLastName"
        ErrorMessage="Ingrese el apellido."
        CssClass="error" Display="Dynamic" />

    <asp:Label runat="server" AssociatedControlID="txtAddress"
        Text="Dirección" />
    <asp:TextBox ID="txtAddress" runat="server"
        CssClass="field" MaxLength="500" />

    <asp:Label ID="lblPhone" runat="server"
    AssociatedControlID="txtPhone"
    Text="Teléfono:" />

<asp:TextBox ID="txtPhone" runat="server"
    MaxLength="9"
    placeholder="0000-0000"
    autocomplete="tel" />

<asp:RequiredFieldValidator
    ID="rfvPhone"
    runat="server"
    ControlToValidate="txtPhone"
    ErrorMessage="El teléfono es obligatorio."
    Display="Dynamic"
    ForeColor="Red" />

<asp:RegularExpressionValidator
    ID="revPhone"
    runat="server"
    ControlToValidate="txtPhone"
    ValidationExpression="^[0-9]{4}-[0-9]{4}$"
    ErrorMessage="Ingrese el teléfono en formato 0000-0000."
    Display="Dynamic"
    ForeColor="Red"
    ValidationGroup="Customer" />

    <asp:Label ID="lblMessage" runat="server" CssClass="error" />
    <br />

    <asp:Button ID="btnSave" runat="server"
        Text="Guardar" OnClick="btnSave_Click" />

    <asp:HyperLink runat="server"
        NavigateUrl="~/Customers.aspx"
        Text="Cancelar" />
</form>
</body>
</html>
