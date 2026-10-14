import { useEffect, useRef, useState } from "react"
import { Check, Cpu, Rocket } from "lucide-react"

const steps = [
  { label: "Trimmed managed assemblies", delay: 200 },
  { label: "Generated native code with ILC", delay: 600 },
  { label: "Wrote a single native binary", delay: 1000 },
]

/**
 * Animated "dotnet publish" card for the Native AOT section. The steps reveal
 * once when the card scrolls into view, while the glow, scanline, progress bar
 * and badge keep moving so the section stays alive.
 */
export function AotVisual() {
  const ref = useRef<HTMLDivElement>(null)
  const [active, setActive] = useState(false)

  useEffect(() => {
    const node = ref.current
    if (!node) return
    if (window.matchMedia("(prefers-reduced-motion: reduce)").matches) {
      setActive(true)
      return
    }

    const observer = new IntersectionObserver(
      ([entry]) => {
        if (entry.isIntersecting) {
          setActive(true)
          observer.disconnect()
        }
      },
      { threshold: 0.35 }
    )
    observer.observe(node)
    return () => observer.disconnect()
  }, [])

  return (
    <div ref={ref} className={`relative ${active ? "aot-active" : ""}`}>
      <div
        aria-hidden
        className="aot-glow pointer-events-none absolute -inset-8 rounded-[2.5rem] bg-primary/25 blur-3xl"
      />
      <div className="relative overflow-hidden rounded-2xl border bg-card shadow-lg shadow-primary/5">
        <div className="flex items-center gap-2 border-b bg-muted/40 px-4 py-3">
          <span className="size-2.5 rounded-full bg-destructive/70" />
          <span className="size-2.5 rounded-full bg-amber-400/80" />
          <span className="size-2.5 rounded-full bg-emerald-400/80" />
          <span className="ml-2 truncate font-mono text-xs text-muted-foreground">
            dotnet publish -c Release
          </span>
        </div>
        <div className="relative flex flex-col gap-3 overflow-hidden p-5 font-mono text-sm">
          <div
            aria-hidden
            className="aot-scan pointer-events-none absolute inset-x-0 h-24 bg-linear-to-b from-transparent via-primary/15 to-transparent"
          />
          <div className="aot-step flex items-center gap-2 text-muted-foreground">
            <span className="text-primary">$</span>
            <span>dotnet publish -c Release</span>
          </div>
          {steps.map((step) => (
            <div
              key={step.label}
              className="aot-step flex items-center gap-2 text-muted-foreground"
              style={{ animationDelay: `${step.delay}ms` }}
            >
              <Check className="size-4 shrink-0 text-emerald-500" />
              {step.label}
            </div>
          ))}
          <div
            className="aot-step flex items-center gap-2 font-medium text-foreground"
            style={{ animationDelay: "1400ms" }}
          >
            <Rocket className="size-4 shrink-0 text-primary" />
            MantaDemo published as a native binary
          </div>
        </div>
        <div className="relative h-1 overflow-hidden bg-muted">
          <div className="aot-load absolute inset-y-0 w-1/3 bg-primary" />
        </div>
        <div className="flex flex-wrap items-center justify-between gap-3 border-t bg-muted/30 px-4 py-3">
          <span className="relative inline-flex items-center gap-1.5 rounded-full border border-primary/30 bg-primary/10 px-2.5 py-1 text-xs font-medium text-primary">
            <span className="relative flex size-2">
              <span className="absolute inline-flex size-full animate-ping rounded-full bg-primary/70" />
              <span className="relative inline-flex size-2 rounded-full bg-primary" />
            </span>
            <Cpu className="size-3.5" aria-hidden />
            Native AOT
          </span>
          <span className="font-mono text-xs text-muted-foreground">
            0 trim · 0 AOT warnings
          </span>
        </div>
      </div>
    </div>
  )
}
