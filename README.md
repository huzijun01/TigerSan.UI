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
### BrushAnimations:

### DoubleAnimations:

### ColorAnimations:

## Attributes:
### TableAttribute:

### TableHeaderAttribute:

## Behaviors:
### DynamicGridBehavior:

### MouseDragBehavior:

## Controls:
### FilePicker:

### NavBar:
#### NavBar:
#### NavButton:
#### NavFolder:

### NumBox:

### PageView:
#### PageBar:
#### PageButton:
#### PageView:

### Pagination:
#### Pagination:
#### PaginationButton:

### ImageButton:

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

### BindingHelper:

### DragHelper:

### GridHelper:

### MsgBox:

### NetworkHelper:

### StartupOnce:
`StartupCheck`:

If the program has already started, it will exit.

### SystemHelper:

### TypeHelper:

### UpdateHelper:

### WindowHelper:

## Models:
### NavBar:
#### NavBarModel:
#### NavButtonModel:
#### NavFolderModel:

### DragData:

### MenuItemModel:

### TableModels:

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
Colors, Constants, Icons.

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
`CustomTextBoxStyle`:

`NoBorderTextBoxStyle`:

`EllipsisTextBoxStyle`:

`TextBoxStyle`: Global.

### TransparentUserControlStyle:
`TransparentUserControlStyle`:

## Templates:
### CircleButtonTemplate:
`CircleButtonTemplate`:

### TextBoxTemplate:
`CustomTextBoxTemplate`:

`VerticalAlignmentTextBoxTemplate`:

`EllipsisTextBoxTemplate`:

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

### DragWindow:

### MenuWindow:
`MenuWindowStyle`:

### PopWindow:
`PopWindowStyle`:

### UpdateWindow:
