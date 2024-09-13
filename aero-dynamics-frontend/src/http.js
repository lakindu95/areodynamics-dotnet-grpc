import axios from 'axios';

// Should load with env
const apiClient = axios.create({
  baseURL: 'http://localhost:5000/v1',
  withCredentials: false,
  headers: {
    'Content-Type': 'application/json'
  }
});

export default apiClient;
