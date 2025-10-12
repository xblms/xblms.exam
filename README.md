
# 星期八在线考试系统（XBLMS.EXAM）

<br />

<img src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/logo.png" height="180" align="center">

<br />
<br />
<br />

## 系统展示

<br />

<img src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/admin/index.png">

<br />

<img src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/home/index.png">

<br />

## 演示环境

> 管理端演示环境

* [点击前往管理端演示](http://182.92.223.118:5000/xblms-admin)
> 用户端演示环境

* [点击前往用户端演示](http://182.92.223.118:5000/home)

> 移动端演示环境

<img width="200" src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/demo/app/二维码.png"/>

<br />

## 介绍

基于 .NET Core 8 + Vue。

支持跨平台部署。

支持人大金仓、达梦、OceanBase、MariaDB、MySql、SqlServer、PostgreSql、SQLite 等多类型数据库。

### 优势

* 一码多库：一套代码，多种类型数据库随意搭配。

* NoSql：无数据库脚本，无sql语句，code first高级版，基于实体对象自动生成数据库结构。

* 一键切换：备份、还原、迁移、切换数据库分分钟搞定。

* 前后端分离：.netcore api restfull 提供轻量级的数据服务，搭配vue前端框架，开发部署简单高效。

* 一码多端：一套代码，适配pc端多端浏览器和移动端多端终端，响应式布局+element轻松实现耳目一新的ui用户体验。

* 跨平台：适配国内主流CPU、操作系统、国产数据库。

* 全方位安全机制

	1、完整且丰富的日志功能（管理员日志+用户日志+错误日志+数据日志）

    2、前后端安全（SQL注入、跨站脚本、非法文件上传、越权访问）

	3、轻松运维（无界面数据查询+审计）

	4、访问限制（白名单、黑名单、ip、ip段、区域）

	5、安全模式，自定义系统安全措施

* 技术支持+个性化定制（非常专业）

* 性能（.net 8 史无前例）

### 版本

* master 为开发版，这里会经常提交优化和更新，为下一个版本做准备。

* [发行版提供较稳定的生成环境部署包和源代码，前往下载](https://gitee.com/xblms/xblmes/releases)

### 系统升级

* 访问/xblms-admin/syncDatabase 进行数据库升级。
* 发布最新的代码替换到原来的部署包即可升级部署文件，不要替换xblms.json文件。

## 发布手册

* 进群获取[qq群:1027427177]

## 支持环境
### 支持的操作系统
#### Windows
|操作系统|版本|架构|
|:-|:-|:-|
|[Windows 10](https://www.microsoft.com/windows/)|Version 1607+|x64, x86, Arm64|
|[Windows 11](https://www.microsoft.com/windows/)|Version 22000+|x64, x86, Arm64|
|[Windows Server](https://learn.microsoft.com/windows-server/)|2012+|x64, x86|
|[Windows Server Core](https://learn.microsoft.com/windows-server/)|2012+|x64, x86|
|[Nano Server](https://learn.microsoft.com/windows-server/get-started/getting-started-with-nano-server)|Version 1809+|x64|

#### Linux
|操作系统|版本|架构|
|:-|:-|:-|
|[Alpine Linux](https://alpinelinux.org/)|3.15+|x64, Arm64, Arm32|
|[CentOS](https://www.centos.org/)|7+|x64|
|[Debian](https://www.debian.org/)|10+|x64, Arm64, Arm32|
|[Fedora](https://opensuse.org/)|33+|x64|
|[OpenSUSE](https://opensuse.org/)|15+|x64|
|[Oracle Linux](https://www.oracle.com/linux/)|7+|x64|
|[Red Hat Enterprise Linux](https://www.redhat.com/en/technologies/linux-platforms/enterprise-linux)|7+|x64, Arm64|
|[SUSE Enterprise Linux (SLES)](https://www.suse.com/products/server/)|12 SP2+|x64|
|[Ubuntu](https://ubuntu.com/)|18.04+|x64, Arm64, Arm32|
|[银河麒麟](https://kylinos.cn/)|10+|x64, Arm64|
|[中标麒麟](https://kylinos.cn/)|7+|x64, Arm64|

### 支持的数据库
|数据库|版本|
|:-|:-|
|[MySql](https://www.mysql.com/)|5.7+|
|[SqlServer](https://www.microsoft.com/en-us/sql-server)|2008+|
|[PostgreSql](https://www.postgresql.org/)|11+|
|[SQLite](https://sqlite.org/)|2.0+|
|[人大金仓](https://www.kingbase.com.cn/)|9.0+|
|[达梦](https://www.dameng.com/)|8.0+|
|[OceanBase](https://www.oceanbase.com/)|4.3+|

## 源码结构
```
├── src (源代码)
│   ├── Datory (数据库基础类)
│   ├── XBLMS (接口基础类)
│   ├── XBLMS.Core (核心代码)
│   ├── XBLMS.Web (UI)
│   │   ├── wwwroot (对外访问目录)
│   │   ├── Controllers (WebApi)
│   │   ├── log (运行日志)
│   │   ├── Pages (页面)
│   │   ├── appsettings.json (配置文件)
│   │   ├── web.config (配置文件，非IIS部署可以删除)
│   │   ├── xblms.json (配置文件)
├── appsettings.json (配置文件)
├── build.sln (解决方案，用于发布)
├── gulpfile.js (配置文件，用于发布)
├── xblms.sln (解决方案，用于开发)
```


## 编译

项目编译需要使用 Visual Studio 2022，你可以从这里下载 [Visual Studio Community 2022](https://www.visualstudio.com/downloads/)

[SDK 开发用](https://dotnet.microsoft.com/zh-cn/download/dotnet/thank-you/sdk-8.0.403-windows-x64-installer)

[运行时 部署用](https://dotnet.microsoft.com/zh-cn/download/dotnet/thank-you/runtime-aspnetcore-8.0.2-windows-hosting-bundle-installer)

## 发布跨平台版本

### Window(x64)：

```
npm install
npm run build-win-x64
dotnet build ./build-win-x64/build.sln -c Release
dotnet publish ./build-win-x64/src/XBLMS.Web/XBLMS.Web.csproj -r win-x64 -c Release -o ./publish/xblms-win-x64
npm run copy-win-x64
```

> 进入文件夹 `./publish/xblms-win-x64` 获取部署文件

### Window(x32)：

```
npm install
npm run build-win-x86
dotnet build ./build-win-x86/build.sln -c Release
dotnet publish ./build-win-x86/src/XBLMS.Web/XBLMS.Web.csproj -r win-x86 -c Release -o ./publish/xblms-win-x86
npm run copy-win-x86
```

> 进入文件夹 `./publish/xblms-win-x86` 获取部署文件

### Linux(x64)：

```
npm install
npm run build-linux-x64
dotnet build ./build-linux-x64/build.sln -c Release
dotnet publish ./build-linux-x64/src/XBLMS.Web/XBLMS.Web.csproj -r linux-x64 -c Release -o ./publish/xblms-linux-x64
npm run copy-linux-x64
```

> 进入文件夹 `./publish/xblms-linux-x64` 获取部署文件

### Linux(arm64)：

```
npm install
npm run build-linux-arm64
dotnet build ./build-linux-arm64/build.sln -c Release
dotnet publish ./build-linux-arm64/src/XBLMS.Web/XBLMS.Web.csproj -r linux-arm64 -c Release -o ./publish/xblms-linux-arm64
npm run copy-linux-arm64
```

> 进入文件夹 `./publish/xblms-linux-arm64` 获取部署文件

## 问题与建议

如发现问题或对产品有任何建议，请提交至 [Gitee Issues](https://gitee.com/xblms/xblmes/issues)。

## License

[GNU Affero General Public License v3.0](LICENSE)

Copyright (C) 2024 XBLMS.EXAM

## 打赏

<img width="200" src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/ds.png"/>

## 对公业务请联系客服微信

<img width="200" src="https://gitee.com/xblms/xblmes/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/linkmewechat.jpg"/>