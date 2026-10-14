import {
  Links,
  Meta,
  Outlet,
  Scripts,
  ScrollRestoration,
  isRouteErrorResponse,
  useLocation,
} from "react-router"

import type { Route } from "./+types/root"
import iconUrl from "~/assets/icon.png"
import { localeFromPathname, site } from "~/lib/site"
import { themeInitScript } from "~/lib/theme"
import "./app.css"

export function Layout({ children }: { children: React.ReactNode }) {
  const { pathname } = useLocation()
  const locale = localeFromPathname(pathname)

  return (
    <html lang={locale === "zh" ? "zh-CN" : "en"}>
      <head>
        <meta charSet="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <meta name="color-scheme" content="light dark" />
        <link rel="icon" type="image/png" href={iconUrl} />
        <script dangerouslySetInnerHTML={{ __html: themeInitScript }} />
        <Meta />
        <Links />
      </head>
      <body>
        {children}
        <ScrollRestoration />
        <Scripts />
      </body>
    </html>
  )
}

export default function App() {
  return <Outlet />
}

export function ErrorBoundary({ error }: Route.ErrorBoundaryProps) {
  let message = "Oops!"
  let details = "An unexpected error occurred."
  let stack: string | undefined

  if (isRouteErrorResponse(error)) {
    message = error.status === 404 ? "404" : "Error"
    details =
      error.status === 404
        ? "The requested page could not be found."
        : error.statusText || details
  } else if (import.meta.env.DEV && error && error instanceof Error) {
    details = error.message
    stack = error.stack
  }

  return (
    <main className="mx-auto flex min-h-svh w-full max-w-3xl flex-col justify-center gap-3 px-6">
      <p className="font-mono text-sm text-muted-foreground">{site.name}</p>
      <h1 className="font-heading text-3xl font-semibold">{message}</h1>
      <p className="text-muted-foreground">{details}</p>
      {stack ? (
        <pre className="overflow-x-auto rounded-xl border bg-muted/40 p-4 text-sm">
          <code className="font-mono">{stack}</code>
        </pre>
      ) : null}
    </main>
  )
}
