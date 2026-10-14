import { reactRouter } from "@react-router/dev/vite"
import tailwindcss from "@tailwindcss/vite"
import { defineConfig } from "vite"

// Must match the React Router `basename` in react-router.config.ts so the
// generated asset URLs also resolve under the GitHub Pages project path.
const basePath = process.env.BASE_PATH ?? "/manta/"

export default defineConfig({
  base: basePath,
  resolve: { tsconfigPaths: true },
  plugins: [tailwindcss(), reactRouter()],
})
