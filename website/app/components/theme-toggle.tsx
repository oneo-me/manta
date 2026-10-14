import { useEffect, useState } from "react"
import { MoonStar, Sun } from "lucide-react"

import { Button } from "~/components/ui/button"
import { themeStorageKey } from "~/lib/theme"

export function ThemeToggle({ label }: { label: string }) {
  const [isDark, setIsDark] = useState(false)

  useEffect(() => {
    setIsDark(document.documentElement.classList.contains("dark"))
  }, [])

  function toggle() {
    const next = !document.documentElement.classList.contains("dark")
    document.documentElement.classList.toggle("dark", next)
    document.documentElement.style.colorScheme = next ? "dark" : "light"
    try {
      localStorage.setItem(themeStorageKey, next ? "dark" : "light")
    } catch {
      // Storage can be unavailable; the in-memory theme still applies.
    }
    setIsDark(next)
  }

  return (
    <Button
      variant="ghost"
      size="icon-sm"
      onClick={toggle}
      aria-label={label}
      title={label}
    >
      {isDark ? <Sun /> : <MoonStar />}
    </Button>
  )
}
