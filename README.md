# StockCrawler

**StockCrawler** is a simple CLI tool can download stock closing price for Taiwan public market.

It grabs history data from **[Yahoo Finance](http://finance.yahoo.com)** and daily closing price from **[TWSE](http://www.twse.com.tw)**.

## Objective

In 2010, I was going to build a stock trading bot. I need all history stock pricing and keep data up-to-date, therefore I developed this tiny tool.

## How to use

There is a CLI executive - **StockCrawlerRunner.exe**

Here is the instruction:

```
StockCrawlerRunner.exe <mode>
 <mode>: -i
     Initialize all stock history data. It will drop all existing data.
 <mode>: -i [StockNo]
     Initialize only one specified stock history data. It will drop all old data by this stock.
 <mode>: -u
     Append the latest price data in database.
```

## Note
This tool now support multiple database platforms, including **MySQL**, **SQL Server**, and **ACCESS**(*deprecated*)

For ACCESS, The database file is located in "*database/Access/Stock.mdb*", and others are in beside folders.

In **configuration**, you have to specify what type of database you are going to use:

```xml
  <appSettings>
    <add key="DB_TYPE" value="MSSQL"/>
	...
  </appSettings>
```
You need to edit the configuration "*StockCrawlerRunner.exe.config*" to **specify the database type** of this mdb file.

```xml
<connectionStrings>
    <add name="StockCrawler.Dao.Properties.Settings.StockConnectionString" connectionString="Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\StockCrawler\database\Stock.mdb" providerName="System.Data.OleDb"/>
</connectionStrings>

or MSSQL as following:

<connectionStrings>
    <add name="StockCrawler.Dao.Properties.Settings.StockConnectionString" connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True" providerName="System.Data.SqlClient" />
</connectionStrings>
```

Have fun~~