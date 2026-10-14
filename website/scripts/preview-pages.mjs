// Serves the assembled Pages artifact the way GitHub Pages does, including
// the `/manta/` base path and the `404.html` fallback. Useful for checking a
// production build locally.

import { fileURLToPath } from "node:url"
import { join, resolve, sep } from "node:path"

const basePath =
  "/" + (process.env.BASE_PATH ?? "/manta/").replace(/^\/+|\/+$/g, "") + "/"
const root = resolve(fileURLToPath(new URL("../build/pages/", import.meta.url)))
const port = Number(process.env.PORT ?? 4173)

const server = Bun.serve({
  port,
  async fetch(request) {
    const { pathname } = new URL(request.url)
    if (!pathname.startsWith(basePath)) {
      return new Response("Not found", { status: 404 })
    }

    let relative = decodeURIComponent(pathname.slice(basePath.length))
    if (relative === "" || relative.endsWith("/")) {
      relative += "index.html"
    }

    const target = resolve(join(root, relative))
    if (target !== root && !target.startsWith(root + sep)) {
      return new Response("Not found", { status: 404 })
    }

    const file = Bun.file(target)
    if (await file.exists()) {
      return new Response(file)
    }

    const fallback = Bun.file(join(root, "404.html"))
    if (await fallback.exists()) {
      return new Response(fallback, {
        status: 404,
        headers: { "content-type": "text/html; charset=utf-8" },
      })
    }
    return new Response("Not found", { status: 404 })
  },
})

console.log(`Preview: http://localhost:${server.port}${basePath}`)
