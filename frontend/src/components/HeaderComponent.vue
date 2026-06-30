<script setup>
import { computed } from 'vue'
import { useRoute } from 'vue-router'

import casinoBg from '@/assets/casino-bg.jpg'
import aboutBg from '@/assets/about.jpg'

const route = useRoute()

const background = computed(() => {
  return route.path === '/about' ? aboutBg : casinoBg
})
</script>

<template>
  <header
    data-aos="zoom-in"
    data-aos-duration="2000"
    class="hero text-light text-center"
    :style="{
  backgroundImage: `linear-gradient(rgba(0,0,0,.55), rgba(0,0,0,.55)), url(${background})`
}"
  >
    <div class="hero-center">
      <h1 class="fw-bold display-4 text-warning text-center glow">
        NastikSlots777
        <br>
        Емулятор-казино
      </h1>
    </div>

    <div class="hero-bottom text-center">
      <h2 mt-5>Один крок до мільйону</h2>
      <h2 mt-5>Ласкаво просимо до світу розваг</h2>
    </div>
  </header>
</template>

<style scoped>
.hero {
  position: relative;
  min-height: 50vh;
  background-size: cover;
  background-position: center;
  border: 4px solid gold;
  border-radius: 10px;
  box-shadow: 0 0 20px gold;
}

.hero-center {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
}

.hero-bottom {
  position: absolute;
  bottom: 40px;
  width: 100%;
}
.glow {
  text-shadow:
    0 0 10px gold,
    0 0 25px gold,
    0 0 40px orange;
}

.hero-center h1 {
  white-space: nowrap;
}


</style>
