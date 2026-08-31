# Online Exam System

An online examination system built with **ASP.NET Core MVC** that provides a complete platform for creating and conducting online exams, managing questions, evaluating answers, and generating certificates.

## ✨ Features

* 👤 User registration and authentication
* 🔐 Role-based access control
* 📝 Question bank management
* ❓ Multiple-choice questions
* ✍️ Essay questions
* 📚 Category-based question management
* 🧪 Exam creation and management
* 👨‍🎓 User enrollment in exams
* ✅ Online exam participation
* 📊 Automatic exam evaluation
* 📈 Exam results
* 🏆 Certificate generation
* 🔑 Change password
* 👥 User management
* 🎯 Different question types
* 📋 Review and check answers

## 🏗️ Architecture

The project follows a layered architecture to separate responsibilities and improve maintainability.

```text
ExamOnline
│
├── Application.Core
│   ├── Domain
│   ├── DTO
│   └── Enums
│
├── Application.Infrastructure
│   ├── DbContext
│   ├── Identity
│   └── ImplementationServices
│
└── ExamOnline
    ├── Controllers
    ├── Views
    ├── wwwroot
    └── Program.cs
```

## 🛠️ Technologies

* **C#**
* **ASP.NET Core MVC**
* **Entity Framework Core**
* **ASP.NET Core Identity**
* **SQL Server**
* **HTML5**
* **CSS3**
* **JavaScript**
* **jQuery**
* **Razor Views**

## 📂 Project Structure

### Application.Core

Contains the core business models and application contracts:

* Domain Entities
* DTOs
* Interfaces
* Enums

### Application.Infrastructure

Responsible for infrastructure and data-access concerns:

* Entity Framework Core
* Database Context
* Identity configuration
* Service implementations
* Identity seeding

### ExamOnline

The main ASP.NET Core MVC application:

* Controllers
* Razor Views
* Authentication
* User interface
* JavaScript
* CSS
* Static files

## 🚀 Getting Started

### Prerequisites

Before running the project, make sure you have:

* .NET SDK
* SQL Server
* Visual Studio 2022 or another compatible IDE

### Installation

Clone the repository:

```bash
git clone https://github.com/mohammadrezabakhshi0930-sys/Online-Exam-System.git
```

Open the solution:

```text
ExamOnline.sln
```

Configure your SQL Server connection string in the application's configuration.

Then create/update the database using Entity Framework Core migrations.

Finally, run the application:

```bash
dotnet run
```

## 🔒 Security

Sensitive information such as database passwords, API keys, and other secrets should not be committed to the repository.

Use environment variables, User Secrets, or another secure configuration mechanism for sensitive settings.

## 📌 Future Improvements

Possible future improvements include:

* Online exam timer improvements
* More advanced reporting
* Question randomization
* Better exam analytics
* RESTful API
* Automated testing
* Docker support

## 👨‍💻 Author

**Mohammad Reza Bakhshi**

GitHub:
https://github.com/mohammadrezabakhshi0930-sys

---

⭐ If you find this project useful, feel free to star the repository.
