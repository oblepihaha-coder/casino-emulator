import { defineStore } from 'pinia'
import { http } from '@/interceptors/http'

const TOKEN_KEY = 'casino_token'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: localStorage.getItem(TOKEN_KEY) || '',
    user: null,
    initialized: false,
  }),

  getters: {
    isAuthenticated: (state) => Boolean(state.token),
  },

  actions: {
    setToken(token) {
      this.token = token || ''

      if (this.token) {
        localStorage.setItem(TOKEN_KEY, this.token)
      } else {
        localStorage.removeItem(TOKEN_KEY)
      }
    },

    async initialize() {
      if (!this.token) {
        this.initialized = true
        return
      }

      try {
        await this.fetchMe()
      } catch {
        this.logout()
      } finally {
        this.initialized = true
      }
    },

    async register({ name, password }) {
      await http.post('/auth/register', { name, password })

      await this.login({ name, password })
    },

    async login({ name, password }) {
      const { data } = await http.post('/auth/login', { name, password })

      this.setToken(data.access_token)
      await this.fetchMe()
    },

    async fetchMe() {
      const { data: profile } = await http.get('/auth/me')
      this.user = profile
    },

    logout() {
      this.setToken('')
      this.user = null
      this.initialized = true
    },
  },
})