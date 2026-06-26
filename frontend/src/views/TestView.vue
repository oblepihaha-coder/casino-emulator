<script>
import NavComponent from '@/components/NavComponent.vue'
import HeaderComponent from '@/components/HeaderComponent.vue'
import FooterComponent from '@/components/FooterComponent.vue'

import { http } from '@/interceptors/http'

export default {
    name: 'TestView',
    components: {
        NavComponent,
        HeaderComponent,
        FooterComponent
    },
    // змінні
    data() {
        return {
            test: "Hello World",
            kek: null
        }
    },
    // методи
    methods: {
        clickButton() {
            alert(`You entered: ${this.test}`);
        },
        async getKek() {
            const { data } = await http.get('/kek')
            this.kek = data?.text
        },
    },
    // хук життєвого циклу, що запускає метод при старті компоненту
    async mounted() {
        this.getKek()
    }
}
</script>

<template>
  <NavComponent />
  <HeaderComponent />
    <section class="p-5">
        <div class="container text-center">
            <input class="form-control m-1" id="test-input" v-model="test" placeholder="Введіть текст..." />
            <h1 class="m-1">Ваш текст: {{ test }}</h1>
            <h1 class="m-1">Ваш кек: {{ kek }}</h1>
            <button v-if="test === '123'" class="btn btn-primary w-100 m-1" @click="clickButton()">Натисніть мене</button>
        </div>
    </section>
  <FooterComponent />
</template>

<style scoped></style>
