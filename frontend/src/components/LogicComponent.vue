<script>
import { getHttpErrorMessage, http } from '@/interceptors/http'

export default {
    name: 'LogicComponent',
    data() {
        return {
        loading: true,
        error: '',
        items: [],

        bet: 100,
        balance: 0,
        symbols: [],
        message: '',
        winAmount: 0,
        isSpinning: false,
        }
    },
    methods: {
        async loadItems() {
            this.loading = true
            this.error = ''

            try {
                const { data } = await http.get('/users')
                this.items = Array.isArray(data) ? data : []
            } catch (e) {
                this.error = getHttpErrorMessage(e, 'Не вдалося завантажити дані.')
            } finally {
                this.loading = false
            }
        },

        async spin() {
    this.error = ''
    this.message = ''
    this.isSpinning = true

    try {
        const { data } = await http.post('/casino/spin', {
            bet: this.bet
        })

        this.symbols = data.symbols
        this.balance = data.balance
        this.message = data.message
        this.winAmount = data.winAmount

    } catch (e) {
        this.error = getHttpErrorMessage(e, 'Ошибка спина')
    } finally {
        this.isSpinning = false
    }
}
    },
    mounted() {
    },
}
</script>

<template>
        <section class="p-5">
    <div class="container text-center">

        <h1 class="mb-4">Казино слот</h1>
        <h3>Баланс: {{ balance }} грн</h3>

        <div class="my-3">
        <input
            v-model="bet"
            type="number"
            class="form-control w-25 mx-auto"
            min="1"
        />
        </div>

        <div class="display-4 my-4">
        {{ symbols[0] || '❔' }}
        {{ symbols[1] || '❔' }}
        {{ symbols[2] || '❔' }}
        </div>

        <button
        class="btn btn-primary"
        @click="spin"
        :disabled="isSpinning"
        >
        {{ isSpinning ? 'Крутимо...' : '🎰 Крутити' }}
        </button>

        <div v-if="message" class="alert alert-info mt-4">
        {{ message }}
        </div>

        <div v-if="error" class="alert alert-danger mt-4">
        {{ error }}
        </div>

        <div v-if="winAmount > 0" class="alert alert-success mt-3">
        +{{ winAmount }} грн 💰
        </div>

    </div>
    </section>
</template>

<style scoped>

</style>
