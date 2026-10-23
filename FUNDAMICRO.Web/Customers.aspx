
<%@ Page Language="VB" AutoEventWireup="false"
    CodeBehind="Customers.aspx.vb"
    Inherits="FUNDAMICRO.Web.Customers" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Administración de clientes</title>
    <meta charset="utf-8" />
    <style>
        body { font-family: Arial; margin: 30px; }
        table { border-collapse: collapse; width: 100%; }
        th, td { border: 1px solid #ddd; padding: 9px; }
        th { background: #075985; color: white; }
        .actions { margin-bottom: 20px; }
        .message { color: #166534; }
    </style>
</head>
<body>
<form id="form1" runat="server">
    <h2>Administración de clientes</h2>

    <p>
        Usuario:
        <asp:Label ID="lblUser" runat="server" />
        &nbsp;|&nbsp;
        <asp:HyperLink runat="server" NavigateUrl="~/LogCustomers.aspx"
            Text="Consultar bitácora" />
        &nbsp;|&nbsp;
        <asp:HyperLink runat="server" NavigateUrl="~/Logout.aspx"
            Text="Cerrar sesión" />
    </p>

    <div class="actions">
        <asp:Button ID="btnNew" runat="server"
            Text="Nuevo cliente" OnClick="btnNew_Click" />
    </div>

    <asp:Label ID="lblMessage" runat="server" CssClass="message" />

    <asp:GridView ID="gvCustomers" runat="server"
        AutoGenerateColumns="False"
        DataKeyNames="Id"
        OnRowCommand="gvCustomers_RowCommand"
        EmptyDataText="No hay clientes activos.">

        <Columns>
            <asp:BoundField DataField="Id" HeaderText="ID" />
            <asp:BoundField DataField="FirtName" HeaderText="Nombre" />
            <asp:BoundField DataField="LastName" HeaderText="Apellido" />
            <asp:BoundField DataField="Address" HeaderText="Dirección" />
            <asp:BoundField DataField="Phone" HeaderText="Teléfono" />

            <asp:ButtonField Text="Editar"
                CommandName="EditCustomer" />

            <asp:ButtonField Text="Dar de baja"
                CommandName="DeactivateCustomer" />
        </Columns>
    </asp:GridView>
</form>
</body>
</html>
