const svg = `<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 32 32"><rect width="32" height="32" rx="8" fill="oklch(0.5975 0.2188 262.059)"/><path d="M9 23V9h4.2l2.8 5.6L18.8 9H23v14h-3.2v-8.6L17.3 20h-2.6l-2.5-5.6V23H9Z" fill="#fff"/></svg>`

/** Inline SVG favicon so the site ships no extra asset. */
export const faviconHref = `data:image/svg+xml,${encodeURIComponent(svg)}`
