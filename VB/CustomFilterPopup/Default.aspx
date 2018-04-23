<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="CustomFilterPopup.WebForm1" %>

<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v15.2, Version=15.2.13.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v15.2, Version=15.2.13.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>


<%@ Register src="FilterPopup.ascx" tagname="FilterPopup" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

</head>
<body>
    <form id="form1" runat="server">
    <div>

        <dx:ASPxPivotGrid ID="ASPxPivotGrid1" runat="server"  DataSourceID="NwindDataSource"
            Width="800px" ClientInstanceName="PivotGrid">
            <Fields>
                <dx:PivotGridField Area="RowArea" AreaIndex="0" Caption="Customer" FieldName="CompanyName"
                    ID="fieldCompanyName">
                </dx:PivotGridField>
                <dx:PivotGridField Area="ColumnArea" AreaIndex="0" Caption="Year" FieldName="OrderDate"
                    ID="fieldOrderDate" GroupInterval="DateYear" UnboundFieldName="UnboundColumn1">
                </dx:PivotGridField>
                <dx:PivotGridField Area="DataArea" AreaIndex="0" Caption="Product Amount" FieldName="ProductAmount"
                    ID="fieldProductAmount">
                </dx:PivotGridField>
                <dx:PivotGridField Area="RowArea" AreaIndex="1" Caption="Products" FieldName="ProductName"
                    ID="fieldProductName">
                </dx:PivotGridField>
            </Fields>
            <OptionsView ShowFilterHeaders="False" ShowHorizontalScrollBar="True" />
        </dx:ASPxPivotGrid>
        <asp:AccessDataSource ID="NwindDataSource" runat="server" DataFile="~/App_Data/nwind.mdb"
            SelectCommand="SELECT * FROM [CustomerReports]"></asp:AccessDataSource>
        <uc1:FilterPopup ID="FilterPopup1" runat="server" PivotGridID="ASPxPivotGrid1" 
            ThemeName="Aqua"  />
    </div>
    </form>
</body>
</html>