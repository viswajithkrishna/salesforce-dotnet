**Use this simple library to integrate your .NET solution against your Salesforce.com data.**

## Quick Sample ##
```
//search for Account
protected void btnSearchAccount_Click(object sender, EventArgs e)
  {
    Account account = new AccountController().GetByName(txtAccountSearch.Text);
    lblAccountResult.Text = account != null ? account.Name : "No result";
  }

//add Account
protected void btnAddAccount_Click(object sender, EventArgs e)
  {
    Account account = new Account();
    account.Name = txtAccountName.Text;
    account.BillingCountry = ddlBillingCountry.SelectedValue;
    string addedAccountId = new AccountController().Add(account);
  }
```

## Quick Start ##
  1. Download the wsdl file from Salesforce (Login | Setup | App Setup | Develop | API)
  1. Run wsdl.exe (if you don't have wsdl.exe and your system is up-to-date, download the Microsoft Windows SDK for Windows 7 and .NET Framework 3.5 SP1 to get it http://www.microsoft.com/downloads/details.aspx?familyid=71DEB800-C591-4F97-A900-BEA146E4FAE1&displaylang=en)
    * Examples:
    * For URI: <br />wsdl /language:CS /out:SforceService.cs http://hostServer/WebserviceRoot/WebServiceName.asmx?WSDL
    * For local file:<br />"C:\Program Files\Microsoft SDKs\Windows\v7.0\Bin\wsdl.exe" /language:CS /out:SforceService.cs "\\server\Users\your.name\Visual Studio 2008\WebSites?\enterprise.wsdl"
  1. Copy the resulting file into the Core of the salesforce-dotnet application
  1. Enter your username and password in the settings of the Gaiaware.Salesforce project

## Quick Overview ##

**Gaiaware.Salesforce**

The API core which abstracts the complexity away

**Gaiaware.SalesforceWrapper**

Your specific Salesforce project
Customize your specific entities and objects.
\Controllers: put your specific queries
\Gaiaware.SalesforceWrapper\Utilities\SforceFields.cs: specify the fields for your objects

**SampleWeb**

Example implementation in ASP.NET

## Want to help? ##
There are always things to improve:
  * better support batch updates/adds
  * better support for receiving complex objects
  * better error handling
  * better logging
  * query via LINQ: LinqToSalesforce
  * unit testing

[Read how you can help](http://code.google.com/p/support/wiki/GettingStarted#Collaborating_on_an_Existing_Project)


---


This project was initiated by Gaiaware - the company behind the open source Ajax framework for ASP.NET: Gaia Ajax
http://gaiaware.net

Read how we use Salesforce: http://www.salesforce.com/eu/customers/hi-tech-software/gaiaware.jsp