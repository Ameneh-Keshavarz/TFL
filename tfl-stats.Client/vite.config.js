// tfl-stats.Client/vite.config.js
import { fileURLToPath, URL } from 'node:url'
import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import { env } from 'node:process'   // 👈 important

export default defineConfig(({ command }) => {
    const isDev = command === 'serve'

    // default server (no https in build)
    const server = {
        port: parseInt(env.DEV_SERVER_PORT || '55811', 10),
        https: false
    }

    if (isDev) {
        const target = env.ASPNETCORE_HTTPS_PORT
            ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}`
            : env.ASPNETCORE_URLS
                ? env.ASPNETCORE_URLS.split(';')[0]
                : 'https://localhost:7056'

        server.proxy = {
            '^/weatherforecast': { target, secure: false },
            '^/api': { target, secure: false }
        }
    }

    return {
        base: '/',    
        plugins: [react()],
        resolve: {
            alias: { '@': fileURLToPath(new URL('./src', import.meta.url)) }
        },
        server
    }
})
