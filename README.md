XPlatformCloudKit
=================

A framework for creating your very own multi-platform app with Azure Mobile Services as a backend. Get started by following the included Powerpoint file located in the root of the project.
The XPlatformCloudKit supports displaying grouped items retrieved from an Azure Mobile Service, Rss Data Services, and/or Local file.  It can aesthetically organize grouped objects of Type Item with property of Title, Subtitle, Description, Image, and Group to produce a Windows Phone 8.X, Windows 8.X, and Android application by means of [Portable Class Libraries](http://msdn.microsoft.com/en-us/library/gg597391.aspx), [Xamarin Studio](https://store.xamarin.com/), and [MVVMCross](https://github.com/MvvmCross/MvvmCross).  The value of the Description property is rendered as HTML, meaning you can do some rather interesting things.  Check out the videos below to see how easy it is to get started!

Video Tutorials 
---------------
(Note: Content may not match current state of the framework)

* [Part 1 - Installing Prereqs from Dreamspark and Building for the First Time](http://www.youtube.com/watch?v=yKGPE95etYM)
* [Part 2 - Use Youtube Playlists to create a Video App (Legend of Zelda - Video Strategy Guide)](http://www.youtube.com/watch?v=gnMgM1z0EHg)
* [Part 3 - Use Local File to create an app with custom data (Microsoft Gaming - Information and News)](http://www.youtube.com/watch?v=hDEmKFSNQwI)
* [Part 4 - Using the XPCK App Builder for quick app development](https://www.youtube.com/watch?v=bcMX1Q8ILaU)
* [Part 5 - The making of XPCK Guide](https://www.youtube.com/watch?v=BHkk-qItiUQ)

Blog Articles featuring XPCK
----------------------------
* [Stuart Lodge's Blog (Author of MVVMCross)](http://slodge.blogspot.co.uk/2014/03/the-awesome-xplatformcloudkit.html)
* [Microsoft Dev Radio](http://channel9.msdn.com/Blogs/DevRadio/Microsoft-DevRadio-Creating-Multi-Platform-apps-with-the-XPlatformCloudkit-SDK-and-Windows-Azure)
* [Awesome Apps Built with XPCK](http://pjdecarlo.com/2013/12/awesome-apps-for-windows-phone-and-windows-8-built-with-xpck.html)
* [Channel 9 / Coding4Fun](http://channel9.msdn.com/coding4fun/blog/One-kit-three-platforms-the-XPlatformCloudKit)
* [B2Codigo Blog](http://www.b2codigo.com/)

Related Content
---------------
* [Build Once for Windows 8 and Phone 8 (Learn more about the patterns, architecture, and design concepts behind XPCK)](http://aka.ms/buildonce)
* [Marketing and Monetization Materials (Learn how to create income with your published application)](http://aka.ms/monetization)
* [App Marketing 101 (Real-World Examples of successful apps built with XPCK)](http://channel9.msdn.com/Series/App-Marketing-Virtual-Workshop/Part-4-App-Marketing-101-Real-World-Examples)
* [App Marketing Virtual Workshop (5 part series marketing seminar for developers, by developers)](http://channel9.msdn.com/Series/App-Marketing-Virtual-Workshop)

Getting Started
---------------

After obtaining and installing the prerequisites, download the .zip of this project (link on the bottom right of [The XPlatformCloudKit Project Home](https://github.com/winappkits/XPlatformCloudKit))
Save the .zip, then right-click=>Properties=>Unblock=>Apply  then extract the contents of the .zip file

You will then want to open the XPlatformCloudKit.sln located in the XPlatformCloudKit folder   
(If you recieve a warning about being bound to Team Foundation Server or inability to open the .Droid project, simply ignore)  

Follow along with the XPlatformCloudKit.pptx lab located in the root of the XPlatformCloudKit-master folder.

Windows Phone 8.1 and Windows 8.1 Universal project requirements:
----------------------------------------------------------------------------
* [Windows 8 Pro - 64 bit](http://windows.microsoft.com/en-us/windows/buy?ocid=GA8_O_WOL_DIS_ShopHP_FPP_Light) - [(DreamSpark Link)](https://www.dreamspark.com/student/Windows-8-App-Development.aspx)
* [Visual Studio 2013 Professional](http://www.microsoft.com/visualstudio/eng/products/visual-studio-overview) - [(DreamSpark Link)](https://www.dreamspark.com/Product/Product.aspx?productid=72) or above + [Windows 8.1 update](http://windows.microsoft.com/en-us/windows-8/update-from-windows-8-tutorial) for Windows 8.1 development
*  [Visual Studio 2013 Update 2](http://www.microsoft.com/en-us/download/details.aspx?id=42666)

Windows 8.0 project requirements:
--------------------------------

* [Windows 8 Pro](http://windows.microsoft.com/en-us/windows/buy?ocid=GA8_O_WOL_DIS_ShopHP_FPP_Light) - [(DreamSpark Link)](https://www.dreamspark.com/student/Windows-8-App-Development.aspx)
* [Visual Studio 2012 Professional](http://www.microsoft.com/en-us/download/details.aspx?id=30682) - [(DreamSpark Link)](https://www.dreamspark.com/Product/Product.aspx?productid=44) or above + [Visual Studio 2012 Update 4](http://support.microsoft.com/kb/2872520/en-us) - [(DreamSpark Link)](https://www.dreamspark.com/Product/Product.aspx?productid=51)  
OR  
[Visual Studio 2013 Professional](http://www.microsoft.com/visualstudio/eng/products/visual-studio-overview) - [(DreamSpark Link)](https://www.dreamspark.com/Product/Product.aspx?productid=72) or above + [Windows 8.1 update](http://windows.microsoft.com/en-us/windows-8/update-from-windows-8-tutorial) for Windows 8 development


Windows Phone 8.0 project requirements:
-----------------------------------------------

* [Windows 8 Pro - 64 bit](http://windows.microsoft.com/en-us/windows/buy?ocid=GA8_O_WOL_DIS_ShopHP_FPP_Light) - [(DreamSpark Link)](https://www.dreamspark.com/student/Windows-8-App-Development.aspx)
* [Visual Studio 2012 Professional](http://www.microsoft.com/en-us/download/details.aspx?id=30682) - [(DreamSpark Link)](https://www.dreamspark.com/Product/Product.aspx?productid=44) or above + [Visual Studio 2012 Update 4](http://support.microsoft.com/kb/2872520/en-us) - [(DreamSpark Link)](https://www.dreamspark.com/Product/Product.aspx?productid=51)  
OR  
[Visual Studio 2013 Professional](http://www.microsoft.com/visualstudio/eng/products/visual-studio-overview) - [(DreamSpark Link)](https://www.dreamspark.com/Product/Product.aspx?productid=72) or above + [Windows 8.1 update](http://windows.microsoft.com/en-us/windows-8/update-from-windows-8-tutorial) for Windows 8 development


Android project requirements
---------------------------------------

- To build in Visual Studio will require a [Xamarin Business License](https://store.xamarin.com/).

Examples of Applications Built with the XPlatformCloudKit
---------------------------------------------------------
* To see all Windows 8 apps that are built with XPCK:  
  On Windows 8 desktop press "Windows Key + R" and type "xplatformcloudkit:" => select OK => Click "Look for app in Store"  
  ![Screenshot](http://i.imgur.com/D5McEnu.png)![Screenshot](http://i.imgur.com/FODb3Sm.png)
* To see latest Windows Phone 8 apps built with XPCK:  
  View this [link](http://marketplaceedgeservice.windowsphone.com/v8/catalog/apps?os=8.0.10211.0&cc=US&lang=en-US&hw=520170499&dm=RM-820_nam_att_100&oemId=NOKIA&moId=att-us&chunkSize=50&orderBy=GlobalRank&tag=phone%2Eprotocol%2Explatformcloudkit) in Internet Explorer  

* Starbucks Menu [Windows 8](http://apps.microsoft.com/windows/en-us/app/starbucks-menu/ad9e782f-9f89-46b9-ae5d-fcc296e43118)  
  ![Windows 8 Screenshot](http://i.imgur.com/7KaRX8t.gif)
* Super Street Fighter 2 - Strategy Guide [Windows 8](http://apps.microsoft.com/windows/en-us/app/655e0b21-6a5f-455e-bc7f-01845c1198f9) | [Windows Phone 8](http://www.windowsphone.com/en-us/store/app/super-street-fighter-2/fe3dbbce-7770-4a41-b395-f42e3819141d)
  ![Windows 8 Screenshot](http://i.imgur.com/vSnRm1s.gif)
  ![Windows Phone 8 Screenshot1](http://cdn.marketplaceimages.windowsphone.com/v8/images/9cbb40b5-3d50-4939-a9b9-0447ba9112fb?imageType=ws_screenshot_large&rotation=0)
  ![Windows Phone 8 Screenshot2](http://cdn.marketplaceimages.windowsphone.com/v8/images/0ae1006f-2878-4ad9-8665-0d9872dba128?imageType=ws_screenshot_large&rotation=0)
  ![Android Emulator Screenshot](http://i.imgur.com/iLeJjBt.png)
