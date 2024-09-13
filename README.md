# Aero Dynamics Project

## Project Overview

The Aerodynamics project manages flight data through a Vue.js frontend and a Dockerized backend API. It includes functionalities for uploading flight data, viewing flights, and editing flight details.

## Getting Started

Follow these instructions to get a copy of the project up and running on your local machine for development purposes.

### Prerequisites

- Node.js and npm (Node Package Manager) installed on your machine.
- .NET 8 installed
- Docker
- Docker Compose

### Technical Overview

* Frontend - Vue3
* Backend - .NET 8

### Quick Start with powershell script

Start all the containers at once

```
./start.ps1
```

Stop all the containers at once

```
./stop.ps1
```

### Running backend or frontend Docker Compose seperately

To run all services using Docker Compose:

AeroDynamics
├── aero-dynamics-backend
│   └── docker-compose.yml
└── aero-dynamics-frontend
    └── docker-compose.yml

```
docker-compose up -d
```

#### Ports

* Frontend - localhost:8085
* Backend - localhost:5000

#### To upload flight details please refer the sample CSV for the format

* Sample Data/SampleCsv.csv
