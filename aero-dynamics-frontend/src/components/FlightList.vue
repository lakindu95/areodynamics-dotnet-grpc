<template>
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

  <div>
    <h2>Flight List</h2>
    <table>
      <thead>
        <tr>
          <th>Flight ID</th>
          <th>Aircraft Registration No</th>
          <th>Destination</th>
          <th>Number of passengers</th>
          <th>Flight Cost</th>
          <th>Note</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="flight in flights.items" :key="flight.flightId">
          <td>{{ flight.flightId }}</td>
          <td>{{ flight.aircraftRegistrationNo }}</td>
          <td>{{ flight.destination }}</td>
          <td>{{ flight.numberOfPassengers }}</td>
          <td>{{ formatCurrency(flight.flightCost) }}</td>
          <td>{{ flight.note }}</td>
          <td>
            <router-link :to="{ name: 'EditFlight', params: { flightId: flight.flightId } }">Edit</router-link>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import apiClient from '../http';

export default {
  data() {
    return {
      flights: []
    };
  },
  methods: {
    async fetchFlights() {
      try {
        const response = await apiClient.get('/flight/getflights');
        this.flights = response.data;

      } catch (error) {
        console.error('Error fetching flights:', error);
      }
    },
    formatCurrency(value) {
      if (value != null) {
        return parseFloat(value).toFixed(2);
      }
      return '';
    }
  },
  mounted() {
    this.fetchFlights();
  }
};
</script>
<style scoped>
/* Styling for the heading */
header {
  display: flex;
  justify-content: center;
  align-items: center;
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

h2 {
  font-size: 20px;
  margin-top: 20px;
  margin-bottom: 10px;
  color: #333;
}

/* Styling for the table */
table {
  width: 100%;
  border-collapse: collapse;
  margin-bottom: 20px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  background-color: #fff;
}

/* Styling for table headers */
th,
td {
  padding: 12px;
  text-align: left;
  border-bottom: 1px solid #ddd;
}

th {
  background-color: #f0f0f0;
  font-weight: bold;
}

/* Alternate row background color */
tbody tr:nth-child(even) {
  background-color: #f9f9f9;
}

/* Styling for router-link inside table cells */
td>router-link {
  display: inline-block;
  text-decoration: none;
  color: #007bff;
  cursor: pointer;
}

td>router-link:hover {
  text-decoration: underline;
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
