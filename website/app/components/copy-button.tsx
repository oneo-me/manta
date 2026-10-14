import { useState } from "react"
import { Check, Copy } from "lucide-react"

import { Button } from "~/components/ui/button"

export function CopyButton({
  value,
  copy,
  copied,
  className,
}: {
  value: string
  copy: string
  copied: string
  className?: string
}) {
  const [isCopied, setIsCopied] = useState(false)
  const label = isCopied ? copied : copy

  async function onCopy() {
    try {
      await navigator.clipboard.writeText(value)
      setIsCopied(true)
      window.setTimeout(() => setIsCopied(false), 2000)
    } catch {
      // Clipboard access can be denied; leave the button unchanged.
    }
  }

  return (
    <Button
      variant="ghost"
      size="icon-sm"
      onClick={onCopy}
      aria-label={label}
      title={label}
      className={className}
    >
      {isCopied ? <Check /> : <Copy />}
    </Button>
  )
}
