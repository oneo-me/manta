# Manta

中文 | [English](README.md)

基于 Avalonia 构建的跨平台桌面应用 UI 工具包和 MVVM 框架。

## 特性

- 40+ 可主题化的自定义控件，支持平滑动画
- 亮色/暗色主题支持，采用 OKLch 色彩空间
- 响应式 MVVM 基础设施，支持基于特性的视图注册
- 多语言支持，编译时代码生成
- 平滑的画笔过渡，支持预乘 Alpha 混合

## 系统要求

- .NET 10.0
- Avalonia 11.3.10

## 控件

### 布局与容器

| 控件 | 说明 |
|------|------|
| MWindow | 自定义窗口，集成标题栏和主题支持 |
| MPanel | 灵活的布局面板，支持水平/垂直模式 |
| MCard | 带边框、阴影和圆角的容器 |
| MExpander | 可展开容器，带动画内容展示 |
| MAnimatedExpander | 高度动画容器，用于平滑展开/折叠 |
| MItems | 项目控件，支持自定义项目模板 |
| MView | MVVM 模式的视图容器 |
| MViewHost | 动态视图切换的宿主控件 |
| MClipContentControl | 支持裁剪的内容控件 |

### 输入

| 控件 | 说明 |
|------|------|
| MButton | 按钮，支持多种变体（Primary、Secondary、Outline、Ghost、Link、Destructive） |
| MToggleButton | 切换按钮，支持 Outline 和 WithText 变体 |
| MCheckBox | 复选框，支持 Default 和 Bordered 变体，可自定义标题 |
| MTextBox | 文本输入框，带动画光标和输入法支持 |
| MComboBox | 下拉选择控件 |
| MCommand | 命令面板，用于可搜索的操作菜单 |

### 展示

| 控件 | 说明 |
|------|------|
| MIcon | 图标显示，支持多种尺寸 |
| MPathIcon | 基于矢量路径的图标 |
| MAvatar | 用户头像，支持图片或首字母 |
| MBadge | 状态徽章，支持多种颜色变体 |
| MBreadcrumb | 导航面包屑 |
| MAlert | 警告消息，支持多种严重级别 |
| MProgressRing | 圆形进度指示器 |
| MRing | 装饰性圆环元素 |

### 数据

| 控件 | 说明 |
|------|------|
| MListBox | 列表，支持选择和虚拟化 |
| MChart | 数据可视化图表 |
| MCarousel | 轮播，用于循环展示内容 |
| MTabStrip | 标签页导航条 |
| MTabTitleBar | 浏览器风格的标签页标题栏 |

### 特殊控件

| 控件 | 说明 |
|------|------|
| MTitleBar | 自定义窗口标题栏，支持拖拽 |
| MScrollViewer | 滚动视图，带样式化滚动条 |
| MAspectRatio | 保持宽高比的容器 |
| MPixelGrid | 像素对齐的网格显示 |
| MBackgroundBlur | 背景模糊效果容器 |

### 布局面板

| 面板 | 说明 |
|------|------|
| MWaterfallPanel | 瀑布流/砌体布局 |
| MWrapPanel | 自动换行流式布局 |
| MAutoGrid | 自动调整大小的网格 |
| MUniformGrid | 均匀单元格网格 |

## 主题

### 色彩系统

| 特性 | 说明 |
|------|------|
| OKLch 色彩空间 | 感知均匀的颜色插值 |
| 主题变体 | 亮色和暗色主题支持 |
| 动态资源 | 运行时主题切换 |
| SmoothBrushTransition | 状态间的动画颜色过渡 |

### 主题资源

| 资源 | 说明 |
|------|------|
| Primary | 主品牌色 |
| Secondary | 次要强调色 |
| Background | 背景色 |
| Foreground | 文本和图标颜色 |
| BorderBrush | 边框颜色 |
| Destructive | 错误/危险颜色 |

## MVVM 框架

### 核心组件

| 组件 | 说明 |
|------|------|
| ViewModel | 基类，支持响应式属性 |
| WindowModel | 窗口专用的视图模型基类 |
| ViewService | 视图生命周期和导航管理 |
| @ViewAttribute | 基于特性的视图注册 |

### 服务

| 特性 | 说明 |
|------|------|
| Service Locator | 依赖注册和解析 |
| Reactive Commands | 命令基础设施，支持异步 |
| Property Change | 自动变更通知 |

## 国际化

| 特性 | 说明 |
|------|------|
| YAML 语言文件 | 人类可读的翻译文件 |
| 源代码生成器 | 编译时语言键生成 |
| 标记扩展 | 用于翻译的 XAML 绑定 |

## 动画

| 特性 | 说明 |
|------|------|
| SmoothBrushTransition | 画笔插值，支持预乘 Alpha |
| MAnimatedExpander | 自动高度内容的高度动画 |
| Animated Cursor | MTextBox 中的平滑文本光标移动 |

## 许可证

MIT
