
--https://github.com/2011-nov02-net/trainer-code/wiki/Project-0-requirements
-- create tables


-- orders table	
	-- order id
	-- customer id
	-- store id
	-- order total
-- order-items table
	-- order id
	-- item id
	-- quantity
-- customers table
	-- first name
	-- middle initial
	-- last name
	-- default store
-- storeLocations table	
	-- location name
-- store invintory table
	-- store id pk
	-- item id pk
	-- quantity
-- item table
	-- item name, pk
	-- item price

--attempt to stop the error from trying to create the schema multiple times
--IF NOT EXISTS (SELECT Store) 
CREATE SCHEMA Store;
GO

DROP TABLE IF EXISTS Store.Invintory;
DROP TABLE IF EXISTS Store.Customers;
DROP TABLE IF EXISTS Store.[Location];
DROP TABLE IF EXISTS Store.Items;

CREATE TABLE Store.Items (
	ItemID int
		NOT NULL IDENTITY(1,1) UNIQUE,
	ItemName NVARCHAR(50)
		NOT NULL,
	ItemPrice MONEY
		NOT NULL,
	CONSTRAINT Item_pk PRIMARY KEY (ItemName)
);

-- CONSIDER adding an id, or merging this trivial table
CREATE TABLE Store.[Location] (
	LocationName NVARCHAR(100)
			NOT NULL PRIMARY KEY
);

CREATE TABLE Store.Invintory (
	StoreLocation NVARCHAR(100)
			NOT NULL,
	ItemName NVARCHAR(50)
		NOT NULL,
	Quantity INT
		NOT NULL,
	-- constraint, quantity >= 0
	CONSTRAINT Inv_CPK PRIMARY KEY (StoreLocation, ItemName),
	CONSTRAINT InvLoc_FK FOREIGN KEY (StoreLocation) REFERENCES Store.[Location] (LocationName),
	CONSTRAINT InvItm_FK FOREIGN KEY (ItemName) REFERENCES Store.Items (ItemName)
);

CREATE TABLE Store.Customers (
	Id INT 
		NOT NULL,
	FirstName NVARCHAR(20)
		NOT NULL,
	LastName NVARCHAR(20)
		NOT NULL,
	MiddleInitial CHAR
		NULL,
	-- DEFAULT STORE
	StoreLocation NVARCHAR(100)
		NULL,
	CONSTRAINT Inv_Name UNIQUE (FirstName, LastName, MiddleInitial),
	CONSTRAINT Cust_PK PRIMARY KEY (Id),
	CONSTRAINT store_FK FOREIGN KEY (StoreLocation) REFERENCES Store.[Location] (LocationName)
);

CREATE TABLE Store.Orders (
	Id	INT
		NOT NULL,
	CustomerID INT
		NOT NULL,
	StoreLocation NVARCHAR(100)
		NOT NULL,
	OrderTotal MONEY
		NOT NULL,
	CONSTRAINT order_PK PRIMARY KEY (Id),
	CONSTRAINT StoreOrders_FK FOREIGN KEY (StoreLocation) REFERENCES Store.[Location] (LocationName),
	CONSTRAINT CustomerOrders_FK FOREIGN KEY (CustomerID) REFERENCES Store.Customers (Id)
);


CREATE TABLE Store.OrderItems (
	OrderID INT
		NOT NULL,
	ItemID NVARCHAR(50)
		NOT NULL,
	Quantity INT
		NOT NULL,
	CONSTRAINT ori_PK PRIMARY KEY (OrderId, ItemID),
	CONSTRAINT OrderItemOrder_FK FOREIGN KEY (OrderID) REFERENCES Store.Orders (Id),
	CONSTRAINT OrderItemItem_FK FOREIGN KEY (ItemID) REFERENCES Store.Items (ItemName)
);