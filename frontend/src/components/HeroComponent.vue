<script setup>

</script>

<template>
    <section class="p-5 text-center">
        <div class="container">
            <h1>Опис</h1>
            <p class="mt-3 fs-4">
                Емулятор казино - це веб-застосунок, розроблений для демонстрації принципів роботи онлайн-казино 
                без використання реальних коштів. 
            </p>

            <p class="fs-4">
                Користувач отримує віртуальний баланс та може взаємодіяти з ігровими автоматами (слотами), роблячи ставки та отримуючи виграші або програші 
                залежно від випадкового результату.
            </p>
    <div class="container mt-5">
        <div class="row text-center">
                    <div class="col">
                    <img src="@/assets/slots.png" class="feature-img" />
                    <h4>Слоти</h4>
                    <p class="fs-5">Випробуйте удачу на ігрових автоматах.</p>
                    </div>

                    <div class="col">
                    <img src="@/assets/balance.png" class="feature-img" />
                    <h4>Баланс</h4>
                    <p class="fs-5">Отримуйте та витрачайте віртуальні кошти.</p>
                    </div>

                    <div class="col">
                    <img src="@/assets/stats.png" class="feature-img" />
                    <h4>Статистика</h4>
                    <p class="fs-5">Переглядайте результати своїх ставок.</p>
                    </div>
          </div>
        </div>            

        </div>
    </section>
</template>

<style scoped>
.feature-img {
  width: 90px;
  height: 90px;
  object-fit: cover;

  border-radius: 12px;
  border: 2px solid gold;
  box-shadow: 0 0 10px gold;

  margin-bottom: 10px;
}


</style>
