DROP procedure IF EXISTS `InsertOrUpdateStockList`;
DELIMITER $$

CREATE PROCEDURE `InsertOrUpdateStockList`(
pStockNo varchar(10), pStockName varchar(50))
BEGIN
	IF EXISTS(SELECT `StockID` FROM `stock` WHERE `StockNo` = pStockNo) THEN
		UPDATE `stock` SET `Enable` = TRUE WHERE `StockNo` = pStockNo;
	ELSE
		INSERT INTO `stock`
		(`StockNo`,
		`StockName`)
		VALUES
		(pStockNo,
		pStockName);

	END IF;
END