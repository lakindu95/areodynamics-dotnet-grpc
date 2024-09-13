<template>
  <div>
    <header>
      <img src="../assets/logo.png" alt="Areo Dynamics Logo" class="logo" />
      <h1>Areo Dynamics</h1>
    </header>
    <nav>
      <ul>
        <li><router-link to="/">Home</router-link></li>
        <li><router-link to="/upload">Upload Flight Data</router-link></li>
        <li><router-link to="/flights">View Flights</router-link></li>
      </ul>
    </nav>
    <br>
    <div class="container">
      <h2>Edit Flight</h2>

      <form @submit.prevent="updateFlight" class="flight-form">
        <label>Number of Passengers:</label>
        <input type="number" v-model.number="flight.numberOfPassengers" @input="validatePassengers" required />
        <span v-if="passengersError" class="error">{{ passengersError }}</span>

        <label>Note:</label>
        <textarea v-model="flight.note"></textarea>
        <button type="submit">Save</button>
      </form>
      <br>
      <router-link to="/flights">Back to Flights</router-link>
    </div>
  </div>
</template>

<script>
import apiClient from '../http';

export default {
  data() {
    return {
      flight: {
        numberOfPassengers: 0,
        note: '',
        cost: 0 // Assuming flight cost will be fetched from API
      },
      passengersError: ''
    };
  },
  computed: {
    formattedCost() {
      // Format cost to have exactly 2 decimal places
      return parseFloat(this.flight.cost).toFixed(2);
    }
  },
  methods: {
    async fetchFlight() {
      try {
        const response = await apiClient.get(`/flight/getflight/${this.$route.params.flightId}`);
        this.flight = response.data;
      } catch (error) {
        console.error('Error fetching flight:', error);
      }
    },
    async validatePassengers() {
      if (this.flight.numberOfPassengers < 0) {
        this.passengersError = 'Number of passengers must be a positive number';
      } else {
        this.passengersError = '';
      }
    },
    async updateFlight() {
      if (this.passengersError) {
        return;
      }
      try {
        const { flightId, numberOfPassengers, note } = this.flight;
        const requestBody = {
          flightId,
          numberOfPassengers,
          note
        };
        await apiClient.put(`/flight/updateflight/${flightId}`, requestBody);
        // Redirect to flights page after successful update
        this.$router.push('/flights');
      } catch (error) {
        console.error('Error updating flight:', error);
      }
    }
  },
  mounted() {
    this.fetchFlight();
  }
};
</script>

<style scoped>
header {
  display: flex;
  justify-content: center;
  /* Horizontally center content */
  align-items: center;
  /* Vertically center content */
}

.logo {
  height: 100px;
  margin-right: 20px;
}

h1 {
  font-size: 36px;
  margin-bottom: 20px;
  color: #333;
  text-align: center;
}

.container {
  max-width: 600px;
  margin: 0 auto;
  padding: 20px;
  border: 1px solid #ccc;
  border-radius: 8px;
  background-color: #fff;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
}

.flight-info {
  margin-bottom: 20px;
}

.flight-info label {
  font-weight: bold;
}

.flight-form label {
  display: block;
  margin-bottom: 5px;
  font-weight: bold;
}

.flight-form input,
.flight-form textarea {
  width: 100%;
  margin-bottom: 10px;
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 4px;
  box-sizing: border-box;
}

button {
  padding: 10px 20px;
  background-color: #007bff;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

button:hover {
  background-color: #0056b3;
}

router-link {
  display: inline-block;
  margin-top: 20px;
  color: #007bff;
  text-decoration: none;
}

router-link:hover {
  text-decoration: underline;
}

.error {
  color: red;
  font-size: 14px;
}

/* Styling for navigation */
nav {
  background-color: #f0f0f0;
  padding: 10px 0;
  border-bottom: 1px solid #ccc;
}

nav ul {
  list-style-type: none;
  padding: 0;
}

nav ul li {
  display: inline-block;
  margin-right: 10px;
}

nav ul li a {
  text-decoration: none;
  color: #555;
  padding: 5px 10px;
  border-radius: 4px;
}

nav ul li a:hover {
  background-color: #ddd;
}
</style>