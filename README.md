# XBLMES 在线考试系统

<img src="https://gitee.com/xblms/xblms/raw/master/src/XBLMS.Web/wwwroot/sitefiles/assets/images/logo.png" height="180" align="center">

<br /><br />

## 介绍

基于 .NET Core 8

支持跨平台部署

支持国产人大金仓、达梦、OceanBase数据库 及 MySql、SqlServer、PostgreSql、SQLite 等数据库



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
dotnet publish ./build-win-x64/src/XBLMS.Cli/XBLMS.Cli.csproj -r win-x64 -c Release -o ./publish/xblms-win-x64
dotnet publish ./build-win-x64/src/XBLMS.Web/XBLMS.Web.csproj -r win-x64 -c Release -o ./publish/xblms-win-x64
```

> Note: 进入文件夹 `./publish/xblms-win-x64` 获取部署文件

### Window(x32)：

```
npm install
npm run build-win-x32
dotnet build ./build-win-x32/build.sln -c Release
dotnet publish ./build-win-x32/src/XBLMS.Cli/XBLMS.Cli.csproj -r win-x32 -c Release -o ./publish/xblms-win-x32
dotnet publish ./build-win-x32/src/XBLMS.Web/XBLMS.Web.csproj -r win-x32 -c Release -o ./publish/xblms-win-x32
```

> Note: 进入文件夹 `./publish/xblms-win-x32` 获取部署文件

### Linux(x64)：

```
npm install
npm run build-linux-x64
dotnet build ./build-linux-x64/build.sln -c Release
dotnet publish ./build-linux-x64/src/XBLMS.Cli/XBLMS.Cli.csproj -r linux-x64 -c Release -o ./publish/xblms-linux-x64
dotnet publish ./build-linux-x64/src/XBLMS.Web/XBLMS.Web.csproj -r linux-x64 -c Release -o ./publish/xblms-linux-x64
```

> Note: 进入文件夹 `./publish/xblms-linux-x64` 获取部署文件

### Linux(arm64)：

```
npm install
npm run build-linux-arm64
dotnet build ./build-linux-arm64/build.sln -c Release
dotnet publish ./build-linux-arm64/src/XBLMS.Cli/XBLMS.Cli.csproj -r linux-arm64 -c Release -o ./publish/xblms-linux-arm64
dotnet publish ./build-linux-arm64/src/XBLMS.Web/XBLMS.Web.csproj -r linux-arm64 -c Release -o ./publish/xblms-linux-arm64
```

> Note: 进入文件夹 `./publish/xblms-linux-arm64` 获取部署文件