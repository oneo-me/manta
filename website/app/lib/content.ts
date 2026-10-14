import {
  Blocks,
  Languages,
  Layers,
  MoonStar,
  Palette,
  PanelsTopLeft,
  WandSparkles,
  type LucideIcon,
} from "lucide-react"

import { site, type Locale } from "~/lib/site"

export interface NavItem {
  label: string
  href: string
}

export interface Feature {
  icon: LucideIcon
  title: string
  description: string
}

export interface CodeSample {
  name: string
  code: string
}

export interface FaqItem {
  question: string
  answer: string
}

export interface UsageOption {
  icon: LucideIcon
  title: string
  description: string
  code: string
}

export interface FooterGroup {
  title: string
  links: NavItem[]
}

export interface LandingContent {
  meta: { title: string; description: string }
  nav: NavItem[]
  header: { language: string; theme: string; github: string }
  hero: {
    badge: string
    title: string
    accent: string
    description: string
    primary: string
    secondary: string
    hint: string
  }
  features: { title: string; description: string; items: Feature[] }
  usage: {
    title: string
    description: string
    items: UsageOption[]
    samples: NavItem
  }
  code: {
    title: string
    description: string
    copy: string
    copied: string
    samples: CodeSample[]
  }
  theming: {
    title: string
    description: string
    bullets: { title: string; description: string }[]
  }
  install: {
    title: string
    description: string
    steps: { label: string; code: string }[]
    requirements: string
  }
  faq: { title: string; description: string; items: FaqItem[] }
  cta: {
    title: string
    description: string
    primary: string
    secondary: string
  }
  footer: { description: string; groups: FooterGroup[]; builtWith: string }
}

const samples: CodeSample[] = [
  {
    name: "Program.cs",
    code: `using Avalonia;
using Manta;

static class Program
{
    [STAThread]
    public static void Main(string[] args)
        => MApp.Build<App>()
            .StartWithClassicDesktopLifetime(args);
}`,
  },
  {
    name: "App.axaml",
    code: `<MApp xmlns="https://github.com/avaloniaui"
      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
      x:Class="MyApp.App">
    <MApp.Styles>
        <MantaTheme />
    </MApp.Styles>
</MApp>`,
  },
  {
    name: "Main_Model.cs",
    code: `using Manta;

[View<Main>]
sealed partial class Main_Model : WindowModel
{
    public string Greeting => Lang.Welcome;

    [Command]
    void Open() => Dialog.Show<About_Model>();
}`,
  },
  {
    name: "Main.axaml",
    code: `<MView xmlns="https://github.com/avaloniaui"
       xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
       x:Class="MyApp.Main">
    <StackPanel Spacing="{MSize Spacing}">
        <TextBlock Text="{Lang Welcome}" />
        <Button Content="{Lang Open}" Command="{Binding Open}" />
    </StackPanel>
</MView>`,
  },
]

export const content: Record<Locale, LandingContent> = {
  en: {
    meta: {
      title: "Manta — Avalonia UI toolkit and MVVM framework",
      description:
        "Manta is a UI toolkit and MVVM framework for building cross-platform desktop applications on Avalonia, with themeable controls and compile-time code generation.",
    },
    nav: [
      { label: "Features", href: "#features" },
      { label: "Usage", href: "#usage" },
      { label: "Code", href: "#code" },
      { label: "Theming", href: "#theming" },
      { label: "Install", href: "#install" },
      { label: "FAQ", href: "#faq" },
    ],
    header: {
      language: "Switch language",
      theme: "Toggle theme",
      github: "View on GitHub",
    },
    hero: {
      badge: "Avalonia 12 · .NET 10",
      title: "Build desktop apps that",
      accent: "feel native.",
      description:
        "Manta is a UI toolkit and MVVM framework for Avalonia. Themeable controls, an explicit MVVM runtime and compile-time code generation — without the ceremony.",
      primary: "Get started",
      secondary: "View on GitHub",
      hint: "dotnet add package ONEO.Manta",
    },
    features: {
      title: "Everything you need, nothing you don't",
      description:
        "Manta layers a focused control set, a predictable MVVM runtime and source generators on top of Avalonia.",
      items: [
        {
          icon: Blocks,
          title: "M-prefixed controls",
          description:
            "A broad set of controls plus themed styles for the Avalonia controls you already use.",
        },
        {
          icon: MoonStar,
          title: "Light and dark themes",
          description:
            "Two palettes built on the OKLch color space, so both stay perceptually even.",
        },
        {
          icon: Layers,
          title: "Explicit MVVM runtime",
          description:
            "Model, ViewModel, WindowModel and DialogModel base classes with no hidden magic.",
        },
        {
          icon: WandSparkles,
          title: "Compile-time view creation",
          description:
            "The [View<T>] attribute generates view plumbing, so models only declare their bindings.",
        },
        {
          icon: PanelsTopLeft,
          title: "Regions and tab navigation",
          description:
            "RegionManager<T> opens, reuses and switches views with a single call.",
        },
        {
          icon: Languages,
          title: "Type-safe localization",
          description:
            "Language keys are generated from YAML files and consumed through the {Lang} markup.",
        },
      ],
    },
    usage: {
      title: "Use only what you need",
      description:
        "Manta is not all-or-nothing. Every layer ships independently, so you can adopt localization, theming, MVVM or the control set on its own.",
      items: [
        {
          icon: Languages,
          title: "Localization only",
          description:
            "Reference ONEO.Manta for the {Lang} markup and generated keys, and keep the Fluent theme and the standard Avalonia controls.",
          code: "Lang.Register(LangData.Load())",
        },
        {
          icon: Palette,
          title: "Theming only",
          description:
            "Add <MantaTheme /> to Application.Styles to restyle Manta and the Avalonia controls. No base classes required.",
          code: "<MantaTheme />",
        },
        {
          icon: Layers,
          title: "MVVM only",
          description:
            "Take Model, WindowModel, [View<T>] and RegionManager<T> while keeping your own look and theme.",
          code: "[View<Main>] sealed partial class Main_Model : WindowModel",
        },
        {
          icon: Blocks,
          title: "Controls only",
          description:
            "Drop the M controls and theme variables into an existing app with a hand-written ObservableObject and RelayCommand.",
          code: "dotnet add package ONEO.Manta",
        },
      ],
      samples: {
        label: "See the components-only and localization-only samples",
        href: site.samples,
      },
    },
    code: {
      title: "Less ceremony, more product",
      description:
        "Register the theme, pair a view model with its view and let the generator wire the rest.",
      copy: "Copy",
      copied: "Copied",
      samples,
    },
    theming: {
      title: "Theming that stays consistent",
      description:
        "Every color, size and radius flows through theme variables. Swap a resource dictionary and the whole application follows.",
      bullets: [
        {
          title: "OKLch color space",
          description:
            "Light and dark palettes share the same perceived lightness steps.",
        },
        {
          title: "Theme variables",
          description:
            "Resolve colors and spacing through MBrush, MSize, MRadius and friends — never hardcode values.",
        },
        {
          title: "One switch, two moods",
          description:
            "Move between light and dark without touching a single control style.",
        },
      ],
    },
    install: {
      title: "Up and running in a minute",
      description:
        "Manta targets .NET 10 and Avalonia 12. Add the packages, then build and run the demo.",
      steps: [
        { label: "Add the library", code: "dotnet add package ONEO.Manta" },
        {
          label: "Add the source generators",
          code: "dotnet add package ONEO.Manta.Generators",
        },
        {
          label: "Explore the reference app",
          code: "cd source && dotnet run --project MantaDemo",
        },
      ],
      requirements: "Requires the .NET 10 SDK.",
    },
    faq: {
      title: "Frequently asked questions",
      description: "Short answers before you open the docs.",
      items: [
        {
          question: "Does Manta work with an existing Avalonia app?",
          answer:
            "Yes. Manta is a set of controls, styles and base classes on top of Avalonia. You can adopt the theme first and migrate to the MVVM runtime at your own pace.",
        },
        {
          question: "Which platforms are supported?",
          answer:
            "Anywhere Avalonia 12 runs: Windows, macOS and Linux, targeting .NET 10.",
        },
        {
          question: "Do I have to use MVVM?",
          answer:
            "No. The controls and themes stand on their own. The MVVM runtime and generators are optional building blocks.",
        },
        {
          question: "Is Manta free?",
          answer:
            "Manta is released under the MIT license, so you can use it in commercial and open-source applications.",
        },
      ],
    },
    cta: {
      title: "Ready to build with Manta?",
      description:
        "Add the package, explore the demo, and shape the toolkit with us.",
      primary: "Read the architecture",
      secondary: "Browse the source",
    },
    footer: {
      description:
        "Avalonia UI toolkit and MVVM framework for cross-platform desktop applications.",
      groups: [
        {
          title: "Product",
          links: [
            { label: "Features", href: "#features" },
            { label: "Install", href: "#install" },
            { label: "NuGet", href: site.nuget },
          ],
        },
        {
          title: "Resources",
          links: [
            {
              label: "Architecture",
              href: site.architecture,
            },
            {
              label: "Changelog",
              href: site.changelog,
            },
            {
              label: "License",
              href: site.license,
            },
          ],
        },
        {
          title: "Community",
          links: [
            { label: "GitHub", href: site.github },
            {
              label: "Issues",
              href: site.issues,
            },
          ],
        },
      ],
      builtWith:
        "Built with React Router and shadcn/ui. Released under the MIT license.",
    },
  },
  zh: {
    meta: {
      title: "Manta — Avalonia UI 工具包与 MVVM 框架",
      description:
        "Manta 是构建 Avalonia 跨平台桌面应用的 UI 工具包与 MVVM 框架，提供可主题化的控件和编译期代码生成。",
    },
    nav: [
      { label: "特性", href: "#features" },
      { label: "按需使用", href: "#usage" },
      { label: "代码", href: "#code" },
      { label: "主题", href: "#theming" },
      { label: "安装", href: "#install" },
      { label: "常见问题", href: "#faq" },
    ],
    header: {
      language: "切换语言",
      theme: "切换主题",
      github: "在 GitHub 上查看",
    },
    hero: {
      badge: "Avalonia 12 · .NET 10",
      title: "打造真正原生的",
      accent: "桌面应用。",
      description:
        "Manta 是构建 Avalonia 应用的 UI 工具包与 MVVM 框架。可主题化的控件、清晰的 MVVM 运行时，以及编译期代码生成——无需繁琐的样板。",
      primary: "开始使用",
      secondary: "在 GitHub 上查看",
      hint: "dotnet add package ONEO.Manta",
    },
    features: {
      title: "该有的都有，多余的都没有",
      description:
        "Manta 在 Avalonia 之上提供专注的控件集、可预测的 MVVM 运行时和源代码生成器。",
      items: [
        {
          icon: Blocks,
          title: "M 前缀控件",
          description: "丰富的控件集合，并为现有 Avalonia 控件提供主题化样式。",
        },
        {
          icon: MoonStar,
          title: "明暗双主题",
          description: "基于 OKLch 色彩空间的两套调色板，感知亮度始终一致。",
        },
        {
          icon: Layers,
          title: "显式 MVVM 运行时",
          description:
            "Model、ViewModel、WindowModel 与 DialogModel 基类，没有隐藏的魔法。",
        },
        {
          icon: WandSparkles,
          title: "编译期视图创建",
          description: "[View<T>] 特性生成视图管线，模型只需声明绑定。",
        },
        {
          icon: PanelsTopLeft,
          title: "区域与标签导航",
          description:
            "RegionManager<T> 用一次调用完成视图的打开、复用与切换。",
        },
        {
          icon: Languages,
          title: "类型安全的本地化",
          description: "语言键由 YAML 文件生成，并通过 {Lang} 标记使用。",
        },
      ],
    },
    usage: {
      title: "只取所需，按需使用",
      description:
        "Manta 并非全有或全无。每一层都独立发布，你可以单独使用本地化、主题、MVVM 或控件集。",
      items: [
        {
          icon: Languages,
          title: "仅使用本地化",
          description:
            "只引用 ONEO.Manta 以获得 {Lang} 标记与生成的键，继续使用 Fluent 主题和标准 Avalonia 控件。",
          code: "Lang.Register(LangData.Load())",
        },
        {
          icon: Palette,
          title: "仅使用主题",
          description:
            "将 <MantaTheme /> 加入 Application.Styles，即可为 Manta 与 Avalonia 控件换肤，无需任何基类。",
          code: "<MantaTheme />",
        },
        {
          icon: Layers,
          title: "仅使用 MVVM",
          description:
            "仅取用 Model、WindowModel、[View<T>] 与 RegionManager<T>，保留自己的外观与主题。",
          code: "[View<Main>] sealed partial class Main_Model : WindowModel",
        },
        {
          icon: Blocks,
          title: "仅使用控件",
          description:
            "在现有应用中直接使用 M 控件与主题变量，搭配手写的 ObservableObject 与 RelayCommand。",
          code: "dotnet add package ONEO.Manta",
        },
      ],
      samples: {
        label: "查看仅控件与仅本地化的示例",
        href: site.samples,
      },
    },
    code: {
      title: "更少样板，更多产品",
      description: "注册主题，将视图模型与视图配对，剩下的交给生成器。",
      copy: "复制",
      copied: "已复制",
      samples,
    },
    theming: {
      title: "保持一致的主题系统",
      description:
        "所有颜色、尺寸与圆角都经由主题变量流转。替换资源字典，整个应用随之改变。",
      bullets: [
        {
          title: "OKLch 色彩空间",
          description: "明暗调色板共享相同的感知亮度阶梯。",
        },
        {
          title: "主题变量",
          description:
            "通过 MBrush、MSize、MRadius 等解析颜色与间距，绝不硬编码。",
        },
        {
          title: "一次切换，两种氛围",
          description: "在明暗之间切换，无需改动任何控件样式。",
        },
      ],
    },
    install: {
      title: "一分钟即可上手",
      description:
        "Manta 面向 .NET 10 与 Avalonia 12。添加包，然后构建并运行示例。",
      steps: [
        { label: "添加库", code: "dotnet add package ONEO.Manta" },
        {
          label: "添加源代码生成器",
          code: "dotnet add package ONEO.Manta.Generators",
        },
        {
          label: "运行参考应用",
          code: "cd source && dotnet run --project MantaDemo",
        },
      ],
      requirements: "需要 .NET 10 SDK。",
    },
    faq: {
      title: "常见问题",
      description: "在打开文档之前，先看几个简短的回答。",
      items: [
        {
          question: "Manta 能用于已有的 Avalonia 应用吗？",
          answer:
            "可以。Manta 是构建在 Avalonia 之上的控件、样式与基类集合。你可以先接入主题，再按自己的节奏迁移到 MVVM 运行时。",
        },
        {
          question: "支持哪些平台？",
          answer:
            "凡是 Avalonia 12 能运行的平台都支持：Windows、macOS 与 Linux，面向 .NET 10。",
        },
        {
          question: "必须使用 MVVM 吗？",
          answer:
            "不必。控件与主题可以独立使用，MVVM 运行时与生成器是可选的构建模块。",
        },
        {
          question: "Manta 是免费的吗？",
          answer: "Manta 基于 MIT 许可证发布，可用于商业与开源应用。",
        },
      ],
    },
    cta: {
      title: "准备好用 Manta 构建了吗？",
      description: "添加包，体验示例，并与我们一起打磨这个工具包。",
      primary: "阅读架构文档",
      secondary: "浏览源代码",
    },
    footer: {
      description: "面向跨平台桌面应用的 Avalonia UI 工具包与 MVVM 框架。",
      groups: [
        {
          title: "产品",
          links: [
            { label: "特性", href: "#features" },
            { label: "安装", href: "#install" },
            { label: "NuGet", href: site.nuget },
          ],
        },
        {
          title: "资源",
          links: [
            {
              label: "架构",
              href: site.architecture,
            },
            {
              label: "更新日志",
              href: site.changelog,
            },
            {
              label: "许可证",
              href: site.license,
            },
          ],
        },
        {
          title: "社区",
          links: [
            { label: "GitHub", href: site.github },
            {
              label: "问题反馈",
              href: site.issues,
            },
          ],
        },
      ],
      builtWith: "由 React Router 与 shadcn/ui 构建，基于 MIT 许可证发布。",
    },
  },
}
