// proxy.config.mjs
export default {
  "/api/**": {
    target: "https://localhost:7237",
    secure: false,
    changeOrigin: true
  }
};