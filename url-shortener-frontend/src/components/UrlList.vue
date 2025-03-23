<script setup>
import { ref, onMounted } from 'vue';
import axios from 'axios';

const urls = ref([]);
const isLoading = ref(false);
const baseUrl = "http://localhost:5043"; // Update according to your backend

const fetchUrls = async () => {
  isLoading.value = true;
  try {
    const response = await axios.get(`${baseUrl}/list`);
    urls.value = response.data.map(url => ({
      ...url,
      fullShortUrl: `${baseUrl}/${url.shortUrl}`
    }));
  } catch (err) {
    console.error("❌ Error while fetching URL list:", err);
  } finally {
    isLoading.value = false;
  }
};

// Truncate long URLs for display
const truncateUrl = (url, maxLength = 50) => {
  return url.length > maxLength ? url.slice(0, maxLength) + '...' : url;
};

// Fetch URLs when the page loads
onMounted(fetchUrls);
</script>

<template>
  <div class="url-list">
    <h2>List of Shortened URLs</h2>

    <!-- Reload Button -->
    <button @click="fetchUrls" :disabled="isLoading">
      <span v-if="isLoading">⏳ Loading...</span>
      <span v-else>🔄 Refresh List</span>
    </button>

    <!-- Display list -->
    <ul v-if="urls.length">
      <li v-for="url in urls" :key="url.shortUrl">
        <p><strong>🔗 Original URL:</strong> 
          <a 
            :href="url.originalUrl" 
            target="_blank" 
            :title="url.originalUrl" 
            class="truncate-url"
          >
            {{ truncateUrl(url.originalUrl) }}
          </a>
        </p>
        <p><strong>🔗 Shortened URL:</strong> 
          <a :href="url.fullShortUrl" target="_blank">{{ url.fullShortUrl }}</a>
        </p>
      </li>
    </ul>

    <!-- Empty state -->
    <p v-else>📭 No shortened URLs found.</p>
  </div>
</template>

<style scoped>
.url-list {
  text-align: center;
  margin: 20px auto;
  max-width: 600px;
  padding: 20px;
  background: #f9f9f9;
  border-radius: 8px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

h2 {
  color: #333;
}

button {
  margin-bottom: 15px;
  padding: 10px 16px;
  cursor: pointer;
  background-color: #007bff;
  color: white;
  border: none;
  border-radius: 5px;
  font-size: 16px;
  transition: 0.3s;
}

button:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}

button:hover:not(:disabled) {
  background-color: #0056b3;
}

ul {
  list-style-type: none;
  padding: 0;
}

li {
  margin-bottom: 12px;
  padding: 12px;
  background: white;
  border-radius: 6px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  transition: 0.3s;
}

li:hover {
  transform: translateY(-2px);
}

a {
  color: #007bff;
  text-decoration: none;
  font-weight: bold;
}

a:hover {
  text-decoration: underline;
}

/* Truncate long URLs visually */
.truncate-url {
  display: inline-block;
  max-width: 100%;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  vertical-align: bottom;
}
</style>
