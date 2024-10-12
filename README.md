# XBLMES 在线考试系统

<img src="https://gitee.com/xblms/xblms/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/logo.png" height="180" align="center">

<br /><br />

## 介绍

基于 .NET Core 8

支持跨平台、国产化部署

支持国产人大金仓、达梦、OceanBase数据库 及 MySql、SqlServer、PostgreSql、SQLite 等数据库

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

## 功能介绍
### 管理端

|  模块   | 介绍  |
|  ----  | ----  |
| 系统安装  | 首次访问需要安装系统，根据选择的数据库自动生成表结构和基础数据  |  
| 登录  | 用户名、密码、验证码登录  |  
| 组织管理  | 单位、部门、岗位无限层级管理  |  
| 用户管理  | 管理员管理、用户管理、用户组管理、角色管理  |  
| 系统管理  | 管理员设置、用户设置、数据库管理、访问拦截管理  |  
| 日志管理  | 管理员日志、用户日志、系统错误日志，日志设置  |  
| 统计图表  | 用户登录统计、访问拦截统计  |  

### 用户端

|  模块   | 介绍  |
|  ----  | ----  |
| 登录  | 用户名、密码、验证码登录  |  
| 修改信息  | 修改当前账户的信息  |  
| 修改密码  | 修改当前账户的密码  |  

### 系统展示

* 管理端
<table>
    <tr>
        <td></td>
        <td></td>
    </tr>
</table>

* 用户端
<table>
    <tr>
        <td></td>
        <td></td>
    </tr>
</table>

## 发布跨平台版本

### Window(x64)：

```
npm install
npm run build-win-x64
dotnet build ./build-win-x64/build.sln -c Release
dotnet publish ./build-win-x64/src/XBLMS.Web/XBLMS.Web.csproj -r win-x64 -c Release -o ./publish/xblms-win-x64
```

> Note: 进入文件夹 `./publish/xblms-win-x64` 获取部署文件

### Window(x32)：

```
npm install
npm run build-win-x32
dotnet build ./build-win-x32/build.sln -c Release
dotnet publish ./build-win-x32/src/XBLMS.Web/XBLMS.Web.csproj -r win-x32 -c Release -o ./publish/xblms-win-x32
```

> Note: 进入文件夹 `./publish/xblms-win-x32` 获取部署文件

### Linux(x64)：

```
npm install
npm run build-linux-x64
dotnet build ./build-linux-x64/build.sln -c Release
dotnet publish ./build-linux-x64/src/XBLMS.Web/XBLMS.Web.csproj -r linux-x64 -c Release -o ./publish/xblms-linux-x64
```

> Note: 进入文件夹 `./publish/xblms-linux-x64` 获取部署文件

### Linux(arm64)：

```
npm install
npm run build-linux-arm64
dotnet build ./build-linux-arm64/build.sln -c Release
dotnet publish ./build-linux-arm64/src/XBLMS.Web/XBLMS.Web.csproj -r linux-arm64 -c Release -o ./publish/xblms-linux-arm64
```

> Note: 进入文件夹 `./publish/xblms-linux-arm64` 获取部署文件