import type { Config } from "@react-router/dev/config"

// GitHub Pages serves the site from https://<owner>.github.io/<repo>/.
// Override BASE_PATH to deploy under a custom domain or path.
const basePath = process.env.BASE_PATH ?? "/manta/"

export default {
  basename: basePath,
  ssr: false,
  prerender: true,
} satisfies Config
