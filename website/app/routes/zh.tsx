import { Landing } from "~/components/landing"
import { content } from "~/lib/content"

import type { Route } from "./+types/zh"

export const meta: Route.MetaFunction = () => [
  { title: content.zh.meta.title },
  { name: "description", content: content.zh.meta.description },
  { property: "og:title", content: content.zh.meta.title },
  { property: "og:description", content: content.zh.meta.description },
  { property: "og:type", content: "website" },
  { name: "twitter:card", content: "summary_large_image" },
]

export default function Zh() {
  return <Landing locale="zh" />
}
