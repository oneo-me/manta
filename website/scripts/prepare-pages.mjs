// Assembles the GitHub Pages artifact.
//
// React Router writes prerendered pages under `build/client/<basename>/`, while
// Vite keeps hashed assets and public files at the root of `build/client`.
// GitHub Pages maps the artifact root to `/<repo>/`, so both parts are merged
// into a single `build/pages` directory that can be uploaded as-is.

import { cp, mkdir, readdir, rm, writeFile } from "node:fs/promises"
import { existsSync } from "node:fs"
import { join, resolve } from "node:path"

const basePath = (process.env.BASE_PATH ?? "/manta/").replace(
  /^\/+|\/+$/g,
  ""
)
const clientDir = resolve("build/client")
const outputDir = resolve("build/pages")
const prerenderedDir = basePath ? join(clientDir, basePath) : clientDir

if (!existsSync(prerenderedDir)) {
  throw new Error(
    `Missing prerendered output at ${prerenderedDir}. Run the build first.`
  )
}

await rm(outputDir, { recursive: true, force: true })
await mkdir(outputDir, { recursive: true })

// Prerendered pages, for example `index.html` and `zh/index.html`.
await cp(prerenderedDir, outputDir, { recursive: true, dereference: true })

// Hashed assets, `favicon.ico` and any other public file live at the root of
// the client build rather than inside the basename directory.
for (const entry of await readdir(clientDir, { withFileTypes: true })) {
  if (
    (basePath && entry.name === basePath) ||
    entry.name === "index.html" ||
    entry.name === "__spa-fallback.html"
  ) {
    continue
  }
  await cp(join(clientDir, entry.name), join(outputDir, entry.name), {
    recursive: entry.isDirectory(),
    dereference: true,
  })
}

// Serve the SPA fallback for paths that were not prerendered.
const fallback = basePath
  ? join(clientDir, "index.html")
  : join(clientDir, "__spa-fallback.html")
if (existsSync(fallback)) {
  await cp(fallback, join(outputDir, "404.html"))
}

// Let GitHub Pages serve files and directories that start with an underscore.
await writeFile(join(outputDir, ".nojekyll"), "")
