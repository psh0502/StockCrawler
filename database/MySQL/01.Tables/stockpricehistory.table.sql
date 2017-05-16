CREATE TABLE `stockpricehistory` (
  `StockID` int(11) NOT NULL,
  `StockDT` datetime NOT NULL,
  `OpenPrice` decimal(10,0) NOT NULL,
  `HighPrice` decimal(10,0) NOT NULL,
  `LowPrice` decimal(10,0) NOT NULL,
  `ClosePrice` decimal(10,0) NOT NULL,
  `Volume` int(11) NOT NULL,
  `AdjClosePrice` decimal(10,0) NOT NULL,
  `DateCreated` datetime NOT NULL,
  PRIMARY KEY (`StockDT`,`StockID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
