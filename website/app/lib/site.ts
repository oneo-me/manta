export const site = {
  name: "Manta",
  github: "https://github.com/oneo-me/manta",
  issues: "https://github.com/oneo-me/manta/issues",
  nuget: "https://www.nuget.org/packages/ONEO.Manta",
  architecture: "https://github.com/oneo-me/manta/blob/main/ARCHITECTURE.md",
  samples: "https://github.com/oneo-me/manta/tree/main/samples",
  changelog: "https://github.com/oneo-me/manta/blob/main/CHANGELOG.md",
  license: "https://github.com/oneo-me/manta/blob/main/LICENSE",
} as const

export type Locale = "en" | "zh"

/** Derives the locale from a router pathname with the basename stripped. */
export function localeFromPathname(pathname: string): Locale {
  return pathname.replace(/\/+$/, "").endsWith("/zh") ? "zh" : "en"
}
