USE `stock`;
DROP procedure IF EXISTS `DisableAllStocks`;

DELIMITER $$
USE `stock`$$
CREATE PROCEDURE `DisableAllStocks` ()
BEGIN
	UPDATE `stock` SET `Enable` = FALSE;
END$$

DELIMITER ;

