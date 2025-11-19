# 1. Description:
A WPF UI library, containing many controls, panels, windows, animations, and converters.

# 2. About:
## Source Code & Example:
GitHub: https://github.com/huzijun01/TigerSan.UI

## Personal Homepage:
bilibili: https://space.bilibili.com/34323512

# 3. How To Use:
Before use, you need to reference "Generic.xaml" in "App.xaml".
```xml
<ResourceDictionary Source="/TigerSan.UI;component/Generic.xaml"/>
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
### FilePicker:

### ImageButton:

### NumBox:

### TableGrid:
#### Major Function:
Data editing.

Row selection.

Candidate value menu.

Change the background of the header and the item.

After modification, the item mask will automatically turn yellow.

When the data is incorrect, the item mask will automatically turn red.

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
### AboutHelper:
`_instance`:

Can be used as global data.

`ShowAboutView`:

`ShowUpdateView`:

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

### NetworkHelper:
`IsNetworkAvailable`:

`GetAsync`:

`IsOK`:

### StartupOnce:
`StartupCheck`:

If the program has already started, it will exit.

### SystemHelper:
`WINDOWPOS`:

`SWP_NOSIZE`:

`GetDpiScale`:

`WndProc_NoResize`:

`GetScreenPosition`:

### TypeHelper:
`GetProp`:

`GetPropNames`:

`DeepCopyList`:

`DeepCopyObject`:

### UpdateHelper:
`_timerUpdate`:

`CheckUpdateAsync`:

`Compare`:

### WindowHelper:
`GetIcon`:

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

### VersionModels:
#### UpdateInfo:

#### ComparisonResults:

#### VersionModel:

## Panels:
### HorizontalAveragePanel:

### VerticalAveragePanel:

## Styles:
### Global:
Colors, Constant.

### BorderStyle:
`CardBorderStyle`:

`SelectBorderStyle`:

### ButtonStyle:
`ButtonTemplate`:

`ButtonStyle`: Global.

`ScreenSaverButtonStyle`:

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

`VerticalAlignmentTextBoxTemplate`:

`TextBoxStyle`: Global.

### TransparentUserControlStyle:
`TransparentUserControlStyle`:

## ViewModels:
### AboutViewModel:

### UpdateViewModel:

## Windows:
### AboutWindow:

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

### UpdateWindow:
