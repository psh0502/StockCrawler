DROP PROCEDURE IF EXISTS `GetStocks`;
DELIMITER $$

CREATE PROCEDURE `GetStocks`()
BEGIN
	SELECT * FROM `stock`
    WHERE `Enable` = TRUE;
END