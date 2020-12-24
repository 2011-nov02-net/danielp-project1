# Every Store

## Project Description

Every Store is a multi-chain online ordering service application for grocery stores, with inventory management. Order any product from our catalogues from stores located all over the world. This application not only makes the process of ordering groceries online easier, but available from anywhere at anytime.

## Technologies Used

* ASP.NET Core - version  3.1
* Bootstrap - version 4
* XUnit
* Azure SQL database


## Features
* Orders update store stock.
* View order history.
* Create new customer accounts.

#### To-do list:
- [ ] Improve navigation and links between pages
- [ ] Take a second pass on styling the pages
- [ ] Add even more tests.

## Getting Started
 
 ### Online Version
 Simply go to [right here](http://noelwiz.azurewebsites.net/).
 
 ### Running Locally
 1. Git clone
 
 
 `git clone git@github.com:2011-nov02-net/danielp-project1.git`
 
  
 2. set up local database
 install sqlite and create a db somewhere
 
 
 More detailed instructions: https://www.tutorialspoint.com/sqlite/sqlite_create_database.htm
  
 3. create the connection string for the db
 
 
 It should be something like this, but with the path to the db on your disk filled in.` Data Source=c:\mydb.db;Version=3; `
 
 
 more details here: https://www.connectionstrings.com/sqlite-net-provider/basic/
 
 
 
 4. set enviorment variables or user secret
 unix:
 `export SqlServer=Your connection string here`
 
 or
 
 windows: 
 Follow this guide to set up the enviorment variable: https://support.shotgunsoftware.com/hc/en-us/articles/114094235653-Setting-global-environment-variables-on-Windows
 
 Additionally, if you want to run the command line version, then create a file called "connectionstring.txt" and put your connection string in it.
 Put the file in /MyStore/MyStore.DataModel/

 5. Use the SQL in the SQL folder to set up the database and fill it with sample data

## Usage

### Console
When you first start the console app, you should get something that looks like this.
In general, all commands are case insensitive, although names are case sensitive. You can type either the letter in { } or the whole word to select an option. 

![console page startup menu](https://github.com/2011-nov02-net/danielp-project1/blob/master/images/console_startup.png)

When you have the console app running, you then have to choose a customer, or create a new one. Bellow is an example of logging in as a customer, which is done by typing their name, however you can also create a new customer and you will be automatically logged in as that customer.

![Login to a console](https://github.com/2011-nov02-net/danielp-project1/blob/master/images/console_login.png)

Once logged in, you can choose a store, and place an order for some items in stock there. It will ask you if you want to see what items are in stock so you don't have to remember the names of anything. You can also remove itmes you've added from the order and re-add them to change the quantity.

![Placing an order for some field guides](https://github.com/2011-nov02-net/danielp-project1/blob/master/images/console_placeorder.png)

After the order has been placed, you can then view the store's order history, or your own order history and see that the new order appears in it.

![Checking that the order was placed](https://github.com/2011-nov02-net/danielp-project1/blob/master/images/console_vieworders.png)

### Web App

## License

This project uses the following license: [Apache License 2.0](https://github.com/2011-nov02-net/danielp-project1/blob/master/LICENSE).

