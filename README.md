# StockCrawler

**StockCrawler** is a simple CLI tool can download stock closing price for Taiwan public market.

It grabs history data from **[Yahoo Finance](http://finance.yahoo.com)** and daily closing price from **[TWSE](http://www.twse.com.tw)**.

## Objective

In 2010, I was going to build a stock trading bot. I need all history stock pricing and keep data up-to-date, therefore I developed this tiny tool.

## How to use

This is a CLI executive - **StockCrawlerRunner.exe**

Here is the instruction:

```
StockCrawlerRunner.exe <mode>
 <mode>: -i
     Initialize all stock history data. It will drop all existing data.
 <mode>: -i [StockNo]
     Initialize only one specified stock history data. It will drop all old data by this stock.
 <mode>: -i [Date:yyyy/MM/dd]");
     Initialize all stock history data by the specified date. It will drop all existing data.");
 <mode>: -i <Date:yyyy/MM/dd> [StockNo]");
     Initialize only one specified stock history data. It will drop all old data by this stock.");
 <mode>: -u");
     Append the latest price data in database.");
 <mode>: -u [Date:yyyy/MM/dd]");
     Append the specified date price data in database.");
 <mode>: -b");
     Update the latest company basic information in database.");

```

## Note

```xml
MSSQL as following:

<connectionStrings>
    <add name="StockCrawler.Dao.Properties.Settings.StockConnectionString" connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True" providerName="System.Data.SqlClient" />
</connectionStrings>
```

Have fun~~
