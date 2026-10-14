import { cn } from "~/lib/utils"

/** The Manta monogram used in the header and footer. */
export function Logo({ className }: { className?: string }) {
  return (
    <span
      aria-hidden
      className={cn(
        "flex size-7 items-center justify-center rounded-lg bg-primary font-heading text-sm leading-none font-bold text-primary-foreground",
        className
      )}
    >
      M
    </span>
  )
}
