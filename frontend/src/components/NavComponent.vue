<script>
import { useAuthStore } from '@/stores/auth'

export default {
  name: 'NavComponent',
  data() {
    return {
      authStore: useAuthStore(),
    }
  },
  computed: {
    isAuthenticated() {
      return this.authStore.isAuthenticated
    },
    loginLink() {
      return { path: '/login', query: { redirect: this.$route.fullPath } }
    },
    registerLink() {
      return { path: '/register', query: { redirect: this.$route.fullPath } }
    },
  },
  methods: {
    logout() {
      this.authStore.logout()
      this.$router.push('/home')
    },
  },
}
</script>

<template>
  <div data-aos="flip-down" data-aos-duration="2000" class="container text-center">
    <nav class="navbar navbar-expand-sm m-3">
      <router-link class="navbar-brand d-flex flex-row" to="/home">
        <i class="fa-solid fa-coins fa-beat me-3"></i>
        <h4 class="font-monospace ">Емулятор казино</h4>
      </router-link>
      <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
      <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse" id="navbarSupportedContent">
        <ul class="navbar-nav mx-auto mb-2 mb-lg-0">
          <li class="nav-item">
            <router-link class="nav-link" aria-current="page" to="/"><i class="fa-solid fa-house me-1"></i>Home</router-link>
          </li>
          <li v-if="isAuthenticated" class="nav-item">
            <router-link class="nav-link" aria-current="page" to="/main"><i class="fa-solid fa-sack-dollar"></i>Casino</router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link" aria-current="page" to="/about"><i class="fa-solid fa-info-circle me-1"></i>Developer</router-link>
          </li>
          <li v-if="!isAuthenticated" class="nav-item">
            <router-link class="nav-link" :to="loginLink"><i class="fa-solid fa-right-to-bracket me-1"></i>Login</router-link>
          </li>
          <li v-if="!isAuthenticated" class="nav-item">
            <router-link class="nav-link" :to="registerLink"><i class="fa-solid fa-user-plus me-1"></i>Join now</router-link>
          </li>
          <li v-if="isAuthenticated" class="nav-item">
            <button class="nav-link" type="button" @click="logout">
              <i class="fa-solid fa-right-from-bracket me-1"></i>Logout
            </button>
          </li>
        </ul>
      </div>
    </nav>
  </div>
</template>

<style scoped>
.navbar-brand,
.nav-link {
  color: gold;
  text-shadow: 0 0 6px rgba(255, 215, 0, 0.6);
}

.nav-link {
  display: flex;
  align-items: center;
  gap: 5px;
}

.nav-link i {
  color: gold;
}

.navbar {
  align-items: center;
}
</style>
