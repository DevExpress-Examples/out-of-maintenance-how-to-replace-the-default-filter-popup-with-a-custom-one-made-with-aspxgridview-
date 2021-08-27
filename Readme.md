<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128577759/13.1.4%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E4669)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [Default.aspx](./CS/CustomFilterPopup/Default.aspx) (VB: [Default.aspx](./VB/CustomFilterPopup/Default.aspx))
* [Default.aspx.cs](./CS/CustomFilterPopup/Default.aspx.cs) (VB: [Default.aspx.vb](./VB/CustomFilterPopup/Default.aspx.vb))
* [FilterPopup.ascx](./CS/CustomFilterPopup/FilterPopup.ascx) (VB: [FilterPopup.ascx](./VB/CustomFilterPopup/FilterPopup.ascx))
* [FilterPopup.ascx.cs](./CS/CustomFilterPopup/FilterPopup.ascx.cs) (VB: [FilterPopup.ascx.vb](./VB/CustomFilterPopup/FilterPopup.ascx.vb))
<!-- default file list end -->
# How to replace the default Filter Popup with a custom one made with ASPxGridView control
<!-- run online -->
**[[Run Online]](https://codecentral.devexpress.com/e4669/)**
<!-- run online end -->


<p>This example demonstrates how to replace the default Filter Popup with a custom one made with the ASPxGridView control. This solution demonstrates only a basic approach, and it is possible to customize it further to achieve a custom result. The whole sample functionality can be divided into three parts:</p>
<p>1. We use the <a href="http://documentation.devexpress.com/#AspNet/DevExpressWebASPxPivotGridASPxPivotGrid_FieldValueTemplatetopic">ASPxPivotGrid.FieldValueTemplate</a> property to assign a custom header template. You can use a simpler sample project demonstrating this approach in the <a href="https://www.devexpress.com/Support/Center/p/E1805">Create Header or Field Value Templates and replicate existing look-and-feel</a> example. Note that in this example we replace the default filter button with a custom one created dynamically.</p>
<br>
<p>2. To get information about the filter applied to a specific field, we use the <a href="http://documentation.devexpress.com/#CoreLibraries/DevExpressXtraPivotGridPivotGridFieldBase_FilterValuestopic">PivotGridFieldBase.FilterValues</a> and <a href="http://documentation.devexpress.com/#CoreLibraries/DevExpressXtraPivotGridPivotGridFieldBase_GetUniqueValuestopic">PivotGridFieldBase.GetUniqueValues</a> properties. To pass filter information from the server to the client, we simply convert filter values to strings. This solution can be not enough in some situations. In this case, it might be necessary to update the code accordingly.</p>
<br>
<p>3. To populate ASPxGridView with data at runtime and apply the specified filter to the ASPxPivotGrid control, we use the <a href="http://documentation.devexpress.com/#AspNet/DevExpressWebASPxPivotGridScriptsASPxClientPivotGrid_PerformCallbacktopic">ASPxClientPivotGrid.PerformCallback </a> and <a href="http://documentation.devexpress.com/#AspNet/DevExpressWebASPxGridViewScriptsASPxClientGridView_PerformCallbacktopic">ASPxClientGridView.PerformCallback</a> methods. The client-side row selection functionality is provided by the ASPxGridView's built-in <a href="http://documentation.devexpress.com/#AspNet/CustomDocument3737">Selection </a> feature.</p>
<br>
<p>To attach the custom filter popup to your ASPxPivotGrid control, add the FilterPopup to the page and specify the following properties:<br>1. The <strong>FilterPopup.PivotGridID </strong>property is used to specify the target pivot grid control. <br>2. The <strong>FilterPopup.ThemeName</strong> property should be used to specify the name of the <a href="https://documentation.devexpress.com/#AspNet/CustomDocument11685">Theme</a> that is used in your application. If the <a href="https://documentation.devexpress.com/#AspNet/CustomDocument6655">Default </a>theme is used, keep this property empty. <br><br><br><strong>See Also:</strong><br><a href="https://www.devexpress.com/Support/Center/p/T572362">T572362: How to replace the default Filter Popup with a custom one made with the MVC Grid View extension</a></p>

<br/>


