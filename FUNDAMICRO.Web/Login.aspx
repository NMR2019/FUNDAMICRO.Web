
<%@ Page Language="VB" AutoEventWireup="false"
    CodeBehind="Login.aspx.vb"
    Inherits="FUNDAMICRO.Web.Login" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>FUNDAMICRO - Inicio de sesión</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <style>
        body {
            font-family: Arial, sans-serif;
            background: #f2f4f7;
            margin: 0;
        }
        .login-box {
            max-width: 380px;
            margin: 80px auto;
            padding: 28px;
            background: white;
            border-radius: 8px;
            box-shadow: 0 2px 12px #ccc;
        }
        .field {
            width: 100%;
            box-sizing: border-box;
            padding: 10px;
            margin: 8px 0 16px;
        }
        .btn {
            width: 100%;
            padding: 11px;
            background: #075985;
            color: white;
            border: 0;
            cursor: pointer;
        }
        .error {
            color: #b91c1c;
        }
    </style>
</head>
<body>
<form id="form1" runat="server">
    <div class="login-box">
        <h2>FUNDAMICRO</h2>
        <p>Ingrese sus credenciales</p>

        <asp:Label runat="server" AssociatedControlID="txtUser"
            Text="Usuario" />
        <asp:TextBox ID="txtUser" runat="server"
            CssClass="field" MaxLength="250" />

        <asp:Label runat="server" AssociatedControlID="txtPassword"
            Text="Contraseña" />
        <asp:TextBox ID="txtPassword" runat="server"
            CssClass="field" TextMode="Password" />

        <asp:Button ID="btnLogin" runat="server"
            Text="Iniciar sesión" CssClass="btn" />

        <br /><br />
        <asp:Label ID="lblMessage" runat="server"
            CssClass="error" />
    </div>
</form>
</body>
</html>
