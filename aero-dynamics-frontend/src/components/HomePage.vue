<template>
  <div class="container">
    <header>
      <img src="../assets/logo.png" alt="Areo Dynamics Logo" class="logo" />
      <h1>Areo Dynamics</h1>
    </header>
    <nav>
      <ul>
        <li><router-link to="/upload">Upload Flight Data</router-link></li>
        <li><router-link to="/flights">View Flights</router-link></li>
      </ul>
    </nav>
    <main>
      <section class="welcome-section">
        <h2>Welcome to Areo Dynamics</h2>
        <p>Your go-to platform for managing and viewing flight data seamlessly.</p>
      </section>
      <section class="cards-section">
        <div class="card">
          <router-link to="/upload">
            <h3>Upload Data</h3>
            <p>Upload your flight data quickly and easily.</p>
          </router-link>
        </div>
        <div class="card">
          <router-link to="/flights">
            <h3>View Flights</h3>
            <p>Access detailed information about all flights.</p>
          </router-link>
        </div>
        <div class="card">
          <router-link to="/flights">
            <h3>Manage Flights</h3>
            <p>Update and manage your flight information efficiently.</p>
          </router-link>
        </div>
      </section>
      <router-view></router-view>
    </main>
  </div>
</template>

<script>
export default {
  name: 'HomePage'
};
</script>

<style scoped>
.container {
  display: flex;
  flex-direction: column;
  min-height: 100vh; /* Ensure the container takes up the full viewport height */
}
a {
    text-decoration: none;
}

header {
  display: flex;
  justify-content: center; /* Horizontally center content */
  align-items: center; /* Vertically center content */
}
.logo {
  height: 100px; /* Adjust the height to 100px */
  margin-right: 20px;
}

h1 {
  font-size: 36px;
  margin-bottom: 20px;
  color: #333;
  text-align: center;
}

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

main {
  flex: 1; /* This will allow the main content to expand and fill the remaining space */
  padding: 20px;
  background-color: #fff;
}

/* Welcome Section */
.welcome-section {
  text-align: center;
  margin-bottom: 40px;
}

.welcome-section h2 {
  font-size: 28px;
  color: #333;
  margin-bottom: 10px;
}

.welcome-section p {
  font-size: 16px;
  color: #666;
}

/* Cards Section */
.cards-section {
  display: flex;
  justify-content: space-around;
  margin-bottom: 40px;
}

.card {
  background-color: #f9f9f9;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  width: 30%;
  text-align: center;
}

.card h3 {
  font-size: 20px;
  color: #333;
  margin-bottom: 10px;
}

.card p {
  font-size: 14px;
  color: #666;
}
</style>
