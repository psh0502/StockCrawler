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
 <mode>: -i [Date:yyyy/MM/dd]
     Initialize all stock history data by the specified date. It will drop all existing data.
 <mode>: -i <Date:yyyy/MM/dd> [StockNo]
     Initialize only one specified stock history data. It will drop all old data by this stock.
 <mode>: -u
     Append the latest price data in database.
 <mode>: -u [Date:yyyy/MM/dd]
     Append the specified date price data in database.
 <mode>: -b
     Update the latest company basic information in database.
 <mode>: -b <stockNo> Update the latest company basic info by the specified stock No
     Update the latest company basic information in database.
 <mode>: -f <taiwan year>
     Update the company finance report since the specified Taiwan year.
```

## Note

```xml
It supports MSSQL currently, so you need modify the following setting to connect your own database.

<connectionStrings>
    <add name="StockCrawler.Dao.Properties.Settings.StockConnectionString" connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True" providerName="System.Data.SqlClient" />
</connectionStrings>
```
BTW, since we adopt Unity DI framework, you can implement your IStockDataService interface to support other database.
Wish to see someone do that.

Here is the injection setting in XXX.exe.config

```xml
<configuration>
  <configSections>
    <section name="unity" type="Microsoft.Practices.Unity.Configuration.UnityConfigurationSection, Unity.Configuration"/>
  </configSections>
  <unity xmlns="http://schemas.microsoft.com/practices/2010/unity">
    <namespace name="StockCrawler.Dao" />
    <namespace name="StockCrawler.Services.Collectors" />
    <assembly name="StockCrawler.Dao" />
    <assembly name="StockCrawler.Services" />

    <container>
      <register type="IStockDataService" mapTo="StockDataServiceMSSQL" />
      <register type="IStockHistoryPriceCollector" mapTo="YaooStockHistoryPriceCollector" />
      <register type="IStockBasicInfoCollector" mapTo="TwseStockBasicInfoCollector" />
      <register type="IStockDailyInfoCollector" mapTo="TwseStockDailyInfoCollector" />
      <register type="IStockReportCollector" mapTo="TwseReportCollector" />
    </container>
  </unity>
</configuration>
```
Have fun~~
