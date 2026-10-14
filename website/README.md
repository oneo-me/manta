# Manta website

The Manta marketing site: a statically prerendered React Router app that is
deployed to GitHub Pages at <https://oneo-me.github.io/manta/>.

- **Framework:** React Router in framework mode with `ssr: false` and full
  prerendering, so every route ships as static HTML.
- **Styling:** Tailwind CSS v4 with the `nova` shadcn/ui preset on Base UI.
- **Content:** English and Simplified Chinese, defined in
  [`app/lib/content.ts`](app/lib/content.ts).

## Requirements

- [Bun](https://bun.sh) 1.4 or newer.

## Commands

```sh
bun install        # install dependencies
bun run dev        # start the dev server at http://localhost:5173/manta/
bun run typecheck  # generate route types and typecheck
bun run build:pages # build the static artifact into build/pages
bun run preview    # serve the built artifact the way GitHub Pages does
bun run format     # format with Prettier
```

The site is served under the `/manta/` base path, so the dev server uses
<http://localhost:5173/manta/> rather than the root.

> `dev` runs the React Router CLI through Bun with the `development`
> condition. Invoking `react-router dev` directly makes its self-restart loop
> under Bun, so keep the explicit invocation.

## Deployment

The [Website workflow](../.github/workflows/website.yml) builds the site and
publishes it with GitHub Pages whenever `website/**` changes on `main`.

Enable it once under **Settings → Pages → Build and deployment → Source →
GitHub Actions**. GitHub Pages is only available for private repositories on
paid plans; the site keeps working unchanged once the repository is public.

## Base path

`BASE_PATH` overrides the deploy path in both `vite.config.ts` and
`react-router.config.ts`. It defaults to `/manta/`, which matches the
`oneo-me/manta` repository. Set it to `/` when serving from a custom domain.

React Router writes prerendered pages under `build/client/manta/`, while Vite
keeps assets at the root of `build/client`. `scripts/prepare-pages.mjs` merges
both into `build/pages` and adds a `404.html` fallback, which is the directory
uploaded to Pages.

## Structure

| Path | Purpose |
| --- | --- |
| `app/root.tsx` | Document shell, theme bootstrap and error boundary. |
| `app/routes.ts` | Route table: `/` and `/zh`. |
| `app/components/landing.tsx` | The landing page sections. |
| `app/lib/content.ts` | English and Simplified Chinese copy and code samples. |
| `app/lib/site.ts` | Shared links and the locale helper. |
| `app/components/ui` | shadcn/ui components. |
| `app/assets/icon.png` | The app icon, used as the header/footer logo and favicon. |

`app/assets/icon.png` is downscaled from `source/icon.png`. Regenerate it rather
than editing it by hand.

Add or update shadcn/ui components with the project's Bun runner:

```sh
bun x --bun shadcn@latest add <component>
```
