export const themeStorageKey = "manta-theme"

/**
 * Runs before paint to apply the stored theme, so the prerendered page does
 * not flash the wrong color scheme.
 */
export const themeInitScript = `(function () {
  try {
    var stored = localStorage.getItem("${themeStorageKey}");
    var dark = stored === "dark" ||
      ((!stored || stored === "system") &&
        window.matchMedia("(prefers-color-scheme: dark)").matches);
    document.documentElement.classList.toggle("dark", dark);
    document.documentElement.style.colorScheme = dark ? "dark" : "light";
  } catch (error) {}
})();`
