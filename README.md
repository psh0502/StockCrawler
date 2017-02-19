# StockCrawler

**StockCrawler** is a simple CLI tool can download stock closing price for Taiwan public market.

It grabs history data from **[Yahoo Finance](http://finance.yahoo.com)** and daily closing price from **[Yahoo Taiwan](http://tw.stock.yahoo.com)**.

##Objective

In 2010, I was going to build a stock trading bot. I need all history stock pricing and keep data up-to-date, therefore I developed this tiny tool by this purpose.

##How to use

There is a CLI executive - **StockCrawlerRunner.exe**

Here is the instruction:

```
StockCrawlerRunner.exe <mode>
 <mode>: -i
     Initialize all stock history data. It will drop all existing data.
 <mode>: -u
     Append the latest price data in database.
```

##Note
it was designed to work with **ACCESS**, a simple file base database.

The database file is located in "*database/Access/Stock.mdb*"

You need to edit the configuration "*StockCrawlerRunner.exe.config*" to **specify the location** of this mdb file.

```
<connectionStrings>
    <add name="StockCrawler.Dao.Properties.Settings.StockConnectionString" connectionString="Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\StockCrawler\database\Stock.mdb" providerName="System.Data.OleDb"/>
</connectionStrings>
```