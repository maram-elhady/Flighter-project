# ✈️ Flighter – Gradution Project

  A teamwork graduation project developed by [Maram Ayman](https://github.com/maram-elhady) and [Hajar Mahmoud](https://github.com/HajarSalem)
  
**Flighter** is a full-stack flight booking system featuring a modern admin dashboard and a mobile booking experience. Built with Blazor WebAssembly and a .NET API, the system supports multi-role access, real-time booking validation, and admin control features.

---

## 📌 Features

- 🧑‍✈️ **Admin Dashboard** (Blazor WASM)
  ✨ Key Features of the Flighter Dashboard:
  🔐 Secure Cookie-Based Authentication with role-based access (Owner / Company Admin)
  🧩 Modular Dashboard Design with real-time updates, no page reloads
  🛫 Flight Management: Add, update, and delete tickets with multi-step forms
  📊 Live Booking Overview: Auto-refresh bookings every few seconds
  👥 User Overview: View and manage registered users

👑 Owner Privileges:
-Add or remove Company Admins
-Full visibility over tickets, bookings, and users
-High-level control with secure, scoped permissions

🏢 Admin (Company) Privileges:
-Add new flight tickets with detailed info and seats
-Delete or manage own tickets only
-View bookings and track seat availability
-See ticket performance with offer-based filters 

- 📱 **Mobile App Integration**
  - Built for users to search, view, and book flights
  - Connects securely to backend APIs

- 🔐 **Authentication & Authorization**
  - JWT-based authentication
  - Role-based access for Owner, Admin, and User

- 💺 **Seat Reservation Logic**
  - Prevents multiple users from booking the same seat at the same time
  - Supports more than one seat(ticket) per booking

- 📧 **Email Features**
  - Email verification during registration
  - Password reset functionality
  - 
- 💳 **Flexible Payment Options**
  - Users can choose to **Pay Now** or **Pay Later**
  - **Pay Now**: Users have **2 minutes** to complete payment before the ticket is released (for testing)
  - **Pay Later**: Users have **5 days** to confirm payment, shortened to **2–3 minutes** for testing
  - Automatically clears expired unpaid reservations

## ⚙️ Setup Instructions
1. Clone the repository
2. Fill in appsettings.json Your:
    Connection string,JWT secret/key,SMTP credentials
3. Run the projects
---

## 🎥 Live Demo
http://hmy.runasp.net/swagger/index.html [API Project]
[🎥 Watch the Blazor dashboard demo on LinkedIn](https://www.linkedin.com/posts/maram-ayman-15b74824a_blazor-dotnet-csharp-activity-7343305824762494980-_qvX?utm_source=share&utm_medium=member_desktop&rcm=ACoAAD2wrtcBX6_pIW_tsPVsa0zmm4wgko4Q_9U)
   This video shows how the admin dashboard works in the Flighter system, built using Blazor WebAssembly and .NET API.

