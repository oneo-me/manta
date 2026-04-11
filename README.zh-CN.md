# Manta

中文 | [English](README.md)

基于 Avalonia 构建的跨平台桌面应用 UI 工具包和 MVVM 框架。

## 特性

- 60+ 可主题化的自定义控件，支持平滑动画
- 亮色/暗色主题支持，采用 OKLch 色彩空间
- 响应式 MVVM 基础设施，支持基于特性的视图注册
- 通过 `RegionManager` 实现标签页/区域导航
- 内置模态对话框系统（`DialogModel`）
- 多语言支持，编译时代码生成
- `SmoothBrushTransition` 实现正确的 OKLch 画笔插值

## 系统要求

- .NET 10.0
- Avalonia 11.3.10

## 控件

### 布局与容器

| 控件 | 说明 |
|------|------|
| MWindow | 自定义窗口，集成标题栏和主题支持 |
| MPanel | 灵活的布局面板（Vertical/Horizontal/Wrap/Column/Waterfall） |
| MCard | 带边框、阴影和圆角的容器 |
| MExpander | 可展开容器，带动画内容展示 |
| MAnimatedExpander | 高度动画容器，用于平滑展开/折叠 |
| MAccordion | 手风琴控件，支持多个可折叠分区 |
| MItems | 项目控件，支持自定义项目模板 |
| MView | MVVM 模式的视图容器 |
| MViewHost | 动态视图切换的宿主控件 |
| MClip | 支持裁剪的内容控件 |
| MBackground | 对话框和弹窗的环境背景容器 |
| MDialogTemplate | 统一的对话框布局（标题 + 取消/确认按钮） |

### 输入

| 控件 | 说明 |
|------|------|
| MButton | 按钮，支持多种变体（Primary、Secondary、Outline、Ghost、Flat、Destructive） |
| MToggleButton | 切换按钮，支持 Outline 和 WithText 变体 |
| MToggleSwitch | 切换开关控件 |
| MCheckBox | 复选框，支持 Default 和 Card 变体，可自定义标题 |
| MRadioButton | 单选按钮，支持 Default 和 Bordered 变体 |
| MTextBox | 文本输入框，带动画光标和输入法支持 |
| MMaskedTextBox | 支持输入掩码的文本输入框 |
| MAutoCompleteBox | 带自动补全下拉列表的文本输入框 |
| MNumericUpDown | 带增减按钮的数字输入框 |
| MComboBox | 下拉选择控件 |
| MSlider | 水平和垂直滑块 |
| MCommandPanel | 命令面板，用于可搜索的操作菜单 |
| MDropDownButton | 带下拉菜单的按钮 |
| MSplitButton | 带分离主操作和下拉菜单的按钮 |
| MToggleSplitButton | 切换式分离按钮 |
| MRepeatButton | 按住时持续触发的按钮 |
| MButtonSpinner | 带增减旋钮的输入控件 |
| MFlyout | 弹出式容器 |

### 展示

| 控件 | 说明 |
|------|------|
| MIcon | 图标显示，支持多种尺寸和样式变体 |
| MPathIcon | 基于矢量路径的图标 |
| MImage | 图片显示控件 |
| MAvatar | 用户头像，支持图片或首字母 |
| MBadge | 状态徽章，支持多种颜色变体 |
| MBreadcrumb | 导航面包屑 |
| MAlert | 警告/通知，支持多种严重级别 |
| MProgressRing | 圆形进度指示器 |
| MProgressBar | 水平进度条 |
| MRingShape | 装饰性圆环/弧形 |
| MSeparator | 水平或垂直分隔线 |
| MHyperlinkButton | 超链接样式按钮 |

### 数据与导航

| 控件 | 说明 |
|------|------|
| MListBox | 列表，支持选择和虚拟化 |
| MTree | 层级树形视图 |
| MCarousel | 轮播，用于循环展示内容 |
| MTabStrip | 标签页导航条 |
| MTabControl | 带内容面板的完整标签页控件 |
| MTabTitleBar | 浏览器风格的标签页标题栏 |
| MMenu | 应用菜单栏 |

### 特殊控件

| 控件 | 说明 |
|------|------|
| MTitleBar | 自定义窗口标题栏，支持拖拽 |
| MScrollViewer | 滚动视图，带样式化滚动条 |
| MAspectRatio | 保持宽高比的容器 |
| MPixelGrid | 像素对齐的网格显示 |
| MBackdropBlur | 背景模糊效果容器 |
| MCanvasGrid | 带网格叠加的无限平移/缩放画布 |
| MToolTip | 样式化工具提示 |

### 布局面板

`MPanel` 通过单一的 `Mode` 属性统一管理所有布局模式：

| 模式 | 说明 |
|------|------|
| `Vertical` | 垂直堆叠 |
| `Horizontal` | 水平堆叠 |
| `Wrap` | 自动换行流式布局 |
| `WrapFull` | 自动换行并拉伸到全宽 |
| `Column` | 固定列网格 |
| `Waterfall` | 瀑布流/砌体布局 |
| `WaterfallBest` | 自动均衡的瀑布流 |
| `VerticalUniform` | 等高垂直堆叠 |
| `HorizontalUniform` | 等宽水平堆叠 |

## 主题

### 色彩系统

| 特性 | 说明 |
|------|------|
| OKLch 色彩空间 | 感知均匀的颜色插值 |
| 亮色/暗色主题 | 完整的亮色和暗色主题支持 |
| 动态资源 | 通过 `MApp.SwitchTheme()` 运行时切换主题 |
| SmoothBrushTransition | 状态间的 OKLch 画笔插值 |
| OnTheme | 按主题变体设置条件值 |

### 设计令牌

| 令牌类 | 说明 |
|--------|------|
| `MBrush` | 语义色彩令牌（30+ 具名键） |
| `MThickness` | 间距刻度 — 4px 基准（S0–S96） |
| `MRadius` | 圆角刻度（None → Full） |
| `MShadow` | 阴影深度刻度（None → XL2） |
| `MSize` | 尺寸刻度 |
| `MRing` | 聚焦环样式 |

主要 `MBrush` 令牌：`Background` · `Foreground` · `Card` · `Primary` · `Secondary` · `Muted` ·
`Accent` · `Destructive` · `Border` · `Input` · `Ring` · `Sidebar*` · `Chart1`–`Chart5`

## MVVM 框架

### 类层级

```
Model
  └── CommandModel
      └── ViewModel
          ├── RegionModel      ← 标签页/区域视图模型
          └── WindowModel
              ├── DialogModel  ← 带 Result 的模态对话框
              └── TrayWindowModel
```

### 核心组件

| 组件 | 说明 |
|------|------|
| `ViewModel` | 带响应式属性和生命周期的基类 |
| `WindowModel` | 顶层窗口视图模型 |
| `DialogModel` | 模态对话框，内置取消/确认命令 |
| `TrayWindowModel` | 系统托盘弹窗（失去焦点时自动关闭） |
| `RegionModel` | 支持 `Close()` 的标签页/区域视图模型 |
| `RegionManager<T>` | 带可观察选择的标签页列表 |
| `ViewManager` | 单槽视图切换器 |
| `ViewTemplate<T>` | 从数据对象创建视图的 `IDataTemplate` |
| `ViewContext<T>` | 通过视觉树的父子上下文传递 |
| `@ViewAttribute` | 基于特性的 ViewModel ↔ View 绑定 |
| `CommandWarper` | 带 `IsExecuting` 和 `OnChanged()` 的缓存命令 |

### 服务定位器

```csharp
// 注册
Service.Register(new MyService());

// 使用（字段初始化器）
readonly MyService _svc = Service.GetService<MyService>();
```

### 应用入口

```csharp
sealed class App : MApp
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    protected override void OnInitialize(out MAppOptions options)
    {
        Service.Register(new ConfigService());

        options = new()
        {
            MainModel = () => new MainView_Model(),
            Exit = () => configService.Save()
        };
    }
}
```

## 国际化

| 特性 | 说明 |
|------|------|
| YAML 语言文件 | 扁平键值翻译文件 |
| 源代码生成器 | 编译时 `LangKey` 常量 |
| `{Lang}` 扩展 | 响应式 XAML 翻译绑定 |
| `{LangEmoji}` | `LangType` 值对应的国旗 Emoji |
| `{LangName}` | 语言本地名称 |
| 40+ `LangType` 枚举 | 内置语言代码 |

## 平台 API

`ViewModel` / `WindowModel` 上的扩展方法 — 线程安全，零样板代码：

| 方法 | 说明 |
|------|------|
| `OpenFilePickerAsync` | 打开文件选择对话框 |
| `OpenFolderPickerAsync` | 打开文件夹选择对话框 |
| `SaveFilePickerAsync` | 打开保存文件对话框 |
| `GetClipboardAsync` | 获取窗口剪贴板 |

## 许可证

MIT
