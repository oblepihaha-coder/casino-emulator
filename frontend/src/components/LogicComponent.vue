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

            history: [],
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

        async loadHistory() {
            try {
                const { data } = await http.get('/casino/history')
                this.history = data
            } catch (e) {
                console.error(e)
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

                await this.loadHistory()
            } catch (e) {
                this.error = getHttpErrorMessage(e, 'Помилка спіна')
            } finally {
                this.isSpinning = false
            }
        },
        async loadBalance() {
            try {
                const { data } = await http.get('/auth/me')
                this.balance = data.balance
            } catch (e) {
                console.error(e)
            }
        }
    },

    mounted() {
        this.loadHistory()
        this.loadBalance()
    }
}
</script>

<template>
        <section class="p-5">
    <div class="container-fluid text-center w-100">

        <h1 class="mb-3">Казино слот</h1>

        <div class="slot-machine">
            <div class="slot-reel" :class="{ spinning: isSpinning }">
            {{ symbols[0] || '❔' }} </div>

            <div class="slot-reel" :class="{ spinning: isSpinning }">
            {{ symbols[1] || '❔' }} </div>

            <div class="slot-reel" :class="{ spinning: isSpinning }">
            {{ symbols[2] || '❔' }} </div>
        </div>
        <div class="game-panel">
        <div class="bet-row mt-3">
        <input
            v-model="bet"
            type="number"
            class="form-control bet-input"
            min="1"
        />

        <button
        class="btn btn-primary"
        @click="spin"
        :disabled="isSpinning"
        >
        {{ isSpinning ? 'Депаємо...' : '🎰 Депнути' }}
        </button>
        </div>

        <div v-if="message" class="alert alert-info mt-3">
        {{ message }}
        </div>

        <div v-if="error" class="alert alert-danger mt-3">
        {{ error }}
        </div>

        <div class="alert alert-success mt-3">
        <div v-if="winAmount > 0">
            +{{ winAmount }} грн 💰
        </div>

        <h3>Баланс: {{ balance }} грн</h3>
        </div>
        </div>
        <div class="history-box mt-4">
  <h3 class="mb-3 fs-1">Історія ставок</h3>


  <div v-if="history.length" class="history-list">
    <div
      v-for="bet in history"
      :key="bet.id"
      class="history-item"
    >
      <div class="date">
        {{ new Date(bet.createdAt).toLocaleString() }}
      </div>

      <div>Ставка: {{ bet.amount }} грн</div>
      <div>Виграш: {{ bet.winAmount }} грн</div>

      <div :class="bet.isWin ? 'win' : 'lose'">
        {{ bet.isWin ? 'Виграш' : 'Програш' }}
      </div>
    </div>
  </div>

  <p v-else class="text-muted">
    Історія ставок порожня.
  </p>
</div>
</div>
    </section>
</template>

<style scoped>
.history-box {
  max-width: 600px;
  margin: 0 auto;
}

.history-list {
  max-height: 300px;   
  overflow-y: auto;    
  padding-right: 5px;
}

.history-item {
  background: rgba(0,0,0,0.6);
  border: 1px solid gold;
  border-radius: 10px;

  padding: 10px;
  margin-bottom: 10px;

  color: white;
}

.history-item .date {
  font-size: 12px;
  opacity: 0.7;
  margin-bottom: 5px;
}

.win {
  color: #00ff88;
  font-weight: bold;
}

.lose {
  color: #ff4d4d;
  font-weight: bold;
}

.slot-machine {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 20px;

  padding: 20px;
  border: 4px solid gold;
  border-radius: 15px;
  width: fit-content;
  max-width: 100%;
  margin: 0 auto;

  background: rgba(0,0,0,0.6);
  box-shadow: 0 0 20px gold;
}

.slot-reel {
  width: 180px;
  height: 180px;

  display: flex;
  align-items: center;
  justify-content: center;

  font-size: 100px;

  border: 2px solid white;
  border-radius: 10px;

  background: rgba(255,255,255,0.1);
}

@keyframes spin {
  0% { transform: translateY(-10px); }
  50% { transform: translateY(10px); }
  100% { transform: translateY(-10px); }
}

.spinning {
  animation: spin 0.2s infinite;
}

.game-panel {
    max-width: 700px;  
    margin: 25px auto;
}

.bet-row {
    display: flex;
    gap: 10px;
}

.bet-row .form-control,
.bet-row .btn {
    flex: 1;
}

.balance {
  color: #00ffe5;
  font-weight: bold;
}

@media (max-width: 768px) {
  .slot-machine {
    gap: 12px;
    padding: 15px;
  }

  .slot-reel {
    width: 140px;
    height: 140px;
    font-size: 75px;
  }
}

@media (max-width: 480px) {
  .slot-machine {
    gap: 8px;
    padding: 10px;
  }

  .slot-reel {
    width: 110px;
    height: 110px;
    font-size: 60px;
  }
}
</style>
