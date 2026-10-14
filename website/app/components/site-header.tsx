import { Link } from "react-router"

import { GithubIcon } from "~/components/github-icon"
import { Logo } from "~/components/logo"
import { ThemeToggle } from "~/components/theme-toggle"
import { Button } from "~/components/ui/button"
import type { LandingContent } from "~/lib/content"
import { site, type Locale } from "~/lib/site"

export function SiteHeader({
  locale,
  content,
}: {
  locale: Locale
  content: LandingContent
}) {
  const nextLocale = locale === "en" ? "zh" : "en"
  const nextLocaleHref = locale === "en" ? "/zh" : "/"

  return (
    <header className="sticky top-0 z-50 border-b border-border/60 bg-background/80 backdrop-blur-md">
      <div className="mx-auto flex h-14 w-full max-w-6xl items-center gap-2 px-4 sm:px-6">
        <Link
          to="/"
          className="flex items-center gap-2 rounded-md font-heading text-base font-semibold"
        >
          <Logo />
          {site.name}
        </Link>

        <nav className="ml-4 hidden items-center gap-1 md:flex">
          {content.nav.map((item) => (
            <a
              key={item.href}
              href={item.href}
              className="rounded-md px-2.5 py-1.5 text-sm text-muted-foreground transition-colors hover:bg-muted hover:text-foreground"
            >
              {item.label}
            </a>
          ))}
        </nav>

        <div className="ml-auto flex items-center gap-1">
          <Button
            variant="ghost"
            size="icon-sm"
            render={
              <a
                href={site.github}
                target="_blank"
                rel="noreferrer"
                aria-label={content.header.github}
                title={content.header.github}
              />
            }
          >
            <GithubIcon />
          </Button>
          <Button
            variant="ghost"
            size="sm"
            render={
              <Link
                to={nextLocaleHref}
                aria-label={content.header.language}
                title={content.header.language}
              />
            }
          >
            {nextLocale === "zh" ? "中文" : "EN"}
          </Button>
          <ThemeToggle label={content.header.theme} />
        </div>
      </div>
    </header>
  )
}
