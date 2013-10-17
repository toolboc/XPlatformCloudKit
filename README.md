XPlatformCloudKit
=================

A Hands-on lab for creating your very own multi-platform app with Azure Mobile Services as a backend. 
More specifically, the XPlatformCloudKit supports displaying grouped items retrieved from an Azure Mobile Service, Rss Data Services, and/or Local file.  It can aesthetically organize grouped objects of Type Item with property of Title, Subtitle, Description, Image, and Group to produce a Windows Phone, Windows 8, and Android application (Android supported only if you have Xamarin Studio license).  
The value of the Description property is rendered as HTML, meaning you can do some rather interesting things, for example see: 
* [The Daily Laugh App] (http://apps.microsoft.com/windows/en-us/app/d4479251-d5e2-458c-bab7-530c38518fbc)

Examples of Applications Built with the XPlatformCloudKit
---------------------------------------------------------
* Minecraft Minions [Windows 8](http://apps.microsoft.com/windows/en-us/app/ca936bcc-f665-4694-844b-afe6ed836e14) | [Windows Phone](http://www.windowsphone.com/en-us/store/app/minecraft-minions/c81a20b6-481f-472f-99c6-8f25b989a50a)
* Super Street Fighter 2 - Strategy Guide [Windows 8](http://apps.microsoft.com/windows/en-us/app/655e0b21-6a5f-455e-bc7f-01845c1198f9) | [Windows Phone 8](http://www.windowsphone.com/en-us/store/app/super-street-fighter-2/fe3dbbce-7770-4a41-b395-f42e3819141d)
  ![Windows 8 Screenshot] (http://wscont2.apps.microsoft.com/winstore/1x/4d4ba297-8149-4a70-9eac-ed2c5dea9da2/Screenshot.242737.100000.jpg)
  ![Windows Phone 8 Screenshot1] (http://cdn.marketplaceimages.windowsphone.com/v8/images/9cbb40b5-3d50-4939-a9b9-0447ba9112fb?imageType=ws_screenshot_large&rotation=0)
  ![Windows Phone 8 Screenshot2] (http://cdn.marketplaceimages.windowsphone.com/v8/images/0ae1006f-2878-4ad9-8665-0d9872dba128?imageType=ws_screenshot_large&rotation=0)
  ![Android Emulator Screenshot] (http://i.imgur.com/iLeJjBt.png)

Video Tutorials
---------------
* [Part 1 - Installing Prereqs from Dreamspark and Building for the First Time](http://www.youtube.com/watch?v=yKGPE95etYM)

Getting Started
---------------

After obtaining and installing the prerequisites, download the .zip of this project (link on the bottom right of [The XPlatformCloudKit Project Home](https://github.com/winappkits/XPlatformCloudKit))
Save the .zip, then right-click=>Properties=>Unblock=>Apply  then extract the contents of the .zip file

You will then want to open the XPlatformCloudKit.sln located in the XPlatformCloudKit folder   
(If you recieve a warning about being bound to Team Foundation Server or inability to open the .Droid project, simply ignore)  

Follow along with the XPlatformCloudKit.pptx lab located in the root of the XPlatformCloudKit-master folder.

Prerequisites for all projects
------------------------------

* [Windows 8 Pro](http://windows.microsoft.com/en-us/windows/buy?ocid=GA8_O_WOL_DIS_ShopHP_FPP_Light) - [(DreamSpark Link)](https://www.dreamspark.com/student/Windows-8-App-Development.aspx)
* [Visual Studio 2012 Professional or above](http://www.microsoft.com/visualstudio/eng/products/visual-studio-overview) - [(DreamSpark Link)](https://www.dreamspark.com/Product/Product.aspx?productid=44)
* [Visual Studio 2012 Update 3](http://support.microsoft.com/kb/2835600) - [(DreamSpark Link)](https://www.dreamspark.com/Product/Product.aspx?productid=51)
* (Does not currently build all project in Visual Studio 2013 RC)

Notes on getting Windows Phone 8 project to run:
-----------------------------------------------

* Requires [Windows 8 Pro - 64 bit](http://windows.microsoft.com/en-us/windows/buy?ocid=GA8_O_WOL_DIS_ShopHP_FPP_Light) - [(DreamSpark Link)](https://www.dreamspark.com/student/Windows-8-App-Development.aspx)
* Requires installation of [Windows Phone 8 SDK](http://aka.ms/phonesdk-cr) - [(DreamSpark Link)](https://www.dreamspark.com/student/Windows-Phone-8-App-Development.aspx)

Notes on getting Android Project to run
---------------------------------------

- To build in Visual Studio will require a [Xamarin Business License](https://store.xamarin.com/).
- You will need to copy the folder and all contents of `XPlatformCloudKit/SupportedFrameworks` to:
  
```
C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETPortable\v4.5\Profile\Profile78\SupportedFrameworks
```
This allows PCLs which target `Profile78` to be consumed by Xamarin IOS/Android projects.
- You must apply the [Xamarin hotfix](http://forums.xamarin.com/discussion/5507/using-system-linq-expressions-in-a-pcl-method-causes-typeloadexpression-mono-android-4-7-10024). This fixes an issue in the `System.Linq.Expressions.dll` facade used by Xamarin
