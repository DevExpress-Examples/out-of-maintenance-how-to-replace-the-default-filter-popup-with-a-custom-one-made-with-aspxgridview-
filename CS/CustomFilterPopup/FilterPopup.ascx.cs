using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxPivotGrid;
using DevExpress.Web;
using System.Threading;

namespace CustomFilterPopup {
    public partial class FilterPopup : System.Web.UI.UserControl {


        protected void Page_Init(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.PivotGridID))
            {
                ASPxPivotGrid pivot = this.Parent.FindControl(PivotGridID) as ASPxPivotGrid;
                if (pivot != null)
                    this.PivotGrid = pivot;
            }
        }

        protected void Page_Load(object sender, EventArgs e) {
            if(!IsPostBack)
                ASPxGridView1.DataBind();
        }

        PivotGridField CurrentField {
            get { 
                return (PivotGridField)PivotGrid.Fields.GetFieldByName((string)Session["CurrentField"]); 
            }           
        }
        void PivotGrid_CustomCallback(object sender, PivotGridCustomCallbackEventArgs e) {
            string[] parameters = e.Parameters.Split(",".ToCharArray());
            var field = PivotGrid.Fields.GetFieldByName(parameters[0]);
            var selectedValueStr = parameters.Skip(1).OrderBy(v=>v).ToArray();
            object[] uniqueValues = field.GetUniqueValues();
            object[] valuesExcluded = uniqueValues.Where(v => !ContainItem(selectedValueStr, Convert.ToString(v))).ToArray();           
            field.FilterValues.ValuesExcluded = valuesExcluded;
        }
        protected void ASPxGridView1_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e) {
            switch (e.Parameters)
            {
                case "ClearGrid" :
                    Session["CurrentField"] = string.Empty;
                    ASPxGridView1.DataBind();
                    break;
                case "InvertFilter":
                    List<object> selectedValues = ASPxGridView1.GetSelectedFieldValues(new string[] { "FilterValue" });
                    ASPxGridView1.Selection.SelectAll();
                    foreach(object val in selectedValues)
                        ASPxGridView1.Selection.UnselectRowByKey(val);
                    break;            
                case "ShowAll":
                    ASPxGridView1.Selection.SelectAll();
                    break;
                case "HideAll":
                    ASPxGridView1.Selection.UnselectAll();
                    break;
                default:
                    Session["CurrentField"] = e.Parameters;
                    ASPxGridView1.DataBind();
                    ASPxGridView1.PageIndex = 0;
                    ASPxGridView1.Selection.UnselectAll();
                    ASPxGridView1.FilterExpression = string.Empty;
                    ASPxGridView1.JSProperties.Add("cpFieldName", e.Parameters);
                    object[] valuesIncludedStr = CurrentField.FilterValues.ValuesIncluded;
                    for (int i = 0; i < ASPxGridView1.VisibleRowCount; i++) {
                        FilterInfo fi = (FilterInfo)ASPxGridView1.GetRow(i);
                        ASPxGridView1.Selection.SetSelection(i, ContainItem(valuesIncludedStr, fi.FilterValue));
                    }
                    break;
            }
        }

        bool ContainItem(object[] values, object target) {
            int left = 0;
            int right = values.Length -1;
            while (left <= right) 
            {
                int middle = (left + right ) / 2;
                int result = ((IComparable)target).CompareTo((IComparable)values[middle]);
                if (result == 0)
                    return true;
                else {
                    if (result < 0)
                        right = middle - 1;
                    else
                        left = middle + 1;
                }
                
            }
            return false;
        }

        protected void ASPxGridView1_DataBinding(object sender, EventArgs e) {
            if (CurrentField == null)
                ASPxGridView1.DataSource = null;
            else {
                var list = from v in CurrentField.GetUniqueValues()
                           select new FilterInfo() { FilterValue = v, ValueText = CurrentField.GetDisplayText(v) };
                ASPxGridView1.DataSource = list.ToArray();
            }
        }

        public class FilterInfo {
            public object FilterValue { get; set; }
            public string ValueText { get; set; }
        }

       

        private ASPxPivotGrid pivotGrid_fld;
        public ASPxPivotGrid PivotGrid {
            get {
                if(pivotGrid_fld == null)
                    throw new Exception("FilterPopup.PivotGrid property is not specified. It is necessary to set this property on each Page_Load");
                return pivotGrid_fld;
            }
            set {
                pivotGrid_fld = value;
                //BindGridView(CurrentField);
                PivotGrid.HeaderTemplate = new HeaderTemplate(ThemeName);
                PivotGrid.CustomCallback += new PivotCustomCallbackEventHandler(PivotGrid_CustomCallback);
            }
        }
        public string PivotGridID { set; get; }
        private string themeName_Field = string.Empty;
        public string ThemeName
        {
            get
            {
                if (PivotGrid != null && !String.IsNullOrEmpty(PivotGrid.Theme))
                    return PivotGrid.Theme;
                return themeName_Field;
            }
            set { themeName_Field = value; }
        }    
    }

    public class HeaderTemplate : ITemplate
    {   
        private string themeName;
        public HeaderTemplate( string themeName)
        {     
            this.themeName = themeName;
        }

        private string FilterButtonOnClick(PivotGridHeaderTemplateContainer c)
        {
            return string.Format(@"            
            {{
                var rects = this.getClientRects();
                gvFilterItems.PerformCallback( '{0}' );                
                DrillDownWindow.ShowAtPos(rects[0].left, rects[0].bottom);
                DrillDownWindow.SetHeaderText( '{1}' );               
            }}",  c.Field.ID, c.Field.Caption + " Filter");
        }
        public void InstantiateIn(Control container)
        {
            PivotGridHeaderTemplateContainer c = (PivotGridHeaderTemplateContainer)container;
            PivotGridHeaderHtmlTable fieldHeaderTable = c.CreateHeader();
            if ( c.Field.Visible  && c.Field.Area != DevExpress.XtraPivotGrid.PivotArea.DataArea && c.Field.Options.AllowFilter != DevExpress.Utils.DefaultBoolean.False)
            {

                var myFilterButton = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                myFilterButton.Attributes["OnClick"] = FilterButtonOnClick(c);

                string themeSufix = String.IsNullOrEmpty(themeName) ? string.Empty : "_" + themeName;
                string cssClassFS = c.Field.FilterValues.HasFilter ? "dxPivotGrid_pgFilterButtonActive{0}" : "dxPivotGrid_pgFilterButton{0}";
                myFilterButton.Attributes["class"] = String.Format(cssClassFS, themeSufix);

                TableCell filterButtonCell = new TableCell();
                filterButtonCell.Width = Unit.Pixel(1);
                filterButtonCell.Controls.Add(myFilterButton);
                TableCell defaultFilterCell = fieldHeaderTable.Rows[0].Cells[fieldHeaderTable.Rows[0].Cells.Count - 1];
                fieldHeaderTable.Rows[0].Cells.Remove(defaultFilterCell);
                fieldHeaderTable.Rows[0].Cells.Add(filterButtonCell);
            }
            c.Controls.Add(fieldHeaderTable);
        }


    }
}