﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CrystalReport.aspx.cs" Inherits="MYLAB_Report.CrystalReport" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
          <asp:Label ID="Label" runat="server" Text="Label"></asp:Label>
    </form>
</body>
</html>
