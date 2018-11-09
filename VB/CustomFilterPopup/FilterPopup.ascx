<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="FilterPopup.ascx.vb"
    Inherits="CustomFilterPopup.FilterPopup" %>
<%@ Register Assembly="DevExpress.Web.v15.2, Version=15.2.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>


<style type="text/css">
    img
    {
        border-width: 0;
    }
</style>
<script type="text/javascript">
    function ApplyFilter(values) {
        PivotGrid.PerformCallback(gvFilterItems.cpFieldName + ',' + values);

    }
    function ClosePopupWindow() {
        DrillDownWindow.HideWindow();
        gvFilterItems.PerformCallback("ClearGrid");
    }

</script>
<dx:ASPxPopupControl ID="ASPxPopupControl1" Modal="true" runat="server" Height="1px"
    AllowDragging="True" ClientInstanceName="DrillDownWindow" Left="200" Top="200"
    CloseAction="CloseButton" Width="284px" Border-BorderWidth="0">
    <Border BorderWidth="0px"></Border>
    <ContentCollection>
        <dx:PopupControlContentControl runat="server">
            <table width="100%">
                <tr>
                    <td>
                        <dx:ASPxGridView ID="ASPxGridView1" runat="server" AutoGenerateColumns="False" ClientInstanceName="gvFilterItems"
                            OnCustomCallback="ASPxGridView1_CustomCallback" KeyFieldName="FilterValue" Width="100%" OnDataBinding="ASPxGridView1_DataBinding">
                            <ClientSideEvents EndCallback="function(s, e) {
    DrillDownWindow.SetClientWindowSize(-1, 100, 100);
}" />
                            <Columns>
                                <dx:GridViewCommandColumn ShowInCustomizationForm="True" ShowSelectCheckbox="True"
                                    VisibleIndex="0">
                                </dx:GridViewCommandColumn>
                                <dx:GridViewDataTextColumn FieldName="ValueText" VisibleIndex="2">
                                </dx:GridViewDataTextColumn>
                            </Columns>
                            <SettingsBehavior AllowDragDrop="False" AllowGroup="False" 
                                AllowSelectByRowClick="True" />
                            <SettingsPager  PageSize="25">
                            </SettingsPager>
                            <Settings GridLines="None" ShowColumnHeaders="False" ShowFilterRow="True" />
                            <Styles>
                                <SelectedRow BackColor="White" Font-Bold="True" ForeColor="#003300">
                                </SelectedRow>
                            </Styles>
                        </dx:ASPxGridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <dx:ASPxButton ID="buttonSelectAll" runat="server" AutoPostBack="False" EnableClientSideAPI="True"
                                        Text="Hide All">
                                        <ClientSideEvents Click="function(s, e) {
                                            if( s.GetText() == 'Show All' )
                                            {                                                
                                                gvFilterItems.PerformCallback( 'ShowAll' );
                                                s.SetText('Hide All')
                                            }
                                            else
                                            {
                                                gvFilterItems.PerformCallback( 'HideAll' );
                                                s.SetText('Show All')
                                            }
}" />
                                    </dx:ASPxButton>
                                </td>
                                <td>
                                    <dx:ASPxButton ID="buttonFilterInver" runat="server" AutoPostBack="False" EnableClientSideAPI="True"
                                        Text="Invert">
                                        <ClientSideEvents Click="function(s, e) {	
    gvFilterItems.PerformCallback( 'InvertFilter' );
}" />
                                    </dx:ASPxButton>
                                </td>
                                <td>
                                    <dx:ASPxButton ID="buttonFilterOk" runat="server" AutoPostBack="False" EnableClientSideAPI="True"
                                        Text="Ok">
                                        <ClientSideEvents Click="function(s, e) {
    gvFilterItems.GetSelectedFieldValues(&quot;FilterValue&quot;, ApplyFilter);
    ClosePopupWindow();
}" />
                                    </dx:ASPxButton>
                                </td>
                                <td>
                                    <dx:ASPxButton ID="buttonFilterCancel" runat="server" AutoPostBack="False" EnableClientSideAPI="True"
                                        Text="Cancel">
                                        <ClientSideEvents Click="function(s, e) {

    ClosePopupWindow();
}" />
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>