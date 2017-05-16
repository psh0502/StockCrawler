DROP procedure IF EXISTS `DisableAllStocks`;

DELIMITER $$

CREATE PROCEDURE `DisableAllStocks` ()
BEGIN
	UPDATE `stock` SET `Enable` = FALSE;
END$$

DELIMITER ;

