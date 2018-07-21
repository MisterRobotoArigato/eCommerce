# Mister Roboto Arigato Store
Code Fellows 401 C#/ASP.NET course e-commerce project.

**Authors**: Jackie L, Earl Jay Caoile <br />
**Version**: 1.0.0

## Overview
This project is a full stack web application that serves as an e-commerce website. 
Users can register and login to an account. They will eventually be able to add items to 
a cart and purchase them. Admins can access an admin portal that allows them to perform 
full CRUD operations on all products in the database.

## Getting Started
The following is required to run the program.
1. Visual Studio 2017 
2. The .NET desktop development workload enabled
3. ASP.NET NuGet packages

## Architecture
This application is created using ASP.NET Core 2.1 Web Application <br />
*Languages*: C#, HTML, CSS <br />
*Tools*: Azure, Visual Studio Team Services, SQL Database, Bootstrap <br />
*Type of Application*: Web Application <br />

## Walk Through

_Landing Page_

![MRA Store Visual 1](Screenshots/MRA-SS1.jpg)

_Register An Account_

From here, anyone can register for an account.

![Register Account](Screenshots/register.png)

_Non Admin/Regular Member View_

![Registered Member View](Screenshots/memberView.png)

_Member Shop_

Clicking _Shop_ in the navbar will take members to the shop.

![Member Shop](Screenshots/shop.png)

_Shop Details_

In the shop, clicking on the _Details_ link will take users 
to the product landing page where more details are shown.

![Product Details](Screenshots/details.png)

_Admin Portal_

Any member who has an email with "doge" in it, will be an admin.

![MRA Store Visual 2](Screenshots/MRA-SS2.jpg)

_Admin Dashboard_

Admins can view the entire inventory or add a product.
![Admin Dashboard](Screenshots/adminDashboard.png)

_Admin View All_

From View All, admins can edit the product description or delete the product.
![Admin View All](Screenshots/AdminViewAll.png)

_Admin Edit Product Info_

Let's edit a product info.
![Admin Edit View](Screenshots/adminEdit.png)

_Admin Edit Results_

The new edits will appear on the view all page.
![Admin Edit Result](Screenshots/adminEditAfter.png)

_Admin Delete_

Another option is to delete a product from inventory. 
Let's delete the bee.

![Admin Delete](Screenshots/deleteBee.png)

_Admin Delete Results_

The bee is now gone!

![Delete Results](Screenshots/deleteBeeAfter.png)

_Admin Add A Product_

From the admin dashboard, admins can add a product. 
Let's add the bee that was deleted.

![Add a Product](Screenshots/addProduct.png)

_Admin Add Product Result_

The newly added bee is back on the inventory list!

![Added Product](Screenshots/addProductAfter.png)