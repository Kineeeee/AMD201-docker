# Sử dụng hình ảnh .NET SDK để build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy toàn bộ source code vào container
COPY . ./

# Restore dependencies
RUN dotnet restore "URLShortener.csproj"


# Build ứng dụng
RUN dotnet publish "URLShortener.csproj" -c Release -o /out

# Dùng runtime image nhẹ hơn để chạy ứng dụng
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy file từ giai đoạn build vào runtime container
COPY --from=build /out .

# Thiết lập biến môi trường cho MongoDB và Redis
ENV MongoDb__ConnectionString=mongodb://mongo:27017
ENV MongoDb__DatabaseName=URLShortenerDB
ENV Redis__ConnectionString=redis:6379

# Mở cổng để API có thể lắng nghe request
EXPOSE 5043

# Chạy ứng dụng
CMD ["dotnet", "URLShortener.dll"]
