# Sử dụng Node.js để build frontend
FROM node:18 AS build
WORKDIR /app

# Copy source code
COPY . ./

# Cài đặt dependencies
RUN npm install

# Build project
RUN npm run build

# Sử dụng Nginx để phục vụ frontend
FROM nginx:latest
WORKDIR /usr/share/nginx/html

# Xóa file mặc định của Nginx
RUN rm -rf ./*

# Copy file build từ bước trước vào Nginx
COPY --from=build /app/dist .

# Copy file cấu hình Nginx
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Mở cổng 80
EXPOSE 4173

# Khởi động Nginx
CMD ["nginx", "-g", "daemon off;"]
