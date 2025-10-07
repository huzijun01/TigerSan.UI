# 1. Description:
A WPF UI library, containing many controls, panels, windows, animations, and converters.

# 2. About:
## Source Code & Example:
C#: https://pan.quark.cn/s/c1db1a17692c

## Personal Homepage:
bilibili: https://space.bilibili.com/34323512

# 3. How To Use:
Before use, you need to reference "Generic.xaml" in "App.xaml".
```xml
<ResourceDictionary Source="pack://application:,,,/TigerSan.UI;component/Generic.xaml" />
```

# 4. Classification:
## Animations:
### DoubleAnimations:
`FadeIn`:

`FadeOut`:

`Rotate`:

`Gradient`:

### ColorAnimations:
`Gradient`:

## Attributes:
### TableAttribute:

### TableHeaderAttribute:

## Behaviors:
### MouseDragBehavior:

## Controls:
### TableGrid:
#### Major Function:
Data editing. Row selection.

Change the background of the header and the item.

After modification, the item mask will automatically turn yellow.

When the data is incorrect, the item mask will automatically turn red.

#### Members:

`TableModel`:

`IsVerifyOK`:

`Refresh`:

`UpdateOldRowDatas`:

#### How to use:

1. Define "Model":
```c#
[Table(Name = "EmployeeInfo")]
public class EmployeeInfo
{
    [TableHeader(
        Title = "Id",
        IsReadOnly = true,
        IsAllowResize = false,
        TextAlignment = TextAlignment.Center)]
    public int Id { get; set; }
    ...
}
```

2. Create a List and add data:
```c#
public TableModel EmployeeTable { get; set; } = new TableModel(typeof(EmployeeInfo));

private void InitTable()
{
    // Set table:
    EmployeeTable.IsShowCheckBox = true;
    EmployeeTable._onSelectedRowDatasChanged += OnSelectedRowDatasChanged;

    // Set header:
    var headerId = EmployeeTable.GetHeaderModel(nameof(EmployeeInfo.Id));
    if (headerId == null) return;
    headerId.Converter = new Int2StringConverter();
    headerId.Background = Generic.Brand; // Set header background.
    ...

    // Set RowDatas:
    EmployeeTable._isAutoRefresh = true; // The default value is ture.
    var RowDatas = new ObservableCollection<object>
    {
        new EmployeeInfo() { Id = 1, Name = "Amy", Age = 18, Gender = false, Salary = 8000.0, JoinDate = DateTime.Now },
        ...
    };
    EmployeeTable.RowDatas = RowDatas; // The table will be refreshed once.
    EmployeeTable.RowDatas.Add(new EmployeeInfo() { Id = 7, Name = "Tom", ... }); // The table will be refreshed once.

    // Set item background:
    var count = EmployeeTable.RowDatas.Count;
    for (int iRow = 0; iRow < count; iRow++)
    {
        var item = EmployeeTable.GetItemModel(iRow, nameof(EmployeeInfo.Name));
        if (item == null)
        {
            LogHelper.Instance.IsNull(nameof(item));
            continue;
        }
        item.Background = Generic.Brand;
    }
}
```

3. Data binding:
```xml
<controls:TableGrid TableModel="{Binding EmployeeTable}"/>
```

### ImageButton:

### PixelDot:

### Loading:

### Select:

### Switch:

### ToolBarButton:

## Converters:
### Bool2ResizeModeConverter:

### Bool2StringConverter:

### Bool2VisibilityConverter:

### DateTime2StringConverter:

### Double2StringConverter:

### Int2StringConverter:

### Object2StringConverter:

### SortMode2VisibilityConverter:

## Helpers:
### GridHelper:
`SetColumnSpan`:

`SetRowColumn`:

### MsgBox:
`GetDialog`:

`ShowDialog`:

`ShowDialogAsync`:

`ShowInformation`:

`ShowSuccess`:

`ShowWarning`:

`ShowError`:

### TypeHelper:
`GetProp`:

`GetPropNames`:

`DeepCopyList`:

`DeepCopyObject`:

### SystemHelper:
`WINDOWPOS`:

`SWP_NOSIZE`:

`GetDpiScale`:

`WndProc_NoResize`:

`GetScreenPosition`:

## Models:
### DragData:
`DragEvent`:

`DragData`:

### MenuItemModel:

### TableModels:
`HandelState`:

`ItemState`:

`ItemType`:

`SortMode`:

`ItemModelBase`:

`HeaderModel`:

`ItemModel`:

`TableModel`:

`HeaderRowUIElement`:

`ItemRowUIElement`:

### Verifications:

## Panels:
### HorizontalAveragePanel:

### VerticalAveragePanel:

## Styles:
### Global:
Colors, Constant.
`TransparentUserControlStyle`:

### BorderStyle:
`CardBorderStyle`:

`SelectBorderStyle`:

### ButtonStyle:
`ButtonStyle`: Global.

`ScreenSaverButtonStyle`:

`FileSelectorButtonStyle`:

`LableButtonStyle`:

### CheckBoxStyle:
`CheckBoxStyle`: Global.

### ContextMenuStyle:
`ContextMenuStyle`: Global.

### ItemContainerStyle:
`MenuScrollButton`:

`ItemContainerStyle`:

### MenuItemStyle:
`MenuItemStyle`:

### PixelDotStyle:
`PixelDotStyle`: Global.

### RadioButtonStyle:
`RadioButtonStyle`: Global.

### ScrollViewerStyle:
`ScrollViewerStyle`:

`ScrollViewerStyle`: Global.

### SquareButtonStyle:
`SquareButtonStyle`:

### TabItemStyle:
`TabControlStyle`: Global.

`TabItemStyle`: Global.

### TextBlockStyle:
`TitleTextBlockStyle`:

`ContentTextBlockStyle`:

`PropNameTextBlockStyle`:

`PropValueTextBlockStyle`:

`UrlTextBlockStyle`:

### TextBoxStyle:
`NoBorderTextBoxStyle`:

`CustomTextBoxTemplate`:

`CustomTextBoxStyle`:

`TextBoxStyle`: Global.

## Windows:
### ByeWindow:

### CustomWindow:
`CustomWindowChrome`:

`NoResizeWindowChrome`:

`CustomWindowStyle`:

### DialogWindow:

### MenuWindow:
`MenuWindowStyle`:

### PopWindow:
`PopWindowStyle`:
# TigerSan.UI
