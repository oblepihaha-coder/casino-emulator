<script setup>
import background from '@/assets/about.jpg'
</script>

<template>
  <header
    data-aos="zoom-in"
    data-aos-duration="2000"
    class="about-header"
    :style="{
      backgroundImage: `linear-gradient(rgba(0,0,0,.55), rgba(0,0,0,.55)), url(${background})`
    }"
  >
  </header>

  <section class="p-5 text-center">
    <div>
        <h1>Сторінка про автора</h1>
            <p class="mt-3 fs-4">
                Я Соколовська Анастасія, студентка групи 3РП-10 ОТФК ОНТУ.
            </p>

            <p class="fs-4">
                Цей проект я створила з метою відчутття азарту без ризиків та перевірки своєї удачі. 
                Сподіваюсь, що кожен має гарну карму та зможе насолодитися маленькими перемогами
            </p>
            </div>
  </section>
</template>

<style scoped>
.about-header {
  position: relative;
  min-height: 50vh;
  background-size: cover;
  background-position: center;
  border: 4px solid gold;
  border-radius: 10px;
  box-shadow: 0 0 20px gold;
}


.about-header h1 {
  color: gold;
  font-size: 56px;
}
</style>
