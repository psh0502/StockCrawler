# StockCrawler

**StockCrawler** is a simple CLI tool can download stock information for Taiwan public market, includes:

- History price information.
- Company basic information.
- Business finanicial reports.
- Market news about stock.
- Forum news about stock.
- History interest issuing result.

It grabs history data from serveral available source, e.g **[Yahoo Finance](http://finance.yahoo.com)**, **[TWSE](http://www.twse.com.tw)** and daily closing price from **[TWSE](http://www.twse.com.tw)**.

***Yahoo** has incompleted price information, **TWSE** is better resource but costs very much time, even days...*
## Objective

In 2010, I wish to build a stock trading bot, but no data means no work.

All history stock data are needed and I have to keep them up-to-date, so this project was born for it.

Some remaining tasks haven't be done yet, so I am still keeping going.

Anyone would like join is welcome.

## How to use

There is a CLI executive - **StockCrawlerRunner.exe**

Here is the instruction:

```
StockCrawlerRunner.exe <mode>
 <mode>: -i
     Initialize all stock history data. It will drop all existing data.
 <mode>: -i [stock number]
     Initialize only one specified stock history data. It deletes all existing data.
 <mode>: -i [Date:yyyy/MM/dd]
     Initialize all stock history data by the specified date. It deletes existing data after the specified date.
 <mode>: -i <Date:yyyy/MM/dd> [stock number]
     Initialize only one specified stock history data. It deletes existing data with this stock.
 <mode>: -u
     Append the latest price data in database.
 <mode>: -u [Date:yyyy/MM/dd]
     Append the specified date price data in database.
 <mode>: -b
     Update the latest company basic information in database.
 <mode>: -b <stock number> Update the latest company basic info by the specified stock number
     Update the latest company basic information in database.
 <mode>: -f
     Update the company finance report since 2015 to this year.
 <mode>: -n
     Get the latest Taiwan stock market news.
 <mode>: -m
     Get the latest 12 monthy income data by each stock.
 <mode>: -is
     Get the latest interest issued result.
 <mode>: -ptt [Date:yyyy/MM/dd]
     Get the latest articles from PTT stock forum. If you assign date, it grab articles by the date.
```

## Note

```xml
It supports MSSQL currently, so you need modify the following setting to connect your own database.

<connectionStrings>
    <add name="StockCrawler.Dao.Properties.Settings.StockConnectionString" 
         connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=Stock;Integrated Security=True" 
         providerName="System.Data.SqlClient" />
</connectionStrings>
```
Since we adopt Unity DI framework, you can implement your IStockDataService interface to support other database.

Wish to see someone do that.

Here is the injection setting in *.exe.config

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
      <register type="IStockForumCollector" mapTo="StockForumCollector" />
      <register type="IStockBasicInfoCollector" mapTo="TwseStockBasicInfoCollector" />
      <register type="IStockDailyInfoCollector" mapTo="TwseStockDailyInfoCollector" />
      <register type="IStockReportCollector" mapTo="TwseReportCollector" />
      <register type="IStockInterestIssuedCollector" mapTo="TwseInterestIssuedCollector" />
      <register type="IStockMarketNewsCollector" mapTo="TwseMarketNewsCollector" />
      <register type="IStockMonthlyIncomeCollector" mapTo="TwseMonthlyIncomeCollector" />
    </container>
  </unity>
</configuration>
```
Each main function, these classes were implemeted with *IJop* interface from **Quartz.net**, means you can host them in **Quartz.Server**.

You can schedule your own triggers by **Quartz**.

I prefer to run this tool by **Windows scheduled tasker**, so I made this tiny cli for it.
