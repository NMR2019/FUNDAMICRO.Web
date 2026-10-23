
<%@ Page Language="VB" AutoEventWireup="false"
    CodeBehind="SetupUser.aspx.vb"
    Inherits="FUNDAMICRO.Web.SetupUser" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Crear administrador inicial</title>
</head>
<body>
<form id="form1" runat="server">
    <h2>Crear administrador inicial</h2>

    <asp:Label runat="server" Text="Nombre completo" />
    <br />
    <asp:TextBox ID="txtFullName" runat="server" MaxLength="250" />
    <br /><br />

    <asp:Label runat="server" Text="Usuario" />
    <br />
    <asp:TextBox ID="txtUserName" runat="server" MaxLength="250" />
    <br /><br />

    <asp:Label ID="lblEmail" runat="server"
    AssociatedControlID="txtEmail"
    Text="Correo electrónico:" />
    <br />

<asp:TextBox ID="txtEmail" runat="server"
    MaxLength="250"
    TextMode="Email"
    placeholder="usuario@dominio.com" />

<asp:RegularExpressionValidator
    ID="revEmail"
    runat="server"
    ControlToValidate="txtEmail"
    ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
    ErrorMessage="Ingrese un correo electrónico válido. Ejemplo: usuario@dominio.com"
    Display="Dynamic"
    ForeColor="Red"
    ValidationGroup="User" />
    <br /><br />

    <asp:Label runat="server" Text="Contraseña" />
    <br />
    <asp:TextBox ID="txtPassword" runat="server"
        TextMode="Password" />
    <br /><br />

    <asp:Label runat="server" Text="Confirmar contraseña" />
    <br />
    <asp:TextBox ID="txtConfirmPassword" runat="server"
        TextMode="Password" />
    <br /><br />

    <asp:Button ID="btnCreate" runat="server"
        Text="Crear administrador" OnClick="btnCreate_Click" />

    <br /><br />
    <asp:Label ID="lblMessage" runat="server" />
</form>
</body>
</html>
