
<%@ Page Language="VB" AutoEventWireup="false"
    CodeBehind="LogCustomers.aspx.vb"
    Inherits="FUNDAMICRO.Web.LogCustomers" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Bitácora de clientes</title>
    <meta charset="utf-8" />
    <style>
        body { font-family: Arial; margin: 25px; }
        .grid { width: 100%; }
        .grid th, .grid td {
            border: 1px solid #ddd;
            padding: 8px;
            vertical-align: top;
        }
        .grid th { background: #075985; color: white; }
    </style>
</head>
<body>
<form id="form1" runat="server">
    <h2>Bitácora de clientes</h2>

    <p>
        <asp:HyperLink runat="server"
            NavigateUrl="~/Customers.aspx"
            Text="Regresar a clientes" />
    </p>

    <asp:GridView ID="gvLogs" runat="server"
        CssClass="grid"
        AutoGenerateColumns="False"
        EmptyDataText="No existen registros en la bitácora.">

        <Columns>
            <asp:BoundField DataField="Id" HeaderText="ID bitácora" />
            <asp:BoundField DataField="IdCustomer" HeaderText="ID cliente" />
            <asp:BoundField DataField="FullUserName" HeaderText="Usuario" />
            <asp:BoundField DataField="ActionUser" HeaderText="Acción" />
            <asp:BoundField DataField="DateRegister"
                HeaderText="Fecha y hora"
                DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
            <asp:BoundField DataField="PreviousDetail"
                HeaderText="Datos anteriores" />
            <asp:BoundField DataField="NewDetail"
                HeaderText="Datos nuevos" />
        </Columns>
    </asp:GridView>
</form>
</body>
</html>
