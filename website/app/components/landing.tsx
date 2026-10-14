import { ArrowRight, BookOpen, Package, Play, Terminal } from "lucide-react"

import { AotVisual } from "~/components/aot-visual"
import { CodeBlock } from "~/components/code-block"
import { CopyButton } from "~/components/copy-button"
import { GithubIcon } from "~/components/github-icon"
import { SiteFooter } from "~/components/site-footer"
import { SiteHeader } from "~/components/site-header"
import {
  Accordion,
  AccordionContent,
  AccordionItem,
  AccordionTrigger,
} from "~/components/ui/accordion"
import { Badge } from "~/components/ui/badge"
import { Button } from "~/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "~/components/ui/card"
import { Separator } from "~/components/ui/separator"
import { Tabs, TabsContent, TabsList, TabsTrigger } from "~/components/ui/tabs"
import { content, type LandingContent } from "~/lib/content"
import { site, type Locale } from "~/lib/site"

const swatches = [
  { name: "primary", className: "bg-primary" },
  { name: "secondary", className: "bg-secondary" },
  { name: "muted", className: "bg-muted" },
  { name: "accent", className: "bg-accent" },
  { name: "border", className: "bg-border" },
  { name: "foreground", className: "bg-foreground" },
]

function SectionHeading({
  title,
  description,
}: {
  title: string
  description: string
}) {
  return (
    <div className="flex max-w-2xl flex-col gap-3">
      <h2 className="font-heading text-3xl font-semibold tracking-tight text-balance sm:text-4xl">
        {title}
      </h2>
      <p className="text-pretty text-muted-foreground">{description}</p>
    </div>
  )
}

/** Wraps the "Native AOT" words in the hero copy with the animated accent. */
function highlightAot(text: string) {
  const [before, after] = text.split("Native AOT")
  if (after === undefined) return text

  return (
    <>
      {before}
      <span className="aot-accent">Native AOT</span>
      {after}
    </>
  )
}

export function Landing({ locale }: { locale: Locale }) {
  const c = content[locale]

  return (
    <div className="flex min-h-svh flex-col">
      <SiteHeader locale={locale} content={c} />

      <main className="flex-1">
        <Hero content={c} />
        <FeaturesSection content={c} />
        <AotSection content={c} />
        <UsageSection content={c} />
        <CodeSection content={c} />
        <ThemingSection content={c} />
        <InstallSection content={c} />
        <FaqSection content={c} />
        <CtaSection content={c} />
      </main>

      <SiteFooter content={c} />
    </div>
  )
}

function Hero({ content: c }: { content: LandingContent }) {
  return (
    <section className="relative overflow-hidden">
      <div
        aria-hidden
        className="pointer-events-none absolute top-0 left-1/2 -z-10 size-[42rem] -translate-x-1/2 -translate-y-1/2 rounded-full bg-primary/15 blur-3xl"
      />
      <div className="mx-auto flex w-full max-w-6xl flex-col items-center gap-6 px-4 pt-20 pb-16 text-center sm:px-6 sm:pt-28 sm:pb-24">
        <Badge variant="secondary">{c.hero.badge}</Badge>
        <h1 className="max-w-3xl font-heading text-4xl font-semibold tracking-tight text-balance sm:text-6xl">
          {c.hero.title} <span className="text-primary">{c.hero.accent}</span>
        </h1>
        <p className="max-w-2xl text-lg text-pretty text-muted-foreground">
          {highlightAot(c.hero.description)}
        </p>
        <div className="flex flex-col gap-3 sm:flex-row">
          <Button size="lg" render={<a href="#install" />}>
            {c.hero.primary}
          </Button>
          <Button
            variant="outline"
            size="lg"
            render={<a href={site.github} target="_blank" rel="noreferrer" />}
          >
            <GithubIcon data-icon="inline-start" />
            {c.hero.secondary}
          </Button>
        </div>
        <div className="mt-2 flex items-center gap-2 rounded-xl border bg-muted/40 py-1.5 pr-1.5 pl-3 font-mono text-sm text-muted-foreground">
          <Terminal className="size-4" aria-hidden />
          <span>{c.hero.hint}</span>
          <CopyButton
            value={c.hero.hint}
            copy={c.code.copy}
            copied={c.code.copied}
          />
        </div>
      </div>
    </section>
  )
}

function FeaturesSection({ content: c }: { content: LandingContent }) {
  return (
    <section
      id="features"
      className="mx-auto w-full max-w-6xl px-4 py-16 sm:px-6 sm:py-24"
    >
      <SectionHeading
        title={c.features.title}
        description={c.features.description}
      />
      <div className="mt-10 grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
        {c.features.items.map((feature) => (
          <Card key={feature.title} className="h-full">
            <CardHeader>
              <span className="flex size-9 items-center justify-center rounded-lg bg-primary/10 text-primary">
                <feature.icon className="size-5" aria-hidden />
              </span>
              <CardTitle className="mt-2">{feature.title}</CardTitle>
            </CardHeader>
            <CardContent className="text-muted-foreground">
              {feature.description}
            </CardContent>
          </Card>
        ))}
      </div>
    </section>
  )
}

function AotSection({ content: c }: { content: LandingContent }) {
  return (
    <section id="aot" className="border-y border-border/60 bg-muted/20">
      <div className="mx-auto w-full max-w-6xl px-4 py-16 sm:px-6 sm:py-24">
        <div className="grid items-center gap-12 lg:grid-cols-2">
          <div className="flex flex-col gap-6">
            <SectionHeading
              title={c.aot.title}
              description={c.aot.description}
            />
            <ul className="flex flex-col gap-4">
              {c.aot.bullets.map((bullet) => (
                <li key={bullet.title} className="flex flex-col gap-1">
                  <span className="font-medium">{bullet.title}</span>
                  <span className="text-sm text-muted-foreground">
                    {bullet.description}
                  </span>
                </li>
              ))}
            </ul>
            <CodeBlock
              code={c.aot.code}
              copy={c.code.copy}
              copied={c.code.copied}
            />
          </div>
          <AotVisual />
        </div>
      </div>
    </section>
  )
}

function UsageSection({ content: c }: { content: LandingContent }) {
  return (
    <section
      id="usage"
      className="mx-auto w-full max-w-6xl px-4 py-16 sm:px-6 sm:py-24"
    >
      <SectionHeading title={c.usage.title} description={c.usage.description} />
      <div className="mt-10 grid gap-4 sm:grid-cols-2">
        {c.usage.items.map((item) => (
          <Card key={item.title} className="h-full">
            <CardHeader>
              <span className="flex size-9 items-center justify-center rounded-lg bg-primary/10 text-primary">
                <item.icon className="size-5" aria-hidden />
              </span>
              <CardTitle className="mt-2">{item.title}</CardTitle>
            </CardHeader>
            <CardContent className="flex flex-col gap-3 text-muted-foreground">
              <span>{item.description}</span>
              <code className="block overflow-x-auto rounded-lg border bg-background px-3 py-2 font-mono text-xs whitespace-nowrap">
                {item.code}
              </code>
            </CardContent>
          </Card>
        ))}
      </div>
      <Button
        variant="link"
        className="mt-8 px-0"
        render={
          <a href={c.usage.samples.href} target="_blank" rel="noreferrer" />
        }
      >
        {c.usage.samples.label}
        <ArrowRight data-icon="inline-end" />
      </Button>
    </section>
  )
}

function CodeSection({ content: c }: { content: LandingContent }) {
  return (
    <section id="code" className="border-y border-border/60 bg-muted/20">
      <div className="mx-auto w-full max-w-6xl px-4 py-16 sm:px-6 sm:py-24">
        <SectionHeading title={c.code.title} description={c.code.description} />
        <Tabs defaultValue={c.code.samples[0].name} className="mt-10 gap-4">
          <TabsList>
            {c.code.samples.map((sample) => (
              <TabsTrigger key={sample.name} value={sample.name}>
                {sample.name}
              </TabsTrigger>
            ))}
          </TabsList>
          {c.code.samples.map((sample) => (
            <TabsContent key={sample.name} value={sample.name}>
              <CodeBlock
                code={sample.code}
                copy={c.code.copy}
                copied={c.code.copied}
              />
            </TabsContent>
          ))}
        </Tabs>
      </div>
    </section>
  )
}

function ThemingSection({ content: c }: { content: LandingContent }) {
  return (
    <section
      id="theming"
      className="mx-auto w-full max-w-6xl px-4 py-16 sm:px-6 sm:py-24"
    >
      <div className="grid items-center gap-12 lg:grid-cols-2">
        <SectionHeading
          title={c.theming.title}
          description={c.theming.description}
        />
        <div className="flex flex-col gap-6">
          <div className="grid grid-cols-6 gap-2">
            {swatches.map((swatch) => (
              <div
                key={swatch.name}
                className="flex flex-col items-center gap-2"
              >
                <div
                  className={`size-10 rounded-lg ring-1 ring-foreground/10 ${swatch.className}`}
                />
                <span className="font-mono text-[0.65rem] text-muted-foreground">
                  {swatch.name}
                </span>
              </div>
            ))}
          </div>
          <Separator />
          <ul className="flex flex-col gap-4">
            {c.theming.bullets.map((bullet) => (
              <li key={bullet.title} className="flex flex-col gap-1">
                <span className="font-medium">{bullet.title}</span>
                <span className="text-sm text-muted-foreground">
                  {bullet.description}
                </span>
              </li>
            ))}
          </ul>
        </div>
      </div>
    </section>
  )
}

function InstallSection({ content: c }: { content: LandingContent }) {
  return (
    <section id="install" className="border-y border-border/60 bg-muted/20">
      <div className="mx-auto w-full max-w-6xl px-4 py-16 sm:px-6 sm:py-24">
        <SectionHeading
          title={c.install.title}
          description={c.install.description}
        />
        <div className="mt-10 grid gap-4 lg:grid-cols-3">
          {c.install.steps.map((step, index) => (
            <Card key={step.code}>
              <CardHeader>
                <CardTitle className="flex items-center gap-2">
                  <span className="flex size-6 items-center justify-center rounded-full bg-primary/10 font-mono text-xs font-semibold text-primary">
                    {index + 1}
                  </span>
                  {step.label}
                </CardTitle>
              </CardHeader>
              <CardContent>
                <code className="block overflow-x-auto rounded-lg border bg-background px-3 py-2 font-mono text-xs">
                  {step.code}
                </code>
              </CardContent>
            </Card>
          ))}
        </div>
        <div className="mt-6 flex flex-wrap items-center gap-4 text-sm text-muted-foreground">
          <span className="inline-flex items-center gap-2">
            <Package className="size-4" aria-hidden />
            {c.install.requirements}
          </span>
          <Button
            variant="outline"
            size="sm"
            render={<a href={site.nuget} target="_blank" rel="noreferrer" />}
          >
            <ArrowRight data-icon="inline-end" />
            NuGet
          </Button>
        </div>
      </div>
    </section>
  )
}

function FaqSection({ content: c }: { content: LandingContent }) {
  return (
    <section
      id="faq"
      className="mx-auto w-full max-w-6xl px-4 py-16 sm:px-6 sm:py-24"
    >
      <SectionHeading title={c.faq.title} description={c.faq.description} />
      <Accordion className="mt-8 max-w-3xl">
        {c.faq.items.map((item) => (
          <AccordionItem key={item.question} value={item.question}>
            <AccordionTrigger>{item.question}</AccordionTrigger>
            <AccordionContent className="text-muted-foreground">
              {item.answer}
            </AccordionContent>
          </AccordionItem>
        ))}
      </Accordion>
    </section>
  )
}

function CtaSection({ content: c }: { content: LandingContent }) {
  return (
    <section className="mx-auto w-full max-w-6xl px-4 pb-20 sm:px-6 sm:pb-28">
      <div className="relative overflow-hidden rounded-2xl border bg-linear-to-b from-primary/10 to-background px-6 py-12 text-center sm:px-12 sm:py-16">
        <div className="flex flex-col items-center gap-5">
          <h2 className="max-w-xl font-heading text-3xl font-semibold tracking-tight text-balance sm:text-4xl">
            {c.cta.title}
          </h2>
          <p className="max-w-lg text-pretty text-muted-foreground">
            {c.cta.description}
          </p>
          <div className="flex flex-col gap-3 sm:flex-row">
            <Button
              size="lg"
              render={
                <a href={site.architecture} target="_blank" rel="noreferrer" />
              }
            >
              <BookOpen data-icon="inline-start" />
              {c.cta.primary}
            </Button>
            <Button
              variant="outline"
              size="lg"
              render={<a href={site.github} target="_blank" rel="noreferrer" />}
            >
              <Play data-icon="inline-start" />
              {c.cta.secondary}
            </Button>
          </div>
        </div>
      </div>
    </section>
  )
}
