# Stage 1: Build the Vue.js application
FROM node:14-alpine AS build
WORKDIR /app

# Copy and install dependencies
COPY package*.json ./
RUN npm install

# Copy the rest of the application code and build the application
COPY . .
RUN npm run build

# Stage 2: Create a lightweight production image
FROM node:14-alpine
WORKDIR /app

# Copy the built files from the build stage
COPY --from=build /app/dist ./dist

# Install a simple HTTP server to serve the static files
RUN npm install -g http-server

# Expose the port the application will run on
EXPOSE 8085

# Command to run the HTTP server
CMD ["http-server", "dist"]
