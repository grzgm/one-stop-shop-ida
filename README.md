# “One Stop Shop” iDA workplace management application – Frontend

**This branch is only for Frontend Source Code, for general project description go to [main branch](https://github.com/grzgm/one-stop-shop-ida)**

A workplace management application designed to streamline iDA employees' tasks and enhance collaboration.

## Table of Contents

- [“One Stop Shop” iDA workplace management application – Frontend](#one-stop-shop-ida-workplace-management-application--frontend)
  - [Table of Contents](#table-of-contents)
  - [About the Project](#about-the-project)
  - [Features](#features)
  - [Technologies Used](#technologies-used)
  - [Software Architecture](#software-architecture)
  - [Project Structure](#project-structure)
  - [Setup and Installation](#setup-and-installation)
  - [Usage](#usage)
  - [Contributors](#contributors)

## About the Project

**Course:** Internship

**Semester:** 5

The "One Stop Shop" application was developed during the Semester 5 internship program. Its purpose is to provide employees at iDA with a centralized platform to manage various workplace tasks, simplify communication, and enhance collaboration. The application integrates multiple tools and services into one interface, enabling employees to focus on their work without the hassle of switching between different platforms.

## Features

- Integration with Microsoft 365 and Slack
- OAuth2.0 Integration
- Unit and End-to-End Testing
- Push Notifications
- Shortcut to the employee portal (Werknemerloket)
- View balance of off days
- Automatic email notifications for sick leave and return-to-work updates
- Auto-update Slack status for sick leave or return-to-work
- Indicate office presence days
- Reserve desks
- Register for lunch in different offices
- Receive lunch reminders
- Access office details (route, parking, rules, Wi-Fi, etc.)
- Shortcut to the expenses management system

## Technologies Used

- **Programming Languages/Technologies:**
  - React
  - TypeScript
  - HTML
  - CSS
  - Progressive Web App (PWA)
- **Libraries/Frameworks:**
  - React Router
  - Vite
  - Playwright (for testing)

## Software Architecture

The application follows a 3-Tier Architecture to ensure modularity, scalability, and separation of concerns. The architecture consists of the following layers:

1. _Presentation Layer (**PL**)_: Displays infomation requested by user. **Depends on the BLL Interfaces**
2. _Business Logic Layer (**BLL**)_: Contains all the core business rules and application logic. **No dependencies.**
3. _Data Access Layer (**DAL**)_: Manages data operations directly from various data sources. **Depends on the BLL Interfaces**

```
+---------------------------------------+
|           Presentation Layer          |
|---------------------------------------|
|                                       |
|       Display generated content       |
|                                       |
+---------------------------------------+
                    |
                    |   Dependency Inversion
                    ▼
+---------------------------------------+
|         Business Logic Layer          |
|---------------------------------------|
|                                       |
|    Handles core application rules     |
|                                       |
+---------------------------------------+
                    ▲
                    |   Dependency Inversion
                    |
+---------------------------------------+
|          Data Access Layer            |
|---------------------------------------|
|                                       |
|            Data management            |
|                                       |
+---------------------------------------+
```

## Project Structure

```
one-stop-shop-ida/
│
├── src/                # PL Source Code
│   ├── api/            # Backend API calls
│   ├── components/     # React components
│   ├── contexts/       # React contexts
│   ├── css/            # CSS style sheets
│   ├── misc/           # Helper functions
│   └── routes/         # React router configruation
│
├── public/             # PWA assets
│
├── .env.local          # Development Local Environment Variables
└── README.md           # Frontend project description
```

## Setup and Installation

Follow the instructions below to set up the project on your local machine.

1. **Clone the Repository**

   ```bash
   git clone https://github.com/grzgm/one-stop-shop-ida.git
   cd one-stop-shop-ida
   ```

   checkout correct branch.

2. **Create Local Environment Variables**

   ```bash
   touch .env.local
   touch .env
   ```

   Add variables:

   - `VITE_GOOGLE_MAPS_API_KEY="your-google-maps-api-key"`,
   - `VITE_VAPID_PUBLIC_KEY = "your-vapid-public-key"`,
   - `VITE_FRONTEND_URI = "your-development-or-production-frontend-uri"`,
   - `VITE_BACKEND_URI = "your-development-or-production-backend-uri"`,

3. **Install Dependencies**

   ```bash
   npm install
   ```

4. **Compile/Build the Project**

   ```bash
   npm run build
   ```

5. **Run the Project**
   ```bash
   npm run dev
   ```

## Usage

- Access the application via the URL provided by the hosting environment.
- Log in using Microsoft 365 iDA account credentials.
- Navigate through the dashboard to:
  - Manage vacations and sick leaves.
  - Register for lunch and check office-specific details (requires Slack log in).
  - Reserve desks and manage your profile.

## Contributors

- [Grzegorz Malisz](https://github.com/grzgm): Author.
