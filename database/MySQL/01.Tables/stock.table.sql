CREATE TABLE `stock` (
  `StockID` int(11) NOT NULL AUTO_INCREMENT,
  `StockNo` varchar(10) NOT NULL,
  `StockName` varchar(10) NOT NULL,
  `Enable` bit(1) NOT NULL DEFAULT b'1',
  `DateCreated` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`StockID`),
  UNIQUE KEY `StockNo_UNIQUE` (`StockNo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
