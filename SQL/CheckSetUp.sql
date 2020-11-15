INSERT INTO Store.Location (LocationName) Values
	('Somewhere'),
	('Elsewhere'),
	('Nowhere'),
	('Right Here')
;

-- select * from Store.Location;

INSERT INTO Store.Customers(Id, FirstName, LastName, MiddleInitial, StoreLocation) VALUES
	(1, 'Somebody', 'Ferret', 't', 'Somewhere'),
	(2, 'Nobody', 'Squirrel', NULL, NULL),
	(3, 'Anybody', 'Squirrel', NULL, 'Nowhere')
;

INSERT INTO Store.Items(ItemName, ItemPrice) VALUES 
	('Acorns', 5.99),
	('Book of Greek Myths', 19.99),
	('Gummy Worms', 0.99),
	('Field Guide to Rodents', 9.99),
	('Hopes', 50000),
	('Dreams', 100000),
	('Memory', 150)
;


INSERT INTO Store.Invintory(StoreLocation, ItemName, Quantity) VALUES
	('Right Here', 'Acorns', 500),
	('Right Here', 'Book of Greek Myths', 4),
	('Somewhere', 'Acorns', 100	),
	('Somewhere', 'Field Guide to Rodents', 10),
	('Elsewhere', 'Gummy Worms', 100),
	('Nowhere', 'Memory', 10)
;

--orders, then order items
INSERT INTO Store.Orders(Id, CustomerID, StoreLocation, OrderTotal, OrderTime) VALUES 
	-- nobody
	(1, 2, 'Elsewhere', 80.89, GETDATE()),
	-- anybody
	(2, 3, 'Right Here', 100, GETDATE())
;

INSERT INTO Store.OrderItems (OrderID, ItemID, Quantity) VALUES
	(1,  'Book of Greek Myths', 1),
	(1, 'Acorns', 10),
	(2, 'Dreams', 1)
;