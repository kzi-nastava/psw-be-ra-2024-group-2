INSERT INTO payment."ShoppingCarts"(
	"Id", "TotalPrice", "TouristId")
	VALUES (-1, 100, -21);
INSERT INTO payment."OrderItems"(
	"Id", "TourName", "Price", "TourId", "UserId", "Token", "ShoppingCartId")
	VALUES (-1, 'tura', 45, -1, -21, false, -1);
INSERT INTO payment."OrderItems"(
	"Id", "TourName", "Price", "TourId", "UserId", "Token", "ShoppingCartId")
	VALUES (-2, 'tura2', 55, -2, -21, false, -1);