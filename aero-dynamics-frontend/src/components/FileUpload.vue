<template>
  <div class="container">
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
    <div class="container-body">
      <div class="upload-section">
        <h2>Upload Flight Data</h2>
        <div class="file-upload">
          <label for="file-upload" class="custom-file-upload">
            <input id="file-upload" type="file" @change="handleFileUpload" />
            Choose file
          </label>
          <p v-if="errorMessage" class="error-message">{{ errorMessage }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import apiClient from '../http'; // Make sure to import your Axios instance

export default {
  name: 'FileUpload',
  data() {
    return {
      errorMessage: '' // Initialize as an empty string
    };
  },
  methods: {
    handleFileUpload(event) {
      const file = event.target.files[0];
      if (!file) return;

      const formData = new FormData();
      formData.append('file', file);

      // Send file to the backend API
      apiClient.post('/flight/upload', formData, {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      })
        .then(response => {
          console.log('File uploaded successfully', response);
          // Handle successful upload response
          this.$router.push('/flights');
        })
        .catch(error => {
          console.error('Error uploading file:', error);
          if (error.response) {
            this.errorMessage = `Error: Issue with the file upload`;
          } else if (error.request) {
            this.errorMessage = 'Error: No response from the server.';
          } else {
            this.errorMessage = `Error: Issue with the CSV file`;
          }
        });
    }
  }
};
</script>

<style scoped>
header {
  display: flex;
  justify-content: center;
  align-items: center;
}

.logo {
  height: 100px;
  margin-right: 20px;
}

/* Overall container styles */
.container-body {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
}

/* Styling for the heading */
h1 {
  font-size: 36px;
  margin-bottom: 20px;
  color: #333;
  text-align: center;
}

/* Navigation styles */
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

/* Styling for upload section */
.upload-section {
  background-color: #f9f9f9;
  padding: 20px;
  border: 1px solid #ccc;
  border-radius: 4px;
  margin-top: 20px;
}

h2 {
  font-size: 24px;
  margin-bottom: 15px;
  color: #333;
}

.file-upload {
  text-align: center;
}

.custom-file-upload {
  cursor: pointer;
  display: inline-block;
  padding: 10px 20px;
  background-color: #007bff;
  color: white;
  border: none;
  border-radius: 4px;
  transition: background-color 0.3s ease;
}

.custom-file-upload:hover {
  background-color: #0056b3;
}

input[type="file"] {
  display: none;
}

/* Styling for error message */
.error-message {
  color: red;
  margin-top: 10px;
  font-weight: bold;
}
</style>
