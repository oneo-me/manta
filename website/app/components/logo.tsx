import iconUrl from "~/assets/icon.png"
import { cn } from "~/lib/utils"

/**
 * The Manta app icon, used as the brand mark in the header and footer. It is
 * decorative: the adjacent site name already labels the link.
 */
export function Logo({ className }: { className?: string }) {
  return (
    <img
      src={iconUrl}
      alt=""
      aria-hidden
      draggable={false}
      className={cn("size-8 shrink-0 select-none", className)}
    />
  )
}
