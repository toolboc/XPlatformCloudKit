XPlatformCloudKit
=================

A Hands-on lab for creating your very own multi-platform app with Azure Mobile Services as a backend.

Examples of Applications Built with the XPlatformCloudKit
---------------------------------------------------------
* Super Street Fighter 2 - Strategy Guide [Windows 8](http://apps.microsoft.com/windows/en-us/app/655e0b21-6a5f-455e-bc7f-01845c1198f9) | [Windows Phone 8](http://www.windowsphone.com/en-us/store/app/super-street-fighter-2/fe3dbbce-7770-4a41-b395-f42e3819141d)
* Minecraft Minions [Windows 8](http://apps.microsoft.com/windows/en-us/app/ca936bcc-f665-4694-844b-afe6ed836e14) | [Windows Phone](http://www.windowsphone.com/en-us/store/app/minecraft-minions/c81a20b6-481f-472f-99c6-8f25b989a50a)

Video Tutorials
---------------
* [Part 1 - Installing Prereqs from Dreamspark and Building for the First Time](http://www.youtube.com/watch?v=yKGPE95etYM)

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
