FROM node:24.11.1-alpine AS builder

WORKDIR /app
COPY ./client/package*.json ./
RUN npm install
COPY ./client .
RUN npm run build
 
FROM node:24.11.1-alpine AS production
WORKDIR /server
COPY ./server/package*.json ./
RUN npm install --production
COPY ./server/ .
COPY --from=builder /app/dist ./build 
EXPOSE 8080

CMD ["node", "app.js"]