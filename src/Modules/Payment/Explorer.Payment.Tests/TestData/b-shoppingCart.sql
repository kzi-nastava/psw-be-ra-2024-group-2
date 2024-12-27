INSERT INTO payment."ShoppingCarts"(
	"Id", "TotalPrice", "TouristId")
	VALUES (-1, 100, -21);
INSERT INTO payment."OrderItems"(
	"Id", "Price", "TourId", "UserId", "Token", "ShoppingCartId", "TimeOfPurchase", "Discriminator")
VALUES (-1, 45, -1, -21, false, -1, NOW(), 'TourOrderItem');

INSERT INTO payment."OrderItems"(
	"Id", "Price", "TourId", "UserId", "Token", "ShoppingCartId", "TimeOfPurchase", "Discriminator")
VALUES (-2, 55, -2, -21, false, -1, NOW(), 'TourOrderItem');

INSERT INTO payment."TouristBonuses"(
	"Id", "TouristId", "CouponCode", "IsUsed")
VALUES (-999, -1, 'ABCD123', false);
INSERT INTO payment."TouristBonuses"(
	"Id", "TouristId", "CouponCode", "IsUsed")
VALUES (-998, -2, 'ABCD321', true);

INSERT INTO payment."Coupons"(
	"Id", "DiscountPercentage", "Code", "TourId", "AuthorId", "AllToursDiscount")
VALUES (-999, 10, 'ABCD123', -100, -1, true);