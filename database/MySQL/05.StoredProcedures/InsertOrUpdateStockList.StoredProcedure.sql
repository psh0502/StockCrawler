CREATE PROCEDURE `InsertOrUpdateStockList` (
IN stockId int, stockNo char, stockName char)
BEGIN
	INSERT INTO `stock`
	(`StockID`,
	`StockNo`,
	`StockName`,
	`Enable`)
	VALUES
	(stockId,
	stockNo,
	stockName,
	true);

END