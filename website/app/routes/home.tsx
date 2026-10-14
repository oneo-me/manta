import { Landing } from "~/components/landing"
import { content } from "~/lib/content"

import type { Route } from "./+types/home"

export const meta: Route.MetaFunction = () => [
  { title: content.en.meta.title },
  { name: "description", content: content.en.meta.description },
  { property: "og:title", content: content.en.meta.title },
  { property: "og:description", content: content.en.meta.description },
  { property: "og:type", content: "website" },
  { name: "twitter:card", content: "summary_large_image" },
]

export default function Home() {
  return <Landing locale="en" />
}
