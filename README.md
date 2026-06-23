# MpangazithaBash Online Shopping Store

A full-stack e-commerce web application built with ASP.NET MVC, Entity Framework, and SQL Server.

## 🛍️ About

MpangazithaBash is an online clothing store that allows customers to browse products, add items to cart, and place orders. It includes a full admin dashboard for managing categories, products, and orders.

## ✨ Features

### Customer Side
- 🏠 Home page with image carousel/slider
- 🔍 Search products by name or category
- 📄 Pagination for product listing
- 🖼️ Product detail page with multiple images
- 🛒 Shopping cart with quantity controls (+/-)
- 💳 Checkout with customer details
- 💰 Payment page
- 📦 Order history
- 👤 User registration and login
- 📍 About and Location pages

### Admin Dashboard
- 📁 Categories management (Add, Edit, Delete)
- 📦 Products management (Add, Edit, Delete, Upload Images)
- 📋 Order management with status updates (Pending, Completed, Cancelled)

## 🛠️ Technologies Used

- **Frontend:** HTML, CSS, Bootstrap 3, Font Awesome, JavaScript
- **Backend:** ASP.NET MVC 5 (C#)
- **Database:** SQL Server with Entity Framework 6 (Database First)
- **Architecture:** Generic Repository Pattern with Unit of Work
- **Other:** PagedList.Mvc, Newtonsoft.Json, Session Management

## 📋 Prerequisites

Before running this project make sure you have:
- Visual Studio 2019 or later
- SQL Server 2017 or later
- SQL Server Management Studio (SSMS)
- .NET Framework 4.7.2 or later

## 🚀 Getting Started

### 1. Clone the repository

git clone  https://github.com/MpangazithaBash/MpangazithaBash.git

### 2. Set up the database
- Open SQL Server Management Studio (SSMS)
- Create a new database called `dbMyOnlineShopping`
- Run the following SQL scripts in order:

```sql
-- Categories Table
CREATE TABLE Tbl_Category (
    CategoryId INT IDENTITY NOT NULL PRIMARY KEY,
    CategoryName VARCHAR(MAX) NOT NULL,
    isAlive BIT NULL,
    isDelete BIT NULL
)

-- Products Table
CREATE TABLE Tbl_Product (
    ProductId INT IDENTITY NOT NULL PRIMARY KEY,
    ProductName VARCHAR(MAX) NOT NULL,
    CategoryId INT NOT NULL,
    IsActive BIT NULL,
    IsDelete BIT NULL,
    CreatedDate DATETIME NULL,
    ModifiedDate DATETIME NULL,
    Description NVARCHAR(500) NULL,
    ProductImage VARCHAR(MAX) NULL,
    IsFeatured BIT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(18,2) NULL,
    FOREIGN KEY (CategoryId) REFERENCES Tbl_Category(CategoryId)
)

-- Members Table
CREATE TABLE Tbl_Member (
    MemberId INT IDENTITY NOT NULL PRIMARY KEY,
    FirstName VARCHAR(MAX) NOT NULL,
    LastName VARCHAR(MAX) NOT NULL,
    Email VARCHAR(MAX) NOT NULL,
    Password VARCHAR(MAX) NOT NULL,
    IsActive BIT NULL,
    IsDelete BIT NULL,
    CreatedOn DATETIME NULL,
    ModifiedOn DATETIME NULL
)

-- Cart Status Table
CREATE TABLE Tbl_CartStatus (
    CartStatusId INT IDENTITY NOT NULL PRIMARY KEY,
    CartStatus VARCHAR(MAX) NOT NULL
)

-- Cart Table
CREATE TABLE Tbl_Cart (
    CartId INT IDENTITY NOT NULL PRIMARY KEY,
    ProductId INT NOT NULL,
    MemberId INT NOT NULL,
    CartStatusId INT NOT NULL,
    FOREIGN KEY (ProductId) REFERENCES Tbl_Product(ProductId),
    FOREIGN KEY (MemberId) REFERENCES Tbl_Member(MemberId),
    FOREIGN KEY (CartStatusId) REFERENCES Tbl_CartStatus(CartStatusId)
)

-- Product Images Table
CREATE TABLE Tbl_ProductImages (
    ImageId INT IDENTITY NOT NULL PRIMARY KEY,
    ProductId INT NOT NULL,
    ImagePath VARCHAR(MAX) NOT NULL,
    FOREIGN KEY (ProductId) REFERENCES Tbl_Product(ProductId)
)

-- Cart Status seed data
INSERT INTO Tbl_CartStatus (CartStatus) VALUES ('Pending')
INSERT INTO Tbl_CartStatus (CartStatus) VALUES ('Completed')
INSERT INTO Tbl_CartStatus (CartStatus) VALUES ('Cancelled')

-- Search Stored Procedure
CREATE PROCEDURE GetBySearch
    @search NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.* FROM Tbl_Product p
    LEFT JOIN Tbl_Category c ON p.CategoryId = c.CategoryId
    WHERE
    p.ProductName LIKE CASE WHEN @search IS NOT NULL
        THEN '%' + @search + '%' ELSE p.ProductName END
    OR
    c.CategoryName LIKE CASE WHEN @search IS NOT NULL
        THEN '%' + @search + '%' ELSE c.CategoryName END
END
```

### 3. Update connection string
- Open `Web.config` in the root of the project
- Find the `connectionStrings` section
- Update the connection string to match your SQL Server:

```xml
<connectionStrings>
    <add name="dbMyOnlineShoppingEntities"
         connectionString="metadata=res://*/DAL.dbMyOnlineShopping.csdl|...|...;
         provider=System.Data.SqlClient;
         provider connection string=&quot;
         data source=YOUR_SERVER_NAME;
         initial catalog=dbMyOnlineShopping;
         integrated security=True;
         MultipleActiveResultSets=True;
         App=EntityFramework&quot;"
         providerName="System.Data.EntityClient" />
</connectionStrings>
```

Replace `YOUR_SERVER_NAME` with your SQL Server instance name.

### 4. Create Images folder
- In the root of the project create a folder called `Images`
- This is where product images will be uploaded

### 5. Run the project
- Press `F5` in Visual Studio to run
- Navigate to `/Product/Dashboard` for the admin panel
- Navigate to `/` for the main store

## 📸 Screenshots

### Home Page
![Home Page](Screenshots/home.png)

### Admin Dashboard
![Admin Dashboard](Screenshots/admin.png)

### Product Detail
![Product Detail](Screenshots/product.png)

### Cart
![Cart](Screenshots/cart.png)

## 👤 Admin Access
Navigate to `/Admin/Dashboard` to access the admin panel.

## 🔑 Default Test Credentials
Register a new account on the website to test the checkout flow.

## 📁 Project Structure
MpangazithaBash/

├── Controllers/

│   ├── HomeController.cs      # Main website logic

│   └── ProductController.cs   # Admin dashboard logic

├── DAL/# Entity Framework models

├── Models/

│   └── Home/# View models

├── Repository/# Generic Repository Pattern

├── Views/

│   ├── Home/# Customer views

│   ├── Product/# Admin views

│   └── Shared/# Layout files

├── Images/# Product images

└── Content/# CSS files

## 🎓 About the Developer

Built by **EdisonThobani Mtshiliba (MpangazithaBash)** as a second-year at university of johannesburg, project demonstrating full-stack web development skills using ASP.NET MVC.

- GitHub: [@MpangazithaBash](https://github.com/MpangazithaBash)

## 📄 License

This project is for educational purposes. 