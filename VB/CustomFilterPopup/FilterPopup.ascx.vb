Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DevExpress.Web.ASPxPivotGrid
Imports DevExpress.Web
Imports System.Threading

Namespace CustomFilterPopup
    Partial Public Class FilterPopup
        Inherits System.Web.UI.UserControl


        Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
            If Not String.IsNullOrEmpty(Me.PivotGridID) Then
                Dim pivot As ASPxPivotGrid = TryCast(Me.Parent.FindControl(PivotGridID), ASPxPivotGrid)
                If pivot IsNot Nothing Then
                    Me.PivotGrid = pivot
                End If
            End If
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
            If Not IsPostBack Then
                ASPxGridView1.DataBind()
            End If
        End Sub

        Private ReadOnly Property CurrentField() As PivotGridField
            Get
                Return CType(PivotGrid.Fields.GetFieldByName(DirectCast(Session("CurrentField"), String)), PivotGridField)
            End Get
        End Property
        Private Sub PivotGrid_CustomCallback(ByVal sender As Object, ByVal e As PivotGridCustomCallbackEventArgs)
            Dim parameters() As String = e.Parameters.Split(",".ToCharArray())
            Dim field = PivotGrid.Fields.GetFieldByName(parameters(0))
            Dim selectedValueStr = parameters.Skip(1).OrderBy(Function(v) v).ToArray()
            Dim uniqueValues() As Object = field.GetUniqueValues()
            Dim valuesExcluded() As Object = uniqueValues.Where(Function(v) (Not ContainItem(selectedValueStr, Convert.ToString(v)))).ToArray()
            field.FilterValues.ValuesExcluded = valuesExcluded
        End Sub
        Protected Sub ASPxGridView1_CustomCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs)
            Select Case e.Parameters
                Case "ClearGrid"
                    Session("CurrentField") = String.Empty
                    ASPxGridView1.DataBind()
                Case "InvertFilter"
                    Dim selectedValues As List(Of Object) = ASPxGridView1.GetSelectedFieldValues(New String() { "FilterValue" })
                    ASPxGridView1.Selection.SelectAll()
                    For Each val As Object In selectedValues
                        ASPxGridView1.Selection.UnselectRowByKey(val)
                    Next val
                Case "ShowAll"
                    ASPxGridView1.Selection.SelectAll()
                Case "HideAll"
                    ASPxGridView1.Selection.UnselectAll()
                Case Else
                    Session("CurrentField") = e.Parameters
                    ASPxGridView1.DataBind()
                    ASPxGridView1.PageIndex = 0
                    ASPxGridView1.Selection.UnselectAll()
                    ASPxGridView1.FilterExpression = String.Empty
                    ASPxGridView1.JSProperties.Add("cpFieldName", e.Parameters)
                    Dim valuesIncludedStr() As Object = CurrentField.FilterValues.ValuesIncluded
                    For i As Integer = 0 To ASPxGridView1.VisibleRowCount - 1
                        Dim fi As FilterInfo = CType(ASPxGridView1.GetRow(i), FilterInfo)
                        ASPxGridView1.Selection.SetSelection(i, ContainItem(valuesIncludedStr, fi.FilterValue))
                    Next i
            End Select
        End Sub

        Private Function ContainItem(ByVal values() As Object, ByVal target As Object) As Boolean
            Dim left As Integer = 0
            Dim right As Integer = values.Length -1
            Do While left <= right
                Dim middle As Integer = (left + right) \ 2
                Dim result As Integer = DirectCast(target, IComparable).CompareTo(DirectCast(values(middle), IComparable))
                If result = 0 Then
                    Return True
                Else
                    If result < 0 Then
                        right = middle - 1
                    Else
                        left = middle + 1
                    End If
                End If

            Loop
            Return False
        End Function

        Protected Sub ASPxGridView1_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
            If CurrentField Is Nothing Then
                ASPxGridView1.DataSource = Nothing
            Else
                Dim list = From v In CurrentField.GetUniqueValues() _
                           Select New FilterInfo() With {.FilterValue = v, .ValueText = CurrentField.GetDisplayText(v)}
                ASPxGridView1.DataSource = list.ToArray()
            End If
        End Sub

        Public Class FilterInfo
            Public Property FilterValue() As Object
            Public Property ValueText() As String
        End Class



        Private pivotGrid_fld As ASPxPivotGrid
        Public Property PivotGrid() As ASPxPivotGrid
            Get
                If pivotGrid_fld Is Nothing Then
                    Throw New Exception("FilterPopup.PivotGrid property is not specified. It is necessary to set this property on each Page_Load")
                End If
                Return pivotGrid_fld
            End Get
            Set(ByVal value As ASPxPivotGrid)
                pivotGrid_fld = value
                'BindGridView(CurrentField);
                PivotGrid.HeaderTemplate = New HeaderTemplate(ThemeName)
                AddHandler PivotGrid.CustomCallback, AddressOf PivotGrid_CustomCallback
            End Set
        End Property
        Public Property PivotGridID() As String
        Private themeName_Field As String = String.Empty
        Public Property ThemeName() As String
            Get
                If PivotGrid IsNot Nothing AndAlso (Not String.IsNullOrEmpty(PivotGrid.Theme)) Then
                    Return PivotGrid.Theme
                End If
                Return themeName_Field
            End Get
            Set(ByVal value As String)
                themeName_Field = value
            End Set
        End Property
    End Class

    Public Class HeaderTemplate
        Implements ITemplate

        Private themeName As String
        Public Sub New(ByVal themeName As String)
            Me.themeName = themeName
        End Sub

        Private Function FilterButtonOnClick(ByVal c As PivotGridHeaderTemplateContainer) As String
            Return String.Format("            " & ControlChars.CrLf & _
"            {{" & ControlChars.CrLf & _
"                var rects = this.getClientRects();" & ControlChars.CrLf & _
"                gvFilterItems.PerformCallback( '{0}' );                " & ControlChars.CrLf & _
"                DrillDownWindow.ShowAtPos(rects[0].left, rects[0].bottom);" & ControlChars.CrLf & _
"                DrillDownWindow.SetHeaderText( '{1}' );               " & ControlChars.CrLf & _
"            }}", c.Field.ID, c.Field.Caption & " Filter")
        End Function
        Public Sub InstantiateIn(ByVal container As Control) Implements ITemplate.InstantiateIn
            Dim c As PivotGridHeaderTemplateContainer = CType(container, PivotGridHeaderTemplateContainer)
            Dim fieldHeaderTable As PivotGridHeaderHtmlTable = c.CreateHeader()
            If c.Field.Visible AndAlso c.Field.Area <> DevExpress.XtraPivotGrid.PivotArea.DataArea AndAlso c.Field.Options.AllowFilter <> DevExpress.Utils.DefaultBoolean.False Then

                Dim myFilterButton = New System.Web.UI.HtmlControls.HtmlGenericControl("div")
                myFilterButton.Attributes("OnClick") = FilterButtonOnClick(c)

                Dim themeSufix As String = If(String.IsNullOrEmpty(themeName), String.Empty, "_" & themeName)
                Dim cssClassFS As String = If(c.Field.FilterValues.HasFilter, "dxPivotGrid_pgFilterButtonActive{0}", "dxPivotGrid_pgFilterButton{0}")
                myFilterButton.Attributes("class") = String.Format(cssClassFS, themeSufix)

                Dim filterButtonCell As New TableCell()
                filterButtonCell.Width = Unit.Pixel(1)
                filterButtonCell.Controls.Add(myFilterButton)
                Dim defaultFilterCell As TableCell = fieldHeaderTable.Rows(0).Cells(fieldHeaderTable.Rows(0).Cells.Count - 1)
                fieldHeaderTable.Rows(0).Cells.Remove(defaultFilterCell)
                fieldHeaderTable.Rows(0).Cells.Add(filterButtonCell)
            End If
            c.Controls.Add(fieldHeaderTable)
        End Sub


    End Class
End Namespace