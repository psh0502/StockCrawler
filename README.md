# StockCrawler

**StockCrawler** is a simple CLI tool can download stock information for Taiwan public market, includes:

- History price information.
- Company basic information.
- Business finanicial reports.
- Market news about stock.
- 3rd-Party analytic.
- Forum news about stock.

It grabs history data from serveral available source, e.g **[Yahoo Finance](http://finance.yahoo.com)**, **[TWSE](http://www.twse.com.tw)** and daily closing price from **[TWSE](http://www.twse.com.tw)**.

***Yahoo** has incompleted price information, **TWSE** is better resource but costs very much time, even days...*
## Objective

In 2010, I was going to build a stock trading bot. 

I need all history stock data and keep them up-to-date, this is why I created it.

There are some bucket lists I haven't sone yet, so I am keeping going.

Anyone would like join is welcome.

## How to use

There is a CLI executive - **StockCrawlerRunner.exe**

Here is the instruction:

```
StockCrawlerRunner.exe <mode>
 <mode>: -i
     Initialize all stock history data. It will drop all existing data.
 <mode>: -i [stock number]
     Initialize only one specified stock history data. It will drop all old data by this stock.
 <mode>: -i [Date:yyyy/MM/dd]
     Initialize all stock history data by the specified date. It will drop all existing data.
 <mode>: -i <Date:yyyy/MM/dd> [stock number]
     Initialize only one specified stock history data. It will drop all old data by this stock.
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
 <mode>: -lz
     Get the latest LazyStock data.
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
      <register type="IStockHistoryPriceCollector" mapTo="YaooStockHistoryPriceCollector" />
      <register type="IStockBasicInfoCollector" mapTo="TwseStockBasicInfoCollector" />
      <register type="IStockDailyInfoCollector" mapTo="TwseStockDailyInfoCollector" />
      <register type="IStockReportCollector" mapTo="TwseReportCollector" />
      <register type="IStockMarketNewsCollector" mapTo="TwseMarketNewsCollector" />
      <register type="ILazyStockCollector" mapTo="LazyStockCollector" />
    </container>
  </unity>
</configuration>
```
Each main function, these classes were implemeted with *IJop* interface from **Quartz.net**, means you can host them in **Quartz.Server**.

You can schedule your own triggers by **Quartz**.

I prefer to run this tool by **Windows scheduled tasker**, so I made this tiny cli for it.
