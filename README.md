# Hospital Management System Web API

## Description
The **Hospital Management System Web API** is a backend service designed to manage hospital operations, including patient records, doctor schedules, appointments, and medical records. This API provides essential functionalities to streamline hospital workflow and ensure efficient data management.

## Features
- Patient registration and management
- Doctor profile management
- Appointment scheduling
- Medical records storage and retrieval
- Role-based authentication and authorization
- API endpoints for CRUD operations

## Tech Stack
- **Backend:** .NET Core Web API (C#)
- **Database:** SQL Server (Entity Framework Core - Code-First Approach)
- **Authentication:** JWT (JSON Web Token)
- **Other Tools:** Swagger for API documentation, Postman for testing

## Installation
### Prerequisites
- .NET SDK installed ([Download Here](https://dotnet.microsoft.com/en-us/download))
- SQL Server installed or running in Docker
- Postman (optional for API testing)

### Steps
1. Clone the repository:
   ```bash
   git clone https://github.com/MohammedNafee/Hospital-Management-System_Web-API.git
   ```
2. Navigate to the project directory:
   ```bash
   cd Hospital-Management-System_Web-API
   ```
3. Install dependencies:
   ```bash
   dotnet restore
   ```
4. Set up the database:
   ```bash
   dotnet ef database update
   ```
5. Run the application:
   ```bash
   dotnet run
   ```
6. Open the API in Swagger:
   - Navigate to `http://localhost:5000/swagger`

## Usage
### Example Endpoints
- **Get all patients**: `GET /api/patients`
- **Get a patient by ID**: `GET /api/patients/{id}`
- **Add a new patient**: `POST /api/patients`
- **Update patient info**: `PUT /api/patients/{id}`
- **Delete a patient**: `DELETE /api/patients/{id}`

## Contributing
Contributions are welcome! Follow these steps:
1. Fork the repository
2. Create a feature branch (`git checkout -b feature-xyz`)
3. Commit changes (`git commit -m 'Add feature xyz'`)
4. Push to the branch (`git push origin feature-xyz`)
5. Open a Pull Request

## License
This project is licensed under the MIT License.

---
Feel free to update this README with additional details or modifications!

