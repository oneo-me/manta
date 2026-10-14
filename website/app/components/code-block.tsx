import { CopyButton } from "~/components/copy-button"
import { cn } from "~/lib/utils"

export function CodeBlock({
  code,
  copy,
  copied,
  className,
}: {
  code: string
  copy: string
  copied: string
  className?: string
}) {
  return (
    <div
      className={cn(
        "relative overflow-hidden rounded-xl border bg-muted/40",
        className
      )}
    >
      <CopyButton
        value={code}
        copy={copy}
        copied={copied}
        className="absolute top-2 right-2"
      />
      <pre className="overflow-x-auto p-4 text-sm leading-relaxed">
        <code className="font-mono text-foreground/90">{code}</code>
      </pre>
    </div>
  )
}
