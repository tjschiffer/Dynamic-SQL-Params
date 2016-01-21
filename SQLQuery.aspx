<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SQLQuery.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html>
<script runat="server">
    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        sqlTable(Table1, Request.QueryString)
    End Sub
</script>
<html>
<head id="Head1" runat="server">
    <script src="assets/js/jquery.min.js" type="text/javascript"></script>
    <script src="assets/js/fixed-header.js" type="text/javascript"></script>
    <title>FL Insurance</title>
    <link href="assets/css/main.css" type="text/css" rel="stylesheet" />
    <style type="text/css">
        body 
        {
            font-size:15px;
            font-size:1.05vw;
        }
    </style>
</head>
<body>
<div class="mainTable">
    <asp:table ID="Table1" runat="server" CssClass="myTable"/>
</div>
</body>
</html>