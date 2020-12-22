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

To-do list:
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

> Here, you instruct other people on how to use your project after they’ve installed it. This would also be a good place to include screenshots of your project in action.

## License

This project uses the following license: [Apache License 2.0](https://github.com/2011-nov02-net/danielp-project1/blob/master/LICENSE).

